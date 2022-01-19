Imports System.Data
Imports cUtilities
Imports cData

Partial Class Site_PMMSupplyTypes
    Inherits System.Web.UI.Page

    Public Property CountResults() As Integer
        'Stores item count for datagrid display
        Get
            If Not (ViewState("CountResults") Is Nothing) Then
                Return ViewState("CountResults")
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("CountResults") = value
        End Set
    End Property
    Public Property dsItems() As DataSet
        'Dataset for storing items
        Get
            Return Session("dsItems")
        End Get
        Set(ByVal value As DataSet)
            Session("dsItems") = value
        End Set
    End Property
    Public Property PageDirty() As Boolean
        'Signals that dsItems() should be refeshed from database
        Get
            Return ViewState("PageDirty")
        End Get
        Set(ByVal value As Boolean)
            ViewState("PageDirty") = value
        End Set
    End Property
    Public Property SearchItem() As String
        'Stores users search values
        Get
            Return Session("SearchItem")
        End Get
        Set(ByVal value As String)
            Session("SearchItem") = value
        End Set
    End Property
    Private ReadOnly Property dsEpicSupplyItems() As DataSet
        'Property to store drop down items, prevents multiple trips to database
        Get
            If Session("dsEpicSupplyItems") Is Nothing Then
                Session("dsEpicSupplyItems") = cData.GetEpicSupplyTypes
            End If
            Return Session("dsEpicSupplyItems")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then
            'Create the page header
            CType(Page.Master.FindControl("MenuHead"),  _
                Web.UI.MiscControls.MenuHeadControl).ImageText = "Epic Item Master Supply Types"
            FillItemPageCount()
            'Check for user settings cookie
            If Not (Request.Cookies("ItemsPerPage") Is Nothing) Then
                ddlItemsPerPage.SelectedValue = Request.Cookies("ItemsPerPage").Value
                dlItems.PageSize = ddlItemsPerPage.SelectedValue
            End If
            LoadItems()
        End If
    End Sub

    Protected Sub LoadItems()
        If (PageDirty = True) Or (Session("dsItems") Is Nothing) Then
            dsItems = cData.GetEpicInterfaceItems(CountResults)
            PageDirty = False
        End If
        With dlItems
            .DataSource = dsItems
            .DataBind()
        End With
    End Sub
    Protected Sub LoadItems(ByVal itemNumber As String)
        If itemNumber <> String.Empty Then
            Try
                dsItems = cData.GetEpicInterfaceItems(CountResults, itemNumber)
                With dlItems
                    .DataSource = dsItems
                    .DataBind()
                End With
            Catch ex As Exception
                LogEvent(ex.Message, Diagnostics.EventLogEntryType.Error, 1)
            End Try

        End If

    End Sub
    Protected Sub FillItemPageCount()
        ddlItemsPerPage.Items.Add(New ListItem("10", "10"))
        ddlItemsPerPage.Items.Add(New ListItem("20", "20"))
        ddlItemsPerPage.Items.Add(New ListItem("30", "30"))

    End Sub

    Protected Sub dlItems_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dlItems.ItemCommand
        If (e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem) Then
            'generate the row values
            Dim ddlItemTypes As Web.UI.MiscControls.DataListDropDown = e.Item.FindControl("ddlItemTypes")
            Dim ddlSupplyTypes As Web.UI.MiscControls.DataListDropDown = e.Item.FindControl("ddlTypes")
            Dim lblItemId As Label = e.Item.FindControl("lblItemId")
            Dim lblItemIdB As Label = e.Item.FindControl("lblItemIdB")
            Dim lblItemNo As Label = e.Item.FindControl("lblItemNo")
            'Local variables
            Dim oldSupplyType As String = String.Empty
            Dim oldItemType As String = String.Empty

            If ddlItemTypes.SelectedIndex <> 0 Then
                If ddlSupplyTypes.SelectedIndex <> 0 Then
                    Try
                        'Update the record
                        cData.UpdateEpicMatchedItems(CInt(lblItemId.Text), CInt(lblItemIdB.Text), _
                            CInt(ddlSupplyTypes.SelectedValue), CInt(ddlItemTypes.SelectedValue), User.Identity.Name)
                        'Loop thru the recordset to get the old values for logging
                        For Each r As DataRow In dsItems.Tables(0).Rows
                            If r("ITEM_NO") = lblItemNo.Text Then
                                'Get the old values for this updated item
                                oldItemType = IIf(IsDBNull(r("ITEM_TYPE_ID")), String.Empty, r("ITEM_TYPE_ID"))
                                oldSupplyType = IIf(IsDBNull(r("SUPPLY_TYPE_ID")), String.Empty, r("SUPPLY_TYPE_ID"))
                            End If
                        Next
                        'Log the changes
                        cData.InsertChangeLog("/EpicSupplyTypes.aspx", User.Identity.Name, "", "EPIC_MATCHED_SUPPLY_TYPES", _
                                                 lblItemId.Text, "ITEM_TYPE_ID", oldItemType, ddlItemTypes.SelectedValue, "SUCCESS")
                        cData.InsertChangeLog("/EpicSupplyTypes.aspx", User.Identity.Name, "", "EPIC_MATCHED_ITEM_TYPES", _
                                    lblItemId.Text, "SUPPLY_TYPE_ID", oldSupplyType, ddlSupplyTypes.SelectedValue, "SUCCESS")

                        lblMessage.Text = "Item '" & lblItemNo.Text & "' successfully updated."
                        PageDirty = True
                    Catch ex As Exception
                        lblMessage.Text = ex.Message
                    End Try
                Else
                    lblMessage.Text = "Please select an Item Type for item '" & lblItemNo.Text & "' before updating!"
                    ddlItemTypes.Focus()
                End If
            Else
                lblMessage.Text = "Please select a Supply Type for item '" & lblItemNo.Text & "' before updating!"
                ddlSupplyTypes.Focus()
            End If

        End If
    End Sub


    Protected Sub dlItems_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dlItems.ItemCreated
        DataGridItemCreated(sender, e, CountResults)
    End Sub
    Protected Sub dlItems_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dlItems.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem) Then
            Dim lblTypes As Label = CType(e.Item.FindControl("lblTypes"), Label)
            Dim lblItemTypes As Label = CType(e.Item.FindControl("lblItemType"), Label)
            'Types
            With CType(e.Item.FindControl("ddlTypes"), Web.UI.MiscControls.DataListDropDown)
                .DataSource = dsEpicSupplyItems().Tables("tblSupplyTypes")
                .DataTextField = ("SUPPLY_TYPE_TEXT")
                .DataValueField = ("SUPPLY_TYPE_ID")
                .DataBind()
                .Items.Insert(0, New ListItem("Select Supply Type", "-1"))
                .SelectedValue = IIf(lblTypes.Text <> String.Empty, lblTypes.Text, "-1")
            End With

            'Item Types
            With CType(e.Item.FindControl("ddlItemTypes"), Web.UI.MiscControls.DataListDropDown)
                .DataSource = dsEpicSupplyItems().Tables("tblItemTypes")
                .DataTextField = ("ITEM_TYPE")
                .DataValueField = ("ITEM_TYPE_ID")
                .DataBind()
                .Items.Insert(0, New ListItem("Select Type", "-1"))
                .SelectedValue = IIf(lblItemTypes.Text <> String.Empty, lblItemTypes.Text, "-1")
            End With
            'Alternating Row style
            If ((CInt(e.Item.ItemIndex) Mod 2) = 0) Then
                'TO DO: Research a way to use the css object instead of hard coding this value
                Dim tr As HtmlTableRow = CType(e.Item.FindControl("trItem"), HtmlTableRow)
                tr.Attributes.CssStyle.Add("background-color", "#BCD4E5")
                tr.Attributes.CssStyle.Add("vertical-align", "middle")
            Else
                CType(e.Item.FindControl("trItem"), HtmlTableRow).Attributes.CssStyle.Add("vertical-align", "middle")
            End If

        End If

    End Sub

    Protected Sub dlItems_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dlItems.PageIndexChanged
        Try
            dlItems.CurrentPageIndex = e.NewPageIndex
            If Not (SearchItem Is Nothing) And (SearchItem <> String.Empty) Then
                LoadItems(SearchItem)
            Else
                LoadItems()
            End If
        Catch ex As Exception
            LogEvent(ex.Message, Diagnostics.EventLogEntryType.Error, 1)
        End Try


    End Sub

    Protected Sub ddlItemsPerPage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlItemsPerPage.SelectedIndexChanged
        dlItems.CurrentPageIndex = 0
        dlItems.PageSize = ddlItemsPerPage.SelectedValue
        Response.Cookies("ItemsPerPage").Value = ddlItemsPerPage.SelectedValue
        Response.Cookies("ItemsPerPage").Expires = DateAdd(DateInterval.Month, 1, Now)
        LoadItems()
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        'Reset the page index, new search.
        dlItems.CurrentPageIndex = 0
        SearchItem = InitControl.Value
        lblResults.Text = "Query Results for Search String: '" & SearchItem & "'"
        LoadItems(SearchItem)
    End Sub

    Protected Sub btnSubmitClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmitClear.Click
	dlItems.CurrentPageIndex = 0
        PageDirty = True
        Session("dsEpicSupplyItems") = Nothing
        Session("dsItems") = Nothing
        dlItems.DataSource = Nothing
        SearchItem = String.Empty
        dlItems.DataBind()
        dlItems.CurrentPageIndex = 0
        LoadItems()
        InitControl.Value = String.Empty
        lblResults.Text = "Items Requiring Supply Type"
    End Sub

    Protected Function GetDisplayDate(ByVal dte As String) As String
        If (dte <> String.Empty) And ((dte <> "1/1/1900") And (dte <> "01/01/1900")) Then
            dte = CDate(dte).ToShortDateString
        Else
            dte = String.Empty
        End If
        Return dte
    End Function

    Protected Function GetDisplayUser(ByVal usr As String) As String
        Return Left(TrimUsr(usr), 12)
    End Function

    Protected Function TrimUsr(ByVal usr As String) As String
        'Remove the users domain if it exists
        If InStr(usr, "\") Then
            Dim chra() As Char = usr.ToCharArray
            usr = String.Empty
            Dim ad As Boolean = False
            For Each c As Char In chra
                If ad Then
                    usr += c
                End If
                If (c = "\") Then
                    ad = True
                End If
            Next
        End If

        Return usr
    End Function

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Not cData.GetUserAuthorization(User.Identity.Name, "/EpicSupplyTypes.aspx") Then
            LogPageHit("/EpicSupplyTypes.aspx", User.Identity.Name, 0, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            Server.Transfer("../Site/na.aspx?u=" & User.Identity.Name)
        Else
            LogPageHit("/EpicSupplyTypes.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
        End If
    End Sub

End Class
