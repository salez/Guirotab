<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Importação
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <%=Util.Header.Css("~/css/uploadify/uploadify.css")%>
    <%=Util.Header.Javascript("~/Scripts/uploadify/jquery.uploadify.v2.1.0.min.js")%>
    
    <script type="text/javascript">

        function VerificaExcel() {
            
            alertar('Verificando planilha...');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("VerificaExcel") %>',
                cache: false,
                dataType: 'html',
                success: function (response) {

                    if (response.indexOf("[ok]") != -1) {

                        Importar();

                    } else {

                        alertar(response);

                    }

                }
            });

        }

        function Importar(){

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("ImportarExcel") %>',
                cache: false,
                dataType: 'html',
                success: function (response) {

                    if (response.indexOf("[ok]") != -1) {

                        alertar('Planilha importada!');

                    } else {

                        alertar(response);

                    }

                }
            });



        }

        function VerificaExcelGerentesRep() {

            alertar('Verificando planilha...');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("VerificaExcelGerentesRep") %>',
                cache: false,
                dataType: 'html',
                success: function (response) {

                    if (response.indexOf("[ok]") != -1) {

                        ImportarGerentesRep();

                    } else {

                        alertar(response);

                    }

                }
            });

        }

        function ImportarGerentesRep() {

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("ImportarExcelGerentesRep") %>',
                cache: false,
                dataType: 'html',
                success: function (response) {

                    if (response.indexOf("[ok]") != -1) {

                        alertar('Planilha importada!');

                    } else {

                        alertar(response);

                    }

                }
            });

        }

        function VerificaExcelTerritorios() {

            alertar('Verificando planilha...');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("VerificaExcelTerritorios") %>',
                cache: false,
                dataType: 'html',
                success: function (response) {

                    if (response.indexOf("[ok]") != -1) {

                        ImportarTerritorios();

                    } else {

                        alertar(response);

                    }

                }
            });

        }

        function ImportarTerritorios() {

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("ImportarExcelTerritorios") %>',
                cache: false,
                dataType: 'html',
                success: function (response) {

                    if (response.indexOf("[ok]") != -1) {

                        alertar('Planilha importada!');

                    } else {

                        alertar(response);

                    }

                }
            });

        }

        function VerificaExcelOrdemProdutos() {

            alertar('Verificando planilha...');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("VerificaExcelOrdemProdutos") %>',
                cache: false,
                dataType: 'html',
                success: function (response) {

                    if (response.indexOf("[ok]") != -1) {

                        ImportarOrdemProdutos();

                    } else {

                        alertar(response);

                    }

                }
            });

        }

        function ImportarOrdemProdutos() {

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("ImportarExcelOrdemProdutos") %>',
                cache: false,
                dataType: 'html',
                success: function (response) {

                    if (response.indexOf("[ok]") != -1) {

                        alertar('Planilha importada!');

                    } else {

                        alertar(response);

                    }

                }
            });

        }

        function VerificaExcelDoutores() {

            alertar('Verificando planilha...');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("VerificaExcelDoutores") %>',
                cache: false,
                dataType: 'html',
                success: function (response) {

                    if (response.indexOf("[ok]") != -1) {

                        ImportarDoutores();

                    } else {

                        alertar(response);

                    }

                }
            });

        }

        function ImportarDoutores() {

            setTimeout(function(){
                alertar('Vericação concluída, importando...')
                },300);

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("ImportarExcelDoutores") %>',
                cache: false,
                dataType: 'html',
                success: function (response) {

                    if (response.indexOf("[ok]") != -1) {

                        setTimeout(function () {
                            alertar('Planilha importada!');
                        }, 400);
                        

                    } else {

                        alertar(response);

                    }

                }
            });

        }

        $(function () {

            $('.data').datepicker({ dateFormat: 'dd/mm/yy' });

            var auth = "<%= Request.Cookies[FormsAuthentication.FormsCookieName]==null ? string.Empty : Request.Cookies[FormsAuthentication.FormsCookieName].Value %>";
            var ASPSESSID = "<%= Session.SessionID %>";

            var message = $("#message");

            // Inicializa o uploadify
            $("#filename").uploadify({
                uploader: '<%= Url.Content("~/Scripts/Uploadify/uploadify.swf") %>',
                script: '<%= Url.Action("Upload")%>',
                buttonImg: '<%= Url.Content("~/images/bt_selecionar.png") %>',
                multi: false,
                auto: true,
                width: 170,
                height: 30,
                scriptData: { ASPSESSID: ASPSESSID, AUTHID: auth },
                cancelImg: '<%= Url.Content("~/images/bt_lixeira.png") %>',
                fileDataName: 'fileData',
                fileDesc: 'Arquivo de Excel (*.xls, *.xlsx)',
                fileExt: '*.xls;*.xlsx',
                onError: function (event, queue, fileInfo, error) {
                    // Display the error message in red.
                    message.css("color", "red");
                    message.html("Error " + error.type + ": " + error.text);
                },
                onComplete: function (event, queueId, fileObj, response, data) {

                    //verifica se foi enviado corretamente
                    if (response.indexOf("[ok]") != -1) {

                        VerificaExcel();

                    } else {

                        alertar(response);

                    }

                }
            });

            // Inicializa o uploadify
            $("#filename2").uploadify({
                uploader: '<%= Url.Content("~/Scripts/Uploadify/uploadify.swf") %>',
                script: '<%= Url.Action("UploadTerritorios")%>',
                buttonImg: '<%= Url.Content("~/images/bt_selecionar.png") %>',
                multi: false,
                auto: true,
                width: 170,
                height: 30,
                scriptData: { ASPSESSID: ASPSESSID, AUTHID: auth },
                cancelImg: '<%= Url.Content("~/images/bt_lixeira.png") %>',
                fileDataName: 'fileData',
                fileDesc: 'Arquivo de Excel (*.xls, *.xlsx)',
                fileExt: '*.xls;*.xlsx',
                onError: function (event, queue, fileInfo, error) {
                    // Display the error message in red.
                    message.css("color", "red");
                    message.html("Error " + error.type + ": " + error.text);
                },
                onComplete: function (event, queueId, fileObj, response, data) {

                    //verifica se foi enviado corretamente
                    if (response.indexOf("[ok]") != -1) {

                        VerificaExcelTerritorios();

                    } else {

                        alertar(response);

                    }

                }
            });

            // Inicializa o uploadify
            $("#filename3").uploadify({
                uploader: '<%= Url.Content("~/Scripts/Uploadify/uploadify.swf") %>',
                script: '<%= Url.Action("UploadOrdemProdutos") %>',
                buttonImg: '<%= Url.Content("~/images/bt_selecionar.png") %>',
                multi: false,
                auto: true,
                width: 170,
                height: 30,
                scriptData: { ASPSESSID: ASPSESSID, AUTHID: auth },
                cancelImg: '<%= Url.Content("~/images/bt_lixeira.png") %>',
                fileDataName: 'fileData',
                fileDesc: 'Arquivo de Excel (*.xls, *.xlsx)',
                fileExt: '*.xls;*.xlsx',
                onError: function (event, queue, fileInfo, error) {
                    // Display the error message in red.
                    message.css("color", "red");
                    message.html("Error " + error.type + ": " + error.text);
                },
                onComplete: function (event, queueId, fileObj, response, data) {

                    //verifica se foi enviado corretamente
                    if (response.indexOf("[ok]") != -1) {

                        VerificaExcelOrdemProdutos();

                    } else {

                        alertar(response);

                    }

                }
            });

            // Inicializa o uploadify
            $("#filename4").uploadify({
                uploader: '<%= Url.Content("~/Scripts/Uploadify/uploadify.swf") %>',
                script: '<%= Url.Action("UploadDoutores") %>',
                buttonImg: '<%= Url.Content("~/images/bt_selecionar.png") %>',
                multi: false,
                auto: true,
                width: 170,
                height: 30,
                scriptData: { ASPSESSID: ASPSESSID, AUTHID: auth },
                cancelImg: '<%= Url.Content("~/images/bt_lixeira.png") %>',
                fileDataName: 'fileData',
                fileDesc: 'Arquivo de Excel (*.xls, *.xlsx)',
                fileExt: '*.xls;*.xlsx',
                onError: function (event, queue, fileInfo, error) {
                    // Display the error message in red.
                    message.css("color", "red");
                    message.html("Error " + error.type + ": " + error.text);
                },
                onComplete: function (event, queueId, fileObj, response, data) {

                    //verifica se foi enviado corretamente
                    if (response.indexOf("[ok]") != -1) {

                        VerificaExcelDoutores();

                    } else {

                        alertar(response);

                    }

                }
            });

            // Inicializa o uploadify
            $("#filename5").uploadify({
                uploader: '<%= Url.Content("~/Scripts/Uploadify/uploadify.swf") %>',
                script: '<%= Url.Action("UploadGerentesRep")%>',
                buttonImg: '<%= Url.Content("~/images/bt_selecionar.png") %>',
                multi: false,
                auto: true,
                width: 170,
                height: 30,
                scriptData: { ASPSESSID: ASPSESSID, AUTHID: auth },
                cancelImg: '<%= Url.Content("~/images/bt_lixeira.png") %>',
                fileDataName: 'fileData',
                fileDesc: 'Arquivo de Excel (*.xls, *.xlsx)',
                fileExt: '*.xls;*.xlsx',
                onError: function (event, queue, fileInfo, error) {
                    // Display the error message in red.
                    message.css("color", "red");
                    message.html("Error " + error.type + ": " + error.text);
                },
                onComplete: function (event, queueId, fileObj, response, data) {

                    //verifica se foi enviado corretamente
                    if (response.indexOf("[ok]") != -1) {

                        VerificaExcelGerentesRep();

                    } else {

                        alertar(response);

                    }

                }
            });

        });
        
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="titulo">
        <h2>Importação</h2>
    </div>
    
    <style>
        label{
            display: block;
            background: none;
            clear: both;
        }
    </style>
    
    <div class="top-tabela-cinza-cadastro conteudo-baixo" style="padding: 10px; min-height: 400px;">

            <h1>Gerentes/Agências</h1>

    	       
            <div id="importarPlanilha" style="clear: both">
                <br />
                <input type="file" id="filename" name="filename" /> 
            </div>
               
            <div class="message" style="display: none;">
			</div>

            <p style="clear: both">
            <br />Está com dificuldades na importação? <a href="<%=Url.Content("~/arquivos/gerenciador_ofertas/modelo.xls") %>">Baixe o modelo de planilha</a>, o nome das colunas devem estar como no modelo, sem acentuação, e os valores digitados corretamente.
            </p>
               
            <div class="sub-menu" style="clear: both;">
                    
                <div style="float: left; display: none;" class="publicacao">
                    <a href="#" id="cancelar">Cancelar</a>
                    <a href="#" id="publicar">Publicar</a>
                </div>
            </div>

            <br /><br />
            <h1>Territórios</h1>

            <div id="Div1" style="clear: both">
                <br />
                <input type="file" id="filename2" name="filename" /> 
            </div>

            <div class="messageTerritorios" style="display: none;">
			</div>

            <p style="clear: both">
            <br />Está com dificuldades na importação? <a href="<%=Url.Content("~/arquivos/gerenciador_ofertas/modelo.xls") %>">Baixe o modelo de planilha</a>, o nome das colunas devem estar como no modelo, sem acentuação, e os valores digitados corretamente.
            </p>

            <br /><br />
            <h1>Ordem Produtos</h1>

            <div id="Div2" style="clear: both">
                <br />
                <input type="file" id="filename3" name="filename" /> 
            </div>

            <div class="messageOrdemProdutos" style="display: none;">
			</div>

            <p style="clear: both">
            <br />Está com dificuldades na importação? <a href="<%=Url.Content("~/arquivos/gerenciador_ofertas/modelo.xls") %>">Baixe o modelo de planilha</a>, o nome das colunas devem estar como no modelo, sem acentuação, e os valores digitados corretamente.
            </p>

            <br /><br />
            <h1>Doutores</h1>

            <div id="Div3" style="clear: both">
                <br />
                <input type="file" id="filename4" name="filename" /> 
            </div>

            <div class="messageDoutores" style="display: none;">
			</div>

            <p style="clear: both">
            <br />Está com dificuldades na importação? <a href="<%=Url.Content("~/arquivos/gerenciador_ofertas/modelo.xls") %>">Baixe o modelo de planilha</a>, o nome das colunas devem estar como no modelo, sem acentuação, e os valores digitados corretamente.
            </p>

            <br /><br />
            <h1>Gerentes Rep</h1>

            <div id="Div4" style="clear: both">
                <br />
                <input type="file" id="filename5" name="filename" /> 
            </div>

            <div class="messageGerentesRep" style="display: none;">
			</div>

            <p style="clear: both">
            <br />Está com dificuldades na importação? <a href="<%=Url.Content("~/arquivos/gerenciador_ofertas/modelo.xls") %>">Baixe o modelo de planilha</a>, o nome das colunas devem estar como no modelo, sem acentuação, e os valores digitados corretamente.
            </p>
        
    </div>
    
    

</asp:Content>


