Imports System.Data
Imports cData
Partial Class ItemDescriptionEdit
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not cData.GetUserAuthorization(User.Identity.Name, "/PMMItems.aspx") Then
                Server.Transfer("na.aspx?u=" & User.Identity.Name)
            End If
            If Request.QueryString("item") <> String.Empty Then
                Session("ItemID") = Request.QueryString("item")
                Session("ItemIDB") = Request.QueryString("itemIdb")
                LoadItem(Request.QueryString("item"), Request.QueryString("itemIdb"))
                Dim sc As sIDescSearchCriteria
                sc.itemNumber = Request.QueryString("s1")
                sc.mfrNumber = Request.QueryString("s2")
                sc.desc = Request.QueryString("s3")
                SaveSearchCriteria(sc)
            Else
                DisplayError("Error in page load, no item to load!")
            End If
        End If
    End Sub

    Private Function LoadItem(ByVal itemId As Integer, ByVal itemIdb As Integer) As Boolean
        Dim ds As DataSet = GetItemAltDescriptions(itemId, itemIdb)
        If ds.Tables(0).Rows.Count > 0 Then
            lblItemNumber.Text = ds.Tables(0).Rows(0)("ITEM_NO").ToString
            lblCatNumber.Text = ds.Tables(0).Rows(0)("CTLG_NO").ToString
            lblManufacturer.Text = ds.Tables(0).Rows(0)("MFR_NAME").ToString
            lblItemDesc.Text = ds.Tables(0).Rows(0)("DESCR").ToString
            InitControl.Value = HttpUtility.HtmlEncode(ds.Tables(0).Rows(0)("DESCR1").ToString)
            'Store the old value for logging purpose
            Session("oldValue") = ds.Tables(0).Rows(0)("DESCR1").ToString
        Else
            DisplayError("Error has occurred, please reload page!<br>Exception info (function=LoadItem,class=ItemDescriptionEdit)")
        End If

    End Function

    Private Sub DisplayError(ByVal msg As String)
        rwError.Style.Item("display") = "block"
        lblErrorMsg.Text = msg
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        CancelReturn()
    End Sub
    Private Sub CancelReturn()
        Dim sc As sIDescSearchCriteria = CType(Session("SearchCriteria"), sIDescSearchCriteria)
        Session("SearchCriteria") = Nothing
        Response.Redirect("PMMItems.aspx?s1=" & sc.itemNumber & "&s2=" & sc.mfrNumber & "&s3=" & sc.desc & "&s4=1")
    End Sub
    Private Function SaveSearchCriteria(ByVal SearchCriteria As sIDescSearchCriteria) As Boolean
        Session("searchCriteria") = SearchCriteria
    End Function

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim booSuccess As Boolean = False
        If IsNothing(Session("ItemID")) Then
            Response.Redirect("timeoutError.aspx?l=PMMItems.aspx")
        End If
        Try
            cData.UpdateItemAltDesc(CInt(Session("ItemID")), CInt(Session("ItemIDB")), InitControl.Value)
            InsertChangeLog("/ItemDescriptionEdit.aspx", User.Identity.Name, Request.ServerVariables("REMOTE_ADDR"), "ITEM", _
                            Session("ItemID"), "DESCR1", Session("oldValue"), InitControl.Value, "SUCCESS")
            'destroy the session variables
            Session("ItemID") = Nothing
            Session("ItemIDB") = Nothing
            Session("oldValue") = Nothing
            booSuccess = True
        Catch ex As Exception
            InsertChangeLog("/ItemDescriptionEdit.aspx", User.Identity.Name, Request.ServerVariables("REMOTE_ADDR"), "ITEM", _
                Session("ItemID"), "DESCR1", Session("oldValue"), InitControl.Value, "ERROR: " & ex.Message)
            DisplayError(ex.Message)
        End Try
        If booSuccess Then
            CancelReturn()
        End If

    End Sub
End Class
