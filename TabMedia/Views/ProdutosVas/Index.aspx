<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Produto>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Produtos
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">
        
        function excluirVa(id, data) {

            $("#dialog-confirm").html('<center>Tem certeza de que deseja excluir o VA incluído em <font color=green> \''+data+'\'</font>?</center>');

            $("#dialog-confirm").dialog({
                title: 'Atenção',
                resizable: false,
                height: 210,
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
                    "Excluir": function () {

                        var divDialog = $(this);

                        divDialog.html('<center>Excluindo VA...</center>');

                        $.ajax({
                            type: 'post',
                            url: '<%=Url.Action("ExcluirVA") %>',
                            data: { idVa: id, idProduto: <%=Model.Id %> },
                            cache: false,
                            success: function (data) {

                                carregaVas();

                                divDialog.dialog("close");

                            }
                        });

                    }
                }
            });

        }

        function carregaVas() {
            
            $('.tabela-container').each(function(){
                
                var tabela = $(this);
                
                $.ajax({
                    type: 'get',
                    url: '<%=Url.Action("GetVas",new { id = Model.Id} ) %>?idcategoria='+tabela.find('.categoriaId').val(),
                    cache: false,
                    dataType: 'html',
                    success: function (data) {

                        tabela.find('.carregando').hide();

                        if (data == '') {
                            tabela.find('.tabelaArquivos').hide();
                            tabela.find('.divSemArquivo').show();
                        } else {

                            tabela.find('.arquivos').html(data);

                            //alert(tabela.find('.arquivos').html());

                            tabela.find('.tabelaArquivos').show();
                            tabela.find('.divSemArquivo').hide();
                        }

                        updateTableSort();

                    }
                });

            });

        }

        $(function () {

            carregaVas();

        });

    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Produtos - <%=Model.Nome %> - VA's </h2>
    </div>

    <div class="conteudo-baixo vas-visualizar">
        
        <%
            foreach (var categoria in (IQueryable<Models.ProdutoVaCategoria>)ViewData["categorias"])
            { 
             %>

            <div class="titulo">

                <h2><%=categoria.Nome %></h2>

            </div>

            <a class="botao" href="<%=Url.Action("cadastro",new { id = Model.Id, idCategoria = categoria.Id}) %>">Adicionar</a>

            <br /><br />

            <div class="tabela-container">
                
                <input type="hidden" value="<%=categoria.Id %>" class="categoriaId" />

                <div class="divSemArquivo" style="padding: 10px 10px 0pt; display: none;">Nenhum registro encontrado</div>

                <div class="carregando">Carregando...</div>

                <table class="tabelaArquivos tabela tablesorter" style="display: none;">
                    <thead>
                        <tr>
                            <th style="width: 100px;">
                                Data
                            </th>
                            <th style="width: 80px;">
                                Slides
                            </th>
                            <th style="width: 80px;">
                                Versão
                            </th>
                            <th style="text-align: left;">
                                Nome
                            </th>
                            <th style="width: 200px;">
                                Status
                            </th>
                            <th class="acao" style="width: 250px;">&nbsp;</th>
                        </tr>
                    </thead>
                    <tbody class="arquivos">
                        <tr style="background-color: White;">
                            <th colspan="6">
                                &nbsp;
                            </th>
                        </tr>
                    </tbody>
                </table>
            </div>

            <br /><br />
        <%
        }
        %>

    </div>

</asp:Content>
