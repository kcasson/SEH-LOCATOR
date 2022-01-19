<%@ Page Language="VB" MasterPageFile="~/StEMM.master" AutoEventWireup="false" CodeFile="PMMItems.aspx.vb" Inherits="PMMItemEdit" title="PMM Item Edit"  %>
<%@ register TagPrefix="MMUC" TagName="PageLoadingDisplay" Src="~/Controls/PageLoadingDisplay.ascx"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="script/client.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ReqInput()
            {
            if ((document.getElementById("ctl00_ContentPlaceHolder1_txtdesc").value == "") && (document.getElementById("ctl00_ContentPlaceHolder1_txtMfgNumber").value == "") && (document.getElementById("ctl00_ContentPlaceHolder1_InitControl").value == ""))
            {
                alert("Please enter a valid search string, one required!");
                document.getElementById("ctl00_ContentPlaceHolder1_InitControl").focus();
                return false;
            }
            else
            {
                document.getElementById("ctl00_ContentPlaceHolder1_btnSubmit").click();
                return true;
               
            }
        }
    </script>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="90%" cellpadding="0" cellspacing="0">
    <tr><td class="menuhead">PMM Item Description Edit</td></tr></table>
    <table cellpadding="0" cellspacing="0"><tr><td>&nbsp;</td></tr><tr class="lineitem"><td align="right">Item Number:&nbsp;</td><td>
        <input onkeypress=" if (event.keyCode == 13){if(ReqInput()==true){ShowLoading('tblResults');document.getElementById('ctl00_ContentPlaceHolder1_btnSubmit').click();};};" 
            id="InitControl" runat="server" type="text" tagname="text" maxlength="15" /></td></tr><tr class="lineitem"><td align="right">Mfr Number:&nbsp;</td><td><input onkeypress=" if (event.keyCode == 13){if(ReqInput()==true){ShowLoading('tblResults');document.getElementById('ctl00_ContentPlaceHolder1_btnSubmit').click();};};"  id="txtMfgNumber" runat="server" /></td><td style="display:none;"><asp:Button ID="btnSubmit" runat="server" /></td></tr>
             <tr class="lineitem"><td align="right">Desc:&nbsp;</td><td><input id="txtdesc" onkeypress=" if (event.keyCode == 13){if(ReqInput()==true){ShowLoading('tblResults');document.getElementById('ctl00_ContentPlaceHolder1_btnSubmit').click();};};"  style="width:300px;" runat="server" /></td></tr><tr><td align="right" colspan="2"><input type="button" id="btnSearch"  onclick="if(ReqInput()==true){ShowLoading('tblResults');};" 
             class="button"  value="Search" /></td></tr></table>
    <table width="90%"><tr><td><hr /></td></tr></table><MMUC:PageLoadingDisplay ID="plDisplay" runat="server" />
    <table width="90%" id="tblResults"><tr><td style="margin-left: 40px">
        <asp:datagrid id="dgItemEdit" Runat="server" Width="90%" PageSize="20" DataKeyField="ITEM_ID" ShowFooter="true"
				ShowHeader="true" AutoGenerateColumns="False" GridLines="None" AllowPaging="true" AllowSorting="true"
				PagerStyle-Position="Top" PagerStyle-Mode="NumericPages">
				<SelectedItemStyle BackColor="LightGoldenrodYellow"></SelectedItemStyle>
				<AlternatingItemStyle CssClass="itemlistitemalt" VerticalAlign="Middle"></AlternatingItemStyle>
				<ItemStyle  CssClass="itemlistitem"   VerticalAlign="Middle"></ItemStyle>
				<Columns>
					<asp:TemplateColumn>
						<HeaderStyle  ></HeaderStyle>
						<HeaderTemplate>
							<table cellspacing="1" cellpadding="1" width="100%" border="0" class="listhead">
								<tr>
									<td align="left" valign="top" width="7%">
										<asp:LinkButton ForeColor="White" id="lkItemNo" onclick="dgSort_Click" Runat="server" text="Item No"
											CommandArgument="ITEM_NO"></asp:LinkButton></td>
									<td align="left" valign="top" width="11%">
										<asp:LinkButton ForeColor="White" id="lkMfrNo" onclick="dgSort_Click" Runat="server" text="Mfr No"
											CommandArgument="CTLG_NO"></asp:LinkButton></td>
									<td align="left" valign="top" width="45%">
										<asp:LinkButton ForeColor="White" id="LkDescr" onclick="dgSort_Click" Runat="server" text="Description"
											CommandArgument="DESCR"></asp:LinkButton></td>		
									<td align="left" valign="top" width="40%">
										<asp:LinkButton ForeColor="White" id="lkAltDesc" onclick="dgSort_Click" Runat="server" text="Alt Desc 1"
											CommandArgument="DESCR1"></asp:LinkButton></td>

								</tr>
							</table>
						</HeaderTemplate>
						<ItemTemplate>
							<table cellspacing="0" cellpadding="0" width="90%" border="0">
								<tr>
									<td title="PMM Item No" valign="bottom" align="left" width="8%">
										<asp:HyperLink id="HyperLink1" Runat="server" text='<%# HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "ITEM_NO"))%>'  NavigateUrl='<%#GetNavigateUrl(DataBinder.Eval(Container.DataItem, "ITEM_ID"),DataBinder.Eval(Container.DataItem, "ITEM_IDB")) %>' ></asp:HyperLink></td>
										
									<td  title="MfrNo" valign="bottom" align="left" width="12%"><%#HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "CTLG_NO"))%></td>
									<td  title="Description" valign="bottom" align="left" width="50%"><%#HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "DESCR"))%></td>
									<td  title="Altdesc1" valign="bottom" align="left" width="40%"><%#HttpUtility.HtmlEncode(DataBinder.Eval(Container.DataItem, "DESCR1"))%></td>
								</tr>
							</table>
						</ItemTemplate>
						<FooterStyle></FooterStyle>
					</asp:TemplateColumn>
				</Columns>
				<PagerStyle CssClass="itemlistitem" VerticalAlign="Middle" HorizontalAlign="Right" Position="Top" PageButtonCount="5"
					 Mode="NumericPages"></PagerStyle>
			</asp:datagrid>
    </td></tr></table>
    

</asp:Content>


