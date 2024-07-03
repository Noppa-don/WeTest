<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/content/Wetest/User.css?ver=1.1.2" rel="stylesheet" type="text/css" />
    <script src="/scripts/Wetest/User.js?ver=1.1.2" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner">
        <div class='logo'></div>
        <div class='pagename'> - Register</div>
    </div>

    <div class='wrapper'>

        <div class="login">
            <div>
                <div class="smalllogo userlogo"></div>
                <input type='text' id='userName' title='Username' placeholder='Username' />
            </div>
            <br />
            <div class='last'>
                <div class="smalllogo pswlogo"></div>
                <input type='password' id='userPass' title='Password' placeholder='Password' />
            </div>
            <div class='btn btnSelected'>Log-in</div>

            <div class='textRight textlink registerlink'>Register ?</div>
        </div>

        <div class="MainMenu"></div>

    </div>
</asp:Content>
