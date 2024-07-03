<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
		.wrapper{padding:7px;}
	</style>
    <script type="text/javascript">
		function sum(){
			//$('#result').val(parseInt($('#n1').val())+parseInt($('#n2').val()));
			$('#result').val($('#n1').val()+$('#n2').val());
		}
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class='wrapper'>
		n1
		<input id='n1' type='number'/>
		n2
		<input id='n2' type='number'/>
		<button onclick='sum()'>บวก</button>
		result
		<input id='result' type='text' readonly/>
	</div>
</asp:Content>
