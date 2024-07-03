var xSessionExpire = 'Session หมดอายุ. กรุณา Login ใหม่อีกครั้ง';
var xobjClick,xobjAlert,xPage;
// ========================= Page Load ======================== //
$(function(){defaultLoad();});
// ============================================================ //
// ======================= Object Event ======================= //
$(document).on('click','.main .ui-header .ui-icon.home',function(e,data){
	showHome();
}).on('click','.main .ui-footer .ui-icon.previous',function(e,data){
	if($('.summary').hasClass('ui-show')){
		if(xPage=='qLv1'){showQLv1();}else{showQLv2();}
	}else if($('.questionnaire.lv2').hasClass('ui-show')){
		showQLv1();
	}else if($('.questionnaire.lv1').hasClass('ui-show')){
		showHome();
	}
}).on('click','.main .ui-footer .ui-icon.next',function(e,data){
	if($('.questionnaire.lv1 .eventName .ui-text input').val()==''){
		$('.my-popup.alert').attr('action','focus');
		xobjAlert=$('.questionnaire.lv1 .eventName .ui-text input');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("กรุณาระบุ"+$('.questionnaire.lv1 .eventName .ui-label').html());
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else if($('.questionnaire.lv1 .person .user').length==0){
		$('.my-popup.alert').attr('action','click');
		xobjAlert=$('.questionnaire.lv1 .person .ui-text.ui-blank');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("กรุณาระบุ"+$('.questionnaire.lv1 .person .ui-label').html());
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else if($('.questionnaire.lv1 .locationType .choice.ui-btn-active').attr('xid')=='out'&&$('.questionnaire.lv1 .eventAddress .ui-text input').val()==''){
		$('.my-popup.alert').attr('action','focus');
		xobjAlert=$('.questionnaire.lv1 .eventAddress .ui-text input');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("กรุณาระบุ"+$('.questionnaire.lv1 .eventAddress .ui-label').html());
		popupOpen($('.my-popup.alert'),99999);
		return;
	}else if($('.question .qMain .answer .ui-icon.select').length==0){
		$('.my-popup.alert').removeAttr('action');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("กรุณาเลือกหัวข้อประเมิน");
		popupOpen($('.my-popup.alert'),99999);
		return;
	}
	$('.detail.summary .ui-body').html("<div class='ui-line'><b>"+$('.questionnaire.lv1 .date .ui-label').html()+"</b> "+$('.questionnaire.lv1 .date .ui-text').html()+" <b>"+$('.questionnaire.lv1 .time .ui-label').html()+"</b> "+$('.questionnaire.lv1 .time .ui-text').html()+"</div>");
	$('.detail.summary .ui-body').append("<div class='ui-line person'><b>"+$('.questionnaire.lv1 .person .ui-label').html()+"</b></div>");
	var xobj=$('.questionnaire.lv1 .person .user');
	for(var i=0;i<xobj.length;i++){
		if($(xobj[i]).hasClass('ui-icon')==false){
			if($(xobj[i]).hasClass('ui-hide')==false){$('.detail.summary .ui-body .ui-line.person').append("<br/>"+$(xobj[i]).html());}
		}
	}
	$('.detail.summary .ui-body .ui-line.person').find('.ui-icon').remove();
	$('.detail.summary .ui-body').append("<div class='ui-line'><b>"+$('.questionnaire.lv1 .eventName .ui-label').html()+"</b><br/>"+$('.questionnaire.lv1 .eventName input').val()+"</div>");
	$('.detail.summary .ui-body').append("<div class='ui-line'><b>"+$('.questionnaire.lv1 .locationType .ui-label').html()+"</b><br/>"+$('.questionnaire.lv1 .locationType .choice.ui-btn-active').html()+"</div>");
	if($('.questionnaire.lv1 .locationType .choice.ui-btn-active').attr('xid')=='out'){
		$('.detail.summary .ui-body').append("<div class='ui-line'><b>"+$('.questionnaire.lv1 .eventAddress .ui-label').html()+"</b><br/>"+$('.questionnaire.lv1 .eventAddress input').val()+"</div>");
	}
	$('.detail.summary .ui-body').append("<div class='ui-line image'><b>"+$('.questionnaire.lv1 .image .ui-label').html()+"</b><div></div></div>");
	var xobj=$('.questionnaire.lv1 .image .box.ui-show');
	for(var i=0;i<xobj.length;i++){
		$('.detail.summary .ui-body .ui-line.image div').append($(xobj[i]).html());
	}
	$('.detail.summary .ui-body .ui-line.image').find('.ui-icon').remove();
	$('.main .summary .question.ui-show').find('.ui-line.ui-btn-active').removeClass('ui-btn-active');
	$('.main .summary .question.ui-show').find('.ui-line .ui-icon').remove();
	$('.main .summary .question.ui-show').find('.ui-line').append("<div class='ui-icon'></div>");
	xobj=$('.main .summary .question.ui-show');
	for(var i=0;i<xobj.length;i++){
		var xobj2=$('.question[qid='+$(xobj[i]).attr('qid')+'] .answer .ui-icon.select');
		if($(xobj2).length>0){$(xobj[i]).find('.ui-line').addClass('ui-btn-active');}
		for(var j=0;j<xobj2.length;j++){
			if($(xobj[i]).attr('qid')==$(xobj2[j]).closest('.question').attr('qid')){$(xobj[i]).find('.ui-icon').attr('class',$(xobj2[j]).attr('class'));}
		}
	}
	showSummary();
}).on('click','.main .summary .question .ui-btn-active',function(e,data){
	$('.detail.question .ui-body').empty().append("<div class='question ui-slider-track ui-corner-all''>"+$(this).closest('.question').html()+"</div>");
	var xobj=$('.main .question[qid='+$(this).closest('.question').attr('qid')+'] .answer .ui-icon.select');
	for(var i=0;i<xobj.length;i++){
		if($(this).attr('qid')!=$(xobj[i]).closest('.question').attr('qid')){
			$('.detail.question .ui-body').append("<div class='sub'><div class='question ui-slider-track ui-corner-all''><div class='ui-line ui-btn-active ui-body-c ui-shadow ui-corner-all'><div class='ui-text'>"+$(xobj[i]).closest('.question').find('.qMain .ui-text').html()+"</div><div class='"+$(xobj[i]).attr('class')+"'></div></div></div></div>");
		}
	}
	$('.detail.question').removeClass('ui-hide');
	throttledresizeHandler();	
}).on('click','.main .ui-footer .ui-icon.download',function(e,data){
	$('.my-popup.confirm').attr('step','save');
	$('.my-popup.confirm .ui-text').html('ต้องการบันทึกและส่งงาน');
	$('.my-popup.confirm .ui-desc').html('<hr/>');
	popupOpen($('.my-popup.confirm'),99999);
}).on('click','.main .ui-header .ui-title .ui-text.ui-shadow',function(e,data){
	$('.summary.detail').removeClass('ui-hide');
	throttledresizeHandler();
}).on('click','.main .ui-header .info',function(e,data){
	popupOpen($('.my-popup.'+$(this).attr('name')));
}).on('click','.main .ui-header .ui-bar .profile',function(e,data){
	popupOpen($('.my-popup.menu'),1100);
	$('.my-popup.menu').css({'top':'70px','left':'unset','right':'10px'});
}).on('click','.my-popup.menu .ui-menu.profile',function(e,data){
	window.location='/questionnaire/profile';
	popupClose($('.my-popup.menu'));
}).on('click','.my-popup.menu .ui-menu.policy',function(e,data){
	window.open('https://italt.co.th/wetestprivacypolicy.html','_blank');
	popupClose($('.my-popup.menu'));
}).on('click','.my-popup.menu .ui-menu.exit',function(e,data){
	$('.my-popup.confirm').attr('step','exit');
	$('.my-popup.confirm .ui-text').html('ต้องการออกจากระบบ ?');
	$('.my-popup.confirm .ui-desc').html('');
	popupOpen($('.my-popup.confirm'),99999);
}).on('click','.my-popup.menu .ui-menu.delete',function(e,data){
	$('.my-popup.confirm').attr('step','delete');
	$('.my-popup.confirm .ui-text').html('ต้องการลบบัญชีผู้ใช้ ?');
	$('.my-popup.confirm .ui-desc').html('หากยืนยันท่านจะไม่สามารถเรียกคืนข้อมูลผู้ใช้งานนี้ได้');
	popupOpen($('.my-popup.confirm'),99999);
}).on('click','.main .ui-header .inbox',function(e,data){
	if($('.main .ui-menu').hasClass('ui-hide')){
		$('.my-popup.confirm').attr('step','pending');
		$('.my-popup.confirm .ui-text').html('ต้องการบันทึกเป็นงานค้าง');
		$('.my-popup.confirm .ui-desc').html('งานค้างจะยังไม่ถูกส่ง สามารถกลับมาแก้ไขได้<hr/>');
		popupOpen($('.my-popup.confirm'),99999);
	}else{
		if($(this).hasClass('empty')){
			$('.my-popup.alert .ui-text').html('ไม่มีงานค้าง');
			popupOpen($('.my-popup.alert'),99999);
		}else{
			getPending();
		}
	}
}).on('click','.detail.pending .ui-body .ui-body-c',function(e,data){
	questionnaireDisplay("docID="+$(this).attr('did'));
	$('.detail.pending').addClass('ui-hide');
}).on('click','.detail.pending .ui-body .ui-icon.bin',function(e,data){
	xobjClick=this;
	questionnaireDeletePending("docID="+$(this).closest('.job').find('.ui-body-c').attr('did'))
}).on('click','.my-popup.confirm button.ok',function(e,data){
	switch($(this).closest('.my-popup').attr('step')){
	case'save':
		save(false);
		popupClose($('.my-popup.confirm'))
	break;
	case'pending':
		popupOpen($('.my-popup.pending'),99999);
		$('.my-popup.confirm button.close').click();
		$('.my-popup.pending .backlogName input').focus();
	break;
	case'exit':
		$.ajax({type:'POST',url:'/questionnaire/questionnaireExit',success:function(data){window.location='/';}});
	break;
	case'delete':
		deleteAccount();
	break;
	}
}).on('click','.my-popup.pending button.ok',function(e,data){
	if($('.my-popup.pending .backlogName input').val()==''){
		$('.my-popup.alert').attr('action','focus');
		xobjAlert=$('.my-popup.pending .backlogName input');
		$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
		$('.my-popup.alert .ui-text').html("กรุณาระบุ"+$('.my-popup.pending .backlogName .ui-label').html());
		popupOpen($('.my-popup.alert'),99999);
		return;
	}
	save(true);
}).on('click','.main .ui-body .ui-menu .menu button',function(e,data){
	$('.questionnaire.lv1').removeAttr('xid').attr('mid',$(this).attr('xid')).attr('name','event');
	if($(this).hasClass('event')){
		$('.questionnaire.lv1').attr('name','event');
	}else{
		$('.questionnaire.lv1').attr('name','person');
	}
	var today=new Date();
	$('.ui-hide.data .file .count').html(0);
	$('.questionnaire.lv1 input').val('');
	$('.questionnaire.lv1 .date .ui-text').html(today.DMY());
	$('.questionnaire.lv1 .time .ui-text').html(today.HM());
	$('.questionnaire.lv1 .person .ui-text').addClass('ui-blank').empty();
	$('.questionnaire.lv1 .image .ui-text').addClass('ui-blank').empty();
	$('.image.detail .ui-body').html("<div class='box ui-blank'></div>");
	$('.question .qMain .answer .ui-icon').removeClass('select');
	$('.ui-hide.data .textarea').empty();
	$('.ui-hide.data .answer').empty();
	showQLv1();
}).on('click','.main .ui-body .questionnaire.lv1 .date .ui-text',function(e,data){
	xobjClick=this;
	$('#dbox').datebox({popupPosition: 'window'});
	$('#dBox').datebox('open');
	$('.ui-popup-container .ui-datebox-container .ui-link').removeClass('ui-btn-left').addClass('ui-btn-right');
	$('.ui-popup-container .ui-datebox-container .ui-header').removeClass('ui-bar-a').addClass('ui-bar-c');
}).on('click','.main .ui-body .questionnaire.lv1 .time .ui-text',function(e,data){
	xobjClick=this;
	$('#tBox').datebox({popupPosition: 'window'});
	$('#tBox').datebox('open');
	$('.ui-popup-container .ui-datebox-container .ui-link').removeClass('ui-btn-left').addClass('ui-btn-right');
	$('.ui-popup-container .ui-datebox-container .ui-header').removeClass('ui-bar-a').addClass('ui-bar-c');
}).on('change','#dBox,#tBox',function(e,data){
	$(xobjClick).html($(this).val());
}).on('click','.questionnaire.lv1 .person .ui-text.ui-blank,.questionnaire.lv1 .person .ui-text .ui-icon.user.add',function(e,data){
	$('.detail.person').removeClass('ui-hide');
	$('.detail.person .ui-body').empty();
	$('.detail.person .ui-filter input').val('').focus();
	throttledresizeHandler();
}).on('input paste','.person.detail .ui-filter input',function(e,data){
	if($(this).val().length<2){return;}
	delay(function(){getUser();},1000);
}).on('click','.person.detail .ui-body .choice',function(e,data){
	if($(this).hasClass('ui-btn-active')){$(this).removeClass('ui-btn-active');}else{$(this).addClass('ui-btn-active');};
}).on('click','.person.detail .ui-footer .ui-icon',function(e,data){
	var xobj=$('.person.detail .ui-body .choice.ui-btn-active');
	for(var i=0;i<xobj.length;i++){
		if($('.questionnaire.lv1 .person .ui-text .user[uid='+$(xobj[i]).attr('xid')+']').length==0){
			$('.questionnaire.lv1 .person .ui-text').prepend("<div class='user ui-shadow ui-corner-all ui-show' uid='"+$(xobj[i]).attr('xid')+"'>"+$(xobj[i]).html()+"<div class='ui-icon delete'></div></div>");
		}else{
			$('.questionnaire.lv1 .person .ui-text .user[uid='+$(xobj[i]).attr('xid')+']').removeClass('ui-hide').addClass('ui-show');
		}
	}
	if($('.questionnaire.lv1 .person .ui-text .ui-icon.user.add').length==0){$('.questionnaire.lv1 .person .ui-text').removeClass('ui-blank').append("<div class='ui-icon user add'></div>");}
	$('.person.detail .ui-icon.close').click();
}).on('click','.questionnaire.lv1 .person .ui-text .user .ui-icon.delete',function(e,data){
	$(this).closest('.user').removeClass('ui-show').addClass('ui-hide delete');
	if($('.questionnaire.lv1 .person .ui-text .user.ui-show').length==0){$('.questionnaire.lv1 .person .ui-text .ui-icon.user.add').remove();$('.questionnaire.lv1 .person .ui-text').addClass('ui-blank');}
}).on('click','.questionnaire.lv1 .locationType .choice',function(e,data){
	$('.questionnaire.lv1 .locationType .choice').removeClass('ui-btn-active');
	$(this).addClass('ui-btn-active')
	if($(this).attr('xid')=='out'){$('.questionnaire.lv1 .eventAddress').closest('.ui-line').removeClass('ui-hide');$('.questionnaire.lv1 .eventAddress input').focus();}else{$('.questionnaire.lv1 .eventAddress').closest('.ui-line').addClass('ui-hide');}
}).on('click','.questionnaire.lv1 .qPlus button',function(e,data){
	showQLv2();
}).on('click','.questionnaire .question .ui-icon.dot',function(e,data){
	xobjClick=$(this).closest('.question');
	if($('.image.detail .ui-body .box[name='+$(xobjClick).attr('qid')+']').length-$('.image.detail .ui-body .box[name='+$(xobjClick).attr('qid')+'].delete').length>0){
		$('.my-popup.attention .ui-btn.camera').addClass('isInfo');
	}else{
		$('.my-popup.attention .ui-btn.camera').removeClass('isInfo');
	}
	if($('.ui-hide.data .textarea textarea[name='+$(xobjClick).attr('qid')+']').length>0&&$('.ui-hide.data .textarea textarea[name='+$(xobjClick).attr('qid')+']').val()!=''){
		$('.my-popup.attention .ui-btn.text').addClass('isInfo');
	}else{
		$('.my-popup.attention .ui-btn.text').removeClass('isInfo');
	}
	popupOpen($('.my-popup.attention'));
}).on('click','.questionnaire .question .ui-icon.plus',function(e,data){
	if($('.qSub.'+$(this).closest('.question').attr('qid')).find('.question').length==0){
		getChoice("questionID="+$(this).closest('.question').attr('qid')+"&menuID="+$(this).closest('.question').attr('mid'));
	}
	$('.qSub.'+$(this).closest('.question').attr('qid')).removeClass('ui-hide');
	$(this).addClass('ui-hide');
	$(this).closest('.qMain').find('.collapsed').removeClass('off').addClass('on');
}).on('click','.questionnaire .question .qMain .ui-icon.collapsed.on',function(e,data){
	$('.qSub.'+$(this).closest('.question').attr('qid')).addClass('ui-hide');
	$(this).closest('.qMain').find('.ui-icon.plus').removeClass('ui-hide');
	$(this).removeClass('on').addClass('off');
}).on('click','.questionnaire .question .answer .ui-icon',function(e,data){
	$(this).closest('.ui-body-c').find('.answer .ui-icon').removeClass('select');
	$(this).addClass('select');
}).on('click','.my-popup.attention .ui-btn.text',function(e,data){
	$('.ui-popup-screen[name=attention]').click()
	$('.text.detail .ui-header .ui-title .ui-text').html("รายละเอียดเพิ่มเติม");
	$('.text.detail').removeClass('ui-hide');
	throttledresizeHandler();
	if($('.ui-hide.data .textarea textarea[name='+$(xobjClick).attr('qid')+']').length>0){$('.text.detail textarea').val($('.ui-hide.data .textarea textarea[name='+$(xobjClick).attr('qid')+']').val());}else{$('.text.detail textarea').val('');}
	$('.text.detail textarea').focus();
}).on('click','.my-popup.attention .ui-btn.camera',function(e,data){
	$('.ui-popup-screen[name=attention]').click()
	$('.image.detail .ui-header .ui-title .ui-text').html("ภาพ");
	$('.image.detail').removeClass('ui-hide');
	throttledresizeHandler();
	$('.image.detail .ui-body .box.ui-show').removeClass('ui-show').addClass('ui-hide');
	$('.image.detail .ui-body .box[name='+$(xobjClick).attr('qid')+']').addClass('ui-show').removeClass('ui-hide');
}).on('click','.detail .ui-icon.close,.detail .ui-icon.back',function(e,data){
	if($(this).closest('.detail').hasClass('text')){
		if($('.ui-hide.data .textarea textarea[name='+$(xobjClick).attr('qid')+']').length>0){
			$('.ui-hide.data .textarea textarea[name='+$(xobjClick).attr('qid')+']').val($('.text.detail textarea').val());
		}else if($('.text.detail textarea').val()!=''){
			$('.ui-hide.data .textarea').append("<textarea name='"+$(xobjClick).attr('qid')+"'>"+$('.text.detail textarea').val()+"</textarea>");
		}
	}
	if($(this).closest('.detail').hasClass('display')){
		$('.detail.display').addClass('ui-hide');
	}else{
		$('.detail').addClass('ui-hide');
	}
}).on('click','.text.detail .ui-body .ui-icon.bin',function(e,data){
	$(this).closest('.ui-body').find('textarea').val('').focus();
}).on('click','.main .ui-body .ui-menu .history',function(e,data){
	$('.history.detail').removeClass('ui-hide');
	if($('.detail.history .ui-menu .ui-icon.ui-btn-active').length==0){
		$('.detail.history .ui-menu .ui-icon.event').addClass('ui-btn-active');
	}
	throttledresizeHandler();
}).on('click','.detail.history .ui-menu .ui-icon',function(e,data){
	$('.detail.history .ui-menu .ui-icon').removeClass('ui-btn-active');
	$(this).addClass('ui-btn-active')
}).on('click','.detail.history .ui-filter .date .ui-text,.detail.history .ui-filter .date .ui-icon',function(e,data){
	xobjClick=$(this).closest('.date').find('.ui-text');
	$('#dbox').datebox({popupPosition: 'window'});
	$('#dBox').datebox('open');
	$('.ui-popup-container .ui-datebox-container .ui-link').removeClass('ui-btn-left').addClass('ui-btn-right');
	$('.ui-popup-container .ui-datebox-container .ui-header').removeClass('ui-bar-a').addClass('ui-bar-c');
}).on('click','.detail.history .ui-filter .ui-icon.check',function(e,data){
	var xobj=$('.detail.history .ui-filter .date .ui-text');
	for(var i=0;i<xobj.length;i++){
		if($(xobj[i]).html()==''){
			$('.my-popup.alert').attr('action','click');
			xobjAlert=$(xobj[i]);
			$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
			$('.my-popup.alert .ui-text').html("ระบุ"+$(xobj[i]).closest('.date').find('.ui-label').html());
			popupOpen($('.my-popup.alert'),99999);
			return;
		}
	}
	getHistory("sDate="+$('.detail.history .ui-filter .date.sDate .ui-text').html()+"&fDate="+$('.detail.history .ui-filter .date.fDate .ui-text').html());
}).on('click','.detail.history .ui-body .ui-body-c .person .ui-label,.detail.history .ui-body .ui-body-c .event',function(e,data){
	if($(this).hasClass('.ui-text.collapsed-off')||$(this).hasClass('.ui-text.collapsed-on')){return;}
	getHistoryDisplay("docID="+$(this).closest('.ui-body-c').attr('did'));
}).on('click','.detail.history .ui-body .ui-body-c .person .ui-text',function(e,data){
	if($(this).hasClass('collapsed-off')){$(this).removeClass('collapsed-off').addClass('collapsed-on');}else{$(this).addClass('collapsed-off').removeClass('collapsed-on');}
}).on('click','.detail.display .ui-body .questionnaire .question .ui-line.ui-btn-active',function(e,data){
	
});
// ============================================================ //
// ======================= control image ====================== //
$(document).on('click','.questionnaire.lv1 .image .ui-text.ui-blank,.questionnaire.lv1 .image .ui-text .box.ui-blank',function(e,data){
	var n1 = parseInt($('.ui-hide.data .file .count').html());
	n1+=1;
	$('.ui-hide.data .file').append("<input id='file"+n1+"' name='lv1' class='ui-hide' type='file' accept='image/*'/>");
	$('.ui-hide.data .file .count').html(n1);
	$('#file'+n1).click();
}).on('click','.image.detail .ui-body .box.ui-blank',function(e,data){
	var n1 = parseInt($('.ui-hide.data .file .count').html());
	n1+=1;
	$('.ui-hide.data .file').append("<input id='file"+n1+"' name='"+$(xobjClick).attr('qid')+"' class='ui-hide' type='file' accept='image/*'/>");
	$('.ui-hide.data .file .count').html(n1);
	$('#file'+n1).click();
}).on('change','input[type=file]',function(e,data) {
	var fileid=$(this).attr('id');
	var name=$(this).attr('name');
	var xobj='';
	if(this.files&&this.files[0]){
		var reader=new FileReader();
		reader.onload=function(e){
			switch(name){
			case'lv1':
				$('.questionnaire.lv1 .image .ui-text').removeClass('ui-blank');
				$('.questionnaire.lv1 .image .ui-text').prepend("<div class='box ui-show' fid='"+fileid+"' name='"+name+"'><div class='ui-icon delete'></div><img src='"+e.target.result+"'/></div>")
				if($('.questionnaire.lv1 .image .ui-text .box.ui-blank').length==0){$('.questionnaire.lv1 .image .ui-text').append("<div class='box ui-blank'></div>");}
			break;
			default:
				$('.image.detail .ui-body').prepend("<div class='box ui-show' fid='"+fileid+"' name='"+name+"'><div class='ui-icon delete'></div><img src='"+e.target.result+"'/></div>")
			}
		};
		reader.readAsDataURL(this.files[0]);
	}
}).on('click','.questionnaire.lv1 .image .ui-text .box .ui-icon.delete,.image.detail .ui-body .box .ui-icon.delete',function(e,data){
	if($(this).closest('.box').attr('fid')!=undefined){$(this).closest('.box').remove();$('#'+$(this).closest('.box').attr('fid')).remove();}else{$(this).closest('.box').addClass('ui-hide delete').removeClass('ui-show');}
	if($(this).closest('.box').attr('name')=='lv1'&&$('.questionnaire.lv1 .image .ui-text .box.ui-show').length==0){$('.questionnaire.lv1 .image .ui-text').addClass('ui-blank');$('.questionnaire.lv1 .image .ui-text .box.ui-blank').remove();}
}).on('click','.image.detail .ui-footer .ui-text',function(e,data){
	var xobj=$('.image.detail .ui-body .box.ui-show')
	for(var i=0;i<xobj.length;i++){
		if($(xobj[i]).attr('fid').length<36){$(xobj[i]).remove();}else{$(xobj[i]).addClass('ui-hide delete');}
	}
});
// ============================================================ //
// =========================== Popup ========================== //
$(document).on('click','.my-popup .ui-btn.close,.my-popup .ui-header .ui-icon.close',function(e,data){
	popupClose($(this).closest('.my-popup'));
	switch($(this).closest('.my-popup').attr('action')){
	case'relogin':
		window.location='/';
	break;
	case'home':
		showHome();
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
function defaultLoad(){
	keepSession('/questionnaire/keepSession');
	getChoice();
	$('div[data-role=page]').page({theme:'c',});
	$('.ui-header .ui-bar').addClass('ui-bar-c');
	$(window).on("throttledresize",throttledresizeHandler);
	throttledresizeHandler();
}
function throttledresizeHandler(){
	$('.wrapper').css('height',$(window).height());
	$('.main .ui-body').css('height',$(window).height()-$('.main .ui-header').height()-$('.main .ui-footer').height()-10);
	$('.detail.text .ui-body').css('height',$(window).height()-$('.detail.text .ui-header').height()-$('.detail.text .ui-footer').height()-10);
	$('.detail.pending .ui-body').css('height',$(window).height()-$('.detail.pending .ui-header').height()-$('.detail.pending .ui-footer').height()-10);
	$('.detail.image .ui-body').css('height',$(window).height()-$('.detail.image .ui-header').height()-$('.detail.image .ui-footer').height()-10);
	$('.detail.person .ui-body').css('height',$(window).height()-$('.detail.person .ui-header').height()-$('.detail.person .ui-filter').height()-$('.detail.person .ui-footer').height()-50);
	$('.detail.summary .ui-body').css('height',$(window).height()-$('.detail.summary .ui-header').height()-10);
	$('.detail.question .ui-body').css('height',$(window).height()-$('.detail.question .ui-header').height()-10);
	$('.detail.history .ui-body').css('height',$(window).height()-$('.detail.history .ui-header').height()-$('.detail.history .ui-filter').height()-50);
	$('.detail.display .ui-body').css('height',$(window).height()-$('.detail.display .ui-header').height()-120);
}
function showHome(){
	$('.main .ui-header .ui-title .ui-text').empty().removeClass('ui-shadow').removeClass('ui-corner-all');
	if($('.main .ui-header .ui-title .ui-icon.inbox').attr('pending')!=undefined){
		$('.main .ui-header .ui-title .ui-icon.inbox').removeClass('empty');
	}else{
		$('.main .ui-header .ui-title .ui-icon.inbox').addClass('empty');
	}
	$('.main .ui-header .info').addClass('ui-hide').removeClass('ui-show');
	$('.main .ui-header .bag').addClass('circle').removeClass('bag');
	$('.main .ui-footer').addClass('ui-hide').removeClass('ui-show');
	$('.main .ui-body .ui-menu').removeClass('ui-hide').addClass('ui-show');
	$('.main .ui-body .questionnaire').addClass('ui-hide').removeClass('ui-show');
	$('.main .ui-body .summary').addClass('ui-hide').removeClass('ui-show');
	$('.detail').addClass('ui-hide');
}
function showQLv1(){
	xPage='qLv1';
	$('.main .ui-header .ui-title').removeClass('ui-slider-track');
	$('.main .ui-header .ui-title .ui-icon.inbox').addClass('empty');
	$('.main .ui-header .info').removeClass('ui-hide').addClass('ui-show');
	$('.main .ui-header .circle').addClass('bag').removeClass('circle');
	$('.main .ui-footer').removeClass('ui-hide').addClass('ui-show');
	$('.main .ui-footer .ui-icon.download').addClass('next').removeClass('download');
	$('.main .ui-body .ui-menu').addClass('ui-hide').removeClass('ui-show');
	$('.questionnaire .question').addClass('ui-hide').removeClass('ui-show');
	$('.main .ui-body .summary').addClass('ui-hide').removeClass('ui-show');
	$('.questionnaire .question[mid='+$('.questionnaire.lv1').attr('mid')+']').removeClass('ui-hide');
	$('.questionnaire').addClass('ui-hide').removeClass('ui-show');
	$('.questionnaire.lv1').removeClass('ui-hide').addClass('ui-show');
	if($('.questionnaire.lv1').attr('name')=='event'){
		$('.main .ui-header .ui-title .ui-text').html("ประเมินงาน/สถานการณ์");
		$('.questionnaire.lv1 .docNo').closest('.ui-line').removeClass('ui-hide');
	}else{
		$('.main .ui-header .ui-title .ui-text').html("ประเมินผู้เกี่ยวข้อง");
		$('.questionnaire.lv1 .docNo').closest('.ui-line').addClass('ui-hide');
	}
	$('.questionnaire.lv1 .eventName input').focus();
	$('.summary .question').addClass('ui-hide').removeClass('ui-show');
	$('.summary .question.'+$('.questionnaire.lv1').attr('mid')).removeClass('ui-hide').addClass('ui-show');
}
function showQLv2(){
	xPage='qLv2';
	$('.main .ui-header .ui-title').removeClass('ui-slider-track');
	$('.main .ui-header .ui-title .ui-text').html($('.questionnaire.lv1 .eventName input').val());
	$('.main .ui-body .questionnaire').addClass('ui-hide').removeClass('ui-show');
	$('.main .ui-body .questionnaire.lv2').removeClass('ui-hide').addClass('ui-show');
	$('.main .ui-body .summary').addClass('ui-hide').removeClass('ui-show');
	$('.main .ui-footer .ui-icon.download').addClass('next').removeClass('download');
}
function showSummary(){
	$('.main .ui-header .ui-title').addClass('ui-slider-track');
	$('.main .ui-header .ui-title .ui-text').addClass('ui-shadow ui-corner-all').html($('.questionnaire.lv1 .eventName input').val());
	$('.main .ui-body .questionnaire').addClass('ui-hide').removeClass('ui-show');
	$('.main .ui-body .summary').removeClass('ui-hide').addClass('ui-show');
	$('.main .ui-footer .ui-icon.next').removeClass('next').addClass('download');
}
function getChoice(condition){
	$.ajax({
		type:'POST',
		url:'/questionnaire/questionnaireGetChoose',
		data:condition,
		success:function(data){
			var xobjQ,xobj,xline;
			for(var i=0;i<data.length;i++){
				switch(data[i].dataType){
				case'relogin':
					$('.my-popup.alert').attr('action','relogin');
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(xSessionExpire);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'error':
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(data[i].errorMsg);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'menu':
					$('.wrapper .main .ui-body .ui-menu .menu').append("<button class='ui-btn ui-shadow ui-corner-all ui-mini ui-btn-active "+data[i].menuName+"' xid='"+data[i].menuID+"'></button>")
					$('.detail.history .ui-filter .ui-menu').append("<div class='ui-slider-track ui-body-c "+data[i].menuName+"' xid='"+data[i].menuID+"'><div class='ui-icon ui-shadow ui-corner-all choice "+data[i].menuName+"'></div></div>")
				break;
				case'pending':
					$('.main .ui-header .ui-title .ui-icon.inbox').attr('pending','1').removeClass('empty');
				break;
				case'question':
					xline="<div class='question' qid='"+data[i].questionID+"' mid='"+data[i].menuID+"'><div class='qMain "+data[i].questionID+"'><div class='ui-body-c ui-shadow ui-corner-all'><div><div class='ui-text'>"+data[i].questionName+"</div><div class='ui-icon dot'></div></div><div class='answer'></div></div></div></div>";
					if(data[i].questionRefID==''){
						xobjQ=$('.wrapper .main .ui-body .questionnaire.'+data[i].headerName+' .qList');
						if($('.main .summary .question[qid='+data[i].questionID+']').length==0){
							$('.main .ui-body .summary').append("<div class='question ui-slider-track ui-corner-all "+data[i].menuID+"' qid='"+data[i].questionID+"'><div class='ui-line ui-body-c ui-shadow ui-corner-all'><div class='ui-text'>"+data[i].questionName+"</div><div class='ui-icon'></div></div></div>")
							$('.detail.display .ui-body .questionnaire').append("<div class='question ui-slider-track ui-corner-all "+data[i].menuID+"' qid='"+data[i].questionID+"'><div class='ui-line ui-body-c ui-shadow ui-corner-all'><div class='ui-text'>"+data[i].questionName+"</div><div class='ui-icon select'></div></div><div class='sub'></div></div>")
						}
					}else{
						xobj=$('.wrapper .main .ui-body .questionnaire.'+data[i].headerName+' .qList .question[qid='+data[i].questionRefID+']');
						if(xobj.length==0){
							xobjQ=$('.wrapper .main .ui-body .questionnaire.'+data[i].headerName+' .qList');
						}else{
							xobjQ=$(xobj).find('.qSub.'+data[i].questionRefID);
						}
					}
					xobj=$('.wrapper .main .ui-body .questionnaire.'+data[i].headerName+' .qList .question[qid='+data[i].questionID+']');
					if(xobj.length==0){$(xobjQ).append(xline);}
					xobj=$('.wrapper .main .ui-body .questionnaire.'+data[i].headerName+' .qList .question[qid='+data[i].questionID+']');
					if(data[i].subQuestion>0&&$(xobj).find('.qMain.'+data[i].questionID+' .ui-icon.plus').length==0){
						$(xobj).find('.qMain').append("<div class='ui-icon plus'></div>")
						$(xobj).append("<div class='qSub "+data[i].questionID+" ui-hide'></div>");
					}
					if($(xobj).closest('.qSub').length>0&&$(xobj).find('.ui-icon.collapsed.off').length==0){$(xobj).find('.qMain.'+data[i].questionID).prepend("<div class='ui-icon collapsed off'></div>");}
					$(xobj).find('.qMain.'+data[i].questionID+' .answer').append("<div class='ui-icon "+data[i].answerName+"' aid='"+data[i].answerID+"'></div>");
					if ($('.ui-hide.data .answer [qid='+data[i].questionID+'][aid='+data[i].answerID+']').length>0){
						$(xobj).find('.qMain.'+data[i].questionID+' .answer .ui-icon[aid='+data[i].answerID+']').click();
					}
				break;
				}
			}
			$('.wrapper .main .ui-body .ui-menu .menu').trigger('create');
		},complete:function(){
		}
	});
}
function getPending(){
	loadPage();
	$.ajax({
		type:'POST',
		url:'/questionnaire/questionnaireGetDocPending',
		data:null,
		success:function(data){
			var xline;
			for(var i=0;i<data.length;i++){
				switch(data[i].dataType){
				case'relogin':
					$('.my-popup.alert').attr('action','relogin');
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(xSessionExpire);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'error':
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(data[i].errorMsg);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'pending':
					if($('.detail.pending .ui-body .job .ui-body-c[did='+data[i].docID+']').length==0){
						xline="<div class='job'><div class='ui-body-c ui-shadow ui-corner-all' did='"+data[i].docID+"'>"+data[i].backlogName+"<div class='ui-text'>"+data[i].backlogDesc+"</div><div class='ui-date'>"+data[i].docDate+"</div></div><div class='ui-icon bin'></div></div>";
						$('.detail.pending .ui-body').append(xline);
					}
				break;
				}
			}
		},complete:function(){
			unloadPage();
			$('.pending.detail').removeClass('ui-hide');
			$('.pending.detail .ui-filter input').focus();
			throttledresizeHandler();
		}
	});
}
function getHistory(condition){
	loadPage();
	$('.detail.history .ui-body').empty();
	$.ajax({
		type:'POST',
		url:'/questionnaire/questionnaireGetHistory',
		data:condition,
		success:function(data){
			var xline;
			for(var i=0;i<data.length;i++){
				switch(data[i].dataType){
				case'relogin':
					$('.my-popup.alert').attr('action','relogin');
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(xSessionExpire);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'error':
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(data[i].errorMsg);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'history':
					if($('.detail.history .ui-body .ui-body-c[did='+data[i].docID+']').length==0){
						xline="<div class='ui-body-c ui-shadow ui-corner-all' did='"+data[i].docID+"'><div class='person'><div class='ui-label'>ชื่อบุคคล</div><div class='ui-text ui-shadow ui-corner-all collapsed-off'>"+data[i].fullName+"</div></div><div class='event'><div class='ui-label'>ชื่องาน/สถานการณ์</div><div class='ui-text'>"+data[i].eventName+"</div></div><div class='ui-date'>"+data[i].eventDate+"</div></div>";
						$('.detail.history .ui-body').append(xline);
					}else{
						$('.detail.history .ui-body .ui-body-c[did='+data[i].docID+'] .person .ui-text').append("<br/>"+data[i].fullName);
					}
				break;
				}
			}
		},complete:function(){
			unloadPage();
			$('.pending.detail').removeClass('ui-hide');
			$('.pending.detail .ui-filter input').focus();
			throttledresizeHandler();
		}
	});
}
function getHistoryDisplay(condition){
	loadPage();
	$('.detail.display .ui-body .main').empty();
	$('.detail.display .ui-body .questionnaire .question').addClass('ui-hide')
	$('.detail.display .ui-body .questionnaire .question .ui-line.ui-btn-active').removeClass('ui-btn-active');
	$('.detail.display .ui-body .questionnaire .question .sub').empty();
	$.ajax({
		type:'POST',
		url:'/questionnaire/questionnaireGetHistoryDisplay',
		data:condition,
		success:function(data){
			var xline;
			for(var i=0;i<data.length;i++){
				switch(data[i].dataType){
				case'relogin':
					$('.my-popup.alert').attr('action','relogin');
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(xSessionExpire);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'error':
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(data[i].errorMsg);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'main':
					$('.detail.display').attr('xid',data[i].docID);
					var eventDate=data[i].eventDate.split(' ');
					$('.detail.display .ui-body .main').html("<div class='ui-line'><b>"+$('.questionnaire.lv1 .date .ui-label').html()+"</b> "+eventDate[0]+" <b>"+$('.questionnaire.lv1 .time .ui-label').html()+"</b> "+eventDate[1]+"</div>");
					$('.detail.display .ui-body .main').append("<div class='ui-line person'><b>"+$('.questionnaire.lv1 .person .ui-label').html()+"</b></div>");
					$('.detail.display .ui-body .main').append("<div class='ui-line'><b>"+$('.questionnaire.lv1 .eventName .ui-label').html()+"</b><br/>"+data[i].eventName+"</div>");
					$('.detail.display .ui-body .main').append("<div class='ui-line'><b>"+$('.questionnaire.lv1 .locationType .ui-label').html()+"</b><br/>"+$('.questionnaire.lv1 .locationType .choice[xid='+data[i].locationType+']').html()+"</div>");
					if($('.questionnaire.lv1 .locationType .choice[xid='+data[i].locationType+']').attr('xid')=='out'){
						$('.detail.display .ui-body .main').append("<div class='ui-line'><b>"+$('.questionnaire.lv1 .eventAddress .ui-label').html()+"</b><br/>"+data[i].eventAddress+"</div>");
					}
					$('.detail.display .ui-body .main').append("<div class='ui-line image'><b>"+$('.questionnaire.lv1 .image .ui-label').html()+"</b><br/></div>");
					$('.detail.display .ui-body .questionnaire .question.'+data[i].menuID).removeClass('ui-hide');
				break;
				case'user':
					$('.detail.display .ui-body .main .ui-line.person').append("<br/>"+data[i].fullName);
				break;
				case'question':
					if(data[i].questionRefID==''){
						$('.detail.display .ui-body .questionnaire .question[qid='+data[i].questionID+'] .ui-line').addClass('ui-btn-active')
						$('.detail.display .ui-body .questionnaire .question[qid='+data[i].questionID+'] .ui-icon').addClass(data[i].answerName)
					}else{
						$('.detail.display .ui-body .questionnaire .question[qid='+data[i].questionRefID+'] .ui-line').addClass('ui-btn-active')
						$('.detail.display .ui-body .questionnaire .question[qid='+data[i].questionRefID+'] .sub').append("<div class='question ui-slider-track ui-corner-all "+data[i].menuID+"' qid='"+data[i].questionID+"'><div class='ui-line ui-body-c ui-shadow ui-corner-all ui-btn-active'><div class='ui-text'>"+data[i].questionName+"</div><div class='ui-icon select "+data[i].answerName+"'></div></div></div>")
					}
				break;
				case'file':
					switch(data[i].fileKey){
					case'lv1':
						$('.detail.display .ui-body .ui-line.image').append("<div class='box ui-show'><img src='"+data[i].filePath+data[i].fileName+"'/></div>");
					break;
					}
				break;
				}
			}
		},complete:function(){
			unloadPage();
			$('.detail.display').removeClass('ui-hide');
		}
	});
}
function questionnaireDeletePending(condition){
	loadPage();
	$.ajax({
		type:'POST',
		url:'/questionnaire/questionnaireDeletePending',
		data:condition,
		success:function(data){
			var xline;
			for(var i=0;i<data.length;i++){
				switch(data[i].dataType){
				case'relogin':
					$('.my-popup.alert').attr('action','relogin');
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(xSessionExpire);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'error':
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(data[i].errorMsg);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'success':
					$(xobjClick).closest('.job').remove();
					if($('.detail.pending .ui-body .ui-body-c').length==0){$('.my-popup.alert').attr('action','home');$('.main .ui-header .ui-title .ui-icon.inbox').removeAttr('pending')}
					$('.my-popup.alert .ui-icon').removeClass('warning').addClass('check');
					$('.my-popup.alert .ui-text').html("ลบเรียบร้อย");
					popupOpen($('.my-popup.alert'),99999);
				break;
				}
			}
		},complete:function(){
			unloadPage();
		}
	});
}
function questionnaireDisplay(condition){
	loadPage();
	$.ajax({
		type:'POST',
		url:'/questionnaire/questionnaireDisplay',
		data:condition,
		success:function(data){
			var xline;
			$('.ui-hide.data .file .count').html(0);
			$('.questionnaire.lv1 .person .ui-text').addClass('ui-blank').empty();
			$('.questionnaire.lv1 .image .ui-text').addClass('ui-blank').empty();
			$('.image.detail .ui-body').html("<div class='box ui-blank'></div>");
			$('.question .qMain .answer .ui-icon').removeClass('select');
			$('.ui-hide.data .textarea').empty();
			$('.ui-hide.data .answer').empty();
			for(var i=0;i<data.length;i++){
				switch(data[i].dataType){
				case'relogin':
					$('.my-popup.alert').attr('action','relogin');
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(xSessionExpire);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'error':
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(data[i].errorMsg);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'main':
					$('.questionnaire.lv1').attr('xid',data[i].docID);
					$('.questionnaire.lv1').attr('mid',data[i].menuID).attr('name',data[i].menuName);
					var eventDate=data[i].eventDate.split(' ');
					$('.questionnaire.lv1 .date .ui-text').html(eventDate[0]);
					$('.questionnaire.lv1 .time .ui-text').html(eventDate[1]);
					$('.questionnaire.lv1 .eventName .ui-text input').val(data[i].eventName);
					$('.questionnaire.lv1 .locationType .choice').removeClass('ui-btn-active');
					$('.questionnaire.lv1 .locationType .choice[xid='+data[i].locationType+']').click();
					$('.questionnaire.lv1 .eventAddress .ui-text input').val(data[i].eventAddress);
					$('.questionnaire.lv1 .person .ui-text .ui-icon.user.add').remove();
					$('.questionnaire.lv1 .person .ui-text').addClass('ui-blank').empty();
				break;
				case'user':
					$('.questionnaire.lv1 .person .ui-text').prepend("<div class='user ui-shadow ui-corner-all ui-show' uid='"+data[i].userID+"'>"+data[i].fullName+"<div class='ui-icon delete'></div></div>");
					if($('.questionnaire.lv1 .person .ui-text .ui-icon.user.add').length==0){$('.questionnaire.lv1 .person .ui-text').removeClass('ui-blank').append("<div class='ui-icon user add'></div>");}
				break;
				case'answer':
					if($('.question .qMain.'+data[i].questionID).length>0){
						$('.question .qMain.'+data[i].questionID+' .answer .ui-icon[aid='+data[i].answerID+']').addClass('select');
					}
					$('.ui-hide.data .answer').append("<div qid='"+data[i].questionID+"' aid='"+data[i].answerID+"'></div>");
					if(data[i].answerName!=''){
						$('.ui-hide.data .textarea').append("<textarea name='"+data[i].questionID+"'>"+data[i].answerName+"</textarea>");
					}
				break;
				case'file':
					switch(data[i].fileKey){
					case'lv1':
						$('.questionnaire.lv1 .image .ui-text').removeClass('ui-blank');
						$('.questionnaire.lv1 .image .ui-text').prepend("<div class='box ui-show' name='"+data[i].fileKey+"' fileName='"+data[i].fileName+"'><div class='ui-icon delete'></div><img src='"+data[i].filePath+data[i].fileName+"'/></div>")
						if($('.questionnaire.lv1 .image .ui-text .box.ui-blank').length==0){$('.questionnaire.lv1 .image .ui-text').append("<div class='box ui-blank'></div>");}
					break;
					default:
						$('.image.detail .ui-body').prepend("<div class='box ui-show' name='"+data[i].fileKey+"' fileName='"+data[i].fileName+"'><div class='ui-icon delete'></div><img src='"+data[i].filePath+data[i].fileName+"'/></div>")
					}
				break;
				}
			}
		},complete:function(){
			unloadPage();
			showQLv1();
		}
	});
}
function getUser(){
	loadPage();
	$('.detail.person .ui-body .ui-show').removeClass('ui-show').addClass('ui-hide');
	$('.detail.person .ui-filter input').blur();
	var post1='keyword='+$('.detail.person .ui-filter input').val();
	$.ajax({
		type:'POST',
		url:'/questionnaire/questionnaireGetUser',
		data:post1,
		success:function(data){
			var xline;
			for(var i=0;i<data.length;i++){
				switch(data[i].dataType){
				case'error':
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(data[i].errorMsg);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'user':
					xline="<div class='ui-show ui-slider-track ui-body-c ui-shadow ui-corner-all'><div class='choice' xid='"+data[i].userID+"'>"+data[i].fullName+"</div></div>";
					if($('.detail.person .ui-body .choice[xid='+data[i].userID+']').length==0){$('.detail.person .ui-body').append(xline);}else{$('.detail.person .ui-body .choice[xid='+data[i].userID+']').closest('.ui-body-c').removeClass('ui-hide').addClass('ui-show');}
				break;
				}
			}
		},complete:function(){
			unloadPage();
			$('.person.detail .ui-filter input').focus();
		}
	});
}
function save(isPending){
	var xobj;
	loadPage();
	var formData=new FormData();
	if($('.questionnaire.lv1').attr('xid')!=undefined){formData.append('did',$('.questionnaire.lv1').attr('xid'))}
	formData.append('menuID',$('.questionnaire.lv1').attr('mid'));
	formData.append('menuType',$('.questionnaire.lv1').attr('name'));
	formData.append('eventDate',$('.questionnaire.lv1 .date .ui-text').html()+' '+$('.questionnaire.lv1 .time .ui-text').html());
	formData.append('eventName',$('.questionnaire.lv1 .eventName .ui-text input').val());
	formData.append('locationType',$('.questionnaire.lv1 .locationType .choice.ui-btn-active').attr('xid'));
	formData.append('eventAddress',$('.questionnaire.lv1 .eventAddress .ui-text input').val());
	if(isPending==true){
		formData.append('backlogName',$('.my-popup.pending .backlogName input').val());
		formData.append('backlogDesc',$('.my-popup.pending .backlogDesc input').val());
	}

	xobj=$('.questionnaire.lv1 .person .user');
	for(var i=0;i<xobj.length;i++){
		if($(xobj[i]).hasClass('ui-icon')==false){
			if($(xobj[i]).hasClass('ui-hide')){
				formData.append('refUserIsDelete','y');
			}else{
				formData.append('refUserIsDelete','n');
			}
			formData.append('refUserID',$(xobj[i]).attr('uid'));
		}
	}
	xobj=$('.question .qMain .answer .ui-icon.select');
	for(var i=0;i<xobj.length;i++){
		formData.append('answerIsDelete','n');
		formData.append('questionID',$(xobj[i]).closest('.question').attr('qid'));
		formData.append('answerID',$(xobj[i]).attr('aid'));
		if($('.ui-hide.data .textarea textarea[name='+$(xobj[i]).closest('.question').attr('qid')+']').length==0){
			formData.append('answerDesc','');
		}else{
			formData.append('answerDesc',$('.ui-hide.data .textarea textarea[name='+$(xobj[i]).closest('.question').attr('qid')+']').val());
		}
	}
	xobj=$('.questionnaire.lv1 .image .box.delete');
	for(var i=0;i<xobj.length;i++){
		formData.append('deleteFileName',$(xobj[i]).attr('fileName'));
		formData.append('deleteRefFile',$(xobj[i]).attr('name'));
	}
	xobj=$('.ui-hide.data .file input')
	for(var i=0;i<xobj.length;i++){
		if($(xobj[i]).attr('name')=='lv1'){
			formData.append('refFile',$(xobj[i]).attr('name'));
			formData.append('xFile',document.getElementById($(xobj[i]).attr('id')).files[0]);	
		}else if($('.question[qid='+$(xobj[i]).attr('name')+'] .qMain .answer .ui-icon.select').length>0){
			formData.append('refFile',$(xobj[i]).attr('name'));
			formData.append('xFile',document.getElementById($(xobj[i]).attr('id')).files[0]);	
		}
	}
	$.ajax({
		type: 'POST',
		url: '/questionnaire/questionnaireSave/',
		data: formData,
		contentType: false,
		processData: false,
		success: function (data) {
			switch (data[0].dataType){
			case'relogin':
				$('.my-popup.alert').attr('action','relogin');
				$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
				$('.my-popup.alert .ui-text').html(xSessionExpire);
				popupOpen($('.my-popup.alert'),99999);
			break;
			case'success':
			case'pending':
				if(data[0].dataType=='pending'){
					$('.main .ui-header .ui-title .ui-icon.inbox').attr('pending','1').removeClass('empty');
				}else{
					$('.main .ui-header .ui-title .ui-icon.inbox').removeAttr('pending').addClass('empty');
				}
				$('.my-popup.pending .ui-btn.close').click();
				$('.my-popup.confirm .ui-btn.close').click();
				$('.my-popup.alert').attr('action','home');
				$('.my-popup.alert .ui-icon').removeClass('warning').addClass('check');
				$('.my-popup.alert .ui-text').html("บันทึกเรียบร้อย");
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
function deleteAccount(){
	loadPage();
	$.ajax({
		type:'POST',
		url:'/questionnaire/questionnaireDeleteAccount',
		data:null,
		success:function(data){
			var xline;
			for(var i=0;i<data.length;i++){
				switch(data[i].dataType){
				case'relogin':
					$('.my-popup.alert').attr('action','relogin');
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(xSessionExpire);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'error':
					$('.my-popup.alert .ui-icon').addClass('warning').removeClass('check');
					$('.my-popup.alert .ui-text').html(data[i].errorMsg);
					popupOpen($('.my-popup.alert'),99999);
				break;
				case'success':
					window.location='/';
				break;
				}
			}
		},complete:function(){
			unloadPage();
		}
	});
}
// ============================================================ //

