using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Foodie.Admin;

namespace Foodie.User
{
	public partial class Menu : System.Web.UI.Page
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
                getCategories();
                getProducts();
                lblMsg.Visible = false;
            }
		}

        private void getCategories()
        {
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Category_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "ACTIVECAT");
            cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
            cmd.CommandType = CommandType.StoredProcedure; //Gọi thẳng tới stored proc thay vì qua sql
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rCategory.DataSource = dt;
            rCategory.DataBind();
        }

        private void getProducts()
        {
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Product_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "ACTIVEPROD");
            cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            //Gán dliệu cho id rProduct
            rProducts.DataSource = dt;
            rProducts.DataBind();
        }

        //Gán Sự kiện thêm giỏ hàng
        protected void rProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            //Ktra có tk ko
            if (Session["userId"] != null)
            {
                bool isCartItemUpdated = false;
                int i = isItemExistInCart( Convert.ToInt32(e.CommandArgument) );
                if(i == 0)
                {
                    //Thêm hàng vào giỏ hàng
                    con = new SqlConnection(Connection.GetConnectionString());
                    cmd = new SqlCommand("Cart_Crud", con);
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                    cmd.Parameters.AddWithValue("@Quantity", 1); //SLượng mặc định là 1
                    cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                    cmd.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        lblMsg.Visible = true;
                        lblMsg.Text = "Đã thêm sản phẩm vào giỏ hàng. <b><a href='Cart.aspx'>Xem giỏ hàng ngay</a></b>";
                        lblMsg.CssClass = "alert alert-success";
                        // Hiển thị thông báo và ẩn sau 3 giây
                        string script = "<script type='text/javascript'>setTimeout(function(){ document.getElementById('" + lblMsg.ClientID + "').style.display = 'none'; }, 3000);</script>";
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMessage", script);
                    }
                    catch(Exception ex)
                    {
                        Response.Write("<script>alert('Error - "+ ex.Message+ " ')<script>");
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                else //Đơn > 0
                {
                    //Thêm hàng đang có vào giỏ hàng
                    Utils utils = new Utils();
                    isCartItemUpdated = utils.updateCartQuantity(i + 1, Convert.ToInt32(e.CommandArgument), 
                        Convert.ToInt32(Session["userId"])); //Cật nhật Slượng mỗi lần thêm 1 đơn + 1
                    lblMsg.Visible = true;
                    lblMsg.Text = "Đã thêm sản phẩm vào giỏ hàng. <b><a href='Cart.aspx'>Xem giỏ hàng ngay</a></b>";
                    lblMsg.CssClass = "alert alert-success";
                    // Hiển thị thông báo mất sau 3s
                    string script = "<script type='text/javascript'>setTimeout(function(){ document.getElementById('" + lblMsg.ClientID + "').style.display = 'none'; }, 3000);</script>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "HideMessage", script);
                    // Tự chuyển hướng sau 1s
                    //string script = "<script type='text/javascript'>setTimeout(function(){ window.location = 'Cart.aspx'; }, 1000);</script>";
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script);

                }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }
        }

        //Lấy Id sphẩm của user
        int isItemExistInCart(int productId)
        {
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "GETBYID");
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            int quantity = 0;
            if(dt.Rows.Count > 0) //có hàng trong giỏ
            {
                quantity = Convert.ToInt32(dt.Rows[0]["Quantity"]); //chuyển slượng thành int
            }
            return quantity;
        }

        //public string LowerCase(object obj)
        //{
        //    return obj.ToString().ToLower();
        //}

    }
}