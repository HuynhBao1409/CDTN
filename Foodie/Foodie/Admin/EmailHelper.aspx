<%@ Page Title="Gửi Email" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="EmailHelper.aspx.cs" Inherits="Foodie.Admin.EmailHelper" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <script>
        /* Ẩn thông báo sau 5 giây */
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblMsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        };
    </script>
    <style>
        .email-compose-container {
            border: 1px solid #dfe3e7;
            border-radius: 4px;
            background-color: #fff;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
            padding: 10px;
        }

            .email-compose-container .form-group {
                margin-bottom: 10px;
            }

                .email-compose-container .form-group label {
                    font-weight: normal;
                    margin-bottom: 0;
                    padding: 5px 0;
                }

            .email-compose-container .form-control {
                border: 1px solid #dfe3e7;
                border-radius: 3px;
                padding: 5px;
            }

            .email-compose-container textarea.form-control {
                min-height: 150px;
                resize: vertical;
            }

            .email-compose-container .btn-send {
                background-color: #1a73e8;
                border-color: #1a73e8;
                color: #fff;
                padding: 8px 20px;
                border-radius: 4px;
            }

        .table-container {
            margin-bottom: 20px;
        }

        .table-responsive {
            overflow-x: auto;
        }

        .table th, .table td {
            vertical-align: middle;
            padding: 8px;
        }

        .btn-select {
            background-color: #f0ad4e;
            border-color: #f0ad4e;
            color: #fff;
            padding: 5px 10px;
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pcoded-inner-content pt-0">
        <div class="align-align-self-end">
            <!-- Label thông báo -->
            <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        </div>

        <div class="main-body">
            <div class="page-wrapper">
                <div class="page-body">
                    <div class="row">
                        <!-- Bảng danh sách liên hệ -->
                        <div class="col-sm-12 col-md-6 col-lg-6">
                            <div class="table-container">
                                <h4 class="sub-title">Danh sách liên hệ</h4>
                                <div class="table-responsive">
                                    <asp:Repeater ID="rContacts" runat="server" OnItemCommand="rContacts_ItemCommand">
                                        <HeaderTemplate>
                                            <table class="table table-bordered table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>#</th>
                                                        <th>Tên khách hàng</th>
                                                        <th>Email</th>
                                                        <th>Ngày gửi</th>
                                                        <th>Tùy chọn</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Container.ItemIndex + 1 %></td>
                                                <td><%# Eval("Name") %></td>
                                                <td><%# Eval("Email") %></td>
                                                <td><%# Eval("CreatedDate", "{0:dd/MM/yyyy HH:mm}") %></td>
                                                <td>
                                                    <asp:LinkButton ID="lnkSelect" runat="server" CssClass="btn btn-select btn-sm" CommandName="select" CommandArgument='<%# Eval("Email") %>'>
                                                        <i class="fas fa-user-check"></i> Chọn
                                                    </asp:LinkButton>                                                    
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>

                        <!-- Form gửi email -->
                        <div class="col-sm-12 col-md-6 col-lg-6">
                            <div class="card">
                                <div class="card-header">
                                    <h4 class="sub-title">Gửi Email Phản Hồi</h4>
                                </div>
                                <div class="card-block">
                                    <div class="email-compose-container">
                                        <div class="form-group">
                                            <label>Đến</label>
                                            <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" placeholder="Email người nhận" required></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revTo" runat="server" ControlToValidate="txtTo" ErrorMessage="Email không hợp lệ!" ValidationExpression="^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$" ForeColor="Red" Display="Dynamic" />
                                        </div>

                                        <div class="form-group">
                                            <label>Tiêu đề</label>
                                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="Nhập tiêu đề email" required></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <label>Nội dung</label>
                                            <asp:TextBox ID="txtBody" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="Nhập nội dung email" required></asp:TextBox>
                                        </div>
                                        <%--Btn--%>
                                        <div class="form-group">
                                            <asp:Button ID="btnSend" runat="server" Text="Gửi Email" CssClass="btn btn-send" OnClick="btnSend_Click" />
                                            <asp:Button ID="btnClear" runat="server" Text="Làm mới" CssClass="btn btn-primary" CausesValidation="false" OnClick="btnClear_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
