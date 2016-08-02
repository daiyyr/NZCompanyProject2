<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="numbers.aspx.cs" Inherits="telco.numbers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %><%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/numbers.js" type="text/javascript"></script>
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
                Edit </div>
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
                            <asp:Literal ID="LiteralTitle" runat="server" Text="Add New Number:"></asp:Literal>
                            <asp:Literal ID="LiteralID" runat="server"></asp:Literal>
                        </b>
                    </td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table class="tier2">
                            <tr>
                                <td>Number(*):</td>
                                <td>
                                    <asp:TextBox ID="TextBoxCode" runat="server" MaxLength="20"></asp:TextBox>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Account(*):</td>
                                <td>
                                    <asp:ComboBox ID="WebComboAccount" runat="server">
                                    </asp:ComboBox>
                                </td>
                                <td>Client:</td>
                                <td>
                                    <asp:ComboBox ID="WebComboClient" runat="server">
                                    </asp:ComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Pending:</td>
                                <td>
                                    <asp:CheckBox ID="CheckBoxPending" runat="server" />
                                </td>
                                <td>Free:</td>
                                <td>
                                    <asp:CheckBox ID="CheckBoxFree" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Note:</td>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBoxNote" runat="server" TextMode="MultiLine" MaxLength="250"></asp:TextBox>
                                </td>
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
            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Number','Account','Client']"
                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                 { name: 'Number', index: 'Number', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},

                          { name: 'Account', index: 'Account', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                                     { name: 'Client', index: 'Client', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}
                             ]"
                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                viewrecords="true" width="700" height="500" url="numbers.aspx/BindJQGrid"
                hasID="false" />
        </div>

    </div>
</asp:Content>
