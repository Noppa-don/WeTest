var xobjClick,xobjAlert;
// ========================= Page Load ======================== //
$(function(){defaultLoad();});
// ============================================================ //
// ======================= Object Event ======================= //
$(document).on('click','.wrapper .ui-body .register .ui-icon.profile',function(e,data){
	$('#profile').click();
}).on('change','input[type=file]',function(e,data) {
	var xobj='';
	if(this.files&&this.files[0]){
		var reader=new FileReader();
		reader.onload=function(e){
			$('.wrapper .ui-body .register .ui-icon.profile').addClass('image').html("<img src='"+e.target.result+"'/>");
		};
		reader.readAsDataURL(this.files[0]);
	}
}).on('input paste','.wrapper .ui-body .register .mobile input',function(e,data){
	lockNumOnly($(this));
	$('.wrapper .ui-body .otp input').removeAttr('userID');
}).on('input paste','.wrapper .ui-body .register .userName input',function(e,data){
	lockUserName($(this));
}).on('input paste','.wrapper .ui-body .register .userPass input,.register .confPass input',function(e,data){
	lockUserPass($(this));
}).on('click','.ui-footer .ui-icon.previous',function(e,data){
	if($('.wrapper .ui-body .otp').hasClass('ui-show')){
		$('.my-popup.confirm .ui-text').html("ต้องการออกจากหน้านี้ ?");
		$('.my-popup.confirm .ui-desc').html("หากใช่ระบบจะไม่บันทึกข้อมูลของท่าน");
		popupOpen($('.my-popup.confirm'),99999);
	}else if($('.wrapper .ui-body .confirm').hasClass('ui-show')){
		$('.wrapper .ui-footer .ui-icon.check').removeClass('check').removeClass('ui-disabled').addClass('next');
		$('.wrapper .ui-body .confirm').addClass('ui-hide').removeClass('ui-show');
		$('.wrapper .ui-body .register').removeClass('ui-hide').addClass('ui-show');
	}else{
		window.location='/questionnaire/login';
	}
}).on('click','.my-popup.confirm button.ok',function(e,data){
	$('.wrapper .ui-header .ui-title').html("ลงทะเบียนผู้ใช้งานใหม่");
	$('.wrapper .ui-footer .ui-icon.check').removeClass('ui-hide');
	$('.wrapper .ui-body .otp').addClass('ui-hide').removeClass('ui-show');
	$('.wrapper .ui-body .confirm').removeClass('ui-hide').addClass('ui-show');
	popupClose($('.my-popup.confirm'));
}).on('click','.ui-footer .ui-icon.next',function(e,data){
	var xobj=$('.wrapper .ui-body .register .ui-line.require');
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
	if($('.wrapper .ui-body .register .mobile input').val().length!=10){
		xobjAlert=$('.wrapper .ui-body .register .mobile input');
		$('.my-popup.alert').attr('action','focus');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("กรุณาระบุ"+$('.wrapper .ui-body .register .mobile .ui-label').html()+"ให้ครบ 10 หลัก");
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else if($('.wrapper .ui-body .register .userName input').val().length<6){
		xobjAlert=$('.wrapper .ui-body .register .userName input');
		$('.my-popup.alert').attr('action','focus');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("กรุณาระบุ"+$('.wrapper .ui-body .register .userName .ui-label').html()+"อย่างน้อย 6 หลัก");
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else if($('.wrapper .ui-body .register .userPass input').val().length<6){
		xobjAlert=$('.wrapper .ui-body .register .userPass input');
		$('.my-popup.alert').attr('action','focus');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("กรุณาระบุ"+$('.wrapper .ui-body .register .userPass .ui-label').html()+"อย่างน้อย 6 หลัก");
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else if(numberArc.indexOf($('.wrapper .ui-body .register .userPass input').val())==1||numberDesc.indexOf($('.wrapper .ui-body .register .userPass input').val())==1||textArc.indexOf($('.wrapper .ui-body .register .userPass input').val())==1||textDesc.indexOf($('.wrapper .ui-body .register .userPass input').val())==1){
		xobjAlert=$('.wrapper .ui-body .register .userPass input');
		$('.my-popup.alert').attr('action','focus');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("ไม่สามารถใช้ Password นี้ได้<br/>กรุณาระบุใหม่อีกครั้ง");
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else if($('.wrapper .ui-body .register .userPass input').val()!=$('.wrapper .ui-body .register .confPass input').val()){
		xobjAlert=$('.wrapper .ui-body .register .userPass input');
		$('.my-popup.alert').attr('action','focus');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("Password ไม่ตรงกัน<br/>กรุณาใส่ Password ใหม่อีกครั้ง");
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else{
		checkBeforeSave();
	};
}).on('click','.wrapper .ui-body .confirm .policy .ui-icon',function(e,data){
	if($(this).hasClass('check')){$(this).removeClass('check');$('.ui-footer .ui-icon.check').addClass('ui-disabled');}else{$(this).addClass('check');$('.ui-footer .ui-icon.check').removeClass('ui-disabled');}
}).on('click','.ui-footer .ui-icon.check',function(e,data){
	if($('.wrapper .ui-body .otp input').attr('userID')==undefined){save();}
	$('.wrapper .ui-header .ui-title').html(" ");
	$('.wrapper .ui-footer .ui-icon.check').addClass('ui-hide');
	$('.wrapper .ui-body .otp').removeClass('ui-hide').addClass('ui-show');
	$('.wrapper .ui-body .confirm').addClass('ui-hide').removeClass('ui-show');
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
		confirmOTP();
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
	case'login':
		window.location='/questionnaire/login';
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
	$('div[data-role=page]').page({theme: 'c',});
	$('.ui-header .ui-bar').addClass('ui-bar-c');
	$(window).on("throttledresize", throttledresizeHandler);
	throttledresizeHandler();
}
function throttledresizeHandler(){
	$('.wrapper .ui-body').css('height',$(window).height()-$('.wrapper .ui-header').height()-$('.wrapper .ui-footer').height()-40);
}
function checkBeforeSave(){
	loadPage();
	var formData=new FormData();
	formData.append('mobile',$('.wrapper .ui-body .register .ui-line.mobile input').val());
	formData.append('userName',$('.wrapper .ui-body .register .ui-line.userName input').val());
	$.ajax({
		type: 'POST',
		url: '/questionnaire/checkBeforeRegister/',
		data: formData,
		contentType: false,
		processData: false,
		success: function (data) {
			switch (data[0].dataType){
			case 'ok':
				$('.wrapper .ui-body .confirm .ui-icon.profile').html($('.wrapper .ui-body .register .ui-icon.profile').html());
				if($('.wrapper .ui-body .confirm .ui-icon.profile').html()==''){$('.wrapper .ui-body .confirm .ui-icon.profile').addClass('blank').removeClass('image');}else{$('.wrapper .ui-body .confirm .ui-icon.profile').addClass('image').removeClass('blank');}
				$('.wrapper .ui-body .confirm .ui-line.fName .ui-text').html($('.wrapper .ui-body .register .ui-line.fName input').val());
				$('.wrapper .ui-body .confirm .ui-line.lName .ui-text').html($('.wrapper .ui-body .register .ui-line.lName input').val());
				$('.wrapper .ui-body .confirm .ui-line.mobile .ui-text').html($('.wrapper .ui-body .register .ui-line.mobile input').val());
				$('.wrapper .ui-body .confirm .ui-line.email .ui-text').html($('.wrapper .ui-body .register .ui-line.email input').val());
				if($('.wrapper .ui-body .confirm .ui-line.email .ui-text').html()==''){$('.wrapper .ui-body .confirm .ui-line.email').addClass('ui-hide');}else{$('.wrapper .ui-body .confirm .ui-line.email').removeClass('ui-hide');}
				$('.wrapper .ui-body .confirm .ui-line.userName .ui-text').html($('.wrapper .ui-body .register .ui-line.userName input').val());
				$('.wrapper .ui-body .confirm .ui-line.userPass .ui-text').html($('.wrapper .ui-body .register .ui-line.userPass input').val());
				$('.wrapper .ui-body .confirm .policy .ui-icon').removeClass('check');
				$('.wrapper .ui-footer .ui-icon.next').removeClass('next').addClass('check').addClass('ui-disabled');

				$('.wrapper .ui-body .register').addClass('ui-hide').removeClass('ui-show');
				$('.wrapper .ui-body .confirm').removeClass('ui-hide').addClass('ui-show');
			break;
			case 'username and mobile duplicate':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("พบชื่อผู้ใช้งานและเบอร์โทรศัพท์นี้ลงทะเบียนในระบบแล้ว<br/>กรุณาเปลี่ยนใหม่");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'username duplicate':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("พบชื่อผู้ใช้งานนี้ลงทะเบียนในระบบแล้ว<br/>กรุณาเปลี่ยนใหม่");
				popupOpen($('.my-popup.alert'),99999);
			break;
			case 'mobile duplicate':
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("พบเบอร์โทรศัพท์นี้ลงทะเบียนในระบบแล้ว<br/>กรุณาตรวจสอบใหม่อีกครั้ง");
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
	formData.append('userID',$('.wrapper .ui-body .otp input').attr('userID'));
	formData.append('mobile',$('.wrapper .ui-body .register .ui-line.mobile input').val());
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
				$('.my-popup.alert .ui-text').html(data[i].errorMsg);
				popupOpen($('.my-popup.alert'),99999);
			break;
			}
		}, complete: function () {
			unloadPage();
		}
	});
}
function save(){
	loadPage();
	var formData=new FormData();
	formData.append('fName',$('.wrapper .ui-body .register .ui-line.fName input').val());
	formData.append('lName',$('.wrapper .ui-body .register .ui-line.lName input').val());
	formData.append('mobile',$('.wrapper .ui-body .register .ui-line.mobile input').val());
	formData.append('email',$('.wrapper .ui-body .register .ui-line.email input').val());
	formData.append('userName',$('.wrapper .ui-body .register .ui-line.userName input').val());
	formData.append('userPass',$('.wrapper .ui-body .register .ui-line.userPass input').val());
	formData.append('xFile',document.getElementById('profile'));
	$.ajax({
		type: 'POST',
		url: '/questionnaire/registerSave/',
		data: formData,
		contentType: false,
		processData: false,
		success: function (data) {
			switch (data[0].dataType){
			case 'success':
				$('.wrapper .ui-header .ui-title').html(" ");
				$('.wrapper .ui-footer .ui-icon.check').addClass('ui-hide');
				$('.wrapper .ui-body .otp').removeClass('ui-hide').addClass('ui-show');
				$('.wrapper .ui-body .confirm').addClass('ui-hide').removeClass('ui-show');
				$('.wrapper .ui-body .otp input').attr('userID',data[0].userID);
				var countDownDate = new Date();
				countDownDate.setMinutes(countDownDate.getMinutes() + parseInt(data[0].otpReNew));
				countDown($('.wrapper .ui-body .otp .ui-btn.reOtp'),countDownDate)
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
				xobjAlert=$('.wrapper .ui-body .otp input');
				$('.my-popup.alert').attr('action','focus');
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html("รหัส OTP ไม่ถูกต้อง<br/>กรุณาใส่ รหัส OTP ใหม่อีกครั้ง");
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
// ============================================================ //
