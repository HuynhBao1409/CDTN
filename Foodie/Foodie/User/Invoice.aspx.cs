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
        protected DataTable GetOrderDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(Connection.GetConnectionString()))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Invoice", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "INVOICBYID");
                        int paymentId;
                        if (!int.TryParse(Request.QueryString["id"], out paymentId))
                        {
                            throw new ArgumentException("Mã thanh toán không hợp lệ");
                        }
                        cmd.Parameters.AddWithValue("@PaymentId", paymentId);
                        if (Session["userId"] == null)
                        {
                            throw new ArgumentException("Mã người dùng không tồn tại trong phiên làm việc");
                        }
                        cmd.Parameters.AddWithValue("@UserId", Convert.ToInt32(Session["userId"]));

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataSet ds = new DataSet();
                            sda.Fill(ds);

                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0].Copy();
                                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                                {
                                    // Kiểm tra và gộp bảng tổng
                                    DataTable totalTable = ds.Tables[1].Copy();
                                    dt.Merge(ds.Tables[1]);
                                }
                            }
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Error: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
                return dt;
            }
        }

        //Download Bills
        protected void lbDownloadInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                string downloadPath = @"D:\HoaDon_ThanhToan.pdf";
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

        //Tạo file Hóa Đơn
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
                // Font hỗ trợ tiếng Việt
                string fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\arial.ttf";
                BaseFont bfntUnicode = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                // Font cho tiêu đề chính
                Font fntHead = new Font(bfntUnicode, 18, Font.BOLD, new BaseColor(33, 37, 41)); // Màu xám đậm

                // Tiêu đề chính
                Paragraph prgHeading = new Paragraph();
                prgHeading.Alignment = Element.ALIGN_CENTER;
                prgHeading.SpacingAfter = 10f;
                prgHeading.Add(new Chunk(strHeader, fntHead));
                document.Add(prgHeading);

                // Font cho thông tin bổ sung
                Font fntAuthor = new Font(bfntUnicode, 10, Font.ITALIC, new BaseColor(108, 117, 125)); // Màu xám nhạt

                // Thông tin cửa hàng, ngày đặt, và phương thức thanh toán
                Paragraph prgAuthor = new Paragraph();
                prgAuthor.Alignment = Element.ALIGN_RIGHT;
                prgAuthor.SpacingAfter = 10f;
                prgAuthor.Add(new Chunk("Đặt hàng từ: FastBite Food", fntAuthor));

                // Ngày đặt hàng
                string orderDate = "Không có";
                if (dtblTable.Rows.Count > 0 && dtblTable.Columns.Contains("OrderDate") &&
                    dtblTable.Rows[0]["OrderDate"] != DBNull.Value)
                {
                    orderDate = Convert.ToDateTime(dtblTable.Rows[0]["OrderDate"]).ToString("dd/MM/yyyy");
                }
                prgAuthor.Add(new Chunk("\nNgày đặt: " + orderDate, fntAuthor));

                // Phương thức thanh toán
                string paymentMethod = "Không xác định";
                if (dtblTable.Rows.Count > 0 && dtblTable.Columns.Contains("PaymentMode"))
                {
                    object paymentModeValue = dtblTable.Rows[0]["PaymentMode"];
                    if (paymentModeValue != DBNull.Value && paymentModeValue != null)
                    {
                        string paymentMode = paymentModeValue.ToString().ToLower();
                        if (paymentMode == "cod")
                        {
                            paymentMethod = "Thanh toán khi nhận hàng";
                        }
                        else if (paymentMode == "card")
                        {
                            paymentMethod = "Thẻ tín dụng";
                            if (dtblTable.Columns.Contains("CardNo") && dtblTable.Rows[0]["CardNo"] != DBNull.Value &&
                                !string.IsNullOrEmpty(dtblTable.Rows[0]["CardNo"].ToString()))
                            {
                                paymentMethod += " (" + dtblTable.Rows[0]["CardNo"].ToString() + ")";
                            }
                        }
                        else if (paymentMode == "qr")
                        {
                            paymentMethod = "Mã QR";
                        }
                    }
                }
                prgAuthor.Add(new Chunk("\nPhương thức thanh toán: " + paymentMethod, fntAuthor));
                document.Add(prgAuthor);

                // Đường phân cách
                Paragraph line = new Paragraph(new Chunk(
                    new iTextSharp.text.pdf.draw.LineSeparator(0.5F, 100.0F, new BaseColor(200, 200, 200), Element.ALIGN_LEFT, 1)
                ));
                document.Add(line);

                // Dòng trống
                document.Add(new Paragraph(" ", fntAuthor));

                // Danh sách cột hiển thị 
                List<string> displayColumns = new List<string> {
                    "SrNo", "OrderNo", "Name", "Price", "Quantity", "TotalPrice"
                };
                List<string> columnHeaders = new List<string> {
                    "STT", "Mã đơn hàng", "Tên sản phẩm", "Đơn giá", "Số lượng", "Thành tiền"
                };

                // Tạo bảng
                PdfPTable table = new PdfPTable(displayColumns.Count);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 0.8f, 3.5f, 2.5f, 1.8f, 1.2f, 2.2f });

                // Font cho tiêu đề cột
                Font fntColumnHeader = new Font(bfntUnicode, 10, Font.BOLD, BaseColor.WHITE);

                // Thêm tiêu đề cột
                for (int i = 0; i < columnHeaders.Count; i++)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.BackgroundColor = new BaseColor(108, 117, 125);
                    cell.Padding = 8;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.BorderColor = new BaseColor(200, 200, 200);
                    cell.AddElement(new Chunk(columnHeaders[i], fntColumnHeader));
                    table.AddCell(cell);
                }

                // Font cho dữ liệu
                Font fntColumnData = new Font(bfntUnicode, 9, Font.NORMAL, BaseColor.BLACK);
                Font fntTotalRow = new Font(bfntUnicode, 10, Font.BOLD, BaseColor.BLACK);

                // Thêm dữ liệu vào bảng
                for (int i = 0; i < dtblTable.Rows.Count; i++)
                {
                    bool isLastRow = (i == dtblTable.Rows.Count - 1);
                    Font currentFont = isLastRow ? fntTotalRow : fntColumnData;

                    for (int j = 0; j < displayColumns.Count; j++)
                    {
                        string colName = displayColumns[j];
                        if (dtblTable.Columns.Contains(colName))
                        {
                            PdfPCell cell = new PdfPCell();
                            cell.Padding = 8;
                            cell.BorderColor = new BaseColor(200, 200, 200);

                            string value = "";
                            if (dtblTable.Rows[i][colName] != DBNull.Value && dtblTable.Rows[i][colName] != null)
                            {
                                value = dtblTable.Rows[i][colName].ToString();

                                if (colName == "TotalPrice" || colName == "Price")
                                {
                                    double numValue;
                                    if (double.TryParse(value, out numValue))
                                    {
                                        value = string.Format("{0:N0} VND", numValue);
                                    }
                                }
                            }

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

                            if (isLastRow && colName == "Name")
                            {
                                value = "Tổng cộng";
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }

                            if (!isLastRow && i % 2 == 1)
                            {
                                cell.BackgroundColor = new BaseColor(245, 245, 245);
                            }

                            Paragraph cellContent = new Paragraph(value, currentFont);
                            cell.AddElement(cellContent);
                            table.AddCell(cell);
                        }
                    }
                }

                document.Add(table);

                // Chân trang
                Paragraph footer = new Paragraph();
                footer.SpacingBefore = 20;
                footer.Alignment = Element.ALIGN_CENTER;
                footer.Add(new Chunk("Cảm ơn quý khách đã đặt hàng tại FastBite Food!", fntAuthor));
                document.Add(footer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi xuất PDF: " + ex.Message);
            }
            finally
            {
                document.Close();
                writer.Close();
                fs.Close();
            }
        }
    }
}