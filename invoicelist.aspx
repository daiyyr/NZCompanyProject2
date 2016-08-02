<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="invoicelist.aspx.cs" Inherits="telco.invoicelist" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content_left">
        <div>Search & Filter</div>
        <div>
            <div>Invoice Number:</div>
            <div>
                <asp:TextBox ID="WebNumericEditSearch" runat="server" Width="100px"></asp:TextBox>
            </div>
            <div>
                <br />
                Client:
            </div>
            <div>
                <asp:ComboBox DropDownStyle="DropDownList" AutoCompleteMode="SuggestAppend" CaseSensitive="False" ID="WebComboClient" runat="server" Width="80px">
                </asp:ComboBox>
            </div>
            <div>
                <br />
                Type:
            </div>
            <div>
                <asp:DropDownList ID="DropDownListType" runat="server" Width="100px">
                </asp:DropDownList>
            </div>
            <div>
                <asp:ImageButton ID="ImageButtonSearch"
                    runat="server" ImageUrl="~/images/menubut_find.gif"
                    OnClick="ImageButtonSearch_Click" />
            </div>
        </div>

        <div>Add</div>
        <div>
            <asp:ImageButton ID="ImageButtonAdd"
                runat="server" ImageUrl="~/images/new.gif" OnClick="ImageButtonAdd_Click" />
        </div>
        <div>Detail</div>
        <div>
            <asp:ImageButton ID="ImageButtonDetails" runat="server" ImageUrl="Images/detail.gif" OnClientClick="return ImageButtonDetails_ClientClick()" />
        </div>

    </div>


    <div id="content_right">

        <div>
            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['ID','Number','Date','Recipient','Amount','Paid Date','Paid Amount']"
                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                 { name: 'Number', index: 'Number', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},

                          { name: 'Date', index: 'Date', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']},formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},
                                                     { name: 'Recipient', index: 'Number', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                                     { name: 'Amount', index: 'Amount', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                                     { name: 'PaidDate', index: 'PaidDate', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']},formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}}   ,
                                                                     { name: 'PaidAmount', index: 'PaidAmount', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}             
                ]"
                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                viewrecords="true" width="700" height="500" url="invoicelist.aspx/BindJQGrid"
                hasID="false" />
        </div>

    </div>
</asp:Content>
