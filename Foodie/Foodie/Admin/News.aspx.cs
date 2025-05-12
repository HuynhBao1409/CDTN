using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Foodie.Admin
{
	public partial class News : System.Web.UI.Page
	{
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
		{
            // Ktra nếu trang được tải lần đầu 
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Bài Viết";
                //Nếu ko có tk admin
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx"); //Trả về trang Login
                }
                else
                {
                    getNews();
                }
            }
            lblMsg.Visible = false;
        }

        //Sự kiện nút Thêm hoặc Ipdate
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false; 
            int newsId = Convert.ToInt32(hdnId.Value); // Lấy IDNews

            // Thiết lập các tham số cho stored procedure
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("News_Crud", con);
            cmd.Parameters.AddWithValue("@Action", newsId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@NewsId", newsId);
            cmd.Parameters.AddWithValue("@Title", txtTitle.Text.Trim());
            cmd.Parameters.AddWithValue("@Content", txtContent.Text.Trim());
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);

            if (fuNewsImage.HasFile) // Ktra xem có file ảnh được tải lên không
            {
                if (Utils.IsValidExtension(fuNewsImage.FileName)) // Ktra định dạng file
                {
                    Guid obj = Guid.NewGuid();
                    fileExtension = Path.GetExtension(fuNewsImage.FileName);
                    imagePath = "../Images/News/" + obj.ToString() + fileExtension;
                    fuNewsImage.PostedFile.SaveAs(Server.MapPath("~/Images/News/") + obj.ToString() + fileExtension);
                    cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
                    isValidToExecute = true;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Hãy chọn đúng định dạng .jpg, .jpeg hoặc .png";
                    lblMsg.CssClass = "alert alert-danger";
                    isValidToExecute = false;
                }
            }
            else // Nếu không có ảnh, trả null
            {
                cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                isValidToExecute = true;
            }
            // Nếu dữ liệu hợp lệ
            if (isValidToExecute)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); //lưu vào db
                    actionName = newsId == 0 ? "đã thêm" : "đã cập nhật";
                    lblMsg.Visible = true;
                    lblMsg.Text = "Tin tức " + actionName + " thành công!";
                    lblMsg.CssClass = "alert alert-success";
                    getNews();
                    clear();
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

        // Hàm lấy tất cả tin tức từ sql
        private void getNews()
        {
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("News_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECTALL");
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rNews.DataSource = dt;
            rNews.DataBind();
        }

        //Xóa dữ liệu trong form
        private void clear()
        {
            txtTitle.Text = string.Empty;
            txtContent.Text = string.Empty;
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOrUpdate.Text = "Thêm";
            imgNews.ImageUrl = string.Empty;
        }

        // Xử lý sự kiện nút Xóa 
        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        // Xử lý các hành động
        protected void rNews_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            con = new SqlConnection(Connection.GetConnectionString());

            if (e.CommandName == "edit") //Nếu sửa
            {
                cmd = new SqlCommand("News_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "SELECT");
                cmd.Parameters.AddWithValue("@NewsId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count > 0) //Nếu tìm thấy tin tức
                {
                    txtTitle.Text = dt.Rows[0]["Title"].ToString();
                    txtContent.Text = dt.Rows[0]["Content"].ToString();
                    cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                    imgNews.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageUrl"].ToString()) ?
                        "../Images/No_image.png" : dt.Rows[0]["ImageUrl"].ToString();
                    imgNews.Height = 200;
                    imgNews.Width = 200;
                    hdnId.Value = dt.Rows[0]["NewsId"].ToString();
                    btnAddOrUpdate.Text = "Cập nhật";
                    LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                    btn.CssClass = "badge badge-warning";
                }
            }
            else if (e.CommandName == "delete") //Nếu xóa
            {
                cmd = new SqlCommand("News_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@NewsId", e.CommandArgument);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Tin tức đã xóa thành công!";
                    lblMsg.CssClass = "alert alert-success";
                    getNews();
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

        //Xử lý Trạng thái
        protected void rNews_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbl = e.Item.FindControl("lblIsActive") as Label; // Tìm control hiển thị trạng thái
                if (lbl.Text == "True")
                {
                    lbl.Text = "Hoạt động";
                    lbl.CssClass = "badge badge-success";
                }
                else
                {
                    lbl.Text = "Ngưng hoạt động";
                    lbl.CssClass = "badge badge-danger";
                }
            }
        }

    }
}