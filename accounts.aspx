<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="accounts.aspx.cs" Inherits="telco.accounts" %>

<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/accounts.js" type="text/javascript"></script>
    <div id="content_left">

        <div class="button">
            <div class="button-title">
                Copy
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonCopy" runat="server" ImageUrl="~/images/copy_large.gif" OnClientClick="return ImageButtonCopy_ClientClick()" />
            </div>
        </div>
        <div class="button">
            <div class="button-title">
                Number
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonCall" runat="server" ImageUrl="~/images/numbers.gif" OnClientClick="return ImageButtonCall_ClientClick()" />
            </div>
        </div>
        <div class="button">
            <div class="button-title">
                Delete
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonDelete" runat="server" ImageUrl="Images/delete_large.gif" OnClientClick="return ImageButtonDelete_ClientClick()" />
            </div>
        </div>
        <div class="button">
            <div class="button-title">
                Close
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonClose" runat="server" CausesValidation="false" ImageUrl="Images/close.gif" OnClientClick="history.back(); return false;" />
            </div>
        </div>

    </div>
    <div id="content_right">
        <div>
            <table id="view_table">
                <tr>
                    <td align="center"><b>Add New Account</b></td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table class="tier2">
                            <tr>
                                <td>Account Code (*):</td>
                                <td>
                                    <asp:TextBox ID="TextBoxCode" runat="server" MaxLength="20"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorCode" runat="server" ControlToValidate="TextBoxCode" 
                                    ErrorMessage="!">
                                </asp:RequiredFieldValidator>--%>
                                </td>
                                <td>Supplier Number (*):</td>
                                <td>
                                    <asp:TextBox ID="TextBoxNumber" runat="server" MaxLength="20"></asp:TextBox>
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidatorNumber" runat="server" ControlToValidate="TextBoxNumber" 
                                    ErrorMessage="!">
                                </asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>Supplier (*):</td>
                                <td>
                                    <asp:ComboBox ID="WebComboSupplier" runat="server" Editable="true">
                                    </asp:ComboBox>
                                </td>
                                <td>Client:</td>
                                <td>
                                    <asp:ComboBox ID="WebComboClient" runat="server" Editable="true">
                                    </asp:ComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="ButtonAdd" runat="server" Text="Add"
                                        OnClick="ButtonAdd_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div>

            <div>
                <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Code','Number','Name']"
                    colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                { name: 'Code', index: 'Code', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Number', index: 'Number', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                 { name: 'Name', index: 'Name', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}

                 ]"
                    rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                    viewrecords="true" width="700" height="500" url="accounts.aspx/BindJQGrid"
                    hasID="false" />
            </div>
        </div>
    </div>
</asp:Content>
