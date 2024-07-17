<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../content/Wetest/Wetest.css?ver=1.1.4" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/User.css?ver=1.1.4" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/jquery-ui-1.8.18.custom.min" type="text/css" />

    <script src="../scripts/Wetest/User.js?ver=1.1.4" type="text/javascript"></script>
    <script src="../scripts/Wetest/jGlobal.js" type="text/javascript"></script>
    <script src="../scripts/Wetest/jquery-ui-1.8.10.offset.datepicker.min.js?ver=1.1.3" type="text/javascript"></script>
    <script src="../scripts/Wetest/jquery-ui-1.10.1.custom.min.js?ver=1.1.3" type="text/javascript"></script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner">
        <div class="logo"></div>
        <div class="UserData flexDiv ui-hide">
            <div class="UserNameandLevel"></div>
        </div>
    </div>
    <div class='wrapper'>
        <div class="login">
            <div>
                <div class="smallLogo userlogo"></div>
                <input type='text' id='userName' title='Username' placeholder='Username' />
            </div>
            <br />
            <div class='last'>
                <div class="smallLogo pswlogo"></div>
                <input type='password' id='userPass' title='Password' placeholder='Password' />
            </div>
            <div id="btnLogin" class='btn btnSelected'>Log-in</div>

            <div class='textRight registerlink'>Register ?</div>
        </div>
        <div class="MainMenu ui-hide">

            <div class="menuButton">
                <div class="btn btnMainMenu" id="btnPracticeMenu">
                    <div class='practiceIcon'></div>
                    <span class="txtMenu">Practice</span>
                </div>
                <div class="btn btnMainMenu" id="btnGoalMenu">
                    <div class='goalIcon'></div>
                    <span class="txtMenu">Goal</span>
                </div>
            </div>
            <br />
            <div class="menuButton">
                <div class="btn btnMainMenu" id="btnMockUpExam">
                    <div class='mockUpExamMenuIcon'></div>
                    <span class="txtMenu">Mock-up Exam</span>
                </div>
                <div class="btn btnMainMenu" id="btnReport">
                    <div class='reportIcon'></div>
                    <span class="txtMenu">Report</span>
                </div>
            </div>
        </div>
        <div class="Goal ui-hide">
            <div class="flexDiv">
                <div><span id="UserLevel"></span></div>
                <div class="btn btnClear btnOrange">Clear</div>
            </div>
            <br />
            <span id="lastestGOAL"></span>
            <br />
            <br />
            <br />
            <div class="flexDiv">
                <div id="TimeUsed" class="fistflexdiv">
                    <div id="TimesUsedPercent" class="Percent">0%</div>
                    <br />
                    <div>Time Used</div>
                </div>

                <div id="PracticeScore">
                    <div id="PracticeScorePercent" class="Percent">0%</div>
                    <br />
                    <div>Total Practice score</div>
                </div>


            </div>
        </div>
  
</div>
      <div class="DetailGoal ui-hide">
          <div class="flexDiv">
                <span id="lastestBigGOAL" class="fistflexdiv"></span>
            <div class="btn btnClear btnOrange">Clear</div>
          </div>
         
            <br />
            <br />
            <br />
            <div class="flexDiv">
                <div class="firstflexdiv">
                    <div></div>
                    <div id="TU">Time Used</div>
                    <div id="TPC">Total Practice Score</div>
                </div>
                <div class="divPercentContain">
                    <div id="ReadingIcon"></div>
                    <div id="ReadingTime" class="ReadingPercent">0%</div>
                    <div class="TimeResult"><span id="ReadingTimeResult" class="smalltxt ui-hide"></span></div>
                    <div id="ReadingPS" class="ReadingPercent PS">0%</div>
                </div>
                <div class="divPercentContain">
                    <div id="ListeningIcon"></div>
                    <div id="ListeningTime" class="ListeningPercent">0%</div>
                    <div class="TimeResult"><span id="ListeningTimeResult" class="smalltxt ui-hide"></span></div>
                    <div id="ListeningPS" class="ListeningPercent PS">0%</div>
                </div>
                <div class="divPercentContain">
                    <div id="VocabIcon"></div>
                    <div id="VocabTime" class="VocabPercent">0%</div>
                    <div class="TimeResult"><span id="VocabularyTimeResult" class="smalltxt ui-hide"></span></div>
                    <div id="VocabularyPS" class="VocabPercent PS">0%</div>
                </div>
                <div class="divPercentContain">
                    <div id="GrammarIcon"></div>
                    <div id="GrammarTime" class="GrammarPercent">0%</div>
                    <div class="TimeResult"><span id="GrammarTimeResult" class="smalltxt ui-hide"></span></div>
                    <div id="GrammarPS" class="GrammarPercent PS">0%</div>
                </div>
                <div class="divPercentContain">
                    <div id="SituationIcon"></div>
                    <div id="SituationTime" class="SituationPercent">0%</div>
                    <div class="TimeResult"><span id="SituationTimeResult" class="smalltxt ui-hide"></span></div>
                    <div id="SituationPS" class="SituationPercent PS">0%</div>
                </div>
        </div>
    </div>
    <div class="footer">
        <div class='footerButton footerGoal ui-hide'>
            <div class="Imagebtn btnBack fistflexdiv"></div>
            <div class="Imagebtn btnSetDetailGoal unActive ui-Right"></div>
        </div>
    </div>

    <div name='alert' id="dialogAlert" class='my-popup alert ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon wrong'></div>
            <div class='ui-text'></div>
            <div id="btnOK" class='btn btnSelected'>OK</div>
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
    <div name='GoalDate' id="dialogGoalDate" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-icon close'></div>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <span id="spnGoalName"></span><br />
            <br />
            <div id="SelectGoalDate"></div>
            <br />
            <span id="spnShowdate"></span>
            <br />
            <br />
            <div id="btnSaveGoal" class="btnSaveGoal unActive"></div>

        </div>
    </div>
    <div id="dialogClearGoal" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon warning'></div>
            <div class='ui-text'>Do you want to clear your GOAL ?</div>
            <div class='ui-Warning-red'>if YES. It's mean start a new GOAL</div>
            <div class="ui-twoButton">
                <div id="btnNotClear" class='btn btnCancel'>No</div>
                <div id="btnConfirmClear" class='btn btnSelected'>Yes</div>
            </div>
        </div>
    </div>

</asp:Content>
