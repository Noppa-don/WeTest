<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../content/Wetest/Wetest.css?ver=1.1.4" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/Practice.css?ver=1.1.4" rel="stylesheet" type="text/css" />
    <script src="../scripts/Wetest/Practice.js?ver=1.1.4" type="text/javascript"></script>
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
                <div id="ReadingLesson" class="flexDiv">
                </div>
                <div id="ReadingOther" class="otherButton"></div>
            </div>
            <div id="skillListen" class="flexDiv skilldiv">
                <div id="iconListen"></div>
                <div id="ListeningLesson" class="flexDiv">
                </div>
                <div id="ListeningOther" class="otherButton"></div>
            </div>
            <div id="skillVocab" class="flexDiv skilldiv">
                <div id="iconVocab"></div>
                <div id="VocabularyLesson" class="flexDiv">
                </div>
                <div id="VocabularyOther" class="otherButton"></div>
            </div>
            <div id="skillGrammar" class="flexDiv skilldiv">
                <div id="iconGrammar"></div>
                <div id="GrammarLesson" class="flexDiv">
                </div>
                <div id="GrammarOther" class="otherButton"></div>
            </div>
            <div id="skillSituation" class="flexDiv skilldiv">
                <div id="iconSituation"></div>
                <div id="SituationLesson" class="flexDiv">
                </div>
                <div id="SituationOther" class="otherButton"></div>
            </div>
        </div>

        <div id="RandomType" class="ui-hide">
            <div class="flexDiv">
                Number of exams
             <div class="btn ui-Left" id="btn20">20</div>
                <div class="btn ui-Left" id="btn30">30</div>
                <div class="btn ui-Left" id="btn50">50</div>
                <div class="btn ui-Left" id="btn100">100</div>
                <div class="btn ui-Left" id="btnUserType"></div>
            </div>
        </div>
        <div id="skillRandom" class="ui-hide">
            <div class="flexDiv">
                skills
             <div class="btn ui-Left" id="btnRandomAll">All</div>
                <div class="ui-Left" id="btnRandomReading"></div>
                <div class="ui-Left" id="btnRandomListening"></div>
                <div class="ui-Left" id="btnRandomVocab"></div>
                <div class="ui-Left" id="btnRandomGrammar"></div>
                 <div class="ui-Left" id="btnRandomSituation"></div>
            </div>
        </div>
    </div>

    <div class='wrapper'>
        <div id="AllReading" class="ui-hide"></div>
        <div id="AllListening" class="ui-hide"></div>
        <div id="AllGrammar" class="ui-hide"></div>
        <div id="AllSituation" class="ui-hide"></div>
        <div id="AllVocabulary" class="ui-hide"></div>
    </div>

    <div class="footer ui-hide">
        <div class='footerButton footerAlldiv'>
            <div class="Imagebtn btnBack"></div>
        </div>
    </div>

    <div name='select' id="dialogSelect" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon done'></div>
            <div class='ui-text'></div>
            <div class="ui-twoButton">
                <div class='btn btnCancel'>No</div>
                <div class='btn btnSelected'>Yes</div>
            </div>
        </div>
    </div>
</asp:Content>
