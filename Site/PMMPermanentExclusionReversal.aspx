<%@ Page Title="" Language="VB" MasterPageFile="~/Site/StEMMNoLeftNav.master" AutoEventWireup="false" CodeFile="PMMPermanentExclusionReversal.aspx.vb" Inherits="Site_PermanentExclusionReversal" %>
<%@ Register Namespace="Web.UI.MiscControls" TagPrefix="MMC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../script/client.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function iLoad_Click() {
            var txt = document.getElementById("ctl00_ContentPlaceHolder1_InitControl");
            var btn = document.getElementById("ctl00_ContentPlaceHolder1_btnLoad");
            if (txt.value != "") {
                btn.click();
            }
            else {
                alert("Please enter a purchase order to lookup!");
                txt.focus();
            }
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
<!--<tr><td><MMC:MenuHeadControl ID="mhc" runat="server" ImageText="PMM Permanent Exclusion Reversal"></MMC:MenuHeadControl></td></tr>-->
<tr><td>
<asp:Panel ID="pPO" runat="server" GroupingText="PMM Permanent Exclusion Reversal">
    <table cellpadding="0" cellspacing="0" width="730px">
    <tr><td colspan="3" style="font-size:small;"><asp:Label ID="lblMessage" 
            runat="server" CssClass="errortext" EnableViewState="False" Font-Size="Smaller"></asp:Label></td></tr>
        <tr><td>&nbsp;</td></tr>
                <tr><td colspan="3" align="right"></td></tr>
        <tr class="itemlistheadsmall" ><td><b>PO Number:</b>&nbsp;<asp:TextBox CssClass="itemlistheadsmall" runat="server" ID="InitControl"></asp:TextBox>
            <input type="button"  value="Load PO" class="button" onclick="iLoad_Click();" id="iLoad" /></td><td style="display:none;"><asp:Button ID="btnLoad" runat="server" /></td></tr>
        <tr><td>&nbsp;</td></tr>
    </table>
    <table cellpadding="0" cellspacing="0" width="730px">
    <tr><td class="itemlistheadsmall" colspan="2"><b>PO:</b> <asp:Label ID="lblLoadedPO" runat="server" CssClass="itemlistheadsmall" Width="75px"></asp:Label><asp:Button ID="btnReverse" runat="server" 
                Text="Reverse Selected Lines" CssClass="button" Height="20px" /></td></tr>
        <tr class="itemlistheadsmall"><td width="5px">&nbsp;</td><td>
            <asp:Panel ID="pLines" runat="server" Height="400px" GroupingText="Lines To Reverse">
                <asp:RadioButton ID="rbAllLines" Checked="true" runat="server" Text="Reverse All Lines" Enabled="false" GroupName="POLines" AutoPostBack="true" /><br />
                <asp:RadioButton ID="rbSelectLines" runat="server" Text="Reverse Select Lines" Enabled="false" GroupName="POLines" AutoPostBack="true" />
                <table><tr><td><asp:CheckBoxList ID="chkLines" runat="server" Enabled="false"></asp:CheckBoxList></td></tr></table>
            </asp:Panel>
            </td></tr>
    </table>
    </asp:Panel></td></tr></table>
</asp:Content>

