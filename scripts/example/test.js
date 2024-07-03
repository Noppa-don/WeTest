var xSessionExpire = 'Session หมดอายุ. กรุณา Login ใหม่อีกครั้ง';
var xSuccess = 'บันทึกเรียบร้อย';
var xNoPermission = 'ไม่มีสิทธิเข้าใช้งาน';
var xDuplication='พบข้อมูลซ้ำ';
var xSave = false
var startDate=1;
var objClick,maxRow;
// ========================= Page Load ======================== //
$(function () {
	defaultLoad();
});
// ============================================================ //
// ======================= Object Event ======================= //
$(document).on('collapsibleexpand','.home fieldset',function(e,data){
	throttledresizeHandler();
}).on('collapsiblecollapse','.home fieldset',function(e,data){
	throttledresizeHandler();
}).on('click', '#btnSearch', function (e, data) {
	$(this).blur();
	loadPage('loading');
	$('.print').addClass('ui-disabled');
	$('.ui-content.home .ui-list.lv1 .ui-body').empty();
	$('.ui-loader h1').html("กำลังโหลดข้อมูล<br/><progress id='pgrBar' max='100' value='0'></progress>");
	getLv1(0,1);
}).on('click', '.back', function (e, data) {
	hideDetail();
});
// ============================================================ //
// =========================== Popup ========================== //
$(document).on('click', '.popup .btnCenter,.popup .btnLeft', function (e, data) {
	var xobj = $(this).closest('.popup');
	popClose(function(){},$(xobj).attr('id'));
	switch($(xobj).attr('id')){
	case'alert':
		switch($(xobj).attr('action')){
		case'relogin':
			window.location = '/';
		break;
		case'no permission':
			window.location = '/global/menu';
		break;
		case'home':
			hideDetail();
		break;
		case'click':
			$($(xobj).attr('name')).click();
		break;
		case'focus':
			$($(xobj).attr('name')).focus();
		break;
		}
	break;
	}
});
// ============================================================ //
// ========================= Function ========================= //
function defaultLoad() {
	hideDetail();
	$('.fitContent.period .choice[xid=d]').click();
	$('.home fieldset').collapsible({ collapsed: false });
	$(window).on("throttledresize", throttledresizeHandler);
	throttledresizeHandler();
	$('#btnSearch').click();
}
function throttledresizeHandler() {
	$('.home .ui-list .ui-body').css('height',$(window).height()-45-$('#hPage').height()-$('.home fieldset').height()-$('.home .ui-list .ui-header').height());
}
function hideDetail(){
	$('#hPage .home').removeClass('hide');
	$('.print').addClass('ui-disabled');
	$('.ui-content').addClass('hide');
	$('.ui-content').addClass('hide');
	$('.ui-content.home').removeClass('hide');
	$('.ui-content.ui-popup').removeClass('hide');
}
var getLv1=function(rowNo,maxRow){
	var post1='keyword=นุช&facName=&secName='
	$.ajax({
		type: 'POST',
		url: '/example/testGetUserPostJson2',
		data: post1,
		dataType: 'json',
		success: function (data) {
			if(data.length==0){closeGetData();return;}
			if(data.length>0){
				switch (data[0].xID){
				case 'relogin':
					closeGetData();
					$('#alert').attr('action','relogin').removeAttr('name');
					$('#alert .content').html(xSessionExpire);
					popOpen(function(){},'alert');
				break;
				case 'error':
					closeGetData();
					$('#alert').removeAttr('action').removeAttr('name');
					$('#alert .content').html(data[0].x1);
					popOpen(function(){},'alert');
				break;
				default:
var xline,i=0,maxRow=data.length;
(function doAppend() {
	xline="<div class='ui-line";
	if($('.ui-content.home .ui-list.lv1 .ui-body .ui-line').length%2===0){xline+=" odd";}
	xline+="'><div class='id'>"+data[i].userID+"</div><div class='prefix'>"+data[i].prefix+"</div><div class='fullName'>"+data[i].fullName+"</div><div class='position'>"+data[i].position+"</div></div>";
	$('.ui-content.home .ui-list.lv1 .ui-body').append(xline);
	i++;
	rowNo=i;
	$('#pgrBar').attr('value',rowNo).attr('max',maxRow);
	if (i<data.length) {
		setTimeout(doAppend,0);
	}else{
		closeGetData();
	}
})();
				}
			}
		}, complete: function (error) {
		}
	});
}
function closeGetData(){
	unloadPage('loading');
	throttledresizeHandler();
}
var loadPage = (function(id){
	$('body').append("<div class='" + id + " bodyDisable' style='z-index:1201;'></div>");
	$.mobile.loading('show', {text: 'loading',textVisible:true,textonly:false});
});
var unloadPage = (function(id){
	$.mobile.loading('hide');
	$('.' + id).remove();
});
// ============================================================ //
