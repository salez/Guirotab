<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Produto>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Relatórios
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">

        $(function () {

            getLinhas();

        });

        function getLinhas() {

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetTerritoriosLinhas")%>',
                cache: false,
                dataType: 'html',
                data: {
                },
                success: function (data) {

                    $('.carregando').hide();

                    if (data == '') {
                        $('#divSemArquivo').show();
                    } else {

                        //$('.tabelas').html('<a class="botao" href="<%=Url.Action("GerarExcelLinhas") %>">Gerar Relatório</a><br/><br/>');
                        $('#selectLinhas').html(data);

                        $('#divSemArquivo').hide();
                    }

                    tableSort();
                    updateTableSort();

                }
            });

        }

        function GetTerritoriosLinha(idLinha) {

            $('.carregando').show();
            $('.tabelas').html('');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetTerritoriosLinha")%>',
                data: {
                    idLinha: idLinha
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
            
            <div id="selectLinhas">
            </div>

            <div id="divSemArquivo" style="padding: 10px 10px 0pt; display: none;">Nenhum registro encontrado.</div>

            <div class="carregando">Carregando...</div>

            <div class="tabelas"></div>

        </div>

    </div>

</asp:Content>
