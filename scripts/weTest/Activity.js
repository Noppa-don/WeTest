var OTPNum, counterId, PageNum, AllPage;
var sec = 0;
var MultiFileCount = 1;
var MultiFileAmount;
var MultiFileSlowAmount;
// ========================= Page Load ======================== //
//20240916 -- ปรับขนาดหน้าจอแสดง Level ที่สามารถสอบได้
$(function () {
    $('div[data-role=page]').page({ theme: 'c', });
    $('#divActivity,#divShowLevel').css('height', $(window).height());
    $('#divQuestionAndAnswer').css('height', $('#divQ').height());

    GetStartTime();
    checkAnsweredFromReport();
    GetQuizConfigVal();
    SetLogo();
});

// ============================================================ //
// ============================================== Object Event ============================================== //
$(document)
// ======================= Activity =========================== //
 .on('focus', '.txtDetail,.ui-select', function (e, data) { $(this).removeClass("InvalidData") })
 //20240726 -- เพิ่มการกดหยุดไฟล์เสียง
 //20240807 -- ปรับการกดหยุดไฟล์เสียง
 //20240830 -- เพิ่มตรวจสอบ Delay การกดเลื่อนข้อ
 .on('click', '.btnNext', function (e, data) {
     if ($('.btnNext').hasClass('UnActive') == false && $('.btnNext').hasClass('wait') == false) {
         DelayChangeExam();
         if ($('.bap-icon').children().hasClass('bap-icon-on')) {
             $('.multiQfileIcon').click()
             if ($('.bap-icon').children().hasClass('bap-icon-on')) { $('.multiQfileIcon').click() }
         }
         if ($('.bap-icon').children().hasClass('bap-icon-on')) {
             $('.multiQfileSlowIcon').click()
             if ($('.bap-icon').children().hasClass('bap-icon-on')) { $('.multiQfileSlowIcon').click() }
         }

         GetQuestionAndAnswer('next', '');
         MultiFileCount = 1;
     }
 })
 //20240726 -- เพิ่มการกดหยุดไฟล์เสียง
 //20240807 -- เพิ่มการกดหยุดไฟล์เสียง
 //20240830 -- เพิ่มตรวจสอบ Delay การกดเลื่อนข้อ
 .on('click', '.btnBack', function (e, data) {
     if ($('.btnBack').hasClass('UnActive') == false && $('.btnBack').hasClass('wait') == false) {
         DelayChangeExam();
         if ($('.bap-icon').children().hasClass('bap-icon-on')) {
             $('.multiQfileIcon').click()
             if ($('.bap-icon').children().hasClass('bap-icon-on')) { $('.multiQfileIcon').click() }
         }
         if ($('.bap-icon').children().hasClass('bap-icon-on')) {
             $('.multiQfileSlowIcon').click()
             if ($('.bap-icon').children().hasClass('bap-icon-on')) { $('.multiQfileSlowIcon').click() }
         }

         GetQuestionAndAnswer('back', '');
         MultiFileCount = 1;
     }
 })
 .on('click', '.divAnswerbar', function (e, data) {
     //20240715 -- ตรวจสอบไม่ให้กดตอบคำถามใน Mode เฉลย
     $('.divAnswerbar').removeClass('Answered');
     $(this).addClass('Answered');

     var post1 = 'AnsweredId=' + $(this).attr('ansid') + '&QuestionId=' + $(this).attr('QId');
     $.ajax({
         type: 'POST',
         url: '/weTest/SaveAnswed',
         data: post1,
         success: function (data) {
             for (var i = 0; i < data.length; i++) {
                 if (data[i].dataType == 'answered') {

                 }
             }
         }
     });
 })
 //202408017 -- เพิ่มการกดหยุดไฟล์เสียง
 .on('click', '#divSendQuiz', function (e, data) {
     if ($('.bap-icon').children().hasClass('bap-icon-on')) {
         $('.multifileIcon').click()
         if ($('.bap-icon').children().hasClass('bap-icon-on')) { $('.multifileIcon').click() }
     }
     $.ajax({
         type: 'POST',
         url: '/weTest/CheckSendDialog',
         success: function (data) {
             for (var i = 0; i < data.length; i++) {
                 $('#dialogSelect').attr('action', 'focus');
                 $('#dialogSelect .ui-text').html(data[i].errorMsg);
                 popupOpen($('#dialogSelect'), 99999);
             }
         }
     });
 })
 .on('click', '#divGotoLogin', function (e, data) {
     window.location = '/Wetest/User';
 })
 .on('click', '#dialogSelect .btnSelected', function (e, data) {

     popupClose($(this).closest('.my-popup'));

     $.ajax({
         type: 'POST',
         url: '/weTest/EndQuiz',
         success: function (data) {
             for (var i = 0; i < data.length; i++) {

                 if (data[i].dataType == 'success2') {
                     window.location = '/Wetest/Activity';
                 } else if (data[i].dataType == 'success3') {

                     $('#divActivity, #divGotoMainMenu').addClass("ui-hide");
                     $('#divShowLevel,.wrapper,.banner,#divGotoLogin').removeClass("ui-hide");
                     $('#spnLevel').html('<b>Congratulations !</b> Your Level is ' + data[i].errorMsg + '<br /><br />You can go to Log-in for practice and exam more.');

                     $('#lr' + data[i].errorMsg).removeClass("LR" + data[i].errorMsg);
                     $('#lr' + data[i].errorMsg).addClass("LR" + data[i].errorMsg + "Selected");
                 } else if (data[i].dataType == 'pass') {
                     $('#divActivity,#divGotoLogin').addClass("ui-hide");
                     $('#divShowLevel,.wrapper,.banner,#divGotoMainMenu').removeClass("ui-hide");
                     $('#spnLevel').html('<b>Congratulations !</b> Your Level up to ' + data[i].errorMsg);

                     $('#lr' + data[i].errorMsg).removeClass("LR" + data[i].errorMsg);
                     $('#lr' + data[i].errorMsg).addClass("LR" + data[i].errorMsg + "Selected");
                 } else if (data[i].dataType == 'notpass') {
                     $('#divActivity,#divGotoLogin').addClass("ui-hide");
                     $('#divShowLevel,.wrapper,.banner,#divGotoMainMenu').removeClass("ui-hide");
                     $('#spnLevel').html('<b>Sorry, You dont passed</b>');

                     $('#lr' + data[i].errorMsg).removeClass("LR" + data[i].errorMsg);
                     $('#lr' + data[i].errorMsg).addClass("LR" + data[i].errorMsg + "Selected");
                 } else if (data[i].dataType == 'showanswer') {
                     $('#divRunningBar, #divAllLeapChoice, #divTime,#divPause').addClass("ui-hide");
                     $('#divAllQuestion, #divShowExplain').removeClass("ui-hide");
                     $('.btnNext').removeClass('UnActive');
                     $('#divSendQuiz').removeClass('SendQuiz').addClass('FinishQuiz');
                     GetQuestionAndAnswer('select', 1);
                 } else if (data[i].dataType == 'gotoreport') {
                     window.location = '/Wetest/Report';
                 } else if (data[i].dataType == 'success4') {
                     window.location = '/Wetest/user';
                 }

             }
         }
     });
 })
 .on('click', '#dialogSelect .btnCancel', function (e, data) {
     popupClose($(this).closest('.my-popup'));
 })
 .on('click', '#divPause', function (e, data) {
     $('#dialogPause').attr('action', 'focus');
     popupOpen($('#dialogPause'), 99999);
     clearInterval(counterId);
 })
 .on('click', '.play', function (e, data) {
     popupClose($(this).closest('.my-popup'));
     QuizTimer()
 })
 .on('click', '#divBtnReportProblem', function (e, data) {
     $('#dialogPromblemQuestion').attr('action', 'focus');
     popupOpen($('#dialogPromblemQuestion'), 99999);
 })
 .on('click', '.ui-icon.close', function (e, data) {
     popupClose($(this).closest('.my-popup'));
 })
 .on('click', '#btnSendProblem', function (e, data) {

     var ProblemDetail = $('.txtDetail').val();
     var ProblemTopic = $('#ProblemTopic').find(":selected").text();
     var ProblemTopicVal = $('#ProblemTopic').find(":selected").val();
     var chackResult = true;
     if (ProblemTopicVal == 0) {
         $('.ui-select').addClass("InvalidData");
         chackResult = false;
     }
     if (ProblemDetail == '') {
         $('.txtDetail').addClass("InvalidData");
         chackResult = false;
     }

     if (chackResult == true) {
         var post1 = 'ProblemTopic=' + ProblemTopic + '&ProblemDetail=' + ProblemDetail;

         $.ajax({
             type: 'POST',
             url: '/weTest/SaveQuestionProblem',
             data: post1,
             success: function (data) {
                 $('.txtDetail').val('');
                 $(".ui-select").val("0").change();

                 popupClose(($('#dialogPromblemQuestion')).closest('.my-popup'));

                 $('#dialogAlert').attr('action', 'focus');
                 $('#dialogAlert .ui-text').html('Thank you for your recommend');
                 popupOpen($('#dialogAlert'), 99999);
             }
         });
     }
 })
 .on('click', '#dialogAlert #btnOK', function (e, data) {
     popupClose($(this).closest('.my-popup'));
 })
 .on('click', '#divShowExplain', function (e, data) {
     if ($('.ExplainQ').hasClass('ui-hide') == false) {
         $('.ExplainQ').addClass("ui-hide");
     } else {
         $('.ExplainQ').removeClass("ui-hide");
     }
 })
 .on('click', '#divGotoMainMenu', function (e, data) {
     window.location = '/Wetest/User';
 })
//20240801 -- กดรูปภาพแล้วขยาย
 .on('click', '#divQuestionAndAnswer img', function (e, data) {
     var img = $(this);
     var bigImg = $('<img />').css({ 'max-width': '80%', 'max-height': '80%', 'display': 'inline', 'margin-top': '10px' });
     bigImg.attr({ src: img.attr('src'), alt: img.attr('alt'), title: img.attr('title') });

     var over = $('<div />').text(' ').css({
         'height': '100%', 'width': '100%', 'background': 'rgba(0,0,0,.82)', 'position': 'fixed', 'top': 0, 'left': 0,
         'opacity': 0.0, 'cursor': 'pointer', 'z-index': 9999, 'text-align': 'center'
     }).append(bigImg).bind('click', function () {
         $(this).fadeOut(300, function () {
             $(this).remove();
         });
     }).insertAfter(this).animate({
         'opacity': 1
     }, 300);
 })
//20240805 -- กดฟังไฟล์เสียง multiQuestion
//20240814 -- เปลี่ยนไป click Icon แล้วให้เล่นไฟล์
//20240830 -- เมื่อกดเล่นไฟล์เสียงที่คำตอบ ให้หยุดเล่น
 .on('click', '.multiQfileIcon', function (e, data) {
     var PlayCount = $('#multiQuestion').attr('PlayCount');
     if (PlayCount == MultiFileAmount) {
         $('#multiQuestion .bap-btn').click();
         $('#multiQfileSlow').attr('PlayCount', 1);
         if ($('.multiQfileSlowIcon').length) {
             $('.multiQfileIcon').addClass('ui-hide');
             $('.multiQfileSlowIcon').removeClass('ui-hide');
         } else if ($('.multiQtxtIcon').length) {
             $('.multiQfileIcon').addClass('ui-hide');
             $('.multiQtxtIcon').removeClass('ui-hide');
         }
     } else {
         $('#multiQuestion .bap-btn').click();
         PlayCount = parseInt(PlayCount) + 1;
         $('#multiQuestion').attr('PlayCount', PlayCount);
     }
 })
//20240805 -- กดฟังไฟล์เสียง multiSlowQuestion
//20240814 -- เปลี่ยนไป click Icon แล้วให้เล่นไฟล์
 .on('click', '.multiQfileSlowIcon', function (e, data) {
     var PlayCount = $('#multiSlowQuestion').attr('PlayCount');
     if (PlayCount == MultiFileSlowAmount) {
         $('#multiSlowQuestion .bap-btn').click();
         if ($('.multiQtxtIcon').length) {
             $('.multiQfileSlowIcon').addClass('ui-hide');
             $('.multiQtxtIcon').removeClass('ui-hide');
         }
     } else {
         $('#multiSlowQuestion .bap-btn').click();
         PlayCount = parseInt(PlayCount) + 1;
         $('#multiSlowQuestion').attr('PlayCount', PlayCount);
     }
 })
//20240814 -- เปิดคำอ่านไฟล์เสียง
 .on('click', '.multiQtxtIcon', function (e, data) {
     $('#multiQtxt').removeClass('ui-hide');
 })
//20240822 -- ปรับการกดเล่นไฟล์เสียงแต่ละคำตอบ
//20240830 -- เมื่อกดเล่นไฟล์เสียงที่คำตอบ ให้หยุดเล่นไฟล์อื่นๆ
 .on('click', '.multiAfileIcon', function (e, data) {
     if ($('#multiQuestion .bap-btn .bap-icon').children().hasClass('bap-icon-on')) {
         var QCount = $('#multiQuestion').attr('PlayCount');
         $('.multiQfileIcon').click()
         if ($('#multiQuestion .bap-btn .bap-icon').children().hasClass('bap-icon-on')) { $('.multiQfileIcon').click() }
         $('#multiQuestion').attr('PlayCount', QCount);
     }
     if ($('#multiSlowQuestion .bap-btn .bap-icon').children().hasClass('bap-icon-on')) {
         var QSCount = $('#multiSlowQuestion').attr('PlayCount');
         $('.multiQfileSlowIcon').click()
         if ($('#multiSlowQuestion .bap-btn .bap-icon').children().hasClass('bap-icon-on')) { $('.multiQfileSlowIcon').click() }
         $('#multiSlowQuestion').attr('PlayCount', QSCount);
     }
     var MID = $(this).attr('MID');
     var PlayCount = $(this).attr('PlayCount');
     if (PlayCount == MultiFileAmount) {
         $('#multiAnswer' + MID + ' .bap-btn').click();

         $('#MA' + MID).attr('PlayCount', 1);

         if ($('#multiAnswerSlow' + MID).length) {
             $('#MA' + MID).addClass('ui-hide');
             $('#MAS' + MID).removeClass('ui-hide');
         } else if ($('#MATIcon' + MID).length) {
             $('#MAS' + MID).addClass('ui-hide');
             $('#MATIcon' + MID).removeClass('ui-hide');
         }
     } else {
         PlayCount = parseInt(PlayCount) + 1;
         $('#multiAnswer' + MID + ' .bap-btn').click();
         $('#MA' + MID).attr('PlayCount', PlayCount);
     }
 })
 .on('click', '.multiAfileSlowIcon', function (e, data) {
     var MID = $(this).attr('MID');
     var PlayCount = $(this).attr('PlayCount');
     if (PlayCount == MultiFileAmount) {
         $('#multiAnswerSlow' + MID + ' .bap-btn').click();

         $('#MAS' + MID).attr('PlayCount', 1);

         if ($('#MATIcon' + MID).length) {
             $('#MAS' + MID).addClass('ui-hide');
             $('#MATIcon' + MID).removeClass('ui-hide');
         }
     } else {
         PlayCount = parseInt(PlayCount) + 1;
         $('#multiAnswerSlow' + MID + ' .bap-btn').click();
         $('#MAS' + MID).attr('PlayCount', 1);
     }
 })

// ==== Dialog ข้อข้าม ========================================================================================= //
 .on('click', '#divAllLeapChoice ,#btnAllChoice', function (e, data) {
     GetLeapChoicePanel(1);
 })
 .on('click', '#dialogLeapChoice .btnNextPage', function (e, data) {
     if ($('#dialogLeapChoice .btnNextPage').hasClass('UnActive') == false) {
         $('#pageLeapchoice' + PageNum).addClass("ui-hide");
         PageNum += 1;
         $('#pageLeapchoice' + PageNum).removeClass("ui-hide");
         if (PageNum == AllPage) {
             $('#dialogLeapChoice .btnNextPage').addClass("UnActive");
         }
         if (PageNum == 2) {
             $('#dialogLeapChoice .btnBackPage').removeClass("UnActive");
         }
     }
 })
 .on('click', '#dialogLeapChoice .btnBackPage', function (e, data) {
     if ($('#dialogLeapChoice .btnBackPage').hasClass('UnActive') == false) {
         $('#pageLeapchoice' + PageNum).addClass("ui-hide");
         PageNum -= 1;
         $('#pageLeapchoice' + PageNum).removeClass("ui-hide");

         if (PageNum == 1) {
             $('#dialogLeapChoice .btnBackPage').addClass("UnActive");
         }
         if (PageNum == AllPage - 1) {
             $('#dialogLeapChoice .btnNextPage').removeClass("UnActive");
         }
     }
 })
 .on('click', '.LeapchoiceItem', function (e, data) {
     var QuestionNo = $(this).attr('qno');
     GetQuestionAndAnswer('select', QuestionNo);

     if (QuestionNo == 1) {
         $('.btnNext').removeClass("UnActive");
         $('.btnBack').addClass("UnActive");
     }
     popupClose($(this).closest('.my-popup'));
 })
 .on('click', '#btnSkip', function (e, data) {
     GetLeapChoicePanel(2);
 })
//20240715 -- ทำต่อข้อล่าสุด
 .on('click', '#btnGoToLast', function (e, data) {
     window.location = '/Wetest/Activity';
 })

// ==== Dialog เฉลย ========================================================================================= //
 //20240715 -- แสดง Dialog เฉลย
 .on('click', '#divAllQuestion', function (e, data) {
     GetAnswerChoicePanel(1)
 })
 //20240715 -- ปุ่ม Next บน Dialog เฉลย
 .on('click', '#dialogResultChoice .btnNextPage', function (e, data) {
     if ($('#dialogResultChoice .btnNextPage').hasClass('UnActive') == false) {
         $('#pageAnswerchoice' + PageNum).addClass("ui-hide");
         PageNum += 1;
         $('#pageAnswerchoice' + PageNum).removeClass("ui-hide");
         if (PageNum == AllPage) {
             $('#dialogResultChoice .btnNextPage').addClass("UnActive");
         }
         if (PageNum == 2) {
             $('#dialogResultChoice .btnBackPage').removeClass("UnActive");
         }
     }
 })
 //20240715 -- ปุ่ม Back บน Dialog เฉลย
 .on('click', '#dialogResultChoice .btnBackPage', function (e, data) {
     if ($('#dialogResultChoice .btnBackPage').hasClass('UnActive') == false) {
         $('#pageAnswerchoice' + PageNum).addClass("ui-hide");
         PageNum -= 1;
         $('#pageAnswerchoice' + PageNum).removeClass("ui-hide");

         if (PageNum == 1) {
             $('#dialogResultChoice .btnBackPage').addClass("UnActive");
         }
         if (PageNum == AllPage - 1) {
             $('#dialogResultChoice .btnNextPage').removeClass("UnActive");
         }
     }
 })
 //20240715 -- ปุ่มดูเฉพาะข้อถูกบน Dialog เฉลย
 .on('click', '#btnRightMode', function (e, data) {
     GetAnswerChoicePanel(2);
 })
 //20240715 -- ปุ่มดูเฉพาะข้อผิดบน Dialog เฉลย
 .on('click', '#btnWrongMode', function (e, data) {
     GetAnswerChoicePanel(3);
 })
 //20240715 -- ปุ่มดูเฉพาะข้อข้ามบน Dialog เฉลย
 .on('click', '#btnLeapChoiceMode', function (e, data) {
     GetAnswerChoicePanel(4);
 })
 //20240715 -- ปุ่มดูข้อทั้งหมดบน Dialog เฉลย
 .on('click', '.AllAnswer', function (e, data) {
     GetAnswerChoicePanel(1);
 })

// ========================================================================================================== //

// ================================================ Function ================================================ //
//20240820 -- เพิ่มการแสดงปุ่มเล่นไฟล์เสียงที่คำตอบ
function GetQuestionAndAnswer(ActionType, QuestionNo) {
    var post1 = 'ActionType=' + ActionType + '&QuestionNo=' + QuestionNo;
    $.ajax({
        type: 'POST',
        url: '/weTest/GetQuestionAndAnswer',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {

                if (data[i].ItemStatus == 'sessionExpired') {
                    window.location = '/Wetest/User';
                    return false;
                }

                if (data[i].ItemType == 1) {
                    $('#divQuestion').html(data[i].Itemtxt);
                    $('#divQuestion').attr('Qid', data[i].ItemId);

                    if (data[i].multiname != null) {
                        $('.QName').append("<div class='multiQfileIcon'></div><div id='multiQuestion' PlayCount='1' class='ui-hide'></div>");
                        setbuttonAudioPlayer('multiQuestion', data[i].multipath);
                    }

                    if (data[i].multiSlowname != null) {
                        $('.QName').append("<div class='multiQfileSlowIcon ui-hide'></div><div id='multiSlowQuestion' PlayCount='1' class='ui-hide'></div>");
                        setbuttonAudioPlayer('multiSlowQuestion', data[i].multiSlowpath);
                    }
                    if (data[i].multitxt != null) {
                        $('.QName').append("<div class='multiQtxtIcon ui-hide'><div id='multiQtxt' class='ui-hide'>" + data[i].multitxt + "</div>");
                    }

                    if (data[i].ItemStatus == 'first') {
                        $('.btnBack').addClass("UnActive");
                        $('.btnNext').removeClass("UnActive");
                    }
                    if (data[i].ItemStatus == 'last') {
                        $('.btnBack').removeClass("UnActive");
                        $('.btnNext').addClass("UnActive");
                    }

                    if (data[i].ItemStatus == 'second') { $('.btnBack,.btnNext').removeClass("UnActive"); }
                    if (data[i].ItemStatus == 'beforelast') { $('.btnNext,.btnNext').removeClass("UnActive"); }
                } else {

                    if (i == 1) {
                        $('#divAnswer').html(data[data.length - 1].Itemtxt);
                    }

                    if (data[i].multiAnsname != null) {
                        $('.AName' + data[i].ItemId).append("<div class='multiAfileIcon' id='MA" + data[i].ItemId + "' MID='" + data[i].ItemId + "' PlayCount='1'></div><div id='multiAnswer" + data[i].ItemId + "' class='ui-hide'></div>");
                        setbuttonAudioPlayer('multiAnswer' + data[i].ItemId, data[i].multiAnspath);
                    }

                    if (data[i].multiAnsSlowname != null) {
                        $('.AName' + data[i].ItemId).append("<div class='multiAfileSlowIcon ui-hide' id='MAS" + data[i].ItemId + "' MID='" + data[i].ItemId + "' PlayCount='1'></div><div id='multiAnswerSlow" + data[i].ItemId + "' class='ui-hide'></div>");
                        setbuttonAudioPlayer('multiAnswerSlow', data[i].multiAnsSlowpath);
                    }
                    if (data[i].multiAnstxt != null) {
                        $('.AName' + data[i].ItemId).append("<div class='multiAtxtIcon ui-hide' id='MATIcon" + data[i].ItemId + "'><div id='MAT" + data[i].ItemId + "'  class='ui-hide'>" + data[i].multiAnstxt + "</div>");
                    }
                }
            }
            setProgressbar();
        }
    });
}
function QuizTimer() {
    function pad(val) { return val > 9 ? val : "0" + val; }
    counterId = setInterval(function () {
        $("#seconds").html(pad(++sec % 60));
        $("#minutes").html(pad(parseInt(sec / 60, 10)));
    }, 1000);
}
function setProgressbar() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetProgressbarStatus',
        success: function (data) {

            for (var i = 0; i < data.length; i++) {

                if (data[i].Result == 'success') {
                    $(".runningStatus").css("width", data[i].AnsweredPercent + '%');
                    $(".runningAmount").html(data[i].AnsweredAmount);

                    if (data[i].AnsweredPercent == '100') {
                        $(".runningStatus").css("border-radius", "1em 1em 1em 1em");
                    }
                }
            }
        }
    });
}
function GetLeapChoicePanel(ChoiceMode) {
    //ChoiceMode 1 : ปกติ , ChoiceMode 2 : เฉพาะข้อข้าม
    var post1 = 'ChoiceMode=' + ChoiceMode
    $.ajax({
        type: 'POST',
        url: '/weTest/GetLeapChoicePanel',
        data: post1,
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                //20240712 -- แก้ปัญหากดแล้วเจอหน้าว่าง check UnActive ปุ่ม next back
                if (data[i].result == 'success') {
                    AllPage = data[i].allPage;
                    PageNum = 1;
                    $('.btnBackPage').addClass("UnActive");

                    if (PageNum == AllPage) {
                        $('.btnNextPage').addClass("UnActive");
                    } else {
                        $('.btnNextPage').removeClass("UnActive");
                    }
                    $('#AllPage').html(data[i].leapChoicetxt)
                    $('#dialogLeapChoice').attr('action', 'focus');
                    popupOpen($('#dialogLeapChoice'), 99999);
                    PageNum = 1;
                }
            }
        }
    });
}
//20240726 -- เพิ่มการตั้งค่าไม่ให้เล่นไฟล์ซ้ำ
function setbuttonAudioPlayer(divname, FilePath) {

    $('#' + divname).buttonAudioPlayer({
        type: 'default',
        loop: false,
        src: FilePath
    });
}
//20240715 -- ดึงข้อมูลสร้าง Dialog เฉลย
function GetAnswerChoicePanel(ChoiceMode) {
    //ChoiceMode 1 : ทั้งหมด , ChoiceMode 2 : ข้อถูก, ChoiceMode 3 : ข้อผิด, ChoiceMode 4 : ข้อข้าม
    var post1 = 'ChoiceMode=' + ChoiceMode
    $.ajax({
        type: 'POST',
        url: '/weTest/GetAnswerChoicePanel',
        data: post1,
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                if (data[i].result == 'success') {
                    AllPage = data[i].allPage;
                    PageNum = 1;
                    $('.btnBackPage').addClass("UnActive");

                    if (PageNum == AllPage) {
                        $('.btnNextPage').addClass("UnActive");
                    } else {
                        $('.btnNextPage').removeClass("UnActive");
                    }
                    $('#AllPage2').html(data[i].AnswerChoicetxt)
                    $('#RightAmount').html(data[i].RightAmount)
                    $('#WrongAmount').html(data[i].WrongAmount)
                    $('#LeapAmount').html(data[i].LeapAmount)
                    popupClose($('#dialogResultChoice').closest('.my-popup'));
                    $('#dialogResultChoice').attr('action', 'focus');
                    popupOpen($('#dialogResultChoice'), 99999);
                    PageNum = 1;
                }
            }
        }
    });
}
//20240722 -- ตรวจสอบว่าเป็นการกดดูเฉลยหรือไม่
//20240903 -- มาจากเฉลยให้เป็นปุ่ม Exit
//20240905 -- มาจากเฉลยไม่แสดงปุ่ม Pause
function checkAnsweredFromReport() {
    $.ajax({
        type: 'POST',
        url: '/weTest/checkAnsweredFromReport',
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'showanswer') {
                    $('#divRunningBar, #divAllLeapChoice, #divTime,#divPause').addClass("ui-hide");
                    $('#divAllQuestion, #divShowExplain').removeClass("ui-hide");
                    GetQuestionAndAnswer('select', 1);
                    $('#divSendQuiz').removeClass('SendQuiz').addClass('FinishQuiz');

                } else {
                    GetQuestionAndAnswer()
                    QuizTimer()
                    PageNum = 1
                }
            }
        }
    });
}
//20240730 -- ปรับ Logo ที่ Menu Bar ตามชุดข้อสอบที่กำลังทำ
function SetLogo() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetQuizLogo',
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                if (data[i].resultType == 'success') {
                    $('#divIcon').html(data[i].QuizMode)
                    $('#Icontxt').html(data[i].QuizName)
                }
            }
        }
    });
}
//20240814 -- ดึงจำนวนครั้งในการเล่นไฟล์เสียง
//20240913 -- ปรับให้ดึง Config ต่างๆ ที่ต้องใช้ตอนเริ่ม Quiz
function GetQuizConfigVal() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetQuizConfigVal',
        success: function (data) {

            for (var i = 0; i < data.length; i++) {

                if (data[i].Result == 'success') {
                    MultiFileAmount = data[i].MultiAmount;
                    MultiFileSlowAmount = data[i].MultiSlowAmount;

                    if (data[i].IsTest == 0) {
                        $('#divShowExplain').addClass('ui-hide');
                    }
                } else {
                    console.log(data[i].ResultTxt)
                }
            }
        }
    });
}
//20240823 -- ตรวจสอบเวลาเริ่มต้น Case Refresh Page แล้วเวลากลับไปเริ่มนับใหม่
function GetStartTime() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetStartTime',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].dataType == 'success') {
                    sec = data[i].errorMsg;
                }
            }
        }
    });
}
//20240830 -- Delay การกดเลื่อนข้อ
function DelayChangeExam() {
    $('.btnNext, .btnBack').addClass('wait');

    var x = setInterval(function () {
        clearInterval(x);
        $('.btnNext, .btnBack').removeClass('wait');
    }, 2000);
}


// ========================================================================================================== //

window.addEventListener("orientationchange", function () {
    location.reload();
});

