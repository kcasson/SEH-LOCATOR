Imports Microsoft.VisualBasic
Imports System.DirectoryServices
Imports cUtilities

Public Class cUserInfo

    Private _NetworkId As String = String.Empty
    Private _Name As String = String.Empty
    Private _IsAdmin As Boolean = False
    Private _IPAddress As String = String.Empty
    Private _UserName As String = String.Empty

    Public ReadOnly Property NetworkId() As String
        Get
            Return _NetworkId
        End Get
    End Property
    Public ReadOnly Property NetworkName() As String
        Get
            Dim l As Integer = InStr(_NetworkId, "\")
            Return Mid(_NetworkId, (l + 1))
        End Get
    End Property

    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Public ReadOnly Property IsAdmin() As Boolean
        Get
            Return _IsAdmin
        End Get
    End Property

    Public ReadOnly Property IPAddress() As String
        Get
            Return _IPAddress
        End Get
    End Property

    Public ReadOnly Property UserName() As String
        Get
            Return _UserName
        End Get
    End Property
    Public Sub New()

    End Sub

    Public Sub New(ByVal request As HttpRequest)
        _NetworkId = request.LogonUserIdentity.Name
        ' _NetworkId = HttpContext.Current.User.Identity.Name
        _IsAdmin = cData.IsAdmin(NetworkName)
        _IPAddress = request.ServerVariables("REMOTE_ADDR")
        _UserName = GetUserName(request.LogonUserIdentity.Name)
        ' _UserName = request.LogonUserIdentity.
    End Sub

    Private Function GetUserName(ByVal name As String) As String

        Dim ba() As Char = name.ToCharArray
        name = String.Empty
        For Each c As Char In ba
            If (c = "\") Or (Len(name) > 0) Then
                name += c
            End If
        Next
        name = Replace(name, "\", "")
        Dim rootEntry As New DirectoryEntry("GC://dc=sehp,dc=stelizabeth,dc=com", _
                  ConfigurationManager.AppSettings("DomainUser"), _
                  ConfigurationManager.AppSettings("DomainUserPass"))

        Dim searcher As New DirectorySearcher(rootEntry)
        searcher.Filter = "(&(objectCategory=person)(sAMAccountname=" & name & "))"
        Dim result As SearchResult
        Try
            result = searcher.FindOne
            If (Not IsNothing(result)) Then
                If (result.Properties.Contains("cn")) Then
                    name = result.Properties("cn")(0)
                End If
            End If

        Catch ex As Exception
            'user id will be returned, let the program continue to process...
            LogEvent(ex.Message, System.Diagnostics.EventLogEntryType.Error, 50001)
        End Try

        Return name
    End Function

End Class
