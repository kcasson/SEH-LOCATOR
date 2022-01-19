Imports cData
Imports cUtilities
Imports System.Data

Imports Web.UI.MiscControls
Partial Class Site_PMMStLItemMatch
    Inherits System.Web.UI.Page

    Private Property StLItemDsData() As DataSet
        Get
            If Not Session("StLItemDsData") Is Nothing Then
                Return Session("StLItemDsData")
            Else
                Dim ds As New DataSet
                Return ds
            End If

        End Get
        Set(ByVal value As DataSet)
            Session("StLItemDsData") = value
        End Set
    End Property
    Private Property StLItemPMMData() As DataSet
        Get
            If Not (Session("StLItemPMMData") Is Nothing) Then
                Return Session("StLItemPMMData")
            Else
                Dim ds As New DataSet
                Return ds
            End If
        End Get
        Set(ByVal value As DataSet)
            Session("StLItemPMMData") = value
        End Set
    End Property
    Private Property dSPageIndex() As Integer
        Get
            If Not (ViewState("dSPageIndex") Is Nothing) Then
                Return ViewState("dSPageIndex")
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("dSPageIndex") = value
        End Set
    End Property
    Private Property PMMPageIndex() As Integer
        Get
            If Not (ViewState("PMMPageIndex") Is Nothing) Then
                Return ViewState("PMMPageIndex")
            Else
                Return 0
            End If

        End Get
        Set(ByVal value As Integer)
            ViewState("PMMPageIndex") = value
        End Set
    End Property
    Private Property dsSelectedIndex() As Integer
        Get
            If Not (ViewState("dsSelectedIndex") Is Nothing) Then
                Return ViewState("dsSelectedIndex")
            Else
                Return -1
            End If

        End Get
        Set(ByVal value As Integer)
            ViewState("dsSelectedIndex") = value
        End Set
    End Property
    Private Property PMMSelectedIndex() As Integer
        Get
            If Not (ViewState("PMMSelectedIndex")) Then
                Return ViewState("PMMSelectedIndex")
            Else
                Return -1
            End If

        End Get
        Set(ByVal value As Integer)
            ViewState("PMMSelectedIndex") = value
        End Set
    End Property
    Public Property dSCountResults() As Integer
        Get
            Return Session("dSCountResults")
        End Get
        Set(ByVal value As Integer)
            Session("dSCountResults") = value
        End Set
    End Property
    Public Property dsPMMCountResults() As Integer
        Get
            Return Session("dsPMMCountResults")
        End Get
        Set(ByVal value As Integer)
            Session("dsPMMCountResults") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Set initial focus
        txtDsItem.Focus()
        If Not IsPostBack Then
            If Not (Request.Cookies("RowCountValue") Is Nothing) Then
                ddlRows.SelectedValue = Request.Cookies("RowCountValue").Value
            End If
            LoadPage()
        End If
    End Sub
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        'Load the page name 
        CType(Page.Master.FindControl("MenuHead"), MenuHeadControl).ImageText = "PMM Item Matching"

    End Sub

    Private Sub LoadPage()
        If Not IsPostBack Then
            Dim oUser As cUserInfo = Session("oUser")
            If Not IsPostBack Then
                If (cData.GetUserAuthorization(oUser.NetworkId, "/PMMStLItemMatch.aspx") Or oUser.IsAdmin) Then
                    LogPageHit("/PMMStLItemMatch.aspx", oUser.NetworkId, 1, Server.MachineName, oUser.IPAddress)
                Else
                    LogPageHit("/PMMStLItemMatch.aspx", oUser.NetworkId, 0, Server.MachineName, oUser.IPAddress)
                    Server.Transfer("na.aspx")
                End If

            End If
            'load the page and set defaults
            With ddlData
                .DataSource = GetStLukeItemMatchDataSets.Tables("DataSets")
                .DataValueField = "DATASET"
                .DataTextField = "DATASET"
                .DataBind()
                .SelectedIndex = 1
            End With
            With ddlSubset
                .DataSource = GetStLukeItemMatchDataSets.Tables("SubSets")
                .DataValueField = "SUBSET"
                .DataTextField = "SUBSET"
                .DataBind()
                .Items.Insert(0, (New ListItem("", "-1")))
            End With
            'chkMatched.Checked = True
            PopulateDsItems()
            dsName.Text = ddlData.SelectedItem.ToString
        End If

    End Sub

    Protected Sub dgDsItems_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDsItems.ItemCommand
        If (e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem) Then
            dsSelectedIndex = e.Item.DataSetIndex
        End If
        dgDsItems.EditItemIndex = e.Item.ItemIndex
        dgDsItems.DataSource = StLItemDsData
        dgDsItems.DataBind()
    End Sub
    Protected Sub dgDsItems_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDsItems.ItemCreated
        DataGridItemCreated(sender, e, dSCountResults)
    End Sub

    Protected Sub dgPMMItems_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPMMItems.ItemCreated
        DataGridItemCreated(sender, e, dsPMMCountResults)
    End Sub
    Protected Sub dgPMMItems_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgPMMItems.ItemCommand
        If (e.Item.ItemType = ListItemType.Item) Or (e.Item.ItemType = ListItemType.AlternatingItem) Then
            PMMSelectedIndex = e.Item.DataSetIndex
        End If
        dgPMMItems.EditItemIndex = e.Item.ItemIndex
        dgPMMItems.DataSource = StLItemPMMData
        dgPMMItems.DataBind()
    End Sub

    Private Sub PopulateDsItems()

        Dim sqlsb As New StringBuilder
        'Clear the edit item
        dgDsItems.EditItemIndex = -1
        If dSPageIndex = 0 Then
            dgDsItems.CurrentPageIndex = 0
        End If
        dsSelectedIndex = -1
        PMMPageIndex = 0
        sqlsb.Append( _
         "SELECT TOP " & ddlRows.SelectedValue & " PMM.dbo.ITEM.ITEM_ID,PMM.dbo.ITEM.ITEM_NO AS PMM_NBR,RTRIM(STL_ITEM.ITEM_NO) AS STL_NBR, " & _
         "PMM.dbo.MFR.NAME AS PMM_MFR,STL_ITEM.MFR_NO AS STL_MFR,STL_ITEM.MFR_CTLG_NO AS STL_CTLG_NO, " & _
         "PMM.dbo.ITEM.CTLG_NO AS PMM_MFR_NO,STL_VEND.NAME AS VENDOR, " & _
         "STL_ITEM.VENDOR_CTLG_NO AS VEND_CAT,STL_ITEM.DESCR AS STL_DESC,  " & _
         "PMM.dbo.ITEM.DESCR AS PMM_DESC  " & _
               " FROM STL_ITEM " & _
         "LEFT JOIN PMM.dbo.ITEM ON STL_ITEM.PMM_ITEM_ID= PMM.dbo.ITEM.ITEM_ID  " & _
         "LEFT JOIN PMM.dbo.MFR ON PMM.dbo.ITEM.MFR_ID=PMM.dbo.MFR.MFR_ID  " & _
         "LEFT JOIN STL_VEND ON STL_ITEM.VENDOR_ID=STL_VEND.VEND_ID AND STL_VEND.DATASET=STL_ITEM.DATASET   ")
        If ddlSubset.SelectedValue <> "-1" Then
            sqlsb.Append("INNER JOIN STL_SUBSET ON STL_ITEM.ITEM_NO = STL_SUBSET.ITEM_NO AND STL_SUBSET.SUBSET = '" & ddlSubset.SelectedValue & "'")
        End If

        sqlsb.Append(" WHERE (SUPPRESS IS NULL OR SUPPRESS='') AND STL_ITEM.DATASET = '" & ddlData.SelectedValue & "'")
        'Build where clause from page
        If txtDsItem.Text <> String.Empty Then
            sqlsb.Append(" AND STL_ITEM.ITEM_NO LIKE '" & UCase(txtDsItem.Text) & "%'")
        End If
        If txtDsMfr.Text <> String.Empty Then
            sqlsb.Append(" AND STL_ITEM.MFR_CTLG_NO LIKE '%" & UCase(txtDsMfr.Text) & "%'")
        End If
        If txtDsPmmItem.Text <> String.Empty Then
            sqlsb.Append(" AND PMM.dbo.ITEM.ITEM_NO LIKE '%" & txtDsPmmItem.Text & "%'")
        End If
        If txtDsCat.Text <> String.Empty Then
            sqlsb.Append(" AND STL_ITEM.VENDOR_CTLG_NO LIKE '%" & UCase(txtDsCat.Text) & "%'")
        End If
        If txtDsVend.Text <> String.Empty Then
            sqlsb.Append(" AND STL_VEND.NAME LIKE '%" & UCase(txtDsVend.Text) & "%'")
        End If
        If txtDsDesc.Text <> String.Empty Then
            sqlsb.Append(" AND STL_ITEM.DESCR LIKE '%" & UCase(txtDsDesc.Text) & "%'")
        End If
        If chkMatched.Checked Then
            sqlsb.Append(" AND ((STL_ITEM.PMM_ITEM_ID IS NOT NULL) OR (STL_ITEM.PMM_ITEM_ID <> ''))")
        End If
        If chkReq.Checked Then
            sqlsb.Append(" AND STL_ITEM.ON_REQ <> '' ")
        End If
        If chkPar.Checked Then
            sqlsb.Append(" AND STL_ITEM.ITEM_NO IN (SELECT ITEM_NO FROM STL_PAR_FORMS WHERE ITEM_NO=STL_ITEM.ITEM_NO) ")
        End If

        sqlsb.Append(" ORDER BY STL_ITEM.MFR_CTLG_NO ")

        StLItemDsData = ExecuteSQLSupport(sqlsb.ToString, dSCountResults)
        With dgDsItems
            .DataSource = StLItemDsData
            .DataBind()
        End With

        Dim stats As New cStatistics(ddlData.SelectedValue, ddlSubset.SelectedValue)
        lblStats.text = String.Format("{0} of {1} items have been matched. ======> {2}% complete", stats.CompletedRecords, stats.TotalRecords, Math.Round(stats.PercentageComplete, 5))
    End Sub

    Private Sub PopulatePMMItems()


        Dim sqlsb As New StringBuilder
        'Clear the edit item
        dgPMMItems.EditItemIndex = -1
        If PMMPageIndex = 0 Then
            dgPMMItems.CurrentPageIndex = 0
        End If
        PMMSelectedIndex = -1
        PMMPageIndex = 0
        sqlsb.Append( _
   "SELECT PMM.dbo.ITEM.ITEM_ID,PMM.dbo.ITEM.ITEM_NO,PMM.dbo.ITEM.STAT,PMM.dbo.ITEM.CTLG_NO, " & _
   "PMM.dbo.MFR.NAME AS MFR,PMM.dbo.VEND.NAME AS VENDOR,PMM.dbo.ITEM_VEND_PKG.CTLG_NO AS VEND_CAT, " & _
   "PMM.dbo.ITEM.DESCR  " & _
   "FROM PMM.dbo.ITEM LEFT JOIN PMM.dbo.MFR ON PMM.dbo.ITEM.MFR_ID = PMM.dbo.MFR.MFR_ID  " & _
    "AND PMM.dbo.ITEM.MFR_IDB = PMM.dbo.MFR.MFR_IDB   " & _
   "LEFT JOIN STL_ITEM ON STL_ITEM.PMM_ITEM_ID=PMM.dbo.ITEM.ITEM_ID AND STL_ITEM.DATASET = '" & RTrim(ddlData.SelectedValue) & "' " & _
   "LEFT JOIN PMM.dbo.ITEM_VEND ON PMM.dbo.ITEM_VEND.ITEM_ID=PMM.dbo.ITEM.ITEM_ID  " & _
    "AND PMM.dbo.ITEM_VEND.ITEM_IDB=PMM.dbo.ITEM.ITEM_IDB AND PMM.dbo.ITEM_VEND.SEQ_NO=1  " & _
    "AND PMM.dbo.ITEM_VEND.CORP_ID=1001  " & _
   "LEFT JOIN PMM.dbo.VEND ON PMM.dbo.VEND.VEND_ID=PMM.dbo.ITEM_VEND.VEND_ID  " & _
    "AND PMM.dbo.VEND.VEND_IDB=PMM.dbo.ITEM_VEND.VEND_IDB  " & _
   "LEFT JOIN PMM.dbo.ITEM_VEND_PKG ON PMM.dbo.ITEM_VEND.ITEM_VEND_ID = PMM.dbo.ITEM_VEND_PKG.ITEM_VEND_ID  " & _
    "AND PMM.dbo.ITEM_VEND.ITEM_VEND_IDB= PMM.dbo.ITEM_VEND_PKG.ITEM_VEND_IDB  " & _
    "AND PMM.dbo.ITEM_VEND_PKG.UM_CD = PMM.dbo.ITEM_VEND.ORDER_UM_CD  " & _
   " WHERE PMM.dbo.ITEM.CTLG_ITEM_IND='Y'  ")
        If txtPMMItem.text <> String.Empty Then
            sqlsb.Append(" AND PMM.dbo.ITEM.ITEM_NO LIKE '" & txtPMMItem.Text & "%'")
        End If
        If txtPMMMfr.text <> String.Empty Then
            sqlsb.Append(" AND PMM.dbo.MFR.NAME LIKE '" & txtPMMMfr.Text & "%'")
        End If
        If txtPMMCtlg.text <> String.Empty Then
            sqlsb.Append(" AND PMM.dbo.ITEM_VEND_PKG.CTLG_NO LIKE '" & txtPMMCtlg.Text & "%'")
        End If
        If txtPMMVendor.text <> String.Empty Then
            sqlsb.Append(" AND PMM.dbo.VEND.NAME LIKE '%" & txtPMMVendor.Text & "%'")
        End If
        If txtPMMDesc.text <> String.Empty Then
            sqlsb.Append(" AND PMM.dbo.ITEM.DESCR LIKE '%" & UCase(txtPMMDesc.text) & "%'")
        End If
        If chkPMMMatched.checked Then
            sqlsb.Append(" AND (STL_ITEM.PMM_ITEM_ID IS NULL) ")
        End If

        sqlsb.Append(" ORDER BY PMM.dbo.ITEM.CTLG_NO ")
        StLItemPMMData = ExecuteSQLSupport(sqlsb.ToString, dsPMMCountResults)
        With dgPMMItems
            .DataSource = StLItemPMMData
            .DataBind()
        End With

    End Sub

    Public Function GetBindingValue(ByVal o As Object) As String
        Dim rtn As String
        If IsDBNull(o) Then
            rtn = "&nbsp;"
        Else
            rtn = o.ToString
        End If
        Return rtn
    End Function

    Protected Sub btnDsSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDsSearch.Click
        dSPageIndex = 0
        PopulateDsItems()
    End Sub

    Protected Sub btnPMMSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPMMSearch.Click
        PMMPageIndex = 0
        PopulatePMMItems()
    End Sub

    Protected Sub chkPMMMatched_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPMMMatched.CheckedChanged
        PopulatePMMItems()
    End Sub

    Protected Sub chkMatched_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkMatched.CheckedChanged
        PopulateDsItems()
    End Sub

    Protected Sub chkPar_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPar.CheckedChanged
        PopulateDsItems()
    End Sub

    Protected Sub chkReq_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkReq.CheckedChanged
        PopulateDsItems()
    End Sub

    Protected Sub btnMatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMatch.Click
        If Not (Session("oUser") Is Nothing) Then
            Dim oUser As cUserInfo = Session("oUser")
            Try
                If ((dsSelectedIndex <> -1)) And ((PMMSelectedIndex <> -1)) Then
                    Dim DsItem As String = RTrim(GetBindingValue(StLItemDsData.Tables(0).Rows(dsSelectedIndex).Item("STL_NBR")))
                    Dim PMMItem As String = RTrim(GetBindingValue(StLItemPMMData.Tables(0).Rows(PMMSelectedIndex).Item("ITEM_ID")))
                    Dim OldPMMItem As String = RTrim(GetBindingValue(StLItemDsData.Tables(0).Rows(dsSelectedIndex).Item("ITEM_ID")))
                    Dim sql As String = "UPDATE STL_ITEM SET PMM_ITEM_ID='" & PMMItem & "' WHERE STL_ITEM.ITEM_NO='" & DsItem & "'"
                    If (DsItem <> String.Empty) And (DsItem <> " ") Then
                        ExecuteSQLSupportNonQuery(sql)
                        Dim Notes As String = "SUCCESS: CHANGED ITEM # FROM " & Replace(OldPMMItem, "&nbsp;", "") & " TO " & PMMItem & " FOR STL ITEM # " & DsItem
                        InsertChangeLog("/PMMStLItemMatch.aspx", oUser.NetworkId, oUser.IPAddress, "STL_ITEM", DsItem, "PMM_ITEM_ID", Replace(OldPMMItem, "&nbsp;", ""), PMMItem, Notes)
                        PopulateDsItems()
                        dgDsItems.CurrentPageIndex = dSPageIndex
                        PopulatePMMItems()
                        dgPMMItems.CurrentPageIndex = PMMPageIndex
                        lblMessage.Text = ("'" & DsItem & "' successfully matched with PMM item '" & PMMItem & "'.")
                    End If
                Else
                    lblMessage.Text = ("Please select items to Match!")
                End If
            Catch ex As Exception
                LogEvent(ex.Message, Diagnostics.EventLogEntryType.Error, 500001)
                lblMessage.Text = ("Error saving selections or no selection available!")
            End Try
        Else
            Server.Transfer("timeoutError.aspx")
        End If
    End Sub

    Protected Sub btnUnMatch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUnMatch.Click
        If Not (Session("oUser") Is Nothing) Then
            Dim oUser As cUserInfo = Session("oUser")
            Try
                If ((dsSelectedIndex <> -1)) Then
                    Dim DsItem As String = RTrim(GetBindingValue(StLItemDsData.Tables(0).Rows(dsSelectedIndex).Item("STL_NBR")))
                    Dim OldPMMItem As String = RTrim(GetBindingValue(StLItemDsData.Tables(0).Rows(dsSelectedIndex).Item("ITEM_ID")))
                    Dim sql As String = "UPDATE STL_ITEM SET PMM_ITEM_ID = NULL WHERE STL_ITEM.ITEM_NO='" & DsItem & "'"
                    If (DsItem <> String.Empty) And (DsItem <> " ") Then
                        ExecuteSQLSupportNonQuery(sql)
                        Dim Notes As String = "SUCCESS: CHANGED ITEM # FROM " & Replace(OldPMMItem, "&nbsp;", "") & " TO NULL FOR STL ITEM # " & DsItem
                        InsertChangeLog("/PMMStLItemMatch.aspx", oUser.NetworkId, oUser.IPAddress, "STL_ITEM", DsItem, "PMM_ITEM_ID", Replace(OldPMMItem, "&nbsp;", ""), "", Notes)
                        PopulateDsItems()
                        dgDsItems.CurrentPageIndex = dSPageIndex
                        PopulatePMMItems()
                        dgPMMItems.CurrentPageIndex = PMMPageIndex
                        lblMessage.Text = ("'" & DsItem & "' successfully unmatched with PMM item '" & OldPMMItem & "'.")
                    End If
                Else
                    lblMessage.Text = ("Please select item to Un-Match!")
                End If
            Catch ex As Exception
                LogEvent(ex.Message, Diagnostics.EventLogEntryType.Error, 500001)
                lblMessage.Text = ("Error saving selections or no selection available!")
            End Try
        Else
            Server.Transfer("timeoutError.aspx")
        End If
    End Sub

    Private Sub ClearDsSearch()
        Dim ds As New DataSet
        txtDsCat.Text = String.Empty
        txtDsDesc.Text = String.Empty
        txtDsItem.Text = String.Empty
        txtDsMfr.Text = String.Empty
        txtDsPmmItem.Text = String.Empty
        txtDsVend.Text = String.Empty
        StLItemDsData = ds
        dsSelectedIndex = -1
        dgDsItems.DataBind()

    End Sub
    Private Sub ClearPMMSearch()
        Dim ds As New DataSet
        txtPMMItem.text = String.Empty
        txtPMMMfr.text = String.Empty
        txtPMMCtlg.text = String.Empty
        txtPMMVendor.text = String.Empty
        txtPMMDesc.text = String.Empty
        StLItemPMMData = ds
        PMMSelectedIndex = -1
        dgPMMItems.DataBind()
    End Sub

    Protected Sub btnDsClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDsClear.Click
        dSPageIndex = 0
        ClearDsSearch()
    End Sub

    Protected Sub btnPMMClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPMMClear.Click
        PMMSelectedIndex = 0
        ClearPMMSearch()
    End Sub

    Protected Sub dgPMMItems_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgPMMItems.PageIndexChanged
        dgPMMItems.CurrentPageIndex = e.NewPageIndex
        dgPMMItems.DataSource = StLItemPMMData
        dgPMMItems.DataBind()
        PMMPageIndex = e.NewPageIndex
    End Sub

    Protected Sub dgDsItems_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDsItems.PageIndexChanged
        dgDsItems.CurrentPageIndex = e.NewPageIndex
        dgDsItems.DataSource = StLItemDsData
        dgDsItems.DataBind()
        dSPageIndex = e.NewPageIndex
    End Sub

    Protected Sub ddlData_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlData.SelectedIndexChanged
        dSPageIndex = 0
        PMMPageIndex = 0
        dsName.Text = ddlData.SelectedValue
        ClearDsSearch()
        PopulateDsItems()
        ClearPMMSearch()
    End Sub

    Protected Sub ddlRows_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRows.SelectedIndexChanged
        Response.Cookies("RowCountValue").Value = ddlRows.SelectedValue
        Response.Cookies("RowCountValue").Expires = DateAdd(DateInterval.Month, 1, Now)
        PopulateDsItems()
    End Sub

    Protected Sub ddlSubset_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSubset.SelectedIndexChanged
        dSPageIndex = 0
        PMMPageIndex = 0
        PopulateDsItems()
    End Sub

End Class

Friend Class cStatistics
    Private _PercentageComplete As Double
    Private _CompletedRecords As Integer
    Private _TotalRecords As Integer
    Public ReadOnly Property PercentageComplete() As Double
        Get
            If TotalRecords <> 0 Then
                Return (CompletedRecords * 100) / TotalRecords
            Else
                Return 0
            End If
        End Get
    End Property
    Public ReadOnly Property CompletedRecords() As Integer
        Get
            Return _CompletedRecords
        End Get
    End Property
    Public ReadOnly Property TotalRecords() As Integer
        Get
            Return _TotalRecords
        End Get
    End Property

    Public Sub New(ByVal dsName As String, ByVal ssName As String)
        If dsName <> String.Empty Then
            Dim ds As DataSet
            Dim count As Integer
            Dim sql As New StringBuilder


            sql.Append("SELECT COUNT(*) ")
            sql.Append("FROM STL_ITEM ")
            If ssName <> "-1" Then
                sql.Append("INNER JOIN STL_SUBSET ON STL_ITEM.ITEM_NO = STL_SUBSET.ITEM_NO AND STL_SUBSET.SUBSET = '" & ssName & "' ")
            End If
            sql.Append("WHERE (SUPPRESS IS NULL OR SUPPRESS='')")
            sql.Append("AND STL_ITEM.DATASET = '" & RTrim(dsName) & "'; ")
            sql.Append("SELECT COUNT(*) ")
            sql.Append("FROM STL_ITEM ")
            If ssName <> "-1" Then
                sql.Append("INNER JOIN STL_SUBSET ON STL_ITEM.ITEM_NO = STL_SUBSET.ITEM_NO AND STL_SUBSET.SUBSET = '" & ssName & "' ")
            End If
            sql.Append("WHERE (SUPPRESS IS NULL OR SUPPRESS='') ")
            sql.Append("AND PMM_ITEM_ID IS NOT NULL ")
            sql.Append("AND STL_ITEM.DATASET = '" & RTrim(dsName) & "';")

            ds = ExecuteSQLSupport(sql.ToString, count)
            _TotalRecords = ds.Tables(0).Rows(0).Item(0)
            _CompletedRecords = ds.Tables(1).Rows(0).Item(0)
        End If
    End Sub
End Class