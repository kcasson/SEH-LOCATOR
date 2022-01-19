Imports System.DirectoryServices
Imports cUtilities
Imports System.Data
Imports cData
Imports Web.UI.MiscControls

Partial Class Site_SiteSecurity
    Inherits System.Web.UI.Page

    Dim userID As String = ConfigurationManager.AppSettings("DomainUser").ToString
    Dim password As String = ConfigurationManager.AppSettings("DomainUserPass").ToString
    Private Property alUsers() As ArrayList
        Get
            If IsNothing(Session("alUsers")) Then
                Session("alUsers") = New ArrayList
            End If
            Return Session("alUsers")
        End Get
        Set(ByVal value As ArrayList)
            Session("alUsers") = value
        End Set
    End Property
    Private Property alApplications() As ArrayList
        Get
            If IsNothing(Session("alApplications")) Then
                Session("alApplications") = New ArrayList
            End If
            Return Session("alApplications")
        End Get
        Set(ByVal value As ArrayList)
            Session("alApplications") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsNothing(Request.QueryString("load")) Then
            LoadADUser(Request.QueryString("load"))
            RefreshUsers()
            refreshDropdown()
            Response.Redirect("./SiteSecurity.aspx")
        End If
        If Not IsNothing(Session("message")) Then
            lblMessage.Text = Session("message")
            Session("message") = Nothing
        End If
        If Not IsPostBack Then
            RefreshUsers()
            RefreshApps()
            refreshDropdown()
            ClearUser()
        End If

    End Sub
    Private Sub refreshDropdown()
        ddlUser.Items.Clear()
        ddlUser.Items.Add(New ListItem("Select User", "-1"))
        ddlPages.Items.Clear()
        ddlPages.Items.Add(New ListItem("Select Application", "-1"))
        For Each User As cADUser In alUsers
            ddlUser.Items.Add(New ListItem(IIf(User.Name = "<NULL>", User.Id, User.Name), User.Id))
        Next
        For Each li As ListItem In alApplications
            ddlPages.Items.Add(li)
        Next
    End Sub
    Private Sub RefreshUsers()
        Dim WebUsers As DataSet = cData.GetWebUsers
        alUsers.Clear()
        'load all the user information into session state
        For Each dr As DataRow In WebUsers.Tables(0).Rows
            'Update user header information
            Dim Usr As New cADUser(dr("USER_ID"), dr("UserName"), dr("UserTitle"), dr("UserPhone"), dr("UserEmail"))
            'Get their authorized pages
            Dim ds As DataSet = cData.GetWebUserAuthorization(Usr.Id)
            For Each adr As DataRow In ds.Tables(0).Rows
                Usr.AuthorizedPages.Add(New ListItem(adr("ApplicationName"), adr("Application")))
            Next
            alUsers.Add(Usr)
        Next

    End Sub
    Private Sub RefreshApps()
        Dim ds As DataSet = cData.GetWebApps
        alApplications.Clear()
        'load the web apps into the array
        For Each dr As DataRow In ds.Tables(0).Rows
            alApplications.Add(New ListItem(dr("ApplicationName"), dr("ApplicationId")))
        Next
    End Sub

    Protected Sub btnAddUserWork_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddUserWork.Click
        

        Dim srColl As System.DirectoryServices.SearchResultCollection = cUtilities.SearchActiveDirectory("", txtHidden.Text, "")
        Dim sb As New StringBuilder
        Dim coll As New ArrayList
        Dim bResults As Boolean = False

        sb.Append("<table cellpadding=""0"" cellspacing=""0"" border=""1"" width=""500"">")
        sb.Append("<tr style=""font-size:smaller;""><td><b>User Id</b></td><td><b>Name</b></td><td><b>Title</b></td><td><b>Phone</b></td></tr>")
        Try
            If Not IsNothing(srColl) Then
                For Each results As SearchResult In srColl
                    Dim pColl As System.DirectoryServices.PropertyCollection
                    If (Not IsNothing(results)) Then
                        Dim deGroup As New DirectoryEntry(results.Path, userID, password, AuthenticationTypes.Secure)
                        'assign a property collection
                        pColl = deGroup.Properties
                        If (InStr(pColl.Item("distinguishedName").Value, "Disabled User") < 1) Then
                            sb.Append("<tr style=""font-size:smaller;""><td width=""60px""><a href=""./SiteSecurity.aspx?load=" & pColl.Item("sAMAccountName").Value & """>" & pColl.Item("sAMAccountName").Value & "</a></td>")
                            sb.Append("<td style=""font-size:smaller;"" width=""100px""><b>" & pColl.Item("displayName").Value & "</b></td>")
                            sb.Append("<td width=""200px"">" & pColl.Item("title").Value & "</td>")
                            sb.Append("<td>" & pColl.Item("telephoneNumber").Value & "</td></tr>")
                            'sb.Append("<td>" & pColl.Item("mail").Value & "</td>")
                            coll.Add(New cADUser(pColl.Item("sAMAccountName").Value, pColl.Item("displayName").Value, pColl.Item("title").Value, pColl.Item("telephoneNumber").Value, pColl.Item("mail").Value))
                            bResults = True
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            LogEvent(ex.Message, Diagnostics.EventLogEntryType.Error, 50001)

        End Try

        If Not bResults Then
            sb.Append("<tr><td align=""center"" style=""font-size:smaller;"" colspan=""4"">No results found!</td></tr>")
        End If

        sb.Append("<tr style=""font-size:smaller;"" class=""itemlisthead""><td colspan=""4"">Click user id to add user.</td></tr></table>")
        lblLuResults.Text = sb.ToString
        Session("cADUsers") = coll
        're-sync users with active directory
        SyncTable()
        'make sure display is correct after post
        tblAddUser.Style("display") = "block"
        tblUserInfo.Style("display") = "none"
        txtHidden.Focus()


    End Sub
    Private Sub SyncTable()
        For Each User As cADUser In alUsers
            Dim srColl As System.DirectoryServices.SearchResultCollection = cUtilities.SearchActiveDirectory(User.IdOnly, String.Empty, String.Empty)
            If Not IsNothing(srColl) Then
                For Each results As SearchResult In srColl
                    Dim pColl As System.DirectoryServices.PropertyCollection
                    If (Not IsNothing(results)) Then
                        Dim deGroup As New DirectoryEntry(results.Path, userID, password, AuthenticationTypes.Secure)
                        'assign a property collection
                        pColl = deGroup.Properties
                        cData.InsertUser(User.Id, pColl.Item("displayName").Value, pColl.Item("title").Value, pColl.Item("telephoneNumber").Value, pColl.Item("mail").Value)
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub LoadADUser(ByVal Id As String)
        Dim rtn As Boolean = False
        If Not IsNothing(Session("cADUsers")) Then
            Dim coll As ArrayList = CType(Session("cADUsers"), ArrayList)
            For Each User As cADUser In coll
                If User.Id = ("SEHP\" & Id) Then
                    'Add user to application security
                    Try
                        If (cData.InsertUser(User.Id, User.Name, User.Title, User.Phone, User.email)) Then
                            Session("message") = "User " & User.Name & " (" & User.Id & ")" & " already existed but has been updated!"
                        Else
                            Session("message") = "User " & User.Name & " (" & User.Id & ")" & " successfully added to web security!"
                        End If
                    Catch ex As Exception
                        lblMessage.Text = ex.Message
                    Finally
                        Session("cADUsers") = Nothing
                    End Try
                End If
            Next
        End If
    End Sub

    Private Sub ClearUser()
        lblADName.Text = String.Empty
        lblADPhone.Text = String.Empty
        lblADTitle.Text = String.Empty
        lblADNetworkId.Text = String.Empty
        lblADemail.Text = String.Empty
        ddlAuthorization.Items.Clear()
        ddlAvailable.Items.Clear()
        chkIOApprover.Checked = False
        chkIOApprover.Enabled = False
        chkIOBuilder.Checked = False
        chkIOBuilder.Enabled = False
        chkIONotify.Checked = False
        chkIONotify.Enabled = False

    End Sub
    Private Sub UpdateUserAuthorization()
        ddlAvailable.Items.Clear()
        ddlAuthorization.Items.Clear()
        For Each User As cADUser In alUsers
            If User.Id = ddlUser.SelectedValue Then
                'load details
                lblADName.Text = User.Name
                lblADPhone.Text = User.Phone
                lblADTitle.Text = User.Title
                lblADNetworkId.Text = User.IdOnly
                lblADemail.Text = User.email
                chkIOApprover.Checked = (cData.GetInvoiceOnlyNotify("Approver").Tables(0).Select("USER_ID = '" & User.Id & "'").Length > 0)
                chkIOApprover.Enabled = False 'Remove approver functionality from workflow, 10/05/2012 KAC
                chkIOBuilder.Checked = (cData.GetInvoiceOnlyNotify("ItemBuilder").Tables(0).Select("USER_ID = '" & User.Id & "'").Length > 0)
                chkIOBuilder.Enabled = True
                chkIONotify.Checked = (cData.GetInvoiceOnlyNotify("Notify").Tables(0).Select("USER_ID = '" & User.Id & "'").Length > 0)
                chkIONotify.Enabled = True
                'load authorized pages
                ddlAuthorization.Items.Clear()
                ddlAvailable.Items.Clear()
                For Each Page As ListItem In alApplications
                    If User.AuthorizedPages.IndexOf(Page) <> -1 Then 'user has authorization
                        ddlAuthorization.Items.Add(Page)
                    Else 'denied authorization
                        ddlAvailable.Items.Add(Page)
                    End If
                Next
            End If
        Next
    End Sub
    Private Sub UpdatePageAuthorization()
        ddlAvailable.Items.Clear()
        ddlAuthorization.Items.Clear()
        'load the users
        For Each User As cADUser In alUsers
            If User.AuthorizedPages.IndexOf(ddlPages.SelectedItem) <> -1 Then 'granted
                ddlAuthorization.Items.Add(New ListItem(User.Name, User.Id))
            ElseIf User.Name <> "<NULL>" Then 'denied
                ddlAvailable.Items.Add(New ListItem(User.Name, User.Id))
            End If
        Next
    End Sub

    Protected Sub btnSvrCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSvrCancel.Click
        lblLuResults.Text = String.Empty
        tblAddUser.Style("display") = "none"
        tblUserInfo.Style("display") = "block"
        txtHidden.Text = String.Empty
    End Sub

    Protected Sub btnAddPage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPage.Click
        If ddlUser.SelectedValue <> "-1" Then
            Dim selected() As Integer = ddlAvailable.GetSelectedIndices
            For i As Integer = 0 To (selected.Length - 1)
                cData.AlterWebUserAuthorization(ddlUser.SelectedValue, ddlAvailable.Items(selected(i)).Value, False)
            Next
            RefreshUsers()
            UpdateUserAuthorization()
        ElseIf ddlPages.SelectedValue <> "-1" Then
            Dim selected() As Integer = ddlAvailable.GetSelectedIndices
            For i As Integer = 0 To (selected.Length - 1)
                cData.AlterWebUserAuthorization(ddlAvailable.Items(selected(i)).Value, ddlPages.SelectedValue, False)
            Next
            RefreshUsers()
            UpdatePageAuthorization()
        End If

    End Sub

    Protected Sub btnDeletePage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeletePage.Click

        If ddlUser.SelectedValue <> "-1" Then
            Dim selected() As Integer = ddlAuthorization.GetSelectedIndices
            For i As Integer = 0 To (selected.Length - 1)
                cData.AlterWebUserAuthorization(ddlUser.SelectedValue, ddlAuthorization.Items(selected(i)).Value, True)
            Next
            RefreshUsers()
            UpdateUserAuthorization()
        ElseIf ddlPages.SelectedValue <> "-1" Then
            Dim selected() As Integer = ddlAuthorization.GetSelectedIndices
            For i As Integer = 0 To (selected.Length - 1)
                cData.AlterWebUserAuthorization(ddlAuthorization.Items(selected(i)).Value, ddlPages.SelectedValue, True)
            Next
            RefreshUsers()
            UpdatePageAuthorization()
        End If

    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Try
            cData.DeleteWebUser(ddlUser.SelectedValue)
            lblMessage.Text = "User " & ddlUser.SelectedItem.Text & " successfully deleted!"
            RefreshUsers()
            refreshDropdown()
            ddlUser.SelectedIndex = -1
            ClearUser()
        Catch ex As Exception
            lblMessage.Text = ex.Message
        End Try

    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If (IsNothing(HttpContext.Current.Session("oUser")) = False) Then
            If (Not CType(Session("oUser"), cUserInfo).IsAdmin) And (Not cData.GetUserAuthorization(User.Identity.Name, "/SiteSecurity.aspx")) Then
                LogPageHit("/SiteSecurity.aspx", User.Identity.Name, 0, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
                Server.Transfer("../Site/na.aspx?u=" & User.Identity.Name)
            Else
                LogPageHit("/SiteSecurity.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            End If
        Else
            Server.Transfer("timeoutError.aspx")
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        'Load the page name 
        CType(Page.Master.FindControl("MenuHead"), MenuHeadControl).ImageText = "PMM Web Site Security"
    End Sub

    Protected Sub ddlPages_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPages.SelectedIndexChanged
        ddlUser.SelectedValue = "-1"
        ClearUser()
        UpdatePageAuthorization()

    End Sub
    Protected Sub ddlUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUser.SelectedIndexChanged
        ddlPages.SelectedValue = "-1"
        If ddlUser.SelectedValue = "-1" Then
            ClearUser()
        Else
            UpdateUserAuthorization()
        End If
    End Sub

    Protected Sub cbIOApprover_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIOApprover.CheckedChanged
        If ddlUser.SelectedIndex <> 0 Then
            AlterInvoiceOnlyNotify(ddlUser.SelectedValue, "Approver", IIf(chkIOApprover.Checked = True, False, True))
            RefreshUsers()
            UpdateUserAuthorization()
        End If
    End Sub

    Protected Sub chkIOBuilder_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIOBuilder.CheckedChanged

        If ddlUser.SelectedIndex <> 0 Then
            AlterInvoiceOnlyNotify(ddlUser.SelectedValue, "ItemBuilder", IIf(chkIOBuilder.Checked = True, False, True))
            RefreshUsers()
            UpdateUserAuthorization()
        End If
    End Sub

    Protected Sub chkIONotify_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIONotify.CheckedChanged
        If ddlUser.SelectedIndex <> 0 Then
            AlterInvoiceOnlyNotify(ddlUser.SelectedValue, "Notify", IIf(chkIONotify.Checked = True, False, True))
            RefreshUsers()
            UpdateUserAuthorization()
        End If
    End Sub
End Class
