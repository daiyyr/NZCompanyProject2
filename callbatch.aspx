<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="callbatch.aspx.cs" Inherits="telco.callbatch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/callbatch.js" type="text/javascript"></script><div id="content_left">

        <div>Refresh</div>
        <div>
            <asp:ImageButton ID="ImageButtonRefresh"
                runat="server" ImageUrl="~/images/refresh.gif"
                OnClick="ImageButtonRefresh_Click" />
        </div>
        <div>SubmitDATA</div>
        <div>
            <asp:ImageButton ID="ImageButtonValidate"
                runat="server" ImageUrl="~/images/check.gif"
                OnClick="ImageButtonValidate_Click" />
        </div>
        <div>Delete All</div>
        <div>
            <asp:ImageButton ID="ImageButtonDeleteAll"
                runat="server" ImageUrl="~/images/DeleteAll.gif"
                OnClick="ImageButtonDeleteAll_Click" />
        </div>
                <div>Delete Upload Files</div>
        <div>
            <asp:ImageButton ID="ImageButton1"
                runat="server" ImageUrl="~/images/DeleteAll.gif"
                OnClick="ImageButton1_Click" />
        </div>
        <div>Close</div>
        <div>
            <asp:ImageButton ID="ImageButtonClose"
                runat="server" ImageUrl="~/images/close.gif"
                OnClick="ImageButtonClose_Click" />
        </div>
    </div>
    <div id="content_right">
        <div>
            <table id="view_table">
                <tr>
                    <td align="center"><b>Import Data File</b></td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table class="tier2">
                            <tr>
                                <td>File Name:</td>
                                <td colspan="2">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </td>
                                <td>
                                    <asp:Button ID="ButtonUpload" runat="server" Text="Upload"
                                        OnClick="ButtonUpload_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>File List</td>
                                <td colspan="3">
                                    <asp:Literal ID="LiteralFileList" runat="server"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td>Supplier(*):</td>
                                <td>
                                    <asp:ComboBox ID="WebComboSupplier" runat="server">
                                    </asp:ComboBox>
                                    <asp:CustomValidator ID="CustomValidatorSupplier" runat="server"
                                        ErrorMessage="!" EnableClientScript="False" Enabled="True"
                                        OnServerValidate="CustomValidatorSupplier_ServerValidate"></asp:CustomValidator>
                                </td>
                                <td>Account(*):</td>
                                <td>
                                    <asp:ComboBox ID="WebComboAccount" runat="server">
                                    </asp:ComboBox>
                                    <asp:CustomValidator ID="CustomValidatorAccount" runat="server"
                                        ErrorMessage="!" EnableClientScript="False" Enabled="True" OnServerValidate="CustomValidatorAccount_ServerValidate"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="ButtonImport" runat="server" Text="Import"
                                        OnClick="ButtonImport_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <div id="progressbar"></div>
                    </td>
                </tr>
                <tr>
                    <td align="right">

                        <div>
                            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Project ID','Account','Number','From','To','Description','Status','Terminated','Date','Time','DateTime','Length','Cost','Type','SubType','Valid']"
                                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                { name: 'Project ID', index: 'Project ID', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Account', index: 'Account', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Number', index: 'Number', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'From', index: 'From', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'To', index: 'To', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Description', index: 'Description', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Status', index: 'Status', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Terminated', index: 'Terminated', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Date', index: 'Date', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']},formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},
                                { name: 'Time', index: 'Time', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'DateTime', index: 'DateTime', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Length', index: 'Length', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Cost', index: 'Cost', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},    
                                { name: 'Type', index: 'Type', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'SubType', index: 'SubType', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},      
                                { name: 'Valid', index: 'Valid', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},                                     
                                 ]"
                                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                                viewrecords="true" width="700" height="500" url="callbatch.aspx/BindJQGrid"
                                hasID="false" />
                        </div>
                    </td>
                    <td>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/overlay.png" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        &nbsp;</td>
                    <td>
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/overlay.png" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>
