Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Enum BinStatus
    NotInInventory = 0
    Active = 1
    PendingInactive = 2
    Inactive = 3
End Enum
Public Class cBin
    '************************************************************************************************
    '*  NAME: cBin
    '*	DESC: This class represents an inventory item bin
    '*	AUTHOR: kcasson 
    '*	DATE: 4/9/2009
    '*		
    '*		Modifications:
    '*			-none
    '************************************************************************************************/
    Private _BinLocation As String
    Private _BinLocationDesc As String
    Private _BinLocationQty As Double
    Private _BinLocationUOM As String
    Private _Status As BinStatus


    Public Sub New(ByVal pBinLocation As String, ByVal pBinLocationDesc As String, _
                            ByVal pBinLocationQty As Double, ByVal pBinLocationUOM As String, ByVal pStatus As Integer)
        _BinLocation = pBinLocation
        _BinLocationDesc = pBinLocationDesc
        _BinLocationQty = pBinLocationQty
        _BinLocationUOM = pBinLocationUOM
        _Status = pStatus
    End Sub
    Public ReadOnly Property BinLocation() As String
        Get
            Return _BinLocation
        End Get
    End Property
    Public ReadOnly Property BinLocationDesc() As String
        Get
            Return _BinLocationDesc
        End Get
    End Property

    Public ReadOnly Property BinLocationQty() As Double
        Get
            Return _BinLocationQty
        End Get
    End Property

    Public ReadOnly Property BinLocationUOM() As String
        Get
            Return _BinLocationUOM
        End Get
    End Property

    Public ReadOnly Property BinLocationAmount() As String
        Get
            Return _BinLocationQty & " " & _BinLocationUOM
        End Get
    End Property
    Public ReadOnly Property Status As BinStatus
        Get
            Return _Status
        End Get
    End Property
End Class

Public Class cBins
    Inherits List(Of cBin)
    '************************************************************************************************
    '*  NAME: cItem
    '*	DESC: This generic class is a list of cBins
    '*	AUTHOR: kcasson 
    '*	DATE: 4/9/2009
    '*		
    '*		Modifications:
    '*			-none
    '************************************************************************************************/
End Class
