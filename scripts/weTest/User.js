var xobjClick, xobjAlert, OTPNum, selectedGoalDate, formatSelectedGoalDate, GoalType;
// ========================= Page Load ====================================================================== //
$(function () {
    $('div[data-role=page]').page({ theme: 'c', });
    $('#userName').focus();
    CheckLoginStatus();
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
    .on('click', '#dialogAlert #btnOK', function (e, data) {
        popupClose($(this).closest('.my-popup'));
    })
    .on('click', '.registerlink', function (e, data) {
        window.location = '/Wetest/Registration';
    })

// ======================= MainMenu =========================== //
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
        $('.Goal,.footerGoal').removeClass('ui-hide');
        $('.pagename').html('- Goal');
    })
//20240715 -- Menu Report
    .on('click', '#btnReport', function (e, data) {
        window.location = '/Wetest/Report';
    })
    .on('click', '#dialogSelect .btnSelected', function (e, data) {
        popupClose($(this).closest('.my-popup'));
        GotoExam();
    })
    .on('click', '#dialogConfirm #btnOK ,#dialogSelect .btnCancel,#dialogLogout .btnCancel,#dialogDeleteAccount .btnCancel', function (e, data) {
        popupClose($(this).closest('.my-popup'));
    })
//20240723 toggle User Menu
    .on('click', '.UserNameandLevel,.UserPhoto', function (e, data) {
        $('.UserMenu').toggleClass('ui-hide');
    })
//20240723 Logout 
    .on('click', '.btnAccountMenu.Logout', function (e, data) {
        $('#dialogLogout').attr('action', 'focus');
        popupOpen($('#dialogLogout'), 99999);
    })
//20240723 Confirm Logout
    .on('click', '.btnConfirmLogout', function (e, data) {
       $.ajax({
           type: 'POST',
           url: '/weTest/Logout',
           success: function (data) {
               for (var i = 0; i < data.length; i++) {
                   if (data[i].Result == 'success') {
                       popupClose($('#dialogLogout').closest('.my-popup'));
                       $('.UserData,.UserMenu,.MainMenu,.Goal,.DetailGoal,.footer,.UserPhoto').addClass('ui-hide');
                       $('.login').removeClass('ui-hide');
                       $('#userName,#userPass').val('');
                   }
               }
           }
       });
   })
//20240723 Delete Account
   .on('click', '.btnAccountMenu.DeleteAccount', function (e, data) {
        $('#dialogDeleteAccount').attr('action', 'focus');
        popupOpen($('#dialogDeleteAccount'), 99999);
    })
//20240723 Confirm Delete Account
   .on('click', '.btnConfirmDelete', function (e, data) {
       $.ajax({
           type: 'POST',
           url: '/weTest/DeleteAccount',
           success: function (data) {
               for (var i = 0; i < data.length; i++) {
                   if (data[i].Result == 'success') {
                       popupClose($('#dialogDeleteAccount').closest('.my-popup'));
                       $('.UserData,.UserMenu,.MainMenu,.Goal,.DetailGoal,.footer,.UserPhoto').addClass('ui-hide');
                       $('.login').removeClass('ui-hide');
                       $('#userName,#userPass').val('');
                   }
               }
           }
       });
   })
//20240723 Confirm Delete Account
   .on('click', '.btnAccountMenu.EditAccount', function (e, data) {
       $.ajax({
           type: 'POST',
           url: '/weTest/SetEditUserMode',
           success: function (data) {
               for (var i = 0; i < data.length; i++) {
                   if (data[i].dataType == 'success') {
                       window.location = '/Wetest/Registration';
                   }
               }
           }
       });
   })
// ========================= Goal ============================= //
//20240715 -- Set Total Goal
    .on('click', '#TimesUsedPercent', function (e, data) {
        GoalType = 'total'
        Goaldate();
    })
//20240715 -- Close Dialog
    .on('click', '.ui-icon.close,#btnNotClear', function (e, data) {
        popupClose($(this).closest('.my-popup'));
    })
//20240715 -- Save Goal
    .on('click', '#btnSaveGoal', function (e, data) {
        SaveGoal();
    })
//20240716 -- Go to set detail GOAL
    .on('click', '.btnSetDetailGoal', function (e, data) {
        $('.Goal,.btnSetDetailGoal').addClass('ui-hide');
        $('.DetailGoal').removeClass('ui-hide');
    })
//20240716 -- Back Button
    .on('click', '.btnBack', function (e, data) {
        if ($('.Goal').hasClass('ui-hide') == false) {
            $('.MainMenu').removeClass('ui-hide');
            $('.Goal,.footerGoal').addClass('ui-hide');
            $('.pagename').html('');
        } else if ($('.DetailGoal').hasClass('ui-hide') == false) {
            $('.Goal,.btnSetDetailGoal').removeClass('ui-hide');
            $('.DetailGoal').addClass('ui-hide');
        }
    })
//20240716 -- Clear Button
    .on('click', '.btnClear', function (e, data) {
        $('#dialogClearGoal').attr('action', 'focus');
        popupOpen($('#dialogClearGoal'), 99999);
    })
//20240716 -- Clear Goal Data
    .on('click', '#btnConfirmClear', function (e, data) {
        ClearGoalDate();
        popupClose($(this).closest('.my-popup'));
    })
//20240716 -- Set Reading Goal
    .on('click', '#ReadingTime', function (e, data) {
        GoalType = 'Reading'
        Goaldate();
    })
//20240716 -- Set Listening Goal
    .on('click', '#ListeningTime', function (e, data) {
        GoalType = 'Listening'
        Goaldate();
    })
//20240716 -- Set Vocab Goal
    .on('click', '#VocabTime', function (e, data) {
        GoalType = 'Vocabulary'
        Goaldate();
    })
//20240716 -- Set Grammar Goal
    .on('click', '#GrammarTime', function (e, data) {
        GoalType = 'Grammar'
        Goaldate();
    })
//20240716 -- Set Situation Goal
    .on('click', '#SituationTime', function (e, data) {
        GoalType = 'Situation'
        Goaldate();
    })

// ========================================================================================================== //

// ================================================ Function ================================================ //

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
            SetUserData(data);
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
            SetUserData(data);
        }
    });
}
//20240715 -- Set Datepicker And Open Dialog
function Goaldate() {

    $('.btnSaveGoal').addClass('unActive');

    if (GoalType == 'total') {
        $('#spnGoalName').html('Set goal from date');
    } else {
        $('#spnGoalName').html('Set goal for ' + GoalType);
    }

    var options = $.extend(global.datepickerOption, {
        minDate: 1,
        onSelect: function () {
            var selectedDate = $("#SelectGoalDate").datepicker("getDate");
            selectedGoalDate = selectedDate.getFullYear() + '-' + (selectedDate.getMonth() + 1) + '-' + selectedDate.getDate();
            formatSelectedGoalDate = selectedDate.getDate() + '/' + (selectedDate.getMonth() + 1) + '/' + selectedDate.getFullYear();

            $('#spnShowdate').html('Your Goal Date : ' + formatSelectedGoalDate)
            $('#btnSaveGoal').removeProp("goaltype");
            $('#btnSaveGoal').attr('goaltype', GoalType);
            $('#btnSaveGoal').removeClass('unActive');
        }
    });

    $("#SelectGoalDate").datepicker(options);

    $('#dialogGoalDate').attr('action', 'focus');
    popupOpen($('#dialogGoalDate'), 99999);

}
//20240715 -- Save Goal Date
function SaveGoal() {
    var post1 = 'selectedGoalDate=' + selectedGoalDate + '&formatSelectedGoalDate=' + formatSelectedGoalDate + '&GoalType=' + GoalType;

    $.ajax({
        type: 'POST',
        url: '/weTest/SaveTotalGoal',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'success') {
                    if (GoalType == 'total') {
                        $('#lastestGOAL').html('Your lastest GOAL : ' + data[i].TotalGoal + ' ( Time left ' + data[i].TotalGoalAmount + ' days )');
                        $('#lastestBigGOAL').html('Your lastest A BIG GOAL : ' + data[i].TotalGoal + ' ( Time left ' + data[i].TotalGoalAmount + ' days )');
                        $('.btnSetDetailGoal').removeClass('unActive');
                        $('.btnClear').removeClass('ui-hide');
                    } else {
                        $('#' + GoalType + 'TimeResult').html('Due date : ' + data[i].TotalGoal + '<br />(Time left ' + data[i].TotalGoalAmount + ' days)');
                        $('#' + GoalType + 'TimeResult').removeClass('ui-hide');
                        $('#' + GoalType + 'PS').removeClass('PS');

                    }

                    popupClose($('#dialogGoalDate').closest('.my-popup'));


                } else {
                    $('#dialogAlert').attr('action', 'focus');
                    $('#dialogAlert .ui-text').html(data[i].Msg);
                    popupOpen($('#dialogAlert'), 99999);
                }

            }
        }
    });
}
//20240716 -- Set User Data
function SetUserData(data) {
    for (var i = 0; i < data.length; i++) {
        if (data[i].Result == 'success') {
            $('.login').addClass('ui-hide');
            $('.UserData,.MainMenu').removeClass('ui-hide');
            $('.pagename').html('');
            //20240716 -- ดึงข้อมูล User เพิ่มเติม
            $('.UserNameandLevel').html('Welcome, ' + data[i].Firstname + '<br />' + data[i].UserLevel);
            $('.expiredDate').html(data[i].ExpiredDate)
            $('.UserData').append(data[i].UserPhoto);
            $('#UserLevel').html('Your Level : ' + data[i].UserLevel + '<br />');

            if (data[i].TotalGoal != '') {
                $('#lastestGOAL').html('Your lastest GOAL : ' + data[i].TotalGoal + ' ( Time left ' + data[i].TotalGoalAmount + ' days )');
                $('#lastestBigGOAL').html('Your lastest A BIG GOAL : ' + data[i].TotalGoal + ' ( Time left ' + data[i].TotalGoalAmount + ' days )');
                $('.btnSetDetailGoal').removeClass('unActive');
                $('.btnClear').removeClass('ui-hide');
            } else {
                $('.btnClear').addClass('ui-hide');
            }
            if (data[i].ReadingGoal != '') {
                $('#ReadingTimeResult').html('Due date : ' + data[i].ReadingGoal + '<br />(Time left ' + data[i].ReadingGoalAmount + ' days)');
                $('#ReadingTimeResult').removeClass('ui-hide');
                $('#ReadingPS').removeClass('PS');
            }
            if (data[i].ListeningGoal != '') {
                $('#ListeningTimeResult').html('Due date : ' + data[i].ListeningGoal + '<br />(Time left ' + data[i].ListeningGoalAmount + ' days)');
                $('#ListeningTimeResult').removeClass('ui-hide');
                $('#ListeningPS').removeClass('PS');
            }
            if (data[i].VocabGoal != '') {
                $('#VocabularyTimeResult').html('Due date : ' + data[i].VocabGoal + '<br />(Time left ' + data[i].VocabGoalAmount + ' days)');
                $('#VocabularyTimeResult').removeClass('ui-hide');
                $('#VocabularyPS').removeClass('PS');
            }
            if (data[i].GrammarGoal != '') {
                $('#GrammarTimeResult').html('Due date : ' + data[i].GrammarGoal + '<br />(Time left ' + data[i].GrammarGoalAmount + ' days)');
                $('#GrammarTimeResult').removeClass('ui-hide');
                $('#GrammarPS').removeClass('PS');
            }
            if (data[i].SituationGoal != '') {
                $('#SituationTimeResult').html('Due date : ' + data[i].SituationGoal + '<br />(Time left ' + data[i].SituationGoalAmount + ' days)');
                $('#SituationTimeResult').removeClass('ui-hide');
                $('#SituationPS').removeClass('PS');
            }

            $('#TimesUsedPercent').html(data[i].TotalGoalDatePercent);
            $('#PracticeScorePercent').html(data[i].TotalGoalScorePercent);

            if (data[i].GrammarScorePercent != '') {
                $('#GrammarPS').html(data[i].GrammarScorePercent);
            }

            if (data[i].VocabScorePercent != '') {
                $('#VocabularyPS').html(data[i].VocabScorePercent);
            }

        } else if (data[i].Result == 'sessionlost') {
            if ($('.login').hasClass('ui-hide') == true) {
                window.location = '/Wetest/User';
            }
        } else {
            $('#dialogAlert').attr('action', 'focus');
            $('#dialogAlert .ui-text').html(data[i].Msg);
            popupOpen($('#dialogAlert'), 99999);
        }
    }
}
//20240716 -- Clear Goal Date
function ClearGoalDate() {
    $.ajax({
        type: 'POST',
        url: '/weTest/ClearGoalDate',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'success') {
                    $('#lastestGOAL,#lastestBigGOAL,#ReadingTimeResult,#ListeningTimeResult,#VocabularyTimeResult,#GrammarTimeResult,#SituationTimeResult').html('');
                    $('.btnClear,#ReadingTimeResult,#ListeningTimeResult,#VocabularyTimeResult,#GrammarTimeResult,#SituationTimeResult,.DetailGoal').addClass('ui-hide');
                    $('#ReadingPS,#ListeningPS,#VocabularyPS,#GrammarPS,#SituationPS').addClass('PS');
                    $('.Goal,.btnSetDetailGoal').removeClass('ui-hide')
                    $('.btnSetDetailGoal').addClass('unActive');
                    $('#TimesUsedPercent,#PracticeScorePercent').html('0%');
                }
            }
        }
    });
}




