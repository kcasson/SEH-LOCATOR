<%@ Page Language="VB" MasterPageFile="~/StEMM.master" AutoEventWireup="false" CodeFile="pwReset.aspx.vb" Inherits="pwReset" title="PMM Password Reset" %>
<%@ Register TagPrefix="MMA" Namespace="Web.UI.MiscControls"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="script/client.js" type="text/javascript"></script>
    <script src="script/pwr.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="90%"><tr><td></td></tr><tr><td class="menuhead">PMM Password Reset</td><td>
    

            </td></tr><tr><td align="right" valign="top" height="25px">
            <MMA:HelpButton id="btnHelp" runat="server" 
                DisplayDocument="Site/Help/PMMWebPasswordResets.pdf" Height="20px"></MMA:HelpButton></td></tr></table><table><tr class="itemlisthead"><td width="400px">Select User:</td><td>User/Name Search:</td></tr><tr>
        <td valign="top" >
        <asp:ListBox width="400px" runat="server" style="width:400px;" 
                ID="ddlUser" Rows="12"></asp:ListBox></td>
            <td valign="top">
                <table><tr><td valign="top"><input type="text" onkeyup="findByname('ctl00_ContentPlaceHolder1_ddlUser','ctl00_ContentPlaceHolder1_InitControl');" id="InitControl" tagname="text" runat="server" size="35" /></td></tr><tr><td class="itemlistitem">
                    &nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td style="display:none;"><asp:Button id="btnReset" runat="server" /></td></tr>
                 <tr><td></td></tr></table></td></tr>
         </table>
         <table><tr><td width="400px" align="right"><input id="Submit" onclick="submitForm();" type="button" value="Reset User Password" class="button" /></td></tr></table><table><tr><td>&nbsp;</td></tr><tr><td><asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="errortext"></asp:Label></td></tr></table><table><tr style="display:none;"><td><input type="hidden" id="hboxText" /><input type="hidden" id="hboxTime" /></td></tr></table>
    </asp:Content>

