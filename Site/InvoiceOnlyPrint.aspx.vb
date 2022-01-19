
Partial Class Site_InvoiceOnlyPrint
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        lblOutput.Text = CStr(Session("IOPrintHTML"))
        Session("IOPrintHTML") = String.Empty


    End Sub
End Class
