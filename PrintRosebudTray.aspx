﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrintRosebudTray.aspx.vb" Inherits="PrintRosebudTray" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
    <div>
        <table width="650px"><tr class="headLabelLarge"><td align="center" style="font-size:large;"><asp:Label runat="server" ID="lblTrayName" ></asp:Label></td></tr>
            <tr align="left"><td align="right"><b>Instrument Count:</b></td><td><asp:Label runat="server" ID="lblCount"></asp:Label></td></tr></table>
        <%--<asp:DataList ID="dlRecipe" runat="server">
            <HeaderTemplate><table width="650px" border="1px" cellpadding="0" cellspacing="0" >
            <tr class="printRowHead"><td>Qty</td><td>Description</td><td>Model</td><td>PMM</td><td>Qty Used</td></tr>
            <tr><td><asp:Label ID="lblMessage" runat="server" CssClass="errortext"></asp:Label></td></tr></HeaderTemplate>
            <ItemTemplate><tr class='<%#GetClass(DataBinder.Eval(Container.DataItem, "Order"))%>'>
                        <td align="center"><%#DataBinder.Eval(Container.DataItem, "Qty")%>
                        <td><%#DataBinder.Eval(Container.DataItem, "Description")%>
                        <td><%#GetValue(DataBinder.Eval(Container.DataItem, "Model"))%></td>
                        <td><%#GetValue(DataBinder.Eval(Container.DataItem, "ICode"))%></td>
                        <td>&nbsp;</td></tr></ItemTemplate>
            <FooterTemplate></table></FooterTemplate>
            
            </asp:DataList>--%>
    <asp:Label ID="lblOutput" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
