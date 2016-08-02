<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="suppliers.aspx.cs" Inherits="telco.suppliers" %>

<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/suppliers.js" type="text/javascript"></script>
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
                    <td align="center"><b>Add New Supplier</b></td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table class="tier2">
                            <tr>
                                <td>Name(*):</td>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBoxName" runat="server" MaxLength="100" Width="400px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorName" runat="server" ErrorMessage="!"
                                        ControlToValidate="TextBoxName"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Account Number:</td>
                                <td>
                                    <asp:TextBox ID="TextBoxAccount" runat="server" MaxLength="20"></asp:TextBox>
                                </td>
                                <td>Contact Person:</td>
                                <td>
                                    <asp:TextBox ID="TextBoxContact" runat="server" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Phone:</td>
                                <td>
                                    <asp:TextBox ID="TextBoxPhone" runat="server" MaxLength="30"></asp:TextBox>
                                </td>
                                <td>Fax:</td>
                                <td>
                                    <asp:TextBox ID="TextBoxFax" runat="server" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Email:</td>
                                <td>
                                    <asp:TextBox ID="TextBoxEmail" runat="server" MaxLength="100"></asp:TextBox>
                                </td>
                                <td>Web:</td>
                                <td>
                                    <asp:TextBox ID="TextBoxWeb" runat="server" MaxLength="100"></asp:TextBox>
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
            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Name']"
                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                         { name: 'Name', index: 'Name', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}
                                 ]"
                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                viewrecords="true" width="700" height="500" url="suppliers.aspx/BindJQGrid"
                hasID="false" />
        </div>
    </div>
</asp:Content>
