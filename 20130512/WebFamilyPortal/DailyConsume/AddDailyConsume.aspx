<%@ Page Title="Add Daily Consume" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Async="true"
    CodeBehind="AddDailyConsume.aspx.cs" Inherits="WebFamilyPortal.DailyConsume.AddDailyConsume" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#<%=AmountTextBox.ClientID%>').blur(function () {//the format of amount.
                var patrn = /^(0|[1-9]\d{0,17})(\.\d{1,2})?$/; //decimal(18,2)
                if ($(this).val() != '') {
                    if (!patrn.exec($(this).val())) {
                        alert('Amount you input is not valid.');
                        $(this).val('');
                        $(this).focus();
                    }
                }
            });
            $('#<%=AmountTextBox.ClientID%>').keydown(function (e) {//forbid entering the 'enter' key.
                var key = window.event ? e.keyCode : e.which;
                if (key.toString() == "13") { return false; }
            });
            //count the length of description.
            //兼容：IE6、IE7、IE8、Firefox、Opera、Chrome、Safari
            if ($.browser.msie) {//IE浏览器  
                $('#<%=DecriptionTextBox.ClientID %>').get(0).onpropertychange = countTheLengthOfDesc;
            } else {//其他浏览器  
                $('#<%=DecriptionTextBox.ClientID %>').get(0).addEventListener("input", countTheLengthOfDesc, false);
            }
            $('#dateImage').click(function () {//click image to trigger textbox to show calendar.
                document.getElementById('<%=DateTextBox.ClientID %>').click();
            });
            $('#<%=SaveConsumeLinkButton.ClientID %>').click(function () {//validate consume information before saving.
                var $amount = $('#<%=AmountTextBox.ClientID %>');
                var $description = $('#<%=DecriptionTextBox.ClientID %>');
                if ($amount.val() == '') {//amount is required.
                    alert('Please input amount.');
                    $amount.focus();
                    return false;
                }
                if ($description.val() == '') {//description is required.
                    alert('Please input description.');
                    $description.focus();
                    return false;
                }
                //To make sure the consume type has not been selected before.
                var flag = false;
                var selectingType = $('#<%=TypeDropDownList.ClientID %> option:selected').text();
                var editType = $('#<%=TypeHiddenField.ClientID %>').val();
                $('#<%=ConsumePanel.ClientID %> fieldset legend').each(function () {
                    if (editType == '') {//Add consume.
                        if (selectingType == $.trim($(this).text())) { flag = true; }
                    }
                    else { //Edit consume.
                        if (selectingType == $.trim($(this).text()) && selectingType != editType) { flag = true; }
                    }
                });
                if (flag) {
                    alert('Consume type "' + selectingType + '" has been selected. Please select another.');
                    return false;
                }
            });
            $('#<%=SaveDailyConsumeButton.ClientID %>').click(function () { //validate daily consume information before saving.
                var date = $('#<%=DateTextBox.ClientID %>').val();
                if (date == '') {
                    alert('Please input date.');
                    return false;
                }
                var consumeLength = $('#<%=ConsumePanel.ClientID %> fieldset').length;
                if (consumeLength == 0) {
                    alert('Please input consume.');
                    return false;
                }
            });
            $('#cancelA').click(function () {
                if (!confirm('Unsaved changes will be lost.')) {
                    return false;
                }
                else {
                    window.location.href = 'DailyConsume.aspx';
                }
            });
        });
        function countTheLengthOfDesc() {
            var maxLength = 500;
            var $description = $(this).val();
            var descriptionLength = $description.length;
            if (descriptionLength > maxLength) {
                $(this).val($description.substring(0, maxLength));
                descriptionLength = maxLength;
            }
            $('#stillSpan').text(maxLength - descriptionLength);
        }
        function saveSuccessfully() {
            alert('Save successfully.');
            window.location.href = 'DailyConsume.aspx';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        Add Daily Consume
    </h2>
    <div>
        <table cellpadding="2" cellspacing="2" style=" margin-left:10px; margin-top:20px;">
            <tr>
                <td style=" width:60px;">Date</td>
                <td style=" width:150px;"><asp:TextBox ID="DateTextBox" runat="server" Width="155" onclick="WdatePicker({lang:'en'});"></asp:TextBox></td>
                <td style=" width:50px;"><img id="dateImage" src="../Images/calendar.gif" alt="" style=" cursor:pointer; vertical-align:middle;"/></td>
                <td style=" width:100px;">Daily Amount</td>
                <td style=" width:80px;">￥ <asp:Label ID="DailyAmountLabel" runat="server" Text="0.00"></asp:Label></td>
                <td style=" width:180px;"><asp:Button ID="SaveDailyConsumeButton" runat="server" 
                        Text="Save Daily Consume" onclick="SaveDailyConsumeButton_Click" /></td>
                <td style=" width:80px;"><a href="#" id="cancelA">Cancel</a></td>
                <td><asp:HiddenField ID="DateHiddenField" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <div><hr style="border: 1px solid #cc;" /></div>
    <div style="width: 460px; float: left;">
        <table cellpadding="2" cellspacing="2" style="margin-left:10px; margin-top:10px; width:80%;">
            <tr>
                <td style=" width:80px;">Type</td>
                <td>
                    <asp:DropDownList ID="TypeDropDownList" runat="server" Width="185">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td rowspan="2">Amount(￥)</td>
                <td>
                    <asp:TextBox ID="AmountTextBox" runat="server" Width="180"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <span style="color: Red;">Such as: 0, 8, 8.8, 88.88</span>
                </td>
            </tr>
            <tr>
                <td colspan="2">Description</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="DecriptionTextBox" runat="server" TextMode="MultiLine" Width="360"
                        Height="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:right;">
                Still can input <span id="stillSpan" style="color:Green; font-size:16px;">500</span> characters.
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="SaveConsumeLinkButton" runat="server" onclick="SaveConsumeLinkButton_Click">Save Consume</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td><asp:HiddenField ID="TypeHiddenField" runat="server" Value="" /></td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </div>
    <div style="width: 460px; float: left;">
        <asp:Panel ID="ConsumePanel" runat="server">
        </asp:Panel>
    </div>
</asp:Content>
