<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="reports.aspx.cs" Inherits="telco.reports" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content_left">
        <div>
        </div>
        <div>
        </div>
    </div>
    <div id="content_right">
        Start<asp:TextBox ID="StartT" runat="server"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender1" CssClass="sappcalendar"
            Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="StartT">
        </asp:CalendarExtender>
        &nbsp;&nbsp;&nbsp; End<asp:TextBox ID="EndT" runat="server"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender2" CssClass="sappcalendar"
            Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="EndT">
        </asp:CalendarExtender>
        <br /><br />
        <asp:Button ID="ButtonAccountReceivable" runat="server"
            Text="Account Receivable Report" OnClick="ButtonAccountReceivable_Click" />
        <br />
        <br />
        <asp:Button ID="btnSearchDebtorDetails" runat="server"
            Text="Debtor Aged Details Report" OnClick="btnSearchDebtorDetails_Click" />
        <asp:DropDownList ID="debtor_client" runat="server">
                        <asp:ListItem>ALL</asp:ListItem>
                    </asp:DropDownList>
    </div>

</asp:Content>
