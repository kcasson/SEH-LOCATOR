Imports Microsoft.VisualBasic
Imports System.Data

Namespace Web.UI.MiscControls

    Public Class HelpButton
        Inherits ImageButton

        Private _DisplayDocument As String = String.Empty

        Public Property DisplayDocument() As String
            Get
                Return _DisplayDocument
            End Get
            Set(ByVal value As String)
                _DisplayDocument = value
            End Set
        End Property

        Public Sub ImageButton_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If (InStr(HttpContext.Current.Request.Path, "Site")) Then
                MyBase.ImageUrl = "../Images/question.PNG"
            Else
                MyBase.ImageUrl = "Images/question.PNG"
            End If
            MyBase.CausesValidation = False
            If DisplayDocument <> String.Empty Then
                MyBase.OnClientClick = "window.open('" & DisplayDocument & "','mywindow','status=1,toolbar=1');"
            End If
            MyBase.ToolTip = "Help"

        End Sub


    End Class
    Public Class ShowWaiting
        Inherits Image
        Public Sub ShowWaiting_Load() Handles MyBase.Load
            MyBase.ImageUrl = "../Images/Loading.gif"
        End Sub
    End Class
    Public Class MenuItem
        Inherits Label

        Private _LinkText As String
        Private _NavURL As String
        Private _ListOrder As Double
        Public Property LinkText() As String
            Get
                Return _LinkText
            End Get
            Set(ByVal value As String)
                _LinkText = value
            End Set
        End Property
        Public Property NavURL() As String
            Get
                Return _NavURL
            End Get
            Set(ByVal value As String)
                value = _NavURL
            End Set
        End Property
        Public Property ListOrder() As Double
            Get
                Return _ListOrder
            End Get
            Set(ByVal value As Double)
                _ListOrder = value
            End Set
        End Property

        Public Sub MenuItem_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim sb As New StringBuilder
            sb.Append("<table><tr><td onmouseover='var dr = document.getElementById(""tbl1"");dr.style.display=""block"";' " & _
                      "onmouseup='var dr = document.getElementById(""tbl1"");dr.style.display=""none"";' " & _
                      "onmouseout='var dr = document.getElementById(""tbl1"");dr.style.display=""none"";'>Test Submenu Link</td>" & _
                      "<td style='display:none;' id='tbl1'><table height=""200px""  valign='bottom'><tr><td height='25px'>Submenu Link 1</td></tr><tr><td height='25px'>Submenu Link 2</td></tr>" & _
                      "<tr><td height='25px'>Submenu Link 3</td></tr></table></td></table>")

            MyBase.Text = sb.ToString

        End Sub
    End Class
    Public Class DataListCheckBox
        Inherits CheckBox

        Private _OrderValue As Integer

        Public Property OrderValue() As Integer
            Get
                Return _OrderValue
            End Get
            Set(ByVal value As Integer)
                _OrderValue = value
            End Set
        End Property
        Public Sub New()
            MyBase.New()

        End Sub
    End Class

    Public Class MenuHeadControl
        Inherits Label

        Private _ImageText As String

        Public Property ImageText() As String
            Get
                Return _ImageText
            End Get
            Set(ByVal value As String)
                _ImageText = value
            End Set
        End Property

        Public Sub MenuHeadControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

            If ImageText <> String.Empty Then
                'Create the page header
                MyBase.Text = "<table>" & _
                  "<tr valign=""top"" class=""menuhead""><td runat=""server"" " & _
                      "style=""width: 650px; height:60; font-family:Arial; color:White; font-weight:bold;"">" & _
                     "&nbsp;" & ImageText & "</td></tr></table>"
            End If

        End Sub

    End Class

    Public Class OperatorDropDown
        Inherits DropDownList


        Public Sub OperatorDropDown_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Add the logical values
            Me.Items.Add(New ListItem("AND", "AND"))
            Me.Items.Add(New ListItem("OR", "OR"))
            Me.Width = 50

        End Sub
    End Class

    Public Class DataListDropDown
        Inherits DropDownList

        Private _UpdateId As String
        Private _UpdateIdB As String

        'Property to hold primary keys of record to update
        Public Property UpdateId() As String
            Get
                Return _UpdateId
            End Get
            Set(ByVal value As String)
                _UpdateId = value
            End Set
        End Property

        Public Property UpdateIdB() As String
            Get
                Return _UpdateIdB
            End Get
            Set(ByVal value As String)
                _UpdateIdB = value
            End Set
        End Property
        Public Sub DataListDropDown_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        End Sub

    End Class

    Public Class UOMDropDown
        Inherits DropDownList
        Public Sub myBaseLoad(sender As Object, e As System.EventArgs) Handles MyBase.Init
            'fill the list
            Dim ds As DataSet = cData.GetUOMs
            For Each r As DataRow In ds.Tables(0).Rows
                Me.Items.Add(New ListItem(r("UOM"), r("UOM")))
            Next

        End Sub

        Public Overrides Sub RenderControl(writer As System.Web.UI.HtmlTextWriter)

            If Me.Enabled = False Then 'override the controls output so it is readable by the user

                Dim sb As New StringBuilder
                sb.Append("<div style=""")
                sb.Append("white-space:nowrap;")
                sb.Append("width:" & Me.Width.ToString & ";")
                sb.Append("height:17px;")
                sb.Append("color:black;")
                sb.Append("font-weight:bolder;")
                sb.Append("font-size:small;")
                sb.Append("overflow:hidden;")
                sb.Append("border:1px solid #000000;")
                sb.Append("valign:middle;")
                sb.Append("text-overflow:ellipsis;""")
                sb.Append(" class=""" & Me.CssClass & """>")
                sb.Append(IIf(Me.Text = String.Empty, "&nbsp;", Me.Text))
                sb.Append("</div>")
                writer.Write(sb.ToString)
            Else 'render normal textbox style
                MyBase.RenderControl(writer)
            End If


        End Sub
           
    End Class

    Public Class DisplayTextBox
        Inherits TextBox
        Private _appendTextTo As String

        Public Property appendTextTo As String
            Get
                Return _appendTextTo
            End Get
            Set(value As String)
                _appendTextTo = value
            End Set
        End Property


        Public Overrides Sub RenderControl(writer As System.Web.UI.HtmlTextWriter)

            If Me.Enabled = False Then 'override the controls output so it is readable by the user

                Dim sb As New StringBuilder
                sb.Append("<div title=""" & Me.Text & """ style=""")
                sb.Append("white-space:nowrap;")
                sb.Append("width:" & Me.Width.ToString & ";")
                If MyBase.TextMode = TextBoxMode.MultiLine Then
                    sb.Append("height:" & (MyBase.Rows * 17) & "px;")
                    sb.Append("overflow:scroll;")
                Else
                    sb.Append("height:17px;")
                    sb.Append("overflow:hidden;")
                End If
                sb.Append("color:black;")
                sb.Append("font-weight:900;")
                sb.Append("font-size:small;")
                sb.Append("border:1px solid #000000;")
                sb.Append("valign:middle;")
                sb.Append("text-overflow:ellipsis;""")
                sb.Append(" class=""" & Me.CssClass & """>")
                sb.Append(IIf(Me.Text = String.Empty, "&nbsp;", _appendTextTo & Me.Text))
                sb.Append("</div>")
                writer.Write(sb.ToString)
            Else 'render normal textbox style
                MyBase.RenderControl(writer)
            End If

        End Sub

    End Class

    Public Class LookupTextBox
        Inherits TextBox

        Private _appendTextTo As String
        Private _searchType As String
        Private _listSize As String


        Public Property appendTextTo As String
            Get
                Return _appendTextTo
            End Get
            Set(value As String)
                _appendTextTo = value
            End Set
        End Property

        Public Property searchType As String
            Get
                Return _searchType
            End Get
            Set(value As String)
                _searchType = value
            End Set
        End Property
        Public Property listSize As String
            Get
                If _listSize <> String.Empty Then
                    Return _listSize
                Else
                    Return "10"
                End If

            End Get
            Set(value As String)
                _listSize = value
            End Set
        End Property


        Public Overrides Sub RenderControl(writer As System.Web.UI.HtmlTextWriter)

            If Me.Enabled = False Then 'override the controls output so it is readable by the user

                Dim sb As New StringBuilder
                sb.Append("<div title=""" & Me.Text & """ style=""")
                sb.Append("white-space:nowrap;")
                sb.Append("width:" & Me.Width.ToString & ";")
                sb.Append("height:17px;")
                sb.Append("font-weight:bolder;")
                sb.Append("font-size:small;")
                sb.Append("overflow:hidden;")
                sb.Append("border:1px solid #000000;")
                sb.Append("valign:middle;")
                sb.Append("text-overflow:ellipsis;""")
                sb.Append(" class=""" & Me.CssClass & """>")
                sb.Append(IIf(Me.Text = String.Empty, "&nbsp;", _appendTextTo & Me.Text))
                sb.Append("</div>")
                writer.Write(sb.ToString)
            Else 'Render control with type ahead lookup
                If Page.IsPostBack Then
                    Dim ds As DataSet = cData.GetCodes(Me.Text, searchType)
                    Dim sb As New StringBuilder
                    sb.Append("<input type='text' id='" & Me.ID & "' ")
                    sb.Append("style='width:" & Me.Width.ToString & ";' ")
                    sb.Append("class='" & Me.CssClass & "' ")
                    sb.Append(" value='" & Me.Text & "'/><br/>")
                    sb.Append("<select id='ddl" & Me.ID & " size='" & listSize & ">")
                    For Each dr As DataRow In ds.Tables(0).Rows
                        sb.Append("<option value='" & dr("TYPE_CD") & "'>" & dr("TYPE_CD") & "</option>")
                    Next
                    sb.Append("</select>")
                    writer.Write(sb.ToString)
                Else
                    MyBase.RenderControl(writer)
                End If




            End If

        End Sub



    End Class


   End Namespace
