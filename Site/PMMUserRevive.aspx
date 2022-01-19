<%@ Page Language="VB" MasterPageFile="~/Site/StEPMMWeb.master" AutoEventWireup="false" CodeFile="PMMUserRevive.aspx.vb" Inherits="pwReset" title="PMM Password Reset" %>
<%@ Register TagPrefix="MMA" Namespace="Web.UI.MiscControls"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../script/client.js" type="text/javascript"></script>
    <script src="../script/pwr.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            height: 34px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="700px"><tr><td class="menuhead">PMM Users</td><td>
            </td></tr></table>
    <table><tr class="itemlisthead"><td width="400px">Active Users:</td><td></td></tr></table>
    <table class="framestyle" style="border-style: solid"><tr>
        <td valign="top">
        <asp:ListBox width="400px" runat="server" 
                ID="ddlUser" Rows="12"></asp:ListBox></td>
            <td valign="top">
                <table><tr><td valign="top"><input type="text" onkeyup="findByname('ctl00_ContentPlaceHolder1_ddlUser','ctl00_ContentPlaceHolder1_txtSrch');" id="txtSrch" tagname="text" runat="server" size="35" /></td></tr><tr><td class="itemlistitem">
                    &nbsp;</td></tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr><td>&nbsp;</td></tr><tr><td style="display:none;"><asp:Button id="btnReset" runat="server" /></td></tr>
                 <tr><td align="right" class="style1"><input id="Submit" onclick="submitForm();" type="button" onkeyup="findByname();" value="Reset User Password" class="button" /></td></tr></table></td></tr>
         </table>
         <table><tr><td class="itemlisthead">Inactive Users:</td></tr></table><table class="framestyle" style="border-style: solid"><tr><td>
         <asp:ListBox ID="lbInactiveUsers" runat="server" Width="400px" Rows="8"></asp:ListBox></td><td valign="top">&nbsp; <input id="InitControl" tagname="text" onkeyup="findByname('ctl00_ContentPlaceHolder1_lbInactiveUsers','ctl00_ContentPlaceHolder1_InitControl');" type="text" runat="server" size="35" /></td></tr>
         <tr><td align="right" class="itemlisthead" style="display: none;"><asp:Button id="btnReviveSub" runat="server" /></td><td></td><td align="right"><input type="button" ID="btnRevive" onclick="reviveUserSubmit();" value="Revive Selected User" class="button" runat="server" /></td></tr>
         </table><table><tr class="itemlisthead"><td><asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="errortext"></asp:Label></td></tr></table>
    </asp:Content>

