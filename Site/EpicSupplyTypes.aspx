<%@ Page Language="VB" MasterPageFile="~/Site/StEMMNoLeftNav.master"AutoEventWireup="false" CodeFile="EpicSupplyTypes.aspx.vb" Inherits="Site_PMMSupplyTypes" title="Epic Item Master Supply Types" %>
<%@ Register TagPrefix="MMC" Namespace="Web.UI.MiscControls" %>
<%@ Register TagPrefix="MML" TagName="PageLoadingDisplay" Src="~/Controls/PageLoadingDisplay.ascx"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../script/client.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    
        //page script, Kenneth Casson 11/17/2009
        
		function Click(e, buttonid)
		{ 
		var bt = document.getElementById(buttonid); 
		if (typeof bt == 'object')
			{ 
				if (event.keyCode == 13)
				{ 
					bt.click();
					return false; 
				} 
			} 
		} 
				
		function checkRequired(btn)
		{
		    var txt = document.getElementById("ctl00_ContentPlaceHolder1_InitControl");
		    var msg = document.getElementById("ctl00_ContentPlaceHolder1_lblMessage");
		    if (txt.value != '')
		    {
		        Submit(btn);
		        //Clear any messages
                msg.value = '';
		    }
		    else
		    {
		        alert('Please enter a search value!');
		        txt.focus();
		    }
		}
		
		function Submit(btn)
		{
		    var btn = document.getElementById(btn);
		    var res = document.getElementById("tblResults");
		    var wrk = document.getElementById("ctl00_ContentPlaceHolder1_tblWorking");
		     
	        btn.click();
	        res.style.display = "none";
	        wrk.style.display = "block";
	        return false;	   
		       
		}
    
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       <table width="800" cellpadding="0" cellspacing="0" style="height:750px; vertical-align:top;" ><tr valign="top"><td>&nbsp;</td><td>&nbsp;</td><td>
        <table  cellpadding="0" cellspacing="0" width="725">
            <tr><td>&nbsp;</td></tr>
            <tr><td class="itemlisthead" align="right" width="100px">PMM/Mfr Item:&nbsp;</td>
                    <td width="120px"><input type="text" id="InitControl" tagname="text" runat="server" onkeydown="return Click(event,'btnSearch');" /></td>
                    <td>&nbsp;<input type="button" class="button" value="Search" id="btnSearch" onclick="return checkRequired('ctl00_ContentPlaceHolder1_btnSubmit');" />&nbsp;<input type="button" ID="btnClear"
                         value="Clear Results" class="button" onclick="Submit('ctl00_ContentPlaceHolder1_btnSubmitClear')" /></td>
                    <td style="display:none;"><asp:button id="btnSubmit"  runat="server"  text="Search" class="button" /><asp:Button ID="btnSubmitClear" runat="server" /></td>
                    <td align="right" class="itemlisthead">Items Per Page:&nbsp;<asp:DropDownList ID="ddlItemsPerPage" runat="server" AutoPostBack="true" Width="50px"></asp:DropDownList></td></tr>
           </table>
            <table width="800" cellpadding="0" cellspacing="0">
            <tr><td width="800" ><hr /></td></tr>
            <tr><td width="800"  class="headLabelMedium">&nbsp;<asp:label ID="lblResults" runat="server" Text="Items Requiring Supply Type"></asp:label><br /><hr /></td></tr>
            <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="errortext" Font-Size="XX-Small" EnableViewState="false"></asp:Label></td></tr>
                     <tr><td colspan="3"><table id="tblWorking" runat="server" style="display:none;width:90%;">
                <tr style="width:85%; height:350px;" ><td align="center">
                <asp:Image runat="server" ImageUrl="~/Images/Loading.gif" height="40" width="40" /><br /><br /><b>Working&nbsp;&nbsp;&nbsp;.&nbsp;&nbsp;&nbsp;.&nbsp;&nbsp;&nbsp;. </b></td>
               </tr>
                </table>
                <table  id="tblResults"><tr><td>
                <asp:DataGrid ID="dlItems" runat="server" AllowPaging="true" AutoGenerateColumns="false" PagerStyle-Mode="NumericPages" PageSize="10">
                    <HeaderStyle CssClass="itemlistheadsmall" />
                    <PagerStyle Mode="NumericPages" Position="Top" HorizontalAlign="Right" PageButtonCount="5" VerticalAlign="Middle" />
                        <Columns>
                            <asp:TemplateColumn>
                                 <HeaderTemplate><table cellpadding="0" cellspacing="0" width="800" border="1"><tr valign="bottom"><td width="45">&nbsp;</td><td width="40px">Item</td><td>Catalog #</td>
                                    <td width="258px">Description</td><td width="105px">Item Type</td>
                                    <td width="160px" >Supply Type</td><td width="95px">Item Type<br />Last Update</td><td width="90px">Supply Type<br />Last Update</td></tr>
                                 </HeaderTemplate>
                                 <ItemTemplate>
                                    <tr style="border:1;" class="itemlistitemsmall" runat="server" id="trItem"><td><asp:Button ID="btnUpdate" CssClass="button" Width="40px" Height="18px" Font-Size="X-Small" Text="Update" runat="server" /></td>
                                    <td width="45px"><%#DataBinder.Eval(Container.DataItem, "ITEM_NO")%></td>
                                    <td><nobr><%#DataBinder.Eval(Container.DataItem, "CTLG_NO")%></nobr></td>
                                    <td width="385px"><%#DataBinder.Eval(Container.DataItem, "DESCR")%></td><td>
                                    <MMC:DataListDropDown Width="105px" ID="ddlItemTypes" CssClass="itemlistheadsmall"
                                       runat="server"></MMC:DataListDropDown></td>
                                    <td width="165"><MMC:DataListDropDown Width="165px" ID="ddlTypes" CssClass="itemlistheadsmall"
                                     runat="server"></MMC:DataListDropDown></td>
                                     <td width="140px"><%#"<b>Date:&nbsp;</b>" & GetDisplayDate(DataBinder.Eval(Container.DataItem, "ITEM_REC_MODIFY_DATE")) & "<br /><b>User:&nbsp;</b>" & GetDisplayUser(DataBinder.Eval(Container.DataItem, "ITEM_REC_MODIFY_USR"))%></td>
                                     <td width="140px"><%#"<b>Date:&nbsp;</b>" & GetDisplayDate(DataBinder.Eval(Container.DataItem, "SUPP_REC_MODIFY_DATE")) & "<br /><b>User:&nbsp;</b>" & GetDisplayUser(DataBinder.Eval(Container.DataItem, "SUPP_REC_MODIFY_USR"))%></td>
                                    <td style="display:none;"><asp:Label runat="server" ID="lblTypes" Text='<%#DataBinder.Eval(Container.DataItem, "SUPPLY_TYPE_ID")%>'></asp:Label>
                                    <asp:Label ID="lblItemId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ITEM_ID")%>'></asp:Label>
                                        <asp:Label ID="lblItemIdB" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ITEM_IDB")%>'></asp:Label>
                                        <asp:Label ID="lblItemNo" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "ITEM_NO")%>'></asp:Label>
                                    </td><td style="display:none;">
                                        <asp:Label runat="server" ID="lblItemType" Text='<%#DataBinder.Eval(Container.DataItem, "ITEM_TYPE_ID")%>'></asp:Label>
                                    </td></tr>
                                </ItemTemplate>
                           
                                <FooterTemplate><tr><td colspan="3">&nbsp;</td></tr></table></FooterTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                     </asp:DataGrid>
               </td></tr></table> </td></tr>
        </table></td></tr></table>
 </asp:Content>

