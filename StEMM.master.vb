
Partial Class StEMM
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Retrieve Application path and store in global variable 
        cGlobal.ApplicationPath = Server.MapPath(Request.ApplicationPath)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If ((Not IsPostBack) Or IsNothing(Session("oUser"))) Then
            Dim oUser As New cUserInfo(Request)
            Session("oUser") = oUser
        End If
    End Sub
End Class

