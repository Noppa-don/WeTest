<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="/Content/example/index.css?ver=2.1.1" rel="stylesheet" type="text/css" />
    <script src="/Scripts/example/index.js?ver=2.1.1" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<input id='fileExcel' type='file' placeholder='text import excel'/>
	<button id='importExcel'>Import Excel</button>
</asp:Content>
