
Partial Class na
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitControl.Text = "User '" & Request.QueryString("u") & "' is not authorized to view this page.<br><br>Please contact PMM support."
    End Sub
End Class
