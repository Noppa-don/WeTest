// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); });
// ============================================================ //
// ======================= Object Event ======================= //
$(document)
// ======================= Activity ======================= //
 .on('click', '#btnLesson', function (e, data) {

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
     $('#RandomType').addClass("ui-hide");
     $('#skillRandom').addClass("ui-hide");
 })
 .on('click', '#btnRandom', function (e, data) {

     $('#LessonType').addClass("ui-hide");
     $('#RandomType').removeClass("ui-hide");
     $('#skillRandom').removeClass("ui-hide");
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
     $('#PracticeType,#LessonType').removeClass('ui-hide');
     $('#AllReading,#AllListening,#AllGrammar,#AllSituation,#AllVocabulary,.footer').addClass('ui-hide');
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
                    }, 2000);
                }
            }
        }
    });
}




