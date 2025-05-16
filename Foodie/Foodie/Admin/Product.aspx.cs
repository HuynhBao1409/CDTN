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
	public partial class Product : System.Web.UI.Page
	{
        //Khai báo
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Sản phẩm";
                //Nếu ko có tk admin
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx"); //Trả về trang Login
                }
                else
                {
                    getProducts();
                }
            }
            lblMsg.Visible = false;
        }

        //Gán sự kiện cho btn Add, Update
        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            bool isValidToExecute = false;
            int productId = Convert.ToInt32(hdnId.Value);
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Product_Crud", con);
            // Truyền các tham số cho stored proc
            cmd.Parameters.AddWithValue("@Action", productId == 0 ? "INSERT" : "UPDATE");
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@Price", txtPrice.Text.Trim());
            cmd.Parameters.AddWithValue("@Quantity", txtQuantity.Text.Trim());
            cmd.Parameters.AddWithValue("@CategoryId", ddlCategories.SelectedValue);
            cmd.Parameters.AddWithValue("@IsActive", cbIsActive.Checked);

            if (fuProductImage.HasFile)
            {
                if (Utils.IsValidExtension(fuProductImage.FileName)) //Kiểm tra định dạng file
                {
                    Guid obj = Guid.NewGuid();
                    fileExtension = Path.GetExtension(fuProductImage.FileName);
                    imagePath = "Images/Product/" + obj.ToString() + fileExtension;
                    fuProductImage.PostedFile.SaveAs(Server.MapPath("~/Images/Product/") + obj.ToString() + fileExtension);
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
                    actionName = productId == 0 ? "đã thêm" : "đã cật nhật";
                    lblMsg.Visible = true;
                    lblMsg.Text = "Sản phẩm " + actionName + " thành công!";
                    lblMsg.CssClass = "alert alert-success";
                    getProducts();
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

        private void getProducts()
        {
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Product_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable(); 
            sda.Fill(dt);
            //Gán dliệu cho id rProduct
            rProduct.DataSource = dt;
            rProduct.DataBind();
        }

        // Reset form trả về mặc định
        private void clear()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtPrice.Text = string.Empty;
            ddlCategories.ClearSelection();
            cbIsActive.Checked = false;
            hdnId.Value = "0";
            btnAddOrUpdate.Text = "Thêm";
            imgProduct.ImageUrl = string.Empty;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        //Gán sự kiện btn Edit, Delete
        protected void rProduct_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            con = new SqlConnection(Connection.GetConnectionString());
            if (e.CommandName == "edit")
            {
                cmd = new SqlCommand("Product_Crud", con);
                //Truyền các tham số dliệu vào Pro_Crud
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", false);
                cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure; 
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable(); 
                sda.Fill(dt);
                // Gán giá trị từ DataTable vào Index
                txtName.Text = dt.Rows[0]["Name"].ToString();
                txtDescription.Text = dt.Rows[0]["Description"].ToString();
                txtPrice.Text = dt.Rows[0]["Price"].ToString();
                txtQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                ddlCategories.SelectedValue = dt.Rows[0]["CategoryId"].ToString();
                cbIsActive.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"]);

                imgProduct.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageUrl"].ToString()) ?
                    "../Images/No_image.png" : "../" + dt.Rows[0]["ImageUrl"].ToString();
                imgProduct.Height = 200;
                imgProduct.Width = 200;
                hdnId.Value = dt.Rows[0]["ProductId"].ToString();
                btnAddOrUpdate.Text = "Update";
                LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                btn.CssClass = "badge badge-warning";
            }
            else if (e.CommandName == "delete") 
            {                
                cmd = new SqlCommand("Product_Crud", con);
                //Truyền các tham số của Pro_Crud
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@Name", DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", false);
                cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                cmd.CommandType = CommandType.StoredProcedure; 
                try
                {
                    con.Open(); 
                    cmd.ExecuteNonQuery();
                    lblMsg.Visible = true;
                    lblMsg.Text = "Sản phẩm đã xóa thành công!";
                    lblMsg.CssClass = "alert alert-success";
                    getProducts(); 
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

        //Gán sự kiện Active và In-Active cho Reapeater
        protected void rProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // Kiểm tra nếu ItemType là mục thông thường hoặc mục xen kẽ
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                //Tìm controllabel của lblIsActive
                Label lblIsActive = e.Item.FindControl("lblIsActive") as Label;
                Label lblQuantity = e.Item.FindControl("lblQuantity") as Label;

                if (lblIsActive.Text == "True")
                {
                    lblIsActive.Text = "Hoạt động";
                    lblIsActive.CssClass = "badge badge-success";
                }
                else
                {
                    lblIsActive.Text = "Ngưng hoạt động";
                    lblIsActive.CssClass = "badge badge-danger";
                }

                if (Convert.ToInt32(lblQuantity.Text) <= 5)
                {
                    lblQuantity.ToolTip = "Sản phẩm sắp hết hàng!";
                    lblQuantity.CssClass = "badge badge-danger";
                }
            }
        }
    }
}