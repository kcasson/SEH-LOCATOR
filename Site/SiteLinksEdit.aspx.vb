Imports System.Data
Imports cData
Imports System.Collections.Generic

Partial Class Site_SiteLinksEdit
    Inherits System.Web.UI.Page

    Private Property ListCount() As Integer
        Get
            If Not Session("ListCount") Is Nothing Then
                Return Session("ListCount")
            Else
                timeoutRedirect()
            End If
        End Get
        Set(ByVal value As Integer)
            Session("ListCount") = value
        End Set
    End Property

    Private Property LinkDataView() As DataView
        Get
            If Not Session("ListCount") Is Nothing Then
                Return Session("LinkDV")
            Else
                timeoutRedirect()
            End If
        End Get
        Set(ByVal value As DataView)
            Session("LinkDV") = value
        End Set
    End Property
    Private Property PageDirty() As Boolean
        Get
            If Not (Session("PageDirty") = Nothing) Then
                Return Session("PageDirty")
            Else
                Return False
            End If
        End Get
        Set(ByVal value As Boolean)
            Session("PageDirty") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Not CType(HttpContext.Current.Session("oUser"), cUserInfo).IsAdmin Then
                Server.Transfer("na.aspx?u=" & User.Identity.Name)
            Else
                DataLoad()
            End If
        End If
    End Sub
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        CheckDirty()
    End Sub

    Sub DataLoad()
        Dim LinkDV As DataView = GetLeftNavLinks.Tables(0).DefaultView
        Me.ListCount = LinkDV.Count
        LinkDataView = LinkDV
        DVBind()
    End Sub
    Sub DVBind()

        With dlLinksEdit
            .DataSource = LinkDataView
            .DataBind()
        End With
    End Sub

    Sub Edit_Command(ByVal sender As Object, ByVal e As DataListCommandEventArgs)
        dlLinksEdit.EditItemIndex = e.Item.ItemIndex
        DVBind()
    End Sub
    Sub Update_Command(ByVal sender As Object, ByVal e As DataListCommandEventArgs)
        Dim intEditIndex As Integer = e.Item.ItemIndex
        Dim newListOrder As Integer = (CType(e.Item.FindControl("ddlListOrder"), DropDownList).SelectedValue)
        'Enable the add, any additions commited
        btnAdd.Enabled = True
        'Update edited values
        LinkDataView.Item(intEditIndex).Row("LinkText") = (CType(e.Item.FindControl("txtLinkText"), TextBox).Text)
        LinkDataView.Item(intEditIndex).Row("NavUrl") = (CType(e.Item.FindControl("txtUrl"), TextBox).Text)
        LinkDataView.Item(intEditIndex).Row("Target") = (CType(e.Item.FindControl("ddlTarget"), DropDownList).SelectedValue)

        For Each r As DataRowView In LinkDataView
            If r("ListOrder") = newListOrder Then
                If r("ListOrder") = ListCount Then
                    r("ListOrder") = (CInt(r("ListOrder")) - 0.5)
                Else
                    r("ListOrder") = (CInt(r("ListOrder")) + 0.5)
                End If
            End If
        Next
        LinkDataView.Item(intEditIndex).Row("ListOrder") = newListOrder
        ReOrderlist()
        dlLinksEdit.EditItemIndex = -1
        DVBind()

        PageDirty = True

    End Sub
    Sub Cancel_Command(ByVal sender As Object, ByVal e As DataListCommandEventArgs)
        Dim drDelete As DataRow
        Dim emptyRow As Boolean = False
        For Each dr As DataRow In LinkDataView.Table.Rows
            If (dr("NavURL") = String.Empty) Then
                emptyRow = True
                drDelete = dr
            End If
        Next
        If emptyRow Then
            LinkDataView.Table.Rows.Remove(drDelete)
        End If

        dlLinksEdit.EditItemIndex = -1
        DVBind()
        btnAdd.Enabled = True

    End Sub
    Sub Delete_Command(ByVal sender As Object, ByVal e As DataListCommandEventArgs)
        LinkDataView.Delete(e.Item.ItemIndex)
        ReOrderlist()
        DVBind()
        PageDirty = True
    End Sub

    Protected Sub dlLinksEdit_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlLinksEdit.ItemDataBound
        If e.Item.ItemType = ListItemType.EditItem Then
            'Load the List Order data
            Dim ddlListOrder As DropDownList = e.Item.FindControl("ddlListOrder")
            For i As Integer = 1 To Me.ListCount
                ddlListOrder.Items.Add(i)
            Next
            ddlListOrder.SelectedValue = CType(e.Item.FindControl("lblListOrder"), Label).Text
            'Load the Target data
            CType(e.Item.FindControl("ddlTarget"), DropDownList).SelectedValue = _
            CType(e.Item.FindControl("lblTarget"), Label).Text
        End If

    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'Disable the Add button until update 
        btnAdd.Enabled = False
        'Add a new row
        Dim table As DataTable = LinkDataView.Table
        Dim dr As DataRow = LinkDataView.Table.NewRow
        dr(0) = 0
        dr(1) = ""
        dr(2) = ""
        dr(3) = "_same"
        dr(4) = (Me.ListCount + 1)
        dr(5) = True
        table.Rows.InsertAt(dr, 0)
        LinkDataView = table.DefaultView
        dlLinksEdit.EditItemIndex = 0
        Me.ListCount = LinkDataView.Count
        DVBind()
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        DataLoad()
        PageDirty = False
    End Sub
    Private Sub ReOrderlist()
        LinkDataView.Sort = "ListOrder"
        LinkDataView.Table.AcceptChanges()
        Dim dv As DataView = LinkDataView.Table.DefaultView
        dv.Sort = "ListOrder"
        Dim cnt As Integer = 1
        For Each r As DataRowView In dv
            r("ListOrder") = cnt
            cnt += 1
        Next
        LinkDataView = dv
        ListCount = dv.Count
    End Sub
    Private Sub CheckDirty()
        If PageDirty Then
            btnCancel.Enabled = True
            btnCommit.Enabled = True
        Else
            btnCancel.Enabled = False
            btnCommit.Enabled = False
        End If
    End Sub

    Protected Sub btnCommit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCommit.Click

        'Build the xml file for update
        Dim sb As New StringBuilder

        sb.Append("<Links>")
        For Each dr As DataRowView In LinkDataView
            sb.Append("<Link>")
            sb.Append("<NavURL>" & dr("NavURL") & "</NavURL>")
            sb.Append("<LinkText>" & dr("LinkText") & "</LinkText>")
            sb.Append("<Target>" & dr("Target") & "</Target>")
            sb.Append("<ListOrder>" & dr("ListOrder") & "</ListOrder>")
            sb.Append("</Link>")
        Next
        sb.Append("</Links>")

        'Update the database
        Try
            AlterWebLinks(sb.ToString)
            'Clear dirty flag
            PageDirty = False
            'Refresh the page
            Response.Redirect("SiteLinksEdit.aspx")
        Catch ex As Exception
            lblMessage.Text = ex.Message
        End Try


    End Sub

    Private Sub timeoutRedirect()
        Response.Redirect("timeoutError.aspx?l=SiteLinksEdit.aspx")
    End Sub
End Class



