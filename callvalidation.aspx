<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="callvalidation.aspx.cs" Inherits="telco.callvalidation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/callvalidation.js" type="text/javascript"></script>
    <div id="content_left">
        <div>Refresh</div>
        <div>
            <asp:ImageButton ID="ImageButtonRefresh"
                runat="server" ImageUrl="~/images/refresh.gif"
                OnClick="ImageButtonRefresh_Click" />
            <div>
                Validate
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonValidate0" runat="server" ImageUrl="~/images/check.gif" OnClientClick="return ImageButtonValidate_ClientClick()"  />
            </div>
        </div>
        <div>Submit</div>
        <div>
            <asp:ImageButton ID="ImageButtonSubmit"
                runat="server" ImageUrl="~/images/Upload.gif"
                OnClick="ImageButtonSubmit_Click" />
        </div>
        <div>Close</div>
        <div>
            <asp:ImageButton ID="ImageButtonClose"
                runat="server" ImageUrl="~/images/close.gif"
                OnClick="ImageButtonClose_Click" />
        </div>
        <div class="button">
            <div class="button-title">
                Copy
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonCopy" runat="server" ImageUrl="~/images/copy_large.gif" OnClientClick="return ImageButtonCopy_ClientClick()" />
            </div>
        </div>
        <div class="button">
            <div class="button-title">
                Details
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonDetails" runat="server" ImageUrl="Images/detail.gif" OnClientClick="return ImageButtonDetails_ClientClick()" />
            </div>
        </div>
        <div class="button">
            <div class="button-title">
                Edit
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonEdit" runat="server" ImageUrl="Images/edit.gif" OnClientClick="return ImageButtonEdit_ClientClick()" />
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
    </div>
    <div id="content_right">
        <div>
            <table id="view_table">
                <tr>
                    <td align="center">
                        <b>
                            <asp:Literal ID="LiteralTitle" runat="server" Text="Add Call Record:"></asp:Literal>
                            <asp:Literal ID="LiteralID" runat="server"></asp:Literal>
                        </b>
                    </td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table class="tier2">
                            <tr>
                                <td><b>Account:</b></td>
                                <td>
                                    <asp:ComboBox ID="WebComboAccount" runat="server">
                                    </asp:ComboBox>
                                </td>
                                <td><b>Number:</b></td>
                                <td>
                                    <asp:ComboBox ID="WebComboNumber" runat="server">
                                    </asp:ComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td><b>From:</b></td>
                                <td>
                                    <asp:TextBox ID="TextBoxFrom" runat="server" MaxLength="20"></asp:TextBox>
                                </td>
                                <td><b>To:</b></td>
                                <td>
                                    <asp:TextBox ID="TextBoxTo" runat="server" MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Description:</b></td>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBoxDescription" runat="server" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Status:</b></td>
                                <td>
                                    <asp:TextBox ID="TextBoxStatus" runat="server" MaxLength="20"></asp:TextBox>
                                </td>
                                <td><b>Terminated:</b></td>
                                <td>
                                    <asp:TextBox ID="TextBoxTerminated" runat="server" MaxLength="20"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><b>DateDate:</b></td>
                                <td>
                                    <asp:TextBox ID="WebDateTimeEditDate" runat="server" DataMode="Date"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender2" CssClass="sappcalendar"
                                        Format="dd/MM/yyyy" runat="server" Enabled="True" TargetControlID="WebDateTimeEditDate">
                                    </asp:CalendarExtender>
                                </td>
                                <td><b>Time:</td>
                                <td>
                                    <asp:TextBox ID="WebDateTimeEditTime" runat="server" DataMode="Text">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Datetime</b></td>
                                <td>
                                    <asp:TextBox ID="WebDateTimeEditDateTime" runat="server" DataMode="Text">
                                    </asp:TextBox>
                                </td>
                                <td><b>Length</b></td>
                                <td>
                                    <asp:TextBox ID="WebNumericEditLenghth" runat="server" DataMode="Int">
                                    </asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Cost</b></td>
                                <td>
                                    <asp:TextBox ID="WebCurrencyEditCost" runat="server">
                                    </asp:TextBox>
                                </td>
                                <td><b>Type</b></td>
                                <td>
                                    <asp:TextBox ID="TextBoxType" runat="server" MaxLength="2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><b>SubType</b></td>
                                <td>
                                    <asp:TextBox ID="TextBoxSubType" runat="server" MaxLength="2"></asp:TextBox>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="ButtonAdd" runat="server" Text="Add"
                                        OnClick="ButtonAdd_Click" />
                                    <asp:Button ID="ButtonUpdate" runat="server" Text="Update" Visible="false"
                                        OnClick="ButtonUpdate_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="overflow: scroll">
            <div>
                <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Project ID','Account','Number','From','To','Description','Status','Terminated','Date','Time','DateTime','Length','Cost','Type','SubType','Valid']"
                    colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                { name: 'Project ID', index: 'Project ID', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Account', index: 'Account', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Number', index: 'Number', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'From', index: 'From', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'To', index: 'To', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Description', index: 'Description', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Status', index: 'Status', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Terminated', index: 'Terminated', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Date', index: 'Date', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']},formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},
                                { name: 'Time', index: 'Time', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'DateTime', index: 'DateTime', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Length', index: 'Length', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Cost', index: 'Cost', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},    
                                { name: 'Type', index: 'Type', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'SubType', index: 'SubType', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},      
                                { name: 'Valid', index: 'Valid', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},                                     
                                 ]"
                    rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                    viewrecords="true" width="700" height="500" url="callvalidation.aspx/BindJQGrid"
                    hasID="false" />
            </div>
        </div>
    </div>
</asp:Content>
