 <%@ Page Title="Zebra Label Adminstration" Language="VB" MasterPageFile="~/Zebra/Zebra.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>
 <%@ MasterType VirtualPath="~/Zebra/Zebra.master" %>  


<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

    <script language="javascript" type="text/javascript">
// <![CDATA[

        function btnOptAdd_onclick(tab) {
            var s = document.getElementById("ctl00_cnt" + tab + "_opt" + tab + "Selected");
            var a = document.getElementById("ctl00_cnt" + tab + "_opt" + tab + "Available");
            var d = document.getElementById("ctl00_divLoading");
            var m = document.getElementById("ctl00_divMain");



            if (a.selectedIndex >= 0) {
                var btn = document.getElementById("ctl00_cnt" + tab + "_btnSvrOpt" + tab + "Add");
                btn.click();
                d.className = "zebraLoading";
                m.className = "zebraBlock";
            }
            else {
                alert("Please select an 'Available' value!");
            }
        }

        function btnOptRemove_onclick(tab) {
            var s = document.getElementById("ctl00_cnt" + tab + "_opt" + tab + "Selected");
            var a = document.getElementById("ctl00_cnt" + tab + "_opt" + tab + "Available");
            var d = document.getElementById("ctl00_divLoading");
            var m = document.getElementById("ctl00_divMain");



            if (s.selectedIndex >= 0) {
                var btn = document.getElementById("ctl00_cnt" + tab + "_btnSvrOpt" + tab + "Remove");
                btn.click();
                d.className = "zebraLoading";
                m.className = "zebraBlock";
            }
            else {
                alert("Please select a 'Selected' value!");
            }
        }
// ]]> 
    </script>
</asp:Content>

<%-- begin Receiving function--%>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="cnt1" >
    <table  width="916px" cellpadding="0" cellspacing="0" style="background-color:White;font-size:small; padding-left:10px">
    <tr>
        <td >
            Sub Options&nbsp;<asp:CheckBox ID="chkTabOpt1" runat="server" AutoPostBack="True" Text="Enabled" />
            <!--include the legend --><%Response.WriteFile("../Include/ZebraLegend.inc")%>
            </nobr>
        </td>
    </tr>
</table>
<table class="zebraFrame" width="100%" cellpadding="0" cellspacing="0" >
    <tr style="border:1px;">
        <td colspan="3" align="center">
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            <table cellpadding="0" cellspacing="0" width="900px">
                <tr valign="top">
                    <td width="175px" style="vertical-align:top;" >
                        <br />
                        <asp:RadioButtonList ID="opt1" runat="server" cellpadding="0" CellSpacing="0" AutoPostBack="true"
                        BorderStyle="Inset" BorderWidth="1">
                        </asp:RadioButtonList>
                    </td>
                    <td style="width:600px;">
                        <table>
                            <tr style="text-align:center;">
                                <td  style="text-align:right; width:400px;"  >
                                    <br />
                                    <table width="245px" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right">
                                                <table class="zebraFrame">
                                                    <tr >
                                                        <td align="center">
                                                            Selected</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ListBox ID="opt1Selected" runat="server" Rows="20" 
                                        Width="300px" SelectionMode="Multiple" ></asp:ListBox>
                                                            <br />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="text-align:center;width:200px;" valign="middle"  >
                                    <div class="zebraButton"  >
                                        <img id="btnOpt1Add" alt="Add Selected Items" src="~/Images/leftArrow.jpg"  runat="server" height="30" width="30"  onclick="btnOptAdd_onclick('1');centerDiv('ctl00_divLoading');" />
                                        <table style="display:none;">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSvrOpt1Add" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <br />
                                    <br />
                                    <div  class="zebraButton" >
                                        <img id="btnOpt1Remove" alt="Remove Selected Items" src="../Images/rightArrow.jpg" onclick="btnOptRemove_onclick('1');centerDiv('ctl00_divLoading');" runat="server" height="30" width="30"  />
                                        <table style="display:none;">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSvrOpt1Remove" runat="server"  />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="600px" valign="top">
                                    <br />
                                    <table class="zebraFrame" width="300px">
                                        <tr>
                                            <td align="center">
                                                Available</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:ListBox ID="opt1Available" runat="server" Rows="20" 
                                Width="300px" SelectionMode="Multiple" ></asp:ListBox>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

</asp:Content>


<%-- begin Issues function--%>
<asp:Content ID="BodyContent1" runat="server" ContentPlaceHolderID="cnt2">
    <table  width="916px" cellpadding="0" cellspacing="0" style="background-color:White;font-size:small; padding-left:10px">
    <tr>
        <td>
            Sub Options&nbsp;<asp:CheckBox  Text="Enabled"
            ID="chkTabOpt2" runat="server" AutoPostBack="True" />
            <%Response.WriteFile("../Include/ZebraLegend.inc")%>
        </td>
    </tr>
</table>
<table class="zebraFrame" width="100%" >
    <tr>
        <td>
            &nbsp;</td>
        <td>
            <table cellpadding="0" cellspacing="0" width="900px">
                <tr valign="top">
                    <td width="175px" style="vertical-align:top;" >
                        <br />
                        <asp:RadioButtonList ID="opt2" runat="server" cellpadding="0" CellSpacing="0"  AutoPostBack="true"
                        BorderStyle="Inset" BorderWidth="1">
                        </asp:RadioButtonList>
                    </td>
                    <td style="width:600px;">
                        <table>
                            <tr style="text-align:center;">
                                <td  style="text-align:right; width:400px;"  >
                                    <br />
                                    <table width="245px" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right">
                                                <table class="zebraFrame">
                                                    <tr >
                                                        <td align="center">
                                                            Selected</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ListBox ID="opt2Selected" runat="server" Rows="20" 
                                        Width="300px" SelectionMode="Multiple" ></asp:ListBox>
                                                            <br />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="text-align:center;width:200px;" valign="middle"  >
                                    <div class="zebraButton"  >
                                        <img id="btnOpt2Add" alt="Add Selected Items" src="~/Images/leftArrow.jpg"  runat="server" onclick="btnOptAdd_onclick('2');centerDiv('ctl00_divLoading');" height="30" width="30" />
                                        <table style="display:none;">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSvrOpt2Add" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <br />
                                    <br />
                                    <div  class="zebraButton" >
                                        <img id="btnOpt2Remove" alt="Remove Selected Items" src="../Images/rightArrow.jpg" runat="server" onclick="btnOptRemove_onclick('2');centerDiv('ctl00_divLoading');" height="30" width="30"  />
                                        <table style="display:none;">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSvrOpt2Remove" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="600px" valign="top">
                                    <br />
                                    <table class="zebraFrame" width="300px">
                                        <tr>
                                            <td align="center">
                                                Available</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:ListBox ID="opt2Available" runat="server" Rows="20" 
                                Width="300px" SelectionMode="Multiple" ></asp:ListBox>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
    </asp:Content>

<%-- begin Delivery function--%>
<asp:Content ID="BodyContent2" runat="server" ContentPlaceHolderID="cnt3">
    <table  width="916px" cellpadding="0" cellspacing="0" style="background-color:White;font-size:small; padding-left:10px">
    <tr>
        <td>
            Sub Options&nbsp;<asp:CheckBox  Text="Enabled"
            ID="chkTabOpt3" runat="server" AutoPostBack="True" />
            <%Response.WriteFile("../Include/ZebraLegend.inc")%>
        </td>
    </tr>
</table>
<table class="zebraFrame" width="100%" >
    <tr>
        <td>
            &nbsp;</td>
        <td>
            <table cellpadding="0" cellspacing="0" width="900px">
                <tr valign="top">
                    <td width="175px" style="vertical-align:top;" >
                        <br />
                        <asp:RadioButtonList ID="opt3" runat="server" cellpadding="0" AutoPostBack="true" CellSpacing="0" 
                        BorderStyle="Inset" BorderWidth="1">
                        </asp:RadioButtonList>
                    </td>
                    <td style="width:600px;">
                        <table>
                            <tr style="text-align:center;">
                                <td >
                                    &nbsp;</td>
                                <td  style="text-align:right; width:400px;"  >
                                    <br />
                                    <table width="245px" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right">
                                                <table class="zebraFrame">
                                                    <tr >
                                                        <td align="center">
                                                            Selected</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ListBox ID="opt3Selected" runat="server" Rows="20" 
                                        Width="300px" SelectionMode="Multiple" ></asp:ListBox>
                                                            <br />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="text-align:center;width:200px;" valign="middle"  >
                                    <div class="zebraButton"  >
                                        <img id="btnOpt3Add" alt="Add Selected Items" src="~/Images/leftArrow.jpg"  runat="server" onclick="btnOptAdd_onclick('3');centerDiv('ctl00_divLoading');" height="30" width="30" />
                                        <table style="display:none;">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSvrOpt3Add" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <br />
                                    <br />
                                    <br />
                                    <div  class="zebraButton" >
                                        <img id="btnOpt3Remove" alt="Remove Selected Items" src="../Images/rightArrow.jpg" runat="server" onclick="btnOptRemove_onclick('3');centerDiv('ctl00_divLoading');" height="30" width="30"  />
                                        <table style="display:none;">
                                            <tr>
                                                <td>
                                                    <asp:Button ID="btnSvrOpt3Remove" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td width="600px" valign="top">
                                    <br />
                                    <table class="zebraFrame" width="300px">
                                        <tr>
                                            <td align="center">
                                                Available</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:ListBox ID="opt3Available" runat="server" Rows="20" 
                                Width="300px" SelectionMode="Multiple" ></asp:ListBox>
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
    </asp:Content>
