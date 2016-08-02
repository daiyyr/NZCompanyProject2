<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="materialadd.aspx.cs" Inherits="telco.materialadd" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %><%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<script id="FormScript" src="js/materialadd.js" type="text/javascript"></script>
<div id="content_left">
</div>
<div id="content_right">
    <table id="view_table">
        <tr>
            <td align="center"> <b><asp:Literal ID="LiteralMode" runat="server">Add Material</asp:Literal></b></td>
            <td align="right">
                <asp:Label ID="LabelID" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table class="tier2">
                    <tr>
                        <td>Code(*):</td>
                        <td>
                            <asp:ComboBox ID="WebComboCode" runat="server" Editable="true" 
                                onselectedrowchanged="WebComboCode_SelectedRowChanged">
                            </asp:ComboBox>

                            <asp:CustomValidator ID="CustomValidatorCode" runat="server" 
                                ErrorMessage="!" onservervalidate="CustomValidatorCode_ServerValidate" ></asp:CustomValidator>
                        </td>
                        <td>Name(*):</td>
                        <td>
                            <asp:TextBox ID="TextBoxName" runat="server" MaxLength="100" Width="400px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorName" runat="server" ErrorMessage="!"
                             ControlToValidate="TextBoxName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Description:</td>
                        <td colspan="3">
                            <asp:TextBox ID="TextBoxDescription" runat="server" MaxLength="250" 
                                TextMode="MultiLine" Width="650px" Height="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Price(*):</td>
                        <td>
                            <asp:TextBox ID="WebCurrencyEditPrice" runat="server" DataMode="Decimal">
                            </asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorPrice" runat="server" 
                                ErrorMessage="!" onservervalidate="CustomValidatorPrice_ServerValidate" ></asp:CustomValidator>
                        </td>
                        <td>Qty(*):</td>
                        <td>
                            <asp:TextBox  ID="WebNumericEditQty" runat="server" DataMode="Int">
                            </asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorQty" runat="server" 
                                ErrorMessage="!" onservervalidate="CustomValidatorQty_ServerValidate" ></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right"> 
                            <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" onclick="ButtonSubmit_Click"/>
                            <asp:Button ID="ButtonCancel" runat="server" Text="Cancel" CausesValidation="false"
                                onclick="ButtonCancel_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
</asp:Content>
