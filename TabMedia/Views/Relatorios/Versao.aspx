<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Produto>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Relatórios
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">

        $(function () {

            GetTerritorios();

            $('#ddlTerritorio').change(function () {

                if ($(this).val() == '') {
                    $('#aFiltrar').attr('href', '<%=Url.Action("index") %>');
                } else {

                    $('#aFiltrar').attr('href', '<%=Url.Action("index") %>' + '?idTerritorio=' + $(this).val());
                }
            });

        });

        function GetTerritorios() {

            $('.carregando').show();

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetRelatorioVersao")%>',
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

                        $('.tabelas').append(data);

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
            
            <div id="divSemArquivo" style="padding: 10px 10px 0pt; display: none;">Nenhum Territorio encontrado.</div>

            <div class="carregando" style="clear: both;">Carregando...</div>

            <div class="tabelas"></div>

        </div>

    </div>

</asp:Content>
