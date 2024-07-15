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
            Dim objList As New clsMain(), fileName As String, fiInfo As FileInfo, filePath As String, oriImage As Image, reImage As Image
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
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

                objList.dataType = "success"
                objList.errorMsg = ""
                L1.Add(objList)
                objList = Nothing

                Session("StudentId") = StdId

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
        Function UploadStudentPhoto()

            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Dim fileName As String, fiInfo As FileInfo, filePath As String, oriImage As Image, reImage As Image
            Try
                For i = 0 To Request.Files.Count - 1
                    Dim file As HttpPostedFileBase = Request.Files(i)
                    If Not file Is Nothing Then
                        fileName = Path.GetFileName(file.FileName)
                        fiInfo = New IO.FileInfo(fileName)
                        If Not Directory.Exists(Server.MapPath("~/WetestPhoto/UserPhoto/")) Then
                            Directory.CreateDirectory(Server.MapPath("~/WetestPhoto/UserPhoto/"))
                        End If
                        filePath = Server.MapPath("~/WetestPhoto/UserPhoto/" & Session("StudentId").ToString.ToLower & fiInfo.Extension)
                        If file.ContentLength < 200000 Then
                            file.SaveAs(filePath)
                        Else
                            oriImage = Image.FromStream(file.InputStream)
                            reImage = resizeImage(oriImage, New Size(1280, 1024))
                            imageCompression(reImage, filePath, 65, file.ContentType)
                        End If
                    Else
                        Dim copyToPath = Server.MapPath("~/WetestPhoto/UserPhoto/" & Session("StudentId").ToString.ToLower & ".png")
                        Dim fullFilePath = Server.MapPath("~/WetestPhoto/UserPhoto/dummyUser.png")

                        System.IO.File.Copy(fullFilePath, copyToPath)
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
        <AcceptVerbs(HttpVerbs.Post)>
        Function UploadDummyStudentPhoto()
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Try

                Dim copyToPath = Server.MapPath("~/WetestPhoto/UserPhoto/" & Session("StudentId").ToString.ToLower & ".png")
                Dim fullFilePath = Server.MapPath("~/WetestPhoto/UserPhoto/dummyUser.png")

                System.IO.File.Copy(fullFilePath, copyToPath)

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

        <AcceptVerbs(HttpVerbs.Post)>
        Function SendOTP()
            Dim L1 As New List(Of clsMain)
            Dim MobileNo As String = Request.Form("MobileNo")
            Dim OTPNum As String = Request.Form("OTPNum")

            Dim url As String

            Dim MyReq As WebRequest

            Dim MyRes As WebResponse

            Dim Rec As Stream

            Dim Reader As StreamReader

            Dim Content As String

            Dim Pos As Integer

            MobileNo = "66" + MobileNo.TrimStart("0")

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

            'Pos = 1

            Dim objList As New clsMain()
            If Pos > 0 Then
                objList.dataType = "success"
                objList.errorMsg = "success"

            Else
                objList.dataType = "error"
                objList.errorMsg = "ไม่สามารถส่งรหัส OTP ได้"

            End If
            L1.Add(objList)
            objList = Nothing
            Return Json(L1, JsonRequestBehavior.AllowGet)

        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function UpdateOTPStatus()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "Update tblStudent set OTPConfirm = @OTPStatus where studentId = @StudentID")
                With cmdMsSql
                    .Parameters.Add("@StudentID", SqlDbType.VarChar).Value = Session("StudentID")
                    .Parameters.Add("@OTPStatus", SqlDbType.VarChar).Value = Request.Form("OTPStatus")
                    .ExecuteNonQuery()
                End With

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

        <AcceptVerbs(HttpVerbs.Post)>
        Function GetPackagePrice()
            Dim L1 As New List(Of clsMain)
            Dim objList As New clsMain()
            Dim PackagePrice = ConfigurationManager.AppSettings("PackagePrice")
            objList.dataType = PackagePrice
            L1.Add(objList)
            objList = Nothing

            Return Json(L1, JsonRequestBehavior.AllowGet)
        End Function
        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckKeyCode()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select Keycode from tblKeycode where KeyCode =@KeyCode and IsActive = 1")
                With cmdMsSql
                    .Parameters.Add("@KeyCode", SqlDbType.VarChar).Value = Request.Form("KeyCode")
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                If dt.Rows.Count = 0 Then
                    objList.dataType = "error"
                    objList.errorMsg = "WeTest Key is wrong !<br><br>Please try again or contact @Italt."
                Else
                    objList.dataType = "success"
                    objList.errorMsg = "Registration Done !<br><br>Do you want go to Placement Test now ?. "

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
        Function CheckDiscount()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "select DiscountId,DiscountType,DiscountAmount,case when expiredate >= getdate() then 0 else 1 end as isExpired,isUsed
                                        from tblDiscount where IsActive = 1 and DiscountCode = @DiscountCode")
                With cmdMsSql
                    .Parameters.Add("@DiscountCode", SqlDbType.VarChar).Value = Request.Form("DiscountCode")
                    .ExecuteNonQuery()
                End With
                dt = getDataTable(cmdMsSql)
                If dt.Rows.Count = 0 Then
                    objList.dataType = "error"
                    objList.errorMsg = "This code does not apply to this promotion!<br>Please try again or Contact us @Italt<br><br>"
                ElseIf dt.Rows(0)("isExpired").ToString = "1" Then
                    objList.dataType = "Expired"
                    objList.errorMsg = "This code is expired!<br>Please try again or Contact us @Italt<br><br>"
                ElseIf dt.Rows(0)("isUsed").ToString = "True" Then
                    objList.dataType = "Used"
                    objList.errorMsg = "This code has alrady beeen used!<br>Please try again or contact @Italt<br><br>"
                Else
                    objList.dataType = "Success"
                    Dim NetPrice As String = "0"
                    Dim PackagePrice = ConfigurationManager.AppSettings("PackagePrice")

                    If dt.Rows(0)("DiscountType").ToString = "1" Then
                        NetPrice = (CInt(PackagePrice) - CInt(dt.Rows(0)("DiscountAmount"))).ToString
                    Else
                        NetPrice = (CInt(PackagePrice) - ((CInt(PackagePrice) * CInt(dt.Rows(0)("DiscountAmount"))) / 100)).ToString
                    End If

                    objList.errorMsg = NetPrice
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
                    objList.dataType = "error"
                    objList.errorMsg = Result
                    L1.Add(objList)
                    objList = Nothing
                End If


            Catch ex As Exception
                objList.dataType = "error"
                objList.errorMsg = ex.Message
                L1.Add(objList)
                objList = Nothing
            End Try
            Return Json(L1, JsonRequestBehavior.AllowGet)
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
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, " Select QuizScoreId from tblQuizScore  where QuizId = @QuizId and QuestionId = @QuestionId;")

                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = Session("QuizId").ToString
                    .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = Request.Form("QuestionId")
                    .ExecuteNonQuery()
                End With

                Dim dtScore As DataTable = getDataTable(cmdMsSql)

                If dtScore.Rows.Count <> 0 Then
                    cmdMsSql = cmdSQL(cn, "update tblQuizScore set AnswerId = @AnsweredId,ResponseAmount = ResponseAmount + 1,Score = a.AnswerScore,LastUpdate = GETDATE()
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
                If Session("QuizMode").ToString = 1 Then
                    objList.dataType = "pmt"
                    objList.errorMsg = "Do you want to send placement test ?"
                ElseIf Session("QuizMode").ToString = 2 Then
                    objList.dataType = "practice"
                    objList.errorMsg = "Do you want to send Practice ?"
                ElseIf Session("QuizMode").ToString = 3 Then
                    objList.dataType = "exam"
                    objList.errorMsg = "Do you want to send Exam ?"
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
        Function EndQuiz()
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As New DataTable, dtScore As New DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                'Update Score

                cmdMsSql = cmdSQL(cn, "Select sum(score) As TotalScore,cast((sum(score)*100)/FullScore As Decimal(18,2)) As PercentScore from tblQuizScore qs inner join tblquiz q On q.quizid = qs.quizid 
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
            Dim L1 As New List(Of clsMain)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As New DataTable, dtAnswered As New DataTable
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                'select count(qs.quizscoreId) from tblQuizScore qs where QuizId = 'DFD6C669-9696-4F2E-97EE-0B8EC34D4760'

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


                objList.dataType = "success"
                objList.errorMsg = PercentAnswered
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
        Function GetChoicePanel()
            Dim L1 As New List(Of clsLeapChoiceData)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsLeapChoiceData()
            Dim LeapChoicetxt As String = ""
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                '20240712 -- เพิ่ม Query ตาม ChoiceMode เพื่อตรวจสอบว่าแสดงข้อข้ามหรือแสดงเฉลยข้อสอบ
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
                cmdMsSql = cmdSQL(cn, "select sum(a.answerScore) as fullscore from tblanswer a 
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
                    QuizData.resultMsg = "Not have Question for PlacementTest"
                Else

                    Dim Fullscore As String = dt(0)(0).ToString

                    cmdMsSql = cmdSQL(cn, "insert into tblquiz(quizId,testsetid,starttime,QuizMode,FullScore) values(@QuizId,@TestsetId,getdate(),@QuizMode,@FullScore);
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
                    End With

                    dt = getDataTable(cmdMsSql)

                    QuizData.QuizId = NewQuizId.ToLower
                    QuizData.FullScore = Fullscore
                    QuizData.QuestionAmount = dt(0)(0)
                    QuizData.resultType = "success"

                    Session("QuizId") = NewQuizId
                    Session("QuestionAmount") = dt(0)(0)
                    Session("QuizMode") = QuizData.QuizMode

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

                Dim QExplain As String = dtQuestion(0)("QuestionExpain_Quiz").ToString
                QExplain = QExplain.Replace("___MODULE_URL___", GenFilePath(QSetId))
                QExplain = "<br><div class=""ExplainQ ui-hide"">" & QExplain & "</div>"

                Dim QName As String = dtQuestion(0)("QuestionName_Quiz").ToString
                QName = QName.Replace("___MODULE_URL___", GenFilePath(QSetId))
                QName = "<div class=""fistflexdiv"">" & dtQuestion(0)("QQNo").ToString & ".</div><div><div>" & QName & "</div>" & QExplain & "</div>"

                Dim objListQuestion As New clsItemQAndA()
                objListQuestion.ItemType = "1"
                objListQuestion.ItemId = dtQuestion(0)("QuestionId").ToString
                objListQuestion.Itemtxt = QName

                cmdMsSql = cmdSQL(cn, "select MultimediaObjId,MfileName from tblMultimediaObject where ReferenceId = @QuestionId and ReferenceType = 1 and isactive = 1;")

                With cmdMsSql
                    .Parameters.Add("@QuestionId", SqlDbType.VarChar).Value = QuestionId
                End With

                Dim dtmulti As DataTable = getDataTable(cmdMsSql)

                If dtmulti.Rows.Count <> 0 Then

                    Dim FullPath As String

                    FullPath = GetFullPath(QSetId).Replace("\", "/")

                    Dim FPath As String = "../file" & FullPath & "/" & dtmulti(0)("MfileName").ToString

                    objListQuestion.multiname = dtmulti(0)("MultimediaObjId").ToString
                    objListQuestion.multipath = FPath
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

                Dim objList2 As New clsItemQAndA()
                Dim AnsHtml As String = ""

                Dim ArrChoicetxt() As String = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j"}

                For i = 0 To dtAnswer.Rows.Count - 1
                    Dim IsAnswered As String = ""

                    If Session("QuizState") IsNot Nothing Then
                        If CInt(dtAnswer(i)("AnswerScore")) > 0 Then
                            IsAnswered = "RightAns"
                        End If

                        If (dtAnswer(i)("AnswerId").ToString = dtAnswer(i)("UserAnswered").ToString) AndAlso CInt(dtAnswer(i)("UserScore")) <= 0 Then
                            IsAnswered = "WrongAns"
                        End If

                    Else
                        If dtAnswer(i)("AnswerId").ToString = dtAnswer(i)("UserAnswered").ToString Then
                            IsAnswered = "Answered"

                        End If
                    End If

                    Dim Aname As String = dtAnswer(i)("AnswerNameQuiz").ToString
                    Aname = Aname.Replace("___MODULE_URL___", GenFilePath(QSetId))

                    Dim AExplain As String = dtAnswer(i)("AnswerExpainQuiz").ToString
                    AExplain = AExplain.Replace("___MODULE_URL___", GenFilePath(QSetId))
                    AExplain = "<br><div class=""ExplainQ ui-hide"">" & AExplain & "</div>"

                    Aname = Aname & AExplain

                    '20240712 -- ปรับการแสดงเฉลยคำตอบ
                    If i Mod 2 = 0 Then
                        AnsHtml &= "<div Class=""divAnswerRow"">
                                    <div Class=""divAnswerbar flexdiv Left " & IsAnswered & """ QId=""" & QuestionId & """ AnsId=""" & dtAnswer(i)("AnswerId").ToString & """>" &
                          "<div class=""fistflexdiv"">" & ArrChoicetxt(i) & ".</div><div>" & Aname & "</div></div>"
                    Else
                        AnsHtml &= "<div Class=""divAnswerbar flexdiv Right " & IsAnswered & """ QId=""" & QuestionId & """ AnsId=""" & dtAnswer(i)("AnswerId").ToString & """>" &
                            "<div class=""fistflexdiv""> " & ArrChoicetxt(i) & ".</div><div>" & Aname & "</div></div></div>"
                    End If

                    objList2.ItemType = "2"
                    objList2.Itemtxt = AnsHtml

                    cmdMsSql = cmdSQL(cn, "select MultimediaObjId,MfileName from tblMultimediaObject where ReferenceId = @AnswerId and ReferenceType = 1 and isactive = 1;")

                    With cmdMsSql
                        .Parameters.Add("@AnswerId", SqlDbType.VarChar).Value = dtAnswer(i)("AnswerId").ToString
                    End With

                    Dim dtAmulti As DataTable = getDataTable(cmdMsSql)

                    If dtAmulti.Rows.Count <> 0 Then

                        Dim FullPath As String

                        FullPath = GetFullPath(QSetId).Replace("\", "/")

                        Dim FPath As String = "../file" & FullPath & "/" & dtmulti(0)("MfileName").ToString

                        objList2.multiname = dtmulti(0)("MultimediaObjId").ToString
                        objList2.multipath = FPath
                    End If


                    L1.Add(objList2)
                Next

                objList2 = Nothing

                SaveQuizScore(QuestionId)

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


                cmdMsSql = cmdSQL(cn, "Insert Into tblStudentLevel(StudentId,LevelId) values(@StudentId,@LevelRegister);")
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

                Dim PassExamScore As Integer = CInt(ConfigurationManager.AppSettings("PassExamPercent"))

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
                cmdMsSql = cmdSQL(cn, "select StudentId,Firstname from tblStudent where Username = @Username and Password = @Password and IsActive = 1")
                With cmdMsSql
                    .Parameters.Add("@Username", SqlDbType.VarChar).Value = Request.Form("Username")
                    .Parameters.Add("@Password", SqlDbType.VarChar).Value = oneWayKN(Request.Form("Password"))
                    .ExecuteNonQuery()
                End With

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count = 0 Then
                    objList.Result = "error"
                    objList.Msg = "Username or Password is wrong !<br><br>Please try again or contact @Italt."
                Else
                    objList.Result = "success"
                    objList.UsrPhoto = "<div class=""UserPhoto"" style=""background: url(/WetestPhoto/UserPhoto/" & dt(0)("StudentId").ToString & ".png);"">"
                    objList.Firstname = dt(0)("Firstname")

                    Session("studentId") = dt(0)("StudentId").ToString
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

        <AcceptVerbs(HttpVerbs.Post)>
        Function CheckLoginStatus()
            Dim L1 As New List(Of clsStudentData)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim objList As New clsStudentData()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                If Session("studentId") IsNot Nothing Then
                    cmdMsSql = cmdSQL(cn, "select StudentId,Firstname from tblStudent where StudentId = @stdId;")
                    With cmdMsSql
                        .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("studentId")
                        .ExecuteNonQuery()
                    End With

                    dt = getDataTable(cmdMsSql)

                    If dt.Rows.Count = 0 Then
                        objList.Result = "error"
                    Else
                        objList.Result = "success"
                        objList.UsrPhoto = "<div class=""UserPhoto"" style=""background: url(/WetestPhoto/UserPhoto/" & dt(0)("StudentId").ToString & ".png);"">"
                        objList.Firstname = dt(0)("Firstname")
                        Session("studentId") = dt(0)("StudentId").ToString
                    End If
                Else
                    objList.Result = "error"
                    objList.Msg = "sessionlost"
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
#End Region

#Region "Practice"
        Function Practice() As ActionResult
            Return View()
        End Function

        <AcceptVerbs(HttpVerbs.Post)>
        Function GetLesson()
            Dim L1 As New List(Of clsPracticeSet)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable

            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================


                Dim skillName() As String = {"Reading", "Listening", "Grammar", "Situation", "Vocabulary"}

                '20240712 -- ปรับ Query ให้ดึงเฉพาะระดับชั้นที่ตรงและน้อยกว่าที่ User สามารถเล่นได้
                For i = 0 To skillName.Count() - 1

                    Dim Skn As String = skillName(i)

                    cmdMsSql = cmdSQL(cn, "Select row_number() over(order by testsetid)As TestsetNo,TestsetId
                                            From tblTestset t inner Join tbllevel l on t.LevelId = l.LevelId
                                            Where testsetname Like '%' + @skillName + '%' and levelno <= (select top 1 levelno from tbllevel l left join tblStudentLevel sl on l.LevelId = sl.LevelId 
                                            where studentId = @stdId)  and t.isactive = 1 and l.isactive = 1 order by TestsetId")
                    With cmdMsSql
                        .Parameters.Add("@skillName", SqlDbType.VarChar).Value = Skn
                        .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId").ToString
                        .ExecuteNonQuery()
                    End With

                    dt = getDataTable(cmdMsSql)

                    Dim skilltxt As String
                    Dim skilltxtShort As String

                    If dt.Rows.Count() <> 0 Then
                        skilltxt = ""
                        skilltxtShort = ""
                        For j = 0 To dt.Rows.Count() - 1
                            skilltxt &= "<div id=" & dt.Rows(j)("TestsetId").ToString & " class=""Lessondiv Lesson" & Skn & """>" & dt.Rows(j)("TestsetNo").ToString & "</div>"
                            If j = 4 Then
                                skilltxtShort = skilltxt
                            End If
                        Next
                        Dim objList As New clsPracticeSet()
                        objList.skillSet = Skn
                        objList.skillTxtAll = skilltxt
                        objList.skillTxtShort = skilltxtShort
                        objList.skillAmount = dt.Rows.Count()
                        L1.Add(objList)
                        objList = Nothing
                    Else
                        Dim objList As New clsPracticeSet()
                        objList.skillSet = Skn
                        objList.skillTxtAll = ""
                        objList.skillTxtShort = ""
                        objList.skillAmount = dt.Rows.Count()
                        L1.Add(objList)
                        objList = Nothing
                    End If
                Next

            Catch ex As Exception
                Dim objList As New clsPracticeSet()
                objList.skillSet = "error"
                objList.skillTxtAll = ex.Message
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


                Dim QuizData As New clsQuizData

                QuizData.QuizMode = "2"
                QuizData.TestsetId = TestsetId

                CreateNewQuiz(QuizData)

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
#End Region

#Region "Class"
        Private Class clsStudentData
            Inherits clsMain
            Public Firstname As String, UsrPhoto As String, Result As String, Msg As String
        End Class
        Private Class clsItemQAndA
            Inherits clsMain
            Public ItemType As String, ItemNo As String, ItemId As String, Itemtxt As String, ItemStatus As String, multiname As String, multipath As String
        End Class
        Public Class clsQuizData
            Inherits clsMain
            Public QuizId As String, QuizMode As String, TestsetId As String, FullScore As String, QuestionAmount As String, resultType As String, resultMsg As String
        End Class
        Public Class clsPracticeSet
            Inherits clsMain
            Public skillSet As String, skillTxtShort As String, skillTxtAll As String, skillAmount As String
        End Class
        Public Class clsLeapChoiceData
            Inherits clsMain
            Public result As String, leapChoicetxt As String, allPage As String
        End Class
#End Region

    End Class
End Namespace