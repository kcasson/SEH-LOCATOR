Imports Microsoft.VisualBasic
Imports cData

Namespace Web.UI.AdminControls
    Public Class cAdminButton
        Inherits System.Web.UI.WebControls.Button

        Private Sub AdminButton_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim usr As cUserInfo = CType(HttpContext.Current.Session("oUser"), cUserInfo)
            Visible = (usr.IsAdmin Or _
            (cData.GetUserAuthorization(usr.NetworkId, "/AdminControls")))
        End Sub

    End Class

    Public Class AdminMenu
        Inherits WebControls.Menu

        Private Sub AdminMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim usr As cUserInfo = CType(HttpContext.Current.Session("oUser"), cUserInfo)
            Visible = (usr.IsAdmin Or _
            (cData.GetUserAuthorization(usr.NetworkId, "/AdminControls")))
        End Sub

    End Class

End Namespace


