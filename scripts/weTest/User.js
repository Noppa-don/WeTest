var xobjClick, xobjAlert, OTPNum, selectedGoalDate, formatSelectedGoalDate, GoalType, SkillId;
var totalGoalAmount;
var ExpiredDateAmount;
// ========================= Page Load ====================================================================== //
$(function () {
    $('div[data-role=page]').page({ theme: 'c', });
    //CheckLoginStatus();
 
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
 //20240927 -- เมื่อไม่ทำ Quiz ต่อให้ Update EndTime เป็นเวลาที่ปิด Dialog นี้ด้วย
    .on('click', '#dialogBackToQuiz .btnNo', function (e, data) {
        UpdateEndQuiz();
        popupClose($(this).closest('.my-popup'));
    })
    .on('click', '#dialogBackToQuiz .btnContinue', function (e, data) {
        window.location = '/Wetest/Activity';
    })
// ======================= MainMenu =========================== //
//20240902 -- Check Expired Date
    .on('click', '#btnMockUpExam', function (e, data) {
        if ($('#btnMockUpExam').hasClass('expired') == true) {
            $('#dialogMustPurchase').attr('action', 'focus');
            popupOpen($('#dialogMustPurchase'), 99999);
        } else if ($('#btnMockUpExam').hasClass('reject') == true) {
            $('#dialogRejectAlert').attr('action', 'focus');
            popupOpen($('#dialogRejectAlert'), 99999);
        } else {
            CheckExamAgain();
        }
    })
//20240902 -- Check Expired Date
    .on('click', '#btnPracticeMenu', function (e, data) {
        if ($('#btnPracticeMenu').hasClass('expired') == true) {
            $('#dialogMustPurchase').attr('action', 'focus');
            popupOpen($('#dialogMustPurchase'), 99999);
        } else if ($('#btnPracticeMenu').hasClass('reject') == true) {
            $('#dialogRejectAlert').attr('action', 'focus');
            popupOpen($('#dialogRejectAlert'), 99999);
        } else {
            window.location = '/Wetest/Practice';
        }
    })
//20240715 -- Menu Goal
//20240828 -- แยกดึงข้อมูล Goal ออกจากการดึงข้อมูล User เพื่อให้ทำงานรวดเร็วขึ้น
//20240902 -- Check Expired Date
//20240916 -- แก้ปัญหา Log-in เข้าเมนู Goal ครั้งแรกไม่แสดงปุ่ม Back
    .on('click', '#btnGoalMenu', function (e, data) {
        if ($('#btnGoalMenu').hasClass('expired') == true) {
            $('#dialogMustPurchase').attr('action', 'focus');
            popupOpen($('#dialogMustPurchase'), 99999);
        } else if ($('#btnGoalMenu').hasClass('reject') == true) {
            $('#dialogRejectAlert').attr('action', 'focus');
            popupOpen($('#dialogRejectAlert'), 99999);
        } else {
            $('.MainMenu,.Assignment').addClass('ui-hide');
            $('.Goal,.footer,.footerGoal').removeClass('ui-hide');
            $('.pagename').html('- Goal');
            GetGoalData();
            GetSkill();
        }
    })
//20240715 -- Menu Report
//20240902 -- Check Expired Date
    .on('click', '#btnReport', function (e, data) {
        if ($('#btnReport').hasClass('expired') == true) {
            $('#dialogMustPurchase').attr('action', 'focus');
            popupOpen($('#dialogMustPurchase'), 99999);
        } else if ($('#btnReport').hasClass('reject') == true) {
            $('#dialogRejectAlert').attr('action', 'focus');
            popupOpen($('#dialogRejectAlert'), 99999);
        } else {
            window.location = '/Wetest/Report';
        }
    })
    .on('click', '#dialogSelect .btnSelected', function (e, data) {
        popupClose($(this).closest('.my-popup'));
        GotoExam();
    })
    .on('click', '#dialogConfirm #btnOK ,#dialogSelect .btnCancel,#dialogLogout .btnCancel,#dialogDeleteAccount .btnCancel,#dialogRejectAlert #btnOKReject,.btnClose', function (e, data) {
        popupClose($(this).closest('.my-popup'));
    })
//20240731 -- ไปหน้าจอแพกเกจ
    .on('click', '.btnbuy, .btnGotoPackage', function (e, data) {
        window.location = '/Wetest/Registration';
    })
//20240731 -- ไปหน้าจอแพกเกจ
    .on('click', '.btnlaterPurchase', function (e, data) {
        UpdateTrialDate();
    })
//20240813 -- ไปหน้าจอ Assignment
//20240902 -- Check Expired Date
    .on('click', '.Assignment', function (e, data) {
        if ($('.Assignment').hasClass('expired') == true) {
            $('#dialogMustPurchase').attr('action', 'focus');
            popupOpen($('#dialogMustPurchase'), 99999);
        } else if ($('.Assignment').hasClass('reject') == true) {
            $('#dialogRejectAlert').attr('action', 'focus');
            popupOpen($('#dialogRejectAlert'), 99999);
        } else {
            window.location = '/Wetest/Assignment';
        }
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
    .on('click', '#btnSaveGoal,#btnSaveSkillGoal', function (e, data) {
        SaveGoal();
    })
//20240716 -- Go to set detail GOAL
    .on('click', '.btnSetDetailGoal', function (e, data) {
        if ($('.btnSetDetailGoal').hasClass('unActive')) {
            return 0;
        } else {
            $('.Goal').addClass('ui-hide');
            //$('.btnSetDetailGoal').addClass('unActive');
            $('.btnSetDetailGoal').addClass('ui-hide');
            $('.DetailGoal').removeClass('ui-hide');
        }

    })
//20240716 -- Back Button
    .on('click', '.btnBack', function (e, data) {
        if ($('.Goal').hasClass('ui-hide') == false) {
            $('.MainMenu').removeClass('ui-hide');
            $('.Goal,.footerGoal').addClass('ui-hide');
            $('.pagename').html('');
        } else if ($('.Noti').hasClass('ui-hide') == false) {
            $('.MainMenu').removeClass('ui-hide');
            $('.Noti,.footerSetting').addClass('ui-hide');
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
        GoalType = 'Reading';
        SkillId = 'FB4B4A71-B777-4164-BA4D-5C1EA9522226';
        SkillGoaldate();
    })
//20240716 -- Set Listening Goal
    .on('click', '#ListeningTime', function (e, data) {
        GoalType = 'Listening';
        SkillId = '44502C7F-D3BE-4D46-9134-3FE40DA230E9';
        SkillGoaldate();
    })
//20240716 -- Set Vocab Goal
    .on('click', '#VocabTime', function (e, data) {
        GoalType = 'Vocabulary';
        SkillId = '31667BAB-89FF-43B3-806F-174774C8DFBF';
        SkillGoaldate();
    })
//20240716 -- Set Grammar Goal
    .on('click', '#GrammarTime', function (e, data) {
        GoalType = 'Grammar';
        SkillId = '5BBD801D-610F-40EB-89CB-5957D05C4A0B';
        SkillGoaldate();
    })
//20240716 -- Set Situation Goal
    .on('click', '#SituationTime', function (e, data) {
        GoalType = 'Situation';
        SkillId = 'F6E25F3E-192B-4081-B141-74720066FB74';
        SkillGoaldate();
    })
    .on('click', 'a.toggler', function (e, data) {
        $(this).toggleClass('off');
    })
//20240716 -- Check Noti
    .on('click', '.btnSetting', function (e, data) {
        var IsCheck = $(this).is(":checked");
        var NotiId = $(this).attr('id');
        UpdateNoti(IsCheck, NotiId);
    })
// ========================================================================================================== //

// ================================================ Function ================================================ //

function checkInvalidLoginData() {

    var CheckError = 'true';

    if ($('#userName').val() == '') { $('#userName').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#userPass').val() == '') { $('#userPass').addClass("InvalidData"); CheckError = 'false'; }

    return CheckError
}
//20240904 -- Check GOAL Noti
function CheckUserLogin() {
    var post1 = 'Username=' + $('#userName').val() + '&Password=' + $('#userPass').val();

    $.ajax({
        type: 'POST',
        url: '/weTest/CheckUserLogin',
        data: post1,
        success: function (data) {
            if (data[0].Result == 'not') {
                $('#dialogAlert').attr('action', 'focus');
                $('#dialogAlert .ui-text').html(data[0].Msg);
                popupOpen($('#dialogAlert'), 99999);
            } else {
                SetUserData(data);
                CheckNotEndQuiz();
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
            SetUserData(data);
        }
    });
}
//20240715 -- Set Datepicker And Open Dialog
//20240813 -- Set Max Date
function Goaldate() {
    $('#btnSaveGoal').addClass('unActive');
    var MaxDate
    $('#spnGoalName').html('Set goal from date');
    MaxDate = ExpiredDateAmount
    var options = $.extend(global.datepickerOption, {
        minDate: 1,
        maxDate: '"+' + MaxDate + 'd"',
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
//20240813 -- เพิ่ม Function สำหรับสร้าง calendar ของแต่ละ Skill
function SkillGoaldate() {
    $('#btnSaveSkillGoal').addClass('unActive');
    var MaxDate
    $('#spnGoalName').html('Set goal for ' + GoalType);
    MaxDate = totalGoalAmount
    var options = $.extend(global.datepickerOption, {
        minDate: 1,
        maxDate: '"+' + MaxDate + 'd"',
        onSelect: function () {
            var selectedDate = $("#SelectSkillGoalDate").datepicker("getDate");
            selectedGoalDate = selectedDate.getFullYear() + '-' + (selectedDate.getMonth() + 1) + '-' + selectedDate.getDate();
            formatSelectedGoalDate = selectedDate.getDate() + '/' + (selectedDate.getMonth() + 1) + '/' + selectedDate.getFullYear();

            $('#spnShowSkilldate').html('Your Goal Date : ' + formatSelectedGoalDate)
            $('#btnSaveSkillGoal').removeProp("goaltype");
            $('#btnSaveSkillGoal').attr('goaltype', GoalType);
            $('#btnSaveSkillGoal').removeClass('unActive');
        }
    });

    $("#SelectSkillGoalDate").datepicker(options);

    $('#dialogSkillGoalDate').attr('action', 'focus');
    popupOpen($('#dialogSkillGoalDate'), 99999);

}
//20240715 -- Save Goal Date
function SaveGoal() {
    var post1 = 'selectedGoalDate=' + selectedGoalDate + '&formatSelectedGoalDate=' + formatSelectedGoalDate + '&GoalType=' + GoalType + '&SkillId=' + SkillId;

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
                        $('.btnClear,.btnSetDetailGoal').removeClass('ui-hide');
                    } else {
                        $('#' + GoalType + 'TimeResult').html('Due date : ' + data[i].TotalGoal + '<br />(Time left ' + data[i].TotalGoalAmount + ' days)');
                        $('#' + GoalType + 'TimeResult').removeClass('ui-hide');
                        $('#' + GoalType + 'PS').removeClass('PS');

                    }
                    if (GoalType == 'total') { totalGoalAmount = data[i].TotalGoalAmount }

                    popupClose($('#dialogGoalDate').closest('.my-popup'));
                    popupClose($('#dialogSkillGoalDate').closest('.my-popup'));
                    totalGoalAmount = data[i].TotalGoalAmount
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
//20240716 -- ปรับการแสดงผล Total Goal case เลยวันที่ที่ตั้งค่าไว้
//20240902 -- Check Expired Date
//20240918 -- ปรับการตรวจสอบสถานะต่างๆ ของ User และแสดง Dialog ที่ถูกต้อง
function SetUserData(data) {
    for (var i = 0; i < data.length; i++) {
        if (data[i].Result == 'sessionlost') {
            //ไม่มี Session StudentId 
            $('.login').removeClass('ui-hide');
            $('.MainMenu,.Goal,.Noti,.DetailGoal,.Assignment,.UserData').addClass('ui-hide');
        } else if (data[i].Result == 'not') {
            //ไม่เจอใน DB
            $('.login').removeClass('ui-hide');
            $('.MainMenu,.Goal,.Noti,.DetailGoal,.Assignment,.UserData').addClass('ui-hide');
        } else {
            $('.login').addClass('ui-hide');
            $('.UserData,.MainMenu').removeClass('ui-hide');
            $('.pagename').html('');
            //20240716 -- ดึงข้อมูล User เพิ่มเติม
            $('.UserNameandLevel').html('Welcome, ' + data[i].Firstname + '<br />' + data[i].UserLevel);
            $('.expiredDate').html(data[i].ExpiredDate)
            ExpiredDateAmount = data[i].ExpiredDateAmount;
            $('.UserData').append(data[i].UserPhoto);
            $('#UserLevel').html('Your Level : ' + data[i].UserLevel + '<br />');
            CheckGoalNoti();
            console.log(data[i].Result);
            if (data[i].Result == 'refill') {
                //หมดอายุ
                $('#btnGoalMenu ,#btnPracticeMenu,#btnMockUpExam,#btnReport,.Assignment').addClass('expired');
                $('#dialogMustPurchase').attr('action', 'focus');
                popupOpen($('#dialogMustPurchase'), 99999);
            } else if (data[i].Result == 'trial') {
                //ทำ Placement Test เสร็จ กลับมาใช้งาน
                $('#dialogPurchase').attr('action', 'focus');
                popupOpen($('#dialogPurchase'), 99999);
            }
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
//20240801 -- Check Exam Again Day
function CheckExamAgain() {
    $.ajax({
        type: 'POST',
        url: '/weTest/CheckExamAgain',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'ok') {
                    $('#dialogSelect').attr('action', 'focus');
                    $('#dialogSelect .ui-text').html(data[i].errorMsg);
                    popupOpen($('#dialogSelect'), 99999);
                } else {
                    $('#dialogAlert').attr('action', 'focus');
                    $('#dialogAlert .ui-text').html(data[i].errorMsg);
                    popupOpen($('#dialogAlert'), 99999);

                }

            }
        }
    });
}
//20240806 -- Update trial date เมื่อกด Later
function UpdateTrialDate() {
    $.ajax({
        type: 'POST',
        url: '/weTest/UpdateTrialDate',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {

                if (data[i].dataType == 'success') {
                    window.location = '/Wetest/User';
                }

            }
        }
    });
}
//20240828 -- แยก Function ดึงข้อมูล Goal ออกจากการดึงข้อมูล User เพื่อให้ทำงานรวดเร็วขึ้น
function GetGoalData() {

    $.ajax({
        type: 'POST',
        url: '/weTest/GetGoalData',
        success: function (data) {
            SetGoal(data);
        }
    });
}
//20240828 -- Bind Goal Data ตามจุดต่างๆ
//20240905 -- ให้แสดงค่าเป็น 0% เมื่อไม่ได้ทำการตั้ง Goal
function SetGoal(data) {
    for (var i = 0; i < data.length; i++) {
        if (data[i].Result == 'ok') {
            if (data[i].TotalGoal != '') {
                $('#lastestGOAL').html('Your lastest GOAL : ' + data[i].TotalGoal + ' ( Time left ' + data[i].TotalGoalAmount + ' days )');
                $('#lastestBigGOAL').html('Your lastest A BIG GOAL : ' + data[i].TotalGoal + ' ( Time left ' + data[i].TotalGoalAmount + ' days )');
                $('.btnSetDetailGoal').removeClass('unActive');
                totalGoalAmount = data[i].TotalGoalAmount
                $('#TimesUsedPercent').html(data[i].TotalDatePercent);
                $('#PracticeScorePercent').html(data[i].TotalScorePercent);
                $('.btnClear,.btnSetDetailGoal').removeClass('ui-hide');
            } else {
                $('.btnClear').addClass('ui-hide');
                $('.btnSetDetailGoal').removeClass('ui-hide');
                $('.btnSetDetailGoal').addClass('unActive');
                $('#TimesUsedPercent').html('0%');
                $('#PracticeScorePercent').html('0%');
            }
            if (data[i].ReadingGoal != '') {
                $('#ReadingTimeResult').html('Due date : ' + data[i].ReadingGoal + '<br />(Time left ' + data[i].ReadingGoalAmount + ' days)');
                $('#ReadingTimeResult').removeClass('ui-hide');
                $('#ReadingPS').removeClass('PS');
                $('#ReadingPS').html(data[i].ReadingScorePercent);
                $('#ReadingTime').html(data[i].ReadingDatePercent);
            } else {
                $('#ReadingTimeResult').addClass('ui-hide');
                $('#ReadingPS').html("0%");
                $('#ReadingTime').html("0%");
            }
            if (data[i].ListeningGoal != '') {
                $('#ListeningTimeResult').html('Due date : ' + data[i].ListeningGoal + '<br />(Time left ' + data[i].ListeningGoalAmount + ' days)');
                $('#ListeningTimeResult').removeClass('ui-hide');
                $('#ListeningPS').removeClass('PS');
                $('#ListeningPS').html(data[i].ListeningScorePercent);
                $('#ListeningTime').html(data[i].ListeningDatePercent);
            } else {
                $('#ListeningTimeResult').addClass('ui-hide');
                $('#ListeningPS').html("0%");
                $('#ListeningTime').html("0%");
            }
            if (data[i].VocabGoal != '') {
                $('#VocabularyTimeResult').html('Due date : ' + data[i].VocabGoal + '<br />(Time left ' + data[i].VocabGoalAmount + ' days)');
                $('#VocabularyTimeResult').removeClass('ui-hide');
                $('#VocabularyPS').removeClass('PS');
                $('#VocabularyPS').html(data[i].VocabScorePercent);
                $('#VocabTime').html(data[i].VocabDatePercent);
            } else {
                $('#VocabularyTimeResult').addClass('ui-hide');
                $('#VocabularyPS').html("0%");
                $('#VocabTime').html("0%");
            }
            if (data[i].GrammarGoal != '') {
                $('#GrammarTimeResult').html('Due date : ' + data[i].GrammarGoal + '<br />(Time left ' + data[i].GrammarGoalAmount + ' days)');
                $('#GrammarTimeResult').removeClass('ui-hide');
                $('#GrammarPS').removeClass('PS');
                $('#GrammarPS').html(data[i].GrammarScorePercent);
                $('#GrammarTime').html(data[i].GrammarDatePercent);
            } else {
                $('#GrammarTimeResult').addClass('ui-hide');
                $('#GrammarPS').html("0%");
                $('#GrammarTime').html("0%");
            }
            if (data[i].SituationGoal != '') {
                $('#SituationTimeResult').html('Due date : ' + data[i].SituationGoal + '<br />(Time left ' + data[i].SituationGoalAmount + ' days)');
                $('#SituationTimeResult').removeClass('ui-hide');
                $('#SituationPS').removeClass('PS');
                $('#SituationPS').html(data[i].GrammarScorePercent);
            } else {
                $('#SituationTimeResult').addClass('ui-hide');
                $('#SituationPS').html("0%");
                $('#SituationPS').html("0%");
            }
        }
    }
}
//20240904 -- Check Goal Date Noti
function CheckGoalNoti() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetGoalNoti',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                console.log(data[i].Result);
                if (data[i].Result == 'noti' || data[i].Result == 'notset') {
                    $('#dialogAlert').attr('action', 'focus');
                    $('#dialogAlert .ui-text').html(data[i].Msg);
                    popupOpen($('#dialogAlert'), 99999);
                }

            }
        }
    });
}
//20240923 -- Check Quiz ที่ทำค้างไว้ปิดไปหรือ Session หลุดไป
function CheckNotEndQuiz() {
    $.ajax({
        type: 'POST',
        url: '/weTest/CheckNotEndQuiz',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'havequiz') {
                    $('#dialogBackToQuiz').attr('action', 'focus');
                    popupOpen($('#dialogBackToQuiz'), 99999);
                }
            }
        }
    });
}
//20240924 -- ดึงปุ่มเลือกสกิลที่จะ Random จาก DB
function GetSkill() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetSkill',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].skillSet == 'error') {
                    //console.log(data[i].skillTxtAll);
                } else {
                    console.log('.' + data[i].skillSet + 'Contain');
                    $('.' + data[i].skillSet + 'Contain').removeClass('ui-hide');
                }
            }
        }
    });
}
//20240927 -- Update End Quiz
function UpdateEndQuiz() {
    $.ajax({
        type: 'POST',
        url: '/weTest/EndQuiz',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
            }
        }
    });
}


