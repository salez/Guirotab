<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Produto>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Relatórios
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">

        $(function () {

            ////////////// DIALOGS ///////////////

            //cria o dialog de adicionar arquivos
            $("#AdicionarArquivo").dialog({
                title: "Adicionar Arquivo",
                height: 400,
                width: 530,
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

            $("#upload-file-slides").click(function () {
                $("#uploadifySlides").uploadifyUpload();
            });

            $("#upload-file-arquivos").click(function () {
                $("#uploadifyArquivos").uploadifyUpload();
            });
            
            $('#ddlTerritorio').change(function () {

                if ($(this).val() == '') {
                    $('#aFiltrar').attr('href', '<%=Url.Action("index") %>');
                } else {

                    $('#aFiltrar').attr('href', '<%=Url.Action("index") %>' + '?idTerritorio=' + $(this).val());
                }
            });

        });

        function GetDoutores() {

            $('.carregando').show();

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetRelatoriosDoutores")%>',
                cache: false,
                dataType: 'html',
                data: {
                    dataInicial: $('#DataDe').val()
                    ,dataFinal: $('#DataAte').val()
                },
                success: function (data) {

                    $('.carregando').hide();

                    if (data == '') {
                        $('#divSemArquivo').show();
                    } else {

                        //$('.tabelas').html('<a class="botao" href="<%=Url.Action("GerarExcelDoutores") %>?dataInicial=' + $('#DataDe').val() + '&dataFinal=' + $('#DataAte').val() + '">Gerar Relatório</a><br/><br/>');
                        $('.tabelas').html(data);

                        $('#divSemArquivo').hide();
                    }

                    tableSort();
                    updateTableSort();

                }
            });
        }

        //ocorre em qlqr mudança ajax e ao carregar a pagina
        $.address.change(function (event) {

            switch ($.address.parameter('n')) {

                case 'relDoutor':
                    //alert('relProduto');
                    var idDoutor = $.address.parameter('i');
                    
                    GetRelatorioProdutos(idDoutor);
                    break;

                case 'relProduto':
                    //alert('relProduto');
                    var idProduto = $.address.parameter('i');
                    var idDoutor = $.address.parameter('d');
                    GetRelatorioProduto(idProduto, idDoutor);
                    break;

                case 'relVa':
                    //alert('relVa');
                    var id = $.address.parameter('i');
                    var idDoutor = $.address.parameter('d');
                    GetRelatorioVa(id,idDoutor);
                    break;

                default:
                    //alert('default');
                    GetDoutores()
                    break;

            }

            $('.tabelas a, h2 a').address(function() {  
                return $(this).attr('href').replace(/^#/, '');  
            });

            // do something depending on the event.value property, e.g.  
            // $('#content').load(event.value + '.xml');  
        });  

        function GetRelatorioProdutos(idDoutor) {

            $('.carregando').show();
            $('.tabelas').html('');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetRelatoriosProdutos")%>',
                data: {
                    idDoutor: idDoutor
                    , dataInicial: $('#DataDe').val()
                    , dataFinal: $('#DataAte').val()
                },
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

        function GetRelatorioProduto(idProduto, idDoutor) {

            $('.carregando').show();
            $('.tabelas').html('');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetRelatorioProduto")%>',
                data: {
                    idProduto: idProduto
                    , idDoutor: idDoutor
                    , dataInicial: $('#DataDe').val()
                    , dataFinal: $('#DataAte').val()
                },
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

        function GetRelatorioVa(idVa, idDoutor) {

            $('.carregando').show();
            $('.tabelas').html('');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetRelatorioVa")%>',
                data: {
                    idVa: idVa
                    ,idDoutor: idDoutor
                    ,dataInicial: $('#DataDe').val()
                    ,dataFinal: $('#DataAte').val()
                },
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
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
    	<h4></h4>
        <h2>Relatórios</h2>
        <h4></h4>
    </div>

    <div class="conteudo-baixo">

        <div class="tabela-container">
            
            <div id="divSemArquivo" style="padding: 10px 10px 0pt; display: none;">Nenhum Produto relacionado.</div>

            <div class="filtros">
                <table>
    				<tr>
        				<th class="linhaCinza"></th>
        			</tr>
                    <tr>
                        <td class="semBarra" valign="middle">
                            <label>Data Inicial&nbsp;&nbsp;<input type="text" class="data" id="DataDe" name="DataDe" /></label>
                        </td>
                        <td class="semBarra" valign="middle">
                            <label>Data Final&nbsp;&nbsp;<input type="text" class="data" id="DataAte" name="DataAte" /></label>
                        </td>
                        <td class="semBarra" valign="middle">
                            &nbsp;&nbsp;
                            <input type="button" value="filtrar" onclick="GetProdutos();" class="botao" />
                            <i>deixe os campos em branco para retornar todos</i>
                        </td>
                    </tr>
    				<tr>
        				<th class="linhaCinza"></th>
        			</tr>
    				<tr>
        				<th class="linhaBranco"></th>
        			</tr>
                </table>
            </div>

            <div class="carregando">Carregando...</div>

            <div class="tabelas"></div>

        </div>

    </div>

</asp:Content>
