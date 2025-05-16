using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Foodie.Admin;

namespace Foodie.User
{
	public partial class Cart : System.Web.UI.Page
	{
        //Khai báo
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        decimal grandTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    grandTotal = 0;
                    getCartItems();
                }
            }
            else
            {
                grandTotal = 0; // Reset grandTotal cho PostBack
            }
        }
        void getCartItems()
        {
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rCartItem.DataSource = dt;
            if (dt.Rows.Count == 0) //Ktra slượng hàng nếu trống
            {
                //Xóa nút tùy chọn
                rCartItem.FooterTemplate = null; 
                rCartItem.FooterTemplate = new CustomTemplate(ListItemType.Footer);
            }
            rCartItem.DataBind();
        }

        //Update, Delete Cart
        protected void rCartItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Utils utils = new Utils();
            if(e.CommandName == "remove")
            {
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Cart_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure; //Gọi thẳng tới stored proc thay vì qua sql
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Cật nhật Slượng trả thành true
                    getCartItems();
                    // Cật nhật lại slượng đơn
                    Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["userId"]));
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error- " + ex.Message + " ')<script>");
                }
                finally
                {
                    con.Close();
                }
            }
            //Update Cart
            if(e.CommandName == "updateCart")
            {
                bool isCartUpdated = false; //Ktra có đơn nào update ko
                //Ktra qua các đơn có trong giỏ
                for (int item = 0; item < rCartItem.Items.Count; item++)
                {
                    if (rCartItem.Items[item].ItemType == ListItemType.Item || rCartItem.Items[item].ItemType == ListItemType.AlternatingItem)
                    {
                        //Lấy dliệu từ control, DB và gán vô các biến
                        TextBox quantity = rCartItem.Items[item].FindControl("txtQuantity") as TextBox;
                        HiddenField _productId = rCartItem.Items[item].FindControl("hdnProductId") as HiddenField;
                        HiddenField _quantity = rCartItem.Items[item].FindControl("hdnQuantity") as HiddenField;
                        int quantityFromCart = Convert.ToInt32(quantity.Text);
                        int ProductId = Convert.ToInt32(_productId.Value);
                        int quantityFromDB = Convert.ToInt32(_quantity.Value);
                        bool isTrue = false;
                        int updatedQuantity = 1;
                        //Nếu slượng đơn mới > slượng cũ 
                        if(quantityFromCart > quantityFromDB)
                        {
                            updatedQuantity = quantityFromCart;
                            isTrue = true;
                        }
                        //Nếu slượng đơn mới < slượng cũ 
                        else if (quantityFromCart < quantityFromDB)
                        {
                            updatedQuantity = quantityFromCart;
                            isTrue = true;
                        }

                        if (isTrue)
                        {
                            // Update lại slượng đơn trong giỏ trong DB
                            isCartUpdated = utils.updateCartQuantity(updatedQuantity, ProductId, Convert.ToInt32(Session["userId"]));

                        }
                    }
                }
                getCartItems();
            }
            //Xử lý thanh toán
            if(e.CommandName == "checkout")
            {
                bool isTrue = false;
                string pName = string.Empty;
                //Ktra qua các sản phẩm có trong giỏ
                for (int item = 0; item < rCartItem.Items.Count; item++)
                {
                    if (rCartItem.Items[item].ItemType == ListItemType.Item || rCartItem.Items[item].ItemType == ListItemType.AlternatingItem)
                    {
                        // Tìm, lấy dl
                        HiddenField _productId = rCartItem.Items[item].FindControl("hdnProductId") as HiddenField;
                        HiddenField _cartQuantity = rCartItem.Items[item].FindControl("hdnQuantity") as HiddenField;
                        HiddenField _productQuantity = rCartItem.Items[item].FindControl("hdnPrdQuantity") as HiddenField;
                        Label productName = rCartItem.Items[item].FindControl("lblName") as Label;
                        int productId = Convert.ToInt32(_productId.Value);
                        int cartQuantity = Convert.ToInt32(_cartQuantity.Value);
                        int productQuantity = Convert.ToInt32(_productQuantity.Value);
                        //Nếu slượng sp trong kho > slượng trong giỏ 
                        if (productQuantity > cartQuantity && productQuantity > 2)
                        {
                            isTrue = true;
                        }
                        else 
                        {
                            isTrue = false;
                            pName = productName.Text.ToString();
                            break;
                        }
                    }
                }
                if (isTrue)
                {
                    Response.Redirect("Payment.aspx");
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Sản phẩm <b>'" + pName + "'</b> đã hết hàng hoặc quá số lượng hàng hiện có!";
                    lblMsg.CssClass = "alert alert-danger";

                }
            }
        }

        //Tính tổng đơn hàng
        protected void rCartItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Tìm, lấy dl ở Label, TextBox
                Label totalPrice = e.Item.FindControl("lblTotalPrice") as Label;
                Label productPrice = e.Item.FindControl("lblPrice") as Label;
                TextBox quantity = e.Item.FindControl("txtQuantity") as TextBox;
                // Kiểm tra và lấy Giá Tiền, loại bỏ và đặt lại " VND" và dấu phẩy
                decimal price = 0;
                if (decimal.TryParse(productPrice.Text.Replace(" VND", "").Replace(",", ""), out decimal parsedPrice))
                {
                    price = parsedPrice;
                }
                // Kiểm tra và lấy Số lượng
                int qty = 0;
                if (int.TryParse(quantity.Text, out int parsedQty))
                {
                    qty = parsedQty;
                }
                // Tính Tổng Tiền = Giá tiền * Số lượng
                decimal calTotalPrice = price * qty;
                totalPrice.Text = calTotalPrice.ToString("N0"); //định dạng lại kiểu số(VD:17,000)
                // Cộng dồn lại tổng các đơn
                grandTotal += calTotalPrice;
            }
            Session["grandTotalPrice"] = grandTotal.ToString("N0");
        }


        // Lớp template khi giỏ hàng trống
        private sealed class CustomTemplate : ITemplate
        {
            private ListItemType ListItemType { get; set; }

            public CustomTemplate(ListItemType type)
            {
                ListItemType = type;
            }

            public void InstantiateIn(Control container)
            {
                if (ListItemType == ListItemType.Footer)
                {
                    var footer = new LiteralControl("<tr><td colspan='5'><b>Giỏ hàng trống.</b><a href='Menu.aspx' class='badge badge-info ml-2'> Quay Lại Mua Sắm Thôi !!</a></td></tr></tbody></table>");
                    container.Controls.Add(footer);
                }
            }
        }
    }
}