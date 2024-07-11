-- tblbook
TRUNCATE TABLE WeTest_dev.dbo.tblBook
INSERT INTO WeTest_dev.dbo.tblBook(BookId,BookGroupId,LevelId,GroupSubjectId,BookName,BookSyllabus,IsActive,LastUpdate)
SELECT b.Book_Id,b.BookGroup_Id,b.Level_Id,b.GroupSubject_Id,b.Book_Name,b.Book_Syllabus,b.IsActive,b.LastUpdate
FROM WeTest.dbo.tblBook b where b.Book_Syllabus = '51' and b.GroupSubject_Id = 'FB677859-87DA-4D8D-A61E-8A76566D69D8'


-- tblquestioncategory
TRUNCATE TABLE WeTest_dev.dbo.tblQuestionCategory
INSERT INTO WeTest_dev.dbo.tblQuestionCategory(QCategoryId,BookGroupId,QCategoryNo,QCategoryName,IsActive,LastUpdate)
SELECT qcat.QCategory_Id, Book_Id,QCategory_No,QCategory_Name, qcat.IsActive, qcat.LastUpdate
FROM WeTest.dbo.tblQuestionCategory qcat inner join WeTest_dev.dbo.tblBook bb on bb.BookGroupId = qcat.Book_Id


-- tblquestionset
TRUNCATE TABLE WeTest_dev.dbo.tblQuestionSet
INSERT INTO WeTest_dev.dbo.tblQuestionSet(QsetId,QcategoryId,QSetNo,QSetName,QSetType,IsRandomQuestion,IsRandomAnswer,QSetName_Quiz,IsActive,LastUpdate)
SELECT QSet_Id ,qs.QCategory_Id , QSet_No , QSet_Name, Qset_type , QSet_IsRandomQuestion ,QSet_IsRandomAnswer , QSet_Name_Quiz,qs.IsActive, qs.LastUpdate
FROM WeTest.dbo.tblQuestionSet qs inner join WeTest_dev.dbo.tblQuestionCategory qc on qs.QCategory_Id = qc.QCategoryId


---- tblquestion
TRUNCATE TABLE WeTest_dev.dbo.tblQuestion
SET IDENTITY_INSERT WeTest_dev.dbo.tblQuestion ON
INSERT INTO WeTest_dev.dbo.tblQuestion (QuestionId,QId,QSetId,QuestionNo,QuestionName,QuestionExpain,QuestionName_Quiz,QuestionExpain_Quiz,IsActive,LastUpdate)
SELECT qus.Question_Id,QId,qus.QSet_Id,Question_No,Question_Name,Question_Expain,Question_Name_Quiz,Question_Expain_Quiz,qus.IsActive,qus.LastUpdate
FROM WeTest.dbo.tblQuestion qus
inner join WeTest.dbo.tblLayoutConfirmed lay on lay.Question_Id = qus.Question_Id inner join WeTest_dev.dbo.tblQuestionset qs on qus.QSet_Id = qs.QsetId
where  lay.QuizTechnicalConfirmed = '1' and lay.QuizEditConfirmed = '1' and QId is not null 
SET IDENTITY_INSERT WeTest.dbo.tblQuestion OFF

-- tblAnswer
TRUNCATE TABLE WeTest_dev.dbo.tblAnswer
INSERT INTO WeTest_dev.dbo.tblAnswer(AnswerId,QuestionId,QSetId,AnswerNo,AnswerName,AnswerExpain,AnswerScore,AnswerScoreMinus,AlwaysShowInLastRow,
AnswerNameQuiz,AnswerExpainQuiz,IsActive,LastUpdate)
SELECT Answer_Id , ans.Question_Id , ans.QSet_Id , Answer_No , Answer_Name, Answer_Expain ,Answer_Score, Answer_ScoreMinus
 , AlwaysShowInLastRow , Answer_Name_Quiz ,Answer_Expain_Quiz , ans.IsActive ,ans.LastUpdate
FROM WeTest.dbo.tblAnswer ans inner join WeTest_dev.dbo.tblQuestion q on ans.Question_Id = q.QuestionId

-- tblTestset
TRUNCATE TABLE WeTest_dev.dbo.tbltestset
INSERT INTO WeTest_dev.dbo.tbltestset(TestsetId,TestSetName,LevelId,IsPlacementTest,IsPractice,IsHomework,IsExam,CreateId,IsActive,LastUpdate)
SELECT TestSet_Id,TestSet_Name,Level_Id,case when (IsQuizMode = 1 and IsStandard = 0) then 1 else 0 end,IsPracticeMode,IsHomeWorkMode,
case when (IsQuizMode = 1 and IsStandard = 1) then 1 else 0 end
,UserId,IsActive,LastUpdate
FROM WeTest.dbo.tbltestset t

-- tblTestsetQuestionset
TRUNCATE TABLE WeTest_dev.dbo.tblTestsetQuestionset
INSERT INTO WeTest_dev.dbo.tblTestsetQuestionset(tsqsid,testsetId,tsqsno,qsetid,LevelId,IsActive,LastUpdate)
SELECT TSQS_Id,TestSet_Id,TSQS_No,qset_Id,level_Id,IsActive,LastUpdate
FROM WeTest.dbo.tblTestsetQuestionset ts

-- tblTestsetQuestionDetail
TRUNCATE TABLE WeTest_dev.dbo.tblTestsetQuestionDetail
INSERT INTO WeTest_dev.dbo.tblTestsetQuestionDetail(TSQDId,TSQSId,TSQDNo,QuestionId,IsActive,LastUpdate)
SELECT TSQd_id,TSQS_Id,q.Question_No,td.Question_Id,td.IsActive,td.LastUpdate
FROM WeTest.dbo.tblTestsetQuestionDetail td inner join WeTest.dbo.tblquestion q on td.Question_Id = q.Question_Id

-- tblMultimediaObject
TRUNCATE TABLE WeTest_dev.dbo.tblMultimediaObject
INSERT INTO WeTest_dev.dbo.tblMultimediaObject(MultimediaObjId,QSetId,MFileName,MFileType,ReferenceId,ReferenceType,IsActive,LastUpdate)
SELECT MultimediaObjId,QSetId,MFileName,MFileType,ReferenceId,ReferenceType,IsActive,LastUpdate
FROM WeTest.dbo.tblMultimediaObject