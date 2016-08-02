<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="taskadd.aspx.cs" Inherits="telco.taskadd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %><%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content_left">
</div>
<div id="content_right">
    <table id="view_table">
        <tr>
            <td align="center"> <b><asp:Literal ID="LiteralMode" runat="server">Add Task</asp:Literal></b></td>
            <td align="right">
                <asp:Label ID="LabelID" runat="server" Text="" Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <table class="tier2">
                    <tr>
                        <td>Project Member(*):</td>
                        <td>
                            <asp:ComboBox ID="WebComboPerson" runat="server">
                            </asp:ComboBox>
                            <asp:CustomValidator ID="CustomValidatorPerson" runat="server" 
                                ErrorMessage="!" onservervalidate="CustomValidatorPerson_ServerValidate"  ></asp:CustomValidator>
                        </td>
                        <td>StarStart Date(*):</td>
                        <td>
                            <asp:TextBox ID="WebDateChooserStartDate" runat="server" ></asp:TextBox>
                              <asp:CalendarExtender ID="CalendarExtender2" CssClass="sappcalendar"
                                    Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateChooserStartDate">
                                </asp:CalendarExtender>
                            <asp:CustomValidator ID="CustomValidatorStartDate" runat="server" 
                                ErrorMessage="!" 
                                onservervalidate="CustomValidatorStartDate_ServerValidate" ></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>End Date(*):</td>
                        <td>
                            <asp:TextBox ID="WebDateChooserEndDate" runat="server" ></asp:TextBox>
                                          <asp:CalendarExtender ID="CalendarExtender1" CssClass="sappcalendar"
                                    Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateChooserEndDate">
                                </asp:CalendarExtender>
                            <asp:CustomValidator ID="CustomValidatorEndDate" runat="server" 
                                ErrorMessage="!" 
                                onservervalidate="CustomValidatorEndDate_ServerValidate"  ></asp:CustomValidator>
                        </td>
                        <td>Duration(hrs)(*):
                        <td>
                            <asp:TextBox  ID="WebNumericEditDuration" runat="server" DataMode="Decimal"
                             HorizontalAlign="Left">
                            </asp:TextBox>
                            <asp:CustomValidator ID="CustomValidatorDuration" runat="server" 
                                ErrorMessage="!" 
                                onservervalidate="CustomValidatorDuration_ServerValidate"  ></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Billable(*):</td>
                        <td>
                            <asp:CheckBox ID="CheckBoxBillable" runat="server" Checked="true" />
                        </td>
                        <td>
                            
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Description:</td>
                        <td colspan="3">
                            <asp:TextBox ID="TextBoxDescription" runat="server" TextMode="MultiLine" 
                                Width="400px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right"> 
                            <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" onclick="ButtonSubmit_Click" 
                                 />
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
