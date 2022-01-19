<%@ Page Title="" Language="VB" MasterPageFile="~/StEMMNoLeftNav.master" AutoEventWireup="false" CodeFile="GenericErrorPage.aspx.vb" Inherits="Site_GenericErrorPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="script/client.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr><td>&nbsp;</td></tr>
        <tr><td style="font-weight:bolder;"><span class="errortext" style="font-size:small;"
            title="" id="LoadFocus">An unhandled exception has occurred and has been logged. <br>Please try loading the page again after a few minutes or contact the email below.</span></td></tr>
            <tr><td><asp:Label ID="lblSupporteMail" runat="server" CssClass="headLabelMedium" Font-Size="Small"></asp:Label></td></tr>
            <tr><td>&nbsp;</td></tr>
            <tr><td><asp:Label ID="lblTimeStamp" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label></td></tr></table>

</asp:Content>







