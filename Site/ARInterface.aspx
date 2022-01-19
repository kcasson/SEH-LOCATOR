<%@ Page Title="" Language="VB" MasterPageFile="~/Site/StEMMNoLeftNav.master" AutoEventWireup="false" CodeFile="ARInterface.aspx.vb" Inherits="Site_ARInterface" MaintainScrollPositionOnPostBack="true" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script src="../script/client.js" type="text/javascript"></script>
    <link href="Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        
        

    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="main">
<div class="itemlisthead"  style="padding:5px;">
<asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>

<table border="1" rules="none" frame="box" width="100%" cellpadding="0" cellspacing="0"><tr align="center"><td>
<asp:UpdatePanel runat="server" ID="UpdatePanelPrefs"  UpdateMode="Conditional" ChildrenAsTriggers="false"><Triggers><asp:AsyncPostBackTrigger ControlID="btnUpdatePrefs" /></Triggers><ContentTemplate>
<table width="100%" rules="none" frame="box">
    <tr><td colspan="3" align="center">Output Definitions</td></tr>
    <tr><td colspan="2"><asp:Label ID="lblErrorMsg" CssClass="errortextsmall"  EnableViewState="false" runat="server" Visible="false"></asp:Label></td></tr>
        <tr><td colspan="2">&nbsp;</td></tr>      
        <tr ><td  align="right" valign="top" width="30%">Fiscal Year:</td><td align="left">
            <asp:TextBox ID="LoadFocus" Width="50px" runat="server" 
                CssClass="itemlistitem" MaxLength="4"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="LoadFocus" ErrorMessage="*" SetFocusOnError="True" 
                ValidationGroup="Settings"></asp:RequiredFieldValidator>
            </td></tr>
        <tr><td align="right"  valign="top">Fiscal Period:</td><td width="30%" align="left">
            <asp:TextBox ID="txtFiscalPeriod" runat="server" Width="30px" 
                CssClass="itemlistitem" MaxLength="2"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="txtFiscalPeriod" ErrorMessage="*" SetFocusOnError="True" 
                ValidationGroup="Settings"></asp:RequiredFieldValidator>
            </td></tr>
        <tr><td align="right"  valign="top">Source Code:</td><td align="left">
            <asp:TextBox ID="txtSourceCode" runat="server" Width="80px" 
                CssClass="itemlistitem" MaxLength="6"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="txtSourceCode" ErrorMessage="*" SetFocusOnError="True" 
                ValidationGroup="Settings"></asp:RequiredFieldValidator>
            </td></tr>
        <tr><td colspan="2">&nbsp;</td></tr>
        <tr><td colspan="2" align="right"><asp:Button ID="btnUpdatePrefs" runat="server" 
                CssClass="button" Text="Update" ValidationGroup="Settings" />&nbsp;</td></tr>
    </table>
    </ContentTemplate></asp:UpdatePanel>
    </td><td>     
        <table class="itemlistitem"  cellpadding="0" cellspacing="0" >
            <tr><td align="left">
                <asp:Panel ID="Panel1" runat="server" GroupingText="Retrieve File" Width="200px" CssClass="itemlistitem" >
                    <asp:RadioButton ID="rbDetail" runat="server" Text="Detailed Output" GroupName="FileOutput" Checked="true" CssClass="itemlistitem" />
                    <br />
                    <asp:RadioButton ID="rbSummary" runat="server" Text="Summary Output" GroupName="FileOutput" CssClass="itemlistitem" />
                    <br />
                    <br />&nbsp;&nbsp;
                    <asp:Button ID="btnGetfile" runat="server" Text="Generate File" CssClass="button" />
                </asp:Panel>
            </td></tr>
            <tr><td><asp:Label ID="lblFileError" runat="server" CssClass="errortextsmall" 
                    EnableViewState="False"></asp:Label></td></tr>
        </table></td></tr></table>
    <br />
    <table border="1" rules="none" frame="box" width="100%">
        <tr><td align="center">Corporation Codes Selection</td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanelCorps" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional" >
                <Triggers><asp:AsyncPostBackTrigger ControlID="btnAdd" /><asp:AsyncPostBackTrigger ControlID="btnDelete" /><asp:AsyncPostBackTrigger ControlID="btnUpdateOffset" /></Triggers>
                <ContentTemplate>   
                   <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanelCorps" >
                    <ProgressTemplate>
                    <table width="100%" class="showScreen" >
                        <tr align="center" valign="middle"><td><asp:Image ID="imgUpdCorps" ImageUrl="~/Images/Loading.gif" runat="server" /><br /><br /><b>Processing changes, please wait . . . </b></td></tr>
                    </table>
                       
                    </ProgressTemplate>
                </asp:UpdateProgress>                 
                <table align="center" class="showBehind">
                    <tr><th align="center">Available Corps</th><th></th><th align="center">Selected Corps/Offsets</th></tr>
                    <tr align="center">
                        <td><asp:listbox ID="lbAvailCorps" runat="server" Rows="7" 
                                CssClass="itemlistitem" Width="250px" SelectionMode="Multiple"></asp:listbox></td>
                        <td width="50px"><asp:ImageButton ID="btnAdd"  runat="server" ImageUrl="~/Images/right.jpg" Width="33px" BorderWidth="1" ToolTip="Add Selected Corps"  /> <br /><br />
                        <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Images/left.jpg" BorderStyle="Ridge" BorderWidth="1"  ToolTip="Remove Selected Corps" 
                                Width="33px" /></td>
                        <td><asp:listbox ID="lbSelectedCorps" runat="server" Rows="7" 
                                CssClass="itemlistitem" Width="250px" SelectionMode="Multiple"></asp:listbox></td>    
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                        <td valign="top" >Offset Number:<br /><asp:textbox ID="txtOffset" runat="server"></asp:textbox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                ControlToValidate="txtOffset" ErrorMessage="<b>*</b>" ValidationGroup="TextOffset"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                        <tr><td colspan="3" align="right"><asp:Button ID="btnUpdateOffset" runat="server" 
                                Text="Update" CssClass="button" ValidationGroup="TextOffset" /></td>
                    </tr>
                </table>

               </ContentTemplate></asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0" width="100%" rules="none" frame="box"> 
        <tr><td>&nbsp;</td></tr>
        <tr align="center"><td><b>Exclude Selected Cost Centers</b><br />
                <asp:listbox  ID="lbCostCenter" runat="server" Rows="10" Width="400px" CssClass="itemlistitem" SelectionMode="Multiple"  ></asp:listbox>
            </td>
            <td><b>Include Selected Expense Codes</b><br />
                <asp:ListBox ID="lbExpCodes" runat="server" Rows="10" Width="300px" CssClass="itemlistitem" SelectionMode="Multiple"  ></asp:ListBox>
            </td>
        </tr>
        <tr><td>&nbsp;</td></tr>
    </table>
<%--    <table border="1" rules="none" frame="box" width="725">
        <tr><td align="center">Account Selection</td></tr>
        <tr><td>&nbsp;</td></tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanelAccts" UpdateMode="Conditional"  runat="server" ChildrenAsTriggers="false">
                    <Triggers><asp:AsyncPostBackTrigger  /><asp:AsyncPostBackTrigger ControlID="imgExcludeAcct" /><asp:AsyncPostBackTrigger ControlID="ddlCorps" /></Triggers>
                        <ContentTemplate>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelAccts" >
                    <ProgressTemplate>

                       
                    </ProgressTemplate>
                </asp:UpdateProgress>
                    </ContentTemplate></asp:UpdatePanel>
            </td>
        </tr>
    </table>--%>

 </div>
 </div>
</asp:Content>

