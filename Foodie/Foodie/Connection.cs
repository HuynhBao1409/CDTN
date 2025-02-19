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
            string[] fileExtension = { ".jpg", ".png", ".jpeg" }; // Đuôi file hợp lệ
            for (int i = 0; i <= fileExtension.Length - 1; i++)  //Check từng file một
			{
                // Kiểm tra xem file có hợp lệ không
                if (fileName.Contains(fileExtension[i]))
				{
                    isValid = true;  // Hợp lệ đặt flag = true
                    break;
				}
			}
            // Trả về kết quả
            return isValid;
        }
	}

}