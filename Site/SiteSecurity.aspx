<%@ Page Language="VB" MasterPageFile="~/Site/StEPMMWeb.master" AutoEventWireup="false" CodeFile="SiteSecurity.aspx.vb" Buffer="true" Inherits="Site_SiteSecurity" title="Site Security" %>
<%@ Register Namespace="Web.UI.MiscControls" TagPrefix="MMC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script src="../script/client.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        //Toggle user add window
        function addUser(o) {
            var usr = document.getElementById("ctl00_ContentPlaceHolder1_tblUserInfo");
            var nwu = document.getElementById("ctl00_ContentPlaceHolder1_tblAddUser");
            var txt = document.getElementById("txtNetworkID");
            if (o == "add") {
                usr.style.display = "none";
                nwu.style.display = "block";
                txt.focus();
            }
            else {
                usr.style.display = "block";
                nwu.style.display = "none";
            }

        }

        function btnAddLookup_click() {
            var usr = document.getElementById("ctl00_ContentPlaceHolder1_tblUserInfo");
            var nwu = document.getElementById("ctl00_ContentPlaceHolder1_tblAddUser");
            var txt = document.getElementById("txtNetworkID");
            var txth = document.getElementById("ctl00_ContentPlaceHolder1_txtHidden");
            var btn = document.getElementById("ctl00_ContentPlaceHolder1_btnAddUserWork");
            var rst = document.getElementById("trResults");
            var wtg = document.getElementById("trWaiting");
            txth.value = txt.value;
            if (txt.value != "") {
                btn.click();
                txt.select();
                wtg.style.display = "block";
                rst.style.display = "none";
                usr.style.display = "none";
                nwu.style.display = "block";
                
            } else {
                alert("Please enter a name to lookup!");
                txt.focus();
            }
        }

        function Menu_HoverDynamic(item) {
            var node = (item.tagName.toLowerCase() == "td") ?
        item :
        item.cells[0];
            var data = Menu_GetData(item);
            if (!data) return;
            var nodeTable = WebForm_GetElementByTagName(node, "table");
            if (data.hoverClass) {
                nodeTable.hoverClass = data.hoverClass;
                WebForm_AppendToClassName(nodeTable, data.hoverClass);
            }
            node = nodeTable.rows[0].cells[0].childNodes[0];
            if (data.hoverHyperLinkClass) {
                node.hoverHyperLinkClass = data.hoverHyperLinkClass;
                WebForm_AppendToClassName(node, data.hoverHyperLinkClass);
            }
            if (data.disappearAfter >= 200) {
                __disappearAfter = data.disappearAfter;
            }
            Menu_Expand(node, data.horizontalOffset, data.verticalOffset);
        }

        function btnCancel_Click() {
            var btn = document.getElementById("ctl00_ContentPlaceHolder1_btnSvrCancel");
            btn.click();
        }

        function iDelete_Click() {
            var btn = document.getElementById("ctl00_ContentPlaceHolder1_btnDelete");

            if (confirm("This will remove the user an all associated security, continue?") == true) {
                btn.click();
            }
        }
        
    </script>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="600px" cellpadding="0" cellspacing="0"><tr align="center"><td><asp:ScriptManager runat="server" ID="sm1"></asp:ScriptManager>
    <table width="600px" cellpadding="0" cellspacing="0" id="tblUserInfo" runat="server">
        <tr align="left">
            <td><table width="600px">
                <tr><td colspan="3"><asp:Label Font-Size="Smaller" ID="lblMessage" runat="server" 
                        CssClass="errortext" EnableViewState="False"></asp:Label></td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr align="right" class="itemlisthead"><td valign="top"><b>User:&nbsp;</b></td><td align="left"><asp:DropDownList  AutoPostBack="true" Width="150px" ID="ddlUser" runat="server"></asp:DropDownList>&nbsp;
                    <input type="button" id="btnAddUser" value="Add User" onclick="addUser('add')" class="button" Width="60px" Height="18px" Font-Size="Smaller"/>&nbsp;
                    <input type="button" onclick="iDelete_Click();" id="iDelete" value="Delete Selected User" class="button" /></td><td style="display:none;"><asp:Button ID="btnDelete" runat="server" /></td></tr>
                <tr><td align="right" class="itemlisthead"><b>Page:&nbsp;</b></td><td><asp:DropDownList runat="server" ID="ddlPages" AutoPostBack="true" Width="250px"></asp:DropDownList></td></tr>
                <tr><td>&nbsp;</td></tr>
                <tr class="headLabelMedium"><td colspan="2"><b>Selected Item Information</b></td></tr>
                </table>
                <table align="center" style="border-style: groove"><tr><td><asp:Panel GroupingText="User Data" runat="server" ID="pnl" Width="600px">
                <table width="100%"  cellpadding="0" cellspacing="0" >
                <tr class="itemlistitem"><td width="50%" height="20px" ><b>Network Id:</b><br />
                    <asp:Label ID="lblADNetworkId" runat="server" Text="&nbsp;"></asp:Label></td></tr>
                    <tr class="itemlistitem"><td  width="90px" height="20px"><nobr><b>Name:</b><br />
                    <asp:Label ID="lblADName" runat="server"></asp:Label></nobr></td><td><b>Phone:</b><br />
                    <asp:Label ID="lblADPhone" runat="server"></asp:Label></td></tr>
                <tr class="itemlistitem"><td  height="20px" ><b>Title:</b><br />
                    <asp:Label ID="lblADTitle" runat="server"></asp:Label></td><td><b>email:</b><br />
                    <asp:Label ID="lblADemail" runat="server"></asp:Label></td></tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr class="menuhead"><td colspan="4"><b>Invoice Only Requests</b></td></tr>
                    
                    <tr><td colspan="4">
                        <table cellpadding="0" cellspacing="0" width="100%" class="itemlistitem">
                            <tr align="center"><td class="noDisplay"><asp:CheckBox ID="chkIOApprover" runat="server" Text="Invoice Only Approver" 
                                
                                    ToolTip="Has the authority to approve Invoice Only requests. This user will be sent an email when these requests are submitted." 
                                    AutoPostBack="True" /></td>
                            <td><asp:CheckBox ID="chkIOBuilder" runat="server" Text="Item Builder" 
                                
                                    ToolTip="User will be notified by email when request is submitted that requires an item build. Has the authority to mark request as complete." 
                                    AutoPostBack="True" /></td>
                            <td><asp:CheckBox ID="chkIONotify" runat="server" Text="Notify for Completion" 
                                
                                    ToolTip="User will be notified by email when request is submitted and approved. Has the authority to mark request as complete." 
                                    AutoPostBack="True" /></td></tr>
                            </table>
                        </td></tr>
                        </table>
                        <tr><td>&nbsp;</td></tr> 
                     </asp:Panel></td></tr><tr align="center"><td>
                <table  cellpadding="0" cellspacing="0"><tr><td>&nbsp;</td></tr>
                <tr class="itemlisthead"><td><b>Authorized:</b></td><td></td><td><b>Available:</b></td></tr>
                <tr><td width="250"><asp:ListBox ID="ddlAuthorization" runat="server" Rows="10" 
                        SelectionMode="Multiple" Width="250" ></asp:ListBox></td><td width="40px" align="center" valign="middle">
                        <asp:Button ID="btnAddPage" runat="server" Text="<--" CssClass="button" />
                <br /><br /><asp:button runat="server" ID="btnDeletePage" Text="-->" CssClass="button" /></td><td><asp:ListBox Width="250" runat="server" ID="ddlAvailable" SelectionMode="Multiple" Rows="10"></asp:ListBox></td></tr>
           </table>
           </td></tr><tr><td>&nbsp;</td></tr>
           <tr class="itemlistitemsmall" align="left"><td><b>Select application(s) from available or authorized and use arrow buttons to add or remove access.</b></td></tr>
    </table></td></tr></table></td></tr></table>

    <table width="500px" cellpadding="0" cellspacing="0" id="tblAddUser" height="400px" style="display:none;" border="1" runat="server" >
    <tr align="left" valign="top" style="background-color:#CCCCCC"><td>
     <table width="500px" cellpadding="0" cellspacing="0">
        <tr align="center"><td class="headLabelMedium" colspan="2">Add User</td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr><td align="right">Name:&nbsp;</td><td>&nbsp;<input type="text" id="txtNetworkID" onkeydown="if (event.keyCode == 13){var btn = document.getElementById('ctl00_ContentPlaceHolder1_btnAddLookup');btn.click();};"/>&nbsp;
        <input type="button" id="btnAddLookup" onclick="btnAddLookup_click();" runat="server" value="Lookup User" class="button" Width="90px" Height="20px" style="font-size:smaller;" />&nbsp;<input type="button" id="btnCancel" class="button" value="Cancel" onclick="btnCancel_Click();" /></td><td style="display:none;">
        <asp:Button ID="btnSvrCancel" runat="server" /></td><td style="display:none;"><asp:TextBox ID="txtHidden" runat="server"></asp:TextBox></td>
            <td style="display:none;"><asp:Button ID="btnAddUserWork" runat="server" /></td></tr>
            <tr><td class="itemlistheadsmall" colspan="3">&nbsp;&nbsp;&nbsp;&nbsp;Enter all or part of the users name.</td></tr>
        <tr><td colspan="2"><hr /></td></tr>
        <tr height="200px"  id="trWaiting" style="display:none; background-color:#CCCCCC;"><td colspan="2" align="center"><MMC:ShowWaiting ID="imgWaiting" runat="server" /><br /><b>Searching Active Directory . . .</b></td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr id="trResults"><td colspan="4"><asp:Label ID="lblLuResults" runat="server"></asp:Label></td></tr>
        </table>
    </td></tr></table>
</asp:Content>

