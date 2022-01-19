<%@ Page Language="VB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    Protected Sub Page_Load() Handles Me.Load
        If Request.QueryString("v") <> String.Empty Then
            lblVersion.Text = "(" & Request.QueryString("v") & ")"
            
        End If
        
    End Sub
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IE Version Error</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h3>Your version of Internet Explorer <asp:label ID="lblVersion" runat="server"></asp:label> is not compatible with this web page. Please upgrade to version 8 or above.</h3>
    </div>
    </form>
</body>
</html>
