<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Home
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">

        function carregaHistorico() {

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetHistorico") %>',
                cache: false,
                dataType: 'html',
                success: function (data) {

                    $('.carregando').hide();

                    if (data == '') {
                        $('#tabelaVasPendentes').hide();
                        $('#divSemVaPendente').show();
                    } else {

                        $('#vasPendentes').html(data);

                        $('#tabelaVasPendentes').show();
                        $('#divSemVaPendente').hide();
                    }

                    //atualiza tablesorter
                    $(".tabela").trigger("update");

                }
            });

        }

        $(function () {

            carregaHistorico();

        });
        
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<%

    var vas = (IEnumerable<Models.ProdutoVa>)ViewData["vas"];

%>

    <div class="titulo">
        <h2>Home</h2>
    </div>
    
    <div class="conteudo-baixo vas-visualizar">
        
        <h2 style="margin-top: 0;">Histórico de Envios</h2>

        <%--<a class="botao" href="javascript:AdicionarSlide();">Adicionar Slide</a>--%>
        <br /><br /><br />

        <div class="tabela-container">
            
            <div id="divSemVaPendente" style="padding: 10px 10px 0pt; display: none;">Nenhum va encontrado</div>

            <div class="carregando">Carregando...</div>

            <table id="tabelaVasPendentes" class="tabela tablesorter" style="display: none;">
                <thead>
                    <tr>
                        <th>
                            Data
                        </th>
                        <th>
                            Produto
                        </th>
                        <th>
                            Slides
                        </th>
                        <th>
                            Status Atual
                        </th>
                    </tr>
                </thead>
                <tbody id="vasPendentes">
                    <tr style="background-color: White;">
                        <th colspan="6">
                            &nbsp;
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>

    </div>

</asp:Content>