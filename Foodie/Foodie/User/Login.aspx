<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Foodie.User.Login" %>

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

    <section class="book_section layout-padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label runat="server" ID="lblMsg"></asp:Label>
                </div>
                <h2 class="mt-3">Đăng Nhập</h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form_container">
                        <img id="userLogin" src=" ../Images/login.jpg" alt="" class="img-thumbnail" />
                    </div>
                </div>
                <!-- Form Login -->
                <div class="col-md-6">
                    <div class="form_container">
                        <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnLogin">
                            <div>
                                <label for="txtUsername" class="form-label">Tên Đăng Nhập</label>
                                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ErrorMessage="*Không được bỏ trống" ControlToValidate="txtUsername"
                                    ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Nhập tên đăng nhập"></asp:TextBox>
                            </div>
                            <!-- Password -->
                            <div>
                                <label for="txtPassword" class="form-label">Mật Khẩu</label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*Không được bỏ trống" ControlToValidate="txtPassword"
                                    ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Nhập mật khẩu"></asp:TextBox>
                            </div>
                            <!-- Btn Login and Forgot Password -->
                            <div class="btn_box d-flex justify-content-between align-items-center">
                                <div>
                                    <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" CssClass="btn btn-success rounded-pill pl-4 pr-4 text-white"
                                        OnClick="btnLogin_Click" />
                                    <span class="pl-3 text-info">Chưa có tài khoản?? <a href="Registration.aspx" class="badge badge-info">Đăng ký..</a></span>
                                </div>
                                <div>
                                    <span class="text-info"><a href="ForgotPassword.aspx" class="badge badge-info">Quên mật khẩu?</a></span>
                                </div>                                
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>
