<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="projectdetails.aspx.cs" Inherits="telco.projectdetails" %>

<%@ Register Assembly="jqGridAdv" Namespace="jqGridAdv" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    &nbsp;<script id="FormScript" src="js/projectdetails.js" type="text/javascript"></script><div runat="server" id="box" style="padding-left: 20%; padding-top: 10%;">

        <table style="width: 500px;" class="tier2">
            <tr>
                <td>Add unbilled plans</td>
                <td>
                    <asp:CheckBox ID="CheckBoxPlans" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Add Data Usage</td>
                <td>
                    <asp:CheckBox ID="CheckBoxDataUsage" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Add unbilled call records</td>
                <td>
                    <asp:CheckBox ID="CheckBoxCallRecords" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Add unbilled materials</td>
                <td>
                    <asp:CheckBox ID="CheckBoxMaterials" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Add unbilled manhours</td>
                <td>
                    <asp:CheckBox ID="CheckBoxManhours" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:Button ID="ButtonCreateInvoice" runat="server" Text="Create Invoice" OnClick="ButtonCreateInvoice_Click" />
                    <asp:Button ID="ButtonCreateInvoice0" runat="server" Text="Cancel" OnClick="ButtonCreateInvoice0_Click" />
                </td>
            </tr>
        </table>

    </div>
    <asp:Panel ID="Panel1" runat="server">
        <div id="content_left">
            <div>Edit</div>
            <div>
                <asp:ImageButton ID="ImageButtonEdit"
                    runat="server" ImageUrl="~/images/Edit.gif"
                    OnClick="ImageButtonEdit_Click" />
            </div>
            <div>Delete</div>
            <div>
                <asp:ImageButton ID="ImageButtonDelete"
                    runat="server" ImageUrl="~/images/delete_large.gif"
                    OnClick="ImageButtonDelete_Click" />
            </div>
            <div>Contracts</div>
            <div>
                <asp:ImageButton ID="ImageButtonContracts"
                    runat="server" ImageUrl="~/images/contracts.gif"
                    OnClick="ImageButtonContracts_Click" />
            </div>
            <div>CallRecords</div>
            <div>
                <asp:ImageButton ID="ImageButtonRecords"
                    runat="server" ImageUrl="~/images/Records.gif"
                    OnClick="ImageButtonRecords_Click" />
            </div>
            <div>Create Invoice</div>
            <div>
                <asp:ImageButton ID="ImageButtonCreateInvoice" runat="server"
                    ImageUrl="~/images/Invoice.gif" OnClick="ImageButtonCreateInvoice_Click1" />
            </div>
            <div>Close</div>
            <div>
                <asp:ImageButton ID="ImageButtonClose" runat="server"
                    ImageUrl="~/images/close.gif" OnClick="ImageButtonClose_Click" />
            </div>
        </div>
        <div id="content_right">
            <table id="view_table" style="width: 100%">
                <tr>
                    <td align="center"><b>
                        <asp:Literal ID="LiteralProjectTitle" runat="server">Project:</asp:Literal></b></td>
                    <td align="right"></td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <table class="tier2">
                            <tr>
                                <td><b>Client Name:</b></td>
                                <td>
                                    <asp:Label ID="LabelClientName" runat="server" Text=""></asp:Label>
                                </td>
                                <td><b>Category:</b></td>
                                <td>
                                    <asp:Label ID="LabelCategory" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Status:</b></td>
                                <td>
                                    <asp:Label ID="LabelStatus" runat="server" Text=""></asp:Label>
                                </td>
                                <td><b>Priority:</b></td>
                                <td>
                                    <asp:Label ID="LabelPriority" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Bill To:</b></td>
                                <td>
                                    <asp:Label ID="LabelBillClientName" runat="server" Text=""></asp:Label>
                                </td>
                                <td><b>Parent Project:</b></td>
                                <td>
                                    <asp:Label ID="LabelParent" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Description:</b></td>
                                <td colspan="3">
                                    <asp:TextBox ID="TextBoxDescription" runat="server" ReadOnly="true"
                                        TextMode="MultiLine" Width="500px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td><b>Project Manager:</b></td>
                                <td>
                                    <asp:Label ID="LabelManager" runat="server" Text=""></asp:Label>
                                </td>
                                <td><b>Start Date:</b></td>
                                <td>
                                    <asp:Label ID="LabelStart" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td><b>End Date:</b></td>
                                <td>
                                    <asp:Label ID="LabelEnd" runat="server" Text=""></asp:Label>
                                </td>
                                <td><b>Deadline:</b></td>
                                <td>
                                    <asp:Label ID="LabelDeadline" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <asp:Button ID="ButtonAddContract" runat="server" Text="Add Contract"
                                        OnClick="ButtonAddContract_Click" />
                                    <asp:Button ID="ButtonImportCallRecords" runat="server"
                                        Text="Import Call Records" OnClick="ButtonImportCallRecords_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Materials:</b>
                    </td>
                    <td>
                        <asp:Button ID="ButtonAddMaterial" runat="server" Text="Add Material"
                            OnClick="ButtonAddMaterial_Click" />

                        <asp:Button ID="Button1" runat="server" Text="Delete" OnClientClick="return ImageButtonDelete1_ClientClick()" />
                        <script>
                            function ImageButtonDelete1_ClientClick() {
                                var grid = jQuery("#" + GetClientId("jqGridTable1") + "_datagrid1");
                                var rowKey = grid.getGridParam("selrow");

                                if (rowKey) {
                                    var ID = grid.getCell(rowKey, 'ID');
                                    __doPostBack('__Page', 'ImageButtonDelete1|' + ID);
                                    return false;
                                }
                                else {
                                    alert("Please select a row first!");
                                    return false;
                                }

                                return false;
                            }
                        </script>
                    </td>
                </tr>
                <tr>

                    <td colspan="2" align="center">


                        <div>
                            <cc1:jqGridAdv runat="server" ID="jqGridTable1" colNames="['ID','Bill','Code','Name','Qty','Price']"
                                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                                    { name: 'Bill', index: 'Code', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}, formatter:'showlink'},
          
                                 { name: 'Code', index: 'Code', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                                     { name: 'Name', index: 'Name', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},

                                                     { name: 'Qty', index: 'Qty', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                                     { name: 'Price', index: 'Price', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}
                 ]"
                                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                                viewrecords="true" width="700" height="200" url="projectdetails.aspx/BindJQGrid1"
                                hasID="false" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Tasks:</b>
                    </td>
                    <td>
                        <asp:Button ID="ButtonAddTask" runat="server" Text="Add Task"
                            OnClick="ButtonAddTask_Click" />

                        <asp:Button ID="Button2" runat="server" Text="Delete" OnClientClick="return ImageButtonDelete2_ClientClick()" />
                        <script>
                            function ImageButtonDelete2_ClientClick() {
                                var grid = jQuery("#" + GetClientId("jqGridTable2") + "_datagrid1");
                                var rowKey = grid.getGridParam("selrow");

                                if (rowKey) {
                                    var ID = grid.getCell(rowKey, 'ID');
                                    __doPostBack('__Page', 'ImageButtonDelete2|' + ID);
                                    return false;
                                }
                                else {
                                    alert("Please select a row first!");
                                    return false;
                                }

                                return false;
                            }
                        </script>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">

                        <div>
                            <cc1:jqGridAdv runat="server" ID="jqGridTable2" colNames="['ID','Bill','Start','End','Duration','Description']"
                                colModel="[
                                { name: 'ID', index: 'ID', width: 50,editable:false, align: 'left', sorttype: 'int', search: true, searchoptions: { sopt: ['eq', 'ne', 'cn', 'nc']} , hidden:true},
                                                    { name: 'Bill', index: 'Code', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}, formatter:'showlink'},
          
                      { name: 'Start', index: 'Code', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}, formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},
                                { name: 'End', index: 'Number', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}, formatter: 'date', formatoptions:{srcformat: 'd/m/Y'}},
                                 { name: 'Duration', index: 'Duration', width: 100, editable:true, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}},
                                 { name: 'Description', index: 'Description', editable:true, width: 100, align: 'left', search: true, searchoptions: { sopt: ['cn', 'nc']}}

                 ]"
                                rowNum="25" rowList="[5, 10, 25, 50, 100]" sortname="ID" sortorder="asc"
                                viewrecords="true" width="700" height="200" url="projectdetails.aspx/BindJQGrid2"
                                hasID="false" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
</asp:Content>
