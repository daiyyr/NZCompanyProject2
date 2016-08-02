<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="clientedit.aspx.cs" Inherits="telco.clientedit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %><%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="content_left">
        
</div>
<div id="content_right">
    <table id="view_table">
        <tr>
            <td align="center"> <b><asp:Literal ID="LiteralMode" runat="server">Edit</asp:Literal></b></td>
            <td align="right">
                
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table class="tier2">
                    <tr>
                        <td>ID:</td>
                        <td><asp:Label ID="LabelID" runat="server" Text="Label"></asp:Label></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>CPN ID:</td>
                        <td>
                            <asp:TextBox  ID="WebNumericEditCPNID" runat="server" 
                                HorizontalAlign="Left" DataMode="Int">
                            </asp:TextBox>
                        </td>
                        <td>Code:</td>
                        <td><asp:TextBox ID="TextBoxCode" runat="server" MaxLength="8"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Name(*):</td>
                        <td colspan="3">
                            <asp:TextBox ID="TextBoxName" runat="server" MaxLength="100" Width="400px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidatorName" runat="server" ErrorMessage="!"
                             ControlToValidate="TextBoxName"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Address:</td>
                        <td colspan="3"><asp:TextBox ID="TextBoxAddress" runat="server" TextMode="MultiLine" MaxLength="250" Width="400px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Post Address:</td>
                        <td colspan="3"><asp:TextBox ID="TextBoxAddress2" runat="server" TextMode="MultiLine" MaxLength="250" Width="400px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>City:</td>
                        <td><asp:TextBox ID="TextBoxCity" runat="server" MaxLength="100"></asp:TextBox></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Country:</td>
                        <td><asp:TextBox ID="TextBoxCountry" runat="server" MaxLength="100"></asp:TextBox></td>
                        <td>Category(*):</td>
                        <td>
                            <asp:ComboBox ID="WebComboCategory" runat="server" Width="125px">
                            </asp:ComboBox>
                            <asp:CustomValidator ID="CustomValidatorCategory" runat="server" ErrorMessage="!"
                                onservervalidate="CustomValidatorCategory_ServerValidate"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Tax:</td>
                        <td><asp:TextBox ID="TextBoxTax" runat="server" MaxLength="30"></asp:TextBox></td>
                        <td>URL:</td>
                        <td><asp:TextBox ID="TextBoxURL" runat="server" MaxLength="250"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Phone:</td>
                        <td><asp:TextBox ID="TextBoxPhone" runat="server" MaxLength="30"></asp:TextBox></td>
                        <td>Fax:</td>
                        <td><asp:TextBox ID="TextBoxFax" runat="server" MaxLength="30"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Email:</td>
                        <td><asp:TextBox ID="TextBoxEmail" runat="server" MaxLength="100"></asp:TextBox></td>
                        <td>Contact:</td>
                        <td><asp:TextBox ID="TextBoxContact" runat="server" MaxLength="100"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right"> 
                            <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" 
                                onclick="ButtonSubmit_Click" />
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