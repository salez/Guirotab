<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Gerenciador de Arquivos
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

        var message;
        var animando;
        var auth;
        var ASPSESSID;
        var idUploadify;
        var idBotaoEnviar;

        $(function () {

            ////////////// DIALOGS ///////////////

            setTimeout(function () {
                //cria o dialog de adicionar arquivos
                $("#AdicionarArquivo").dialog({
                    title: "Adicionar Arquivo",
                    height: 400,
                    width: 530,
                    close: function () {
                        DestroyUploadifyInstance();
                    },
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
            }, 1000);

            ///////////// UPLOADIFY ////////////////

            // Get a reference to the div for messages.
            message = $(".message");

            animando = false;

            auth = "<% = Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value %>";
            ASPSESSID = "<%= Session.SessionID %>";
            idUploadify;
            idBotaoEnviar;



            $("#upload-file-slides").click(function () {
                $("#uploadifySlides").uploadifyUpload();
            });

            $("#upload-file-arquivos").click(function () {
                $("#uploadifyArquivos").uploadifyUpload();
            });

            carregaArquivos();

        });

        function CreateUploadifyInstance() {
            $("#uploadifyArquivos").uploadify({
                wmode: 'transparent',
                hideButton: true,
                width: 170,
                height: 80,
                uploader: '<%= Url.Content("~/Scripts/Uploadify/uploadify.swf") %>',
                expressInstall: '<%= Url.Content("~/Flash/expressInstall.swf") %>',
                multi: true,
                queueID: 'arquivosQueue',
                cancelImg: '<%= Url.Content("~/images/bt_lixeira.png") %>',
                scriptData: { ASPSESSID: ASPSESSID, AUTHID: auth },
                fileDataName: 'fileData',
                sizeLimit: 943718400,
                //fileDesc: 'Arquivo .pdf',
                //fileExt: '*.pdf;',
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

                    $(idBotaoEnviar).addClass('disabled');

                },
                onError: function (event, queueId, fileObj, errorObj) {

                    // Display the error message in red.
                    message.css("color", "red");
                    message.html("Ocorreu um erro durante o upload, sua conexão pode estar com problemas, tente novamente - Error " + errorObj.type + ": " + errorObj.text);

                },
                onSelectOnce: function (event, data) {

                    if (data.fileCount >= 1) {
                        $(idBotaoEnviar).removeClass('disabled');
                    }

                },
                onCancel: function (event, ID, fileObj, data) {

                    if (data.fileCount < 1) {
                        $(idBotaoEnviar).addClass('disabled');
                    }

                },
                onProgress: function (event, queueID, fileObj, data) {
                    var speed = data.speed;
                    var percentage = data.percentage;

                    if (speed == 0) {
                        speed = 1;
                    }

                    var segundos = (fileObj.size - data.bytesLoaded) / 1024 / speed;
                    var minutos = segundos / 60;
                    var horas = minutos / 60;
                    var tempo = 0;

                    if (horas < 1) {
                        if (minutos >= 1) {

                            if (minutos / Math.round(minutos) == 1) {
                                //MOSTRAR EM MINUTOS
                                tempo = Math.floor(minutos) + " minuto(s)";

                            } else if (minutos / Math.round(minutos) != 1) {
                                //MOSTRAR EM MINUTOS E SEGUNDOS	
                                tempo = Math.floor(segundos - Math.floor(minutos) * 60);
                                tempo = Math.floor(minutos) + " minuto(s) e " + tempo + " segundos";

                            }
                        } else {
                            //MOSTRAR EM  SEGUNDOS	
                            tempo = Math.floor(segundos) + " segundo(s)";
                        }
                    } else {
                        if (horas / Math.round(horas) == 1) {
                            //MOSTRAR EM HORAS	
                            tempo = Math.floor(horas) + " hora(s)";

                        } else if (horas / Math.round(horas) != 1) {
                            //MOSTRAR HORAS E MINUTOS	
                            tempo = Math.floor(minutos - Math.floor(horas) * 60);
                            tempo = Math.floor(horas) + " hora(s) e " + tempo + " minutos";

                        }

                    }

                    $('#uploadifyArquivos' + queueID + ' .uploadifyProgressBar').css('width', percentage + '%');

                    $('#uploadifyArquivos' + queueID + ' .percentage').html('');
                    $('#uploadifyArquivos' + queueID + ' .percentage').append(' ' + percentage + '% - ' + speed + ' KB/s' + " <br><br>Tempo estimado:" + tempo);

                    return false;
                }
            });
        }

        function DestroyUploadifyInstance() {
            $("#uploadifyArquivos").unbind("uploadifySelect");
            $('#uploadifyArquivosQueue').remove();
            swfobject.removeSWF('uploadifyArquivosUploader');
        }

        function carregaArquivos() {

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetArquivos") %>',
                cache: false,
                dataType: 'html',
                success: function (data) {

                    if (data == '') {
                        $('#tabelaArquivos').hide();
                        $('#divSemArquivo').show();
                    } else {

                        $('#arquivos').html(data);

                        $('#tabelaArquivos').show();
                        $('#divSemArquivo').hide();
                    }

                    $('.tabela').trigger('update');

                }
            });

        }

        var criou = false;

        function AdicionarArquivo() {
            
            $(".message").hide();

            $("#AdicionarArquivo").dialog('open');

            DestroyUploadifyInstance();            

            setTimeout(function () { CreateUploadifyInstance(); criou = true; }, 1000);

            idUploadify = "#uploadifyArquivos";
            idBotaoEnviar = '#upload-file-slides';

        }
      
    </script>
    
    <style type="text/css">
        
        .uploadifyQueueItem
        {
            width: 95%;
        }
        
        .uploadify object { 
            position:absolute;
            z-index: 999;
            left:0; 
            right:0;
            margin-top: 15px;
            margin-left: 15px;
            width: 170px; 
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
        <h2>Gerenciador de Arquivos</h2>
        <h4></h4>
    </div>

    <div class="conteudo-baixo vas-visualizar">
        
        <h2 style="margin-top: 0;">Arquivos</h2>

        <a class="botaoAdicionar" href="javascript:AdicionarArquivo();">Adicionar Arquivo</a>
        <br /><br />

        <div class="tabela-container">
            
            <div id="divSemArquivo" style="padding: 10px 10px 0pt; display: none;">Nenhum Arquivo adicionado</div>

            <table id="tabelaArquivos" class="tabela tablesorter">
                <thead>
                	<tr>
                    	<th class="linhaCinza" colspan="4"></th>
                    </tr>
                	<tr>
                    	<th class="linhaBranco" colspan="4"></th>
                    </tr>
                    <tr>
                        <th class="semBarra" style="width: 150px;">
                            Data de Inclusão
                        </th>
                        <th class="semBarra">
                            Arquivo
                        </th>
                        <th class="semBarra">
                            Tipo
                        </th>
                        <th class="semBarra acao" style="width: 200px;">&nbsp;</th>
						
                        <tr>
                        	<th class="linhaCinza" colspan="4"></th>
                        </tr>
                        <tr>
                        	<th class="linhaBranco" colspan="4"></th>
                        </tr>
                </thead>
                <tbody id="arquivos">
                    <tr>
                        <th>&nbsp;
                            
                        </th>
                        <th>&nbsp;
                            
                        </th>
                        <th>&nbsp;
                            
                        </th>
                        <th class="acao">&nbsp;</th>
                    </tr>
                </tbody>
            </table>
        </div>
		<div class="linhaBranco"></div>
		<div class="linhaCinza"></div>

        <%--Modal--%>

        <div id="AdicionarArquivo">

            <i>Selecione o arquivo desejado e clique em <b|>Enviar Arquivo(s)</b></i>

            <br />

            <div class="message" style="display: none;">
            </div>

            <div class="uploadify">    
                
                <input type="file" id="uploadifyArquivos" name="Uploadify" />

                <div class="botoes">
                    <a id="btnProcurarArquivos" href="#">Procurar Arquivo(s)</a> <a class="disabled" id="upload-file-arquivos" href="#">Enviar Arquivo(s)</a>
                </div>
                
                <div id="arquivosQueue"></div>

            </div>

            <div id="dialog-confirm" style="display: none;">
                Tem certeza de que deseja excluir este arquivo?
            </div>

        </div>

    </div>

</asp:Content>
