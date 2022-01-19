<%@ Page Language="VB" MasterPageFile="~/StEMM.master" AutoEventWireup="false" CodeFile="PMMItemEdit.aspx.vb" Inherits="PMMItemEdit" title="PMM Item Edit"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script src="script/client.js" type="text/javascript"></script>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="90%" cellpadding="0" cellspacing="0">
    <tr><td class="menuhead">PMM Item Description Edit</td></tr></table>
    <table cellpadding="0" cellspacing="0"><tr><td>&nbsp;</td></tr><tr class="lineitem"><td align="right">Item Number:</td><td>
        <input onkeypress="return ButtonClick(event,'ctl00$ContentPlaceHolder1$btnSubmit','ctl00_ContentPlaceHolder1_tblResults')" 
            id="InitControl" runat="server" type="text" maxlength="15" />
            </td></tr><tr class="lineitem"><td align="right">Mfr Number:</td><td><input id="txtMfgNumber" runat="server" /></td><td style="display:none;"><asp:Button ID="btnSubmit" runat="server" /></td></tr>
             <tr class="lineitem"><td align="right">Desc:</td><td><input id="txtDesc" style="width:300px;" runat="server" /></td><td><input type="button" id="btnSearch"  onclick="RequiredInput('ctl00_ContentPlaceHolder1_InitControl','Please enter a valid search string!','ctl00_ContentPlaceHolder1_tblResults','ctl00$ContentPlaceHolder1$btnSubmit');" 
             class="button"  value="Search" /></td></tr></table>
    <table width="90%"><tr><td><hr /></td></tr></table>
    <table width="90"><tr><td>
        <asp:DataGrid OnEditCommand="dgItemEdit_EditCommand" 
            runat="server" DataKeyField="ItemID"  ID="dgItemEdit" 
            Width="700px" CellPadding="0" EnableTheming="True"><HeaderStyle CssClass="itemlisthead" />
<ItemStyle CssClass="itemlistitem" /><AlternatingItemStyle CssClass="itemlistitemalt" />
<HeaderStyle CssClass="listhead" />
        <Columns>
            <asp:EditCommandColumn CancelText="Cancel" 
                EditText="Edit" UpdateText="Update"></asp:EditCommandColumn>
             <asp:BoundColumn HeaderText="Item Number" ReadOnly="true">
                 <HeaderStyle Width="40px" />
            </asp:BoundColumn>
            
            <asp:BoundColumn DataField="MfrNumber" HeaderText="Mfr Number" ReadOnly="True">
            </asp:BoundColumn>
            <asp:BoundColumn DataField="Descr" HeaderText="Description" ReadOnly="True">
            </asp:BoundColumn>
            <asp:BoundColumn DataField="AltDesc1" HeaderText="Alt Desc 1"></asp:BoundColumn>
            <asp:BoundColumn DataField="AltDesc2" HeaderText="Alt Desc 2"></asp:BoundColumn>
            
        </Columns><EditItemStyle BackColor="Yellow" />
    </asp:DataGrid></td></tr></table>
    
    
</asp:Content>

