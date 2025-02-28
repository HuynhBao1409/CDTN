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
        //Khai báo SQL Connect
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
                        // Nếu không dùng ảnh mặc định
                        finalImageUrl = "../Images/No_user.jpg";
                    }
                }
                Session["imageUrl"] = finalImageUrl;
                Session["createdDate"] = dt.Rows[0]["CreatedDate"].ToString();
            }

        }
    }
}