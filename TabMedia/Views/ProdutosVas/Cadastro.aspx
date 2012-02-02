﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.ProdutoVa>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Cadastro de <%=Model.ProdutoVaCategoria.Nome %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
	<script type="text/javascript">
		
		//#region JS
		
		$(function () {

			//#region DIALOGS

            //cria o dialog de adicionar slide
			$("#modalFarmaciaComoFunciona").dialog({
				title: "Como Funciona?",
				width: 1000,
				height: 720,
				resizable: false,
				modal: true,
				show: {
					effect: 'drop',
					direction: 'up'
				},
				hide: {
					effect: 'drop',
					direction: 'up'
				},
				autoOpen: false,
                open: function(){
                    
                    setTimeout(function(){
						$('#como-funciona-farmacias').anythingSlider({
				        easing: "easeInOutExpo",
				        autoPlay: false,
				        delay: 8000,
				        startStopped: false,
				        animationTime: 500,
				        hashTags: false,
				        buildNavigation: false,
				        pauseOnHover: false,
				        startText: "Go",
				        stopText: "Stop",
				        navigationFormatter: formatText
			        });
					},500);
                    
                }
			});


            $('#AdicionarSlide').hide();
            $('#AdicionarSlide').jqm({modal: true,toTop: true});

            $('#AdicionarArquivo').hide();
            $('#AdicionarArquivo').jqm({modal: true,toTop: true});

            $('#AdicionarFarmacia').hide();
            $('#AdicionarFarmacia').jqm({modal: true,toTop: true});

			//cria o dialog de pre visualizacao do VA
			$("#PreVisualizar").dialog({
				title: "Visualizar VA",
				width: 1000,
				height: 750,
				resizable: false,
				modal: true,
				show: {
					effect: 'drop',
					direction: 'up'
				},
				hide: {
					effect: 'drop',
					direction: 'up'
				},
				autoOpen: false,
				close: function(){
					$('.anyContainer').html('<div class="anythingSlider" id="pre-visualizar-slides" style="float: left;"><div class="wrapper"></div></div>');
				}
			});

			//cria o dialog de alterar nome do Arquivo
			$("#AlterarNomeArquivo").dialog({
				title: "Alterar Nome do Arquivo",
				height: 165,
				width: 350,
				resizable: false,
				modal: true,
				show: {
					effect: 'drop',
					direction: 'up'
				},
				hide: {
					effect: 'drop',
					direction: 'up'
				},
				autoOpen: false
			});

            //cria o dialog de alterar nome do Arquivo
			$("#modalEspecialidades").dialog({
				title: "Especialidades",
				height: 250,
				width: 270,
				resizable: false,
				modal: true,
				show: {
					effect: 'drop',
					direction: 'up'
				},
				hide: {
					effect: 'drop',
					direction: 'up'
				},
				autoOpen: false,
                buttons: {
                    Ok : function(){ 

                        $('#frmEspecialidades').ajaxSubmit({
				            cache: false,
				            dataType: 'html',
				            success: function(data) {
					            $("#modalEspecialidades").dialog('close');
                                carregaSlides();
				            }
			            });
                    }
                }
			});

			//#endregion

			//#region UPLOADIFY

			// Get a reference to the div for messages.
			var message = $(".message");

			var animando = false;

			var auth = "<% = Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value %>";
			var ASPSESSID = "<%= Session.SessionID %>";


			$("#uploadifySlides").uploadify({
				wmode: 'transparent',
				width: 170,
				uploader: '<%= Url.Content("~/Scripts/Uploadify/uploadify.swf") %>',
				expressInstall: '<%= Url.Content("~/Flash/expressInstall.swf") %>',
				queueID: 'slidesQueue',
				cancelImg: '<%= Url.Content("~/images/bt_lixeira.png") %>',
				buttonImg: '<%= Url.Content("~/images/bt_selecionar.png") %>',
				scriptData: { idVa: <%=Model.Id %>, ASPSESSID: ASPSESSID, AUTHID: auth },
				fileDataName: 'fileData',
				sizeLimit: 943718400,
				fileDesc: 'Arquivo .pdf, .mp4, .zip, .jpg, .gif, .png',
				fileExt: '*.pdf;*.mp4;*.zip;*.jpg;*.gif;*.png',
				script: '<%= Url.Action("UploadSlide") %>',
				auto: false,
				multi: true,
				buttonText: "Procurar",
				onProgress: function (event, queueId, fileObj, data){

					if(data.percentage > 99){
						message.css("color", "green");
						message.html("processando arquivo(s)...");
						message.fadeIn();
					}

				},
				onComplete: function (event, queueId, fileObj, response, data) {
					
					// Display the successful message in green and fade out 
					// after 3 seconds.
					message.css("color", "green");
					message.html(response);
					message.fadeIn();

					carregaSlides();

				},
				onAllComplete: function (event, data) {
					
					$('#upload-file-slides').addClass('disabled');

				},
				onError: function (event, queueId, fileObj, errorObj) {

					// Display the error message in red.
					message.css("color", "red");
					message.html("Ocorreu um erro durante o upload, verifique se o arquivo não está danificado, se sua conexão não está com algum problema e tente novamente. Caso o erro persista, entre em contato com o suporte - Error " + errorObj.type + ": " + errorObj.text);
					message.fadeIn();

				},
				onSelectOnce: function (event, data) {

					if (data.fileCount >= 1) {
						$('#upload-file-slides').removeClass('disabled');
					}

				},
				onCancel: function (event, ID, fileObj, data) {

					if (data.fileCount < 1) {
						$('#upload-file-slides').addClass('disabled');
					}

				}
			});

			$("#uploadifyArquivos").uploadify({
				wmode: 'transparent',
				width: 170,
				height: 80,
				uploader: '<%= Url.Content("~/Scripts/Uploadify/uploadify.swf") %>',
				expressInstall: '<%= Url.Content("~/Flash/expressInstall.swf") %>',
				multi: true,
				queueID: 'arquivosQueue',
				cancelImg: '<%= Url.Content("~/images/bt_lixeira.png") %>',
				buttonImg: '<%= Url.Content("~/images/bt_selecionar.png") %>',
				scriptData: { idVa: <%=Model.Id %>, ASPSESSID: ASPSESSID, AUTHID: auth },
				fileDataName: 'fileData',
				sizeLimit: 943718400,
				fileDesc: 'Arquivos .pdf;.rtf;.txt;.doc;.docx;.xls;.xlsx;.csv;.mp4',
				fileExt: '*.pdf;*.rtf;*.txt;*.doc;*.docx;*.xls;*.xlsx;*.csv;*.mp4',
				script: '<%= Url.Action("UploadArquivo") %>',
				auto: false,
				multi: true,
				buttonText: "Procurar",
				onComplete: function (event, queueId, fileObj, response, data) {

					// Display the successful message in green and fade out 
					// after 3 seconds.
					message.css("color", "green");
					message.html(response);
					message.fadeIn();

					carregaArquivos();

				},
				onAllComplete: function (event, data) {

					$('#upload-file-arquivos').addClass('disabled');

				},
				onError: function (event, queueId, fileObj, errorObj) {

					// Display the error message in red.
					message.css("color", "red");
					message.html("Ocorreu um erro durante o upload, sua conexão pode estar com problemas, tente novamente - Error " + errorObj.type + ": " + errorObj.text);

				},
				onSelectOnce: function (event, data) {

					if (data.fileCount >= 1) {
						$('#upload-file-arquivos').removeClass('disabled');
					}

				},
				onCancel: function (event, ID, fileObj, data) {

					if (data.fileCount < 1) {
						$('#upload-file-arquivos').addClass('disabled');
					}

				}
			});

            $("#uploadifyFarmacias").uploadify({
				wmode: 'transparent',
				width: 170,
				height: 80,
				uploader: '<%= Url.Content("~/Scripts/Uploadify/uploadify.swf") %>',
				expressInstall: '<%= Url.Content("~/Flash/expressInstall.swf") %>',
				multi: true,
				queueID: 'farmaciasQueue',
				cancelImg: '<%= Url.Content("~/images/bt_lixeira.png") %>',
				buttonImg: '<%= Url.Content("~/images/bt_selecionar.png") %>',
				scriptData: { idVa: <%=Model.Id %>, farmacia: true, ASPSESSID: ASPSESSID, AUTHID: auth },
				fileDataName: 'fileData',
				sizeLimit: 943718400,
				fileDesc: 'Arquivos .jpg,.png',
				fileExt: '*.jpg;*.png;',
				script: '<%= Url.Action("UploadSlide") %>',
				auto: false,
				multi: true,
				buttonText: "Procurar",
				onComplete: function (event, queueId, fileObj, response, data) {

					// Display the successful message in green and fade out 
					// after 3 seconds.
					message.css("color", "green");
					message.html(response);
					message.fadeIn();

                    carregaFarmacias();

				},
				onAllComplete: function (event, data) {

					$('#upload-file-farmacias').addClass('disabled');

				},
				onError: function (event, queueId, fileObj, errorObj) {

					// Display the error message in red.
					message.css("color", "red");
					message.html("Ocorreu um erro durante o upload, sua conexão pode estar com problemas, tente novamente - Error " + errorObj.type + ": " + errorObj.text);

				},
				onSelectOnce: function (event, data) {
                    
					if (data.fileCount >= 1) {
						$('#upload-file-farmacias').removeClass('disabled');
					}

				},
				onCancel: function (event, ID, fileObj, data) {

					if (data.fileCount < 1) {
						$('#upload-file-farmacias').addClass('disabled');
					}

				}
			});

            $("#upload-file-farmacias").click(function () {
                message.css("color", "green");
				message.html("enviando arquivo(s)...");
				message.fadeIn();

				$("#uploadifyFarmacias").uploadifyUpload();
			});

			$("#upload-file-slides").click(function () {

                message.css("color", "green");
				message.html("enviando arquivo(s)...");
				message.fadeIn();

				$("#uploadifySlides").uploadifyUpload();
			});

			$("#upload-file-arquivos").click(function () {

                message.css("color", "green");
				message.html("enviando arquivo(s)...");
				message.fadeIn();

				$("#uploadifyArquivos").uploadifyUpload();
			});

			//#endregion

            //#region ANYTHING SLIDER

            

            //#endregion

			carregaArquivos();
			carregaSlides();
            carregaFarmacias();

		});

        function farmaciaComoFunciona(){

            $("#modalFarmaciaComoFunciona").dialog('open');

        }

		function AlterarNomeArquivo(idArquivo, nomeArquivo){
			
			$('#idArquivo').val(idArquivo);
			$('#nomeArquivo').val(nomeArquivo);

			$("#AlterarNomeArquivo").dialog('open');

		}

		function AlterarNomeArquivoSubmit(){
			
			$("#AlterarNomeArquivo").dialog('close');

			alertar('Atualizando nome do arquivo...');

			$.ajax({
				type: 'post',
				url: '<%=Url.Action("AlterarNomeArquivo") %>',
				cache: false,
				data: {
					idArquivo: $('#idArquivo').val(),
					idVa: <%=Model.Id %>,
					nomeArquivo: $('#nomeArquivo').val()
				},
				dataType: 'html',
				success: function (data) {
					
					fecharAlerta();
					carregaArquivos();

				}
			});

		}

		//#region FUNCTIONS

		function AtualizarOrdem() {

			var cont = 1;

			$('#slides li').each(function () {
				$(this).find('span').first().html('Slide ' + cont);
				cont++;
			});

			 $('#frmSlides').ajaxSubmit({
				url: '<%=Url.Action("AtualizarOrdem") %>',
				cache: false,
				dataType: 'html',
				data: {
					idVa: <%=Model.Id %>
				},
				success: function(data) {
					
				}
			});

		}

        function AtualizaLimiteVeiculacao(){
            
			$.ajax({
				type: 'post',
				url: '<%=Url.Action("AtualizaLimiteVeiculacao")%>',
				cache: false,
				data: {
					idVa: <%=Model.Id %>,
                    limiteVeiculacao: $('#DataLimiteVeiculacao').val()
				},
				success: function (data) {

				}
			});

        }

		function carregaSlides() {

			$.ajax({
				type: 'get',
				url: '<%=Url.Action("GetSlides", new { idVa = Model.Id }) %>',
				cache: false,
				dataType: 'html',
				success: function (data) {

					if (data == '') {
						$('#divSlides').hide();
						$('#divSemSlide').show();
					} else {
						$('#divSlides').show();
						$('#divSemSlide').hide();
					}

					$('#slides').html(data);

					//ao carregar, torna a lista ordenável
					$('#slides').sortable({
						tolerance: 'pointer',
						cursor: 'pointer',
						scrollSensitivity: 100,
						scrollSpeed: 40,
						opacity: 0.6,
						revert: true,
						stop: function (event, ui) {

							AtualizarOrdem();

						}
					});

					//desabilita a seleção de texto enquanto faz o drag
					$("#slides").disableSelection();

					$('#slides .checkbox').click(function(){
						
						$.ajax({
							type: 'post',
							url: '<%=Url.Action("AtualizarAutoPlay") %>',
							data: {
								IdSlide: $(this).val(),
								idVa: <%=Model.Id %>,
								autoPlay: $(this).is(':checked')
							},
							cache: false,
							dataType: 'html',
							success: function (data) {

							}
						});

					});

				}
			});

		}

		function carregaArquivos() {

			$.ajax({
				type: 'get',
				url: '<%=Url.Action("GetArquivos", new { idVa = Model.Id } ) %>',
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

        function carregaFarmacias() {

			$.ajax({
				type: 'get',
				url: '<%=Url.Action("GetSlides", new { idVa = Model.Id }) %>',
                data: {
                    farmacia: true
                },
				cache: false,
				dataType: 'html',
				success: function (data) {

					if (data == '') {
						$('#divFarmacias').hide();
						$('#divSemFarmacia').show();
					} else {
						$('#divFarmacias').show();
						$('#divSemFarmacia').hide();
					}

					$('#farmacias').html(data);

				}
			});

		}

		function AdicionarSlide() {

			$(".message").hide();

			$('#AdicionarSlide').jqmShow(); 
			
		}

		function AdicionarArquivo() {

			$(".message").hide();

            $('#AdicionarArquivo').jqmShow(); 

		}

        function AdicionarFarmacia() {

			$(".message").hide();

			$('#AdicionarFarmacia').jqmShow(); 

		}

		function Salvar(){
			
            if(!formIsValid()){
                return;
            }

			OrganizarArquivos(function(){

				alertar('Salvando...');

				$.ajax({
					type: 'post',
					url: '<%=Url.Action("CadastroSalvar")%>',
					cache: false,
					data: {
						idVa: <%=Model.Id %>,
						enviarAprovacao: false,
                        nome:  $('#Nome').val(),
                        descricao:  $('#Descricao').val(),
                        palavrasChave:  $('#PalavrasChave').val(),
                        limiteVeiculacao: $('#DataLimiteVeiculacao').val()
					},
					success: function (data) {
					
						redirecionaProduto();

					}
				});
			
			});
			
		}

		function EnviarAprovacao() {
			
            if(!formIsValid()){
                return;
            }

			OrganizarArquivos(function(){

				alertar('Enviando para aprovação...');

				$.ajax({
					type: 'post',
					url: '<%=Url.Action("CadastroSalvar")%>',
					cache: false,
					data: {
						idVa: <%=Model.Id %>,
						enviarAprovacao: true,
                        nome:  $('#Nome').val(),
                        descricao:  $('#Descricao').val(),
                        palavrasChave:  $('#PalavrasChave').val(),
                        limiteVeiculacao: $('#DataLimiteVeiculacao').val()
					},
					success: function (data) {
					
						redirecionaProduto();

					}
				});

			});

		}

		function SalvarEPublicar() {
			
            if(!formIsValid()){
                return;
            }

			OrganizarArquivos(function(){

				alertar('Publicando...');

				$.ajax({
					type: 'post',
					url: '<%=Url.Action("CadastroSalvarEPublicar")%>',
					cache: false,
					data: {
						idVa: <%=Model.Id %>,
                        nome:  $('#Nome').val(),
                        descricao:  $('#Descricao').val(),
                        palavrasChave:  $('#PalavrasChave').val(),
                        limiteVeiculacao: $('#DataLimiteVeiculacao').val()
					},
					success: function (data) {
					
						redirecionaProduto();

					}
				});

			});

		}

        function SalvarETestar() {
			
            if(!formIsValid()){
                return;
            }

			OrganizarArquivos(function(){

				alertar('Salvando para Teste...');

				$.ajax({
					type: 'post',
					url: '<%=Url.Action("CadastroSalvarETestar")%>',
					cache: false,
					data: {
						idVa: <%=Model.Id %>,
                        nome:  $('#Nome').val(),
                        descricao:  $('#Descricao').val(),
                        palavrasChave:  $('#PalavrasChave').val(),
                        limiteVeiculacao: $('#DataLimiteVeiculacao').val()
					},
					success: function (data) {
					
						redirecionaProduto();

					}
				});

			});

		}

		function redirecionaProduto(){

			alertar('OK! Redirecionando...');

			window.location = '<%=Url.Action("index", new { id = Model.IdProduto}) %>';

		}

		function OrganizarArquivos(funcao){
			
            if(!formIsValid()){
                return;
            }

			alertar('Validando informações...');

            $.ajax({
				type: 'post',
				url: '<%=Url.Action("ValidaVa")%>',
				cache: false,
				data: {
				    idVa: <%=Model.Id %>,
                    nome:  $('#Nome').val(),
                    descricao:  $('#Descricao').val(),
                    palavrasChave:  $('#PalavrasChave').val(),
                    limiteVeiculacao: $('#DataLimiteVeiculacao').val()
				},
				success: function (data) {

					if(data.split(':')[0] == 'erro'){
						alertar('Erro: '+ data.split(':')[1]);
						return;
					}
					
                    //informações ok, continua gerando o zip e chamando a funcao desejada

					$.ajax({
				        type: 'post',
				        url: '<%=Url.Action("CadastroGerarZip")%>',
				        cache: false,
				        data: {
					        idVa: <%=Model.Id %>
				        },
				        success: function (data) {

					        if(data.split(':')[0] == 'erro'){
						        alertar('Ocorreu um erro durante a organização dos arquivos para utilização no Ipad.<br><br> Erro: '+ data.split(':')[1]);
						        return;
					        }
					
					        funcao();
					
				        }
			        });

				}

			});

			

		}

		function excluirSlide(idSlide) {
			
		   alertar('Excluindo Slide...');

			$.ajax({
				type: 'post',
				url: '<%=Url.Action("ExcluirSlide", new { idVa = Model.Id}) %>',
				cache: false,
				data: {
					idSlide: idSlide
				},
				success: function (data) {

					carregaSlides();
                    carregaFarmacias();
					//completo
                    fecharAlerta();

				}
			});

		}

		function excluirArquivo(idArquivo, nome) {
			
			$("#dialog-confirm").html('<center>Tem certeza de que deseja excluir o arquivo <font color=green> \'' + nome + '\'</font>?</center>');
			
			$("#dialog-confirm").dialog({
				title: 'Atenção',
				resizable: false,
				height: 210,
				modal: true,
				autoopen: false,
				show: {
					effect: 'drop',
					direction: 'up'
				},
				hide: {
					effect: 'drop',
					direction: 'up'
				},
				buttons: {

					"Cancelar": function () {
						$(this).dialog("close");
					},

					"Excluir": function () {
						var divDialog = $(this);

						divDialog.html('<center>Apagando arquivo...</center>');

						$.ajax({
							type: 'post',
							url: '<%=Url.Action("ExcluirArquivo", new { idVa = Model.Id}) %>',
							data: { idArquivo: idArquivo },
							cache: false,
							success: function (data) {

								carregaArquivos();

								divDialog.dialog("close");

							}
						});
					}

				}
			});

		}
		

		function formatText(index, panel) {
			return index + "";
		};

		function VisualizarVA(){

			$(".message").hide();

//			$("#PreVisualizar").dialog('open');
//STATUS=NO, TOOLBAR=NO, LOCATION=NO, DIRECTORIES=NO, RESISABLE=NO, SCROLLBARS=YES, TOP=10, LEFT=10, WIDTH=770, HEIGHT=400
            var newwindow = window.open('<%=Url.Action("CadastroPreVisualizar", new { idVa = Model.Id }) %>','name','STATUS=NO, TOOLBAR=NO, LOCATION=NO, DIRECTORIES=NO, RESISABLE=NO, SCROLLBARS=NO, height='+screen.height+',width='+screen.width+',top=0,left=0, fullscreen=yes');

//			$.ajax({
//				type: 'get',
//				url: '<%=Url.Action("CadastroPreVisualizar", new { idVa = Model.Id }) %>',
//				cache: false,
//				dataType: 'html',
//				success: function (data) {

//					$('#pre-visualizar-slides .wrapper').html(data);

//					setTimeout(function(){
//						$('#pre-visualizar-slides').anythingSlider({
//							easing: "easeInOutExpo",
//							autoPlay: false,
//							delay: 8000,
//							startStopped: false,
//							animationTime: 500,
//							hashTags: false,
//							buildNavigation: false,
//							pauseOnHover: false,
//							startText: "Go",
//							stopText: "Stop",
//							navigationFormatter: formatText
//						});
//					},500);
//					
//				}
//			});

		}

        function EditarEspecialidades(idSlide){
            $('#modalEspecialidades').dialog('open');

            $.ajax({
				type: 'post',
				url: '<%=Url.Action("GetSlideEspecialidades") %>',
				cache: false,
				dataType: 'html',
                data: {idSlide: idSlide},
				success: function (data) {

					$('#especialidades').html(data);
					
				}
			});
        }

        function FarmaciaProximo() {
			$('#como-funciona-farmacias').data('AnythingSlider').goForward();
		};

		function FarmaciaAnterior() {
			$('#como-funciona-farmacias').data('AnythingSlider').goBack();
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
    	<h4></h4>
		<h2><%=Model.Produto.Nome %> > <%=Model.Nome %> > Cadastro de <%=Model.ProdutoVaCategoria.Nome %></h2>
        <h4></h4>
	</div>
	
	<div class="conteudo-baixo">
    
		<div class="vas-visualizar">
			
			<div style="margin-top: 0; font-size: 15px; font-weight: bold;">
                <label for="Nome" style="width: 150px; display: block; float: left;">Nome</label> 
                <%= Html.TextBoxFor(model => model.Nome, new { @class = "validate[required,lenght[0,100]]", maxlenght = "100" })%>
            </div>
            <br />
            <div style="background:url(../../images/input_textarea.png) no-repeat 150px 0; height:100px; margin-top: 0; font-size: 15px; font-weight: bold;">
                <label for="Descricao" style="background:none; border:medium none; width: 150px; display: block; float: left;">Descrição</label> 
                <%= Html.TextAreaFor(model => model.Descricao, new { style = "background:none; border:medium none; padding:5px 0; height: 90px; width: 500px;", @class = "validate[required]", maxlenght = "100" })%>
            </div>
            <br />
            <div style="margin-top: 0; font-size: 15px; font-weight: bold;">
                <label for="PalavrasChave" style="width: 150px; display: block; float: left;">Palavras-Chave</label> 
                <%= Html.TextBoxFor(model => model.PalavrasChave, new { @class = "validate[lenght[0,400]]", maxlenght = "100" })%>
            </div>
            <br />
            <div style="margin-top: 0; font-size: 15px; font-weight: bold;">
                <label for="DataLimiteVeiculacao" style="width: 150px; display: block; float: left;">Limite de Veiculação</label> 
                <%=Html.TextBox("DataLimiteVeiculacao", Model.DataLimiteVeiculacao.Formata(Util.Data.FormatoData.DiaMesAno), new { @class = "data validate[required]", onchange = "AtualizaLimiteVeiculacao()" })%>
            </div>
            <br />
			
            <%
                if (Model.ProdutoVaCategoria.Tipo == (char)Models.ProdutoVaCategoria.EnumTipo.Apresentacao)
                { 
                 %>

			<%//++ SLIDES %>

			<h2 style="margin-top: 0;">Slides</h2>

			<a class="botaoAddSlides" href="javascript:AdicionarSlide();">Adicionar Slide</a>
			<br /><br />
            <div class="linhaCinza"></div>

			<div class="conteudo-secundario-container" style="float: left; width: 950px; margin-bottom:30px;">
				<div class="conteudo-secundario">
				
					<div id="divSemSlide" style="padding: 10px 0pt; display: none;">Nenhum Slide adicionado</div>

					<div id="divSlides">
					
						<i>Segure e arraste o slide para alterar a ordem de apresentação.</i><br /><br />
						
						<form id="frmSlides" method="post" action="">
							<ul id="slides">
								<!--Slides-->
								<div class="carregando">Carregando...</div>
							</ul>
						</form>
					</div>

				</div>
			</div>

			<%
                }
                 %>
            
			<%
            
                if (Model.ProdutoVaCategoria.Tipo == (char)Models.ProdutoVaCategoria.EnumTipo.Anexo)
                {

                //++ ARQUIVOS %>

			<div style="clear:both;" class="linhaCinza"></div>

			<br><br>
			<h2>Arquivo</h2>

			<a class="botaoAddArquivo" href="javascript:AdicionarArquivo();">Adicionar Arquivo</a>
			<br /><br />

			<div class="tabela-container">
				<div class="linhaCinza"></div>
				<div class="linhaBranco"></div>
				<div id="divSemArquivo" style="margin:19px 0; padding:0; display: none; text-indent:-999999px; width:181px; height:182px; background:url(<%=Url.Content("~/") %>images/add_arquivo.jpg);">Nenhum Arquivo adicionado</div>

				<div class="carregando">Carregando...</div>

				<table id="tabelaArquivos" class="tabela tablesorter" style="display: none; margin-bottom:50px;">
					<thead>
						<tr>
							<th class="semBarra">
								Data de Inclusão
							</th>
							<th class="semBarra">
								Arquivo
							</th>
							<th class="semBarra">
								Tipo
							</th>
							<th class="semBarra acao" style="width: 160px;">&nbsp;</th>
						</tr>
                    	<tr>
                        	<th class="linhaCinza" colspan="4"></th>
                        </tr>
                    	<tr>
                        	<th class="linhaBranco" colspan="4"></th>
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
			<div class="linhaBranco"></div>
			<div class="linhaCinza"></div>
            
            <div style="height:50px;"></div>

            <%
                }
                 %>

                <%
            if (Model.ProdutoVaCategoria.Tipo == (char)Models.ProdutoVaCategoria.EnumTipo.Apresentacao && Model.ProdutoVaCategoria.SomenteUmAtivo)
            { 
                %>

            <%--<%//++ Farmacias %>
            
            <h2 style="margin-bottom: 18px;">Farmácias <img src="<%=Url.Content("~/") %>images/tutorial/va/farmacia/new.jpg" style="height:39px; width:98px; position: absolute; margin-top: -11px; margin-left: 4px;" /></a></h2>
            
            <a class="botaoAddFundo" href="javascript:AdicionarFarmacia();">Adicionar Plano de Fundo</a>
            <br /><br />
            <div class="linhaCinza"></div>
            <a style="float:right; margin:23px 290px 0 30px; text-decoration:underline;" href="javascript:farmaciaComoFunciona();">Como Funciona?</a>
            <a style="float:right; margin:23px 0; text-decoration:underline;" href="<%=Url.Content("~/") %>images/tutorial/va/farmacia/template.zip" target="_blank">Download Template</a>

			
            <div id="divSemFarmacia" style="background:url(<%=Url.Content("~/") %>images/fundo_de_farmacias.jpg); height:251px; no-repeat 0 0; padding:0; display: none;"></div>
            <br />
            <div style="background:url(/Images/);"></div>
            
			<div id="divFarmacia">
					
				<ul id="farmacias">
					<!--Slides-->
					<div class="carregando">Carregando...</div>
				</ul>

			</div>--%>

            <%
                }
                 %>

			<div class="botoes">
                
                <% if (Model.ProdutoVaCategoria.Tipo == (char)Models.ProdutoVaCategoria.EnumTipo.Apresentacao)
                   { %>
				<a class="botaoVisualizarVA" href="javascript:VisualizarVA();">Visualizar VA</a>
                <% } %>

                <a class="botaoSalvarTeste" href="javascript:SalvarETestar();">Salvar Como Teste</a>
				<a  class="botaoSalvar" href="javascript:Salvar();">Salvar</a>

				<% if (Autenticacao.AutorizaPermissao("aprovar", "produtosvas") && Autenticacao.AutorizaPermissao("publicar", "produtosvas") && Sessao.Site.UsuarioInfo.IsAdministrador()) { %>

				   <a class="botaoSalvarPlublicar" href="javascript:SalvarEPublicar();">Salvar e Publicar</a>

				 <% }
				   else if (Model.IsAprovado() && Autenticacao.AutorizaPermissao("publicar", "produtosvas"))
				   {%>
					   
					<a class="botaoSalvarPlublicar" href="javascript:SalvarEPublicar();">Salvar e Publicar</a>     
							  
				<% } else {%>
				    
                    <% if (Sessao.Site.UsuarioInfo.IsGerente())
                       { %>
					    <a  class="botaoEnviarAprovacao" href="javascript:EnviarAprovacao();">Enviar para Aprovação / Aprovar</a>     
                    <%}
                       else
                       { %>
                       <a  class="botaoEnviarAprovacao" href="javascript:EnviarAprovacao();">Enviar para Aprovação</a>     
                    <%} %>

				<% } %> 
			</div>


		</div>


        <%// Modal Como Funciona %>

        <div id="modalFarmaciaComoFunciona">

            
            <a href="javascript:FarmaciaAnterior();" class="anterior" style="float: left; width: 50px; position: relative; top: 300px;">
				<img src="<%=Url.Content("~/") %>images/anterior.gif" alt="Anterior" />
			</a>


			<div class="">

				<div class="anythingSliderFarmacia" id="como-funciona-farmacias" style="float: left;">
					<div class="wrapper">
                        <ul>
						<!--pre-visualizar-slides-->
                        <li>
                            <img src="<%=Url.Content("~/") %>images/tutorial/va/farmacia/01.jpg" />
                        </li>
                        <li>
                           <a href="<%=Url.Content("~/") %>images/tutorial/va/farmacia/template.zip" target="_blank">
                                <img src="<%=Url.Content("~/") %>images/tutorial/va/farmacia/02.jpg" />
                            </a>
                        </li>
                        <li>
                            <img src="<%=Url.Content("~/") %>images/tutorial/va/farmacia/03.jpg" />
                        </li>
                        <li>
                            <img src="<%=Url.Content("~/") %>images/tutorial/va/farmacia/04.jpg" />
                        </li>
                        </ul>
					</div>
				</div>

			</div>

			<a href="javascript:FarmaciaProximo();" class="proximo" style="float: left; width: 50px; position: relative; top: 300px; margin-left: 30px;">
				<img src="<%=Url.Content("~/") %>images/proximo.gif" alt="Próximo" />
			</a>

        </div>

        <%//++ Modal Adicionar Farmacia %>
		
		<div id="AdicionarFarmacia" class="jqmWindow" style="height: 400px;">

            <div style="float: left">
                <h2>Adicionar Arquivo</h2>
            </div>
            <div style="float: right;">
                <a href="#" class="jqmClose"><img src="<%=Url.Content("~/images/bt_close_new.jpg") %>" /></a>
            </div>

            <div style="clear: both; padding: 10px 0 0 0;">
		    
			    Adicione um arquivo .jpg ou .png

			    <br />

			    <div class="message" style="display: none;">
			    </div>

			    <div class="uploadify">    

				    <div class="botoes">
					    <input type="file" id="uploadifyFarmacias" name="Uploadify" /> <a class="disabled" id="upload-file-farmacias" href="javascript:">Enviar Arquivo</a>
				    </div>

				    <div id="farmaciasQueue"></div>

			    </div>   

            </div>

		</div>

		<%//++ Modal Adicinar Slide %>
		
		<div id="AdicionarSlide" class="jqmWindow" style="height: 400px;">

            <div style="float: left">
                <h2>Adicionar Arquivo</h2>
            </div>
            <div style="float: right;">
                <a href="#" class="jqmClose"><img src="<%=Url.Content("~/images/bt_close_new.jpg") %>" /></a>
            </div>

            <div style="clear: both; padding: 10px 0 0 0;">
		
			    Adicione um pacote HTML (.zip), um arquivo .pdf, .mp4, .jpg ou .png ao VA

			    <br />

			    <div class="message" style="display: none;">
			    </div>

			    <div class="uploadify">    

				    <div class="botoes">
					    <input type="file" id="uploadifySlides" name="Uploadify" /> <a class="disabled" id="upload-file-slides" href="javascript:">Enviar Arquivo(s)</a>
				    </div>

				    <div id="slidesQueue"></div>

			    </div>   

            </div>

		</div>

		<%//++ Modal Adicinar Arquivo %>

		<div id="AdicionarArquivo" class="jqmWindow" style="height: 400px;">
            
            <div style="float: left">
                <h2>Adicionar Arquivo</h2>
            </div>
            <div style="float: right;">
                <a href="#" class="jqmClose"><img src="<%=Url.Content("~/images/bt_close_new.jpg") %>" /></a>
            </div>

            <div style="clear: both; padding: 10px 0 0 0;">

			    Adicione um arquivo aos arquivos do VA (extensões disponíveis: .pdf, .rtf, .doc, .docx, .xls, .xlsx, .txt, .csv)

			    <br />

			    <div class="message" style="display: none;">
			    </div>

			    <div class="uploadify">    
				
				    <div class="botoes">

					    <input type="file" id="uploadifyArquivos" name="Uploadify" /> <a class="disabled" id="upload-file-arquivos" href="javascript:">Enviar Arquivo(s)</a>
				    </div>

				    <div id="arquivosQueue"></div>

			    </div>  
                 
            </div>

		</div>

		<%//++ Modal Pré Visualização %>

		<div id="PreVisualizar">

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
					</div>
				</div>

			</div>

			<a href="javascript:proximo();" class="proximo" style="float: left; width: 50px; position: relative; top: 300px; margin-left: 30px;">
				<img src="<%=Url.Content("~/") %>images/proximo.gif" alt="Próximo" />
			</a>

		</div>

		<%//++ Modal Alterar nome do Arquivo %>

		<div id="AlterarNomeArquivo">
			
            
			<input type="hidden" id="idArquivo" />
			<input type="text" id="nomeArquivo" style="width: 250px;" />

			<br /><br />
			<a class="disabled" href="javascript:AlterarNomeArquivoSubmit();">Alterar Nome</a>

		</div>

        <%//++ Modal Especialidades %>

		<div id="modalEspecialidades">
			
			<form id="frmEspecialidades" action="<%=Url.Action("AtualizarSlideEspecialidades") %>" method="post">

                <div id="especialidades"></div>

            </form>

		</div>
		
	</div>

</asp:Content>