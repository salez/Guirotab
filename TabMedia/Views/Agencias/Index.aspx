<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Agências
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">

        function carregaAgencias() {

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetAgencias") %>',
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

                    updateTableSort();

                }
            });

        }

        function editar(idAgencia) {

            $("#idAgencia").val(idAgencia);

            $(".dialog").dialog("open");

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetProdutos") %>',
                cache: false,
                dataType: 'html',
                data: {
                    idAgencia: idAgencia
                },
                success: function (data) {

                    $('#produtos').html(data);

                }
            });

        }

        $(function () {

            carregaAgencias();

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
                            carregaAgencias();
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
        <h2>Agências</h2>
    </div>
    
    <div class="conteudo-baixo vas-visualizar">

        <div class="tabela-container">
            
            <div id="divSemVaPendente" style="padding: 10px 10px 0pt; display: none;">Nenhuma agência encontrada</div>

            <div class="carregando">Carregando...</div>

            <table id="tabelaVasPendentes" class="tabela tablesorter" style="display: none;">
                <thead>
                    <tr>
                        <th>
                            Agência
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
                <tbody id="vasPendentes">
                    <tr style="background-color: White;">
                        <th colspan="6">
                            &nbsp;
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="dialog">
        
            <form id="frmProdutos" action="<%=Url.Action("AtualizaProdutosAgencia") %>" method="post">
            
                <input type="hidden" id="idAgencia" name="idAgencia" />

                <div id="produtos">
        
                </div>

            </form>

        </div>

    </div>

</asp:Content>