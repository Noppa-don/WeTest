<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../content/Wetest/Wetest.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/User.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <script src="../scripts/Wetest/User.js?ver=1.1.1" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner">
        <div class='logo'></div>
    </div>

    <div class='wrapper'>

        <div class="login ui-hide">
            <div>
                <div class="smallLogo userlogo"></div>
                <input type='text' id='userName' title='Username' placeholder='Username' />
            </div>
            <br />
            <div class='last'>
                <div class="smallLogo pswlogo"></div>
                <input type='password' id='userPass' title='Password' placeholder='Password' />
            </div>
            u
            <div id="btnLogin" class='btn btnSelected'>Log-in</div>

            <div class='textRight registerlink'>Register ?</div>
        </div>

        <div class="MainMenu">
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
    <div name='alert' id="dialogAlert" class='my-popup alert ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon wrong'></div>
            <div class='ui-text'></div>
            <div id="btnOK" class='btn btnSelected'>OK</div>
        </div>
    </div>

</asp:Content>
