<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="NewPassword.aspx.cs" Inherits="Foodie.User.NewPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="book_section layout-padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label runat="server" ID="lblMsg"></asp:Label>
                </div>
                <h2 class="mt-3">Đặt Mật Khẩu Mới</h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form_container">
                        <img id="newPasswordImg" src="../Images/new-password.jpg" alt="Mật Khẩu Mới" class="img-thumbnail" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form_container">
                        <asp:Panel ID="pnlNewPassword" runat="server" DefaultButton="btnSubmit">
                            <%--New Password--%> 
                            <div>
                                <label for="txtNewPassword" class="form-label">Mật Khẩu Mới</label>
                                <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ErrorMessage="*Vui lòng nhập mật khẩu mới" ControlToValidate="txtNewPassword"
                                    ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Nhập mật khẩu mới"></asp:TextBox>
                            </div>
                            <%--Confirm Password--%>
                            <div>
                                <label for="txtConfirmPassword" class="form-label">Xác Nhận Mật Khẩu</label>
                                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ErrorMessage="*Vui lòng xác nhận mật khẩu" ControlToValidate="txtConfirmPassword"
                                    ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvConfirmPassword" runat="server" ErrorMessage="*Mật khẩu không khớp" ControlToValidate="txtConfirmPassword"
                                    ControlToCompare="txtNewPassword" ForeColor="Red" Display="Dynamic" Font-Size="Small"></asp:CompareValidator>
                                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Xác nhận mật khẩu"></asp:TextBox>
                            </div>
                            <%--Update Password--%>
                            <div class="btn_box mt-3">
                                <asp:Button ID="btnSubmit" runat="server" Text="Cập Nhật" CssClass="btn btn-success rounded-pill pl-4 pr-4 text-white"
                                    OnClick="btnSubmit_Click" />
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
