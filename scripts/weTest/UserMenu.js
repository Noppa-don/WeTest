// ========================= Page Load ====================================================================== //
$(function () {
    $('div[data-role=page]').page({ theme: 'c', });
    CheckLoginStatus();
});

// ========================================================================================================== //

// ============================================== Object Event ============================================== //
$(document)
    //20240923 -- ย้าย Function User Menu มารวมที่เดียวกัน
   .on('click', '.UserNameandLevel,.UserPhoto', function (e, data) {
        $('.UserMenu').toggleClass('ui-hide');
   })

   .on('click', '.btnAccountMenu.Logout', function (e, data) {
        $('#dialogLogout').attr('action', 'focus');
        popupOpen($('#dialogLogout'), 99999);
    })
   .on('click', '.btnConfirmLogout', function (e, data) {
        $.ajax({
            type: 'POST',
            url: '/weTest/Logout',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].Result == 'success') {
                        popupClose($('#dialogLogout').closest('.my-popup'));
                        window.location = '/Wetest/User';
                    }
                }
            }
        });
   })

   .on('click', '.btnAccountMenu.DeleteAccount', function (e, data) {
       $('#dialogDeleteAccount').attr('action', 'focus');
       popupOpen($('#dialogDeleteAccount'), 99999);
   })
   .on('click', '.btnConfirmDelete', function (e, data) {
       $.ajax({
           type: 'POST',
           url: '/weTest/DeleteAccount',
           success: function (data) {
               for (var i = 0; i < data.length; i++) {
                   if (data[i].Result == 'success') {
                       popupClose($('#dialogDeleteAccount').closest('.my-popup'));
                       $('.UserData,.UserMenu,.MainMenu,.Goal,.DetailGoal,.footer,.UserPhoto').addClass('ui-hide');
                       $('.login').removeClass('ui-hide');
                       $('#userName,#userPass').val('');
                   }
               }
           }
       });
   })

   .on('click', '.btnAccountMenu.EditAccount', function (e, data) {
       $.ajax({
           type: 'POST',
           url: '/weTest/SetEditUserMode',
           success: function (data) {
               for (var i = 0; i < data.length; i++) {
                   if (data[i].dataType == 'success') {
                       window.location = '/Wetest/Registration';
                   }
               }
           }
       });
   })

   .on('click', '.btnAccountMenu.Setting', function (e, data) {
       GetSettingItem();

   })

   .on('click', '.btnAccountMenu.RefillKey', function (e, data) {
        $.ajax({
            type: 'POST',
            url: '/weTest/SetRefillKeyMode',
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    if (data[i].dataType == 'success') {
                        window.location = '/Wetest/Registration';
                    }
                }
            }
        });
   })

   .on('click', '.btnSetting', function (e, data) {
        var IsCheck = $(this).is(":checked");
        var NotiId = $(this).attr('id');
        UpdateNoti(IsCheck, NotiId);
    })
   .on('click', '#dialogConfirm #btnOK ,#dialogSelect .btnCancel,#dialogLogout .btnCancel,#dialogDeleteAccount .btnCancel,#dialogRejectAlert #btnOKReject,.btnClose', function (e, data) {
        popupClose($(this).closest('.my-popup'));
    })

// ========================================================================================================== //

// ================================================ Function ================================================ //

function GetSettingItem() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetSettingItem',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'success') {
                    $('#NotiItem').html(data[i].ResultTxt);
                }
            }
            $('.UserMenu,.MainMenu,.Goal,.DetailGoal,.footerGoal,.Assignment').addClass('ui-hide');
            $('#PracticeType,#LessonType,#RandomType,#skillRandom,.footerAlldiv').addClass('ui-hide');
            $('.SearchReport,#skillRandom,.reportData,.footerAlldiv').addClass('ui-hide');

            $('.Noti,.footerSetting').removeClass('ui-hide');
        }
    });
}

function UpdateNoti(IsCheck, NotiId) {
    var post1 = 'IsCheck=' + IsCheck + '&NotiId=' + NotiId;
    $.ajax({
        type: 'POST',
        url: '/weTest/UpdateNoti',
        data: post1,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'notsetgoal') {
                    console.log('notsetgoal');
                    $('#' + NotiId).attr('checked', false);
                    $('#dialogAlert').attr('action', 'focus');
                    $('#dialogAlert .ui-text').html("You don't set any goal");
                    popupOpen($('#dialogAlert'), 99999);
                }
            }
        }
    });
}

function CheckLoginStatus() {
    $.ajax({
        type: 'POST',
        url: '/weTest/CheckLoginStatus',
        success: function (data) {
            SetUserData(data);
        }
    });
}

function SetUserData(data) {
    for (var i = 0; i < data.length; i++) {
        if (data[i].Result == 'sessionlost') {
            //ไม่มี Session StudentId 
            $('.login').removeClass('ui-hide');
            $('.MainMenu,.Goal,.Noti,.DetailGoal,.Assignment,.UserData').addClass('ui-hide');
        } else if (data[i].Result == 'not') {
            //ไม่เจอใน DB
            $('.login').removeClass('ui-hide');
            $('.MainMenu,.Goal,.Noti,.DetailGoal,.Assignment,.UserData').addClass('ui-hide');
        } else {
            $('.login').addClass('ui-hide');
            $('.UserData,.MainMenu').removeClass('ui-hide');
            $('.pagename').html('');
            //20240716 -- ดึงข้อมูล User เพิ่มเติม
            $('.UserNameandLevel').html('Welcome, ' + data[i].Firstname + '<br />' + data[i].UserLevel);
            $('.expiredDate').html(data[i].ExpiredDate)
            ExpiredDateAmount = data[i].ExpiredDateAmount;
            $('.UserData').append(data[i].UserPhoto);
            $('#UserLevel').html('Your Level : ' + data[i].UserLevel + '<br />');
            CheckGoalNoti();
            console.log(data[i].Result);
            if (data[i].Result == 'refill') {
                //หมดอายุ
                $('#btnGoalMenu ,#btnPracticeMenu,#btnMockUpExam,#btnReport,.Assignment').addClass('expired');
                $('#dialogMustPurchase').attr('action', 'focus');
                popupOpen($('#dialogMustPurchase'), 99999);
            } else if (data[i].Result == 'trial') {
                //ทำ Placement Test เสร็จ กลับมาใช้งาน
                $('#dialogPurchase').attr('action', 'focus');
                popupOpen($('#dialogPurchase'), 99999);
            }
        }
    }
}

//20240904 -- Check Goal Date Noti
function CheckGoalNoti() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetGoalNoti',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                console.log(data[i].Result);
                if (data[i].Result == 'noti' || data[i].Result == 'notset') {
                    $('#dialogAlert').attr('action', 'focus');
                    $('#dialogAlert .ui-text').html(data[i].Msg);
                    popupOpen($('#dialogAlert'), 99999);
                }

            }
        }
    });
}


