<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="contractedit.aspx.cs" Inherits="telco.contractedit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %><%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content_left">
    </div>
    <div id="content_right">
        <table id="view_table">
            <tr>
                <td align="center"><b>
                    <asp:Literal ID="LiteralMode" runat="server">Edit</asp:Literal></b></td>
                <td align="right"></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <table class="tier2">
                        <tr>
                            <td>ID:</td>
                            <td>
                                <asp:Label ID="LabelID" runat="server" Text="Label"></asp:Label></td>
                            <td>Plan:</td>
                            <td>
                                <asp:ComboBox ID="WebComboPlan" runat="server"
                                    onselectedrowchanged="WebComboPlan_SelectedRowChanged" AutoPostBack="True" OnSelectedIndexChanged="WebComboPlan_SelectedIndexChanged">
                                </asp:ComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Client(*):</td>
                            <td>
                                <asp:ComboBox ID="WebComboClientID" runat="server">
                                </asp:ComboBox>
                                <asp:CustomValidator ID="CustomValidatorClientID" runat="server"
                                    ErrorMessage="!" OnServerValidate="CustomValidatorClientID_ServerValidate"></asp:CustomValidator>
                            </td>
                            <td>Code(*):</td>
                            <td>
                                <asp:TextBox ID="TextBoxCode" runat="server" MaxLength="8"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldCode" runat="server" ErrorMessage="!"
                                    ControlToValidate="TextBoxCode"></asp:RequiredFieldValidator>
                            </td>
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
                            <td>Included Numbers:</td>
                            <td>
                                <asp:TextBox ID="WebNumericEditNumber" runat="server"
                                    HorizontalAlign="Left" DataMode="Int">
                                </asp:TextBox>
                            </td>
                            <td>Local Minutes:</td>
                            <td>
                                <asp:TextBox ID="WebNumericEditLC" runat="server"
                                    HorizontalAlign="Left" DataMode="Int">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>National Minutes:</td>
                            <td>
                                <asp:TextBox ID="WebNumericEditNC" runat="server"
                                    HorizontalAlign="Left" DataMode="Int">
                                </asp:TextBox>
                            </td>
                            <td>International Minutes:</td>
                            <td>
                                <asp:TextBox ID="WebNumericEditIC" runat="server"
                                    HorizontalAlign="Left" DataMode="Int">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Mobile Minutes:</td>
                            <td>
                                <asp:TextBox ID="WebNumericEditMC" runat="server"
                                    HorizontalAlign="Left" DataMode="Int">
                                </asp:TextBox>
                            </td>
                            <td>Plan Minutes:</td>
                            <td>
                                <asp:TextBox ID="WebNumericEditDC" runat="server"
                                    HorizontalAlign="Left" DataMode="Int">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Setup Fee:</td>
                            <td>
                                <asp:TextBox ID="WebCurrencyEditSetupFee" runat="server"
                                    HorizontalAlign="Left" DataMode="Decimal">
                                </asp:TextBox>
                            </td>
                            <td>Rental:</td>
                            <td>
                                <asp:TextBox ID="WebCurrencyEditCharge" runat="server"
                                    HorizontalAlign="Left" DataMode="Decimal">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Start Date:</td>
                            <td>
                                <asp:TextBox ID="WebDateTimeEditStart" runat="server"
                                    HorizontalAlign="Left" DataMode="DateOrNull" ></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" CssClass="sappcalendar"
                                    Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateTimeEditStart">
                                </asp:CalendarExtender>
                            </td>
                            <td>End Date:</td>
                            <td>
                                <asp:TextBox ID="WebDateTimeEditEnd" runat="server"
                                    HorizontalAlign="Left" DataMode="DateOrNull" ></asp:TextBox>

                                <asp:CalendarExtender ID="WebDateTimeEditEnd_CalendarExtender" CssClass="sappcalendar"
                                    Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateTimeEditEnd">
                                </asp:CalendarExtender>

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
                                    <asp:TextBox ID="DataT" runat="server"></asp:TextBox>
                                </td>
                                <td>Exceed Rate</td>
                                <td>
                                    <asp:TextBox ID="RateT" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        <tr>
                                <td>&nbsp;</td>
                                <td>
                                    &nbsp;</td>
                                <td>Exceed Rate Meter</td>
                                <td>
                                    <asp:TextBox ID="RateMeterT" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        <tr>
                            <td colspan="4">
                                <table>
                                    <tr>
                                        <td>Pending:</td>
                                        <td>
                                            <asp:CheckBox ID="CheckBoxPending" runat="server" />
                                        </td>
                                        <td>Locked:</td>
                                        <td>
                                            <asp:CheckBox ID="CheckBoxLocked" runat="server" />
                                        </td>
                                        <td>Ended:</td>
                                        <td>
                                            <asp:CheckBox ID="CheckBoxEnded" runat="server" />
                                        </td>
                                        <td>Auto Renew:</td>
                                        <td>
                                            <asp:CheckBox ID="CheckBoxAutoRenew" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="right">
                                <asp:Button ID="ButtonSubmit" runat="server" Text="Submit"
                                    OnClick="ButtonSubmit_Click" />
                                <asp:Button ID="ButtonCancel" runat="server" Text="Cancel"
                                    OnClick="ButtonCancel_Click" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
