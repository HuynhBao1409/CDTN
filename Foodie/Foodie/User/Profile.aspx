<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Foodie.User.Profile" %>

<%@ Import Namespace="Foodie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%
        string imageUrl = Session["imageUrl"].ToString();
    %>

    <section class="book_section layout-padding">
        <div class="container">
            <div class="heading_container">
                <h2 class="mt-3">Thông tin Tài Khoản</h2>
            </div>
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="card-title mb-4">
                                <div class="d-flex justify-content-start">
                                    <%--Avatar (lỗi load ảnh)--%>
                                    <div class="image-container">
                                        <img src="<%= Utils.GetImageUrl(imageUrl) %>"
                                            id="imgProfile" style="width: 150px; height: 150px;" class="img-thumbnail"
                                            onerror="this.src='../Images/No_user.jpg'" />
                                        <div class="middle pt-2">
                                            <a href="Registration.aspx?id=<% Response.Write(Session["userId"]); %>" class="btn btn-warning">
                                                <i class="fa fa-pencil"></i>Chỉnh sửa
                                            </a>
                                        </div>
                                    </div>
                                    <!-- Details User -->
                                    <div class="userData ml-3">
                                        <h2 class="d-block" style="font-size: 1.5rem; font-weight: bold">
                                            <a href="javascript:void(0);">
                                                <% Response.Write(Session["name"]); %>
                                            </a>
                                        </h2>

                                        <h6 class="d-block">
                                            <a href="javascript:void(0);">
                                                <asp:Label ID="lblUsername" runat="server" ToolTip="Tên người dùng duy nhất">
                                                    @<% Response.Write(Session["username"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>

                                        <h6 class="d-block">
                                            <a href="javascript:void(0);">
                                                <asp:Label ID="lblEmail" runat="server" ToolTip="Email người dùng">
                                                    <% Response.Write(Session["email"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>

                                        <h6 class="d-block">
                                            <a href="javascript:void(0);">
                                                <asp:Label ID="lblCreatedDate" runat="server" ToolTip="Tài khoản được tạo vào">
                                                     <% Response.Write(Session["createdDate"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>
                                    </div>
                                    <!-- Details User End -->

                                </div>
                            </div>
                            <!-- NavTab điều hướng -->
                            <div class="row">
                                <div class="col-12">
                                    <ul class="nav nav-tabs mb-4" id="myTab" role="tablist">
                                        <!-- info -->
                                        <li class="nav-item">
                                            <a class="nav-link active text-info" id="basicInfo-tab" data-toggle="tab" href="#basicInfo" role="tab"
                                                aria-controls="basicInfo" aria-selected="true">
                                                <i class="fa fa-id-badge mr-2"></i>Thông tin cơ bản
                                            </a>
                                        </li>
                                        <!-- History -->
                                        <li class="nav-item">
                                            <a class="nav-link text-info" id="connectedServices-tab" data-toggle="tab" href="#connectedServices" role="tab"
                                                aria-controls="connectedServices" aria-selected="false">
                                                <i class="fa fa-clock-o mr-2"></i>Lịch sử Mua hàng
                                            </a>
                                        </li>
                                    </ul>

                                    <div class="tab-content ml-1" id="myTabContent">
                                        <%--Info Account--%>
                                        <div class="tab-pane fade show active" id="basicInfo" role="tabpanel" aria-labelledby="basicInfo-tab">
                                            <asp:Repeater ID="rUserProfile" runat="server">
                                                <ItemTemplate>
                                                    <!-- FullName -->
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Họ và Tên</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Name") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <!-- UserName -->
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Tên Đăng Nhập</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Username") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <!-- Mobile -->
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Số Điện Thoại</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Mobile") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <!-- Email -->
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Email</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Email") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <!-- PostCode -->
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Mã Bưu Chính</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("PostCode") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <!-- Address -->
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Địa Chỉ</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Address") %>
                                                        </div>
                                                    </div>
                                                    <hr />


                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <%--Info Account End--%>

                                        <%--Order History--%>
                                        <div class="tab-pane fade" id="connectedServices" role="tabpanel" aria-labelledby="ConnectedServices-tab">

                                            <asp:Repeater ID="rPurchaseHistory" runat="server" OnItemDataBound="rPurchaseHistory_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="container">
                                                        <div class="row pt-1 pb-1" style="background-color: lightgray">
                                                            <div class="col-5">
                                                            <%--STT--%>
                                                                <span class="badge badge-pill badge-dark text-white">
                                                                    <%# Eval("SrNo") %>
                                                                </span>
                                                            <%--Payment Mode--%>
                                                                <!-- Ktra đk nếu ko phải cod thì là card -->
                                                                   Phương thức: <%# Eval("PaymentMode").ToString() ==  "cod" ? "Thanh toán khi nhận hàng" : Eval("PaymentMode").ToString().ToUpper() %> 
                                                            </div>
                                                            <%--CardNo--%>
                                                            <div class="col-5">
                                                                <%# string.IsNullOrEmpty( Eval("CardNo").ToString()) ? "" : "Số Thẻ: " + Eval("CardNo") %>
                                                            </div>
                                                            <%--Invoice--%>
                                                            <div class="col-2" style="text-align: end">
                                                                <a href="Invoice.aspx?id=<%# Eval("PaymentId") %>" class="btn btn-sm">
                                                                    <i class="fa fa-download mr-2"></i>Hóa đơn</a>
                                                            </div>
                                                        </div>
                                                        <asp:HiddenField ID="hdnPaymentId" runat="server" Value='<%# Eval("PaymentId") %>' />

                                                        <%--Infor Orders Table--%>
                                                        <asp:Repeater ID="rOrders" runat="server">
                                                            <HeaderTemplate>
                                                                <div class="table-responsive-sm">
                                                                    <table class="table data-table-export table-bordered table-hover">
                                                                        <thead class=" bg-dark text-white">
                                                                            <tr>
                                                                                <th class="text-center">Tên Sản Phẩm</th>
                                                                                <th class="text-center">Giá tiền</th>
                                                                                <th class="text-center">Slượng</th>
                                                                                <th class="text-center">Tổng Tiền</th>
                                                                                <th class="text-center">Mã Đơn Hàng</th>
                                                                                <th class="text-center">Trạng Thái</th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="text-center align-middle">
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                    </td>
                                                                    <td class="text-center align-middle">
                                                                        <%# Eval("Price") != DBNull.Value ? string.Format("{0:N0}", Convert.ToDouble(Eval("Price"))) + " VND" : "" %>
                                                                    </td>
                                                                    <td class="text-center align-middle">
                                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                    </td>
                                                                    <td class="text-center align-middle">
                                                                        <%# Eval("TotalPrice") != DBNull.Value ? string.Format("{0:N0}", Convert.ToDouble(Eval("TotalPrice"))) + " VND" : "" %>
                                                                    </td>
                                                                    <td class="text-center align-middle" style="word-wrap: break-word;">
                                                                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("OrderNo") %>'></asp:Label>
                                                                    </td>
                                                                    <td class="text-center align-middle">
                                                                        <asp:Label ID="lblStatus" runat="server"  Text='<%# Eval("Status")%>'
                                                                            CssClass='<%# Eval("Status").ToString() == "Đã giao" ? "badge badge-success" : 
                                                                                       Eval("Status").ToString() == "Đã hủy đơn" ? "badge badge-danger" : "badge badge-warning" %>' >
                                                                        </asp:Label>
                                                                    </td>

                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </tbody>
                                                                </table>
                                                                </div>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                        <%--Infor Orders Table End--%>

                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>

                                        </div>
                                        <%--Order History End--%>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>
