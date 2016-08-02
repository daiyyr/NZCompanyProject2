<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="datausage.aspx.cs" Inherits="telco.datausage" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content_left">

        <div>Delete Batch</div>
        <div>
            <asp:ImageButton ID="ImageButtonDelete" runat="server" ImageUrl="~/images/delete_large.gif" OnClientClick="return ImageButtonDelete_ClientClick();" />

        </div>
    </div>
    <div id="content_right">
        <div>
            <table id="view_table">
                <tr>
                    <td align="center"><b>Import Data File</b></td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table class="tier2">
                            <tr>
                                <td>File Name:</td>
                                <td colspan="2">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </td>
                                <td>Days<asp:DropDownList ID="DaysDL" runat="server">
                                    <asp:ListItem>7</asp:ListItem>
                                    <asp:ListItem>30</asp:ListItem>
                                    <asp:ListItem>31</asp:ListItem>
                                    <asp:ListItem>28</asp:ListItem>
                                    <asp:ListItem>29</asp:ListItem>
                                </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Upload" Style="height: 26px" />
                                </td>
                            </tr>
                            <tr>
                                <td>Start Date</td>
                                <td>
                                    <asp:TextBox ID="StartDateT" runat="server" Enabled="False"></asp:TextBox>
                                    <%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>--%>
                                </td>
                                <td>End Date</td>
                                <td>
                                    <asp:TextBox ID="EndDateT" runat="server" Enabled="False"></asp:TextBox>
                                    <%--           <asp:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="sappcalendar" Enabled="True" Format="dd/MM/yyyy" TargetControlID="StartDateT">
                                    </asp:CalendarExtender>--%>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="ButtonImport" runat="server" Text="Import"
                                        OnClick="ButtonImport_Click" style="height: 26px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <div id="progressbar"></div>
                    </td>
                </tr>
                <tr>
                    <td align="right">

                        <div>
                            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Batch','StartDate','EndDate','BillDate','radius_user','ub_username','dslplanname','email','DataLimit','shapeuser','Shaped','1st_notice','2nd_notice','3rd_notice','tot_upload','tot_download','tot_offpeakupload','tot_offpeakdownload','tot_usage','utilization','GroupName']"
                                colModel="[

                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                                                 { name: 'StartDate', index: 'StartDate', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},             
                                 { name: 'StartDate', index: 'StartDate', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']},formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},             
                                { name: 'EndDate', index: 'EndDate', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']},formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},             
                                    { name: 'BillDate', index: 'BillDate', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']},formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},             
                                  { name: 'radius_user', index: 'radius_user', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'ub_username', index: 'ub_username', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'dslplanname', index: 'dslplanname', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'email', index: 'email', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'DataLimit', index: 'DataLimit', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'shapeuser', index: 'shapeuser', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Shaped', index: 'Shaped', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: '1st_notice', index: '1st_notice', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: '2nd_notice', index: '2nd_notice', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']},formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},
                                { name: '3rd_notice', index: '3rd_notice', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'tot_upload', index: 'tot_upload', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'tot_download', index: 'tot_download', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'tot_offpeakupload', index: 'tot_offpeakupload', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},    
                                { name: 'tot_offpeakdownload', index: 'tot_offpeakdownload', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'tot_usage', index: 'tot_usage', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},      
                                { name: 'utilization', index: 'utilization', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},             
                                                { name: 'GroupName', index: 'GroupName', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},                                

                                    ]"
                                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                                viewrecords="true" width="950" height="500" url="datausage.aspx/BindJQGrid"
                                hasID="false" />
                        </div>
                    </td>
                    <td>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/overlay.png" />
                    </td>
                </tr>
                <tr>
                    <td align="right">&nbsp;</td>
                    <td>
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/overlay.png" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>
