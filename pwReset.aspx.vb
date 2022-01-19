Option Explicit On

Imports cData
Imports cUtilities
Imports System.Net.Mail


Partial Class pwReset
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not cData.GetUserAuthorization(User.Identity.Name, "/pwReset.aspx") Then
            LogPageHit("pwReset.aspx", User.Identity.Name, 0, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            Server.Transfer("na.aspx?u=" & User.Identity.Name)
        End If
        If Not IsPostBack Then
            LogPageHit("pwReset.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            loadUsers()
        End If
    End Sub

    Private Function loadUsers() As Boolean
        With ddlUser
            .DataSource = GetUsers()
            .DataTextField = "USER_NAME"
            .DataValueField = "USR_ID"
            .DataBind()
        End With
    End Function

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Dim pw As New cPMMEncryption()

        'Build unique password
        Dim strPw As String = "pmm" & Date.Now.Year.ToString & Date.Now.Millisecond.ToString

        Try
            'Reset the user
            UpdateUserPassword(CInt(ddlUser.SelectedValue), pw.encryptString(strPw))
            Dim pos As Integer = (InStr(ddlUser.SelectedItem.Text, "-") + 2)
            Dim length As Integer = ((Len(ddlUser.SelectedItem.Text)))
            Dim strUname As String = RTrim(Mid(ddlUser.SelectedItem.Text, pos, length))
            '
            lblMessage.Text = "User " & strUname & "'s temporary password is '" & strPw & "'."
            InsertChangeLog("/pwReset.aspx", User.Identity.Name, Request.ServerVariables("REMOTE_ADDR"), "USR", _
                            ddlUser.SelectedValue, "PSWD", "", "", "SUCCESS: User " & RTrim(Mid(ddlUser.SelectedItem.Text, pos, length)) & "'s Password reset.")

            SendEmail(New MailAddress(ConfigurationManager.AppSettings("SupportEmail")), New MailAddress("PMM@stelizabeth.com", "PMM"), "*** User " & _
                        strUname & "'s password has been reset. ", _
                            "<table><tr><td>User " & strUname & "'s password reset by network user '" & User.Identity.Name & "' using the Materials Management web interface.</td></tr></table>")

            InitControl.Focus()
        Catch ex As Exception
            lblMessage.Text = "Error saving, Message: " & ex.Message
        End Try



    End Sub


End Class
