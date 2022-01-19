
Imports cData
Imports System.Data
Imports Web.UI.MiscControls

Partial Class _Default
    Inherits System.Web.UI.Page

    Private Enum optionTypes
        DisableAll
        EnableAll
        SubOptionsOnly
        MainOptionsOnly

    End Enum
    Public Property dsSubOptions() As DataSet
        Get
            If Not IsNothing(ViewState("dsSubOptions")) Then
                Return ViewState("dsSubOptions")
            Else
                Dim ds As New DataSet
                ViewState("dsSubOptions") = ds
                Return ds
            End If
        End Get
        Set(ByVal value As DataSet)
            ViewState("dsSubOptions") = value
        End Set
    End Property
    Private Property currentOption As Integer
        Get
            If Not IsNothing(ViewState("currentOption")) Then
                Return ViewState("currentOption")
            Else
                Dim i As Integer = 1
                ViewState("currentOption") = i
                Return i
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("currentOption") = value
        End Set
    End Property

    Private Property currentTab As Integer
        Get
            If Not IsNothing(ViewState("currentTab")) Then
                Return ViewState("currentTab")
            Else
                Dim i As Integer = 1
                ViewState("currentTab") = i
                Return i
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("currentTab") = value
        End Set
    End Property
    Private Property currentLoc As String
        Get
            If Not IsNothing(ViewState("currentLoc")) Then
                Return ViewState("currentLoc")
            Else
                Dim l As String = String.Empty
                ViewState("currentLoc") = l
                Return l
            End If

        End Get
        Set(ByVal value As String)
            ViewState("currentLoc") = value
        End Set
    End Property
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'check user security
        If Not cData.GetUserAuthorization(User.Identity.Name, "/Zebra.aspx") Then
            LogPageHit("Zebra.aspx", User.Identity.Name, 0, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
            Server.Transfer("../na.aspx?u=" & User.Identity.Name)
        End If

        'bind the following events to their local subroutines
        AddHandler Master.ZebraLocationChange, AddressOf locationChange
        AddHandler Master.ZebraTabChange, AddressOf tabChange
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If (Not IsPostBack) Then
            initializePage()
            For i As Integer = 1 To 3
                ProcessContentcontrols(i, optionTypes.DisableAll)
            Next
        End If
    End Sub

    Protected Sub initializePage()
        initOptions()

    End Sub

    Protected Sub initOptions()
        Dim sql As String = _
        "select SE_BASIS_ID BASIS_ID, SE_SUBBASIS_ID SUBBASIS_ID, SE_SUBBASIS_DESC SUBBASIS_DESC from PS_SE_PL_SUBBAS_ID"

        Dim ds As DataSet = ExecutePSSQLSupport(sql)
        For Each r As DataRow In ds.Tables(0).Rows
            Select Case r("BASIS_ID")
                Case Is = 1
                    opt1.Items.Add(New ListItem("<td>&nbsp;</td><td>" & r("SUBBASIS_DESC") & "<br /><hr /></td>", r("SUBBASIS_ID")))
                Case Is = 2
                    opt2.Items.Add(New ListItem("<td>&nbsp;</td><td>" & r("SUBBASIS_DESC") & "<br /><hr /></td>", r("SUBBASIS_ID")))
                Case Is = 3
                    opt3.Items.Add(New ListItem("<td>&nbsp;</td><td>" & r("SUBBASIS_DESC") & "<br /><hr /></td>", r("SUBBASIS_ID")))
            End Select
        Next

    End Sub
    Protected Sub refreshPage()
        performOptionButtonChange(currentTab, currentOption, currentLoc)

    End Sub
    Private Sub populateSubCheckBox(ByVal tab As Integer, ByVal opt As Integer, ByVal locId As String)
        'load the page variables
        currentLoc = locId
        currentOption = opt
        currentTab = tab
        'is this option enabled?
        Dim sql As String = "select [SE_ENABLED] [ENABLED] " & _
            "from PS_SE_PL_CONF_SUB " & _
            "where (SE_BASIS_ID = " & tab & ") " & _
            "and SE_SUBBASIS_ID = " & opt & _
            " and SE_SERVER_ID = '" & locId & "';"
        Dim e As Boolean = Convert.ToBoolean(ExecutePSSQLScaler(sql))
        'retrieve the tab info
        Dim tb As ContentPlaceHolder = Me.Master.FindControl("cnt" & tab)
        'set the checkbox
        CType(tb.FindControl("chkTabOpt" & tab), CheckBox).Checked = e
        If e And (CType(Me.Master.FindControl("ckTab" & tab), CheckBox)).Checked Then 'display the content
            ProcessContentcontrols(tab, optionTypes.EnableAll)
        ElseIf Not (CType(Me.Master.FindControl("ckTab" & tab), CheckBox)).Checked Then
            ProcessContentcontrols(tab, optionTypes.MainOptionsOnly)
        ElseIf (CType(Me.Master.FindControl("ckTab" & tab), CheckBox)).Checked And Not e Then
            'CType(Me.Master.FindControl("ckTab" & tab), CheckBox).Enabled = True
            ProcessContentcontrols(tab, optionTypes.SubOptionsOnly)
        Else
            ProcessContentcontrols(tab, optionTypes.DisableAll)
        End If
    End Sub

    Private Sub performOptionButtonChange(ByVal tab As Integer, ByVal opt As Integer, ByVal locId As String)
        'this subroutine handles the loading of a selected option button
        populateSubCheckBox(tab, opt, locId)
        'Retrieve the tab/option information
        Dim cnt As ContentPlaceHolder = Me.Master.FindControl("cnt" & tab)
        Dim ds As DataSet = ExecutePSSQLSupport(cZebraSQL.SubOptionRetreiveSQL(tab, opt, locId))
        CType(CType(Me.Master.FindControl("cnt" & tab), ContentPlaceHolder).FindControl("opt" & tab), RadioButtonList).SelectedValue = (opt)
        'available
        Dim lbAvailable As ListBox = cnt.FindControl("opt" & tab & "Available")
        'selected
        Dim lbSelected As ListBox = cnt.FindControl("opt" & tab & "Selected")
        'initialize the list boxes
        lbAvailable.Items.Clear()
        lbSelected.Items.Clear()
        For Each r As DataRow In ds.Tables(0).Rows
            Dim strStyle As String = String.Empty
            If cUtilities.checkDBNull(r("INDICATOR")) <> String.Empty Then 'determine this items status
                If IsNumeric(r("INDICATOR")) Then
                    Select Case r("INDICATOR")
                        Case Is = "A"
                            strStyle = "color:black;"
                        Case Is = "I"
                            strStyle = "color:red;"
                    End Select
                Else
                    strStyle = IIf(r("INDICATOR") = "I", "color:red;", "color:black")
                End If
            End If
            Dim li As New ListItem
            If (cUtilities.checkDBNull(r("COMPARISON_VALUE")) = String.Empty) Then
                With li
                    .Attributes("style") = strStyle
                    .Text = r("TXT_VALUE")
                    .Value = r("VALUE")
                End With
                lbAvailable.Items.Add(li)
            Else
                With li
                    .Attributes("style") = strStyle
                    .Text = r("TXT_VALUE")
                    .Value = r("VALUE")
                End With
                lbSelected.Items.Add(li)
            End If
        Next

    End Sub

    Protected Sub locationChange(ByVal sender As Object, ByVal e As ZebraLocationChangeEventArgs)
        'set the current location
        currentLoc = e.locationName
        If e.locationName <> String.Empty Then
            'default the first option in the list...
            currentTab = 1
            currentOption = 1
            'Set selected tab to 1
            CType(Me.Master.FindControl("lblSelectedTab"), HtmlInputText).Value = 1
            performOptionButtonChange(currentTab, currentOption, currentLoc)
            populateSubCheckBox(currentTab, currentOption, currentLoc)
        Else
            For i As Integer = 1 To 3
                ProcessContentcontrols(i, optionTypes.DisableAll)
            Next
        End If

    End Sub
    Protected Sub tabChange(ByVal sender As Object, ByVal e As ZebraTabChangeEventArgs)
        If (currentLoc <> String.Empty) Then 'only load if a location is selected
            If (e.TabNumber <> 0) And (e.TabNumber <> -1) Then
                currentTab = e.TabNumber
            End If
            'default to the first option
            CType(CType(Me.Master.FindControl("cnt" & currentTab), ContentPlaceHolder).FindControl("opt" & currentTab), RadioButtonList).SelectedIndex = 0
            'default to the 1st option on the tab
            performOptionButtonChange(currentTab, 1, currentLoc)
            Dim cnt As ContentPlaceHolder = CType(Me.Master.FindControl("cnt" & currentTab), ContentPlaceHolder)
            CType(cnt.FindControl("opt" & currentTab), RadioButtonList).SelectedIndex = 0
        Else
            ProcessContentcontrols(currentTab, optionTypes.DisableAll)
        End If
    End Sub

    Protected Sub opt1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt1.SelectedIndexChanged
        performOptionButtonChange(currentTab, (opt1.SelectedValue), currentLoc)
    End Sub
    Protected Sub opt2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt2.SelectedIndexChanged
        performOptionButtonChange(currentTab, (opt2.SelectedValue), currentLoc)
    End Sub
    Protected Sub opt3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles opt3.SelectedIndexChanged
        performOptionButtonChange(currentTab, (opt3.SelectedValue), currentLoc)
    End Sub

    Private Sub ProcessContentcontrols(ByVal tab As Integer, ByVal optionType As optionTypes)

        Dim cnt As ContentPlaceHolder = CType(Me.Master.FindControl("cnt" & tab), ContentPlaceHolder)
        Select Case optionType
            Case Is = optionTypes.EnableAll
                'ENABLE
                With CType(Me.Master.FindControl("ckTab" & tab), CheckBox) 'Tab Check Box
                    .Enabled = True
                End With
                CType(cnt.FindControl("chkTabOpt" & tab), CheckBox).Enabled = True 'Sub options check box
                CType(cnt.FindControl("opt" & tab), RadioButtonList).Enabled = True 'Option list
                With CType(cnt.FindControl("btnOpt" & tab & "Add"), HtmlImage) 'Arrow Add
                    .Style("cursor") = "pointer"
                End With
                With CType(cnt.FindControl("btnOpt" & tab & "Remove"), HtmlImage)  'Arrow Delete
                    .Style("cursor") = "pointer"
                End With
                CType(cnt.FindControl("opt" & tab & "Available"), ListBox).Enabled = True 'Available Listbox
                CType(cnt.FindControl("opt" & tab & "Selected"), ListBox).Enabled = True 'Selected Listbox
            Case Is = optionTypes.DisableAll
                'DISABLE
                With CType(Me.Master.FindControl("ckTab" & tab), CheckBox) 'Tab Check Box
                    .Enabled = False
                End With
                CType(cnt.FindControl("chkTabOpt" & tab), CheckBox).Enabled = False 'Sub options check box
                CType(cnt.FindControl("opt" & tab), RadioButtonList).Enabled = False 'Option list
                With CType(cnt.FindControl("btnOpt" & tab & "Add"), HtmlImage) 'Arrow Add
                    .Style("cursor") = "none"
                End With
                With CType(cnt.FindControl("btnOpt" & tab & "Remove"), HtmlImage)  'Arrow Delete
                    .Style("cursor") = "none"
                End With
                With CType(cnt.FindControl("opt" & tab & "Available"), ListBox)
                    .Enabled = False 'Available Listbox
                    .Items.Clear()
                End With
                With CType(cnt.FindControl("opt" & tab & "Selected"), ListBox)
                    .Enabled = False 'Selected Listbox
                    .Items.Clear()
                End With

            Case Is = optionTypes.MainOptionsOnly
                'DISABLE
                With CType(cnt.FindControl("opt" & tab & "Available"), ListBox)
                    .Enabled = False 'Available Listbox
                    .Items.Clear()
                End With
                With CType(cnt.FindControl("opt" & tab & "Selected"), ListBox)
                    .Enabled = False 'Selected Listbox
                    .Items.Clear()
                End With
                With CType(cnt.FindControl("btnOpt" & tab & "Add"), HtmlImage) 'Arrow Add
                    .Style("cursor") = "none"
                End With
                With CType(cnt.FindControl("btnOpt" & tab & "Remove"), HtmlImage)  'Arrow Delete
                    .Style("cursor") = "none"
                End With
                CType(cnt.FindControl("chkTabOpt" & tab), CheckBox).Enabled = False 'Sub options check box
                CType(cnt.FindControl("opt" & tab), RadioButtonList).Enabled = False 'Option list
                'ENABLE
                With CType(Me.Master.FindControl("ckTab" & tab), CheckBox) 'Tab Check Box
                    .Enabled = True
                End With
            Case Is = optionTypes.SubOptionsOnly
                'DISABLE
                CType(cnt.FindControl("opt" & tab & "Available"), ListBox).Enabled = False 'Available Listbox
                CType(cnt.FindControl("opt" & tab & "Selected"), ListBox).Enabled = False 'Selected Listbox
                With CType(cnt.FindControl("btnOpt" & tab & "Add"), HtmlImage) 'Arrow Add
                    .Style("cursor") = "none"
                End With
                With CType(cnt.FindControl("btnOpt" & tab & "Remove"), HtmlImage)  'Arrow Delete
                    .Style("cursor") = "none"
                End With
                'ENABLE
                CType(cnt.FindControl("chkTabOpt" & tab), CheckBox).Enabled = True 'Sub options check box
                CType(cnt.FindControl("opt" & tab), RadioButtonList).Enabled = True 'Option list
                With CType(Me.Master.FindControl("ckTab" & tab), CheckBox) 'Tab Check Box
                    .Enabled = True
                End With
        End Select

    End Sub
    'The following subroutines handle enabling and disabling tab functions
    Protected Sub chkTabOpt1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkTabOpt1.CheckedChanged
        performTabOptChange(1, opt1.SelectedValue, chkTabOpt1.Checked)
    End Sub
    Protected Sub chkTabOpt2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkTabOpt2.CheckedChanged
        performTabOptChange(2, opt2.SelectedValue, chkTabOpt2.Checked)
    End Sub

    Protected Sub chkTabOpt3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkTabOpt3.CheckedChanged
        performTabOptChange(3, opt3.SelectedValue, chkTabOpt3.Checked)
    End Sub

    Private Sub performTabOptChange(ByVal tab As Integer, ByVal opt As Integer, ByVal value As Boolean)
        Dim sql As String
        sql = "update PS_SE_PL_CONF_SUB " & _
        "set [SE_ENABLED] =  " & getBit(value) & _
        " where (SE_BASIS_ID = " & tab & ") " & _
        "and SE_SUBBASIS_ID = " & opt & _
        " and SE_SERVER_ID = '" & currentLoc & "';"

        If (ExecutePSSQLSupportNonQuery(sql)) = 0 Then 'the record doesn't exist for this option, add it...
            sql = _
                "insert into PS_SE_PL_CONF_SUB (SE_SERVER_ID,SE_BASIS_ID,SE_SUBBASIS_ID,SE_ENABLED)" & _
                "values('" & currentLoc & "'," & tab & "," & opt & "," & getBit(value) & ")"
            ExecutePSSQLSupportNonQuery(sql)
        End If
        populateSubCheckBox(tab, opt, currentLoc)

    End Sub

    Private Function getBit(ByVal value As Boolean) As Int16
        If value Then
            Return 1
        Else
            Return 0
        End If
    End Function


    'the following subroutines handle all selection changes
    Protected Sub btnSvrOpt1Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSvrOpt1Add.Click
        If chkTabOpt1.Checked Then
            Try
                Dim selected() As Integer = opt1Available.GetSelectedIndices
                For i As Integer = 0 To (selected.Length - 1)
                    AlterZebraSelection(currentLoc, 1, opt1.SelectedValue, opt1Available.Items(selected(i)).Value, False)
                Next

                refreshPage()
            Catch ex As Exception
                'TO-DO: Add error handling message to screen
            End Try
        End If
    End Sub
    Protected Sub btnSvrOpt2Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSvrOpt2Add.Click
        If chkTabOpt2.Checked Then
            Try
                Dim selected() As Integer = opt2Available.GetSelectedIndices
                For i As Integer = 0 To (selected.Length - 1)
                    AlterZebraSelection(currentLoc, 2, opt2.SelectedValue, opt2Available.Items(selected(i)).Value, False)
                Next
                refreshPage()
            Catch ex As Exception
                'TO-DO: Add error handling message to screen
            End Try
        End If

    End Sub
    Protected Sub btnSvrOpt3Add_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSvrOpt3Add.Click
        If chkTabOpt3.Checked Then
            Try
                Dim selected() As Integer = opt3Available.GetSelectedIndices
                For i As Integer = 0 To (selected.Length - 1)
                    AlterZebraSelection(currentLoc, 3, opt3.SelectedValue, opt3Available.Items(selected(i)).Value, False)
                Next
                refreshPage()
            Catch ex As Exception
                'TO-DO: Add error handling message to screen
            End Try
        End If

    End Sub

    Protected Sub btnSvrOpt1Remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSvrOpt1Remove.Click
        If chkTabOpt1.Checked Then
            Try
                Dim selected() As Integer = opt1Selected.GetSelectedIndices
                For i As Integer = 0 To (selected.Length - 1)
                    AlterZebraSelection(currentLoc, 1, opt1.SelectedValue, opt1Selected.Items(selected(i)).Value, True)
                Next
                refreshPage()
            Catch ex As Exception
                'TO-DO: Add error handling message to screen
            End Try
        End If

    End Sub
    Protected Sub btnSvrOpt2Remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSvrOpt2Remove.Click
        If chkTabOpt2.Checked Then
            Try
                Dim selected() As Integer = opt2Selected.GetSelectedIndices
                For i As Integer = 0 To (selected.Length - 1)
                    AlterZebraSelection(currentLoc, 2, opt2.SelectedValue, opt2Selected.Items(selected(i)).Value, True)
                Next
                refreshPage()
            Catch ex As Exception
                'TO-DO: Add error handling message to screen
            End Try
        End If

    End Sub
    Protected Sub btnSvrOpt3Remove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSvrOpt3Remove.Click
        If chkTabOpt3.Checked Then
            Try
                Dim selected() As Integer = opt3Selected.GetSelectedIndices
                For i As Integer = 0 To (selected.Length - 1)
                    AlterZebraSelection(currentLoc, 3, opt3.SelectedValue, opt3Selected.Items(selected(i)).Value, True)
                Next
                refreshPage()
            Catch ex As Exception
                'TO-DO: Add error handling message to screen
            End Try
        End If
    End Sub

End Class
