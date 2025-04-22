using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.User
{
	public partial class Payment : System.Web.UI.Page
	{
        //Khai báo
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr, dr1;
        DataTable dt;
        SqlTransaction transaction = null;
        string _name = string.Empty; string _cardNo = string.Empty; string _expiryDate = string.Empty; string _cvv = string.Empty;
        string _address = string.Empty; string _paymentMode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                
            }
        }

        //Xử lý nút Thanh Toán bằng thẻ
        protected void lbCardSubmit_Click(object sender, EventArgs e)
        {
            //Lấy dliệu và loại bỏ khoảng trắng
            _name = txtName.Text.Trim();
            _cardNo = txtCardNo.Text.Trim();
            _cardNo = string.Format("**************{0}", txtCardNo.Text.Trim().Substring(12,4)); //che mã thẻ và chừa 4 số cuối
            _expiryDate = txtExpMonth.Text.Trim() + "/" + txtExpYear.Text.Trim(); //Date: MM/YYYY
            _cvv = txtCvv.Text.Trim();
            _address = txtAddress.Text.Trim();
            _paymentMode = "card"; //Thanh toán bằng thẻ
            if(Session["userId"] != null)
            {
                //Thực thi hàm void
                OrderPayment(_name, _cardNo, _expiryDate, _cvv, _address, _paymentMode);
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        //Xử lý nút xác nhận khi nhận hàng
        protected void lbCodSubmit_Click(object sender, EventArgs e)
        {
            _address = txtCODAddress.Text.Trim();
            _paymentMode = "cod"; //Thanh toán khi nhận hàng
            if (Session["userId"] != null)
            {
                OrderPayment(_name, _cardNo, _expiryDate, _cvv, _address, _paymentMode);
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        //Xử lý nút tạo mã QR
        protected void btnConfirmQr_Click(object sender, EventArgs e)
        {
            _paymentMode = "qr"; //Thanh toán bằng mã QR
            if (Session["userId"] != null)
            {
                // Lấy nội dung chuyển khoản từ hidden field
                string qrNote = hfQrNote.Value;
                if (string.IsNullOrEmpty(qrNote))
                {
                    // Nếu hidden field rỗng, tạo mới (trường hợp lỗi)
                    Random random = new Random();
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    qrNote = "Thanh toan don hang " + new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());
                }
                Session["qrNote"] = qrNote; // Lưu nội dung chuyển khoản để sử dụng trong Invoice.aspx nếu cần
                _address = "";
                // Gọi hàm xử lý thanh toán
                OrderPayment(_name, _cardNo, _expiryDate, _cvv, _address, _paymentMode);
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }


        void OrderPayment(string name, string cardNo, string expiryDate, string cvv,string address, string paymentMode)
        {
            int paymentId; int productId; int quantity;
            dt = new DataTable();
            //DataTable chứa ttin đơn hàng
            dt.Columns.AddRange(new DataColumn[7]
            {
                new DataColumn("OrderNo", typeof(string)),
                new DataColumn("ProductId", typeof(int)),
                new DataColumn("Quantity", typeof(int)),
                new DataColumn("UserId", typeof(int)),
                new DataColumn("Status", typeof(string)), 
                new DataColumn("PaymentId", typeof(int)),
                new DataColumn("OrderDate", typeof(DateTime)),
            });

            con = new SqlConnection(Connection.GetConnectionString());
            con.Open();

            #region Sql Transaction
            transaction = con.BeginTransaction();//tạo giao dịch trong sql
            cmd = new SqlCommand("Save_Payment", con, transaction);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@CardNo", cardNo);
            cmd.Parameters.AddWithValue("@ExpiryDate", expiryDate);
            cmd.Parameters.AddWithValue("@Cvv", cvv);
            cmd.Parameters.AddWithValue("@Address", address);
            cmd.Parameters.AddWithValue("@PaymentMode", paymentMode);
            // Tham số đầu ra lấy ID thanh toán sau khi insert
            SqlParameter outputParam = new SqlParameter("@InsertedId", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParam);
            try
            {
                cmd.ExecuteNonQuery(); // Thực thi lưu dl vào database
                paymentId = Convert.ToInt32(outputParam.Value);  // Lấy ID thanh toán vừa tạo

                #region Lấy hàng trong Giỏ
                //cmd lấy dl từ giỏ hàng của user từ Cart_Crud
                cmd = new SqlCommand("Cart_Crud", con, transaction); 
                cmd.Parameters.AddWithValue("@Action", "SELECT");
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure; 
                dr = cmd.ExecuteReader(); 
                while (dr.Read())
                {
                    productId = (int)dr["productId"];
                    quantity = (int)dr["Quantity"];
                    // Cật nhật Slượng sphẩm trong kho
                    UpdateQuantity(productId, quantity, transaction, con);
                    //Xóa đơn trong giỏ sau khi thanh toán
                    DeleteCartItem(productId, transaction, con);

                    //Thêm ttin payment tạm thời
                    dt.Rows.Add(Utils.GetUniqueId(), productId, quantity, (int)Session["userId"],"Đang xử lý", 
                       paymentId, Convert.ToDateTime(DateTime.Now));

                }
                dr.Close();
                #endregion Lấy hàng trong Giỏ

                #region Chi tiết Đơn Hàng
                if(dt.Rows.Count > 0) //Nếu có trên 1 đơn trong giỏ thì lưu
                {
                    cmd = new SqlCommand("Save_Orders", con, transaction);
                    cmd.Parameters.AddWithValue("@tblOrders", dt);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery(); // Thực thi lưu dl vào database
                }
                #endregion Chi tiết Đơn Hàng

                transaction.Commit();
                lblMsg.Visible = true;
                lblMsg.Text = "Bạn đã đặt hàng thành công!!";
                lblMsg.CssClass = "alert alert-success";
                Response.AddHeader("REFRESH", "2;URL=Invoice.aspx?id=" + paymentId);
            }
            catch (Exception e)
            {
                try
                {
                    transaction.Rollback();
                }
                catch(Exception ex)
                {
                    //Không rollback dc thì báo lỗi
                    Response.Write("<script>alert('" + ex.Message + "');</script>");
                }
            }
            #endregion Sql Transaction
            finally
            {
                con.Close();
            }
        }
        
        //Cật nhật lại slượng sau khi thanh toán
        void UpdateQuantity(int _productId, int _quantity, SqlTransaction sqlTransaction, SqlConnection sqlConnection)
        {
            int dbQuantity;
            cmd = new SqlCommand("Product_Crud", sqlConnection, sqlTransaction); //gọi để lấy slượng htại từ DB
            cmd.Parameters.AddWithValue("@Action", "GETBYID"); 
            cmd.Parameters.AddWithValue("@ProductId", _productId);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                dr1 = cmd.ExecuteReader();
                while (dr1.Read())
                {
                    dbQuantity = (int)dr1["Quantity"];// Lấy số lượng hàng từ DB
                    // Ktra slượng hàng trong kho lớn hơn slượng mua và > 2 (tránh bị âm)
                    if (dbQuantity > _quantity && dbQuantity > 2)
                    {
                        // Hàng trong kho (cũ) - số hàng đã mua = Số lượng hàng(mới)
                        dbQuantity = dbQuantity - _quantity;
                        cmd = new SqlCommand("Product_Crud", sqlConnection, sqlTransaction);
                        cmd.Parameters.AddWithValue("@Action", "QTYUPDATE"); //Gọi cmd cập nhật lại số lượng
                        cmd.Parameters.AddWithValue("@Quantity", dbQuantity);
                        cmd.Parameters.AddWithValue("@ProductId", _productId);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery(); // Thực thi lưu dl vào database
                    }
                }
                dr1.Close();
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            
        }

        
        //Xóa đơn trong giỏ sau khi thanh toán
        void DeleteCartItem(int _productId, SqlTransaction sqlTransaction, SqlConnection sqlConnection)
        {
            //Gọi cmd để xóa dliệu trong giỏ của user
            cmd = new SqlCommand("Cart_Crud", sqlConnection, sqlTransaction);
            cmd.Parameters.AddWithValue("@Action", "DELETE"); 
            cmd.Parameters.AddWithValue("@ProductId", _productId);
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
        }
    }
}