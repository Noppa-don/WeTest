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
 })
 .on('click', '#btnRandom', function (e, data) {
     $('#btnLesson').removeClass('btnSelected');
     $('#btnRandom').addClass('btnSelected');
 })
 .on('click', '.btnBack', function (e, data) {
     window.location = '/Wetest/User';
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
