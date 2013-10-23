<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsumeControl.ascx.cs"
    Inherits="WebFamilyPortal.Controls.ConsumeControl" %>
<script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    $(function () {
        $('#<%=DeleteLinkButton.ClientID %>').click(function () {
            if (!confirm('confirm to delete?')) {
                return false;
            }
        });
    });
</script>
<fieldset style="border: 1px solid #cc; padding: 0 20px;">
    <legend>
        <asp:Label ID="TypeLabel" runat="server" style="color:green;font-size:14px;"></asp:Label>
    </legend>
    <table cellpadding="2" cellspacing="2" style="margin-bottom:5px;">
        <tr>
            <td style="width:80px; color:Red;">Amount(￥)</td>
            <td><asp:Label ID="AmountLabel" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="2" style="color:Red;"> Description</td>
        </tr>
        <tr>
            <td colspan="2"><asp:Label ID="DescriptionLabel" runat="server" style="word-break:break-all;" Width="360"></asp:Label></td>
        </tr>
        <tr>
            <td><asp:LinkButton ID="EditLinkButton" runat="server" 
                    onclick="EditLinkButton_Click">Edit</asp:LinkButton></td>
            <td><asp:LinkButton ID="DeleteLinkButton" runat="server" 
                    onclick="DeleteLinkButton_Click">Delete</asp:LinkButton></td>
        </tr>
    </table>
</fieldset>
