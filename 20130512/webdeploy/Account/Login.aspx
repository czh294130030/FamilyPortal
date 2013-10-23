<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Login.aspx.cs" Inherits="WebFamilyPortal.Account.Login" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="../Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(function () {
            $('#OKButton').click(function () {
                if ($(this).attr('disabled') == true) {
                    return false;
                }
                $(this).attr('disabled', true);
                var $account = $('#AccountTextBox');
                var $password = $('#PasswordTextBox');
                if ($account.val() == '') {//account is required.
                    alert('Please input account.');
                    $(this).attr('disabled', false);
                    $account.focus();
                    return false;
                }
                if ($password.val() == '') {//password is required.
                    alert('Please input password.');
                    $(this).attr('disabled', false);
                    $password.focus();
                    return false;
                }
                var account = $account.val();
                var password = $password.val();
                $.ajax({
                    type: 'Get',   //访问WebService使用Get方式请求
                    url: '../Ajax.aspx',
                    data: 'account=' + account + '&password=' + password + '&action=login',
                    success: function (result) {//回调函数，result，返回值
                        if (result == "True") {
                            window.location.href = '../Default.aspx';
                        }
                        else {
                            alert('Login failed. Please try again.');
                            $('#OKButton').attr('disabled', false);
                            $password.val('');
                            $account.focus();
                            return false;
                        }
                    }
                });
            });
            $(document).keydown(function (event) {//when user click "enter" key which will trigger OKButton's click event.
                if (event.keyCode == 13) {
                    $('#OKButton').triggerHandler('click');
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2 style=" margin-left:10px;">
        Login
    </h2>
    <table cellpadding="5" cellspacing="5">
        <tr>
            <td>Account</td>
            <td>
                <input id="AccountTextBox" type="text" style=" width:160px;" />
                <span style=" color:Red;">*</span>
            </td>
        </tr>
        <tr>
            <td>Password</td>
            <td>
                <input id="PasswordTextBox" type="password" style=" width:160px;" />
                <span style=" color:Red;">*</span>
            </td>
        </tr>
        <tr>
            <td><input id="OKButton" type="button" value="OK"/></td>
            <td><a href="../Default.aspx">Cancel</a></td>
        </tr>
    </table>
</asp:Content>
