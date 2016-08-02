<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="telco.users" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                Edit
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonEdit" runat="server" ImageUrl="Images/edit.gif" OnClientClick="return ImageButtonEdit_ClientClick()" />
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
                    <td align="center">
                        <b>
                            <asp:Literal ID="LiteralTitle" runat="server" Text="Add New User:"></asp:Literal>
                            <asp:Literal ID="LiteralID" runat="server"></asp:Literal>
                        </b>
                    </td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table class="tier2">
                            <tr>
                                <td>Login(*):</td>
                                <td>
                                    <asp:TextBox ID="TextBoxLogin" runat="server" MaxLength="20"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorLogin" runat="server" ErrorMessage="!"
                                        ControlToValidate="TextBoxLogin"></asp:RequiredFieldValidator>
                                </td>
                                <td>User Name:</td>
                                <td>
                                    <asp:TextBox ID="TextBoxName" runat="server" MaxLength="100"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Password:</td>
                                <td>
                                    <asp:TextBox ID="TextBoxPassword" runat="server" MaxLength="8" TextMode="Password"></asp:TextBox>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="ButtonAdd" runat="server" Text="Add"
                                        OnClick="ButtonAdd_Click" />
                                    <asp:Button ID="ButtonUpdate" runat="server" Text="Update" Visible="false"
                                        OnClick="ButtonUpdate_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
 

        <div>
            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Login','Name']"
                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                                    { name: 'Login', index: 'Login', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                         { name: 'Name', index: 'Name', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}
                                 ]"
                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                viewrecords="true" width="700" height="500" url="users.aspx/BindJQGrid"
                hasID="false" />
        </div>
    </div>
</asp:Content>
