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

        function GetProdutos() {

            $('.carregando').show();

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetRelatoriosProdutos")%>',
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

                        $('.tabelas').html('<a class="botaoExportarExcel" style="margin:10px 0;" href="<%=Url.Action("GerarExcelProdutos") %>?dataInicial=' + $('#DataDe').val() + '&dataFinal=' + $('#DataAte').val() + '">Exportar Excel</a>');
                        $('.tabelas').append(data);

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

                case 'relProduto':
                    //alert('relProduto');
                    var id = $.address.parameter('i');
                    GetRelatorioProduto(id);
                    break;

                case 'relVa':
                    //alert('relVa');
                    var id = $.address.parameter('i');
                    GetRelatorioVa(id);
                    break;

                default:
                    //alert('default');
                    GetProdutos()
                    break;

            }

            $('.tabela a, h2 a').address(function() {  
                return $(this).attr('href').replace(/^#/, '');  
            });

            // do something depending on the event.value property, e.g.  
            // $('#content').load(event.value + '.xml');  
        });  

        function GetRelatorioProduto(idProduto) {

            $('.carregando').show();
            $('.tabelas').html('');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetRelatorioProduto")%>',
                data: {
                    idProduto: idProduto
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

        function GetRelatorioVa(idVa) {

            $('.carregando').show();
            $('.tabelas').html('');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetRelatorioVa")%>',
                data: {
                    idVa: idVa
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
        
        <%--<center><img src="<%=Url.Action("GerarGrafico") %>" /></center>--%>

        <div class="tabela-container">
            
            <div id="divSemArquivo" style="padding: 10px 10px 0pt; display: none;">Nenhum Produto relacionado.</div>

            <div class="filtros" style="width: 100%; clear: both;">
                <table>
                    <tr>
                        <td class="semBarra" valign="middle">
                            <label><b>Data Inicial&nbsp;&nbsp;</b><input type="text" class="data" id="DataDe" name="DataDe" value="<%=DateTime.Now.AddMonths(-1).Formata(Util.Data.FormatoData.DiaMesAno) %>" /></label>
                        </td>
                        <td class="semBarra" valign="middle">
                            <label><b>Data Final&nbsp;&nbsp;</b><input type="text" class="data" id="DataAte" name="DataAte" value="<%=DateTime.Now.Formata(Util.Data.FormatoData.DiaMesAno) %>" /></label>
                        </td>
                        <td class="semBarra" valign="middle">
                            &nbsp;&nbsp;
                            <input type="image" value="filtrar" onclick="GetProdutos();" class="botaoFiltrar2" />
                        </td>
                    </tr>
                    <tr>
                    	<td class="semBarra" colspan="3" valign="middle"><b><i>deixe os campos em branco para retornar todos</i></b></td>
                    </tr>
                </table>
                <h4></h4>
            </div>
         
            <div class="carregando" style="clear: both;">Carregando...</div>
			
            <div class="tabelas"></div>
            
        </div>

    </div>

</asp:Content>
