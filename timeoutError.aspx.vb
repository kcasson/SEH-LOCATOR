
Partial Class timeoutError
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        InitControl.Text = "<a href=" & Request.QueryString("l") & ">Reload Page</a>"
    End Sub


End Class
