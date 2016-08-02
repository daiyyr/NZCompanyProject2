<%@ Page Title="" Language="C#" MasterPageFile="~/master.Master" AutoEventWireup="true" CodeBehind="projectscontracts.aspx.cs" Inherits="telco.projectscontracts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="content_left">
        <div>Go Back</div>
        <div>
            <asp:ImageButton ID="ImageButtonGoBack"
                runat="server" ImageUrl="~/images/goback.gif" OnClick="ImageButtonGoBack_Click" />
        </div>
    </div>
    <div id="content_right">
        <table id="view_table" style="width: 800px;">
            <tr>
                <td align="center"><b>Add Contract Into The Project</b></td>
                <td align="right"></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <table class="tier2" style="width: 100%">
                        <tr>
                            <td><b>Contracts</b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="UltraWebGrid1" runat="server"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="ButtonAdd" runat="server" Text="Add"
                                                    OnClick="ButtonAdd_Click" UID='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>
                            </td>
                        </tr>

                        <tr>
                            <td><b>Contracts In Project</b></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="UltraWebGrid2" runat="server"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="ButtonBill" runat="server" Text="Bill"
                                                    OnClick="ButtonBill_Click" UID='<%# Eval("TID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="ButtonRemove" runat="server" Text="Remove"
                                                    OnClick="ButtonRemove_Click" UID='<%# Eval("ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
