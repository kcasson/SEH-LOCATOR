<%@ Page Title="" Language="VB" MasterPageFile="~/Site/StEPMMWeb.master" AutoEventWireup="false" CodeFile="InvoiceOnlyAdmin.aspx.vb" Inherits="Site_InvoiceOnlyAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table width="720px" border="0" cellpadding="0" cellspacing="0">
        <tr height="20px"><td colspan="2" class="menuhead"> &nbsp;Invoice Only Administrator Workbench</td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td width="200px"><b>Select By Status:</b><br />
            <asp:DropDownList ID="LoadFocus" runat="server" AutoPostBack="true">
            <asp:ListItem Text="Select" Value="-1"></asp:ListItem>
            <asp:ListItem Text="Pending Item Build" Value="3"></asp:ListItem>
            <asp:ListItem Text="Pending Approval" Value="0"></asp:ListItem>
            <asp:ListItem Text="Pending Completion" Value="1"></asp:ListItem>
            <asp:ListItem Text ="Complete" Value="4"></asp:ListItem>
                </asp:DropDownList></td>
            <td ><b>Select By User:</b><br />
            <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="true"></asp:DropDownList>
            </td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td colspan="2"><hr /></td></tr>
        <tr><asp:DataGrid ID="dgInvoiceOnlyList" runat="server" AllowPaging="true" PageSize="20" AutoGenerateColumns="false" Width="100%" PagerStyle-Mode="NumericPages" PagerStyle-Position="Top"
                 >
            <Columns>
                <asp:HyperLinkColumn 
                    HeaderStyle-Font-Bold="true"
                    HeaderText="Request" 
                    DataTextField="InvoiceOnlyID"
                    DataNavigateUrlFormatString="ItemUsedChangeOrder.aspx?reqId={0}"
                    DataNavigateUrlField="InvoiceOnlyID"
                    Target="_blank" >
                    <ItemStyle CssClass="itemlistitem" />
                </asp:HyperLinkColumn>
                    <asp:TemplateColumn>
                    <HeaderTemplate>
                        <b>Vendor</b>
                    </HeaderTemplate>
                    <ItemStyle CssClass="itemlistitem" />
                    <ItemTemplate>
                        <asp:Label ID="Label2" Text='<%# DataBinder.Eval(Container.DataItem, "Vendor")%>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn >

                     <HeaderTemplate>
                        <b>Created By</b>
                    </HeaderTemplate>
                    <ItemStyle CssClass="itemlistitem" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" Text='<%# DataBinder.Eval(Container.DataItem, "UserName")%>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                                <asp:TemplateColumn >

                     <HeaderTemplate>
                        <b>Request Status</b>
                    </HeaderTemplate>
                    <ItemStyle CssClass="itemlistitem" />
                    <ItemTemplate>
                        <asp:Label ID="Label1" Text='<%# DataBinder.Eval(Container.DataItem, "Status")%>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>

            </Columns>
            
            </asp:DataGrid></tr>
    </table>
</asp:Content>

