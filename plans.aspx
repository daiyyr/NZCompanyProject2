<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="plans.aspx.cs" Inherits="telco.plans" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/plans.js" type="text/javascript"></script>
    <div id="content_left">
        <div class="button">
            <div class="button-title">
                Copy
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonCopy" runat="server" ImageUrl="~/images/copy_large.gif" OnClientClick="return ImageButtonCopy_ClientClick()"  />
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
                    <td align="center"><b>Add New Plan</b></td>
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
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Name(*):</td>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBoxName" runat="server" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidatorName" runat="server" ErrorMessage="!"
                                        ControlToValidate="TextBoxName"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Type(*):</td>
                                <td>
                                    <asp:ComboBox ID="WebComboType" runat="server">
                                    </asp:ComboBox>
                                    <asp:CustomValidator ID="CustomValidatorType" runat="server"
                                        ErrorMessage="!" OnServerValidate="CustomValidatorType_ServerValidate"></asp:CustomValidator>
                                </td>
                                <td>Freq(*):</td>
                                <td>
                                    <asp:ComboBox ID="WebComboFreq" runat="server">
                                    </asp:ComboBox>
                                    <asp:CustomValidator ID="CustomValidatorFreq" runat="server"
                                        ErrorMessage="!" OnServerValidate="CustomValidatorFreq_ServerValidate"></asp:CustomValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>Included Number:</td>
                                <td>
                                    <asp:TextBox ID="WebNumericEditNumber" runat="server" DataMode="Int">
                                    </asp:TextBox>
                                </td>
                                <td>Local Minutes:</td>
                                <td>
                                    <asp:TextBox ID="WebNumericEditLC" runat="server" DataMode="Int">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>National Minutes:</td>
                                <td>
                                    <asp:TextBox ID="WebNumericEditNC" runat="server" DataMode="Int">
                                    </asp:TextBox>
                                </td>
                                <td>International Minutes:</td>
                                <td>
                                    <asp:TextBox ID="WebNumericEditIC" runat="server" DataMode="Int">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Mobile Minutes:</td>
                                <td>
                                    <asp:TextBox ID="WebNumericEditMC" runat="server" DataMode="Int">
                                    </asp:TextBox>
                                </td>
                                <td>Plan Minutes:</td>
                                <td>
                                    <asp:TextBox ID="WebNumericEditDC" runat="server" DataMode="Int">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Setup Fee:</td>
                                <td>
                                    <asp:TextBox ID="WebCurrencyEditSetupFee" runat="server" DataMode="Decimal">
                                    </asp:TextBox>
                                </td>
                                <td>Rental Fee:</td>
                                <td>
                                    <asp:TextBox ID="WebCurrencyEditCharge" runat="server" DataMode="Decimal">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>&nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td>Included Data</td>
                                <td>
                                    <asp:TextBox ID="DataT" runat="server">0</asp:TextBox>
                                </td>
                                <td>Exceed Rate</td>
                                <td>
                                    <asp:TextBox ID="RateT" runat="server">0</asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>Exceed Rate Meter</td>
                                <td>
                                    <asp:TextBox ID="RateMeterT" runat="server">0</asp:TextBox>
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
            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Code','Name']"
                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                 { name: 'Code', index: 'Code', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},

                          { name: 'Name', index: 'Name', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}
                             ]"
                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                viewrecords="true" width="700" height="500" url="plans.aspx/BindJQGrid"
                hasID="false" />
        </div>
    </div>
</asp:Content>
