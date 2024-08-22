var OTPNum, PackagePrice, StudentType, file, EditMode;

// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); $('#Firstname').focus(); });
checkEditMode();
checkRefillKey();
// ============================================================ //
// ======================= Object Event ======================= //
$(document)
// ======================= Register ======================= //
    .on('focus', '#Firstname, #Surname, #MobileNo, #EMail, #Username, #Password, #ConfirmPassword', function (e, data) { $(this).removeClass("InvalidData") })
    .on('keypress blur', '#Firstname, #Surname, #MobileNo, #EMail, #Username, #Password', function (e, data) {
        var xkey = keyA(e);
        if (xkey == 13) { keyEnterNextItem(e); }
    })
    .on('keypress blur', '#ConfirmPassword', function (e, data) {
        var xkey = keyA(e);
        if (xkey == 13) {
            if (checkInvalidRegisterData() == 'true') {
                SetConfirmtxt()
                $('#captionConfirmPassword,#captionPassword,.txtData').addClass('ui-hide');
                $('.spnData').removeClass('ui-hide');
            };
        }
    })
    .on('click', '.footerRegister .btnNext', function (e, data) {
        if ($('.spnData').hasClass('ui-hide')) {
            if (checkInvalidRegisterData() == 'true') {
                SetConfirmtxt()
                $('#captionConfirmPassword, #captionPassword,.txtData,#btnStudent, #btnOther').addClass('ui-hide');
                $('.spnData,#capType').removeClass('ui-hide');
            };
        } else {
            if (EditMode) { SaveEditUser(); } else { SaveNewUser(); }
        }
    })
    .on('click', '.footerRegister .btnBack', function (e, data) {
        if ($('.spnData').hasClass('ui-hide')) {
            window.location = '/Wetest/User';
        } else {
            $('#captionConfirmPassword, #captionPassword,.txtData,#btnStudent, #btnOther').removeClass('ui-hide');
            $('.spnData').addClass('ui-hide');
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
    .on('change', '#fileSlip', function (e, data) {
        var _URL = window.URL || window.webkitURL;
        var image;
        if ((file = this.files[0])) {
            image = new Image();
            image.onload = function () {
                src = this.src;
                var filesUpload = $("#fileSlip").get(0).files;
                $('#SlipName').val(filesUpload[0].name);
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
            var OSId = $(this).attr('OSId')
            CheckAndUpdateOTPStatus(ttb, OSId)
        }
    })
    .on('click', '.footerOTP .btnBack', function (e, data) {
        EditMode = true;
        $('.register').removeClass('ui-hide');
        $('#captionConfirmPassword, #captionPassword,.txtData,#btnStudent, #btnOther,.footerRegister').removeClass('ui-hide');
        $('.spnData,.otp,.footerOTP').addClass('ui-hide');
    })
// ======================= Payment ======================= //
    .on('focus keypress', '#txtDiscountCode', function (e, data) {
        $(this).removeClass("InvalidData");
        $('#dialogDiscount .ui-Warning-red').addClass('ui-hide');
    })
    .on('click', '#btnCheckKey', function (e, data) {
        //CheckKeycode();
        //popupOpen($('#dialogDiscount'), 99999);
        CheckDiscount();
    })
    .on('click', '#btnPayment', function (e, data) {
        $(this).addClass("ui-hide");
        $('#spnWarning').text('You can use Mobile Banking for scan to pay.');
        $('.btnQR').removeClass("ui-hide");
    })
    .on('click', '#btnDiscount', function (e, data) {
        popupOpen($('#dialogDiscount'), 99999);
    })
    //20240716 -- skip payment
    //20240723 -- Update UpdateTrialDate skip
    .on('click', '#btnskip', function (e, data) {
        UpdateTrialDate();
    })
    .on('click', '#dialogConfirm #btnOK ,#dialogSelect .btnCancel,#btnOKOTP,.btnAcceptPolicy.btnActive', function (e, data) {
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
    //20240723 -- Dialog Confirm SaveUser
    .on('click', '#dialogConfirmSaveUser #btnComplete', function (e, data) {
        popupClose($(this).closest('.my-popup'));
        window.location = '/Wetest/User';
    })
    //20240716 -- Browse Slip file
    .on('click', '.btnSlipPhoto', function (e, data) {
        event.preventDefault();
        $("#fileSlip").click();
    })
    //20240730 -- cancelPolicy
    .on('click', '.btncancelPolicy', function (e, data) {
        window.location = '/Wetest/User';
    })
    .on('click', '.btnAcceptPolicy.btnUnActive', function (e, data) {
        return 0;
    })
    .on('change', '#chkAccept', function (e, data) {
        if ($('.btnAcceptPolicy').hasClass('btnUnActive')) {
            $('.btnAcceptPolicy').removeClass('btnUnActive');
            $('.btnAcceptPolicy').addClass('btnActive');
        } else {
            $('.btnAcceptPolicy').removeClass('btnActive');
            $('.btnAcceptPolicy').addClass('btnUnActive');
        }
    })
    //20240731 -- เลือก Package
    .on('click', '.btnChoosePackage', function (e, data) {
        var PPrice = $(this).attr('price');
        $('#PackagePrice').html(PPrice);
        $('.package,.footerRegister').addClass("ui-hide");
        $('.payment,.footerPayment').removeClass("ui-hide");
    })
    .on('click', '#btnConfirmPayment', function (e, data) {
        var PPrice = $(this).attr('price');
        $('#PackagePrice').html(PPrice);
        //$('.package,.footerRegister').addClass("ui-hide");
        //$('.payment,.footerPayment').removeClass("ui-hide");
        console.log($('#SlipName').val())
        if ($('#SlipName').val() == '') { $('#SlipName').addClass("InvalidData"); } else {
            UploadSlip();

        }

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
    if (EditMode == false) {
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
    }

    return CheckError
}
function SetConfirmtxt() {
    console.log($('#Firstname').val());
    $('.spFirstname').html($('#Firstname').val());
    $('.spSurname').text($('#Surname').val());
    $('.spMobileNo').text($('#MobileNo').val());
    $('.spEMail').text($('#EMail').val());
    $('.spUsername').text($('#Username').val());
}
function SaveNewUser() {
    var post1 = 'FirstName=' + $('.spFirstname').text() + '&Surname=' + $('.spSurname').text() + '&Fullname=' + $('.spFirstname').text() + $('.spSurname').text() + '&MobileNo=' + $('.spMobileNo').text() +
        '&EMail=' + $('.spEMail').text() + '&Username=' + $('.spUsername').text() + '&Password=' + $('#Password').val() + '&StudentType=' + StudentType;
    $.ajax({
        type: 'POST',
        url: '/weTest/SaveUser',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'pass') {
                    UploadStudentPhoto();
                } else {
                    $('#dialogConfirm').attr('action', 'focus');
                    $('#dialogConfirm .ui-text').html(data[i].errorMsg);
                    popupOpen($('#dialogConfirm'), 99999);
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
                            break;
                        case 'nototp':
                            $('.otp,.footerOTP').removeClass('ui-hide');
                            $('.register,.footerRegister').addClass('ui-hide');
                            sendOTP();
                            break;
                        case 'success':
                            window.location = '/Wetest/User';
                            break;
                    }
                }
            }
        });
    } else {
        if (EditMode) {
            window.location = '/Wetest/User';
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
                                break;
                            case 'nototp':
                                $('.otp,.footerOTP').removeClass('ui-hide');
                                $('.register,.footerRegister').addClass('ui-hide');
                                sendOTP();
                                break;
                            case 'success':
                                window.location = '/Wetest/User';
                                break;
                        }
                    }
                }
            });
        }
    }
}
//20240716 -- Upload Slip fie
function UploadSlip() {

    var data = new FormData();

    var files = $("#fileSlip").get(0).files;

    if (files.length > 0) {
        data.append("UploadedImage", files[0]);
        $.ajax({
            type: 'POST',
            url: '/weTest/UploadSlipFile',
            contentType: false,
            processData: false,
            data: data,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    switch (data[i].dataType) {
                        case 'error':
                            console.log(data[i].errorMsg);
                            break;
                        case 'success':
                            UpdateWaitApproveSlip()
                            
                    }
                }
            }
        });
    }
}
//20240723 -- ปรับวิธีการส่ง OTP
function sendOTP() {
    OTPNum = Math.floor(100000 + Math.random() * 900000);

    var post1 = 'MobileNo=' + $('.spMobileNo').text() + '&OTPNum=' + OTPNum;
    $.ajax({
        type: 'POST',
        url: '/weTest/sendOTP',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                switch (data[i].ResultStatus) {
                    case 'error':
                        console.log(data[i].Resulttxt);
                        break;
                    case 'over':
                        console.log(data[i].Resulttxt);
                        $('#dialogConfirmOTP').attr('action', 'focus');
                        $('#dialogConfirmOTP .ui-text').html(data[i].Resulttxt);
                        popupOpen($('#dialogConfirmOTP'), 99999);
                        break;
                    case 'success':
                        $('#btnConfirm').attr('OSId', data[i].OSId);
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
                        }, data[i].ReponseTime);
                        break;
                }
            }
        }
    });
}
//20240723 -- ปรับวิธีการตรวจสอบ OTP และเพิ่มการบันทึกการตอบกลับ OTP
//20240731 -- ตรวจสอบ OTP ผ่านแล้วให้ไปทำ Placement Test
function CheckAndUpdateOTPStatus(OTPNum, OSId) {
    var post1 = 'OTPNum=' + OTPNum + '&OSId=' + OSId
    $.ajax({
        type: 'POST',
        url: '/weTest/CheckAndUpdateOTPStatus',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                switch (data[i].ResultStatus) {
                    case 'error':
                        console.log(data[i].Resulttxt);
                        break;
                    case 'success':
                        //$('.otp ,.footerOTP').addClass('ui-hide');
                        //$('.payment ,.footerPayment').removeClass('ui-hide');
                        //GetPackagePrice();
                        //break;
                        $('#dialogSelect').attr('action', 'focus');
                        $('#dialogSelect .ui-text').html('Register Done!<br /> Do you want go to Placement Test now ?.');
                        popupOpen($('#dialogSelect'), 99999);
                        break;
                    default:
                        $('#dialogConfirm').attr('action', 'focus');
                        $('#dialogConfirm .ui-text').html(data[i].Resulttxt);
                        popupOpen($('#dialogConfirm'), 99999);
                        break;
                }
            }
        }
    });
}
function GetPackagePrice() {
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
//20240820 -- เพิ่มการเก็บ DiscountId เพื่อทการบันทึกลงใน tblN                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
function CheckDiscount() {
    if ($('#txtKeyCode').val() == '') { $('#txtKeyCode').addClass("InvalidData"); } else {
        var post1 = 'DiscountCode=' + $('#txtKeyCode').val();
        $.ajax({
            type: 'POST',
            url: '/weTest/CheckDiscount',
            data: post1,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {

                    if (data[i].Result == 'Success') {
                        $('#dialogDiscount .ui-Warning-red').addClass('ui-hide');
                        $('#spnWarningDiscount').removeClass('ui-hide')
                        $('#PackagePrice').html(data[i].ResultTxt);
                        $('#btnConfirmPayment').attr('DiscountId', data[i].DiscountId)
                    } else {
                        $('#dialogConfirm').attr('action', 'focus');
                        $('#dialogConfirm .ui-text').html(data[i].ResultTxt);
                        popupOpen($('#dialogConfirm'), 99999);
                    }

                }
            }
        });
    }

}
//20240723 -- Update ExpiredDate case กด skip
//20240731 -- Update แล้วให้ไปหน้าเมนูเลย
function UpdateTrialDate() {
    $.ajax({
        type: 'POST',
        url: '/weTest/UpdateTrialDate',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {

                if (data[i].dataType == 'success') {
                    window.location = '/Wetest/User';
                } else {
                    $('#dialogDiscount .ui-Warning-red').html(data[i].errorMsg);
                    $('#dialogDiscount .ui-Warning-red').removeClass('ui-hide');
                }

            }
        }
    });
}
//20240806 -- Update วันที่รอ Approvee Slip
function UpdateWaitApproveSlip() {
    var DiscountId = $('#btnConfirmPayment').attr('DiscountId')
    console.log(DiscountId);
    var post1 = 'DiscountCode=' + DiscountId + '&PackageId=1702F1EF-8FD5-443A-A68D-4599BC9F9E54';

    $.ajax({
        type: 'POST',
        url: '/weTest/UpdateWaitApproveSlip',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {

                if (data[i].dataType == 'success') {
                    window.location = '/Wetest/User';
                } else {
                    $('#dialogDiscount .ui-Warning-red').html(data[i].errorMsg);
                    $('#dialogDiscount .ui-Warning-red').removeClass('ui-hide');
                }

            }
        }
    });
}
//20240723 -- Check EditMode And Get User Data
//20240814 -- ดึงรูป User มาแสดงเมื่อเข้าเมนูแก้ไข
function checkEditMode() {
    $.ajax({
        type: 'POST',
        url: '/weTest/checkEditMode',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'edit') {
                    $('#Firstname').val(data[i].Firstname);
                    $('#Surname').val(data[i].Surname);
                    $('#MobileNo').val(data[i].MobileNo);
                    $('#EMail').val(data[i].Email);
                    $('#Username').val(data[i].Username);
                    $('#btnStudent, #btnOther').addClass('ui-hide');
                    var PhotoPath = 'url("/WetestPhoto/UserPhoto/' + data[i].StudentId + '.png")'
                    $('.btnPhoto').css('background', PhotoPath);
                    EditMode = true;
                } else if (data[i].Result == 'add') {
                    EditMode = false;
                    OpenPolicy();
                } else if (data[i].Result == 'purchess') {
                    EditMode = false;
                    $('.package').removeClass('ui-hide');
                    $('.register').addClass('ui-hide');
                }
            }
        }
    });
}
//20240723 -- SaveEditUser
function SaveEditUser() {
    var post1 = 'FirstName=' + $('.spFirstname').text() + '&Surname=' + $('.spSurname').text() + '&MobileNo=' + $('.spMobileNo').text() +
      '&EMail=' + $('.spEMail').text() + '&Username=' + $('.spUsername').text() + '&Password=' + $('#Password').val();
    $.ajax({
        type: 'POST',
        url: '/weTest/UpdateUser',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                switch (data[i].dataType) {
                    case 'error':
                        console.log(data[i].errorMsg);
                        break;
                    case 'success':
                        UploadStudentPhoto()
                        break;
                }
            }
        }
    });
}
//20240730 -- Open Policy Dialog
function OpenPolicy() {
    console.log('OpenPolicy');
    $('#dialogPolicy').attr('action', 'focus');
    popupOpen($('#dialogPolicy'), 99999);
}
//20240816 -- ตรวจสอบว่ากดมาจาก Refill Key ใช่มั้ย
function checkRefillKey() {
    $.ajax({
        type: 'POST',
        url: '/weTest/checkRefillKey',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'refillkey') {
                    $('.package').removeClass('ui-hide');
                    $('.register,.footerRegister .btnNext').addClass('ui-hide');
                }
            }
        }
    });
}




