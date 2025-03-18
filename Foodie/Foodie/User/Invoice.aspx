<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="Foodie.User.Invoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        /* Ẩn thông báo 5s */
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblMsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>
        <%--Table--%>
        <div class="container">
            <asp:Repeater ID="rOrderItem" runat="server">
                <HeaderTemplate>
                    <table class="table table-responsive-sm table-bordered table-hover" id="tblInVoice">
                        <thead class="bg-dark text-white">
                            <tr>
                                <th>Stt</th>
                                <th>Mã đơn hàng</th>
                                <th>Tên Sản Phẩm</th>
                                <th>Giá tiền</th>
                                <th>SSố Lượng</th>
                                <th>Tổng Cộng</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("SrNo") != DBNull.Value ? Eval("SrNo") : "" %></td>
                        <td><%# Eval("OrderNo") != DBNull.Value ? Eval("OrderNo") : "" %></td>
                        <td><%# Eval("Name") != DBNull.Value ? Eval("Name") : "" %></td>
                        <td><%# Eval("Price") != DBNull.Value ? string.Format("{0:N0}", Eval("Price")) + " VND" : "" %></td>
                        <td><%# Eval("Quantity") != DBNull.Value ? Eval("Quantity") : "" %></td>
                        <td><%# Eval("TotalPrice") != DBNull.Value ? string.Format("{0:N0}", Eval("TotalPrice")) + " VND" : "" %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
                    </table>
                </FooterTemplate>
            </asp:Repeater>

            <div class="text-center">
                <asp:LinkButton ID="lbDownloadInvoice" runat="server" CssClass="btn btn-info"
                    OnClick="lbDownloadInvoice_Click">
                    <i class="fa fa-file-pdf-o mr-2"></i> Tải Hóa Đơn
                </asp:LinkButton>
            </div>

        </div>
    </section>
</asp:Content>
