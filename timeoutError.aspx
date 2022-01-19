<%@ Page Language="VB" MasterPageFile="~/StEMM.master" AutoEventWireup="false" CodeFile="timeoutError.aspx.vb" Inherits="timeoutError" title="Materials Management Web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="script/client.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3 style="color:Red;">Session Timeout please click the link below to reload page.</h3>
    
    <asp:label ID="InitControl" runat="server"></asp:label>
    
</asp:Content>

