Imports System.Data.SqlClient
Imports System.Web.Mvc

Namespace Controllers
    Public Class weServiceController
        Inherits mUtility
        ' ====================================================================== Login =====================================================================
        Function login() As ActionResult
            Return View()
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function checkLogin()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("weService"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select userID=lower(convert(varchar(36),userID))" &
                    " from tblUser" &
                    " where isActive=1 and userName=@userName and userPass=@password")
                With cmdMsSql
                    .Parameters.Add("@userName", SqlDbType.VarChar).Value = Request.Form("userName")
                    .Parameters.Add("@password", SqlDbType.VarChar).Value = oneWayKN(Request.Form("password"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                Dim objList As New clsMain()
                If dt.Rows.Count = 0 Then
                    objList.dataType = "error"
                    objList.errorMsg = "User Name หรือ Password ไม่ถูกต้อง กรุณาระบุใหม่อีกครั้ง"
                Else
                    objList.dataType = "success"
                    Session("xyz") = dt(0)("userID")
                    cmdMsSql = cmdSQL(cn, "update tblUser" &
                        " set isLogin=getdate()" &
                        " where userID=@userID")
                    With cmdMsSql
                        .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(dt(0)("userID"))
                        .ExecuteNonQuery()
                    End With
                End If
                L1.Add(objList)
                objList = Nothing
skipFunction:
            Catch ex As Exception
                Dim objList As New clsMain()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        ' ==================================================================================================================================================
        ' ==================================================================== weService ===================================================================
        Function weService() As ActionResult
            Return View()
        End Function
        ' ==================================================================================================================================================
    End Class
End Namespace