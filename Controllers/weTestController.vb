Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mvc
Imports System.Drawing
Imports System.Threading
Imports System.Net

Namespace Controllers
    Public Class weTestController
        Inherits mUtility

#Region "Registration"
        Function Registration() As ActionResult
            Return View()
        End Function

        <AcceptVerbs(HttpVerbs.Post)>
        Function SaveUser()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim StdId As String = Guid.NewGuid.ToString
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                Dim CheckResult As String = CheckDuplicateUser()
                If CheckResult = "pass" Then

                    cmdMsSql = cmdSQL(cn, "Insert into tblStudent(StudentID,FirstName,Surname,MobileNo,Email,Username,Password,StudentType) 
                                        values(@StudentID,@FirstName,@Surname,@MobileNo,@Email,@Username,@Password,@StudentType)")
                    With cmdMsSql
                        .Parameters.Add("@StudentID", SqlDbType.VarChar).Value = StdId
                        .Parameters.Add("@FirstName", SqlDbType.VarChar).Value = Request.Form("FirstName")
                        .Parameters.Add("@Surname", SqlDbType.VarChar).Value = Request.Form("Surname")
                        .Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = Request.Form("MobileNo")
                        .Parameters.Add("@Email", SqlDbType.VarChar).Value = Request.Form("Email")
                        .Parameters.Add("@Username", SqlDbType.VarChar).Value = Request.Form("Username")
                        .Parameters.Add("@Password", SqlDbType.VarChar).Value = oneWayKN(Request.Form("Password"))
                        .Parameters.Add("@StudentType", SqlDbType.VarChar).Value = Request.Form("StudentType")
                        .ExecuteNonQuery()
                    End With

                    objList.dataType = "pass"
                    objList.errorMsg = ""

                    Session("StudentId") = StdId
                Else
                    objList.dataType = "notpass"
                    objList.errorMsg = CheckResult
                End If

                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "notpass"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function UploadStudentPhoto()
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Dim fileName As String, fiInfo As FileInfo, filePath As String, oriImage As Image, reImage As Image
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                For i = 0 To Request.Files.Count - 1
                    Dim file As HttpPostedFileBase = Request.Files(i)
                    If Not file Is Nothing Then
                        fileName = Path.GetFileName(file.FileName)
                        fiInfo = New IO.FileInfo(fileName)
                        If Not Directory.Exists(Server.MapPath("~/WetestPhoto/UserPhoto/")) Then
                            Directory.CreateDirectory(Server.MapPath("~/WetestPhoto/UserPhoto/"))
                        End If

                        filePath = Server.MapPath("~/WetestPhoto/UserPhoto/" & Session("StudentId").ToString.ToLower & ".png")

                        If IO.File.Exists(filePath) Then
                            IO.File.Delete(filePath)
                        End If

                        If file.ContentLength < 200000 Then
                            file.SaveAs(filePath)
                        Else
                            oriImage = Image.FromStream(file.InputStream)
                            reImage = resizeImage(oriImage, New Size(1280, 1024))
                            imageCompression(reImage, filePath, 65, file.ContentType)
                            file.SaveAs(filePath)
                        End If
                    Else
                        Dim copyToPath = Server.MapPath("~/WetestPhoto/UserPhoto/" & Session("StudentId").ToString.ToLower & ".png")
                        Dim fullFilePath = Server.MapPath("~/WetestPhoto/UserPhoto/dummyUser.png")

                        System.IO.File.Copy(fullFilePath, copyToPath)
                    End If
                Next

                cmdMsSql = cmdSQL(cn, "Select os.ResponseStatus from tblstudent s inner join tblOTPStatus os 
                                        On s.StudentId = os.ReferenceId And s.studentid = @StdId")
                With cmdMsSql
                    .Parameters.Add("@StdId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0)("ResponseStatus") = 1 Then
                        objList.dataType = "success"
                    Else
                        objList.dataType = "nototp"
                    End If
                Else
                    objList.dataType = "nototp"
                End If

                objList.errorMsg = ""
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function UploadDummyStudentPhoto()
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                Dim copyToPath = Server.MapPath("~/WetestPhoto/UserPhoto/" & Session("StudentId").ToString.ToLower & ".png")
                Dim fullFilePath = Server.MapPath("~/WetestPhoto/UserPhoto/dummyUser.png")

                If IO.File.Exists(copyToPath) Then
                    IO.File.Delete(copyToPath)
                End If

                System.IO.File.Copy(fullFilePath, copyToPath)

                cmdMsSql = cmdSQL(cn, "Select os.ResponseStatus from tblstudent s inner join tblOTPStatus os 
                                        On s.StudentId = os.ReferenceId And s.studentid = @StdId")
                With cmdMsSql
                    .Parameters.Add("@StdId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count <> 0 Then
                    If dt.Rows(0)("ResponseStatus") = 3 Then
                        objList.dataType = "success"
                    Else
                        objList.dataType = "nototp"
                    End If
                Else
                    objList.dataType = "nototp"
                End If

                objList.errorMsg = ""
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function SendOTP()

            Dim L1 As New List(Of clsOTPStatus)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsOTPStatus()
            Dim OSId As String = Guid.NewGuid.ToString
            Try
                Dim MobileNo As String = "66" & Request.Form("MobileNo").TrimStart("0")
                Dim OTPNum As String = Request.Form("OTPNum")

                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                '20240730 เพิ่มการจำกัดจำนวนการส่ง otp ตาม web.config
                Dim OTPPerDay = Getconfig("OTPPerDay")
                cmdMsSql = cmdSQL(cn, "select count(osid) + 1 as otpToday from tblOTPStatus where ReferenceId = @StudentID;")
                With cmdMsSql
                    .Parameters.Add("@StudentID", SqlDbType.VarChar).Value = Session("StudentID")
                    .ExecuteNonQuery()
                End With

                dt = getDataTable(cmdMsSql)

                If CInt(dt(0)("otpToday")) > OTPPerDay Then
                    objList.ResultStatus = "over"
                    objList.Resulttxt = "You have sent OTP more than " & OTPPerDay & " times per day. Please contact us @italt."
                Else
                    Dim url As String
                    Dim MyReq As WebRequest
                    Dim MyRes As WebResponse
                    Dim Rec As Stream
                    Dim Reader As StreamReader
                    Dim Content As String
                    Dim Pos As Integer
                    Dim apiKey As String = "whIRYTWJkMat1SiuQBs1vhlw5kJ9ZCAw7PLcp5sNHs8="
                    Dim clientID As String = "6ee932cc-aba1-46b2-9b1e-e2f60dd239de"
                    url = "https://api.send-sms.in.th/api/v2/SendSMS?SenderID=WeTell&Message=" & "OTP code for Wetest : " & OTPNum & "&MobileNumbers=" + MobileNo + "&ApiKey=" & apiKey & "&ClientId=" & clientID & "&is_unicode=true"
                    MyReq = WebRequest.Create(url)
                    MyReq.ContentLength = 0
                    MyReq.Method = "GET"
                    MyReq.ContentType = "application/json; charset=utf-8"
                    MyRes = MyReq.GetResponse
                    Rec = MyRes.GetResponseStream
                    Reader = New StreamReader(Rec, Encoding.UTF8)
                    Content = Reader.ReadToEnd
                    Pos = Content.IndexOf("Success", 0)

                    If Pos > 0 Then
                        cmdMsSql = cmdSQL(cn, "insert into tblOTPStatus(OSId,OTPCode,ReferenceId)values(@OSId,@OTPNum,@StudentID);")
                        With cmdMsSql
                            .Parameters.Add("@StudentID", SqlDbType.VarChar).Value = Session("StudentID")
                            .Parameters.Add("@OTPNum", SqlDbType.VarChar).Value = OTPNum
                            .Parameters.Add("@OSId", SqlDbType.VarChar).Value = OSId
                            .ExecuteNonQuery()
                        End With
                        Dim OTPAgainTime = Getconfig("OTPAgainTime")
                        objList.ResultStatus = "success"
                        objList.ReponseTime = OTPAgainTime
                        objList.OSId = OSId

                    Else
                        cmdMsSql = cmdSQL(cn, "insert into tblOTPStatus(OTPCode,ReferenceId,SendStatus)values(@OTPNum,@StudentID,0)")
                        With cmdMsSql
                            .Parameters.Add("@StudentID", SqlDbType.VarChar).Value = Session("StudentID")
                            .Parameters.Add("@OTPNum", SqlDbType.VarChar).Value = OTPNum
                            .ExecuteNonQuery()
                        End With

                        objList.ResultStatus = "error"
                        objList.Resulttxt = "Can not Send OTP. Please try again"

                    End If
                End If

                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.ResultStatus = "error"
                objList.Resulttxt = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)

        End Function
        '20240723 -- ปรับวิธีการตรวจสอบ OTP และเพิ่มการบันทึกการตอบกลับ OTP
        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckAndUpdateOTPStatus()
            Dim L1 As New List(Of clsOTPStatus)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsOTPStatus()
            Dim OTPNum As String = Request.Form("OTPNum")
            Dim OSId As String = Request.Form("OSId")
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                Dim OTPAgainTime = Getconfig("OTPResponseTime")

                cmdMsSql = cmdSQL(cn, "select OTPCode,DATEDIFF(ms,sendtime,getdate()) as SendTime from tblOTPStatus where OSId = @OSId and isactive = 1;")

                With cmdMsSql
                    .Parameters.Add("@OSId", SqlDbType.VarChar).Value = OSId
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows().Count <> 0 Then
                    If dt.Rows(0)("sendtime") > OTPAgainTime Then
                        cmdMsSql = cmdSQL(cn, "Update tblOTPStatus set ResponseTime = getdate(),ResponseStatus = 3 , lastupdate = getdate() where osid = @OSId;")

                        With cmdMsSql
                            .Parameters.Add("@OSId", SqlDbType.VarChar).Value = OSId
                            .ExecuteNonQuery()
                        End With

                        objList.ResultStatus = "Expired"
                        objList.Resulttxt = "OTP is Expired! Please click Send again"
                    ElseIf dt.Rows(0)("OTPCode").ToString <> OTPNum Then

                        cmdMsSql = cmdSQL(cn, "Update tblOTPStatus set ResponseTime = getdate(),ResponseStatus = 2 , lastupdate = getdate() where osid = @OSId;")

                        With cmdMsSql
                            .Parameters.Add("@OSId", SqlDbType.VarChar).Value = OSId
                            .ExecuteNonQuery()
                        End With

                        objList.ResultStatus = "wrong"
                        objList.Resulttxt = "OTP is wrong! Please try again"
                    Else
                        cmdMsSql = cmdSQL(cn, "Update tblOTPStatus set ResponseTime = getdate(),ResponseStatus = 1 , lastupdate = getdate() where osid = @OSId;
                                               Update tblStudent set isActive = 1 where studentId = @stdId;")

                        With cmdMsSql
                            .Parameters.Add("@OSId", SqlDbType.VarChar).Value = OSId
                            .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                            .ExecuteNonQuery()
                        End With

                        objList.ResultStatus = "success"
                        objList.Resulttxt = ""
                    End If

                End If

                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.ResultStatus = "error"
                objList.Resulttxt = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetPackagePrice()
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Dim PackagePrice = Getconfig("PackagePrice")
            objList.dataType = PackagePrice
            L1.Add(objList)
            objList = Nothing

            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240826 -- ปรับการตรวจสอบ Keycode และบันทึกข้อมูลต่างๆ
        '20240905 -- check Expired date,Update IsUsed
        '20240909 -- check Keycode จาก LicenseKey
        '20240911 -- ตรวจสอบจำนวนการลงทะเบียน
        '20240913 -- ตรวจสอบ Keycode ที่ Expired = null คือ ไม่มีวันหมดอายุ
        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckKeyCode()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim cnl As SqlConnection
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()

                cnl = New SqlConnection(sqlCon("licenseKey"))
                If cnl.State = 0 Then cnl.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cnl, "select KeyCodeId,KeyCodeDateAmount from wetest_tblKeycode 
                                        where KeyCode = @KeyCode and (KeyCodeExpiredDate >= getdate() or KeyCodeExpiredDate is null)
                                        and IsActive = 1 and (IsUsed = 0 or RegisteredAmount < KeycodeUseAmount) ;")
                With cmdMsSql
                    .Parameters.Add("@KeyCode", SqlDbType.VarChar).Value = Request.Form("KeyCode")
                    .ExecuteNonQuery()
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count = 0 Then
                    objList.dataType = "error"
                    objList.errorMsg = "WeTest Key is wrong !<br><br>Please try again or contact @Italt."
                Else
                    cmdMsSql = cmdSQL(cn, "Update tblstudent set ExpiredDate = getdate() + @KeyCodeDateAmount where studentId = @stdId;
                                            Insert Into tblregister(StudentId,KeyCodeId,RegisterStatus) values(@stdId,@KeyCodeId,2);")

                    With cmdMsSql
                        .Parameters.Add("@KeyCodeId", SqlDbType.VarChar).Value = dt("0")("KeyCodeId").ToString
                        .Parameters.Add("@KeyCodeDateAmount", SqlDbType.Int).Value = CInt(dt("0")("KeyCodeDateAmount"))
                        .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                        .ExecuteNonQuery()
                    End With

                    cmdMsSql = cmdSQL(cnl, "Update wetest_tblKeycode set isused = 1,RegisteredAmount = RegisteredAmount + 1 where KeyCodeId = @KeyCodeId;
                                            select CONVERT (varchar(10),getdate() + keycodedateAmount, 103) AS expiredDate 
                                            from wetest_tblKeycode where keycodeId = @KeyCodeId;")

                    With cmdMsSql
                        .Parameters.Add("@KeyCodeId", SqlDbType.VarChar).Value = dt("0")("KeyCodeId").ToString
                        .ExecuteNonQuery()
                    End With

                    dt = getDataTable(cmdMsSql)

                    objList.dataType = "success"
                    objList.errorMsg = "Package OK! You can use WeTest until " & dt.Rows(0)("expiredDate").ToString

                End If
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
                If cnl IsNot Nothing Then If cnl.State = 1 Then cnl.Close() : cnl.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240913 -- ตรวจสอบ Discount ที่ Expired = null คือ ไม่มีวันหมดอายุ
        '20240917 -- ตรวจสอบจำนวนครั้งในการใช้ Discount Key
        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckDiscount()
            Dim L1 As New List(Of clsDiscount)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim cnl As SqlConnection
            Dim objList As New clsDiscount()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()

                cnl = New SqlConnection(sqlCon("licenseKey"))
                If cnl.State = 0 Then cnl.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cnl, "select DiscountId,DiscountType,DiscountAmount,
                                        case when (expiredate >= getdate() or expiredate is null) then 0 else 1 end as isExpired,
                                        case when RegisteredAmount = UseAmount then 1 else 0 end as isUsed
                                        from wetest_tblDiscount where IsActive = 1 and DiscountCode = @DiscountCode;")
                With cmdMsSql
                    .Parameters.Add("@DiscountCode", SqlDbType.VarChar).Value = Request.Form("DiscountCode")
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                If dt.Rows.Count = 0 Then
                    objList.Result = "error"
                    objList.ResultTxt = "This code does not apply to this promotion!<br>Please try again or Contact us @Italt<br><br>"
                ElseIf dt.Rows(0)("isExpired").ToString = "1" Then
                    objList.Result = "Expired"
                    objList.ResultTxt = "This code is expired!<br>Please try again or Contact us @Italt<br><br>"
                ElseIf dt.Rows(0)("isUsed").ToString = "1" Then
                    objList.Result = "Used"
                    objList.ResultTxt = "This code has alrady beeen used!<br>Please try again or contact @Italt<br><br>"
                Else
                    cmdMsSql = cmdSQL(cn, "select RHId from tblregister where StudentId = @stdId and DiscountId = @discountId;")
                    With cmdMsSql
                        .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId")
                        .Parameters.Add("@discountId", SqlDbType.VarChar).Value = dt.Rows(0)("DiscountId").ToString
                        .ExecuteNonQuery()
                    End With

                    dt = getDataTable(cmdMsSql)

                    If dt.Rows.Count > 0 Then
                        objList.Result = "Used"
                        objList.ResultTxt = "This code has alrady beeen used!<br>Please try again or contact @Italt<br><br>"
                    Else
                        objList.Result = "Success"
                        Dim NetPrice As String = "0"
                        Dim PackagePrice = Getconfig("PackagePrice")

                        If dt.Rows(0)("DiscountType").ToString = "1" Then
                            NetPrice = (CInt(PackagePrice) - CInt(dt.Rows(0)("DiscountAmount"))).ToString
                        Else
                            NetPrice = (CInt(PackagePrice) - ((CInt(PackagePrice) * CInt(dt.Rows(0)("DiscountAmount"))) / 100)).ToString
                        End If

                        objList.ResultTxt = NetPrice
                        objList.DiscountId = dt.Rows(0)("DiscountId").ToString
                    End If

                End If
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.Result = "error"
                objList.ResultTxt = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
                If cnl IsNot Nothing Then If cnl.State = 1 Then cnl.Close() : cnl.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240807 -- check slip upload amount and update expired date
        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckSlipAmount()
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select top 1 RegisterAmount from tblregister where studentId = @stdid order by lastupdate desc;")
                With cmdMsSql
                    .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("StudentId")
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                Dim UploadAmount As Integer
                If dt.Rows.Count <> 0 Then
                    UploadAmount = CInt(dt.Rows(0)(0)) + 1
                Else
                    UploadAmount = 1
                End If

                Dim ConfigAmount As Integer = CInt(Getconfig("ApproveSlipAmount"))

                If UploadAmount <= ConfigAmount Then
                    cmdMsSql = cmdSQL(cn, "insert into tblRegister(studentId,PackageId,RegisterStatus,RegisterAmount,DiscountId,
                                            NetPrice) values(@stdid,'1702F1EF-8FD5-443A-A68D-4599BC9F9E54',1,@RegisAmount,
                                            @DiscountId,@NetPrice);")
                    With cmdMsSql
                        .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("StudentId")
                        .Parameters.Add("@RegisAmount", SqlDbType.VarChar).Value = UploadAmount.ToString
                        .Parameters.Add("@DiscountId", SqlDbType.VarChar).Value = Request.Form("DiscountId")
                        .Parameters.Add("@NetPrice", SqlDbType.VarChar).Value = Request.Form("NetPrice")
                        .ExecuteNonQuery()
                    End With
                    objList.dataType = "success"
                    objList.errorMsg = ""

                Else
                    objList.dataType = "limit"
                    objList.errorMsg = ""
                End If
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function UploadSlipFile()

            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Dim fileName As String, fiInfo As FileInfo, filePath As String, oriImage As Image, reImage As Image
            Try
                For i = 0 To Request.Files.Count - 1
                    Dim file As HttpPostedFileBase = Request.Files(i)
                    If Not file Is Nothing Then
                        fileName = Path.GetFileName(file.FileName)
                        fiInfo = New IO.FileInfo(fileName)
                        If Not Directory.Exists(Server.MapPath("~/WetestPhoto/Slip/")) Then
                            Directory.CreateDirectory(Server.MapPath("~/WetestPhoto/Slip/"))
                        End If
                        filePath = Server.MapPath("~/WetestPhoto/Slip/" & Session("StudentId").ToString.ToLower & ".png")
                        If file.ContentLength < 200000 Then
                            file.SaveAs(filePath)
                        Else
                            oriImage = Image.FromStream(file.InputStream)
                            reImage = resizeImage(oriImage, New Size(1280, 1024))
                            imageCompression(reImage, filePath, 65, file.ContentType)
                            file.SaveAs(filePath)
                        End If
                    End If
                Next

                objList.dataType = "success"
                objList.errorMsg = ""
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240723 -- Update ExpiredDate case กด skip ให้ใช้ TrialDate
        <AcceptVerbs(HttpVerbs.Post)>
        Function UpdateTrialDate()

            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Dim stdId As String = Session("studentid").ToString
            Dim TrialTime = Getconfig("TrialTime")
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "update tblStudent set ExpiredDate =  DATEADD(HH, @TrialTime, getdate()) where StudentId = @stdId;")

                With cmdMsSql
                    .Parameters.Add("@stdId", SqlDbType.VarChar).Value = stdId
                    .Parameters.Add("@TrialTime", SqlDbType.Int).Value = CInt(TrialTime)
                    .ExecuteNonQuery()
                End With

                objList.dataType = "success"
                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240806 -- Update ExpiredDate case Upload Slip แล้ว
        '20240816 -- เพิ่มการ Insert tblRegister
        '20240820 -- เพิ่มการ Insert DiscountId
        '20240902 -- เพิ่มการบันทึก Verify Code และ Update Expired Date = Package Expired Date
        <AcceptVerbs(HttpVerbs.Post)>
        Function UpdateWaitApproveSlip()

            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Dim stdId As String = Session("studentid").ToString
            Dim TrialTime = Getconfig("ApproveSlipTime")
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                Dim DiscountId

                If Request.Form("DiscountCode").ToString() = "undefined" Then
                    DiscountId = DBNull.Value
                Else
                    DiscountId = Request.Form("DiscountCode").ToString()
                End If

                cmdMsSql = cmdSQL(cn, "update tblStudent set ExpiredDate =  (select getdate() + packagetime/24 
                                        from tblpackage where PackageId = @PackageId) where StudentId = @stdId;
                                       insert into tblregister select newid(),@stdId,@PackageId ,null,1 ,1,@DiscountId,'500',null,1,getdate(),@Verifycode;")

                With cmdMsSql
                    .Parameters.Add("@stdId", SqlDbType.VarChar).Value = stdId
                    .Parameters.Add("@TrialTime", SqlDbType.Int).Value = CInt(TrialTime)
                    .Parameters.Add("@DiscountId", SqlDbType.VarChar).Value = DiscountId
                    .Parameters.Add("@Verifycode", SqlDbType.VarChar).Value = Request.Form("VerifyCode").ToString
                    .Parameters.Add("@PackageId", SqlDbType.VarChar).Value = Request.Form("PackageId").ToString
                    .ExecuteNonQuery()
                End With

                objList.dataType = "success"
                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function SaveFirstPlacementTest()
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()

            Try
                Dim Result As String = CheckAndCreatePlacementTest()

                If Result = "success1" Then
                    objList.dataType = "success"
                    L1.Add(objList)
                    objList = Nothing
                Else
                    objList.dataType = "Error"

                    objList.errorMsg = Result
                    L1.Add(objList)
                    objList = Nothing
                End If


            Catch ex As Exception
                objList.dataType = "Error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240723 -- Check Edit Mode and Get User Data
        '20240826 -- ปรับการตรวจสอบ Mode เมื่อเข้าหน้าจอ Register (register edituser purchees)
        <AcceptVerbs(HttpVerbs.Post)>
        Function checkMode()
            Dim L1 As New List(Of clsStudentData)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsStudentData()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                If Session("RefillKey") IsNot Nothing Then
                    objList.Result = "purchess"
                ElseIf Session("EditUserData") IsNot Nothing Then
                    objList = GetUserData(Session("studentid"))
                    objList.Result = "edituser"
                Else
                    objList.Result = "register"
                End If

                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.Result = "error"
                objList.Msg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240723 -- Update User Data
        <AcceptVerbs(HttpVerbs.Post)>
        Function UpdateUser()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim StdId As String = Session("studentId")
            Dim objList As New clsMain(), fileName As String, fiInfo As FileInfo, filePath As String, oriImage As Image, reImage As Image
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "Update tblStudent set FirstName = @FirstName,Surname = @Surname,MobileNo = @MobileNo,Email = @Email,Username = @Username  where studentId = @StudentID;")
                With cmdMsSql
                    .Parameters.Add("@StudentID", SqlDbType.VarChar).Value = StdId
                    .Parameters.Add("@FirstName", SqlDbType.VarChar).Value = Request.Form("FirstName")
                    .Parameters.Add("@Surname", SqlDbType.VarChar).Value = Request.Form("Surname")
                    .Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = Request.Form("MobileNo")
                    .Parameters.Add("@Email", SqlDbType.VarChar).Value = Request.Form("Email")
                    .Parameters.Add("@Username", SqlDbType.VarChar).Value = Request.Form("Username")
                    .ExecuteNonQuery()
                End With

                If Request.Form("Password").ToString <> "" Then
                    cmdMsSql = cmdSQL(cn, "Update Password = @Password where studentId = @StudentID;")
                    With cmdMsSql
                        .Parameters.Add("@Password", SqlDbType.VarChar).Value = oneWayKN(Request.Form("Password"))
                    End With
                End If

                objList.dataType = "success"
                objList.errorMsg = ""
                L1.Add(objList)
                objList = Nothing

                Session("EditUserData") = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function checkRefillKey()
            Dim L1 As New List(Of clsMain)
            Dim StdId As String = Session("studentId")
            Dim objList As New clsMain()
            If Session("RefillKey") IsNot Nothing Then
                objList.dataType = "refillkey"
            Else
                objList.dataType = "not"
            End If
            objList.errorMsg = ""
            L1.Add(objList)
            objList = Nothing
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function

        Function CheckDuplicateUser()
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim Duptxt As String = ""
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "select firstname + Surname as Fullname,MobileNo,username,email,studentType 
                                        from tblStudent where (firstname + Surname = @Fullname or MobileNo = @MobileNo 
                                        or Username = @Username or email = @Email) and IsActive = 1 and ExpiredDate > getdate();")

                With cmdMsSql
                    .Parameters.Add("@Fullname", SqlDbType.VarChar).Value = Request.Form("Fullname")
                    .Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = Request.Form("MobileNo")
                    .Parameters.Add("@Email", SqlDbType.VarChar).Value = Request.Form("Email")
                    .Parameters.Add("@Username", SqlDbType.VarChar).Value = Request.Form("Username")
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows().Count() > 0 Then

                    If dt.Rows(0)("studentType") <> Request.Form("StudentType") Then
                        If dt.Rows(0)("Username") = Request.Form("Username") Then
                            Duptxt &= ",Username"
                        End If
                    Else
                        If dt.Rows(0)("Fullname") = Request.Form("Fullname") Then
                            Duptxt &= ",Name"
                        End If
                        If dt.Rows(0)("MobileNo") = Request.Form("MobileNo") Then
                            Duptxt &= ",Mobile No."
                        End If
                        If dt.Rows(0)("email") = Request.Form("Email") Then
                            Duptxt &= ",E-Mail"
                        End If
                        If dt.Rows(0)("Username") = Request.Form("Username") Then
                            Duptxt &= ",Username"
                        End If
                    End If
                End If

                If Duptxt = "" Then
                    Return "pass"
                Else
                    Duptxt = Duptxt.Substring(1) & " is already exist!"
                    Return Duptxt
                End If
            Catch ex As Exception
                Return "error"
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
        End Function
        '20240806 -- Getconfig from db
        Function Getconfig(ConfigName As String)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select SettingValue from tblsetting where SettingName = @ConfigName and isactive = 1;")
                With cmdMsSql
                    .Parameters.Add("@ConfigName", SqlDbType.VarChar).Value = ConfigName
                    .ExecuteNonQuery()
                End With

                dt = getDataTable(cmdMsSql)
                Return dt(0)(0)
            Catch ex As Exception

            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try

        End Function

#End Region

#Region "Activity"
        Function Activity() As ActionResult
            Return View()
        End Function

        <AcceptVerbs(HttpVerbs.Post)>
        Function SaveAnswed()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Try
                '20240715 -- ตรวจสอบไม่ให้กดตอบคำถามใน Mode เฉลย
                If Session("QuizState") Is Nothing Then

                    ' ================ Check Permission ================
                    cn = New SqlConnection(sqlCon("Wetest"))
                    If cn.State = 0 Then cn.Open()
                    ' ==================================================
                    cmdMsSql = cmdSQL(cn, " Select QuizScoreId from tblQuizScore  where QuizId = @QuizId And QuestionId = @QuestionId;")

                    With cmdMsSql
                        .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                        .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = Request.Form("QuestionId")
                        .ExecuteNonQuery()
                    End With

                    Dim dtScore As DataTable = getDataTable(cmdMsSql)

                    If dtScore.Rows.Count <> 0 Then
                        cmdMsSql = cmdSQL(cn, "update tblQuizScore Set AnswerId = @AnsweredId,ResponseAmount = ResponseAmount + 1,Score = a.AnswerScore,LastUpdate = GETDATE()
                                            from tblQuizScore qs inner join tblAnswer a on qs.QuestionId = a.QuestionId and a.QuestionId = @QuestionId and a.AnswerId = @AnsweredId
                                            where qs.QuizScoreId = @QuizScoreId;")
                        With cmdMsSql
                            .Parameters.Add("@QuizScoreId", SqlDbType.VarChar).Value = dtScore(0)("QuizScoreId").ToString
                            .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = Request.Form("QuestionId")
                            .Parameters.Add("@AnsweredId", SqlDbType.VarChar).Value = Request.Form("AnsweredId")
                            .ExecuteNonQuery()
                        End With
                    Else
                        cmdMsSql = cmdSQL(cn, "insert into tblQuizScore select newid(),@QuizId,@QuestionId,@AnsweredId,1,getdate(),a.AnswerScore,1,getdate()
                                            from tblQuizAnswer qa inner join tblAnswer a on qa.AnswerId = a.AnswerId where qa.quizid = @QuizId and qa.AnswerId = @AnsweredId;")
                        With cmdMsSql
                            .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                            .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = Request.Form("QuestionId")
                            .Parameters.Add("@AnsweredId", SqlDbType.VarChar).Value = Request.Form("AnsweredId")
                            .ExecuteNonQuery()
                        End With
                    End If

                    objList.dataType = "success"
                    objList.errorMsg = ""
                Else
                    objList.dataType = "answered"
                    objList.errorMsg = ""
                End If
                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240814 -- เพิ่ม Assignment
        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckSendDialog()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As New DataTable, dtScore As New DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                '20240712 -- เพิ่ม dialog กดจบการทำฝึกฝน
                '20240814 -- เพิ่ม dialog กดจบการทำ Assignment
                If Session("QuizMode") IsNot Nothing Then
                    If Session("QuizMode").ToString = 1 Then
                        objList.dataType = "pmt"
                        objList.errorMsg = "Do you want to send placement test ?"
                    ElseIf Session("QuizMode").ToString = 2 Then
                        objList.dataType = "practice"
                        If Session("QuizState") = "showanswer" Then
                            objList.errorMsg = "Do you want to exit Practice ?"
                        Else
                            objList.errorMsg = "Do you want to send Practice ?"
                        End If
                    ElseIf Session("QuizMode").ToString = 3 Then
                        objList.dataType = "exam"
                        objList.errorMsg = "Do you want to send Exam ?"
                    ElseIf Session("QuizMode").ToString = 4 Then
                        objList.dataType = "assignment"
                        objList.errorMsg = "Do you want to send Assignment ?"
                    End If
                End If

                '20240722 -- เพิ่ม dialog กลับไปหน้า Report
                If Session("AnsweredfromReport") = "true" Then
                    objList.dataType = "report"
                    objList.errorMsg = "Do you want to go back to report ?"
                End If

                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240814 -- เพิ่ม Assignment
        '202408028 -- ปรับการบันทึกเวลาจบ
        <AcceptVerbs(HttpVerbs.Post)>
        Function EndQuiz()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As New DataTable, dtScore As New DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                If Session("AnsweredfromReport") IsNot Nothing Then
                    objList.dataType = "gotoreport"
                    Session("QuizState") = Nothing
                    Session("QuizId") = Nothing
                    Session("QuestionNo") = Nothing
                    Session("AnsweredfromReport") = Nothing
                Else
                    'Update Score
                    cmdMsSql = cmdSQL(cn, "Select sum(score) As TotalScore,cast((sum(score)*100)/FullScore As Decimal(18,2)) As PercentScore 
                                           from tblQuizScore qs inner join tblquiz q On q.quizid = qs.quizid 
                                            where qs.QuizId = @QuizId Group by FullScore;")
                    With cmdMsSql
                        .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                    End With

                    dtScore = getDataTable(cmdMsSql)

                    cmdMsSql = cmdSQL(cn, "Update tblquiz set TotalScore = @TotalScore, PercentScore = @PercentScore, EndTime = getdate(),lastupdate = getdate() where quizId = @QuizId;")

                    With cmdMsSql
                        .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId")
                        .Parameters.Add("@TotalScore", SqlDbType.VarChar).Value = dtScore(0)("TotalScore")
                        .Parameters.Add("@PercentScore", SqlDbType.VarChar).Value = dtScore(0)("PercentScore")
                    End With

                    dt = getDataTable(cmdMsSql)

                    'Next Step

                    If Session("QuizMode") = "1" Then
                        'QuizMode 1 : PlacementTest
                        Dim Result As String = CheckAndCreatePlacementTest()
                        'ครั้งที่ 1 ให้ทำชุดที่ 2 ต่อ
                        If Result = "success2" Then
                            objList.dataType = "success2"
                            Session("QuestionNo") = Nothing
                        Else
                            'ครั้งที่ 2 ให้บันทึก Level ที่ทำได้
                            Dim ResultTxt As String = UpdateStudentLevel(CInt(dtScore(0)("PercentScore")))
                            If ResultTxt <> "error" Then
                                objList.dataType = "success3"
                                objList.errorMsg = ResultTxt
                                Session("QuizId") = Nothing
                                Session("QuestionNo") = Nothing
                            End If
                        End If
                        '20240712 -- เพิ่มการตรวจสอบสถานะการกดส่ง Practice
                    ElseIf Session("QuizMode") = "2" Then
                        'QuizMode 2 : Practice
                        If Session("QuizState") IsNot Nothing Then
                            Session("QuizState") = Nothing
                            Session("QuizId") = Nothing
                            Session("QuestionNo") = Nothing
                            objList.dataType = "success2"
                        Else
                            Session("QuizState") = "showanswer"
                            objList.dataType = "showanswer"
                            Session("QuestionNo") = Nothing
                        End If
                    ElseIf Session("QuizMode") = "3" Then
                        'QuizMode 3 : Exam
                        objList = CheckAndUplevel()
                        Session("QuizId") = Nothing
                        Session("QuestionNo") = Nothing
                    ElseIf Session("QuizMode") = "4" Then
                        'QuizMode 4 : Assignment
                        objList.dataType = "success4"
                        Session("QuizId") = Nothing
                        Session("QuestionNo") = Nothing
                    End If
                End If
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetProgressbarStatus()
            Dim L1 As New List(Of clsProgressbar)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As New DataTable, dtAnswered As New DataTable
            Dim objList As New clsProgressbar()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "select count(qs.quizscoreId) as AnsweredNum,q.FullScore from tblQuizScore qs inner join tblquiz q on qs.QuizId = q.QuizId where qs.QuizId = @QuizId group by q.FullScore;")
                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                End With

                dtAnswered = getDataTable(cmdMsSql)

                Dim PercentAnswered As String

                If dtAnswered.Rows.Count = 0 Then
                    PercentAnswered = 0
                Else
                    PercentAnswered = ((CInt(dtAnswered(0)("AnsweredNum")) * 100)) / CInt(dtAnswered(0)("FullScore")).ToString
                End If


                objList.Result = "success"
                objList.AnsweredPercent = PercentAnswered
                objList.AnsweredAmount = dtAnswered(0)("AnsweredNum").ToString & "/" & CInt(dtAnswered(0)("FullScore")).ToString
                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.Result = "error"
                objList.ResultTxt = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function SaveQuestionProblem()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "insert into tblquestionProblem select newid(),qq.QuestionId,@ProblemTopic,@ProblemDetail,1,@StudentId,1,getdate() 
                                        from tblquizquestion qq where QuizId = @QuizId and qqno = @QQno")

                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                    .Parameters.Add("@QQno", SqlDbType.VarChar).Value = Session("QuestionNo").ToString
                    .Parameters.Add("@ProblemTopic", SqlDbType.VarChar).Value = Request.Form("ProblemTopic")
                    .Parameters.Add("@ProblemDetail", SqlDbType.VarChar).Value = Request.Form("ProblemDetail")
                    .Parameters.Add("@StudentId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                    .ExecuteNonQuery()
                End With

                objList.dataType = "success"
                objList.errorMsg = " "
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetLeapChoicePanel()
            Dim L1 As New List(Of clsLeapChoiceData)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsLeapChoiceData()
            Dim LeapChoicetxt As String = ""
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                '20240712 -- เพิ่ม Query ตาม ChoiceMode เพื่อตรวจสอบว่าแสดงทั้งหมดหรือข้อข้าม
                If Request.Form("ChoiceMode") = "1" Then
                    cmdMsSql = cmdSQL(cn, "select qqno,qq.QuestionId,qs.QuizScoreId,qs.AnswerId from tblQuizQuestion qq left join tblQuizScore qs on qq.QuizId = qs.QuizId and qq.QuestionId = qs.QuestionId 
                                        where qq.quizId = @QuizId order by qqno")
                ElseIf Request.Form("ChoiceMode") = "2" Then
                    cmdMsSql = cmdSQL(cn, "select qqno,qq.QuestionId,qs.QuizScoreId,qs.AnswerId from tblQuizQuestion qq left join tblQuizScore qs on qq.QuizId = qs.QuizId and qq.QuestionId = qs.QuestionId 
                                        where qq.quizId = @QuizId and qs.QuizScoreId is not null and qs.AnswerId is null order by qqno")
                End If

                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                End With

                dt = getDataTable(cmdMsSql)

                Dim PageNum As Integer
                PageNum = Math.Ceiling(dt.Rows.Count() / 10)

                Dim rowNum As Integer = 0

                For i = 1 To PageNum
                    If i = 1 Then
                        LeapChoicetxt &= "<div id=""pageLeapchoice" & i & """ class=""pageLeapchoice"">"
                    Else
                        LeapChoicetxt &= "<div id=""pageLeapchoice" & i & """ class=""pageLeapchoice ui-hide"">"
                    End If

                    For k = 1 To 2
                        LeapChoicetxt &= "<div class=""flexDiv"">"
                        For r = 1 To 5
                            Dim ChoiceClass As String = ""
                            Dim PanelClass As String = ""
                            If dt(rowNum)("QuizScoreId").ToString = "" Then
                                ChoiceClass = "DisabledChoice"
                                PanelClass = "UnActive"
                            ElseIf dt(rowNum)("AnswerId").ToString = "" Then
                                ChoiceClass = "NotAnsweredChoice"
                            Else
                                ChoiceClass = "AnsweredChoice"
                            End If

                            If dt(rowNum)("QQNo").ToString = Session("questionno") Then
                                ChoiceClass = "stay"
                            End If

                            LeapChoicetxt &= "<div class=""LeapchoiceItem" & PanelClass & """ qno=""" & dt(rowNum)("QQNo").ToString & """><div class=""" & ChoiceClass & """></div><span class=""QQNo"">" & dt(rowNum)("QQNo").ToString & "</span></div>"
                            rowNum += 1
                            If rowNum > dt.Rows.Count() - 1 Then Exit For
                        Next
                        LeapChoicetxt &= "</div>"
                        If rowNum > dt.Rows.Count() - 1 Then Exit For
                    Next
                    LeapChoicetxt &= "</div>"
                Next

                objList.result = "success"
                objList.leapChoicetxt = LeapChoicetxt
                objList.allPage = PageNum
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.result = "error"
                objList.leapChoicetxt = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240715 -- สร้าง Panel แสดงข้อเฉลย
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetAnswerChoicePanel()
            Dim L1 As New List(Of clsAnswerChoice)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsAnswerChoice()
            Dim AnswerChoicetxt As String = ""
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                'ChoiceMode 1 : ทั้งหมด , ChoiceMode 2 : ข้อถูก, ChoiceMode 3 : ข้อผิด, ChoiceMode 4 : ข้อข้าม
                cmdMsSql = cmdSQL(cn, "select qqno,qq.QuestionId,qs.QuizScoreId,qs.AnswerId,qs.score from tblQuizQuestion qq left join tblQuizScore qs on qq.QuizId = qs.QuizId and qq.QuestionId = qs.QuestionId 
                                        where qq.quizId = @QuizId order by qqno")

                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                End With

                dt = getDataTable(cmdMsSql)

                Dim ResultRightdata = From r In dt Where r("score").ToString <> "" AndAlso CInt(r("score")) > 0
                Dim ResultWrongdata = From r In dt Where r("score").ToString <> "" AndAlso CInt(r("score")) <= 0 And r("AnswerId").ToString <> ""
                Dim ResultLeapdata = From r In dt Where r("QuizScoreId").ToString = "" Or r("AnswerId").ToString = ""

                If Request.Form("ChoiceMode") = "2" Then
                    If ResultRightdata.Count <> 0 Then
                        dt = ResultRightdata.CopyToDataTable
                    Else
                        dt = Nothing
                    End If
                ElseIf Request.Form("ChoiceMode") = "3" Then
                    If ResultWrongdata.Count <> 0 Then
                        dt = ResultWrongdata.CopyToDataTable
                    Else
                        dt = Nothing
                    End If
                ElseIf Request.Form("ChoiceMode") = "4" Then
                    If ResultLeapdata.Count <> 0 Then
                        dt = ResultLeapdata.CopyToDataTable
                    Else
                        dt = Nothing
                    End If
                End If

                Dim PageNum As Integer = 0
                If dt IsNot Nothing Then
                    PageNum = Math.Ceiling(dt.Rows.Count() / 10)

                    Dim rowNum As Integer = 0

                    For i = 1 To PageNum
                        If i = 1 Then
                            AnswerChoicetxt &= "<div id=""pageAnswerchoice" & i & """ class=""pageAnswerchoice"">"
                        Else
                            AnswerChoicetxt &= "<div id=""pageAnswerchoice" & i & """ class=""pageAnswerchoice ui-hide"">"
                        End If

                        For k = 1 To 2
                            AnswerChoicetxt &= "<div class=""flexDiv"">"
                            For r = 1 To 5
                                Dim ChoiceClass As String = ""

                                If dt(rowNum)("QuizScoreId").ToString = "" Or dt(rowNum)("AnswerId").ToString = "" Then
                                    ChoiceClass = "LeapAnsweredChoice"
                                ElseIf CInt(dt(rowNum)("score")) > 0 Then
                                    ChoiceClass = "RightAnsweredChoice"
                                Else
                                    ChoiceClass = "WrongAnsweredChoice"
                                End If

                                AnswerChoicetxt &= "<div class=""LeapchoiceItem"" qno=""" & dt(rowNum)("QQNo").ToString & """><div class=""" & ChoiceClass & """></div><span class=""QQNo"">" & dt(rowNum)("QQNo").ToString & "</span></div>"
                                rowNum += 1
                                If rowNum > dt.Rows.Count() - 1 Then Exit For
                            Next
                            AnswerChoicetxt &= "</div>"
                            If rowNum > dt.Rows.Count() - 1 Then Exit For
                        Next
                        AnswerChoicetxt &= "</div>"
                    Next
                End If



                objList.result = "success"
                objList.AnswerChoicetxt = AnswerChoicetxt
                objList.allPage = PageNum
                objList.RightAmount = ResultRightdata.Count
                objList.WrongAmount = ResultWrongdata.Count
                objList.LeapAmount = ResultLeapdata.Count
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.result = "error"
                objList.AnswerChoicetxt = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function checkAnsweredFromReport()
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()

            Try
                If Session("AnsweredfromReport") IsNot Nothing Then
                    objList.dataType = "showanswer"
                Else
                    objList.dataType = "no"
                End If
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.ToString
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240730 -- ดึง Logo และ text ตามเมนูที่เข้าทำ Quiz
        '20240731 -- ปรับการดึง Icon ตามสกิลที่เลือกสร้างชุดข้อสอบ
        '20240911 -- ปรับการดึง Icon ตามสกิลที่เลือกสร้างชุดข้อสอบ
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetQuizLogo()
            Dim L1 As New List(Of clsQuizData)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As New DataTable, dtQuiz As New DataTable, dtSkill As DataTable
            Dim objList As New clsQuizData()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "select q.QuizMode,q.QuizName,q.FullScore,IsStandart from  tblquiz q 
                                        inner join tblTestset t on q.TestSetId = t.testsetid
                                        where q.QuizId = @QuizId group by q.FullScore,q.QuizMode,q.QuizName,IsStandart;")
                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                End With

                dtQuiz = getDataTable(cmdMsSql)

                objList.resultType = "success"

                Select Case dtQuiz(0)("QuizMode")
                    Case "1"
                        objList.QuizMode = "<div class=""LogoPT""></div>"
                        objList.QuizName = "Placement Test"
                    Case "2"
                        cmdMsSql = cmdSQL(cn, "Select distinct pei.EI_id,pei.EI_Code from tblQuizQuestion qq inner join tblQuestionEvaluationIndexItem qei 
                                                On qq.QuestionId = qei.Question_Id inner Join tblEvaluationIndex ei on qei.EI_Id = ei.EI_Id 
                                                inner join tblEvaluationIndex pei on ei.Parent_Id = pei.ei_id                                                        
                                                And pei.EI_Id in('31667BAB-89FF-43B3-806F-174774C8DFBF','5BBD801D-610F-40EB-89CB-5957D05C4A0B',
                                                'FB4B4A71-B777-4164-BA4D-5C1EA9522226','25DA1FAB-EB20-4B1D-8409-C2FB08FC61B3','44502C7F-D3BE-4D46-9134-3FE40DA230E9') where qq.QuizId = @QuizId 
                                                and qq.isactive = 1 and qei.IsActive = 1;")
                        With cmdMsSql
                            .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                        End With

                        dtSkill = getDataTable(cmdMsSql)

                        Dim skilltxt As String = ""
                        For i = 0 To dtSkill.Rows.Count - 1
                            Select Case dtSkill(i)("Ei_Id").ToString.ToUpper
                                Case "31667BAB-89FF-43B3-806F-174774C8DFBF"
                                    skilltxt &= " <div Class='Logovocab'></div>"
                                Case "5BBD801D-610F-40EB-89CB-5957D05C4A0B"
                                    skilltxt &= " <div Class='Logogrammar'></div>"
                                Case "FB4B4A71-B777-4164-BA4D-5C1EA9522226"
                                    skilltxt &= " <div Class='Logoread'></div>"
                                Case "44502C7F-D3BE-4D46-9134-3FE40DA230E9"
                                    skilltxt &= " <div Class='Logolisten'></div>"
                            End Select
                            If i = 1 Then
                                skilltxt &= "<br />"
                            End If
                        Next

                        objList.QuizMode = skilltxt
                        If dtQuiz(0)("IsStandart").ToString.ToLower = "true" Then
                            objList.QuizName = dtSkill(0)("EI_Code").Replace("Skills ", "") & " " & dtQuiz(0)("QuizName")
                        Else
                            objList.QuizName = ""
                        End If

                    Case "3"
                        objList.QuizMode = "<div class=""logoM""></div>"
                        objList.QuizName = "Mock-up Exam"
                End Select

                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.resultType = "Error"
                objList.resultMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240823 -- ดึงเวลาเริ่มทำควิซ Case กด Refresh แล้วเวลาเริ่มนับใหม่
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetStartTime()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As New DataTable, dtQuiz As New DataTable, dtSkill As DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "select datediff(ss,StartTime,getdate()) as DiffTime from tblQuiz where QuizId = @QuizId;")
                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                End With

                dtQuiz = getDataTable(cmdMsSql)

                If dtQuiz.Rows.Count <> 0 Then

                End If
                objList.dataType = "success"
                objList.errorMsg = dtQuiz.Rows(0)("DiffTime")

                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "Error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function

        Function CreateNewQuiz(QuizData As clsQuizData) As clsQuizData
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()

            Dim NewQuizId As String = Guid.NewGuid.ToString

            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "Select sum(a.answerScore) As fullscore from tblanswer a 
                                            inner join tblquestion q on a.questionid = q.questionid 
                                            inner join tbltestsetquestionDetail tsqd on q.questionId = tsqd.questionId
                                            inner join tbltestsetquestionSet tsqs on tsqs.tsqsid = tsqd.tsqsid
                                            where a.isactive = 1 and q.isactive = 1 and tsqd.isactive = 1 and tsqs.isactive = 1 
                                            and tsqs.testsetid = @TestsetId;")
                With cmdMsSql
                    .Parameters.Add("@TestsetId", SqlDbType.VarChar).Value = QuizData.TestsetId
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count = 0 Then
                    QuizData.resultType = "error"
                    QuizData.resultMsg = "Not have Placement test for use please contact @italt"
                ElseIf dt.Rows(0)(0).ToString = "" Then
                    QuizData.resultType = "error"
                    QuizData.resultMsg = "Not have Question in Placement test for use please contact @italt"
                Else

                    Dim Fullscore As String = dt(0)(0).ToString

                    cmdMsSql = cmdSQL(cn, "insert into tblquiz(quizId,testsetid,starttime,QuizMode,FullScore,QuizName) values(@QuizId,@TestsetId,getdate(),@QuizMode,@FullScore,@QuizName);
                                           insert into tblquizSession(quizid, studentid)values(@QuizId,@stdId);
                                           insert into tblquizQuestion select newid(),@QuizId,questionId,ROW_NUMBER() over (order by newid()),1,getdate() 
                                           from tbltestsetquestiondetail tsqd inner join tbltestsetquestionset tsqs on tsqd.tsqsid = tsqs.tsqsid
                                           where tsqd.isactive = 1 and tsqs.isactive = 1 and tsqs.testsetid = @TestsetId;
                                           insert into tblQuizAnswer select newid(),@QuizId,qq.QuestionId,AnswerId,
                                           ROW_NUMBER() OVER(PARTITION BY qq.questionId ORDER BY newid()),1,getdate() 
                                           from tblAnswer a inner join tblQuizQuestion qq on a.QuestionId = qq.QuestionId where a.IsActive = 1 and qq.quizid = @QuizId;
                                           select count(QuestionId) as QuestionAmount from tblQuizQuestion where QuizId = @QuizId"
                                      )

                    With cmdMsSql
                        .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = NewQuizId.ToLower
                        .Parameters.Add("@FullScore", SqlDbType.VarChar).Value = Fullscore
                        .Parameters.Add("@TestsetId", SqlDbType.VarChar).Value = QuizData.TestsetId.ToLower
                        .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId").ToString.ToLower
                        .Parameters.Add("@QuizMode", SqlDbType.VarChar).Value = QuizData.QuizMode
                        .Parameters.Add("@QuizName", SqlDbType.VarChar).Value = QuizData.QuizName
                    End With

                    dt = getDataTable(cmdMsSql)

                    QuizData.QuizId = NewQuizId.ToLower
                    QuizData.FullScore = Fullscore
                    QuizData.QuestionAmount = dt(0)(0)
                    QuizData.resultType = "success"

                    Session("QuizId") = NewQuizId
                    Session("QuestionAmount") = dt(0)(0)
                    Session("QuizMode") = QuizData.QuizMode
                    Session("QuestionNo") = 1
                    Session("QuizState") = Nothing
                    QuizData = Nothing
                End If
            Catch ex As Exception
                QuizData.resultType = "error"
                QuizData.resultMsg = ex.Message
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return QuizData
        End Function
        '20240805 -- เพิ่มการดึงไฟล์เสียงแบบ Slow และคำอธิบาย
        '20240820 -- เพิ่มการดึงไฟล์เสียงคำตอบ
        Function GetQuestionAndAnswer()
            Dim objList As New clsItemQAndA()
            Dim L1 As New List(Of clsItemQAndA)

            If Session("QuizId") Is Nothing Then
                objList.ItemStatus = "sessionExpired"
                L1.Add(objList)
                objList = Nothing
                Return Json(L1, JsonRequestBehavior.AllowGet)
            End If

            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dtQuestion As DataTable, dtAnswer As DataTable
            Dim QuizId As String = Session("QuizId")
            Dim QuestionNo As String = 1

            Dim ActionType = Request.Form("ActionType")

            Try
                If ActionType = "select" Then
                    Session("QuestionNo") = Request.Form("QuestionNo")
                Else
                    If Session("QuestionNo") IsNot Nothing Then
                        If ActionType <> "undefined" Then
                            If ActionType = "next" Then
                                Session("QuestionNo") = CInt(Session("QuestionNo")) + 1
                            Else
                                Session("QuestionNo") = CInt(Session("QuestionNo")) - 1
                            End If
                        End If
                    Else
                        Session("QuestionNo") = 1
                    End If
                End If

                QuestionNo = Session("QuestionNo")



                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "select qq.QQNo,qq.QuestionId,q.QuestionName_Quiz,q.QSetId,q.QuestionExpain_Quiz from tblQuestion q inner join tblQuizQuestion qq 
                                        on q.QuestionId = qq.QuestionId where quizid = @QuizId and QQNo = @QuestionNo")
                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = QuizId
                    .Parameters.Add("@QuestionNo", SqlDbType.VarChar).Value = QuestionNo
                End With

                dtQuestion = getDataTable(cmdMsSql)

                Dim QuestionId As String = dtQuestion(0)("QuestionId").ToString
                Dim QSetId As String = dtQuestion(0)("QSetId").ToString

                SaveQuizScore(QuestionId)

                Dim QExplain As String = dtQuestion(0)("QuestionExpain_Quiz").ToString
                QExplain = QExplain.Replace("___MODULE_URL___", GenFilePath(QSetId))
                QExplain = "<br><div class=""ExplainQ ui-hide"">" & QExplain & "</div>"

                Dim QName As String = dtQuestion(0)("QuestionName_Quiz").ToString
                QName = QName.Replace("___MODULE_URL___", GenFilePath(QSetId))
                QName = "<div class=""fistflexdiv"">" & dtQuestion(0)("QQNo").ToString & ".</div><div><div class='QName'>" & QName & "<br /><br /></div>" & QExplain & "</div>"

                Dim objListQuestion As New clsItemQAndA()
                objListQuestion.ItemType = "1"
                objListQuestion.ItemId = dtQuestion(0)("QuestionId").ToString
                objListQuestion.Itemtxt = QName

                '------------------Get MultimediaFile----------------------
                Dim FullPath As String

                FullPath = GetFullPath(QSetId).Replace("\", "/")

                cmdMsSql = cmdSQL(cn, "select MultimediaObjId,MfileName,MFileExplain from tblMultimediaObject where ReferenceId = @QuestionId and ReferenceType = 1 and MFileLevel = 1 and isactive = 1;")

                With cmdMsSql
                    .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = QuestionId
                End With

                Dim dtmulti As DataTable = getDataTable(cmdMsSql)

                If dtmulti.Rows.Count <> 0 Then

                    Dim FPath As String = "../file" & FullPath & "/" & dtmulti(0)("MfileName").ToString

                    objListQuestion.multiname = dtmulti(0)("MultimediaObjId").ToString
                    objListQuestion.multipath = FPath

                    If dtmulti.Rows(0)("MFileExplain").ToString <> "" Then
                        objListQuestion.multitxt = dtmulti.Rows(0)("MFileExplain").ToString
                    End If
                End If

                cmdMsSql = cmdSQL(cn, "select MultimediaObjId,MfileName from tblMultimediaObject where ReferenceId = @QuestionId and ReferenceType = 1 and MFileLevel = 2 and isactive = 1;")

                With cmdMsSql
                    .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = QuestionId
                End With

                Dim dtmultiSlow As DataTable = getDataTable(cmdMsSql)

                If dtmultiSlow.Rows.Count <> 0 Then

                    Dim FPath As String = "../file" & FullPath & "/" & dtmulti(0)("MfileName").ToString

                    objListQuestion.multiSlowname = dtmulti(0)("MultimediaObjId").ToString
                    objListQuestion.multiSlowpath = FPath
                End If


                If QuestionNo = 1 Then
                    objListQuestion.ItemStatus = "first"
                ElseIf QuestionNo = 2 Then
                    objListQuestion.ItemStatus = "second"
                ElseIf QuestionNo = Session("QuestionAmount") Then
                    objListQuestion.ItemStatus = "last"
                ElseIf QuestionNo = CInt(Session("QuestionAmount")) - 1 Then
                    objListQuestion.ItemStatus = "beforelast"
                End If

                L1.Add(objListQuestion)

                objListQuestion = Nothing

                '----Answer----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                '20240712 -- แก้ Query สำหรับแสดงเฉลย
                cmdMsSql = cmdSQL(cn, "Select qa.qano,qa.AnswerId,a.AnswerNameQuiz,qs.AnswerId as UserAnswered,qs.Score as UserScore,a.AnswerExpainQuiz, a.AnswerScore
                                        from tblQuizAnswer QA inner join tblAnswer A on qa.AnswerId = a.AnswerId  
                                        left join tblQuizScore qs on qa.quizid = qs.quizid and qa.QuestionId = qs.QuestionId 
                                        where qa.QuestionId = @QuestionId and Qa.QuizId = @QuizId order by QANo")

                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = QuizId
                    .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = QuestionId
                End With

                dtAnswer = getDataTable(cmdMsSql)


                Dim AnsHtml As String = ""

                Dim ArrChoicetxt() As String = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j"}

                For i = 0 To dtAnswer.Rows.Count - 1
                    Dim objListAnswer As New clsItemQAndA()
                    Dim IsAnswered As String = ""
                    Dim UserAns As String = ""
                    Dim divName As String = ""
                    If Session("QuizState") IsNot Nothing Then
                        If CInt(dtAnswer(i)("AnswerScore")) > 0 Then
                            IsAnswered = "RightAns"
                        End If

                        If (dtAnswer(i)("AnswerId").ToString = dtAnswer(i)("UserAnswered").ToString) AndAlso CInt(dtAnswer(i)("UserScore")) <= 0 Then
                            IsAnswered = "WrongAns"
                        End If
                        divName = "Ans"
                        If dtAnswer(i)("AnswerId").ToString = dtAnswer(i)("UserAnswered").ToString Then
                            UserAns = "UserAns"
                        End If
                    Else
                        If dtAnswer(i)("AnswerId").ToString = dtAnswer(i)("UserAnswered").ToString Then
                            IsAnswered = "Answered"
                        End If
                    End If

                    Dim Aname As String = dtAnswer(i)("AnswerNameQuiz").ToString
                    Aname = Aname.Replace("___MODULE_URL___", GenFilePath(QSetId))

                    '20240820 -- Answer Multimedia File
                    cmdMsSql = cmdSQL(cn, "select MultimediaObjId,MfileName,MFileExplain from tblMultimediaObject where ReferenceId = @AnswerId and ReferenceType = 1 and MFileLevel = 1 and isactive = 1;")

                    With cmdMsSql
                        .Parameters.Add("@AnswerId", SqlDbType.VarChar).Value = dtAnswer(i)("AnswerId").ToString
                    End With

                    Dim dtmultiAns As DataTable = getDataTable(cmdMsSql)

                    If dtmultiAns.Rows.Count <> 0 Then

                        Dim FPath As String = "../file" & FullPath & "/" & dtmultiAns(0)("MfileName").ToString
                        objListAnswer.multiAnsname = dtmultiAns(0)("MultimediaObjId").ToString
                        objListAnswer.multiAnspath = FPath

                        If dtmultiAns.Rows(0)("MFileExplain").ToString <> "" Then
                            objListAnswer.multiAnstxt = dtmultiAns.Rows(0)("MFileExplain").ToString
                        End If
                    End If

                    cmdMsSql = cmdSQL(cn, "select MultimediaObjId,MfileName from tblMultimediaObject where ReferenceId = @AnswerId and ReferenceType = 1 and MFileLevel = 2 and isactive = 1;")

                    With cmdMsSql
                        .Parameters.Add("@AnswerId", SqlDbType.VarChar).Value = dtAnswer(i)("AnswerId").ToString
                    End With

                    Dim dtmultiAnsSlow As DataTable = getDataTable(cmdMsSql)

                    If dtmultiAnsSlow.Rows.Count <> 0 Then

                        Dim FPath As String = "../file" & FullPath & "/" & dtmultiAns(0)("MfileName").ToString

                        objListAnswer.multiAnsSlowname = dtmultiAns(0)("MultimediaObjId").ToString
                        objListAnswer.multiAnsSlowpath = FPath
                    End If

                    Dim AExplain As String = dtAnswer(i)("AnswerExpainQuiz").ToString
                    AExplain = AExplain.Replace("___MODULE_URL___", GenFilePath(QSetId))
                    AExplain = "<br><div class=""ExplainQ ui-hide"">" & AExplain & "</div>"

                    Aname = Aname & AExplain

                    '20240712 -- ปรับการแสดงเฉลยคำตอบ
                    If i Mod 2 = 0 Then
                        AnsHtml &= "<div Class=""divAnswerRow"">
                                    <div Class=""divAnswerbar" & divName & " flexdiv Left " & IsAnswered & """ QId=""" & QuestionId & """ AnsId=""" & dtAnswer(i)("AnswerId").ToString & """>" &
                          "<div class=""fistflexdiv " & UserAns & """>" & ArrChoicetxt(i) & ".</div><div class='AName" & dtAnswer(i)("AnswerId").ToString & "'>" & Aname & "</div></div>"
                    Else
                        AnsHtml &= "<div Class=""divAnswerbar" & divName & " flexdiv Right " & IsAnswered & """ QId=""" & QuestionId & """ AnsId=""" & dtAnswer(i)("AnswerId").ToString & """>" &
                            "<div class=""fistflexdiv " & UserAns & """> " & ArrChoicetxt(i) & ".</div><div class='AName" & dtAnswer(i)("AnswerId").ToString & "'>" & Aname & "</div></div></div>"
                    End If

                    objListAnswer.ItemType = "2"
                    objListAnswer.Itemtxt = AnsHtml
                    objListAnswer.ItemId = dtAnswer(i)("AnswerId").ToString
                    cmdMsSql = cmdSQL(cn, "select MultimediaObjId,MfileName from tblMultimediaObject where ReferenceId = @AnswerId and ReferenceType = 1 and isactive = 1;")

                    With cmdMsSql
                        .Parameters.Add("@AnswerId", SqlDbType.VarChar).Value = dtAnswer(i)("AnswerId").ToString
                    End With

                    Dim dtAmulti As DataTable = getDataTable(cmdMsSql)

                    If dtAmulti.Rows.Count <> 0 Then

                        Dim FPath As String = "../file" & FullPath & "/" & dtAmulti(0)("MfileName").ToString

                        objListAnswer.multiname = dtAmulti(0)("MultimediaObjId").ToString
                        objListAnswer.multipath = FPath
                    End If
                    L1.Add(objListAnswer)
                    objListAnswer = Nothing
                Next
            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dtQuestion IsNot Nothing Then dtQuestion.Dispose() : dtQuestion = Nothing
                If dtAnswer IsNot Nothing Then dtAnswer.Dispose() : dtAnswer = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try

            Return Json(L1, JsonRequestBehavior.AllowGet)

        End Function
        Function CheckAndCreatePlacementTest()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim LevelId As String, PMTNum As String = "", ResultMsg As String = ""

            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                'check PMTNum

                cmdMsSql = cmdSQL(cn, "Select q.PercentScore from tblQuizSession qss inner join tblquiz q On q.QuizId = qss.QuizId 
                                        where qss.StudentId = @StudentId and q.QuizMode = 1")
                With cmdMsSql
                    .Parameters.Add("@StudentId", SqlDbType.VarChar).Value = Session("StudentId")
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count = 0 Then
                    LevelId = "14A28F3D-1AFF-429D-B7A1-927A28E010BD"
                    PMTNum = 1
                ElseIf dt.Rows.Count = 1 Then
                    If dt(0)("PercentScore") >= 80 Then
                        'test Advanced
                        LevelId = "6736D029-6B78-4570-9DBB-991217DA8FEE"
                    Else
                        'test Foundation
                        LevelId = "E5DBFA06-C4CE-4CE2-9F47-60E9CB99A38C"
                    End If
                    PMTNum = 2
                Else
                    ResultMsg = "success3"
                    Return ResultMsg
                End If

                cmdMsSql = cmdSQL(cn, "Select top 1 testsetId from tbltestset where isPlacementTest = 1 And levelid = @LevelId order by newid()")

                With cmdMsSql
                    .Parameters.Add("@LevelId", SqlDbType.VarChar).Value = LevelId
                    .ExecuteNonQuery()
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count = 0 Then
                    ResultMsg = "Dont have testset from placementtest!"
                Else
                    Dim QuizData As New clsQuizData

                    QuizData.QuizMode = "1"
                    QuizData.TestsetId = dt(0)(0).ToString
                    QuizData.QuizName = "Placement Test"
                    CreateNewQuiz(QuizData)

                    If QuizData.resultType = "success" Then
                        cmdMsSql = cmdSQL(cn, "Insert into tblPlacementTest(quizid,pmtnum,levelid)values (@QuizId,@PMTNum,@LevelId);")
                        With cmdMsSql
                            .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                            .Parameters.Add("@LevelId", SqlDbType.VarChar).Value = LevelId
                            .Parameters.Add("@PMTNum", SqlDbType.VarChar).Value = PMTNum
                            .ExecuteNonQuery()
                        End With

                    End If

                    ResultMsg = "success" & PMTNum

                End If
            Catch ex As Exception
                ResultMsg = ex.Message
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return ResultMsg
        End Function
        Function UpdateStudentLevel(PercentScore As Integer)
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As New DataTable
            Dim objList As New clsMain()
            Dim LevelRegister As String, LevelRegisterNum As String, ResultStatus As String
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================


                cmdMsSql = cmdSQL(cn, "Select LevelId from tblPlacementTest where PMTNum = 2;")

                dt = getDataTable(cmdMsSql)
                Dim LevelId As String = dt(0)(0).ToString

                If LevelId.ToUpper = "6736D029-6B78-4570-9DBB-991217DA8FEE" Then
                    'Advanced
                    If PercentScore >= 80 Then
                        'Register Advances
                        LevelRegister = "6736D029-6B78-4570-9DBB-991217DA8FEE"
                        LevelRegisterNum = 5
                    ElseIf PercentScore >= 60 And PercentScore < 80 Then
                        'Register Upper-Intermediate
                        LevelRegister = "2E0FFC04-BCEE-45BE-9C0C-B40742523F43"
                        LevelRegisterNum = 4
                    ElseIf PercentScore < 60 Then
                        'Register Intermediate
                        LevelRegister = "14A28F3D-1AFF-429D-B7A1-927A28E010BD"
                        LevelRegisterNum = 3
                    End If
                ElseIf LevelId.ToUpper = "E5DBFA06-C4CE-4CE2-9F47-60E9CB99A38C" Then
                    'Foundation
                    If PercentScore >= 80 Then
                        'Register Intermediate
                        LevelRegister = "14A28F3D-1AFF-429D-B7A1-927A28E010BD"
                        LevelRegisterNum = 3
                    ElseIf PercentScore >= 60 And PercentScore < 80 Then
                        'Register Pre-Intermediate
                        LevelRegister = "DB95E7F8-7BF3-468D-AD9E-0AAF1B328D45"
                        LevelRegisterNum = 2
                    ElseIf PercentScore < 60 Then
                        'Register Foundation
                        LevelRegister = "E5DBFA06-C4CE-4CE2-9F47-60E9CB99A38C"
                        LevelRegisterNum = 1
                    End If
                End If


                cmdMsSql = cmdSQL(cn, "Update tblStudentLevel Set IsActive = 0 where StudentId = @StudentId;
                                       Insert Into tblStudentLevel(StudentId,LevelId) values(@StudentId,@LevelRegister);")
                With cmdMsSql
                    .Parameters.Add("@StudentId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                    .Parameters.Add("@LevelRegister", SqlDbType.VarChar).Value = LevelRegister.ToString
                    .ExecuteNonQuery()
                End With

                ResultStatus = LevelRegisterNum

            Catch ex As Exception
                ResultStatus = "error"
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return ResultStatus
        End Function
        Function SaveQuizScore(QuestionId As String)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dtScore As DataTable
            Dim Result As Boolean = True
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, " Select QuizScoreId from tblQuizScore  where QuizId = @QuizId and QuestionId = @QuestionId;")

                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                    .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = QuestionId
                    .ExecuteNonQuery()
                End With

                dtScore = getDataTable(cmdMsSql)

                If dtScore.Rows.Count = 0 Then
                    cmdMsSql = cmdSQL(cn, "insert into tblQuizScore(QuizId,QuestionId,FirstResponse) values (@QuizId,@QuestionId,getdate());")
                    With cmdMsSql
                        .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                        .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = QuestionId
                        .ExecuteNonQuery()
                    End With
                End If

            Catch ex As Exception
                Result = False
            Finally
                If dtScore IsNot Nothing Then dtScore.Dispose() : dtScore = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Result
        End Function
        Function GenFilePath(ByVal QSetId As String) As String
            Dim rootPath As String = "../file/"
            Dim filePath As String
            filePath = QSetId.Substring(0, 1) + "/" + QSetId.Substring(1, 1) + "/" + QSetId.Substring(2, 1) +
            "/" + QSetId.Substring(3, 1) + "/" + QSetId.Substring(4, 1) + "/" + QSetId.Substring(5, 1) +
            "/" + QSetId.Substring(6, 1) + "/" + QSetId.Substring(7, 1) + "/"
            filePath = filePath + "{" + QSetId + "}/"
            Return rootPath + filePath
        End Function
        Private Function GetMultiControl(QSetId As String, QuestionId As String)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()

            ' ================ Check Permission ================
            cn = New SqlConnection(sqlCon("Wetest"))
            If cn.State = 0 Then cn.Open()
            ' ==================================================

            Dim FullPath As String

            FullPath = GetFullPath(QSetId).Replace("\", "/")

            cmdMsSql = cmdSQL(cn, "select MultimediaObjId,MfileName from tblMultimediaObject where ReferenceId = @QuestionId and ReferenceType = 1 and isactive = 1;")

            With cmdMsSql
                .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = QuestionId
            End With

            dt = getDataTable(cmdMsSql)
            Return dt

        End Function
        Private Function GetFullPath(qsetId As String) As String

            Dim FullPath As String
            Dim QuestionSetId As String = qsetId
            Dim PathProJect As String = Server.MapPath("../")  '"D:\Development\QuickTest\Source\QuickTest\QuickTest" + "\"
            'ทำการสร้าง Array เพื่อมาเก็บ QsetId ที่ตัดมาทีละหลักจำนวนทั้งหมด 8 หลัก แล้วต่อด้วย QsetId เต็มๆอีกทีนึง
            Dim ArrayCreateFolder As New ArrayList
            ArrayCreateFolder.Add(QuestionSetId.Substring(0, 1))
            ArrayCreateFolder.Add(QuestionSetId.Substring(1, 1))
            ArrayCreateFolder.Add(QuestionSetId.Substring(2, 1))
            ArrayCreateFolder.Add(QuestionSetId.Substring(3, 1))
            ArrayCreateFolder.Add(QuestionSetId.Substring(4, 1))
            ArrayCreateFolder.Add(QuestionSetId.Substring(5, 1))
            ArrayCreateFolder.Add(QuestionSetId.Substring(6, 1))
            ArrayCreateFolder.Add(QuestionSetId.Substring(7, 1))
            ArrayCreateFolder.Add("{" & QuestionSetId & "}")
            Dim path As String = PathProJect
            'loop เพื่อทำการต่อสตริง "\" เข้าไปหลังจาก QsetId ที่ตัดมาทีละหลัก , เงื่อนไขการจบ loop วนจนครบ Array คือ 9 รอบ
            For Each i In ArrayCreateFolder
                path &= i & "\"
            Next
            'Dim BackSlash As String = "\"
            'Dim Slash As String = "/"
            'Dim ReplaceSlash As String = Replace(path, BackSlash, Slash)
            Dim ReplaceSlash As String = path
            Dim RootUrl As String = "\"
            'ดึง Path ปัจจุบันขึ้นมา เช่น "D:/Development/QuickTest/Source/QuickTest/QuickTest/"
            Dim PathNotUse As String = Server.MapPath("../")
            'ทำการนำมา Replace โดยตัดส่วนหน้าทั้งหมดทิ้งให้เหลือแต่ QsetId ที่เริ่มจากหลักแรกมา เช่น \9\5\6\8\f\6\3\7\{9568f637-3230-47b4-81e1-a04e45b75132}\
            Dim CompleteFullPath As String = Replace(ReplaceSlash, PathNotUse, RootUrl)
            'สุดท้ายตัด \ ตัวสุดท้ายออกไป
            FullPath = CompleteFullPath.Remove(CompleteFullPath.Length - 1, 1)
            Return FullPath
        End Function
        Function CheckAndUplevel()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As New DataTable
            Dim objList As New clsMain()
            Dim ResultStatus As String = ""
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                Dim PassExamScore As Integer = CInt(Getconfig("PassExamPercent"))

                cmdMsSql = cmdSQL(cn, "Select q.PercentScore,t.LevelId from tblquiz q inner join tbltestset t on q.TestSetId = t.TestsetId  where q.quizid = @QuizId")

                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId")
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count <> 0 Then

                    If CInt(dt(0)("PercentScore")) >= PassExamScore Then
                        cmdMsSql = cmdSQL(cn, "Insert Into tblStudentLevel(StudentId,LevelId) values(@StudentId,@LevelUp);
                                                select levelno from tbllevel where levelId = @LevelUp;")

                        With cmdMsSql
                            .Parameters.Add("@StudentId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                            .Parameters.Add("@LevelUp", SqlDbType.VarChar).Value = dt(0)("LevelId").ToString

                        End With

                        dt = getDataTable(cmdMsSql)

                        objList.dataType = "pass"
                        objList.errorMsg = dt.Rows(0)("levelno")
                    Else
                        cmdMsSql = cmdSQL(cn, "select levelno - 1 as levelno from tbllevel where levelId = @LevelUp;")

                        With cmdMsSql
                            .Parameters.Add("@LevelUp", SqlDbType.VarChar).Value = dt(0)("LevelId").ToString

                        End With

                        dt = getDataTable(cmdMsSql)

                        objList.dataType = "notpass"
                        objList.errorMsg = dt.Rows(0)("levelno")
                    End If


                End If

            Catch ex As Exception
                ResultStatus = ""
                objList.dataType = "error"
                objList.errorMsg = ex.ToString
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return objList
        End Function


#End Region

#Region "User"
        Function User() As ActionResult
            Return View()
        End Function

        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckUserLogin()
            Dim L1 As New List(Of clsStudentData)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsStudentData()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select s.StudentId from tblStudent s where s.Username = @Username and s.Password = @Password  and s.IsActive = 1;")

                With cmdMsSql
                    .Parameters.Add("@Username", SqlDbType.VarChar).Value = Request.Form("Username")
                    .Parameters.Add("@Password", SqlDbType.VarChar).Value = oneWayKN(Request.Form("Password"))
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count = 0 Then
                    objList.Result = "not"
                    objList.Msg = "Username or Password is wrong !<br><br>Please try again or contact @Italt."
                Else
                    Session("studentid") = dt.Rows(0)(0).ToString
                    objList = GetUserData(Session("studentid"))
                End If

                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.Result = "error"
                objList.Msg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240716 -- แก้ Query และเก็บค่าลง objList เพื่อดึงข้อมูล Goal ที่ตั้งไว้
        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckLoginStatus()
            Dim L1 As New List(Of clsStudentData)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsStudentData()
            Try
                objList = GetUserData(Session("studentid"))

                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.Result = "error"
                objList.Msg = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function CreateMockUpExam()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                Dim StdId As String = Session("StudentId").ToString

                cmdMsSql = cmdSQL(cn, "select top 1 l.levelno + 1 as LevelNuum from tblstudentlevel sl inner join tbllevel l on sl.LevelId = l.LevelId where StudentId = @StdId order by sl.lastupdate desc;")
                With cmdMsSql
                    .Parameters.Add("@StdId", SqlDbType.VarChar).Value = StdId
                End With

                dt = getDataTable(cmdMsSql)

                cmdMsSql = cmdSQL(cn, "select top 1 t.TestsetId,t.levelId from tbltestset t inner join tblLevel l on t.LevelId = l.LevelId where IsExam = 1 and t.IsActive = 1 and Levelno = @LevelNuum order by newid();")
                With cmdMsSql
                    .Parameters.Add("@LevelNuum", SqlDbType.VarChar).Value = dt(0)("LevelNuum").ToString
                End With

                dt = getDataTable(cmdMsSql)

                Dim QuizData As New clsQuizData

                QuizData.QuizMode = "3"
                QuizData.TestsetId = dt(0)("TestsetId").ToString
                QuizData.QuizName = "Mockup Exam"
                CreateNewQuiz(QuizData)

                If QuizData.resultType = "success" Then
                    cmdMsSql = cmdSQL(cn, "insert into tblMockupExam(QuizId,LevelId)values(@QuizId,@LevelId);")
                    With cmdMsSql
                        .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                        .Parameters.Add("@LevelId", SqlDbType.VarChar).Value = dt(0)("levelId").ToString
                        .ExecuteNonQuery()
                    End With

                End If

                objList.dataType = "success"
                objList.errorMsg = ""
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240715 -- Save TotalGoal
        '20240819 -- ปรับการเก็บข้อมูล Goal ให้เก็บวันที่เริ่มของแต่ละ Goal เพิ่ม (ปิด skill ที่ยังไม่มีไปก่อน)
        <AcceptVerbs(HttpVerbs.Post)>
        Function SaveTotalGoal()
            Dim L1 As New List(Of clsStudentData)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsStudentData()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                Dim StdId As String = Session("StudentId").ToString
                Dim GoalType As String = Request.Form("GoalType").ToLower
                Dim SkillId As String = Request.Form("SkillId").ToLower
                Select Case GoalType
                    Case "total"
                        cmdMsSql = cmdSQL(cn, "Update tblstudentGoal set isActive = 0 ,LastUpdate = getdate() where StudentId = @StdId and GoalType = 1;
                                       Insert into tblstudentGoal(StudentId,GoalType,StartDate,EndDate)values(@StdId,1,getdate(),@GoalDate);
                                       select datediff(day,getdate(),@GoalDate);")
                    'Case "reading"
                    '    cmdMsSql = cmdSQL(cn, "Update tblstudentGoal set isActive = 0 ,LastUpdate = getdate() where StudentId = @StdId
                    '                            and skillIKd = ;
                    '                   Insert into tblstudentGoal(StudentId,GoalType,StartDate,EndDate)values(@StdId,1,getdate(),@GoalDate);
                    '                   select datediff(day,getdate(),@GoalDate);")
                    'Case "listening"
                    '    cmdMsSql = cmdSQL(cn, "Update tblstudentGoal set ListeningGoal = @GoalDate ,LastUpdate = getdate() where StudentId = @StdId and isActive = 1;
                    '                        select datediff(day,getdate(),@GoalDate);")
                    Case "vocabulary"
                        cmdMsSql = cmdSQL(cn, "Update tblstudentGoal set isActive = 0 ,LastUpdate = getdate() where StudentId = @StdId and GoalType = 2 and skillId = @SkillId;
                                                Insert into tblstudentGoal(StudentId,GoalType,StartDate,EndDate,skillId)
                                                values(@StdId,2,getdate(),@GoalDate,@SkillId);
                                                select datediff(day,getdate(),@GoalDate);")
                    Case "grammar"
                        cmdMsSql = cmdSQL(cn, "Update tblstudentGoal set isActive = 0 ,LastUpdate = getdate() where StudentId = @StdId and GoalType = 2 and skillId = @SkillId;
                                                Insert into tblstudentGoal(StudentId,GoalType,StartDate,EndDate,skillId)
                                                values(@StdId,2,getdate(),@GoalDate,@SkillId);
                                                select datediff(day,getdate(),@GoalDate);")
                        'Case "situation"
                        '    cmdMsSql = cmdSQL(cn, "Update tblstudentGoal set SituationGoal = @GoalDate ,LastUpdate = getdate() where StudentId = @StdId and isActive = 1;
                        '                        select datediff(day,getdate(),@GoalDate);")
                End Select

                With cmdMsSql
                    .Parameters.Add("@StdId", SqlDbType.VarChar).Value = StdId
                    .Parameters.Add("@GoalDate", SqlDbType.VarChar).Value = Request.Form("selectedGoalDate")
                    .Parameters.Add("@SkillId", SqlDbType.VarChar).Value = SkillId
                End With

                dt = getDataTable(cmdMsSql)

                objList.Result = "success"
                objList.TotalGoal = Request.Form("formatSelectedGoalDate")
                objList.TotalGoalAmount = dt.Rows(0)(0).ToString
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.Result = "error"
                objList.Msg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240716 -- Clear Goal
        <AcceptVerbs(HttpVerbs.Post)>
        Function ClearGoalDate()
            Dim L1 As New List(Of clsStudentData)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsStudentData()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                Dim StdId As String = Session("StudentId").ToString

                cmdMsSql = cmdSQL(cn, "Update tblstudentGoal set isActive = 0 ,LastUpdate = getdate() where StudentId = @StdId;")
                With cmdMsSql
                    .Parameters.Add("@StdId", SqlDbType.VarChar).Value = StdId
                    .ExecuteNonQuery()
                End With

                objList.Result = "success"
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.Result = "error"
                objList.Msg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240723 -- Logout
        <AcceptVerbs(HttpVerbs.Post)>
        Function Logout()
            Dim L1 As New List(Of clsStudentData)
            Dim objList As New clsStudentData()
            Try
                Session.Contents.RemoveAll()
                objList.Result = "success"
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.Result = "error"
                objList.Msg = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240723 -- Delete Account
        <AcceptVerbs(HttpVerbs.Post)>
        Function DeleteAccount()
            Dim L1 As New List(Of clsStudentData)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsStudentData()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                Dim StdId As String = Session("StudentId").ToString

                cmdMsSql = cmdSQL(cn, "Update tblstudent set isActive = 0 ,LastUpdate = getdate() where StudentId = @StdId;")
                With cmdMsSql
                    .Parameters.Add("@StdId", SqlDbType.VarChar).Value = StdId
                    .ExecuteNonQuery()
                End With

                objList.Result = "success"
                L1.Add(objList)
                objList = Nothing
                Session("studentId") = Nothing
            Catch ex As Exception
                objList.Result = "error"
                objList.Msg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240723 -- Delete Account
        <AcceptVerbs(HttpVerbs.Post)>
        Function SetEditUserMode()
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Try
                Session("EditUserData") = True
                Session("RefillKey") = Nothing
                objList.dataType = "success"
                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240801 -- CheckExamAgain
        '20240805 -- CheckExamAgain เพิ่มเวลาเล่นได้อีกครั้ง
        '20240826 -- เพิ่ม Config Highest Level และตรวจสอบ
        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckExamAgain()

            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Try

                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                Dim HighestLevel As Integer = CInt(Getconfig("HighestLevel"))
                cmdMsSql = cmdSQL(cn, "Select top 1 LevelNo from tblStudentLevel sl inner join tblLevel l On sl.levelId = l.levelId 
                                        where studentid = @StudentID order by sl.LastUpdate desc")
                With cmdMsSql
                    .Parameters.Add("@StudentID", SqlDbType.VarChar).Value = Session("StudentID")
                    .ExecuteNonQuery()
                End With

                dt = getDataTable(cmdMsSql)

                If CInt(dt.Rows(0)("LevelNo")) < HighestLevel Then
                    Dim ExamAgainDay = Getconfig("ExamAgainDay")

                    cmdMsSql = cmdSQL(cn, "select top 1  datediff(day,StartTime,getdate()) as LastExamDayAmount,CONVERT (varchar(10), DATEADD(day,@exaAmount, StartTime), 103) as  DateNextime from tblQuiz q 
                                        inner join tblquizsession qs on q.quizid = qs.quizId 
                                        where q.QuizMode = 3 and qs.StudentId = @StudentID order by q.lastupdate desc")
                    With cmdMsSql
                        .Parameters.Add("@StudentID", SqlDbType.VarChar).Value = Session("StudentID")
                        .Parameters.Add("@exaAmount", SqlDbType.Int).Value = CInt(ExamAgainDay)
                        .ExecuteNonQuery()
                    End With

                    dt = getDataTable(cmdMsSql)

                    If dt.Rows.Count = 0 Or ExamAgainDay = 0 Then
                        objList.dataType = "ok"
                        objList.errorMsg = "Do you want to start exam for up level ?"
                    Else
                        If (CInt(dt.Rows(0)("LastExamDayAmount")) > ExamAgainDay) Then
                            objList.dataType = "ok"
                            objList.errorMsg = "Do you want to start exam for up level ?"
                        Else
                            objList.dataType = "no"
                            objList.errorMsg = "You can't make Mock Up Exam. Plase try again at " & dt.Rows(0)("DateNextime") & "."
                        End If
                    End If
                Else
                    objList.dataType = "no"
                    objList.errorMsg = "Congratulations! You are at the highest level"
                End If
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)

        End Function
        '20240814 -- ดึง Config จำนวนครั้งในการเล่นไฟล์เสียง
        '20240913 -- ปรับให้ดึง Config ต่างๆ ที่ต้องใช้ตอนเริ่ม Quiz
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetQuizConfigVal()
            Dim L1 As New List(Of clsQuizConfigVal)
            Dim objList As New clsQuizConfigVal()
            Try
                Dim MultimediaAmount As Integer = CInt(Getconfig("MultimediaAmount"))
                Dim MultimediaSlowAmount As Integer = CInt(Getconfig("MultimediaSlowAmount"))
                Dim IsTest As Integer = CInt(Getconfig("IsTest"))

                objList.Result = "success"
                objList.MultiAmount = MultimediaAmount
                objList.MultiSlowAmount = MultimediaSlowAmount
                objList.IsTest = IsTest
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.Result = "error"
                objList.ResultTxt = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '2024816 -- SetRefillKeyMode
        <AcceptVerbs(HttpVerbs.Post)>
        Function SetRefillKeyMode()
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Try
                Session("RefillKey") = True

                objList.dataType = "success"
                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240828 -- Function Get Goal Data
        '20240903 -- ปรับ Query เมื่อหมดเวลาที่ตั้ง Goal ไว้
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetGoalData()
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, dtSkill As DataTable
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsStudentData()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                'Get GoalDate , GoalAmount, DatePercent


                cmdMsSql = cmdSQL(cn, "select GoalType,SkillId,CONVERT(varchar(10),enddate, 103) as GoalDate
                                        ,case when datediff(day,getdate(),Enddate) < 0 then 0 else datediff(day,getdate(),Enddate) end as GoalAmount
                                        ,case when enddate < getdate() then 100 
                                        else (100 - ((DATEDIFF(DAY,getdate(),enddate)*100) / DATEDIFF(DAY,startdate,enddate))) end as DatePercent 
                                        from tblstudentGoal where StudentId = @stdid and isactive = 1;")
                With cmdMsSql
                    .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("StudentId").ToString
                End With

                dtSkill = getDataTable(cmdMsSql)

                If dtSkill.Rows.Count <> 0 Then
                    For i = 0 To dtSkill.Rows.Count - 1
                        Select Case dtSkill(i)("GoalType").ToString
                            Case "1"
                                objList.TotalGoal = dtSkill(i)("GoalDate").ToString
                                objList.TotalGoalAmount = dtSkill.Rows(i)("GoalAmount").ToString
                                objList.TotalDatePercent = dtSkill.Rows(i)("DatePercent").ToString & "%"

                                Dim TotalScore As Integer
                                Dim UserScore As Integer

                                cmdMsSql = cmdSQL(cn, "Select Case When sum(q.totalscore) Is null Then 0 Else sum(q.totalscore) End As UserScore 
                                            from tblQuiz q inner join tblquizSession qs on q.quizId = qs.QuizId 
                                            inner join tblstudentGoal sg on qs.studentId = sg.StudentId
                                            where qs.StudentId = @stdid and q.starttime between sg.Startdate and sg.enddate and sg.isactive = 1")

                                With cmdMsSql
                                    .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("studentid").ToString
                                End With

                                dt = getDataTable(cmdMsSql)

                                If dt.Rows.Count <> 0 AndAlso dt.Rows(0)("UserScore") <> 0 Then

                                    UserScore = CInt(dt.Rows(0)("UserScore"))

                                    cmdMsSql = cmdSQL(cn, "select sum(a.AnswerScore) as TotalScore 
                                                            from tbltestset t inner join tblTestSetQuestionSet ts on t.TestSetId = ts.TestSetId 
                                                            inner join tblTestSetQuestionDetail td on ts.TSQSId = td.TSQSId inner join tblAnswer a on td.QuestionId = a.QuestionId
                                                            inner join tblQuestionEvaluationIndexItem qei on a.QuestionId = qei.Question_Id 
                                                            inner join tblEvaluationIndex ei on qei.ei_id = ei.ei_id
                                                            inner join tblSkill sk on ei.Parent_Id = sk.EI_Id and sk.isactive = 1
                                                            where t.LevelId = (select LevelId from tblstudentLevel where isActive = 1 and StudentId = @stdid) 
                                                            and t.IsActive = 1 and ts.IsActive = 1 and td.IsActive = 1 and td.IsActive = 1 and a.IsActive = 1 and t.IsPractice = 1;")

                                    With cmdMsSql
                                        .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("studentid").ToString
                                    End With

                                    dt = getDataTable(cmdMsSql)

                                    TotalScore = CInt(dt.Rows(0)("TotalScore"))

                                    Dim SPercent As Decimal = ((UserScore * 100) / TotalScore)
                                    If SPercent <> 0 Then
                                        SPercent = Format(SPercent, "N2")
                                    End If

                                    objList.TotalScorePercent = SPercent.ToString & "%"
                                Else
                                    objList.TotalScorePercent = "0%"
                                End If

                            Case "2"
                                'Wait For Edit เพิ่ม Skill ที่ยังไม่มี
                                Select Case dtSkill(i)("SkillId").ToString.ToLower
                                    Case "31667BAB-89FF-43B3-806F-174774C8DFBF".ToLower
                                        objList.VocabGoal = dtSkill(i)("GoalDate").ToString
                                        objList.VocabGoalAmount = dtSkill(i)("GoalAmount").ToString
                                        objList.VocabDatePercent = dtSkill.Rows(i)("DatePercent").ToString & "%"
                                        objList.VocabScorePercent = GetskillScorePercent("31667BAB-89FF-43B3-806F-174774C8DFBF", cn)
                                    Case "5BBD801D-610F-40EB-89CB-5957D05C4A0B".ToLower
                                        objList.GrammarGoal = dtSkill(i)("GoalDate").ToString
                                        objList.GrammarGoalAmount = dtSkill(i)("GoalAmount").ToString
                                        objList.GrammarDatePercent = dtSkill.Rows(i)("DatePercent").ToString & "%"
                                        objList.GrammarScorePercent = GetskillScorePercent("5BBD801D-610F-40EB-89CB-5957D05C4A0B", cn)
                                End Select
                        End Select
                    Next

                    If objList.VocabGoal Is Nothing Then
                        objList.VocabGoal = ""
                        objList.VocabGoalAmount = ""
                        objList.VocabDatePercent = "0%"
                        objList.VocabScorePercent = "0%"
                    End If

                    If objList.GrammarGoal Is Nothing Then
                        objList.GrammarGoal = ""
                        objList.GrammarGoalAmount = ""
                        objList.GrammarDatePercent = "0%"
                        objList.GrammarScorePercent = "0%"
                    End If

                    objList.ReadingGoal = ""
                    objList.ReadingGoalAmount = ""
                    objList.ReadingDatePercent = "0%"
                    objList.ReadingScorePercent = "0%"

                    objList.SituationGoal = ""
                    objList.SituationGoalAmount = ""
                    objList.SituationDatePercent = "0%"
                    objList.SituationScorePercent = "0%"

                    objList.ListeningGoal = ""
                    objList.ListeningGoalAmount = ""
                    objList.ListeningDatePercent = "0%"
                    objList.ListeningScorePercent = "0%"
                Else

                    objList.TotalGoal = ""
                    objList.TotalGoalAmount = ""
                    objList.TotalDatePercent = ""

                    objList.ReadingGoal = ""
                    objList.ReadingGoalAmount = ""
                    objList.ReadingDatePercent = ""

                    objList.SituationGoal = ""
                    objList.SituationGoalAmount = ""
                    objList.SituationDatePercent = ""

                    objList.ListeningGoal = ""
                    objList.ListeningGoalAmount = ""
                    objList.ListeningDatePercent = ""

                    objList.GrammarGoal = ""
                    objList.GrammarGoalAmount = ""
                    objList.GrammarDatePercent = ""

                    objList.VocabGoal = ""
                    objList.VocabGoalAmount = ""
                    objList.VocabDatePercent = ""
                End If
                objList.Result = "ok"
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240722 -- เพิ่ม ExpiredDate
        '20240723 -- เพิ่ม User Data สำหรับแก้ไข
        '20240813 -- select จำนวนวันที่จะหมดอายุเพื่อเอาไปใช้กำหนด Max Date ของ calendar
        '20240819 -- ปรับวิธีการดึง Student Goal
        '20240822 -- ปรับวิธีการดึง Total Goal case เลยวันที่ตั้งค่าไว้
        '20240826 -- ปรับวิธีการตรวจสอบ Expired Date 
        '20240827 -- ปรับวิธีการคำนวน % Goal
        '20240902 -- ปรับการตรวจสอบ ExpiredDate
        '20240905 -- ปรับการตรวจสอบกรณียังไม่ได้ Update trial date
        Function GetUserData(stdId As String)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, dtSkill As DataTable
            Dim objList As New clsStudentData()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                If stdId Is Nothing Then
                    objList.Result = "sessionlost"
                Else
                    cmdMsSql = cmdSQL(cn, "select case when ExpiredDate is null then 0 
                                           when ExpiredDate < getdate() then 1 else 2 end as ExpiredStatus 
                                           from tblstudent s where s.StudentId = @stdid and s.isactive = 1;")
                    With cmdMsSql
                        .Parameters.Add("@stdid", SqlDbType.VarChar).Value = stdId
                    End With

                    dt = getDataTable(cmdMsSql)

                    If dt.Rows.Count = 0 Then
                        'Not Register
                        objList.Result = "not"
                    Else

                        Select Case CInt(dt.Rows(0)("ExpiredStatus"))
                            Case 0
                                objList.Result = "trial"
                                Session("RefillKey") = True
                            Case 1
                                objList.Result = "refill"
                                Session("RefillKey") = True
                            Case 2
                                objList.Result = "ok"
                        End Select

                        cmdMsSql = cmdSQL(cn, "select top 1 s.StudentId,Firstname,Surname,MobileNo,Email,Username,l.LevelShortName,CONVERT (varchar(10),s.ExpiredDate , 103) AS expiredDate
                                                ,DATEDIFF(DAY,getdate(),ExpiredDate) as ExpiredDateAmount from tblStudent s inner join tblStudentLevel sl on s.studentId = sl.StudentId 
                                                inner join tblLevel l on sl.levelId = l.LevelId where s.studentId = @stdid and s.IsActive = 1 and sl.IsActive = 1  
                                                order by sl.lastupdate desc;")
                        With cmdMsSql
                            .Parameters.Add("@stdid", SqlDbType.VarChar).Value = stdId
                        End With

                        dt = getDataTable(cmdMsSql)

                        If dt.Rows.Count <> 0 Then
                            objList.StudentId = dt(0)("StudentId").ToString
                            objList.UserPhoto = "<div class=""UserPhoto"" style=""background: url(../WetestPhoto/UserPhoto/" & dt(0)("StudentId").ToString & ".png);"">"
                            objList.Firstname = dt(0)("Firstname").ToString
                            objList.Surname = dt(0)("Surname").ToString
                            objList.MobileNo = dt(0)("MobileNo").ToString
                            objList.Email = dt(0)("Email").ToString
                            objList.Username = dt(0)("Username").ToString
                            objList.ExpiredDate = "Your account expire date is : " & dt(0)("expiredDate").ToString
                            objList.ExpiredDateAmount = dt(0)("ExpiredDateAmount").ToString
                            objList.UserLevel = dt(0)("LevelShortName").ToString
                        End If
                    End If

                End If
            Catch ex As Exception
                objList.Result = "Error"
                objList.Msg = ex.Message
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return objList
        End Function
        '20240827 -- Function Get Percent Score แต่ละ Skill
        Function GetskillScorePercent(skillId As String, cn As SqlConnection)
            Dim cmdMsSql As SqlCommand
            '20240722 เพิ่มการคำนวน % แบบแยกสกิล
            cmdMsSql = cmdSQL(cn, "select ei_id,(userscore * 100)/answerscore as skillPercent from (
                                                    select EI_Id,case when sum(totalscore) is null then 0 else sum(totalscore) end as UserScore ,answerScore from(
                                                    select distinct pei.EI_Id,qz.totalscore,sum(a.AnswerScore) as answerScore
                                                    from tblQuiz qz inner join tblquizSession qs on qz.quizId = qs.QuizId 
                                                    inner join tblQuizQuestion qq on qz.QuizId = qq.QuizId
                                                    inner join tblstudentGoal sg on qs.studentId = sg.StudentId
                                                    inner join tblquestion q on q.questionId = qq.QuestionId
                                                    inner join tblanswer a on q.QuestionId = a.QuestionId
                                                    inner join tblQuestionEvaluationIndexItem qei on q.QuestionId = qei.Question_Id and qei.question_Id = qq.QuestionId
                                                    inner join tblEvaluationIndex ei on qei.EI_Id = ei.EI_Id inner join tblEvaluationIndex pei on ei.Parent_Id = pei.ei_id 
                                                    and pei.EI_Id in(@skillId)
                                                    where qs.StudentId = @stdid and qz.starttime between sg.StartDate 
                                                    and sg.enddate and sg.isactive = 1 group by pei.ei_id,qz.totalscore)a group by ei_id,answerScore)b;")
            With cmdMsSql
                .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("studentid").ToString
                .Parameters.Add("@skillId", SqlDbType.VarChar).Value = skillId
            End With

            Dim dtTotal = getDataTable(cmdMsSql)

            If dtTotal.Rows.Count <> 0 Then
                Return Format(dtTotal.Rows(0)("skillPercent"), "N2") & "%"
            Else
                Return "0%"
            End If

        End Function
        '20240904 -- Function Get Goal Notification
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetGoalNoti()
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, dtSkill As DataTable
            Dim L1 As New List(Of clsStudentData)
            Dim objList As New clsStudentData()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "select SNId from tblStudentNoti where isactive = 1 and studentId = @stdid 
                                        and NotiId = '061790A3-A512-48EF-BB9C-B60C6F738768';")
                With cmdMsSql
                    .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("studentId").ToString
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count <> 0 Then
                    Dim SettingNoti As Integer = CInt(Getconfig("NotiGoalTime"))

                    cmdMsSql = cmdSQL(cn, "select DATEDIFF(day,getdate(),EndDate) as GoalEndDate 
                                        from tblStudentGoal where studentId = @stdid and isactive = 1 and GoalType = 1;")
                    With cmdMsSql
                        .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("studentId").ToString
                    End With

                    dt = getDataTable(cmdMsSql)

                    If CInt(dt.Rows(0)("GoalEndDate")) <= SettingNoti And CInt(dt.Rows(0)("GoalEndDate")) > 0 Then
                        'แจ้งเตือน
                        objList.Result = "noti"
                        objList.Msg = "Close to the GOAL"
                    Else
                        objList.Result = "not"
                    End If
                Else
                    objList.Result = "not"
                End If

                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.Result = "Error"
                objList.Msg = ex.Message
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240906 -- Function Get Setting Item
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetSettingItem()
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim L1 As New List(Of clsNoti)
            Dim objList As New clsNoti()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "select n.NotiId,NotiName,sn.SNId
                                        from tblNotification n left  join  tblStudentNoti sn on n.NotiId = sn.NotiId 
                                        and sn.StudentId = @stdid and sn.IsActive = 1 where  n.IsActive = 1;")

                With cmdMsSql
                    .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("studentId").ToString
                End With

                dt = getDataTable(cmdMsSql)

                Dim SettingiItem As String = ""

                For i = 0 To dt.Rows.Count() - 1
                    SettingiItem &= "<div class='flexDiv settingItem'>
                                    <span class='firstflexdiv settingType'>" & dt.Rows(i)("NotiName").ToString & "</span>
                                    <input type='checkbox' class='btnSetting' id='" & dt.Rows(i)("NotiId").ToString & "'"
                    If dt.Rows(i)("SNId").ToString <> "" Then
                        SettingiItem &= " checked "
                    End If
                    SettingiItem &= " /></div>"
                Next

                If dt.Rows.Count() <> 0 Then
                    objList.Result = "success"
                    objList.ResultTxt = SettingiItem
                End If
                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.Result = "Error"
                objList.ResultTxt = ex.Message
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240912 -- Function Update Noti Status
        <AcceptVerbs(HttpVerbs.Post)>
        Function UpdateNoti()
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim L1 As New List(Of clsNoti)
            Dim objList As New clsNoti()
            Dim isCheck As String
            If Request.Form("IsCheck").ToLower = "true" Then isCheck = "1" Else isCheck = "0"

            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                Dim GoalResult As Boolean = True
                If Request.Form("NotiId").ToString.ToUpper = "061790A3-A512-48EF-BB9C-B60C6F738768" And isCheck = "1" Then
                    cmdMsSql = cmdSQL(cn, "Select SGId from tblStudentGoal where StudentId = @stdid 
                                            and isactive = 1 and EndDate > getdate();")
                    With cmdMsSql
                        .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("studentId").ToString
                    End With

                    dt = getDataTable(cmdMsSql)

                    If dt.Rows.Count() = 0 Then
                        GoalResult = False
                    End If
                End If

                If GoalResult Then
                    cmdMsSql = cmdSQL(cn, "Select NotiId from tblStudentNoti where  NotiId = @notiId And StudentId = @stdid;")

                    With cmdMsSql
                        .Parameters.Add("@notiId", SqlDbType.VarChar).Value = Request.Form("NotiId").ToString
                        .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("studentId").ToString
                    End With

                    dt = getDataTable(cmdMsSql)

                    If dt.Rows.Count = 0 Then
                        cmdMsSql = cmdSQL(cn, "Insert Into tblStudentNoti(studentId, NotiId) values(@stdid,@notiId);")
                    Else
                        cmdMsSql = cmdSQL(cn, "Update tblStudentNoti Set isActive = @isCheck where NotiId = @notiId And StudentId = @stdid;")
                    End If

                    With cmdMsSql
                        .Parameters.Add("@notiId", SqlDbType.VarChar).Value = Request.Form("NotiId").ToString
                        .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("studentId").ToString
                        .Parameters.Add("@isCheck", SqlDbType.VarChar).Value = isCheck
                        .ExecuteNonQuery()
                    End With

                    objList.Result = "success"
                Else
                    objList.Result = "notsetgoal"
                End If

                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.Result = "Error"
                objList.ResultTxt = ex.Message
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function

        '20240923 -- Function Check Quiz ที่ทำค้างไว้ปิดไปหรือ Session หลุดไป
        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckNotEndQuiz()
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()

            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select top 1 q.quizId from tblQuiz q inner join tblquizsession qs 
                                        on q.QuizId = qs.QuizId where qs.StudentId = @stdid
                                        and EndTime is null and QuizMode = 2 order by q.LastUpdate desc;")

                With cmdMsSql
                    .Parameters.Add("@stdid", SqlDbType.VarChar).Value = Session("studentId").ToString
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count <> 0 Then
                    Session("QuizId") = dt.Rows(0)(0).ToString
                    Session("QuizMode") = "2"
                    objList.dataType = "havequiz"
                Else
                    objList.dataType = "nothave"
                End If

                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "Error"
                objList.errorMsg = ex.Message
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function

#End Region

#Region "Practice"
        Function Practice() As ActionResult
            Return View()
        End Function
        '20240712 -- ปรับ Query ให้ดึงเฉพาะระดับชั้นที่ตรงและน้อยกว่าที่ User สามารถเล่นได้
        '20240912 -- ปรับ Query ให้ดึงข้อสอบจากตัวชี้วัดให้ถูกต้อง
        '20240924 -- เพิ่มการดึงสกิลที่จะแสดงจาก DB
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetLesson()
            Dim L1 As New List(Of clsPracticeSet)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable, dtSkill As DataTable
            Dim LevelId As String = Request.Form("LevelId")
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "select EI_Id,EI_Name from tblskill where IsActive = 1;")
                dtSkill = getDataTable(cmdMsSql)

                For i = 0 To dtSkill.Rows.Count() - 1

                    Dim Skn As String = dtSkill.Rows(i)("EI_Id").ToString

                    cmdMsSql = cmdSQL(cn, "Select row_number() over(order by a.testsetid) As TestsetNo,a.TestsetId,a.QuizId 
                                            from (select distinct t.TestsetId,qrs.QuizId 
                                            from tblTestset t left join (select TestSetId,count(q.quizid) as quizId 
                                            from tblquiz q inner join tblQuizSession qs on q.QuizId = qs.QuizId 
                                            where StudentId = @stdId and q.QuizMode = 2 group by testsetid)qrs 
                                            on t.TestsetId = qrs.TestSetId inner join tblTestSetQuestionSet tsqs on t.testsetId = tsqs.TestSetId
                                            inner join tblTestSetQuestionDetail tsqd on tsqs.TSQSId = tsqd.TSQSId inner join tblQuestionEvaluationIndexItem qei on tsqd.QuestionId = qei.Question_Id
                                            inner join tblEvaluationIndex ei on qei.ei_id = ei.EI_Id and ei.Parent_Id = @skillId
                                            where t.levelId = @LevelId and t.isactive = 1 and t.IsPractice = 1 and ei.IsActive = 1 and t.IsStandart = 1)a;")
                    With cmdMsSql
                        .Parameters.Add("@skillId", SqlDbType.VarChar).Value = Skn
                        .Parameters.Add("@LevelId", SqlDbType.VarChar).Value = LevelId
                        .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId")
                        .ExecuteNonQuery()
                    End With

                    dt = getDataTable(cmdMsSql)

                    Dim skilltxt As String
                    Dim skilltxtShort As String

                    If dt.Rows.Count() <> 0 Then
                        skilltxt = ""
                        skilltxtShort = ""
                        For j = 0 To dt.Rows.Count() - 1

                            skilltxt &= "<div id=" & dt.Rows(j)("TestsetId").ToString & " class=""Lessondiv Lesson" & dtSkill.Rows(i)("EI_Name").ToString & """>" & dt.Rows(j)("TestsetNo").ToString

                            If dt.Rows(j)("QuizId").ToString <> "" Then
                                skilltxt &= "<div class='CheckQuiz'></div>"
                            End If

                            skilltxt &= "</div>"

                            If j = 4 Then
                                skilltxtShort = skilltxt
                            End If
                        Next
                        Dim objList As New clsPracticeSet()
                        objList.skillSet = dtSkill.Rows(i)("EI_Name").ToString
                        objList.skillTxtAll = skilltxt
                        objList.skillTxtShort = skilltxtShort
                        objList.skillAmount = dt.Rows.Count()
                        L1.Add(objList)
                        objList = Nothing
                    Else
                        Dim objList As New clsPracticeSet()
                        objList.skillSet = dtSkill.Rows(i)("EI_Name").ToString
                        objList.skillTxtAll = ""
                        objList.skillTxtShort = ""
                        objList.skillAmount = dt.Rows.Count()
                        L1.Add(objList)
                        objList = Nothing
                    End If
                Next

            Catch ex As Exception
                Dim objList As New clsPracticeSet()
                objList.skillSet = "Error"
                objList.skillTxtAll = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If dtSkill IsNot Nothing Then dtSkill.Dispose() : dtSkill = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function CreatePractice()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                Dim StdId As String = Session("StudentId").ToString
                Dim TestsetId As String = Request.Form("TestsetId")
                Dim TestsetName As String = Request.Form("TestsetName")

                Dim QuizData As New clsQuizData

                QuizData.QuizMode = "2"
                QuizData.TestsetId = TestsetId
                QuizData.QuizName = TestsetName
                CreateNewQuiz(QuizData)

                objList.dataType = "success"
                objList.errorMsg = ""
                L1.Add(objList)
                objList = Nothing
                Session("AnsweredfromReport") = Nothing
            Catch ex As Exception
                objList.dataType = "Error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240718 -- ปรับวิธีการบันทึกข้อสอบแบบสุ่มตามตัวชี้วัด
        '20240830 -- ปรับ Query สุ่มข้อสอบจากระดับชั้นปัจจุบัน
        <AcceptVerbs(HttpVerbs.Post)>
        Function RandomPractice()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql1 As SqlCommand, cmdMsSql2 As SqlCommand, cmdMsSql3 As SqlCommand
            Dim TestsetId As String = Guid.NewGuid.ToString
            Dim tsqsId As String = Guid.NewGuid.ToString
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                'Insert tblTestset tblTestsetQuestionSet tblTestsetQuestionDetail แล้ว return Testset Id ไปส่งให้ทำข้อสอบ practice

                cmdMsSql1 = cmdSQL(cn, "Insert Into tbltestset Select @testsetid,'RandomExam',sl.LevelId,0,1,0,0,0,@stdId,1,getdate() 
                                        from tblstudent s inner join tblStudentLevel sl on s.studentId = sl.StudentId where s.studentId = @stdId 
                                        and sl.IsActive = 1;")
                With cmdMsSql1
                    .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                    .Parameters.Add("@testsetid", SqlDbType.VarChar).Value = TestsetId
                    .ExecuteNonQuery()
                End With

                cmdMsSql2 = cmdSQL(cn, "insert into tblTestSetQuestionSet select @tsqsId,@TestsetId,1,newid(),null,1,getdate();")
                With cmdMsSql2
                    .Parameters.Add("@TestsetId", SqlDbType.VarChar).Value = TestsetId
                    .Parameters.Add("@tsqsId", SqlDbType.VarChar).Value = tsqsId
                    .ExecuteNonQuery()
                End With

                If Request.Form("arrSkill") = "All" Then
                    cmdMsSql2 = cmdSQL(cn, "insert into tblTestSetQuestionDetail 
                                            Select newid(),@tsqsId,row_number() over (order by questionId),QuestionId,1,getdate() from (
                                            select top (@ExamAmount) q.questionId from tblquestion q 
                                            inner join tblQuestionset qs on q.qsetid = qs.QSetId 
                                            inner join tblQuestionCategory qc on qs.QCategoryId  = qc.QCategoryId
                                            inner join tblbook b on qc.BookGroupId = b.BookGroupId
                                            inner join tblstudentLevel sl on b.LevelId = sl.LevelId 
                                            inner join tblQuestionEvaluationIndexItem qei on q.QuestionId = qei.Question_Id
                                            inner join tblEvaluationIndex ei on qei.EI_Id = ei.EI_Id
                                            inner join tblSkill sk on sk.EI_Id = ei.Parent_Id where b.BookSyllabus = '51'
                                            and q.isactive = 1 and qs.isactive = 1 and qc.isactive = 1 and b.IsActive = 1 and qei.IsActive = 1 and sk.IsActive = 1 and ei.IsActive = 1 
                                            and sl.IsActive = 1 and q.QuestionId not in (
                                            select questionId from tblquizquestion qq inner join tblQuizSession qs 
                                            on qq.QuizId = qs.quizid where qs.studentId = @stdId) order by newid())a;")
                    With cmdMsSql2
                        .Parameters.Add("@ExamAmount", SqlDbType.TinyInt).Value = CInt(Request.Form("ExamAmount"))
                        .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                        .Parameters.Add("@tsqsId", SqlDbType.VarChar).Value = tsqsId
                        .ExecuteNonQuery()
                    End With
                Else

                    Dim skilltxt() As String = Request.Form("arrSkill").Split(",")
                    Dim skt As String = ""
                    For Each i In skilltxt
                        skt &= ",'" & i & "'"
                    Next

                    skt = skt.Substring(1, skt.Length - 1)

                    InsertRandomQuestion(Session("StudentId"), skt, Request.Form("ExamAmount"), tsqsId)
                End If

                Dim objList As New clsMain()
                objList.dataType = "success"
                objList.errorMsg = TestsetId
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                Dim objList As New clsMain()
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If cmdMsSql1 IsNot Nothing Then cmdMsSql1.Dispose() : cmdMsSql1 = Nothing
                If cmdMsSql2 IsNot Nothing Then cmdMsSql2.Dispose() : cmdMsSql2 = Nothing
                If cmdMsSql3 IsNot Nothing Then cmdMsSql3.Dispose() : cmdMsSql3 = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)

        End Function
        '20240730 -- สร้าง Dropdown สำหรับเลือกระดับชั้นที่ต้องการให้แสดงชุดข้อสอบ
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetLevel()
            Dim L1 As New List(Of clsPracticeLevel)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsPracticeLevel()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================


                cmdMsSql = cmdSQL(cn, "select LevelId,LevelName from tbllevel where levelno <= (select top 1 levelno from tbllevel l 
                                        left join tblStudentLevel sl on l.LevelId = sl.LevelId where studentId = @stdId order by sl.lastupdate desc) order by levelno desc;")
                With cmdMsSql
                    .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                    .ExecuteNonQuery()
                End With

                dt = getDataTable(cmdMsSql)

                For i = 0 To dt.Rows().Count() - 1
                    Dim objList2 As New clsPracticeLevel()
                    objList2.result = "success"
                    objList2.LevelId = dt.Rows(i)("LevelId").ToString
                    objList2.LevelName = dt.Rows(i)("LevelName").ToString
                    L1.Add(objList2)
                    objList2 = Nothing
                Next


            Catch ex As Exception
                objList.result = "error"
                objList.LevelName = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240924 -- เพิ่มการดึงสกิลที่จะแสดงจาก DB
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetSkill()
            Dim L1 As New List(Of clsPracticeSet)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dtSkill As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "select EI_Id,EI_Name from tblskill where IsActive = 1;")
                dtSkill = getDataTable(cmdMsSql)

                For i = 0 To dtSkill.Rows.Count() - 1
                    Dim objList As New clsPracticeSet()
                    objList.skillSet = dtSkill.Rows(i)("EI_Name").ToString
                    L1.Add(objList)
                    objList = Nothing
                Next

            Catch ex As Exception
                Dim objList As New clsPracticeSet()
                objList.skillSet = "Error"
                objList.skillTxtAll = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dtSkill IsNot Nothing Then dtSkill.Dispose() : dtSkill = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function

        '20240909 -- สุ่มข้อสอบ และบันทึก
        Function InsertRandomQuestion(StudentId As String, skt As String, QuestionAmount As String, tsqsId As String) As DataTable
            Dim cn As SqlConnection, cmdMsSql1 As SqlCommand, cmdMsSql2 As SqlCommand, cmdMsSql3 As SqlCommand
            Dim sqlBuilder As StringBuilder = New StringBuilder()
            Dim sqlBuilder2 As StringBuilder = New StringBuilder()
            Dim sqlBuilder3 As StringBuilder = New StringBuilder()
            Dim dtRandomQuestion As DataTable

            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()

                'Get NewQuestion
                Dim dtNewQuestion As DataTable = GetNewRandomQuestion(skt, StudentId, QuestionAmount)
                Dim NewQuestionAmount As Integer = dtNewQuestion.Rows.Count()

                If NewQuestionAmount <> 0 Then
                    InsertQuestionTestset(dtNewQuestion, tsqsId)

                    If NewQuestionAmount < QuestionAmount Then
                        'Get WrongQuestion
                        Dim dtWrongQuestion = GetWrongRandomQuestion(skt, StudentId, QuestionAmount - NewQuestionAmount, tsqsId)
                        Dim WrongQuestionAmount As Integer = dtWrongQuestion.Rows.Count()

                        If WrongQuestionAmount <> 0 Then
                            InsertQuestionTestset(dtWrongQuestion, tsqsId)

                            If WrongQuestionAmount + NewQuestionAmount < QuestionAmount Then
                                'GetAllQuestion
                                Dim dtAllQuestion = GetRigthRandomQuestion(skt, StudentId, QuestionAmount - (NewQuestionAmount + WrongQuestionAmount), tsqsId)
                                InsertQuestionTestset(dtAllQuestion, tsqsId)
                            End If
                        End If
                    End If

                End If

            Catch ex As Exception

            End Try

        End Function
        '20240909 -- ดึงข้อสอบที่ยังไม่เคยทำ
        '20240923 -- ปรับ Query แก้ปัญหาข้อสอบซ้ำ
        Function GetNewRandomQuestion(skt As String, StudentId As String, questionAmount As String) As DataTable
            Dim cn As SqlConnection, cmdMsSql As SqlCommand
            Dim sqlBuilder As StringBuilder = New StringBuilder()
            Dim dt As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()

                sqlBuilder.Append("select top (")
                sqlBuilder.Append(questionAmount)
                sqlBuilder.Append(") a.QuestionId from(select distinct QuestionId 
                                from tblQuestionEvaluationIndexItem qei
                                inner join tblQuestion q on qei.Question_Id = q.QuestionId
                                inner join tblQuestionset qs on qs.QSetId = q.QSetId
                                inner join tblQuestionCategory qc on qs.QCategoryId = qc.QCategoryId
                                inner join tblbook b on b.bookgroupId = qc.bookGroupId
                                inner join tblstudentLevel sl on sl.LevelId = b.LevelId and sl.IsActive = 1
                                inner join tblEvaluationIndex ei on qei.EI_Id = ei.EI_Id 
                                where ei.Parent_Id in(")

                sqlBuilder.Append(skt)

                sqlBuilder.Append(") and sl.studentId = '")
                sqlBuilder.Append(StudentId)
                sqlBuilder.Append("' and q.IsActive = 1 and qs.IsActive = 1 and qc.IsActive = 1 and b.IsActive = 1 
                                and qei.IsActive = 1)a order by newid();")

                cmdMsSql = cmdSQL(cn, sqlBuilder.ToString())

                dt = getDataTable(cmdMsSql)
                Return dt
            Catch ex As Exception

            End Try

        End Function
        '20240909 -- ดึงข้อสอบที่เคยทำผิด
        Function GetWrongRandomQuestion(skt As String, StudentId As String, questionAmount As String, tsqsId As String) As DataTable
            Dim cn As SqlConnection, cmdMsSql As SqlCommand
            Dim sqlBuilder As StringBuilder = New StringBuilder()
            Dim dt As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()

                sqlBuilder.Append("select top (")
                sqlBuilder.Append(questionAmount)
                sqlBuilder.Append(") QuestionId
                                from tblQuestionEvaluationIndexItem qei
                                inner join tblQuestion q on qei.Question_Id = q.QuestionId
                                inner join tblQuestionset qs on qs.QSetId = q.QSetId
                                inner join tblQuestionCategory qc on qs.QCategoryId = qc.QCategoryId
                                inner join tblbook b on b.bookgroupId = qc.bookGroupId
                                inner join tblstudentLevel sl on sl.LevelId = b.LevelId and sl.IsActive = 1
                                inner join tblEvaluationIndex ei on qei.EI_Id = ei.EI_Id 
                                where ei.Parent_Id in(")

                sqlBuilder.Append(skt)

                sqlBuilder.Append(") and sl.studentId = '")
                sqlBuilder.Append(StudentId)
                sqlBuilder.Append("' and q.IsActive = 1 and qs.IsActive = 1 and qc.IsActive = 1 and b.IsActive = 1 
                                and qei.IsActive = 1 and q.QuestionId in (select questionId  from tblQuizScore qso 
                                inner join tblQuizSession qs on qso.QuizId = qs.QuizId where qs.StudentId = '")
                sqlBuilder.Append(StudentId)
                sqlBuilder.Append("' and qso.Score <= 0) and q.QuestionId not in(select QuestionId 
                                     from tbltestsetQuestionDetail where tsqsId = '")
                sqlBuilder.Append(tsqsId)
                sqlBuilder.Append("') order by newid();")

                cmdMsSql = cmdSQL(cn, sqlBuilder.ToString())

                dt = getDataTable(cmdMsSql)
                Return dt
            Catch ex As Exception

            End Try
        End Function
        '20240909 -- ดึงข้อสอบที่เคยทำถูก
        Function GetRigthRandomQuestion(skt As String, StudentId As String, questionAmount As String, tsqsId As String) As DataTable
            Dim cn As SqlConnection, cmdMsSql As SqlCommand
            Dim sqlBuilder As StringBuilder = New StringBuilder()
            Dim dt As DataTable
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()

                sqlBuilder.Append("select top (")
                sqlBuilder.Append(questionAmount)
                sqlBuilder.Append(") QuestionId
                                from tblQuestionEvaluationIndexItem qei
                                inner join tblQuestion q on qei.Question_Id = q.QuestionId
                                inner join tblQuestionset qs on qs.QSetId = q.QSetId
                                inner join tblQuestionCategory qc on qs.QCategoryId = qc.QCategoryId
                                inner join tblbook b on b.bookgroupId = qc.bookGroupId
                                inner join tblstudentLevel sl on sl.LevelId = b.LevelId and sl.IsActive = 1
                                inner join tblEvaluationIndex ei on qei.EI_Id = ei.EI_Id 
                                where ei.Parent_Id in(")

                sqlBuilder.Append(skt)

                sqlBuilder.Append(") and sl.studentId = '")
                sqlBuilder.Append(StudentId)
                sqlBuilder.Append("' and q.IsActive = 1 and qs.IsActive = 1 and qc.IsActive = 1 and b.IsActive = 1 
                                and qei.IsActive = 1 and q.QuestionId in (select questionId  from tblQuizScore qso 
                                inner join tblQuizSession qs on qso.QuizId = qs.QuizId where qs.StudentId = '")
                sqlBuilder.Append(StudentId)
                sqlBuilder.Append("' and qso.Score > 0) and q.QuestionId not in(select QuestionId 
                                     from tbltestsetQuestionDetail where tsqsId = '")
                sqlBuilder.Append(tsqsId)
                sqlBuilder.Append("') order by newid();")

                cmdMsSql = cmdSQL(cn, sqlBuilder.ToString())

                dt = getDataTable(cmdMsSql)
                Return dt
            Catch ex As Exception

            End Try
        End Function
        '20240909 -- บันทึกข้อสอบ
        Function InsertQuestionTestset(dtQuestion As DataTable, tsqsId As String)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand
            Dim sqlBuilder As StringBuilder = New StringBuilder()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                For i = 0 To dtQuestion.Rows.Count - 1

                    sqlBuilder.Append("insert into tblTestSetQuestionDetail Select distinct newid(),@tsqsId,
                                        case when max(tsqdNo) + 1 is null then 1 else max(tsqdNo) + 1 end,'")
                    sqlBuilder.Append(dtQuestion.Rows(i)("QuestionId").ToString)
                    sqlBuilder.Append("',1,getdate() from tbltestsetQuestionDetail where tsqsId = '")
                    sqlBuilder.Append(tsqsId)
                    sqlBuilder.Append("';")
                Next

                cmdMsSql = cmdSQL(cn, sqlBuilder.ToString())

                With cmdMsSql
                    .Parameters.Add("@tsqsId", SqlDbType.VarChar).Value = tsqsId
                    .ExecuteNonQuery()
                End With

            Catch ex As Exception

            End Try
        End Function


#End Region

#Region "Report"
        Function Report() As ActionResult
            Return View()
        End Function
        '20240719 -- ดึงข้อมูล Report
        '20240722 -- ปรับการดึงข้อมูล และการสร้าง div Report Item ต่างๆ
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetReport()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                If Session("StudentId") IsNot Nothing Then
                    If Request.Form("PracticeType").ToString = "0" Then
                        objList.errorMsg = GetRandomReport(Request.Form("StartDate").ToString, Request.Form("EndDate").ToString, Request.Form("PracticeType").ToString)
                    Else
                        objList.errorMsg = GetLessonReport(Request.Form("StartDate").ToString, Request.Form("EndDate").ToString, Request.Form("PracticeType").ToString, Request.Form("LevelId").ToString)
                    End If

                    objList.dataType = "success"
                Else
                    objList.dataType = "sessionlost"
                End If

                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function SetSeesionAnswered()
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Try
                Session("AnsweredfromReport") = "true"
                Session("QuizId") = Request.Form("quizId").ToString
                Session("QuizState") = "showanswer"

                objList.dataType = "success"
                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240722 -- GetLessonReport
        '20240830 -- เพิ่ม Filter Level
        '20240903 -- ปรับ Query ให้ Order by วันที่ให้ถูกต้อง
        '20240911 -- ปรับการแสดงผล
        Function GetLessonReport(StartDate, EndDate, PracticeType, LevelId)

            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim reportdata As String = ""
            Try
                ''StartDate=' + startdate + '&EndDate=' + enddate + '&PracticeType=' + PracticeType
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                'Query Hard code id ของตัวชี้วัดไว้ก่อน คิดว่าควรจะต้องหาวิธีบอกว่ามีตัวชี้วัดไหนที่จะใช้บ้าง
                Dim skilltxt() As String = Request.Form("arrSkill").Split(",")
                Dim skt As String = ""
                For Each i In skilltxt
                    skt &= ",'" & i & "'"
                Next

                skt = skt.Substring(1, skt.Length - 1)

                Dim sqlBuilder As StringBuilder = New StringBuilder()
                sqlBuilder.Append("select distinct FORMAT (q.StartTime, 'dd/MM/yyyy') as Quizdate,FORMAT (q.StartTime, 'hh:mm tt') as starttime,
                                        case when FORMAT (q.endtime, 'hh:mm tt') is null then 'none' else FORMAT (q.endtime, 'hh:mm tt') end as endtime ,q.QuizName,
                                        case when cast(cast(q.TotalScore as int) as varchar) + '/' + cast(cast(q.FullScore as int) as varchar) is null 
                                        then '0/' + cast(cast(q.FullScore as int) as varchar) else cast(cast(q.TotalScore as int) as varchar) + '/' + cast(cast(q.FullScore as int) as varchar) end as Score,
                                        eiparent.EI_Id,q.testsetId,q.quizId,q.StartTime as orderTime  from tblTestset t inner join tbltestsetquestionset tqs on t.TestsetId = tqs.TestSetId 
                                        inner join tblTestSetQuestionDetail tqd on tqs.tsqsid = tqd.TSQSId inner join tblQuestionEvaluationIndexItem qei on tqd.questionId = qei.question_Id 
                                        inner join tblEvaluationIndex ei on qei.EI_Id = ei.EI_Id inner join tblEvaluationIndex eiParent on eiParent.EI_Id = ei.parent_id")
                If skt <> "'All'" Then
                    sqlBuilder.Append(" And eiParent.ei_id in(")
                    sqlBuilder.Append(skt)
                    sqlBuilder.Append(")")
                Else
                    sqlBuilder.Append(" And eiParent.ei_id in(select ei_Id from tblskill where isactive = 1)")
                End If

                sqlBuilder.Append(" inner join tblQuiz q on q.TestSetId = t.testsetId inner join tblQuizSession qs on q.QuizId = qs.QuizId 
                                    where IsPractice = 1 and IsStandart = @IsStandard and ei.IsActive = 1 and qei.isactive = 1 and qs.studentid  = @stdId and t.IsActive = 1")

                Select Case Request.Form("StartDate").ToString
                    Case "week"
                        sqlBuilder.Append(" AND q.StartTime >= dateadd(day, 1-datepart(dw, getdate()), CONVERT(date,getdate())) 
                                            AND q.StartTime <  dateadd(day, 8-datepart(dw, getdate()), CONVERT(date,getdate())) ")
                    Case "month"
                        sqlBuilder.Append(" AND q.starttime >= datefromparts(year(getdate()), month(getdate()), 1) ")
                    Case Else
                        sqlBuilder.Append(" AND q.starttime between @StartDate and @EndDate ")
                End Select

                If LevelId <> "" And LevelId <> "undefined" Then
                    sqlBuilder.Append(" And t.LevelId = @LevelId")
                End If

                sqlBuilder.Append(" order by orderTime desc;")

                cmdMsSql = cmdSQL(cn, sqlBuilder.ToString())

                With cmdMsSql
                    .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                    .Parameters.Add("@IsStandard", SqlDbType.VarChar).Value = PracticeType
                    .Parameters.Add("@StartDate", SqlDbType.VarChar).Value = StartDate
                    .Parameters.Add("@EndDate", SqlDbType.VarChar).Value = EndDate
                    .Parameters.Add("@LevelId", SqlDbType.VarChar).Value = LevelId
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows().Count = 0 Then
                    reportdata = "nodata"
                Else
                    For Each i In dt.Rows

                        Dim ItemClass As String

                        Select Case i("Ei_Id").ToString.ToUpper
                            Case "31667BAB-89FF-43B3-806F-174774C8DFBF"
                                ItemClass = "vocabItem"
                            Case "5BBD801D-610F-40EB-89CB-5957D05C4A0B"
                                ItemClass = "grammarItem"
                            Case "FB4B4A71-B777-4164-BA4D-5C1EA9522226"
                                ItemClass = "readingItem"
                            Case "44502C7F-D3BE-4D46-9134-3FE40DA230E9"
                                ItemClass = "listenItem"
                            Case Else
                                ItemClass = "noeiItem"
                        End Select

                        reportdata &= "<div Class='flexDiv'><div class='divDate reportItem " & ItemClass & " firstflexdiv'>" & i("Quizdate") &
                                        "</div><div class='divStartTime reportItem " & ItemClass & "'>" & i("starttime") &
                                        "</div><div class='divEndTime reportItem " & ItemClass & "'>" & i("endtime") &
                                        "</div><div Class='divTestsetName reportItem " & ItemClass & "'>" & i("QuizName") &
                                        "</div><div Then Class='divScore reportItem " & ItemClass & "'>" & i("Score") &
                                        "</div><div class='divAnswered reportItem " & ItemClass & "' QuizId='" & i("quizId").ToString & "'></div>" &
                                        "<div Class='divAgain reportItem' testsetId='" & i("testsetId").ToString & "' testsetname='" & i("QuizName").ToString & "'></div></div>"

                    Next
                End If
            Catch ex As Exception
                Return "error"
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return reportdata
        End Function
        '20240722 -- GetRandomReport
        '20240903 -- ปรับ Query ให้ Order by วันที่ให้ถูกต้อง
        Function GetRandomReport(StartDate, EndDate, PracticeType)

            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim ReportData As String = ""
            Try
                ''StartDate=' + startdate + '&EndDate=' + enddate + '&PracticeType=' + PracticeType
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                'Query Hard code id ของตัวชี้วัดไว้ก่อน คิดว่าควรจะต้องหาวิธีบอกว่ามีตัวชี้วัดไหนที่จะใช้บ้าง
                Dim skilltxt() As String = Request.Form("arrSkill").Split(",")
                Dim skt As String = ""
                For Each i In skilltxt
                    skt &= ",'" & i & "'"
                Next

                skt = skt.Substring(1, skt.Length - 1)

                Dim sqlBuilder As StringBuilder = New StringBuilder()
                sqlBuilder.Append("select distinct FORMAT (q.StartTime, 'dd/MM/yyyy') as Quizdate,FORMAT (q.StartTime, 'hh:mm tt') as starttime,
                                        case when FORMAT (q.endtime, 'hh:mm tt') is null then 'none' else FORMAT (q.endtime, 'hh:mm tt') end as endtime ,
                                        case when cast(cast(q.TotalScore as int) as varchar) + '/' + cast(cast(q.FullScore as int) as varchar) is null 
                                        then '0/' + cast(cast(q.FullScore as int) as varchar) else cast(cast(q.TotalScore as int) as varchar) + '/' + cast(cast(q.FullScore as int) as varchar) end as Score,
                                        q.testsetId,q.quizId,q.StartTime as orderTime from tblTestset t inner join tbltestsetquestionset tqs on t.TestsetId = tqs.TestSetId 
                                        inner join tblTestSetQuestionDetail tqd on tqs.tsqsid = tqd.TSQSId inner join tblQuestionEvaluationIndexItem qei on tqd.questionId = qei.question_Id 
                                        inner join tblEvaluationIndex ei on qei.EI_Id = ei.EI_Id inner join tblEvaluationIndex eiParent on eiParent.EI_Id = ei.parent_id")
                If skt <> "'All'" Then
                    sqlBuilder.Append(" And eiParent.ei_id in(")
                    sqlBuilder.Append(skt)
                    sqlBuilder.Append(")")
                Else
                    sqlBuilder.Append(" And eiParent.ei_id in('31667BAB-89FF-43B3-806F-174774C8DFBF','5BBD801D-610F-40EB-89CB-5957D05C4A0B','FB4B4A71-B777-4164-BA4D-5C1EA9522226','25DA1FAB-EB20-4B1D-8409-C2FB08FC61B3')")
                End If

                sqlBuilder.Append(" inner join tblQuiz q on q.TestSetId = t.testsetId inner join tblQuizSession qs on q.QuizId = qs.QuizId 
                                    where IsPractice = 1 and IsStandart = @IsStandard and ei.IsActive = 1 and qei.isactive = 1 and qs.studentid  = @stdId and t.IsActive = 1")

                Select Case Request.Form("StartDate").ToString
                    Case "week"
                        sqlBuilder.Append(" AND q.StartTime >= dateadd(day, 1-datepart(dw, getdate()), CONVERT(date,getdate())) 
                                            AND q.StartTime <  dateadd(day, 8-datepart(dw, getdate()), CONVERT(date,getdate())) ")
                    Case "month"
                        sqlBuilder.Append(" AND q.starttime >= datefromparts(year(getdate()), month(getdate()), 1) ")
                    Case Else
                        sqlBuilder.Append(" AND q.starttime between @StartDate and @EndDate ")
                End Select

                sqlBuilder.Append("  order by orderTime desc;")


                cmdMsSql = cmdSQL(cn, sqlBuilder.ToString())

                With cmdMsSql
                    .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                    .Parameters.Add("@IsStandard", SqlDbType.VarChar).Value = PracticeType
                    .Parameters.Add("@StartDate", SqlDbType.VarChar).Value = StartDate
                    .Parameters.Add("@EndDate", SqlDbType.VarChar).Value = EndDate
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows().Count = 0 Then
                    ReportData = "nodata"
                Else
                    For Each i In dt.Rows


                        ReportData &= "<div class='flexDiv'><div class='divDate reportItem random firstflexdiv'>" & i("Quizdate") &
                                        "</div><div class='divStartTime reportItem random'>" & i("starttime") &
                                        "</div><div class='divEndTime reportItem random'>" & i("endtime") & "</div>"

                        If Request.Form("PracticeType").ToString = "0" Then

                            cmdMsSql = cmdSQL(cn, "Select distinct pei.EI_id from tblQuizQuestion qq inner join tblQuestionEvaluationIndexItem qei On qq.QuestionId = qei.Question_Id
                                                        inner Join tblEvaluationIndex ei on qei.EI_Id = ei.EI_Id inner join tblEvaluationIndex pei on ei.Parent_Id = pei.ei_id 
                                                        And pei.EI_Id in('31667BAB-89FF-43B3-806F-174774C8DFBF','5BBD801D-610F-40EB-89CB-5957D05C4A0B','FB4B4A71-B777-4164-BA4D-5C1EA9522226','25DA1FAB-EB20-4B1D-8409-C2FB08FC61B3')
                                                        where qq.QuizId = @quizId and qq.isactive = 1 and qei.IsActive = 1;")
                            With cmdMsSql
                                .Parameters.Add("@quizId", SqlDbType.VarChar).Value = i("quizId").ToString
                            End With

                            Dim dtEi As DataTable = getDataTable(cmdMsSql)

                            If dtEi.Rows.Count <> 0 Then
                                ReportData &= "<div Class='divEI reportItem random'>"
                                For Each k In dtEi.Rows
                                    Select Case k("Ei_Id").ToString.ToUpper
                                        Case "31667BAB-89FF-43B3-806F-174774C8DFBF"
                                            ReportData &= " <div Class='Logovocab'></div>"
                                        Case "5BBD801D-610F-40EB-89CB-5957D05C4A0B"
                                            ReportData &= " <div Class='Logogrammar'></div>"
                                    End Select
                                Next
                                ReportData &= "</div>"
                            End If


                        Else
                            ReportData &= " < div Class='divTestsetName reportItem random'>" & i("QuizName") & "</div>"
                        End If

                        ReportData &= "<div Then Class='divScore reportItem random'>" & i("Score") &
                     "</div><div class='divAnswered reportItem random' QuizId='" & i("quizId").ToString & "'></div><div class='divAgain reportItem' testsetId='" & i("testsetId").ToString & "'></div></div>"

                    Next
                End If
            Catch ex As Exception
                Return "error"
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return ReportData
        End Function
#End Region

#Region "Assignment"
        Function Assignment() As ActionResult
            Return View()
        End Function
        '20240813 -- Get Assignment
        '20240814 -- ปรับ Get Assignment เพิ่มข้อมูลสำหรับเอาไปสร้าง Quiz
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetAssignment()

            Dim L1 As New List(Of clsAssignment)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsAssignment()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ================================================== 

                cmdMsSql = cmdSQL(cn, "select a.AssignmentId,AssignmentName,CONVERT (varchar(10), EndDate, 103) as EndDate
                                        ,datediff(day,getdate(),enddate) as EndDateAmount,ReferenceId
                                        from tblAssignment a inner join tblAssignmentDetail ad on a.AssignmentId = ad.AssignmentId 
                                        where assignto = @stdId and a.IsActive = 1 and ad.IsActive = 1 order by enddate")
                With cmdMsSql
                    .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentID")
                    .ExecuteNonQuery()
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count <> 0 Then
                    objList.Result = "success"
                    For i As Integer = 0 To dt.Rows.Count - 1
                        Select Case CInt(dt(i)("EndDateAmount"))
                            Case < 0
                                objList.OverDue &= "<div class=""assignItem"">
                                                    <div class=""redAssignment"" refId=""" & dt(i)("ReferenceId").ToString & """ refName=""" & dt(i)("AssignmentName").ToString & """></div>
                                                    <div>" & dt(i)("AssignmentName") & "</div>
                                                    <div class=""assignDate"">" & dt(i)("EndDate") & "</div></div>"
                            Case = 0
                                objList.Today &= "<div class=""assignItem"">
                                                  <div class=""orangeAssignment"" refId=""" & dt(i)("ReferenceId").ToString & """ refName=""" & dt(i)("AssignmentName").ToString & """></div>
                                                  <div>" & dt(i)("AssignmentName") & "</div>
                                                  <div class=""assignDate"">" & dt(i)("EndDate") & "</div></div>"
                            Case 1 To 6
                                objList.ThisWeek &= "<div class=""assignItem"">
                                                    <div class=""greenAssignment"" refId=""" & dt(i)("ReferenceId").ToString & """ refName=""" & dt(i)("AssignmentName").ToString & """></div>
                                                    <div>" & dt(i)("AssignmentName") & "</div>
                                                    <div class=""assignDate"">" & dt(i)("EndDate") & "</div></div>"
                            Case > 6
                                objList.NextWeek &= "<div class=""assignItem"">
                                                    <div class=""greenAssignment"" refId=""" & dt(i)("ReferenceId").ToString & """ refName=""" & dt(i)("AssignmentName").ToString & """></div>
                                                    <div>" & dt(i)("AssignmentName") & "</div>
                                                    <div class=""assignDate"">" & dt(i)("EndDate") & "</div></div>"
                        End Select
                    Next
                Else
                    objList.Result = "nodata"
                End If

                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.Result = "error"
                objList.ResultTxt = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)

        End Function
        '20240814 -- สร้าง Quiz Assignment
        <AcceptVerbs(HttpVerbs.Post)>
        Function CreateAssignment()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                Dim StdId As String = Session("StudentId").ToString
                Dim TestsetId As String = Request.Form("TestsetId")
                Dim TestsetName As String = Request.Form("TestsetName")

                Dim QuizData As New clsQuizData

                QuizData.QuizMode = "4"
                QuizData.TestsetId = TestsetId
                QuizData.QuizName = TestsetName
                CreateNewQuiz(QuizData)

                objList.dataType = "success"
                objList.errorMsg = ""
                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "Error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function

#End Region

#Region "Admin"
        Function Admin() As ActionResult
            Return View()
        End Function

        '20240819 -- ดึงข้อมูลสลิป
        '20240826 -- ปรับให้ดึงข้อมูล Slip จาก Status ต่างๆ
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetJobDetail()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()

            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ================================================== 

                Dim jobType As Integer = CInt(Request.Form("JobType"))
                Dim jobTypestr As String = ""
                Select Case jobType
                    Case 1
                        jobTypestr = " And r.PackageId Is Not null And r.discountId Is null;"
                    Case 2
                        jobTypestr = " And r.PackageId Is Not null And r.DiscountId Is Not null;"
                    Case 3
                        jobTypestr = " And r.PackageId Is null And r.KeyCodeId Is Not null;"
                End Select

                cmdMsSql = cmdSQL(cn, "Select distinct r.RHId,  CONVERT(varchar(10), r.lastupdate, 103) as SlipDate
                                        ,FORMAT(r.lastupdate,'hh:mm tt') as SlipTime
                                        ,PackageName + ' ' + cast(packageprice as varchar) + ' Bath'  as SlipDetail
                                        ,' ' + s.Firstname + ' ' + s.Surname as StudentName
                                        From tblRegister r inner Join tblstudent s on r.studentId = s.studentId 
                                        inner Join tblpackage p on p.packageId = r.packageId where RegisterStatus = @JobStatus" & jobTypestr)
                With cmdMsSql
                    .Parameters.Add("@JobStatus", SqlDbType.Int).Value = CInt(Request.Form("JobStatus"))
                End With

                dt = getDataTable(cmdMsSql)

                Dim jobDetail As String = ""
                If dt.Rows.Count <> 0 Then
                    For i = 0 To dt.Rows.Count() - 1
                        jobDetail &= "<div Class=""flexDiv jobDetailItem "" RHId=""" & dt.Rows(i)("RHId").ToString & """><div Class=""UploadDate"">" & dt.Rows(i)("SlipDate") & "</div>
                                            <div Class=""UploadTime"">" & dt.Rows(i)("SlipTime") & "</div>
                                            <div Class=""UploadDetail"">" & dt.Rows(i)("SlipDetail") & dt.Rows(i)("StudentName") & "</div></div>"
                    Next

                End If
                objList.dataType = "success"
                objList.errorMsg = jobDetail
                L1.Add(objList)
                objList = Nothing
            Catch ex As Exception
                objList.dataType = "Error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240820 -- ดึงข้อมูลรายละเอียดสลิป
        '20240902 -- เพิ่มแสดงเบอร์โทรศัพท์ที่หน้ารายละเอียด
        <AcceptVerbs(HttpVerbs.Post)>
        Function GetSlip()
            Dim L1 As New List(Of clsSlip)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim cnl As SqlConnection
            Dim objList As New clsSlip()

            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()

                cnl = New SqlConnection(sqlCon("licenseKey"))
                If cnl.State = 0 Then cnl.Open()
                ' ================================================== 

                cmdMsSql = cmdSQL(cn, "Select 'Date : ' + CONVERT(varchar(10), r.lastupdate, 103) + '  Time : ' + FORMAT(r.lastupdate,'hh:mm tt') as SlipTime
                                        ,PackageName + ' ' + cast(packageprice as varchar) + ' Bath'  as SlipDetail
                                        ,' ' + s.Firstname + ' ' + s.Surname as StudentName,s.StudentId,s.MobileNo
                                        From tblRegister r inner Join tblstudent s on r.studentId = s.studentId inner Join tblpackage p on p.packageId = r.packageId 
                                        left join tblDiscount d on r.DiscountId = d.DiscountId where rhid = @rhid;")
                With cmdMsSql
                    .Parameters.Add("@rhid", SqlDbType.VarChar).Value = Request.Form("rhid")
                    .ExecuteNonQuery()
                End With

                dt = getDataTable(cmdMsSql)

                Dim SlipDetail As String = ""
                If dt.Rows.Count <> 0 Then
                    For i = 0 To dt.Rows.Count() - 1
                        SlipDetail &= "<div class=""UploadDateTime"">" & dt.Rows(0)("SlipTime") & "</div>
                                       <div class=""PackageName"">Package : " & dt.Rows(0)("SlipDetail") & "</div>
                                       <div class=""Discount"">Discount : -</div>
                                       <div class=""UploadName"">Name : " & dt.Rows(0)("StudentName") & "</div>
                                       <div class=""UploadMobileNo"">Mobile No. : " & dt.Rows(0)("MobileNo") & "</div>"
                    Next
                    objList.Result = "success"
                    objList.ResultTxt = SlipDetail
                    objList.slipURL = "/WetestPhoto/Slip/" & dt.Rows(0)("StudentId").ToString & ".png"
                    L1.Add(objList)
                    objList = Nothing
                End If

            Catch ex As Exception
                objList.Result = "Error"
                objList.ResultTxt = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        '20240820 -- บันทึก Status Slip
        '20240822 -- บันทึกหมาายเหตุ กรณี Reject Slip
        <AcceptVerbs(HttpVerbs.Post)>
        Function ConfirmSlip()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()

            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ================================================== 


                cmdMsSql = cmdSQL(cn, "Update tblRegister set RegisterStatus = @RegisterStatus , registeramount += 1 
                                       ,RejectReason = @RejectReason,lastupdate = getdate() where rhid = @rhid;
                                       update tblstudent set ExpiredDate = getdate() + PackageTime / 24
                                       from tblregister r inner join tblPackage p on r.packageId = p.packageId 
                                       inner join tblstudent s on s.StudentId = r.StudentId where rhid = @rhid;")
                With cmdMsSql
                    .Parameters.Add("@rhid", SqlDbType.VarChar).Value = Request.Form("rhid").ToString
                    .Parameters.Add("@RegisterStatus", SqlDbType.Int).Value = CInt(Request.Form("RegisterStatus"))
                    .Parameters.Add("@RejectReason", SqlDbType.VarChar).Value = Request.Form("RejectReason")
                    .ExecuteNonQuery()
                End With


                objList.dataType = "success"

                L1.Add(objList)
                objList = Nothing

            Catch ex As Exception
                objList.dataType = "Error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            Finally
                If dt IsNot Nothing Then dt.Dispose() : dt = Nothing
                If cmdMsSql IsNot Nothing Then cmdMsSql.Dispose() : cmdMsSql = Nothing
                If cn IsNot Nothing Then If cn.State = 1 Then cn.Close() : cn.Dispose() : cn = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
#End Region

#Region "Class"
        Private Class clsStudentData
            Inherits clsMain
            Public StudentId As String, Firstname As String, Surname As String, MobileNo As String, Email As String, Username As String, UserPhoto As String _
            , Result As String, Msg As String, UserLevel As String, ExpiredDate As String, ExpiredDateAmount As String _
            , TotalGoal As String, TotalGoalAmount As String, TotalDatePercent As String, TotalScorePercent As String _
            , ReadingGoal As String, ReadingGoalAmount As String, ReadingDatePercent As String, ReadingScorePercent As String _
            , ListeningGoal As String, ListeningGoalAmount As String, ListeningDatePercent As String, ListeningScorePercent As String _
            , VocabGoal As String, VocabGoalAmount As String, VocabDatePercent As String, VocabScorePercent As String _
            , GrammarGoal As String, GrammarGoalAmount As String, GrammarDatePercent As String, GrammarScorePercent As String _
            , SituationGoal As String, SituationGoalAmount As String, SituationDatePercent As String, SituationScorePercent As String, GoalAlertDateAmount As String

        End Class
        Private Class clsItemQAndA
            Inherits clsMain
            Public ItemType As String, ItemNo As String, ItemId As String, Itemtxt As String, ItemStatus As String _
                , multiname As String, multipath As String, multiSlowname As String, multiSlowpath As String, multitxt As String _
                , multiAnsname As String, multiAnspath As String, multiAnsSlowname As String, multiAnsSlowpath As String, multiAnstxt As String
        End Class
        Public Class clsQuizData
            Inherits clsMain
            Public QuizId As String, QuizMode As String, TestsetId As String, FullScore As String, QuestionAmount As String, resultType As String, resultMsg As String, QuizName As String
        End Class
        Public Class clsPracticeSet
            Inherits clsMain
            Public skillSet As String, skillTxtShort As String, skillTxtAll As String, skillAmount As String
        End Class
        Public Class clsPracticeLevel
            Inherits clsMain
            Public result As String, LevelId As String, LevelName As String
        End Class
        Public Class clsLeapChoiceData
            Inherits clsMain
            Public result As String, leapChoicetxt As String, allPage As String
        End Class
        Public Class clsAnswerChoice
            Inherits clsMain
            Public result As String, AnswerChoicetxt As String, allPage As String, RightAmount As String, WrongAmount As String, LeapAmount As String
        End Class
        '20240723 -- OTP Data
        Public Class clsOTPStatus
            Inherits clsMain
            Public OSId As String, ReponseTime As String, ResultStatus As String, Resulttxt As String
        End Class
        Public Class clsProgressbar
            Inherits clsMain
            Public Result As String, ResultTxt As String, AnsweredAmount As String, AnsweredPercent As String
        End Class
        '20240813 -- Assignment Data
        Public Class clsAssignment
            Inherits clsMain
            Public Result As String, ResultTxt As String, OverDue As String, Today As String, ThisWeek As String, NextWeek As String
        End Class
        '20240814 -- Assignment Data
        Public Class clsQuizConfigVal
            Inherits clsMain
            Public Result As String, ResultTxt As String, MultiAmount As Integer, MultiSlowAmount As Integer, IsTest As Integer
        End Class
        Public Class clsDiscount
            Inherits clsMain
            Public Result As String, ResultTxt As String, DiscountId As String
        End Class
        Public Class clsSlip
            Inherits clsMain
            Public Result As String, ResultTxt As String, slipURL As String
        End Class
        Public Class clsNoti
            Inherits clsMain
            Public Result As String, ResultTxt As String, isNoti As String
        End Class

#End Region

    End Class
End Namespace