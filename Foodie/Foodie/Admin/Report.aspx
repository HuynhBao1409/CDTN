<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Foodie.Admin.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="pcoded-inner-content pt-0">

        <div class="main-body">
            <div class="page-wrapper">
                <div class="page-body">
                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-header">
                                    <div class="container">
                                        <div class="form-row">
                                            <%--From Date--%>
                                            <div class="form-group col-md-4">
                                                <label>Từ Ngày</label>
                                                <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ForeColor="Red" ErrorMessage="*"
                                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtFromDate"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <%--To Date--%>
                                            <div class="form-group col-md-4">
                                                <label>Đến Ngày</label>
                                                <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ForeColor="Red" ErrorMessage="*"
                                                    SetFocusOnError="true" Display="Dynamic" ControlToValidate="txtToDate"></asp:RequiredFieldValidator>
                                                <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <%--Btn Search--%>
                                            <div class="form-group col-md-4">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary mt-md-4"
                                                    OnClick="btnSearch_Click" />
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <!-- Form Contact-->
                                <div class="card-block">
                                    <div class="row">
                                        <!-- List Contact -->
                                        <div class="col-12 mobile-inputs">
                                            <h4 class="sub-title">Báo Cáo Doanh Thu</h4>
                                            <div class="card-block table-border-style">
                                                <div class="table-responsive">
                                                    <!-- Datatable -->
                                                    <asp:Repeater ID="rReport" runat="server">
                                                        <HeaderTemplate>
                                                            <table class="table data-table-export table-hover nowrap">
                                                                <thead>
                                                                    <tr>
                                                                        <th class="table-plus">Stt</th>
                                                                        <th>Tên người dùng</th>
                                                                        <th>Email</th>
                                                                        <th>Đơn đặt hàng</th>
                                                                        <th>Thời gian</th>
                                                                        <th>Tổng hóa đơn</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td class="table-plus"><%#Eval("SrNo") %> </td>
                                                                <td><%#Eval("Name") %></td>
                                                                <td><%#Eval("Email") %></td>
                                                                <td><%#Eval("TotalOrders") %></td>
                                                                <td><%#Eval("OrderDateFormatted") %></td>
                                                                <td class="text-right">
                                                                    <%# string.Format("{0:N0} VND", Eval("TotalPrice")) %>
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
                                    <%--Diagram--%>
                                        <div class="row pl-4">
                                            <asp:Label ID="lblTotal" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                                        </div>
                                        <div class="row pl-4 mt-4">
                                            <div class="col-12">
                                                <canvas id="revenueChart" style="max-height: 400px;"></canvas>
                                            </div>
                                        </div>
                                    <%--Diagram end--%>                                    
                                </div>
                                <!-- End Form -->
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

<script type="text/javascript">
    window.onload = function () {
        // Lấy dữ liệu thô từ ViewState
        var rawData = <%= ViewState["ChartRawData"] %> || [];

        // Kiểm tra nếu không có dữ liệu
        if (rawData.length === 0) {
            console.log("Không có dữ liệu để vẽ biểu đồ.");
            return;
        }

        // Lấy danh sách ngày/tháng/năm duy nhất
        var dates = [...new Set(rawData.map(row => row.OrderDateFormatted))];

        // Sắp xếp ngày theo thứ tự thời gian
        dates.sort(function (a, b) {
            // Chuyển định dạng "DD/MM/YYYY" thành đối tượng Date để so sánh
            var dateA = new Date(a.split('/').reverse().join('-')); // Chuyển "DD/MM/YYYY" thành "YYYY-MM-DD"
            var dateB = new Date(b.split('/').reverse().join('-'));
            return dateA - dateB; // Sắp xếp tăng dần
        });

        // Tính tổng doanh thu cho từng ngày
        var totalRevenueByDate = dates.map(function (date) {
            var total = rawData
                .filter(row => row.OrderDateFormatted === date)
                .reduce((sum, row) => sum + parseFloat(row.TotalPrice), 0);
            return total;
        });

        // Vẽ biểu đồ đường
        var ctx = document.getElementById('revenueChart').getContext('2d');
        new Chart(ctx, {
            type: 'line',
            data: {
                labels: dates,
                datasets: [{
                    label: 'Tổng Doanh Thu (VND)',
                    data: totalRevenueByDate,
                    borderColor: 'rgba(0, 123, 255, 1)',
                    backgroundColor: 'rgba(0, 123, 255, 0.1)',
                    fill: true,
                    tension: 0.3
                }]
            },
            options: {
                responsive: true,
                scales: {
                    y: {
                        beginAtZero: true,
                        title: {
                            display: true,
                            text: 'Doanh Thu (VND)'
                        }
                    },
                    x: {
                        title: {
                            display: true,
                            text: 'Ngày/Tháng/Năm'
                        }
                    }
                }
            }
        });
    };
</script>
</asp:Content>
