<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="projectlist2.aspx.cs" Inherits="telco.projectlist2" %>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content_left">
        <div>Add</div>
        <div>
            <asp:ImageButton ID="ImageButtonAdd"
                runat="server" ImageUrl="~/images/new.gif" OnClick="ImageButtonAdd_Click" />
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
                    Close
                </div>
                <div>
                    <asp:ImageButton ID="ImageButtonClose" runat="server" CausesValidation="false" ImageUrl="Images/close.gif" OnClientClick="history.back(); return false;" />
                </div>
            </div>
        </div>
    </div>
    <div id="content_right">
        <div>
                        <cc1:jqgridadv runat="server" id="jqGridTable" colnames="['ID','Title','Status','Start','Deadline','Category']"
                colmodel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                                         { name: 'Title', index: 'Title', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},

                                                         { name: 'Status', index: 'Status', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},

                                                         { name: 'Start', index: 'Start', sorttype: 'date', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}, formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},

                            { name: 'Deadline', index: 'Deadline', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']},formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},
                            { name: 'Category', index: 'Category', width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}

                                 ]"
                rownum="25" rowlist="[5, 10, 25, 50, 100]" sortname="Start" sortorder="desc"
                viewrecords="true" width="700" height="500" url="projectlist2.aspx/BindJQGrid"
                hasid="false" />
        </div>


    </div>
</asp:Content>
