Imports System.Configuration.ConfigurationManager
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Net
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography
Imports System.Web.Mvc
Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json

Public Class mConnection
    Inherits Controller
    Public Function sqlCon(Optional ByVal strDB As String = Nothing) As String
        If strDB = Nothing Then strDB = ""
        Select Case strDB.ToString.ToLower
            Case "questionnaire"
                Return ConnectionStrings("connQuestionnaire").ConnectionString
            Case Else
                Return ConnectionStrings("portaltest").ConnectionString
        End Select
    End Function
    Public Function cmdSQL(ByVal cn As SqlConnection, ByVal strSQL As String) As SqlCommand
        Dim cmdSave As SqlCommand
        cmdSave = New SqlCommand(strSQL, cn)
        Return cmdSave
    End Function
    Public Function cmdSQL(ByVal cn As MySqlConnection, ByVal strSQL As String) As MySqlCommand
        Dim cmdSave As MySqlCommand
        cmdSave = New MySqlCommand(strSQL, cn)
        Return cmdSave
    End Function
    Public Function getDataTable(ByVal sqlCmd As SqlCommand) As DataTable
        Dim da As SqlDataAdapter
        Dim dt As New DataTable
        Try
            da = New SqlDataAdapter(sqlCmd)
            da.Fill(dt)
        Catch ex As Exception
        Finally
            If da IsNot Nothing Then da.Dispose() : da = Nothing
        End Try
        Return dt
    End Function
    Public Function getDataTable(ByVal sqlCmd As MySqlCommand) As DataTable
        Dim da As MySqlDataAdapter
        Dim dt As New DataTable
        Try
            da = New MySqlDataAdapter(sqlCmd)
            da.Fill(dt)
        Catch ex As Exception
        Finally
            If da IsNot Nothing Then da.Dispose() : da = Nothing
        End Try
        Return dt
    End Function
    Public Function cGuid(ByVal str As String) As System.Data.SqlTypes.SqlGuid
        Return CType(System.Data.SqlTypes.SqlGuid.Parse(str), System.Guid)
    End Function
    Sub writeLogs(cn As SqlConnection, subjectDtID As String, connectID As String, methodName As String, msg As String, isDetail As Boolean, isError As Boolean)
        Dim cmdTmp As SqlCommand
        Try
            If cn.State = 0 Then cn.Open()
            cmdTmp = cmdSQL(cn, "insert into appLogs (logID,appID,appName,version,subjectDtID,connectID,methodName,msg,isDetail,isError,sysDate) values (newid(),@appID,@appName,@version,@subjectDtID,@connectID,@methodName,@msg,@isDetail,@isError,getdate())")
            With cmdTmp
                If subjectDtID Is Nothing Then
                    .Parameters.Add("@subjectDtID", SqlDbType.UniqueIdentifier).Value = DBNull.Value
                Else
                    .Parameters.Add("@subjectDtID", SqlDbType.UniqueIdentifier).Value = cGuid(subjectDtID)
                End If
                If connectID Is Nothing Then
                    .Parameters.Add("@connectID", SqlDbType.UniqueIdentifier).Value = DBNull.Value
                Else
                    .Parameters.Add("@connectID", SqlDbType.UniqueIdentifier).Value = cGuid(connectID)
                End If
                .Parameters.Add("@methodName", SqlDbType.VarChar).Value = methodName
                .Parameters.Add("@appID", SqlDbType.UniqueIdentifier).Value = cGuid(Assembly.GetExecutingAssembly().GetCustomAttribute(Of GuidAttribute).Value)
                .Parameters.Add("@appName", SqlDbType.VarChar).Value = Assembly.GetExecutingAssembly().GetName().Name
                .Parameters.Add("@version", SqlDbType.VarChar).Value = Assembly.GetExecutingAssembly().GetName().Version
                .Parameters.Add("@msg", SqlDbType.VarChar).Value = msg
                .Parameters.Add("@isDetail", SqlDbType.Bit).Value = isDetail
                .Parameters.Add("@isError", SqlDbType.Bit).Value = isError
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            If Not Directory.Exists(Server.MapPath("~/DataFiles")) Then Directory.CreateDirectory(Server.MapPath("~/DataFiles"))
            If Not Directory.Exists(Server.MapPath("~/DataFiles/Logs")) Then Directory.CreateDirectory(Server.MapPath("~/DataFiles/Logs"))
            Dim fileLog As String = Server.MapPath("~/DataFiles/Logs/") & "Log_" & Now.ToString("yyyyMMdd") & ".txt"
            If Not System.IO.File.Exists(fileLog) Then System.IO.File.Create(fileLog).Dispose()
            Dim objWriter As New System.IO.StreamWriter(fileLog, True)
            objWriter.WriteLine(Now.ToString("dd/MM/yyyy HH:mm:ss:fff") & Space(5) & methodName & " : " & msg)
            objWriter.Close()
            objWriter = Nothing
            deleteFileLog()
        Finally
            If cmdTmp IsNot Nothing Then cmdTmp.Dispose() : cmdTmp = Nothing
        End Try
    End Sub
    Sub deleteFileLog()
        Dim directory As IO.DirectoryInfo
        Try
            directory = New IO.DirectoryInfo(Server.MapPath("~/DataFiles/Logs"))
            For Each file As IO.FileInfo In directory.GetFiles
                If (Now - file.CreationTime).Days > System.Configuration.ConfigurationManager.AppSettings("logDay") Then file.Delete()
            Next
        Catch ex As Exception
        Finally
            If directory IsNot Nothing Then directory = Nothing
        End Try
    End Sub
End Class
Public Class mEncryptedData
    Inherits mConnection
    Public Function oneWayKN(ByVal strPass As String)
        Dim md5Obj As New System.Security.Cryptography.MD5CryptoServiceProvider()
        Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strPass)
        bytesToHash = md5Obj.ComputeHash(bytesToHash)
        Dim strResult As String = ""
        For Each b As Byte In bytesToHash
            strResult += b.ToString("x2")
        Next
        md5Obj = Nothing
        bytesToHash = Nothing
        Return strResult
    End Function
    Public Function twoWay(ByVal StrED As String, ByVal str As String)
        Dim memStream As MemoryStream
        Dim rc2 As DESCryptoServiceProvider, iCryTF As ICryptoTransform, byteTmp() As Byte, myCrypt As CryptoStream
        Dim iv() As Byte = {28, 1, 19, 76, 20, 11, 4, 13}
        Try
            memStream = New MemoryStream
            rc2 = New DESCryptoServiceProvider
            rc2.Key = Encoding.UTF8.GetBytes("P@ssw0rd")
            rc2.IV = iv
            If StrED.ToUpper = "E" Then
                iCryTF = rc2.CreateEncryptor
                byteTmp = Encoding.UTF8.GetBytes(str)
            Else
                iCryTF = rc2.CreateDecryptor
                byteTmp = Convert.FromBase64String(str)
            End If
            myCrypt = New CryptoStream(memStream, iCryTF, CryptoStreamMode.Write)
            myCrypt.Write(byteTmp, 0, byteTmp.Length)
        Catch ex As Exception
        Finally
            If Not myCrypt Is Nothing Then myCrypt.Close() : myCrypt = Nothing
            If Not byteTmp Is Nothing Then byteTmp = Nothing
            If Not iCryTF Is Nothing Then iCryTF = Nothing
            If Not rc2 Is Nothing Then rc2 = Nothing
        End Try
        If StrED.ToUpper = "E" Then
            Return Convert.ToBase64String(memStream.ToArray())
        Else
            Return Encoding.UTF8.GetString(memStream.ToArray())
        End If
    End Function
End Class
Public Class mUtility
    Inherits mEncryptedData
    Public Class clsMain
        Public dataType As String, errorMsg As String
    End Class
    Function delFileName(isLike As Boolean, strFolder As String, strFileName As String)
        Dim returnText As String
        If Not Directory.Exists(Server.MapPath("~/DataFiles/" & strFolder & "/")) Then returnText = "directory not exist" : GoTo exitFunction
        Dim di As DirectoryInfo, fileList As FileInfo()
        Try
            di = New DirectoryInfo(Server.MapPath("~/DataFiles/" & strFolder))
            If isLike = True Then
                fileList = di.GetFiles(strFileName & "*.*")
            Else
                fileList = di.GetFiles(strFileName & ".*")
            End If
            For Each fInfo In fileList
                fInfo.Delete()
            Next
            returnText = "success"
        Catch ex As Exception
            returnText = ex.Message
        Finally
            di = Nothing : fileList = Nothing
        End Try
exitFunction:
        Return returnText
    End Function
    Public Shared Function resizeImage(ByVal image As Image, ByVal size As Size, Optional ByVal preserveAspectRatio As Boolean = True) As Image
        Dim newWidth As Integer
        Dim newHeight As Integer
        If preserveAspectRatio Then
            Dim originalWidth As Integer = image.Width
            Dim originalHeight As Integer = image.Height
            If size.Width > originalWidth And size.Height > originalHeight Then GoTo oriSize
            Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
            Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
            Dim percent As Single = If(percentHeight < percentWidth, percentHeight, percentWidth)
            newWidth = CInt(originalWidth * percent)
            newHeight = CInt(originalHeight * percent)
        Else
oriSize:
            newWidth = size.Width
            newHeight = size.Height
        End If
        Dim newImage As Image = New Bitmap(newWidth, newHeight)
        Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
            graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
        End Using
        Return newImage
    End Function
    Public Sub imageCompression(ByVal image As Image, ByVal szFileName As String, ByVal lCompression As Long, mimeType As String)
        Dim eps As EncoderParameters = New EncoderParameters(1)
        eps.Param(0) = New EncoderParameter(Encoder.Quality, lCompression)
        Dim ici As ImageCodecInfo = getEncoderInfo(mimeType)
        image.Save(szFileName, ici, eps)
    End Sub
    Public Function getEncoderInfo(ByVal mimeType As String) As ImageCodecInfo
        Dim j As Integer
        Dim encoders As ImageCodecInfo()
        encoders = ImageCodecInfo.GetImageEncoders()
        For j = 0 To encoders.Length
            If encoders(j).MimeType = mimeType Then
                Return encoders(j)
            End If
        Next j
        Return Nothing
    End Function
    Function sms(phoneNo As String, fromName As String, msg As String,
            Optional cn As SqlConnection = Nothing, Optional cmdMsSql As SqlCommand = Nothing, Optional strSql As String = Nothing)
        Dim httpWebRequest As HttpWebRequest, response As HttpWebResponse, returnText As String
        Dim ApiKey As String = "whIRYTWJkMat1SiuQBs1vhlw5kJ9ZCAw7PLcp5sNHs8=", ClientId As String = "6ee932cc-aba1-46b2-9b1e-e2f60dd239de"
        Dim url As String
        Try
            phoneNo = Mid(phoneNo, 2, phoneNo.Length - 1)
            url = "https://api.send-sms.in.th/api/v2/SendSMS?ApiKey=" & ApiKey & "&ClientId=" & ClientId & "&MobileNumbers=66" & phoneNo & "&SenderID=" & fromName & "&Message=" & msg & "&is_unicode=true"
            httpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
            httpWebRequest.Method = "get"
            response = CType(httpWebRequest.GetResponse(), HttpWebResponse)
            returnText = New StreamReader(response.GetResponseStream()).ReadToEnd()
            If cn IsNot Nothing Then
                cmdMsSql = cmdSQL(cn, strSql)
                With cmdMsSql
                    .Parameters.Add("@message", SqlDbType.VarChar).Value = returnText
                    .ExecuteNonQuery()
                End With
            End If
            If returnText.ToLower.IndexOf("success") = -1 Then
                returnText = "fail"
            Else
                returnText = "success"
            End If
skipFunction:
        Catch ex As Exception
            returnText = 0
        Finally
            httpWebRequest = Nothing : response = Nothing : ApiKey = Nothing : ClientId = Nothing : url = Nothing
        End Try
        Return returnText
    End Function
    Public Function sendNotificationFromOneSignal(userID As String, title As String, msg As String)
        Dim request As HttpWebRequest, result As Object,
                appID As String = "bd54bbc2-59ba-4b58-ad3a-435a08df1145", restAPIKey As String = "Basic Y2E4YTU3ZTctOGE3ZS00NzNmLTg4YzItZWE3M2U5NDUwMzU4"
        Dim returnText As String
        Try
            request = DirectCast(WebRequest.Create("https://onesignal.com/api/v1/notifications"), HttpWebRequest)
            request.ContentType = "application/json;charset=utf-8"
            request.Method = "POST"
            request.Headers.Add("authorization", restAPIKey)
            Dim json As String = "{""app_id"": """ & appID & """,""include_external_user_ids"": [""" & userID & """]" &
                    ",""headings"":{""en"":""" & title & """},""contents"": {""en"": """ & msg & """}" &
                    "}"
            Using streamWriter As New StreamWriter(request.GetRequestStream())
                streamWriter.Write(json)
                streamWriter.Flush()
                streamWriter.Close()
            End Using
            Dim httpResponse As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
            Using streamReader As New StreamReader(httpResponse.GetResponseStream())
                returnText = streamReader.ReadToEnd()
            End Using

            result = JsonConvert.DeserializeObject(returnText)
            returnText = result.ToString ' result("id")
        Catch ex As Exception
            returnText = ex.Message
        Finally
            request = Nothing : result = Nothing
        End Try
        Return returnText
    End Function

End Class