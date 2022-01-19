Imports cData
Imports System.Data

Partial Class Site_RosebudTrayView
    Inherits System.Web.UI.Page

    Private Property dsRecipe() As DataSet
        Get
            Dim ds As New DataSet
            If Not Session("dsRecipe") Is Nothing Then
                ds = Session("dsRecipe")
            End If
            Return ds
        End Get
        Set(ByVal value As DataSet)
            Session("dsRecipe") = value
        End Set
    End Property
    Public Property dsLabel() As String
        Get
            Dim st As String = String.Empty
            If Not Session("dsLabel") Is Nothing Then
                st = Session("dsLabel")
            End If
            Return st
        End Get
        Set(ByVal value As String)
            Session("dsLabel") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            LoadTrays()
            Session("dsRecipe") = Nothing
            Session("dsLabel") = Nothing
            LogPageHit("/RosebudTrayView.aspx", User.Identity.Name, 1, Server.MachineName, Request.ServerVariables("REMOTE_ADDR"))
        End If
    End Sub

    Private Sub LoadTrays()
        With ddlTrays
            .DataSource = GetRosebudTrays()
            .DataTextField = ("TRAY")
            .DataValueField = ("TTYPE")
            .DataBind()
        End With
    End Sub


    Protected Sub btnRetrieve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetrieve.Click
        If ddlTrays.SelectedIndex <> "-1" Then
            dsRecipe = GetRosebudTrayRecipe(ddlTrays.SelectedValue.ToString)
            If dsRecipe.Tables(0).Rows.Count > 0 Then
                With dgTrayContents
                    .DataSource = dsRecipe
                    .DataBind()
                End With
                If Not (IsNothing(Session("oUser"))) Then
                    lblPackingInfo.Text = "<b>Packed by:</b> " & CType(Session("oUser"), cUserInfo).UserName
                End If
            Else
                dsRecipe.Tables(0).Rows.Clear()
                lblPackingInfo.Text = String.Empty
                With dgTrayContents
                    .DataSource = dsRecipe
                    .DataBind()
                End With
                trNoData.Style("display") = "block"
            End If
            dsLabel = ddlTrays.SelectedItem.ToString
            lblDisplayed.Text = dsLabel
        Else
            dsLabel = String.Empty
            lblMsg.Text = "Please select a tray!"
        End If

    End Sub

    Public Function GetClass(ByVal str As String) As String
        Dim rtn As String = "itemlistitem"
        If (str = "-") Then
            rtn = "rosebudcomment"
        End If

        Return rtn
    End Function


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If dsRecipe.Tables.Count > 0 Then
            If dsRecipe.Tables(0).Rows.Count > 0 Then
                trPrint.Style("display") = "block"
            Else
                trPrint.Style("display") = "none"
            End If
        Else
            trPrint.Style("display") = "none"
        End If

    End Sub
End Class
