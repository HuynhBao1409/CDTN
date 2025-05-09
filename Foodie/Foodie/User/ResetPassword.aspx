<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="Foodie.User.ResetPassword" %>
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
                <h2 class="mt-3">Nhập Mã OTP</h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form_container">
                        <img id="resetPasswordImg" src="../Images/reset-password.jpg" style="width: 400px; height: auto;" alt="Đặt Lại Mật Khẩu" class="img-thumbnail" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form_container">
                        <asp:Panel ID="pnlResetPassword" runat="server" DefaultButton="btnSubmit">
                            <%--OTP--%>
                            <div>
                                <label for="txtOTP" class="form-label">Mã OTP</label>
                                <asp:RequiredFieldValidator ID="rfvOTP" runat="server" ErrorMessage="*Vui lòng nhập mã OTP" ControlToValidate="txtOTP"
                                    ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control" placeholder="Nhập mã OTP"></asp:TextBox>
                            </div>
                            <%--Btn Submit--%>
                            <div class="btn_box mt-3">
                                <asp:Button ID="btnSubmit" runat="server" Text="Xác Nhận" CssClass="btn btn-success rounded-pill pl-4 pr-4 text-white"
                                    OnClick="btnSubmit_Click" />
                                <%--Btn Resend--%>
                                <asp:Button ID="btnResend" runat="server" Text="Gửi Lại OTP" CssClass="btn btn-info rounded-pill pl-4 pr-4 text-white ml-2"
                                    OnClick="btnResend_Click" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
