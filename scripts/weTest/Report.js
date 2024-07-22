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
     PracticeType = '1'
 })
 .on('click', '#btnRandom', function (e, data) {
     $('#btnLesson').removeClass('btnSelected');
     $('#btnRandom').addClass('btnSelected');
     PracticeType = '0'
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
 })
//20240718 open dialog start date
 .on('click', '#btnStartDate', function (e, data) {
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
    })
//20240718 -- Search
 .on('click', '#btnSearch', function (e, data) {
     var startdate, enddate;
     if ($('#rdbThisWeek').is(":checked")) {
         startdate = 'week';
     }
     if ($('#rdbThisMonth').is(":checked")) {
         startdate = 'month';
     }
     if ($('#rdbChooseDate').is(":checked")) {
         startdate = selectedStartDate
         enddate = selectedEndDate
     }

     var post1 = 'StartDate=' + startdate + '&EndDate=' + enddate + '&PracticeType=' + PracticeType;

     $.ajax({
         type: 'POST',
         url: '/weTest/GetReport',
         data: post1,
         success: function (data) {
             for (var i = 0; i < data.length; i++) {
                 if (data[i].Result == 'success') {
                    
                 } else {
                     $('#dialogAlert').attr('action', 'focus');
                     $('#dialogAlert .ui-text').html(data[i].Msg);
                     popupOpen($('#dialogAlert'), 99999);
                 }

             }
         }
     });
 })


//20240718 Dialog Select filter daate
function OpenFilterdate() {

    $('.btnSelectDate').addClass('unActive');

    $('#spnDateName').html('Select ' + DateType + ' date');

    var options = $.extend(global.datepickerOption, {
        maxDate: 1,
        onSelect: function () {
            var selected = $("#SelectFilterDate").datepicker("getDate");

            if (DateType == 'start') {
                selectedStartDate = selected.getFullYear() + '-' + (selected.getMonth() + 1) + '-' + selected.getDate();
            } else {
                selectedEndDate = selected.getFullYear() + '-' + (selected.getMonth() + 1) + '-' + selected.getDate();
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