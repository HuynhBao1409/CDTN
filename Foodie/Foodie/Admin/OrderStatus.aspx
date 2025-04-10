<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="OrderStatus.aspx.cs" Inherits="Foodie.Admin.OrderStatus" %>

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

    <div class="pcoded-inner-content pt-0">
        <div class="align-align-self-end">
            <!-- label thông báo -->
            <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        </div>

        <div class="main-body">
            <div class="page-wrapper">
                <div class="page-body">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header">
                                </div>
                                <!-- Form OrderStatus-->
                                <div class="card-block">
                                    <div class="row">
                                        <div class="col-sm-6 col-md-8 col-lg-8">
                                            <h4 class="sub-title">Danh Sách Đặt Hàng</h4>

                                            <div class="card-block table-border-style">
                                                <div class="table-responsive">
                                                    <!-- Datatable -->
                                                    <asp:Repeater ID="rOrderStatus" runat="server" OnItemCommand="rOrderStatus_ItemCommand">
                                                        <HeaderTemplate>
                                                            <table class="table data-table-export table-hover nowrap">
                                                                <thead>
                                                                    <tr>
                                                                        <th class="table-plus">Mã Đơn Hàng</th>
                                                                        <th>Ngày Đặt Hàng</th>
                                                                        <th>Trạng thái</th>
                                                                        <th>Tên Sản Phẩm</th>
                                                                        <th>Tổng Tiền</th>
                                                                        <th>Phương Thức Thanh Toán</th>
                                                                        <th class="datatable-nosort">Chỉnh Sửa</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="table-plus"><%#Eval("OrderNo") %> </td>
                                                                <td>
                                                                    <%#Eval("OrderDate") %>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'
                                                                        CssClass='<%# Eval("Status").ToString() == "Đã giao" ? "badge badge-success" : 
                                                                                   Eval("Status").ToString() == "Đã hủy đơn" ? "badge badge-danger" : "badge badge-warning" %>'>
                                                                    </asp:Label>
                                                                </td>
                                                                <td><%#Eval("Name") %> </td>
                                                                <td class="text-right"><%# Eval("TotalPrice") != DBNull.Value ? string.Format("{0:N0}", Convert.ToDouble(Eval("TotalPrice"))) + " VND" : "" %> </td>
                                                                <td><%#Eval("PaymentMode") %> </td>
                                                                <td>
                                                                    <!-- btn -->
                                                                    <asp:LinkButton ID="lnkEdit" Text="Edit" runat="server" CssClass="badge badge-primary"
                                                                        CommandArgument='<%# Eval("OrderDetailsId") %>' CommandName="edit">
                                                                         <i class="ti-pencil"></i>
                                                                     </asp:LinkButton>                                                                    
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tbody>
                                                            </table>
           
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                    <!-- Datatable end -->
                                                </div>
                                            </div>

                                        </div>
                                        <!-- Update Status -->
                                        <div class="col-sm-6 col-md-4 col-lg-4 mobile-inputs">
                                            <asp:Panel ID="pUpdateOrderStatus" runat="server">
                                            <h4 class="sub-title">Cập Nhật Trạng Thái</h4>
                                            <div>
                                                 <div class="form-group">
                                                     <label>Trạng Thái Đơn Hàng</label>
                                                     <div>
                                                         <!-- Form Edit -->
                                                         <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="form-control">
                                                             <asp:ListItem Value="0">Chọn Trạng Thái</asp:ListItem>
                                                             <asp:ListItem>Đang xử lý</asp:ListItem>
                                                             <asp:ListItem>Đã gửi đi</asp:ListItem>
                                                             <asp:ListItem>Đã giao</asp:ListItem>
                                                             <asp:ListItem>Đã hủy đơn</asp:ListItem>
                                                         </asp:DropDownList>
                                                         <asp:RequiredFieldValidator ID="rfvDdlOrderStatus" runat="server" ForeColor="Red" ControlToValidate="ddlOrderStatus"
                                                             ErrorMessage="Trạng Thái không được để trống" SetFocusOnError="true" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                                         <asp:HiddenField ID="hdnId" runat="server" Value="0" />
                                                     </div>
                                                 </div>                                                 
                                                 <!-- Button -->
                                                 <div class="pb-5">
                                                     <asp:Button ID="btnUpdate" runat="server" Text="Cập nhật" CssClass="btn btn-primary"
                                                         OnClick="btnUpdate_Click" />
                                                     &nbsp;     
                                                     <asp:Button ID="btnCancel" runat="server" Text="Hủy" CssClass="btn btn-primary"
                                                          OnClick="btnCancel_Click" />
                                                 </div>
                                            </div>
                                            </asp:Panel>                                            
                                        </div>

                                    </div>
                                </div>
                                <!-- End Form -->
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
