var DateType, selectedDate, formatSelectedDate;
// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); });
// ============================================================ //
// ======================= Object Event ======================= //
$(document)
// ======================= Activity ======================= //
 .on('click', '#btnLesson', function (e, data) {
     $('#btnLesson').addClass('btnSelected');
     $('#btnRandom').removeClass('btnSelected');
 })
 .on('click', '#btnRandom', function (e, data) {
     $('#btnLesson').removeClass('btnSelected');
     $('#btnRandom').addClass('btnSelected');
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
     OpenFilterdate();
 })
//20240718 open dialog end date
 .on('click', '#btnEndDate', function (e, data) {
     DateType = 'end';
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

//20240718 Dialog Select filter daate
function OpenFilterdate() {

    $('.btnSelectDate').addClass('unActive');

    $('#spnGoalName').html('Select ' + DateType + ' date');

    var options = $.extend(global.datepickerOption, {
        maxDate: 1,
        onSelect: function () {
            var selected = $("#SelectFilterDate").datepicker("getDate");
            selectedDate = selected.getFullYear() + '-' + (selected.getMonth() + 1) + '-' + selected.getDate();
            formatSelectedDate = selected.getDate() + '/' + (selected.getMonth() + 1) + '/' + selected.getFullYear();

            $('#spnShowdate').html('Your ' + DateType + ' Date : ' + formatSelectedDate)
            $('#btnSelectDate').removeClass('unActive');
           
        }
    });

    $("#SelectFilterDate").datepicker(options);

    $('#dialogFilterDate').attr('action', 'focus');
    popupOpen($('#dialogFilterDate'), 99999);

}