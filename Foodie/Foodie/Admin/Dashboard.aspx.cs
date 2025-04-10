using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.Admin
{
	public partial class Dashboard : System.Web.UI.Page
	{
		//Gán sự kiện cho thanh điều hướng
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				Session["breadCrum"] = "Trang chủ";
				//Nếu ko có tk admin
				if (Session["admin"] == null) 
                {
					Response.Redirect("../User/Login.aspx"); //Trả về trang Login
                }
				else
				{
                    // Tạo DashboardCount và Đếm số lượng trong hệ thống
                    DashboardCount dashboard = new DashboardCount();
					Session["category"] = dashboard.Count("CATEGORY");
					Session["product"] = dashboard.Count("PRODUCT");
					Session["order"] = dashboard.Count("ORDER");
					Session["delivered"] = dashboard.Count("DELIVERED");
					Session["pending"] = dashboard.Count("PENDING");
					Session["cancelled"] = dashboard.Count("CANCELLED");
					Session["user"] = dashboard.Count("USER");
					Session["soldAmount"] = dashboard.Count("SOLDAMOUNT");
					Session["contact"] = dashboard.Count("CONTACT");
				}
			}
		}
	}
}