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
	public partial class Login : System.Web.UI.Page
	{
        //Khai báo
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
		{
            if (Session["userId"] != null)
            {
                Response.Redirect("Default.aspx");
            }
		}

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //Ktra phải acc Admin ko
            if (txtUsername.Text.Trim() == "Admin" && txtPassword.Text.Trim() == "123")
			{
				Session["admin"] = txtUsername.Text.Trim();
				Response.Redirect("../Admin/Dashboard.aspx");
			}
			else
			{
                // Tạo kết nối và cmd cho SQL và stored proc
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("User_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "SELECT4LOGIN");
                cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@Password", txtPassword.Text.Trim());
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                //Ktra tk Login
                if (dt.Rows.Count == 1) 
                {
                    Session["username"] = txtUsername.Text.Trim();
                    Session["userId"] = dt.Rows[0]["UserId"];
                    lblMsg.Visible = true;
                    lblMsg.Text = "Đăng nhập thành công! Đang chuyển hướng...";
                    lblMsg.CssClass = "alert alert-success";
                    // Chuyển hướng sau 1s
                    string script = "setTimeout(function(){ window.location='Default.aspx'; }, 1000);";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Thông tin đăng nhập không hợp lệ..!!";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
        }
    }
}