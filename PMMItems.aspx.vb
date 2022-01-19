Imports System.Data
Imports cData
Partial Class PMMItemEdit
    Inherits System.Web.UI.Page

    Public Property CountResults() As Integer
        Get
            If IsNothing(ViewState("CountResults")) Then
                Return 0
            Else
                Return CInt(ViewState("CountResults"))
            End If
        End Get
        Set(ByVal Value As Integer)
            ViewState("CountResults") = Value
        End Set
    End Property
    Public Property ColumnsSort() As List(Of SortDirection)
        Get
            Return CType(ViewState("ColumnsSort"), List(Of SortDirection))
        End Get
        Set(ByVal value As List(Of SortDirection))
            ViewState("ColumnsSort") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not cData.GetUserAuthorization(User.Identity.Name, "/PMMItems.aspx") Then
                If IsNothing(Request.QueryString("s4")) Then
                    LogPageHit("/PMMItems.aspx", User.Identity.Name, 0, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
                End If
                Server.Transfer("na.aspx?u=" & User.Identity.Name)
            Else
                If IsNothing(Request.QueryString("s4")) Then
                    LogPageHit("/PMMItems.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
                End If
            End If

            If (Not IsNothing(Request.QueryString("s1"))) Or _
                (Not IsNothing(Request.QueryString("s2")) Or (Not IsNothing(Request.QueryString("s3")))) Then
                InitControl.Value = Request.QueryString("s1")
                txtMfgNumber.Value = Request.QueryString("s2")
                txtdesc.Value = Request.QueryString("s3")
                LoadItems()
            End If
        End If
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        LoadItems()
    End Sub

    Public Function GetNavigateURL(ByVal itemId As String, ByVal itemIdb As String) As String
        Return "ItemDescriptionEdit.aspx?item=" & itemId & "&itemidb=" & itemIdb & "&s1=" & InitControl.Value & "&s2=" & txtMfgNumber.Value & "&s3=" & txtdesc.Value

    End Function

    Private Sub BindData(ByVal dv As DataView)
        With dgItemEdit
            .DataSource = dv
            .DataBind()
        End With
    End Sub

    Protected Sub dgItemEdit_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgItemEdit.ItemCreated

        If e.Item.ItemType = ListItemType.Pager Then
            ' Change label of the pager
            Dim cString As String = "Page "
            Dim oTableCell As TableCell
            Dim oSpecialCell As TableCell
            Dim oControl As Control
            Dim oResultsTableCell As New TableCell

            With oResultsTableCell
                .Wrap = True
                .Text = "Search results : "
                If CountResults > 0 Then
                    If dgItemEdit.PageSize * (dgItemEdit.CurrentPageIndex + 1) <= CountResults Then
                        .Text &= CStr(dgItemEdit.PageSize * (dgItemEdit.CurrentPageIndex) + 1) & " - " & CStr(dgItemEdit.PageSize * (dgItemEdit.CurrentPageIndex + 1))
                        .Text &= " of " & CStr(CountResults) & " " & CStr(IIf(CountResults = 1, "record", "records"))
                    Else
                        .Text &= CStr(dgItemEdit.PageSize * (dgItemEdit.CurrentPageIndex) + 1) & " - " & CStr(CountResults)
                        .Text &= " of " & CStr(CountResults) & " " & CStr(IIf(CountResults = 1, "record", "records"))
                    End If
                Else
                    .Text &= CStr(CountResults) & " " & CStr(IIf(CountResults = 1, "record", "records"))
                End If
                .HorizontalAlign = HorizontalAlign.Left
                .VerticalAlign = VerticalAlign.Middle
                .CssClass = "itemlistitem"
            End With

            For Each oTableCell In e.Item.Cells
                For Each oControl In oTableCell.Controls
                    If TypeOf oControl Is System.Web.UI.WebControls.Label Then
                        CType(oControl, Label).Text = cString & CType(oControl, Label).Text
                    Else
                        If TypeOf oControl Is System.Web.UI.WebControls.LinkButton Then
                            CType(oControl, LinkButton).Text = "[" & CType(oControl, LinkButton).Text & "]"
                        End If
                    End If
                Next
                oSpecialCell = oTableCell
            Next

            While e.Item.Cells.Count > 0
                e.Item.Cells.RemoveAt(0)
            End While

            Dim oNewCell As New TableCell

            Dim oTable As New Table
            Dim oTableRow As New TableRow
            With oTableRow.Cells
                .Add(oResultsTableCell)
                .Add(oSpecialCell)
            End With

            Dim oUnit As New Unit(100D, UnitType.Percentage)

            With oTable
                .Width = oUnit
                .Rows.Add(oTableRow)
            End With

            With oNewCell
                .HorizontalAlign = HorizontalAlign.Left
                .VerticalAlign = VerticalAlign.Middle
                .Width = oUnit
                .Controls.Add(oTable)
            End With

            With oSpecialCell
                .HorizontalAlign = HorizontalAlign.Right
                .VerticalAlign = VerticalAlign.Middle
                .CssClass = "itemlistitem"
            End With
            e.Item.Cells.Add(oNewCell)
        End If
    End Sub

    Protected Sub dgItemEdit_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgItemEdit.PageIndexChanged
        Dim dv As DataView = CType(Session("dv"), DataView)
        dgItemEdit.CurrentPageIndex = e.NewPageIndex
        BindData(dv)
    End Sub

    Public Sub dgSort_click(ByVal source As Object, ByVal e As System.EventArgs)
        Dim dv As DataView = CType(Session("dv"), DataView)
        Dim lbLink As LinkButton = CType(source, LinkButton)
        Dim strSortExpression As String = lbLink.CommandArgument
        dv.Sort = strSortExpression
        BindData(dv)
        Session("dv") = dv
    End Sub

    Public Sub LoadItems()
        Dim dv As DataView = cData.GetItemAltDescriptions(InitControl.Value, txtMfgNumber.Value, txtdesc.Value, CountResults)
        dgItemEdit.CurrentPageIndex = 0
        'Load data
        dgItemEdit.DataSource = dv
        dgItemEdit.DataBind()
        'Store data for later 
        Session("dv") = dv
    End Sub


End Class



