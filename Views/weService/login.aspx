<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/weService/login.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <script src="/scripts/weService/login.js?ver=1.1.1" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class='wrapper'>
		<div class='ui-head'>
			<div class='title'>WeService</div>
			<div>
				<div class='language ui-icon thai'></div>
				<div class='logo'></div>
				<div class='ui-choose ui-hide'><div class='ui-icon thai' name='thai' val='0'></div><div class='ui-icon english' name='english' val='1'></div><div class='ui-icon myanmar' name='myanmar' val='2'></div></div>
			</div>
		</div>
		<div class='login'>
			<div class='ui-label'>กรุณาลงชื่อเข้าสู่ระบบ</div>
			<div class='box'><input type='text' id='userName' placeholder='ชื่อผู้ใช้'/></div>
			<div class='box'><input type='password' id='userPass' placeholder='รหัสผ่าน'/></div>
			<div class='forgot textRight ui-hide'>ลืมรหัสผ่าน</div>
			<p>&nbsp;</p>
			<div class='btn'><button class='ui-btn ui-shadow ui-corner-all ui-mini ui-btn-active'>เข้าสู่ระบบ</button></div>
		</div>
		<div name='alert' class='my-popup alert ui-popup-container ui-popup-hidden'>
			<div class='ui-content ui-body-c ui-corner-all ui-shadow'>
				<div class='ui-icon warning'></div>
				<div class='ui-text'></div>
				<button class='ui-btn ui-shadow ui-corner-all ui-mini close'>ตกลง</button>
			</div>
		</div>
	</div>
</asp:Content>
