<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="invoicedetails.aspx.cs" Inherits="telco.invoicedetails" %>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/invoicedetails.js" type="text/javascript"></script>
    <div id="content_left">
        <div>Delete</div>
        <div>
            <asp:ImageButton ID="ImageButtonDelete" runat="server"
                ImageUrl="~/images/delete_large.gif" OnClick="ImageButtonDelete_Click" />
        </div>
        <div>Copy</div>
        <div>
            <asp:ImageButton ID="ImageButtonCopy" runat="server"
                ImageUrl="~/images/copy_large.gif" OnClick="ImageButtonCopy_Click" />
        </div>
        <div>Export PDF</div>
        <div>
            <asp:ImageButton ID="ImageButtonExportPDF" runat="server"
                ImageUrl="~/images/export_pdf.gif" OnClick="ImageButtonExportPDF_Click" />
        </div>
        <div>Set Payment Date</div>
        <div>
            <asp:ImageButton ID="ImageButtonSetPaymentDate" runat="server"
                ImageUrl="~/images/calendar.gif"
                OnClick="ImageButtonSetPaymentDate_Click" />
        </div>
    </div>
    <div id="content_right">
        <table id="view_table">
            <tr>
                <td>
                    <table class="tier2" width="750">
                        <tr>
                            <td colspan="4" align="left">
                                <b>Invoice:<asp:Literal ID="LiteralInvoiceNumber" runat="server"></asp:Literal></b>
                            </td>
                        </tr>
                        <tr>
                            <td><b>Invoice No.:</b></td>
                            <td>
                                <asp:Literal ID="LiteralInvoiceNumber2" runat="server"></asp:Literal></td>
                            <td><b>Invoice Date:</b></td>
                            <td>
                                <asp:Literal ID="LiteralInvoiceDate" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td><b>Customer Name:</b></td>
                            <td>
                                <asp:Literal ID="LiteralCustomerName" runat="server"></asp:Literal></td>
                            <td><b>Due Date:</b></td>
                            <td>
                                <asp:Literal ID="LiteralInvoiceDue" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td><strong>Paid Amount:</strong></td>
                            <td>
                                <asp:Label ID="PaidAmountL" runat="server"></asp:Label>
                            </td>
                            <td><b>Date Paid:</b></td>
                            <td>
                                <asp:Literal ID="LiteralPaydate" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td><b>Term:</b></td>
                            <td>
                                <asp:Literal ID="LiteralTerm" runat="server"></asp:Literal></td>
                        </tr>
                        <tr>
                            <td><b>Details:</b></td>
                            <td colspan="3">
                                <b>Total: </b>
                                <asp:Literal ID="LiteralTotal" runat="server"></asp:Literal><br />
                                <b>GST: </b>
                                <asp:Literal ID="LiteralGST" runat="server"></asp:Literal><br />
                                <b>GSTTotal: </b>
                                <asp:Literal ID="LiteralGSTTotal" runat="server"></asp:Literal>
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
                    viewrecords="true" width="700" height="500" url="invoicedetails.aspx/BindJQGrid"
                    hasID="false" />
            </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
