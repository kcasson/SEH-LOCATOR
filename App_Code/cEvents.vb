Imports Microsoft.VisualBasic

Public Class cEvents



End Class
Public Class ZebraTabChangeEventArgs
    Inherits EventArgs
    'This class is used as the event arguments for a tab change on the master page
    Private _TabNumber As Integer

    Public Property TabNumber As Integer
        Get
            Return _TabNumber
        End Get
        Set(ByVal value As Integer)
            _TabNumber = value
        End Set
    End Property
End Class
Public Class ZebraLocationChangeEventArgs
    Inherits EventArgs

    Private _enableControls As Boolean
    Private _locationName As String
    Private _tabStatus As New List(Of stabStatus)

    Public Property enableControls() As Boolean
        Get
            Return _enableControls
        End Get
        Set(ByVal value As Boolean)
            _enableControls = value
        End Set
    End Property
    Public Property locationName As String
        Get
            Return _locationName
        End Get
        Set(ByVal value As String)
            _locationName = value
        End Set
    End Property
    Public Property tabSatuses() As List(Of stabStatus)
        Get
            Return _tabStatus
        End Get
        Set(ByVal value As List(Of stabStatus))
            _tabStatus = value
        End Set
    End Property

End Class



Public Class stabStatus
    Public Sub New(ByVal name As String, ByVal chkd As Boolean)
        tabName = name
        checked = chkd
    End Sub
    Public tabName As String
    Public checked As Boolean
End Class


'delegate signiture for location changes on master page
Public Delegate Sub ZebraLocationChangeEventHandler(ByVal sender As Object, ByVal e As ZebraLocationChangeEventArgs)
'delegate signiture for tab changes on master page
Public Delegate Sub ZebraTabChangeEventHandler(ByVal sender As Object, ByVal e As ZebraTabChangeEventArgs)

