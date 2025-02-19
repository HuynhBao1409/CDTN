<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="Foodie.Admin.Category" %>

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
    <script>
        function ImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imgCategory.ClientID %>').prop('src', e.target.result)
                        .width(200)
                        .height(200);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
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
                                <!-- Form Category-->
                                <div class="card-block">
                                    <div class="row">
                                        <div class="col-sm-6 col-md-4 col-lg-4">
                                            <h4 class="sub-title">Danh Mục</h4>
                                            <div>
                                                <div class="form-group">
                                                    <label>Tên Danh Mục</label>
                                                    <div>
                                                        <!-- TextBox -->
                                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control"
                                                            placeholder="Điền Tên Loại Danh Mục" required></asp:TextBox>
                                                        <asp:HiddenField ID="hdnId" runat="server" Value="0" /> 
                                                    </div>
                                                 </div>
                                                <!-- FileUpload -->
                                                <div class="form-group">
                                                    <label>Ảnh</label>
                                                    <div>
                                                        <asp:FileUpload ID="fuCategoryImage" runat="server" CssClass="form-control"
                                                            onchange="ImagePreview(this);"/> 
                                                    </div>
                                                </div>
                                                <!-- CheckBox -->
                                                <div class="form-check pl-4">
                                                    <asp:CheckBox ID="cbIsActive" runat="server" Text="&nbsp; Duyệt"
                                                        CssClass="form-check-input" /> 
                                                 </div>
                                                <!-- Button -->
                                                 <div class="pb-5">
                                                     <asp:Button ID="btnAddOrUpdate" runat="server" Text="Thêm" CssClass="btn btn-primary"
                                                         OnClick="btnAddOrUpdate_Click"/>
                                                        &nbsp;
                                                     <asp:Button ID="btnClear" runat="server" Text="Làm mới" CssClass="btn btn-primary"
                                                        CausesValidation="false" />
                                                 </div>
                                                 <div>
                                                    <asp:Image ID="imgCategory" runat="server" CssClass="img-thumbnail" />
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
