﻿var IsLesson, selectedStartDate, selectedEndDate, formatSelectedDate;
// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); });
CheckLoginStatus();
GetSkill();
// ============================================================ //
// ======================= Object Event ======================= //
$(document)
// ======================= Activity ======================= //
//20240830 -- เพิ่ม Filter Level
 .on('click', '#btnLesson', function (e, data) {
     $('#btnLesson').addClass('btnSelected');
     $('#btnRandom').removeClass('btnSelected');
     $('#skillRandom').removeClass('ui-hide');
     IsLesson = '1'
     console.log(selectedStartDate);
     if (typeof selectedStartDate != 'undefined') {
         if (selectedStartDate == 'manual') {
             if ($('#StartDate').val() != '' && $('#EndDate').val() != '') {
                 $('#btnSearch').removeClass('unActive')
             } else {
                 $('#btnSearch').addClass('unActive')
             }
         } else {
             $('#btnSearch').removeClass('unActive')
         }
     }
     GetLevel();
 })
 .on('click', '#btnRandom', function (e, data) {
     $('#btnLesson').removeClass('btnSelected');
     $('#ChooseLevel,#skillRandom').addClass('ui-hide');
     $('#btnRandom').addClass('btnSelected');
     IsLesson = '0'
     if (typeof selectedStartDate != 'undefined') {
         if (selectedStartDate == 'manual') {
             if ($('#StartDate').val() != '' && $('#EndDate').val() != '') {
                 $('#btnSearch').removeClass('unActive')
             } else {
                 $('#btnSearch').addClass('unActive')
             }
         } else {
             $('#btnSearch').removeClass('unActive')
         }
     }
 })
 .on('change', '#rdbThisWeek', function (e, data) {
     if (this.checked) {
         selectedStartDate = 'week';
         $('.calendarlogo').addClass('unActive');
         $('#StartDate').val('');
         $('#EndDate').val('');
         $('.filterdate').addClass('ui-hide');
         if (typeof IsLesson != 'undefined') { $('#btnSearch').removeClass('unActive') }
     }
 })
 .on('change', '#rdbMonth', function (e, data) {
     if (this.checked) {
         selectedStartDate = 'month';
         $('.calendarlogo').addClass('unActive');
         $('#StartDate').val('');
         $('#EndDate').val('');
         $('.filterdate').addClass('ui-hide');
         if (typeof IsLesson != 'undefined') { $('#btnSearch').removeClass('unActive') }
     }
 })
 .on('change', '#rdbChooseDate', function (e, data) {
     if (this.checked) {
         selectedStartDate = 'manual';
         $('.calendarlogo').removeClass('unActive');
         $('.filterdate').removeClass('ui-hide');
         $('#btnSearch').addClass('unActive')
     }

 })
 .on('click', '.btnBack', function (e, data) {
     window.location = '/Wetest/User';
 })
//20240718 -- Choose Random All Skill
//20240916 -- ปรับการ toggle class ให้แสดงผลถูกต้อง
 .on('click', '#btnRandomAll', function (e, data) {
     $('#btnRandomAll').toggleClass('btnSelected');
     if ($('#btnRandomAll').hasClass('btnSelected')) {
         $('.btnSkill').addClass('Selected');
     } else {
         $('.btnSkill').removeClass('Selected');
     }
     SearchReport();
 })
//20240718 -- Choose Random Skill
//20240916 -- ปรับการ toggle class ให้แสดงผลถูกต้อง
 .on('click', '.btnSkill', function (e, data) {
     $(this).toggleClass('Selected');
     var numItems = $('.Selected').length
     if (numItems == 5) {
         $('#btnRandomAll').addClass('btnSelected');
     } else {
         $('#btnRandomAll').removeClass('btnSelected');
     }
     SearchReport();
 })
//20240718 open dialog start date
 .on('click', '#btnStartDate', function (e, data) {
     if ($('#btnStartDate').hasClass('unActive')) { return false; }
     DateType = 'start';
     if ($('#StartDate').val() != '') {
         $('#spnShowdate').html('Your start date : ' + $('#StartDate').val());
     } else {
         $('#spnShowdate').addClass('ui-hide');
     }
     OpenFilterdate();
 })
//20240718 open dialog end date
 .on('click', '#btnEndDate', function (e, data) {
     if ($('#btnEndDate').hasClass('unActive')) { return false; }
     DateType = 'end';
     if ($('#EndDate').val() != '') {
         $('#spnShowdate').html('Your end date : ' + $('#EndDate').val());
     } else {
         $('#spnShowdate').addClass('ui-hide');
     }
     OpenFilterdate();
 })
//20240718 -- Close Dialog
 .on('click', '.ui-icon.close', function (e, data) {
     popupClose($(this).closest('.my-popup'));
 })
//20240718 -- Choose StrartDate
 .on('click', '#btnSelectDate', function (e, data) {
     if (DateType == 'start') {
         $('#StartDate').val(formatSelectedDate);
     } else {
         $('#EndDate').val(formatSelectedDate);
     }
     popupClose($(this).closest('.my-popup'));

     if ($('#StartDate').val() != '' && $('#EndDate').val() != '') {
         $('#btnSearch').removeClass('unActive')
     }
 })
//20240718 -- Search
 .on('click', '#btnSearch', function (e, data) {

     if ($('#btnSearch').hasClass('unActive')) { return false; }

     SearchReport();
 })
//20240722 -- Play Again
 .on('click', '.divAgain', function (e, data) {

     var TestsetId = $(this).attr('testsetId');
     var TestsetName = $(this).attr('testsetname');
     $('#dialogSelectAgain').attr('action', 'focus');
     $('#dialogSelectAgain .ui-text').html('Do you want to do this practice again ?');
     $('#dialogSelectAgain .btnSelected').attr('testsetid', TestsetId);
     $('#dialogSelectAgain .btnSelected').attr('testsetname', TestsetName);

     popupOpen($('#dialogSelectAgain'), 99999);

 })
 .on('click', '#dialogSelectAgain .btnSelected', function (e, data) {

     popupClose($(this).closest('.my-popup'));
     console.log($(this).attr('testsetname'));
     GotoPracticeAgain($(this).attr('testsetid'), $(this).attr('testsetname'));
 })
 .on('click', '#dialogSelectAgain .btnCancel', function (e, data) {

     popupClose($(this).closest('.my-popup'));

 })
//20240722 -- Play Answered
 .on('click', '.divAnswered', function (e, data) {
     var post1 = 'quizId=' + $(this).attr('quizId');

     $.ajax({
         type: 'POST',
         url: '/weTest/SetSeesionAnswered',
         data: post1,
         success: function (data) {
             for (var i = 0; i < data.length; i++) {
                 if (data[i].dataType == 'success') {
                     var x = setInterval(function () {
                         clearInterval(x);
                         window.location = '/Wetest/Activity';
                     }, 1000);
                 }
             }
         }
     });
 })

// ======================= function ======================= //
//20240718 -- Dialog Select filter date
function OpenFilterdate() {

    $('.btnSelectDate').addClass('unActive');

    $('#spnDateName').html('Select ' + DateType + ' date');

    var options = $.extend(global.datepickerOption, {
        maxDate: 1,
        onSelect: function () {
            var selected = $("#SelectFilterDate").datepicker("getDate");

            if (DateType == 'start') {
                //2024-07-22 14:18:55.457
                selectedStartDate = selected.getFullYear() + '-' + (selected.getMonth() + 1) + '-' + selected.getDate() + ' 00:00:00.000';
            } else {
                selectedEndDate = selected.getFullYear() + '-' + (selected.getMonth() + 1) + '-' + selected.getDate() + ' 23:59:59.000';
            }

            formatSelectedDate = selected.getDate() + '/' + (selected.getMonth() + 1) + '/' + selected.getFullYear()

            $('#spnShowdate').html('Your ' + DateType + ' date : ' + formatSelectedDate);
            $('#spnShowdate').removeClass('ui-hide');
            $('#btnSelectDate').removeClass('unActive');

        }
    });

    $("#SelectFilterDate").datepicker(options);

    $('#dialogFilterDate').attr('action', 'focus');
    popupOpen($('#dialogFilterDate'), 99999);

}
//20240722 -- Search Report with Filter
//20240823 -- Check Session
//20240830 -- เพิ่ม Filter Level
function SearchReport() {
    var startdate, enddate, LevelId;
    if ($('#rdbThisWeek').is(":checked")) {
        startdate = 'week';
    }
    if ($('#rdbMonth').is(":checked")) {
        startdate = 'month';
    }
    if ($('#rdbChooseDate').is(":checked")) {
        startdate = selectedStartDate
        enddate = selectedEndDate
    }

    if ($('#ChooseLevel').hasClass('ui-hide') == false) {
        LevelId = $('#ddlLevel').val();
        console.log(LevelId);
    }

    var arrSkill = new Array();

    if ($("#btnRandomAll").hasClass('btnSelected') == true) {
        arrSkill.push('All');
    } else {
        $('.Selected').each(function (i, obj) {
            if ($(this).hasClass('btnSituation') == false) {
                arrSkill.push($(this).attr('id'));
            }
        });
    }

    skill = arrSkill.join(',');

    if (skill == '') { skill = 'All'; }

    var post1 = 'StartDate=' + startdate + '&EndDate=' + enddate + '&PracticeType=' + IsLesson + '&arrSkill=' + skill + '&LevelId=' + LevelId;

    $.ajax({
        type: 'POST',
        url: '/weTest/GetReport',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'success') {
                    $('.nodata').addClass('ui-hide');
                    $('.reportDetail').removeClass('ui-hide');
                    $('.detailData').html(data[i].errorMsg);
                } else if (data[i].dataType == 'nodata') {
                    $('.nodata').removeClass('ui-hide');
                    $('.reportDetail').addClass('ui-hide');
                } else if (data[i].dataType == 'sessionlost') {
                    window.location = '/Wetest/user';
                } else {
                    $('#dialogAlert').attr('action', 'focus');
                    $('#dialogAlert .ui-text').html(data[i].errorMsg);
                    popupOpen($('#dialogAlert'), 99999);
                }

            }
        }
    });
}
//20240722 -- Play Again
function GotoPracticeAgain(TestsetId, TestsetName) {
    var post1 = 'TestsetId=' + TestsetId + '&TestsetName=' + TestsetName;

    $.ajax({
        type: 'POST',
        url: '/weTest/CreatePractice',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'success') {
                    var x = setInterval(function () {
                        clearInterval(x);
                        window.location = '/Wetest/Activity';
                    }, 1000);
                }
            }
        }
    });
}
//20240723 -- Get name level and Photo
function CheckLoginStatus() {
    $.ajax({
        type: 'POST',
        url: '/weTest/CheckLoginStatus',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'success') {
                    $('.UserNameandLevel').html('Welcome, ' + data[i].Firstname + '<br />' + data[i].UserLevel);
                    $('.UserData').append(data[i].UserPhoto);
                    $('#UserLevel').html('Your Level : ' + data[i].UserLevel + '<br />');

                }
            }
        }
    });
}
//20240830 -- สร้าง DropdownLevel
function GetLevel() {

    $.ajax({
        type: 'POST',
        url: '/weTest/GetLevel',
        success: function (data) {
            var selectHTML = "";
            if (data[0].result == 'success') {
                if (data.length > 1) {
                    selectHTML = '<select id="ddlLevel">';
                    var LevelId;
                    for (var i = 0; i < data.length; i++) {
                        selectHTML += "<option value='" + data[i].LevelId + "'>" + data[i].LevelName + "</option>";
                        LevelId = data[0].LevelId
                    }
                    selectHTML += "</select>";
                    $('#SelectLevel').html(selectHTML);
                    $('#ChooseLevel').removeClass('ui-hide');
                } else { $('#ChooseLevel').addClass('ui-hide'); }
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
                    $('.btn' + data[i].skillSet).removeClass('ui-hide');
                }
            }
        }
    });
}