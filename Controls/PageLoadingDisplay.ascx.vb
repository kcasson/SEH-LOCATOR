
Partial Class Controls_PageLoadingDisplay
    Inherits System.Web.UI.UserControl


    Private _ControlToClose As String = String.Empty
    Public Property ControlToClose() As String
        Get
            Return _ControlToClose
        End Get
        Set(ByVal value As String)
            _ControlToClose = value
        End Set
    End Property

    'Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click 
    '    Try
    '        Dim cnt As HtmlTable = Me.Parent.FindControl(_ControlToClose)
    '        cnt.Style.Item("display") = "none"
    '        tblWorking.Style.Item("display") = "block"
    '    Catch ex As Exception
    '        Throw New SystemException("Error Executing PageLoading Display!")
    '    End Try
    'End Sub
End Class
