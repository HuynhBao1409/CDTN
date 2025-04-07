<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs" Inherits="Foodie.Admin.Contacts" %>
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
                             <!-- Form Contact-->
                             <div class="card-block">
                                 <div class="row">
                                     <!-- List Contact -->
                                     <div class="col-12 mobile-inputs">
                                         <h4 class="sub-title">Danh sách Feedback</h4>
                                         <div class="card-block table-border-style">
                                             <div class="table-responsive">
                                                 <!-- Datatable -->
                                                 <asp:Repeater ID="rContacts" runat="server" OnItemCommand="rContacts_ItemCommand">
                                                     <HeaderTemplate>
                                                         <table class="table data-table-export table-hover nowrap">
                                                             <thead>
                                                                 <tr>
                                                                     <th class="table-plus">Stt</th>
                                                                     <th>Tên người dùng</th>
                                                                     <th>Email</th>
                                                                     <th>Tiêu đề</th>
                                                                     <th>Nội dung</th>
                                                                     <th>Ngày gửi</th>
                                                                     <th class="datatable-nosort">Xóa</th>
                                                                 </tr>
                                                             </thead>
                                                             <tbody>
                                                     </HeaderTemplate>
                                                     <ItemTemplate>
                                                         <tr>
                                                             <td class="table-plus"><%#Eval("SrNo") %> </td>
                                                             <td><%#Eval("Name") %></td>
                                                             <td><%#Eval("Email") %></td>
                                                             <td><%#Eval("Subject") %></td>
                                                             <td><%#Eval("Message") %></td>
                                                             <td><%#Eval("CreatedDate") %> </td>
                                                             <td>
                                                                 <!-- btn -->
                                                                 <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" CommandName="delete"
                                                                     CssClass="badge bg-danger" CommandArgument='<%# Eval("ContactId") %>'
                                                                     OnClientClick="return confirm('Bạn có chắc chắn muốn xóa phản hồi này?');">
                                                                         <i class="ti-trash"></i>
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
