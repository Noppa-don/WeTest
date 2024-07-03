var xobjClick,xobjAlert;
var jobCancelMsg="ข้อความที่กรอกจะถูกลบ ?",jobConfirmSaveMsg="ต้องการบันทึกการแจ้งงาน ?",saveSuccessMsg="บันทึกเรียบร้อย !";
// ========================= Page Load ======================== //
$(function(){defaultLoad();});
// ============================================================ //
// ==================== Set Label Language ==================== //
var language=[]

language[0]={hello:'สวัสดี'
	,menuJob:'แจ้งงาน'
	,menuPending:'งานค้าง'
	,menuHistory:'งานเสร็จ'
	,profileInfo:'ข้อมูลส่วนตัว'
	,logout:'ออกจากระบบ'
	,deleteAccount:'ลบบัญชีผู้ใช้งาน'
	,privacyPolicy:'นโยบายความเป็นส่วนตัว'
	,newJob:'งานใหม่'
	,staffConfirm:'รอยืนยันรับงาน'
	,userConfirmDate:'รอยืนยันเลื่อนกำหนดเสร็จ'
	,staffPending:'กำลังดำเนินการ'
	,staffLate:'งานล่าช้า'
	,staffTransfer:'งานที่มอบหมายต่อให้ผู้อื่น'
	,userConfirm:'รอยืนยันปิดงาน'
	,keywordLabel:'ออกจากระบบ'
	,keywordSearch:'ค้นหาเลขงาน / รายละเอียดงาน'
	,week:'สัปดาห์'
	,month:'เดือน'
	,custom:'กำหนดเอง'
	,msgSDate:'กรุณาระบุวันเริ่ม'
	,msgFDate:'กรุณาระบุวันสิ้นสุด'
	,chooseJobGroup:'เลือกหัวข้อที่ต้องการแจ้งงาน'
	,sDate:'เริ่ม'
	,fDate:'สิ้นสุด'
	,jobType:'ประเภทงาน'
	,msgJobType:'กรุณาเลือกประเภทงาน'
	,jobCancelMsg:'ข้อความที่กรอกจะถูกลบ ?'
	,jobConfirmSaveMsg:'ต้องการบันทึกการแจ้งงาน ?'
	,saveSuccessMsg:'บันทึกเรียบร้อย !'
	,popupAlertOK:'ตกลง'
	,popupAlertCancel:'ยกเลิก'}
language[1]={hello:'Hello'
	,menuJob:'inform'
	,menuPending:'Backlog'
	,menuHistory:'Completed work history'
	,profileInfo:'Personal Information'
	,logout:'Log Out'
	,deleteAccount:'Delete Account'
	,privacyPolicy:'Privacy & Policy'
	,newJob:'New Job'
	,staffConfirm:'Confirm Job'
	,userConfirmDate:'Waiting for confirmation'
	,staffPending:'In progress'
	,staffLate:'Late work'
	,staffTransfer:'Assign to other'
	,userConfirm:'Waiting for finishing'
	,keywordSearch:'Search for job number / job details'
	,week:'Week'
	,month:'Month'
	,custom:'Custom'
	,chooseJobGroup:'Select the topic you want'
	,sDate:'Start'
	,fDate:'End'
	,msgSDate:'Please fill start date'
	,msgFDate:'Please fill end date'
	,jobType:'Work type'
	,msgJobType:'Please fill work type'
	,jobCancelMsg:'Filled text will be deleted ?'
	,jobConfirmSaveMsg:'Do you want to save ?'
	,saveSuccessMsg:'Save completed !'
	,popupAlertOK:'OK'
	,popupAlertCancel:'Cancel'}
language[2]={hello:'မင်္ဂလာပါ'
	,menuJob:'အကြောင်းကြား'
	,menuPending:'အလုပ်က မပြီးသေးဘူး။'
	,menuHistory:'ပြီးမြောက်ခဲ့သော အလုပ်မှတ်တမ်း'
	,profileInfo:'ကိုယ်ပိုင်သတင်းအချက်အလက်များ'
	,logout:'ထွက်လိုက်ပါ။'
	,deleteAccount:'အကောင့်ဖျက်ပါ။'
	,privacyPolicy:'ကိုယ်ရေးအချက်အလက်မူဝါဒ'
	,newJob:'အလုပ်သစ်'
	,staffConfirm:'အလုပ်အတည်ပြုပါ။'
	,userConfirmDate:'အတည်ပြုချက်ကို စောင့်နေပါတယ်။'
	,staffPending:'ဆောင်ရွက်ဆဲဖြစ်သည်'
	,staffLate:'အလုပ်နောက်ကျ'
	,staffTransfer:'အခြားသူများကို အလုပ်အပ်ပါ။'
	,userConfirm:'ပြီးအောင်စောင့်နေတယ်။'
	,keywordSearch:'အလုပ်နံပါတ်/အလုပ်အသေးစိတ်အချက်အလက်များကို ရှာဖွေပါ။'
	,week:'တစ်ပတ်'
	,month:'လ'
	,custom:'စိတ်ကြိုက်'
	,chooseJobGroup:'Select the topic you want'
	,sDate:'စတင်ပါ။'
	,fDate:'အဆုံး'
	,msgSDate:'စတင်မည့်ရက်ကို ဖြည့်ပါ။'
	,msgFDate:'ကျေးဇူးပြု၍ ကုန်ဆုံးရက်ကို ဖြည့်ပါ။'
	,jobType:'အလုပ်အမျိုးအစား'
	,msgJobType:'အလုပ်အမျိုးအစားကို ဖြည့်စွက်ပါ။'
	,jobCancelMsg:'ထည့်သွင်းထားသော စာသားကို ဖျက်လိုက်ပါမည်။ ?'
	,jobConfirmSaveMsg:'အလုပ်အကြောင်းကြားချက်များကို သိမ်းဆည်းလိုပါသည်။ ?'
	,saveSuccessMsg:'အောင်မြင်စွာ သိမ်းဆည်းခဲ့သည်။ !'
	,popupAlertOK:'အိုကေ'
	,popupAlertCancel:'ပယ်ဖျက်ပါ။'}
// ============================================================ //
// ======================= Object Event ======================= //
$(document).on('click','.header .language',function(e,data){
	const element=this;
	var xobj=$('.header .ui-list');
	$(xobj).removeClass('ui-hide').addClass('ui-show').css({'top':element.offsetTop,'left':element.offsetLeft-$(xobj).width()+$(element).width()});
	$('body').append("<div class='ui-popup-screen ui-overlay-f in' name='language'></div>")
}).on('click','.header .ui-list .ui-icon',function(e,data){
	$('.header .language').removeClass('thai english myanmar').addClass($(this).attr('name'));
	$('.header .ui-row.loginName .hello').html(language[parseInt($(this).attr('val'))].hello);
	$('.content.main .menu.job .ui-text').html(language[parseInt($(this).attr('val'))].menuJob);
	$('.content.main .menu.pending .ui-text').html(language[parseInt($(this).attr('val'))].menuPending);
	$('.content.main .menu.history .ui-text').html(language[parseInt($(this).attr('val'))].menuHistory);
	$('.content.profile .menu.profile .ui-text').html(language[parseInt($(this).attr('val'))].profileInfo);
	$('.content.profile .menu.logout .ui-text').html(language[parseInt($(this).attr('val'))].logout);
	$('.content.profile .menu.delete .ui-text').html(language[parseInt($(this).attr('val'))].deleteAccount);
	$('.content.profile .menu.policy .ui-text').html(language[parseInt($(this).attr('val'))].privacyPolicy);
	$('.content.pending .menu.new .ui-text .name').html(language[parseInt($(this).attr('val'))].newJob);
	$('.content.pending .menu.staffConfirm .ui-text .name').html(language[parseInt($(this).attr('val'))].staffConfirm);
	$('.content.pending .menu.userConfirmDate .ui-text .name').html(language[parseInt($(this).attr('val'))].userConfirmDate);
	$('.content.pending .menu.staffPending .ui-text .name').html(language[parseInt($(this).attr('val'))].staffPending);
	$('.content.pending .menu.staffLate .ui-text .name').html(language[parseInt($(this).attr('val'))].staffLate);
	$('.content.pending .menu.staffTransfer .ui-text .name').html(language[parseInt($(this).attr('val'))].staffTransfer);
	$('.content.pending .menu.userConfirm .ui-text .name').html(language[parseInt($(this).attr('val'))].userConfirm);
	$('.content.history .title .ui-text').html(language[parseInt($(this).attr('val'))].menuHistory);
	
	$('.keyword input').val(language[parseInt($(this).attr('val'))].keywordSearch);
	$('.period .week').html(language[parseInt($(this).attr('val'))].week);
	$('.period .month').html(language[parseInt($(this).attr('val'))].month);
	$('.period .custom').html(language[parseInt($(this).attr('val'))].custom);
	$('.content.job .lv1 .title .ui-text').html(language[parseInt($(this).attr('val'))].chooseJobGroup);
	$('#sDate').attr('placeholder',language[parseInt($(this).attr('val'))].sDate);
	$('#fDate').attr('placeholder',language[parseInt($(this).attr('val'))].fDate);
	$('.sDate input').attr('placeholder',$('.sDate.request').length==0?language[parseInt($(this).attr('val'))].sDate:"* "+language[parseInt($(this).attr('val'))].sDate);
	$('.fDate input').attr('placeholder',$('.fDate.request').length==0?language[parseInt($(this).attr('val'))].fDate:"* "+language[parseInt($(this).attr('val'))].fDate);
	$('.sDate input').attr('msg',language[parseInt($(this).attr('val'))].msgSDate);
	$('.fDate input').attr('msg',language[parseInt($(this).attr('val'))].msgFDate);
	$('.sDate .ui-label').html(language[parseInt($(this).attr('val'))].sDate);
	$('.fDate .ui-label').html(language[parseInt($(this).attr('val'))].fDate);
	$('#shDate').attr('placeholder',language[parseInt($(this).attr('val'))].sDate);
	$('#fhDate').attr('placeholder',language[parseInt($(this).attr('val'))].fDate);
	$('.shDate input').attr('placeholder',language[parseInt($(this).attr('val'))].sDate);
	$('.fhDate input').attr('placeholder',language[parseInt($(this).attr('val'))].fDate);
	$('.jobType input').attr('placeholder',$('.jobType.request').length==0?language[parseInt($(this).attr('val'))].jobType:"* "+language[parseInt($(this).attr('val'))].jobType);
	$('.jobType input').attr('msg',language[parseInt($(this).attr('val'))].msgJobType);
	$('.jobType .ui-label').html(language[parseInt($(this).attr('val'))].jobType);
	
	$('.my-popup .footer .ok').html(language[parseInt($(this).attr('val'))].popupAlertOK);
	$('.my-popup.alert .footer .ok').html(language[parseInt($(this).attr('val'))].popupAlertOK);
	$('.my-popup.confirm .footer .close').html(language[parseInt($(this).attr('val'))].popupAlertCancel);
	jobCancelMsg=language[parseInt($(this).attr('val'))].jobCancelMsg;
	jobConfirmSaveMsg=language[parseInt($(this).attr('val'))].jobConfirmSaveMsg;
	saveSuccessMsg=language[parseInt($(this).attr('val'))].saveSuccessMsg;
	$('.ui-popup-screen[name=language]').click();
}).on('click','.header .bar .menu .ui-icon.profile,.content.main .menu',function(e,data){
	$('.header .ui-line').addClass('ui-hide').removeClass('ui-show');
	$('.header .bar').removeClass('ui-hide').addClass('ui-show');
	$('.header .bar .app').removeClass('ui-hide').addClass('ui-show');
	$('.header .bar .menu .ui-icon').removeClass('ui-hide lv2').addClass('ui-show');
	$('.header .bar .menu .ui-icon.profile').addClass('ui-hide').removeClass('ui-show');
	$('.content .lv').addClass('ui-hide').removeClass('ui-show');
	$('.content .lv1').removeClass('ui-hide').addClass('ui-show');
	$('.content').addClass('ui-hide').removeClass('ui-show');
	$('.content.'+$(this).attr('name')).removeClass('ui-hide').addClass('ui-show');
}).on('click','.header .bar .menu .ui-icon.delete',function(e,data){
	if($(this).hasClass('lv2')){
		xobj=$('.content.ui-show .lv2').find('input,textarea')
		for(var i=0;i<xobj.length;i++){
			if($(xobj[i]).val()!=''){
				$('.my-popup.confirm .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.confirm .ui-text').html(jobCancelMsg);
				popupOpen($('.my-popup.confirm'),1100);
				return;				
			}
		}
		if($('.content.ui-show .lv2 .image .ui-text').find('.ui-image').length>0){
			$('.my-popup.confirm .ui-icon').addClass('warning').removeClass('check');
			$('.my-popup.confirm .ui-text').html(jobCancelMsg);
			popupOpen($('.my-popup.confirm'),1100);
			return;				
		}
		$('.content .lv').addClass('ui-hide').removeClass('ui-show');
		$('.content .lv1').removeClass('ui-hide').addClass('ui-show');
		$(this).removeClass('lv2');
	}else{
		$('.header .bar .menu .ui-icon').removeClass('ui-hide').addClass('ui-show');
		$('.header .bar .menu .ui-icon.delete').addClass('ui-hide').removeClass('ui-show');
		$('.header .ui-line').removeClass('ui-hide').addClass('ui-show');
		$('.header .bar .app').addClass('ui-hide').removeClass('ui-show');
		$('.content').addClass('ui-hide').removeClass('ui-show');
		$('.content.main').removeClass('ui-hide').addClass('ui-show');
	}
	$('.footer').addClass('ui-hide').removeClass('ui-show');
}).on('click','.content.history .lv1 .filter .condition .choice',function(e,data){
	$(this).closest('.period').find('.choice').removeClass('select');
	$(this).addClass('select');
	$(this).hasClass('custom')==true?$('.content.history .lv1 .filter .condition .date').removeClass('ui-hide').addClass('ui-show'):$('.content.history .lv1 .filter .condition .date').addClass('ui-hide').removeClass('ui-show')
}).on('click','.content.history .lv1 .filter .condition .date .shDate,.content.history .lv1 .filter .condition .date .fhDate,.content.job .lv2 .sDate,.content.job .lv2 .fDate',function(e,data){
	xobjClick=this;
	if($(this).hasClass('shDate')){
		$('#shDate').val($(this).find('input').val())
		$('#shDate').datebox("open");
	}else if($(this).hasClass('fhDate')){
		$('#fhDate').val($(this).find('input').val())
		$('#fhDate').datebox("open");
	}else if($(this).hasClass('sDate')){
		$('#sDate').val($(this).find('input').val())
		$('#sDate').datebox("open");
	}else if($(this).hasClass('fDate')){
		$('#fDate').val($(this).find('input').val())
		$('#fDate').datebox("open");
	}
	var xobj=$('.ui-popup-container .ui-datebox-container .ui-btn-left.ui-link')
	$(xobj).removeClass('ui-btn-left').addClass('ui-btn-right');
	$(xobj).closest('.ui-popup-container').css('top',(($(window).height()-$(xobj).closest('.ui-popup-container').height())/2)+'px');
	$(xobj).closest('.ui-popup-container').css('left',(($(window).width()-$(xobj).closest('.ui-popup-container').width())/2)+'px');
}).on('change','#sDate,#fDate,#shDate,#fhDate',function(e,data){
	$(xobjClick).find('input').val($(this).val());
}).on('click','.content.history .lv1 .ui-list .ui-block,.content.job .ui-menu .menu',function(e,data){
	$(this).closest('.content').find('.lv').addClass('ui-hide').removeClass('ui-show');
	$(this).closest('.content').find('.lv2').removeClass('ui-hide').addClass('ui-show');
	if($(this).closest('.content').hasClass('job')){
		$('.content.job .lv2').find('input,textarea').val('').removeAttr('xid').closest('.lv2').find('.image .ui-text').empty();
		$('.content.job .lv2 .jobGroup').html("<div class='ui-image'>"+$(this).find('.ui-icon').html()+"</div><div class='ui-text'>"+$(this).closest('.menu').find('.ui-text').html()+"</div>");
		$('.footer .ui-icon').addClass('ui-hide').removeClass('ui-show');
		$('.footer .ui-icon.next').removeClass('ui-hide').addClass('ui-show');
		$('.footer').removeClass('ui-hide').addClass('ui-show');
	}
	$('.header .bar .menu .ui-icon.delete').addClass('lv2');
}).on('click','.content.job .lv2 .jobType,.content.job .lv2 .location',function(e,data){
	xobjClick=this;
	$('.my-popup.choose .ui-title').html($(xobjClick).find('input').attr('msg'));
	$('.my-popup.choose .ui-list .ui-corner-all').addClass('ui-hide').removeClass('ui-show');
	$(this).hasClass('jobType')?$('.my-popup.choose .ui-list .ui-corner-all.jobType').removeClass('ui-hide').addClass('ui-show'):$('.my-popup.choose .ui-list .ui-corner-all.location').removeClass('ui-hide').addClass('ui-show')
	popupOpen($('.my-popup.choose'),1110);
}).on('click','.content.job .lv2 .description .image .ui-icon.camera',function(e,data){
	xobjClick=this;
	$('.count.job').html(parseInt($('.count.job').html())+1);
	$('.file.job').append("<input type='file' id='job"+$('.count.job').html()+"'/>");
	$('#job'+$('.count.job').html()).click();
}).on('change','.file.job input',function(e,data){
	var fileid=$(this).attr('id');
	var xobj='';
	if(this.files && this.files[0]){
		var reader=new FileReader();
		reader.onload=function(e){
			$(xobjClick).closest('.image').find('.ui-text').append("<div class='ui-image ui-shadow' name='"+fileid+"'><img src='"+e.target.result+"'/><div class='ui-icon delete'></div></div>");
		};
		reader.readAsDataURL(this.files[0]);
	}
}).on('click','.content.job .lv2 .description .image .ui-image .ui-icon.delete',function(e,data){
	$('#'+$(this).closest('.ui-image').attr('name')).remove();
	$(this).closest('.ui-image').addClass('ui-hide');
}).on('click','.content.job .lv .image .ui-image',function(e,data){
	$('.my-popup.fullImage img').attr('src',$(this).find('img').attr('src'));
	popupOpen($('.my-popup.fullImage'),1110);	
}).on('click','.content.job .lv2 .isRef',function(e,data){
	if($(this).find('.ui-icon').hasClass('select')){
		$(this).find('.ui-icon').removeClass('select');
		$('.content.job .lv2 .refReason').removeClass('request');
		$('.content.job .lv2 .rowRef').addClass('ui-hide').removeClass('ui-show');
	}else{
		$(this).find('.ui-icon').addClass('select');
		$('.content.job .lv2 .rowRef').removeClass('ui-hide').addClass('ui-show');
		$('.content.job .lv2 .refReason').addClass('request');
		$('.content.job .lv2 .userRef').click();
	}
}).on('click','.content.job .lv2 .userRef',function(e,data){
	xobjClick=this;
	popupOpen($('.my-popup.userRef'),1110);
	$('.my-popup.userRef .ui-filter input').focus();
}).on('click','.footer .ui-icon.back',function(e,data){
	$('.content.ui-show .lv').addClass('ui-hide').removeClass('ui-show');
	$('.content.ui-show .lv2').removeClass('ui-hide').addClass('ui-show');
	$('.footer .ui-icon').addClass('ui-hide').removeClass('ui-show');
	$('.footer .ui-icon.next').removeClass('ui-hide').addClass('ui-show');
}).on('click','.footer .ui-icon.next',function(e,data){
	var xobj,display=$('.content.job .lv3');
	$(display).empty().append("<div class='ui-column jobGroup'>"+$('.content.job .lv2 .jobGroup').html()+"</div>");
	xobj=$('.content.job .lv2 .ui-block');
	for(var i=0;i<xobj.length;i++){
		if($(xobj[i]).hasClass('ui-hide')==false){
			if($(xobj[i]).hasClass('request')&&$(xobj[i]).find('input,textarea').val()==''){
				if($(xobj[i]).hasClass('jobDesc')){
					xobjAlert=$(xobj[i]).find('input,textarea');
					$('.my-popup.alert').attr('action','focus');
				}else{
					xobjAlert=xobj[i];
					$('.my-popup.alert').attr('action','click');
				}
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html($(xobj[i]).find('input,textarea').attr('msg'));
				popupOpen($('.my-popup.alert'),99999);
				return;				
			}else{
				if($(xobj[i]).hasClass('image')){
					if($(display).find('.ui-text.image').length==0){
						$(display).append("<div class='ui-column image'><div class='ui-label'></div><div class='ui-text image'>"+$(xobj[i]).find('.ui-text').html()+"</div></div>");
					}else{
						$(display).find('.ui-text.image').append($(xobj[i]).find('.ui-text').html());
					}
				}else if($(xobj[i]).hasClass('isRef')){
					if($(xobj[i]).find('.ui-icon').hasClass('select')){
						$(display).append("<div class='ui-column'><div class='ui-label'>"+$(xobj[i]).find('.ui-text').html()+"</div><div class='ui-text userRef'></div></div>");
					}
				}else if($(xobj[i]).hasClass('userRef')){
					$(display).find('.userRef').append($(xobj[i]).find('input,textarea').val());
				}else if($(xobj[i]).hasClass('refReason')){
					if($(display).find('.userRef').length>0){
						$(display).append("<div class='ui-column'><div class='ui-label'>"+$(xobj[i]).find('input,textarea').attr('placeholder')+"</div><div class='ui-text refReason'>"+$(xobj[i]).find('input,textarea').val()+"</div></div>");
					}
				}else{
					$(display).append("<div class='ui-column "+$(xobj[i]).attr('name')+"'><div class='ui-label'>"+$(xobj[i]).find('input,textarea').attr('placeholder')+"</div><div class='ui-text'>"+$(xobj[i]).find('input,textarea').val()+"</div></div>");
				}
			}
		}
	}
	$(display).find('.ui-icon.delete').remove();
	$('.content.ui-show .lv').addClass('ui-hide').removeClass('ui-show');
	$('.content.ui-show .lv3').removeClass('ui-hide').addClass('ui-show');
	$('.footer .ui-icon').removeClass('ui-hide').addClass('ui-show');	
	$('.footer .ui-icon.next').addClass('ui-hide').removeClass('ui-show');
}).on('click','.footer .ui-icon.save',function(e,data){
	$('.my-popup.confirm .ui-icon').removeClass('warning').addClass('confirmSave');
	$('.my-popup.confirm .ui-text').html(jobConfirmSaveMsg);
	popupOpen($('.my-popup.confirm'),1100);
});
// ============================================================ //
// =========================== Popup ========================== //
$(document).on('click','.my-popup .footer .close',function(e,data){
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
	case'lv1':
		$('.content .lv').addClass('ui-hide').removeClass('ui-show');
		$('.content .lv1').removeClass('ui-hide').addClass('ui-show');
	break;
	}
}).on('click','.ui-popup-screen',function(e,data){
	switch($(this).attr('name')){
	case'language':
		$('.header .ui-list').addClass('ui-hide').removeClass('ui-show');
		$('body').find('.ui-popup-screen[name='+$(this).attr('name')+']').remove();
	break;
	default:
		popupClose($('.my-popup[name='+$(this).attr('name')+']'));
	}
}).on('click','.my-popup .footer .ok',function(e,data){
	switch($(this).closest('.ui-content').find('.ui-text').html()){
	case jobCancelMsg:
		$('.content .lv').addClass('ui-hide').removeClass('ui-show');
		$('.content .lv1').removeClass('ui-hide').addClass('ui-show');
		$('.header .bar .menu .ui-icon.delete').removeClass('lv2');
	break;
	case jobConfirmSaveMsg:
		$('.my-popup.alert').attr('action','lv1');
		$('.my-popup.alert .ui-icon').removeClass('warning').addClass('check');
		$('.my-popup.alert .ui-text').html(saveSuccessMsg);
		popupOpen($('.my-popup.alert'),99999);
	break;
	}
	$(this).closest('.ui-content').find('.footer .close').click();
}).on('click','.my-popup.choose .ui-list .ui-corner-all',function(e,data){
	$(xobjClick).find('input').val($(this).html());
	$('.ui-popup-screen.choose').click();
	if($(xobjClick).closest('.ui-column').next().find('input,textarea').val()==''){
		$(xobjClick).closest('.ui-column').next().click();
	}
}).on('click','.my-popup.userRef .ui-list .ui-corner-all',function(e,data){
	$(xobjClick).find('input').val($(this).html());
	$('.ui-popup-screen.userRef').click();
	$('.content.job .lv2 .refReason textarea').focus();
});
// ============================================================ //
// ========================= Function ========================= //
function defaultLoad(){
	$('div[data-role=page]').page({theme:'f',});
	$('.header .ui-list .ui-icon.thai').click();
	$(window).on('throttledresize', throttledresizeHandler);
	throttledresizeHandler();
}
function throttledresizeHandler(){
	$('.ui-content').css('height',$(window).height()-32);
	$('.my-popup').css('max-height',$(window).height()*0.8);
	$('.my-popup .ui-content').css('height','unset');
	if(navigator.userAgentData.mobile==true){
		$('.my-popup .ui-content').css('width',$(window).width()*0.8);
	}else{
		$('.my-popup .ui-content').css({'width':$(window).width()*0.6,'max-width':'400px'});
	}
}
function nextDate(xobj){
	if($(xobjClick).find('input').val()==''){return;}
	if($(xobjClick).hasClass('sDate')){
		if($('.content.job .lv2 .fDate input').val()==''){
			$('.content.job .lv2 .fDate').click();
		}
	}else{
		if($('.content.history .lv1 .filter .condition .date .fhDate input').val()==''){
			$('.content.history .lv1 .filter .condition .date .fhDate').click();
		}
	}
}
// ============================================================ //
