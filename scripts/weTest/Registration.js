var OTPNum, PackagePrice, StudentType, file;

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

    .on('click', '.footerRegister .btnNext', function (e, data) {
        if ($('.spnData').is(':visible')) {
            SaveNewUser();
        } else {
            if (checkInvalidRegisterData() == 'true') {
                $('#captionConfirmPassword, #captionPassword').css("display", "none");
                $('.spnData').css("display", "block");
                $('#capType').removeClass('ui-hide');
                $('.txtData,#btnStudent, #btnOther').css("display", "none");
            };
        }
    })
    .on('click', '.footerRegister .btnBack', function (e, data) {
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
        $('#btnStudent, #btnOther').removeClass("InvalidData");
        StudentType = $(this).attr("stdType");
        $(this).addClass("btnSelected");
        $('.spType').text($(this).text());
    })

    .on('change', '#file', function (e, data) {
        var _URL = window.URL || window.webkitURL;
        var image;
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
    .on('click', '#btnSendAgain', function (e, data) {
        $('#btnSendAgain').addClass('btnUnActive');
        sendOTP()
    })
    .on('click', '#btnConfirm', function (e, data) {
        var ttb = $('#txtOTP').val();
        if ($('#txtOTP').val() == '') { $('#txtOTP').addClass("InvalidData"); } else {
            if (ttb == OTPNum) {
                UpdateOTPStatus(2);
                $('.otp ,.footerOTP').addClass('ui-hide');
                $('.payment ,.footerPayment').removeClass('ui-hide');
                GetPackagePrice();
            } else {
                UpdateOTPStatus(1);
                $('#dialogConfirm').attr('action', 'focus');
                $('#dialogConfirm .ui-text').html('OTP is wrong! Please try again');
                popupOpen($('#dialogConfirm'), 99999);
            }
        }
    })

// ======================= Payment ======================= //
     .on('focus keypress', '#txtDiscountCode', function (e, data) {
         $(this).removeClass("InvalidData");
         $('#dialogDiscount .ui-Warning-red').addClass('ui-hide');
     })

    .on('click', '#btnCheckKey', function (e, data) {
        CheckKeycode();
    })
    .on('click', '#btnPayment', function (e, data) {
        $(this).addClass("ui-hide");
        $('#spnWarning').text('You can use Mobile Banking for scan to pay.');
        $('.btnQR').removeClass("ui-hide");
    })
    .on('click', '#btnDiscount', function (e, data) {
        popupOpen($('#dialogDiscount'), 99999);
    })
    .on('click', '#dialogConfirm #btnOK ,#dialogSelect .btnCancel', function (e, data) {
        popupClose($(this).closest('.my-popup'));
    })
    .on('click', '.ui-icon.close ', function (e, data) {
        $('#txtDiscountCode').val('');
        $('#dialogDiscount .ui-textWarning').addClass('ui-hide');
        popupClose($(this).closest('.my-popup'));
    })
    .on('click', '#dialogSelect .btnSelected', function (e, data) {

        popupClose($(this).closest('.my-popup'));
        GotoQuiz();
    })
    .on('click', '#dialogDiscount #btnCheckDiscountCode', function (e, data) {
        CheckDiscount();
    })

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

    if ($('#btnStudent, #btnOther').hasClass("btnSelected")) { } else {
        $('#btnStudent, #btnOther').addClass("InvalidData");
        CheckError = 'false';
    }

    return CheckError
}
function SaveNewUser() {
    var post1 = 'FirstName=' + $('.spFirstname').text() + '&Surname=' + $('.spSurname').text() + '&MobileNo=' + $('.spMobileNo').text() +
        '&EMail=' + $('.spEMail').text() + '&Username=' + $('.spUsername').text() + '&Password=' + $('#Password').val() + '&StudentType=' + StudentType;
    $.ajax({
        type: 'POST',
        url: '/weTest/SaveUser',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                switch (data[i].dataType) {
                    case 'error':
                        console.log(data[i].errorMsg);
                    case 'success':
                        UploadStudentPhoto();
                }
            }
        }
    });
}
function UploadStudentPhoto() {

    var data = new FormData();

    var files = $("#file").get(0).files;

    if (files.length > 0) {
        console.log(0);
        data.append("UploadedImage", files[0]);

        $.ajax({
            type: 'POST',
            url: '/weTest/UploadStudentPhoto',
            contentType: false,
            processData: false,
            data: data,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    switch (data[i].dataType) {
                        case 'error':
                            console.log(data[i].errorMsg);
                        case 'success':
                            console.log(data[i].errorMsg);
                            $('.otp').removeClass('ui-hide');
                            $('.register').addClass('ui-hide');
                            $('.footerRegister').addClass('ui-hide');
                            sendOTP();
                    }
                }
            }
        });
    } else {
       
        $.ajax({
            type: 'POST',
            url: '/weTest/UploadDummyStudentPhoto',
            contentType: false,
            processData: false,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    switch (data[i].dataType) {
                        case 'error':
                            console.log(data[i].errorMsg);
                        case 'success':
                            console.log(data[i].errorMsg);
                            $('.otp').removeClass('ui-hide');
                            $('.register').addClass('ui-hide');
                            $('.footerRegister').addClass('ui-hide');
                            sendOTP();
                    }
                }
            }
        });
    }
}
function sendOTP() {
    OTPNum = Math.floor(100000 + Math.random() * 900000);
   // console.log(OTPNum);

    var post1 = 'MobileNo=' + $('.spMobileNo').text() + '&OTPNum=' + OTPNum;
    $.ajax({
        type: 'POST',
        url: '/weTest/sendOTP',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                switch (data[i].dataType) {
                    case 'error':
                        console.log(data[i].errorMsg);
                    case 'success':
                        var startTime = new Date().getTime()
                        var countDownDate = new Date();

                        countDownDate.setSeconds(countDownDate.getSeconds() + 10);

                        var x = setInterval(function () {
                            var now = new Date().getTime();
                            var distance = countDownDate - now;
                            if (distance < 0) {
                                clearInterval(x);
                                $('#btnSendAgain').removeClass('btnUnActive');
                            }
                        }, 1000);
                }
            }
        }
    });
}
function UpdateOTPStatus(OTPStatus) {
    var post1 = 'OTPStatus=' + OTPStatus
    $.ajax({
        type: 'POST',
        url: '/weTest/UpdateOTPStatus',
        data: post1,
        success: function (data) {

        }
    });
}
function GetPackagePrice() {
    console.log('PackagePrice');
    $.ajax({
        type: 'POST',
        url: '/weTest/GetPackagePrice',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                PackagePrice = data[i].dataType
                $('#PackagePrice').text(PackagePrice);
            }
        }
    });
}
function CheckKeycode() {
    if ($('#txtKeyCode').val() == '') { $('#txtKeyCode').addClass("InvalidData"); } else {
        var post1 = 'KeyCode=' + $('#txtKeyCode').val();
        $.ajax({
            type: 'POST',
            url: '/weTest/CheckKeyCode',
            data: post1,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {

                    if (data[i].dataType == 'success') {
                        $('#dialogSelect').attr('action', 'focus');
                        $('#dialogSelect .ui-text').html(data[i].errorMsg);
                        popupOpen($('#dialogSelect'), 99999);
                    } else {
                        $('#dialogConfirm').attr('action', 'focus');
                        $('#dialogConfirm .ui-text').html(data[i].errorMsg);
                        popupOpen($('#dialogConfirm'), 99999);
                    }
                }
            }
        });
    }
}
function GotoQuiz() {
    popupOpen($('#dialogGotoQuiz'), 99999);

    $.ajax({
        type: 'POST',
        url: '/weTest/SaveFirstPlacementTest',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'success') {
                    var x = setInterval(function () {
                        clearInterval(x);
                        window.location = '/Wetest/Activity';
                    }, 2000);
                }
            }
        }
    });


}
function CheckDiscount() {
    if ($('#txtDiscountCode').val() == '') { $('#txtDiscountCode').addClass("InvalidData"); } else {
        var post1 = 'DiscountCode=' + $('#txtDiscountCode').val();
        $.ajax({
            type: 'POST',
            url: '/weTest/CheckDiscount',
            data: post1,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {

                    if (data[i].dataType == 'Success') {
                        0
                        $('#txtDiscountCode').val('');
                        $('#dialogDiscount .ui-Warning-red').addClass('ui-hide');
                        $('#spnWarningDiscount').removeClass('ui-hide')
                        $('#PackagePrice').html(data[i].errorMsg);
                        popupClose($('#dialogDiscount'), 99999);

                    } else {
                        $('#dialogDiscount .ui-Warning-red').html(data[i].errorMsg);
                        $('#dialogDiscount .ui-Warning-red').removeClass('ui-hide');

                    }

                }
            }
        });
    }

}