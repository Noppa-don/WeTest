<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../content/Wetest/Wetest.css?ver=1.1.2" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/Practice.css?ver=1.1.2" rel="stylesheet" type="text/css" />
    <script src="../scripts/Wetest/Practice.js?ver=1.1.2" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner">
        <div class='logo'></div>
        <div class='pagename'></div>
    </div>

    <div class='wrapper'>

        <div id="PracticeType" class="flexDiv">
            choose from
             <div class="btn ui-Left" id="btnLesson">Lesson</div>
            <div class="btn ui-Left" id="btnRandom">Random</div>
        </div>
        <div id="LessonType" class="ui-hide">
            <div id="skillReading" class="flexDiv skilldiv">
                <div id="iconReading"></div>
                <div id="ReadingLesson class="flexDiv">
                    <div class="Lessondiv LessonR">1</div> 
                    <div class="Lessondiv LessonR">2</div> 
                    <div class="Lessondiv LessonR">3</div> 
                    <div class="Lessondiv LessonR">4</div> 
                    <div class="Lessondiv LessonR">5</div>
                </div>
                <div id="otherReading" class="otherButton"></div>
            </div>
           
            <div id="skillListen" class="flexDiv skilldiv">
                <div id="iconListen"></div>
                <div id="ListenLesson class="flexDiv"> 
                    <div class="Lessondiv LessonL">1</div> 
                    <div class="Lessondiv LessonL">2</div> 
                    <div class="Lessondiv LessonL">3</div> 
                    <div class="Lessondiv LessonL">4</div> 
                    <div class="Lessondiv LessonL">5</div>
                </div>
                <div id="otherListen" class="otherButton"></div>
            </div>
            <div id="skillVocab" class="flexDiv skilldiv">
                <div id="iconVocab"></div> 
                <div id="VocabLesson class="flexDiv"> 
                    <div class="Lessondiv LessonV">1</div> 
                    <div class="Lessondiv LessonV">2</div> 
                    <div class="Lessondiv LessonV">3</div> 
                    <div class="Lessondiv LessonV">4</div> 
                    <div class="Lessondiv LessonV">5</div>
                </div>
                <div id="otherVocab" class="otherButton"></div>
            </div>
            <div id="skillGramma" class="flexDiv skilldiv">
                <div id="iconGramma"></div> 
                <div id="GrammaLesson class="flexDiv"> 
                    <div class="Lessondiv LessonG">1</div> 
                    <div class="Lessondiv LessonG">2</div> 
                    <div class="Lessondiv LessonG">3</div> 
                    <div class="Lessondiv LessonG">4</div> 
                    <div class="Lessondiv LessonG">5</div>
                </div>
                <div id="otherGramma" class="otherButton"></div>
            </div>
            <div id="skillSituation" class="flexDiv skilldiv">
                <div id="iconSituation"></div>
                <div id="SituationLesson class="flexDiv">  
                    <div class="Lessondiv LessonS">1</div> 
                    <div class="Lessondiv LessonS">2</div> 
                    <div class="Lessondiv LessonS">3</div> 
                    <div class="Lessondiv LessonS">4</div> 
                    <div class="Lessondiv LessonS">5</div>
                </div>
                <div id="otherSituation" class="otherButton"></div>
            </div>


        </div>
    </div>


</asp:Content>
