<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="telco.login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sapp Telco</title>
    <link rel="stylesheet" type="text/css" href="master.css" />
    <link rel="stylesheet" type="text/css" href="sub1.css" />
    <link rel="stylesheet" type="text/css" href="sub2.css" />
    <script id="MasterPageJS" type="text/javascript">
        function disableEnterKey(e) {
            var key;
            if (window.event)
                key = window.event.keyCode; //IE
            else
                key = e.which; //firefox      
            return (key != 13);
        }

    </script>
</head>
<body id="masterbody" onkeypress="return disableEnterKey(event)">
    <form id="form1" runat="server">
        <div id="container">
            <div id="header">
                <div id="left_logo">
                    <asp:Image ID="ImageLogo" runat="server" CssClass="ImageLogo"
                        ImageUrl="~/images/simpleapp-logo-t.png" ImageAlign="Middle" />
                </div>
                <div id="middle_msg">
                    <asp:Label ID="LabelAlertBoard" runat="Server" Font-Size="15px" ForeColor="Red"></asp:Label>
                </div>
                <div id="right_login">
                </div>
            </div>
            <div id="main">
                <div id="menu">
                </div>
                <div id="content">
                    <table id="view_table">
                        <tr>
                            <td align="center">
                                <table class="tier2">
                                    <tr>
                                        <td>Login:</td>
                                        <td>
                                            <asp:TextBox ID="TextBoxLogin" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Password:</td>
                                        <td>
                                            <asp:TextBox ID="TextBoxPassword" runat="server" MaxLength="8"
                                                TextMode="Password" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="ButtonLogin" runat="server" Text="Login"
                                                OnClick="ButtonLogin_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="rooter"></div>
        </div>
    </form>
</body>
</html>
