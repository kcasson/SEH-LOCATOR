Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Text
Imports cData

Partial Class SPDPerpetualInventory
    Inherits System.Web.UI.Page
    '************************************************************************************************
    '*  NAME: PMMInventoryQuantities.aspx.vb
    '*	DESC: This page displays available inventory based on criteria entered.
    '*	AUTHOR: kcasson 
    '*	DATE: 4/9/2009
    '*		
    '*		Modifications:
    '*			-none
    '************************************************************************************************/
    Private ReadOnly Property CurrentUser() As cUserInfo
        Get
            If Not IsNothing(Session("oUser")) Then
                Return Session("oUser")
            Else
                Return New cUserInfo
            End If
        End Get
    End Property
    Private Property itemDO() As cItemBinLocations
        Get
            If Not IsNothing(Session("itemDO")) Then
                Return Session("itemDO")
            Else
                Return New cItemBinLocations
            End If
        End Get
        Set(ByVal value As cItemBinLocations)
            Session("itemDO") = value
        End Set
    End Property
    Private Property itemInvoices() As DataSet
        Get
            If Not IsNothing(Session("itemInvoices")) Then
                Return Session("itemInvoices")
            Else
                Return New DataSet
            End If

        End Get
        Set(ByVal value As DataSet)
            Session("itemInvoices") = value
        End Set
    End Property
    Private Property itemPOs() As DataSet
        Get
            If Not IsNothing(Session("itemPOs")) Then
                Return Session("itemPOs")
            Else
                Return New DataSet
            End If

        End Get
        Set(ByVal value As DataSet)
            Session("itemPOs") = value
        End Set
    End Property
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'LoadItems()
        If Not IsPostBack Then
            LogPageHit("/PMMInventoryQuantities", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
        Else
            'ViewState("ddlSelected") = InitControl.SelectedIndex
            'ViewState("descText") = hdisc.Value
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        ClearItemInfo()
        itemDO = New cItemBinLocations(txtMfrNo.Value, InitControl.Value, txtDesc.Value, IIf(Not IsNothing(Session("oUser")), Session("oUser"), New cUserInfo()))
        'Pass the user information to the class
        If itemDO.Count > 0 Then
            With lvItems
                .SelectedIndex = -1
                .DataSource = itemDO.Items
                .DataBind()
            End With
            If itemDO.Count = 1 Then
                With lvItems
                    .SelectedIndex = 0
                    .DataSource = itemDO.Items
                    .DataBind()
                End With
                loadItemDetails(0)
            End If
        Else
            'Clear the Items
            With lvItems
                .DataSource = Nothing
                .DataBind()
            End With
            ClearItemInfo()
            lblMessage.Text = "No Items found matching the criteria entered!"
        End If

    End Sub

    Public Sub LoadItems()
        'With InitControl
        '    .DataSource = CType(Application("dsItems"), DataSet)
        '    .DataValueField = "Item_ID"
        '    .DataTextField = "ItemInfo"
        '    .DataBind()
        '    .Items.Insert(0, New ListItem("Select an item or enter description", ""))
        'End With
        'If ViewState("ddlSelected") <> String.Empty Then
        '    InitControl.SelectedIndex = ViewState("ddlSelected")
        'End If
        'If ViewState("descText") <> String.Empty Then
        '    txtDesc.Value = ViewState("descText")
        'End If
    End Sub

    Protected Sub lvItems_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles lvItems.ItemCommand

        If e.CommandName <> "Select" Then
            If itemDO.ItemCurrentSort = SortDirection.Ascending Then
                itemDO.Items.Sort(GetColumnInt(e.CommandArgument), cItem.SortDirection.DESC)
                itemDO.ItemCurrentSort = SortDirection.Descending
            Else
                itemDO.Items.Sort(GetColumnInt(e.CommandArgument), cItem.SortDirection.ASC)
                itemDO.ItemCurrentSort = SortDirection.Ascending
            End If
        End If

        With lvItems
            .DataSource = itemDO.Items
            .SelectedIndex = -1
            .DataBind()
        End With
        ClearItemInfo()
    End Sub

    'Private Function GetStatusCodeDesc(ByVal strErrorCode As String) As String
    '    'Get the error code descriptions from the enumeration and return the description.
    '    Dim LineErrors As String() = System.Enum.GetNames(GetType(OrderLine.lineErrors))

    '    Return LineErrors(strErrorCode)

    'End Function

    Protected Sub lvItems_PagePropertiesChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvItems.PagePropertiesChanged
        ClearItemInfo()
        With lvItems
            .SelectedIndex = -1
            .DataSource = itemDO.Items
            .DataBind()
        End With
    End Sub

    Protected Sub lvItems_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewSelectEventArgs) Handles lvItems.SelectedIndexChanging

        With lvItems
            .SelectedIndex = e.NewSelectedIndex
            .DataSource = itemDO.Items
            .DataBind()
        End With

        If e.NewSelectedIndex <> -1 Then
            loadItemDetails(e.NewSelectedIndex)
        Else
            ClearItemInfo()
        End If
    End Sub
    Private Sub loadItemDetails(ByVal pIndex As Integer)
        Try
            'Item selected, load the details
            pItemInfo.Visible = True
            Dim item As cItem = itemDO.Items(lvItems.Items(pIndex).DataItemIndex)
            lblItemNo.Text = item.ItemNumber
            lblChargeInd.Text = item.Chargeable
            lblFinanceDate.Text = IIf(item.DateLastSentToFinance <> "1/1/1900", Format(item.DateLastSentToFinance, "d"), " ")
            lblLawsonDetail.Text = item.LawsonNumber
            lblInterface.Text = item.IsPendingFinanceInterace
            lblComCode.Text = item.ComdtyCode
            lblSupplyType.Text = item.SupplyType
            lblItemType.Text = item.ItemType

            'load the vendors
            With ddlVendors
                .DataSource = item.Vendors
                .DataTextField = ("VendorName")
                .DataValueField = ("ItemVendId")
                .DataBind()
            End With
            Select Case item.Status
                Case Is = 1
                    lblStatus.ForeColor = Drawing.Color.Green
                Case Is = 2
                    lblStatus.ForeColor = Drawing.Color.DarkOrange
                Case Is = 3
                    lblStatus.ForeColor = Drawing.Color.Red
                Case Else
                    lblStatus.ForeColor = Drawing.Color.Tomato
            End Select
            lblStatus.Text = item.StatusText

            If item.Vendors.Count > 0 Then
                'load the vendor packaging data
                With gvPackaging
                    .DataSource = item.Vendors(ddlVendors.SelectedIndex).Packaging
                    .DataBind()
                End With
            End If

            If (item.Bins.Count > 0) And (item.Status <> 3) Then
                'load the inventory data
                With gvInventory
                    .DataSource = item.Bins
                    .DataBind()
                End With
            End If

            If item.ParLocations.Count > 0 Then
                'load the par locations
                With gvParLocations
                    .DataSource = item.ParLocations
                    .DataBind()
                End With
            End If

        Catch ex As Exception
            cUtilities.LogEvent("NOTE: This exception has been handled. CLASS: PMMInventoryQuantities.aspx.vb FUNCTION: LoadItemDetails ERROR: " & ex.Message, Diagnostics.EventLogEntryType.Error, 50000)

        End Try
    End Sub
    Private Sub ClearItemInfo()
        pItemInfo.Visible = False

        'Clear the packaging data
        With gvPackaging
            .DataSource = Nothing
            .DataBind()
        End With
        'Clear the inventory data
        With gvInventory
            .DataSource = Nothing
            .DataBind()
        End With
        'Clear the Parlocation data
        With gvParLocations
            .DataSource = Nothing
            .DataBind()
        End With


    End Sub

    Protected Sub ddlVendors_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlVendors.SelectedIndexChanged
        Dim item As cItem = itemDO.Items(lvItems.Items(lvItems.SelectedIndex).DataItemIndex)
        With gvPackaging
            .DataSource = item.Vendors(ddlVendors.SelectedIndex).Packaging
            .DataBind()
        End With
    End Sub

    Private Function GetColumnInt(ByVal colName As String) As Integer
        'This function returns the enumeration value from a string. If the name is not found a (-1) is returned.
        Dim a() As String = System.Enum.GetNames(GetType(cItem.SortBy))
        Dim count As Integer = 0
        Dim rtn As Integer = -1
        For Each s As String In a
            If s = colName Then
                rtn = count
                Exit For
            End If
            count += 1
        Next

        Return rtn
    End Function

    Private Sub enableEntry()
        InitControl.Disabled = False
        txtDesc.Disabled = False
        txtMfrNo.Disabled = False
        lvItems.Enabled = True
    End Sub

    Private Sub disableEntry()
        InitControl.Disabled = True
        txtDesc.Disabled = True
        txtMfrNo.Disabled = True
        lvItems.Enabled = False
    End Sub

    Protected Sub lvPOs_PagePropertiesChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvPOs.PagePropertiesChanged
        With lvPOs
            .DataSource = itemPOs
            .DataBind()
        End With
    End Sub

    Protected Sub lvInvoices_PagePropertiesChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvInvoices.PagePropertiesChanged
        With lvInvoices
            .DataSource = itemInvoices
            .DataBind()
        End With
    End Sub


    Protected Sub lkPO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkPO.Click

        'Retrieve PO data
        itemPOs = GetPOsByItemNumber(lblItemNo.Text, CurrentUser.NetworkName)

        With lvPOs
            .DataSource = itemPOs
            .DataBind()
        End With
        disableEntry()
        If itemPOs.Tables(0).Rows.Count = 0 Then
            trNoPORecords.Style.Item("display") = "block"
        End If
        'set div classes
        divPO.Attributes("class") = "showScreen"
        Dim divMain As HtmlGenericControl = Me.Master.FindControl("divMain")
        divMain.Attributes("class") = "blockScreen"


        lblPODisplayHead.Text = "Item " & lblItemNo.Text & " Purchase Order Information"

    End Sub

    Protected Sub lkInvoices_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkInvoices.Click
        itemInvoices = GetInvoicesByItemNumber(lblItemNo.Text, CurrentUser.NetworkName)
        With lvInvoices
            .DataSource = itemInvoices
            .DataBind()
        End With
        disableEntry()
        If itemInvoices.Tables(0).Rows.Count = 0 Then
            trNoInvRecords.Style.Item("display") = "block"
        End If
        divInv.Attributes("class") = "showScreen"
        Dim divMain As HtmlGenericControl = Me.Master.FindControl("divMain")
        divMain.Attributes("class") = "blockScreen"
        ''center window on users browser
        lblInvDisplayHead.Text = "Item " & lblItemNo.Text & " Invoice Information"
    End Sub

    Protected Sub btnClosePO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClosePO.Click

        enableEntry()
        Dim divMain As HtmlGenericControl = Me.Master.FindControl("divMain")
        divMain.Attributes("class") = String.Empty
        divPO.Attributes("class") = "noDisplay"

    End Sub
    Protected Sub lkCloseInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lkCloseInvoice.Click
        enableEntry()
        Dim divMain As HtmlGenericControl = Me.Master.FindControl("divMain")
        divMain.Attributes("class") = String.Empty
        divInv.Attributes("class") = "noDisplay"
    End Sub
End Class
