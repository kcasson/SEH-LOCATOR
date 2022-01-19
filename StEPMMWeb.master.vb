Imports cData
Imports System.Data
Partial Class StEPMMWeb
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If ((Not IsPostBack) Or IsNothing(Session("oUser"))) Then
            Dim oUser As New cUserInfo(Request)
            Session("oUser") = oUser
            DataLoad()
            lblUser.Text = oUser.NetworkId
            lblIP.Text = oUser.IPAddress
        End If

    End Sub
    Sub DataLoad()
        Dim ds As DataSet = GetLeftNavLinks()
        Dim count As Integer = 0
        Dim subWidth As Integer = 0
        If ds.Tables(0).Rows.Count > 0 Then
            For Each r As DataRow In ds.Tables(0).Rows
                'Load the links
                If Len(r("LinkText")) > subWidth Then
                    subWidth = Len(r("LinkText"))
                End If
                If Not (IsDBNull(r("SubLinkOrder_2"))) Then 'Child 2
                    mnuLinks.Items((r("ListOrder") - 1)).ChildItems((r("SubLinkOrder") - 1)).ChildItems.Add(New MenuItem((" " & r("LinkText") & "&nbsp;"), r("NavUrl"), "", r("NavUrl"), r("Target")))
                    If (r("Header")) Then

                    End If

                ElseIf Not (IsDBNull(r("SubLinkOrder"))) Then 'Child
                    mnuLinks.Items((r("ListOrder") - 1)).ChildItems.Add(New MenuItem((" " & r("LinkText") & "&nbsp; "), r("NavUrl"), "", r("NavUrl"), r("Target")))
                ElseIf Not (IsDBNull(r("ListOrder"))) Then 'Parent
                    mnuLinks.Items.Add(New MenuItem(("• " & r("LinkText")), r("NavUrl"), "", r("NavUrl"), r("Target")))
                End If
            Next
        End If
        mnuLinks.DynamicMenuItemStyle.Width = 265
    End Sub


End Class

