Imports cData
Imports System.Data
Imports System.IO
Imports System.Net

Partial Class Site_ARInterface
    Inherits System.Web.UI.Page

    Protected Friend Enum fileType
        flat
        excel
    End Enum

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'Check security 
        If (IsNothing(HttpContext.Current.Session("oUser")) = False) Then
            If (Not cData.GetUserAuthorization(User.Identity.Name, "ARInterface.aspx")) Then
                LogPageHit("ARInterface.aspx", User.Identity.Name, 0, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
                Server.Transfer("../Site/na.aspx?u=" & User.Identity.Name)
            Else
                LogPageHit("ARInterface.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            End If
        Else
            Server.Transfer("timeoutError.aspx")
        End If
        'set initial focus
        LoadFocus.Focus()
        If Not IsPostBack Then
            initializePage()
        End If
    End Sub

    Private Sub initializePage()
        'ScriptManager.RegisterClientScriptInclude(sm1, Me.GetType, "key", "~\script\ajaxScript.js")

        refreshSettings()
        refreshCorps()
        loadExpCodes()
        loadCostCenters()
        ' refreshAccounts()

    End Sub
    Private Sub refreshSettings()
        'load available setting
        Dim ds As DataSet = GetARSettings("STAR")
        If ds.Tables(0).Rows.Count > 0 Then
            For Each r As DataRow In ds.Tables(0).Rows
                LoadFocus.Text = r("FiscalYear")
                Session("FiscalYear") = r("FiscalYear")
                txtFiscalPeriod.Text = r("FiscalPeriod")
                Session("FiscalPeriod") = r("FiscalPeriod")
                txtSourceCode.Text = r("SourceCode")
                Session("SourceCode") = r("SourceCode")
            Next
        End If


    End Sub
    Private Sub refreshCorps()
        lbAvailCorps.Items.Clear()
        lbSelectedCorps.Items.Clear()
        Dim cds As DataSet = GetARCorps()
        For Each r As DataRow In cds.Tables(0).Rows
            If r("OffsetCorpID") Is DBNull.Value Then
                lbAvailCorps.Items.Add(New ListItem(RTrim(r("ACCT_NO")) & " - " & RTrim(r("NAME")), r("CORP_ID")))
            Else
                lbSelectedCorps.Items.Add(New ListItem(RTrim(r("ACCT_NO")) & " - " & RTrim(r("NAME")) & " / " & r("Offset"), r("CORP_ID")))
            End If

        Next
    End Sub

    Private Sub loadExpCodes()
        Dim sql As String = "select distinct rtrim(EXP_CODE_ACCT_NO) EXP_CODE_ACCT_NO, EXP_CODE_NAME from ACCR_RECP  order by EXP_CODE_ACCT_NO"
        Dim ds As DataSet = cData.ExecuteSQL(sql)

        For Each dr As DataRow In ds.Tables(0).Rows
            lbExpCodes.Items.Add(New ListItem(dr("EXP_CODE_ACCT_NO") & " - " & dr("EXP_CODE_NAME"), dr("EXP_CODE_ACCT_NO")))
        Next

    End Sub

    Private Sub loadCostCenters()
        Dim sql As String = "select distinct rtrim(CC_ACCT_NO) CC_ACCT_NO, CC_NAME from ACCR_RECP order by CC_ACCT_NO"
        Dim ds As DataSet = cData.ExecuteSQL(sql)

        For Each dr As DataRow In ds.Tables(0).Rows
            lbCostCenter.Items.Add(New ListItem(dr("CC_ACCT_NO") & " - " & dr("CC_NAME"), dr("CC_ACCT_NO")))
        Next

    End Sub

    'Private Sub refreshAccounts()
    '    lbExcludedAccounts.Items.Clear()
    '    lbIncludedAccounts.Items.Clear()

    '    Dim currentAccount As String = String.Empty
    '    Dim ds As DataSet = GetARAccounts()
    '    For Each r As DataRow In ds.Tables(0).Rows
    '        If (currentAccount <> CStr(r("CORP_ID"))) And Not (ddlCorps.Items.Contains(New ListItem(r("CORP_ACCT_NO") & "-" & r("CORP_NAME"), r("CORP_ID")))) Then
    '            ddlCorps.Items.Add(New ListItem(r("CORP_ACCT_NO") & "-" & r("CORP_NAME"), r("CORP_ID")))
    '            currentAccount = CStr(r("CORP_ID"))
    '        End If
    '        If (r("EXC_ACCT") Is DBNull.Value) Then
    '            If ddlCorps.SelectedValue = r("CORP_ID") Then
    '                Dim li As New ListItem(r("ACCT_TEXT") & "  (" & r("CC_NAME") & "-" & r("EXP_CODE_NAME") & ")", r("CORP_ID") & "|" & r("CC_ID") & "|" & r("EXP_CODE_ID"))
    '                If r("INACTIVE") = "Y" Then 'color inactive
    '                    li.Attributes("style") = "color:red;"
    '                End If
    '                lbExcludedAccounts.Items.Add(li)
    '            End If
    '        Else
    '            If ddlCorps.SelectedValue = r("CORP_ID") Then
    '                Dim li As New ListItem(r("ACCT_TEXT") & "  (" & r("CC_NAME") & "-" & r("EXP_CODE_NAME") & ")", r("CORP_ID") & "|" & r("CC_ID") & "|" & r("EXP_CODE_ID"))
    '                If r("INACTIVE") = "Y" Then 'color inactive
    '                    li.Attributes("style") = "color:red;"
    '                End If
    '                lbIncludedAccounts.Items.Add(li)
    '            End If
    '        End If
    '    Next

    'End Sub

    Protected Sub btnUpdateOffset_Click(sender As Object, e As System.EventArgs) Handles btnUpdateOffset.Click
        If btnUpdateOffset.Text <> String.Empty Then
            If lbSelectedCorps.SelectedValue <> String.Empty Then
                Dim selected() As Integer = lbSelectedCorps.GetSelectedIndices
                For i As Integer = 0 To (selected.Length - 1)
                    Try
                        Dim oldValue As String = lbSelectedCorps.Items(selected(i)).Text
                        Dim newValue As String = txtOffset.Text
                        alterARCorps(lbSelectedCorps.Items(selected(i)).Value, "3025", txtOffset.Text, CType(Session("oUser"), cUserInfo).NetworkId, False)
                        txtOffset.Text = String.Empty
                        txtOffset.Focus()
                        lblErrorMsg.Text = "Offset added successfully!"
                        lblErrorMsg.Visible = True
                        'log it
                        InsertChangeLog("ARInterface.aspx", CType(Session("oUser"), cUserInfo).NetworkId, "", "AR_Offsets", lbSelectedCorps.Items(selected(i)).Value, "Offset", oldValue, newValue, "Offset Modified Successfully")
                    Catch ex As Exception
                        lblErrorMsg.Text = ex.Message
                        lblErrorMsg.Visible = True
                    End Try
                Next
                refreshCorps()
                txtOffset.Focus()
            Else
                lblErrorMsg.Text = "Please select a Corporation to add this offset"
                lblErrorMsg.Visible = True
            End If
        End If

    End Sub

    Protected Sub btnUpdatePrefs_Click(sender As Object, e As System.EventArgs) Handles btnUpdatePrefs.Click
        If (LoadFocus.Text <> String.Empty) And _
    (txtFiscalPeriod.Text <> String.Empty) And _
     (txtSourceCode.Text <> String.Empty) Then
            Try
                UpdateARSettings("STAR", LoadFocus.Text, txtFiscalPeriod.Text, CType(Session("oUser"), cUserInfo).NetworkId, txtSourceCode.Text)
                lblErrorMsg.Text = "Settings have been updated successfully!"
                lblErrorMsg.Visible = True
                'determine old values from session variables
                Dim old As String = String.Empty
                old = IIf(Not IsNothing(Session("FiscalYear")), ("FiscalYear:" & Session("FiscalYear") & " FiscalPeriod:" & Session("FiscalPeriod") & _
                    " SourceCode:" & Session("SourceCode")), String.Empty)
                InsertChangeLog("ARInterface.aspx", CType(Session("oUser"), cUserInfo).NetworkId, "", "AR_Settings", "", "All", old _
                                , "FiscalYear:" & LoadFocus.Text & " FiscalPeriod: " & txtFiscalPeriod.Text & " SourceCode: " & txtSourceCode.Text, "STAR Settings Updated Successfully")
            Catch ex As Exception
                lblErrorMsg.Text = ex.Message
                lblErrorMsg.Visible = True
            End Try
        End If
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click

        If lbAvailCorps.SelectedValue <> "" Then
            Dim selected() As Integer = lbAvailCorps.GetSelectedIndices
            For i As Integer = 0 To (selected.Length - 1)
                Try
                    alterARCorps(lbAvailCorps.Items(selected(i)).Value, "3025", "", CType(Session("oUser"), cUserInfo).NetworkId, False)
                Catch ex As Exception
                    lblErrorMsg.Text = ex.Message
                    lblErrorMsg.Visible = True
                End Try
            Next
            refreshCorps()
        Else
            lblErrorMsg.Text = "Please select a corporation to add."
            lblErrorMsg.Visible = True
        End If

    End Sub

    Protected Sub btnDelete_Click(sender As Object, e As System.EventArgs) Handles btnDelete.Click
        If lbSelectedCorps.SelectedValue <> "" Then
            Dim selected() As Integer = lbSelectedCorps.GetSelectedIndices
            For i As Integer = 0 To (selected.Length - 1)
                Try
                    alterARCorps(lbSelectedCorps.Items(selected(i)).Value, "3025", "", CType(Session("oUser"), cUserInfo).NetworkId, True)
                Catch ex As Exception
                    lblErrorMsg.Text = ex.Message
                    lblErrorMsg.Visible = True
                End Try

            Next
            refreshCorps()
        Else
            lblErrorMsg.Text = "Please select a corporation to delete."
            lblErrorMsg.Visible = True
        End If
    End Sub

    'Protected Sub ddlCorps_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCorps.SelectedIndexChanged
    '    refreshAccounts()
    'End Sub

    'Protected Sub btnAddAcct_Click(sender As Object, e As System.EventArgs) Handles imgExcludeAcct.Click
    '    If lbIncludedAccounts.SelectedValue <> "-1" Then
    '        Dim selected() As Integer = lbIncludedAccounts.GetSelectedIndices
    '        For i As Integer = 0 To (selected.Length - 1)
    '            'get the primary key values
    '            Dim cValues() As String = lbIncludedAccounts.Items(selected(i)).Value.Split("|")
    '            Dim corp As Integer = CInt(cValues(0))
    '            Dim cc As Integer = CInt(cValues(1))
    '            Dim expCode As Integer = CInt(cValues(2))
    '            Try
    '                alterARAccts(corp, cc, expCode, CType(Session("oUser"), cUserInfo).NetworkId, True)
    '            Catch ex As Exception
    '                'This has been logged to the system log. Errors will be emailed to support
    '            End Try
    '        Next
    '        refreshAccounts()
    '    End If
    '    imgIncludeAcct.Enabled = True

    'End Sub

    'Protected Sub btnDeleteAcct_Click(sender As Object, e As System.EventArgs) Handles imgIncludeAcct.Click
    '    If lbExcludedAccounts.SelectedValue <> "-1" Then
    '        Dim selected() As Integer = lbExcludedAccounts.GetSelectedIndices
    '        For i As Integer = 0 To (selected.Length - 1)
    '            'get the primary key values
    '            Dim cValues() As String = lbExcludedAccounts.Items(selected(i)).Value.Split("|")
    '            Dim corp As Integer = CInt(cValues(0))
    '            Dim cc As Integer = CInt(cValues(1))
    '            Dim expCode As Integer = CInt(cValues(2))
    '            Try
    '                alterARAccts(corp, cc, expCode, CType(Session("oUser"), cUserInfo).NetworkId, False)
    '            Catch ex As Exception
    '                'This has been logged to the system log. Errors will be emailed to support.
    '            End Try
    '        Next
    '        refreshAccounts()
    '    End If
    '    imgExcludeAcct.Enabled = True
    'End Sub


    Protected Sub btnGetfile_Click(sender As Object, e As System.EventArgs) Handles btnGetfile.Click
        Dim cc As New StringBuilder
        Dim ec As New StringBuilder
        'get selected cost centers to exclude
        Dim ccselected() As Integer = lbCostCenter.GetSelectedIndices
        Dim ccCount As Integer = 0
        For i As Integer = 0 To (ccselected.Length - 1)
            If ccCount > 0 Then
                cc.Append(", " & lbCostCenter.Items(ccselected(i)).Value())
            Else
                cc.Append(lbCostCenter.Items(ccselected(i)).Value())
            End If

        Next
        'get selected exp codes to include
        Dim ecselected() As Integer = lbExpCodes.GetSelectedIndices
        Dim ecCount As Integer = 0
        For i As Integer = 0 To (ecselected.Length - 1)
            If ecCount > 0 Then
                ec.Append("," & lbExpCodes.Items(ecselected(i)).Value())
            Else
                ec.Append(lbExpCodes.Items(ecselected(i)).Value())
            End If
            ecCount += 1
        Next
        If rbDetail.Checked Then
            Try
                Dim filePath As String = Request.PhysicalApplicationPath & "App_Data\AR" & Now.ToFileTime
                Dim fs As New StreamWriter(filePath, False)
                Dim ds As DataSet = GenerateARFileDetail(cc.ToString, ec.ToString)
                For Each r As DataRow In ds.Tables(0).Rows
                    fs.WriteLine(r("receipt"))
                Next
                fs.Flush()
                fs.Close()
                fetchFile(filePath, fileType.excel)
                'delete the file, no longer needed
                IO.File.Delete(filePath)
            Catch ex As Exception
                lblFileError.Text = ex.Message
                lblFileError.Visible = True
            End Try
        ElseIf rbSummary.Checked Then
            Try
                Dim filePath As String = Request.PhysicalApplicationPath & "App_Data\AR" & Now.ToFileTime
                Dim fs As New StreamWriter(filePath, False)
                Dim ds As DataSet = GenerateARFileSummary(cc.ToString, ec.ToString)
                For Each r As DataRow In ds.Tables(0).Rows
                    fs.WriteLine(r("receipt"))
                Next
                fs.Flush()
                fs.Close()
                fetchFile(filePath, fileType.flat)
                'delete the file, no longer needed
                IO.File.Delete(filePath)
            Catch ex As Exception
                lblFileError.Text = ex.Message
                lblFileError.Visible = True
            End Try

        End If

    End Sub
    Protected Sub fetchFile(filePath As String, outputType As fileType)
        Try
            Dim req As New WebClient
            Dim res As HttpResponse = HttpContext.Current.Response
            res.Clear()
            res.ClearContent()
            res.ClearHeaders()
            res.Buffer = True
            If outputType = fileType.flat Then
                res.AddHeader("Content-Disposition", "attachment;filename=AROut.txt")
            Else
                res.AddHeader("Content-Disposition", "attachment;filename=AR.csv")
                res.ContentType = "application/vnd.ms-excel"
            End If

            Dim data As Byte() = req.DownloadData(filePath)
            res.BinaryWrite(data)
            res.End()

        Catch ex As Exception

            Throw ex
        End Try
    End Sub


End Class
