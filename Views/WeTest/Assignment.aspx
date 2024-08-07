<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../content/Wetest/Wetest.css?ver=1.1.6" rel="stylesheet" type="text/css" />
    <link href="../content/Wetest/Assignment.css?ver=1.1.6" rel="stylesheet" type="text/css" />
    <script src="../scripts/Wetest/Assignment.js?ver=1.1.6" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner">
        <div class="AppData">
            <div class="logo"></div>
            <div class='pagename'>- Practice</div>
        </div>
        <div class="UserData flexDiv">
            <div class="UserNameandLevel"></div>
        </div>
    </div>

    <div class='wrapper'>
        <div id="OverDue" class="flexDiv">
           <div>Over due</div>
           <div id="OverdueItem"></div>
        </div>
        <div id="Today" class="flexDiv">
           <div>Today</div>
           <div id="TodayItem"></div>
        </div>
        <div id="ThisWeek" class="flexDiv">
           <div>2 - 7 days</div>
           <div id="ThisWeekItem"></div>
        </div>
        <div id="NextWeek" class="flexDiv">
           <div>Next Week</div>
           <div id="NextWeekItem"></div>
        </div>
    </div>
      <div class="footer">
        <div class='footerButton footerAlldiv'>
            <div class="Imagebtn btnBack"></div>
            <div class="Imagebtn btnStart ui-hide"></div>
        </div>
    </div>
</asp:Content>
