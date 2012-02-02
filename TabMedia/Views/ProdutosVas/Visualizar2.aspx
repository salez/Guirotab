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

			carregaArquivos();

			carregaComentarios();

		});

		//#region FUNCTIONS

		function carregaSlides() {
			
			$.ajax({
				type: 'get',
				url: '<%=Url.Action("VisualizarGetHtmlVa", new { idVa = Model.Id }) %>',
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

		function carregaArquivos() {

			$.ajax({
				type: 'get',
				url: '<%=Url.Action("VisualizarGetArquivos", new { idVa = Model.Id } ) %>',
				cache: false,
				dataType: 'html',
				success: function (data) {
					
					$('.carregando').hide();

					if (data == '') {
						$('#tabelaArquivos').hide();
						$('#divSemArquivo').show();
					} else {

						$('#tabelaArquivos').show();
						$('#divSemArquivo').hide();
					}

					$('#arquivos').html(data);

					updateTableSort();

				}
			});

		}

		function carregaComentarios() {
            
            $('#comentarios').html($('#comentarios').html() + htmlCarregando);

			$.ajax({
				type: 'get',
				url: '<%=Url.Action("VisualizarGetComentarios", new { idVa = Model.Id } ) %>',
				cache: false,
				dataType: 'html',
				success: function (data) {

					$('#comentarios').html(data);

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

        function comentarVA(){
            
           if(formIsValid() && $('#mensagem').val() != ''){
                
                $.ajax({
				type: 'post',
				url: '<%=Url.Action("VisualizarComentar")%>',
				cache: false,
				data: {
					idVa: <%=Model.Id %>,
					mensagem: $('#mensagem').val()
				},
				success: function (data) {
					
					carregaComentarios();
                    $('#mensagem').val('');

				}
			});

           }

        }

        function AprovarVA(){
           
           comentarVA();

           alertar('Aprovando VA...');
            
           $.ajax({
				type: 'post',
				url: '<%=Url.Action("AprovarVA")%>',
				cache: false,
				data: {
					idVa: <%=Model.Id %>
				},
				success: function (data) {
					
                    alertar('VA Aprovado, atualizando informações...');

					window.location.reload();

				}
		    });

        }

        function ReprovarVA(){
            
            comentarVA();

           alertar('Reprovando VA...');
            
            $.ajax({
				type: 'post',
				url: '<%=Url.Action("ReprovarVA")%>',
				cache: false,
				data: {
					idVa: <%=Model.Id %>
				},
				success: function (data) {
					
                    alertar('VA Reprovado, atualizando informações...');

					window.location.reload();

				}
			});

           

        }

        function PublicarVA(){
            
            comentarVA();

           alertar('Publicando VA...');
                
            $.ajax({
				type: 'post',
				url: '<%=Url.Action("PublicarVA")%>',
				cache: false,
				data: {
					idVa: <%=Model.Id %>
				},
				success: function (data) {
					
                    alertar('VA publicado! atualizando...');

					window.location.reload();

				}
			});

        }

        function AprovarEPublicarVA(){
            
           comentarVA();

           alertar('Publicando VA...');
                
            $.ajax({
				type: 'post',
				url: '<%=Url.Action("AprovarEPublicarVA")%>',
				cache: false,
				data: {
					idVa: <%=Model.Id %>
				},
				success: function (data) {
					
                    alertar('VA publicado! atualizando...');

					window.location.reload();

				}
			});

        }

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
		<h2><%=Model.Produto.Nome %> - <%=Model.Nome %> - Visualizar VA</h2>
	</div>
	
	<div class="conteudo-baixo">
		
		<div class="vas-visualizar">
				
				<h2 style="margin-top: 0;" class="h2Input">Nome: &nbsp;&nbsp;<span style="font-weight: normal;"><%=Model.Nome %></span> </h2>

                <%if (Model.ProdutoVaCategoria.Tipo == (char)Models.ProdutoVaCategoria.EnumTipo.Anexo)
                  { %>
                <h2 style="margin-top: 0;" class="h2Input">Link HTML(javascript): &nbsp;&nbsp;<span style="font-weight: normal;text-transform: none">javascript:openAttachment(<%=Model.Id%>)</span> </h2>
		        <%} %>

                <%if (Model.ProdutoVaCategoria.Tipo == (char)Models.ProdutoVaCategoria.EnumTipo.Apresentacao)
                  { %>

				<h2 style="margin-top: 0;">Slides</h2>

				<div id="PreVisualizar" >
				 
					<div style="float: left; clear: both; height: 40px; width: 100%;">
						Utilize as setas para navegar no VA
					</div>

					<a href="javascript:anterior();" class="anterior" style="float: left; width: 50px; position: relative; top: 300px;">
						<img src="<%=Url.Content("~/") %>images/anterior.gif" alt="Anterior" />
					</a>


					<div class="anyContainer">

						<div class="anythingSlider" id="pre-visualizar-slides" style="float: left;">
							<div class="wrapper">
								<!--pre-visualizar-slides-->
                                <div class="carregando">Carregando...</div>
							</div>
						</div>

					</div>

					<a href="javascript:proximo();" class="proximo" style="float: left; width: 50px; position: relative; top: 300px; margin-left: 15px;">
						<img src="<%=Url.Content("~/") %>images/proximo.gif" alt="Próximo" />
					</a>

				</div>

                <%} %>

                <%if (Model.ProdutoVaCategoria.Tipo == (char)Models.ProdutoVaCategoria.EnumTipo.Anexo)
                  { %>

				<h2 style="margin-top: 0;">Arquivo</h2>

				<div class="conteudo-secundario-container" style="float: left; width: 600px;">
                    
                    <div class="carregando">Carregando...</div>

					<table id="tabelaArquivos" class="tabela tablesorter" style="display: none;">
						<thead>
							<tr>
								<th>
									Arquivo
								</th>
								<th>
									Tipo
								</th>
								<th>
									Data de Inclusão
								</th>
								<th class="acao">&nbsp;</th>
							</tr>
						</thead>
						<tbody id="arquivos">
							<tr>
								<th colspan="4">
							
								</th>
							</tr>
						</tbody>
					</table>

				</div>

                <% } %>

                <h2>STATUS: &nbsp;&nbsp;<span style="font-weight: normal;"><%=Model.GetStatus() %></span> </h2>

                <div style="float: left;">
                    <form action="" method="post">
					    <%--<label for="Nome" style="width: 100px; display: block; float: left;">Nome</label> --%>
					    <div style="float: left;">
						    <%= Html.TextArea("mensagem", new { style = "width: 500px; height: 100px;", @class = "validate[required,length[0,500]]" })%>
					    </div>
                    </form>
                    
				</div>
				
                <div style="clear: both; padding: 10px 0 0">
                    <a href="javascript:comentarVA();" class="botao">Comentar</a>
                    <% if (Model.IsPendente() || Model.IsReprovado() && Model.Status != (char)Models.ProdutoVa.EnumStatus.Inativo && Autenticacao.AutorizaPermissao("aprovar", "produtosvas"))
                        { %>
					    <a href="javascript:AprovarVA();" class="botao">Aprovar</a>
                    <% } %>

                    <% if (Model.IsAprovado() && Autenticacao.AutorizaPermissao("publicar", "produtosvas"))
                        { %>
                        <a href="javascript:PublicarVA();" class="botao">Publicar</a>
                    <%
                        }
                        else if (Model.IsAprovadoByGP() && Autenticacao.AutorizaPermissao("aprovar", "produtosvas") && Autenticacao.AutorizaPermissao("publicar", "produtosvas"))
                        { %>

    				    <a href="javascript:AprovarEPublicarVA();" class="botao">Aprovar e Publicar</a>

                    <% } %>

                    <% if (!Model.IsReprovado() && !Model.IsAtivo() && Autenticacao.AutorizaPermissao("aprovar", "produtosvas"))
                        { %>
					    <a href="javascript:ReprovarVA();" class="botao">Reprovar</a>
                    <% } %>
				</div>
				

				<h2>Comentários</h2>

				<div class="comentarios" id="comentarios" style="float: left; clear: both;">
					<div class="carregando">Carregando...</div>
				</div>
				

		</div>

	</div>

</asp:Content>