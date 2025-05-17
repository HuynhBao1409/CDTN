    using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace Foodie
{
	public class SendMail
	{
        public bool SendMailFunction(string to, string subject, string body)
        {
            string hostEmail = "smtp.gmail.com"; 
            int portEmail = 587; 
            string emailSender = "haogiabuynh01@gmail.com"; 
            string passwordSender = "axeu qegu prji qvcp"; // Mật khẩu ứng dụng

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(emailSender);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(hostEmail, portEmail);
                smtp.Credentials = new NetworkCredential(emailSender, passwordSender);
                smtp.EnableSsl = true;
                smtp.Send(mail);

                smtp.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi gửi mail: " + ex.Message);
                return false;
            }
        }
    }
}