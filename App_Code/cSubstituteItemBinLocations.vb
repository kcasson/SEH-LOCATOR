Option Strict Off
Option Explicit Off

Imports Microsoft.VisualBasic
Imports cData
Imports System.Data
Imports System.Collections.Generic
Imports cUtilities
Imports System.Diagnostics

Public Class cSubstituteItemBinLocations




    '************************************************************************************************
    '*  NAME: cSubstituteItemBinLocations
    '*	DESC: This class is stores sub inventory item and bin locations
    '*	AUTHOR: kcasson 
    '*	DATE: 1/24/2022
    '*		
    '*		Modifications:

    '************************************************************************************************/
    Private _Status As Boolean = False
        Private _Count As Integer = 0
        Private _Items As New cItems
        Private _CurrentSessionUser As New cUserInfo
        Private _ItemCurrentSort As cItem.SortDirection = cItem.SortDirection.ASC

        Public ReadOnly Property Status() As Boolean
            Get
                Return _Status
            End Get
        End Property
        Public ReadOnly Property Count() As Integer
            Get
                Return _Count
            End Get
        End Property
        Public ReadOnly Property Items() As cItems
            Get
                Return _Items
            End Get
        End Property
        Public Property CurrentSessionUser() As cUserInfo
            Get
                Return _CurrentSessionUser
            End Get
            Set(ByVal value As cUserInfo)
                _CurrentSessionUser = value
            End Set
        End Property
        Public Property ItemCurrentSort() As SortDirection
            Get
                Return _ItemCurrentSort
            End Get
            Set(ByVal value As SortDirection)
                _ItemCurrentSort = value
            End Set
        End Property


        Public Sub New()
        'Dim logNumber As Integer
        'Load all the items
        'Log time start
        'logNumber = LogProcTime(0, "SEMC_UP_GET_ITEM_BIN_QTY")
        Dim ds As DataSet = cData.GetSubBinQuantities("")
        'Log time finish
        'LogProcTime(logNumber, "SEMC_UP_GET_ITEM_BIN_QTY")
        If Load(ds) Then
                _Status = True
            End If
        End Sub

    Public Sub New(ByVal pmmNumber As String, ByVal user As cUserInfo)
        'Dim logNumber As Integer
        _CurrentSessionUser = user
        'Log time start
        'logNumber = LogProcTime(0, "SEMC_UP_GET_ITEM_BIN_QTY", "mfrNumer='" & mfrNumber & "' | pmmNumber='" & pmmNumber & "' | desc='" & desc & "'")
        'Load specific items based on search value
        Dim ds As DataSet = GetSubBinQuantities(pmmNumber)
        'Log time finish
        'LogProcTime(logNumber, "SEMC_UP_GET_ITEM_BIN_QTY")
        If Load(ds) Then
            _Status = True
        End If
    End Sub

    'Public Sub New(ByVal itemNumber As String, ByVal DescSearchString As String)
    '    'Load specific items based on search value
    '    Dim ds As DataSet = GetBinQuantities(itemNumber, DescSearchString)
    '    If Load(ds) Then
    '        _Status = True
    '    End If
    'End Sub

    Private Function Load(ByVal ds As DataSet) As Boolean
            Dim ItemCurrent As String = String.Empty
            Dim VendorCurrent As String = String.Empty
            Dim rtn As Boolean
            Try
                If ds.Tables("ItemBinLocations").Rows.Count > 0 Then
                    Dim itm As cItem = New cItem
                    For i As Integer = 0 To (ds.Tables("ItemBinLocations").Rows.Count - 1)

                        If ((ds.Tables("ItemBinLocations").Rows(i)("Item_No") <> ItemCurrent)) _
                                Or ItemCurrent = String.Empty Then
                            If ItemCurrent <> String.Empty Then 'This is where we add the item
                                'Retrieve the vendors
                                Dim vds As DataSet = GetItemVendors(itm.ItemId)
                                For Each r As DataRow In vds.Tables(0).Rows
                                    Dim vnd As cItemVendor
                                    vnd = New cItemVendor(r("VEND_ID"), r("VEND_IDB"), r("NAME"), r("SEQ_NO"), r("ITEM_VEND_ID"), r("ITEM_VEND_IDB"), r("ORDER_UM_CD"))
                                    'Add the packaging
                                    Dim pck As DataSet = GetItemVendorPackaging(vnd.ItemVendId, CurrentSessionUser.NetworkName)
                                    For Each pkr As DataRow In pck.Tables(0).Rows
                                        vnd.Packaging.Add(New cPack(IIf(vnd.OrderingUOM = pkr("UM_CD"), True, False), pkr("UM_CD"), pkr("TO_UM_CD"), pkr("TO_QTY"), pkr("PRICE"), pkr("CTLG_NO"), pkr("UPN")))
                                    Next
                                    itm.Vendors.Add(vnd)
                                Next
                                'Add Par Locations
                                Dim ploc As DataSet = GetParLocations(itm.ItemId)
                                For Each r As DataRow In ploc.Tables(0).Rows
                                    Dim par As cParLocation
                                    par = New cParLocation(r("BUSINESS_UNIT"), r("INV_CART_ID"), r("COMPARTMENT"), r("QTY_OPTIMAL"), r("UNIT_OF_MEASURE"), r("AVG_CART_USAGE"))

                                    itm.ParLocations.Add(par)
                                Next

                                _Items.Add(itm)
                            End If
                            'New item
                            itm = New cItem(ds.Tables("ItemBinLocations").Rows(i)("ITEM_ID"), ds.Tables("ItemBinLocations").Rows(i)("ITEM_NO"), ds.Tables("ItemBinLocations").Rows(i)("ITEM_DESC"), ds.Tables("ItemBinLocations").Rows(i)("MFR_CTLG_NO"), ds.Tables("ItemBinLocations").Rows(i)("MFR_NAME"), ds.Tables("ItemBinLocations").Rows(i)("COMDTY_CD"), ds.Tables("ItemBinLocations").Rows(i)("CORP_NAME"), ds.Tables("ItemBinLocations").Rows(i)("VEND_NAME"), ds.Tables("ItemBinLocations").Rows(i)("VEND_CTLG_NO"), ds.Tables("ItemBinLocations").Rows(i)("CHARGEABLE"), "1/1/1901", ds.Tables("ItemBinLocations").Rows(i)("ITEM_STATUS"), ds.Tables("ItemBinLocations").Rows(i)("LAWSON_NO"), ds.Tables("ItemBinLocations").Rows(i)("DESCR1"), ds.Tables("ItemBinLocations").Rows(i)("SUPPLY_TYPE"), ds.Tables("ItemBinLocations").Rows(i)("ITEM_TYPE"))

                            ItemCurrent = ds.Tables("ItemBinLocations").Rows(i)("ITEM_NO")
                            'VendorCurrent = ds.Tables("ItemBinLocations").Rows(i)("VEND_NAME")
                        End If

                        'Load the bin
                        itm.Bins.Add(New cBin(ds.Tables("ItemBinLocations").Rows(i)("BIN_LOC"), ds.Tables("ItemBinLocations").Rows(i)("LOC_NAME"), ds.Tables("ItemBinLocations").Rows(i)("LOC_QTY"), ds.Tables("ItemBinLocations").Rows(i)("LOC_UOM"), ds.Tables("ItemBinLocations").Rows(i)("LOC_STAT")))
                    Next
                    'Get the last one
                    Dim vdsLast As DataSet = GetItemVendors(itm.ItemId)
                    For Each r As DataRow In vdsLast.Tables(0).Rows
                        Dim vndLast As cItemVendor
                        vndLast = New cItemVendor(r("VEND_ID"), r("VEND_IDB"), r("NAME"), r("SEQ_NO"), r("ITEM_VEND_ID"), r("ITEM_VEND_IDB"), r("ORDER_UM_CD"))
                        'Add the packaging
                        Dim pck As DataSet = GetItemVendorPackaging(vndLast.ItemVendId, CurrentSessionUser.NetworkName)
                        For Each pkr As DataRow In pck.Tables(0).Rows
                            vndLast.Packaging.Add(New cPack(IIf(vndLast.OrderingUOM = pkr("UM_CD"), True, False), pkr("UM_CD"), pkr("TO_UM_CD"), pkr("TO_QTY"), pkr("PRICE"), pkr("CTLG_NO"), pkr("UPN")))
                        Next
                        itm.Vendors.Add(vndLast)
                    Next

                    'Add Par Locations
                    Dim plocLast As DataSet = GetParLocations(itm.ItemId)
                    For Each r As DataRow In plocLast.Tables(0).Rows
                        Dim par As cParLocation
                        par = New cParLocation(r("BUSINESS_UNIT"), r("INV_CART_ID"), r("COMPARTMENT"), r("QTY_OPTIMAL"), r("UNIT_OF_MEASURE"), r("AVG_CART_USAGE"))

                        itm.ParLocations.Add(par)
                    Next

                    _Items.Add(itm)

                End If
                _Count = Me.Items.Count
                rtn = True
            Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: Load Class: cItemBinLocations " & "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            rtn = False
            End Try
            Return rtn

        End Function




    End Class
