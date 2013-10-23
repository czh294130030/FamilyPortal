<%@ Page Title="Daily Consume" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    Async="true" CodeBehind="DailyConsume.aspx.cs" Inherits="WebFamilyPortal.DailyConsume.DailyConsume" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        #prev
        {
            cursor: pointer;
            margin-top: 136px;
        }
        #next
        {
            cursor: pointer;
            margin-top: 136px;
        }
        .prev_content
        {
            width: 17px;
            height: 296px;
            float: left;
        }
        .next_content
        {
            width: 17px;
            height: 296px;
            float: left;
        }
        .v_content
        {
            position: relative;
            overflow: hidden;
            width: 412px;
            height: 296px;
            margin: 0px;
            float: left;
        }
        .v_content_list
        {
            position: absolute;
            width: 1236px;
            top: 0px;
            left: 0px;
        }
        .v_content_item
        {
            float: left;
        }
    </style>
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            var item_length = $('div.v_content_item').length;
            var item_index = 1;
            var v_content_width = $('div.v_content').width();
            $('#prev').click(function () {//Show the previous chart. If it is the first chart, then show the last one.
                if (!$('div.v_content_list').is(':animated')) {
                    if (item_index == 1) {
                        $('div.v_content_list').animate({ left: '-=' + v_content_width * (item_length - 1) }, 'slow');
                        item_index = item_length;
                    }
                    else {
                        $('div.v_content_list').animate({ left: '+=' + v_content_width }, 'slow');
                        item_index--;
                    }
                }
            });
            $('#next').click(function () {//Show the next chart. If it is the last chart, then show the first one.
                if (!$('div.v_content_list').is(':animated')) {
                    if (item_index == item_length) {
                        $('div.v_content_list').animate({ left: '+=' + v_content_width * (item_length - 1) }, 'slow');
                        item_index = 1;
                    }
                    else {
                        $('div.v_content_list').animate({ left: '-=' + v_content_width }, 'slow');
                        item_index++;
                    }
                }
            });
        });
        function deleteSuccessfully() {
            alert('Delete successfully.');
            window.location.href = 'DailyConsume.aspx';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Daily Consume
    </h2>
    <div style="margin-top: 12px; margin-bottom: 12px;">
        <span style="margin-right: 10px;">This</span>
        <asp:DropDownList ID="TimePeriodDropDownList" runat="server" AutoPostBack="True"
            OnSelectedIndexChanged="TimePeriodDropDownList_SelectedIndexChanged">
            <asp:ListItem Text="Day" Value="Day"></asp:ListItem>
            <asp:ListItem Text="Week" Value="Week"></asp:ListItem>
            <asp:ListItem Text="Month" Value="Month" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Quarter" Value="Quarter"></asp:ListItem>
            <asp:ListItem Text="Year" Value="Year"></asp:ListItem>
        </asp:DropDownList>
        <a href="AddDailyConsume.aspx" style="margin-left:10px;">Add Daily Consume</a>
    </div>
    <div style="width: 50%; float: left;">
        <asp:GridView ID="DailyConsumeGridView" runat="server" AutoGenerateColumns="False"
            AllowPaging="True" CellPadding="4" ForeColor="#333333" GridLines="None" Style="width: 100%;
            margin-top: 5px;" 
            OnPageIndexChanging="DailyConsumeGridView_PageIndexChanging" 
            onrowcommand="DailyConsumeGridView_RowCommand">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Amount" HeaderText="Amount(￥)" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Date" HeaderText="Date" HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <HeaderTemplate>
                        View
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="ViewLinkButton" runat="server" CommandName="ViewDailyConsume"
                            CommandArgument='<%#Eval("DailyID") %>'>View</asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <HeaderTemplate>
                        Edit
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="EditLinkButton" runat="server" CommandName="EditDailyConsume"
                            CommandArgument='<%#Eval("DailyID") %>'>Edit</asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <HeaderTemplate>
                        Delete
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="DeleteLinkButton" runat="server" CommandName="DeleteDailyConsume"
                            CommandArgument='<%#Eval("DailyID") %>' OnClientClick="return confirm('Confirm to delete?');">Delete</asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    </div>
    <div style="width: 50%; float: left;">
        <div class="prev_content">
            <img id="prev" src="../Images/prev.png" alt="" />
        </div>
        <div class="v_content">
            <div class="v_content_list">
                <div class="v_content_item">
                    <asp:Chart ID="ColumnChart" runat="server" Palette="BrightPastel" BackColor="#D3DFF0"
                        Width="412px" Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom"
                        BorderWidth="2" BorderColor="181, 64, 1">
                        <Titles>
                            <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                Text="Consume Chart" Name="Title1" ForeColor="26, 59, 105">
                            </asp:Title>
                        </Titles>
                        <Legends>
                            <asp:Legend TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent"
                                Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" Enabled="False"
                                Name="Default">
                            </asp:Legend>
                        </Legends>
                        <BorderSkin SkinStyle="Emboss"></BorderSkin>
                        <Series>
                            <asp:Series Name="Series1" BorderColor="180, 26, 59, 105" ChartType="Column" IsValueShownAsLabel="true">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White"
                                BackColor="Transparent" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                    WallWidth="0" IsClustered="False" />
                                <AxisY LineColor="64, 64, 64, 64" LabelAutoFitMaxFontSize="8" TitleFont="Trebuchet MS, 10.25pt">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64" LabelAutoFitMaxFontSize="8" TitleFont="Trebuchet MS, 10.25pt">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsEndLabelVisible="False" Format="MM-dd" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
                <div class="v_content_item">
                    <asp:Chart ID="LineChart" runat="server" Palette="BrightPastel" BackColor="#D3DFF0"
                        ImageType="Png" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)" Width="412px"
                        Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2"
                        BorderColor="181, 64, 1">
                        <Titles>
                            <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                Text="Consume Chart" Name="Title1" ForeColor="26, 59, 105">
                            </asp:Title>
                        </Titles>
                        <Legends>
                            <asp:Legend Enabled="False" IsTextAutoFit="False" Name="Default" BackColor="Transparent"
                                Font="Trebuchet MS, 8.25pt, style=Bold">
                            </asp:Legend>
                        </Legends>
                        <BorderSkin SkinStyle="Emboss"></BorderSkin>
                        <Series>
                            <asp:Series MarkerSize="8" BorderWidth="3" XValueType="Double" Name="Series1" ChartType="Line"
                                MarkerStyle="Circle" ShadowColor="Black" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240"
                                ShadowOffset="2" YValueType="Double" IsValueShownAsLabel="true">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid"
                                BackSecondaryColor="White" BackColor="Transparent" ShadowColor="Transparent"
                                BackGradientStyle="TopBottom">
                                <Area3DStyle Rotation="25" Perspective="9" LightStyle="Realistic" Inclination="40"
                                    IsRightAngleAxes="False" WallWidth="3" IsClustered="False" />
                                <AxisY LineColor="64, 64, 64, 64" TitleFont="Trebuchet MS, 10.25pt">
                                    <LabelStyle Font="Trebuchet MS, 10.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64" TitleFont="Trebuchet MS, 10.25pt">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
                <div class="v_content_item">
                    <asp:Chart ID="PieChart" runat="server" Palette="BrightPastel" BackColor="#D3DFF0"
                        Height="296px" Width="412px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom"
                        BorderWidth="2" BorderColor="26, 59, 105" IsSoftShadows="False" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)">
                        <Titles>
                            <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                Text="Consume Chart" Name="Title1" ForeColor="26, 59, 105">
                            </asp:Title>
                        </Titles>
                        <Legends>
                            <asp:Legend TitleFont="Microsoft Sans Serif, 8pt, style=Bold" BackColor="Transparent"
                                IsEquallySpacedItems="True" Font="Trebuchet MS, 8pt, style=Bold" IsTextAutoFit="False"
                                Name="Default">
                            </asp:Legend>
                        </Legends>
                        <BorderSkin SkinStyle="Emboss"></BorderSkin>
                        <Series>
                            <asp:Series ChartArea="Area1" XValueType="Double" Name="Series1" ChartType="pie"
                                Font="Trebuchet MS, 8.25pt, style=Bold" CustomProperties="DoughnutRadius=25, PieDrawingStyle=Concave, CollectedLabel=Other, MinimumRelativePieSize=20"
                                MarkerStyle="Circle" BorderColor="64, 64, 64, 64" Color="180, 65, 140, 240" YValueType="Double"
                                Label="#PERCENT{P1}">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="Area1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent"
                                BackColor="Transparent" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                <AxisY2>
                                    <MajorGrid Enabled="False" />
                                    <MajorTickMark Enabled="False" />
                                </AxisY2>
                                <AxisX2>
                                    <MajorGrid Enabled="False" />
                                    <MajorTickMark Enabled="False" />
                                </AxisX2>
                                <Area3DStyle PointGapDepth="900" Rotation="162" IsRightAngleAxes="False" WallWidth="25"
                                    IsClustered="False" />
                                <AxisY LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                    <MajorTickMark Enabled="False" />
                                </AxisY>
                                <AxisX LineColor="64, 64, 64, 64">
                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                    <MajorGrid LineColor="64, 64, 64, 64" Enabled="False" />
                                    <MajorTickMark Enabled="False" />
                                </AxisX>
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                </div>
            </div>
        </div>
        <div class="next_content">
            <img id="next" src="../Images/next.png" alt="" />
        </div>
    </div>
</asp:Content>
