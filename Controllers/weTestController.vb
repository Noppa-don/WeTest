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
        Function SendOTP()
            Dim L1 As New List(Of clsMain)
            'Dim MobileNo As String = Request.Form("MobileNo")
            'Dim OTPNum As String = Request.Form("OTPNum")
            'Dim b As String
            'b = (CInt(Math.Ceiling(Rnd() * 6)) + 6).ToString

            'Dim url As String

            'Dim MyReq As WebRequest

            'Dim MyRes As WebResponse

            'Dim Rec As Stream

            'Dim Reader As StreamReader

            'Dim Content As String

            Dim Pos As Integer

            'MobileNo = "66" + MobileNo.TrimStart("0")

            'Dim apiKey As String = "whIRYTWJkMat1SiuQBs1vhlw5kJ9ZCAw7PLcp5sNHs8="

            'Dim clientID As String = "6ee932cc-aba1-46b2-9b1e-e2f60dd239de"

            'url = "https://api.send-sms.in.th/api/v2/SendSMS?SenderID=WeTell&Message=" & "OTP code for Wetest : " & OTPNum & "&MobileNumbers=" + MobileNo + "&ApiKey=" & apiKey & "&ClientId=" & clientID & "&is_unicode=true"

            'MyReq = WebRequest.Create(url)

            'MyReq.ContentLength = 0

            'MyReq.Method = "GET"

            'MyReq.ContentType = "application/json; charset=utf-8"

            'MyRes = MyReq.GetResponse

            'Rec = MyRes.GetResponseStream

            'Reader = New StreamReader(Rec, Encoding.UTF8)

            'Content = Reader.ReadToEnd

            'Pos = Content.IndexOf("Success", 0)

            Pos = 1

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
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dt As DataTable
            Dim NewQuizId As String = Guid.NewGuid.ToString
            Dim objList As New clsMain()
            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================
                cmdMsSql = cmdSQL(cn, "Select top 1 testsetId from tbltestset where isPlacementTest = 1 And levelid = 'E5DBFA06-C4CE-4CE2-9F47-60E9CB99A38C' order by newid()")

                dt = getDataTable(cmdMsSql)

                If dt.Rows.Count = 0 Then
                    objList.dataType = "error"
                    objList.errorMsg = "Dont have testset from placementtest!"
                Else
                    Dim QuizData As New clsQuizData

                    QuizData.QuizType = "1"
                    QuizData.TestsetId = dt(0)(0).ToString

                    CreateNewQuiz(QuizData)

                    If QuizData.resultType = "success" Then
                        cmdMsSql = cmdSQL(cn, "Insert into tblPlacementTest(quizid,pmtnum,levelid,fullscore)values (@QuizId,1,'E5DBFA06-C4CE-4CE2-9F47-60E9CB99A38C',@FullScore);")
                        With cmdMsSql
                            .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = QuizData.QuizId
                            .Parameters.Add("@FullScore", SqlDbType.VarChar).Value = QuizData.FullScore
                            .ExecuteNonQuery()
                        End With

                    End If

                    objList.dataType = "success"
                    L1.Add(objList)
                    objList = Nothing
                End If
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

#Region "Activity"
        Function Activity() As ActionResult
            Return View()
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

                    cmdMsSql = cmdSQL(cn, "insert into tblquiz(quizId,testsetid,starttime,QuizMode) values(@QuizId,@TestsetId,getdate(),1);
                                           insert into tblquizSession(quizid, studentid)values(@QuizId,@stdId);
                                           insert into tblquizQuestion select newid(),@QuizId,questionId,ROW_NUMBER() over (order by newid()),1,getdate() 
                                           from tbltestsetquestiondetail tsqd inner join tbltestsetquestionset tsqs on tsqd.tsqsid = tsqs.tsqsid
                                           where tsqd.isactive = 1 and tsqs.isactive = 1 and tsqs.testsetid = @TestsetId;
                                           insert into tblQuizAnswer select newid(),@QuizId,qq.QuestionId,AnswerId,
                                           ROW_NUMBER() OVER(PARTITION BY qq.questionId ORDER BY newid()),1,getdate() 
                                           from tblAnswer a inner join tblQuizQuestion qq on a.QuestionId = qq.QuestionId where a.IsActive = 1;
                                           select count(QuestionId) as QuestionAmount from tblQuizQuestion where QuizId = @QuizId"
                                      )

                    With cmdMsSql
                        .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = NewQuizId.ToLower
                        .Parameters.Add("@FullScore", SqlDbType.VarChar).Value = Fullscore
                        .Parameters.Add("@TestsetId", SqlDbType.VarChar).Value = QuizData.TestsetId.ToLower
                        .Parameters.Add("@stdId", SqlDbType.VarChar).Value = Session("StudentId").ToString.ToLower
                    End With

                    dt = getDataTable(cmdMsSql)

                    QuizData.QuizId = NewQuizId.ToLower
                    QuizData.FullScore = Fullscore
                    QuizData.QuestionAmount = dt(0)(0)
                    QuizData.resultType = "success"

                    Session("QuizId") = NewQuizId
                    Session("QuestionAmount") = dt(0)(0)
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
            Dim QuizId As String = Session("QuizId")
            'Dim QuizId As String = "E40DD92D-3C67-43FA-8FD6-AC0EFD1D1A00"
            Dim QuestionNo As String = 1

            Dim ActionType = Request.Form("ActionType")

            If Session("QuestionNo") IsNot Nothing Then
                If ActionType = "next" Then
                    Session("QuestionNo") = CInt(Session("QuestionNo")) + 1
                Else
                    Session("QuestionNo") = CInt(Session("QuestionNo")) - 1
                End If
            Else
                Session("QuestionNo") = 1
            End If

            QuestionNo = Session("QuestionNo")

            Dim L1 As New List(Of clsItemQAndA)
            Dim cn As SqlConnection, cmdMsSql As SqlCommand, dtQuestion As DataTable, dtAnswer As DataTable

            Try
                ' ================ Check Permission ================
                cn = New SqlConnection(sqlCon("Wetest"))
                If cn.State = 0 Then cn.Open()
                ' ==================================================

                cmdMsSql = cmdSQL(cn, "select qq.QQNo,qq.QuestionId,q.QuestionName_Quiz from tblQuestion q inner join tblQuizQuestion qq 
                                        on q.QuestionId = qq.QuestionId where quizid = @QuizId and QQNo = @QuestionNo")
                With cmdMsSql
                    .Parameters.Add("@QuizId", SqlDbType.VarChar).Value = QuizId
                    .Parameters.Add("@QuestionNo", SqlDbType.VarChar).Value = QuestionNo
                End With

                dtQuestion = getDataTable(cmdMsSql)

                Dim objListQuestion As New clsItemQAndA()
                objListQuestion.ItemType = "1"
                objListQuestion.ItemId = dtQuestion(0)("QuestionId").ToString
                objListQuestion.Itemtxt = dtQuestion(0)("QQNo").ToString & ". " & dtQuestion(0)("QuestionName_Quiz").ToString

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

                Dim QuestionId As String = dtQuestion(0)("QuestionId").ToString

                cmdMsSql = cmdSQL(cn, "Select distinct qa.qano,qa.AnswerId,a.AnswerNameQuiz from tblQuizAnswer QA inner join tblAnswer A on qa.AnswerId = a.AnswerId  
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

                    If i Mod 2 = 0 Then
                        AnsHtml &= "<div Class=""divAnswerRow""><div Class=""divAnswerLeft"" AnsId=""" & dtAnswer(i)("AnswerId").ToString & """>" & ArrChoicetxt(i) & ". " & dtAnswer(i)("AnswerNameQuiz").ToString & "</div>"
                    Else
                        AnsHtml &= "<div Class=""divAnswerRight"" AnsId=""" & dtAnswer(i)("AnswerId").ToString & """>" & ArrChoicetxt(i) & ". " & dtAnswer(i)("AnswerNameQuiz").ToString & "</div></div>"
                    End If

                    objList2.ItemType = "2"
                    objList2.Itemtxt = AnsHtml
                    L1.Add(objList2)
                Next

                objList2 = Nothing

            Catch ex As Exception
                Dim objList As New clsItemQAndA()
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
#End Region
        Private Class clsStudentData
            Inherits clsMain
            Public Firstname As String, UsrPhoto As String, Result As String, Msg As String
        End Class
        Private Class clsItemQAndA
            Inherits clsMain
            Public ItemType As String, ItemNo As String, ItemId As String, Itemtxt As String, ItemStatus As String
        End Class

        Public Class clsQuizData
            Inherits clsMain
            Public QuizId As String, QuizType As String, TestsetId As String, FullScore As String, QuestionAmount As String, resultType As String, resultMsg As String
        End Class
    End Class
End Namespace