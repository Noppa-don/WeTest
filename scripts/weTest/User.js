var xobjClick, xobjAlert, OTPNum;

// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); $('#userName').focus(); });

// ============================================================ //

// ======================= Object Event ======================= //
$(document)
// ======================= Login ======================= //

    .on('focus', '#userName, #userPass', function (e, data) { $(this).removeClass("InvalidData") })

    .on('keypress blur', '#userName, #userPass', function (e, data) {
        var xkey = keyA(e);
        if (xkey == 13) { keyEnterNextItem(e); }
    })
    .on('keypress blur', '#ConfirmPassword', function (e, data) {
        var xkey = keyA(e);
        if (xkey == 13) { checkLogin(); }
    })

    .on('click', '#btnLogin', function (e, data) {
        if (checkInvalidLoginData() == 'true') {
            CheckUserLogin();
        }
    })
    .on('click', '#dialogAlert #btnOK', function (e, data) {
        popupClose($(this).closest('.my-popup'));
        $('#userName').focus();
    })
    .on('click', '.registerlink', function (e, data) {
        window.location = '/Wetest/Registration';
    })




// ============================================================ //

// ========================= Function ========================= //

function checkInvalidLoginData() {

    var CheckError = 'true';

    if ($('#userName').val() == '') { $('#userName').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#userPass').val() == '') { $('#userPass').addClass("InvalidData"); CheckError = 'false'; }

    return CheckError
}

function CheckUserLogin() {
    var post1 = 'Username=' + $('#userName').val() + '&Password=' + $('#userPass').val();
    console.log(post1);
    $.ajax({
        type: 'POST',
        url: '/weTest/CheckUserLogin',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'success') {
                    $('.login').addClass('ui-hide');
                    $('.MainMenu').removeClass('ui-hide');
                } else {
                    $('#dialogAlert').attr('action', 'focus');
                    $('#dialogAlert .ui-text').html(data[i].errorMsg);
                    popupOpen($('#dialogAlert'), 99999);
                }

            }
        }
    });
}

