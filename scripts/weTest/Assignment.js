var ExamAmount, skill
// ========================= Page Load ======================== //
$(function () { $('div[data-role=page]').page({ theme: 'c', }); });
GetAssignment();
// ============================================================ //
// ======================= Object Event ======================= //
$(document)
// ======================= Activity ======================= //
.on('click', '.btnBack', function (e, data) {
    window.location = '/Wetest/User';
})
.on('click', '.redAssignment,.orangeAssignment,.greenAssignment', function (e, data) {
    var TestsetId = $(this).attr('refId');
    var TestsetName = $(this).attr('refName');
    GotoAssignment(TestsetId, TestsetName);
})

GotoAssignment

//20240813 -- Get Assignment
function GetAssignment() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetAssignment',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'success') {
                    $('#OverdueItem').html(data[i].OverDue);
                    $('#TodayItem').html(data[i].Today);
                    $('#ThisWeekItem').html(data[i].ThisWeek);
                    $('#NextWeekItem').html(data[i].NextWeek);
                }

            }
        }
    });
}
//20240814 -- Go to Assignment
function GotoAssignment(TestsetId, TestsetName) {
    console.log(TestsetId);
    console.log(TestsetName);
    var post1 = 'TestsetId=' + TestsetId + '&TestsetName=' + TestsetName;
    $.ajax({
        type: 'POST',
        url: '/weTest/CreateAssignment',
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



