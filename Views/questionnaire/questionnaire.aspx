<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/questionnaire/questionnaire.css?ver=1.1.7" rel="stylesheet" type="text/css" />
    <script src="/scripts/questionnaire/questionnaire.js?ver=1.1.7" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class='wrapper'>
		<div class='main'>
			<div class='ui-header'>
				<div class='ui-blank'></div>
				<div class='ui-bar'><div class='ui-icon home'></div><div class='ui-icon profile'></div></div>
				<div class='ui-title'><div class='ui-icon info ui-hide' name='information'></div><div class='ui-text ui-btn-active'></div><div class='ui-icon inbox empty'></div></div>
			</div>
			<div class='ui-body'>
				<div class='ui-menu'>
					<div class='menu'></div>
					<button class='ui-btn ui-shadow ui-corner-all ui-mini ui-btn-active history'></button>
				</div>
				<div class='questionnaire lv1 ui-hide'>
					<div class='ui-line'><div class='date'><div class='ui-label'>วันที่</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'>&nbsp;</div><div class='ui-icon calender'></div></div><div class='time'><div class='ui-label'>เวลา</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'>&nbsp;</div><div class='ui-icon clock'></div></div></div>
					<div class='ui-line'>
						<div class='person'>
							<div class='ui-label'>ชื่อผู้เกี่ยวข้อง</div>
							<div class='ui-text ui-body-c ui-shadow ui-corner-all ui-blank'></div>
						</div>
					</div>
					<div class='ui-line'><div class='eventName'><div class='ui-label'>ชื่องาน/สถานการณ์</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' maxlength='255'/></div></div></div>
					<div class='ui-line ui-hide'><div class='docNo'><div class='ui-label'>เลขงาน/เลขอ้างอิง</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'>&nbsp;</div></div></div>
					<div class='ui-line'><div class='locationType'><div class='ui-label'>ตำแหน่งของเหตุการณ์</div><div class='ui-text ui-slider-track ui-body-c ui-shadow ui-corner-all ui-left'><div class='choice ui-btn-active' xid='in'>ภายใน</div></div><div class='ui-text ui-slider-track ui-body-c ui-shadow ui-corner-all ui-right'><div class='choice' xid='out'>ภายนอก</div></div></div></div>
					<div class='ui-line ui-hide'><div class='eventAddress'><div class='ui-label'>ตำแหน่งของเหตุการณ์</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' maxlength='300'/></div></div></div>
					<div class='ui-line'><div class='image'><div class='ui-label'>ถ่ายรูป/แนบรูป</div><div class='ui-text ui-body-c ui-shadow ui-corner-all ui-blank'></div></div></div>
					<div class='qList'></div>
					<div class='ui-line'><div class='qPlus'><button class='ui-btn ui-shadow ui-corner-all ui-mini ui-btn-active'>ต้องการทำเพิ่มเติม</button></div></div>
				</div>
				<div class='questionnaire lv2 ui-hide'><div class='qList'></div></div>
				<div class='summary ui-hide'></div>
			</div>
			<div class='ui-footer ui-hide'>
				<div class='ui-line'><div class='ui-icon previous'></div><div class='ui-icon next'></div></div>
			</div>
		</div>
		<div name='summary' class='detail summary ui-page-theme-c ui-hide'>
			<div class='ui-header'>
				<div class='ui-blank'></div>
				<div class='ui-title ui-bar-c'><div class='ui-text'>รายละเอียด</div><div class='ui-icon close'></div></div>
			</div>
			<div class='ui-body'></div>
		</div>
		<div name='question' class='detail question ui-page-theme-c ui-hide'>
			<div class='ui-header'>
				<div class='ui-blank'></div>
				<div class='ui-title ui-bar-c'><div class='ui-text'>รายละเอียด</div><div class='ui-icon close'></div></div>
			</div>
			<div class='ui-body'></div>
		</div>
		<div name='event' class='detail event ui-page-theme-c ui-hide'>
			<div class='ui-header'>
				<div class='ui-blank'></div>
				<div class='ui-title ui-bar-c'><div class='ui-text'></div><div class='ui-icon close'></div></div>
			</div>
			<div class='ui-filter'>
				<div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' placeholder='ค้นหาเลขงาน/ชื่องาน'/></div><div class='ui-icon search'></div>
			</div>
			<div class='ui-body'></div>
		</div>
		<div name='person' class='detail person ui-page-theme-c ui-hide'>
			<div class='ui-header'>
				<div class='ui-blank'></div>
				<div class='ui-title ui-bar-c'><div class='ui-text'>เลือกชื่อผู้เกี่ยวข้อง</div><div class='ui-icon close'></div></div>
			</div>
			<div class='ui-filter'>
				<div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' placeholder='ค้นหา ชื่อ / นามสกุล'/></div><div class='ui-icon search'></div>
			</div>
			<div class='ui-body'></div>
			<div class='ui-footer'>
				<div class='ui-line'><div class='ui-icon check'></div></div>
			</div>
		</div>
		<div name='text' class='detail text ui-page-theme-c ui-hide'>
			<div class='ui-header'>
				<div class='ui-blank'></div>
				<div class='ui-title ui-bar-c'><div class='ui-text'></div><div class='ui-icon close'></div></div>
			</div>
			<div class='ui-body'>
				<textarea></textarea><div class='ui-icon bin'></div>
			</div>
			<div class='ui-footer'>
				<div class='ui-line'><div class='ui-icon check ui-hide'></div></div>
			</div>
		</div>
		<div name='pending' class='detail pending ui-page-theme-c ui-hide'>
			<div class='ui-header'>
				<div class='ui-blank'></div>
				<div class='ui-title ui-bar-c'><div class='ui-text'>งานค้างที่ยังไม่ได้ส่ง</div><div class='ui-icon close'></div></div>
			</div>
			<div class='ui-body'></div>
			<div class='ui-footer'>
				<div class='ui-line'><div class='ui-icon check ui-hide'></div></div>
			</div>
		</div>
		<div name='history' class='detail history ui-page-theme-c ui-hide'>
			<div class='ui-header'>
				<div class='ui-blank'></div>
				<div class='ui-title ui-bar-c'><div class='ui-icon back'></div><div class='ui-text'>ประวัติที่ผ่านมา</div><div class='ui-icon'></div></div>
			</div>
			<div class='ui-filter'>
				<div class='ui-menu'></div>
				<div class='ui-line first'><div class='ui-label'>เลือกช่วงเวลา</div></div>
				<div class='ui-line'><div class='date sDate'><div class='ui-label'>วันที่</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'></div><div class='ui-icon calender'></div></div><div class='date fDate'><div class='ui-label'>ถึง</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'></div><div class='ui-icon calender'></div></div><div class='ui-icon check'></div></div>
			</div>
			<div class='ui-body'></div>
		</div>
		<div name='display' class='detail display ui-page-theme-c ui-hide'>
			<div class='ui-header'>
				<div class='ui-blank'></div>
				<div class='ui-title ui-bar-c'><div class='ui-icon info white'></div><div class='ui-text'>สรุปงาน/สถานการณ์</div><div class='ui-icon close'></div></div>
			</div>
			<div class='ui-body'><div class='main'></div><div class='questionnaire'></div></div>
		</div>
	</div>
	<div name='menu' class='my-popup menu ui-popup-container ui-popup-hidden'>
		<div class='ui-content'>
			<div class='profile ui-menu ui-body-c ui-shadow ui-corner-all'><div class='ui-icon profile blank'></div><div class='ui-text'>ข้อมูลส่วนตัว</div></div>
			<div class='policy ui-menu ui-body-c ui-shadow ui-corner-all'><div class='ui-icon policy'></div><div class='ui-text'>ข้อกำหนดการใช้งาน</div></div>
			<div class='exit ui-menu ui-body-c ui-shadow ui-corner-all'><div class='ui-icon exit'></div><div class='ui-text'>ออกจากระบบ</div></div>
			<div class='delete ui-menu ui-body-c ui-shadow ui-corner-all'><div class='ui-icon delete'></div><div class='ui-text'>ลบบัญชีผู้ใช้</div></div>
		</div>
		<div class='ui-hide'><div class='ui-icon close'></div></div>
	</div>
	<div name='pending' class='my-popup pending ui-popup-container ui-popup-hidden'>
		<button class='ui-btn ui-shadow ui-corner-all ui-mini ui-btn-active ui-icon close'></button>
		<div class='ui-content ui-body-c ui-corner-all ui-shadow'>
			<div class='ui-line backlogName'><div class='ui-label'>ชื่องานค้าง</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' maxlength='100'/></div></div>
			<div class='ui-line backlogDesc'><div class='ui-label'>หมายเหตุ (ถ้ามี)</div><div class='ui-text ui-body-c ui-shadow ui-corner-all'><input type='text' maxlength='300'/></div></div>
			<button class='ui-btn ui-shadow ui-corner-all ui-mini ok'>บันทึก</button>
		</div>
	</div>
	<div name='image' class='detail image ui-page-theme-c ui-hide'>
		<div class='ui-header'>
			<div class='ui-blank'></div>
			<div class='ui-title ui-bar-c'><div class='ui-text'></div><div class='ui-icon close'></div></div>
		</div>
		<div class='ui-body'><div class='box ui-blank'></div></div>
		<div class='ui-footer'>
			<div class='ui-line'><div class='ui-text ui-corner-all ui-shadow'>ลบทั้งหมด</div><div class='ui-icon check ui-hide'></div></div>
		</div>
	</div>
	<div name='attention' class='my-popup attention ui-popup-container ui-popup-hidden'>
		<div class='ui-content ui-body-c ui-corner-all ui-shadow'>
			<button class='ui-btn ui-shadow ui-corner-all ui-mini camera'></button><button class='ui-btn ui-shadow ui-corner-all ui-mini text'></button>
		</div>
	</div>
	<div name='information' class='my-popup information ui-popup-container ui-popup-hidden'>
		<button class='ui-btn ui-shadow ui-corner-all ui-mini ui-btn-active ui-icon close'></button>
		<div class='ui-content ui-body-c ui-corner-all ui-shadow'>
			<div class='ui-line'><div class='ui-icon pain select'></div><div class='ui-text'>แย่</div></div>
			<div class='ui-line'><div class='ui-icon bad select'></div><div class='ui-text'>ไม่ดี</div></div>
			<div class='ui-line'><div class='ui-icon normal select'></div><div class='ui-text'>ตามเกณฑ์</div></div>
			<div class='ui-line'><div class='ui-icon good select'></div><div class='ui-text'>ดี</div></div>
			<div class='ui-line'><div class='ui-icon excellent select'></div><div class='ui-text'>เยี่ยม</div></div>
			<div class='ui-line'><div class='ui-icon noData select'></div><div class='ui-text'>ไม่มีข้อมูล</div></div>
		</div>
	</div>
	<div name='alert' class='my-popup alert ui-popup-container ui-popup-hidden'>
		<div class='ui-content ui-body-c ui-corner-all ui-shadow'>
			<div class='ui-icon warning'></div>
			<div class='ui-text'></div>
			<button class='ui-btn ui-shadow ui-corner-all ui-mini close'>ตกลง</button>
		</div>
	</div>
	<div name='confirm' class='my-popup confirm ui-popup-container ui-popup-hidden'>
		<div class='ui-content ui-body-c ui-corner-all ui-shadow'>
			<div class='ui-text'></div>
			<div class='ui-desc'>งานค้างจะยังไม่ถูกส่ง สามารถกลับมาแก้ไขได้<hr/></div>
			
			<button class='ui-btn ui-shadow ui-corner-all ui-mini close'>ไม่ใช่</button>
			<button class='ui-btn ui-shadow ui-corner-all ui-mini ok'>ใช่</button>
		</div>
	</div>
	<div class='ui-hide data'>
		<input id='dBox' data-role='datebox' data-theme='c' data-overlay-theme='c' type='text' placeholder='' data-options='{ "mode":"calbox", "overrideDateFormat": "%d/%m/%Y","overrideDateFieldOrder": ["d","m","y"]}'>
		<input id='tBox' data-role='datebox' data-theme='c' data-overlay-theme='c' type='text' data-options='{ "mode":"datebox", "overrideDateFormat": "%H:%M","overrideDateFieldOrder": ["h", "i"],"overrideHeaderFormat":"%H:%M"}'>
		<div class='file'><div class='count'>0</div></div>
		<div class='textarea'></div>
		<div class='answer'></div>
	</div>
</asp:Content>
