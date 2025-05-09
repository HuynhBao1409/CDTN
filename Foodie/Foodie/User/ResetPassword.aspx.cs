using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.User
{
	public partial class ResetPassword : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                lblMsg.Visible = false;
                string email = Request.QueryString["email"]; 
                // Ktra nếu email không tồn tại, hoặc không hợp lệ
                if (string.IsNullOrEmpty(email) || Session["ResetEmail"] == null || email != Session["ResetEmail"].ToString())
                {
                    Response.Redirect("ForgotPassword.aspx"); 
                }
            }
        }

        // Sự kiện nút Xác Nhận
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["OTPData"] == null)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Mã OTP không tồn tại hoặc đã hết hạn.";
                    lblMsg.CssClass = "alert alert-danger";
                    return;
                }

                // Lấy dữ liệu OTP từ Session
                var otpData = (dynamic)Session["OTPData"];
                string storedOtp = otpData.otp; // Mã OTP đã lưu
                DateTime generatedAt = otpData.generatedAt; // Thời gian tạo OTP

                if (storedOtp != txtOTP.Text.Trim())
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Mã OTP không đúng.";
                    lblMsg.CssClass = "alert alert-danger";
                    return;
                }

                // Ktra nếu OTP đã hết hạn (2 phút)
                if ((DateTime.UtcNow - generatedAt).TotalSeconds > 120)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Mã OTP đã hết hạn. Vui lòng yêu cầu mã mới.";
                    lblMsg.CssClass = "alert alert-danger";
                    Session.Remove("OTPData"); // Xóa OTPData khỏi Session
                    return;
                }

                lblMsg.Visible = true;
                lblMsg.Text = "Xác nhận OTP thành công! Đang chuyển hướng...";
                lblMsg.CssClass = "alert alert-success";
                string email = Session["ResetEmail"].ToString();
                string script = $"setTimeout(function(){{ window.location='NewPassword.aspx?email={Server.UrlEncode(email)}'; }}, 2000);";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Error: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
        }

        // Sự kiện nút Gửi Lại OTP
        protected void btnResend_Click(object sender, EventArgs e)
        {
            try
            {
                string email = Session["ResetEmail"]?.ToString();
                if (string.IsNullOrEmpty(email)) // Ktra nếu email không hợp lệ
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Email không hợp lệ.";
                    lblMsg.CssClass = "alert alert-danger";
                    Response.Redirect("ForgotPassword.aspx");
                    return;
                }

                // Ktra OTPData tồn tại và chưa đủ 30 giây kể từ lần gửi trước
                if (Session["OTPData"] != null)
                {
                    var otpData = (dynamic)Session["OTPData"];
                    DateTime generatedAt = otpData.generatedAt;
                    //Ktra nếu chưa đủ 30s
                    if ((DateTime.UtcNow - generatedAt).TotalSeconds < 30)
                    {
                        // Tính thời gian còn lại (Time hiện tại - Time vừa tạo)
                        int remainingSeconds = (int)(30 - (DateTime.UtcNow - generatedAt).TotalSeconds);
                        lblMsg.Visible = true;
                        lblMsg.Text = $"Vui lòng đợi {remainingSeconds} giây trước khi yêu cầu mã mới.";
                        lblMsg.CssClass = "alert alert-danger";
                        return;
                    }
                }

                // Tạo mã OTP mới
                string newOtp = GenerateOTP();
                Session["OTPData"] = new { otp = newOtp, generatedAt = DateTime.UtcNow };// Lưu OTP mới và thời gian tạo

                string emailBody = $"<p>Chào bạn,</p><p>Mã OTP mới để đặt lại mật khẩu của bạn là: <strong>{newOtp}</strong>. Mã này có hiệu lực trong 2 phút.</p>";
                SendMail sendMail = new SendMail();
                if (sendMail.SendMailFunction(email, "Mã OTP Đặt Lại Mật Khẩu Mới", emailBody))
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Mã OTP mới đã được gửi đến email của bạn!";
                    lblMsg.CssClass = "alert alert-success";
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Lỗi gửi email. Vui lòng thử lại.";
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
            return random.Next(100000, 999999).ToString();
        }
    }
}