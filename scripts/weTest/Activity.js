var OTPNum
// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); $('#Firstname').focus(); });

GetQuestionAndAnswer()

// ============================================================ //
// ======================= Object Event ======================= //
$(document)
// ======================= Activity ======================= //

 .on('click', '.btnNext', function (e, data) {
     if ($('.btnNext').hasClass('UnActive') == false) {GetQuestionAndAnswer('next'); }

 })
 .on('click', '.btnBack', function (e, data) {
     if ($('.btnBack').hasClass('UnActive') == false) { GetQuestionAndAnswer('back'); }
 })
 .on('click', '.divAnswerbar', function (e, data) {
     $('.divAnswerbar').removeClass("Answered");
     $(this).addClass("Answered");

     var post1 = 'AnsweredId=' + $(this).attr('ansid') + '&QuestionId=' + $(this).attr('QId');

     $.ajax({
         type: 'POST',
         url: '/weTest/SaveAnswed',
         data: post1,
         success: function (data) {

         }
     });
 })
 .on('click', '#divSendQuiz', function (e, data) {
     $.ajax({
         type: 'POST',
         url: '/weTest/EndQuiz',
         success: function (data) {
             for (var i = 0; i < data.length; i++) {
                 if (data[i].dataType == 'success2') {
                     window.location = '/Wetest/Activity';
                 }
             }
         }
     });
 })
// ============================================================ //

// ========================= Function ========================= //

function GetQuestionAndAnswer(ActionType) {
    var post1 = 'ActionType=' + ActionType;
    $.ajax({
        type: 'POST',
        url: '/weTest/GetQuestionAndAnswer',
        data: post1,
        success: function (data) {
    
            for (var i = 0; i < data.length; i++) {

                if (data[i].ItemStatus == 'sessionExpired') {
                    window.location = '/Wetest/User';
                }

                if (data[i].ItemType == 1) {
                    $('#divQuestion').html(data[i].Itemtxt);
                    $('#divQuestion').attr('Qid', data[i].ItemId);

                    console.log(data[i].ItemStatus);

                    if (data[i].ItemStatus == 'first') { $('.btnBack').addClass("UnActive"); }
                    if (data[i].ItemStatus == 'last') { $('.btnNext').addClass("UnActive"); }

                    if (data[i].ItemStatus == 'second') { $('.btnBack').removeClass("UnActive"); }
                    if (data[i].ItemStatus == 'beforelast') { $('.btnNext').removeClass("UnActive"); }
                } else {
                    $('#divAnswer').html(data[i].Itemtxt);
                }
            }
        }
    });
}
