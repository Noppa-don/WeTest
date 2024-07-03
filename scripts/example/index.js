var xSessionExpire = 'Session timeout';
var xNoPermission = 'ไม่มีสิทธิเข้าใช้งาน';
var xSave = false
// ========================= Page Load ======================== //
$(function () {
	defaultLoad();
});
// ============================================================ //
// ======================= Object Event ======================= //
$(document).on('click', '#dList .dhead .line .sort', function (e, data) {
}).on('click', '#importExcel', function (e, data) {
	importExcel();
});
// ============================================================ //
// =========================== Popup ========================== //
$(document).on('click', '.popup .btnCenter,.popup .btnLeft', function (e, data) {
	
});
// ============================================================ //
// ========================= Function ========================= //
function defaultLoad() {
	$(window).on("throttledresize", throttledresizeHandler);
	throttledresizeHandler();
}
function throttledresizeHandler() {
}
function hidexDetail(){
}
function showxDetail(){
}
function importExcel() {
	var formData = new FormData();
	formData.append('xFile', document.getElementById('fileExcel').files[0]);
	$.ajax({
		type: 'POST',
		url: '/example/importExcel',
		data: formData,
		contentType: false,
		processData: false,
		success: function (data) {
			alert(data);
		}, complete: function () {
		}
	});
}
// ============================================================ //
