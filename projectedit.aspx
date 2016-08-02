<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="projectedit.aspx.cs" Inherits="telco.projectedit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %><%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/projectedit.js" type="text/javascript"></script>
    <div id="content_left">
    </div>
    <div id="content_right">
        <table id="view_table">
            <tr>
                <td align="center"><b>
                    <asp:Literal ID="LiteralMode" runat="server">Add Project</asp:Literal></b></td>
                <td align="right">
                    <asp:Label ID="LabelID" runat="server" Text="" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <table class="tier2">
                        <tr>
                            <td>Title(*):</td>
                            <td colspan="3">
                                <asp:TextBox ID="TextBoxTitle" runat="server" MaxLength="100" Width="500px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorTitle" runat="server" ErrorMessage="!"
                                    ControlToValidate="TextBoxTitle"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Client(*):</td>
                            <td>
                                <asp:ComboBox DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False" ID="WebComboClient" runat="server" BackColor="White"
                                    BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black"
                                    SelBackColor="DarkBlue" SelForeColor="White" Version="4.00" Editable="true">
                                </asp:ComboBox>
                                <asp:CustomValidator ID="CustomValidatorClient" runat="server"
                                    ErrorMessage="!" OnServerValidate="CustomValidatorClient_ServerValidate"></asp:CustomValidator>
                            </td>
                            <td>Category(*):</td>
                            <td>
                                <asp:ComboBox DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False" ID="WebComboCategory" runat="server" BackColor="White"
                                    BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black"
                                    SelBackColor="DarkBlue" SelForeColor="White" Version="4.00">
                                </asp:ComboBox>
                                <asp:CustomValidator ID="CustomValidatorCategory" runat="server"
                                    ErrorMessage="!"
                                    OnServerValidate="CustomValidatorCategory_ServerValidate"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>Status(*):</td>
                            <td>
                                <asp:ComboBox DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False" ID="WebComboStatus" runat="server" BackColor="White"
                                    BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black"
                                    SelBackColor="DarkBlue" SelForeColor="White" Version="4.00">
                                </asp:ComboBox>
                                <asp:CustomValidator ID="CustomValidatorStatus" runat="server"
                                    ErrorMessage="!" OnServerValidate="CustomValidatorStatus_ServerValidate"></asp:CustomValidator>
                            </td>
                            <td>Priority:</td>
                            <td>
                                <asp:ComboBox DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False" ID="WebComboPriority" runat="server" BackColor="White"
                                    BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black"
                                    SelBackColor="DarkBlue" SelForeColor="White" Version="4.00">
                                </asp:ComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Bill To Client:</td>
                            <td>
                                <asp:ComboBox DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False" ID="WebComboBillClient" runat="server" BackColor="White"
                                    BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black"
                                    SelBackColor="DarkBlue" SelForeColor="White" Version="4.00" Editable="true">
                                </asp:ComboBox>
                            </td>
                            <td>Parent Project:</td>
                            <td>
                                <asp:ComboBox DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False" ID="WebComboParent" runat="server" BackColor="White"
                                    BorderColor="Silver" BorderStyle="Solid" BorderWidth="1px" ForeColor="Black"
                                    SelBackColor="DarkBlue" SelForeColor="White" Version="4.00">
                                </asp:ComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Description:</td>
                            <td colspan="3">
                                <asp:TextBox ID="TextBoxDescription" runat="server" MaxLength="250"
                                    TextMode="MultiLine" Width="500px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Project Manager:</td>
                            <td>
                                <asp:ComboBox DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False" ID="WebComboManager" runat="server">
                                </asp:ComboBox>
                            </td>
                            <td>Start Date:</td>
                            <td>
                                <asp:TextBox ID="WebDateChooserStart" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" CssClass="sappcalendar"
                                    Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateChooserStart">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>End Date:</td>
                            <td>
                                <asp:TextBox ID="WebDateChooserEnd" runat="server"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" CssClass="sappcalendar"
                                    Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateChooserEnd">
                                </asp:CalendarExtender>
                            </td>
                            <td>Deadline:</td>
                            <td>
                                <asp:TextBox ID="WebDateChooserDeadline" runat="server">
               
                                </asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender3" CssClass="sappcalendar"
                                    Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateChooserDeadline">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" OnClick="ButtonSubmit_Click" />
                                <asp:Button ID="ButtonCancel" runat="server" Text="Cancel"
                                    OnClick="ButtonCancel_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
