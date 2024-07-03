<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/example/test.css?ver=1.1.2" rel="stylesheet" type="text/css" />
    <script src="/scripts/example/test.js?ver=1.1.2" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div id='hPage' class='ui-header ui-bar-inherit ui-header-fixed slidedown'>
		<div class='ui-title'>ทดสอบ</div>
		<div class='left'>
			<a class='menu home' href='' data-role='button' data-icon='grid' data-iconpos='left' data-transition='sLeft' rel='external'>เมนู</a>
		</div>
		<div class='right'>
			<a class='print preview home' href='#' data-role='button' data-icon='action' data-transition='sRight'>Print Preview</a>
		</div>
	</div>
	<div class='ui-content home'>
		<fieldset data-role='collapsible' data-iconpos='right'>
			<legend>เงื่อนไข</legend>
			<div class='condition'>
				<div class='fitContent period'><div class='ui-label'>ช่วงเวลา</div><div class='ui-text'><div class='choice' xid='d'>วัน</div><div class='choice' xid='w'>สัปดาห์</div><div class='choice' xid='m'>เดือน</div><div class='choice' xid='q'>3 เดือน</div><div class='choice' xid='h'>6 เดือน</div><div class='choice' xid='y'>ปี</div><div class='choice' xid='c'>กำหนดเอง</div><div class='choice date sDate hide' text='วันที่เริ่ม'>วันที่เริ่ม</div><div class='choice date fDate hide' text='ถึง'>ถึง</div></div></div>
			</div>
			<div class='condition btn'><a id='btnSearch' href='#' data-mini='true' data-role='button' data-icon='search' data-iconpos='right' data-transition='sRight'>ค้นหา</a></div>
		</fieldset>
		<div class='ui-list lv1' name='lv1'><div class='ui-header'><div class='ui-line'><div class='id'>ID</div><div class='prefix'>คำนำหน้าชื่อ</div><div class='fullName'>ชื่อ - สกุล</div><div class='position'>ตำแหน่ง</div></div></div><div class='ui-body'></div></div>
	</div>
	<div class='hide'>
		<input id='dbox' data-role='datebox' type='text' placeholder='' data-options='{ "mode":"calbox", "overrideDateFormat": "%d/%m/%Y","overrideDateFieldOrder": ["d","m","y"]}'>
		<div id='xlsExport'>
			<div class='ui-title'></div>
			<p/>
			<table style='font-size:10px;width:100%;border-collapse:collapse;' border='1px solid gray' cellpadding='5'>
			</table>
		</div>
	</div>
</asp:Content>
