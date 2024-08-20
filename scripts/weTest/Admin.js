// ========================= Page Load ====================================================================== //
$(function () {
    $('div[data-role=page]').page({ theme: 'c', });
    $('#userName').focus();
});

// ========================================================================================================== //

// ============================================== Object Event ============================================== //
$(document)
// ======================== Login ============================== //
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
    .on('click', '#btnPaymentList', function (e, data) {
        $('.MainMenu').addClass('ui-hide');
        $('.PaymentList').removeClass('ui-hide');
        GetJobDetail();
    })


function CheckUserLogin() {
    $.ajax({
        type: 'POST',
        url: '/weTest/CheckUserLogin',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {

                if (data[i].dataType == 'success') {
              
                }
            }
        }
    });
}

function GetJobDetail() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetJobDetail',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {

                if (data[i].dataType == 'success') {
                    $('.JobDetail').html(data[i].errorMsg);
                }

            }
        }
    });
}




