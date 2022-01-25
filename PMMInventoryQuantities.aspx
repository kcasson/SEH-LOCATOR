<%@ Page Language="VB" MasterPageFile="~/StEMMNoLeftNav.master" AutoEventWireup="true" CodeFile="PMMInventoryQuantities.aspx.vb" Inherits="SPDPerpetualInventory" title="PeopleSoft Location Quantities" MaintainScrollPositionOnPostback="true" %>
<%@ register TagPrefix="MMUC" TagName="PageLoadingDisplay" Src="Controls/PageLoadingDisplay.ascx"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <body onload="centerDiv('ctl00_ContentPlaceHolder1_divPO');centerDiv('ctl00_ContentPlaceHolder1_divInv');LoadFocus();"></body>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="script/client.js" type="text/javascript"></script>
    <script type="text/javascript" src="script/iqc.js"></script>
    <script type="text/javascript" language="javascript">

        /*  Author: Kenneth Casson
            Date: 08/24/2010
            Description: Page script
        */
        function showNote(r,t) {
           //Close any open notes to save space page
           rows=document.getElementById(t).getElementsByTagName("tr");
           for (var j = 0; j < rows.length; j++) {
               var str = rows[j].id
               if ((str.substring(0, 6) == 'trHead') || (str.substring(0, 6) == 'trLine')) {
                   var po = r.substring(6)
                   if (('trHead' + po != rows[j].id) && ('trLine' + po != rows[j].id)) {
                       rows[j].style.display = 'none';
                   }
                }
               }

            //Displays header and line notes
            var tr = document.getElementById(r);
            if (tr.style.display == 'none') {
                tr.style.display = 'block';
            }
            else {
                tr.style.display = 'none';
            }
        }
        function centerDiv(obj) {
            //center the passed in object on the page
            var dv = document.getElementById(obj);
            var x = (document.body.clientWidth / 2) - (dv.offsetWidth / 2);
            var y = (document.body.clientHeight / 2) - (dv.offsetHeight / 2);
            dv.style.top = y;
            dv.style.left = x;

        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server" >

    <%-- Begin purchase order pop-out display --%>
    <div id="divPO" class="noDisplay" runat="server" >
       <table width="100%"><tr valign="bottom"><td align="right"><asp:LinkButton runat="server" ID="btnClosePO" Text="Close PO Information"></asp:LinkButton></td></tr>
        <tr align="center" class="menuhead" style="border-style:solid;"><td>
            <asp:Label ID="lblPODisplayHead" runat="server" ></asp:Label>
        </td></tr>
        <tr enableviewstate="false" id="trNoPORecords" runat="server" style="display:none;"><td align="center" class="errortext"><br />No Purchase Order History Retrieved For This Item</td></tr>
        <tr><td>            <asp:ListView ID="lvPOs" runat="server" >
                <LayoutTemplate>
                    <table  border="1" id="tPO" width="100%" >
                        <tr class="popouthead"><td>PO Number</td><td>PO Date</td><td>Line</td><td>Qty Ord</td><td>UOM</td><td>Price</td><td>Ext Price</td><td>Buyer</td><td>PO Note</td><td>Line Note</td></tr>
                        <tr runat="server" id="itemPlaceholder"></tr>
                    </table>
                 <asp:DataPager ID="DataPager1" runat="server" PageSize="15" PagedControlID="lvPOs">
                 <Fields>
                  <asp:NumericPagerField CurrentPageLabelCssClass="itemlistheadsmall" NextPreviousButtonCssClass="itemlistheadsmall"    ButtonCount="10"
                    NumericButtonCssClass="itemlistheadsmall" PreviousPageText="<<" NextPageText=">>"  />
                </Fields> 
            </asp:DataPager>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr class="popoutlist"><td><%# Eval("PO_NO") %></td><td><%#CDate(Eval("PO_DATE")).ToShortDateString%></td><td align="right"><%#Eval("LINE_NO")%></td>
                        <td align="right"><%# Eval("QTY") %></td><td align="right"><%#Eval("UOM")%></td><td align="right"><%#Format(Eval("PRICE"), "$###0.00")%></td><td align="right"><%#Format(Eval("EXT_PRICE"), "$###0.00")%></td><td><%#Eval("BUYER")%></td>
                        <td align="center"><%#IIf(Eval("HEAD_NOTE") <> String.Empty, "<img alt=""Click for Note"" src=""Images/Note.png"" width=""18"" height=""20"" onclick=""javascript:showNote('trHead" & Eval("PO_NO") & Eval("LINE_NO") & "','tPO');""  />", "&nbsp;")%></td>
                        <td align="center"><%#IIf(Eval("LINE_NOTE") <> String.Empty, "<img alt=""Click for Note"" src=""Images/Note.png"" width=""18"" height=""20"" onclick=""javascript:showNote('trLine" & Eval("PO_NO") & Eval("LINE_NO") & "','tPO');""  />", "&nbsp;")%></td>
                    </tr>
                    <tr style="font-size:10pt;display:none;" id='trHead<%#Eval("PO_NO") & Eval("LINE_NO") %>' ><td colspan="10"><b>Header Note:</b>&nbsp;<asp:label ID="lblHeadNote" runat="server" ToolTip='<%#Eval("HEAD_NOTE")%>' Text='<%#Eval("HEAD_NOTE")%>'></asp:label></td></tr>
                    <tr style="font-size:10pt;display:none;" id='trLine<%#Eval("PO_NO") & Eval("LINE_NO") %>' ><td colspan="10"><b>Line Note:</b>&nbsp;<asp:label ID="lblLineNote"  runat="server" ToolTip='<%#Eval("LINE_NOTE")%>' Text='<%#Eval("LINE_NOTE")%>'></asp:label></td></tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="popoutlistalt" ><td><%# Eval("PO_NO") %></td><td><%#CDate(Eval("PO_DATE")).ToShortDateString%></td><td align="right"><%#Eval("LINE_NO")%></td>
                        <td align="right"><%# Eval("QTY") %></td><td align="right"><%#Eval("UOM")%></td><td align="right"><%#Format(Eval("PRICE"), "$###0.00")%></td><td align="right"><%#Format(Eval("EXT_PRICE"), "$###0.00")%></td><td><%#Eval("BUYER")%></td>
                        <td align="center"><%#IIf(Eval("HEAD_NOTE") <> String.Empty, "<img alt=""Click for Note"" src=""Images/Note.png"" width=""18"" height=""20"" onclick=""javascript:showNote('trHead" & Eval("PO_NO") & Eval("LINE_NO") & "','tPO');"" />", "&nbsp;")%></td>
                        <td align="center"><%#IIf(Eval("LINE_NOTE") <> String.Empty, "<img alt=""Click for Note"" src=""Images/Note.png"" width=""18"" height=""20"" onclick=""javascript:showNote('trLine" & Eval("PO_NO") & Eval("LINE_NO") & "','tPO');"" />", "&nbsp;")%></td>
                    </tr>
                    <tr style="font-size:10pt;background-color:#BCD4E5;display:none;" id='trHead<%#Eval("PO_NO")& Eval("LINE_NO") %>'><td colspan="10"><b>Header Note:</b>&nbsp;<asp:label ID="lblHeadNote" runat="server" ToolTip='<%#Eval("HEAD_NOTE")%>' Text='<%#Eval("HEAD_NOTE")%>'></asp:label></td></tr>
                    <tr style="font-size:10pt;background-color:#BCD4E5;display:none;" id='trLine<%#Eval("PO_NO")& Eval("LINE_NO") %>'><td colspan="10"><b>Line Note:</b>&nbsp;<asp:label ID="lblLineNote" runat="server" ToolTip='<%#Eval("LINE_NOTE")%>' Text='<%#Eval("LINE_NOTE")%>'></asp:label></td></tr>
                        
                </AlternatingItemTemplate>
            </asp:ListView>
           
        </td></tr><tr class="noDisplay"><td><asp:Button ID="btnLoadPO" runat="server" /></td></tr></table>
            
        
    </div>
    <%--End PO display--%>
    
        <%-- Begin Invoice pop-out display --%>
    <div id="divInv" class="noDisplay" runat="server" >
       <table width="100%">
           <tr valign="bottom"><td align="right"><asp:LinkButton runat="server" ID="lkCloseInvoice" Text="Close Invoice Information"></asp:LinkButton></td></tr>
            <tr align="center" class="menuhead" style="border-style:solid;"><td>
            <asp:Label ID="lblInvDisplayHead" runat="server" ></asp:Label>
        </td></tr>
        <tr id="trNoInvRecords" enableviewstate="false" runat="server" style="display:none;"><td align="center" class="errortext"><br />No Invoice History Retrieved For This Item</td></tr>            
        <tr><td>
            <asp:ListView ID="lvInvoices" runat="server" >
                <LayoutTemplate>
                    <table cellpadding="0" cellspacing="0" border="1" id="tInv" width="100%">
                        <tr class="popouthead"><td>Inv No</td><td>Inv Date</td><td>Line</td><td>PO</td><td>Ord Qty</td><td>UOM</td><td>Price</td><td>Pmt Qty</td><td>UOM</td><td>Pmt Price</td><td>Vendor</td><td>Vend No</td><td>AP Clerk</td><td>Inv Note</td><td>Line Note</td></tr>
                        <tr runat="server" id="itemPlaceholder"></tr>
                   </table>
                 <asp:DataPager ID="DataPager1" runat="server" PageSize="15" PagedControlID="lvInvoices">
                 <Fields>
                  <asp:NumericPagerField CurrentPageLabelCssClass="itemlistheadsmall" NextPreviousButtonCssClass="itemlistheadsmall"    ButtonCount="10"
                    NumericButtonCssClass="itemlistheadsmall" PreviousPageText="<<" NextPageText=">>"  />
                </Fields> 
            </asp:DataPager>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr class="popoutlist"><td><%# Eval("INV_NO") %></td><td align="right"><%#CDate(Eval("INV_DATE")).ToShortDateString%></td><td align="right"><%#Eval("LINE_NO")%></td><td><%#Eval("PO_NO")%></td>
                        <td align="right"><%# Eval("QTY") %></td><td><%#Eval("UOM")%></td><td align="right"><%#Format(Eval("PO_PRICE"), "$###0.00")%></td><td align="right"><%#Eval("PMT_QTY")%></td><td><%#Eval("PMT_UOM")%></td>
                        <td align="right"><%#Format(Eval("PMT_PRICE"), "$###0.00")%></td><td><asp:label ID="lblVend" runat="server" Text='<%#Left(Eval("VEND_NAME"), 14)%>' Tooltip='<%#Eval("VEND_NAME")%>'></asp:label></td><td align="center"><%# Eval("VEND_CODE")%></td><td><asp:label ID="lblUsr" runat="server" Text='<%#Left(Eval("USR"), 13)%>' ToolTip='<%#Eval("USR")%>'></asp:label></td>
                        <td align="center"><%#IIf(Eval("HEAD_NOTE") <> String.Empty, "<img alt=""Click for Note"" src=""Images/Note.png"" width=""18"" height=""20"" onclick=""javascript:showNote('trHead" & Eval("INV_NO") & Eval("LINE_NO") & "','tInv');""  />", "&nbsp;")%></td>
                        <td align="center"><%#IIf(Eval("LINE_NOTE") <> String.Empty, "<img alt=""Click for Note"" src=""Images/Note.png"" width=""18"" height=""20"" onclick=""javascript:showNote('trLine" & Eval("INV_NO") & Eval("LINE_NO") & "','tInv');""  />", "&nbsp;")%></td>
                    </tr>
                    <tr style="font-size:10pt;display:none;" id='trHead<%#Eval("INV_NO") & Eval("LINE_NO") %>'><td colspan="15"><b>Header Note:</b>&nbsp;<asp:label ID="lblHeadNote" runat="server" ToolTip='<%#Eval("HEAD_NOTE")%>' Text='<%#Eval("HEAD_NOTE")%>'></asp:label></td></tr>
                    <tr style="font-size:10pt;display:none;" id='trLine<%#Eval("INV_NO") & Eval("LINE_NO") %>'><td colspan="15"><b>Line Note:</b>&nbsp;<asp:label ID="lblLineNote"  runat="server" ToolTip='<%#Eval("LINE_NOTE")%>' Text='<%#Eval("LINE_NOTE")%>'></asp:label></td></tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="popoutlistalt"><td><%# Eval("INV_NO") %></td><td align="right"><%#CDate(Eval("INV_DATE")).ToShortDateString%></td><td align="right"><%#Eval("LINE_NO")%></td><td><%#Eval("PO_NO")%></td>
                        <td align="right"><%# Eval("QTY") %></td><td><%#Eval("UOM")%></td><td align="right"><%#Format(Eval("PO_PRICE"), "$###0.00")%></td><td align="right"><%#Eval("PMT_QTY")%></td><td><%#Eval("PMT_UOM")%></td><td align="right"><%#Format(Eval("PMT_PRICE"), "$###0.00")%></td>
                        <td><asp:label ID="lblVend" runat="server" Text='<%#Left(Eval("VEND_NAME"), 14)%>' ToolTip='<%#Eval("VEND_NAME")%>'></asp:label></td><td align="center"><%# Eval("VEND_CODE")%></td><td><asp:label ID="lblUsr" runat="server" Text='<%#Left(Eval("USR"), 13)%>' ToolTip='<%#Eval("USR")%>'></asp:label></td>
                        <td align="center"><%#IIf(Eval("HEAD_NOTE") <> String.Empty, "<img alt=""Click for Note"" src=""Images/Note.png"" width=""18"" height=""20"" onclick=""javascript:showNote('trHead" & Eval("INV_NO") & Eval("LINE_NO") & "','tInv');"" />", "&nbsp;")%></td>
                        <td align="center"><%#IIf(Eval("LINE_NOTE") <> String.Empty, "<img alt=""Click for Note"" src=""Images/Note.png"" width=""18"" height=""20"" onclick=""javascript:showNote('trLine" & Eval("INV_NO") & Eval("LINE_NO") & "','tInv');"" />", "&nbsp;")%></td>
                    </tr>
                    <tr style="font-size:10pt;background-color:#BCD4E5;display:none;" id='trHead<%#Eval("INV_NO") & Eval("LINE_NO") %>'><td colspan="15"><b>Header Note:</b>&nbsp;<asp:label ID="lblHeadNote" runat="server" ToolTip='<%#Eval("HEAD_NOTE")%>' Text='<%#Eval("HEAD_NOTE")%>'></asp:label></td></tr>
                    <tr style="font-size:10pt;background-color:#BCD4E5;display:none;" id='trLine<%#Eval("INV_NO") & Eval("LINE_NO") %>'><td colspan="15"><b>Line Note:</b>&nbsp;<asp:label ID="lblLineNote" runat="server" ToolTip='<%#Eval("LINE_NOTE")%>' Text='<%#Eval("LINE_NOTE")%>'></asp:label></td></tr>
                </AlternatingItemTemplate>
            </asp:ListView>
           
        </td></tr><tr class="noDisplay"><td><asp:Button ID="Button1" runat="server" /></td></tr></table>
            
    </div>
    <%--End Invoice display--%>
    
<%--Main (default) display begins here--%>

    <table><tr><td>&nbsp;</td><td>
    <asp:Panel ID="Panel1" runat="server" GroupingText="PeopleSoft Items" >
<table width="95%" cellpadding="0" cellspacing="0"><tr valign="top"><td>&nbsp;</td><td>
    <table width="90%" cellpadding="0" cellspacing="0">
    <tr ><td>&nbsp;</td></tr></table>
    <table cellpadding="0" cellspacing="0">
    <tr valign="top"><td align="right" class="itemlistheadsmall" width="67">Item No:&nbsp;</td>
        <td style="display:none;" >
        <input type="text" id="hPMMNo" style="width:0px;" runat="server" /></td>
        <td class="itemlistheadsmall">
        <input style="width:100px;"  
            id="InitControl" runat="server" type="text" maxlength="15" title="Enter all or part of Item number, press enter or click Search. All items containing this string will be returned.Example:(29927, 299, 27)" onkeypress="if (event.keyCode == 13){var btn = document.getElementById('btnSearch');btn.click();};"
                class="itemlistheadsmall" /></td>
        <td class="itemlistitemsmall">&nbsp;</td><td align="right" class="itemlistheadsmall">Mfr Ctlg No:&nbsp;</td>
        <td style="display:none;" >
        <input type="text" id="hMfrNo" style="width:0px;" runat="server" /></td> <td>
        <input style="width:100px;"  onkeypress="if (event.keyCode == 13){var btn = document.getElementById('btnSearch');btn.click();};"
            id="txtMfrNo" tagname="text" runat="server" type="text" maxlength="20" title="Enter all or part of Manufacturer number, press enter or click Search. All items containing this string will be returned. Only one field is required. Example: (1001-513, 1001, 513)"
                class="itemlistheadsmall" /></td>
            <td class="itemlistitemsmall"></td></tr>
      </table><table>
    <tr valign="top" class="itemlistitemsmall" ><td align="left" 
            class="itemlistheadsmall"><b>Description:</b></td>
        <td style="display:none;">
        <input type="text" id="hdisc" style="width:0px;" runat="server" /></td><td>
        <input style="width:400px;"  onkeypress="if (event.keyCode == 13){var btn = document.getElementById('btnSearch');btn.click();};"
            id="txtDesc" runat="server" type="text" maxlength="50" title="Enter all or part of item description. Use '%' for separated words. Example: (suture, tray%synthes)" 
                class="itemlistheadsmall"/></td>
            <td align="right" valign="baseline">
            <input type="button" id="btnSearch"  onclick="var btn = document.getElementById('ctl00_ContentPlaceHolder1_btnSubmit');populateHidden();btn.click();ShowLoading('ctl00_ContentPlaceHolder1_tblResults');" 
             class="buttonsmall"  value="Search" /><input class="buttonsmall" type="button" onclick="resetSearch();" id="btnReset" value="Reset Search" /></td><td style="display:none;">
            <asp:Button ID="btnSubmit" runat="server" Width="27px" Height="18px" /></td></tr></table><table width="800">
    <tr class="itemlistitemsmall"><td><b>Note:</b> Only items showing bins are known to be in the location.</td></tr>
    

</table>
<table  width="90%"><tr><td><hr /></td></tr></table>
<table id="ctl00_ContentPlaceHolder1_tblResults"><tr><td>
    <table  width="90%" cellpadding="0" cellspacing="0" border="1" ><tr><td>
        <asp:ListView ID="lvItems" runat="server"    >
            <LayoutTemplate>
                <table width="800px" cellpadding="1" cellspacing="1" border="1" ><tr><td colspan="6" class="errortextsmall">Click item number for item detail. (click on column header to sort.)</td></tr>
                    <tr class="headLabelSmall">
                        <td><nobr><asp:LinkButton id="lkItemNo" Text="Item No" runat="server" CommandArgument="ItemNumber"></asp:LinkButton></nobr></td>
                        <td><nobr><asp:LinkButton id="lkCatNo" Text="Catalog No" runat="server" CommandArgument="ManufacturerNumber"></asp:LinkButton></nobr></td>
                        <td><nobr>Mfr</nobr></td>
                        <td><nobr><asp:LinkButton id="lkDescr" Text="Description" runat="server" CommandArgument="Description"></asp:LinkButton></nobr></td>
                        <td><nobr>Desc 1</nobr></td></tr>
                    <tr runat="server" id="itemPlaceholder"></tr>
                </table>
            <asp:DataPager ID="DataPager1" runat="server" PageSize="4" PagedControlID="lvItems">
            <Fields>
              <asp:NumericPagerField CurrentPageLabelCssClass="itemlistheadsmall" NextPreviousButtonCssClass="itemlistheadsmall"   ButtonCount="10"
                NumericButtonCssClass="itemlistheadsmall" PreviousPageText="<<" NextPageText=">>"  />
            </Fields> 
          </asp:DataPager>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="itemlistitemsmall">
                    <td><asp:LinkButton id="SelectButton" runat="server" text='<%# Eval("ItemNumber") %>' CommandName="Select"></asp:LinkButton>&nbsp;</td>
                    <td><asp:Label runat="server" ID="lblCat" Text='<%# Eval("MfrNumber")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblMfr" Text='<%# Eval("MfrName")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblDesc" Text='<%# Eval("ItemDesc")%>'></asp:Label></td>
                    <td><asp:Label ID="lblDesc1" runat="server" Text='<%# Eval("ItemDesc1") %>'></asp:Label></td>

                </tr></ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="itemlistitemaltsmall"><td><asp:LinkButton ID="SelectButton" runat="server" text='<%# Eval("ItemNumber") %>' CommandName="Select"></asp:LinkButton>&nbsp;</td>
                    <td><asp:Label runat="server" ID="lblCat" Text='<%# Eval("MfrNumber")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblMfr" Text='<%# Eval("MfrName")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblDesc" Text='<%# Eval("ItemDesc")%>'></asp:Label></td>
                    <td><asp:Label ID="lblDesc1" runat="server" Text='<%# Eval("ItemDesc1")%>'></asp:Label></td>

                </tr></AlternatingItemTemplate>
                <SelectedItemTemplate><tr class="itemlistitemselectsmall"><td><asp:label runat="server" ID="lblItem" text='<%# Eval("ItemNumber") %>'></asp:label></td>
                    <td><asp:Label runat="server" ID="lblCat" Text='<%# Eval("MfrNumber")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblMfr" Text='<%# Eval("MfrName")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblDesc" Text='<%# Eval("ItemDesc")%>'></asp:Label></td>
                    <td><asp:Label ID="lblDesc1" runat="server" Text='<%# Eval("ItemDesc1")%>'></asp:Label></td>
                    </tr></SelectedItemTemplate></asp:ListView></td></tr>

    </table>
                    
                    <table><tr><td><asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="errortextsmall"></asp:Label></td></tr>
      <tr id="trItem" runat="server"><td>
        <asp:Panel ID="pItemInfo" runat="server" Visible="false" GroupingText="Item Information" Width="700px"><table cellpadding="0" cellspacing="0" width="100%">
                <tr valign="top">
                    <td class="itemlistheadsmall" align="right">Item Number:&nbsp;</td>
                    <td><asp:Label ID="lblItemNo" runat="server" CssClass="itemlistitemsmall"></asp:Label></td>
                    <td align="right" class="itemlistheadsmall">Commodity Code:&nbsp;</td><td><asp:Label ID="lblComCode" runat="server" CssClass="itemlistitemsmall"></asp:Label></td>
                    
                </tr>
                <tr valign="top">
                    <td class="itemlistheadsmall" align="right">Chargeable:&nbsp;</td>
                    <td><asp:Label  ID="lblChargeInd" runat="server" CssClass="itemlistitemsmall"></asp:Label></td>
                    <td align="right" class="itemlistheadsmall">Item Status:&nbsp;</td>
                    <td><asp:Label ID="lblStatus" runat="server" CssClass="itemlistitemsmall"></asp:Label></td>
                </tr>
                <tr valign="top">
                    <td align="right" class="itemlistheadsmall">Item Type:&nbsp;</td><td><asp:Label ID="lblItemType" runat="server" CssClass="itemlistitemsmall"></asp:Label>&nbsp;</td>
                    <td align="right" class="itemlistheadsmall">Supply Type:&nbsp;</td><td><asp:Label ID="lblSupplyType" runat="server" CssClass="itemlistitemsmall"></asp:Label></td>
                </tr>
                </table>
            
            <table cellpadding="0" cellspacing="0" width="755px">
                <tr><td colspan="3"><hr /></td></tr>
                <tr style="display:none;">
                    <td width="50%" valign="top" class="itemlisthead">
                       <table style="border-style:groove;" cellpadding="0" cellspacing="0" width="100%">
                        <tr height="1px"><td>Vendor</td></tr>
                        <tr height="1px"><td><hr /></td></tr>
                        <tr ><td>
                        <asp:DropDownList ID="ddlVendors" runat="server" Width="300px" AutoPostBack="true" CssClass="itemlistheadsmall" ></asp:DropDownList><br />
                        <asp:GridView ID="gvPackaging" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-CssClass="itemlistitemaltsmall"
                             HeaderStyle-CssClass="headLabelSmall" RowStyle-CssClass="itemlistitemsmall" CellPadding="1" CellSpacing="1" BorderWidth="1" BorderStyle="Ridge" >
                            <Columns>
                                <asp:BoundField HeaderText="Default" DataField="DefaultText" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField HeaderText="UOM" DataField="UOM" NullDisplayText="" />
                                <asp:BoundField HeaderText="Contains" DataField="Contains" NullDisplayText="" ItemStyle-HorizontalAlign="Right" />
                                <asp:BoundField HeaderText="Price" DataField="PriceFormatted" ItemStyle-HorizontalAlign="Right" NullDisplayText="" />
                                <asp:BoundField HeaderText="Catalog" DataField="Catalog" NullDisplayText="" />
                                <asp:BoundField HeaderText="UPN" DataField="UPN" NullDisplayText="" />
                            </Columns>     
                        </asp:GridView>
                       </td></tr></table>
                    </td>
                    </tr>
                    </tr>
                    </tr>
                    </tr>
                    <td class="itemlisthead" valign="top">                       
                    <table style="border-style:groove;" cellpadding="0" cellspacing="0" width="100%">
                        <tr height="1px"><td>Inventory</td></tr>
                        <tr height="1px"><td><hr /></td></tr>
                        <tr><td>
                        <asp:GridView  ID="gvInventory" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-CssClass="itemlistitemalt"
                             HeaderStyle-CssClass="headLabelSmall" RowStyle-CssClass="itemlistitem" CellPadding="1" CellSpacing="1" BorderWidth="1" BorderStyle="Ridge" Width="100%"  >
                             <Columns>
                                <asp:BoundField HeaderText="Quantity" DataField="BinLocationAmount" NullDisplayText="" />
                                <asp:BoundField HeaderText="Bin" DataField="BinLocation" NullDisplayText="" />
                                <asp:BoundField HeaderText="Inventory" DataField="BinLocationDesc" NullDisplayText="" />
                                <asp:BoundField HeaderText="Status" DataField="Status" NullDisplayText="" />
                             </Columns>
                        </asp:GridView>
                       </td></tr>

                    </table>
                    </td>
    <!-- Start Substitute items section ***********************************************************************************************************************************************************-->
    <tr><td><hr /></td></tr>
    <!-- hide when no subs available for item -->
    <tr><td style="border-top:solid;border-bottom:solid;border-left:solid;border-right:solid;border-color:red;">
    <table width="100%" style="border-color:red;">
      <tr>
        <td class="itemlisthead">Item <asp:Label ID="lblSubOrigItem" runat="server" CssClass="itemlisthead"></asp:Label> Substitute Item(s) <asp:Label ID="lblHasNoSubs" runat="server" CssClass="itemlisthead" ></asp:Label></td></tr>
            <tr align="center"><td>
     <table  width="100%" cellpadding="0" cellspacing="0" border="1" ><tr><td>
        <asp:ListView ID="lvSubs" runat="server"    >
            <LayoutTemplate>
                <table width="750px" cellpadding="1" cellspacing="1" border="1" ><tr><td colspan="6" class="errortextsmall" align="left">Click item number for item detail. Ordered by Priority</td></tr>
                    <tr class="headLabelSmall">
                        <td><nobr>Item No</nobr></td>
                        <td><nobr>Catalog No</nobr></td>
                        <td><nobr>Mfr</nobr></td>
                        <td><nobr>Description</nobr></td>
                        <td><nobr>Desc 1</nobr></td></tr>
                    <tr runat="server" id="itemPlaceholder"></tr>
                </table>
            <asp:DataPager ID="DataPager1" runat="server" PageSize="4" PagedControlID="lvItems">
            <Fields>
              <asp:NumericPagerField CurrentPageLabelCssClass="itemlistheadsmall" NextPreviousButtonCssClass="itemlistheadsmall"   ButtonCount="10"
                NumericButtonCssClass="itemlistheadsmall" PreviousPageText="<<" NextPageText=">>"  />
            </Fields> 
          </asp:DataPager>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="itemlistitemsmall">
                    <td><asp:LinkButton id="SelectButton" runat="server" text='<%# Eval("ItemNumber") %>' CommandName="Select" ForeColor="Red" Font-Bold="true"></asp:LinkButton>&nbsp;</td>
                    <td><asp:Label runat="server" ID="lblCat" Text='<%# Eval("MfrNumber")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblMfr" Text='<%# Eval("MfrName")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblDesc" Text='<%# Eval("ItemDesc")%>'></asp:Label></td>
                    <td><asp:Label ID="lblDesc1" runat="server" Text='<%# Eval("ItemDesc1") %>'></asp:Label></td>

                </tr></ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="itemlistitemaltsmall"><td><asp:LinkButton ID="SelectButton" runat="server" text='<%# Eval("ItemNumber") %>' CommandName="Select" ForeColor="Red" Font-Bold="true"></asp:LinkButton>&nbsp;</td>
                    <td><asp:Label runat="server" ID="lblCat" Text='<%# Eval("MfrNumber")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblMfr" Text='<%# Eval("MfrName")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblDesc" Text='<%# Eval("ItemDesc")%>'></asp:Label></td>
                    <td><asp:Label ID="lblDesc1" runat="server" Text='<%# Eval("ItemDesc1")%>'></asp:Label></td>

                </tr></AlternatingItemTemplate>
                <SelectedItemTemplate><tr class="itemlistitemselectsmall"><td><asp:label runat="server" ID="lblItem" ForeColor="Red" Font-Bold="true" text='<%# Eval("ItemNumber") %>'></asp:label></td>
                    <td><asp:Label runat="server" ID="lblCat" Text='<%# Eval("MfrNumber")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblMfr" Text='<%# Eval("MfrName")%>'></asp:Label></td>
                    <td><asp:Label runat="server" ID="lblDesc" Text='<%# Eval("ItemDesc")%>'></asp:Label></td>
                    <td><asp:Label ID="lblDesc1" runat="server" Text='<%# Eval("ItemDesc1")%>'></asp:Label></td>
                    </tr></SelectedItemTemplate></asp:ListView></td></tr>

    </table>
                </td></tr>

    <tr><td class="itemlisthead" valign="top">                       
    <table style="border-style:groove;" cellpadding="0" cellspacing="0" width="100%">
        <tr height="1px"><td>Substitute Item Inventory</td></tr>
        <tr height="1px"><td><hr /></td></tr>
        <tr><td>
        <asp:GridView  ID="gvSubItem" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-CssClass="itemlistitemalt"
                HeaderStyle-CssClass="headLabelSmall" RowStyle-CssClass="itemlistitem" CellPadding="1" CellSpacing="1" BorderWidth="1" BorderStyle="Ridge" Width="100%"  >
                <Columns>
                <asp:BoundField HeaderText="Quantity" DataField="BinLocationAmount" NullDisplayText="" />
                <asp:BoundField HeaderText="Bin" DataField="BinLocation" NullDisplayText="" />
                <asp:BoundField HeaderText="Inventory" DataField="BinLocationDesc" NullDisplayText="" />
                <asp:BoundField HeaderText="Status" DataField="Status" NullDisplayText="" />
                </Columns>
        </asp:GridView>
        </td></tr>

    </table>
    </td>
        
</tr>
        </table></td>
        <!-- hide when no subs available for item (END) -->
    </tr>
    <!-- End Substitute items section ***********************************************************************************************************************************************************-->
             <tr>
                 <td>
                     <!--<asp:LinkButton ID="lkPO" runat="server" Text="Purchase Orders"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton ID="lkInvoices" runat="server" Text="Invoices"></asp:LinkButton>--></td>
                 <tr>
                     <td></td>
                 </tr>
                 <tr>
                     <td></td>
                 </tr>
                </tr> 
           </table>
        <table>
            <tr><td> </td></tr>
                <tr><td> </td></tr>
        </table>
            <table width="100%"><tr><td>
               <table style="border-style:groove; font-weight:bolder; font-size:small;" cellpadding="0" cellspacing="0" width="100%" >
                        <tr height="1px"><td>Business Unit 12000 Par Locations</td></tr>
                        <tr height="1px"><td><hr /></td></tr>
                        <tr style="text-align:left;"><td>
                        <asp:GridView  ID="gvParLocations" runat="server" AutoGenerateColumns="false" AlternatingRowStyle-CssClass="itemlistitemalt" width="100%"
                             HeaderStyle-CssClass="headLabelSmall" RowStyle-CssClass="itemlistitem" CellPadding="1" CellSpacing="1" BorderWidth="1" BorderStyle="Ridge"  >
                             <Columns>
                                <asp:BoundField HeaderText="Business Unit" DataField="BusinessUnit" NullDisplayText="" />
                                <asp:BoundField HeaderText="Par Location ID" DataField="InvCartId" NullDisplayText="" />
                                <asp:BoundField HeaderText="UOM" DataField="UOM" NullDisplayText="" />
                                <asp:BoundField HeaderText="Compartment" DataField="Compartment" NullDisplayText="" />
                                 <asp:BoundField HeaderText="Quantity Optimal" DataField="OptimalQty" NullDisplayText="" />
                                 <asp:BoundField HeaderText="Average Cart Usage" DataField="AvgCartUsage" NullDisplayText="" />
                             </Columns>
                        </asp:GridView>
                       </td></tr></table>
                       </td></tr></table>
      </asp:Panel></td></tr></table></td></tr></table><MMUC:PageLoadingDisplay ID="plDisplay" runat="server" /> </td><td>&nbsp;</td></tr></table></asp:Panel></td><td>&nbsp;</td></tr></table>

</asp:Content>

