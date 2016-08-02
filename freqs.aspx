<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="freqs.aspx.cs" Inherits="telco.freqs" %>

<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/freqs.js" type="text/javascript"></script>
    <div id="content_left">
        <div>
            <div class="button">
                <div class="button-title">
                    Delete
                </div>
                <div>
                    <asp:ImageButton ID="ImageButtonDelete" runat="server" ImageUrl="Images/delete_large.gif" OnClientClick="return ImageButtonDelete_ClientClick()" />
                </div>
            </div>
        </div>
    </div>
    <div id="content_right">
        <div>
            <table id="view_table">
                <tr>
                    <td align="center"><b>Add New Freq</b></td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table class="tier2">
                            <tr>
                                <td>Freq Name(*):</td>
                                <td>
                                    <asp:TextBox ID="TextBoxName" runat="server" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorName" runat="server" ErrorMessage="!"
                                        ControlToValidate="TextBoxName"></asp:RequiredFieldValidator>
                                </td>
                                <td></td>
                                <td></td>
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
                                 { name: 'Name', index: 'Name', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}

                 ]"
                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                viewrecords="true" width="700" height="500" url="freqs.aspx/BindJQGrid"
                hasID="false" />

        </div>
    </div>
</asp:Content>
