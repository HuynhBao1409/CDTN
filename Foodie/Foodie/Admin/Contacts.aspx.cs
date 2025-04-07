using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Foodie.User;

namespace Foodie.Admin
{
	public partial class Contacts : System.Web.UI.Page
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
                Session["breadCrum"] = "Feedback";
                //Nếu ko có tk admin
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");//Trả về trang Login
                }
                else
                {
                    getContacts();
                }
            }
        }

        private void getContacts()
        {
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("ContactSp", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rContacts.DataSource = dt;
            rContacts.DataBind();
        }

        protected void rContacts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("ContactSp", con);
                //Truyền các tham số của User_Crud
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@ContactId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open(); //Mở db
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Fhản hồi đã xóa thành công!";
                    lblMsg.CssClass = "alert alert-success";
                    getContacts();
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error: " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}