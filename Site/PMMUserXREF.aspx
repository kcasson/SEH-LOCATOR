<%@ Page Language="VB" MasterPageFile="~/Site/StEPMMWeb.master" AutoEventWireup="false" CodeFile="PMMUserXREF.aspx.vb" Inherits="Site_PMMUserXREF" title="User ID/Name - Cross Reference" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="../script/client.js" type="text/javascript"></script>
    <script src="../script/pwr.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="700px">
    <tr class="menuhead"><td colspan="2" width="100%">PMM User ID/Name Cross Reference</td></tr>
    <tr><td colspan="2"></td></tr>
</table>
<table>
    <tr class="itemlisthead"><td valign="top">Users:<br />
        <asp:ListBox ID="ddlUser" Rows="22" 
            runat="server" Width="457px" ></asp:ListBox></td><td class="itemlistitem" 
            valign="top"><strong>User ID/Name Search: </strong><br /><input type="text"  
                id="InitControl" size="20" runat="server" onkeyup="findByname('ctl00_ContentPlaceHolder1_ddlUser','ctl00_ContentPlaceHolder1_InitControl')"/></td></tr>
                
    
</table>

</asp:Content>

