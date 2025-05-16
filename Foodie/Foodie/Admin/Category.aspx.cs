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
                Session["breadCrum"] = "Danh mục";
                //Nếu ko có tk admin
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx"); //Trả về trang Login
                }
                else
                {
                    getCategories();
                }
            }
            lblMsg.Visible = false;
        }

        //Gán sự kiện cho btn Add Category
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int categoryId = Convert.ToInt32(hdnId.Value); 
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Crud", con);
            // Truyền các tham số cho stored proc
            cmd.Parameters.AddWithValue("@Action", categoryId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@CategoryId", categoryId); 
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);

            if (fuCategoryImage.HasFile)  
            {
                if (Utils.IsValidExtension(fuCategoryImage.FileName)) //Kiểm tra định dạng file
                {
                    Guid obj = Guid.NewGuid();
                    fileExtension = Path.GetExtension(fuCategoryImage.FileName);
                    imagePath = "../Images/Category/" + obj.ToString() + fileExtension;
                    fuCategoryImage.PostedFile.SaveAs(Server.MapPath("~/Images/Category/") + obj.ToString() + fileExtension);
                    cmd.Parameters.AddWithValue("@ImageUrl", imagePath);
                    isValidToExecute = true;
                }
                else
                {
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

            if (isValidToExecute) 
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); //Chèn dl vào db
                    actionName = categoryId == 0 ? "đã thêm" : "đã cật nhật";
                    lblMsg.Visible = true;
                    lblMsg.Text = "Danh mục " + actionName + " thành công!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories();
                    clear();
                }
                catch (Exception ex)
                {
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
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
            cmd.CommandType = CommandType.StoredProcedure; //Gọi thẳng tới stored proc thay vì qua sql
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rCategory.DataSource = dt;
            rCategory.DataBind();
        }

        // Reset form trả về mặc định
        private void clear()
        {
            txtName.Text = string.Empty;
            cbIsActive.Checked = false;
            hdnId.Value = "0"; 
            btnAddOrUpdate.Text = "Thêm";
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
            con = new SqlConnection(Connection.GetConnectionString());

            if (e.CommandName == "edit") 
            {
                cmd = new SqlCommand("Category_Crud", con);
                //Truyền các tham số vào Cate_Crud
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@CategoryId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", false);
                cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure;
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable(); 
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
            else if (e.CommandName == "delete") 
            {
                //con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Category_Crud", con); 
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@CategoryId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", false);
                cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Danh mục đã xóa thành công!";
                    lblMsg.CssClass = "alert alert-success";
                    getCategories();
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

        //Gán sự kiện Active và In-Active
        protected void rCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Kiểm tra nếu ItemType là mục thông thường hoặc mục xen kẽ
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lbl = e.Item.FindControl("lblIsActive") as Label;
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