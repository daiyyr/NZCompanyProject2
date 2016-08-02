<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="callrecords.aspx.cs" Inherits="telco.callrecords" %>

<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/callrecords.js" type="text/javascript"></script>
    <div id="content_left">
        <div class="button">
            <div class="button-title">
                Billed
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonBilled" runat="server" ImageUrl="~/images/records_disabled.gif"  Enabled="False" OnClick="ImageButtonBilled_Click" />
            </div>
        </div>
        <div>Delete All</div>
        <div>
            <asp:ImageButton ID="ImageButtonDeleteAll"
                runat="server" ImageUrl="~/images/DeleteAll.gif"
                OnClick="ImageButtonDeleteAll_Click" />
        </div>
        <div>Close</div>
        <div>
            <asp:ImageButton ID="ImageButtonClose" runat="server" CausesValidation="false" ImageUrl="Images/close.gif" OnClientClick="history.back(); return false;" />
        </div>
    </div>
    <div id="content_right">

        <div>
            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','From','To','Description','Status','Date','Time','Length']"
                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                { name: 'From', index: 'From', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'To', index: 'To', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Description', index: 'Description', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Status', index: 'Status', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Date', index: 'Date', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']},formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},
                                { name: 'Time', index: 'Time', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                { name: 'Length', index: 'Length', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}
                              
                                 ]"
                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                viewrecords="true" width="700" height="500" url="callrecords.aspx/BindJQGrid"
                hasID="false" />
        </div>
        <div>
        </div>
    </div>

</asp:Content>
