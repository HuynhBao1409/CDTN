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
	public partial class NewPassword : System.Web.UI.Page
	{
        SqlConnection con;
        SqlCommand cmd;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) //Nếu ko phải tải trang lần đầu
            {
                lblMsg.Visible = false;
                string email = Request.QueryString["email"];
                //Ktra nếu email không tồn tại, hoặc không hợp lệ
                if (string.IsNullOrEmpty(email) || Session["ResetEmail"] == null || email != Session["ResetEmail"].ToString())
                {
                    Response.Redirect("ForgotPassword.aspx");
                }
            }
        }

        // Sự kiện nút Update
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string email = Session["ResetEmail"]?.ToString();
                //Ktra nếu email ko hợp lệ
                if (string.IsNullOrEmpty(email))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Email không hợp lệ.";
                    lblMsg.CssClass = "alert alert-danger";
                    return;
                }

                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("User_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "UPDATEPASSWORD"); //Cật nhật mật khẩu
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", txtNewPassword.Text.Trim());
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery(); //Lưu vào db
                con.Close();

                // Xóa dữ liệu OTP,Email khỏi Session
                Session.Remove("OTPData");
                Session.Remove("ResetEmail");

                lblMsg.Visible = true;
                lblMsg.Text = "Cập nhật mật khẩu thành công! Đang chuyển hướng...";
                lblMsg.CssClass = "alert alert-success";
                string script = "setTimeout(function(){ window.location='Login.aspx'; }, 2000);";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Lỗi: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
        }
    }
}