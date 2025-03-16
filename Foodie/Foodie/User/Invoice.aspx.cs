using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using System.IO;

namespace Foodie.User
{
    public partial class Invoice : System.Web.UI.Page
    {
        // Khai báo
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userId"] != null)
                {
                    if (Request.QueryString["id"] != null)
                    {
                        rOrderItem.DataSource = GetOrderDetails();
                        rOrderItem.DataBind();
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        // Chứa dữ liệu đơn hàng
        DataTable GetOrderDetails()
        {
            double grandTotal = 0; // mặc định
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Invoice", con);
            cmd.Parameters.AddWithValue("@Action", "INVOICEBYID");
            cmd.Parameters.AddWithValue("@PaymentId", Convert.ToInt32(Request.QueryString["id"]));
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count > 0) // Nếu có đơn
            {
                foreach (DataRow drow in dt.Rows) // Lượt qua các dòng
                {
                    grandTotal += Convert.ToDouble(drow["TotalPrice"]);
                }
            }

            // Thêm hàng tổng kết
            DataRow dr = dt.NewRow();
            dr["TotalPrice"] = grandTotal;
            dt.Rows.Add(dr);
            return dt;
        }

        protected void lbDownloadInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                string downloadPath = @"D:\order_invoice.pdf";
                DataTable dtbl = GetOrderDetails();
                ExportToPdf(dtbl, downloadPath, "Hóa Đơn Mua Hàng");

                WebClient client = new WebClient();
                Byte[] buffer = client.DownloadData(downloadPath);
                if (buffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length", buffer.Length.ToString());
                    Response.BinaryWrite(buffer);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Error Message:- " + ex.Message.ToString();
            }
        }

        void ExportToPdf(DataTable dtblTable, string strPdfPath, string strHeader)
        {
            // Tạo file PDF
            FileStream fs = new FileStream(strPdfPath, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            try
            {
                // ----- PHẦN TIÊU ĐỀ BÁO CÁO -----
                BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fntHead = new Font(bfntHead, 16, Font.BOLD, BaseColor.GRAY);

                // Tiêu đề chính
                Paragraph prgHeading = new Paragraph();
                prgHeading.Alignment = Element.ALIGN_CENTER;
                prgHeading.Add(new Chunk(strHeader.ToUpper(), fntHead));
                document.Add(prgHeading);

                // ----- THÔNG TIN THÊM -----
                BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fntAuthor = new Font(btnAuthor, 8, Font.ITALIC, BaseColor.GRAY);

                // Tên cửa hàng và ngày đặt hàng
                Paragraph prgAuthor = new Paragraph();
                prgAuthor.Alignment = Element.ALIGN_RIGHT;
                prgAuthor.Add(new Chunk("Order From: Foodie Fast Food", fntAuthor));

                // Thêm ngày đặt hàng nếu có
                string orderDate = "N/A";
                if (dtblTable.Rows.Count > 0 && dtblTable.Columns.Contains("OrderDate") &&
                    dtblTable.Rows[0]["OrderDate"] != DBNull.Value)
                {
                    orderDate = Convert.ToDateTime(dtblTable.Rows[0]["OrderDate"]).ToString("dd/MM/yyyy");
                }
                prgAuthor.Add(new Chunk("\nOrder Date: " + orderDate, fntAuthor));
                document.Add(prgAuthor);

                // ----- ĐƯỜNG PHÂN CÁCH -----
                Paragraph line = new Paragraph(new Chunk(
                    new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)
                ));
                document.Add(line);
                document.Add(new Chunk("\n", fntHead)); // Thêm dòng trống

                // ----- BẢNG DỮ LIỆU -----
                // Danh sách các cột cần hiển thị
                List<string> displayColumns = new List<string> {
            "SrNo", "OrderNo", "Name", "Price", "Quantity", "TotalPrice"
        };

                // Tạo bảng
                PdfPTable table = new PdfPTable(displayColumns.Count);
                table.WidthPercentage = 100; // Sử dụng 100% chiều rộng có sẵn
                table.SetWidths(new float[] { 1f, 2f, 3f, 2f, 1.5f, 2f }); // Tỷ lệ chiều rộng các cột

                // Font cho header
                Font fntColumnHeader = new Font(bfntHead, 9, Font.BOLD, BaseColor.WHITE);

                // Thêm header cho bảng
                foreach (string colName in displayColumns)
                {
                    if (dtblTable.Columns.Contains(colName))
                    {
                        PdfPCell cell = new PdfPCell();
                        cell.BackgroundColor = BaseColor.GRAY;
                        cell.Padding = 5;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.AddElement(new Chunk(dtblTable.Columns[colName].ColumnName.ToUpper(), fntColumnHeader));
                        table.AddCell(cell);
                    }
                }

                // Font cho dữ liệu
                Font fntColumnData = new Font(bfntHead, 8, Font.NORMAL, BaseColor.BLACK);
                Font fntTotalRow = new Font(bfntHead, 9, Font.BOLD, BaseColor.BLACK);

                // Thêm dữ liệu vào bảng
                for (int i = 0; i < dtblTable.Rows.Count; i++)
                {
                    bool isLastRow = (i == dtblTable.Rows.Count - 1); // Kiểm tra hàng tổng
                    Font currentFont = isLastRow ? fntTotalRow : fntColumnData;

                    foreach (string colName in displayColumns)
                    {
                        if (dtblTable.Columns.Contains(colName))
                        {
                            PdfPCell cell = new PdfPCell();
                            cell.Padding = 5;

                            // Xử lý hiển thị giá trị
                            string value = "";
                            if (dtblTable.Rows[i][colName] != DBNull.Value && dtblTable.Rows[i][colName] != null)
                            {
                                value = dtblTable.Rows[i][colName].ToString();

                                // Định dạng tiền tệ cho cột giá
                                if (colName == "TotalPrice" || colName == "Price")
                                {
                                    double numValue;
                                    if (double.TryParse(value, out numValue))
                                    {
                                        value = string.Format("{0:N0} VND", numValue);
                                    }
                                }
                            }

                            // Định dạng căn chỉnh theo loại dữ liệu
                            if (colName == "TotalPrice" || colName == "Price" || colName == "Quantity")
                            {
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else if (colName == "SrNo")
                            {
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            }
                            else
                            {
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }

                            // Định dạng hàng tổng
                            if (isLastRow && colName == "Name")
                            {
                                value = "TỔNG CỘNG";
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }

                            Paragraph cellContent = new Paragraph(value, currentFont);
                            cell.AddElement(cellContent);
                            table.AddCell(cell);
                        }
                    }
                }

                document.Add(table);

                // ----- THÔNG TIN FOOTER -----
                Paragraph footer = new Paragraph();
                footer.SpacingBefore = 20;
                footer.Alignment = Element.ALIGN_CENTER;
                footer.Add(new Chunk("Cảm ơn quý khách đã đặt hàng tại Foodie Fast Food!", fntAuthor));
                document.Add(footer);
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi tại đây thay vì sử dụng alert
                System.Diagnostics.Debug.WriteLine("PDF Export Error: " + ex.Message);
            }
            finally
            {
                // Đảm bảo các tài nguyên được đóng đúng cách
                document.Close();
                writer.Close();
                fs.Close();
            }
        }
    }
}