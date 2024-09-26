var ExamAmount, skill
// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); });
CheckLoginStatus();
// ============================================================ //
// ======================= Object Event ======================= //
$(document)
// ======================= Activity ======================= //
 .on('click', '#btnLesson', function (e, data) {
     $('#btnLesson').addClass('btnSelected');
     $('#btnRandom').removeClass('btnSelected');

     $('#LessonType').removeClass("ui-hide");
     $('#RandomType,#skillRandom,.btnStart').addClass("ui-hide");
     GetLevel();
 })
 .on('click', '#btnRandom', function (e, data) {
     $('#btnLesson').removeClass('btnSelected');
     $('#btnRandom').addClass('btnSelected');

     $('#LessonType, #ChooseLevel, #Lessondivcon').addClass("ui-hide");
     $('#RandomType ,#skillRandom,.btnStart').removeClass("ui-hide");
     GetSkill();
 })
 .on('click', '#ReadingOther', function (e, data) {

     $('#PracticeType,#LessonType,.AllPSkill').addClass('ui-hide');
     $('#AllReading,.footer,.AllPracticeSet').removeClass('ui-hide');

 })
 .on('click', '#ListeningOther', function (e, data) {

     $('#PracticeType,#LessonType,.AllPSkill').addClass('ui-hide');
     $('#AllListening,.footer,.AllPracticeSet').removeClass('ui-hide');
 })
 .on('click', '#GrammarOther', function (e, data) {

     $('#PracticeType,#LessonType,.AllPSkill').addClass('ui-hide');
     $('#AllGrammar,.footer,.AllPracticeSet').removeClass('ui-hide');
 })
 .on('click', '#SituationOther', function (e, data) {

     $('#PracticeType,#LessonType,.AllPSkill').addClass('ui-hide');
     $('#AllSituation,.footer,.AllPracticeSet').removeClass('ui-hide');
 })
 .on('click', '#VocabularyOther', function (e, data) {

     $('#PracticeType,#LessonType,.AllPSkill').addClass('ui-hide');
     $('#AllVocabulary,.footer,.AllPracticeSet').removeClass('ui-hide');
 })
 .on('click', '.btnBack', function (e, data) {
     if ($('.AllPracticeSet').hasClass('ui-hide')) {
         window.location = '/Wetest/User';
     } else {
         $('.AllPracticeSet').addClass('ui-hide');
         $('#PracticeType,#LessonType').removeClass("ui-hide");
     }

 })
 .on('click', '.Lessondiv', function (e, data) {

     var TestsetName = $(this).text();
     var TestsetId = $(this).attr('id');
     $('#dialogSelect').attr('action', 'focus');
     $('#dialogSelect .ui-text').html('Do you want to start practice ' + TestsetName + ' now!');
     $('#dialogSelect .btnSelected').attr('TestsetId', TestsetId);
     $('#dialogSelect  .btnSelected').attr('TestsetName', TestsetName);
     popupOpen($('#dialogSelect'), 99999);
 })
 .on('click', '.ui-icon.close ', function (e, data) {
     popupClose($(this).closest('.my-popup'));
 })
 .on('click', '#dialogSelect .btnSelected', function (e, data) {
     popupClose($(this).closest('.my-popup'));
     GotoPractice($(this).attr('TestsetId'), $(this).attr('TestsetName'));
 })
 .on('click', '#dialogSelect .btnCancel', function (e, data) {
     popupClose($(this).closest('.my-popup'));
 })
 //20240717 -- Choose Exam Amount
 .on('click', '.btnAmount', function (e, data) {
     $('.btnAmount').removeClass('btnSelected');
     $('#UserType').val('');
     $(this).addClass('btnSelected');
 })
 //20240717 -- Check numeric key press
 .on('keypress', '#UserType', function (e, data) {
     if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
         event.preventDefault(); //stop character from entering input
     }
 })
//20240717 -- Choose Random All Skill
//20240916 -- ปรับการ toggle class ให้แสดงผลถูกต้อง
 .on('click', '#btnRandomAll', function (e, data) {
     $('#btnRandomAll').toggleClass('btnSelected');
     if ($('#btnRandomAll').hasClass('btnSelected')) {
         $('.btnSkill').addClass('Selected');
     } else {
         $('.btnSkill').removeClass('Selected');
     }
 })
//20240717 -- Choose Random Skill
//20240916 -- ปรับการ toggle class ให้แสดงผลถูกต้อง
 .on('click', '.btnSkill', function (e, data) {
     $(this).toggleClass('Selected');
     var numItems = $('.Selected').length
     if (numItems == 5) {
         $('#btnRandomAll').addClass('btnSelected');
     } else {
         $('#btnRandomAll').removeClass('btnSelected');
     }
 })
//20240717 -- Start Random
//20240917 -- ตรวจสอบการเลือกจำนวนข้อ,skill และแจ้งเตือน
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

     if (ExamAmount == '' && (skill == '' || skill == 'btnRandomAll')) {
         $('#dialogAlert').attr('action', 'focus');
         $('#dialogAlert .ui-text').html('Please choose your skill and amount of practice');
         popupOpen($('#dialogAlert'), 99999);
         return 0;
     } else if (ExamAmount == '' || ExamAmount == '0') {
         $('#dialogAlert').attr('action', 'focus');
         $('#dialogAlert .ui-text').html('Please fill amount of practice');
         popupOpen($('#dialogAlert'), 99999);
         return 0;
     } else if (skill == '' || skill == 'btnRandomAll') {
         $('#dialogAlert').attr('action', 'focus');
         $('#dialogAlert .ui-text').html('Please choose your skill');
         popupOpen($('#dialogAlert'), 99999);
         return 0;
     } else {

         var post1 = 'ExamAmount=' + ExamAmount + '&arrSkill=' + skill;
         $.ajax({
             type: 'POST',
             url: '/weTest/RandomPractice',
             data: post1,
             success: function (data) {
                 for (var i = 0; i < data.length; i++) {
                     if (data[i].dataType == 'success') {
                         console.log(data[i].errorMsg);
                         GotoPractice(data[i].errorMsg, 'RandomPractice');
                     }
                 }
             }
         });
     }
 })
//20240730 -- Select Dropdown
 .on('change', 'select', function (e, data) {
     var post1 = 'LevelId=' + this.value;
     $.ajax({
         type: 'POST',
         url: '/weTest/GetLesson',
         data: post1,
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

                     $('#Lessondivcon').removeClass('ui-hide');
                 }
             }
         }
     });
 })
//20240917 -- Close Alert
 .on('click', '#dialogAlert .btnOK', function (e, data) {
     popupClose($(this).closest('.my-popup'));
 })

function GotoPractice(TestsetId, TestsetName) {

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
//20240730 -- สร้าง Dropdown สำหรับเลือกระดับชั้นที่ต้องการให้แสดงชุดข้อสอบ
//20240805 -- ปรับการแสดงผล
//20240814 -- ปรับการแสดงผล
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
                    BindPracticeItem(LevelId)
                } else {

                    for (var i = 0; i < data.length; i++) {

                        BindPracticeItem(data[i].LevelId)
                    }
                }

            }
        }
    });
}
//20240814 -- ย้าย Function สร้างชุดข้อสอบฝึกฝน
function BindPracticeItem(LevelId) {
    var post1 = 'LevelId=' + LevelId;
    $.ajax({
        type: 'POST',
        url: '/weTest/GetLesson',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].skillSet == 'error') {
                } else {

                    $('#' + data[i].skillSet + 'Lesson').html(data[i].skillTxtShort);
                    $('#skill' + data[i].skillSet).removeClass('ui-hide');

                    if (data[i].skillAmount < 6) {
                        $('#' + data[i].skillSet + 'Other').addClass('ui-hide');
                    } else {
                        $('#' + data[i].skillSet + 'Other').removeClass('ui-hide');
                        $('#All' + data[i].skillSet).html('');
                        $('#All' + data[i].skillSet).html(data[i].skillTxtAll);

                    }

                    $('#Lessondivcon').removeClass('ui-hide');
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
                    $('.btn' + data[i].skillSet).removeClass('ui-hide');
                }
            }
        }
    });
}






