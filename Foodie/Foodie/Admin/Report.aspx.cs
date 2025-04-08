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
	public partial class Report : System.Web.UI.Page
	{
        //Khai báo
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        //Thanh điều hướng
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Báo Cáo Doanh Thu";
                //Nếu ko có tk admin
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");//Trả về trang Login
                }                
            }
        }
        private void getReportData(DateTime fromDate, DateTime toDate)
        {
            double grandTotal = 0;
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("SellingReport", con);
            // Thêm tham số ngày bắt đầu và kết thúc
            cmd.Parameters.AddWithValue("@FromDate", fromDate);
            cmd.Parameters.AddWithValue("@ToDate", toDate);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            // Nếu có dữ liệu trong bảng
            if (dt.Rows.Count > 0)
            {
                // Duyệt qua row để cộng dồn TotalPrice
                foreach (DataRow dr in dt.Rows)
                {
                    grandTotal += Convert.ToDouble(dr["TotalPrice"]); // Cộng tổng tiền
                }
                lblTotal.Text = "Tổng doanh thu: " + grandTotal.ToString("N0") + " VND";
                lblTotal.CssClass = "badge badge-primary";
            }
            rReport.DataSource = dt;
            rReport.DataBind();
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DateTime fromDate = Convert.ToDateTime(txtFromDate.Text);
            DateTime toDate = Convert.ToDateTime(txtToDate.Text);
            //ktra ngày KThúc lớn hơn hiện tại
            if (toDate > DateTime.Now)
            {
                Response.Write("<script>alert('Ngày kết thúc không được lớn hơn ngày hiện tại!');</script>");
            }
            else if (fromDate > toDate)
            {
                Response.Write("<script>alert('Ngày bắt đầu không được lớn hơn ngày kết thúc!');</script>");
            }
            else
            {
                getReportData(fromDate, toDate);
            }

        }
    }
}