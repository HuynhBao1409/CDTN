using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Foodie.User
{
    public partial class Contact : System.Web.UI.Page
	{
        //Khai báo
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạo kết nối và cmd cho SQL và stored proc
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("ContactSp", con);
                //Truyền tham số cho hành động insert
                cmd.Parameters.AddWithValue("@Action", "INSERT");
                cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@Subject", txtSubject.Text.Trim());
                cmd.Parameters.AddWithValue("@Message", txtMessage.Text.Trim());
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open(); //Mở db
                cmd.ExecuteNonQuery(); //Chèn dl vào db
                lblMsg.Visible = true;
                lblMsg.Text = "Cảm ơn bạn đã liên hệ, chúng tôi sẽ xem xét yêu cầu/câu hỏi của bạn!";
                lblMsg.CssClass = "alert alert-success";
                clear();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert(' " + ex.Message + " ');</script>");
            }
            finally
            {
                con.Close();
            }
        }

        private void clear()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtMessage.Text = string.Empty;
        }
    }
}