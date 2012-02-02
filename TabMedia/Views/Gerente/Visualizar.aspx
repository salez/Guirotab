<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.ProdutoVa>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Visualizar VA
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	
	<script type="text/javascript">
		
		//#region JS
		
		$(function () {

			//#region UPLOADIFY

			carregaSlides();

		});

		//#region FUNCTIONS

		function carregaSlides() {
            
            $.ajax({
				type: 'get',
				url: '<%=Url.Action("GetVisualizacao", new { idVa = Model.Id }) %>',
				cache: false,
				dataType: 'html',
				success: function (data) {

					$('#pre-visualizar-slides .wrapper').html(data);

					setTimeout(function(){
						$('#pre-visualizar-slides').anythingSlider({
							easing: "easeInOutExpo",
							autoPlay: false,
							delay: 8000,
							startStopped: false,
							animationTime: 500,
							hashTags: false,
							buildNavigation: false,
							pauseOnHover: false
						});
					},500);
					
				}
			});


		}


		function Salvar(){
			
			alertar('Salvando...');

			$.ajax({
				type: 'post',
				url: '<%=Url.Action("CadastroSalvar")%>',
				cache: false,
				data: {
					idVa: <%=Model.Id %>,
					nome: $('#Nome').val(),
					enviarAprovacao: false
				},
				success: function (data) {
					
					OrganizarArquivos();

				}
			});
			
		}
		
		function proximo() {
			$('#pre-visualizar-slides').data('AnythingSlider').goForward();
		};

		function anterior() {
			$('#pre-visualizar-slides').data('AnythingSlider').goBack();
		};

		function formatText(index, panel) {
			return index + "";
		};

		//#endregion

		//#endregion

	</script>
	
	<style type="text/css">
		
		.uploadifyQueueItem
		{
			width: 95%;
		}
		
		.uploadify object { 
			
			float: left;
		}
		
		.disabled
		{
			background: #AAA !important; 
		}
		
		.message
		{
			text-align: center;
			border: 1px solid;
			padding: 10px;
			margin-top: 10px;
		}

	</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

	<div class="titulo">
		<h2>Visualizar VA</h2>
	</div>
	
	<div class="conteudo-baixo">

		<div id="PreVisualizar">

			<div style="float: left; clear: both; height: 40px; width: 100%;">
				<i>Utilize as setas para navegar no VA</i>
			</div>

			<a href="javascript:anterior();" class="anterior" style="float: left; width: 50px; position: relative; top: 300px;">
				<img src="<%=Url.Content("~/") %>images/anterior.gif" alt="Anterior" />
			</a>


            <div class="anyContainer">

			    <div class="anythingSlider" id="pre-visualizar-slides" style="float: left;">
				    <div class="wrapper">
					    <!--pre-visualizar-slides-->
				    </div>
			    </div>

            </div>

			<a href="javascript:proximo();" class="proximo" style="float: left; width: 50px; position: relative; top: 300px; margin-left: 30px;">
				<img src="<%=Url.Content("~/") %>images/proximo.gif" alt="Próximo" />
			</a>

		</div>

	</div>

</asp:Content>