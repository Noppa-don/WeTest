var OTPNum
// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); $('#Firstname').focus(); });

SaveNextAnswerAndGetQuestion()

// ============================================================ //
// ======================= Object Event ======================= //
$(document)
// ======================= Activity ======================= //


 .on('click', '.btnNext', function (e, data) {
     console.log('#btnNext');
     SaveNextAnswerAndGetQuestion();
 })

// ============================================================ //

// ========================= Function ========================= //

function checkInvalidRegisterData() {

    var CheckError = 'true';

    if ($('#Firstname').val() == '') { $('#Firstname').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#Surname').val() == '') { $('#Surname').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#MobileNo').val() == '' || $('#MobileNo').val().length != 10) { $('#MobileNo').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#EMail').val() == '') { $('#EMail').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#Username').val() == '') { $('#Username').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#Password').val() == '') { $('#Password').addClass("InvalidData"); CheckError = 'false'; }
    if ($('#ConfirmPassword').val() == '') { $('#ConfirmPassword').addClass("InvalidData"); CheckError = 'false'; }

    if ($('#ConfirmPassword').val() != $('#Password').val()) {
        $('#Password, #ConfirmPassword').addClass("InvalidData");
        CheckError = 'false';
    }

    if ($('#btnStudent, #btnOther').hasClass("btnSelected")) { } else {
        $('#btnStudent, #btnOther').addClass("InvalidData");
        CheckError = 'false';
    }

    return CheckError
}

function SaveNewUser() {
    var post1 = 'FirstName=' + $('.spFirstname').text() + '&Surname=' + $('.spSurname').text() + '&MobileNo=' + $('.spMobileNo').text() +
        '&EMail=' + $('.spEMail').text() + '&Username=' + $('.spUsername').text() + '&Password=' + $('#Password').val() + '&StudentType=' + StudentType;
    $.ajax({
        type: 'POST',
        url: '/weTest/SaveUser',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                switch (data[i].dataType) {
                    case 'error':
                        console.log(data[i].errorMsg);
                    case 'success':
                        UploadStudentPhoto();
                }
            }
        }
    });
}

function SaveNextAnswerAndGetQuestion() {
    $.ajax({
        type: 'POST',
        url: '/weTest/SaveNextAnswerAndGetQuestion',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].ItemType == 1) {
                    $('#divQuestion').html(data[i].ItemNo + '. ' + data[i].Itemtxt);
                    $('#divQuestion').attr('Qid', data[i].ItemId);
                } else {
                    //console.log('A : no ' + data[i].ItemNo + ' id ' + data[i].ItemId + ' txt ' + data[i].Itemtxt);
                    $('#divAnswer').html(data[i].Itemtxt);
                }
            }
        }
    });
}
