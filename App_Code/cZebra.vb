Imports Microsoft.VisualBasic

<Serializable()> _
Public Class cZebraTab
    Inherits cLocation

    Public enabledCurrent As Boolean
    Public enabledProposed As Boolean
    Public subOptions As New List(Of cSubOption)

End Class
<Serializable()> _
Public Class cLocation
    Public ServerID As String
    Public ServerName As String
    Public LocationID As String
    Public LocationName As String
    Public IgnorePatChrg As Boolean
    Public SuppressSepLabel As Boolean

    Public Function IsMatch(ByVal strServerID As String) As Boolean
        If strServerID = ServerID Then
            Return True
        Else
            Return False
        End If
    End Function

End Class
<Serializable()> _
Public Class cSubOption

    Public enabledCurrent As Boolean
    Public enabledProposed As Boolean
    Public optionValuesCurrent As New List(Of Boolean)
    Public optionValuesProposed As New List(Of Boolean)
    Public AvailableCurrent As New List(Of MenuItem)
    Public AvailableProposed As New List(Of MenuItem)
    Public SelectedCurrent As New List(Of MenuItem)
    Public SelectedProposed As New List(Of MenuItem)

End Class
<Serializable()> _
Public Class cZebraTabs
    Public tabs As New List(Of cZebraTab)
End Class

Public Class cZebraSQL
    'This class builds the sql statements for the zebra program

    Public Shared Function SubOptionRetreiveSQL(ByVal tab As Integer, ByVal opt As Integer, ByVal locId As String) As String
        Dim rtn As String = String.Empty

        Select Case opt
            Case Is = 1 'Cost center option
                rtn = "SELECT A.SE_COMP_VALUE COMPARISON_VALUE ,CC.SE_COST_CTR,RTRIM(CC.SE_COST_CTR) AS VALUE,CC.DESCR AS COST_CTR_NAME,CC.EFF_STATUS AS INDICATOR, " & _
                "(CC.SE_COST_CTR + ' - ' + CC.DESCR) AS TXT_VALUE " & _
            "FROM PS_SE_COST_CENTER CC " & _
            "LEFT JOIN ( " & _
            "SELECT COMP.* FROM PS_SE_PL_CO_SB_CMP AS COMP  " & _
            "WHERE	 " & _
                "COMP.SE_SERVER_ID= '" & locId & "'" & _
                "AND COMP.SE_BASIS_ID= " & tab & _
                "AND COMP.SE_SUBBASIS_ID= " & opt & _
                ") AS A ON CC.SE_COST_CTR=A.SE_COMP_VALUE " & _
            "where  " & _
                 "CC.SETID = 'SHARE' AND " & _
                "CC.EFFDT = (select MAX(ed.EFFDT) " & _
                    "from PS_SE_COST_CENTER ed  " & _
                    "where ed.SE_COST_CTR = CC.SE_COST_CTR " & _
                        "and ed.SETID = 'SHARE' " & _
                        "and ed.EFFDT <= GETDATE()) " & _
            "ORDER BY TXT_VALUE "
            Case Is = 2 'Corp
                rtn = "SELECT A.SE_COMP_VALUE COMPARISON_VALUE,CORP.BUSINESS_UNIT CORP_ID,CORP.BUSINESS_UNIT ACCT_NO,RTRIM(CORP.BUSINESS_UNIT) AS VALUE,CORP.DST_CNTRL_ID CORP_NAME,'A' AS INDICATOR," & _
                       " RTRIM(CORP.BUSINESS_UNIT)+' - '+RTRIM(CORP.DST_CNTRL_ID) AS TXT_VALUE" & _
                  " FROM PS_BUS_UNIT_TBL_AP CORP" & _
                  " LEFT JOIN " & _
                 " (SELECT COMP.SE_COMP_VALUE FROM PS_SE_PL_CO_SB_CMP AS COMP " & _
                 " WHERE COMP.SE_SERVER_ID='" & locId & "'" & _
                 " AND COMP.SE_BASIS_ID=" & tab & _
                  " AND COMP.SE_SUBBASIS_ID=" & opt & ") AS A " & _
                  " ON CORP.BUSINESS_UNIT=A.SE_COMP_VALUE" & _
                 " ORDER BY CORP.BUSINESS_UNIT"
            Case Is = 3 'UOMs
                rtn = "SELECT A.SE_COMP_VALUE COMPARISON_VALUE, CODE_TABLE.UNIT_OF_MEASURE AS VALUE,CODE_TABLE.DESCR AS UOM_DESC,'A' AS INDICATOR,CODE_TABLE.UNIT_OF_MEASURE+' - '+RTRIM(CODE_TABLE.DESCR) AS TXT_VALUE	" & _
                " FROM PS_UNITS_TBL CODE_TABLE " & _
                "  LEFT JOIN " & _
                " (SELECT COMP.* FROM PS_SE_PL_CO_SB_CMP AS COMP " & _
                " WHERE COMP.SE_SERVER_ID='" & locId & "'" & _
                " AND COMP.SE_BASIS_ID=" & tab & _
                "  AND COMP.SE_SUBBASIS_ID=	" & opt & _
                "  ) AS A " & _
                " ON CODE_TABLE.UNIT_OF_MEASURE=A.SE_COMP_VALUE " & _
                " ORDER BY TXT_VALUE;"
            Case Is = 4 'User
                rtn = "SELECT A.SE_COMP_VALUE COMPARISON_VALUE,USR.OPRID AS VALUE,USR.OPRDEFNDESC AS USR_NAME,'A' AS INDICATOR,RTRIM(USR.OPRID) + ' - ' +RTRIM(USR.OPRDEFNDESC) AS TXT_VALUE  " & _
                   " FROM PSOPRDEFN USR	" & _
                   "  inner join PS_USERCLASS_VW  on PS_USERCLASS_VW.OPRID = USR.OPRID " & _
                    "  LEFT OUTER JOIN " & _
                   "  (SELECT COMP.* FROM PS_SE_PL_CO_SB_CMP AS COMP " & _
                   "  WHERE COMP.SE_SERVER_ID='" & locId & "' AND COMP.SE_BASIS_ID=" & tab & _
                   "  AND COMP.SE_SUBBASIS_ID =	4) A on A.SE_COMP_VALUE = USR.OPRID " & _
                   "  where PS_USERCLASS_VW.ROLENAME in ('SEAP PMM') " & _
                   " ORDER BY USR.OPRDEFNDESC;"
            Case Is = 5 'Location
                rtn = "SELECT A.SE_COMP_VALUE COMPARISON_VALUE,LOC.LOCATION AS VALUE,LOC.DESCR AS LOC_NAME,LOC.EFF_STATUS AS INDICATOR,RTRIM(LOC.LOCATION)+' - ' +RTRIM(LOC.DESCR) AS TXT_VALUE  " & _
                " FROM PS_LOCATION_TBL LOC" & _
                 " LEFT JOIN " & _
                " (SELECT COMP.* FROM PS_SE_PL_CO_SB_CMP AS COMP " & _
                " WHERE " & _
                " COMP.SE_SERVER_ID='" & locId & "'" & _
                " AND COMP.SE_BASIS_ID=" & tab & _
                "  AND COMP.SE_SUBBASIS_ID=" & opt & _
                "  ) AS A " & _
                " ON LOC.LOCATION=A.SE_COMP_VALUE " & _
                " WHERE LOC.SETID = 'SHARE' " & _
                " AND LOC.EFFDT = (" & _
                "	SELECT MAX(EFFDT)" & _
                "	FROM PS_LOCATION_TBL EFF" & _
                "	WHERE LOC.LOCATION = EFF.LOCATION" & _
                "	AND EFF.SETID = 'SHARE'" & _
                "	AND EFF.EFFDT <= GETDATE())" & _
                " ORDER BY LOC.DESCR"
            Case Is = 6 'Item List
                rtn = "SELECT A.SE_COMP_VALUE COMPARISON_VALUE,ITEM.INV_ITEM_ID AS VALUE,ITEM.INV_ITEM_ID,ITEM.DESCR,'A' AS INDICATOR,RTRIM(ITEM.INV_ITEM_ID)+' - '+RTRIM(ITEM.DESCR) AS TXT_VALUE " & _
                " FROM PS_MASTER_ITEM_TBL ITEM" & _
                " INNER JOIN PS_BU_ITEMS_INV INV ON INV.INV_ITEM_ID = ITEM.INV_ITEM_ID " & _
                " INNER JOIN PS_BUS_UNIT_TBL_IN BU ON BU.BUSINESS_UNIT = INV.BUSINESS_UNIT " & _
                "  LEFT JOIN " & _
                " (SELECT COMP.* FROM PS_SE_PL_CO_SB_CMP AS COMP " & _
                " WHERE " & _
                "	COMP.SE_SERVER_ID='" & locId & "' " & _
                "	AND COMP.SE_BASIS_ID=" & tab & _
                "	AND COMP.SE_SUBBASIS_ID= " & opt & ") AS A " & _
               "  ON ITEM.INV_ITEM_ID=A.SE_COMP_VALUE " & _
               "  WHERE BU.LOCATION=(SELECT LOCATION FROM PS_SE_PL_SERVERS WHERE SE_SERVER_ID = '" & locId & "'" & _
                 "  ) ORDER BY TXT_VALUE"

        End Select

        Return rtn
    End Function




End Class


