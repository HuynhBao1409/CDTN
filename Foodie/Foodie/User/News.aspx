<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="Foodie.User.News" %>

<%@ Import Namespace="Foodie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style>
        .food_section {
            min-height: calc(100vh - 200px); 
            display: flex;
            flex-direction: column;
            padding: 90px 0; 
            padding-bottom: 40px; 
        }

        .filters-content {
            flex-grow: 1; 
            display: flex;
            flex-direction: column;
        }

        .food_section .filters_menu {
            padding: 0;
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            list-style-type: none;
            margin: 45px 0 20px 0;
        }

        .food_section .filters_menu li {
            padding: 7px 25px;
            cursor: pointer;
            border-radius: 25px;
        }

        .food_section .filters_menu li.active {
            background-color: #222831;
            color: #ffffff;
        }

        .row.grid {
            display: flex;
            flex-direction: column; 
            align-items: center; 
            width: 100%;
            margin: 0;
            max-width: 100%; 
        }

        .news-card {
            width: 100%;
            max-width: 1100px; 
            padding: 0 10px;
            margin-bottom: 25px;
            box-sizing: border-box;
        }

        .news-card .box {
            background-color: #4c4c4c;
            border-radius: 15px;
            color: #ffffff;
            overflow: hidden;
            transition: all 0.2s;
            display: flex; 
            height: 160px; 
            align-items: stretch; 
            width: 100%; 
        }

        .news-card .box .img-box {
            background: #f1f2f3;
            width: 40%; 
            display: flex;
            justify-content: center;
            align-items: center;
            border-radius: 15px 0 0 15px; 
            overflow: hidden; 
            position: relative; 
            margin: 0; 
            padding: 0; 
        }

        .news-card .box .img-box img {
            width: 100%;
            height: 100%;
            object-fit: cover; 
            position: absolute; 
            top: 0;
            left: 0;
            bottom: 0; 
            margin: 0; 
            display: block;
        }

        .news-card .box .detail-box {
            width: 60%; 
            padding: 15px;
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            height: 100%; 
            box-sizing: border-box; 
        }

        .news-card .box .detail-box h5 {
            font-weight: 600;
            font-size: 1.2rem;
            margin: 0;
        }

        .news-card .box .detail-box p {
            font-size: 0.9rem;
            margin: 5px 0;
            flex-grow: 1;
            overflow: hidden;
            text-overflow: ellipsis;
            display: -webkit-box;
            -webkit-line-clamp: 3; 
            -webkit-box-orient: vertical;
        }

        .news-card .box .detail-box .options {
            display: flex;
            justify-content: flex-start;
        }

        .news-card .box .detail-box .options h6 {
            margin: 0;
            color: #ddd; 
            font-size: 0.8rem;
        }

        .news-card .box:hover .img-box img {
            transform: scale(1.05); 
            transition: transform 0.3s ease; 
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- news section -->
    <section class="food_section layout_padding">
        <div class="container">
            <div class="heading_container heading_center">
                <div class="align-self-end">
                    <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                </div>
                <h2>Tin Tức</h2>
            </div>
            <!-- Filter by Time -->
            <ul class="filters_menu">
                <li class="active" data-filter="*" data-type="all">Tất cả bài viết</li>
                <li data-filter=".new" data-type="new">Tin mới</li>
            </ul>

            <div class="filters-content">
                <div class="row grid">
                    <!-- News Cards -->
                    <asp:Repeater ID="rNews" runat="server">
                        <ItemTemplate>
                            <div class="col-sm-6 col-lg-4 news-card all <%# Convert.ToBoolean(Eval("IsNew")) ? "new" : "" %>">
                                <div class="box">
                                    <div class="img-box">
                                        <img src="<%# Utils.GetImageUrl(Eval("ImageUrl")) %>" alt="News Image">
                                    </div>
                                    <div class="detail-box">
                                        <!-- Title -->
                                        <h5><%# Eval("Title") %></h5>
                                        <!-- Content (shortened) -->
                                        <p><%# Eval("Content") %></p>
                                        <!-- Time -->
                                        <div class="options">
                                            <h6><%# Eval("TimeAgo") %></h6>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>                
            </div>
        </div>
    </section>
    <!-- end news section -->
</asp:Content>