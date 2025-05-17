using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.Admin
{
	public partial class EmailHelper : System.Web.UI.Page
	{
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Gửi Email";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");
                }
                LoadContacts();
            }
            lblMsg.Visible = false;
        }

        private void LoadContacts()
        {

            // Tạo kết nối và gọi User_Crud để lấy danh sách người dùng
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("User_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT4ADMIN");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            // Gán dữ liệu vào Repeater
            rContacts.DataSource = dt;
            rContacts.DataBind();
        }

        //Xử lý nút chọn trong Repeater
        protected void rContacts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "select")
            {
                string email = e.CommandArgument.ToString(); //lấy email từ CommandArgument
                txtTo.Text = email;
            }
        }

        //Xử lý nút gửi
        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (Page.IsValid) //Ktra dliệu hợp lệ
            {
                string to = txtTo.Text.Trim();
                string subject = txtSubject.Text.Trim();
                string body = txtBody.Text.Trim();

                var emailHelper = new SendMail();
                bool result = emailHelper.SendMailFunction(to, subject, body);

                lblMsg.Visible = true;
                if (result)
                {
                    lblMsg.Text = "Đã gửi email thành công!";
                    lblMsg.CssClass = "alert alert-success";
                }
                else
                {
                    lblMsg.Text = "Gửi email thất bại.";
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
        }
        private void clear()
        {
            txtTo.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtBody.Text = string.Empty;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}