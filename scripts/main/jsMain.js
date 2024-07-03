function keepSession(url){
window.setInterval(function(){$.ajax({type:'POST',url:url,success:function(data){}});},60000)};
var delay=(function(){
	var timer=0;
	return function(callback,ms){
		clearTimeout(timer);
		timer=setTimeout(callback,ms);
	};
})();
function keyA(e){
	var xkey;
	document.all==true?xkey=window.event.keyCode:xkey=e.which
	return xkey;
}
function keyEnterNextItem(e){
    var xkey=keyA(e);
	if(xkey==13){$(":input")[$(":input").index(document.activeElement) + 1].focus();}
}
function lockNumOnly(xobj){
	var xText=$(xobj).val();
	var xResult='';
	for(var i=0;i<xText.length;i++){
		var xChar=xText.substring(i,i+1);
		var code=xChar.charCodeAt(0);
		if (code>=48&&code<=57){xResult+=xChar;}
	}
	$(xobj).val(xResult);
}
function lockUserName(xobj){
	var xText=$(xobj).val();
	var xResult='';
	for(var i=0;i<xText.length;i++){
		var xChar=xText.substring(i,i+1);
		var code=xChar.charCodeAt(0);
		if ((code>=45&&code<=46)||(code>=48&&code<=57)||(code>=64&&code<=90)||(code>=97&&code<=122)||code==95){xResult+=xChar;}
	}
	$(xobj).val(xResult);
}
function lockUserPass(xobj){
	var xText=$(xobj).val();
	var xResult='';
	for(var i=0;i<xText.length;i++){
		var xChar=xText.substring(i,i+1);
		var code=xChar.charCodeAt(0);
		if (code==33||(code>=35&&code<=38)||(code>=40&&code<=47)||(code>=48&&code<=60)||code==62||(code>=64&&code<=91)||(code>=93&&code<=95)||(code>=97&&code<=125)){xResult+=xChar;}
	}
	$(xobj).val(xResult);
}
Date.prototype.HM=function(){
	var dat = new Date(this.valueOf())
	var hh = dat.getHours();
	var min = dat.getMinutes();
	if(hh<10){hh='0'+hh}
	if(min<10){min='0'+min}
	return hh+':'+min;
}
Date.prototype.DMY=function(){
	var dat = new Date(this.valueOf())
    var dd = dat.getDate();
    var mm = dat.getMonth() + 1;
    var yyyy = dat.getFullYear();
    if(dd<10){dd='0'+dd}
    if(mm<10){mm='0'+mm}
    return dd+'/'+mm+'/'+yyyy;
}
var countDown=(function(xobj,countToTime){
	$(xobj).addClass('ui-disabled');
	var x=setInterval(function(){
		var now = new Date();
		var distance = countToTime - now;
		var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
		var seconds = Math.floor((distance % (1000 * 60)) / 1000);
		if(minutes>0){
			$(xobj).html(minutes + "m " + seconds + "s");
		}else{
			$(xobj).html(seconds + "s");
		}
		if(distance<=0){
			clearInterval(x);
			$(xobj).html($(xobj).attr('title')).removeClass('ui-disabled');
		}
	},1000);
});
// ================= popup ================= //
var popupOpen=(xobj,zIndex)=>{
	$(xobj).removeClass('ui-popup-hidden').addClass('ui-popup-active');
	$(xobj).find('.ui-content').css('max-height',($(window).height()*0.6)+'px');
	if (zIndex==undefined){zIndex = 1201;}
	$(xobj).css({
		"z-index":zIndex,
		"top":(($(window).height()-$(xobj).height())/2.5),
		"left":(($(window).width()-$(xobj).width())/2)
	});
	$('body').append("<div class='ui-popup-screen ui-overlay-b in "+$(xobj).attr('name')+"' name='"+$(xobj).attr('name')+"'></div>")
};
var popupClose=(xobj)=>{
	$(xobj).addClass('ui-popup-hidden').removeClass('ui-popup-active');
	$('body').find('.ui-popup-screen[name='+$(xobj).attr('name')+']').remove();
};
// ========================================= //
// ============== loading page ============= //
var loadPage=()=>{
	$('body').append("<div class='ui-popup-screen ui-overlay-b in' name='loading'></div>")
	$.mobile.loading('show',{text: 'loading',textVisible:true,textonly:false});
};
var unloadPage=()=>{
	$.mobile.loading('hide');
	$('body').find('.ui-popup-screen[name=loading]').remove();
};
// ========================================= //
/*
function showNavPopupDeal() {
    let val_ScrollTop = $(window).scrollTop(); //ใช้ jQuery
    let val_PageOffset = window.pageYOffset; //ใช้ Javascript
    let obj = {
        ScrollTop: val_ScrollTop
        , PageOffset: val_PageOffset
    }
    console.log("intScrollTop =>", obj);

    return val_ScrollTop;
}

$(document).ready(function(){
    //do
    window.onscroll = function () {
        showNavPopupDeal();
    };    
});
var timeleft = 5;
var downloadTimer = setInterval(function () {
  $(".headTimeOut").html(timeleft);

  timeleft -= 1;
  if (timeleft <= 0) {
    clearInterval(downloadTimer);
    //clear
    $(".headTimeOut").html("");
    //go url
    parent.location.href = "URL";
  }
}, 1000);
*/