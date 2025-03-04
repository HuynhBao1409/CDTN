using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Foodie.Admin;

namespace Foodie.User
{
	public partial class Cart : System.Web.UI.Page
	{
        //Khai báo
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        decimal grandTotal = 0;
        protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    grandTotal = 0;
                    getCartItems();
                }
            }
            else
            {
                grandTotal = 0; // Reset grandTotal cho PostBack
            }
        }
        void getCartItems()
        {
            // Tạo kết nối và cmd cho SQL và stored proc
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT");
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            rCartItem.DataSource = dt;
            if (dt.Rows.Count == 0) //Ktra slượng hàng nếu trống
            {
                //Xóa nút tùy chọn
                rCartItem.FooterTemplate = null; 
                rCartItem.FooterTemplate = new CustomTemplate(ListItemType.Footer);
            }
            rCartItem.DataBind();
        }

        // Xóa đơn hàng khỏi giỏ
        protected void rCartItem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Utils utils = new Utils();
            if(e.CommandName == "remove")
            {
                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Cart_Crud", con);
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@ProductId", e.CommandArgument);
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery(); // Cật nhật Slượng trả thành true
                    getCartItems();
                    // Đếm giỏ hàng
                    Session["cartCount"] = utils.cartCount(Convert.ToInt32(Session["userId"]));
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error- " + ex.Message + " ')<script>");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        //Tính tổng giá đơn hàng
        protected void rCartItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // Tìm và lấy ở các control Label, TextBox
                Label totalPrice = e.Item.FindControl("lblTotalPrice") as Label;
                Label productPrice = e.Item.FindControl("lblPrice") as Label;
                TextBox quantity = e.Item.FindControl("txtQuantity") as TextBox;
                // Kiểm tra và lấy Giá Tiền, loại bỏ " VND" và dấu phẩy
                decimal price = 0;
                if (decimal.TryParse(productPrice.Text.Replace(" VND", "").Replace(",", ""), out decimal parsedPrice))
                {
                    price = parsedPrice;
                }
                // Kiểm tra và lấy Số lượng
                int qty = 0;
                if (int.TryParse(quantity.Text, out int parsedQty))
                {
                    qty = parsedQty;
                }
                // Tính Tổng Tiền = Giá tiền * Số lượng
                decimal calTotalPrice = price * qty;
                totalPrice.Text = calTotalPrice.ToString("N0");
                // Cộng dồn vào tổng giá trị
                grandTotal += calTotalPrice;
            }
            Session["grandTotalPrice"] = grandTotal.ToString("N0");
        }

        // Lớp template khi giỏ hàng trống
        private sealed class CustomTemplate : ITemplate
        {
            private ListItemType ListItemType { get; set; }

            public CustomTemplate(ListItemType type)
            {
                ListItemType = type;
            }

            public void InstantiateIn(Control container)
            {
                if (ListItemType == ListItemType.Footer)
                {
                    var footer = new LiteralControl("<tr><td colspan='5'><b>Giỏ hàng trống.</b><a href='Menu.aspx' class='badge badge-info ml-2'> Quay Lại Mua Sắm Thôi !!</a></td></tr></tbody></table>");
                    container.Controls.Add(footer);
                }
            }
        }
    }
}