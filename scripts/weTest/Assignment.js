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

//20240813 -- Get Assignment
function GetAssignment() {
    $.ajax({
        type: 'POST',
        url: '/weTest/GetAssignment',
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Result == 'success') {
                    console.log(data[i].OverDue);
                    $('#OverdueItem').html(data[i].OverDue);
                    $('#TodayItem').html(data[i].Today);
                    $('#ThisWeekItem').html(data[i].ThisWeek);
                    $('#NextWeekItem').html(data[i].NextWeek);
                }

            }
        }
    });
}



