using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foodie.User
{
	public partial class News : System.Web.UI.Page
	{
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getNews();
                lblMsg.Visible = false;
            }
        }

        // Hàm lấy tất cả để hiển thị tin tức
        private void getNews()
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("News_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECTALL");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            // Thêm cột IsNew để đánh dấu tin tức mới
            dt.Columns.Add("IsNew", typeof(bool));
            // Thêm cột TimeAgo để hiển thị thời gian tương đối
            dt.Columns.Add("TimeAgo", typeof(string));

            DateTime currentDate = DateTime.Now; //Lấy ngày hiện tại
            foreach (DataRow row in dt.Rows)
            {
                DateTime createdDate = Convert.ToDateTime(row["CreatedDate"]);
                // Tin mới: Đăng trong vòng 2 ngày gần nhất
                row["IsNew"] = (currentDate - createdDate).TotalDays <= 2;

                // Tính toán thời gian tương đối
                TimeSpan timeDiff = currentDate - createdDate;
                if (timeDiff.TotalMinutes < 60)
                {
                    row["TimeAgo"] = $"{(int)timeDiff.TotalMinutes} phút trước";
                }
                else if (timeDiff.TotalHours < 24)
                {
                    row["TimeAgo"] = $"{(int)timeDiff.TotalHours} giờ trước";
                }
                else if (timeDiff.TotalDays < 30)
                {
                    row["TimeAgo"] = $"{(int)timeDiff.TotalDays} ngày trước";
                }
                else
                {
                    row["TimeAgo"] = createdDate.ToString("dd/MM/yyyy");
                }
            }

            // Tạo DataView để lọc và sắp xếp dữ liệu (mới nhất trước)
            DataView dv = dt.DefaultView;
            dv.RowFilter = "IsActive = 1"; // Chỉ hiển thị đang hoạt động
            dv.Sort = "CreatedDate DESC"; // Sắp xếp tin tức theo thời gian đăng mới nhất

            // Gán tất cả dữ liệu cho Repeater mà không phân trang
            rNews.DataSource = dv;
            rNews.DataBind();
        }
    }
}