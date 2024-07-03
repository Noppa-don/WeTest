var xobjClick,xobjAlert;
// ========================= Page Load ======================== //
$(function(){$('div[data-role=page]').page({theme:'f',});$('#userName').focus();});
// ============================================================ //
// ==================== Set Label Language ==================== //
var language=[]

language[0]={loginLabel:'กรุณาลงชื่อเข้าสู่ระบบ'
	,loginUser:'ชื่อผู้ใช้'
	,loginPass:'รหัสผ่าน'
	,loginForgot:'ลืมรหัสผ่าน ?'
	,loginButton:'เข้าสู่ระบบ'
	,popupAlertOK:'ตกลง'}
language[1]={loginLabel:'Please login'
	,loginUser:'username'
	,loginPass:'password'
	,loginForgot:'Forgot Password ?'
	,loginButton:'Login'
	,popupAlertOK:'OK'}
language[2]={loginLabel:'ကျေးဇူးပြု၍ ဝင်ရောက်ပါ။'
	,loginUser:'အသုံးပြုသူအမည်'
	,loginPass:'စကားဝှက်'
	,loginForgot:'စကားဝှက်မေ့နေပါသလား ?'
	,loginButton:'လော့ဂ်အင်'
	,popupAlertOK:'အိုကေ'}
// ============================================================ //
// ======================= Object Event ======================= //
$(document).on('click','.ui-head .language',function(e,data){
	//alert(language[2].loginLabel);
	const element=this;
	$('.ui-head .ui-choose').removeClass('ui-hide').css({'top':element.offsetTop,'left':element.offsetLeft-$('.ui-head .ui-choose').width()+$(element).width()});
	$('body').append("<div class='ui-popup-screen ui-overlay-f in' name='language'></div>")
}).on('click','.ui-head .ui-choose .ui-icon',function(e,data){
	$('.ui-head .language').removeClass('thai english myanmar').addClass($(this).attr('name'));
	$('.login .ui-label').html(language[parseInt($(this).attr('val'))].loginLabel);
	$('#userName').attr('placeholder',language[parseInt($(this).attr('val'))].loginUser);
	$('#userPass').attr('placeholder',language[parseInt($(this).attr('val'))].loginPass);
	$('.login .forgot').html(language[parseInt($(this).attr('val'))].loginForgot);
	$('.login button').html(language[parseInt($(this).attr('val'))].loginButton);
	$('.my-popup.alert button').html(language[parseInt($(this).attr('val'))].popupAlertOK);
	$('.ui-popup-screen[name=language]').click();
}).on('keypress','#userName',function(e,data){
	keyEnterNextItem(e);
}).on('keypress','#userPass',function(e,data){
	var xkey = keyA(e);
	if(xkey==13){$(this).blur();checkLogin();}
}).on('click','.login button',function(e,data){
	$(this).blur();
	checkLogin();
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
	switch($(this).attr('name')){
	case'language':
		$('.ui-head .ui-choose').addClass('ui-hide');
		$('body').find('.ui-popup-screen[name='+$(this).attr('name')+']').remove();
	break;
	default:
		popupClose($('.my-popup[name='+$(this).attr('name')+']'));
	}
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
		url:'/weService/checkLogin',
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