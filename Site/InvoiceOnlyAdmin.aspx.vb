Imports cData
Imports System.Data

Partial Class Site_InvoiceOnlyAdmin
    Inherits System.Web.UI.Page

    Private Property dsData As DataSet
        Get
            If (Not IsNothing(ViewState("dsData"))) Then
                Return ViewState("dsData")
            Else
                Return New DataSet
            End If
        End Get
        Set(value As DataSet)
            ViewState("dsData") = value
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        'only item builders and notify for completion users can view this page
        If Not cData.GetUserAuthorization(User.Identity.Name, "/ItemUsedChangeOrder.aspx") Then
            Server.Transfer("na.aspx?u=" & User.Identity.Name)
        End If

        If Not IsPostBack Then
            cData.LogPageHit("InvoiceOnlyAdmin.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            loadUsers()
            LoadFocus.SelectedValue = 1
            loadRequestsbyStatus()
        End If


    End Sub

    Private Sub loadUsers()

        Dim sql As String = _
            "select distinct io.CreatedBy, u.UserName from InvoiceOnly io inner join AUTH_LIST_USERS u on u.USER_ID = io.CreatedBy and CreatedDate > '2012-12-21 11:12:51.000'  order by CreatedBy"
        Dim ds As DataSet = ExecuteSQLSupport(sql)

        For Each dr As DataRow In ds.Tables(0).Rows
            ddlUser.Items.Add(New ListItem(dr("UserName"), dr("CreatedBy")))
        Next

        ddlUser.Items.Insert(0, (New ListItem("", "-1")))


    End Sub
    Protected Sub LoadFocus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LoadFocus.SelectedIndexChanged
        dgInvoiceOnlyList.CurrentPageIndex = 0
        loadRequestsbyStatus()
        ddlUser.SelectedIndex = 0


    End Sub

    Protected Sub ddlUser_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUser.SelectedIndexChanged
        dgInvoiceOnlyList.CurrentPageIndex = 0
        loadRequestsbyUser()
        LoadFocus.SelectedIndex = 0

    End Sub
    Private Sub loadRequestsbyStatus()
        Dim sql As String = _
            "select InvoiceOnlyID, Vendor, case Status when 0 then 'Not Approved' when 1 then 'Pending Completion' when 3 then 'Pending Item Build' when 4 then " & _
            "'Complete' end as Status, u.UserName " & _
            "from InvoiceOnly io " & _
            "inner join AUTH_LIST_USERS u on u.USER_ID = io.CreatedBy " & _
            "where Status =" & LoadFocus.SelectedValue & _
            " and CreatedDate > '2012-12-21 11:12:51.000' order by InvoiceOnlyID desc;"

        dsData = ExecuteSQLSupport(sql)
        dgInvoiceOnlyList.DataSource = dsData
        dgInvoiceOnlyList.DataBind()

    End Sub
    Private Sub loadRequestsbyUser()
        Dim sql As String = _
            "select InvoiceOnlyID, Vendor, case Status when 0 then 'Not Approved' when 1 then 'Pending Completion' when 3 then 'Pending Item Build' when 4 then " & _
            "'Complete' end as Status, u.UserName " & _
            "from InvoiceOnly io " & _
            "inner join AUTH_LIST_USERS u on u.USER_ID = io.CreatedBy " & _
            "where io.CreatedBy = '" & ddlUser.SelectedValue & _
            "' and CreatedDate > '2012-12-21 11:12:51.000'  order by InvoiceOnlyID desc;"

        dsData = ExecuteSQLSupport(sql)
        dgInvoiceOnlyList.DataSource = dsData
        dgInvoiceOnlyList.DataBind()

    End Sub


    Protected Sub dgInvoiceOnlyList_PageIndexChanged(source As Object, e As DataGridPageChangedEventArgs) Handles dgInvoiceOnlyList.PageIndexChanged
        dgInvoiceOnlyList.CurrentPageIndex = e.NewPageIndex
        dgInvoiceOnlyList.DataSource = dsData
        dgInvoiceOnlyList.DataBind()
    End Sub


End Class
