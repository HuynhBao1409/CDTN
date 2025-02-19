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
	public partial class Category : System.Web.UI.Page
	{
        //Khai báo SQL Connect
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            // Khai báo các biến
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int categoryId = Convert.ToInt32(hdnId.Value);  // Lấy ID từ hidden field, = 0 thêm mới, >= 0 thì cập nhật
            // Tạo kết nối DB
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Crud", con);
            // Thêm các parameter cho stored proc
            cmd.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");  // Nếu ID = 0 thì Insert, không thì Update
            cmd.Parameters.AddWithValue("@CategoryId", categoryId); // Truyền ID vào
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);

            // Kiểm tra xem có upload ảnh không
            if (fuCategoryImage.HasFile)  
            {
                if (Utils.IsValidExtension(fuCategoryImage.FileName)) //Kiểm tra định dạng file
                {
                    // Tạo tên file ngẫu nhiên bằng GUID
                    Guid obj = Guid.NewGuid();
                    fileExtension = Path.GetExtension(fuCategoryImage.FileName);
                    imagePath = "Images/Category/" + obj.ToString() + fileExtension;
                    // Lưu file vào thư mục ~/Images/Category/
                    fuCategoryImage.PostedFile.SaveAs(Server.MapPath("~/Images/Category/") + obj.ToString() + fileExtension);
                    cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
                    isValidToExecute = true;
                }
                else
                {
                    // Báo lỗi nếu file không đúng định dạng
                    lblMsg.Visible = true;
                    lblMsg.Text = "Hãy chọn đúng định dạng .jpg, .jpeg hoặc .png ";
                    lblMsg.CssClass = "alert alert-danger";
                    isValidToExecute = false;
                }
            }
            else
            {
                isValidToExecute = true;
            }
            // Nếu mọi thứ hợp lệ thì tiếp tục kiểm tra
            if (isValidToExecute) 
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    // Hiển thị thông báo thành công
                    actionName = categoryId == 0 ? "đã thêm" : "đã cật nhật";
                    lblMsg.Visible = true;
                    lblMsg.Text = "Danh mục " + actionName + " thành công!";
                    lblMsg.CssClass = "alert alert-success";
                    //getCategories();
                    clear();
                }
                catch (Exception ex)
                {
                    // Hiển thị lỗi nếu có
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error - " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close();
                }
            }
        }

        // Reset form về mặc định
        private void clear()
        {
            txtName.Text = string.Empty;
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOrUpdate.Text = "Add";
        }
    }
}