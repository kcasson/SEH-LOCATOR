<%@ Page Title="" Language="VB" MasterPageFile="~/Site/StEMMNoLeftNav.master" AutoEventWireup="false" CodeFile="ParExOpenOrders.aspx.vb" Inherits="Site_ParExOpenOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../script/client.js" type="text/javascript"></script>
    <script type="text/javascript">
        // page script
        // ParExOpenOrders.aspx
        // Kenneth Casson, 10/25/2011
        function toggleDetail(obj) {
            var name = 'img'+obj
            var img = document.getElementById(name);
            name = 'trDtl' + obj
            var elm = document.getElementById(name);
            var bld = document.getElementById(obj);
            var rows = document.getElementById('tOrder').getElementsByTagName("tr");

            if (elm!=null){
                if (elm.style.display=="none") {
                    elm.style.display = 'block';
                    if (img!=null) {
                        img.src = '../Images/minus.gif';
                        bld.className = 'itemlisthead';
                        
                    }
                }
                else {
                    elm.style.display = 'none';
                    if (img != null) {
                        img.src = '../Images/plus.gif';
                        bld.className = 'itemlistitemsmall';
                    } 
                }
            }
           // close any currently open detail sections
           for (var j = 0; j < rows.length; j++) {
               var str = rows[j].id;
               var imgname = 'img' + str.substring(5);
               var imgplus = document.getElementById(imgname);
               var bldclass = document.getElementById(str.substring(5));
  
               if ((str.substring(0, 5) == 'trDtl')&&(str!=name)) {
                   rows[j].style.display = 'none';
                   if (imgplus != null) { // set image icon back to default
                       imgplus.src = '../Images/plus.gif';
                   }
                   if (bldclass!=null) {
                       bldclass.className = 'itemlistitemsmall';
                   }

               }
           }
        }
    
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:Panel ID="Panel1" runat="server" GroupingText="<b>Par Excellence Open Orders</b>" >
        <table cellpadding="0" cellspacing="0" height="300px" width="100%" >
            <tr valign="top"><td width="100%" align="right" class="itemlisthead" valign="top">Corporation: 
                <asp:DropDownList AutoPostBack="true" ID="initControl" runat="server">
                    <asp:ListItem Selected="True" Text="Edgewood" Value="0"></asp:ListItem>
                    <asp:ListItem Text="East" Value="1"></asp:ListItem>
                    <asp:ListItem Text="West" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </td>
            </tr>
            <tr valign="top">
                <td valign="top">
                <asp:Label ID="lblNoOrders" Text="No open orders available for this location" CssClass="errortext" runat="server" Visible="false" EnableViewState="false"></asp:Label>
                <asp:datagrid ID="dgOpenOrders" runat="server" AutoGenerateColumns="false" Width="100%" 
                    AllowPaging="true" PageSize="25" PagerStyle-Mode="NumericPages" PagerStyle-Position="Bottom" PagerStyle-CssClass="itemlistheadsmall">
                     <Columns>
                        <asp:TemplateColumn>
                            <HeaderTemplate>
                            <table  cellpadding="0" cellspacing="0" width="100%" id="tOrder"  >
                                <tr class="itemlisthead" valign="top">
                                    <td>Requisition</td>
                                    <td>Order</td>
                                    <td>Date</td>
                                    <td>Status</td>
                                    <td>Lines</td>
                                    <td>PAR Location</td>
                                    <td>Replenishment Source</td>
                                 </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                     <tr class="itemlistitemsmall" id="<%# DataBinder.Eval(Container.DataItem, "OrderNumber") %>">
                                        <td><img title="Toggle Detail" id="img<%# DataBinder.Eval(Container.DataItem, "OrderNumber") %>"  alt="details" src="../Images/plus.gif" onmouseover="this.style.cursor='hand';" 
                                            onclick="javascript:toggleDetail('<%# DataBinder.Eval(Container.DataItem, "OrderNumber") %>');"/> 
                                            <asp:label ID="lblReqNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ReqNumber")%>'></asp:label></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "OrderNumber")%></td>
                                        <td><%# formatDate(DataBinder.Eval(Container.DataItem, "OrderDate"))%></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "Status")%></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "LineCount")%></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "PARLocation")%></td>
                                        <td><%# DataBinder.Eval(Container.DataItem, "ReplenishmentSource")%></td>
                                     </tr>
                                     <tr  id="trDtl<%# DataBinder.Eval(Container.DataItem, "OrderNumber")%>" class="itemlistitemsmall" style="display:none;">
                                        <td colspan="9" ><hr />
                                            <asp:Label ID="lblPmmInfo" runat="server" Text="All PMM items are complete for this requisition." CssClass="errortextsmall" 
                                                 Visible="false" EnableViewState="false"></asp:Label>
                                            <asp:datagrid ID="dgReqDetail" runat="server" ItemStyle-BorderStyle="Inset" 
                                                AlternatingItemStyle-BorderStyle="Inset" HeaderStyle-BorderStyle="Inset"  >
                                                <HeaderStyle CssClass="itemlistheadsmall"/>
                                                <ItemStyle CssClass="itemlistitemaltsmall" />
                                                <AlternatingItemStyle CssClass="itemlistitemsmall" />
                                            </asp:datagrid>
                                            <br />
                                            <br />
                                            <hr />
                                        </td>
                                     </tr>

                            </ItemTemplate>
                            <FooterTemplate></table></FooterTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:datagrid>                
            </td></tr>
        </table>
    </asp:Panel>

</asp:Content>

