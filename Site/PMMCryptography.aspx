<%@ Page Language="VB" MasterPageFile="~/Site/StEPMMWeb.master" AutoEventWireup="false" CodeFile="PMMCryptography.aspx.vb" Inherits="Site_PMMCryptography" title="PMM Cryptography" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../script/client.js" type="text/javascript"></script>
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="font-size:small;">
    <asp:Panel ID="pnl1" runat="server" GroupingText="User Information" width="600px">
        <br />
        <span ><b>Select User:</b></span>
        <br />
        <asp:DropDownList ID="ddlUser" runat="server" AutoPostBack="true"></asp:DropDownList>
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh List" CssClass="button" />
        <br />
        <asp:TextBox ID="txtSearch" runat="server" ></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="User Search" CssClass="button" />
        <br />
        <asp:Panel ID="pnl2" runat="server" GroupingText="Password Information">
        <br />
        <b>Users Password:</b>
        <br />
        <asp:Label ID="lblPassword" runat="server" EnableViewState="false"></asp:Label>
        <br />
        <br />
        <b>Users Encrypted Password:</b>
        <br />
        <asp:Label ID="lblEncPassword" runat="server"></asp:Label>
        <br />
        <asp:Label ID="lblDisplay" CssClass="errortext" runat="server" ></asp:Label>
        <br />
        </asp:Panel>
        <br />
        <b>Note:</b> System admin users are not available.
     </asp:Panel>
    </div>
</asp:Content>

