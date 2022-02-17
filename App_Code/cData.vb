Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports cUtilities
Imports System.Diagnostics

Public Class cData
    '/************************************************************************************************
    '*
    '*  NAME: cData
    '*	DESC: This is the data class for the project
    '*	AUTHOR: kcasson 
    '*	DATE: 4/9/2009
    '*		
    '*		Modifications:
    '*			- Kenneth Casson, 10/30/2009
    '*              Added exception logging
    '*          - Kenneth Casson, 4/13/2020
    '*              Fixed problem with GetBinQuantities
    '*
    '************************************************************************************************/

    'Public Enum User
    Public Shared Function GetBinQuantities(ByVal mfrNo As String, ByVal pmmNo As String, ByVal itemDesc As String) As DataSet

        Dim sql As StringBuilder = New StringBuilder


        sql.Append("select " & vbCrLf)
        sql.Append("i.INV_ITEM_ID ITEM_ID ")
        sql.Append(",i.INV_ITEM_ID																	ITEM_NO " & vbCrLf)
        sql.Append(",i.DESCR60																		ITEM_DESC " & vbCrLf)
        sql.Append(",isnull(im.MFG_ITM_ID,'')																	MFR_CTLG_NO " & vbCrLf)
        sql.Append(",isnull(vm.DESCR60,'')																		MFR_NAME " & vbCrLf)
        sql.Append(",c.DESCR+' ('+c.CATEGORY_CD+')'													COMDTY_CD " & vbCrLf)
        sql.Append(",isnull(lt.DESCR, '')																		LOC_NAME " & vbCrLf)
        sql.Append(",isnull(piv.STORAGE_AREA+' ' + piv.STOR_LEVEL_1+'-'+piv.STOR_LEVEL_2 " & vbCrLf)
        sql.Append("+'-'+piv.STOR_LEVEL_3+' '+ piv.STOR_LEVEL_4, '')									BIN_LOC " & vbCrLf)
        sql.Append(",cast(isnull(piv.QTY, 0) as decimal(18,2))																LOC_QTY " & vbCrLf)
        sql.Append(",isnull(um.UNIT_OF_MEASURE, '')																LOC_UOM " & vbCrLf)
        sql.Append(",''																				CORP_NAME " & vbCrLf)
        sql.Append(",isnull(v.NAME1, '')																		VEND_NAME " & vbCrLf)
        sql.Append(",isnull(iv.ITM_ID_VNDR, '')																	VEND_CTLG_NO " & vbCrLf)
        sql.Append(",case when isnull(ei.SE_CHARGE_ITEM_FLG,'N' ) = 'Y' then 1 " & vbCrLf)
        sql.Append("else 0 end																	CHARGEABLE " & vbCrLf)
        sql.Append(",a.PRICE_LIST																	COST " & vbCrLf)
        sql.Append(",case  " & vbCrLf)
        sql.Append("when ei.SE_CHARGE_UOM = ' ' then i.UNIT_MEASURE_STD " & vbCrLf)
        sql.Append("else ei.SE_CHARGE_UOM end													PATIENT_UOM	 " & vbCrLf)
        sql.Append(",''																				SENT_TO_FINANCE " & vbCrLf)
        sql.Append(",isnull(i.ITM_STATUS_CURRENT, '')															ITEM_STATUS " & vbCrLf)
        sql.Append(",''																				LAWSON_NO " & vbCrLf)
        sql.Append(",isnull(i.DESCR, '')																		DESCR1 " & vbCrLf)
        sql.Append(",isnull((select XLATLONGNAME from XLATTABLE_VW where FIELDNAME = 'SE_SUPPLY_TYPE' " & vbCrLf)
        sql.Append("and FIELDVALUE = ei.SE_SUPPLY_TYPE and EFF_STATUS = 'A'  " & vbCrLf)
        sql.Append("and EFFDT = (select MAX(EFFDT) from XLATTABLE_VW where FIELDNAME ")
        sql.Append("= 'SE_SUPPLY_TYPE'	and FIELDVALUE = ei.SE_SUPPLY_TYPE  " & vbCrLf)
        sql.Append("and EFF_STATUS = 'A' and EFFDT <= GETDATE()) ), '')							SUPPLY_TYPE	 " & vbCrLf)
        sql.Append(",isnull((select XLATLONGNAME from XLATTABLE_VW where FIELDNAME = 'SE_ITEM_TYPE' " & vbCrLf)
        sql.Append("and FIELDVALUE = ei.SE_ITEM_TYPE and EFF_STATUS = 'A'  ")
        sql.Append("and EFFDT = (select MAX(EFFDT) from XLATTABLE_VW where FIELDNAME " & vbCrLf)
        sql.Append("= 'SE_ITEM_TYPE'	and FIELDVALUE = ei.SE_ITEM_TYPE " & vbCrLf)
        sql.Append("and EFF_STATUS = 'A' and EFFDT <= GETDATE()) ), '')							ITEM_TYPE					 							 " & vbCrLf)

        sql.Append(",1																				LOC_STAT " & vbCrLf)
        sql.Append("from PS_MASTER_ITEM_TBL i (nolock) " & vbCrLf)
        sql.Append("left outer join PS_SE_IN_ITEM_EPIC ei (nolock) on ei.SETID = i.SETID and ei.INV_ITEM_ID = i.INV_ITEM_ID " & vbCrLf)
        sql.Append("left outer join PS_INV_ITEM_UOM um (nolock) on um.INV_ITEM_ID = i.INV_ITEM_ID and um.SETID = i.SETID and um.DFLT_UOM_STOCK = 'Y' " & vbCrLf)
        sql.Append("left outer join PS_ITEM_MFG mi (nolock) on mi.SETID = i.SETID and mi.INV_ITEM_ID = i.INV_ITEM_ID and mi.PREFERRED_MFG = 'Y' " & vbCrLf)
        sql.Append("left outer join PS_MANUFACTURER m (nolock) on m.SETID = mi.SETID and m.MFG_ID = mi.MFG_ID " & vbCrLf)
        sql.Append("left outer join PS_ITM_VENDOR iv (nolock) on iv.SETID = i.SETID and iv.INV_ITEM_ID = i.INV_ITEM_ID and iv.ITM_VNDR_PRIORITY = 1 " & vbCrLf)
        sql.Append("left outer join PS_ITM_VENDOR_MFG im (nolock) on im.SETID = iv.SETID and im.INV_ITEM_ID = iv.INV_ITEM_ID and im.VENDOR_SETID = 'SHARE' " & vbCrLf)
        sql.Append("and im.VENDOR_ID = iv.VENDOR_ID  and im.PREFERRED_MFG = 'Y' " & vbCrLf)
        sql.Append("And im.VNDR_LOC = iv.ITMV_PRIORITY_LOC " & vbCrLf)
        sql.Append("left outer join PS_MANUFACTURER vm (nolock) on vm.SETID = 'SHARE' and vm.MFG_ID = im.MFG_ID  " & vbCrLf)
        sql.Append("left outer join PS_VNDR_VNDSET_VW v (nolock) on v.VENDOR_SETID = iv.VENDOR_SETID and v.VENDOR_ID = iv.VENDOR_ID " & vbCrLf)
        sql.Append("left outer join PS_PURCH_ITEM_ATTR a (nolock) on a.SETID = i.SETID and a.INV_ITEM_ID = i.INV_ITEM_ID " & vbCrLf)
        sql.Append("left outer join PS_ITEM_CAT_TBL_VW c (nolock) on i.SETID = c.SETID and i.CATEGORY_ID = c.CATEGORY_ID and c.EFFDT =  " & vbCrLf)
        sql.Append("(select MAX(EFFDT) from PS_ITEM_CAT_TBL_VW where SETID = c.SETID and CATEGORY_ID = c.CATEGORY_ID and EFFDT <= GETDATE()) " & vbCrLf)
        sql.Append("left outer join PS_BU_ITEMS_INV bui on bui.INV_ITEM_ID = i.INV_ITEM_ID and bui.ITM_STATUS_CURRENT = 1  " & vbCrLf)
        'sql.Append("and bui.ITM_STATUS_EFFDT = (select MAX(ITM_STATUS_EFFDT) from PS_BU_ITEMS_INV where INV_ITEM_ID = bui.INV_ITEM_ID  " & vbCrLf)
        'sql.Append("and ITM_STATUS_CURRENT = 1 and ITM_STATUS_EFFDT <= GETDATE()) " & vbCrLf)
        sql.Append("left outer join PS_DEFAULT_LOC_INV dli on dli.BUSINESS_UNIT = bui.BUSINESS_UNIT and dli.INV_ITEM_ID = bui.INV_ITEM_ID	 " & vbCrLf)
        sql.Append("Left outer join PS_PHYSICAL_INV piv on piv.BUSINESS_UNIT =bui.BUSINESS_UNIT And piv.INV_ITEM_ID = bui.INV_ITEM_ID	 " & vbCrLf)
        sql.Append("left outer join PS_BUS_UNIT_TBL_IN bti on bti.BUSINESS_UNIT = dli.BUSINESS_UNIT " & vbCrLf)
        sql.Append("left outer join PS_LOCATION_TBL lt on lt.LOCATION = bti.LOCATION and lt.SETID = 'SHARE' " & vbCrLf)
        sql.Append("And lt.EFF_STATUS = 'A' and lt.EFFDT = (select max(EFFDT) from PS_LOCATION_TBL where SETID = lt.SETID and LOCATION = lt.LOCATION and EFFDT <= cast(getdate() as date))" & vbCrLf)
        sql.Append("where " & vbCrLf)
        sql.Append(" i.INV_ITEM_ID like '" & pmmNo & "%' " & vbCrLf)
        If mfrNo <> String.Empty Then
            sql.Append(" and im.MFG_ITM_ID like '" & mfrNo & "%' " & vbCrLf)
        End If
        If itemDesc <> String.Empty Then
            sql.Append(" and upper(i.DESCR60) like '%" & UCase(itemDesc) & "%'" & vbCrLf)
        End If
        sql.Append(" order by i.INV_ITEM_ID; ")


        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionPSStringSupport"))


        Dim myDataAdapter As New SqlDataAdapter(sql.ToString, myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.Text



        myDataAdapter.SelectCommand.CommandTimeout = 200

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            results.Tables(0).TableName = "ItemBinLocations"
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetBinQuantities Class: cData Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function

    Public Shared Function GetSubBinQuantities(ByVal itemId As String) As DataSet

        Dim sql As StringBuilder = New StringBuilder


        sql.Append("Select s.SUB_ITM_ID ITEM_ID, s.SUB_ITM_ID ITEM_NO, i.DESCR60 ITEM_DESC,  isnull(mi.MFG_ITM_ID, 'UNKNOWN') MFR_CTLG_NO, isnull(m.DESCR60,'') MFR_NAME, c.DESCR+' ('+c.CATEGORY_CD+')'	COMDTY_CD " & vbCrLf)
        sql.Append(", isnull(lt.DESCR, '')																		LOC_NAME " & vbCrLf)
        sql.Append(",isnull(piv.STORAGE_AREA+' ' + piv.STOR_LEVEL_1+'-'+piv.STOR_LEVEL_2 " & vbCrLf)
        sql.Append(" +'-'+piv.STOR_LEVEL_3+' '+ piv.STOR_LEVEL_4, '')									BIN_LOC " & vbCrLf)
        sql.Append(",cast(isnull(piv.QTY, 0) as decimal(18,2))																LOC_QTY" & vbCrLf)
        sql.Append(", isnull(um.UNIT_OF_MEASURE, '')																LOC_UOM " & vbCrLf)
        sql.Append(",''																				CORP_NAME " & vbCrLf)
        sql.Append(", isnull(v.NAME1, '')																		VEND_NAME " & vbCrLf)
        sql.Append(",isnull(iv.ITM_ID_VNDR, '')																	VEND_CTLG_NO " & vbCrLf)
        sql.Append(",case when isnull(ei.SE_CHARGE_ITEM_FLG,'N' ) = 'Y' then 1 " & vbCrLf)
        sql.Append("Else 0 End																	CHARGEABLE " & vbCrLf)
        sql.Append(",a.PRICE_LIST																	COST " & vbCrLf)
        sql.Append(",case  " & vbCrLf)
        sql.Append("when ei.SE_CHARGE_UOM = ' ' then i.UNIT_MEASURE_STD " & vbCrLf)
        sql.Append("Else ei.SE_CHARGE_UOM End													PATIENT_UOM	 " & vbCrLf)
        sql.Append(",''																				SENT_TO_FINANCE " & vbCrLf)
        sql.Append(",isnull(i.ITM_STATUS_CURRENT, '')															ITEM_STATUS " & vbCrLf)
        sql.Append(",''																				LAWSON_NO " & vbCrLf)
        sql.Append(",isnull(i.DESCR, '')																		DESCR1 " & vbCrLf)
        sql.Append(",isnull((select XLATLONGNAME from XLATTABLE_VW where FIELDNAME = 'SE_SUPPLY_TYPE' " & vbCrLf)
        sql.Append("And FIELDVALUE = ei.SE_SUPPLY_TYPE And EFF_STATUS = 'A'  " & vbCrLf)
        sql.Append("And EFFDT = (select MAX(EFFDT) from XLATTABLE_VW where FIELDNAME = 'SE_SUPPLY_TYPE'	and FIELDVALUE = ei.SE_SUPPLY_TYPE  " & vbCrLf)
        sql.Append("And EFF_STATUS = 'A' and EFFDT <= GETDATE()) ), '')							SUPPLY_TYPE	 " & vbCrLf)
        sql.Append(",isnull((select XLATLONGNAME from XLATTABLE_VW where FIELDNAME = 'SE_ITEM_TYPE' " & vbCrLf)
        sql.Append("And FIELDVALUE = ei.SE_ITEM_TYPE And EFF_STATUS = 'A'  and EFFDT = (select MAX(EFFDT) from XLATTABLE_VW where FIELDNAME " & vbCrLf)
        sql.Append(" = 'SE_ITEM_TYPE'	and FIELDVALUE = ei.SE_ITEM_TYPE " & vbCrLf)
        sql.Append("And EFF_STATUS = 'A' and EFFDT <= GETDATE()) ), '')							ITEM_TYPE					 							 " & vbCrLf)
        sql.Append(",1																				LOC_STAT " & vbCrLf)
        sql.Append("From PS_SUBSTITUTE_ITM s" & vbCrLf)
        sql.Append("inner Join PS_MASTER_ITEM_TBL i on i.SETID = s.SETID And i.INV_ITEM_ID = s.SUB_ITM_ID" & vbCrLf)
        sql.Append("Left outer join PS_SE_IN_ITEM_EPIC ei (nolock) on ei.SETID = i.SETID And ei.INV_ITEM_ID = i.INV_ITEM_ID " & vbCrLf)
        sql.Append("Left outer join PS_INV_ITEM_UOM um (nolock) on um.INV_ITEM_ID = i.INV_ITEM_ID And um.SETID = i.SETID And um.DFLT_UOM_STOCK = 'Y' " & vbCrLf)
        sql.Append("Left outer join PS_ITEM_MFG mi (nolock) on mi.SETID = i.SETID And mi.INV_ITEM_ID = i.INV_ITEM_ID And mi.PREFERRED_MFG = 'Y' " & vbCrLf)
        sql.Append("Left outer join PS_MANUFACTURER m (nolock) on m.SETID = mi.SETID And m.MFG_ID = mi.MFG_ID " & vbCrLf)
        sql.Append("Left outer join PS_ITM_VENDOR iv (nolock) on iv.SETID = i.SETID And iv.INV_ITEM_ID = i.INV_ITEM_ID And iv.ITM_VNDR_PRIORITY = 1 " & vbCrLf)
        sql.Append("Left outer join PS_ITM_VENDOR_MFG im (nolock) on im.SETID = iv.SETID And im.INV_ITEM_ID = iv.INV_ITEM_ID And im.VENDOR_SETID = 'SHARE' " & vbCrLf)
        sql.Append("And im.VENDOR_ID = iv.VENDOR_ID  And im.PREFERRED_MFG = 'Y' " & vbCrLf)
        sql.Append("And im.VNDR_LOC = iv.ITMV_PRIORITY_LOC " & vbCrLf)
        sql.Append("Left outer join PS_MANUFACTURER vm (nolock) on vm.SETID = 'SHARE' and vm.MFG_ID = im.MFG_ID  " & vbCrLf)
        sql.Append("Left outer join PS_VNDR_VNDSET_VW v (nolock) on v.VENDOR_SETID = iv.VENDOR_SETID And v.VENDOR_ID = iv.VENDOR_ID" & vbCrLf)
        sql.Append("Left outer join PS_PURCH_ITEM_ATTR a (nolock) on a.SETID = i.SETID And a.INV_ITEM_ID = i.INV_ITEM_ID " & vbCrLf)
        sql.Append("Left outer join PS_ITEM_CAT_TBL_VW c (nolock) on i.SETID = c.SETID And i.CATEGORY_ID = c.CATEGORY_ID And c.EFFDT =" & vbCrLf)
        sql.Append("(select MAX(EFFDT) from PS_ITEM_CAT_TBL_VW where SETID = c.SETID And CATEGORY_ID = c.CATEGORY_ID And EFFDT <= GETDATE()) " & vbCrLf)
        sql.Append("Left outer join PS_BU_ITEMS_INV bui on bui.INV_ITEM_ID = i.INV_ITEM_ID And bui.ITM_STATUS_CURRENT = 1  " & vbCrLf)
        sql.Append("Left outer join PS_DEFAULT_LOC_INV dli on dli.BUSINESS_UNIT = bui.BUSINESS_UNIT And dli.INV_ITEM_ID = bui.INV_ITEM_ID	 " & vbCrLf)
        sql.Append("Left outer join PS_PHYSICAL_INV piv on piv.BUSINESS_UNIT =bui.BUSINESS_UNIT And piv.INV_ITEM_ID = bui.INV_ITEM_ID	 " & vbCrLf)
        sql.Append("Left outer join PS_BUS_UNIT_TBL_IN bti on bti.BUSINESS_UNIT = dli.BUSINESS_UNIT " & vbCrLf)
        sql.Append("Left outer join PS_LOCATION_TBL lt on lt.LOCATION = bti.LOCATION And lt.SETID = 'SHARE' " & vbCrLf)
        sql.Append("And lt.EFF_STATUS = 'A' and lt.EFFDT = (select max(EFFDT) from PS_LOCATION_TBL where SETID = lt.SETID and LOCATION = lt.LOCATION and EFFDT <= cast(getdate() as date))" & vbCrLf)
        sql.Append("where i.ITM_STATUS_CURRENT = 1" & vbCrLf)
        'sql.Append("and isnull(piv.STORAGE_AREA+' ' + piv.STOR_LEVEL_1+'-'+piv.STOR_LEVEL_2 +'-'+piv.STOR_LEVEL_3+' '+ piv.STOR_LEVEL_4, '') <> ''" & vbCrLf)
        sql.Append("And s.INV_ITEM_ID = '" & itemId & "'" & vbCrLf)
        sql.Append("order by s.SUB_PRIORITY_NBR" & vbCrLf)


        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionPSStringSupport"))


        Dim myDataAdapter As New SqlDataAdapter(sql.ToString, myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.Text



        myDataAdapter.SelectCommand.CommandTimeout = 200

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            results.Tables(0).TableName = "ItemBinLocations"
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetBinQuantities Class: cData Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function


    Public Shared Function GetParLocations(ByVal pmmNo As String) As DataSet

        Dim sql As StringBuilder = New StringBuilder


        sql.Append("select BUSINESS_UNIT, INV_CART_ID, COMPARTMENT, QTY_OPTIMAL, UNIT_OF_MEASURE, AVG_CART_USAGE  " & vbCrLf)
        sql.Append("from PS_CART_TEMPL_INV ")
        sql.Append("where INV_ITEM_ID = '" & pmmNo & "' " & vbCrLf)
        sql.Append("and BUSINESS_UNIT = '12000' " & vbCrLf)
        sql.Append("order by BUSINESS_UNIT, INV_CART_ID; " & vbCrLf)



        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionPSStringSupport"))


        Dim myDataAdapter As New SqlDataAdapter(sql.ToString, myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.Text



        myDataAdapter.SelectCommand.CommandTimeout = 200

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            results.Tables(0).TableName = "ItemParLocations"
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetParLocations Class: cData Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function



    Public Shared Function GetItemAltDescriptions(ByVal pmmNo As String, ByVal mfrNo As String, ByVal itemDesc As String, ByRef intCountResults As Integer) As DataView

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_ITEM_ALT_DESC", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        Dim pMfrItem As New SqlParameter("@MfrNo", SqlDbType.VarChar, 20)
        Dim pPmmItem As New SqlParameter("@ItemNo", SqlDbType.VarChar, 15)
        Dim pItemDesc As New SqlParameter("@ItemDesc", SqlDbType.VarChar, 50)

        pMfrItem.Value = UCase(mfrNo)
        pPmmItem.Value = UCase(pmmNo)
        pItemDesc.Value = UCase(itemDesc)

        'add the parameter values
        myDataAdapter.SelectCommand.Parameters.Add(pMfrItem)
        myDataAdapter.SelectCommand.Parameters.Add(pPmmItem)
        myDataAdapter.SelectCommand.Parameters.Add(pItemDesc)
        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            intCountResults = myDataAdapter.Fill(results)
            results.Tables(0).TableName = "ItemAltDescriptions"
            Return results.Tables(0).DefaultView
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetItemAltDescriptions Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function
    Public Shared Function GetItemAltDescriptions(ByVal itemId As Integer, ByVal itemIdb As Integer) As DataSet
        'Overloaded Function GetItemAltDescriptions
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_ITEM_ALT_DESC", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        Dim pItemId As New SqlParameter("@ItemId", SqlDbType.Int)
        Dim pItemIdb As New SqlParameter("@ItemIdb", SqlDbType.Int)

        pItemId.Value = itemId
        pItemIdb.Value = itemIdb


        'add the parameter values
        myDataAdapter.SelectCommand.Parameters.Add(pItemId)
        myDataAdapter.SelectCommand.Parameters.Add(pItemIdb)
        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            Return results
        Catch ex As Exception
            'START HERE
            LogEvent("Exception Thrown by the following; Function: GetItemAltDescriptions Overload Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function

    Public Shared Function GetUserAuthorization(ByVal userId As String, ByVal applicationName As String) As Boolean

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_USER_AUTHORIZATION", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        Dim puserId As New SqlParameter("@UserId", SqlDbType.VarChar, 25)
        Dim papplicationName As New SqlParameter("@Application", SqlDbType.VarChar, 50)

        puserId.Value = userId
        papplicationName.Value = applicationName


        'add the parameter values
        myDataAdapter.SelectCommand.Parameters.Add(puserId)
        myDataAdapter.SelectCommand.Parameters.Add(papplicationName)
        myDataAdapter.SelectCommand.CommandTimeout = 100

        Try
            myConnection.Open()
            Return myDataAdapter.SelectCommand.ExecuteScalar
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetUserAuthorization Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function

    Public Shared Function IsAdmin(ByVal userId As String) As Boolean

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_IS_ADMIN", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        Dim puserId As New SqlParameter("@UserId", SqlDbType.VarChar, 25)
        'assign values
        puserId.Value = userId

        'add the parameter values
        myDataAdapter.SelectCommand.Parameters.Add(puserId)
        myDataAdapter.SelectCommand.CommandTimeout = 100

        Try
            myConnection.Open()
            Return myDataAdapter.SelectCommand.ExecuteScalar
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: IsAdmin Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function

    Public Shared Function LogProcTime(ByVal RecordId As Integer, ByVal ProcName As String, ByVal Parameters As String) As Integer

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_LOG_PROC_EXEC", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        Dim pRecordId As New SqlParameter("@ProcTimeId", SqlDbType.Int)
        Dim pProcName As New SqlParameter("@ProcName", SqlDbType.NVarChar, 100)
        Dim pParams As New SqlParameter("@Params", SqlDbType.NVarChar, 255)

        'assign values
        pRecordId.Value = RecordId
        pProcName.Value = ProcName
        pParams.Value = Parameters

        'add the parameter values
        myDataAdapter.SelectCommand.Parameters.Add(pRecordId)
        myDataAdapter.SelectCommand.Parameters.Add(pProcName)
        myDataAdapter.SelectCommand.Parameters.Add(pParams)

        myDataAdapter.SelectCommand.CommandTimeout = 100
        Dim ds As New DataSet
        Try
            myConnection.Open()
            Return myDataAdapter.SelectCommand.ExecuteScalar()

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: LogProcTime Class: cData " & _
                    "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function
    Public Shared Function LogProcTime(ByVal RecordId As Integer, ByVal ProcName As String) As Integer

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_LOG_PROC_EXEC", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        Dim pRecordId As New SqlParameter("@ProcTimeId", SqlDbType.Int)
        Dim pProcName As New SqlParameter("@ProcName", SqlDbType.NVarChar, 100)

        'assign values
        pRecordId.Value = RecordId
        pProcName.Value = ProcName

        'add the parameter values
        myDataAdapter.SelectCommand.Parameters.Add(pRecordId)
        myDataAdapter.SelectCommand.Parameters.Add(pProcName)


        myDataAdapter.SelectCommand.CommandTimeout = 100
        Dim ds As New DataSet
        Try
            myConnection.Open()
            Return myDataAdapter.SelectCommand.ExecuteScalar()

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: LogProcTime Class: cData " & _
                    "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function

    Public Shared Function GetItems() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
        Dim strSQL As String = "SELECT ITEM_ID, ITEM_IDB, (RTRIM(CTLG_NO)" & _
            " + ' / ' + RTRIM(DESCR)) AS ItemInfo FROM ITEM WHERE STAT <> 3 AND IMPORT_STATUS = 0 AND CTLG_ITEM_IND " & _
            "= 'Y' AND CHARINDEX('~',ITEM.ITEM_NO) < 1 ORDER BY CTLG_NO"
        Dim myDataAdapter As New SqlDataAdapter(strSQL, myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.Text

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetItems Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function

    Public Shared Function GetUsers() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
        Dim strSQL As String = "SELECT USR_ID, USR_IDB, RTRIM(LOGIN_ID) AS LOGIN_ID,  " & _
                "RTRIM(NAME) AS NAME, RTRIM(LOGIN_ID) + ' - ' + " & _
                "LTRIM(SUBSTRING([NAME],(CHARINDEX(',',[NAME])+1),(LEN(RTRIM([NAME]))-CHARINDEX(',',[NAME]))))+' '+ " & _
                "SUBSTRING([NAME],0,(CHARINDEX(',',[NAME]))) AS [USER_NAME], RTRIM(PSWD) AS PSWD " & _
                "FROM USR " & _
                "WHERE INACT_IND <> 'Y'  " & _
                "AND [NAME] NOT LIKE 'PMM%' " & _
                "ORDER BY [USER_NAME] "
        Dim myDataAdapter As New SqlDataAdapter(strSQL, myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.Text

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetUsers Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function

    Public Shared Function GetUsers(ByVal Inactive As Boolean) As DataSet
        'Overloaded function GetUsers
        Dim strSQL As String
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
        If Not Inactive Then
            strSQL = "SELECT USR_ID, USR_IDB, RTRIM(LOGIN_ID) AS LOGIN_ID, " & _
                "RTRIM(NAME) AS NAME, RTRIM(LOGIN_ID) + ' - ' + " & _
                "LTRIM(SUBSTRING([NAME],(CHARINDEX(',',[NAME])+1),(LEN(RTRIM([NAME]))-CHARINDEX(',',[NAME]))))+' '+ " & _
                "SUBSTRING([NAME],0,(CHARINDEX(',',[NAME]))) AS [USER_NAME] " & _
                "FROM USR " & _
                "WHERE INACT_IND <> 'Y' " & _
                "AND [NAME] NOT LIKE 'PMM%' " & _
                "AND USR_ID <> 1453 " & _
                "ORDER BY [USER_NAME] "
        Else
            strSQL = "SELECT USR_ID, USR_IDB, RTRIM(LOGIN_ID) AS LOGIN_ID, " & _
                "RTRIM(NAME) AS NAME, RTRIM(LOGIN_ID) + ' - ' + " & _
                "LTRIM(SUBSTRING([NAME],(CHARINDEX(',',[NAME])+1),(LEN(RTRIM([NAME]))-CHARINDEX(',',[NAME]))))+' '+ " & _
                "SUBSTRING([NAME],0,(CHARINDEX(',',[NAME]))) AS [USER_NAME] " & _
                "FROM USR " & _
                "WHERE INACT_IND <> 'N'  " & _
                "ORDER BY [USER_NAME] "
        End If

        Dim myDataAdapter As New SqlDataAdapter(strSQL, myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.Text

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetUsers Class: cData " & _
                    "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function

    Public Shared Function GetRosebudTrays() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringRosebud"))
        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_TRAYS", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetRosebudTrays Class: cData " & _
                    "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function
    Public Shared Function GetRosebudTrayRecipe(ByVal TType As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringRosebud"))
        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_TRAY_RECIPE", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        Dim pTtype As New SqlParameter("@TTYPE", SqlDbType.VarChar, 8)
        'assign values
        pTtype.Value = TType

        'add the parameter values
        myDataAdapter.SelectCommand.Parameters.Add(pTtype)
        myDataAdapter.SelectCommand.CommandTimeout = 100
        Dim ds As New DataSet
        Try
            myDataAdapter.Fill(ds)
            Return ds
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetRosebudTrayRecipe Class: cData " & _
                    "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function

    Public Shared Function GetHSMInterfaceItems() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
        Dim sql As String = _
        "SELECT  dbo.ITEM.ITEM_NO, " & _
        "dbo.ITEM.CTLG_NO, " & _
        "dbo.ITEM.DESCR, " & _
          "CASE WHEN ITEM_EXPORT_QUE.ITEM_ID IS NOT NULL " & _
            "THEN 'X' END AS PENDING_PMM_ACTION, " & _
          "CASE WHEN ITEM_INTERFACE.ITEM_ID IS NOT NULL " & _
            "THEN 'X' END AS PENDING_HSM_ACTION " & _
            "FROM(dbo.ITEM) " & _
        "LEFT OUTER JOIN dbo.ITEM_EXPORT_QUE ON dbo.ITEM.ITEM_ID = dbo.ITEM_EXPORT_QUE.ITEM_ID " & _
         "AND dbo.ITEM.ITEM_IDB = dbo.ITEM_EXPORT_QUE.ITEM_IDB " & _
        "LEFT OUTER JOIN dbo.ITEM_INTERFACE ON dbo.ITEM.ITEM_ID = dbo.ITEM_INTERFACE.ITEM_ID " & _
         "AND dbo.ITEM.ITEM_IDB = dbo.ITEM_INTERFACE.ITEM_IDB " & _
        "WHERE     (dbo.ITEM_INTERFACE.ITEM_ID IS NOT NULL) OR " & _
                              "(dbo.ITEM_EXPORT_QUE.ITEM_ID IS NOT NULL) "

        Dim myDataAdapter As New SqlDataAdapter(sql, myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.Text

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetHSMInterfaceItems Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try



    End Function

    Public Shared Function GetLeftNavLinks() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_WEB_LINKS", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetLeftNavLinks Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function
    Public Shared Function GetEpicSupplyTypes() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim da As New SqlDataAdapter("SEMC_UP_GET_EPIC_SUPPLY_TYPES", myConnection)
        da.SelectCommand.CommandType = CommandType.StoredProcedure

        da.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            da.Fill(results)
            results.Tables(0).TableName = "tblSupplyTypes"
            results.Tables(1).TableName = "tblItemTypes"

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetEpicSupplyTypes() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            da.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    Public Shared Function GetEpicInterfaceItems(ByRef CountResults As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_EPIC_IF_ITEMS", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            CountResults = myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetEpicInterfaceItems() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    Public Shared Function GetEpicInterfaceItems(ByRef CountResults As Integer, ByVal ItemNumber As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_EPIC_IF_ITEMS", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        'Add the parameter
        Dim pItemNumber As New SqlParameter("@ItemNumber", SqlDbType.VarChar, 15)
        pItemNumber.Value = ItemNumber
        myDataAdapter.SelectCommand.Parameters.Add(pItemNumber)

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            CountResults = myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetEpicInterfaceItems() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    Public Shared Function GetPOLineNumbers(ByVal PONumber As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_PO_LINES", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        'Add the parameter
        Dim pPONumber As New SqlParameter("@PONumber", SqlDbType.VarChar, 22)
        pPONumber.Value = PONumber
        myDataAdapter.SelectCommand.Parameters.Add(pPONumber)

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetPOLineNumbers() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    Public Shared Function GetPOsByItemNumber(ByVal ItemNo As String,User As string) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_ITEM_POs", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        'Add the parameter
        Dim pItemNumber As New SqlParameter("@ItemNumber", SqlDbType.VarChar, 15)
        pItemNumber.Value = ItemNo
        myDataAdapter.SelectCommand.Parameters.Add(pItemNumber)
        Dim pUser As New SqlParameter("@User", SqlDbType.VarChar, 50)
        pUser.Value = User
        myDataAdapter.SelectCommand.Parameters.Add(pUser)

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetPOsByItemNumber() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    Public Shared Function GetInvoicesByItemNumber(ByVal ItemNo As String, ByVal User As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_ITEM_INVOICES", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        'Add the parameter
        Dim pItemNumber As New SqlParameter("@ItemNumber", SqlDbType.VarChar, 15)
        pItemNumber.Value = ItemNo
        myDataAdapter.SelectCommand.Parameters.Add(pItemNumber)
        Dim pUser As New SqlParameter("@User", SqlDbType.VarChar, 50)
        pUser.Value = User
        myDataAdapter.SelectCommand.Parameters.Add(pUser)

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetInvoicesByItemNumber() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    '****************************************************************************************************
    '* PAREX Procedures           
    '* 10/24/2011
    '****************************************************************************************************
    Public Shared Function GetParExOpenOrders(Optional ByVal Corp As Integer = 0) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringParEx"))
        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_OPNORD", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterCorp As New SqlParameter("@CorpLoc", Data.DbType.Int16)
        parameterCorp.Value = Corp
        myDataAdapter.SelectCommand.Parameters.Add(parameterCorp)

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetParExOpenOrders Class: cData " & _
                    "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function

    Public Shared Function GetParExReqDetail(ByVal Req As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_REQ_DTL", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim parameterReq As New SqlParameter("@Req", Data.DbType.String)
        parameterReq.Value = Req
        myDataAdapter.SelectCommand.Parameters.Add(parameterReq)

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)
            Return results
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetParExReqDetail Class: cData " & _
                    "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

    End Function



    '**************************************************************************************************

    Public Shared Function ReversePOExclusion(ByVal PONumber As String, ByVal POLines As String) As Boolean

        Dim sc As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim cmd As New SqlCommand("SEMC_UP_REVERSE_EXCLUSION", sc)
        Dim rtn As Boolean = False

        Try
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@po_no", SqlDbType.VarChar, 22)).Value = PONumber
            cmd.Parameters.Add(New SqlParameter("@po_line", SqlDbType.VarChar, 8000)).Value = POLines

            sc.Open()
            'Execute the insert statement.
            cmd.ExecuteNonQuery()
            rtn = True
        Catch e As SqlException
            'LogEvent("Exception Thrown by the following; Function: InsertUser Class: cData " & _
            '    "Message: " & e.Message, EventLogEntryType.Error, 50001)
            Throw e
        Finally
            cmd.Dispose()
            sc.Close()
            sc.Dispose()
        End Try

        Return rtn
    End Function
    Public Shared Function GetWebUsers() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_WEB_USERS", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetEpicInterfaceItems() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    Public Shared Function GetWebUserAuthorization(ByVal UserId As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_WEBUSER_AUTHORIZATIONS", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        'Add the parameter
        Dim pUserId As New SqlParameter("@USER_ID", SqlDbType.VarChar, 35)
        pUserId.Value = UserId


        myDataAdapter.SelectCommand.Parameters.Add(pUserId)
        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetEpicInterfaceItems() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function
    Public Shared Function GetWebApps() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_WEB_APPLICATIONS", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetEpicInterfaceItems() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function
    Public Shared Function GetItemVendors(ByVal ItemId As String) As DataSet
        'This function only currently returns item vendors for the Edgewood corp and dbid 3025. 
        'Add additional paramters for multi corp information

        Dim sql As New StringBuilder

        sql.Append("select " & vbCrLf)
        sql.Append("i.VENDOR_ID ITEM_VEND_ID " & vbCrLf)
        sql.Append(",i.VENDOR_ID ITEM_VEND_IDB  " & vbCrLf)
        sql.Append(",i.ITM_VNDR_PRIORITY SEQ_NO " & vbCrLf)
        sql.Append(",u.UNIT_MEASURE_VOL ORDER_UM_CD " & vbCrLf)
        sql.Append(",i.VENDOR_ID VEND_ID " & vbCrLf)
        sql.Append(",1 VEND_IDB " & vbCrLf)
        sql.Append(",v.NAME1 NAME" & vbCrLf)
        sql.Append("from PS_ITM_VENDOR i " & vbCrLf)
        sql.Append("inner join PS_VENDOR v on v.VENDOR_ID = i.VENDOR_ID and v.SETID = i.SETID " & vbCrLf)
        sql.Append("inner join PS_INV_ITEM_UOM u on u.SETID = 'SHARE' and u.INV_ITEM_ID = i.INV_ITEM_ID and u.DFLT_UOM_REQ = 'Y' " & vbCrLf)
        sql.Append("where i.INV_ITEM_ID = '" & ItemId & "' " & vbCrLf)
        sql.Append("order by i.ITM_VNDR_PRIORITY ")

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionPSStringSupport"))
        Dim myDataAdapter As New SqlDataAdapter(sql.ToString, myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.Text
        myDataAdapter.SelectCommand.CommandTimeout = 200



        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetItemVendors() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function
    Public Shared Function GetItemVendorPackaging(ByVal ItemVendorId As Integer, ByVal UserId As String) As DataSet


        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_ITEM_VENDOR_PKG", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@ItemVendId", SqlDbType.Int)).Value = ItemVendorId
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@ItemVendIdb", SqlDbType.Int)).Value = 3025
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@User", SqlDbType.VarChar)).Value = UserId

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetItemVendorPackaging() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function
    Public Shared Function GetCodes(ByVal searchStr As String, searchType As String) As DataSet


        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_CODES", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@SearchString", SqlDbType.VarChar, 255)).Value = searchStr
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@SearchType", SqlDbType.VarChar, 50)).Value = searchType

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetCodes() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function


    Public Shared Function AlterWebUserAuthorization(ByVal UserId As String, ByVal ApplicationId As String, ByVal Delete As Boolean) As Boolean

        Dim sc As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim cmd As New SqlCommand("SEMC_UP_ALTER_WEBUSER_AUTHORIZATION", sc)
        Dim rtn As Boolean = False

        Try
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@USER_ID", SqlDbType.VarChar, 35)).Value = UserId
            cmd.Parameters.Add(New SqlParameter("@ApplicationId", SqlDbType.VarChar, 100)).Value = ApplicationId
            cmd.Parameters.Add(New SqlParameter("@Delete", SqlDbType.VarChar, 150)).Value = Delete

            sc.Open()
            'Execute the insert statement.
            cmd.ExecuteNonQuery()
            rtn = True
        Catch e As SqlException
            'LogEvent("Exception Thrown by the following; Function: InsertUser Class: cData " & _
            '    "Message: " & e.Message, EventLogEntryType.Error, 50001)
            Throw e
        Finally
            cmd.Dispose()
            sc.Close()
            sc.Dispose()
        End Try

        Return rtn
    End Function

    Public Shared Function DeleteWebUser(ByVal UserId As String) As Boolean

        Dim sc As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim cmd As New SqlCommand("SEMC_UP_DELETE_WEBUSER", sc)
        Dim rtn As Boolean = False

        Try
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@USER_ID", SqlDbType.VarChar, 35)).Value = UserId

            sc.Open()
            'Execute the statement.
            cmd.ExecuteNonQuery()
            rtn = True
        Catch e As SqlException
            'LogEvent("Exception Thrown by the following; Function: InsertUser Class: cData " & _
            '    "Message: " & e.Message, EventLogEntryType.Error, 50001)
            Throw e
        Finally
            cmd.Dispose()
            sc.Close()
            sc.Dispose()
        End Try

        Return rtn
    End Function

    Public Shared Function GetStLukeItemMatchDataSets() As DataSet
        Dim cn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim sql As String = "SELECT DISTINCT DATASET FROM STL_ITEM ORDER BY DATASET; SELECT DISTINCT SUBSET FROM STL_SUBSET ORDER BY SUBSET;"
        Dim da As New SqlDataAdapter(sql, cn)

        Dim ds As New DataSet
        da.SelectCommand.CommandType = CommandType.Text
        Try
            da.Fill(ds)
            ds.Tables(0).TableName = "DataSets"
            ds.Tables(1).TableName = "SubSets"
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetStLukeItemMatchDataSets() Class: cData " & _
            "Message: " & ex.Message, EventLogEntryType.Error, 50001)
        Finally
            cn.Close()
            da.Dispose()
            cn.Dispose()
        End Try

        Return ds
    End Function
    Public Shared Function ExecuteSQLSupport(ByVal sql As String) As DataSet
        Dim cn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim da As New SqlDataAdapter(sql, cn)

        Dim ds As New DataSet
        da.SelectCommand.CommandType = CommandType.Text
        da.SelectCommand.CommandTimeout = 60
        Try
            da.Fill(ds)
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: ExecuteSQLSupport() Class: cData Statement:" & _
                sql & " Message: " & ex.Message, EventLogEntryType.Error, 50001)
        Finally
            cn.Close()
            da.Dispose()
            cn.Dispose()
        End Try

        Return ds
    End Function
    Public Shared Function ExecutePSSQLSupport(ByVal sql As String) As DataSet
        Dim cn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionPSStringSupport"))
        Dim da As New SqlDataAdapter(sql, cn)

        Dim ds As New DataSet
        da.SelectCommand.CommandType = CommandType.Text
        da.SelectCommand.CommandTimeout = 60
        Try
            da.Fill(ds)
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: ExecuteSQLSupport() Class: cData Statement:" & _
                sql & " Message: " & ex.Message, EventLogEntryType.Error, 50001)
        Finally
            cn.Close()
            da.Dispose()
            cn.Dispose()
        End Try

        Return ds
    End Function
    Public Shared Function ExecuteSQLSupport(ByVal sql As String, ByRef CountResults As Integer) As DataSet
        Dim cn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim da As New SqlDataAdapter(sql, cn)

        Dim ds As New DataSet
        'verify sql 
        If Not ((InStr(sql, "delete") > 0) And (InStr(sql, "update") > 0) And (InStr(sql, "drop") > 0)) Then

            da.SelectCommand.CommandType = CommandType.Text
            da.SelectCommand.CommandTimeout = 60
            Try
                CountResults = da.Fill(ds)
            Catch ex As Exception
                LogEvent("Exception Thrown by the following; Function: ExecuteSQLSupport() Class: cData Statement:" & _
                    sql & " Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Finally
                cn.Close()
                da.Dispose()
                cn.Dispose()
            End Try
        End If


        Return ds
    End Function
    Public Shared Function ExecuteSQL(ByVal sql As String) As DataSet
        Dim cn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
        Dim da As New SqlDataAdapter(sql, cn)

        Dim ds As New DataSet
        da.SelectCommand.CommandType = CommandType.Text
        da.SelectCommand.CommandTimeout = 60
        Try
            da.Fill(ds)
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: ExecuteSQLSupport() Class: cData Statement:" & _
                sql & " Message: " & ex.Message, EventLogEntryType.Error, 50001)
        Finally
            cn.Close()
            da.Dispose()
            cn.Dispose()
        End Try

        Return ds
    End Function
    Public Shared Function ExecuteSQLSupportNonQuery(ByVal sql As String) As Integer
        Dim cn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim da As New SqlDataAdapter(sql, cn)

        'Dim ds As New DataSet
        Dim i As Integer
        da.SelectCommand.CommandType = CommandType.Text
        da.SelectCommand.CommandTimeout = 60
        cn.Open()
        Try
            i = da.SelectCommand.ExecuteNonQuery()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: ExecuteSQLSupport() Class: cData Statement:" & _
                sql & " Message: " & ex.Message, EventLogEntryType.Error, 50001)
        Finally
            cn.Close()
            da.Dispose()
            cn.Dispose()
        End Try

        Return i
    End Function
    Public Shared Function ExecutePSSQLSupportNonQuery(ByVal sql As String) As Integer
        Dim cn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionPSStringSupport"))
        Dim da As New SqlDataAdapter(sql, cn)

        'Dim ds As New DataSet
        Dim i As Integer
        da.SelectCommand.CommandType = CommandType.Text
        da.SelectCommand.CommandTimeout = 60
        cn.Open()
        Try
            i = da.SelectCommand.ExecuteNonQuery()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: ExecuteSQLSupport() Class: cData Statement:" & _
                sql & " Message: " & ex.Message, EventLogEntryType.Error, 50001)
        Finally
            cn.Close()
            da.Dispose()
            cn.Dispose()
        End Try

        Return i
    End Function
    Public Shared Function ExecuteSQLScaler(ByVal sql As String) As Object
        Dim cn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim da As New SqlDataAdapter(sql, cn)

        Dim o As New Object
        da.SelectCommand.CommandType = CommandType.Text
        da.SelectCommand.CommandTimeout = 60
        cn.Open()
        Try
            o = da.SelectCommand.ExecuteScalar()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: ExecuteSQLSupport() Class: cData Statement:" & _
                sql & " Message: " & ex.Message, EventLogEntryType.Error, 50001)
        Finally
            cn.Close()
            da.Dispose()
            cn.Dispose()
        End Try

        Return o
    End Function
    Public Shared Function ExecutePSSQLScaler(ByVal sql As String) As Object
        Dim cn As New SqlConnection(ConfigurationManager.AppSettings("ConnectionPSStringSupport"))
        Dim da As New SqlDataAdapter(sql, cn)

        Dim o As New Object
        da.SelectCommand.CommandType = CommandType.Text
        da.SelectCommand.CommandTimeout = 60
        cn.Open()
        Try
            o = da.SelectCommand.ExecuteScalar()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: ExecuteSQLSupport() Class: cData Statement:" & _
                sql & " Message: " & ex.Message, EventLogEntryType.Error, 50001)
        Finally
            cn.Close()
            da.Dispose()
            cn.Dispose()
        End Try

        Return o
    End Function

    Public Shared Function LogPageHit(ByVal webPage As String, ByVal user As String, ByVal accessGranted As Integer, _
                                        ByVal serverName As String, ByVal ip As String) As Boolean
        Dim rtn As Boolean
        Dim sc As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim da As New SqlDataAdapter("SEMC_UP_LOG_PAGE_HIT", sc)
        da.SelectCommand.CommandType = CommandType.StoredProcedure

        Dim pHost As New SqlParameter("@HOST", SqlDbType.VarChar, 25)
        Dim pWebPage As New SqlParameter("@WEBPAGE", SqlDbType.VarChar, 255)
        Dim pUserID As New SqlParameter("@USERID", SqlDbType.VarChar, 50)
        Dim pIP As New SqlParameter("@IP", SqlDbType.VarChar, 15)
        Dim pAccessGranted As New SqlParameter("@ACCESSGRANTED", SqlDbType.Bit)

        pHost.Value = serverName
        pWebPage.Value = webPage
        pUserID.Value = user
        pIP.Value = ip
        pAccessGranted.Value = accessGranted

        'add the parameter values
        da.SelectCommand.Parameters.Add(pHost)
        da.SelectCommand.Parameters.Add(pWebPage)
        da.SelectCommand.Parameters.Add(pUserID)
        da.SelectCommand.Parameters.Add(pIP)
        da.SelectCommand.Parameters.Add(pAccessGranted)

        da.SelectCommand.CommandTimeout = 100

        Try
            sc.Open()
            da.SelectCommand.ExecuteNonQuery()
            rtn = True
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: LogPageHit Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            sc.Close
        End Try

        Return rtn


    End Function

    Public Shared Function InsertUser(ByVal UserId As String, ByVal UserName As String, ByVal UserTitle As String, ByVal UserPhone As String, email As String) As Boolean

        Dim sc As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim cmd As New SqlCommand("SEMC_UP_INSERT_AUTH_USR", sc)
        Dim rtn As Boolean = False

        Try
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@USER_ID", SqlDbType.VarChar, 35)).Value = UserId
            cmd.Parameters.Add(New SqlParameter("@UserName", SqlDbType.VarChar, 100)).Value = UserName
            cmd.Parameters.Add(New SqlParameter("@UserTitle", SqlDbType.VarChar, 150)).Value = UserTitle
            cmd.Parameters.Add(New SqlParameter("@UserPhone", SqlDbType.VarChar, 20)).Value = UserPhone
            cmd.Parameters.Add(New SqlParameter("@mail", SqlDbType.VarChar, 100)).Value = email

            sc.Open()
            'Execute the insert statement.
            rtn = cmd.ExecuteScalar
        Catch e As SqlException
            'LogEvent("Exception Thrown by the following; Function: InsertUser Class: cData " & _
            '    "Message: " & e.Message, EventLogEntryType.Error, 50001)
            rtn = False
            Throw e
        Finally
            cmd.Dispose()
            sc.Close()
            sc.Dispose()
        End Try

        Return rtn
    End Function
    Public Shared Function AlterWebLinks(ByVal strXML As String) As String

        Dim sc As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim cmd As New SqlCommand("SEMC_UP_ALTER_WEB_LINKS", sc)

        Try
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add(New SqlParameter("@tXML", SqlDbType.Text)).Value = strXML

            sc.Open()
            'Execute the update statement.
            cmd.ExecuteNonQuery()

            'Return the message to the calling procedure
            Return "Success"

        Catch e As SqlException
            LogEvent("Exception Thrown by the following; Function: AlterWebLinks Class: cData " & _
                "Message: " & e.Message, EventLogEntryType.Error, 50001)
            Throw New System.Exception(e.Message)
        Finally
            cmd.Dispose()
            sc.Close()
            sc.Dispose()
        End Try


    End Function

    Public Shared Sub UpdateItemAltDesc(ByVal itemId As Integer, ByVal itemIdb As Integer, ByVal itemDesc As String)

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
        Dim myCommand As New SqlCommand("SEMC_UP_UPDATE_ITEM_ALT_DESC", myConnection)

        myCommand.CommandType = CommandType.StoredProcedure

        Dim pItemId As New SqlParameter("@ItemId", SqlDbType.Int)
        pItemId.Value = itemId
        myCommand.Parameters.Add(pItemId)

        Dim pItemIdb As New SqlParameter("@ItemIdb", SqlDbType.Int)
        pItemIdb.Value = itemIdb
        myCommand.Parameters.Add(pItemIdb)

        Dim pItemDesc As New SqlParameter("@ItemDesc", SqlDbType.VarChar, 255)
        pItemDesc.Value = UCase(itemDesc)
        myCommand.Parameters.Add(pItemDesc)
        Try
            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: UpdateItemAltDesc Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try


    End Sub

    Public Shared Sub UpdateEpicMatchedItems(ByVal itemId As Integer, ByVal itemIdb As Integer, ByVal itemSupplyTypeId As Integer, _
                                               ByVal itemTypeId As Integer, ByVal UserNetId As String)

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myCommand As New SqlCommand("SEMC_UP_UPDATE_EPIC_IF_ITEMS", myConnection)

        myCommand.CommandType = CommandType.StoredProcedure

        Dim pItemId As New SqlParameter("@ItemId", SqlDbType.Int)
        pItemId.Value = itemId
        myCommand.Parameters.Add(pItemId)

        Dim pItemIdb As New SqlParameter("@ItemIdb", SqlDbType.Int)
        pItemIdb.Value = itemIdb
        myCommand.Parameters.Add(pItemIdb)

        Dim pitemSupplyTypeId As New SqlParameter("@SupplyTypeId", SqlDbType.Int)
        pitemSupplyTypeId.Value = itemSupplyTypeId
        myCommand.Parameters.Add(pitemSupplyTypeId)

        Dim pitemTypeId As New SqlParameter("@ItemTypeId", SqlDbType.Int)
        pitemTypeId.Value = itemTypeId
        myCommand.Parameters.Add(pitemTypeId)

        Dim pUserNetId As New SqlParameter("@UserNetId", SqlDbType.VarChar)
        pUserNetId.Value = UserNetId
        myCommand.Parameters.Add(pUserNetId)

        Try
            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            'LogEvent("Exception Thrown by the following; Function: UpdateItemAltDesc Class: cData " & _
            '    "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try


    End Sub

    Public Shared Sub UpdateUserPassword(ByVal userId As Integer, ByVal password As String)

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionString"))
        Dim myCommand As New SqlCommand("SEMC_UP_UPDATE_USR", myConnection)

        myCommand.CommandType = CommandType.StoredProcedure

        Dim pItemId As New SqlParameter("@USR_ID", SqlDbType.Int)
        pItemId.Value = userId
        myCommand.Parameters.Add(pItemId)

        Dim pItemIdb As New SqlParameter("@USR_IDB", SqlDbType.Int)
        pItemIdb.Value = 3025
        myCommand.Parameters.Add(pItemIdb)

        Dim pPassword As New SqlParameter("@Password", SqlDbType.VarChar, 20)
        pPassword.Value = password
        myCommand.Parameters.Add(pPassword)
        Try
            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: UpdateUserPassword Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try


    End Sub

    Public Shared Sub InsertChangeLog(ByVal page As String, ByVal usr As String, ByVal ip As String, ByVal tblModified As String, ByVal rowKey As String, _
                            ByVal colModified As String, ByVal oldValue As String, ByVal newValue As String, ByVal notes As String)

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myCommand As New SqlCommand("SEMC_UP_INSERT_APP_TRANS", myConnection)

        myCommand.CommandType = CommandType.StoredProcedure

        Dim ppage As New SqlParameter("@APP", SqlDbType.VarChar, 50)
        ppage.Value = page
        myCommand.Parameters.Add(ppage)

        Dim puser As New SqlParameter("@USR", SqlDbType.VarChar, 50)
        puser.Value = usr
        myCommand.Parameters.Add(puser)

        Dim pip As New SqlParameter("@IP", SqlDbType.VarChar, 25)
        pip.Value = ip
        myCommand.Parameters.Add(pip)

        Dim ptblModified As New SqlParameter("@TABLE_MOD", SqlDbType.VarChar, 25)
        ptblModified.Value = tblModified
        myCommand.Parameters.Add(ptblModified)

        Dim pcolModified As New SqlParameter("@COL_MOD", SqlDbType.VarChar, 25)
        pcolModified.Value = colModified
        myCommand.Parameters.Add(pcolModified)

        Dim prowKey As New SqlParameter("@ROW_KEY", SqlDbType.VarChar, 25)
        prowKey.Value = rowKey
        myCommand.Parameters.Add(prowKey)

        Dim poldValue As New SqlParameter("@OLD_VALUE", SqlDbType.VarChar, 50)
        poldValue.Value = Left(oldValue, 50)
        myCommand.Parameters.Add(poldValue)

        Dim pnewValue As New SqlParameter("@NEW_VALUE", SqlDbType.VarChar, 50)
        pnewValue.Value = Left(newValue, 50)
        myCommand.Parameters.Add(pnewValue)

        Dim pNotes As New SqlParameter("@NOTES", SqlDbType.NVarChar, 2000)
        pNotes.Value = notes
        myCommand.Parameters.Add(pNotes)

        Try
            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: InsertChangeLog Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try


    End Sub

#Region "Zebra Project "
    Public Shared Sub AlterZebraSelection(ByVal serverId As String, ByVal basisId As Integer, ByVal subbasisId As Integer, ByVal comparisonValue As String, ByVal delete As Boolean)

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionPSStringSupport"))
        Dim myCommand As New SqlCommand("SE_ZEB_CFG_VAL", myConnection)

        myCommand.CommandType = CommandType.StoredProcedure

        Dim pServerId As New SqlParameter("@SERVER_ID", SqlDbType.VarChar, 10)
        pServerId.Value = serverId
        myCommand.Parameters.Add(pServerId)

        Dim pBasisId As New SqlParameter("@BASIS_ID", SqlDbType.Int)
        pBasisId.Value = basisId
        myCommand.Parameters.Add(pBasisId)

        Dim pSubBasisId As New SqlParameter("@SUBBASIS_ID", SqlDbType.Int)
        pSubBasisId.Value = subbasisId
        myCommand.Parameters.Add(pSubBasisId)

        Dim pComparisonValue As New SqlParameter("@COMPARISON_VALUE", SqlDbType.VarChar, 15)
        pComparisonValue.Value = comparisonValue
        myCommand.Parameters.Add(pComparisonValue)

        Dim pDelete As New SqlParameter("@DELETE", SqlDbType.Bit)
        pDelete.Value = delete
        myCommand.Parameters.Add(pDelete)

        Try
            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: AlterZebraSelection Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try


    End Sub

#End Region

#Region "Accrued Receipts Data"
    '*********************************************************************************************************
    '*  Description -   This region contains data objects for the Accrued Receipts interface
    '*  Author -        Kenneth Casson
    '*  Date -          04/27/2012
    '*********************************************************************************************************

    Public Shared Function GetARSettings(ByRef SettingType As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_AR_GET_SETTINGS", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@SettingType", SettingType))


        'myDataAdapter.SelectCommand.CommandTimeout = 0

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetARSettings() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function


    Public Shared Function GetARCorps() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_AR_GET_CORPS", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        'myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetARSettings() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function
    Public Shared Function GetUOMs() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_UOM", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        'myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetARSettings() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    Public Shared Function GetARAccounts() As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_AR_GET_ACCTS", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        'myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@CorpId", corpId))

        'myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetARSettings() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    Public Shared Function GenerateARFileDetail(excludedCostCenters As String, includedExpCodes As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_EXP_ACCR_RCPT_DTL", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@costCenters", excludedCostCenters))
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@expCodes", includedExpCodes))

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetARSettings() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    Public Shared Function GenerateARFileSummary(excludedCostCenters As String, includedExpCodes As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_EXP_ACCR_RCPT", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@costCenters", excludedCostCenters))
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@expCodes", includedExpCodes))

        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetARSettings() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function



    Public Shared Sub UpdateARSettings(ByVal SettingType As String, ByVal FiscalYear As String, FiscalPeriod As String, User As String, SourceCode As String)

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myCommand As New SqlCommand("SEMC_UP_AR_ALTER_SETTINGS", myConnection)

        myCommand.CommandType = CommandType.StoredProcedure

        myCommand.Parameters.Add(New SqlParameter("@FiscalYear", FiscalYear))
        myCommand.Parameters.Add(New SqlParameter("@FiscalPeriod", FiscalPeriod))
        myCommand.Parameters.Add(New SqlParameter("@SourceCode", SourceCode))
        myCommand.Parameters.Add(New SqlParameter("@User", User))
        myCommand.Parameters.Add(New SqlParameter("@SettingType", SettingType))

        Try
            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: UpdateARSettings() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try


    End Sub
    Public Shared Sub alterARCorps(ByVal corpId As String, ByVal corpIdb As String, offset As String, User As String, delete As Boolean)

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myCommand As New SqlCommand("SEMC_UP_AR_ALTER_CORP", myConnection)

        myCommand.CommandType = CommandType.StoredProcedure

        myCommand.Parameters.Add(New SqlParameter("@corp_id", corpId))
        myCommand.Parameters.Add(New SqlParameter("@corp_idb", corpIdb))
        myCommand.Parameters.Add(New SqlParameter("@offset", offset))
        myCommand.Parameters.Add(New SqlParameter("@user", User))
        myCommand.Parameters.Add(New SqlParameter("@delete", delete))

        Try
            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: alterARCorps() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try


    End Sub

    Public Shared Sub alterARAccts(ByVal corpId As Integer, ByVal ccId As Integer, expCodeId As Integer, User As String, delete As Boolean)

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myCommand As New SqlCommand("SEMC_UP_AR_ALTER_ACCTS", myConnection)

        myCommand.CommandType = CommandType.StoredProcedure

        myCommand.Parameters.Add(New SqlParameter("@corp_id", corpId))
        myCommand.Parameters.Add(New SqlParameter("@cc_id", ccId))
        myCommand.Parameters.Add(New SqlParameter("@exp_code_id", expCodeId))
        myCommand.Parameters.Add(New SqlParameter("@user", User))
        myCommand.Parameters.Add(New SqlParameter("@delete", delete))

        Try
            myConnection.Open()
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: alterARCorps() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try


    End Sub


#End Region

#Region "InvoiceOnly"
    Public Shared Function AlterInvoiceOnly(InvoiceOnlyId As String, PONumber As String, Vendor As String, Manufacturer As String, Location As String, _
                    NewItemLog As String, isConsignment As Boolean, CartNumber As String, SalesRepContact As String, Phone As String, User As String, _
                    Status As Int16, ApproveDecisionBy As Object, PatientName As String, DoctorName As String, SurgeryDate As String, CaseNumber As String, _
                    Notes As String, AttachedFile As String) As Integer
        Dim returnValue As Integer
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myCommand As New SqlCommand("SEMC_UP_INSERT_INVONLY", myConnection)

        myCommand.CommandType = CommandType.StoredProcedure

        myCommand.Parameters.Add(New SqlParameter("@InvoiceOnlyID", InvoiceOnlyId))
        myCommand.Parameters.Add(New SqlParameter("@PONumber", PONumber))
        myCommand.Parameters.Add(New SqlParameter("@Vendor", Vendor))
        myCommand.Parameters.Add(New SqlParameter("@Manufacturer", Manufacturer))
        myCommand.Parameters.Add(New SqlParameter("@Location", Location))
        myCommand.Parameters.Add(New SqlParameter("@NewItemLog", NewItemLog))
        myCommand.Parameters.Add(New SqlParameter("@IsConsignment", isConsignment))
        myCommand.Parameters.Add(New SqlParameter("@CartNumber", CartNumber))
        myCommand.Parameters.Add(New SqlParameter("@Phone", Phone))
        myCommand.Parameters.Add(New SqlParameter("@SalesRepContact", SalesRepContact))
        myCommand.Parameters.Add(New SqlParameter("@User", User))
        myCommand.Parameters.Add(New SqlParameter("@Status", Status))
        myCommand.Parameters.Add(New SqlParameter("@ApproveDecisionBy", ApproveDecisionBy))
        myCommand.Parameters.Add(New SqlParameter("@PatientName", PatientName))
        myCommand.Parameters.Add(New SqlParameter("@DoctorName", DoctorName))
        myCommand.Parameters.Add(New SqlParameter("@SurgeryDate", SurgeryDate))
        myCommand.Parameters.Add(New SqlParameter("@CaseNumber", CaseNumber))
        myCommand.Parameters.Add(New SqlParameter("@Notes", Notes))
        myCommand.Parameters.Add(New SqlParameter("@AttachedFile", AttachedFile))


        Try
            myConnection.Open()
            returnValue = myCommand.ExecuteScalar
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: AlterInvoiceOnly() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

        Return returnValue

    End Function

    Public Shared Sub AlterInvoiceOnlyLines(InvoiceOnlyLineId As String, InvoiceOnlyId As Integer, ItemNumber As String, ProductNumber As String, _
    ConversionQuantity As Double, DispensingUOM As String, Price As Double, Description As String, CommodityCode As String, ExpenseCode As String, isChargeable As String, isImplant As Boolean, isChanged As Boolean)
        Dim returnValue As Integer
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))
        Dim myCommand As New SqlCommand("SEMC_UP_INSERT_INVONLYDETAIL", myConnection)

        myCommand.CommandType = CommandType.StoredProcedure

        myCommand.Parameters.Add(New SqlParameter("@InvoiceOnlyLineID", InvoiceOnlyLineId))
        myCommand.Parameters.Add(New SqlParameter("@InvoiceOnlyID", InvoiceOnlyId))
        myCommand.Parameters.Add(New SqlParameter("@ItemNumber", ItemNumber))
        myCommand.Parameters.Add(New SqlParameter("@ProductNumber", ProductNumber))
        myCommand.Parameters.Add(New SqlParameter("@ConversionQty", ConversionQuantity))
        myCommand.Parameters.Add(New SqlParameter("@DispensingUOM", DispensingUOM))
        myCommand.Parameters.Add(New SqlParameter("@Price", Price))
        myCommand.Parameters.Add(New SqlParameter("@Description", Description))
        myCommand.Parameters.Add(New SqlParameter("@CommodityCode", CommodityCode))
        myCommand.Parameters.Add(New SqlParameter("@ExpenseCode", ExpenseCode))
        myCommand.Parameters.Add(New SqlParameter("@IsChargeable", isChargeable))
        myCommand.Parameters.Add(New SqlParameter("@IsImplant", isImplant))
        myCommand.Parameters.Add(New SqlParameter("@IsChanged", isChanged))


        Try
            myConnection.Open()
            returnValue = myCommand.ExecuteNonQuery
        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: AlterInvoiceOnlyLines() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myCommand.Dispose()
        End Try

    End Sub
    Public Shared Function GetItemDetail(ItemNumber As String, ProductNumber As String) As DataSet
        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_ITEM_DETAIL", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@ItemNumber", ItemNumber))
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@ProductNumber", ProductNumber))


        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetItemDetail() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function

    Public Shared Function GetInvoiceOnlyRequest(ReqId As Integer) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_INVONLY", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@ReqId", ReqId))


        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetInvoiceOnlyRequest() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function



    Public Shared Function GetInvoiceOnlyNotify(userType As String) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_GET_INVONLY_NOTIFY", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@userType", userType))



        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: GetInvoiceOnlyNotify() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function



    Public Shared Function AlterInvoiceOnlyNotify(user As String, userType As String, delete As Boolean) As DataSet

        Dim myConnection As New SqlConnection(ConfigurationManager.AppSettings("ConnectionStringSupport"))

        Dim myDataAdapter As New SqlDataAdapter("SEMC_UP_ALTER_INVONLY_NOTIFY", myConnection)
        myDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure

        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@userNetId", user))
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@userType", userType))
        myDataAdapter.SelectCommand.Parameters.Add(New SqlParameter("@delete", delete))


        myDataAdapter.SelectCommand.CommandTimeout = 100

        Dim results As New System.Data.DataSet
        Try
            myDataAdapter.Fill(results)

        Catch ex As Exception
            LogEvent("Exception Thrown by the following; Function: AlterInvoiceOnlyNotify() Class: cData " & _
                "Message: " & ex.Message, EventLogEntryType.Error, 50001)
            Throw ex
        Finally
            myConnection.Close()
            myDataAdapter.Dispose()
            myConnection.Dispose()
        End Try

        Return results
    End Function


#End Region

End Class
