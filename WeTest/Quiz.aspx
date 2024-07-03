<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/Wetest/login.css?ver=1.1.1" rel="stylesheet" type="text/css" />
    <script src="/scripts/Wetest/login.js?ver=1.1.1" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class='wrapper'>
        <div id="DivActivity">
            <div id="DivTopControlBar">
                <div id="DivTestsetIcon"></div>
                <div id="DivRunningBar"></div>
                <div id="DivBtnReportProblem"></div>
            </div>
            <div id="DivQuestionAndAnswer"></div>
            <div id="DivSideMenuBar"> 
                <div id="DivTime"></div>
                <div id="DivBtnPause"></div>
                <div id="DivBtnAllQuestion"></div>
            </div>
            <div id="DivPlayer">
                <div id="DivPrevious"></div>
                <div id="DivNext"></div>
            </div>
            <div id="DivSendActivity"></div>
        </div>
        <div id="DivReportProblem"></div>
        <div id="DivPause"></div>
        <div id="DivAllQuestion"></div>
        <div id="DivDialog"></div>
    </div>
</asp:Content>
