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
        <div class='pagename'>- Practice</div>
    </div>

    <div id="Filter"></div>
    <div id="List"></div>
</asp:Content>
