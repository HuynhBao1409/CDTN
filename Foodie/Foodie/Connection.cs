using System;
using System.Collections.Generic;
using System.Configuration;
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

    }

}