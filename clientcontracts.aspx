<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="clientcontracts.aspx.cs" Inherits="telco.clientcontracts" %>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script id="FormScript" src="js/clientcontracts.js" type="text/javascript"></script>
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
                <asp:ImageButton ID="ImageButtonDelete" runat="server" ImageUrl="~/images/delete_large.gif" OnClientClick="return ImageButtonDelete_ClientClick()" />
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
            <cc1:jqgridadv runat="server" id="jqGridTable" colnames="['ID','Code','Name','Type']"
                colmodel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                { name: 'Code', index: 'Code', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                 { name: 'Name', index: 'Name', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},

                          { name: 'Type', index: 'Type', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}
                 ]"
                rownum="25" rowlist="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                viewrecords="true" width="700" height="500" url="clientcontracts.aspx/BindJQGrid"
                hasid="false" />
        </div>
    </div>
</asp:Content>
