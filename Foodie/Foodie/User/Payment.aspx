<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="Foodie.User.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .rounded {
            border-radius: 1rem
        }

        .nav-pills .nav-link {
            color: #555
        }

            .nav-pills .nav-link.active {
                color: white
            }

        .bold {
            font-weight: bold
        }

        .card {
            padding: 40px 50px;
            border-radius: 20px;
            border: none;
            box-shadow: 1px 5px 10px 1px rgba(0, 0, 0, 0.2)
        }
    </style>

    <script>
        /*ẩn thông báo lỗi */
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblMsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
        /* tooltip*/
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    </script>

    <%--Chức năng ngăn nút quay lại--%>
    <script type="text/javascript">
        function DisableBackButton() {
            window.history.forward()
        }
        DisableBackButton();
        window.onload = DisableBackButton;
        window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
        window.onunload = function () { void (0) }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="book_section" style="background-image: url('../Images/payment-bg.png'); width: 100%; height: 100%; background-repeat: no-repeat; background-size: auto; background-attachment: fixed; background-position: left;">

        <div class="container py-5">
            <div class="align-self-end">
                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
            </div>
            <!-- Mục đích demo -->
            <div class="row mb-4">
                <div class="col-lg-8 mx-auto text-center">
                    <h1 class="display-6">Thanh Toán Đơn Hàng</h1>
                </div>
            </div>
            <!-- End -->
            <div class="row pb-5">
                <div class="col-lg-6 mx-auto">
                    <div class="card ">
                        <div class="card-header">
                            <div class="bg-white shadow-sm pt-4 pl-2 pr-2 pb-2">
                                <!-- Loại thanh toán -->
                                <ul role="tablist" class="nav bg-light nav-pills rounded nav-fill mb-3">
                                    <li class="nav-item"><a data-toggle="pill" href="#credit-card" class="nav-link active "><i class="fa fa-credit-card mr-2"></i>Thẻ Tín Dụng </a></li>
                                    <li class="nav-item"><a data-toggle="pill" href="#paypal" class="nav-link "><i class="fa fa-money mr-2"></i>Thanh Toán Khi Nhận Hàng </a></li>
                                </ul>
                                <!-- End -->
                            </div>
                            <!-- Nội dung form thẻ tín dụng -->
                            <div class="tab-content">
                                <!-- thông tin thẻ tín dụng -->
                                <div id="credit-card" class="tab-pane fade show active pt-3">
                                    <div role="form">
                                        <%--Name Card--%>
                                        <div class="form-group">
                                            <label for="txtName">
                                                <h6>Chủ Thẻ</h6>
                                            </label>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="Tên không được để trống"
                                                ControlToValidate="txtName" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"
                                                ValidationGroup="card">*</asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                ErrorMessage="Tên không chứa ký tự đặc biệt" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"
                                                ValidationExpression="^[a-zA-Z\s]+$" ControlToValidate="txtName" ValidationGroup="card">*
                                            </asp:RegularExpressionValidator>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Tên Chủ Thẻ"></asp:TextBox>
                                        </div>
                                        <%--Card Number--%>
                                        <div class="form-group">
                                            <label for="txtCardNo">
                                                <h6>Mã Thẻ</h6>
                                            </label>
                                            <asp:RequiredFieldValidator ID="rfvCardNo" runat="server" ErrorMessage="Mã thẻ không được để trống"
                                                ControlToValidate="txtCardNo" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"
                                                ValidationGroup="card">*</asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                ErrorMessage="Số thẻ phải có 16 chữ số" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"
                                                ValidationExpression="[0-9]{16}" ControlToValidate="txtCardNo" ValidationGroup="card">*
                                            </asp:RegularExpressionValidator>
                                            <div class="input-group">
                                                <asp:TextBox ID="txtCardNo" runat="server" CssClass="form-control" placeholder="Số thẻ hợp lệ"
                                                    TextMode="Number"></asp:TextBox>
                                                <div class="input-group-append">
                                                    <span class="input-group-text text-muted">
                                                        <i class="fa fa-cc-visa mx-1"></i>
                                                        <i class="fa fa-cc-mastercard mx-1"></i>
                                                        <i class="fa fa-cc-paypal mx-1"></i>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <%--Card Number End--%>
                                        <div class="row">
                                            <div class="col-sm-8">
                                                <%--Ngày Hết Hạn--%>
                                                <div class="form-group">
                                                    <label>
                                                        <span class="hidden-xs">
                                                            <h6>Ngày Hết Hạn</h6>
                                                        </span>
                                                    </label>
                                                    <asp:RequiredFieldValidator ID="rfvExpMonth" runat="server" ErrorMessage="Tháng hết hạn không được để trống"
                                                        ControlToValidate="txtExpMonth" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"
                                                        ValidationGroup="card">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Display="Dynamic"
                                                        ErrorMessage="Tháng hết hạn phải là 2 chữ số" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationExpression="[0-9]{2}" ControlToValidate="txtExpMonth" ValidationGroup="card">*
                                                    </asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="rfvExpYear" runat="server" ErrorMessage="Năm hết hạn không được để trống"
                                                        ControlToValidate="txtExpYear" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"
                                                        ValidationGroup="card">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Display="Dynamic"
                                                        ErrorMessage="Năm hết hạn phải là 4 chữ số" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationExpression="[0-9]{4}" ControlToValidate="txtExpYear" ValidationGroup="card">*
                                                    </asp:RegularExpressionValidator>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="txtExpMonth" runat="server" CssClass="form-control" placeholder="TT"
                                                            TextMode="Number"></asp:TextBox>
                                                        <asp:TextBox ID="txtExpYear" runat="server" CssClass="form-control" placeholder="NNNN"
                                                            TextMode="Number"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <%--End--%>
                                            </div>
                                            <%--CVV--%>
                                            <div class="col-sm-4">
                                                <div class="form-group mb-4">
                                                    <label data-toggle="tooltip" title="Mã 3 chữ số ở mặt sau thẻ">
                                                        <h6>CVV <i class="fa fa-question-circle d-inline"></i></h6>
                                                    </label>
                                                    <asp:RequiredFieldValidator ID="rfvCvv" runat="server" ErrorMessage="Số CVV không được để trống"
                                                        ControlToValidate="txtCvv" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"
                                                        ValidationGroup="card">*</asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" Display="Dynamic"
                                                        ErrorMessage="Số CVV phải là 3 chữ số" ForeColor="Red" SetFocusOnError="true"
                                                        ValidationExpression="[0-9]{3}" ControlToValidate="txtCvv" ValidationGroup="card">*
                                                    </asp:RegularExpressionValidator>
                                                    <asp:TextBox ID="txtCvv" runat="server" CssClass="form-control" placeholder="Số CVV"
                                                        TextMode="Number"></asp:TextBox>
                                                </div>
                                            </div>
                                            <%--CVV End--%>
                                        </div>
                                        <%--Address--%>
                                        <div class="form-group">
                                            <label for="txtAddress">
                                                <h6>Địa Chỉ Giao Hàng</h6>
                                            </label>
                                            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ErrorMessage="Địa chỉ không được để trống" ForeColor="Red"
                                                ControlToValidate="txtAddress" Display="Dynamic" SetFocusOnError="true" ValidationGroup="card">*
                                            </asp:RequiredFieldValidator>
                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Địa Chỉ Giao Hàng"
                                                TextMode="MultiLine" ValidationGroup="card"></asp:TextBox>
                                        </div>
                                        <%--Submit--%>
                                        <div class="card-footer">
                                            <asp:LinkButton ID="lbCardSubmit" runat="server" CssClass="subscribe btn btn-info btn-block shadow-sm"
                                                ValidationGroup="card" OnClick="lbCardSubmit_Click">Xác Nhận Thanh Toán</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                                <!-- Card End -->
                                <!-- Thông tin thanh toán khi nhận hàng -->
                                <div id="paypal" class="tab-pane fade pt-3">
                                    <%--Address--%>
                                    <div class="form-group">
                                        <label for="txtCODAddress">
                                            <h6>Địa Chỉ Giao Hàng</h6>
                                        </label>
                                        <asp:TextBox ID="txtCODAddress" runat="server" CssClass="form-control" placeholder="Địa Chỉ Giao Hàng"
                                            TextMode="MultiLine"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvCODAddress" runat="server" ErrorMessage="Địa chỉ không được để trống" ForeColor="Red"
                                            ControlToValidate="txtCODAddress" Display="Dynamic" SetFocusOnError="true" ValidationGroup="cod"
                                            Font-Names="Segoe Script"></asp:RequiredFieldValidator>
                                    </div>
                                    <p>
                                        <%--Submit--%>
                                        <asp:LinkButton ID="lbCodSubmit" runat="server" CssClass="btn btn-info" ValidationGroup="cod" OnClick="lbCodSubmit_Click">
                                            <i class="fa fa-cart-arrow-down mr-2"></i>Xác Nhận Đơn Hàng</asp:LinkButton>
                                    </p>
                                    <p class="text-muted">
                                        Lưu ý: Khi nhận hàng, bạn sẽ thanh toán toàn bộ số tiền. 
                                    Sau khi hoàn tất quá trình thanh toán, bạn có thể kiểm tra trạng thái đơn hàng được cập nhật.
                                    </p>
                                </div>
                                <!-- End -->
                              

                            </div>
                            <%--Total Bill--%>
                            <div class="card-footer">
                                <b class="badge badge-success badge-pill shadow-sm">Tổng Đơn Hàng: <% Response.Write(Session["grandTotalPrice"]); %> VND </b>
                                <div class="pt-1">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" ValidationGroup="card"
                                        HeaderText="Vui lòng sửa các lỗi sau" Font-Names="Segoe Script" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </section>
</asp:Content>
