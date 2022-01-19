Imports System.Security.Cryptography
Imports System.IO
Imports cData
Partial Class Site_PMMCryptography
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            'Create the page header
            CType(Page.Master.FindControl("MenuHead"),  _
                Web.UI.MiscControls.MenuHeadControl).ImageText = "PMM Cryptography"
            LoadUsers()
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (IsNothing(Session("oUser")) = False) Then
            If Not CType(Session("oUser"), cUserInfo).IsAdmin Then
                LogPageHit("/PMMCryptography.aspx", User.Identity.Name, 0, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
                Server.Transfer("na.aspx")
            Else
                LogPageHit("/PMMCryptography.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            End If
        Else
            Server.Transfer("timeoutError.aspx")
        End If
    End Sub
    Private Sub LoadUsers()
        With ddlUser
            .DataSource = cData.GetUsers
            .DataTextField = ("USER_NAME")
            .DataValueField = ("PSWD")
            .DataBind()
            .Items.Insert(0, "Waiting on your input")
        End With
    End Sub
    Protected Sub ddlUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUser.SelectedIndexChanged
        txtSearch.Text = String.Empty
        RetrievePassword()
    End Sub
    Private Sub RetrievePassword()
        If (ddlUser.SelectedIndex <> 0) Then
            If Len(ddlUser.SelectedValue) <= 11 Then 'Current limit is 11 characters
                Dim pw As New cPMMEncryption
                lblPassword.Text = pw.decryptString(ddlUser.SelectedValue)
            Else
                lblPassword.ForeColor = Drawing.Color.Red
                lblPassword.Text = "The selected users password length exceeds the current limits of this class!"
            End If
            lblEncPassword.Text = ddlUser.SelectedValue
        Else
            ClearTxt()
        End If
    End Sub
    Private Sub ClearTxt()
        txtSearch.Text = String.Empty
        lblPassword.Text = String.Empty
        lblEncPassword.Text = String.Empty
    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        ClearTxt()
        LoadUsers()
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If (txtSearch.Text <> String.Empty) Then
            For Each i As ListItem In ddlUser.Items
                If InStr(UCase(i.Text), UCase(txtSearch.Text)) Then
                    ddlUser.SelectedIndex = ddlUser.Items.IndexOf(i)
                    Exit For 'String found
                End If
            Next
            RetrievePassword()
        End If
    End Sub
End Class
