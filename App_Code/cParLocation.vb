Imports Microsoft.VisualBasic
Imports System.Collections.Generic


Public Class cParLocation
    '************************************************************************************************
    '*  NAME: cParLocation
    '*	DESC: This class represents an inventory Par Item Location
    '*	AUTHOR: kcasson 
    '*	DATE: 4/13/2020
    '*		
    '*		Modifications:
    '*			-none
    '************************************************************************************************/
    Private _BusinessUnit As String
    Private _InvCartId As String
    Private _Compartment As String
    Private _OptimalQty As Double
    Private _UOM As String
    Private _AvgCartUsage As Double

    Public Sub New()

    End Sub

    Public Sub New(ByVal pBusinessUnit As String, ByVal pInvCartID As String, ByVal pCompartment As String, ByVal pQtyOptimal As Double, ByVal pUOM As String, pAvgCartUsage As Double)
        _BusinessUnit = pBusinessUnit
        _InvCartId = pInvCartID
        _Compartment = pCompartment
        _OptimalQty = pQtyOptimal
        _UOM = pUOM
        _AvgCartUsage = pAvgCartUsage


    End Sub
    Public ReadOnly Property BusinessUnit() As String
        Get
            Return _BusinessUnit
        End Get
    End Property
    Public ReadOnly Property InvCartId() As String
        Get
            Return _InvCartId
        End Get
    End Property

    Public ReadOnly Property Compartment() As String
        Get
            Return _Compartment
        End Get
    End Property

    Public ReadOnly Property OptimalQty() As Double
        Get
            Return _OptimalQty
        End Get
    End Property

    Public ReadOnly Property UOM() As String
        Get
            Return _UOM
        End Get
    End Property
    Public ReadOnly Property AvgCartUsage As Double
        Get
            Return _AvgCartUsage
        End Get
    End Property
End Class

Public Class cParLocations
    Inherits List(Of cParLocation)
    '************************************************************************************************
    '*  NAME: cItem
    '*	DESC: This generic class is a list of cParLocations
    '*	AUTHOR: kcasson 
    '*	DATE: 4/12/2020
    '*		
    '*		Modifications:
    '*			-none
    '************************************************************************************************/
End Class

