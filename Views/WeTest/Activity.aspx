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
                <div id="divQuestionRun"></div>
                <div id="divGoal"></div>
            </div>
            <div id="divBtnReportProblem"></div>
        </div>

        <div id="divPlayground">
            <div id="divQ">
                <div id="divQuestionAndAnswer">
                    <div id="divQuestion">
                        1. Mike is a good boy _____________ is eigth years old.
                    </div>
                    <div id="divAnswer">
                        <div class="divAnswerRow">
                            <div class="divAnswerLeft">A. She</div>
                            <div class="divAnswerRight">B. He</div>
                        </div>
                        <div class="divAnswerRow">
                            <div class="divAnswerLeft">C. I</div>
                            <div class="divAnswerRight">D. We</div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="divSideMenuBar">
                <div id="divTime">15 : 00</div>
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

</asp:Content>
