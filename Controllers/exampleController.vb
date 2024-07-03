Imports System.Web.Mvc
Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports Spire.Xls 'example https://github.com/eiceblue/Spire.XLS-for-.NET/tree/master/VB-Examples'
Imports System.IO
Imports System.Data.SqlClient
Imports System.Net
Imports Newtonsoft.Json.Linq
Imports Newtonsoft.Json

Namespace Controllers
    Public Class exampleController
        Inherits mUtility
        '        Function index() As ActionResult
        '            Return View()
        '        End Function
        '        Function xx()
        '            Return Assembly.GetExecutingAssembly().GetCustomAttribute(Of GuidAttribute).Value
        '        End Function
        '        Function importExcel()
        '            Dim file As HttpPostedFileBase, fileInfo As IO.FileInfo, workbook As New Workbook(), workSheet As Worksheet,
        '                returnText As String, fileName As String, xPath As String
        '            Try
        '                For j As Integer = 0 To Request.Files.Count - 1
        '                    file = Request.Files(j)
        '                    fileInfo = New IO.FileInfo(Path.GetFileName(file.FileName))
        '                    fileName = Path.GetFileName(file.FileName)
        '                    returnText = System.Guid.NewGuid.ToString
        '                    If Not Directory.Exists(Server.MapPath("~/DataFiles/bookImport")) Then Directory.CreateDirectory(Server.MapPath("~/DataFiles/bookImport"))
        '                    Directory.CreateDirectory(Server.MapPath("~/DataFiles/bookImport/" & returnText))
        '                    xPath = Path.Combine(Server.MapPath("~/DataFiles/bookImport/" & returnText), fileName)
        '                    file.SaveAs(xPath)
        '                    fileInfo = New IO.FileInfo(xPath)
        '                    If fileInfo.Extension = ".xls" Or fileInfo.Extension = ".xlsx" Then
        '                        workbook.LoadFromFile(fileInfo.FullName)
        '                        workSheet = workbook.Worksheets(0)
        '                        MsgBox(workSheet.Range(16, 1).Value.ToString)
        '                    End If
        '                Next
        '                returnText = "success"
        '            Catch ex As Exception
        '                returnText = ex.Message
        '            Finally
        '                If workSheet IsNot Nothing Then workSheet.Dispose() : workSheet = Nothing
        '                If workbook IsNot Nothing Then workbook.Dispose() : workbook = Nothing
        '                If fileInfo IsNot Nothing Then fileInfo = Nothing
        '                If file IsNot Nothing Then file = Nothing
        '            End Try
        '            Return returnText
        '        End Function
        '        Function test() As ActionResult
        '            Return View()
        '        End Function
        '        Function testGetUserGet(id As String)
        '            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
        '            Dim S1 As New StringBuilder
        '            Try
        '                ' ================ Check Permission ================
        '                cn = New SqlConnection(sqlCon("portaltest"))
        '                If cn.State = 0 Then cn.Open()
        '                ' ==================================================
        '                cmdMsSql = cmdSQL(cn, "select *" &
        '                    " from (" &
        '                        " select userID=u.user_id,prefix=isnull(u.user_prefix,'')" &
        '                            " ,fullName=isnull(u.user_firstname+' '+u.user_lastname,''),nickName=isnull(u.user_nickName,'')" &
        '                            " ,userPosition=isnull(p.pos_name,'')" &
        '                            " ,facID=p.fac_id,secID=p.sec_id,partID=p.part_id,subPartID=p.subpart_id" &
        '                            " ,email=replace(isnull(u.user_email,''),'-','')" &
        '                            " ,mobile=replace(replace(isnull(u.user_mobile,''),'-',''),' ','')" &
        '                            " ,userStatus=u.user_status,userActive=u.isActive,orgTyprID=up.orgType_id" &
        '                        " from tblUser u" &
        '                            " left join tbluserposition up on u.user_id=up.user_id" &
        '                                " and up.orgType_id=(select orgType_id from tblOrganizeType where isActive=1 And isDefault=1) And isnull(up.up_isgroup,1) <> 0" &
        '                            " left join tblposition p on up.pos_id=p.pos_id" &
        '                    " ) t1" &
        '                    " where userStatus=0 and userActive=1" &
        '                        " and (fullName like '%'+@keyword+'%' or userPosition like '%'+@keyword+'%')")
        '                With cmdMsSql
        '                    .Parameters.Add("@keyword", SqlDbType.VarChar).Value = id
        '                    .ExecuteNonQuery()
        '                End With
        '                dt = getDataTable(cmdMsSql)
        '                S1.Append("<table><tbody>")
        '                For Each dr In dt.Rows
        '                    S1.Append("<tr><td>" & dr("prefix").ToString & "</td><td>" & dr("fullName").ToString & "</td><td>" & dr("userPosition").ToString & "</td></tr>")
        '                Next
        '                S1.Append("</tbody></table>")
        'skipFunction:
        '            Catch ex As Exception
        '                S1.Append("<table><tbody><tr><td>" & ex.Message & "</td></tr></tbody></table>")
        '            Finally
        '                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
        '                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
        '                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
        '            End Try
        'exitFunction:
        '            Return S1.ToString
        '        End Function
        '        <AcceptVerbs(HttpVerbs.Post)>
        '        Function testGetUserPostJson1() As JsonResult
        '            Dim L1 As New List(Of userList)
        '            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, strSql As String
        '            Try
        '                ' ================ Check Permission ================
        '                cn = New SqlConnection(sqlCon("portaltest"))
        '                If cn.State = 0 Then cn.Open()
        '                ' ==================================================
        '                strSql = "select *" &
        '                    " from (" &
        '                        " select userID=convert(varchar(36),u.user_id),prefix=isnull(u.user_prefix,'')" &
        '                            " ,fullName=isnull(u.user_firstname+' '+u.user_lastname,''),nickName=isnull(u.user_nickName,'')" &
        '                            " ,userPosition=isnull(p.pos_name,'')" &
        '                            " ,facName=f.fac_name,secName=s.sec_name,partName=part.part_name,subPartName=sp.subpart_name" &
        '                            " ,email=replace(isnull(u.user_email,''),'-','')" &
        '                            " ,mobile=replace(replace(isnull(u.user_mobile,''),'-',''),' ','')" &
        '                            " ,userStatus=u.user_status,userActive=u.isActive,orgTyprID=up.orgType_id" &
        '                        " from tblUser u" &
        '                            " left join tbluserposition up on u.user_id=up.user_id" &
        '                                " and up.orgType_id=(select orgType_id from tblOrganizeType where isActive=1 And isDefault=1) And isnull(up.up_isgroup,1) <> 0" &
        '                            " left join tblposition p on up.pos_id=p.pos_id" &
        '                            " left join tblfaction f on p.fac_id=f.fac_id" &
        '                            " left join tblsection s on p.sec_id=s.sec_id" &
        '                            " left join tblpart part on p.part_id=part.part_id" &
        '                            " left join tblsubpart sp on p.subpart_id=sp.subpart_id" &
        '                    " ) t1" &
        '                    " where userStatus=0 and userActive=1" &
        '                        " and (fullName like '%'+@keyword+'%' or userPosition like '%'+@keyword+'%' or facName like '%'+@facName+'%'" &'
        '                            " or secName like '%'+@secName+'%' or partName like '%'+@partName+'%' or subPartName like '%'+@subPartName+'%')"
        '                cmdMsSql = cmdSQL(cn, strSql)
        '                With cmdMsSql
        '                    .Parameters.Add("@keyword", SqlDbType.VarChar).Value = Request.Form("keyword")
        '                    .Parameters.Add("@facName", SqlDbType.VarChar).Value = Request.Form("facName")
        '                    .Parameters.Add("@secName", SqlDbType.VarChar).Value = Request.Form("secName")
        '                    .Parameters.Add("@partName", SqlDbType.VarChar).Value = Request.Form("partName")
        '                    .Parameters.Add("@subPartName", SqlDbType.VarChar).Value = Request.Form("subPartName")
        '                    .ExecuteNonQuery()
        '                End With
        '                dt = getDataTable(cmdMsSql)
        '                For Each dr In dt.Rows
        '                    L1.Add(New userList(dr("userID"), dr("prefix"), dr("fullName"), dr("userPosition")))
        '                Next
        'skipFunction:
        '            Catch ex As Exception
        '                L1.Add(New userList("error", ex.Message))
        '            Finally
        '                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
        '                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
        '                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
        '            End Try
        'exitFunction:
        '            Return Json(L1, JsonRequestBehavior.AllowGet)
        '        End Function
        '        <AcceptVerbs(HttpVerbs.Post)>
        '        Function testGetUserPostJson2() As JsonResult
        '            Dim L1 As New List(Of userList)
        '            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, strSql As String
        '            Try
        '                ' ================ Check Permission ================
        '                cn = New SqlConnection(sqlCon("portaltest"))
        '                If cn.State = 0 Then cn.Open()
        '                ' ==================================================
        '                strSql = "select *" &
        '                    " from (" &
        '                        " select userID=convert(varchar(36),u.user_id),prefix=isnull(u.user_prefix,'')" &
        '                            " ,fullName=isnull(u.user_firstname+' '+u.user_lastname,''),nickName=isnull(u.user_nickName,'')" &
        '                            " ,userPosition=isnull(p.pos_name,'')" &
        '                            " ,facName=f.fac_name,secName=s.sec_name,partName=part.part_name,subPartName=sp.subpart_name" &
        '                            " ,email=replace(isnull(u.user_email,''),'-','')" &
        '                            " ,mobile=replace(replace(isnull(u.user_mobile,''),'-',''),' ','')" &
        '                            " ,userStatus=u.user_status,userActive=u.isActive,orgTyprID=up.orgType_id" &
        '                        " from tblUser u" &
        '                            " left join tbluserposition up on u.user_id=up.user_id" &
        '                                " and up.orgType_id=(select orgType_id from tblOrganizeType where isActive=1 And isDefault=1) And isnull(up.up_isgroup,1) <> 0" &
        '                            " left join tblposition p on up.pos_id=p.pos_id" &
        '                            " left join tblfaction f on p.fac_id=f.fac_id" &
        '                            " left join tblsection s on p.sec_id=s.sec_id" &
        '                            " left join tblpart part on p.part_id=part.part_id" &
        '                            " left join tblsubpart sp on p.subpart_id=sp.subpart_id" &
        '                    " ) t1" &
        '                    " where userStatus=0 and userActive=1" &
        '                        " and (fullName like '%'+@keyword+'%' or userPosition like '%'+@keyword+'%' or facName like '%'+@facName+'%' or secName like '%'+@secName+'%'"
        '                If Request.Form("partName") IsNot Nothing Then strSql += " or partName like '%'+@partName+'%'"
        '                If Request.Form("subPartName") IsNot Nothing Then strSql += " or subPartName like '%'+@subPartName+'%'"
        '                strSql += ")"
        '                cmdMsSql = cmdSQL(cn, strSql)
        '                With cmdMsSql
        '                    .Parameters.Add("@keyword", SqlDbType.VarChar).Value = Request.Form("keyword")
        '                    .Parameters.Add("@facName", SqlDbType.VarChar).Value = Request.Form("facName")
        '                    .Parameters.Add("@secName", SqlDbType.VarChar).Value = Request.Form("secName")
        '                    If Request.Form("partName") IsNot Nothing Then .Parameters.Add("@partName", SqlDbType.VarChar).Value = Request.Form("partName")
        '                    If Request.Form("subPartName") IsNot Nothing Then .Parameters.Add("@subPartName", SqlDbType.VarChar).Value = Request.Form("subPartName")
        '                    .ExecuteNonQuery()
        '                End With
        '                dt = getDataTable(cmdMsSql)
        '                For Each dr In dt.Rows
        '                    L1.Add(New userList(dr("userID"), dr("prefix"), dr("fullName"), dr("userPosition")))
        '                Next
        'skipFunction:
        '            Catch ex As Exception
        '                L1.Add(New userList("error", ex.Message))
        '            Finally
        '                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
        '                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
        '                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
        '            End Try
        'exitFunction:
        '            Return Json(L1, JsonRequestBehavior.AllowGet)
        '        End Function
        Function test()
            'sms("0897667441", "WeTell", Now.ToString("yyyy-MM-dd HH:mm") & ": test 2")
            'Return sendNotificationFromOneSignal("1195f273-c508-4b25-acdf-649120d7b661", "ทดสอบ", "test 20240304 1400")
            Return Now.ToString("dd-MM-yyyy HH:mm:ss")
        End Function
        Function activity() As ActionResult
            Return View()
        End Function
    End Class
End Namespace