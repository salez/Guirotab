<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Gerentes
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">

        function carregaGerentesProduto() {

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetGerentes") %>',
                cache: false,
                dataType: 'html',
                success: function (data) {

                    $('.carregando').hide();

                    if (data == '') {
                        $('#tabelaVasPendentes').hide();
                        $('#divSemVaPendente').show();
                    } else {

                        $('#gerentesProduto').html(data);

                        $('#tabelaVasPendentes').show();
                        $('#divSemVaPendente').hide();
                    }

                    updateTableSort();

                }
            });

        }

        function carregaGerentesMarketing() {

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetGerentesMarketing") %>',
                cache: false,
                dataType: 'html',
                success: function (data) {

                    $('.carregando').hide();

                    if (data == '') {
                        $('#tabelaGerentesMarketing').hide();
                        $('#divSemGerenteMarketing').show();
                    } else {

                        $('#gerentesMarketing').html(data);

                        $('#tabelaGerentesMarketing').show();
                        $('#divSemGerenteMarketing').hide();
                    }

                    updateTableSort();

                }
            });

        }

        function editar(idGerente) {

            $("#idGerente").val(idGerente);

            $(".dialog").dialog("open");

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetProdutos") %>',
                cache: false,
                dataType: 'html',
                data: {
                    idGerente: idGerente
                },
                success: function (data) {

                    $('#produtos').html(data);

                }
            });

        }

        $(function () {

            carregaGerentesProduto();
            carregaGerentesMarketing();

            //cria o dialog
            $(".dialog").dialog({
                title: "Editar Produtos",
                height: 500,
                width: 500,
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

                    "Cancelar": function () {
                        $(this).dialog("close");
                    },
                    "Salvar": function () {

                        alertar("Salvando");

                        $('#frmProdutos').ajaxSubmit(function () {
                            fecharAlerta();
                            carregaGerentesProduto();
                            carregaGerentesMarketing();
                        });

                        $(this).dialog("close");

                    }

                }

            });

        });
        
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Gerentes</h2>
    </div>
    
    <div class="conteudo-baixo vas-visualizar">
        
        <h2>Gerentes de Produto</h2>

        <div class="tabela-container">
            
            <div id="divSemVaPendente" style="padding: 10px 10px 0pt; display: none;">Nenhum gerente encontrado</div>

            <div class="carregando">Carregando...</div>

            <table id="tabelaVasPendentes" class="tabela tablesorter" style="display: none;">
                <thead>
                    <tr>
                        <th>
                            Gerente
                        </th>
                        <th>
                            Email
                        </th>
                        <th>
                            Produtos
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody id="gerentesProduto">
                    <tr style="background-color: White;">
                        <th colspan="6">
                            &nbsp;
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>

        <h2>Gerentes de Marketing</h2>

        <div class="tabela-container">
            
            <div id="divSemGerenteMarketing" style="padding: 10px 10px 0pt; display: none;">Nenhum gerente encontrado</div>

            <div class="carregando">Carregando...</div>

            <table id="tabelaGerentesMarketing" class="tabela tablesorter" style="display: none;">
                <thead>
                    <tr>
                        <th>
                            Gerente
                        </th>
                        <th>
                            Email
                        </th>
                        <th>
                            Produtos
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody id="gerentesMarketing">
                    <tr style="background-color: White;">
                        <th colspan="6">
                            &nbsp;
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="dialog">
        
            <form id="frmProdutos" action="<%=Url.Action("AtualizaProdutosGerente") %>" method="post">
            
                <input type="hidden" id="idGerente" name="idGerente" />

                <div id="produtos">
        
                </div>

            </form>

        </div>

    </div>

</asp:Content>