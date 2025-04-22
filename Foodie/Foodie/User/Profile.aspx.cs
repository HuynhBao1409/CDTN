using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Foodie.User
{
    public partial class Profile : System.Web.UI.Page
    {
        //Khai báo
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
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
                    getUserDetails();
                    getPurchaseHistory();
                }
            }
        }
        //Sự kiện Edit Profile
        void getUserDetails()
        {

            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("User_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT4PROFILE");
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            // Tạo SqlDataAdapter để lấy dữ liệu từ DB
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rUserProfile.DataSource = dt;
            rUserProfile.DataBind();
            // Kiểm tra hồ sơ có hợp lệ
            if (dt.Rows.Count == 1)
            {
                // Gán dữ liệu từ bảng
                Session["name"] = dt.Rows[0]["Name"].ToString();
                Session["email"] = dt.Rows[0]["Email"].ToString();
                string dbImageUrl = dt.Rows[0]["ImageUrl"].ToString();
                string finalImageUrl;
                if (string.IsNullOrEmpty(dbImageUrl))
                {
                    finalImageUrl = "../Images/No_image.png";
                }
                else //Tạm vá lỗi load ảnh Profile
                {
                    // Kiểm tra có load dc ảnh không
                    string physicalPath = Server.MapPath(".." + dbImageUrl);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        finalImageUrl = ".." + dbImageUrl;
                    }
                    else
                    {
                        // Nếu không load dc dùng ảnh mặc định
                        finalImageUrl = "../Images/No_user.jpg";
                    }
                }
                Session["imageUrl"] = finalImageUrl;
                Session["createdDate"] = dt.Rows[0]["CreatedDate"].ToString();
            }

        }

        //Lịch sử Mua Hàng
        void getPurchaseHistory()
        {
            int sr = 1;
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Invoice", con);
            cmd.Parameters.AddWithValue("@Action", "ODRHISTORY"); //Lấy lịch sử giao dịch của userId
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            // Thêm cột stt
            dt.Columns.Add("SrNo", typeof(Int32));
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dataRow in dt.Rows) //Lượt qua các dòng trong DataTable
                {
                    dataRow["SrNo"] = sr;
                    sr++;
                }
            }
            if (dt.Rows.Count == 0) //Ktra nếu DataTable trống
            {
                //Xóa nút tùy chọn
                rPurchaseHistory.FooterTemplate = null;
                rPurchaseHistory.FooterTemplate = new CustomTemplate(ListItemType.Footer);
            }
            // Lkết và gán dl DataTable với control để hiển thị 
            rPurchaseHistory.DataSource = dt;
            rPurchaseHistory.DataBind();
        }

        //Hiển thị chi tiết Hóa đơn 
        protected void rPurchaseHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Kiểm tra nếu mục hiện tại là một Item (dữ liệu thông thường) hoặc AlternatingItem (dữ liệu xen kẽ)
            // Đảm bảo Code bên trong chỉ chạy cho các dữ liệu thực tế, bỏ qua header, footer
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                double grandTotal = 0;
                //Tìm control
                HiddenField paymentId = e.Item.FindControl("hdnPaymentId") as HiddenField;
                Repeater repOrders = e.Item.FindControl("rOrders") as Repeater;

                // Tạo kết nối và cmd cho SQL và stored proc
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Invoice", con);
                cmd.Parameters.AddWithValue("@Action", "INVOICBYID"); //Lấy id Hóa đơn của userId
                cmd.Parameters.AddWithValue("@PaymentId", Convert.ToInt32(paymentId.Value));
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                // Tính tổng tiền tất cả sản phẩm 
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in dt.Rows) //Lượt qua các dòng
                    {
                        grandTotal += Convert.ToDouble(dataRow["TotalPrice"]); // Cộng dồn tổng tiền
                    }
                }
                // Thêm dòng mới hiển thị tổng tiền
                DataRow dr = dt.NewRow();
                dr["TotalPrice"] = grandTotal;
                dt.Rows.Add(dr);

                // Lkết và gán dl DataTable với control để hiển thị 
                repOrders.DataSource = dt;
                repOrders.DataBind();
            }
        }

        // Lớp template khi lịch sử trống
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
                    var footer = new LiteralControl("<tr><td><b>Chờ gì nữa mà không đặt hàng?</b><a href='Menu.aspx' class='badge badge-info ml-2'>Nhấn để đặt hàng</a></td></tr></tbody></table>");
                    container.Controls.Add(footer);
                }
            }
        }


    }
}