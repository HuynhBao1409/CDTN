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
	public partial class ForgotPassword : System.Web.UI.Page
	{
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
        // Sự kiện nút Gửi Mã OTP
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("User_Crud", con);
                //Truyền tham số cho stored procedure
                cmd.Parameters.AddWithValue("@Action", "SELECTBYEMAIL"); //Tìm user theo email
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);

                // Ktra nếu có email tồn tại
                if (dt.Rows.Count == 1)
                {
                    // Tạo mã OTP ngẫu nhiên
                    string otp = GenerateOTP();
                    Session["OTPData"] = new { otp = otp, generatedAt = DateTime.UtcNow }; // Lưu OTP và thời gian tạo
                    Session["ResetEmail"] = txtEmail.Text.Trim();

                    string emailBody = $"<p>Chào bạn!,</p><p>Mã OTP để đặt lại mật khẩu của bạn là: <strong>{otp}</strong>. Mã này có hiệu lực trong 2 phút.</p>";
                    SendMail sendMail = new SendMail();
                    if (sendMail.SendMailFunction(txtEmail.Text.Trim(), "Mã OTP Đặt Lại Mật Khẩu", emailBody))
                    {
                        // Nếu gửi email thành công
                        lblMsg.Visible = true;
                        lblMsg.Text = "Mã OTP đã được gửi đến email của bạn!"; 
                        lblMsg.CssClass = "alert alert-success";
                        // Chuyển hướng đến trang ResetPassword
                        Response.Redirect($"ResetPassword.aspx?email={Server.UrlEncode(txtEmail.Text.Trim())}");
                    }
                    else
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Lỗi gửi email. Vui lòng thử lại.";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Email không tồn tại.";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Error: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
        }

        // Hàm tạo mã OTP ngẫu nhiên (6 chữ số)
        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString(); // Trả về số ngẫu nhiên từ 100000 đến 999999
        }
    }
}