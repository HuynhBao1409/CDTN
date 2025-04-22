<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Foodie.User.Contact" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        .form_container,
        .map_container {
            padding: 0; 
            margin: 0; 
            width: 100 %; 
            box - sizing: border - box; 
        }
        
        .map_container iframe {
            width: 100 % !important; 
            border - radius: 8px; 
        }
        @media(max - width: 767.98px) {
        .form_container,
        .map_container {
                margin - bottom: 20px; 
            }
        .map_container iframe {
                height: 300px; 
            }
        }
    </script>
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

    <!-- book section -->
    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <div class="align-self-end">
                    <asp:Label runat="server" ID="lblMsg"></asp:Label>
                </div>
                <div class="align-self-end">
                    <asp:Label runat="server" ID="Label1"></asp:Label>
                </div>
                <h2>Gửi Feedback của bạn</h2>
            </div>
            <div class="row">
                <div class="col-md-6">
                    <div class="form_container">
                        <!-- Name -->
                        <div class="mb-3">
                            <label for="txtName" class="form-label">Họ và Tên</label>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ErrorMessage="*Không được để trống" ControlToValidate="txtName"
                                ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Nhập Họ và Tên"
                                ToolTip="Họ và Tên">
                            </asp:TextBox>
                        </div>
                        <!-- Email -->
                        <div class="mb-3">
                            <label for="txtEmail" class="form-label">Email</label>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="*Không được để trống"
                                ControlToValidate="txtEmail" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Nhập Email"
                                ToolTip="Email" TextMode="Email">
                            </asp:TextBox>
                        </div>
                        <!-- Subject -->
                        <div class="mb-3">
                            <label for="txtSubject" class="form-label">Tiêu đề</label>
                            <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ErrorMessage="*Không được để trống"
                                ControlToValidate="txtSubject" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="Nhập Tiêu đề"
                                ToolTip="Tiêu đề">
                            </asp:TextBox>
                        </div>
                        <!-- Message -->
                        <div class="mb-3">
                            <label for="txtMessage" class="form-label">Nội dung</label>
                            <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ErrorMessage="*Không được để trống"
                                ControlToValidate="txtMessage" ForeColor="Red" Display="Dynamic" SetFocusOnError="true" Font-Size="Small">
                            </asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" placeholder="Nhập nội dung Feedback"
                                ToolTip="Nội dung" TextMode="MultiLine">
                            </asp:TextBox>
                        </div>
                        <div class="btn_box">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-warning rounded-pill pl-4 pr-4 text-white"
                                OnClick="btnSubmit_Click" />
                        </div>

                    </div>
                </div>
                <div class="col-md-6">
                    <div class="map_container">
                        <iframe
                            src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d974.7515168317389!2d109.19485637445808!3d12.24786815176955!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3170678245d4a09f%3A0xe29f60a4f8071f33!2zVHJ1bmcgVMOibS TUaMawxqFuZyBN4bqhaSBOaGEgVHJhbmc!5e0!3m2!1svi!2s!4v1744954293505!5m2!1svi!2s"
                            width="100%"
                            height="350"
                            style="border: 0; display: block; border-radius: 8px;"
                            loading="lazy"
                            referrerpolicy="no-referrer-when-downgrade"></iframe>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- end book section -->

</asp:Content>
