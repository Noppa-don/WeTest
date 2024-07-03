<%@ Page Title="" Language="VB" MasterPageFile="~/Views/Shared/ms2018.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
		.ui-content{max-width:500px;margin:0 auto;}
		
		.ui-label{padding:8px 0 8px 0;}
		.ui-icon{width:30px;height:30px;}	.ui-icon.add{background:url(/images/questionnaire/add.png);background-repeat:no-repeat;background-size:contain;}
		
		.main{display:flex;margin-bottom:10px;}
		.main div{flex:1;}
		.main .ui-input-text{margin:unset;}
		
		.sub{padding-left:20px;}
		.ui-block{margin-bottom:10px;}
	</style>
    <script type="text/javascript">
		$(document).on('click','.footer .add',function(e,data){
			addActivity($(this).closest('.wrapper').find('.body'));
		}).on('click','.main .add',function(e,data){
			var xobj=$(this).closest('.ui-block').find('.sub');
			addActivity(xobj[0]);
		});
		function addActivity(xobj){
			$('.ui-content').attr('currentID',parseInt($('.ui-content').attr('currentID'))+1)
			$(xobj).append("<div class='ui-block' xid='"+$('.ui-content').attr('currentID')+"'><div class='main'><div><input type='text' placeholder='ชื่อกิจกรรม'/></div><div class='ui-icon add'></div></div><div class='sub'></div></div>");
			$(xobj).trigger('create');
		}
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class='ui-content' currentID='0'>
		<div class='wrapper'>
			<div class='ui-label'>กิจกรรม</div>
			<div class='body'></div>
			<div class='footer'><div class='ui-icon add'></div></div>
		</div>
	</div>
</asp:Content>
