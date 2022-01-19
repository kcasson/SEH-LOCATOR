<%@ Page Title="" Language="VB" MasterPageFile="~/Site/StEPMMWeb.master" AutoEventWireup="false" CodeFile="GenericErrorPage.aspx.vb" Inherits="Site_GenericErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr><td>&nbsp;</td></tr>
        <tr><td><asp:Label runat="server" CssClass="errortext" Font-Size="Small"
            Text="An unhandled exception has occurred and has been logged, please contact PMM Support." ID="lblErrorText"></asp:Label></td></tr>
            <tr><td><asp:Label ID="lblSupporteMail" runat="server" CssClass="headLabelMedium" Font-Size="Small"></asp:Label></td></tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td><asp:Label ID="lblTimeStamp" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label></td></tr></table>

</asp:Content>

