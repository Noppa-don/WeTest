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
        <div id="OverDue" class="flexDiv">
           <div class="redTypeName">Over due</div>
           <div id="OverdueItem" class="redContainer flexDiv"></div>
        </div>
        <div id="Today" class=" flexDiv">
           <div class="orangeTypeName">Today</div>
           <div id="TodayItem" class="orangeContainer flexDiv"></div>
        </div>
        <div id="ThisWeek" class="flexDiv">
           <div class="greenTypeName">2 - 7 days</div>
           <div id="ThisWeekItem" class="greenContainer flexDiv"></div>
        </div>
        <div id="NextWeek" class="flexDiv">
           <div class="greenTypeName">Next Week</div>
           <div id="NextWeekItem" class="greenContainer flexDiv"></div>
        </div>
    </div>
      <div class="footer">
        <div class='footerButton footerAlldiv'>
            <div class="Imagebtn btnBack"></div>
        </div>
    </div>
</asp:Content>
