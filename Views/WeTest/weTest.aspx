<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/weTest/weTest.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <script src="/scripts/weTest/weTest.js?ver=1.1.1" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class='ui-content'>
<div class='header ui-row ui-show'>
	<div class='ui-column ui-line bar'>
		<div class='ui-column app ui-hide'><div class='ui-icon logo'></div><div class='ui-text'>WeService</div></div>
		<div class='ui-column menu'>
			<div class='ui-blank'></div><div class='language ui-icon thai ui-show'></div><div class='profile ui-icon' name='profile'></div><div class='ui-icon delete ui-hide'></div>
		</div>
	</div>
	<div class='ui-list ui-column ui-hide'><div class='ui-icon thai' name='thai' val='0'></div><div class='ui-icon english' name='english' val='1'></div><div class='ui-icon myanmar' name='myanmar' val='2'></div></div>
</div>
<div class='content main ui-show'>
	<div class='lv1 ui-column'></div>
</div>
<div class='footer ui-column'>
	<div class='ui-icon back'></div>
	<div class='ui-icon next'></div>
	<div class='ui-icon save'></div>
</div>
		<div name='alert' class='my-popup alert ui-popup-container ui-popup-hidden'>
			<div class='ui-content ui-body-f ui-corner-all ui-shadow'>
				<div class='ui-icon warning'></div>
				<div class='ui-text'></div>
				<div class='footer'><div class='ui-shadow ui-corner-all close'>ตกลง</div></div>
			</div>
		</div>
		<div name='confirm' class='my-popup confirm ui-popup-container ui-popup-hidden'>
			<div class='ui-content ui-body-f ui-corner-all ui-shadow'>
				<div class='ui-icon warning'></div>
				<div class='ui-text'></div>
				<div class='ui-column footer'><div class='ui-shadow ui-corner-all close'>ยกเลิก</div><div class='ui-shadow ui-corner-all ok'>ตกลง</div></div>
			</div>
		</div>
		<div name='choose' class='my-popup choose ui-popup-container ui-popup-hidden'>
			<div class='ui-content ui-body-f ui-corner-all ui-shadow'>
				<div class='ui-title ui-shadow ui-corner-all'></div>
				<div class='ui-list ui-row'>
					<div class='ui-shadow ui-corner-all jobType'>หลอดไฟเสีย</div>
					<div class='ui-shadow ui-corner-all jobType'>ไฟช็อต</div>
					<div class='ui-shadow ui-corner-all jobType'>ปลั๊กไฟเสีย</div>
					<div class='ui-shadow ui-corner-all jobType'>อื่นๆ โปรดระบุรายละเอียด</div>
					<div class='ui-shadow ui-corner-all location'>แผนก E-Zone</div>
					<div class='ui-shadow ui-corner-all location'>แผนก Prepress</div>
				</div>
			</div>
		</div>
		<div name='userRef' class='my-popup userRef ui-popup-container ui-popup-hidden'>
			<div class='ui-content ui-body-f ui-corner-all ui-shadow'>
				<div class='ui-title ui-shadow ui-corner-all'>เลือกชื่อ</div>
				<div class='ui-filter'><input type='search' placeholder='ค้นหาชื่อ - นามสกุล'/></div>
				<div class='ui-list ui-row'>
					<div class='ui-shadow ui-corner-all'>aaa bbbb</div>
					<div class='ui-shadow ui-corner-all'>ccc dddddddddd</div>
					<div class='ui-shadow ui-corner-all'>aaeee bbbb</div>
					<div class='ui-shadow ui-corner-all'>yyyy dddddddddd</div>
					<div class='ui-shadow ui-corner-all'>ooooooo bbbb</div>
					<div class='ui-shadow ui-corner-all'>pppp dddddddddd</div>
					<div class='ui-shadow ui-corner-all'>mmmm bbbb</div>
					<div class='ui-shadow ui-corner-all'>vvv dddddddddd</div>
					<div class='ui-shadow ui-corner-all'>vvasdfsadv dddddddddd</div>
					<div class='ui-shadow ui-corner-all'>Test</div>
				</div>
			</div>
		</div>
		<div name='fullImage' class='my-popup fullImage ui-popup-container ui-popup-hidden'>
			<div class='ui-content ui-body-f ui-corner-all ui-shadow'>
				<img src=''/>
			</div>
		</div>
		<div class='ui-hide'>
			<div class='count job'>0</div>
			<div class='file job'></div>
			<input id='sDate' data-role='datebox' type='text' placeholder='วันที่เริ่ม' data-options='{"mode":"datebox","closeCallback":"nextDate","overrideDateFormat": "%d/%m/%Y","overrideDateFieldOrder": ["d","m","y"]}'>
			<input id='fDate' data-role='datebox' type='text' placeholder='วันที่สิ้นสุด' data-options='{"mode":"datebox","overrideDateFormat": "%d/%m/%Y","overrideDateFieldOrder": ["d","m","y"]}'>
			<input id='shDate' data-role='datebox' type='text' placeholder='วันที่เริ่ม' data-options='{"mode":"datebox","closeCallback":"nextDate","overrideDateFormat": "%d/%m/%Y","overrideDateFieldOrder": ["d","m","y"]}'>
			<input id='fhDate' data-role='datebox' type='text' placeholder='วันที่สิ้นสุด' data-options='{"mode":"datebox","overrideDateFormat": "%d/%m/%Y","overrideDateFieldOrder": ["d","m","y"]}'>
		</div>
	</div>
</asp:Content>
