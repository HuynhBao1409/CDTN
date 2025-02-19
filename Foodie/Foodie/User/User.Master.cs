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
            // Kiểm tra nếu URL hiện tại KHÔNG chứa "Default.aspx"
            if (!Request.Url.AbsoluteUri.ToString().Contains("Default.aspx"))
			{
				form1.Attributes.Add("class", "sub_page"); // Thêm class "sub_page" vào form1
            }
			else
			{
				form1.Attributes.Remove("class");  // Xóa thuộc tính class khỏi form1
                // Tạo một control mới từ file SliderUserControl.ascx
                Control sliderUserControl = (Control)Page.LoadControl("SliderUserControl.ascx");

                // Thêm control slider vào panel 
                pnlSliderUC.Controls.Add(sliderUserControl);

            }
		}
	}
}