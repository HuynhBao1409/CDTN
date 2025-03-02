using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Foodie
{
	public class Connection
	{
		public static string GetConnectionString()
		{
            // Trả về chuỗi kết nối có tên "cs" từ file cấu hình (.config)
            return ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        }
	}

	public class Utils
	{
        //Khai báo SQL Connect
        SqlConnection con;
        SqlCommand cmd;
        //Check đuôi file (chỉ nhận jpg, png, jpeg)
        public static bool IsValidExtension(string fileName)
		{
            bool isValid = false;
            string[] fileExtension = { ".jpg", ".png", ".jpeg" }; 
            for (int i = 0; i <= fileExtension.Length - 1; i++)  //Check từng file một
			{
                if (fileName.Contains(fileExtension[i]))
				{
                    isValid = true;  
                    break;
				}
			}
            return isValid;
        }

        // Để ảnh mặc định nếu không có ảnh
        public static string GetImageUrl(Object url)
        {
            string url1 = "";
            if (string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
            {
                url1 = "../Images/No_image.png";
            }
            else
            {
                url1 = string.Format("../{0}", url);
            }
            // return ResolveUrl(url1);
            return url1;
        }
        // Cật nhật số lượng giỏ hàng
        public bool updateCartQuantity(int quantity, int productId, int userId)
        {
            bool isUpdated = false; //mặc định false
            //Thêm hàng vào giỏ hàng
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Cart_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "UPDATE");
            cmd.Parameters.AddWithValue("@ProductId", productId);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@UserId", userId);
            //cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                con.Open();
                cmd.ExecuteNonQuery(); // Cật nhật Slượng trả thành true
                isUpdated = true; 
            }
            catch (Exception ex)
            {
                isUpdated = false;
                System.Web.HttpContext.Current.Response.Write("<script>alert('Error- " + ex.Message + " ')<script>");
            }
            finally
            {
                con.Close();
            }
            return isUpdated;
        }

    }

}