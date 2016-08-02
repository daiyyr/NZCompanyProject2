<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="invoiceedit.aspx.cs" Inherits="telco.invoiceedit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/invoiceedit.js" type="text/javascript"></script>
    <div id="content_left">
        <div>Submit Invoice</div>
        <div>
            <asp:ImageButton ID="ImageButtonCreateInvoice" runat="server"
                ImageUrl="~/images/Submit.gif" OnClick="ImageButtonCreateInvoice_Click" />
        </div>
    </div>
    <div id="content_right">

        <table id="view_table">
            <tr>
                <td>
                    <table class="tier2" width="750">
                        <tr>
                            <td colspan="4" align="left"><b>Create Invoice</b></td>
                        </tr>
                        <tr>
                            <td>Invoice Number(*):</td>
                            <td>
                                <asp:TextBox ID="WebNumericEditInvoiceNumber" runat="server"></asp:TextBox>

                                <asp:CustomValidator ID="CustomValidatorInvoiceNumber" runat="server"
                                    ErrorMessage="!"
                                    OnServerValidate="CustomValidatorInvoiceNumber_ServerValidate"></asp:CustomValidator>
                            </td>
                            <td>Invoice Date(*):</td>
                            <td>

                                <asp:TextBox ID="WebDateChooserInvoiceDate" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" CssClass="sappcalendar"
                                    Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateChooserInvoiceDate">
                                </asp:CalendarExtender>
                                <asp:CustomValidator ID="CustomValidatorInvoiceDate" runat="server"
                                    ErrorMessage="!"
                                    OnServerValidate="CustomValidatorInvoiceDate_ServerValidate"></asp:CustomValidator>

                            </td>
                        </tr>
                        <tr>
                            <td>Due Date:</td>
                            <td>
                                <asp:TextBox ID="WebDateChooserInvoiceDue" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" CssClass="sappcalendar"
                                    Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateChooserInvoiceDue">
                                </asp:CalendarExtender>
                            </td>
                            <td>Term:</td>
                            <td>
                                <asp:TextBox ID="TextBoxTerm" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Customer(*):</td>
                            <td>

                                <asp:ComboBox ID="WebComboCustomer" runat="server"></asp:ComboBox>

                                <asp:CustomValidator ID="CustomValidatorCustomer" runat="server"
                                    ErrorMessage="!"
                                    OnServerValidate="CustomValidatorCustomer_ServerValidate"></asp:CustomValidator>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table class="tier2" width="750">
                        <tr>
                            <td colspan="4">
                                <asp:Literal ID="LiteralTitle" runat="server" Text="Add New Line:"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>Product Code(*)</td>
                            <td>Description</td>
                            <td>Qty(*)</td>
                            <td>Price(*)</td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Literal ID="LiteralID" runat="server" Visible="false"></asp:Literal>
                                <asp:TextBox ID="TextBoxProductCode" runat="server" Width="150"></asp:TextBox>

                            </td>
                            <td valign="top">
                                <asp:TextBox ID="TextBoxDescription" runat="server" Width="350" TextMode="MultiLine"></asp:TextBox>
                            </td>
                            <td valign="top">

                                <asp:TextBox ID="WebNumericEditQty" runat="server" datamode="Int" Width="50">
                                </asp:TextBox>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="WebNumericEditPrice" runat="server" datamode="Decimal" Width="150">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="left">
                                <asp:Button ID="ButtonAdd" runat="server" Text="Add Line" CausesValidation="false"
                                    OnClick="ButtonAdd_Click" />
                                <asp:Button ID="ButtonUpdate" runat="server" Text="Update" Visible="false" CausesValidation="false"
                                    OnClick="ButtonUpdate_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>

                    <div>
                        <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Code','Description','Qty','Price']"
                            colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                { name: 'Code', index: 'Code', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Description', index: 'Description', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                 { name: 'Qty', index: 'Qty', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                                     { name: 'Price', index: 'Price', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}

                 ]"
                            rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                            viewrecords="true" width="700" height="500" url="invoiceedit.aspx/BindJQGrid"
                            hasID="false" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
