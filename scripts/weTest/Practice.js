var ExamAmount, skill
// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); });
// ============================================================ //
// ======================= Object Event ======================= //
$(document)
// ======================= Activity ======================= //
 .on('click', '#btnLesson', function (e, data) {
     $('#btnLesson').addClass('btnSelected');
     $('#btnRandom').removeClass('btnSelected');
     $.ajax({
         type: 'POST',
         url: '/weTest/GetLesson',
         contentType: false,
         processData: false,
         success: function (data) {
             for (var i = 0; i < data.length; i++) {
                 if (data[i].skillSet == 'error') {
                     //console.log(data[i].skillTxtAll);
                 } else {

                     $('#' + data[i].skillSet + 'Lesson').html(data[i].skillTxtShort);

                     if (data[i].skillAmount < 6) {
                         $('#' + data[i].skillSet + 'Other').addClass('ui-hide');
                     } else {
                         $('#' + data[i].skillSet + 'Other').removeClass('ui-hide');
                         $('#All' + data[i].skillSet).html(data[i].skillTxtAll);
                     }
                 }
             }
         }
     });

     $('#LessonType').removeClass("ui-hide");
     $('#RandomType,#skillRandom,.btnStart').addClass("ui-hide");
 })
 .on('click', '#btnRandom', function (e, data) {
     $('#btnLesson').removeClass('btnSelected');
     $('#btnRandom').addClass('btnSelected');

     $('#LessonType').addClass("ui-hide");
     $('#RandomType ,#skillRandom,.btnStart').removeClass("ui-hide");
 })
 .on('click', '#ReadingOther', function (e, data) {
  
     $('#PracticeType,#LessonType').addClass('ui-hide');
     $('#AllReading,.footer').removeClass('ui-hide');

 })
 .on('click', '#ListeningOther', function (e, data) {

     $('#PracticeType,#LessonType').addClass('ui-hide');
     $('#AllListening,.footer').removeClass('ui-hide');
 })
 .on('click', '#GrammarOther', function (e, data) {
    
     $('#PracticeType,#LessonType').addClass('ui-hide');
     $('#AllGrammar,.footer').removeClass('ui-hide');
 })
 .on('click', '#SituationOther', function (e, data) {
  
     $('#PracticeType,#LessonType').addClass('ui-hide');
     $('#AllSituation,.footer').removeClass('ui-hide');
 })
 .on('click', '#VocabularyOther', function (e, data) {

     $('#PracticeType,#LessonType').addClass('ui-hide');
     $('#AllVocabulary,.footer').removeClass('ui-hide');
 })
 .on('click', '.btnBack', function (e, data) {
     window.location = '/Wetest/User';
 })
 .on('click', '.Lessondiv', function (e, data) {
        var TestsetName = $(this).text();
        var TestsetId = $(this).attr('id');
        $('#dialogSelect').attr('action', 'focus');
        $('#dialogSelect').attr('TestsetName', TestsetName);
        $('#dialogSelect').attr('TestsetId', TestsetId);

        $('#dialogSelect .ui-text').html('Do you want to start practice ' + TestsetName + ' now!');
        $('#dialogSelect .btnSelected').attr('TestsetId', TestsetId);

        popupOpen($('#dialogSelect'), 99999);
    })
 .on('click', '.ui-icon.close ', function (e, data) {
      popupClose($(this).closest('.my-popup'));
 })
 .on('click', '#dialogSelect .btnSelected', function (e, data) {

      popupClose($(this).closest('.my-popup'));
      GotoPractice($(this).attr('testsetid'));
 })
 .on('click', '#dialogSelect .btnCancel', function (e, data) {

     popupClose($(this).closest('.my-popup'));

 })
 //20240717 Choose Exam Amount
 .on('click', '.btnAmount', function (e, data) {
     $('.btnAmount').removeClass('btnSelected');
     $('#UserType').val('');
     $(this).addClass('btnSelected');
 })
 //20240717 Check numeric key press
 .on('keypress', '#UserType', function (e, data) {
    if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
        event.preventDefault(); //stop character from entering input
    }
 })
//20240717 Choose Random All Skill
 .on('click', '#btnRandomAll', function (e, data) {
     $('#btnRandomAll').toggleClass('btnSelected');
     $('.btnSkill').toggleClass('Selected');
 })
//20240717 Choose Random Skill
 .on('click', '.btnSkill', function (e, data) {
     $(this).toggleClass('Selected');
     $('#btnRandomAll').removeClass('Selected');
 })
//20240717 Start Random
 .on('click', '.btnStart', function (e, data) {
     if ($(".btnAmount.btnSelected").attr('id') == 'btnUserType') {
         ExamAmount = $("#UserType").val();
     } else {
        ExamAmount = $(".btnAmount.btnSelected").text();
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

        var post1 = 'ExamAmount=' + ExamAmount + '&arrSkill=' + skill;
     $.ajax({
         type: 'POST',
         url: '/weTest/RandomPractice',
         data: post1,
         success: function (data) {
             for (var i = 0; i < data.length; i++) {
                 if (data[i].dataType == 'success') {
                     console.log(data[i].errorMsg);
                     GotoPractice(data[i].errorMsg);
                 }
             }
         }
     });
 })

function GotoPractice(TestsetId) {

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




