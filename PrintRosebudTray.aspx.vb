Imports System.Data
Partial Class PrintRosebudTray
    Inherits System.Web.UI.Page

    Private Property drCount() As Integer
        Get
            If (Not ViewState("drCount") = Nothing) Then
                Return ViewState("drCount")
            Else
                Return 0
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("drCount") = value
        End Set
    End Property


    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then

            If (Not Session("dsRecipe") Is Nothing) And (Not Session("dsLabel") Is Nothing) Then

                'fill the labels
                Page.Title = CStr(Session("dsLabel"))
                Dim ds As DataSet = CType(Session("dsRecipe"), DataSet)
                Dim cnt As Integer = 0
                Dim lc As Integer = 0
                Dim pc As Integer = 1
                Dim sb As New StringBuilder
                'create the tables
                For Each rw As DataRow In ds.Tables(0).Rows
                    'Get the actual instrument count
                    If IsNumeric(rw("Qty")) Then
                        cnt += CInt(rw("Qty"))
                    End If
                Next
                lblTrayName.Text = CStr(Session("dsLabel"))
                lblCount.Text = CStr(cnt)

                sb.Append("<table width=""650px"" border=""1px"" cellpadding=""0"" cellspacing=""0"">")
                'header
                sb.Append("<tr class=""printRowHead""><td align=""center"">Qty</td><td>Model</td><td>Description" & _
                                    "</td><td>PMM #</td><td>Qty Used</td></tr>")
                'lines
                For Each dr As DataRow In ds.Tables(0).Rows
                    lc += 1
                    sb.Append("<tr class=""" & GetClass(dr("Qty")) & _
                              """><td align=""center"">" & dr("Qty") & "</td><td>" & GetValue(dr("Model")) & _
                                "</td><td>" & GetValue(Left(dr("Description"), 48)) & _
                                "</td><td>" & " " & "</td><td>&nbsp;</td></tr>")
                    '"</td><td>" & GetValue(dr("PMM")) & "</td><td>&nbsp;</td></tr>")
                    If ((lc = 49) And (pc < 2)) Or ((lc = 50) And (pc > 1)) Then 'New page
                        sb.Append("</table>")
                        sb.Append("<table border=""0""><tr height=""20px""><td>&nbsp;</td></tr></table>" & _
                                  "<table width=""650px"" border=""1px"" cellpadding=""0"" cellspacing=""0"">")
                        sb.Append("<tr class=""printRowHead""><td align=""center"">Qty</td><td>Model</td><td>Description" & _
                                        "</td><td>PMM #</td><td>Qty Used</td></tr>")
                        lc = 0
                        pc += 1
                    End If
                Next
                sb.Append("</table>")
                If Not (IsNothing(Session("oUser"))) Then
                    sb.Append("<table><tr><td>&nbsp;</td></tr><tr><td><b>Tray packed by: </b>" & _
                              CType(Session("oUser"), cUserInfo).UserName & "</td></tr></table>")
                End If

                lblOutput.Text = sb.ToString
            Else
                lblTrayName.Text = "Error loading data! <br> Please reload page.<br><br>" & _
                            "<a href='http://pmm:82/RosebudTrayView.aspx'>Reload Page</a>"
            End If
        End If
    End Sub

    Public Function GetClass(ByVal str As String) As String
        Dim rtn As String = "itemlistitem"
        If (str = "-") Then
            rtn = "printRowAlt"
        End If

        Return rtn
    End Function

    Public Function GetValue(ByVal str As String) As String
        If str = "-" Then
            str = "&nbsp;"
        End If

        Return str
    End Function

End Class
