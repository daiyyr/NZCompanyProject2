<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="contractdetails.aspx.cs" Inherits="telco.contractdetails" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content_left">
    <div>Go Back</div>
    <div>
        <asp:ImageButton ID="ImageButtonGoBack"
            runat="server" ImageUrl="~/images/goback.gif" 
            onclick="ImageButtonGoBack_Click" />
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
                <table class="tier2" width="749px">
                    <tr>
                        <td>ID:</td>
                        <td><asp:Label ID="LabelID" runat="server" Text=""></asp:Label></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Code:</td>
                        <td><asp:Label ID="LabelCode" runat="server" Text=""></asp:Label></td>
                        <td>Name:</td>
                        <td><asp:Label ID="LabelName" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Type:</td>
                        <td><asp:Label ID="LabelType" runat="server" Text=""></asp:Label></td>
                        <td>Freq:</td>
                        <td><asp:Label ID="LabelFreq" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Number:</td>
                        <td><asp:Label ID="LabelNumber" runat="server" Text=""></asp:Label></td>
                        <td>Local Calls (mins):</td>
                        <td><asp:Label ID="LabelLC" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td>National Calls (mins):</td>
                        <td><asp:Label ID="LabelNC" runat="server" Text=""></asp:Label></td>
                        <td>International Calls (mins):</td>
                        <td><asp:Label ID="LabelIC" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Mobile Calls (mins):</td>
                        <td><asp:Label ID="LabelMC" runat="server" Text=""></asp:Label></td>
                        <td>Discount Calls (mins):</td>
                        <td><asp:Label ID="LabelDC" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Setup Fee:</td>
                        <td><asp:Label ID="LabelSetupFee" runat="server" Text=""></asp:Label></td>
                        <td>Rental Fee:</td>
                        <td><asp:Label ID="LabelCharge" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Start Date:</td>
                        <td><asp:Label ID="LabelStart" runat="server" Text=""></asp:Label></td>
                        <td>End Date:</td>
                        <td><asp:Label ID="LabelEnd" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Included Data</td>
                        <td><asp:Label ID="LabelIncludeData" runat="server"></asp:Label></td>
                        <td>Exceed Rate</td>
                        <td><asp:Label ID="LabelExceedRate" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td>Exceed Rate Meter</td>
                        <td><asp:Label ID="LabelExceedRateMeter" runat="server"></asp:Label></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table>
                                <tr>
                                    <td>Pending:</td>
                                    <td>
                                        <asp:CheckBox ID="CheckBoxPending" runat="server" Enabled="false"/>
                                    </td>
                                    <td>Locked:</td>
                                    <td>
                                        <asp:CheckBox ID="CheckBoxLocked" runat="server" Enabled="false"/>
                                    </td>
                                    <td>Ended:</td>
                                    <td>
                                        <asp:CheckBox ID="CheckBoxEnded" runat="server" Enabled="false"/>
                                    </td>
                                    <td>Auto Renew:</td>
                                    <td>
                                        <asp:CheckBox ID="CheckBoxAutoRenew" runat="server" Enabled="false"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
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
