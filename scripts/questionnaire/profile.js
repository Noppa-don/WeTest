var xSessionExpire = 'Session หมดอายุ. กรุณา Login ใหม่อีกครั้ง';
var xobjClick,xobjAlert;
// ========================= Page Load ======================== //
$(function(){defaultLoad();});
// ============================================================ //
// ======================= Object Event ======================= //
$(document).on('click','.wrapper .ui-header .ui-icon.close',function(e,data){
	window.location='/questionnaire/questionnaire';
}).on('click','.wrapper .ui-body .ui-icon.profile',function(e,data){
	$('#profile').click();
}).on('change','input[type=file]',function(e,data) {
    reader.readAsDataURL(selectedFile);
	var xobj='';
	if(this.files&&this.files[0]){
		var reader=new FileReader();
		reader.onload=function(e){
			$('.wrapper .ui-body .ui-icon.profile').addClass('image').html("<img src='"+e.target.result+"'/>");
		};
		reader.readAsDataURL(this.files[0]);
	}
}).on('input paste','.wrapper .ui-body .mobile input',function(e,data){
	lockNumOnly($(this));
}).on('input paste','.wrapper .ui-body .userName input',function(e,data){
	lockUserName($(this));
}).on('input paste','.wrapper .ui-body .userPass input,.wrapper .ui-body .confPass input',function(e,data){
	lockUserPass($(this));
}).on('click','.ui-footer .ui-icon.previous',function(e,data){
	$('.wrapper .ui-body .otp').addClass('ui-hide').removeClass('ui-show');
	$('.wrapper .ui-body .profile').removeClass('ui-hide').addClass('ui-show');
	$('.wrapper .ui-footer .ui-icon.previous').addClass('ui-hide').removeClass('ui-show');
	$('.wrapper .ui-footer .ui-icon.check').removeClass('ui-hide').addClass('ui-show');
}).on('click','.ui-footer .ui-icon.check',function(e,data){
	var xobj=$('.wrapper .ui-body .ui-line.require');
	var numberArc="0123456789",numberDesc="9876543210",textArc="abcdefghijklmnopqrstuvwxyz",textDesc="zyxwvutsrqponmlkjihgfedcba";
	for(var i=0;i<xobj.length;i++){
		if($(xobj[i]).find('input').val()==''){
			xobjAlert=$(xobj[i]).find('input');
			$('.my-popup.alert').attr('action','focus');
			$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
			$('.my-popup.alert .ui-text').html("กรุณาระบุ"+$(xobj[i]).find('.ui-label').html());
			popupOpen($('.my-popup.alert'),99999);
			return;
		}
	}
	if($('.wrapper .ui-body .mobile input').val().length!=10){
		xobjAlert=$('.wrapper .ui-body  .mobile input');
		$('.my-popup.alert').attr('action','focus');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("กรุณาระบุ"+$('.wrapper .ui-body .mobile .ui-label').html()+"ให้ครบ 10 หลัก");
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else if($('.wrapper .ui-body .userPass input').val().length>0&&$('.wrapper .ui-body .userPass input').val().length<6){
		xobjAlert=$('.wrapper .ui-body .userPass input');
		$('.my-popup.alert').attr('action','focus');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("กรุณาระบุ"+$('.wrapper .ui-body .userPass .ui-label').html()+"อย่างน้อย 6 หลัก");
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else if(numberArc.indexOf($('.wrapper .ui-body .userPass input').val())==1||numberDesc.indexOf($('.wrapper .ui-body .userPass input').val())==1||textArc.indexOf($('.wrapper .ui-body .userPass input').val())==1||textDesc.indexOf($('.wrapper .ui-body .userPass input').val())==1){
		xobjAlert=$('.wrapper .ui-body .userPass input');
		$('.my-popup.alert').attr('action','focus');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("ไม่สามารถใช้ Password นี้ได้<br/>กรุณาระบุใหม่อีกครั้ง");
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else if($('.wrapper .ui-body .userPass input').val()!=$('.wrapper .ui-body .confPass input').val()){
		xobjAlert=$('.wrapper .ui-body .userPass input');
		$('.my-popup.alert').attr('action','focus');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("Password ไม่ตรงกัน<br/>กรุณาใส่ Password ใหม่อีกครั้ง");
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else{
		checkBeforeSave();
	};
}).on('input paste','.wrapper .ui-body .otp input',function(e,data){
	lockNumOnly($(this));
}).on('click','.wrapper .ui-body .otp .ui-btn.reOtp',function(e,data){
	resendOTP();
}).on('click','.wrapper .ui-body .otp .ui-btn.ok',function(e,data){
	if($('.wrapper .ui-body .otp input').val().length!=6){
		xobjAlert=$('.wrapper .ui-body .otp input');
		$('.my-popup.alert').attr('action','focus');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("ระบุ รหัส OTP ไม่ครบ");
		popupOpen($('.my-popup.alert'),99999);
	}else{
		save(true);
	}
})
// ============================================================ //
// =========================== Popup ========================== //
$(document).on('click','.my-popup .ui-btn.close,.my-popup .ui-header .ui-icon.close',function(e,data){
	popupClose($(this).closest('.my-popup'));
	switch($(this).closest('.my-popup').attr('action')){
	case'relogin':
		window.location = '/';
	break;
	case'questionnaire':
		window.location='/questionnaire/questionnaire';
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
	$('.my-popup.'+$(this).attr('name')+' .ui-btn.close').click();
});
// ============================================================ //
// ========================= Function ========================= //
function defaultLoad(){
	keepSession('/questionnaire/keepSession');
	$('div[data-role=page]').page({theme:'c',});
	$('.ui-header .ui-bar').addClass('ui-bar-c');
	getProfile();
	$(window).on("throttledresize", throttledresizeHandler);
	throttledresizeHandler();
}
function throttledresizeHandler(){
	$('.wrapper .ui-body').css('height',$(window).height()-$('.wrapper .ui-header').height()-$('.wrapper .ui-footer').height()-40);
}
function getProfile(){
	loadPage();
	$.ajax({
		type: 'POST',
		url: '/questionnaire/getProfile/',
		data: null,
		contentType: false,
		processData: false,
		success: function (data) {
			switch (data[0].dataType){
			case 'success':
				if(data[0].profileImage==undefined){$('.wrapper .ui-body .ui-icon.profile').removeClass('image').empty();}else{$('.wrapper .ui-body .ui-icon.profile').addClass('image').html("<img src='"+data[0].profileImage+"'/>");}
				$('.wrapper .ui-body .profile .ui-line.fName input').val(data[0].firstName);
				$('.wrapper .ui-body .profile .ui-line.lName input').val(data[0].lastName);
				$('.wrapper .ui-body .profile .ui-line.mobile input').val(data[0].mobile).attr('ori',data[0].mobile);
				$('.wrapper .ui-body .profile .ui-line.email input').val(data[0].email);
				$('.wrapper .ui-body .profile .ui-line.userName input').val(data[0].userName);
				$('.wrapper .ui-body .profile .ui-line.userPass input').val('');
				$('.wrapper .ui-body .profile .ui-line.confPass input').val('');
				$('.wrapper .ui-body .profile').removeClass('ui-hide').addClass('ui-show');
				$('.wrapper .ui-body .otp').addClass('ui-hide').removeClass('ui-show');
			break;
			case'relogin':
				$('.my-popup.alert').attr('action','relogin');
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html(xSessionExpire);
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'error':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html(data[0].errorMsg);
				popupOpen($('.my-popup.alert'),99999);
			break;
			}
		}, complete: function () {
			unloadPage();
		}
	});
}
function checkBeforeSave(){
	if($('.wrapper .ui-body .profile .ui-line.mobile input').attr('ori')==$('.wrapper .ui-body .profile .ui-line.mobile input').val()){
		$('.wrapper .ui-footer').removeClass('ui-hide').addClass('ui-show');
		save();
		return;
	}
	loadPage();
	var formData=new FormData();
	formData.append('mobile',$('.wrapper .ui-body .profile .ui-line.mobile input').val());
	$.ajax({
		type: 'POST',
		url: '/questionnaire/checkMobile/',
		data: formData,
		contentType: false,
		processData: false,
		success: function (data) {
			switch (data[0].dataType){
			case 'success':
				$('.wrapper .ui-body .profile').addClass('ui-hide').removeClass('ui-show');
				$('.wrapper .ui-body .otp').removeClass('ui-hide').addClass('ui-show');
				$('.wrapper .ui-footer .ui-icon.previous').removeClass('ui-hide').addClass('ui-show');
				$('.wrapper .ui-footer .ui-icon.check').addClass('ui-hide').removeClass('ui-show');
				$('.wrapper .ui-body .otp input').focus();
				if($('.wrapper .ui-body .otp .ui-btn.reOtp').html()=='ส่ง OTP อีกครั้ง'){
					var countDownDate = new Date();
					countDownDate.setMinutes(countDownDate.getMinutes() + parseInt(data[0].otpReNew));
					countDown($('.wrapper .ui-body .otp .ui-btn.reOtp'),countDownDate)
				}
			break;
			case 'mobile duplicate':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("พบเบอร์โทรศัพท์นี้ลงทะเบียนในระบบแล้ว<br/>กรุณาตรวจสอบใหม่อีกครั้ง");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'over day':
			case 'over 30 day':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("ขอ OTP เกินกำหนด<br/>กรุณาลองใหม่อีกครั้งภายหลัง");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'otp error':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("พบปัญหาในการส่ง OTP<br/>กรุณาลองใหม่อีกครั้งภายหลัง");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'error':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html(data[i].errorMsg);
				popupOpen($('.my-popup.alert'),99999);
			break;
			}
		}, complete: function () {
			unloadPage();
		}
	});
}
function resendOTP(){
	loadPage();
	var formData=new FormData();
	formData.append('mobile',$('.wrapper .ui-body .profile .ui-line.mobile input').val());
	$.ajax({
		type: 'POST',
		url: '/questionnaire/resendOTP/',
		data: formData,
		contentType: false,
		processData: false,
		success: function (data) {
			switch (data[0].dataType){
			case 'success':
				var countDownDate = new Date();
				countDownDate.setMinutes(countDownDate.getMinutes() + parseInt(data[0].otpReNew));
				countDown($('.wrapper .ui-body .otp .ui-btn.reOtp'),countDownDate)
			break;
			case 'over day':
			case 'over 30 day':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("ขอ OTP เกินกำหนด<br/>กรุณาลองใหม่อีกครั้งภายหลัง");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'otp error':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("พบปัญหาในการส่ง OTP<br/>กรุณาลองใหม่อีกครั้งภายหลัง");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'error':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html(data[0].errorMsg);
				popupOpen($('.my-popup.alert'),99999);
			break;
			}
		}, complete: function () {
			unloadPage();
		}
	});
}
function save(isOTP){
	var xobj;
	loadPage();
	var formData=new FormData();
	formData.append('fName',$('.wrapper .ui-body .profile .ui-line.fName input').val());
	formData.append('lName',$('.wrapper .ui-body .profile .ui-line.lName input').val());
	formData.append('mobile',$('.wrapper .ui-body .profile .ui-line.mobile input').val());
	formData.append('email',$('.wrapper .ui-body .profile .ui-line.email input').val());
	if($('.wrapper .ui-body .profile .ui-line.userPass input').val()!=''){formData.append('userPass',$('.wrapper .ui-body .profile .ui-line.userPass input').val());}
	if(isOTP==true){formData.append('otp',$('.wrapper .ui-body .otp input').val());}
	formData.append('xFile',document.getElementById('profile'));
	$.ajax({
		type: 'POST',
		url: '/questionnaire/profileSave/',
		data: formData,
		contentType: false,
		processData: false,
		success: function (data) {
			switch (data[0].dataType){
			case 'success':
				$('.my-popup.alert').attr('action','questionnaire');
				$('.my-popup.alert .ui-icon').removeClass('warning').addClass('check');
				$('.my-popup.alert .ui-text').html("บันทึกเรียบร้อย");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'incorrect':
				xobjAlert=$('.wrapper .ui-body .otp input');
				$('.my-popup.alert').attr('action','focus');
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("รหัส OTP ไม่ถูกต้อง<br/>กรุณาใส่ รหัส OTP ใหม่อีกครั้ง");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'otp error':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("พบปัญหาในการส่ง OTP<br/>กรุณาลองใหม่อีกครั้งภายหลัง");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'error':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html(data[0].errorMsg);
				popupOpen($('.my-popup.alert'),99999);
			break;
			}
		}, complete: function () {
			unloadPage();
		}
	});
}
function confirmOTP(){
	loadPage();
	var formData=new FormData();
	formData.append('userID',$('.wrapper .ui-body .otp input').attr('userID'));
	formData.append('otp',$('.wrapper .ui-body .otp input').val());
	$.ajax({
		type: 'POST',
		url: '/questionnaire/registerConfirmOTP/',
		data: formData,
		contentType: false,
		processData: false,
		success: function (data) {
			switch (data[0].dataType){
			case 'success':
				$('.my-popup.alert').attr('action','login');
				$('.my-popup.alert .ui-icon').removeClass('warning').addClass('check');
				$('.my-popup.alert .ui-text').html("ลงทะเบียน เรียบร้อย!<br/>ระบบกำลังนำท่านไปหน้าเข้าสู่ระบบ<br/>กรุณารอสักครู่..");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'expire':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("รหัส OTP หมดอายุ กำลังจัดส่งให้ใหม่ กรุณารอสักครู่");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'incorrect':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("รหัส OTP ไม่ถูกต้อง<br/>กรุณาใส่ รหัส OTP ใหม่อีกครั้ง");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'error':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html(data[0].errorMsg);
				popupOpen($('.my-popup.alert'),99999);
			break;
			}
		}, complete: function () {
			unloadPage();
		}
	});
}
// ============================================================ //
