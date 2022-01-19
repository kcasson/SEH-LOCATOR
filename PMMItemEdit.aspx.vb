Imports System.Data
Partial Class PMMItemEdit
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dsTest As New DataSet
        Dim t As New DataTable
        Dim dr As DataRow
        t.Columns.Add(New DataColumn("ItemID"))
        t.Columns.Add(New DataColumn("ItemNumber"))
        t.Columns.Add(New DataColumn("MfrNumber"))
        t.Columns.Add(New DataColumn("Descr"))
        t.Columns.Add(New DataColumn("AltDesc1"))
        t.Columns.Add(New DataColumn("AltDesc2"))


        dr = t.NewRow
        dr.Item("ItemID") = "1"
        dr.Item("ItemNumber") = "123456"
        dr.Item("MfrNumber") = "ABCDEF"
        dr.Item("Descr") = "Test item for Description change"
        dr.Item("AltDesc1") = "DESC1"
        dr.Item("AltDesc2") = "DESC2"
        t.Rows.Add(dr)
        dsTest.Tables.Add(t)



        dgItemEdit.DataSource = dsTest
        dgItemEdit.DataBind()

    End Sub

    Sub dgItemEdit_EditCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)

        dgItemEdit.EditItemIndex = e.Item.ItemIndex
        dgItemEdit.DataBind()

    End Sub

End Class
