<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Foodie.User.Registration" %>
<%@ Import Namespace="Foodie" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        /* Ẩn thông báo 5s */
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                var lblMsg = document.getElementById("<%=lblMsg.ClientID %>");
                if (lblMsg) { // Kiểm tra xem phần tử có tồn tại không
                    lblMsg.style.display = "none";
                }
            }, seconds * 1000);
        };
    </script>
    <script>
        function ImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imgUser.ClientID %>').prop('src', e.target.result)
                        .width(200)
                        .height(200);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                </div>
                <asp:Label ID="lblHeaderMsg" runat="server" Text="<h2>Đăng ký tài khoản</h2>"></asp:Label>
            </div>
            <!-- Form Register -->
            <div class="row">

                <div class="col-md-6">
                    <div class="form_container">
                        <!-- FullName -->
                        <div class="mb-3">
                            <label for="txtName" class="form-label">Họ và Tên</label>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="*Không được để trống" ControlToValidate="txtName"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revName" runat="server" ErrorMessage="*Tên không chứa số và ký tự đặc biệt"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[a-zA-ZÀ-ỹ\s]+$"
                                ControlToValidate="txtName" Font-Size="Small">
                            </asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Nhập Họ và Tên"
                                ToolTip="Họ và Tên">
                            </asp:TextBox>
                        </div>
                        <!-- UserName -->
                        <div class="mb-3">
                            <label for="txtUsername" class="form-label">Tên Đăng Nhập</label>
                            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ErrorMessage="*Không được để trống"
                                ControlToValidate="txtUsername" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"  Font-Size="Small">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Nhập Tên Đăng Nhập"
                                ToolTip="Tên Đăng Nhập">
                            </asp:TextBox>
                        </div>
                        <!-- Email -->
                        <div class="mb-3">
                            <label for="txtEmail" class="form-label">Email</label>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="*Không được để trống"
                                ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"  Font-Size="Small">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Nhập Email"
                                ToolTip="Email" TextMode="Email">
                            </asp:TextBox>
                        </div>
                        <!-- Phone No. -->
                        <div class="mb-3">
                            <label for="txtMobile" class="form-label">Số Điện Thoại</label>
                            <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ErrorMessage="*Không được để trống" ControlToValidate="txtMobile"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true"  Font-Size="Small">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revMobile" runat="server" ErrorMessage="*Số Điện Thoại phải có 10 hoặc 11 chữ số"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[0-9]{10,11}$"
                                ControlToValidate="txtMobile" Font-Size="Small">
                            </asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="Nhập Số Điện Thoại"
                                ToolTip="Số Điện Thoại" TextMode="Number">
                            </asp:TextBox>
                        </div>

                    </div>
                </div>

                <div class="col-md-6">
                    <div class="form_container">

                        <!-- Address -->
                        <div class="mb-3">
                            <label for="txtAddress" class="form-label">Địa Chỉ</label>
                            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ErrorMessage="*Không được để trống"
                                ControlToValidate="txtAddress"  Font-Size="Small"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Nhập Địa chỉ"
                                ToolTip="Địa chỉ" TextMode="MultiLine">
                            </asp:TextBox>
                        </div>
                        <!-- PostCode -->
                        <div class="mb-3">
                            <label for="txtPostCode" class="form-label">Mã Bưu Chính</label>
                            <asp:RequiredFieldValidator ID="rfvPostCode" runat="server" ErrorMessage="*Không được để trống"
                                ControlToValidate="txtPostCode" ForeColor="Red" Display="Dynamic" SetFocusOnError="true"  Font-Size="Small">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revPostCode" runat="server" ErrorMessage="*Mã Bưu Chính phải có đúng 6 chữ số"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" ValidationExpression="^[0-9]{6}$"
                                ControlToValidate="txtPostCode" Font-Size="Small">
                            </asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="form-control" placeholder="Nhập Mã Post/Zip"
                                ToolTip="Mã Bưu Chính" TextMode="Number">
                            </asp:TextBox>
                        </div>
                        <!-- Avatar -->
                        <div class="mb-3">
                            <label for="fuUserImage" class="form-label">Ảnh Đại Diện</label>
                            <asp:FileUpload ID="fuUserImage" runat="server" CssClass="form-control" ToolTip="Ảnh Đại Diện"
                                onchange="ImagePreview(this);" />
                        </div>
                        <!-- Password -->
                        <div class="mb-3">
                            <label for="txtPassword" class="form-label">Mật khẩu</label>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="*Không được để trống"
                                ControlToValidate="txtPassword" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small">
                            </asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Nhập Mật Khẩu"
                                    ToolTip="Mật Khẩu" TextMode="Password">
                                </asp:TextBox>
                        </div>

                    </div>
                </div>
                <!-- btn Register-->
                <div class="row pl-4">
                    <div class="btn_box">
                        <asp:Button ID="btnRegister" runat="server" Text="Đăng ký" CssClass="btn btn-success rounded-pill pl-4 pr-4 text-white"
                            OnClick="btnRegister_Click" />

                        <asp:Label ID="lblAlreadyUser" runat="server" CssClass="pl-3 text-black-100"
                            Text="Đã đăng ký rồi? <a href='Login.aspx' class='badge badge-info'> Đăng nhập..</a>"></asp:Label>
                    </div>
                </div>

                <div class="row p-5">
                    <div style="align-items: center">
                        <asp:Image ID="imgUser" runat="server" CssClass="img-thumbnail" />
                    </div>
                </div>

            </div>
            <!-- Form Register End-->
        </div>
    </section>


</asp:Content>
