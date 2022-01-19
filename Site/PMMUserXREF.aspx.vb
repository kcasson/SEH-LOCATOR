Imports cData
Partial Class Site_PMMUserXREF
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LogPageHit("/PMMUserXREF", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            With ddlUser
                .DataSource = GetUsers()
                .DataTextField = ("USER_NAME")
                .DataValueField = ("USR_ID")
                .DataBind()
            End With
        End If
    End Sub

End Class
