<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../content/Wetest/Wetest.css?ver=1.1.2" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/User.css?ver=1.1.2" rel="stylesheet" type="text/css" />
    <script src="../scripts/Wetest/User.js?ver=1.1.2" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner">
        <div class='logo'></div>
        <div class='UserData  ui-hide'>
            <div class="UserNameandLevel"></div>
            <div class='UserPhoto'></div>
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

</asp:Content>
