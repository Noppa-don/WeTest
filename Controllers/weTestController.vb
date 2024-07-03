Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Mvc
Imports System.Drawing
Imports System.Threading
Imports System.Net

Namespace Controllers
    Public Class weTestController
        Inherits mUtility
        Function weTest() As ActionResult
            Return View()
        End Function

#Region "Registration"
        Function Registration() As ActionResult
            Return View()
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

#End Region

#Region "Activity"
        Function Activity() As ActionResult
            Return View()
        End Function
#End Region

#Region "User"
        Function User() As ActionResult
            Return View()
        End Function
#End Region



    End Class

End Namespace