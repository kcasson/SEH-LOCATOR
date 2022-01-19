Imports System.Data
Imports cUtilities
Imports Web.UI.MiscControls
Imports System.Net
Imports System.IO

Partial Class Site_ItemUsedChangeOrder
    Inherits System.Web.UI.Page

    Const DocPath As String = "Documents\InvoiceOnly\" 'Path for storing uploaded documents

    <Serializable()> _
    Private Class composer
        Public userId As String
        Public email As String
    End Class

    Private Enum searchTypes
        Commodity
        Expense
        Vendor
        Manufacturer
        Location
        Description
        Product
        Item
    End Enum

    Private Enum emailTypes
        Approval
        Notification
    End Enum
    Private Enum RequestStatuses
        Submitted
        Approved
        Disapproved
        PendingItemBuild
        Complete
    End Enum
    Private Property itemCount As Integer
        Get
            If (Not IsNothing(viewstate("itemCount"))) Then
                Return viewstate("itemCount")
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            viewstate("itemCount") = value
        End Set
    End Property
    Private Property currentItemIndex As Integer
        Get
            If (Not IsNothing(ViewState("currentItemIndex"))) Then
                Return ViewState("currentItemIndex")
            Else
                Return 0
            End If
        End Get
        Set(value As Integer)
            ViewState("currentItemIndex") = value
        End Set
    End Property
    Private Property searchType As searchTypes
        Get
            If (Not IsNothing(viewstate("searchType"))) Then
                Return (viewstate("searchType"))
            Else
                Return searchTypes.Commodity
            End If

        End Get
        Set(value As searchTypes)
            viewstate("searchType") = value
        End Set
    End Property

    Private Property RequestNumber As Integer
        Get
            If (Not IsNothing(viewstate("RequestId"))) Then
                Return CInt(viewstate("RequestId"))
            Else
                Return 0
            End If

        End Get
        Set(value As Integer)
            viewstate("RequestId") = value
        End Set
    End Property

    Private Property dgEnabled As Boolean
        Get
            If (Not IsNothing(viewstate("dgEnabled"))) Then
                Return CBool(viewstate("dgEnabled"))
            Else
                Return True
            End If
        End Get
        Set(value As Boolean)
            viewstate("dgEnabled") = value
        End Set
    End Property

    Private Property itemIsNew As Integer
        Get
            If (Not IsNothing(ViewState("itemIsNew"))) Then
                Return CInt(ViewState("itemIsNew"))
            Else
                Return -1
            End If
        End Get
        Set(value As Integer)
            ViewState("itemIsNew") = value
        End Set
    End Property
    Private Property ComposerInfo As composer
        Get
            If (Not IsNothing(ViewState("composer"))) Then
                Return CType(ViewState("composer"), composer)
            Else
                Dim c As New composer
                ViewState("composer") = c
                Return c
            End If

        End Get
        Set(value As composer)
            ViewState("composer") = value
        End Set
    End Property
    Private Property FileName As String
        Get
            If (Not IsNothing(ViewState("FileName"))) Then
                Return CStr(ViewState("FileName"))
            Else
                Return String.Empty
            End If
        End Get
        Set(value As String)
            ViewState("FileName") = value
        End Set
    End Property

    Private Property chgItemString As StringBuilder
        Get
            If (Not IsNothing(ViewState("chgItemString"))) Then
                Return CType(ViewState("chgItemString"), StringBuilder)
            Else
                Dim sb As New StringBuilder
                ViewState("chgItemString") = sb
                Return sb
            End If
        End Get
        Set(value As StringBuilder)
            ViewState("chgItemString") = value
        End Set
    End Property

    Private Property itemCollection As DataTable
        Get
            If Not IsNothing(viewstate("itemCollection")) Then
                Return CType(viewstate("itemCollection"), DataTable)
            Else
                Dim dt As New DataTable
                dt.Columns.Add(New DataColumn("ItemNumber", GetType(String)))
                dt.Columns.Add(New DataColumn("ProductNumber", GetType(String)))
                dt.Columns.Add(New DataColumn("OrderUOM", GetType(String)))
                dt.Columns.Add(New DataColumn("ConversionQty", GetType(Double)))
                dt.Columns.Add(New DataColumn("DispensingUOM", GetType(String)))
                dt.Columns.Add(New DataColumn("Price", GetType(Double)))
                dt.Columns.Add(New DataColumn("Description", GetType(String)))
                dt.Columns.Add(New DataColumn("CommodityCode", GetType(String)))
                dt.Columns.Add(New DataColumn("ExpenseCode", GetType(String)))
                dt.Columns.Add(New DataColumn("Chargeable", GetType(Boolean)))
                dt.Columns.Add(New DataColumn("Implant", GetType(Boolean)))
                dt.Columns.Add(New DataColumn("InvoiceOnlyID", GetType(Integer)))
                dt.Columns.Add(New DataColumn("InvoiceOnlyLineID", GetType(Integer)))
                dt.Columns.Add(New DataColumn("isChanged", GetType(String)))

                addItem(dt) 'add the first item
                viewstate("itemCollection") = dt
                Return dt

            End If
        End Get
        Set(value As DataTable)
            viewstate("itemCollection") = value
        End Set
    End Property
    Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        ' Try
        'If (Left(Request.Browser.Type, 2) = "IE") Then
        '    If Not (Request.Browser.Version >= 8) Then
        '        Server.Transfer("IEVersionError.aspx?v=" & Request.Browser.Version.ToString)
        '    End If

        'End If
        'Catch ex As Exception
        'Server.Transfer("IEVersionError.aspx?v=" & Request.Browser.Type)
        'End Try

        'this header tag is required to override IE compatibility mode for intranet sites
        Response.AddHeader("X-UA-Compatible", "IE=8")
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'Verify user is authorized. 
        If Not cData.GetUserAuthorization(User.Identity.Name, "/ItemUsedChangeOrder.aspx") Then
            Server.Transfer("na.aspx?u=" & User.Identity.Name)
        End If
        cData.LogPageHit("ItemsUsedChangeOrder.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
        If Not IsPostBack Then
            ViewState.Clear() 'Delete any previous requests
            Dim jscript As String = String.Empty
            dgItems.EditItemIndex = 0
            If Not IsNothing(Request.QueryString("reqId")) Then
                If Not loadExistingRequest(Request.QueryString("reqId")) Then
                    jscript = "alert('An error occurred while loading record " & Request.QueryString("reqId") & ", if data is present it's not reliable.');"
                End If
            Else
                lblNotify.Text = "(NEW)"
                'clear any previous requests
                'itemCollection.Rows.Clear()
                RequestNumber = 0
            End If

            'add the menu title for this page
            Dim hd As Web.UI.MiscControls.MenuHeadControl = CType(Page.Master.FindControl("MenuHead"), Web.UI.MiscControls.MenuHeadControl)
            hd.ImageText = "INVOICE ONLY - REQUISITIONS"
            lblHeader.Text = "ITEMS USED/CHANGE ORDER"
            dgItems.DataSource = itemCollection
            dgItems.DataBind()
            If dgItems.EditItemIndex <> -1 Then
                CType(dgItems.Items(dgItems.EditItemIndex).FindControl("ddlDispensingUOM"), UOMDropDown).SelectedValue = "EA"
            End If
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
            jscript & "setFocus('ctl00_ContentPlaceHolder1_dgItems_ctl01_txtItemCode');", True)
        End If

    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If FileName <> String.Empty Then
            aFileLink.HRef = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath & "/Documents/InvoiceOnly/" & FileName
            trDisplayFile.Attributes.Item("class") = ""
            trLoadFile.Attributes.Item("class") = "noDisplay"
            If RequestNumber > 0 Then
                lbDeleteFile.CssClass = "noDisplay"
            End If

        Else
            trDisplayFile.Attributes.Item("class") = "noDisplay"
            trLoadFile.Attributes.Item("class") = ""
        End If



    End Sub


    Private Function addItem(dt As DataTable) As DataTable
        Dim dr As DataRow
        dr = dt.NewRow
        dt.Rows.Add(dr)
        itemIsNew = dt.Rows.Count
        Return dt
    End Function


    'Code Search options
    Protected Sub btnCommoditySearch_Click(sender As Object, e As System.EventArgs) Handles btnCommoditySearch.Click
        Search()
        'Register client to keep the search div visible on postback
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                " document.getElementById('divCommoditySearch').setAttribute('class','sitePopout');centerDiv('divCommoditySearch');setFocus('ctl00_ContentPlaceHolder1_txtCommoditySearch')", True)

    End Sub

    Private Sub Search()
        dgCommodityCodes.CurrentPageIndex = 0
        dgCommodityCodes.DataSource = cData.GetCodes(UCase(txtCommoditySearch.Text), DirectCast(searchType, searchTypes).ToString)
        dgCommodityCodes.DataBind()
    End Sub

    Protected Sub dgCommodityCodes_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgCommodityCodes.ItemCommand
        'This sub routine handles search functionality, user clicks on results from search

        If e.CommandName = "RefCommodity" Then
            Dim txt As TextBox
            If (searchType = searchTypes.Commodity) Or (searchType = searchTypes.Expense) Or (searchType = searchTypes.Description) Or (searchType = searchTypes.Product) _
                    Or (searchType = searchTypes.Item) Then
                ' Add logic for addition operation here.
                For Each dli As DataListItem In dgItems.Items
                    If dli.ItemType = ListItemType.EditItem Then
                        If (searchType = searchTypes.Item) Or (searchType = searchTypes.Product) Or (searchType = searchTypes.Description) Then

                            Dim ds As DataSet
                            Try
                                If (searchType = searchTypes.Product) Or (searchType = searchTypes.Description) Then
                                    'ds = cData.GetItemDetail("", CType(e.Item.Cells(0).Controls(0), LinkButton).Text)
                                    ds = cData.GetItemDetail(CStr(dgCommodityCodes.DataKeys(e.Item.ItemIndex)), String.Empty)
                                Else
                                    ds = cData.GetItemDetail(CType(e.Item.Cells(0).Controls(0), LinkButton).Text, "")
                                End If

                                If ds.Tables(0).Rows.Count > 0 Then
                                    CType(dli.FindControl("txtItemCode"), TextBox).Text = ds.Tables(0).Rows(0)("ITEM_NO")
                                    CType(dli.FindControl("txtProductCode"), TextBox).Text = ds.Tables(0).Rows(0)("CTLG_NO")
                                    CType(dli.FindControl("txtPrice"), TextBox).Text = ds.Tables(0).Rows(0)("PRICE")
                                    CType(dli.FindControl("ddlDispensingUOM"), DropDownList).SelectedValue = ds.Tables(0).Rows(0)("UOM")
                                    CType(dli.FindControl("txtDescriptionCode"), TextBox).Text = ds.Tables(0).Rows(0)("DESCR")
                                    CType(dli.FindControl("txtCommodityCode"), TextBox).Text = ds.Tables(0).Rows(0)("COMM_CODE")
                                    CType(dli.FindControl("txtExpenseCode"), TextBox).Text = ds.Tables(0).Rows(0)("EXP_CODE")
                                    CType(dli.FindControl("chkChargeable"), CheckBox).Checked = ds.Tables(0).Rows(0)("CHARGEABLE")
                                    CType(dli.FindControl("chkImplant"), CheckBox).Checked = ds.Tables(0).Rows(0)("IMPLANT_IND")
                                    If txtVendor.Text = String.Empty Then
                                        txtVendor.Text = ds.Tables(0).Rows(0)("VENDOR")
                                    End If
                                    If txtManufacturer.Text = String.Empty Then
                                        txtManufacturer.Text = ds.Tables(0).Rows(0)("MFR")
                                    End If
                                End If
                            Catch ex As Exception
                                LogEvent(ex.Message, Diagnostics.EventLogEntryType.Error, 500001)
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                                    " alert('Error has occurred, please contact PMM support!');", True)

                            End Try
                        Else
                            txt = dli.FindControl("txt" & DirectCast(searchType, searchTypes).ToString & "Code")
                            txt.Text = e.Item.Cells(1).Text
                        End If

                    End If
                Next
            Else
                txt = Master.FindControl("ContentPlaceHolder1").FindControl("txt" & DirectCast(searchType, searchTypes).ToString)
                txt.Text = CType(e.Item.Cells(0).Controls(0), LinkButton).Text
            End If

            dgCommodityCodes.DataBind()
            txtCommoditySearch.Text = String.Empty
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
             "enableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem'); ", True)

        End If
    End Sub
    Protected Sub dgCommodityCodes_PageIndexChanged(source As Object, e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgCommodityCodes.PageIndexChanged
        dgCommodityCodes.CurrentPageIndex = e.NewPageIndex
        dgCommodityCodes.DataSource = cData.GetCodes(UCase(txtCommoditySearch.Text), DirectCast(searchType, searchTypes).ToString)
        dgCommodityCodes.DataBind()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
            " document.getElementById('divCommoditySearch').setAttribute('class','sitePopout');centerDiv('divCommoditySearch');setFocus('ctl00_ContentPlaceHolder1_txtCommoditySearch')", True)

    End Sub

    'dgItems control functions
    Protected Sub NewItem_Click()
        saveItem() 'verify previously modified item is saved before adding new 
        dgItems.DataSource = addItem(itemCollection) 'add an item to the collection
        dgItems.EditItemIndex = (itemCollection.Rows.Count - 1)
        dgItems.DataBind()
        If dgItems.EditItemIndex <> -1 Then
            CType(dgItems.Items(dgItems.EditItemIndex).FindControl("ddlDispensingUOM"), UOMDropDown).SelectedValue = "EA"
        End If
        DisableEditDelete()
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                    "setFocus('ctl00_ContentPlaceHolder1_dgItems_ctl01_txtItemCode');", True)

    End Sub
    Protected Sub Delete_Command(sender As Object, e As DataListCommandEventArgs)
        'Delete the item
        itemCollection.Rows.RemoveAt(e.Item.ItemIndex)
        itemCount -= 1
        dgItems.DataSource = itemCollection
        dgItems.DataBind()


    End Sub
    Protected Sub Update_Command(sender As Object, e As DataListCommandEventArgs)
        saveItem()
        dgItems.EditItemIndex = -1
        dgItems.DataSource = itemCollection
        dgItems.DataBind()
        EnableEditDelete()
        ScriptManager.RegisterStartupScript(Page, Page.GetType, Guid.NewGuid.ToString, _
                                            "setFocus('ctl00_ContentPlaceHolder1_dgItems_ctl03_txtVendor');", True)
        If lblNotify.Text = "(PENDING ITEM BUILD)" Then
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
            " disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');", True)
        End If
    End Sub
    Protected Sub Edit_Command(sender As Object, e As DataListCommandEventArgs)
        saveItem()
        dgItems.EditItemIndex = e.Item.ItemIndex
        dgItems.DataSource = itemCollection
        dgItems.DataBind()
        DisableEditDelete()
        If lblNotify.Text = "(PENDING ITEM BUILD)" Then
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
            " disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');", True)
        End If

    End Sub

    Protected Sub dgItems_ItemCommand(source As Object, e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dgItems.ItemCommand
        Select Case e.CommandArgument
            Case "Commodity"
                searchType = searchTypes.Commodity
                txtCommoditySearch.Text = CType(dgItems.Items(dgItems.EditItemIndex).FindControl("txtCommodityCode"), TextBox).Text
                Search()
                lblSearchHeader.Text = "Commodity Code Search"
                lblSearchType.Text = "Commodity Code"
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                " document.getElementById('divCommoditySearch').setAttribute('class','sitePopout');centerDiv('divCommoditySearch');disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');setFocus('ctl00_ContentPlaceHolder1_txtCommoditySearch');", True)

            Case "ExpenseCode"
                searchType = searchTypes.Expense
                txtCommoditySearch.Text = CType(dgItems.Items(dgItems.EditItemIndex).FindControl("txtExpenseCode"), TextBox).Text
                Search()
                lblSearchHeader.Text = "Expense Code Search"
                lblSearchType.Text = "Expense Code"
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                " document.getElementById('divCommoditySearch').setAttribute('class','sitePopout');centerDiv('divCommoditySearch');disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');setFocus('ctl00_ContentPlaceHolder1_txtCommoditySearch');", True)

            Case "Description"
                searchType = searchTypes.Description
                If (CType(dgItems.Items(dgItems.EditItemIndex).FindControl("txtDescriptionCode"), TextBox).Text <> String.Empty) Then
                    txtCommoditySearch.Text = CType(dgItems.Items(dgItems.EditItemIndex).FindControl("txtDescriptionCode"), TextBox).Text
                    Search()
                End If
                lblSearchHeader.Text = "Item Description Search"
                lblSearchType.Text = "Description"
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                " document.getElementById('divCommoditySearch').setAttribute('class','sitePopout');centerDiv('divCommoditySearch');disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');setFocus('ctl00_ContentPlaceHolder1_txtCommoditySearch');", True)
            Case Is = "Product"
                searchType = searchTypes.Product
                If (CType(dgItems.Items(dgItems.EditItemIndex).FindControl("txtProductCode"), TextBox).Text <> String.Empty) Then
                    txtCommoditySearch.Text = CType(dgItems.Items(dgItems.EditItemIndex).FindControl("txtProductCode"), TextBox).Text
                    Search()
                End If
                lblSearchHeader.Text = "Item Product Code Search"
                lblSearchType.Text = "Product"
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                " document.getElementById('divCommoditySearch').setAttribute('class','sitePopout');centerDiv('divCommoditySearch');disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');setFocus('ctl00_ContentPlaceHolder1_txtCommoditySearch');", True)
            Case Is = "Item"
                searchType = searchTypes.Item
                If (CType(dgItems.Items(dgItems.EditItemIndex).FindControl("txtItemCode"), TextBox).Text <> String.Empty) Then
                    txtCommoditySearch.Text = CType(dgItems.Items(dgItems.EditItemIndex).FindControl("txtItemCode"), TextBox).Text
                    Search()
                End If
                lblSearchHeader.Text = "Item Information Search"
                lblSearchType.Text = "Item"
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                " document.getElementById('divCommoditySearch').setAttribute('class','sitePopout');centerDiv('divCommoditySearch');disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');setFocus('ctl00_ContentPlaceHolder1_txtCommoditySearch');", True)
            Case Is = "EditDelete"
                itemCollection.Rows.RemoveAt(e.Item.ItemIndex)
                If itemIsNew <> (dgItems.EditItemIndex + 1) Then
                    itemCount -= 1
                End If
                dgItems.EditItemIndex = -1
                dgItems.DataSource = itemCollection
                dgItems.DataBind()
                btnSubmitForm.Enabled = True

        End Select

    End Sub

    Protected Sub dgItems_ItemCreated(sender As Object, e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dgItems.ItemCreated

        'Fill the row each time it is loaded regaurdless of the item type
        If (e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem) Or (e.Item.ItemType = ListItemType.EditItem) Then

            CType(e.Item.FindControl("txtItemCode"), TextBox).Text = checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("ItemNumber"))
            CType(e.Item.FindControl("txtProductCode"), TextBox).Text = checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("ProductNumber"))

            CType(e.Item.FindControl("txtConversionQty"), TextBox).Text = checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("ConversionQty"))
            If checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("DispensingUOM")) <> String.Empty Then
                CType(e.Item.FindControl("ddlDispensingUOM"), UOMDropDown).SelectedValue = checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("DispensingUOM"))
            End If
            CType(e.Item.FindControl("txtPrice"), TextBox).Text = checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("Price"))
            CType(e.Item.FindControl("txtDescriptionCode"), TextBox).Text = checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("Description"))
            CType(e.Item.FindControl("txtCommodityCode"), TextBox).Text = checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("CommodityCode"))
            CType(e.Item.FindControl("txtExpenseCode"), TextBox).Text = checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("ExpenseCode"))
            CType(e.Item.FindControl("chkChargeable"), CheckBox).Checked = IIf(checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("Chargeable")) = _
                                    String.Empty, False, checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("Chargeable")))
            CType(e.Item.FindControl("chkImplant"), CheckBox).Checked = IIf(checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("Implant")) = _
                                    String.Empty, False, checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("Implant")))
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                If checkDBNull(itemCollection.Rows(e.Item.ItemIndex)("IsChanged")) = "True" Then
                    CType(e.Item.FindControl("imgChangedFlag"), Image).Visible = True
                Else
                    CType(e.Item.FindControl("imgChangedFlag"), Image).Visible = False
                End If
            End If


            If (e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem) Then
                If dgEnabled = True Then
                    CType(e.Item.FindControl("btnEdit"), Button).Enabled = True
                    CType(e.Item.FindControl("btnDelete"), ImageButton).Enabled = True
                Else
                    CType(e.Item.FindControl("btnEdit"), Button).Enabled = False
                    CType(e.Item.FindControl("btnDelete"), ImageButton).Enabled = False
                End If

            End If
        End If
        If (e.Item.ItemType = ListItemType.Footer) Then
            If dgEnabled Then
                CType(e.Item.FindControl("btnNewItem"), Button).Enabled = True
            Else
                CType(e.Item.FindControl("btnNewItem"), Button).Enabled = False
            End If
        End If


    End Sub
    Protected Sub saveItem()
        Dim i As Integer = dgItems.EditItemIndex
        If (dgItems.EditItemIndex <> -1) And (itemCollection.Rows.Count <> 0) And (dgItems.Items.Count >= i) Then
            If CType(dgItems.Items(i).FindControl("txtProductCode"), TextBox).Text <> String.Empty And _
                CType(dgItems.Items(i).FindControl("txtConversionQty"), TextBox).Text <> String.Empty And _
                CType(dgItems.Items(i).FindControl("txtPrice"), TextBox).Text <> String.Empty And _
                CType(dgItems.Items(i).FindControl("txtCommodityCode"), TextBox).Text <> String.Empty And _
                CType(dgItems.Items(i).FindControl("txtExpenseCode"), TextBox).Text <> String.Empty And _
                CType(dgItems.Items(i).FindControl("txtExpenseCode"), TextBox).Text <> String.Empty Then 'check the required fields

                'save the the record to the datatable
                itemCollection.Rows(i)("ItemNumber") = UCase(CType(dgItems.Items(i).FindControl("txtItemCode"), TextBox).Text)
                itemCollection.Rows(i)("ProductNumber") = UCase(CType(dgItems.Items(i).FindControl("txtProductCode"), TextBox).Text)
                itemCollection.Rows(i)("ConversionQty") = _
                    UCase(IIf(Not isNumeric(CType(dgItems.Items(i).FindControl("txtConversionQty"), TextBox).Text), 0, CType(dgItems.Items(i).FindControl("txtConversionQty"), TextBox).Text))
                itemCollection.Rows(i)("DispensingUOM") = CType(dgItems.Items(i).FindControl("ddlDispensingUOM"), DropDownList).SelectedValue
                itemCollection.Rows(dgItems.Items(i).ItemIndex)("Price") = _
                    UCase(IIf(Not isNumeric(CType(dgItems.Items(i).FindControl("txtPrice"), TextBox).Text), 0, CType(dgItems.Items(i).FindControl("txtPrice"), TextBox).Text))
                itemCollection.Rows(dgItems.Items(i).ItemIndex)("Description") = UCase(CType(dgItems.Items(i).FindControl("txtDescriptionCode"), TextBox).Text)
                itemCollection.Rows(dgItems.Items(i).ItemIndex)("CommodityCode") = UCase(CType(dgItems.Items(i).FindControl("txtCommodityCode"), TextBox).Text)
                itemCollection.Rows(dgItems.Items(i).ItemIndex)("ExpenseCode") = UCase(CType(dgItems.Items(i).FindControl("txtExpenseCode"), TextBox).Text)
                itemCollection.Rows(dgItems.Items(i).ItemIndex)("Chargeable") = CType(dgItems.Items(i).FindControl("chkChargeable"), CheckBox).Checked
                itemCollection.Rows(dgItems.Items(i).ItemIndex)("Implant") = CType(dgItems.Items(i).FindControl("chkImplant"), CheckBox).Checked
                itemCount += 1
                itemIsNew = -1
            End If
        End If
    End Sub

    Protected Sub DisableEditDelete()
        'Disable Edit/Delete buttons
        For Each d As DataListItem In dgItems.Items
            If (d.ItemType = ListItemType.AlternatingItem) Or (d.ItemType = ListItemType.Item) Then
                Dim btn As Button = d.FindControl("btnEdit")
                btn.Enabled = False
                Dim ib As ImageButton = d.FindControl("btnDelete")
                ib.ImageUrl = "~/Images/errorGrey.png"
                ib.Enabled = False
            End If
        Next
        btnSubmitForm.Enabled = False
    End Sub

    Protected Sub EnableEditDelete()
        'Enable the edit/delete buttons
        For Each d As DataListItem In dgItems.Items
            If (d.ItemType = ListItemType.AlternatingItem) Or (d.ItemType = ListItemType.Item) Then
                Dim btn As Button = d.FindControl("btnEdit")
                btn.Enabled = True
                Dim ib As ImageButton = d.FindControl("btnDelete")
                ib.ImageUrl = "~/Images/error.ico"
                ib.Enabled = True
            End If
        Next
        btnSubmitForm.Enabled = True
    End Sub
    Protected Sub DisableForm()
        dgEnabled = False
        txtCartNumber.Enabled = False
        txtCommoditySearch.Enabled = False
        txtContact.Enabled = False
        txtItemLog.Enabled = False
        txtLocation.Enabled = False
        txtManufacturer.Enabled = False
        txtPhone.Enabled = False
        txtPO.Enabled = False
        txtVendor.Enabled = False
        imgLocation.Visible = False
        imgManufacturer.Visible = False
        imgVendor.Visible = False
        txtPatientName.Enabled = False
        txtDoctor.Enabled = False
        txtCaseNumber.Enabled = False
        txtSurgeryDate.Enabled = False
        rbConsignment.Enabled = False
        btnSubmitForm.Enabled = False
        spCalendar.Visible = False
        txtNotes.Enabled = False


    End Sub
    Protected Sub EnableForm()
        dgEnabled = True
        txtCartNumber.Enabled = True
        txtCommoditySearch.Enabled = True
        txtContact.Enabled = True
        txtItemLog.Enabled = True
        txtLocation.Enabled = True
        txtManufacturer.Enabled = True
        txtPhone.Enabled = True
        txtPO.Enabled = True
        txtVendor.Enabled = True
        imgLocation.Enabled = True
        imgManufacturer.Enabled = True
        imgVendor.Enabled = True
        txtPatientName.Enabled = True
        txtDoctor.Enabled = True
        txtCaseNumber.Enabled = True
        txtSurgeryDate.Enabled = True
        rbConsignment.Enabled = True
        btnSubmitForm.Enabled = True
        spCalendar.Visible = True
        txtNotes.Enabled = True
    End Sub

    'Submit form all items
    Protected Sub btnSubmitForm_Click(sender As Object, e As System.EventArgs) Handles btnSubmitForm.Click
        If FileName <> String.Empty Then
            If dgItems.EditItemIndex = -1 Then 'no items in edit mode
                If lblRequestNumber.Text = String.Empty Then
                    saveItem() 'make sure any pending items are saved
                    If itemCount > 0 Then 'verify user has entered items
                        'Save the header
                        Try
                            Dim id As Integer = cData.AlterInvoiceOnly("", _
                               UCase(txtPO.Text), _
                               UCase(txtVendor.Text), _
                               UCase(txtManufacturer.Text), _
                                txtLocation.Text, _
                             UCase(txtItemLog.Text), _
                              rbConsignment.SelectedValue, _
                            UCase(txtCartNumber.Text), _
                            UCase(txtContact.Text), _
                             txtPhone.Text, _
                              User.Identity.Name.ToString, _
                                0, _
                                  Convert.DBNull, _
                                   UCase(txtPatientName.Text), _
                                   UCase(txtDoctor.Text), _
                                    txtSurgeryDate.Text, _
                                     UCase(txtCaseNumber.Text), _
                                           UCase(txtNotes.Text), _
                                           FileName)
                            RequestNumber = id
                            'Save the detail records
                            updateItems()

                            lblRequestNumber.Text = "Request Number: " & CStr(id)
                            RequestNumber = id
                            DisableForm()
                            'generateUserEmail(emailTypes.Approval, id) 'Removed for changes removing approver from work flow. (10/5/2012, KAC)

                            'successful save notify user
                            '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                            '" alert('Request #" & id & " has been created!');", True)

                            ApproveRequest()

                            Response.Redirect("http://" & Request.Url.Authority & Request.Path)
                        Catch ex As Exception
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                        " alert('Error has occurred: " & ex.Message & "');", True)
                        End Try


                    Else
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                        " alert('Please add an item before submitting this form!');", True)

                    End If
                End If
            Else
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
        " alert('Please save all items before continuing!');", True)
            End If
        Else
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
" alert('Request completion requires an attached invoice!');setFocus('ctl00_ContentPlaceHolder1_flInvoice');", True)
        End If
    End Sub

    Protected Function loadExistingRequest(RequestId As String) As Boolean
        RequestNumber = RequestId
        Dim rtn As Boolean = False
        Try

            'delete existing items from collection and reset the items
            itemCollection.Rows.Clear()
            dgItems.EditItemIndex = -1


            Dim ds As DataSet = cData.GetInvoiceOnlyRequest(RequestId)
            If ds.Tables(0).Rows.Count > 0 Then
                DisableForm()
                'load composer
                ComposerInfo.userId = RTrim(ds.Tables(0).Rows(0)("ComposerId"))
                ComposerInfo.email = RTrim(ds.Tables(0).Rows(0)("ComposerEmail"))
                'load header
                lblRequestNumber.Text = "Request Number: " & ds.Tables(0).Rows(0)("InvoiceOnlyId")
                txtPO.Text = ds.Tables(0).Rows(0)("PONumber")
                txtVendor.Text = ds.Tables(0).Rows(0)("Vendor")
                txtManufacturer.Text = ds.Tables(0).Rows(0)("Manufacturer")

                txtLocation.Text = ds.Tables(0).Rows(0)("Location")
                txtCartNumber.Text = ds.Tables(0).Rows(0)("CartNumber")
                rbConsignment.SelectedValue = IIf(ds.Tables(0).Rows(0)("IsConsignment") = False, 0, 1)
                txtContact.Text = ds.Tables(0).Rows(0)("SalesRepContact")
                txtPhone.Text = ds.Tables(0).Rows(0)("Phone")
                FileName = ds.Tables(0).Rows(0)("AttachedFile")
                Select Case ds.Tables(0).Rows(0)("Status")
                    Case RequestStatuses.Submitted 'Status is submitted, awaiting approval
                        lblNotify.Text = "(This request has not been approved)"
                        If (cData.GetInvoiceOnlyNotify("Approver").Tables(0).Select("USER_ID = '" & User.Identity.Name & "'").Length >= 1) Then 'only show approval buttons if user is authorized
                            trApprovals.Attributes.Item("class") = ""
                            dgEnabled = True
                        End If
                        trSubmit.Attributes.Item("class") = "noDisplay"
                        trEpic.Attributes.Item("class") = "noDisplay"
                        trCompletion.Attributes.Item("class") = "noDisplay"
                        trNotes.Attributes.Item("class") = ""
                        txtNotes.Enabled = True
                        txtNotes.Focus()
                    Case RequestStatuses.Approved 'Status is Appoved, awaiting completion
                        lblNotify.Text = "(APPROVED FOR COMPLETION)"
                        If (cData.GetInvoiceOnlyNotify("Notify").Tables(0).Select("USER_ID = '" & User.Identity.Name & "'").Length >= 1) Then 'only show approval buttons if user is authorized
                            trCompletion.Attributes.Item("class") = ""
                        End If
                        trSubmit.Attributes.Item("class") = "noDisplay"
                        trApprovals.Attributes.Item("class") = "noDisplay"
                        trEpic.Attributes.Item("class") = "noDisplay"
                        'trNotes.Attributes.Item("class") = ""
                        txtPO.Enabled = True
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                        " setFocus('ctl00_ContentPlaceHolder1_txtPO');", True)
                        dgEnabled = True
                    Case RequestStatuses.Disapproved
                        lblNotify.Text = "(REQUEST DENIED)"
                        trSubmit.Attributes.Item("class") = "noDisplay"
                        trApprovals.Attributes.Item("class") = "noDisplay"
                        trCompletion.Attributes.Item("class") = "noDisplay"
                        trEpic.Attributes.Item("class") = "noDisplay"
                        trNotes.Attributes.Item("class") = ""
                        dgEnabled = False
                    Case RequestStatuses.PendingItemBuild
                        lblNotify.Text = "(PENDING ITEM BUILD)"
                        If (cData.GetInvoiceOnlyNotify("ItemBuilder").Tables(0).Select("USER_ID = '" & User.Identity.Name & "'").Length >= 1) Then 'only show approval buttons if user is authorized
                            trUpdateItem.Attributes.Item("class") = ""
                            trEpic.Attributes.Item("class") = ""
                        End If
                        trApprovals.Attributes.Item("class") = "noDisplay"
                        trCompletion.Attributes.Item("class") = "noDisplay"
                        trSubmit.Attributes.Item("class") = "noDisplay"
                        dgEnabled = True
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                            " disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');", True)

                    Case RequestStatuses.Complete
                        lblNotify.Text = "(COMPLETE)"
                        trSubmit.Attributes.Item("class") = "noDisplay"
                        trApprovals.Attributes.Item("class") = "noDisplay"
                        trCompletion.Attributes.Item("class") = "noDisplay"
                        trEpic.Attributes.Item("class") = "noDisplay"
                        'trNotes.Attributes.Item("class") = ""
                        dgEnabled = False
                End Select
                tdPrint.Attributes.Item("class") = String.Empty
                txtPatientName.Text = ds.Tables(0).Rows(0)("PatientName")
                txtDoctor.Text = ds.Tables(0).Rows(0)("DoctorName")
                txtSurgeryDate.Text = ds.Tables(0).Rows(0)("SurgeryDate")
                txtCaseNumber.Text = ds.Tables(0).Rows(0)("CaseNumber")
                txtNotes.Text = ds.Tables(0).Rows(0)("Notes")


                If ds.Tables(1).Rows.Count > 0 Then
                    'load items
                    For Each dr As DataRow In ds.Tables(1).Rows
                        Dim newRow As DataRow = itemCollection.NewRow
                        newRow("ItemNumber") = dr("ItemNumber")
                        newRow("ProductNumber") = dr("ProductNumber")
                        newRow("ConversionQty") = dr("Qty")
                        newRow("DispensingUOM") = dr("DispensingUOM")
                        newRow("Price") = dr("Price")
                        newRow("Description") = dr("Description")
                        newRow("CommodityCode") = dr("CommodityCode")
                        newRow("ExpenseCode") = dr("ExpenseCode")
                        newRow("Chargeable") = dr("isChargeable")
                        newRow("Implant") = dr("isImplant")
                        newRow("InvoiceOnlyID") = dr("InvoiceOnlyID")
                        newRow("InvoiceOnlyLineID") = dr("InvoiceOnlyLineID")
                        newRow("IsChanged") = dr("IsChanged")
                        itemCollection.Rows.Add(newRow)
                    Next


                    rtn = True
                End If
            End If

        Catch ex As Exception
            rtn = False
        End Try
        Return rtn

    End Function

    Protected Sub imgVendor_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgVendor.Click
        searchType = searchTypes.Vendor
        txtCommoditySearch.Text = txtVendor.Text
        Search()
        lblSearchHeader.Text = "Vendor Search"
        lblSearchType.Text = "Vendor"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
        " document.getElementById('divCommoditySearch').setAttribute('class','sitePopout');centerDiv('divCommoditySearch');disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');setFocus('ctl00_ContentPlaceHolder1_txtCommoditySearch');", True)


    End Sub

    Protected Sub imgManufacturer_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgManufacturer.Click
        searchType = searchTypes.Manufacturer
        txtCommoditySearch.Text = txtManufacturer.Text
        Search()
        lblSearchHeader.Text = "Manufacturer Search"
        lblSearchType.Text = "Manufacturer"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
        " document.getElementById('divCommoditySearch').setAttribute('class','sitePopout');centerDiv('divCommoditySearch');disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');setFocus('ctl00_ContentPlaceHolder1_txtCommoditySearch');", True)

    End Sub

    Protected Sub imgLocation_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgLocation.Click
        searchType = searchTypes.Location
        txtCommoditySearch.Text = txtLocation.Text
        Search()
        lblSearchHeader.Text = "Location (Expense Code) Search"
        lblSearchType.Text = "Location"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
        " document.getElementById('divCommoditySearch').setAttribute('class','sitePopout');centerDiv('divCommoditySearch');disableElement('ctl00_ContentPlaceHolder1_dgItems_ctl02_btnNewItem');setFocus('ctl00_ContentPlaceHolder1_txtCommoditySearch');", True)

    End Sub
    Private Sub updateRequestStatus(status As RequestStatuses, Optional PONumber As String = "")
        Dim strSql As String = String.Empty
        If PONumber <> String.Empty Then
            strSql = _
                "update InvoiceOnly " & _
                "set Status = " & status & _
                " ,PONumber = '" & PONumber & "'" & _
                " ,UpdatedBy = '" & User.Identity.Name & "'" & _
                " ,UpdatedDate = '" & Now & "'" & _
                " where InvoiceOnlyID = " & RequestNumber
        Else
            strSql = _
            "update InvoiceOnly " & _
            "set Status = " & status & _
            " ,UpdatedBy = '" & User.Identity.Name & "'" & _
            " ,UpdatedDate = '" & Now & "'" & _
            " where InvoiceOnlyID = " & RequestNumber
        End If

        Try
            cData.ExecuteSQLSupportNonQuery(strSql)
        Catch ex As Exception
            LogEvent(ex.Message, Diagnostics.EventLogEntryType.Error, 500001)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
        " alert('Error occurred on update!');", True)
        End Try
    End Sub

    Private Sub generateUserEmail(type As emailTypes, requestId As Integer)
        Select Case type
            Case Is = emailTypes.Approval
                Dim sb As New StringBuilder
                sb.Append("<table>")
                sb.Append("<tr><td><b>A request has been created that requires approval. Please click the following link to approve:</b></td></tr>")
                sb.Append("<tr><td><a href='http://" & Request.Url.Authority & Request.Path & "?reqId=" & requestId & "'>Invoice Only Request Number " & requestId & "</a></td></tr>")
                sb.Append("</table>")
                sb.Append(PrintForm())

                Dim ds As DataSet = cData.GetInvoiceOnlyNotify("Approver")

                For Each dr As DataRow In ds.Tables(0).Rows
                    cUtilities.SendEmail(New Mail.MailAddress(dr("UserEmail")), New Mail.MailAddress("pmm@stelizabeth.com"), "Invoice Only Request #" & requestId & " needs to be approved.", sb.ToString)
                Next

        End Select


    End Sub

    'kcasson - 11/2/2012, removed approval features per Lori

    'Protected Sub btnApprove_Click(sender As Object, e As System.EventArgs) Handles btnApprove.Click

    '    If (cData.GetInvoiceOnlyNotify("Approver").Tables(0).Select("USER_ID = '" & User.Identity.Name & "'").Count >= 1) Then
    '        If RequestNumber <> 0 Then
    '            AddNotes(txtNotes.Text)
    '            updateItems() 'just in case there were changes
    '            'send a notification email to the proper individuals
    '            Dim needsItemBuild As Boolean = False
    '            For Each dr As DataRow In itemCollection.Rows
    '                If dr("itemNumber") = String.Empty Then
    '                    needsItemBuild = True
    '                    Exit For
    '                End If
    '            Next
    '            Dim stat As RequestStatuses
    '            If needsItemBuild Then
    '                'create notification
    '                Dim sb As New StringBuilder
    '                sb.Append("<table>")
    '                sb.Append("<tr><td><b>A request has been created that requires item build(s). Please click the following link to access the information:</b></td></tr>")
    '                sb.Append("<tr><td><a href='http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber & "'>Invoice Only Request Number " & RequestNumber & "</a></td></tr>")
    '                sb.Append("</table>")
    '                Dim ds As DataSet = cData.GetInvoiceOnlyNotify("ItemBuilder")
    '                For Each dr As DataRow In ds.Tables(0).Rows
    '                    cUtilities.SendEmail(New Mail.MailAddress(dr("UserEmail")), New Mail.MailAddress("pmm@stelizabeth.com"), "Invoice Only Request #" & RequestNumber & " needs item build(s).", sb.ToString)
    '                Next
    '                stat = RequestStatuses.PendingItemBuild 'item(s) need to be built
    '            Else
    '                Dim sb As New StringBuilder
    '                sb.Append("<table>")
    '                sb.Append("<tr><td><b>An invoice only request has been submitted. Please click the following link to review the information:</b></td></tr>")
    '                sb.Append("<tr><td><a href='http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber & "'>Invoice Only Request Number " & RequestNumber & "</a></td></tr>")
    '                sb.Append("</table>")
    '                Dim ds As DataSet = cData.GetInvoiceOnlyNotify("Notify")
    '                For Each dr As DataRow In ds.Tables(0).Rows
    '                    cUtilities.SendEmail(New Mail.MailAddress(dr("UserEmail")), New Mail.MailAddress("pmm@stelizabeth.com"), "Invoice Only Request #" & RequestNumber & ".", sb.ToString)
    '                Next
    '                stat = RequestStatuses.Approved 'approved, pending completion
    '            End If
    '            'update the record status
    '            updateRequestStatus(stat)
    '            'Reload form for new status
    '            Response.Redirect("http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber)
    '        End If


    '    End If

    'End Sub

    Protected Sub ApproveRequest()

        If RequestNumber <> 0 Then
            'AddNotes(txtNotes.Text)
            'updateItems() 'just in case there were changes
            'send a notification email to the proper individuals
            Dim needsItemBuild As Boolean = False
            For Each dr As DataRow In itemCollection.Rows
                If dr("itemNumber") = String.Empty Then
                    needsItemBuild = True
                    Exit For
                End If
            Next
            Dim stat As RequestStatuses
            If needsItemBuild Then
                'create notification
                Dim sb As New StringBuilder
                sb.Append("<table>")
                sb.Append("<tr><td><b>A request has been created that requires item build(s). Please click the following link to access the information:</b></td></tr>")
                sb.Append("<tr><td><a href='http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber & "'>Invoice Only Request Number " & RequestNumber & "</a></td></tr>")
                sb.Append("</table>")
                sb.Append(PrintForm())
                Dim ds As DataSet = cData.GetInvoiceOnlyNotify("ItemBuilder")
                For Each dr As DataRow In ds.Tables(0).Rows
                    cUtilities.SendEmail(New Mail.MailAddress(dr("UserEmail")), New Mail.MailAddress("pmm@stelizabeth.com"), "Invoice Only Request #" & RequestNumber & " for Vendor " & txtVendor.Text & ", needs item build(s).", sb.ToString)
                Next
                stat = RequestStatuses.PendingItemBuild 'item(s) need to be built
            Else
                Dim sb As New StringBuilder
                sb.Append("<table>")
                sb.Append("<tr><td><b>An invoice only request has been submitted. Please click the following link to review the information:</b></td></tr>")
                sb.Append("<tr><td><a href='http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber & "'>Invoice Only Request Number " & RequestNumber & "</a></td></tr>")
                sb.Append("</table>")
                sb.Append(PrintForm())
                Dim ds As DataSet = cData.GetInvoiceOnlyNotify("Notify")
                For Each dr As DataRow In ds.Tables(0).Rows
                    cUtilities.SendEmail(New Mail.MailAddress(dr("UserEmail")), New Mail.MailAddress("pmm@stelizabeth.com"), "Invoice Only Request #" & RequestNumber & " for Vendor " & txtVendor.Text & ".", sb.ToString)
                Next
                stat = RequestStatuses.Approved 'approved, pending completion
            End If
            'update the record status
            updateRequestStatus(stat)
            'Reload form for new status
            'Response.Redirect("http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber)
        End If




    End Sub

    Protected Sub btnReject_Click(sender As Object, e As System.EventArgs) Handles btnReject.Click
        If txtNotes.Text <> String.Empty Then
            AddNotes(txtNotes.Text)
            updateItems()
            'update the record status to rejected
            updateRequestStatus(RequestStatuses.Disapproved)
            Dim sb As New StringBuilder
            sb.Append("<table>")
            sb.Append("<tr><td><b>Invoice Only Request #" & RequestNumber & " has been <b>REJECTED</b>. Please click the following link to review the information:</b></td></tr>")
            sb.Append("<tr><td><a href='http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber & "'>Invoice Only Request Number " & RequestNumber & "</a></td></tr>")
            sb.Append("<tr><td><br><b>APPROVERS NOTES:</b><br>" & txtNotes.Text & "</td></tr>")
            sb.Append("</table>")
            Dim ds As DataSet = cData.GetInvoiceOnlyNotify("Notify")
            cUtilities.SendEmail(New Mail.MailAddress(ComposerInfo.email), New Mail.MailAddress("pmm@stelizabeth.com"), "Invoice Only Request #" & RequestNumber & ".", sb.ToString)
            'Reload form for new status
            Response.Redirect("http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber)
            'should we notify the user or someone else here?
        Else
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                    " alert('When rejecting a request the Approvers Notes must be entered!');setFocus('ctl00_ContentPlaceHolder1_txtNotes');", True)
        End If

    End Sub

    Protected Sub btnComplete_Click(sender As Object, e As System.EventArgs) Handles btnComplete.Click

        If txtPO.Text <> String.Empty Then
            updateItems()
            updateRequestStatus(RequestStatuses.Complete, txtPO.Text)
            Dim sb As New StringBuilder
            sb.Append("<table>")
            sb.Append("<tr><td><b>Invoice Only Request #" & RequestNumber & " has been completed. Please click the following link to review the information:</b></td></tr>")
            sb.Append("<tr><td><a href='http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber & "'>Invoice Only Request Number " & RequestNumber & "</a></td></tr>")
            sb.Append("</table>")
            sb.Append(PrintForm())
            Dim ds As DataSet = cData.GetInvoiceOnlyNotify("Notify")
            cUtilities.SendEmail(New Mail.MailAddress(ComposerInfo.email), New Mail.MailAddress("pmm@stelizabeth.com"), "Invoice Only Request #" & RequestNumber & " for Vendor " & txtVendor.Text & ".", sb.ToString)
            'Reload form for new status
            Response.Redirect("http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber)
        Else
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
        " alert('Reqest completion requires a PO Number to be entered!');setFocus('ctl00_ContentPlaceHolder1_txtPO');", True)
        End If



    End Sub

    Protected Sub btnUpdateItemInfo_Click(sender As Object, e As System.EventArgs) Handles btnUpdateItemInfo.Click
        If RequestNumber <> 0 Then
            If dgItems.EditItemIndex <> -1 Then
                saveItem()
            End If
            'verify all pmm numbers exist prior to update
            Dim itemsExist As Boolean = True
            For Each dr As DataRow In itemCollection.Rows
                If checkDBNull(dr("ItemNumber")) = String.Empty Then
                    itemsExist = False
                End If
            Next
            If itemsExist Then
                updateItems()
                updateRequestStatus(RequestStatuses.Approved)
                Dim sb As New StringBuilder
                sb.Append("<table>")
                sb.Append("<tr><td><b>An invoice only request has been submitted. Please click the following link to for the information:</b></td></tr>")
                sb.Append("<tr><td><a href='http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber & "'>Invoice Only Request Number " & RequestNumber & "</a></td></tr>")
                sb.Append("</table>")
                sb.Append(PrintForm())
                Dim ds As DataSet = cData.GetInvoiceOnlyNotify("Notify")
                For Each dr As DataRow In ds.Tables(0).Rows
                    cUtilities.SendEmail(New Mail.MailAddress(dr("UserEmail")), New Mail.MailAddress("pmm@stelizabeth.com"), "Invoice Only Request #" & RequestNumber & " for Vendor " & txtVendor.Text & ".", sb.ToString)
                Next
                'Reload form for new status
                Response.Redirect("http://" & Request.Url.Authority & Request.Path & "?reqId=" & RequestNumber)
            Else
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                " alert('Please add all item numbers before proceeding to update this request!');", True)
            End If


        End If

    End Sub
    Private Sub updateItems()
        'Save the detail records
        For Each dr As DataRow In itemCollection.Rows
            Try
                cData.AlterInvoiceOnlyLines(checkDBNull(dr("InvoiceOnlyLineID")), _
                     RequestNumber, _
                    checkDBNull(dr("ItemNumber")), _
                    checkDBNull(dr("ProductNumber")), _
                      checkDBNull(dr("ConversionQty")), _
                    checkDBNull(dr("DispensingUOM")), _
                    checkDBNull(dr("Price")), _
                    checkDBNull(dr("Description")), _
                    checkDBNull(dr("CommodityCode")), _
                    checkDBNull(dr("ExpenseCode")), _
                    checkDBNull(dr("Chargeable")), _
                    checkDBNull(dr("Implant")), _
                    itemChanged(dr("ItemNumber"), dr))


            Catch ex As Exception
                LogEvent(ex.Message, Diagnostics.EventLogEntryType.Error, 500001)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
            " alert('Error occurred on update!');", True)
            End Try
        Next

        If chgItemString.Length > 0 Then
            Dim msg As String
            msg = "<table><tr><td>The following items on the request have triggered flags indicating changes, please review.</td></tr>" & _
                chgItemString.ToString & "</table>"
            SendEmail(New Mail.MailAddress("pmm@stelizabeth.com"), New Mail.MailAddress("pmm@stelizabeth.com"), "Invoice Only #: " & RequestNumber & " has flagged item(s).", msg)
        End If
    End Sub

    Private Sub AddNotes(text As String)
        'adds composer and date time to the note then breaks it up for line spacing on the page.
        Dim count As Integer = 0
        Dim note As New StringBuilder
        note.Append("<b>Note Composer</b>: " & User.Identity.Name & "<br/>")
        note.Append("<b>Date/Time</b>: " & Now & "<br/>")
        For Each c As Char In text.ToCharArray
            If (count <= 80) Or (c <> " ") Then
                note.Append(c)
            Else
                note.Append("<br/>" & c)
                count = 0
            End If
            count += 1
        Next
        Dim sql As String = _
            "update InvoiceOnly set Notes = '" & note.ToString & "' + '<br/><br/>' + Notes  where InvoiceOnlyID = " & RequestNumber

        Try
            cData.ExecuteSQLSupportNonQuery(sql)
        Catch ex As Exception
            LogEvent("Error updating notes for Invoice Only Request number " & RequestNumber, Diagnostics.EventLogEntryType.Error, 500001)
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
            " alert('Error occurred on update!');", True)
        End Try
    End Sub
    Protected Sub imgClose_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgClose.Click
        dgCommodityCodes.CurrentPageIndex = 0
        dgCommodityCodes.DataBind()
        txtCommoditySearch.Text = String.Empty
    End Sub

    Protected Sub txtCaseNumber_TextChanged(sender As Object, e As System.EventArgs) Handles txtCaseNumber.TextChanged
        If txtCaseNumber.Text <> "N\A" Or txtCaseNumber.Text <> "n\a" Then
            Try 'This is an async postback to determine if this case and vendor combo already exists. If so, notify user of that fact.
                Dim strSQL As String = "select InvoiceOnlyID from InvoiceOnly where CaseNumber = '" & txtCaseNumber.Text & "' and Vendor = '" & txtVendor.Text & "';"
                Dim caseId As Integer = cData.ExecuteSQLScaler(strSQL)
                If caseId > 0 Then
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
                        " alert('Case number " & txtCaseNumber.Text & " already exists on request #" & caseId & ", please verify this request has not already been entered!');", True)
                    txtCaseNumber.Text = String.Empty
                    txtCaseNumber.Focus()
                End If
            Catch ex As Exception
                Throw New System.Exception(ex.Message)
            End Try
        End If
    End Sub


    Protected Sub btnFileUpload_Click(sender As Object, e As EventArgs) Handles btnFileUpload.Click

        Dim strPath As String = Request.PhysicalApplicationPath & DocPath
        Dim strFile As String

        If flInvoice.HasFile Then
            'generate a unique file name
            strFile = "IOF" & Replace(Now.ToShortDateString, "/", "") & Replace(Replace(Now.ToLongTimeString, ":", ""), " ", "") & IO.Path.GetExtension(flInvoice.PostedFile.FileName)
            strPath += strFile

            Try
                flInvoice.SaveAs(strPath)
                FileName = strFile


            Catch ex As Exception
                LogEvent(ex.Message, Diagnostics.EventLogEntryType.Error, 500000)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
            " alert('Error has occurred loading file " & strPath & " error message: " & ex.Message + ");", True)
            End Try
        Else
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
" alert('Please select a file to upload!');", True)

        End If





    End Sub


    Protected Sub lbDeleteFile_Click(sender As Object, e As EventArgs) Handles lbDeleteFile.Click
        Dim strPath As String = Request.PhysicalApplicationPath & DocPath
        strPath += FileName
        FileName = String.Empty

        Try

            IO.File.Delete(strPath)

        Catch ex As Exception

        End Try
    End Sub

    Protected Function PrintForm() As String


        'fill the labels

        Dim cnt As Integer = 0
        Dim lc As Integer = 0
        Dim pc As Integer = 1
        Dim sb As New StringBuilder
        'Add the head
        sb.Append("<table width=""720px"" border=""0px"" cellpadding=""0"" cellspacing=""0"">")
        sb.Append("<tr width='100%' valign='top'><td align='center'>&nbsp;</td></tr>")
        sb.Append("<tr width='100%' valign='top' class='headLabelLarge'><td align='center' colspan='3'><b>INVOICE ONLY REQUISITION</b></td></tr>")
        sb.Append("<tr width='100%' valign='top' class='headLabelLarge'><td align='center' colspan='3'><b>Request #: " & RequestNumber & "</b></td></tr>")
        sb.Append("<tr width='100%' valign='top' class='headLabelLarge'><td align='center' colspan='3'><b>Status:</b>" & lblNotify.Text & "</td></tr>")
        sb.Append("<tr width='100%' valign='top'><td align='center' colspan='3'>&nbsp;</td></tr>")
        sb.Append("<tr><td colspan='2' valign='top'><b>PO Number: </b>" & txtPO.Text & "</td><td align='right'><b>Date Printed: </b>" & Now.ToShortDateString & "</tr>")
        sb.Append("<tr width='100%' valign='top'><td colspan='3'><hr></td></tr>")
        sb.Append("<tr width='100%' valign='top'><td>&nbsp;</td></tr>")
        sb.Append("<tr valign='top'><td><b>Vendor: </b><br>" & txtVendor.Text & "</td><td><b>Manufacturer: </b><br>" & txtManufacturer.Text & "</td><td><b>Location: </b><br>" & txtLocation.Text & "</td></tr>")
        sb.Append("<tr width='100%' valign='top'><td>&nbsp;</td></tr>")
        sb.Append("<tr valign='top'><td><b>Consignment Item: </b><br>" & IIf(rbConsignment.SelectedValue = 0, "No", "Yes") & "</td><td colspan='2'><b>Cart Number: </b><br>" & txtCartNumber.Text & "</td></tr>")
        sb.Append("<tr width='100%' valign='top'><td>&nbsp;</td></tr>")
        sb.Append("<tr valign='top'><td><b>Sales Rep: </b><br>" & txtContact.Text & "</td><td colspan='2'><b>Phone: </b><br>" & txtPhone.Text & "</td></tr>")
        sb.Append("<tr width='100%' valign='top'><td>&nbsp;</td></tr>")
        sb.Append("<tr valign='top' width='100%' style='color:white; background-color:black;' ><td colspan='3'>PATIENT CHARGE INFORMATION</td></tr>")
        sb.Append("<tr valign='top'><td><b>Patient Name: </b><br>" & txtPatientName.Text & "</td><td colspan='2'><b>Surgery Date: </b><br>" & txtSurgeryDate.Text & "</td></tr>")
        sb.Append("<tr valign='top' width='100%'><td>&nbsp;</td></tr>")
        sb.Append("<tr valign='top'><td colspan='3'><b>Doctor Name: </b><br>" & txtDoctor.Text & "</td></tr>")
        sb.Append("<tr width='100%' valign='top'><td colspan='3'><hr></td></tr>")
        sb.Append("</table>")
        sb.Append("<table width=""680px"" border=""0px"" cellpadding=""0"" cellspacing=""0"">")
        sb.Append("<tr align='left'><th>PMM<br>Number</th><th>Product<br>Number</th><th>Qty</th><th>UOM</th><th>Price</th><th>Commodity<br>Code</th><th>Expense<br>Code</th><th>Chargeable</th><th>Implant</th><tr>")
        If itemCollection.Rows.Count <> 0 Then
            For Each dr As DataRow In itemCollection.Rows
                sb.Append("<tr><td align='center'>" & dr("ItemNumber") & "</td><td align='center'>" & dr("ProductNumber") & "</td><td align='center'>" & dr("ConversionQty") & "</td><td align='center'>" & dr("DispensingUOM") & "</td><td align='center'>" & dr("Price") & "</td><td align='center'>" & dr("CommodityCode") & "</td><td align='center'>" & dr("ExpenseCode") & "</td><td align='center'>" & dr("Chargeable") & "</td><td align='center'>" & dr("Implant") & "</td><tr>")
                sb.Append("<tr  valign='top'><td colspan='9'>" & dr("Description") & "</td></tr>")
                sb.Append("<tr valign='top' width='100%'><td>&nbsp;</td></tr>")
            Next
        Else
            sb.Append("<tr><td colspan='9'>No Items on Form</td></tr>")
        End If
        sb.Append("</table>")

        Return sb.ToString



    End Function

    Public Function GetClass(ByVal str As String) As String
        Dim rtn As String = "itemlistitem"
        If (str = "-") Then
            rtn = "printRowAlt"
        End If

        Return rtn
    End Function

    Public Function GetValue(ByVal str As String) As String
        If str = "-" Then
            str = "&nbsp;"
        End If

        Return str
    End Function


    Protected Sub imPrint_Click(sender As Object, e As ImageClickEventArgs) Handles imPrint.Click
        If itemCollection.Rows.Count <> 0 Then

            Session("IOPrintHTML") = PrintForm()

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
" window.open('InvoiceOnlyPrint.aspx','_blank');", True)
        Else
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), _
" alert('Error printing document!');", True)
        End If


    End Sub

    Protected Function itemChanged(itemNumber As String, dr As DataRow) As Boolean
        Dim changed As Boolean = False
        If dr("ItemNumber") <> String.Empty Then
            Dim item As DataSet = cData.GetItemDetail(dr("ItemNumber"), "")
            If item.Tables(0).Rows.Count > 0 Then
                If (dr("ItemNumber") <> item.Tables(0).Rows(0)("ITEM_NO")) Or (dr("ProductNumber") <> item.Tables(0).Rows(0)("CTLG_NO")) Or _
                          (dr("Description") <> item.Tables(0).Rows(0)("DESCR")) Or _
                        (dr("CommodityCode") <> item.Tables(0).Rows(0)("COMM_CODE")) Or (dr("ExpenseCode") <> item.Tables(0).Rows(0)("EXP_CODE")) Or _
                        (dr("Chargeable") <> CBool(item.Tables(0).Rows(0)("CHARGEABLE"))) Or (dr("Implant") <> CBool(item.Tables(0).Rows(0)("IMPLANT_IND"))) Then
                    changed = True

                    'log the changes for later notification
                    chgItemString.Append("<tr><td>&nbsp;</td></tr>")
                    chgItemString.Append("<tr><td><b>******* Summary of Req Item " & dr("ItemNumber") & " Changes *******</b></td></tr>")
                    If dr("ItemNumber") <> item.Tables(0).Rows(0)("ITEM_NO") Then
                        chgItemString.Append("<tr><td><b>Req Item:</b> " & dr("ItemNumber") & " <b> <------> PMM Item:</b> " & item.Tables(0).Rows(0)("ITEM_NO") & "</td></tr>")
                    End If
                    If dr("ProductNumber") <> item.Tables(0).Rows(0)("CTLG_NO") Then
                        chgItemString.Append("<tr><td><b>Req Product:</b> " & dr("ProductNumber") & "<b> <------> PMM Product:</b> " & item.Tables(0).Rows(0)("CTLG_NO") & "</td></tr>")
                    End If
                    If dr("Description") <> item.Tables(0).Rows(0)("DESCR") Then
                        chgItemString.Append("<tr><td><b>Req Description:</b> " & dr("Description") & " <------> " & vbCrLf & "<b>PMM Description:</b> " & item.Tables(0).Rows(0)("DESCR") & "</td></tr>")
                    End If
                    If dr("CommodityCode") <> item.Tables(0).Rows(0)("COMM_CODE") Then
                        chgItemString.Append("<tr><td><b>Req Commodity Code:</b> " & dr("CommodityCode") & "<b> <------> PMM Commodity Code:</b> " & item.Tables(0).Rows(0)("COMM_CODE") & "</td></tr>")
                    End If
                    If dr("ExpenseCode") <> item.Tables(0).Rows(0)("EXP_CODE") Then
                        chgItemString.Append("<tr><td><b>Req Expense Code:</b> " & dr("ExpenseCode") & "<b> <------> PMM Expense Code:</b> " & item.Tables(0).Rows(0)("EXP_CODE") & "</td></tr>")
                    End If
                    If dr("Chargeable") <> CBool(item.Tables(0).Rows(0)("CHARGEABLE")) Then
                        chgItemString.Append("<tr><td><b>Req Chargeable:</b> " & dr("Chargeable") & "<b> <------> PMM Chargeable:</b> " & CBool(item.Tables(0).Rows(0)("CHARGEABLE")) & "</td></tr>")
                    End If
                    If dr("Implant") <> CBool(item.Tables(0).Rows(0)("IMPLANT_IND")) Then
                        chgItemString.Append("<tr><td><b>Req Implant:</b> " & dr("Implant") & "<b> <------> PMM Implant:</b> " & CBool(item.Tables(0).Rows(0)("IMPLANT_IND")) & "</td></tr>")
                    End If
                    chgItemString.Append("<tr><td>---------------------------------------------------------------------</td></tr>")

                End If
                End If
            End If
            Return changed
    End Function
End Class

