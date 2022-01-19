Imports System.Data
Imports cData
Partial Class Site_PermanentExclusionReversal
    Inherits System.Web.UI.Page

    Private Property LoadedPO() As String
        Get
            If Not IsNothing(Session("LoadedPO")) Then
                Return Session("LoadedPO")
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            Session("LoadedPO") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oUser As cUserInfo = Session("oUser")
        If Not IsPostBack Then
            If (cData.GetUserAuthorization(oUser.NetworkId, "/PMMPermanentExclusionReversal.aspx")) Then
                LogPageHit("/PMMPermanentExclusionReversal.aspx", oUser.NetworkId, 1, Server.MachineName, oUser.IPAddress)
            Else
                LogPageHit("/PMMPermanentExclusionReversal.aspx", oUser.NetworkId, 0, Server.MachineName, oUser.IPAddress)
                Server.Transfer("na.aspx")
            End If

        End If
        If Not IsPostBack Then
            btnReverse.Enabled = False
        End If

    End Sub

    Protected Sub btnLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        chkLines.Items.Clear()
        Try
            LoadedPO = RTrim(InitControl.Text)
            Dim ds As DataSet = cData.GetPOLineNumbers(LoadedPO)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each dr As DataRow In ds.Tables(0).Rows
                    chkLines.Items.Add(New ListItem("Line: " & dr("LINE_NO") & " " & dr("ITEM_DESCR"), dr("LINE_NO")))
                Next
                btnReverse.Enabled = True
                rbAllLines.Enabled = True
                rbSelectLines.Enabled = True
                ToggleLines()
            Else
                btnReverse.Enabled = False
                rbAllLines.Enabled = False
                rbSelectLines.Enabled = False
                rbAllLines.Checked = True
                LoadedPO = String.Empty
                lblMessage.Text = "No PO lines found that are permanently excluded!"
            End If
        Catch ex As Exception
            lblMessage.Text = ex.Message
        End Try

    End Sub

    Protected Sub rbSelectLines_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbSelectLines.CheckedChanged
        ToggleLines()
    End Sub


    Protected Sub rbAllLines_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbAllLines.CheckedChanged
        ToggleLines()
    End Sub

    Private Sub ToggleLines()
        If (rbSelectLines.Checked = True) Then
            chkLines.Enabled = True
            For i As Integer = 0 To (chkLines.Items.Count - 1)
                chkLines.Items(i).Selected = False
            Next
        Else
            chkLines.Enabled = False
            For i As Integer = 0 To (chkLines.Items.Count - 1)
                chkLines.Items(i).Selected = True
            Next
        End If
    End Sub

    Protected Sub btnReverse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReverse.Click

        Dim sb As New StringBuilder
        Dim count As Integer = 0
        For i As Integer = 0 To (chkLines.Items.Count - 1)
            If (chkLines.Items(i).Selected = True) Then
                If count <> 0 Then
                    sb.Append(",")
                End If
                sb.Append(chkLines.Items(i).Value)
                count += 1
            End If
        Next
        If ((count > 0) And (LoadedPO <> String.Empty)) Then
            Try
                cData.ReversePOExclusion(LoadedPO, sb.ToString)
                InsertChangeLog("\PMMPermanentExclusionReversal.aspx", CType(Session("oUser"), cUserInfo).Name, "", "PO_LINE", _
                                 "", "", "", "", "SUCCESS - " & "Line(s) " & sb.ToString & " on PO '" & LoadedPO & "' have been successfully reversed.")
                lblMessage.Text = "Line(s) " & sb.ToString & " on PO '" & LoadedPO & "' have been successfully reversed."
                LoadedPO = String.Empty
                InitControl.Text = String.Empty
            Catch ex As System.Exception
                lblMessage.Text = ex.Message
            Finally
                chkLines.Items.Clear()
                rbAllLines.Enabled = False
                rbSelectLines.Enabled = False
                InitControl.Focus()
                btnReverse.Enabled = False
            End Try

        Else
            lblMessage.Text = "Please select a line to reverse."
        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        lblLoadedPO.Text = LoadedPO
    End Sub
End Class
