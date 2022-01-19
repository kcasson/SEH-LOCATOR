Imports cData
Imports System.Data

Partial Class Site
    Inherits System.Web.UI.MasterPage

    Private _Locations As New List(Of cLocation)
    'these events tell the content pages when there is a change on the master
    Public Event ZebraLocationChange As ZebraLocationChangeEventHandler
    Public Event ZebraTabChange As ZebraTabChangeEventHandler

    Private Property MenuItem1() As MenuItem
        Get
            If Not IsNothing(Session("MenuItem1")) Then
                Return Session("MenuItem1")
            Else
                Dim m As New MenuItem("", 0)
                Session("MenuItem1") = m
                Return m
            End If

        End Get
        Set(ByVal value As MenuItem)
            Session("MenuItem1") = value
        End Set
    End Property
    Private Property MenuItem2() As MenuItem
        Get
            If Not IsNothing(ViewState("MenuItem2")) Then
                Return Session("MenuItem2")
            Else
                Dim m As New MenuItem("", 1)
                Session("MenuItem2") = m
                Return m
            End If

        End Get
        Set(ByVal value As MenuItem)
            Session("MenuItem2") = value
        End Set
    End Property
    Private Property MenuItem3() As MenuItem
        Get
            If Not IsNothing(ViewState("MenuItem3")) Then
                Return Session("MenuItem3")
            Else
                Dim m As New MenuItem("", 2)
                Session("MenuItem3") = m
                Return m
            End If

        End Get
        Set(ByVal value As MenuItem)
            Session("MenuItem3") = value
        End Set
    End Property
    Public Property MenuHeadings() As Generic.List(Of String)
        Get
            If Not IsNothing(ViewState("MenuHeadings")) Then
                Return CType(ViewState("MenuHeadings"), Generic.List(Of String))
            Else
                Dim l As New Generic.List(Of String)
                ViewState("MenuHeadings") = l
                Return l
            End If
        End Get
        Set(ByVal value As Generic.List(Of String))
            ViewState("MenuHeadings") = value
        End Set
    End Property
    Public Property SelectedTab() As Integer
        Get
            If Not IsNothing(ViewState("SelectedTab")) Then
                Return CInt(ViewState("SelectedTab"))
            Else
                Return 2 'Default
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("SelectedTab") = value
        End Set
    End Property

    Public Property Locations() As List(Of cLocation)
        Get
            If Not IsNothing(ViewState("Locations")) Then
                Return CType(ViewState("Locations"), List(Of cLocation))
            Else
                Dim l As New List(Of cLocation)
                ViewState("Locations") = l
                Return l
            End If

        End Get
        Set(ByVal value As List(Of cLocation))
            ViewState("Locations") = value
        End Set
    End Property
    Private Property SelectedLocation As String
        Get
            If Not IsNothing(ViewState("SelectedLocation")) Then
                Return ViewState("SelectedLocation")
            Else
                ViewState("SelectedLocation") = String.Empty
                Return String.Empty
            End If
        End Get
        Set(ByVal value As String)
            ViewState("SelectedLocation") = value
        End Set
    End Property

    Private Property PrevLocationIndex As Integer
        Get
            If Not IsNothing(ViewState("PrevLocationIndex")) Then
                Return ViewState("PrevLocationIndex")
            Else
                ViewState("PrevLocationIndex") = 0
                Return 0
            End If
        End Get
        Set(value As Integer)
            ViewState("PevLocationIndex") = value
        End Set
    End Property

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request.QueryString("act") = "LocAdd" Then
            AddLocation_Click()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If ((Not IsPostBack) And (MenuHeadings.Count = 0))  Then 'Load the menu heads from the database
            initPage()
        End If
        ''check to see if tab should be reset
        'If (SelectedLocation <> ddlWorkstation.SelectedValue) And (ddlWorkstation.SelectedIndex <> 0) Then
        '    lblSelectedTab.Value = String.Empty
        'End If
        If (lblTabSwitch.Value <> "1") Or (SelectedLocation <> ddlWorkstation.SelectedValue) And (ddlWorkstation.SelectedIndex <> 0) Then
            lblSelectedTab.Value = String.Empty
            lblTabSwitch.Value = String.Empty
        End If
        'Must set tab on each postback
        ChangeTabSelection()
        If ddlWorkstation.Items.Count = 0 Then
            UpdateLocations()
        End If

    End Sub
    Private Sub initPage()
        'initilize 
        MenuItem1 = Nothing
        MenuItem2 = Nothing
        MenuItem3 = Nothing

        'The following returns the tab names
        Dim sql As String = _
        "select SE_BASIS_ID BASIS_ID, SE_BASIS_DESCR BASIS_DESCRIPTION from PS_SE_PL_BASIS_ID order by SE_BASIS_ID"
        'Load defaults, load these from database...
        Dim ds As DataSet = ExecutePSSQLSupport(sql)
        Dim i As Integer
        Try 'loading 
            For Each tabElement As DataRow In ds.Tables(0).Rows
                If i > 2 Then
                    Throw New System.Exception("This application is currently unable to load more than three tabs.")
                Else
                    'Populate tab and label headings
                    NavigationMenu.Items.Add(New MenuItem("", i))
                    CType(Me.FindControl("ckTab" & CStr(i + 1)), CheckBox).Text = tabElement("BASIS_DESCRIPTION") & " Enabled"
                    MenuHeadings.Add(tabElement("BASIS_DESCRIPTION"))
                    i += 1
                End If
            Next
        Catch ex As Exception
            'TO DO: Add error handling routine

        End Try
        UpdateLocations()
        SelectedTab = 0
        lblSelectedTab.Value = 0


    End Sub


    Public Sub ChangeTabSelection()

        If lblSelectedTab.Value <> String.Empty Then 'Is this an actual tab selection or a postback?
            'declare the arumentents for the tab change events
            Dim args As New ZebraTabChangeEventArgs

            SelectedTab = lblSelectedTab.Value
            'reset the value so we don't reprocess 
            lblSelectedTab.Value = String.Empty
            args.TabNumber = (SelectedTab + 1)
            'raise the tab change event
            RaiseEvent ZebraTabChange(Me, args)
        End If
        MultiView1.ActiveViewIndex = Int32.Parse(SelectedTab)
        Dim i As Integer

        Try 'Update the selected tab
            For i = 0 To MenuHeadings.Count - 1
                Select Case i
                    Case Is = 0
                        NavigationMenu.Items.Add(MenuItem1)
                    Case Is = 1
                        NavigationMenu.Items.Add(MenuItem2)
                    Case Is = 2
                        NavigationMenu.Items.Add(MenuItem3)
                End Select
                If i = Int32.Parse(SelectedTab) Then 'selected tab, modify the tab to reflect selection
                    NavigationMenu.Items(i).Text = "<tr><td align='center' background='../Images/selectedtab.PNG' no-repeat width='187px'" & _
                        "height='29px'><font class='menutext'>" & MenuHeadings.Item(i).ToString & "</font></td></tr>"
                Else
                    NavigationMenu.Items(i).Text = "<tr><td align='center' background='../Images/tab.PNG' no-repeat width='187px'" & _
                        "height='29px'><font class='menutext'><a href='javascript:tabChange(" & i & ")'>" & MenuHeadings.Item(i).ToString & "</a></font></td></tr>"
                End If
            Next
        Catch ex As Exception
            'TO DO: Add error handeling routine

        End Try
    End Sub
    Public Sub UpdateLocations()
        'clear current locattions
        ddlWorkstation.Items.Clear()

        Dim sql As String = _
        "select s.SE_SERVER_ID SERVER_ID, s.SE_SERVER_NAME SERVER_NAME, case when l.DESCR is null then '' else s.LOCATION end as LOC_ID, " & _
    "	isnull(l.DESCR,'') DESCR, s.SE_IGNORE_PAT_CHRG IGNORE_PAT_CHRG, s.SE_SUPP_SEP_LABEL SUPPRESS_SEPARATOR_LABELS " & _
     "   from PS_SE_PL_SERVERS s " & _
      " left outer join PS_SHIPTO_TBL l on l.SHIPTO_ID = s.LOCATION and l.SETID = 'SHARE' " & _
     " where l.EFFDT = (select MAX(ed.EFFDT) from PS_SHIPTO_TBL ed where l.SHIPTO_ID = ed.SHIPTO_ID and l.SETID = ed.SETID and ed.EFFDT <= GETDATE()) or (l.EFFDT is null) " & _
      "  order by s.LOCATION; "


        Dim dsLocations As DataSet = ExecutePSSQLSupport(sql)
        For Each rLocation As DataRow In dsLocations.Tables(0).Rows
            Dim newLocation As New cLocation
            newLocation.ServerID = RTrim(rLocation("SERVER_ID"))
            newLocation.ServerName = RTrim(rLocation("SERVER_NAME"))
            newLocation.LocationID = RTrim(rLocation("LOC_ID"))
            newLocation.LocationName = RTrim(rLocation("LOC_ID")) & " - " & RTrim(rLocation("DESCR"))
            newLocation.IgnorePatChrg = rLocation("IGNORE_PAT_CHRG")
            newLocation.SuppressSepLabel = rLocation("SUPPRESS_SEPARATOR_LABELS")
            'Add it to the dropdown list
            ddlWorkstation.Items.Add(New ListItem(newLocation.ServerName, newLocation.ServerID))
            'Add to edit screen too
            ddlEditExistingLocation.Items.Add(New ListItem(newLocation.ServerID, newLocation.ServerID))
            'store it in the viewstate
            Locations.Add(newLocation)
        Next
        'load the PMM Inventory location drop down list control
        sql = "select st.SHIPTO_ID LOC_ID, st.DESCR NAME from PS_SHIPTO_TBL st where SETID = 'SHARE' and EFF_STATUS = 'A'" & _
            "and st.EFFDT = (select MAX(ed.EFFDT) from PS_SHIPTO_TBL ed where st.SHIPTO_ID = " & _
             "ed.SHIPTO_ID and st.SETID = ed.SETID and ed.EFFDT <= GETDATE())"
        dsLocations.Tables.Clear()
        dsLocations = ExecutePSSQLSupport(sql)
        'Insert the page load display
        ddlWorkstation.Items.Insert(0, "Select a Location")
        For Each dr As DataRow In dsLocations.Tables(0).Rows
            Dim blnFound As Boolean = False
            For Each l As cLocation In Locations
                If l.LocationID = dr("LOC_ID") Then
                    blnFound = True 'This location already has a server tied to it. Do not allow future selections.
                End If
            Next
            If Not blnFound Then
                ddlLocation.Items.Add(New ListItem(dr("LOC_ID") & " - " & dr("NAME"), dr("LOC_ID")))

            End If
        Next
        ddlEditExistingLocation.Items.Insert(0, (New ListItem("Select existing location to edit", "")))
        ddlLocation.Items.Insert(0, (New ListItem("Select Inventory Location", "")))
        dsLocations = Nothing



    End Sub

    Protected Sub ddlWorkStation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlWorkstation.SelectedIndexChanged
        SelectedLocation = ddlWorkstation.SelectedValue
        Dim args As New ZebraLocationChangeEventArgs
        lblSelectedTab.Value = String.Empty
        SelectedTab = 0
        If ddlWorkstation.SelectedIndex <> 0 Then
            args.locationName = ddlWorkstation.SelectedValue
        Else
            args.locationName = String.Empty
        End If

        'get the tab statuses
        For i As Integer = 1 To 3
            CType(Me.FindControl("ckTab" & i), CheckBox).Checked = _
                IIf(args.locationName <> String.Empty, isTabEnabled(i, args.locationName), False)
        Next
        If ddlWorkstation.SelectedIndex = 0 Then
            lblLocation.Text = String.Empty
            args.enableControls = False
        Else
            For Each l As cLocation In Locations
                If l.IsMatch(ddlWorkstation.SelectedValue) Then
                    lblLocation.Text = l.LocationName
                    Exit For
                End If
            Next
            'enable the content page controls
            args.enableControls = True
        End If

        RaiseEvent ZebraLocationChange(Me, args)

    End Sub

    Protected Function isTabEnabled(ByVal tab As Integer, ByVal location As String) As Boolean
        Dim sql As String = _
            "select b.SE_ENABLED [ENABLED] " & _
            "from PS_SE_PL_SERVERS s " & _
            "inner join PS_SE_PL_CONF_BAS b on b.SE_SERVER_ID = s.SE_SERVER_ID " & _
            "inner join PS_SHIPTO_TBL st on st.SHIPTO_ID = s.LOCATION  " & _
            "where s.SE_SERVER_ID = '" & location & "' " & _
            "and b.SE_BASIS_ID = " & tab

        Return Convert.ToBoolean(ExecutePSSQLScaler(sql))
    End Function
    Private Sub performTabCheckedChange(ByVal tab As Integer, ByVal location As String, ByVal value As Integer)
        Dim sql As String = _
            "update PS_SE_PL_CONF_BAS " & _
                " set SE_ENABLED = " & cBit(value) & _
            " where SE_SERVER_ID = '" & location & "' " & _
            " and SE_BASIS_ID = " & tab

        If ExecutePSSQLSupportNonQuery(sql) = 0 Then 'Add the record, it didn't exist
            sql = _
            "insert into PS_SE_PL_CONF_BAS (SE_ENABLED, SE_SERVER_ID, SE_BASIS_ID)" & _
             "values (" & cBit(value) & ",'" & location & "'," & tab & ")"
            ExecutePSSQLSupportNonQuery(sql)
        End If
        'lblSelectedTab.Value = tab
        'ChangeTabSelection()



    End Sub

    Protected Sub AddLocation_Click()
        divMain.Attributes("class") = "zebraBlock"
        divAddPage.Attributes("class") = "zebraShow"

        Menu1.Enabled = False
        ddlWorkstation.Enabled = False
        ckTab1.Enabled = False
        'CType(cnt1.FindControl("ckSub11"), CheckBox).Enabled = False
        'CType(cnt1.FindControl("optReceiving"), RadioButtonList).Enabled = False
        txtServerId.Focus()
    End Sub

    Protected Sub btnAddLocation_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddLocation.Click
        If (txtServerId.Text <> String.Empty) And _
            (txtServerName.Text <> String.Empty) And _
                (ddlLocation.SelectedIndex <> 0) Then
            Dim sql As String
            If (ddlEditExistingLocation.SelectedIndex <> 0) Then
                'This is an existing location that is being updated
                sql = _
                "update PS_SE_PL_SERVERS " & _
                "set SE_SERVER_NAME = '" & RTrim(txtServerName.Text) & "', " & _
                "LOCATION = " & ddlLocation.SelectedValue & ", " & _
                "SE_IGNORE_PAT_CHRG = " & cBit(ckIgnorePatCharge.Checked) & ", " & _
                "SE_SUPP_SEP_LABEL = " & cBit(ckSuppSepLabels.Checked) & " " & _
                "where SE_SERVER_ID = '" & RTrim(txtServerId.Text) & "';"

            Else
                'New location insert the record
                sql = _
            "insert into PS_SE_PL_SERVERS " & _
                "(SE_SERVER_ID,SE_SERVER_NAME,LOCATION,SE_IGNORE_PAT_CHRG,SE_SUPP_SEP_LABEL) " & _
                "values ( '" & _
                txtServerId.Text & "', '" & _
                txtServerName.Text & "', " & _
                ddlLocation.SelectedValue & ", " & _
                cBit(ckIgnorePatCharge.Checked) & ", " & _
                cBit(ckSuppSepLabels.Checked) & "); "
            End If
            'execute the transaction
            Try
                ExecutePSSQLSupport(sql)
                Response.Redirect("../Zebra/Default.aspx")
            Catch ex As Exception
                lblError.Text = "Error has occurred! Message: " & ex.Message
                lblError.Visible = True
            End Try
        End If


    End Sub

    Protected Sub ddlEditExistingLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlEditExistingLocation.SelectedIndexChanged

        'remove any locations already tied from list
        For Each l As cLocation In Locations
            If ddlLocation.Items.Contains(New ListItem(l.LocationName, l.LocationID)) Then
                Dim index As Integer = ddlLocation.Items.IndexOf(New ListItem(l.LocationName, l.LocationID))
                ddlLocation.Items.RemoveAt(index)
            End If
        Next
        If (ddlEditExistingLocation.SelectedIndex <> 0) Then
            For Each l As cLocation In Locations
                If l.IsMatch(ddlEditExistingLocation.SelectedValue) Then
                    txtServerId.Text = l.ServerID
                    txtServerId.Enabled = False
                    txtServerName.Text = l.ServerName
                    ckIgnorePatCharge.Checked = l.IgnorePatChrg
                    ckSuppSepLabels.Checked = l.SuppressSepLabel
                    ddlLocation.Items.Add(New ListItem(l.LocationName, l.LocationID))
                    ddlLocation.SelectedValue = l.LocationID
                    PrevLocationIndex = ddlLocation.SelectedIndex
                    txtServerName.Focus()
                End If
            Next
        Else
            txtServerId.Text = String.Empty
            txtServerId.Enabled = True
            txtServerName.Text = String.Empty
            ddlLocation.SelectedIndex = 0
            'ddlLocation.Enabled = True
            ckIgnorePatCharge.Checked = False
            ckSuppSepLabels.Checked = False
            txtServerId.Focus()
        End If

    End Sub

    Private Function cBit(ByVal value As Boolean) As Int16
        If value Then
            Return 1
        Else
            Return 0
        End If
    End Function


    Protected Sub ckTab1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckTab1.CheckedChanged

        performTabCheckedChange(1, ddlWorkstation.SelectedValue, ckTab1.Checked)
        Dim args As New ZebraTabChangeEventArgs
        args.TabNumber = -1
        RaiseEvent ZebraTabChange(Me, args)

    End Sub
    Protected Sub ckTab2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckTab2.CheckedChanged

        performTabCheckedChange(2, ddlWorkstation.SelectedValue, ckTab2.Checked)
        Dim args As New ZebraTabChangeEventArgs
        args.TabNumber = -1
        RaiseEvent ZebraTabChange(Me, args)
    End Sub
    Protected Sub ckTab3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ckTab3.CheckedChanged

        performTabCheckedChange(3, ddlWorkstation.SelectedValue, ckTab3.Checked)
        Dim args As New ZebraTabChangeEventArgs
        args.TabNumber = -1
        RaiseEvent ZebraTabChange(Me, args)
    End Sub




End Class


