
Partial Class Site_ParExOpenOrders
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            cData.LogPageHit("ParExOpenOrders.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            With dgOpenOrders
                .DataSource = cData.GetParExOpenOrders()
                .DataBind()
            End With
            If dgOpenOrders.Items.Count < 1 Then
                lblNoOrders.Visible = True
            End If

        End If




    End Sub

    Public Function formatDate(ByVal fmtDate As String) As String
        Dim strYear As New StringBuilder
        Dim strDay As New StringBuilder
        Dim strMonth As New StringBuilder
        Dim charCount As Integer = 1

        For Each c As Char In fmtDate.ToCharArray
            Select Case charCount
                Case Is <= 4
                    strYear.Append(c)
                Case Is <= 6
                    strMonth.Append(c)
                Case Is <= 8
                    strDay.Append(c)
                Case Else
                    Exit For
            End Select
            charCount += 1
        Next

        Return strMonth.ToString & "/" & strDay.ToString & "/" & strYear.ToString

    End Function


    Protected Sub dgOpenOrders_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgOpenOrders.ItemDataBound

        Dim dgo As DataGridItem = e.Item

        If (dgo.ItemType = ListItemType.Item) Or (dgo.ItemType = ListItemType.AlternatingItem) Then
            Dim dgd As DataGrid = dgo.FindControl("dgReqDetail")
            Dim lbl As Label = dgo.FindControl("lblReqNumber")
            Dim lblPmm As Label = dgo.FindControl("lblPmmInfo")
            With dgd
                .DataSource = cData.GetParExReqDetail(lbl.Text)
                .DataBind()
            End With
            If dgd.Items.Count < 1 Then
                lblPmm.Visible = True
            End If

        End If


    End Sub

    Protected Sub initControl_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles initControl.SelectedIndexChanged
        With dgOpenOrders
            .DataSource = cData.GetParExOpenOrders(initControl.SelectedValue)
            .DataBind()
        End With
        If dgOpenOrders.Items.Count < 1 Then
            lblNoOrders.Visible = True
        End If
    End Sub
End Class
