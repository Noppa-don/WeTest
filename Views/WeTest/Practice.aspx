<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../content/Wetest/Wetest.css?ver=1.1.6" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/Practice.css?ver=1.1.6" rel="stylesheet" type="text/css" />

    <script src="../scripts/Wetest/Practice.js?ver=1.1.6" type="text/javascript"></script>
    <script src="../scripts/Wetest/User.js?ver=1.1.6" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

        <div class="banner">
        <div class="AppData">
            <div class="logo"></div>
            <div class='pagename'></div>
        </div>
<div class="UserData flexDiv ui-hide">
            <div class="UserNameandLevel"></div>
        </div>
        <div class="UserMenu ui-hide">
            <div class="btnAccountMenu EditAccount flexDiv">
                <div class="Accountlogo firstflexdiv"></div>
                <div>Account</div>
            </div>
            <div class="btnAccountMenu RefillKey flexDiv">
                <div class="Accountlogo firstflexdiv"></div>
                <div>Refill Wetest Key</div>
            </div>
            <div class="btnAccountMenu Setting flexDiv">
                <div class="Accountlogo firstflexdiv"></div>
                <div>Setting</div>
            </div>
            <div class="btnAccountMenu Logout flexDiv">
                <div class="Logoutlogo firstflexdiv"></div>
                <div class="firstflexdiv">Log out</div>
            </div>
            <div class="btnAccountMenu DeleteAccount flexDiv">
                <div class="Deletelogo firstflexdiv"></div>
                <div>Delete Account</div>
            </div>
        </div>
    </div>

    <div class='wrapper'>

        <div id="PracticeType" class="flexDiv">
            choose from
             <div class="btn ui-Left" id="btnLesson">Lesson</div>
            <div class="btn ui-Left" id="btnRandom">Random</div>
        </div>

        <div id="LessonType">
            <div id="ChooseLevel" class="flexDiv ui-hide">
                choose Level
             <div id="SelectLevel"></div>
            </div>
            <div id="Lessondivcon" class="ui-hide">
                <div id="skillReading" class="flexDiv skilldiv">
                    <div class="iconReading"></div>
                    <div id="ReadingLesson" class="flexDiv">
                    </div>
                    <div id="ReadingOther" class="otherButton"></div>
                </div>
                <div id="skillListen" class="flexDiv skilldiv">
                    <div class="iconListen"></div>
                    <div id="ListeningLesson" class="flexDiv">
                    </div>
                    <div id="ListeningOther" class="otherButton"></div>
                </div>
                <div id="skillVocab" class="flexDiv skilldiv">
                    <div class="iconVocab"></div>
                    <div id="VocabularyLesson" class="flexDiv">
                    </div>
                    <div id="VocabularyOther" class="otherButton"></div>
                </div>
                <div id="skillGrammar" class="flexDiv skilldiv">
                    <div class="iconGrammar"></div>
                    <div id="GrammarLesson" class="flexDiv">
                    </div>
                    <div id="GrammarOther" class="otherButton"></div>
                </div>
                <div id="skillSituation" class="flexDiv skilldiv">
                    <div class="iconSituation"></div>
                    <div id="SituationLesson" class="flexDiv">
                    </div>
                    <div id="SituationOther" class="otherButton"></div>
                </div>
            </div>

        </div>

        <div id="RandomType" class="ui-hide">
            <div class="flexDiv">
                Number of exams
             <div class="btn btnAmount firstflexdiv" id="btn20">20</div>
                <div class="btn btnAmount" id="btn30">30</div>
                <div class="btn btnAmount" id="btn50">50</div>
                <div class="btn btnAmount" id="btn100">100</div>
                <div class="btn btnAmount" id="btnUserType">
                    <input type='text' id='UserType' />
                </div>
            </div>
        </div>
        <div id="skillRandom" class="ui-hide">
            <div class="flexDiv">
                skills
             <div class="btn btnSkill firstflexdiv" id="btnRandomAll">All</div>
                <div class="btnSkill btnReading" id="FB4B4A71-B777-4164-BA4D-5C1EA9522226"></div>
                <div class="btnSkill btnListen" id="44502C7F-D3BE-4D46-9134-3FE40DA230E9"></div>
                <div class="btnSkill btnVocab" id="31667BAB-89FF-43B3-806F-174774C8DFBF"></div>
                <div class="btnSkill btnGrammar" id="5BBD801D-610F-40EB-89CB-5957D05C4A0B"></div>
                <div class="btnSkill btnSituation" id="Situation"></div>
            </div>
        </div>
    </div>

    <div class='wrapper AllPracticeSet ui-hide'>
        <div id="AllReading" class="AllPSkill ui-hide"></div>
        <div id="AllListening" class="AllPSkill ui-hide"></div>
        <div id="AllGrammar" class="AllPSkill ui-hide"></div>
        <div id="AllSituation" class="AllPSkill ui-hide"></div>
        <div id="AllVocabulary" class="AllPSkill ui-hide"></div>
    </div>

    <div class="footer">
        <div class='footerButton footerAlldiv'>
            <div class="Imagebtn btnBack"></div>
            <div class="Imagebtn btnStart ui-hide"></div>
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
        <div name='select' id="dialogLogout" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon done'></div>
            <div class='ui-text'>Do you want to Logout ?</div>
            <div class="ui-twoButton">
                <div class='btn btnCancel'>No</div>
                <div class='btn btnConfirmLogout'>Yes</div>
            </div>
        </div>
    </div>
    <div name='select' id="dialogDeleteAccount" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon done'></div>
            <div class='ui-text'>Do you want to Delete your account ?</div>
            <div class="ui-twoButton">
                <div class='btn btnCancel'>No</div>
                <div class='btn btnConfirmDelete'>Yes</div>
            </div>
        </div>
    </div>
    <div name='alert' id="dialogAlert" class='my-popup alert ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon wrong'></div>
            <div class='ui-text'></div>
            <div class='btn btnOK btnSelected'>OK</div>
        </div>
    </div>
</asp:Content>
