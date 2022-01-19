
Partial Class StEMM
    Inherits System.Web.UI.MasterPage
    Public Property CurrentSessionUser() As cUserInfo
        Get
            If Not IsNothing(Session("oUser")) Then
                Return Session("oUser")
            Else
                Return New cUserInfo()
            End If
        End Get
        Set(ByVal value As cUserInfo)
            Session("oUser") = value
        End Set
    End Property
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Retrieve Application path and store in global variable 
        cGlobal.ApplicationPath = Server.MapPath(Request.ApplicationPath)
        If ((Not IsPostBack) Or IsNothing(Session("oUser"))) Then
            Dim oUser As New cUserInfo(Request)
            Session("oUser") = oUser
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class

