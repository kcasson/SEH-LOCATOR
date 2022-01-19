<%@ Page Language="VB" MasterPageFile="~/StEPMMWeb.master" AutoEventWireup="false" CodeFile="RosebudTrayView.aspx.vb" Inherits="Site_RosebudTrayView" title="Rosebud Trays" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script src="script/client.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    /* kcasson, 9/18/2009
        page script*/
        function ddlSearch()
        {
            var dl = document.getElementById("ctl00_ContentPlaceHolder1_ddlTrays");
            var st = LCase(document.getElementById("ctl00_ContentPlaceHolder1_InitControl").value);
            var sz = dl.options.length;
            
            for (i=0;i<=(sz-1);i++)
            {
                var tx = dl.options[i].text;
                tx = LCase(tx); //Make variable upper case for compare
                if (tx.indexOf(st)>=0)
                {
                    dl.selectedIndex = i;
                    break;
                }
            }
        } 
            
        function LCase(strInput) 
        { 
            return strInput.toLowerCase(); 
        }
        function frmSubmit()
        {
            var btn = document.getElementById('ctl00_ContentPlaceHolder1_btnRetrieve');
            var dl = document.getElementById("ctl00_ContentPlaceHolder1_ddlTrays")
            if (dl.selectedIndex != -1)
            {
                btn.click();
            }
            else
            {
                alert('Please select a tray!');
                dl.focus();
            }
        }
    
    </script>
    <style type="text/css">
        .style3
        {
            width: 715px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="border-style: groove" width="735px"><tr><td>
    <!-- Menu Head -->
    <table width="700" cellpadding="0" cellspacing="0"><tr class="menuhead"><td>Rosebud Tray Recipes</td><td></td></tr></table>
    <!-- Page Dynamics -->
    <table width="700" cellpadding="0" cellspacing="0">
    <tr class="itemlisthead"><td valign="top">Search:<br /><input type="text" onkeyup="ddlSearch();" id="InitControl" tagname="text"  runat="server" size="35" /></td>
    <td valign="top" id="trPrint" runat="server" align="right" style="display:none;" colspan="2"><a href="PrintRosebudTray.aspx" target="_blank" title="Print Tray Recipe">
        <img src="Images/Print.jpg" alt="Print Tray Contents" title="Print Tray Contents" 
            
            style="border-style: none; border-color: inherit; border-width: medium; height: 29px; width: 45px;" /><br />Print Tray</a><a href="PrintRosebudTray.aspx">&nbsp;</a></td></tr></table>
    <table cellpadding="0" cellspacing="0"><tr class="itemlisthead"><td class="style3">Tray Listing:<br />
        <asp:listbox ID="ddlTrays" runat="server" Width="560"  Rows="5" ></asp:listbox>
        <input type="button" value="Retrieve Recipe" id="btnSubmit" class="button" 
                onclick="frmSubmit();"   /></td>
        <td style="display:none;"><asp:Button ID="btnRetrieve" Text="Retrieve Recipe" CssClass="button" runat="server" /></td>
    </tr></table></td></tr></table>
    <table style="border-style: groove" ><tr>
        <td ><table width="721px" 
    >
    <tr class="listhead"><td align="center"><asp:Label ID="lblDisplayed" runat="server" ></asp:Label></td></tr>
    <tr id="trNoData" enableviewstate="false" runat="server" style="display:none;"><td class="errortext">No data retrieved for this tray . . .</td></tr></table>
    <table><tr><td><asp:DataList ID="dgTrayContents" runat="server" >
    
        <HeaderTemplate><div style="overflow:auto;height:360px;width:700px;"><table width="680px" border="1" cellpadding="0" cellspacing="0"><tr class="listhead"><td>Qty</td><td>Model</td><td>Description</td><td>PMM #</td></tr></HeaderTemplate>
        <ItemTemplate><tr class='<%#GetClass(DataBinder.Eval(Container.DataItem, "Qty"))%>'>
                                <td><%#DataBinder.Eval(Container.DataItem, "Qty")%></td>
                                <td><%#DataBinder.Eval(Container.DataItem, "Model")%>
                                <td><%#DataBinder.Eval(Container.DataItem, "Description")%>
                                <td>&nbsp;</td>
                               <%-- <td><%#DataBinder.Eval(Container.DataItem, "PMM")%></td></tr>--%>
                                </ItemTemplate>
                <FooterTemplate><tr><td></td></tr><tr><td></td></tr></table></div></FooterTemplate>
            </asp:DataList></td></tr><tr class="menuhead"><td><asp:Label ID="lblPackingInfo" runat="server"></asp:Label></td></tr></table><table><tr><td><asp:Label ID="lblMsg" 
                runat="server" CssClass="errortext" EnableViewState="False"></asp:Label></td></tr></table></td></tr></table>
    
    
</asp:Content>

