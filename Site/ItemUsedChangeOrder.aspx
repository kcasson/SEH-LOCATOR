<%@ Page Language="VB" MasterPageFile="~/Site/StEMMNoLeftNav.master" AutoEventWireup="false" CodeFile="ItemUsedChangeOrder.aspx.vb"  Inherits="Site_ItemUsedChangeOrder" Title="Items Used Change Order" %>
<%@ Register TagPrefix="MMM" Namespace="Web.UI.MiscControls"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<script language="javascript" src="../script/datetimepicker.js" type="text/javascript"></script>
<script language="javascript" type="text/javascript">
    ////////////////////////////////////////////////////////////////////////////////
    // Date: 8/29/2011
    // Description: ItemUsedChangeOrder.aspx page script
    // Author: Kenneth Casson
    // Changes:
    //          (none)
    ////////////////////////////////////////////////////////////////////////////////



    function centerDiv(obj) {
        //center the passed in object on the page
        var dv = document.getElementById(obj);
        var x = Math.abs((document.body.clientWidth / 2) - (dv.clientWidth / 2));
        var y = Math.abs((document.body.clientHeight / 2) - ((dv.clientHeight / 2)));

        dv.style.top = (y + 'px');
        dv.style.left = (x + 'px');
        //return false;
    }

    function cancelAction(obj) {
        var dv1 = document.getElementById(obj);
        var dv2 = document.getElementById('divMain');
        if (dv1 != null) {
            dv1.className = "siteNoDisplay";
            //toggleControls('divMain');
        }
        return true;
    }

    function submitClick(obj) {
        var sb = document.getElementById(obj);
        if (sb != null) {
            sb.click();
            return true;
        }
        return false;
    }

    function setFocus(obj) {
        var f = document.getElementById(obj);
        if (f != null) {
            if (f.disabled == false) {
                try {
                    f.focus();
                }
                catch (err) {
                }
            }
                      
        }
    }

    function disableElement(obj) {
        var o = document.getElementById(obj);
        if (o != null) {
            o.disabled = true;
        }
    }

    function enableElement(obj) {
        var o = document.getElementById(obj);
        if (o != null) {
            o.disabled = false;
        }
    }
</script>
<script src="../script/client.js" type="text/javascript"></script>
    <link href="../Site/Styles/StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="AsyncScriptManager" runat="server" EnablePartialRendering="true" ></asp:ScriptManager>
<!-- begin search content here -->
<div id="divCommoditySearch" class="siteNoDisplay"  >

            <table width="100%">
                <tr><td align="right"><asp:ImageButton ImageUrl="~/Images/Close.jpg" runat="server" Width="50px" Height="50px" ID="imgClose" /><br /><hr /></td></tr>
                <tr><td><b><asp:Label ID="lblSearchHeader" runat="server"></asp:Label></b><br /><hr /></td></tr>
                <tr><td>Search Codes then Click the Desired <asp:Label ID="lblSearchType" runat="server"></asp:Label> for Selection.</td></tr>
                <tr>
                    <td class="itemlistheadsmall"><nobr>Search String:<br /><MMM:DisplayTextBox ID="txtCommoditySearch"  runat="server"  ValidationGroup="grpSearch"
                        CssClass="itemlistheadsmall" ToolTip="Enter all or a portion of commodity code or description"></MMM:DisplayTextBox>
                            <asp:Button runat="server" ID="btnCommoditySearch" Text="Search" CssClass="button"  ValidationGroup="grpSearch" /></nobr></td>
                </tr>
                <tr>
                    <td >
                        <asp:DataGrid ID="dgCommodityCodes" AllowPaging="true" PageSize="20" PagerStyle-Mode="NumericPages"  DataKeyField="ID"  ValidationGroup="grpSearch" runat="server" AutoGenerateColumns="false" Width="100%" >
                            <Columns>
                                
                                <asp:ButtonColumn ButtonType="LinkButton" HeaderText="Code"  DataTextField="TYPE_CD" ItemStyle-CssClass="itemlistitemsmall" 
                                         CommandName="RefCommodity"  ></asp:ButtonColumn>
                                <asp:BoundColumn HeaderText="Code Description" DataField="DESC" ItemStyle-CssClass="itemlistitemsmall"></asp:BoundColumn>
                                
                             </Columns>
                
                        </asp:DataGrid>
                    </td>
                </tr> 
                <%--<tr><td><asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" /> </td></tr>--%>                <%--<tr><td><input type="button" class="button" value="Cancel" onclick="return cancelAction('divCommoditySearch');" /> </td></tr>--%>
            </table>

</div>
<!-- end commodity search content -->

<!-- begin main page content here -->
<div id="divMain">
    <table  width="100%"><tr><td>
        <table  cellpadding="0" cellspacing="0" width="100%"  border="0" >
            <tr><td>
                <table cellpadding="0" cellspacing="0" width="100%"  >
                    <tr><td style=" background-color:Black; color:White; font-weight:bold; font-size:small;" colspan="2"><asp:Label id="lblRequestNumber" runat="server"></asp:Label></td></tr>
                    <tr><td style=" background-color:Black; color:Red; font-weight:bolder; font-size:medium;" colspan="2" align="center">Status: <asp:Label id="lblNotify" runat="server"></asp:Label></td></tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr><td class="itemlistheadsmall">PO Number:</td></tr>
                    <tr><td class="itemlistitemsmall" ><MMM:DisplayTextBox ID="txtPO" CssClass="itemlistitemaltsmall" runat="server" Width="150px" MaxLength="22"></MMM:DisplayTextBox></td></tr>
                    <tr ><td align="center"><asp:Label ID="lblHeader" runat="server" CssClass="headLabelMedium"></asp:Label></td></tr>
                    <tr><td>
                        <asp:Datalist ID="dgItems" BorderColor="Black" BorderWidth="1"  
                            CellPadding="0" CellSpacing="0"  runat="server" AutoGenerateColumns="false"
                             OnDeleteCommand="Delete_Command"
                             OnEditCommand="Edit_Command"
                             OnUpdateCommand="Update_Command">
                            <HeaderStyle CssClass="itemlistheadsmall"/>
                            <ItemStyle CssClass="itemlistitemsmall"  />
                            
                                <HeaderTemplate>
                                   <table cellpadding="1" cellspacing="1"   >
                                     <tr><td colspan="12"  valign="top">
                                            <asp:ValidationSummary   DisplayMode="BulletList" ID="vsPage" ValidationGroup="grpPage" HeaderText="Please Check the Following Errors." runat="server" CssClass="errortextsmall" />
                                            <asp:ValidationSummary   DisplayMode="BulletList" ID="vsSubmit" ValidationGroup="grpSubmit"  HeaderText="Please Check the Following Errors." runat="server" CssClass="errortextsmall" />
                                         </td></tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td align="center">PMM<br />Number</td>
                                            <td align="center" width="100px"><b>Product<br />Number</b></td>
                                            <td align="center" ><b>Qty</b></td>
                                            <td align="center"><b>UOM</b></td>
                                            <td align="center"><b>Price</b></td>
                                            <td align="center"><b>Description</b></td>
                                            <td align="center"><b>Commodity<br />Code</b></td>
                                            <td align="center"><b>Expense<br />Code</b></td>
                                            <td align="center"><b>Chargeable</b></td>
                                            <td align="center"><b>Implant</b></td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr><td colspan="12"><hr /></td></tr>
                                    </HeaderTemplate>
                                       <ItemTemplate>
                                         <tr  valign="top"><nobr>
                                           <td ><asp:button ID="btnEdit"  runat="server" Text="Edit"  CssClass="buttonsmall" CommandName="Edit"/> 
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" ImageUrl="~/Images/error.ico" ToolTip="Delete this item" Height="12px" Width="12px" />
                                           &nbsp;</td>
                                           <td > 
                                                <MMM:DisplayTextBox ID="txtItemCode" runat="server" Width="60px" CssClass="itemlistitemaltsmall"  Enabled="false" ></MMM:DisplayTextBox>
                                            </td>
                                            <td >  
                                                <MMM:DisplayTextBox ID="txtProductCode" runat="server" Width="85px" CssClass="itemlistitemaltsmall" enabled="false" ></MMM:DisplayTextBox>
                                                
                                            </td>
                                            <td ><MMM:DisplayTextBox ID="txtConversionQty" runat="server" Width="40px" CssClass="itemlistitemaltsmall" Enabled="false"></MMM:DisplayTextBox></td>
                                            <td ><MMM:UOMDropDown ID="ddlDispensingUOM" runat="server" Width="50px" CssClass="itemlistitemaltsmall" Enabled="false"></MMM:UOMDropDown></td>
                                            <td ><MMM:DisplayTextBox ID="txtPrice" runat="server" Width="60px" CssClass="itemlistitemaltsmall" Enabled="false" appendTextTo="$"></MMM:DisplayTextBox></td>
                                            <td ><MMM:DisplayTextBox ID="txtDescriptionCode" runat="server" Width="250px"  Rows="3" CssClass="itemlistitemaltsmall" Enabled="false"></MMM:DisplayTextBox></td>
                                            <td >
                                                <MMM:DisplayTextBox ID="txtCommodityCode" runat="server" Width="90px" CssClass="itemlistitemaltsmall" enabled="false"></MMM:DisplayTextBox>
                                                
                                            </td>
                                            <td >
                                                <MMM:DisplayTextBox ID="txtExpenseCode" runat="server" Width="90px" CssClass="itemlistitemaltsmall" Enabled="false"></MMM:DisplayTextBox>
                                                
                                            </td>
                                            <td align="center"><asp:CheckBox ID="chkChargeable" runat="server" Width="20px" Enabled="false"/></td>
                                            <td align="center" valign="top"><asp:CheckBox ID="chkImplant" runat="server" Width="20px" Enabled="false"  />
                                                </td><td><asp:Image ImageUrl="~/Images/flag.jpg" Width="20" Visible="false" Height="20" ID="imgChangedFlag" runat="server" /></td>
                                           
                                            </nobr>
                                          </tr>
                                    </ItemTemplate>
                                    <EditItemStyle Font-Bold="true"   />
                                    <EditItemTemplate>
                                        <tr valign="top" style="border-style:inset; background-color:#006699"><nobr>
                                            <td valign="top"><asp:Button ID="btnSave" ValidationGroup="grpPage" runat="server" Text="Save" CssClass="buttonsmall" CommandName="Update"   />
                                            <asp:ImageButton ID="ibDelete" runat="server" CommandArgument="EditDelete" ImageUrl="~/Images/error.ico" Height="12px" Width="12px" /></td>
                                           <td align="center">  
                                                <MMM:DisplayTextBox ID="txtItemCode" runat="server" Width="40px" CssClass="itemlistitemaltsmall" Enabled="true"></MMM:DisplayTextBox>&nbsp;
                                                <asp:ImageButton ID="ImageButton2"  runat="server" ImageUrl="~/Images/search.ico" CommandArgument="Item"
                                                    Height="12px" Width="12px"  AlternateText="Find Item Information"/>
                                            </td>
                                            <td align="center">  
                                                <MMM:DisplayTextBox ID="txtProductCode" runat="server" Width="60px" CssClass="itemlistitemaltsmall" Enabled="true" ></MMM:DisplayTextBox>&nbsp;
                                                <asp:ImageButton ID="ImageButton1"  runat="server" ImageUrl="~/Images/search.ico" CommandArgument="Product"
                                                    Height="12px" Width="12px"  AlternateText="Find Product Code"/>
                                                <asp:RequiredFieldValidator ID="rvProductNumber" ValidationGroup="grpPage"  ControlToValidate="txtProductCode" ErrorMessage="Product Number is Required." Text="*" CssClass="errortextsmall" runat="server">
                                                    </asp:RequiredFieldValidator>
                                                
                                            </td>
                                            <td align="center"><MMM:DisplayTextBox ID="txtConversionQty" runat="server" Width="30px" CssClass="itemlistitemaltsmall" Enabled="true" ></MMM:DisplayTextBox>
                                            <asp:RequiredFieldValidator ID="rfvConversionQty"  ValidationGroup="grpPage"  ControlToValidate="txtConversionQty" ErrorMessage="Quantity is Required." text="*" CssClass="errortextsmall" runat="server">
                                                    </asp:RequiredFieldValidator>
                                                <asp:RangeValidator id="rvQty" runat="server"  ControlToValidate="txtConversionQty"  type="Double" Text="*" 
                                                    ErrorMessage="Please Enter a Valid Quantity." ValidationGroup="grpPage" cssclass="errortextsmall"></asp:RangeValidator>
                                               </td>
                                            <td align="center"><MMM:UOMDropDown ID="ddlDispensingUOM" runat="server" Width="50px" CssClass="itemlistitemaltsmall" Enabled="true"></MMM:UOMDropDown></td>
                                            <td align="center"><b>$ </b><MMM:DisplayTextBox ID="txtPrice"  runat="server" Width="45px" CssClass="itemlistitemaltsmall" Enabled="true"></MMM:DisplayTextBox>
                                            <asp:RequiredFieldValidator ID="rfvPrice" ValidationGroup="grpPage"  ControlToValidate="txtPrice" ErrorMessage="Price is Required." text="*"  CssClass="errortextsmall" runat="server">
                                                    </asp:RequiredFieldValidator>
                                            <asp:RangeValidator id="rvPrice"  runat="server" ControlToValidate="txtPrice"  type="Double" Text="*"  ValidationGroup="grpPage"
                                                    ErrorMessage="Please Enter a Valid Price." cssclass="errortextsmall"></asp:RangeValidator>
                                            </td>
                                            <td align="center" valign="top"><MMM:DisplayTextBox ID="txtDescriptionCode"  runat="server" Width="250px" TextMode="MultiLine" Rows="3" CssClass="itemlistitemaltsmall" Enabled="true"></MMM:DisplayTextBox>
                                            <asp:RequiredFieldValidator ID="rvDescription" ValidationGroup="grpPage"  ControlToValidate="txtDescriptionCode" ErrorMessage="Description is Required." text="*" CssClass="errortextsmall" runat="server">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:ImageButton ID="imgDescriptionSearch"  runat="server" ImageUrl="~/Images/search.ico" CommandArgument="Description"
                                                    Height="12px" Width="12px"  AlternateText="Find Description"/>
                                            </td>

                                            <td align="center">
                                                <MMM:DisplayTextBox ID="txtCommodityCode" runat="server"  Width="90px" CssClass="itemlistitemaltsmall"></MMM:DisplayTextBox>
                                                <asp:RequiredFieldValidator ID="rvCommodityCode" ValidationGroup="grpPage"  ControlToValidate="txtCommodityCode" ErrorMessage="Commodity Code is Required" text="*" CssClass="errortextsmall" runat="server">
                                                    </asp:RequiredFieldValidator>
                                                <asp:ImageButton ID="imgCommoditySearch" runat="server" CommandArgument="Commodity" ImageUrl="~/Images/search.ico" Height="12px" Width="12px"   AlternateText="Find Commodity Code"/>
                                            </td>
                                            <td align="center">
                                                <MMM:DisplayTextBox ID="txtExpenseCode" runat="server"  Width="90px" CssClass="itemlistitemaltsmall" Enabled="true"></MMM:DisplayTextBox>
                                                <asp:RequiredFieldValidator ID="rvExpenseCode" ValidationGroup="grpPage"  ControlToValidate="txtExpenseCode" ErrorMessage="Expense Code is Required." text="*" CssClass="errortextsmall" runat="server">
                                                    </asp:RequiredFieldValidator>
                                                <asp:ImageButton ID="imgExpenseSearch"  runat="server" ImageUrl="~/Images/search.ico" CommandArgument="ExpenseCode"
                                                    Height="12px" Width="12px"  AlternateText="Find Expense Code"/>
                                            </td>
                                            <td align="center"><asp:CheckBox ID="chkChargeable" runat="server" Width="20px" Enabled="true"/></td>
                                            <td align="center"><asp:CheckBox ID="chkImplant" runat="server" Width="20px" Enabled="true"  /></td>
                                            <td>&nbsp;</td>
                                                                                                              </nobr>
                                          </tr>
                                    </EditItemTemplate>

                                       <FooterStyle  />
                                     <FooterTemplate>
                                        <tr ><td  colspan="12"  align="right"><asp:Button ID="btnNewItem" runat="server" OnClick="NewItem_Click" ValidationGroup="grpPage"  Text="Add Item" class="button" /></td></tr>
                                        <tr><td colspan="12"><hr /></td></tr>
                                        <tr><td colspan="12">&nbsp;</td></tr>
                                        <tr><td colspan="12" >
                                        
                                    </FooterTemplate>
                        </asp:Datalist></td></tr></table>
                            <table cellpadding="1" cellspacing="1" class="itemlistitemsmall" width="100%"   >
                            <tr valign="top">
                                <td align="left"  >Vendor:<br /><nobr><MMM:DisplayTextBox ID="txtVendor" runat="server" CssClass="itemlistitemaltsmall" Width="250px" >
                                </MMM:DisplayTextBox><asp:RequiredFieldValidator ID="RfvVendor" runat="server" ControlToValidate="txtVendor" ValidationGroup="grpSubmit" ErrorMessage="Vendor is Required." Text="*"></asp:RequiredFieldValidator><asp:ImageButton ID="imgVendor"  runat="server" ImageUrl="~/Images/search.ico" CommandArgument="Vendor"
                                                    Height="12px" Width="12px"  AlternateText="Find Vendor"/></nobr></td>
                                <td align="left" ><b>Manufacturer:</b><br /><MMM:DisplayTextBox ID="txtManufacturer" runat="server" CssClass="itemlistitemaltsmall" Width="250px" ></MMM:DisplayTextBox> <asp:ImageButton ID="imgManufacturer"  runat="server" ImageUrl="~/Images/search.ico" CommandArgument="Manufacturer"
                                                    Height="12px" Width="12px"  AlternateText="Find Manufacturer"/>
                                    </td>
                                <td align="left" ><b>Location (Cost Center):</b><br /><MMM:DisplayTextBox ID="txtLocation" runat="server" CssClass="itemlistitemaltsmall" Width="150px" ></MMM:DisplayTextBox>
                                <asp:ImageButton ID="imgLocation"  runat="server" ImageUrl="~/Images/search.ico" CommandArgument="Location"
                                                    Height="12px" Width="12px"  AlternateText="Find Location"/>
                                    <asp:RequiredFieldValidator ID="rfvLocation" runat="server" ControlToValidate="txtLocation" ValidationGroup="grpSubmit" ErrorMessage="Location is Required." Text="*"></asp:RequiredFieldValidator></td>
                                <td class="noDisplay" align="left" >New Item Log:<br /><MMM:DisplayTextBox ID="txtItemLog" runat="server" CssClass="itemlistitemaltsmall" Width="125px" ></MMM:DisplayTextBox>
                                </td>
                            </tr><tr>
                                    <td align="left" colspan="4" ><b>Consignment:</b> <asp:RequiredFieldValidator ID="rfvConsignment" runat="server" ControlToValidate="rbConsignment" ValidationGroup="grpSubmit" ErrorMessage="Consignent is Required." Text="*"></asp:RequiredFieldValidator>
                                    <br />
                                        <asp:RadioButtonList ID="rbConsignment" runat="server"  CssClass="itemlistitemsmall" Width="100px">
                                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:RadioButtonList>
                                                        
                                    </td>
                            </tr>
                            <tr id="trEpic" runat="server" class="noDisplay"><td colspan="6"><br /><a href="http://pmm:82/Site/EpicSupplyTypes.aspx" target="_blank" class="itemlistheadsmall"><b>EPIC: UPDATE THE ITEM TYPE & SUPPLY TYPE CODES; IRRIGATIONS/SOLUTIONS/SUTURE/BASIC SUPPLIES/SPECIALTY SUPPLIES/TROCARS <br />  STAPLERS/MISC SUPPLIES.<br />
                                EXAMPLE: IMPLANT-CLICK UPDATE.</b></a><br /><br />
                                </td></tr>
                            <tr><td align="left"  >Cart Number:<br /><MMM:DisplayTextBox ID="txtCartNumber" runat="server" CssClass="itemlistitemaltsmall" Width="200px" ></MMM:DisplayTextBox></td>
                            <td class="noDisplay">Manufacturer's Case Number:<br /><asp:UpdatePanel runat="server" ChildrenAsTriggers="true" UpdateMode="Always" ID="updatePanelCaseNumber" ><ContentTemplate>
                                    <MMM:DisplayTextBox ID="txtCaseNumber" runat="server" 
                                        CssClass="itemlistitemaltsmall" Width="200" AutoPostBack="True">
                                     </MMM:DisplayTextBox>
                                     </ContentTemplate></asp:UpdatePanel></td>
                            </tr>
                            <tr><td align="left" colspan="2" >Sales Rep./Contact Person:<br /><MMM:DisplayTextBox ID="txtContact" runat="server" CssClass="itemlistitemaltsmall" Width="300px" ></MMM:DisplayTextBox></td></tr>
                            <tr><td align="left" >Phone Number:<br /><MMM:DisplayTextBox ID="txtPhone" runat="server" CssClass="itemlistitemaltsmall" Width="250px" ></MMM:DisplayTextBox>
                                <br />
                                </td></tr>
                            <tr id="trNotes" runat="server" class="siteNoDisplay"><td colspan="3"><b>Approver's Notes:</b><br /><MMM:DisplayTextBox ID="txtNotes" CssClass="itemlistitemaltsmall" runat="server" TextMode="MultiLine" Rows="5" Width="600px"></MMM:DisplayTextBox><br /><br /></td></tr>
                            <tr><td align="left" class="menuhead" colspan="4">Patient Charge Information</td></tr>
                            <tr><td align="left">Patient Name:<br /><MMM:DisplayTextBox ID="txtPatientName" runat="server" CssClass="itemlistitemaltsmall" Width="200"></MMM:DisplayTextBox>
                                <asp:RequiredFieldValidator ID="rfvPatientName" runat="server" ControlToValidate="txtPatientName" Text="*" SetFocusOnError="true" ErrorMessage="Patient Name is Required." ValidationGroup="grpSubmit"  ></asp:RequiredFieldValidator>
                                </td>
                                </tr>
                            <tr><td align="left">Doctor Name:<br /><MMM:DisplayTextBox ID="txtDoctor" runat="server" CssClass="itemlistitemaltsmall" Width="200"></MMM:DisplayTextBox>
                            <asp:RequiredFieldValidator ID="rfvDoctorName" runat="server" ControlToValidate="txtDoctor" Text="*" SetFocusOnError="true" ErrorMessage="Doctor Name is Required." ValidationGroup="grpSubmit" ></asp:RequiredFieldValidator>
                                </td></tr>
                            <tr><td align="left" valign="bottom">Date of Surgery:<br /><MMM:DisplayTextBox ID="txtSurgeryDate"   runat="server" CssClass="itemlistitemaltsmall" Width="100px"></MMM:DisplayTextBox>
                            <asp:RequiredFieldValidator ID="rfvSurgeryDate" runat="server" ControlToValidate="txtSurgeryDate" Text="*" SetFocusOnError="true" ErrorMessage="Surgery Date is Required." ValidationGroup="grpSubmit" ></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator
                                    ID="revSurgeryDate" runat="server" ErrorMessage="Please Enter a Valid Date" Text="*" ValidationGroup="grpSubmit" ControlToValidate="txtSurgeryDate" ValidationExpression="^([0-9]{1,2})/([0-9]{1,2})/([0-9]{4,4})$"></asp:RegularExpressionValidator>
                             &nbsp;<span id="spCalendar" runat="server"><a href="javascript:NewCal('ctl00_ContentPlaceHolder1_txtSurgeryDate','mmddyyyy');"><img alt="Pick a date" src="../Images/cal.gif" style="width:15px;height:15px;" border="1"/></a> <b>MM/DD/YYYY</b><br />
                                 
                                </span></td><td align="right" colspan="3" id="tdPrint" runat="server" class="noDisplay">
                                    <asp:ImageButton ID="imPrint" runat="server" ImageUrl="../Images/print2.jpg" Width="49px" /><br /></td></tr>
                            </table>
                    </td></tr>
                    <tr id="trLoadFile" runat="server" ><td align="left" class="itemlistitemsmall" >Attach Invoice:<br /><asp:FileUpload ID="flInvoice" runat="server" ToolTip="Browse to where the invoice is stored on your PC and select to upload file to server." /> 
                                    <asp:Button ID="btnFileUpload" runat="server" Text="Upload Invoice" /></td>
                    </tr>
                    <tr id="trDisplayFile" class="noDisplay" runat="server">
                        <td class="itemlistheadsmall"><br />
                            <a href="" runat="server" id="aFileLink" target="_blank">Click to View Attached Invoice</a> <asp:button ID="lbDeleteFile" runat="server" Text="Delete File"></asp:button>
                        </td>
                    </tr>
                    <tr id="trSubmit" runat="server"><td align="right">
                            <asp:Button ID="btnSubmitForm" runat="server" Text="Create Request" ValidationGroup="grpSubmit"  /></td>
                    </tr>
                    <tr id="trApprovals" runat="server" class="noDisplay">
                    <td  align="right"><asp:Button ID="btnApprove" runat="server" ValidationGroup="vgApproval" Text="Approve" ToolTip="Click to Approve this form for processing"  />&nbsp;
                    <asp:Button ID="btnReject" runat="server" Text="Reject" ValidationGroup="vgApproval" ToolTip="Click to Deny this form for processing"  /></td></tr>
                    <tr align="right" id="trCompletion" runat="server" class="noDisplay"><td><asp:Button ID="btnComplete" runat="server" ValidationGroup="vgComplete" ToolTip="Click to mark this reqest complete" Text="Request Complete" /> </td></tr>
                     <tr align="right" id="trUpdateItem" runat="server" class="noDisplay"><td>
                         <asp:Button ID="btnUpdateItemInfo" runat="server" ValidationGroup="vgUpdate" 
                             ToolTip="Click to update this request" 
                             Text="Update Item Information &amp; Submit for Completion" /> </td></tr>
                    <tr><td>&nbsp;</td></tr>

          </table>
   
</div>
<!-- end main page content -->

</asp:Content>

