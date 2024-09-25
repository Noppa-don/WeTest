<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/weTest/Wetest.css?ver=1.1.7" rel="stylesheet" type="text/css" />
    <link href="/content/weTest/Report.css?ver=1.1.7" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/jquery-ui-1.8.18.custom.min" type="text/css" />

    <script src="/scripts/weTest/Report.js?ver=1.1.6" type="text/javascript"></script>
    <script src="/scripts/weTest/UserMenu.js?ver=1.1.6" type="text/javascript"></script>
    <script src="../scripts/Wetest/jGlobal.js" type="text/javascript"></script>
    <script src="../scripts/Wetest/jquery-ui-1.8.10.offset.datepicker.min.js?ver=1.1.3" type="text/javascript"></script>
    <script src="../scripts/Wetest/jquery-ui-1.10.1.custom.min.js?ver=1.1.3" type="text/javascript"></script>
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

        <div class="SearchReport flexDiv">
            <div class="Filter firstflexdiv">
                <div id="PracticeType" class="flexDiv">
                    Practice from
                    <div class="btn firstflexdiv" id="btnLesson">Lesson</div>
                    <div class="btn " id="btnRandom">Random</div>

                </div>
                <div id="ChooseLevel" class="flexDiv ui-hide">
                    choose Level
                        <div id="SelectLevel"></div>
                </div>
                <div id="TimeData" class="flexDiv">
                    <div>
                        <label for="rdbThisWeek" class="rdb">This week</label><input type="radio" name="radio-1" class="firstflexdiv" id="rdbThisWeek" />
                    </div>
                    <div>
                        <label for="rdbMonth" class="rdb">This month</label><input type="radio" name="radio-1" id="rdbMonth" />
                    </div>
                    <div>
                        <label for="rdbChooseDate" class="rdb">Choose from date</label><input type="radio" name="radio-1" id="rdbChooseDate" />
                    </div>
                    <div>
                        <input type='text' class="filterdate ui-hide" id='StartDate' />
                    </div>
                    <div class="calendarlogo unActive" id="btnStartDate"></div>

                    <div>
                        <input type='text' class="filterdate ui-hide" id='EndDate' />
                    </div>
                    <div class="calendarlogo unActive" id="btnEndDate"></div>
                </div>
            </div>
            <div>
                <div class="Imagebtn unActive" id="btnSearch"></div>
            </div>
        <h1></h1>
        </div>

        <div id="skillRandom">
            <div class="flexDiv">
                <div class="btn btnSelected firstflexdiv" id="btnRandomAll">All</div>
                <div class="btnSkill Selected btnReading ui-hide" id="FB4B4A71-B777-4164-BA4D-5C1EA9522226"></div>
                <div class="btnSkill Selected btnListening ui-hide" id="44502C7F-D3BE-4D46-9134-3FE40DA230E9"></div>
                <div class="btnSkill Selected btnVocabulary ui-hide" id="31667BAB-89FF-43B3-806F-174774C8DFBF"></div>
                <div class="btnSkill Selected btnGrammar ui-hide" id="5BBD801D-610F-40EB-89CB-5957D05C4A0B"></div>
                <div class="btnSkill Selected btnSituation ui-hide" id="F6E25F3E-192B-4081-B141-74720066FB74"></div>
            </div>
        </div>

        <div class='reportData'>
            <div class="nodata">- No data -</div>
            <div class="reportDetail ui-hide">
                <div class="dataHeader flexDiv">
                    <div class="headerItem divDate firstflexdiv">Date</div>
                    <div class="headerItem divStartTime">Start</div>
                    <div class="headerItem divEndTime">End</div>
                    <div class="headerItem divTestsetName">Book Number</div>
                    <div class="headerItem divScore">Score</div>
                    <div class="headerItem divRightSpace"></div>
                </div>
                <div class="detailData"></div>
            </div>
        </div>

        <div class="Noti ui-hide">
            Notification<br />
            <br />
            <div id="NotiItem"></div>
        </div>
    </div>

    <div class="footer">
        <div class='footerButton footerAlldiv'>
            <div class="Imagebtn btnBack"></div>
            <div class="Imagebtn btnStart ui-hide"></div>
        </div>
        <div class='footerButton footerSetting ui-hide'>
            <div class="Imagebtn btnBack fistflexdiv"></div>
        </div>
    </div>

    <div name='FilterDate' id="dialogFilterDate" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-icon close'></div>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <span id="spnDateName"></span>
            <br />
            <br />
            <div id="SelectFilterDate"></div>
            <br />
            <span id="spnShowdate"></span>
            <br />
            <br />
            <div id="btnSelectDate" class="btnSelectDate unActive"></div>

        </div>
    </div>

    <div name='select' id="dialogSelectAgain" class='my-popup confirm ui-popup-container ui-popup-hidden'>
        <div class='ui-content ui-body-c ui-corner-all ui-shadow'>
            <div class='ui-icon done'></div>
            <div class='ui-text'>Do you want to do this practice again ?</div>
            <div class="ui-twoButton">
                <div class='btn btnCancel'>No</div>
                <div class='btn btnSelected'>Yes</div>
            </div>
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
