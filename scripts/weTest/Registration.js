var xobjClick, xobjAlert, OTPNum;

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
            $('.otp').removeClass('ui-hide');
            $('.register').addClass('ui-hide');
            $('.footerRegister').addClass('ui-hide');
            sendOTP();
        }else{
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
    .on('click', '#btnSendAgain', function (e, data) {
        $('#btnSendAgain').addClass('btnUnActive');
        sendOTP()
    })
    .on('click', '#btnConfirm', function (e, data) {
        var ttb = $('#txtOTP').val();
        console.log(ttb);
        console.log(OTPNum);
        if (ttb == OTPNum) {
            $('.otp ,.footerOTP').addClass('ui-hide');
            $('.payment ,.footerPayment').removeClass('ui-hide');
        } else {
            console.log('otp not ok');
        }
    })

// ======================= Payment ======================= //
    .on('click', '#btnCheckKey', function (e, data) {
        $('.my-popup.alert').attr('action', 'focus');
        $('.my-popup.alert .ui-text').html('Please wait...<br><br>We are taking you to Placement Test.');
        popupOpen($('.my-popup.alert'), 99999);

        var x = setInterval(function () {
                clearInterval(x);
                window.location = '/Wetest/Activity';
        }, 3000);
    })
    .on('click', '#btnPayment', function (e, data) {
        $(this).addClass("ui-hide");
        $('#spnPleaseWarning').text('You can use Mobile Banking for scan to pay.');
        $('.btnQR').removeClass("ui-hide");
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

    if ($('#btnStudent, #btnOther').hasClass("btnSelected")) {} else {
        $('#btnStudent, #btnOther').addClass("InvalidData");
        CheckError = 'false';
    }

    return CheckError
}
function sendOTP() {
    //$('#MobileNo').val('08833449955');
    //var post1 = 'MobileNo=' + $('#MobileNo').val();
    OTPNum = Math.floor(100000 + Math.random() * 900000);
    console.log(OTPNum);
    var post1 = 'MobileNo=' + '0834955364' + '&OTPNum=' + OTPNum;
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
                        console.log(data[i].errorMsg);
                        var startTime = new Date().getTime()
                        var countDownDate = new Date();
                        countDownDate.setSeconds(countDownDate.getSeconds() + 10);

                        var x = setInterval(function () {
                            var now = new Date().getTime();
                            var distance = countDownDate - now;
                           // document.getElementById("demo").innerHTML = Math.floor((distance % (1000 * 60)) / 1000) + "s ";
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

