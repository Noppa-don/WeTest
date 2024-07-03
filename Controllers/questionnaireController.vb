Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mvc
Imports System.Drawing
Imports System.Threading

Namespace Controllers
    Public Class questionnaireController
        Inherits mUtility
        ' ====================================================================== Login =====================================================================
        Function login() As ActionResult
            Return View()
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function checkLogin()
            Dim L1 As New List(Of objList)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select userID=lower(convert(varchar(36),userID))" &
                    " from tblUser" &
                    " where isnull(endDate,dateadd(day,1,getdate()))>getdate() and userName=@userName and userPass=@password")
                With cmdMsSql
                    .Parameters.Add("@userName", SqlDbType.VarChar).Value = Request.Form("userName")
                    .Parameters.Add("@password", SqlDbType.VarChar).Value = oneWayKN(Request.Form("password"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                Dim objList As New objList()
                If dt.Rows.Count = 0 Then
                    objList.dataType = "error"
                    objList.errorMsg = "User Name หรือ Password ไม่ถูกต้อง กรุณาระบุใหม่อีกครั้ง"
                Else
                    objList.dataType = "success"
                    Session("xyz") = "EFEDE283-D8A8-4558-9FA9-446DA69C3706" ' dt(0)("userID")
                End If
                L1.Add(objList)
                objList = Nothing
skipFunction:
            Catch ex As Exception
                Dim objList As New objList()
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
        ' ==================================================================== register ====================================================================
        Function register() As ActionResult
            Return View()
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function checkBeforeRegister()
            Dim L1 As New List(Of objList)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New objList()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select case when userName=@userName and mobile=@mobile then 'username and mobile duplicate'" &
                        " when userName=@userName then 'username duplicate' else 'mobile duplicate' end" &
                    " from tbluser" &
                    " where isnull(endDate,dateadd(day,1,getdate()))>getdate() and isOTP=1" &
                        " and (userName=@userName or mobile=@mobile)")
                With cmdMsSql
                    .Parameters.Add("@mobile", SqlDbType.VarChar).Value = Request.Form("mobile")
                    .Parameters.Add("@userName", SqlDbType.VarChar).Value = Request.Form("userName")
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                objList = New objList()
                If dt.Rows.Count = 0 Then
                    objList.dataType = "ok"
                Else
                    objList.dataType = dt(0)(0)
                End If
                L1.Add(objList)
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                objList = Nothing
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function registerSave()
            Dim L1 As New List(Of objList)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, userID As Guid, otp As String, otpID As Guid, i As Int16
            Dim objList As New objList(), fileName As String, fiInfo As FileInfo, filePath As String, oriImage As Image, reImage As Image
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                userID = System.Guid.NewGuid
                otpID = System.Guid.NewGuid
                otp = Now.ToString("ffffff")
                i = 0
resendOTP:
                If sms(Request.Form("mobile"), "WeTell", "Your OTP password is " & otp, cn, cmdMsSql,
                       "insert into tblUserVerifyLog (logID,otpID,message) values (newid(),'" & otpID.ToString & "',@message)") <> "success" Then
                    Thread.Sleep(10000)
                    i += 1
                    If i < 3 Then GoTo resendOTP
                    GoTo sendOtpError
                End If
                cmdMsSql = cmdSQL(cn, "select * from tblUser where userID=@userID" &
                    " if @@rowcount = 0" &
                    " begin" &
                        " insert into tblUser (userID,userFirstName,userLastName,mobile,email,userName,userPass,isSync,sysDate)" &
                        " values (@userID,@fName,@lName,@mobile,@email,@userName,@userPass,0,getdate());" &
                        " insert into tblUserVerify(otpID,userID,otp,sysDate) values (@otpID,@userID,@otp,getdate())" &
                    " end;")
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = userID
                    .Parameters.Add("@fName", SqlDbType.VarChar).Value = Request.Form("fName")
                    .Parameters.Add("@lName", SqlDbType.VarChar).Value = Request.Form("lName")
                    .Parameters.Add("@mobile", SqlDbType.VarChar).Value = Request.Form("mobile")
                    .Parameters.Add("@email", SqlDbType.VarChar).Value = Request.Form("email")
                    .Parameters.Add("@userName", SqlDbType.VarChar).Value = Request.Form("userName")
                    .Parameters.Add("@userPass", SqlDbType.VarChar).Value = oneWayKN(Request.Form("userPass"))
                    .Parameters.Add("@otpID", SqlDbType.UniqueIdentifier).Value = otpID
                    .Parameters.Add("@otp", SqlDbType.VarChar).Value = otp
                    .ExecuteNonQuery()
                End With
                For i = 0 To Request.Files.Count - 1
                    Dim file As HttpPostedFileBase = Request.Files(i)
                    If Not file Is Nothing Then
                        fileName = Path.GetFileName(file.FileName)
                        fiInfo = New IO.FileInfo(fileName)
                        If Not Directory.Exists(Server.MapPath("~/datafiles/questionnaire/profile/")) Then
                            Directory.CreateDirectory(Server.MapPath("~/datafiles/questionnaire/profile/"))
                        End If
                        filePath = Path.Combine(Server.MapPath("~/datafiles/questionnaire/profile/"), userID.ToString.ToLower & fiInfo.Extension)
                        If file.ContentLength < 200000 Then
                            file.SaveAs(filePath)
                        Else
                            oriImage = Image.FromStream(file.InputStream)
                            reImage = resizeImage(oriImage, New Size(1280, 1024))
                            imageCompression(reImage, filePath, 65, file.ContentType)
                        End If
                    End If
                Next
                cmdMsSql = cmdSQL(cn, "select otpReNew from tblConfig where isnull(endDate,dateadd(day,1,getdate()))>getdate()")
                cmdMsSql.ExecuteNonQuery()
                dt = getDataTable(cmdMsSql)
                objList = New objList()
                objList.dataType = "success"
                objList.userID = userID.ToString.ToLower
                objList.otpReNew = dt(0)("otpReNew")
                L1.Add(objList)
                If 1 = 2 Then
sendOtpError:
                    objList = New objList()
                    objList.dataType = "otp error"
                    L1.Add(objList)
                End If
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                userID = Nothing : otp = Nothing : otpID = Nothing : i = Nothing : fiInfo = Nothing : filePath = Nothing : oriImage = Nothing : reImage = Nothing

                objList = Nothing
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function registerConfirmOTP()
            Dim L1 As New List(Of objList)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, userID As Guid
            Dim objList As New objList()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                If Guid.TryParse(Request.Form("userID"), userID) = False Then
                    objList = New objList()
                    objList.dataType = "userID incorrect"
                    L1.Add(objList)
                    GoTo skipFunction
                End If
                dt = checkOTP(cn, cmdMsSql, userID, Request.Form("otp"))
                If dt.Rows.Count = 0 Then
                    objList = New objList()
                    objList.dataType = "incorrect"
                    L1.Add(objList)
                    GoTo skipFunction
                End If
                If dt(0)(0) = "success" Then
                    cmdMsSql = cmdSQL(cn, "update tblUser set isOTP=1 where userID=@userID;" &
                    "update tblUserVerify set verifyDate=getdate() where verifyDate is null and userID=@userID and otp=@otp;")
                    With cmdMsSql
                        .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = userID
                        .Parameters.Add("@otp", SqlDbType.VarChar).Value = Request.Form("otp")
                        .ExecuteNonQuery()
                    End With
                End If
                objList = New objList()
                objList.dataType = dt(0)(0)
                L1.Add(objList)
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                userID = Nothing

                objList = Nothing
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function resendOTP()
            Dim L1 As New List(Of objList)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, userID As Guid, otp As String, otpID As Guid, i As Int16
            Dim objList As New objList()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                If Request.Form("userID") IsNot Nothing Then userID = cGuid(Request.Form("userID")) Else userID = cGuid(Session("xyz"))
                dt = checkRequestOTP(cn, cmdMsSql, userID)
                If dt(0)(0) <> "ok" Then GoTo sendOtpOver
                otpID = System.Guid.NewGuid
                otp = Now.ToString("ffffff")
                i = 0
resendOTP:
                If sms(Request.Form("mobile"), "WeTell", "Your OTP password is " & otp, cn, cmdMsSql,
                       "insert into tblUserVerifyLog (logID,otpID,message) values (newid(),'" & otpID.ToString & "',@message)") <> "success" Then
                    Thread.Sleep(10000)
                    i += 1
                    If i < 3 Then GoTo resendOTP
                    GoTo sendOtpError
                End If
                cmdMsSql = cmdSQL(cn, "update tblUserVerify set endDate=getdate() where verifyDate is null and endDate is null and userID=@userID;" &
                        " insert into tblUserVerify(otpID,userID,otp,sysDate) values (@otpID,@userID,@otp,getdate());")
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = userID
                    .Parameters.Add("@otpID", SqlDbType.UniqueIdentifier).Value = otpID
                    .Parameters.Add("@otp", SqlDbType.VarChar).Value = otp
                    .ExecuteNonQuery()
                End With
                cmdMsSql = cmdSQL(cn, "select otpReNew from tblConfig where isnull(endDate,dateadd(day,1,getdate()))>getdate()")
                cmdMsSql.ExecuteNonQuery()
                dt = getDataTable(cmdMsSql)
                objList = New objList()
                objList.dataType = "success"
                objList.otpReNew = dt(0)("otpReNew")
                If 1 = 2 Then
sendOtpError:
                    objList = New objList()
                    objList.dataType = "otp error"
                End If
                If 1 = 2 Then
sendOtpOver:
                    objList = New objList()
                    objList.dataType = dt(0)(0)
                End If
                L1.Add(objList)
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                otp = Nothing : otpID = Nothing : i = Nothing

                objList = Nothing
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        Function checkRequestOTP(cn As SqlConnection, cmdMsSql As SqlCommand, userID As Guid) As DataTable
            Try
                cmdMsSql = cmdSQL(cn, "select case when (select count(*) from tblUserVerify where convert(date,sysDate)=convert(date,getdate()) and userID=@userID)>otpPerDay then 'over day'" &
                        " when (select count(*) from tblUserVerify where convert(date,sysDate) between dateadd(day,-30,convert(date,getdate())) and convert(date,getdate()) and userID=@userID)>otpPerMonth then 'over 30 day'" &
                        " else 'ok' end" &
                    " from tblConfig")
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = userID
                    .ExecuteNonQuery()
                End With
                Return getDataTable(cmdMsSql)
            Catch ex As Exception
                Return Nothing
            End Try
        End Function
        Function checkOTP(cn As SqlConnection, cmdMsSql As SqlCommand, userID As Guid, otp As String) As DataTable
            cmdMsSql = cmdSQL(cn, "select case" &
                    " when datediff(minute,sysDate,getdate())>(select otpTime from tblConfig where isnull(endDate,dateadd(day,1,getdate()))>getdate()) then 'expire'" &
                    " when otp<>@otp then 'incorrect'" &
                    " else 'success' end" &
                " from tblUserVerify" &
                " where isnull(endDate,dateadd(day,1,getdate()))>getdate() and verifyDate is null and userID=@userID")
            With cmdMsSql
                .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = userID
                .Parameters.Add("@otp", SqlDbType.VarChar).Value = Request.Form("otp")
                .ExecuteNonQuery()
            End With
            Return getDataTable(cmdMsSql)
        End Function
        ' ==================================================================================================================================================
        ' ===================================================================== profile ====================================================================
        Function profile() As ActionResult
            Return View()
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function getProfile()
            Dim L1 As New List(Of clsProfile)
            Dim objList As New clsProfile()
            If Session("xyz") Is Nothing Then objList = New clsProfile() : objList.dataType = "relogin" : L1.Add(objList) : objList = Nothing : GoTo exitFunction
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, di As DirectoryInfo, files As FileInfo()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select userFirstName,userLastName,mobile,email,userName from tblUser where userID=@userID")
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                objList = New clsProfile()
                objList.dataType = "success"
                objList.firstName = dt(0)("userFirstName")
                objList.lastName = dt(0)("userLastName")
                objList.mobile = dt(0)("mobile")
                objList.email = dt(0)("email")
                objList.userName = dt(0)("userName")
                If Directory.Exists(Server.MapPath("~/datafiles/questionnaire/profile/")) Then
                    di = New DirectoryInfo(Server.MapPath("~/datafiles/questionnaire/profile/"))
                    files = di.GetFiles("*.*")
                    For Each fileName In files
                        objList.profileImage = "/datafiles/questionnaire/profile/" & fileName.Name
                    Next
                End If
                L1.Add(objList)
skipFunction:
            Catch ex As Exception
                objList = New clsProfile()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                di = Nothing : files = Nothing : objList = Nothing

                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function checkMobile()
            Dim L1 As New List(Of objList)
            Dim objList As New objList()
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, otpID As Guid, otp As String, i As Int16
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select 'mobile duplicate'" &
                    " from tbluser" &
                    " where isnull(endDate,dateadd(day,1,getdate()))>getdate() and isOTP=1 and userID<>@userID" &
                        " and mobile=@mobile")
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                    .Parameters.Add("@mobile", SqlDbType.VarChar).Value = Request.Form("mobile")
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                objList = New objList()
                If dt.Rows.Count = 0 Then
                    dt = checkRequestOTP(cn, cmdMsSql, cGuid(Session("xyz")))
                    If dt(0)(0) <> "ok" Then GoTo sendOtpOver
                    otpID = System.Guid.NewGuid
                    otp = Now.ToString("ffffff")
                    i = 0
resendOTP:
                    If sms(Request.Form("mobile"), "WeTell", "Your OTP password is " & otp, cn, cmdMsSql,
                       "insert into tblUserVerifyLog (logID,otpID,message) values (newid(),'" & otpID.ToString & "',@message)") <> "success" Then
                        Thread.Sleep(10000)
                        i += 1
                        If i < 3 Then GoTo resendOTP
                        GoTo sendOtpError
                    End If
                    cmdMsSql = cmdSQL(cn, "update tblUserVerify set endDate=getdate() where verifyDate is null and endDate is null and userID=@userID;" &
                        " insert into tblUserVerify(otpID,userID,otp,sysDate) values (@otpID,@userID,@otp,getdate())")
                    With cmdMsSql
                        .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                        .Parameters.Add("@mobile", SqlDbType.VarChar).Value = Request.Form("mobile")
                        .Parameters.Add("@otpID", SqlDbType.UniqueIdentifier).Value = otpID
                        .Parameters.Add("@otp", SqlDbType.VarChar).Value = otp
                        .ExecuteNonQuery()
                    End With
                    cmdMsSql = cmdSQL(cn, "select otpReNew from tblConfig where isnull(endDate,dateadd(day,1,getdate()))>getdate()")
                    cmdMsSql.ExecuteNonQuery()
                    dt = getDataTable(cmdMsSql)
                    objList = New objList()
                    objList.dataType = "success"
                    objList.userID = Session("xyz").ToString.ToLower
                    objList.otpReNew = dt(0)("otpReNew")
                Else
                    objList.dataType = dt(0)(0)
                End If
                If 1 = 2 Then
sendOtpError:
                    objList.dataType = "otp error"
                End If
                If 1 = 2 Then
sendOtpOver:
                    objList.dataType = dt(0)(0)
                End If
                L1.Add(objList)
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                objList = Nothing : otpID = Nothing : otp = Nothing : i = Nothing
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function profileSave()
            Dim L1 As New List(Of objList)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, userID As Guid, strSql As String
            Dim objList As New objList(), fileName As String, fiInfo As FileInfo, filePath As String, oriImage As Image, reImage As Image
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                If Request.Form("otp") IsNot Nothing Then
                    dt = checkOTP(cn, cmdMsSql, cGuid(Session("xyz")), Request.Form("otp"))
                    If dt.Rows.Count = 0 Then
                        objList = New objList()
                        objList.dataType = "incorrect"
                        L1.Add(objList)
                        GoTo skipFunction
                    End If
                End If

                strSql = "update tblUser" &
                    " set userFirstName=@fName,userLastName=@lName,mobile=@mobile,email=@email"
                If Request.Form("userPass") IsNot Nothing Then strSql += ",userPass=@userPass"
                strSql += " where userID=@userID;"
                If Request.Form("otp") IsNot Nothing Then strSql += "update tblUserVerify set verifyDate=getdate()" &
                " where verifyDate is null and userID=@userID and otp=@otp;"

                cmdMsSql = cmdSQL(cn, strSql)
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                    .Parameters.Add("@fName", SqlDbType.VarChar).Value = Request.Form("fName")
                    .Parameters.Add("@lName", SqlDbType.VarChar).Value = Request.Form("lName")
                    .Parameters.Add("@mobile", SqlDbType.VarChar).Value = Request.Form("mobile")
                    .Parameters.Add("@email", SqlDbType.VarChar).Value = Request.Form("email")
                    If Request.Form("userPass") IsNot Nothing Then .Parameters.Add("@userPass", SqlDbType.VarChar).Value = oneWayKN(Request.Form("userPass"))
                    If Request.Form("otp") IsNot Nothing Then .Parameters.Add("@otp", SqlDbType.VarChar).Value = Request.Form("otp")
                    .ExecuteNonQuery()
                End With
                For i = 0 To Request.Files.Count - 1
                    Dim file As HttpPostedFileBase = Request.Files(i)
                    If Not file Is Nothing Then
                        fileName = Path.GetFileName(file.FileName)
                        fiInfo = New IO.FileInfo(fileName)
                        If Not Directory.Exists(Server.MapPath("~/datafiles/questionnaire/profile/")) Then
                            Directory.CreateDirectory(Server.MapPath("~/datafiles/questionnaire/profile/"))
                        End If
                        filePath = Path.Combine(Server.MapPath("~/datafiles/questionnaire/profile/"), userID.ToString.ToLower & fiInfo.Extension)
                        If file.ContentLength < 200000 Then
                            file.SaveAs(filePath)
                        Else
                            oriImage = Image.FromStream(file.InputStream)
                            reImage = resizeImage(oriImage, New Size(1280, 1024))
                            imageCompression(reImage, filePath, 65, file.ContentType)
                        End If
                    End If
                Next
                objList = New objList()
                objList.dataType = "success"
                L1.Add(objList)
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                userID = Nothing : fiInfo = Nothing : filePath = Nothing : oriImage = Nothing : reImage = Nothing

                objList = Nothing
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        ' ==================================================================================================================================================
        ' ================================================================== questionnaire =================================================================
        Function questionnaire() As ActionResult
            Return View()
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function questionnaireGetChoose()
            Dim L1 As New List(Of objList)
            Dim objList As New objList()
            If Session("xyz") Is Nothing Then objList = New objList() : objList.dataType = "relogin" : L1.Add(objList) : objList = Nothing : GoTo exitFunction
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, strSql As String
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                If Request.Form("questionID") Is Nothing Then
                    cmdMsSql = cmdSQL(cn, "select menuID=lower(convert(varchar(36),menuID)),menuName from questionnaireMenu" &
                    " where isnull(endDate,dateadd(day,1,getdate()))>getdate()")
                    cmdMsSql.ExecuteNonQuery()
                    dt = getDataTable(cmdMsSql)
                    For Each dr In dt.Rows
                        objList = New objList()
                        objList.dataType = "menu"
                        objList.menuID = dr("menuID")
                        objList.menuName = dr("menuName")
                        L1.Add(objList)
                    Next
                    cmdMsSql = cmdSQL(cn, "select count(*) from questionnaireUserAnsMS" &
                        " where isnull(endDate,dateadd(day,1,getdate()))>getdate() and backlogName is not null and userSaveID=@userID")
                    With cmdMsSql
                        .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                        .ExecuteNonQuery()
                    End With
                    dt = getDataTable(cmdMsSql)
                    If dt(0)(0) > 0 Then
                        objList = New objList()
                        objList.dataType = "pending"
                        L1.Add(objList)
                    End If
                End If
                strSql = "select menuID=lower(convert(varchar(36),qMenu.menuID))" &
                        " ,headerName=case when qh.headerName='overview' then 'lv1' when qh.headerName='detail' then 'lv2' else qh.headerName end" &
                        " ,questionID=lower(convert(varchar(36),qd.questionID)),questionName=qd.questionDesc" &
                        " ,questionRefID=lower(isnull(convert(varchar(36),qd.questionRefID),''))" &
                        " ,answerID=lower(isnull(convert(varchar(36),qa.answerID),'')),answerName=qa.answerDesc" &
                        " ,subQuestion=(select count(*) from questionnaireDT where questionRefID=qd.questionID)" &
                    " from questionnaireMenu qMenu" &
                        " left join questionnaireMS qm on qMenu.menuID=qm.menuID" &
                        " left join questionnaireDT qd on qm.questionnaireID=qd.questionnaireID" &
                        " left join questionnaireHeader qh on qd.headerID=qh.headerID" &
                        " left join questionnaireGroupAnswer qga on qd.answerGroupID=qga.answerGroupID" &
                        " left join questionnaireAnswer qa on qga.answerID=qa.answerID or qd.questionID=qa.questionID" &
                    " where isnull(qm.endDate,dateadd(day,1,getdate()))>getdate() and qm.isDefault=1"
                If Request.Form("questionID") Is Nothing Then
                    strSql += " and (qh.headerName='overview' or (qh.headerName='detail' and questionRefID is null))"
                Else
                    strSql += " and qMenu.menuID=@menuID and qd.questionRefID=@questionID"
                End If
                strSql += " order by qh.headerName desc,menuID,qd.sort,qa.sort"
                cmdMsSql = cmdSQL(cn, strSql)
                With cmdMsSql
                    If Request.Form("questionID") IsNot Nothing Then
                        .Parameters.Add("@questionID", SqlDbType.UniqueIdentifier).Value = cGuid(Request.Form("questionID"))
                        .Parameters.Add("@menuID", SqlDbType.UniqueIdentifier).Value = cGuid(Request.Form("menuID"))
                    End If
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                For Each dr In dt.Rows
                    objList = New objList()
                    objList.dataType = "question"
                    objList.menuID = dr("menuID")
                    objList.headerName = dr("headerName")
                    objList.questionID = dr("questionID")
                    objList.questionName = dr("questionName")
                    objList.questionRefID = dr("questionRefID")
                    objList.answerID = dr("answerID")
                    objList.answerName = dr("answerName")
                    objList.subQuestion = dr("subQuestion")
                    L1.Add(objList)
                Next
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                objList = Nothing
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function questionnaireGetHistory()
            Dim L1 As New List(Of objList)
            Dim objList As New objList()
            If Session("xyz") Is Nothing Then objList = New objList() : objList.dataType = "relogin" : L1.Add(objList) : objList = Nothing : GoTo exitFunction
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select docID=lower(convert(varchar(36),m.docID)),eventDate=convert(varchar,m.eventDate,103) +' '+convert(varchar(5),eventDate,114)" &
                        " ,m.eventName,refUser=u.userFirstName+' '+u.userLastName" &
                    " from questionnaireUserAnsMS m" &
                        " left join questionnaireUserAnsRefUser ru on m.docID=ru.docID" &
                        " left join tblUser u on ru.userRefID=u.userID" &
                    " where isnull(m.endDate,dateadd(day,1,getdate()))>getdate()" &
                        " and isnull(ru.endDate,dateadd(day,1,getdate()))>getdate()" &
                        " and m.backlogName is null" &
                        " and m.userSaveID=@userID and m.eventDate between @sDate and dateadd(day,1, @fDate)")
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                    .Parameters.Add("@sDate", SqlDbType.Date).Value = Request.Form("sDate")
                    .Parameters.Add("@fDate", SqlDbType.Date).Value = Request.Form("fDate")
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                For Each dr In dt.Rows
                    objList = New objList()
                    objList.dataType = "history"
                    objList.docID = dr("docID")
                    objList.eventDate = dr("eventDate")
                    objList.eventName = dr("eventName")
                    objList.fullName = dr("refUser")
                    L1.Add(objList)
                Next
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                objList = Nothing
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function questionnaireGetHistoryDisplay()
            Dim L1 As New List(Of objList)
            Dim objList As New objList()
            If Session("xyz") Is Nothing Then objList = New objList() : objList.dataType = "relogin" : L1.Add(objList) : objList = Nothing : GoTo exitFunction
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, dv As DataView, di As DirectoryInfo, files As FileInfo(), questionnaireID As Guid
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select docID=lower(convert(varchar(36),qum.docID))" &
                        " ,menuID=lower(convert(varchar(36),qm.menuID))" &
                        " ,menuName=qMenu.menuName" &
                        " ,eventDate=convert(varchar,qum.eventDate,103) +' '+convert(varchar(5),qum.eventDate,114)" &
                        " ,qum.eventName,locationType=case when qum.eventTypeID=1 then 'in' else 'out' end" &
                        " ,qum.eventAddress,qum.questionnaireID" &
                    " from questionnaireUserAnsMS qum" &
                        " left join questionnaireMS qm on qum.questionnaireID=qm.questionnaireID" &
                        " left join questionnaireMenu qMenu on qm.menuID=qMenu.menuID" &
                    " where qum.docID=@docID")
                With cmdMsSql
                    .Parameters.Add("@docID", SqlDbType.UniqueIdentifier).Value = cGuid(Request.Form("docID"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                For Each dr In dt.Rows
                    objList = New objList()
                    objList.dataType = "main"
                    objList.docID = dr("docID")
                    objList.menuID = dr("menuID")
                    objList.menuName = dr("menuName")
                    objList.eventDate = dr("eventDate")
                    objList.eventName = dr("eventName")
                    objList.locationType = dr("locationType")
                    objList.eventAddress = dr("eventAddress")
                    questionnaireID = dr("questionnaireID")
                    L1.Add(objList)
                Next
                cmdMsSql = cmdSQL(cn, "select userID=lower(convert(varchar(36),qu.userRefID)),fullName=u.userFirstName+' '+u.userLastName" &
                    " from questionnaireUserAnsRefUser qu" &
                        " left join tbluser u on qu.userRefID=u.userID" &
                    " where isnull(qu.endDate,dateadd(day,1,getdate()))>getdate() and qu.docID=@docID")
                With cmdMsSql
                    .Parameters.Add("@docID", SqlDbType.UniqueIdentifier).Value = cGuid(Request.Form("docID"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                For Each dr In dt.Rows
                    objList = New objList()
                    objList.dataType = "user"
                    objList.userID = dr("userID")
                    objList.fullName = dr("fullName")
                    L1.Add(objList)
                Next
                cmdMsSql = cmdSQL(cn, "select questionID,questionRefID from questionnaireDT" &
                    " where isnull(endDate,dateadd(day,1,getdate()))>getdate() and questionnaireID=@questionnaireID" &
                    " order by sort")
                With cmdMsSql
                    .Parameters.Add("@questionnaireID", SqlDbType.UniqueIdentifier).Value = questionnaireID
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                dv = New DataView(dt)
                cmdMsSql = cmdSQL(cn, "select questionID=lower(convert(varchar(36),qa.questionID))" &
                        " ,qd.questionDesc,answer=a.answerDesc" &
                    " from questionnaireUserAnsDT qa" &
                        " left join questionnaireDT qd on qa.questionID=qd.questionID" &
                        " left join questionnaireAnswer a on qa.answerID=a.answerID" &
                    " where isnull(qa.endDate,dateadd(day,1,getdate()))>getdate() and qa.docID=@docID" &
                    " order by qd.sort")
                With cmdMsSql
                    .Parameters.Add("@docID", SqlDbType.UniqueIdentifier).Value = cGuid(Request.Form("docID"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                For Each dr In dt.Rows
                    objList = New objList()
                    objList.dataType = "question"
                    objList.questionID = dr("questionID").ToString
                    objList.questionName = dr("questionDesc")
                    dv.RowFilter = "questionID='" & dr("questionID").ToString & "'"
                    If IsDBNull(dv(0)("questionRefID")) Then
                        objList.questionRefID = ""
                        objList.answerName = dr("answer")
                    Else
findQuestion:
                        dv.RowFilter = "questionID='" & dv(0)("questionRefID").ToString & "'"
                        If IsDBNull(dv(0)("questionRefID")) Then
                            objList.questionRefID = dv(0)("questionID").ToString
                            objList.answerName = dr("answer")
                        Else
                            GoTo findQuestion
                        End If
                    End If
                    L1.Add(objList)
                Next
                If Directory.Exists(Server.MapPath("~/datafiles/questionnaire/" & Request.Form("docID") & "/lv1/")) Then
                    di = New DirectoryInfo(Server.MapPath("~/datafiles/questionnaire/" & Request.Form("docID") & "/lv1/"))
                    files = di.GetFiles("*.*")
                    For Each fileName In files
                        objList = New objList()
                        objList.dataType = "file"
                        objList.fileName = fileName.Name
                        objList.fileKey = "lv1"
                        objList.filePath = "/datafiles/questionnaire/" & Request.Form("docID") & "/lv1/"
                        L1.Add(objList)
                    Next
                End If
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                di = Nothing : files = Nothing : objList = Nothing : questionnaireID = Nothing
                If dv IsNot Nothing Then dv.Dispose() : dv = Nothing

                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function questionnaireGetDocPending()
            Dim L1 As New List(Of objList)
            Dim objList As New objList()
            If Session("xyz") Is Nothing Then objList = New objList() : objList.dataType = "relogin" : L1.Add(objList) : objList = Nothing : GoTo exitFunction
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select docID=lower(convert(varchar(36),docID)),backlogName,backlogDesc" &
                        " ,docDate=convert(varchar,sysdate,103) +' '+convert(varchar(5),sysDate,114)" &
                    " from questionnaireUserAnsMS" &
                    " where isnull(endDate,dateadd(day,1,getdate()))>getdate() and backlogName is not null and userSaveID=@userID")
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                For Each dr In dt.Rows
                    objList = New objList()
                    objList.dataType = "pending"
                    objList.docID = dr("docID")
                    objList.backlogName = dr("backlogName")
                    objList.backlogDesc = dr("backlogDesc")
                    objList.docDate = dr("docDate")
                    L1.Add(objList)
                Next
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                objList = Nothing
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function questionnaireGetUser()
            Dim L1 As New List(Of objList)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New objList()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select userID=lower(convert(varchar(36),u.userID)),fullName=u.userFirstName+' '+u.userLastName" &
                    " from tblUser u" &
                        " left join tblUserSubscribe us on u.userID=us.userID" &
                    " where isnull(u.endDate,dateadd(day,1,getdate()))>getdate()" &
                        " and isnull(us.endDate,dateadd(day,1,getdate()))>getdate()" &
                        " and u.userID<>@userID" &
                        " and us.subscribeID in (select subscribeID from tblUserSubscribe where isnull(endDate,dateadd(day,1,getdate()))>getdate() and  userID=@userID)" &
                        " and u.userFirstName+' '+u.userLastName like '%'+@keyword+'%'")
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                    .Parameters.Add("@keyword", SqlDbType.VarChar).Value = Request.Form("keyword")
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                For Each dr In dt.Rows
                    objList = New objList()
                    objList.dataType = "user"
                    objList.userID = dr("userID")
                    objList.fullName = dr("fullName")
                    L1.Add(objList)

                Next
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                objList = Nothing
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function questionnaireDeletePending()
            Dim L1 As New List(Of objList)
            Dim objList As New objList()
            If Session("xyz") Is Nothing Then objList = New objList() : objList.dataType = "relogin" : L1.Add(objList) : objList = Nothing : GoTo exitFunction
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "update questionnaireUserAnsMS set endDate=getdate() where docID=@docID")
                With cmdMsSql
                    .Parameters.Add("@docID", SqlDbType.UniqueIdentifier).Value = cGuid(Request.Form("docID"))
                    .ExecuteNonQuery()
                End With
                objList = New objList()
                objList.dataType = "success"
                L1.Add(objList)
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                objList = Nothing

                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function questionnaireDisplay()
            Dim L1 As New List(Of objList)
            Dim objList As New objList()
            If Session("xyz") Is Nothing Then objList = New objList() : objList.dataType = "relogin" : L1.Add(objList) : objList = Nothing : GoTo exitFunction
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, di As DirectoryInfo, files As FileInfo()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select docID=lower(convert(varchar(36),qum.docID))" &
                        " ,menuID=lower(convert(varchar(36),qm.menuID))" &
                        " ,menuName=qMenu.menuName" &
                        " ,eventDate=convert(varchar,qum.eventDate,103) +' '+convert(varchar(5),qum.eventDate,114)" &
                        " ,qum.eventName,locationType=case when qum.eventTypeID=1 then 'in' else 'out' end" &
                        " ,qum.eventAddress" &
                    " from questionnaireUserAnsMS qum" &
                        " left join questionnaireMS qm on qum.questionnaireID=qm.questionnaireID" &
                        " left join questionnaireMenu qMenu on qm.menuID=qMenu.menuID" &
                    " where qum.docID=@docID")
                With cmdMsSql
                    .Parameters.Add("@docID", SqlDbType.UniqueIdentifier).Value = cGuid(Request.Form("docID"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                For Each dr In dt.Rows
                    objList = New objList()
                    objList.dataType = "main"
                    objList.docID = dr("docID")
                    objList.menuID = dr("menuID")
                    objList.menuName = dr("menuName")
                    objList.eventDate = dr("eventDate")
                    objList.eventName = dr("eventName")
                    objList.locationType = dr("locationType")
                    objList.eventAddress = dr("eventAddress")
                    L1.Add(objList)
                Next
                cmdMsSql = cmdSQL(cn, "select userID=lower(convert(varchar(36),qu.userRefID)),fullName=u.userFirstName+' '+u.userLastName" &
                    " from questionnaireUserAnsRefUser qu" &
                        " left join tbluser u on qu.userRefID=u.userID" &
                    " where isnull(qu.endDate,dateadd(day,1,getdate()))>getdate() and qu.docID=@docID")
                With cmdMsSql
                    .Parameters.Add("@docID", SqlDbType.UniqueIdentifier).Value = cGuid(Request.Form("docID"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                For Each dr In dt.Rows
                    objList = New objList()
                    objList.dataType = "user"
                    objList.userID = dr("userID")
                    objList.fullName = dr("fullName")
                    L1.Add(objList)
                Next
                cmdMsSql = cmdSQL(cn, "select questionID=lower(convert(varchar(36),questionID))" &
                        " ,answerID=lower(convert(varchar(36),answerID)),answerDesc=isnull(answerDesc,'')" &
                    " from questionnaireUserAnsDT" &
                    " where isnull(endDate,dateadd(day,1,getdate()))>getdate() and docID=@docID")
                With cmdMsSql
                    .Parameters.Add("@docID", SqlDbType.UniqueIdentifier).Value = cGuid(Request.Form("docID"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                For Each dr In dt.Rows
                    objList = New objList()
                    objList.dataType = "answer"
                    objList.questionID = dr("questionID")
                    objList.answerID = dr("answerID")
                    objList.answerName = dr("answerDesc")
                    L1.Add(objList)
                    If Directory.Exists(Server.MapPath("~/datafiles/questionnaire/" & Request.Form("docID") & "/" & dr("questionID") & "/")) Then
                        di = New DirectoryInfo(Server.MapPath("~/datafiles/questionnaire/" & Request.Form("docID") & "/" & dr("questionID") & "/"))
                        files = di.GetFiles("*.*")
                        For Each fileName In files
                            objList = New objList()
                            objList.dataType = "file"
                            objList.fileName = fileName.Name
                            objList.fileKey = dr("questionID")
                            objList.filePath = "/datafiles/questionnaire/" & Request.Form("docID") & "/" & dr("questionID") & "/"
                            L1.Add(objList)
                        Next
                    End If
                Next
                If Directory.Exists(Server.MapPath("~/datafiles/questionnaire/" & Request.Form("docID") & "/lv1/")) Then
                    di = New DirectoryInfo(Server.MapPath("~/datafiles/questionnaire/" & Request.Form("docID") & "/lv1/"))
                    files = di.GetFiles("*.*")
                    For Each fileName In files
                        objList = New objList()
                        objList.dataType = "file"
                        objList.fileName = fileName.Name
                        objList.fileKey = "lv1"
                        objList.filePath = "/datafiles/questionnaire/" & Request.Form("docID") & "/lv1/"
                        L1.Add(objList)
                    Next
                End If
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                di = Nothing : files = Nothing : objList = Nothing

                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function questionnaireSave()
            Dim L1 As New List(Of objList)
            Dim objList As New objList()
            If Session("xyz") Is Nothing Then objList = New objList() : objList.dataType = "relogin" : L1.Add(objList) : objList = Nothing : GoTo exitFunction
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, strSql As String, dtnow As Date, docID As Guid,
                refUserIsDelete() As String, refUserID() As String, answerIsDelete() As String, questionID() As String, answerID() As String, answerDesc() As String,
                deleteFileName() As String, deleteRefFile() As String, refFile() As String,
                fileName As String, fiInfo As FileInfo, filePath As String, oriImage As Image, reImage As Image
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                If Request.Form("did") Is Nothing Then docID = System.Guid.NewGuid Else docID = cGuid(Request.Form("did"))
                refUserIsDelete = Request.Form.GetValues("refUserIsDelete")
                refUserID = Request.Form.GetValues("refUserID")

                answerIsDelete = Request.Form.GetValues("answerIsDelete")
                questionID = Request.Form.GetValues("questionID")
                answerID = Request.Form.GetValues("answerID")
                answerDesc = Request.Form.GetValues("answerDesc")

                deleteFileName = Request.Form.GetValues("deleteFileName")
                deleteRefFile = Request.Form.GetValues("deleteRefFile")
                refFile = Request.Form.GetValues("refFile")

                cmdMsSql = cmdSQL(cn, "select getdate()")
                cmdMsSql.ExecuteNonQuery()
                dt = getDataTable(cmdMsSql)
                dtnow = dt(0)(0)

                strSql = " update questionnaireUserAnsMS" &
                    " set questionnaireID=(select questionnaireID from questionnaireMS where isDefault=1 and menuID=@menuID)" &
                        ",eventDate=@eventDate,eventName=@eventName,eventTypeID=@eventTypeID,eventAddress=@eventAddress,backlogName=@backlogName,backlogDesc=@backlogDesc" &
                        ",sysDate=@sysDate,userSaveID=@userSaveID" &
                    " where docID=@docID" &
                    " if @@rowcount = 0" &
                    " begin" &
                        " insert into questionnaireUserAnsMS (docID,questionnaireID,menuType,eventDate,eventName,eventTypeID,eventAddress,backlogName,backlogDesc,sysdate,userSaveID)" &
                        " values (@docID,(select questionnaireID from questionnaireMS where isDefault=1 and menuID=@menuID)" &
                            ",@menuType,@eventDate,@eventName,@eventTypeID,@eventAddress,@backlogName,@backlogDesc,@sysDate,@userSaveID)" &
                    " end;"
                If Not refUserIsDelete Is Nothing Then
                    For i As Integer = 0 To UBound(refUserIsDelete)
                        strSql += "if @refUserIsDelete" & i.ToString("000") & " = 'y'" &
                            " begin" &
                                " update questionnaireUserAnsRefUser set endDate=@sysDate,sysDate=@sysDate,userSaveID=@userSaveID" &
                                " where docID=@docID and userRefID=@userRefID" & i.ToString("000") &
                            " end" &
                            " else" &
                            " begin" &
                                " update questionnaireUserAnsRefUser" &
                                " set endDate=null,sysDate=@sysDate,userSaveID=@userSaveID" &
                                " where docID=@docID and userRefID=@userRefID" & i.ToString("000") &
                                " if @@rowcount = 0" &
                                " begin" &
                                    " insert into questionnaireUserAnsRefUser (docID,userRefID,sysdate,userSaveID)" &
                                    " values (@docID,@userRefID" & i.ToString("000") & ",@sysDate,@userSaveID)" &
                                " end" &
                            " end;"
                    Next
                End If
                If Not answerIsDelete Is Nothing Then
                    For i As Integer = 0 To UBound(answerIsDelete)
                        strSql += "if @answerIsDelete" & i.ToString("000") & " = 'y'" &
                            " begin" &
                                " update questionnaireUserAnsDT set endDate=@sysDate,sysDate=@sysDate,userSaveID=@userSaveID" &
                                " where docID=@docID and questionID=@questionID" & i.ToString("000") & " and answerID=@answerID" & i.ToString("000") &
                            " end" &
                            " else" &
                            " begin" &
                                " update questionnaireUserAnsDT" &
                                " set answerDesc=@answerDesc" & i.ToString("000") & ",endDate=null,sysDate=@sysDate,userSaveID=@userSaveID" &
                                " where docID=@docID and questionID=@questionID" & i.ToString("000") & " and answerID=@answerID" & i.ToString("000") &
                                " if @@rowcount = 0" &
                                " begin" &
                                    " insert into questionnaireUserAnsDT (docID,questionID,answerID,answerDesc,sysdate,userSaveID)" &
                                    " values (@docID,@questionID" & i.ToString("000") & ",@answerID" & i.ToString("000") & ",@answerDesc" & i.ToString("000") &
                                        ",@sysDate,@userSaveID)" &
                                " end" &
                            " end;"
                    Next
                End If

                cmdMsSql = cmdSQL(cn, strSql)
                With cmdMsSql
                    .Parameters.Add("@docID", SqlDbType.UniqueIdentifier).Value = docID
                    .Parameters.Add("@menuID", SqlDbType.UniqueIdentifier).Value = cGuid(Request.Form("menuID"))
                    .Parameters.Add("@menuType", SqlDbType.VarChar).Value = Request.Form("menuType")
                    If IsDate(Request.Form("eventDate")) Then
                        .Parameters.Add("@eventDate", SqlDbType.DateTime).Value = Request.Form("eventDate")
                    Else
                        .Parameters.Add("@eventDate", SqlDbType.DateTime).Value = DBNull.Value
                    End If
                    .Parameters.Add("@eventName", SqlDbType.VarChar).Value = Request.Form("eventName")
                    If Request.Form("locationType").ToLower = "in" Then
                        .Parameters.Add("@eventTypeID", SqlDbType.TinyInt).Value = 1
                    Else
                        .Parameters.Add("@eventTypeID", SqlDbType.TinyInt).Value = 2
                    End If
                    .Parameters.Add("@eventAddress", SqlDbType.VarChar).Value = Request.Form("eventAddress")
                    If Request.Form("backlogName") Is Nothing Then
                        .Parameters.Add("@backlogName", SqlDbType.VarChar).Value = DBNull.Value
                        .Parameters.Add("@backlogDesc", SqlDbType.VarChar).Value = DBNull.Value
                    Else
                        .Parameters.Add("@backlogName", SqlDbType.VarChar).Value = Request.Form("backlogName")
                        .Parameters.Add("@backlogDesc", SqlDbType.VarChar).Value = Request.Form("backlogDesc")
                    End If
                    If Not refUserIsDelete Is Nothing Then
                        For i As Integer = 0 To UBound(refUserIsDelete)
                            .Parameters.Add("@refUserIsDelete" & i.ToString("000"), SqlDbType.VarChar).Value = refUserIsDelete(i)
                            .Parameters.Add("@userRefID" & i.ToString("000"), SqlDbType.UniqueIdentifier).Value = cGuid(refUserID(i))
                        Next
                    End If
                    If Not answerIsDelete Is Nothing Then
                        For i As Integer = 0 To UBound(answerIsDelete)
                            .Parameters.Add("@answerIsDelete" & i.ToString("000"), SqlDbType.VarChar).Value = answerIsDelete(i)
                            .Parameters.Add("@questionID" & i.ToString("000"), SqlDbType.UniqueIdentifier).Value = cGuid(questionID(i))
                            .Parameters.Add("@answerID" & i.ToString("000"), SqlDbType.UniqueIdentifier).Value = cGuid(answerID(i))
                            If answerDesc(i) = "" Then
                                .Parameters.Add("@answerDesc" & i.ToString("000"), SqlDbType.VarChar).Value = DBNull.Value
                            Else
                                .Parameters.Add("@answerDesc" & i.ToString("000"), SqlDbType.VarChar).Value = answerDesc(i)
                            End If
                        Next
                    End If
                    .Parameters.Add("@sysDate", SqlDbType.DateTime).Value = dtnow
                    .Parameters.Add("@userSaveID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                    .ExecuteNonQuery()
                End With

                If Not deleteFileName Is Nothing Then
                    For i As Integer = 0 To UBound(deleteFileName)
                        delFileName(False, "questionnaire/" & docID.ToString.ToLower & "/" & deleteRefFile(i), deleteFileName(i))
                    Next
                End If
                For i As Integer = 0 To Request.Files.Count - 1
                    Dim file As HttpPostedFileBase = Request.Files(i)
                    If Not file Is Nothing Then
                        fileName = Path.GetFileName(file.FileName)
                        fiInfo = New IO.FileInfo(fileName)
                        If Not Directory.Exists(Server.MapPath("~/datafiles/questionnaire/" & docID.ToString.ToLower & "/" & refFile(i) & "/")) Then
                            Directory.CreateDirectory(Server.MapPath("~/datafiles/questionnaire/" & docID.ToString.ToLower & "/" & refFile(i) & "/"))
                        End If
                        filePath = Path.Combine(Server.MapPath("~/datafiles/questionnaire/" & docID.ToString.ToLower & "/" & refFile(i) & "/"), System.Guid.NewGuid.ToString & fiInfo.Extension)
                        If file.ContentLength < 200000 Then
                            file.SaveAs(filePath)
                        Else
                            oriImage = Image.FromStream(file.InputStream)
                            reImage = resizeImage(oriImage, New Size(1280, 1024))
                            imageCompression(reImage, filePath, 65, file.ContentType)
                        End If
                    End If
                Next
                cmdMsSql = cmdSQL(cn, "select count(*) from questionnaireUserAnsMS" &
                        " where isnull(endDate,dateadd(day,1,getdate()))>getdate() and backlogName is not null and userSaveID=@userID")
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                If dt(0)(0) = 0 Then
                    objList = New objList()
                    objList.dataType = "success"
                    L1.Add(objList)
                Else
                    objList = New objList()
                    objList.dataType = "pending"
                    L1.Add(objList)
                End If
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                strSql = Nothing : dtnow = Nothing : docID = Nothing : refUserIsDelete = Nothing : refUserID = Nothing : deleteFileName = Nothing
                deleteRefFile = Nothing : refFile = Nothing : fileName = Nothing : fiInfo = Nothing : filePath = Nothing : oriImage = Nothing : reImage = Nothing
                objList = Nothing

                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        ' ==================================================================================================================================================
        ' ====================================================================== other =====================================================================
        <AcceptVerbs(HttpVerbs.Post)>
        Function questionnaireExit()
            Session.Abandon()
            Return Now.ToString("yyyy-MM-dd HH:mm:ss")
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function questionnaireDeleteAccount()
            Dim L1 As New List(Of objList)
            Dim objList As New objList()
            If Session("xyz") Is Nothing Then objList = New objList() : objList.dataType = "relogin" : L1.Add(objList) : objList = Nothing : GoTo exitFunction
            Dim cn As SqlConnection, cmdMsSql As SqlCommand
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("questionnaire"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "update tblUser set endDate=getdate(),sysDate=getdate(),userSaveID=@userID where userID=@userID")
                With cmdMsSql
                    .Parameters.Add("@userID", SqlDbType.UniqueIdentifier).Value = cGuid(Session("xyz"))
                    .ExecuteNonQuery()
                End With
                objList = New objList()
                objList.dataType = "success"
                L1.Add(objList)
skipFunction:
            Catch ex As Exception
                objList = New objList()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
            Finally
                objList = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
exitFunction:
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function keepSession()
            Return Now.ToString("yyyy-MM-dd HH:mm:ss")
        End Function
        Private Class objList
            Inherits clsMain
            Public menuID As String, menuName As String, headerName As String, questionID As String, questionName As String,
            questionRefID As String, answerID As String, answerName As String, subQuestion As String, userID As String, fullName As String,
            docID As String, eventDate As String, eventName As String, locationType As String, eventAddress As String, backlogName As String, backlogDesc As String, docDate As String,
            fileName As String, fileKey As String, filePath As String, otpReNew As String
        End Class
        Private Class clsProfile
            Inherits clsMain
            Public firstName As String, lastName As String, mobile As String, email As String, userName As String, profileImage As String
        End Class
        ' ==================================================================================================================================================

    End Class
End Namespace