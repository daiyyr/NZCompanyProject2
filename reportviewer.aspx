<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reportviewer.aspx.cs" Inherits="telco.reportviewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %> 

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="LabelAlertBoard" runat="Server" Font-Size="15px" ForeColor="Red"></asp:Label>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        </div>
        <div>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="800px" Height="1200px">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
