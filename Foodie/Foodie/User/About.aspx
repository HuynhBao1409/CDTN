<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Foodie.User.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- about section -->

  <section class="about_section layout_padding">
    <div class="container  ">

      <div class="row">
        <div class="col-md-6 ">
          <div class="img-box">
            <img src="../Public/images/about-img.png" alt="">
          </div>
        </div>
        <div class="col-md-6">
          <div class="detail-box">
            <div class="heading_container">
              <h2>
                Chúng tôi là FastBite
              </h2>
            </div>
            <p>
              FastBite mang đến cho bạn trải nghiệm ẩm thực nhanh chóng và tiện lợi, từ những chiếc burger nóng hổi đến khoai tây chiên giòn tan. 
                Mỗi món ăn được chế biến kỹ lưỡng, phục vụ liền tay, đảm bảo vừa ngon miệng vừa tiết kiệm thời gian. 
                Dù là bữa trưa vội vã hay buổi tối lười nấu ăn, FastBite luôn sẵn sàng đồng hành cùng bạn.
            </p>
            <a href="">
              Xem thêm
            </a>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- end about section -->


</asp:Content>
