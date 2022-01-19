<%@ Page Language="VB" MasterPageFile="~/Site/StEPMMWeb.master" MaintainScrollPositionOnPostback="true" AutoEventWireup="false" CodeFile="SiteLinksEdit.aspx.vb" Inherits="Site_SiteLinksEdit" title="Materials Management - Site Left Navigation Editor" %>
<%@ Register TagPrefix="MMM" Namespace="Web.UI.MiscControls"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="../script/client.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- The following table is for administration edit mode only -->
<table cellpadding="0" cellspacing="0"><tr ><td colspan="2" class="menuhead" width="615px">Links Editor</td></tr><tr><td >
<asp:Button ID="btnAdd" runat="server" Text="Add Link" CssClass="button" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel Pending Changes" CssClass="button" />
            <asp:Button ID="btnCommit" runat="server" Text="Commit" CssClass="button" 
                   /></td><td valign="top"  align="right">
    <MMM:HelpButton id="btnHelp" runat="server" DisplayDocument="Help/LinkEditorHelp.pdf"></MMM:HelpButton></td></tr></table>
        
            <asp:DataList ID="dlLinksEdit" runat="server" 
             OnEditCommand="Edit_Command" 
             OnCancelCommand="Cancel_Command" 
             OnUpdateCommand="Update_Command"
             OnDeleteCommand="Delete_Command"
             DataKeyField="WebLink_ID" Width="613px"   >
            <HeaderStyle CssClass="leftNavEdit" />
            <HeaderTemplate><table width="613px"><tr style="width:100%;"><td width="100%" align="center" style="font-size:small;"><strong>PMM Support Links</strong></td></tr><tr><td><hr /></td></tr></table></HeaderTemplate>
            <ItemStyle CssClass="leftNavEdit"   />

            <ItemTemplate><tr style="width:100%;">
                <td class="leftNavEdit">• <strong><%#HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "LinkText"))%></strong></td></tr>
                <td class="leftNavEdit"><strong>Navigate to:</strong> <asp:label ID="lblNavURL" runat="server" Text='<%#HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "NavURL"))%>'></asp:label></td></tr>
                <td class="leftNavEdit"><strong>Target:</strong> <asp:label ID="lblTarget" runat="server" Text='<%#HttpUtility.HtmlEncode(IIf((DataBinder.Eval(Container.DataItem, "Target") = "_blank"), "New Window", "Same Window"))%>'></asp:label>
                    &nbsp;<strong>List Order:</strong>&nbsp;<asp:label ID="lblListOrder" runat="server" Text='<%#HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "ListOrder"))%>'></asp:label></td></tr>
                <tr style="width:613px;"><td width="613px" class="leftNavEdit"><asp:LinkButton Text="Edit" CommandName="Edit" runat="server" ID="edit" />
      <asp:LinkButton Text="Delete" CommandName="Delete" Runat="server" ID="delete" /></td></tr>
                <tr style="width:613px;"><td class="leftNavEdit"><hr /></td></tr></ItemTemplate>
            <EditItemStyle CssClass="itemlistedit" />
            <EditItemTemplate><table>
            <tr class="itemlistedit"><td width="65px" align="right"><strong>Link Text:</strong></td><td align="left"><asp:TextBox ID="txtLinkText" Width="475px" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "LinkText")%>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvLinkText" runat="server" ControlToValidate="txtLinkText" CssClass="errortext" ToolTip="Link Text Required!" SetFocusOnError="true" ErrorMessage="*"></asp:RequiredFieldValidator></td></tr>
            <tr class="itemlistedit"><td width="65px" align="right"><strong>Nav URL:</strong></td><td align="left"><asp:TextBox ID="txtUrl" Width="475px" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NavURL")%>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUrl" runat="server" ControlToValidate="txtUrl" CssClass="errortext" ToolTip="Navigate URL Required!" SetFocusOnError="true" ErrorMessage="*"></asp:RequiredFieldValidator></td></tr>
            </table>
            <table  cellpadding="0" cellspacing="0">
            <tr class="itemlistedit"><td style="display:none;"><asp:label ID="lblTarget" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Target")%>'></asp:label></td><td width="68px" align="right">&nbsp;&nbsp;&nbsp;<strong>Target:&nbsp;</strong></td>
                <td align="left"><asp:DropDownList ID="ddlTarget" runat="server" Width="115px" ><asp:ListItem Text="Same Window" Value="_self"></asp:ListItem>
                    <asp:ListItem Text="New Window" Value="_blank"></asp:ListItem></asp:DropDownList></td><td align="right"><strong>List Order:&nbsp;</strong></td>
                    <td style="display:none;"><asp:Label ID="lblListOrder" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ListOrder")%>'></asp:Label></td><td><asp:DropDownList ID="ddlListOrder" runat="server"></asp:DropDownList></td></tr>
            <tr class="itemlistedit"><td width="65px" colspan="4"><asp:LinkButton CausesValidation="false" Text="Cancel" CommandName="Cancel" Runat="server" ID="cancel" />&nbsp;<asp:LinkButton Text="Update" CommandName="Update" 
                      Runat="server" ID="update" /></tr></td>
            </table><hr /></EditItemTemplate>
            </asp:DataList>
           <table width="100%" cellpadding="0" cellspacing="0">
            <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="errortext"></asp:Label></td></tr></table>
            
   </asp:Content>