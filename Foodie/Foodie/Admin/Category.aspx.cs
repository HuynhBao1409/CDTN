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

        //Gán sự kiện cho thanh điều hướng
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Danh mục";
                getCategories();
            }
            lblMsg.Visible = false; //Ẩn thông báo
        }

        //Gán sự kiện cho btn Add Category
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            // Khai báo các biến
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int categoryId = Convert.ToInt32(hdnId.Value); 
            // Tạo kết nối DB
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Crud", con);
            // Thêm các tham số cho stored procedures
            cmd.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");  // Nếu ID = 0 thì Insert, không thì Update
            cmd.Parameters.AddWithValue("@CategoryId", categoryId); // Truyền ID vào
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);

            // Kiểm tra xem có upload ảnh không
            if (fuCategoryImage.HasFile)  
            {
                if (Utils.IsValidExtension(fuCategoryImage.FileName)) //Kiểm tra định dạng file
                {
                    // Tạo tên file ngẫu nhiên bằng Guid
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
                    // Báo lỗi nếu không đúng định dạng
                    lblMsg.Visible = true;
                    lblMsg.Text = "Hãy chọn đúng định dạng .jpg, .jpeg hoặc .png ";
                    lblMsg.CssClass = "alert alert-danger";
                    isValidToExecute = false;
                }
            }
            else
            {
                cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
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
                    getCategories();
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

        private void getCategories()
        {
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Crud", con);
            //Truyền tham số
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd); //lấy dliệu từ DB
            dt = new DataTable(); //tạo table mới
            sda.Fill(dt);
            //Gán dliệu cho id rCategory
            rCategory.DataSource = dt;
            rCategory.DataBind();
        }

        // Reset form trả về mặc định
        private void clear()
        {
            txtName.Text = string.Empty;
            cbIsActive.Checked = false;
            hdnId.Value = "0"; 
            btnAddOrUpdate.Text = "Add";
            imgCategory.ImageUrl = string.Empty;
        }

        //btn_Clear
        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        //Gán sự kiện btn Edit, Delete
        protected void rCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false; 
            con = new SqlConnection(Connection.GetConnectionString()); //Kết nối SQL

            if (e.CommandName == "edit") // Kiểm tra Command nếu là "edit"
            {
                cmd = new SqlCommand("Category_Crud", con); // Gọi stored proc
                //Truyền các tham số dliệu cần chỉnh sửa của Cate_Crud
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@CategoryId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", false);
                cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable(); //Tạo DataTable để chứa dữ liệu
                sda.Fill(dt);
                // Gán giá trị từ DataTable vào Index
                txtName.Text = dt.Rows[0]["Name"].ToString();
                cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                // Kiểm tra nếu ko có ảnh thì dùng ảnh mặc định
                imgCategory.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageUrl"].ToString()) ? 
                    "../Images/No_image.png" : "../" + dt.Rows[0]["ImageUrl"].ToString();
                imgCategory.Height = 200;
                imgCategory.Width = 200;
                hdnId.Value = dt.Rows[0]["CategoryId"].ToString();
                btnAddOrUpdate.Text = "Update";
                LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                btn.CssClass = "badge badge-warning";
            }
            else if (e.CommandName == "delete") // Kiểm tra nếu là "delete"
            {
                //con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Category_Crud", con); // Gọi stored proc
                //Truyền các tham số dliệu cần xóa của Cate_Crud
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@CategoryId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", false);
                cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure; // Đặt kiểu lệnh cho stored proc
                try
                {
                    con.Open(); //mở SQL
                    cmd.ExecuteNonQuery(); //Thực thị xóa
                    lblMsg.Visible = true;
                    lblMsg.Text = "Danh mục đã xóa thành công!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories(); // Cập nhật lại dsách
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true; // Hiển thị thông báo
                    lblMsg.Text = "Error: " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close(); // Đóng SQL
                }
            }
        }

        //Gán sự kiện Active và In-Active cho Reapeater
        protected void rCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Kiểm tra nếu ItemType là mục thông thường hoặc mục xen kẽ
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Tìm controllabel của lblIsActive
                Label lbl = e.Item.FindControl("lblIsActive") as Label;
                // Kiểm tra lbl
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