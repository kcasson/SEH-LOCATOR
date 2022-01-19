Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class cVendor

    Private _VendorId As Integer
    Private _VendorIdb As Integer
    Private _VendorName As String

    Public ReadOnly Property VendorId() As Integer
        Get
            Return _VendorId
        End Get
    End Property
    Public ReadOnly Property VendorIdb() As Integer
        Get
            Return _VendorIdb
        End Get
    End Property

    Public ReadOnly Property VendorName() As String
        Get
            Return _VendorName
        End Get
    End Property

    Public Sub New(ByVal pVendorId As Integer, ByVal pVendorIdb As Integer, ByVal pVendorName As String)
        _VendorId = pVendorId
        _VendorIdb = pVendorIdb
        _VendorName = pVendorName

    End Sub

End Class
Public Class cVendors
    Inherits List(Of cVendor)


End Class

Public Class cItemVendor
    Inherits cVendor

    Private _ItemVendId As Integer
    Private _ItemVendIdb As Integer
    Private _VendorSequenceNumber As Integer
    Private _OrderingUOM As String
    Private _Packaging As New cPackaging

    Public ReadOnly Property ItemVendId() As Integer
        Get
            Return _ItemVendId
        End Get
    End Property
    Public ReadOnly Property ItemVendIdb() As Integer
        Get
            Return _ItemVendIdb
        End Get
    End Property
    Public ReadOnly Property VendorSequenceNumber() As Integer
        Get
            Return _VendorSequenceNumber
        End Get
    End Property
    Public ReadOnly Property OrderingUOM() As String
        Get
            Return _OrderingUOM
        End Get
    End Property
    Public ReadOnly Property Packaging() As cPackaging
        Get
            Return _Packaging
        End Get
    End Property
    Public Sub New(ByVal pVendorId As Integer, ByVal pVendorIdb As Integer, ByVal pVendorName As String, _
                   ByVal pSequenceNumber As String, ByVal pItemVendorId As Integer, ByVal pItemVendorIdb As Integer, _
                    ByVal pOrderingUOM As String)
        MyBase.New(pVendorId, pVendorIdb, pVendorName)
        _ItemVendId = pItemVendorId
        _ItemVendIdb = pItemVendorIdb
        _OrderingUOM = pOrderingUOM
    End Sub

End Class
Public Class cItemVendors
    Inherits List(Of cItemVendor)

End Class

Public Class cPack
    Private _DefaultUOM As Boolean
    Private _UOM As String
    Private _Contains As Double
    Private _ContainsUOM As String
    Private _Price As Double
    Private _Catalog As String
    Private _UPN As String

    Public ReadOnly Property DefaultUOM() As Boolean
        Get
            Return _DefaultUOM
        End Get
    End Property
    Public ReadOnly Property DefaultText() As String
        Get
            If _DefaultUOM Then
                Return "X"
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property UOM() As String
        Get
            Return _UOM
        End Get
    End Property
    Public ReadOnly Property Contains() As String
        Get
            Return _Contains & " " & _ContainsUOM
        End Get
    End Property
    Public ReadOnly Property ContainsUOM() As String
        Get
            Return _ContainsUOM
        End Get
    End Property

    Public ReadOnly Property Price() As Double
        Get
            Return _Price
        End Get
    End Property
    Public ReadOnly Property Catalog() As String
        Get
            Return _Catalog
        End Get
    End Property
    Public ReadOnly Property UPN() As String
        Get
            Return _UPN
        End Get
    End Property
    Public ReadOnly Property PriceFormatted() As String
        Get
            Return IIf(_Price <> 0, Format(_Price, "c"), "")
        End Get
    End Property

    Public Sub New(ByVal pDefaultUOM As Boolean, ByVal pUOM As String, _
            ByVal pContainsUOM As String, ByVal pContains As String, ByVal pPrice As Double, ByVal pCatalog As String, _
                    ByVal pUPN As String)
        _DefaultUOM = pDefaultUOM
        _UOM = pUOM
        _Contains = pContains
        _ContainsUOM = pContainsUOM
        _Price = pPrice
        _Catalog = pCatalog
        _UPN = pUPN

    End Sub
End Class
Public Class cPackaging
    Inherits List(Of cPack)

End Class
