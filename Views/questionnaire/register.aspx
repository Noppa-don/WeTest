<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/questionnaire/register.css?ver=1.1.2" rel="stylesheet" type="text/css" />
    <script src="/scripts/questionnaire/register.js?ver=1.1.2" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class='wrapper'>
		<div class='ui-header'>
			<div class='ui-title'><div class='ui-text'>ลงทะเบียนผู้ใช้งานใหม่</div></div>
		</div>
		<div class='ui-body'>
			<div class='register'>
				<div class='ui-icon profile'></div>
				<div class='ui-line fName require'><div class='ui-label'>ชื่อ*</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' maxlength='50'/></div></div>
				<div class='ui-line lName require'><div class='ui-label'>นามสกุล*</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' maxlength='50'/></div></div>
				<div class='ui-line mobile require'><div class='ui-label'>เบอร์โทรศัพท์*</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' maxlength='10'/></div></div>
				<div class='ui-line email'><div class='ui-label'>E-Mail</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' maxlength='100'/></div></div>
				<div class='ui-line userName require'><div class='ui-label'>ชื่อผู้ใช้งาน*</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' maxlength='50'/></div></div>
				<div class='ui-line userPass require'><div class='ui-label'>Password*</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='password' maxlength='50'/></div></div>
				<div class='ui-line confPass require'><div class='ui-label'>ยืนยัน Password*</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='password' maxlength='50'/></div></div>
			</div>
			<div class='otp ui-hide'>
				<div class='ui-line title'>ระบบกำลังส่ง OTP ไปที่โทรศัพท์ของท่าน</div>
				<div class='ui-line title'>กรุณาใส่รหัส OTP แล้วกดยืนยัน</div>
				<div class='ui-line otp'><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' maxlength='6' placeholder='ใส่รหัส OTP ที่นี่'/></div></div>
				<div class='ui-line title small'>* หากรอสักครู่แล้วไม่ได้รับรหัส OTP</div>
				<div class='ui-line title small'>กรุณากดปุ่ม "ส่ง OTP อีกครั้ง"</div>
				<div class='ui-line btn'><button class='ui-btn ui-shadow ui-corner-all ui-mini reOtp' title='ส่ง OTP อีกครั้ง'>ส่ง OTP อีกครั้ง</button><button class='ui-btn ui-shadow ui-corner-all ui-mini ok'>ยืนยัน</button></div>
			</div>
		</div>
		<div class='ui-footer'>
			<div class='ui-line'><div class='ui-icon previous'></div><div class='ui-icon next'></div></div>
		</div>
	</div>
	<div name='confirm' class='my-popup confirm ui-popup-container ui-popup-hidden'>
		<div class='ui-content ui-body-c ui-corner-all ui-shadow'>
			<div class='ui-text'></div>
			<div class='ui-desc'></div>			
			<button class='ui-btn ui-shadow ui-corner-all ui-mini close'>ไม่ใช่</button>
			<button class='ui-btn ui-shadow ui-corner-all ui-mini ok'>ใช่</button>
		</div>
	</div>
	<div name='alert' class='my-popup alert ui-popup-container ui-popup-hidden'>
		<div class='ui-content ui-body-c ui-corner-all ui-shadow'>
			<div class='ui-icon warning'></div>
			<div class='ui-text'></div>
			<button class='ui-btn ui-shadow ui-corner-all ui-mini close'>ตกลง</button>
		</div>
	</div>
	<div class='ui-hide data'>
		<input type='file' id='profile' accept='image/*'/>
	</div>
</asp:Content>
