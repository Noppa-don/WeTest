<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/weService/weService.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <script src="/scripts/weService/weService.js?ver=1.1.1" type="text/javascript"></script>
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
	<div class='ui-column ui-line logo'><div class='ui-text'>WeService</div><div class='ui-icon logo'></div></div>
	<div class='ui-column ui-line loginName'><div class='ui-text hello'>สวัสดี</div><div class='ui-text name'>xxx xxxxx</div></div>
	<div class='ui-list ui-column ui-hide'><div class='ui-icon thai' name='thai' val='0'></div><div class='ui-icon english' name='english' val='1'></div><div class='ui-icon myanmar' name='myanmar' val='2'></div></div>
</div>
<div class='content main ui-show'>
	<div class='lv1 ui-column'>
		<div class='menu job' name='job'><div class='ui-shadow'><div class='ui-icon job'></div></div><div class='ui-text'>แจ้งงาน</div></div>
		<div class='menu pending' name='pending'><div class='ui-shadow'><div class='ui-icon pending'><div class='ui-number'><div class='num'>13</div></div></div></div><div class='ui-text'>งานค้างทั้งหมด</div></div>
		<div class='menu history' name='history'><div class='ui-shadow'><div class='ui-icon history'></div></div><div class='ui-text'>ประวัติงานเสร็จ</div></div>
	</div>
</div>
<div class='content profile ui-hide' name='profile'>
	<div class='lv lv1 ui-row ui-show'>
		<div class='ui-shadow ui-corner-all ui-column menu profile'><div class='ui-icon profile'></div><div class='ui-text'>ข้อมูลส่วนตัว</div></div>
		<div class='ui-shadow ui-corner-all ui-column menu logout'><div class='ui-icon logout'></div><div class='ui-text'>ออกจากระบบ</div></div>
		<div class='ui-shadow ui-corner-all ui-column menu delete'><div class='ui-icon delete'></div><div class='ui-text'>ลบบัญชีผู้ใช้งาน</div></div>
		<div class='menu policy'><div class='ui-text'>นโยบายความเป็นส่วนตัว</div></div>
	</div>
	<div class='lv lv2 ui-row ui-hide'></div>
</div>
<div class='content job ui-hide' name='job'>
	<div class='lv lv1 ui-row ui-show'>
		<div class='title'><div class='ui-text'>เลือกหัวข้อที่ต้องการแจ้งงาน</div></div>
		<div class='ui-menu ui-column'>
			<div class='ui-row menu misse' name='misse'><div class='ui-icon'><img src='/images/weService/4-9.png'/></div><div class='ui-text'>IT Support</div></div>
			<div class='ui-row menu mispgm' name='mispgm'><div class='ui-icon'><img src='/images/weService/4-2.png'/></div><div class='ui-text'>โปรแกรมเมอร์</div></div>
			<div class='ui-row menu aaaa' name='aaaa'><div class='ui-icon'><img src='/images/weService/4-3.png'/></div><div class='ui-text'>ไฟฟ้า</div></div>
			<div class='ui-row menu bbbb' name='bbbb'><div class='ui-icon'><img src='/images/weService/4-4.png'/></div><div class='ui-text'>ประปา</div></div>
			<div class='ui-row menu cccc' name='cccc'><div class='ui-icon'><img src='/images/weService/4-5.png'/></div><div class='ui-text'>แอร์/เครื่องปรับอากาศ</div></div>
		</div>
	</div>
	<div class='lv lv2 ui-row ui-hide'>
		<div class='ui-column jobGroup'></div>
		<div class='ui-column'>
			<div class='ui-column ui-block request ui-corner-all date sDate' name='sDate'>
				<div class='ui-text'><input type='text' readonly/></div><div class='ui-icon calendar'></div>
			</div>
			<div class='ui-column ui-block request ui-corner-all date fDate' name='fDate'>
				<div class='ui-text'><input type='text' readonly/></div><div class='ui-icon calendar'></div>
			</div>
		</div>
		<div class='ui-column ui-block request ui-corner-all jobType' name='jobType'>
			<div class='ui-text'><input type='text' text='กรุณาเลือกประเภทงาน'  placeholder='*ประเภทงาน' readonly/></div><div class='ui-icon dropdown'></div>
		</div>
		<div class='ui-column ui-block ui-corner-all serial' name='serial'>
			<div class='ui-text'><input type='text' placeholder='หมายเลขซีเรียล' /></div>
		</div>
		<div class='ui-column ui-block request ui-corner-all location' name='location'>
			<div class='ui-text'><input type='text' text='กรุณาเลือกสถานที่' placeholder='*สถานที่' readonly/></div><div class='ui-icon dropdown'></div>
		</div>
		<div class='ui-column ui-block ui-corner-all roomNo ui-hide' name='roomNo'>
			<div class='ui-text'><input type='text' placeholder='เลขห้อง' readonly/></div>
		</div>
		<div class='ui-corner-all'>
			<div class='ui-row description'>
				<div class='ui-column ui-block jobDesc request' name='jobDesc'>
					<div class='ui-text'><textarea text='กรุณาระบุรายละเอียดที่ต้องการแจ้ง' placeholder='*รายละเอียดที่ต้องการแจ้ง' ></textarea></div>
				</div>
			</div>
			<div class='ui-row description'>
				<div class='ui-column ui-block image'>
					<div class='ui-text'></div>
					<div class='ui-icon camera'></div>
				</div>
			</div>
		</div>
		<div class='ui-column ui-block isRef'>
			<div class='ui-icon checkbox'></div><div class='ui-text'>ต้องการแจ้งในนามผู้อื่น ?</div>
		</div>
		<div class='ui-row rowRef ui-hide'>
			<div class='ui-column'>
				<div class='ui-column ui-corner-all ui-block userRef' name='userRef'>
					<div class='ui-text'><input type='text' placeholder='ชื่อ - สกุล' readonly/></div>
				</div>
				<div class='ui-column ui-corner-all mobile' name='mobile'>
					<div class='ui-text'><input type='text' placeholder='เบอร์โทรศัพท์' readonly/></div>
				</div>
			</div>
			<div class='ui-column ui-corner-all ui-block refReason' name='refReason'>
				<div class='ui-text'><textarea placeholder='*เหตุผลแจ้งแทน' /></textarea></div>
			</div>
		</div>
	</div>
	<div class='lv lv3 ui-row ui-hide'>
	</div>
</div>
<div class='content pending ui-hide' name='pending'>
	<div class='lv lv1 ui-row ui-show'>
		<div class='filter ui-column'>
			<div class='ui-row condition'>
				<div class='ui-column keyword'><input type='text' placeholder='ค้นหาเลขงาน / รายละเอียดงาน'/></div>
				<div class='ui-column group'>
					<div class='' name='misse'><div class='ui-icon select ui-shadow'><img src='/images/weService/4-9.png'/></div></div>
					<div class='' name='history'><div class='ui-icon select ui-shadow'><img src='/images/weService/4-2.png'/></div></div>
				</div>
			</div>
			<div class='ui-icon search btn'></div>
		</div>
		<div class='ui-column menu new'><div class='ui-icon job new'></div><div class='ui-text ui-column'><div class='name'>งานใหม่</div><div class='count'>(2)</div></div></div>
		<div class='ui-column menu staffConfirm'><div class='ui-icon job staffConfirm'></div><div class='ui-text ui-column'><div class='name'>รอยืนยันรับงาน</div><div class='count'>(2)</div></div></div>
		<div class='ui-column menu userConfirmDate'><div class='ui-icon job userConfirmDate'></div><div class='ui-text ui-column'><div class='name'>รอยืนยันเลื่อนกำหนดเสร็จ</div><div class='count'></div></div></div>
		<div class='ui-column menu staffPending'><div class='ui-icon job staffPending'></div><div class='ui-text ui-column'><div class='name'>กำลังดำเนินการ</div><div class='count'>(5)</div></div></div>
		<div class='ui-column menu staffLate'><div class='ui-icon job staffLate'></div><div class='ui-text ui-column'><div class='name'>งานล่าช้า</div><div class='count'></div></div></div>
		<div class='ui-column menu staffTransfer'><div class='ui-icon job staffTransfer'></div><div class='ui-text ui-column'><div class='name'>งานที่มอบหมายต่อให้ผู้อื่น</div><div class='count'>(3)</div></div></div>
		<div class='ui-column menu userConfirm'><div class='ui-icon job userConfirm'></div><div class='ui-text ui-column'><div class='name'>รอยืนยันปิดงาน</div><div class='count'>(1)</div></div></div>
	</div>
	<div class='lv lv2 ui-row ui-hide'></div>
</div>
<div class='content history ui-hide' name='history'>
	<div class='lv lv1 ui-row ui-show'>
		<div class='ui-column title'><div class='ui-icon history'></div><div class='ui-text'>ประวัติงานเสร็จ</div></div>
		<div class='filter ui-column'>
			<div class='ui-row condition'>
				<div class='ui-column keyword'><input type='text' placeholder='ค้นหาเลขงาน / รายละเอียดงาน'/></div>
				<div class='ui-column period'><div class='ui-corner-all ui-shadow choice select week' name='week'>สัปดาห์</div><div class='ui-corner-all ui-shadow choice month' name='month'>เดือน</div><div class='ui-corner-all ui-shadow choice custom' name='custom'>กำหนดเอง</div></div>
				<div class='ui-column date ui-hide'>
					<div class='ui-column ui-corner-all shDate'>
						<div class='ui-text'><input type='text' text='กรุณากรอกวันที่เริ่ม' placeholder='วันที่เริ่ม' readonly/></div><div class='ui-icon calendar'></div>
					</div>
					<div class='ui-column ui-corner-all fhDate'>
						<div class='ui-text'><input type='text' text='กรุณากรอกวันที่สิ้นสุด' placeholder='สิ้นสุด' readonly/></div><div class='ui-icon calendar'></div>
					</div>
				</div>
				<div class='ui-column group'>
					<div class='' name='misse'><div class='ui-icon select ui-shadow'><img src='/images/weService/4-9.png'/></div></div>
					<div class='' name='history'><div class='ui-icon select ui-shadow'><img src='/images/weService/4-2.png'/></div></div>
				</div>
			</div>
			<div class='ui-icon search btn'></div>
		</div>
		<div class='ui-list ui-row'>
			<div class='ui-block ui-row ui-shadow ui-corner-all'>
				<div class='ui-row noData'>-ไม่พบข้อมูลที่ค้นหา -</div>
			</div>
			<div class='ui-block ui-row ui-shadow ui-corner-all'>
				<div class='ui-column'>
					<div class='ui-column jobGroup'><div class='ui-image'><img src='/images/weService/4-9.png'/></div><div class='ui-text'>IT Support</div></div><div class='ui-column jobNo'><div class='ui-text ui-corner-all ui-shadow'>Ref:koch022312</div></div>
				</div>
				<div class='ui-column'>
					<div class='ui-label'>ชื่อ</div><div class='ui-text'>กชมน</div>
				</div>
				<div class='ui-column jobType'>
					<div class='ui-label'>ประเภทงาน</div><div class='ui-text'>AAAAAAAAA</div>
				</div>
				<div class='ui-column'>
					<div class='ui-column date sDate'>
						<div class='ui-label'>เริ่ม</div><div class='ui-text'>06/03/2024</div>
					</div>
					<div class='ui-column date fDate'>
						<div class='ui-label'>สิ้นสุด</div><div class='ui-text'>06/03/2024</div>
					</div>
				</div>
				<div class='ui-column'>
					<div class='ui-text jobDate'>วันที่แจ้ง: 05/03/2024 |  16:20</div>
				</div>
			</div>
		</div>
	</div>
	<div class='lv lv2 ui-row ui-hide'>
		<div class="ui-column jobGroup"><div class="ui-image"><img src="/images/weService/4-3.png"></div><div class="ui-text">ไฟฟ้า</div></div>
		<div class="ui-column"><div class="ui-label">วันที่แจ้ง</div><div class="ui-text">07/03/2024 13:12</div></div>
		<div class="ui-column"><div class="ui-label">ชื่อ</div><div class="ui-text">aaaa bbbbbb</div></div>
		<div class="ui-column sDate"><div class="ui-label">*เริ่ม</div><div class="ui-text">07/03/2024</div></div>
		<div class="ui-column fDate"><div class="ui-label">*สิ้นสุด</div><div class="ui-text">07/03/2024</div></div>
		<div class="ui-column"><div class="ui-label">*ประเภทงาน</div><div class="ui-text">ปลั๊กไฟเสีย</div></div>
		<div class="ui-column"><div class="ui-label">หมายเลขซีเรียล (ถ้ามี)</div><div class="ui-text"></div></div>
		<div class="ui-column"><div class="ui-label">*สถานที่</div><div class="ui-text">แผนก Prepress</div></div>
		<div class="ui-column"><div class="ui-label">*รายละเอียดที่ต้องการแจ้ง</div><div class="ui-text">ddddd</div></div>
		<div class="ui-column image"><div class="ui-label"></div><div class="ui-text ui-column image"><div class='ui-image ui-shadow'><img src='/images/weService/4-3.png'/></div></div></div>
		<div class="ui-column"><div class="ui-label">ต้องการแจ้งในนามผู้อื่น ?</div><div class="ui-text isRef">ooooooo bbbb</div></div>
	</div>
</div>
<div class='footer ui-column ui-hide'>
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
