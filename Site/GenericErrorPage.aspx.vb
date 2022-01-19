
Partial Class Site_GenericErrorPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblSupporteMail.Text = ConfigurationManager.AppSettings("SupportEmail").ToString
            If Not IsNothing(Request.QueryString("aspxErrorPath")) Then
                cUtilities.LogEvent("Error occurred at the following location: " & Request.QueryString("aspxErrorPath"), Diagnostics.EventLogEntryType.Error, 50001)
            End If
            lblTimeStamp.Text = "Error Timestamp: " & Now.ToString
        End If
    End Sub

End Class
