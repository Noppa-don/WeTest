var IsLesson, selectedStartDate, selectedEndDate, formatSelectedDate;
// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); });
// ============================================================ //
// ======================= Object Event ======================= //
$(document)
// ======================= Activity ======================= //
 .on('click', '#btnLesson', function (e, data) {
     $('#btnLesson').addClass('btnSelected');
     $('#btnRandom').removeClass('btnSelected');
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
 })
 .on('click', '#btnRandom', function (e, data) {
     $('#btnLesson').removeClass('btnSelected');
     $('#btnRandom').addClass('btnSelected');
     IsLesson = '0'
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
 })
 .on('change', '#rdbThisWeek', function (e, data) {
     if(this.checked) {
         selectedStartDate = 'week';
         $('.calendarlogo').addClass('unActive');
         $('#StartDate').val('');
         $('#EndDate').val('');
         if (typeof IsLesson != 'undefined') { $('#btnSearch').removeClass('unActive') }
     }
 })
 .on('change', '#rdbMonth', function (e, data) {
     if (this.checked) {
         selectedStartDate = 'month';
         $('.calendarlogo').addClass('unActive');
         $('#StartDate').val('');
         $('#EndDate').val('');
         if (typeof IsLesson != 'undefined') { $('#btnSearch').removeClass('unActive') }
     }
 })
 .on('change', '#rdbChooseDate', function (e, data) {
     if (this.checked) {
         selectedStartDate = 'manual';
         $('.calendarlogo').removeClass('unActive');
         $('#btnSearch').addClass('unActive')
     }
 
 })
 .on('click', '.btnBack', function (e, data) {
     window.location = '/Wetest/User';
 }) 
//20240718 Choose Random All Skill
 .on('click', '#btnRandomAll', function (e, data) {
     $('#btnRandomAll').toggleClass('btnSelected');
     $('.btnSkill').toggleClass('Selected');
 })
//20240718 Choose Random Skill
 .on('click', '.btnSkill', function (e, data) {
     $(this).toggleClass('Selected');
     $('#btnRandomAll').removeClass('Selected');
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

     if( $('#StartDate').val() != '' && $('#EndDate').val() != ''){
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
     $('#dialogSelectAgain').attr('action', 'focus');
     $('#dialogSelectAgain .ui-text').html('Do you want to do this practice again ?');
     $('#dialogSelectAgain .btnSelected').attr('testsetid', TestsetId);

     popupOpen($('#dialogSelectAgain'), 99999);

 })
 .on('click', '#dialogSelectAgain .btnSelected', function (e, data) {

     popupClose($(this).closest('.my-popup'));
     GotoPracticeAgain($(this).attr('testsetid'));
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
function SearchReport() {
    var startdate, enddate;
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

    var arrSkill = new Array();

    if ($("#btnRandomAll").hasClass('btnSelected') == true) {
        arrSkill.push('All');
    } else {
        $('.Selected').each(function (i, obj) {
            arrSkill.push($(this).attr('id'));
        });
    }

    skill = arrSkill.join(',');

    if (skill == '') { skill = 'All'; }

    var post1 = 'StartDate=' + startdate + '&EndDate=' + enddate + '&PracticeType=' + IsLesson + '&arrSkill=' + skill;

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
function GotoPracticeAgain(TestsetId) {
    var post1 = 'TestsetId=' + TestsetId;

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