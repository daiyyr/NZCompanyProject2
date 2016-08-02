<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="invoicepaydate.aspx.cs" Inherits="telco.invoicepaydate" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %><%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content_left">
    <div>Go Back</div>
    <div>
        <asp:ImageButton ID="ImageButtonGoBack" runat="server" 
            ImageUrl="~/images/goback.gif" onclick="ImageButtonGoBack_Click"   />
    </div>
    <div>Submit</div>
    <div>
        <asp:ImageButton ID="ImageButtonSubmit" runat="server" 
            ImageUrl="~/images/Submit.gif" onclick="ImageButtonSubmit_Click"   />
    </div>
</div>
<div id="content_right">
    <table id="view_table">
        <tr>
            <td>
                <table class="tier2" width="750">
                    <tr>
                        <td>Invoice No.:</td>
                        <td>
                            <asp:Literal ID="LiteralNumber" runat="server"></asp:Literal>
                        </td>
                        <td>Invoice Date:</td>
                        <td>
                            <asp:Literal ID="LiteralDate" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>Term of Payment:</td>
                        <td>
                            <asp:Literal ID="LiteralTerm" runat="server"></asp:Literal>
                        </td>
                        <td>Due Date:</td>
                        <td>
                            <asp:Literal ID="LiteralDue" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>Amount:</td>
                        <td>
                            <asp:Literal ID="AmountL" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>Date Paid:</td>
                        <td>
                            <asp:TextBox ID="WebDateChooserPayDate" runat="server" ></asp:TextBox>
                                                                 <asp:CalendarExtender ID="CalendarExtender2" CssClass="sappcalendar"
                                    Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateChooserPayDate">
                                </asp:CalendarExtender>
                        </td>
                        <td>Paid Amount</td>
                        <td>
                            <asp:TextBox ID="PaidAmountT" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
</asp:Content>
