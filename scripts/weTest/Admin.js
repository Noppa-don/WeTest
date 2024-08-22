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
    .on('click', '.jobDetailItem', function (e, data) {
        var RHId = $(this).attr('RHId');
        ShowJobDetail(RHId)
        $('.PaymentList').addClass('ui-hide');
        $('.PaymentDetail,.footerslip').removeClass('ui-hide');

    })
    //20240820 -- cancel confirm slip
    .on('click', '.btnCancelConfirmSlip', function (e, data) {
        $('.PaymentList').removeClass('ui-hide');
        $('.PaymentDetail,.footerslip').addClass('ui-hide');

    })
    //20240820 -- confirm slip
    .on('click', '.btnConfirmSlip', function (e, data) {
        $('#dialogConfirm').attr('action', 'focus');
        popupOpen($('#dialogConfirm'), 99999);
    })
    .on('click', '#dialogConfirm .btnconfirm', function (e, data) {
        var RHId = $(this).attr('RHId');
        SaveConfirmSlip(RHId,2);
        popupClose($(this).closest('.my-popup'));
    })

    //20240820 -- reject slip
    .on('click', '.btnReject', function (e, data) {
        $('#dialogReject').attr('action', 'focus');
        popupOpen($('#dialogReject'), 99999);
     })
    .on('click', '#dialogReject .btnRejectConfirm', function (e, data) {
        var RHId = $(this).attr('RHId');
        SaveConfirmSlip(RHId, 3);
        popupClose($(this).closest('.my-popup'));
    })

    .on('click', '#dialogConfirm .btnNo,#dialogReject .btnNo', function (e, data) {
        popupClose($(this).closest('.my-popup'));
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
//20240820 -- ดึงข้อมูลรายละเอียดสลิป
function ShowJobDetail(RHId) {
    var data = 'RHId=' + RHId
    $.ajax({
        type: 'POST',
        url: '/weTest/GetSlip',
        data: data,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {

                if (data[i].Result == 'success') {
                    $('.slipDetail').html(data[i].ResultTxt);
                    $('.slipPhoto').css('background-image', 'url(' + data[i].slipURL + ')');
                    $('#dialogConfirm .btnconfirm').attr('RHId', RHId);
                    $('#dialogReject .btnRejectConfirm').attr('RHId', RHId);
                }
            }
        }
    });
}
//20240820 -- บันทึกสถานะ
function SaveConfirmSlip(RHId,RegisterStatus) {
    var data = 'RHId=' + RHId + '&RegisterStatus=' + RegisterStatus
    $.ajax({
        type: 'POST',
        url: '/weTest/ConfirmSlip',
        data: data,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {


            }
        }
    });
}




