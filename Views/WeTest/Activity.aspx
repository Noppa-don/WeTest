<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../content/Wetest/Wetest.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/Activity.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <script src="../scripts/Wetest/Activity.js?ver=1.1.1" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="divActivity">
        <div id="divTopControlBar">
            <div id="divTestsetIcon">
                <div id="divIcon"></div>
                <span id="ptLogo">Placement Test</span>
            </div>
            <div id="divRunningBar">
                <div id="divIconRun"></div>
                <div id="divQuestionRun">
                    <div class="runningStatus"></div>
                </div>
                <div id="divGoal"></div>
            </div>
            <div id="divBtnReportProblem"></div>
        </div>

        <div id="divPlayground">
            <div id="divQ">
                <div id="divQuestionAndAnswer">
                    <div id="divQuestion"></div>
                    <div id="divAnswer"></div>
                </div>
            </div>

            <div id="divSideMenuBar">
                <div id="divTime"><span id="minutes"></span>:<span id="seconds"></span></div>
                <div id="divPause"></div>
                <div id="divAllQuestion"></div>
            </div>
        </div>

        <div class='footer'>
            <div class="footerButton">
                <div class="Imagebtn btnBack"></div>
                <div class="Imagebtn btnNext"></div>
            </div>
            <div id="divSendQuiz"></div>
        </div>
    </div>

    <div class="banner ui-hide">
        <div class='logo'></div>
        <div class='pagename'></div>
    </div>

    <div class='wrapper ui-hide'>
        <div id="divShowLevel">
            <b>Congratulations !</b> Your Level is<span id="spnLevel"></span><br />
            <br />
            You can go to Log-in for practice and exam more. 
            <br />
            <br />
            <br />
            <div id="divShowLevelImage" style="display: flex; height: 250px;">
                <div id="lr1" class="divLevelRecommend LR1"></div>
                <div id="lr2" class="divLevelRecommend LR2"></div>
                <div id="lr3" class="divLevelRecommend LR3"></div>
                <div id="lr4" class="divLevelRecommend LR4"></div>
                <div id="lr5" class="divLevelRecommend LR5 "></div>
            </div>
            <br />
            <br />
            <br />
            <div>
                <div id="divGotoLogin"></div>
            </div>


        </div>

    </div>
    *
    <div name='select' id="dialogSelect" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon send'></div>
            <div class='ui-text'></div>
            <div class="ui-twoButton">
                <div class='btn btnCancel'>No</div>
                <div class='btn btnSelected'>Yes</div>
            </div>
        </div>
    </div>

    <div name='pause' id="dialogPause" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-cbig '>
            <div class='ui-iconbig play'></div>
        </div>
    </div>

    <div name='pq' id="dialogPromblemQuestion" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-icon close'></div>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-textHeader'>Send Problem Question</div>
            <br />
            <div>
                Problem
                <div>
                    <select id='ProblemTopic'>
                        <option value='0'>Select...</option>
                        <option value='1'>The question/answer is not clear</option>
                        <option value='2'>The question explain/answer explain is not clear</option>
                        <option value='3'>Wrong answer</option>
                        <option value='4'>Don't understand the usage</option>
                        <option value='5'>Other problem</option>
                    </select>
                </div>
                <br />
                Detail
                <div>
                    <textarea name="txtDetail" id='txtDetail' class="txtDetail" cols="40" rows="6" maxlength="1000"></textarea></div>
                <br />
                <div id="btnSendProblem" class='btn btnSelected'>Send Problem</div>
            </div>
        </div>
    </div>

    <div name='alert' id="dialogAlert" class='my-popup alert ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon done'></div>
            <div class='ui-text'></div>
            <div id="btnOK" class='btn btnSelected'>OK</div>
        </div>
    </div>

    <div name='lp' id="dialogLeapChoice" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-icon close'></div>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div id="PanelLeapChoice">
                <div class="Imagebtn btnBackPage UnActive"></div>
                <div id="AllPage"></div>
                <div class="Imagebtn btnNextPage"></div>
            </div>
            <div class="ui-twoButton">
                <div class='btn btnOrange'>Go to skip</div>
                <div class='btn'>Go to lastest</div>
            </div>
        </div>
    </div>
</asp:Content>
