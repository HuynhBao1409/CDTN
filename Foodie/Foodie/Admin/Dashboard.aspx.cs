﻿using System;
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
			}
		}
	}
}