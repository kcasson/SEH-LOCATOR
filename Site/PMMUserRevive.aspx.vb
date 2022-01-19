Option Explicit On

Imports cData
Imports cUtilities
Imports System.Net.Mail


Partial Class pwReset
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not cData.GetUserAuthorization(User.Identity.Name, "/PMMUserRevive.aspx") Then
            LogPageHit("PMMUserRevive.aspx", User.Identity.Name, 0, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            Server.Transfer("na.aspx?u=" & User.Identity.Name)
        End If
        If Not IsPostBack Then
            LogPageHit("PMMUserRevive.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            loadUsers()
            loadInactiveUsers()
        End If
    End Sub

    Private Function loadUsers() As Boolean
        With ddlUser
            .DataSource = GetUsers(False)
            .DataTextField = "USER_NAME"
            .DataValueField = "USR_ID"
            .DataBind()
        End With
    End Function
    Private Function loadInactiveUsers() As Boolean
        With lbInactiveUsers
            .DataSource = GetUsers(True)
            .DataTextField = "USER_NAME"
            .DataValueField = "USR_ID"
            .DataBind()
        End With
    End Function

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Dim u As cResetUser
        ResetUser(CInt(ddlUser.SelectedValue), ddlUser.SelectedItem.Text, u)
        lblMessage.Text = "User " & u.UserName & "'s temporary password is '" & u.NewPassword & "'."
    End Sub

    Protected Sub btnReviveSub_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReviveSub.Click
        Dim u As cResetUser
        ResetUser(CInt(lbInactiveUsers.SelectedValue), lbInactiveUsers.SelectedItem.Text, u)
        lblMessage.Text = "User " & u.UserName & " has been restored and their temporary password is '" & u.NewPassword & "'."
        'Reload the list boxes to reflect updates
        loadUsers()
        loadInactiveUsers()
    End Sub

    Private Sub ResetUser(ByVal userID As Integer, ByVal UserName As String, ByRef u As cResetUser)
        Dim pw As New cPMMEncryption()
        Dim ret As String = String.Empty

        'Build unique password
        u.NewPassword = "pmm" & Date.Now.Year.ToString & Date.Now.Millisecond.ToString
        Try
            'Reset the user
            UpdateUserPassword(userID, pw.encryptString(u.NewPassword))
            Dim pos As Integer = (InStr(UserName, "-") + 2)
            Dim length As Integer = ((Len(UserName)))
            u.UserName = RTrim(Mid(UserName, pos, length))
            '

            InsertChangeLog("/PMMUserRevive.aspx", User.Identity.Name, Request.ServerVariables("REMOTE_ADDR"), "USR", _
                            ddlUser.SelectedValue, "PSWD", "", "", "SUCCESS: User " & RTrim(Mid(ddlUser.SelectedItem.Text, pos, length)) & "'s Password reset.")

            SendEmail(New MailAddress(ConfigurationManager.AppSettings("SupportEmail")), New MailAddress("PMM@stelizabeth.com", "PMM"), "*** User " & _
                        u.UserName & "'s password has been reset. ", _
                            "<table><tr><td>User " & u.UserName & "'s password reset by network user '" & User.Identity.Name & "' using the Materials Management web interface.</td></tr></table>")

            InitControl.Focus()
        Catch ex As Exception
            lblMessage.Text = "Error saving, Message: " & ex.Message
        End Try
    End Sub
End Class

Public Structure cResetUser
    Public UserName As String
    Public NewPassword As String
End Structure
