<%@ Page Language="VB" MasterPageFile="~/StEMM.master" AutoEventWireup="false" CodeFile="ItemDescriptionEdit.aspx.vb" Inherits="ItemDescriptionEdit" title="PMM Item Description Edit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="StyleSheet.css"  type="text/css" />
<script src="script/client.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">


</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="90%" cellpadding="0" cellspacing="0">
    <tr  ><td class="menuhead">Item Description Edit</td></tr></table>
<table width="90%">
    <tr><td></td></tr>
    <tr><td align="right" width="150px" class="itemlisthead">Item Number: </td>
        <td ><asp:Label ID="lblItemNumber" runat="server"></asp:Label></td></tr>
    <tr><td align="right" width="150px"  class="itemlisthead">Mfr Cat Number: </td>
        <td ><asp:Label ID="lblCatNumber" runat="server"></asp:Label></td></tr>
    <tr><td align="right" width="150px"  class="itemlisthead">Manufacturer: </td>
        <td><asp:Label ID="lblManufacturer" runat="server"></asp:Label></td></tr>
    <tr><td align="right"  width="150px"  class="itemlisthead">Description: </td>
        <td colspan="3" ><asp:Label ID="lblItemDesc" runat="server"></asp:Label></td></tr>
    <tr><td align="right" width="150px"  class="itemlisthead">Alternate Description: </td>
        <td  width="450px">
        <input type="text" id="InitControl" tagname="text" runat="server" size="60" maxlength="255" 
            /></td><td></td></tr></table><table width="550px">
    <tr align="right"><td colspan="2"><asp:Button ID="btnUpdate" class="button" Text="Update" runat="server" /><asp:Button ID="btnCancel" Text="Cancel" class="button" runat="server" /></td></tr></table> 
    <table><tr id="rwError" runat="server" style="display:none;"><td><asp:Label EnableViewState="false" ID="lblErrorMsg" runat="server" CssClass="errortext"></asp:Label></td></tr></table>
  </asp:Content>

