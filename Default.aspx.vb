
Partial Class _Default
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Production site redirect, does not work in development...
        Response.Redirect("/Site/Default.aspx")
    End Sub
End Class
