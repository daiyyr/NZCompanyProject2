 <%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="clientslist.aspx.cs" Inherits="telco.clientslist" %>

<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script id="FormScript" src="js/clientslist.js" type="text/javascript"></script>
    <div id="content_left">
        <div>Add</div>
        <div>
            <asp:ImageButton ID="ImageButtonAdd"
                runat="server" ImageUrl="~/images/new.gif"
                OnClick="ImageButtonAdd_Click" />
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
        <div class="button">
            <div class="button-title">
                Close
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonClose" runat="server" CausesValidation="false" ImageUrl="Images/close.gif" OnClientClick="history.back(); return false;" />
            </div>
        </div>
    </div>
    <div id="content_right">
        <div>
            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Name','Category']"
                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                 { name: 'Name', index: 'Name', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},

                          { name: 'Category', index: 'Category', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}
                 ]"
                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                viewrecords="true" width="700" height="500" url="clientslist.aspx/BindJQGrid"
                hasID="false" />
        </div>
    </div>
</asp:Content>
