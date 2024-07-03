var xobjClick,xobjAlert;
// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); $('#userName').focus(); });

// ============================================================ //
// ======================= Object Event ======================= //
$(document).on('keypress','#userName',function(e,data){
	keyEnterNextItem(e);
}).on('keypress','#userPass',function(e,data){
	var xkey = keyA(e);
	if(xkey==13){$(this).blur();checkLogin();}
}).on('click','.login button',function(e,data){
	$(this).blur();
	checkLogin();
}).on('click', '.login .registerlink', function (e, data) {
    $('.login').css('display', 'none');
    window.location = '/Wetest/Registration';
})
// ============================================================ //
// =========================== Popup ========================== //
$(document).on('click','.my-popup .ui-btn.close,.my-popup .ui-header .ui-icon.close',function(e,data){
	popupClose($(this).closest('.my-popup'));
	switch($(this).closest('.my-popup').attr('action')){
	case'relogin':
		window.location = '/';
	break;
	case'no permission':
		window.location = '/global/menu';
	break;
	case'home':
	break;
	case'click':
		$(xobjAlert).click();
	break;
	case'focus':
		$(xobjAlert).focus();
	break;
	}
}).on('click','.ui-popup-screen',function(e,data){
	popupClose($('.my-popup[name='+$(this).attr('name')+']'));
});
// ============================================================ //
// ========================= Function ========================= //
function checkLogin(){
	var xobj=$('.login input');
	for(var i=0;i<xobj.length;i++){
		if($(xobj[i]).val()==''){
			$('.my-popup.alert').attr('action','focus');
			$('.my-popup.alert .ui-text').html('กรุณาระบุ'+$(xobj[i]).attr('title'));
			popupOpen($('.my-popup.alert'),99999);
			xobjAlert=$(xobj[i]);
			return;
		}
	}
	loadPage();
	var post1='userName='+$('#userName').val()+'&password='+$('#userPass').val();
	$.ajax({
		type:'POST',
		url:'/questionnaire/checkLogin',
		data:post1,
		success:function(data){
			var xline;
			for(var i=0;i<data.length;i++){
				switch(data[i].dataType){
				case'error':
					$('.my-popup.alert .ui-text').html(data[i].errorMsg);
					popupOpen($('.my-popup.alert'),99999);
				break;
				default:
					window.location = '/questionnaire/questionnaire';
				}
			}
		},complete:function(){
			unloadPage();
		}
	});
}
// ============================================================ //