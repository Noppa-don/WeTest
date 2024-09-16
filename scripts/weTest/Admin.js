// ========================= Page Load ====================================================================== //
$(function () {
    $('div[data-role=page]').page({ theme: 'c', });
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
    //20240826 -- เพิ่มการส่ง JobStatus
    .on('click', '#btnPaymentList, #doingJob', function (e, data) {
        $('.MainMenu').addClass('ui-hide');
        $('.PaymentList').removeClass('ui-hide');
        GetJobDetail(1);
    })
    //20240826 -- เลือกการชำระเงินที่มีปัญหา
    .on('click', '#problemPayment', function (e, data) {
         GetJobDetail(3);
    })
    //20240826 -- เลือกการชำระเงินที่สำเร็จแล้ว
    .on('click', '#successPayment', function (e, data) {
        GetJobDetail(2);
    })
    .on('click', '.seeDetail', function (e, data) {
        var RHId = $(this).attr('RHId');
        ShowJobDetail(RHId)
        $('.PaymentList,.divUploadSlip').addClass('ui-hide');
        $('.PaymentDetail,.footerslip').removeClass('ui-hide');
    })
    //20240820 -- cancel confirm slip
    .on('click', '.btnCancelConfirmSlip', function (e, data) {
        $('.PaymentList').removeClass('ui-hide');
        $('.PaymentDetail,.footerslip,.footerUpdateRejectSlip').addClass('ui-hide');

    })
    //20240820 -- confirm slip
    .on('click', '.btnConfirmSlip', function (e, data) {
        $('#dialogConfirm').attr('action', 'focus');
        popupOpen($('#dialogConfirm'), 99999);
    })
    .on('click', '#dialogConfirm .btnconfirm', function (e, data) {
        var RHId = $(this).attr('RHId');
        SaveConfirmSlip(RHId,2,'');
        popupClose($(this).closest('.my-popup'));
    })
    //20240820 -- reject slip
    .on('click', '.btnReject', function (e, data) {
        $('#dialogReject').attr('action', 'focus');
        popupOpen($('#dialogReject'), 99999);
     })
    .on('click', '#dialogReject .btnRejectConfirm', function (e, data) {
        var RHId = $(this).attr('RHId');
        var RejectReason = $('#txtReason').val();
        popupClose($(this).closest('.my-popup'));
        SaveConfirmSlip(RHId, 3, RejectReason);

    })
    .on('click', '#dialogConfirm .btnNo,#dialogReject .btnNo', function (e, data) {
        popupClose($(this).closest('.my-popup'));
    })
    //20240826 -- Click Menu
    .on('click', '.jobdiv', function (e, data) {
        $('.jobdiv').removeClass('Active');
        $(this).addClass('Active');
    })
    //20240902 -- Select Reject Job
    .on('click', '.UploadNewSlip', function (e, data) {
        var RHId = $(this).attr('RHId');
        ShowJobDetail(RHId)
        $('.PaymentList').addClass('ui-hide');
        $('.PaymentDetail,.footerUpdateRejectSlip,.divUploadSlip').removeClass('ui-hide');

    })
    //20240913 -- Upload NewSlip File
    .on('click', '.btnSlipPhoto', function (e, data) {
        event.preventDefault();
        $("#fileSlip").click();
    })
    //20240913 -- Upload NewSlip File
    .on('change', '#fileSlip', function (e, data) {
     var _URL = window.URL || window.webkitURL;
     var image;
     if ((file = this.files[0])) {
         image = new Image();
         image.onload = function () {
             src = this.src;
             var filesUpload = $("#fileSlip").get(0).files;
             $('#SlipName').val(filesUpload[0].name);
             $('.slipPhoto').css('background-image', 'url(' + src + ')');
             e.preventDefault();
         }
     };
     image.src = _URL.createObjectURL(file);
    })
    //20240913 -- Update Reject Slip
    .on('click', '.btnUploadNewSlip', function (e, data) {
        $('#dialogConfirm').attr('action', 'focus');
        popupOpen($('#dialogConfirm'), 99999);
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
//20240826 -- เพิ่มสถานะการดึง Job แบบต่างๆ
function GetJobDetail(JobStatus) {
    var data = 'JobStatus=' + JobStatus
    $.ajax({
        type: 'POST',
        data: data,
        url: '/weTest/GetJobDetail',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {

                if (data[i].dataType == 'success') {
                    $('.JobDetail').html(data[i].errorMsg);
                    if (JobStatus == 1) {
                        $('.jobDetailItem').addClass('seeDetail');
                    } else if (JobStatus == 3) {
                        $('.jobDetailItem').addClass('UploadNewSlip');
                    }

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
function SaveConfirmSlip(RHId, RegisterStatus, RejectReason) {
    var data = 'RHId=' + RHId + '&RegisterStatus=' + RegisterStatus + '&RejectReason=' + RejectReason
    $.ajax({
        type: 'POST',
        url: '/weTest/ConfirmSlip',
        data: data,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'success') {
                    $('.PaymentDetail').addClass('ui-hide');
                    $('.PaymentList').removeClass('ui-hide');
                }

            }
        }
    });
}






