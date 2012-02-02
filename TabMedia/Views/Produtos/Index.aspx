<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Produto>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Produtos
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">
        
        //deletar arquivos
        function deletar(id, nome) {

            $("#dialog-confirm").html('<center>Tem certeza de que deseja excluir o arquivo <font color=green> \''+nome+'\'</font>?</center>');

            $("#dialog-confirm").dialog({
                title: 'Atenção',
                resizable: false,
                height: 140,
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
                    "Deletar": function () {

                        var divDialog = $(this);

                        divDialog.html('<center>Apagando arquivo...</center>');

                        $.ajax({
                            type: 'post',
                            url: '<%=Url.Action("ExcluiArquivo") %>',
                            data: { id: id },
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

        $(function () {

            ////////////// DIALOGS ///////////////

            //cria o dialog de adicionar slide
//            $("#AdicionarImagem").dialog({
//                title: "Adicionar/Alterar Imagem de Produto",
//                height: 400,
//                width: 530,
//                resizable: false,
//                modal: true,
//                show: {
//                    effect: 'drop',
//                    direction: 'up'
//                },
//                hide: {
//                    effect: 'drop',
//                    direction: 'up'
//                },
//                autoOpen: false
//            });


            $('#AdicionarImagem').hide();
            $('#AdicionarImagem').jqm({ modal: true, toTop: true });

            // Get a reference to the div for messages.
            var message = $(".message");

            var auth = "<% = Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value %>";
            var ASPSESSID = "<%= Session.SessionID %>";


            $("#uploadifyImagem").uploadify({
                wmode: 'transparent',
                width: 170,
                uploader: '<%= Url.Content("~/Scripts/Uploadify/uploadify.swf") %>',
                expressInstall: '<%= Url.Content("~/Flash/expressInstall.swf") %>',
                queueID: 'slidesQueue',
                cancelImg: '<%= Url.Content("~/images/bt_lixeira.png") %>',
                buttonImg: '<%= Url.Content("~/images/bt_selecionar.png") %>',
                scriptData: { ASPSESSID: ASPSESSID, AUTHID: auth },
                fileDataName: 'fileData',
                sizeLimit: 943718400,
                fileDesc: 'Arquivo .pdf, .mp4, .zip, .jpg, .gif, .png',
                fileExt: '*.pdf;*.mp4;*.zip;*.jpg;*.gif;*.png',
                script: '<%= Url.Action("UploadProdutoImagem") %>',
                auto: false,
                multi: false,
                buttonText: "Procurar",
                onProgress: function (event, queueId, fileObj, data) {

                    if (data.percentage > 99) {
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

                    carregaProdutos();

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

                    $('#uploadifyImagem').uploadifySettings('scriptData', { idProduto: $('#idProduto').val() });

                },
                onCancel: function (event, ID, fileObj, data) {

                    if (data.fileCount < 1) {
                        $('#upload-file-slides').addClass('disabled');
                    }

                }
            });

            carregaProdutos();

            $('#ddlTerritorio').change(function () {

                if ($(this).val() == '') {
                    $('#aFiltrar').attr('href', '<%=Url.Action("index") %>');
                } else {

                    $('#aFiltrar').attr('href', '<%=Url.Action("index") %>' + '?idTerritorio=' + $(this).val());
                }
            });

            $("#upload-file-slides").click(function () {
                $("#uploadifyImagem").uploadifyUpload();
            });

        });

        function trocaImagem(idProduto) {

            $('#idProduto').val(idProduto);

            $(".message").hide();

            $('#AdicionarImagem').jqmShow(); 

        }

        function carregaProdutos() {

            $.ajax({
                type: 'get',
                url: '<%=(Request.QueryString["idTerritorio"] == null)?Url.Action("GetProdutos"):Url.Action("GetProdutosTerritorio", new { idTerritorio = Request.QueryString["idTerritorio"] }) %>',
                cache: false,
                dataType: 'html',
                success: function (data) {

                    $('.carregando').hide();

                    if (data == '') {
                        $('#divSemArquivo').show();
                    } else {

                        $('.tabelas').html(data);

                        $('#divSemArquivo').hide();
                    }

                    tableSort();
                    updateTableSort();
                    

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
        <h2>Produtos</h2>
    </div>

    <div class="conteudo-baixo vas-visualizar">
        
        <%  if (Autenticacao.VerificaPermissao("VerificarTerritorios","Produtos") == Autenticacao.StatusAutenticacao.Autorizado)
            { %>

        Território <%=Html.DropDownList("ddlTerritorio", new Models.TerritorioRepository().GetTerritorios().ToSelectList("Id", "Id", Request.QueryString["idTerritorio"]), "Selecione")%>

        <a id="aFiltrar" class="botao" href="<%=Url.Action("index") %>">Filtrar</a>

        <br /><br />

        <% } %>

        <div class="tabela-container">
            
            <div id="divSemArquivo" style="padding: 10px 10px 0pt; display: none;">Nenhum Produto relacionado.</div>

            <div class="carregando">Carregando...</div>

            <div class="linhaCinza"></div>

            <div class="tabelas"></div>

        </div>

    </div>

    <div id="AdicionarImagem" class="jqmWindow" style="height: 400px;">
		
        <div style="float: left">
            <h2>Adicionar Arquivo</h2>
        </div>
        <div style="float: right;">
            <a href="#" class="jqmClose"><img src="<%=Url.Content("~/images/bt_close_new.jpg") %>" /></a>
        </div>

        <div style="clear: both; padding: 10px 0 0 0;">

            <input type="hidden" id="idProduto"/>

		    Selecione o arquivo e clique em <b>Enviar Arquivo</b>. Extensões permitidas: .jpg, .png, .gif

		    <br />

		    <div class="message" style="display: none;">
		    </div>

		    <div class="uploadify">    
        
			    <div class="botoes">
				    <input type="file" id="uploadifyImagem" name="Uploadify" /> <a class="disabled" id="upload-file-slides" href="javascript:">Enviar Arquivo(s)</a>
			    </div>

			    <div id="slidesQueue"></div>

		    </div>   

        </div>

	</div>

</asp:Content>
