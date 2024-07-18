<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/weTest/weTest.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <script src="/scripts/weTest/weTest.js?ver=1.1.1" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner">
        <div class='logo'></div>
        <div class='pagename'>- Report</div>
    </div>
    <div id="Filter">
        <div id="PracticeType" class="flexDiv">
            Practice from
            <div class="btn firstflexdiv" id="btnLesson">Lesson</div>
            <div class="btn" id="btnRandom">Random</div>
        </div>
        <div id="Time" class="flexDiv">
            <div>This week</div>
            <div>This month</div>
            <div>Choose from date</div>
            <div id="startDate"></div>
            To
            <div id="endDate"></div>
            <div id="btnSearch" class="btn"></div>
        </div>
        <div id="skill" class="flexDiv">
            skills
        <div class="btn btnSkill firstflexdiv" id="btnRandomAll">All</div>
            <div class="btnSkill btnReading" id="Reading"></div>
            <div class="btnSkill btnListen" id="Listening"></div>
            <div class="btnSkill btnVocab" id="Vocabulary"></div>
            <div class="btnSkill btnGrammar" id="Grammar"></div>
            <div class="btnSkill btnSituation" id="Situation"></div>
        </div>

    </div>

    <div id="ReportData">
    </div>
    <div class="footer">
        <div class='footerButton footerAlldiv'>
            <div class="Imagebtn btnBack"></div>
        </div>
    </div>
</asp:Content>
