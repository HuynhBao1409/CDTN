using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.User
{
	public partial class User : System.Web.UI.MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            // Kiểm tra nếu URL hiện tại 
            if (!Request.Url.AbsoluteUri.ToString().Contains("Default.aspx"))
			{
				form1.Attributes.Add("class", "sub_page"); 
            }
			else
			{
				form1.Attributes.Remove("class");  // Xóa thuộc tính class khỏi form1
                // Tạo một control mới 
                Control sliderUserControl = (Control)Page.LoadControl("SliderUserControl.ascx");
                // Thêm control slider vào panel 
                pnlSliderUC.Controls.Add(sliderUserControl);

            }
            //Trạng thái btn
            if (Session["userId"] != null)
            {
                lbLoginOrLogout.Text = "Đăng xuất";
            }
            else
            {
                lbLoginOrLogout.Text = "Đăng nhập";        
            }
		}

        //Sự kiện Login/Logout
        protected void lbLoginOrLogout_Click(object sender, EventArgs e)
        {
            if (Session["userId"] == null) //Ktra nếu ch TK 
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                Session.Abandon(); //xóa toàn bộ dữ liệu
                Response.Redirect("Login.aspx");
            }
        }

        protected void lbRegisterOrProfile_Click(object sender, EventArgs e)
        {
            if (Session["userId"] != null) //Ktra nếu có TK
            {
                lbRegisterOrProfile.ToolTip = "Hồ Sơ Cá Nhân";
                Response.Redirect("Profile.aspx");
            }
            else
            {
                lbRegisterOrProfile.ToolTip = "Đăng ký Tài Khoản";
                Response.Redirect("Registration.aspx");
            }
        }
    }
}