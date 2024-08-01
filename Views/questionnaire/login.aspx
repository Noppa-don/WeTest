<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/questionnaire/login.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <script src="/scripts/questionnaire/login.js?ver=1.1.1" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class='wrapper'>
		<div class='logo'></div>
		<div class='login'>
			<div>ชื่อผู้ใช้</div>
            <div><input type='text' id='userName' title='ชื่อผู้ใช้'/></div>
			<div>รหัสผ่าน</div>
            <div class='last'><input type='password' id='userPass' title='รหัสผ่าน'/></div>
			<div class='textRight'>ลืมรหัสผ่าน</div>
			<p>&nbsp;</p>
			<div class='textRight register'>ลงทะเบียนใหม่ ?</div>
			<div class='btn'><button class='ui-btn ui-shadow ui-corner-all ui-mini ui-btn-active' title='เข้าสู่ระบบ'>เข้าสู่ระบบ</button></div>
		</div>
		<div name='alert' class='my-popup alert ui-popup-container ui-popup-hidden'>
			<div class='ui-content ui-body-c ui-corner-all ui-shadow'>
				<div class='ui-icon Wrong'></div>
				<div class='ui-text'></div>
				<button class='btn btnActive'>ตกลง</button>
			</div>
		</div>
	</div>
</asp:Content>
