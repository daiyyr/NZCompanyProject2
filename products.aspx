<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="products.aspx.cs" Inherits="telco.products" %>

<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script id="FormScript" src="js/products.js" type="text/javascript"></script>
    <div id="content_left">
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
                    <td align="center"><b>Add New Product</b></td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table class="tier2">
                            <tr>
                                <td>Code(*):</td>
                                <td>
                                    <asp:TextBox ID="TextBoxCode" runat="server" MaxLength="8"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorCode" runat="server" ErrorMessage="!"
                                        ControlToValidate="TextBoxCode"></asp:RequiredFieldValidator>
                                </td>
                                <td>Price(*):</td>
                                <td>
                                    <asp:TextBox ID="WebCurrencyEditPrice" runat="server"></asp:TextBox>

                                    <asp:CustomValidator ID="CustomValidatorPrice" runat="server"
                                        ErrorMessage="!"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Name(*):</td>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBoxName" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorName" runat="server" ErrorMessage="!"
                                        ControlToValidate="TextBoxName"></asp:RequiredFieldValidator>
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
            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Code','Name','Price']"
                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                                    { name: 'Code', index: 'Code', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                         { name: 'Name', index: 'Name', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                     { name: 'Price', index: 'Price', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}
                 ]"
                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                viewrecords="true" width="700" height="500" url="product.aspx/BindJQGrid"
                hasID="false" />
        </div>

    </div>
</asp:Content>
