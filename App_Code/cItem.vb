Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class cItem

    '************************************************************************************************
    '*  NAME: cItem
    '*	DESC: This class represents an inventory item
    '*	AUTHOR: kcasson 
    '*	DATE: 4/9/2009
    '*		
    '*		Modifications:
    '*			-none
    '************************************************************************************************/
    Private _ItemId As String
    Private _ItemNumber As String
    Private _ItemDesc As String
    Private _ItemDesc1 As String
    Private _MfrNumber As String
    Private _MfrName As String
    Private _ComdtyCode As String
    Private _CorpName As String
    Private _VendName As String
    Private _VendCatNumber As String
    Private _LawsonNumber As String
    Private _Chargeable As Boolean
    Private _ItemType As String
    Private _SupplyType As String
    Private _DateLastSentToFinance As Date
    Private _Status As Integer
    Private _Bins As New cBins
    Private _Vendors As New cItemVendors
    Private _ParLocations As New cParLocations

    'enumerations
    Public Enum SortBy
        ItemNumber
        LawsonNumber
        ManufacturerNumber
        Description
    End Enum
    Public Enum SortDirection
        ASC
        DESC
    End Enum

    Public Sub New()

    End Sub
    Public Sub New(ByVal pItemId As String, ByVal pItemNumber As String, ByVal pItemDesc As String, ByVal pMfrNumber As String, ByVal pMfrName As String, ByVal pComdtyCode As String, ByVal pCorpName As String, ByVal pVendName As String, ByVal pVendCatNumber As String, ByVal pChargeable As Boolean, ByVal pDateLastSentToFinance As Date, ByVal pStatus As Integer, ByVal pLawsonNumber As String, ByVal pItemDesc1 As String, ByVal pSupplyType As String, ByVal pItemType As String)

        _ItemId = pItemId
        _ItemNumber = pItemNumber
        _ItemDesc = pItemDesc
        _ItemDesc1 = pItemDesc1
        _MfrNumber = pMfrNumber
        _MfrName = pMfrName
        _ComdtyCode = pComdtyCode
        _CorpName = pCorpName
        _VendName = pVendName
        _VendCatNumber = pVendCatNumber
        _LawsonNumber = pLawsonNumber
        _Chargeable = pChargeable
        _DateLastSentToFinance = pDateLastSentToFinance
        _Status = pStatus
        _SupplyType = pSupplyType
        _ItemType = pItemType

    End Sub
    Public ReadOnly Property ItemId() As String
        Get
            Return _ItemId
        End Get
    End Property

    Public ReadOnly Property ItemNumber() As String
        Get
            Return _ItemNumber
        End Get

    End Property

    Public ReadOnly Property ItemDesc() As String
        Get
            Return _ItemDesc
        End Get

    End Property
    Public ReadOnly Property ItemDesc1() As String
        Get
            Return _ItemDesc1
        End Get
    End Property

    Public ReadOnly Property MfrNumber() As String
        Get
            Return _MfrNumber
        End Get

    End Property
    Public ReadOnly Property MfrName() As String
        Get
            Return _MfrName
        End Get
    End Property

    Public ReadOnly Property ComdtyCode() As String
        Get
            Return _ComdtyCode
        End Get

    End Property
    Public ReadOnly Property CorpName() As String
        Get
            Return _CorpName
        End Get

    End Property
    Public ReadOnly Property VendName() As String
        Get
            Return _VendName
        End Get
    End Property
    Public ReadOnly Property VendCatNumber() As String
        Get
            Return _VendCatNumber
        End Get
    End Property
    Public ReadOnly Property LawsonNumber() As String
        Get
            Return _LawsonNumber
        End Get
    End Property
    Public ReadOnly Property Chargeable() As Boolean
        Get
            Return _Chargeable
        End Get
    End Property
    Public ReadOnly Property DateLastSentToFinance() As Date
        Get
            Return _DateLastSentToFinance
        End Get
    End Property
    Public ReadOnly Property IsPendingFinanceInterace() As Boolean
        Get
            If (Chargeable) And (DateLastSentToFinance = "1/1/1900") Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property
    Public ReadOnly Property SupplyType() As String
        Get
            Return _SupplyType
        End Get
    End Property
    Public ReadOnly Property ItemType() As String
        Get
            Return _ItemType
        End Get
    End Property
    Public ReadOnly Property Status() As Integer
        Get
            Return _Status
        End Get
    End Property
    Public ReadOnly Property StatusText() As String
        Get
            Dim rtn As String = ""

            Select Case _Status
                Case Is = 1
                    rtn = "Active"
                Case Is = 2
                    rtn = "Pending Inactive"
                Case Is = 3
                    rtn = "Inactive"
                Case Is = 4
                    rtn = "Incomplete"
            End Select
            Return rtn
        End Get
    End Property

    Public ReadOnly Property Bins() As cBins
        Get
            Return _Bins
        End Get
    End Property

    Public ReadOnly Property Vendors() As cItemVendors
        Get
            Return _Vendors
        End Get
    End Property

    Public ReadOnly Property ParLocations() As cParLocations
        Get
            Return _ParLocations
        End Get
    End Property


End Class
'****************************************************************************************************
'cItems sorting classes
Public Class sortItemNoAscHelper
    Implements IComparer(Of cItem)

    Public Function Compare(ByVal x As cItem, ByVal y As cItem) As Integer Implements System.Collections.Generic.IComparer(Of cItem).Compare
        'Sort by item number ascending...
        If x.ItemNumber > y.ItemNumber Then
            Return 1
        ElseIf y.ItemNumber < x.ItemNumber Then
            Return -1
        Else
            Return 0
        End If
    End Function
    Public Shared Function sortItemNoAsc() As Generic.IComparer(Of cItem)
        Return New sortItemNoAscHelper
    End Function
End Class
Public Class sortItemNoDescHelper
    Implements Generic.IComparer(Of cItem)

    Public Function Compare(ByVal x As cItem, ByVal y As cItem) As Integer Implements System.Collections.Generic.IComparer(Of cItem).Compare
        'Sort by item number descending...
        If x.ItemNumber < y.ItemNumber Then
            Return 1
        ElseIf y.ItemNumber > x.ItemNumber Then
            Return -1
        Else
            Return 0
        End If
    End Function
    Public Shared Function sortItemNoDesc() As Generic.IComparer(Of cItem)
        Return New sortItemNoDescHelper
    End Function
End Class
Public Class sortLawsonNoAscHelper
    Implements Generic.IComparer(Of cItem)

    Public Function Compare(ByVal x As cItem, ByVal y As cItem) As Integer Implements System.Collections.Generic.IComparer(Of cItem).Compare
        'Sort by Lawson number ascending...
        If x.LawsonNumber > y.LawsonNumber Then
            Return 1
        ElseIf y.LawsonNumber < x.LawsonNumber Then
            Return -1
        Else
            Return 0
        End If
    End Function
    Public Shared Function sortLawsonNoAsc() As Generic.IComparer(Of cItem)
        'This function returns an instance of its containing class for comparison
        Return New sortLawsonNoAscHelper
    End Function
End Class
Public Class sortLawsonNoDescHelper
    Implements Generic.IComparer(Of cItem)

    Public Function Compare(ByVal x As cItem, ByVal y As cItem) As Integer Implements System.Collections.Generic.IComparer(Of cItem).Compare
        'Sort by Lawson number ascending...
        If x.LawsonNumber < y.LawsonNumber Then
            Return 1
        ElseIf y.LawsonNumber > x.LawsonNumber Then
            Return -1
        Else
            Return 0
        End If
    End Function
    Public Shared Function sortLawsonNoDesc() As Generic.IComparer(Of cItem)
        'This function returns an instance of its containing class for comparison operations
        Return New sortLawsonNoDescHelper
    End Function
End Class

Public Class sortMfrNoAscHelper
    Implements Generic.IComparer(Of cItem)

    Public Function Compare(ByVal x As cItem, ByVal y As cItem) As Integer Implements System.Collections.Generic.IComparer(Of cItem).Compare
        'Sort by manufacturer number
        If x.MfrNumber > y.MfrNumber Then
            Return 1
        ElseIf x.MfrNumber < y.MfrNumber Then
            Return -1
        Else
            Return 0
        End If
    End Function
    Public Shared Function sortMfrNoAsc() As System.Collections.Generic.IComparer(Of cItem)
        Return New sortMfrNoAscHelper
    End Function
End Class
Public Class sortMfrNoDescHelper
    Implements Generic.IComparer(Of cItem)

    Public Function Compare(ByVal x As cItem, ByVal y As cItem) As Integer Implements System.Collections.Generic.IComparer(Of cItem).Compare
        'Sort by manufacturer number
        If x.MfrNumber < y.MfrNumber Then
            Return 1
        ElseIf x.MfrNumber > y.MfrNumber Then
            Return -1
        Else
            Return 0
        End If
    End Function
    Public Shared Function sortMfrNoDesc() As System.Collections.Generic.IComparer(Of cItem)
        Return New sortMfrNoDescHelper
    End Function
End Class

Public Class sortDescriptionAscHelper
    Implements Generic.IComparer(Of cItem)

    Public Function Compare(ByVal x As cItem, ByVal y As cItem) As Integer Implements System.Collections.Generic.IComparer(Of cItem).Compare
        'Sort by Description
        If x.ItemDesc > y.ItemDesc Then
            Return 1
        ElseIf x.ItemDesc < y.ItemDesc Then
            Return -1
        Else
            Return 0
        End If
    End Function
    Public Shared Function sortDescrAsc() As System.Collections.Generic.IComparer(Of cItem)
        Return New sortDescriptionAscHelper
    End Function
End Class

Public Class sortDescriptionDescHelper
    Implements Generic.IComparer(Of cItem)

    Public Function Compare(ByVal x As cItem, ByVal y As cItem) As Integer Implements System.Collections.Generic.IComparer(Of cItem).Compare
        'Sort by Description
        If x.ItemDesc < y.ItemDesc Then
            Return 1
        ElseIf x.ItemDesc > y.ItemDesc Then
            Return -1
        Else
            Return 0
        End If
    End Function
    Public Shared Function sortDescrDesc() As System.Collections.Generic.IComparer(Of cItem)
        Return New sortDescriptionDescHelper
    End Function
End Class

Public Class cItems
    Inherits List(Of cItem)
    '************************************************************************************************
    '*  NAME: cItem
    '*	DESC: This generic class is a list of cItems
    '*	AUTHOR: kcasson 
    '*	DATE: 4/9/2009
    '*		
    '*		Modifications:
    '*			-none
    '************************************************************************************************/

    Public Overloads Sub Sort(ByVal column As cItem.SortBy, ByVal direction As cItem.SortDirection)
        Select Case column
            Case Is = cItem.SortBy.ItemNumber
                If direction = cItem.SortDirection.DESC Then
                    MyBase.Sort(sortItemNoDescHelper.sortItemNoDesc)
                Else
                    MyBase.Sort(sortItemNoAscHelper.sortItemNoAsc)
                End If
            Case Is = cItem.SortBy.LawsonNumber
                If direction = cItem.SortDirection.DESC Then
                    MyBase.Sort(sortLawsonNoDescHelper.sortLawsonNoDesc)
                Else
                    MyBase.Sort(sortLawsonNoAscHelper.sortLawsonNoAsc)
                End If
            Case Is = cItem.SortBy.ManufacturerNumber
                If direction = cItem.SortDirection.DESC Then
                    MyBase.Sort(sortMfrNoDescHelper.sortMfrNoDesc)
                Else
                    MyBase.Sort(sortMfrNoAscHelper.sortMfrNoAsc)
                End If
            Case Is = cItem.SortBy.Description
                If direction = cItem.SortDirection.DESC Then
                    MyBase.Sort(sortDescriptionDescHelper.sortDescrDesc)
                Else
                    MyBase.Sort(sortDescriptionAscHelper.sortDescrAsc)
                End If

        End Select

    End Sub
End Class
