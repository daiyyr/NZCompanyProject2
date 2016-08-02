<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="numberlist.aspx.cs" Inherits="telco.numberlist" %>

<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script id="FormScript" src="js/clientslist.js" type="text/javascript"></script>
    <div id="content_left">
        <div>
            <div class="button">
                <div class="button-title">
                    Delete</div>
                <div>
                    <asp:ImageButton ID="ImageButton5" runat="server" ImageUrl="~/images/delete_large.gif" OnClientClick="return DeleteClick();" />
               <script>
                   function DeleteClick() {
                       if (confirm("Are you sure you want to delete the item?") == true) {
                           var grid = jQuery("#" + GetClientId("jqGridTable") + "_datagrid1");
                           var rowKey = grid.getGridParam("selrow");

                           if (rowKey) {
                               var ID = grid.getCell(rowKey, 'rate_cards_id');
                               __doPostBack('__Page', 'Delete|' + ID);
                               return false;
                           }
                           else {
                               alert("Please select a row first!");
                               return false;
                           }
                       }
                   }
               </script>
                     </div>
            </div>
        </div>
        <div class="button">
            <div class="button-title">
            </div>
            <div>
            </div>
        </div>
    </div>
    <div id="content_right">
        <div>
            <cc1:jqGridAdv runat="server" ID="jqGridTable" colNames="['rate_cards_id','rate_cards_code','rate_cards_description','rate_cards_number','rate_cards_rate','rate_cards_discount']"
                colModel="[
                                { name: 'rate_cards_id', index: 'rate_cards_id', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                 { name: 'rate_cards_code', index: 'rate_cards_code', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},

                          { name: 'rate_cards_description', index: 'rate_cards_description', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                          { name: 'rate_cards_number', index: 'rate_cards_number', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},

                                          { name: 'rate_cards_rate', index: 'rate_cards_rate', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                    { name: 'rate_cards_discount', index: 'rate_cards_discount', width: 50, editable:true, edittype:'checkbox', editoptions:{value:'1:0'}, align: 'left', search: false}
                 ]"
                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="rate_cards_id" sortorder="asc" inlineNav="true"
                viewrecords="true" width="700" height="500" url="numberlist.aspx/BindJQGrid"  editurl="numberlist.aspx/DataGridCommsSave"
                hasID="false" />
            </div>
    </div>
</asp:Content>
