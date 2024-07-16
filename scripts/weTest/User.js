var xobjClick, xobjAlert, OTPNum,selectedGoalDate,formatSelectedGoalDate;
// ========================= Page Load ======================== //
$(function () {
    $('div[data-role=page]').page({ theme: 'c', });
    $('#userName').focus();
});
CheckLoginStatus();

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

// ======================= MainMenu ======================= //
    .on('click', '#btnMockUpExam', function (e, data) {
  
        $('#dialogSelect').attr('action', 'focus');
        $('#dialogSelect .ui-text').html('Do you want to start exam for up level ?');
        popupOpen($('#dialogSelect'), 99999);
    })
    .on('click', '#btnPracticeMenu', function (e, data) {
         window.location = '/Wetest/Practice';
    })
//20240715 -- Menu Goal
    .on('click', '#btnGoalMenu', function (e, data) {
        $('.MainMenu').addClass('ui-hide');
        $('.Goal').removeClass('ui-hide');
    })
    .on('click', '#dialogSelect .btnSelected', function (e, data) {
        popupClose($(this).closest('.my-popup'));
        GotoExam();
    })
    .on('click', '#dialogConfirm #btnOK ,#dialogSelect .btnCancel', function (e, data) {
        popupClose($(this).closest('.my-popup'));
    })

// ======================= Goal ======================= //
//20240715 -- Set Total Goal
    .on('click', '#TimesUsedPercent', function (e, data) {
        Goaldate();
    })
//20240715 -- Close Dialog
    .on('click', '.ui-icon.close', function (e, data) {
     popupClose($(this).closest('.my-popup'));
    })
//20240715 -- Save Goal
    .on('click', '.btnSaveGoal', function (e, data) {
        SaveTotalGoal();
      
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
   
    $.ajax({
        type: 'POST',
        url: '/weTest/CheckUserLogin',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'success') {
                    $('.login').addClass('ui-hide');
                    $('.UserNameandLevel').html(data[i].Firstname + '<br />' + data[i].Firstname);

                    $('.UserData,.MainMenu').removeClass('ui-hide');
                } else {
                    $('#dialogAlert').attr('action', 'focus');
                    $('#dialogAlert .ui-text').html(data[i].Msg);
                    popupOpen($('#dialogAlert'), 99999);
                }

            }
        }
    });
}
function GotoExam() {
    $.ajax({
        type: 'POST',
        url: '/weTest/CreateMockUpExam',
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
function CheckLoginStatus() {
    $.ajax({
        type: 'POST',
        url: '/weTest/CheckLoginStatus',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'success') {
                    $('.login').addClass('ui-hide');
                    $('.UserNameandLevel').html(data[i].Firstname + '<br />' + data[i].Firstname);

                    $('.UserData,.MainMenu').removeClass('ui-hide');
                }

            }
        }
    });
}
//20240715 -- Set Datepicker And Open Dialog
function Goaldate() {

    var options = $.extend(global.datepickerOption, {
        minDate: 1,
        onSelect: function () {
            var selectedDate = $("#SelectGoalDate").datepicker("getDate");
            selectedGoalDate = selectedDate.getFullYear() + '-' + (selectedDate.getMonth() + 1) + '-' + selectedDate.getDate();
            formatSelectedGoalDate = selectedDate.getDate() + '/' + (selectedDate.getMonth() + 1) + '/' + selectedDate.getFullYear();
            console.log(selectedGoalDate);
            $('#spnShowdate').html('Your Goal Date : ' + formatSelectedGoalDate)
            $('.btnSaveGoal').removeClass('unActive');
        }
    });

    $("#SelectGoalDate").datepicker(options);

    $('#dialogGoalDate').attr('action', 'focus');
    popupOpen($('#dialogGoalDate'), 99999);
    
}
//20240715 -- Save Goal Date
function SaveTotalGoal() {
    var post1 = 'selectedGoalDate=' + selectedGoalDate + '&formatSelectedGoalDate=' + formatSelectedGoalDate;

    $.ajax({
        type: 'POST',
        url: '/weTest/SaveTotalGoal',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'success') {
                    $('#lastestGOAL').html(data[i].errorMsg);
                    popupClose($('#dialogGoalDate').closest('.my-popup'));
                } else {
                    $('#dialogAlert').attr('action', 'focus');
                    $('#dialogAlert .ui-text').html(data[i].errorMsg);
                    popupOpen($('#dialogAlert'), 99999);
                }

            }
        }
    });
}




