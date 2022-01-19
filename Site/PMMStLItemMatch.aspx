<%@ Page Language="VB" MasterPageFile="~/Site/StEMMNoLeftNav.master" AutoEventWireup="false" CodeFile="PMMStLItemMatch.aspx.vb" Inherits="Site_PMMStLItemMatch" title="St. Luke to PMM Item Matching" %>
<%@ Register namespace="Web.UI.MiscControls" TagPrefix="MMC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../script/client.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table><tr><td>&nbsp;</td><td>
    <table width="830px" cellspacing="0">
        <tr align="left" class="itemlistheadsmall"><td width="70px"><nobr>Dataset:&nbsp;<asp:DropDownList 
                CssClass="itemlistheadsmall" ID="ddlData" runat="server" Width="200px" 
                AutoPostBack="True"></asp:DropDownList>&nbsp;
        Data Subset:&nbsp;<asp:DropDownList CssClass="itemlistheadsmall" ID="ddlSubset" 
                runat="server" Width="200px" AutoPostBack="True"></asp:DropDownList></nobr></td></tr>
        <tr><td><hr /></td></tr>
        
    </table>
    <table width="830px" cellpadding="0" cellspacing="0">
    <tr><td><asp:Label ID="lblMessage" EnableViewState="false" CssClass="errortext" Font-Size="X-Small" runat="server"></asp:Label></td></tr>
        <tr><td colspan="2">
            <table style="border-style:groove" width="825px" cellpadding="0" cellspacing="0">
                <tr><td>
                    <table width="100%">
                        <tr class="headLabelMedium"><td><asp:Label ID="dsName" runat="server"></asp:Label> Items</td></tr>
                        <tr class="itemlistheadsmall"><td align="center"><asp:Label ID="lblStats" runat="server" CssClass="itemlistheadsmall"></asp:Label></td></tr>
                        <tr><td><hr style="height: -12px" /></td></tr>
                    </table>
                 </td></tr>
                 <tr><td>
                     <table  cellpadding="0" cellspacing="0" width="825px">
                         <tr valign="bottom"><td>
                            <table cellpadding="0" cellspacing="0">
                                <tr class="itemlistheadsmall"><td align="right">Item:&nbsp;</td><td><asp:Textbox CssClass="itemlistheadsmall" ID="txtDsItem" width="120px" runat="server" TabIndex="1"></asp:Textbox></td></tr>
                                <tr class="itemlistheadsmall" ><td align="right">Manufacturer No:&nbsp;</td><td><asp:Textbox  CssClass="itemlistheadsmall" ID="txtDsMfr" width="120px" runat="server" TabIndex="3"></asp:Textbox></td></tr>
                                <tr class="itemlistheadsmall"><td align="right">PMM Item No:&nbsp;</td><td><asp:Textbox CssClass="itemlistheadsmall" ID="txtDsPmmItem" width="120px" runat="server" TabIndex="5"></asp:Textbox></td></tr>
                            </table>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr class="itemlistheadsmall"><td align="right">&nbsp;Catalog No:&nbsp;</td><td><asp:Textbox CssClass="itemlistheadsmall" ID="txtDsCat" width="120px" runat="server" TabIndex="2"></asp:Textbox></td></tr>
                                <tr class="itemlistheadsmall"><td align="right">&nbsp;Vendor No:&nbsp;</td><td><asp:Textbox  CssClass="itemlistheadsmall" ID="txtDsVend" width="120px" runat="server" TabIndex="4"></asp:Textbox></td></tr>
                                <tr class="itemlistheadsmall"><td align="right">&nbsp;Description:&nbsp;</td><td><asp:Textbox CssClass="itemlistheadsmall" ID="txtDsDesc" width="175px" runat="server" TabIndex="6"></asp:Textbox></td></tr>
                            </table>
                        </td>
                        <td >
                            <table cellpadding="0" cellspacing="0">
                                <tr><td><nobr><asp:CheckBox CssClass="itemlistheadsmall" ID="chkMatched" 
                                        runat="server" Text="Matched Items Only" AutoPostBack="True" /></nobr></td></tr>
                                <tr><td><asp:CheckBox  CssClass="itemlistheadsmall" ID="chkPar" runat="server" 
                                        Text="Par Items" ToolTip="Check this for Par Items " AutoPostBack="True" /></td></tr>
                                <tr><td><asp:CheckBox ID="chkReq" CssClass="itemlistheadsmall" runat="server" 
                                        Text="Req Items" ToolTip="Check this for Req Items " AutoPostBack="True" /></td>   </tr>
                            </table>
                        </td>
                        <td align="right" >
                            <table cellpadding="0" cellspacing="0">
                                <tr class="itemlistheadsmall" valign="middle"><td width="100px">
                                    <nobr>Limit Rows To:&nbsp;<asp:DropDownList ID="ddlRows" CssClass="itemlistheadsmall" runat="server" AutoPostBack="true">
                                        <asp:ListItem Value="100" Text="100"></asp:ListItem>
                                        <asp:ListItem Value="500" Text="500"></asp:ListItem>
                                        <asp:ListItem Value="1000" Text="1000"></asp:ListItem>
                                    </asp:DropDownList></nobr>
                                    </td></tr>
                                <tr align="right" valign="bottom"><td><asp:Button ID="btnDsClear" runat="server" 
                                        CssClass="button" Text="Clear" Width="60px" /></td></tr>
                                <tr align="right" valign="bottom"><td><asp:Button ID="btnDsSearch" runat="server" CssClass="button" Text="Search" Width="60px" /></td></tr>
                            </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:DataGrid ID="dgDsItems" runat="server" AllowPaging="true" AutoGenerateColumns="false" Width="825" PagerStyle-Position="Top" PageSize="3" DataKeyField="STL_NBR">

                                    <PagerStyle Mode="NumericPages" Position="Top" HorizontalAlign="Right" PageButtonCount="5" VerticalAlign="Middle"  />
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <ItemTemplate><tr id="trItem" class="itemlistitemsmall" ><td valign="middle"><asp:Button ID="btnSelect" runat="server" CssClass="button" Text="Select" Width="40px" Height="18px" Font-Size="X-Small"/></td><td><%#GetBindingValue(DataBinder.Eval(Container.DataItem, "PMM_NBR"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"STL_NBR"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"STL_MFR"))%></td>
                                                <td><%#GetBindingValue(DataBinder.Eval(Container.DataItem, "STL_CTLG_NO"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem, "PMM_MFR_NO"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"VENDOR"))%></td><td><%#GetBindingValue(DataBinder.Eval(Container.DataItem, "VEND_CAT"))%></td>
                                                <td><%#GetBindingValue(DataBinder.Eval(Container.DataItem, "STL_DESC"))%></td><td><%#GetBindingValue(DataBinder.Eval(Container.DataItem, "PMM_DESC"))%></td></tr>
                                            </ItemTemplate>
                                            <HeaderTemplate><table border="1" width="100%" cellpadding="0" cellspacing="0"><tr class="itemlistheadsmall"><td></td><td>PMM Number</td><td>Stl Number</td><td>Stl Mfr</td>
                                                <td>Stl Ctlg</td><td>PMM Mfr</td><td>Vendor</td><td>Vendor Ctlg</td>
                                                <td>Stl Desc</td><td>PMM Desc</td></tr>
                                            </HeaderTemplate>
                                           <FooterTemplate></table></FooterTemplate>
                                           <EditItemTemplate><tr id="trItem" style="background-color:Yellow;" class="itemlistitemselectsmall"><td><asp:Button ID="btnSelect" runat="server" CssClass="button" Text="Select" Width="40px" Height="18px" Font-Size="X-Small" /></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"PMM_NBR"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"STL_NBR"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"STL_MFR"))%></td>
                                                <td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"STL_CTLG_NO"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem, "PMM_MFR_NO"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"VENDOR"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"VEND_CAT"))%></td>
                                                <td><%#GetBindingValue(DataBinder.Eval(Container.DataItem, "STL_DESC"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"PMM_DESC"))%></td></tr></EditItemTemplate>
                                        </asp:TemplateColumn>
                                     </Columns>
                               </asp:DataGrid>
                            </td>
                       </tr></table></td></tr>
                     </table>
                    </td>        
                 </tr>        
            </table>
            <table cellpadding="0" cellspacing="0" width="830px" >
            <tr align="center"><td><asp:Button ID="btnMatch" runat="server" CssClass="button" Text="Match Selected Items" /></td>
                <td><asp:Button ID="btnUnMatch" runat="server" CssClass="button" 
                        Text="Un-Match Selected Item" />
             </td></tr>
         </table>
        <table style="border-style:groove" width="825px" cellpadding="0" cellspacing="0">
        
                <tr><td>
                    <table width="100%">
                        <tr class="headLabelMedium"><td>PMM Items</td></tr>
                        <tr><td><hr style="height: -12px" /></td></tr>
                  </table>
                 </td></tr>
                 <tr><td>
                     <table  cellpadding="0" cellspacing="0" width="825px">
                         <tr valign="bottom"><td>
                            <table cellpadding="0" cellspacing="0">
                                <tr class="itemlistheadsmall"><td align="right">Item:&nbsp;</td><td>
                                    <asp:Textbox CssClass="itemlistheadsmall" ID="txtPMMItem" width="120px" 
                                        runat="server" TabIndex="1"></asp:Textbox></td></tr>
                                <tr class="itemlistheadsmall"><td align="right">Manufacturer No:&nbsp;</td><td>
                                    <asp:Textbox  CssClass="itemlistheadsmall" ID="txtPMMMfr" width="120px" 
                                        runat="server" TabIndex="3"></asp:Textbox></td></tr>
                                <tr class="itemlistheadsmall"><td align="right"></td><td>
                                    </td></tr>
                            </table>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr class="itemlistheadsmall"><td align="right">&nbsp;Catalog No:&nbsp;</td><td>
                                    <asp:Textbox CssClass="itemlistheadsmall" ID="txtPMMCtlg" width="120px" 
                                        runat="server" TabIndex="2"></asp:Textbox></td></tr>
                                <tr class="itemlistheadsmall"><td align="right">&nbsp;Vendor No:&nbsp;</td><td>
                                    <asp:Textbox  CssClass="itemlistheadsmall" ID="txtPMMVendor" width="120px" 
                                        runat="server" TabIndex="4"></asp:Textbox></td></tr>
                                <tr class="itemlistheadsmall"><td align="right">&nbsp;Description:&nbsp;</td><td>
                                    <asp:Textbox CssClass="itemlistheadsmall" ID="txtPMMDesc" width="175px" 
                                        runat="server" TabIndex="6"></asp:Textbox></td></tr>
                            </table>
                        </td>
                        <td >
                            <table cellpadding="0" cellspacing="0">
                                <tr><td><nobr>
                                    <asp:CheckBox CssClass="itemlistheadsmall" ID="chkPMMMatched" 
                                        runat="server" Text="Show Unmatched Items Only" AutoPostBack="True" /></nobr></td></tr>
                            </table>
                        </td>
                        <td align="right" >
                            <table cellpadding="0" cellspacing="0">
                                <tr><td width="100px"></td></tr>
                                <tr align="right" valign="bottom"><td><asp:Button ID="btnPMMClear" runat="server" 
                                        CssClass="button" Text="Clear" Width="60px" /></td></tr>
                                <tr align="right" valign="bottom"><td><asp:Button ID="btnPMMSearch" runat="server" 
                                        CssClass="button" Text="Search" Width="60px" /></td></tr>
                            </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:DataGrid ID="dgPMMItems" runat="server" AllowPaging="true" AutoGenerateColumns="false" Width="825" PagerStyle-Position="Top" PageSize="3" DataKeyField="ITEM_ID">

                                    <PagerStyle Mode="NumericPages" Position="Top" HorizontalAlign="Right" PageButtonCount="5" VerticalAlign="Middle"  />
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <ItemTemplate><tr id="trItem" class="itemlistitemsmall"><td><asp:Button ID="btnSelect" runat="server" CssClass="button" Text="Select" Width="40px" Height="18px" Font-Size="X-Small"  /></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"ITEM_ID"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"ITEM_NO"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"STAT"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"CTLG_NO"))%></td>
                                                <td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"MFR"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem, "VENDOR"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"VEND_CAT"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"DESCR"))%></td>
                                                </tr>
                                            </ItemTemplate>
                                           <HeaderTemplate><table border="1" cellpadding="0" cellspacing="0"><tr class="itemlistheadsmall"><td><td>Item ID</td></td><td>PMM Number</td><td>Status</td><td>Ctlg</td>
                                                <td>Mfr</td><td>Vendor</td><td>Vendor Ctlg</td><td>Description</td>
                                                </tr>
                                            </HeaderTemplate>
                                           <FooterTemplate></table></FooterTemplate>
                                           <EditItemTemplate><tr id="trItem" style="background-color:Yellow;" class="itemlistitemselectsmall"><td><asp:Button ID="btnSelect" runat="server" CssClass="button" Text="Select" Width="40px" Height="18px" Font-Size="X-Small" /></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"ITEM_ID"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"ITEM_NO"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"STAT"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"CTLG_NO"))%></td>
                                                <td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"MFR"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem, "VENDOR"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"VEND_CAT"))%></td><td><%#GetBindingValue(DataBinder.Eval(container.dataitem,"DESCR"))%></td>
                                                </tr></EditItemTemplate>
                                        </asp:TemplateColumn>
                                     </Columns>
                               </asp:DataGrid>
                            </td>
                       </tr>
                       
                     </table>
                </td></tr>
             </table></td><td>&nbsp;</td></tr></table>
</asp:Content>

