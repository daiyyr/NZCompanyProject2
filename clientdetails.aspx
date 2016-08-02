<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="clientdetails.aspx.cs" Inherits="telco.clientdetails" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script id="FormScript" src="js/clientdetails.js" type="text/javascript"></script>
<div id="content_left">
    <div>Contracts</div>
    <div>
        <asp:ImageButton ID="ImageButtonContracts" runat="server" 
            ImageUrl="~/images/contracts.gif" onclick="ImageButtonContracts_Click" />
    </div>
    <div>Generate Invoice</div>
    <div>
        <asp:ImageButton ID="ImageButtonCreateInvoice" runat="server" 
            ImageUrl="~/images/Invoice.gif" onclick="ImageButtonCreateInvoice_Click"/>
    </div>
</div>
<div id="content_right">
    <table id="view_table">
        <tr>
            <td align="center"> <b>Client:</b> <asp:Label ID="LabelClientName" runat="server" Text="client_name" Font-Bold="true"></asp:Label></td>
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
                                    <td><asp:TextBox ID="TextBoxCPNID" runat="server" ReadOnly="True"></asp:TextBox></td>
                                    <td>Code:</td>
                                    <td><asp:TextBox ID="TextBoxCode" runat="server" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Name:</td>
                                    <td colspan="3"><asp:TextBox ID="TextBoxName" runat="server" ReadOnly="true" Width="400px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Address:</td>
                                    <td colspan="3"><asp:TextBox ID="TextBoxAddress" runat="server" TextMode="MultiLine" ReadOnly="true" Width="400px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Post Address:</td>
                                    <td colspan="3"><asp:TextBox ID="TextBoxAddress2" runat="server" TextMode="MultiLine" ReadOnly="true" Width="400px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>City:</td>
                                    <td><asp:TextBox ID="TextBoxCity" runat="server" ReadOnly="true"></asp:TextBox></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td>Country:</td>
                                    <td><asp:TextBox ID="TextBoxCountry" runat="server" ReadOnly="true"></asp:TextBox></td>
                                    <td>Category:</td>
                                    <td><asp:TextBox ID="TextBoxCategory" runat="server" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Tax:</td>
                                    <td><asp:TextBox ID="TextBoxTax" runat="server" ReadOnly="true"></asp:TextBox></td>
                                    <td>URL:</td>
                                    <td><asp:TextBox ID="TextBoxURL" runat="server" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Phone:</td>
                                    <td><asp:TextBox ID="TextBoxPhone" runat="server" ReadOnly="true"></asp:TextBox></td>
                                    <td>Fax:</td>
                                    <td><asp:TextBox ID="TextBoxFax" runat="server" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>Email:</td>
                                    <td><asp:TextBox ID="TextBoxEmail" runat="server" ReadOnly="true"></asp:TextBox></td>
                                    <td>Contact:</td>
                                    <td><asp:TextBox ID="TextBoxContact" runat="server" ReadOnly="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right"> 
                                        <asp:Button ID="ButtonDelete" runat="server" Text="Delete" 
                                            onclick="ButtonDelete_Click"/>
                                        <asp:Button ID="ButtonEdit" runat="server" Text="Edit" 
                                            onclick="ButtonEdit_Click" />
                                    </td>
                                </tr>
                            </table>
            </td>
        </tr>
    </table>
</div>
</asp:Content>