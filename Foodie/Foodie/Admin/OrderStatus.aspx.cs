using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Foodie.Admin
{
	public partial class OrderStatus : System.Web.UI.Page
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
                Session["breadCrum"] = "Trạng thái Đơn Hàng";
                //Nếu ko có tk admin
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx"); //Trả về trang Login
                }
                else
                {
                    getOrderStatus();
                }
            }
            lblMsg.Visible = false;
            pUpdateOrderStatus.Visible = false;
        }
        private void getOrderStatus()
        {
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Invoice", con);
            cmd.Parameters.AddWithValue("@Action", "GETSTATUS");
            cmd.CommandType = CommandType.StoredProcedure; //Gọi thẳng tới stored proc thay vì qua sql
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rOrderStatus.DataSource = dt;
            rOrderStatus.DataBind();
        }

        //Edit Order Status
        protected void rOrderStatus_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            if (e.CommandName == "edit")
            {
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Invoice", con);
                //Truyền các tham số vào Invoice
                cmd.Parameters.AddWithValue("@Action", "STATUSBYID");
                cmd.Parameters.AddWithValue("@OrderDetailsId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                // Gán giá trị từ DataTable vào Index
                ddlOrderStatus.SelectedValue = dt.Rows[0]["Status"].ToString();                
                hdnId.Value = dt.Rows[0]["OrderDetailsId"].ToString();
                pUpdateOrderStatus.Visible = true;
                LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                btn.CssClass = "badge badge-warning";
            }
        }

        //Btn Update
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int orderDetailsId = Convert.ToInt32(hdnId.Value);
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Invoice", con);
            // Truyền các tham số cho stored proc
            cmd.Parameters.AddWithValue("@Action", "UPDSTATUS");
            cmd.Parameters.AddWithValue("@OrderDetailsId", orderDetailsId);
            cmd.Parameters.AddWithValue("@Status", ddlOrderStatus.SelectedValue);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                lblMsg.Visible = true;
                lblMsg.Text = "Cập nhật trạng thái thành công!";
                lblMsg.CssClass = "alert alert-success";
                getOrderStatus();
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

        //Btn Cancel
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pUpdateOrderStatus.Visible = false;
        }
    }
}