Imports Microsoft.VisualBasic
Imports cData
Imports System.Collections.Generic
Imports System.Text.RegularExpressions
Imports System.Net.Mail
Imports System.ComponentModel
Imports System.Web.UI
Imports System.Diagnostics
Imports cUtilities
Imports System.Security.Cryptography
Imports System.DirectoryServices

Public Enum eDirection
    Ascending
    Descending
End Enum
Public Class cUtilities
    '/************************************************************************************************
    '*  NAME: cUtilities
    '*	DESC: Project Utilities class
    '*	AUTHOR: Kenneth Casson
    '*	DATE: 3/20/2009
    '*		
    '*		Modifications:
    '*			- none
    '*
    '************************************************************************************************/
    Public Shared Function BreakLine(ByVal st As String, ByVal length As Integer) As String
        'Returns limited string with breaks at length parameter
        Dim sb As New StringBuilder
        If st.Length > length Then
            Dim c() As Char = st.ToCharArray
            Dim i As Integer = 1
            For Each Chr As Char In c
                If i = length Then
                    sb.Append("<br>")
                    i = 1
                End If
                sb.Append(Chr)
                i += 1
            Next
            st = sb.ToString
        End If
        Return st

    End Function
    Public Shared Function checkDBNull(ByVal val As Object) As String
        If IsDBNull(val) Then
            Return String.Empty
        Else
            Return val.ToString
        End If

    End Function
    Public Shared Function isNumeric(val As Object) As Boolean
        Dim retrn As Boolean
        Try
            Dim i As Double = val
            retrn = True
        Catch ex As Exception
            retrn = False
        End Try
        Return retrn

    End Function
    Public Shared Function SendEmail(ByVal mailTo As String, ByVal mailFrom As String, ByVal mailSubject As String, ByVal Message As String) As Boolean

        Dim mm As New MailMessage(mailFrom, mailTo, mailSubject, Message)
        Return EmailWork(mm)

    End Function

    Public Shared Function SendEmail(ByVal mailTo As MailAddress, ByVal mailFrom As MailAddress, ByVal mailSubject As String, ByVal htmlBody As String) As Boolean

        Dim mm As New MailMessage(mailFrom, mailTo)
        mm.IsBodyHtml = True
        mm.Subject = mailSubject
        mm.Body = htmlBody
        Return EmailWork(mm)

    End Function

    Private Shared Function EmailWork(ByVal mm As MailMessage) As Boolean
        Dim booReturn As Boolean = False
        Dim cl As New SmtpClient("mail.stelizabeth.com")
        Try
            'send email
            '  cl.Send(mm)
            booReturn = True
        Catch ex As Exception
            booReturn = False
            LogEvent(ex.Message, EventLogEntryType.Error, 5001)
        Finally
            'clean up
            mm.Dispose()
            mm = Nothing
            cl = Nothing
        End Try
        Return booReturn
    End Function

    Public Shared Function LogEvent(ByVal message As String, ByVal Type As System.Diagnostics.EventLogEntryType, ByVal EventID As Integer) As Boolean
        Dim rtn As Boolean = True
        ' This code logs entries to a text file within the web data directory
        Dim fs As New IO.FileStream(cGlobal.ApplicationPath & "\App_Data\WebEventLog.log", IO.FileMode.Append)
        Dim sw As New IO.StreamWriter(fs)
        Try
            sw.WriteLine(Now & " - (" & Type & "/" & EventID & ") " & message)
            sw.Flush()
            sw.Close()
        Catch
            rtn = False
        End Try

        'Send me an email so I can fix the issue
        SendEmail("kenneth.casson@stelizabeth.com", "pmm@stelizabeth.com", "Web Error Message", message)

        ''Win Event log code, requires ASPNET server security rights added...
        'If Not EventLog.SourceExists("MaterialsMngWeb") Then
        '    EventLog.CreateEventSource("MaterialsMngWeb", "Application")
        'End If
        'Dim el As New EventLog
        'el.Source = "MaterialsMngWeb"
        'Try
        '    el.WriteEntry("MaterialsMngWeb - " & message, Type, EventID)
        'Catch
        'Finally
        '    el = Nothing
        'End Try

        Return rtn
    End Function
    Public Shared Function FindControlRecursive(ByVal root As Control, ByVal Id As String) As Control

        If root.ID = Id Then
            Return root
        End If

        For Each ctl As Control In root.Controls
            Dim FoundCtl As Control = FindControlRecursive(ctl, Id)
            If Not (FoundCtl Is Nothing) Then
                Return FoundCtl
            End If
        Next

        Return Nothing

    End Function
    Public Shared Function SearchActiveDirectory(ByVal networkId As String, ByVal displayName As String, ByVal surName As String) As System.directoryservices.SearchResultCollection

        Dim userID As String = ConfigurationManager.AppSettings("DomainUser").ToString
        Dim password As String = ConfigurationManager.AppSettings("DomainUserPass").ToString
        Dim GlobalCat As String = ("GC://dc=sehp,dc=stelizabeth,dc=com")
        Dim rootEntry As New DirectoryEntry(GlobalCat, _
                          userID, _
                          password)
        Dim results As SearchResultCollection


        Dim deSearch As New DirectorySearcher(rootEntry)
            If (networkId <> String.Empty) And (displayName <> String.Empty) Then
                deSearch.Filter = String.Format("(&(SAMAccountName={0})(displayName={1}))", networkId, displayName)
            ElseIf (networkId <> String.Empty) And (displayName = String.Empty) Then
                deSearch.Filter = String.Format("(&(SAMAccountName={0}))", networkId)
            ElseIf (networkId = String.Empty) And (displayName <> String.Empty) Then
                deSearch.Filter = String.Format("(&(displayName=*{0}*)(!(OU=Disabled Users)))", displayName)
            End If


            'get the group result
            results = deSearch.FindAll

        'End If

        rootEntry.Close()


        Return results
    End Function
    Public Shared Sub DataGridItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs, ByVal CountResults As Integer)

        If e.Item.ItemType = ListItemType.Pager Then
            ' Change label of the pager
            Dim cString As String = "Page "
            Dim oTableCell As TableCell
            Dim oSpecialCell As New TableCell
            Dim oControl As Control
            Dim oResultsTableCell As New TableCell
            Dim dg As DataGrid = CType(sender, DataGrid)

            oSpecialCell.Text = ""


            With oResultsTableCell
                .Wrap = True
                .Text = "Results: "

                If CountResults > 0 Then
                    If dg.PageSize * (dg.CurrentPageIndex + 1) <= CountResults Then
                        .Text &= CStr(dg.PageSize * (dg.CurrentPageIndex) + 1) & " - " & CStr(dg.PageSize * (dg.CurrentPageIndex + 1))
                        .Text &= " of " & CStr(CountResults) & " " & CStr(IIf(CountResults = 1, "record", "records"))
                    Else
                        .Text &= CStr(dg.PageSize * (dg.CurrentPageIndex) + 1) & " - " & CStr(CountResults)
                        .Text &= " of " & CStr(CountResults) & " " & CStr(IIf(CountResults = 1, "record", "records"))
                    End If
                Else
                    .Text &= CStr(CountResults) & " " & CStr(IIf(CountResults = 1, "record", "records"))
                End If

                .HorizontalAlign = HorizontalAlign.Left
                .VerticalAlign = VerticalAlign.Middle
                .CssClass = "itemlistheadsmall"
            End With

            For Each oTableCell In e.Item.Cells
                For Each oControl In oTableCell.Controls
                    If TypeOf oControl Is System.Web.UI.WebControls.Label Then
                        CType(oControl, Label).Text = cString & CType(oControl, Label).Text
                    Else
                        If TypeOf oControl Is System.Web.UI.WebControls.LinkButton Then
                            CType(oControl, LinkButton).Text = "[" & CType(oControl, LinkButton).Text & "]"
                        End If
                    End If
                Next
                oSpecialCell = oTableCell
            Next

            While e.Item.Cells.Count > 0
                e.Item.Cells.RemoveAt(0)
            End While

            Dim oNewCell As New TableCell

            Dim oTable As New Table
            Dim oTableRow As New TableRow
            With oTableRow.Cells
                .Add(oResultsTableCell)
                .Add(oSpecialCell)
            End With

            Dim oUnit As New Unit(100D, UnitType.Percentage)

            With oTable
                .Width = oUnit
                .Rows.Add(oTableRow)
            End With

            With oNewCell
                .HorizontalAlign = HorizontalAlign.Left
                .VerticalAlign = VerticalAlign.Middle
                .Width = oUnit
                .Controls.Add(oTable)
            End With

            With oSpecialCell
                .HorizontalAlign = HorizontalAlign.Right
                .VerticalAlign = VerticalAlign.Middle
                .CssClass = "itemlistheadsmall"
            End With

            e.Item.Cells.Add(oNewCell)

        End If
    End Sub
End Class

Public Class cGlobal
    '/************************************************************************************************
    '*  NAME: cGlobal
    '*	DESC: Project Globals class
    '*	AUTHOR: Kenneth Casson
    '*	DATE: 10/30/2009
    '*		
    '*		Modifications:
    '*			- none
    '*
    '************************************************************************************************/
    Private Shared _ApplicationPath As String
    Public Shared Property ApplicationPath() As String
        Get
            Return _ApplicationPath
        End Get
        Set(ByVal value As String)
            _ApplicationPath = value
        End Set
    End Property
End Class
Public Class cPMMEncryption
    '/************************************************************************************************
    '*  NAME: cPMMEncryption
    '*	DESC: This class is used to encrypt and decrypt strings.  It's main purpose is to
    '*            allow the St. E help desk to reset passwords.
    '*	AUTHOR: Kenneth Casson
    '*	DATE: 8/11/2009
    '*		
    '*		Modifications:
    '*			- Kenneth Casson, 11/4/2009
    '*              Added decrypt functionality and commenting. 
    '*                  Limitations at this point
    '*                      * Maximum of 11 characters 
    '*                      * CaseSensitive not working
    '*          - Kenneth Casson, 7/16/2010
    '*              Fixed v15 bug with keys                    
    '*
    '************************************************************************************************/

    Public Enum eCaseType
        CaseIncensitive
        CaseSensitive
    End Enum


    Private _CharKey As New Hashtable
    Private _NumKey As New Hashtable
    Private _Case As eCaseType

    Public Property CharKey() As Hashtable
        Get
            Return _CharKey
        End Get
        Set(ByVal value As Hashtable)
            _CharKey = value
        End Set
    End Property

    Public Property NumKey() As Hashtable
        Get
            Return _NumKey
        End Get
        Set(ByVal value As Hashtable)
            _NumKey = value
        End Set
    End Property

    Public Sub New()
        LoadDefaultValues()
    End Sub

    Public Sub New(ByVal caseType As eCaseType)
        LoadDefaultValues()
        _Case = caseType
    End Sub

    Public Function encryptString(ByVal str As String) As String
        Dim sb As New StringBuilder
        Dim i As Integer

        If (Len(str) >= 8) And (Len(str) <= 11) Then
            Try
                If _Case = eCaseType.CaseIncensitive Then
                    str = UCase(str)
                End If
                Dim ca As Char() = str.ToCharArray
                Dim x As Integer = 0
                Dim frm As Integer = ((str.Length - str.Length) - (str.Length - 8))
                For i = frm To (str.Length - (str.Length - 7)) Step 1
                    sb.Append(Chr(Math.Abs(toDecimal(ca(x), i))))
                    x += 1
                Next

                revStr(sb)

            Catch ex As Exception
                LogEvent("Exception Thrown by the following; Function: EncryptString Class: cPMMEncryption " & _
                    "Message: " & ex.Message, EventLogEntryType.Error, 50001)
                Throw New System.Exception("Error encrypting string, Message: " & ex.Message)
            End Try
        Else
            Throw New System.Exception("String must be between 8 and 11 characters in length!")

        End If

        Return sb.ToString
    End Function

    Public Function decryptString(ByVal str As String) As String
        Dim sbr As New StringBuilder
        Dim sb As New StringBuilder
        Dim i As Integer

        If (Len(str) >= 8) And (Len(str) <= 11) Then
            Try
                sbr.Append(RTrim(str))
                revStr(sbr)
                str = sbr.ToString

                Dim ca As Char() = str.ToCharArray
                Dim x As Integer = 0
                Dim frm As Integer = ((str.Length - str.Length) - (str.Length - 8))
                For i = frm To (str.Length - (str.Length - 7)) Step 1
                    sb.Append(Chr(Math.Abs(toCharacter(ca(x), i))))
                    x += 1
                Next

            Catch ex As Exception
                LogEvent("Exception Thrown by the following; Function: EncryptString Class: cPMMEncryption " & _
                    "Message: " & ex.Message, EventLogEntryType.Error, 50001)
                Throw New System.Exception("Error encrypting string, Message: " & ex.Message)
            End Try
        Else
            Throw New System.Exception("String must be between 8 and 11 characters in length!")

        End If

        Return sb.ToString
    End Function

    Private Function toDecimal(ByVal str As String, ByVal position As Integer) As Integer

        Dim dec As Integer = 0
        'Do the math
        If Not Regex.IsMatch(str, "[a-zA-Z]") Then
            dec = (Asc(str) + NumKey(position))
        Else
            dec = (Asc(str) + CharKey(position))
        End If

        Return ValueCheck(dec)

    End Function


    Private Function toCharacter(ByVal str As String, ByVal position As Integer) As Integer

        Dim dec As Integer = 0
        'Do the math
        If CheckNumeric(str, position) Then
            dec = (Asc(str) - NumKey(position))
        Else
            dec = (Asc(str) - CharKey(position))
        End If

        Return ValueCheck(dec)

    End Function

    Private Function ValueCheck(ByVal value As Integer) As Integer
        Dim rtn As Integer = 0

        If (Math.Abs(value) > 126) Then
            rtn = ((Math.Abs(value) - 126) + 32)
        ElseIf ((value) < 33) Then
            rtn = ((126 - Math.Abs(value)) - 32) 'do some more testing here... then deploy
        Else
            rtn = value
        End If

        Return rtn

    End Function

    Private Function CheckNumeric(ByVal s As String, ByVal pos As Integer) As Boolean
        'this private function is used for decryption.  It determins which Key to use (i.e. Numeric/Character).
        Dim rtn As Boolean = False
        Dim ch() As Char
        Select Case pos
            Case Is = -3
                ch = "jihgfedcba`_!""#%&'()*+,-./0125PQRUpqr".ToCharArray
            Case Is = -2
                ch = "lmnopqrstuvwxyz|}~!""#$%&'()*+.IJKNijk ".ToCharArray
            Case Is = -1
                ch = "efghijklmnopqrsuvwxyz{|}~!""#$'BCDGbcd".ToCharArray
            Case Is = 0
                ch = "^_`abcdefghijklnopqrstuvwxyz{~;<=@[\]".ToCharArray
            Case Is = 1
                ch = "WXYZ[\]^_`abcdeghijklmnopqrstw4569TUV".ToCharArray
            Case Is = 2
                ch = "PQRSTUVWXYZ[\]^`abcdefghijklmp-./2MNO".ToCharArray
            Case Is = 3
                ch = "IJKLMNOPQRSTUVWYZ[\]^_`abcdefi&'(+FGH".ToCharArray
            Case Is = 4
                ch = "BCDEFGHIJKLMNOPRSTUVWXYZ[\]^_b}~!$?@A".ToCharArray
            Case Is = 5
                ch = ";<=>?@ABCDEFGHIKLMNOPQRSTUVWX[vwx{89:".ToCharArray
            Case Is = 6
                ch = "456789:;<=>?@ABDEFGHIJKLMNOPQTopqt123".ToCharArray
            Case Is = 7
                ch = "-./0123456789:;=>?@ABCDEFGHIJMhijm*+,".ToCharArray
            Case Else
                Throw New System.Exception("Position invalid for current procedure")
        End Select
        For Each c As Char In ch
            If c = s Then
                rtn = True
                Exit For
            End If
        Next
        Return rtn
    End Function

    Public Function getNumEncValues() As String
        Dim ca As Char() = " !""#$%&'()*+,-.0123456789:;<=@[\]`{|}".ToCharArray
        Dim sb As New StringBuilder
        For Each c As Char In ca
            sb.Append(Chr(ValueCheck(toDecimal(c, 5))))
        Next
        Return sb.ToString
    End Function

    Private Sub LoadDefaultValues()
        'Initialize class with the default keys
        'Character
        CharKey.Add(-3, 21) '1
        CharKey.Add(-2, 14) '2
        CharKey.Add(-1, 7)  '3
        CharKey.Add(0, 0)   '4 
        CharKey.Add(1, -7)  '5
        CharKey.Add(2, -14) '6
        CharKey.Add(3, -21) '7 (21
        CharKey.Add(4, -28) '8 (28
        CharKey.Add(5, -35)  '9
        CharKey.Add(6, 52)  '10 -42
        CharKey.Add(7, 45)  '11

        'Numeric 
        NumKey.Add(-3, -11)
        NumKey.Add(-2, 76)
        NumKey.Add(-1, 69)
        NumKey.Add(0, 62)
        NumKey.Add(1, 55)
        NumKey.Add(2, 48)
        NumKey.Add(3, 41)
        NumKey.Add(4, 34)
        NumKey.Add(5, 27)
        NumKey.Add(6, 20)
        NumKey.Add(7, 13)


        _Case = eCaseType.CaseIncensitive
    End Sub

    Private Sub revStr(ByRef sb As StringBuilder)
        Dim c As Char() = sb.ToString
        Dim sr As New StringBuilder
        Dim i As Integer

        For i = (c.Length - 1) To 0 Step -1
            sr.Append(c(i))
        Next

        sb.Replace(sb.ToString, sr.ToString)

    End Sub



End Class

<Serializable()> _
Public Structure sIDescSearchCriteria
    Public itemNumber As String
    Public mfrNumber As String
    Public desc As String
End Structure

<Serializable()> _
Public Class cADUser
    Public Id As String
    Public IdOnly As String
    Public Name As String
    Public Title As String
    Public Phone As String
    Public email As String
    Private _AuthorizedPages As New Generic.List(Of ListItem)

    Public Property AuthorizedPages As Generic.List(Of ListItem)
        Get
            Return _AuthorizedPages
        End Get
        Set(ByVal value As Generic.List(Of ListItem))
            _AuthorizedPages = value
        End Set

    End Property







    Public Sub New(ByVal pid As String, ByVal pName As String, ByVal pTitle As String, ByVal pPhone As String, pEmail As String)
        Id = IIf(InStr(pid, "SEHP\") = False, ("SEHP\" & pid), pid)
        IdOnly = Right(pid, (Len(pid) - InStr(pid, "\")))
        Name = pName
        Title = pTitle
        Phone = pPhone
        email = pEmail

    End Sub
End Class
'************************************************************************
'* Left Nav classes
'* Author: Kenneth Casson
'* Date: 3/19/2010
'************************************************************************
<Serializable()> _
Public Class cSubList

    Public ListNumber As Integer
    Public Links As New List(Of cLink)

End Class
<Serializable()> _
Public Class cLink
    Public Enum Targets
        _blank
        _same
    End Enum
    Private _NavURL As String
    Private _LinkText As String
    Private _Target As String
    Private _ListOrder As Integer

    Public Property NavURL() As String
        Get
            Return _NavURL
        End Get
        Set(ByVal value As String)
            _NavURL = value
        End Set
    End Property

    Public Property LinkText() As String
        Get
            Return _LinkText
        End Get
        Set(ByVal value As String)
            _LinkText = value
        End Set
    End Property

    Public Property target() As Targets
        Get
            Return _Target
        End Get
        Set(ByVal value As Targets)
            _Target = value
        End Set
    End Property
    Public Property ListOrder() As Integer
        Get
            Return _ListOrder
        End Get
        Set(ByVal value As Integer)
            _ListOrder = value
        End Set
    End Property
    Public Sub New(ByVal pNavURL As String, ByVal pLinkText As String, ByVal pTarget As Targets, ByVal pListOrder As Integer)
        _NavURL = pNavURL
        _LinkText = pLinkText
        _Target = pTarget
        _ListOrder = pListOrder
    End Sub
End Class

