var xobjClick, xobjAlert;
// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); $('#Firstname').focus(); });

// ============================================================ //
// ======================= Object Event ======================= //
$(document)

// ======================= Register ======================= //
    .on('focus', '#Firstname, #Surname, #MobileNo, #EMail, #Username, #Password, #ConfirmPassword', function (e, data) { $(this).removeClass("InvalidData") })
    .on('keypress blur', '#Firstname, #Surname, #MobileNo, #EMail, #Username, #Password', function (e, data) {
        $('.sp' + $(this).attr('id')).text($(this).val());
        var xkey = keyA(e);
        if (xkey == 13) { keyEnterNextItem(e); }
    })
    .on('keypress blur', '#ConfirmPassword', function (e, data) {
        var xkey = keyA(e);
        if (xkey == 13) {
            if (checkInvalidRegisterData() == 'true') {
                $('#captionConfirmPassword').css("display", "none");
                $('#captionPassword').css("display", "none");
                $('.spnData').css("display", "block");
                $('.txtData').css("display", "none");
            };

            //checkLogin();
        }
    })
    .on('click', '#btnNext', function (e, data) {
        if ($('.spnData').is(':visible')) {
            $('.otp').css("display", "block");
            $('.register').css("display", "none");
            $('.footerRegister').addClass('ui-hide');
        }else{
            if (checkInvalidRegisterData() == 'true') {
                $('#captionConfirmPassword, #captionPassword').css("display", "none");
                $('.spnData').css("display", "block");
                $('#capType').removeClass('ui-hide');
                $('.txtData,#btnStudent, #btnOther').css("display", "none");


            };
        }
    })
    .on('click', '#btnBack', function (e, data) {
        if ($('.spnData').is(':visible')) {
            $('#captionConfirmPassword, #captionPassword').css("display", "block");
            $('.spnData').css("display", "none");
            $('.txtData').css("display", "block");
        } else {
            window.location = '/Wetest/User';
        }

    })
    .on('click', '.btnPhoto', function (e, data) {
        event.preventDefault();
        $("#file").click();
    })
    .on('click', '#btnStudent, #btnOther', function (e, data) {
        $('#btnStudent, #btnOther').removeClass("btnSelected")
        $(this).addClass("btnSelected");
        $('.spType').text($(this).text());
    })
    .on('change', '#file', function (e, data){
        var _URL = window.URL || window.webkitURL;
        var image, file;
        if ((file = this.files[0])) {
            image = new Image();
            image.onload = function () {
                src = this.src;
                $('.btnPhoto').css('background', 'none');
                $('.btnPhoto').css('width', 'auto');
                $('.btnPhoto').html('<img id="imgUser" src="' + src + '">');
                e.preventDefault();
            }
        };
       image.src = _URL.createObjectURL(file);
    })

// ======================= OTP ======================= //
    .on('keyup', '#txtOTP', function (e, data) {
     if ($('#txtOTP').val().length == 6) {
         $('#btnConfirm').removeClass('btnUnActive');
     }

     var xkey = keyA(e);
     if (xkey == 8) {
         if ($('#txtOTP').val().length < 6) {
             $('#btnConfirm').addClass('btnUnActive');
         }
     }
 })

// ============================================================ //
// =========================== Popup ========================== //

$(document).on('click', '.my-popup .ui-btn.close,.my-popup .ui-header .ui-icon.close', function (e, data) {
    popupClose($(this).closest('.my-popup'));
    switch ($(this).closest('.my-popup').attr('action')) {
        case 'relogin':
            window.location = '/';
            break;
        case 'no permission':
            window.location = '/global/menu';
            break;
        case 'home':
            break;
        case 'click':
            $(xobjAlert).click();
            break;
        case 'focus':
            $(xobjAlert).focus();
            break;
    }
}).on('click', '.ui-popup-screen', function (e, data) {
    popupClose($('.my-popup[name=' + $(this).attr('name') + ']'));
});

// ============================================================ //
// ========================= Function ========================= //

function checkInvalidRegisterData() {

    var CheckError = 'true';

    if ($('#Firstname').val() == '') { $('#Firstname').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#Surname').val() == '') { $('#Surname').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#MobileNo').val() == '' || $('#MobileNo').val().length != 10) { $('#MobileNo').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#EMail').val() == '') { $('#EMail').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#Username').val() == '') { $('#Username').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#Password').val() == '') { $('#Password').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#ConfirmPassword').val() == '') { $('#ConfirmPassword').addClass("InvalidData"); CheckError = 'false'; }

    if ($('#ConfirmPassword').val() != $('#Password').val()) {
        $('#Password, #ConfirmPassword').addClass("InvalidData");
        CheckError = 'false';
    }

    if ($('#btnStudent, #btnOther').hasClass("btnSelected")) {} else {
        $('#btnStudent, #btnOther').addClass("InvalidData");
        CheckError = 'false';
    }

    return CheckError
}
function checkLogin() {
    var xobj = $('.login input');
    for (var i = 0; i < xobj.length; i++) {
        if ($(xobj[i]).val() == '') {
            $('.my-popup.alert').attr('action', 'focus');
            $('.my-popup.alert .ui-text').html('กรุณาระบุ' + $(xobj[i]).attr('title'));
            popupOpen($('.my-popup.alert'), 99999);
            xobjAlert = $(xobj[i]);
            return;
        }
    }
    loadPage();
    var post1 = 'userName=' + $('#userName').val() + '&password=' + $('#userPass').val();
    $.ajax({
        type: 'POST',
        url: '/questionnaire/checkLogin',
        data: post1,
        success: function (data) {
            var xline;
            for (var i = 0; i < data.length; i++) {
                switch (data[i].dataType) {
                    case 'error':
                        $('.my-popup.alert .ui-text').html(data[i].errorMsg);
                        popupOpen($('.my-popup.alert'), 99999);
                        break;
                    default:
                        window.location = '/questionnaire/questionnaire';
                }
            }
        }, complete: function () {
            unloadPage();
        }
    });
}
