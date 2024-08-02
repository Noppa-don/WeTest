var OTPNum,counterId, PageNum, AllPage;

var sec = 0;
// ========================= Page Load ======================== //
$(function () {
    $('div[data-role=page]').page({ theme: 'c', });
    checkAnsweredFromReport();
});

// ============================================================ //
// ============================================== Object Event ============================================== //
$(document)
// ======================= Activity =========================== //

 .on('focus', '.txtDetail,.ui-select', function (e, data) { $(this).removeClass("InvalidData") })
 //20240726 -- เพิ่มการกดหยุดไฟล์เสียง
 .on('click', '.btnNext', function (e, data) {
     if ($('.bap-icon').children().hasClass('bap-icon-on')) { $('.bap-btn').click()}
    
     if ($('.btnNext').hasClass('UnActive') == false) {
         GetQuestionAndAnswer('next', '');
         setProgressbar();
     }
 })
//20240726 -- เพิ่มการกดหยุดไฟล์เสียง
 .on('click', '.btnBack', function (e, data) {
     if ($('.bap-icon').children().hasClass('bap-icon-on')) { $('.bap-btn').click()}

     if ($('.btnBack').hasClass('UnActive') == false) {
         GetQuestionAndAnswer('back', '');
         setProgressbar();
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
 .on('click', '#divSendQuiz', function (e, data) {
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
                     $('#divRunningBar, #divAllLeapChoice, #divTime').addClass("ui-hide");
                     $('#divAllQuestion, #divShowExplain').removeClass("ui-hide");
                     GetQuestionAndAnswer('select', 1);
                 } else if (data[i].dataType == 'gotoreport') {
                     window.location = '/Wetest/Report';
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
     console.log($('.ExplainQ').hasClass('ui-hide'));
     if ($('.ExplainQ').hasClass('ui-hide') == false) {
         $('.ExplainQ').addClass("ui-hide");
     } else {
         $('.ExplainQ').removeClass("ui-hide");
     }
 })
 .on('click', '#divGotoMainMenu', function (e, data) {
     window.location = '/Wetest/User';
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
 //20240801 -- กดรูปภาพแล้วขยาย
.on('click', '#divQuestionAndAnswer img', function (e, data) {
    var img = $(this);
    var bigImg = $('<img />').css({ 'max-width': '80%', 'max-height': '80%', 'display': 'inline', 'margin-top': '10px' });
    bigImg.attr({ src: img.attr('src'), alt: img.attr('alt'), title: img.attr('title') });

    var over = $('<div />').text(' ').css({ 'height': '100%', 'width': '100%', 'background': 'rgba(0,0,0,.82)', 'position': 'fixed', 'top': 0, 'left': 0,
        'opacity': 0.0, 'cursor': 'pointer', 'z-index': 9999, 'text-align': 'center'
    }).append(bigImg).bind('click', function () {
        $(this).fadeOut(300, function () {
            $(this).remove();
        });
    }).insertAfter(this).animate({
        'opacity': 1
    }, 300);
});
// ========================================================================================================== //

// ================================================ Function ================================================ //

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
                }

                if (data[i].ItemType == 1) {
                    $('#divQuestion').html(data[i].Itemtxt);
                    $('#divQuestion').attr('Qid', data[i].ItemId);

                    if (data[i].multiname != null) {
                        $('#divQuestion').append("<br><br><div id='" + data[i].multiname + "'></div>");
                        setbuttonAudioPlayer(data[i].multiname, data[i].multipath);
                    }

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

                    if (data[i].errorMsg == '100') {
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
        loop:false,
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
function checkAnsweredFromReport() {
    $.ajax({
        type: 'POST',
        url: '/weTest/checkAnsweredFromReport',
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                console.log(data[i].dataType);
                if (data[i].dataType == 'showanswer') {
                    $('#divRunningBar, #divAllLeapChoice, #divTime').addClass("ui-hide");
                    $('#divAllQuestion, #divShowExplain').removeClass("ui-hide");
                    GetQuestionAndAnswer('select', 1);
                } else {
                    GetQuestionAndAnswer()
                    QuizTimer()
                    setProgressbar();
                    PageNum = 1
                }
                SetLogo();
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

// ========================================================================================================== //

