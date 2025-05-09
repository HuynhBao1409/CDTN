<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Foodie.User.ForgotPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="book_section layout-padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label runat="server" ID="lblMsg"></asp:Label>
                </div>
                <h2 class="mt-3">Quên Mật Khẩu</h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form_container">
                        <img id="forgotPasswordImg" src="../Images/forgot-password.png" style="width: 450px; height: auto;" alt="Quên Mật Khẩu" class="img-thumbnail" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form_container">
                        <asp:Panel ID="pnlForgotPassword" runat="server" DefaultButton="btnSubmit">
                            <div>
                                <label for="txtEmail" class="form-label">Nhập Email</label>
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="*Vui lòng nhập email" ControlToValidate="txtEmail"
                                    ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ErrorMessage="*Email không hợp lệ" ControlToValidate="txtEmail"
                                    ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$" ForeColor="Red" Display="Dynamic" Font-Size="Small"></asp:RegularExpressionValidator>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Nhập email của bạn"></asp:TextBox>
                            </div>
                            <%--Btn OTP--%>
                            <div class="btn_box mt-3">
                                <asp:Button ID="btnSubmit" runat="server" Text="Gửi Mã OTP" CssClass="btn btn-success rounded-pill pl-4 pr-4 text-white"
                                    OnClick="btnSubmit_Click" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
