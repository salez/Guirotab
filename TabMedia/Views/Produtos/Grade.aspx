<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.Produto>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Produtos
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    
    <script type="text/javascript">

        $(function () {

            ////////////// DIALOGS ///////////////

            //cria o dialog de adicionar slide
            $("#modalAdicionarProduto").dialog({
                title: "Adicionar Produto",
                height: 250,
                width: 400,
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
                    Cancelar: function () {
                        $('#modalAdicionarProduto').dialog('close');
                    },
                    Adicionar: function () {

                        alertar('Adicionando Produto...');

                        $('#frmAdicionarProduto').ajaxSubmit({
                            cache: false,
                            dataType: 'html',
                            success: function (data) {
                                $("#modalAdicionarProduto").dialog('close');
                                carregaGrade();
                                alertar('Produto Adicionado!');
                            }
                        });


                    }
                }
            });



            //cria o dialog de adicionar slide
            $("#modalAdicionar").dialog({
                title: "Adicionar",
                height: 250,
                width: 400,
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
                    Cancelar: function () {
                        $('#modalAdicionar').dialog('close');
                    },
                    Adicionar: function () {
                        alertar('Adicionando Linha...');
                        $('#frmAdicionar').ajaxSubmit({
                            cache: false,
                            dataType: 'html',
                            success: function (data) {
                                $("#modalAdicionar").dialog('close');
                                carregaGrade();
                                alertar('Linha Adicionada a Grade!');
                            }
                        });


                    }
                }
            });

            ///// GERAL

            carregaGrade();

        });

        function AdicionarProduto() {
            $('#modalAdicionarProduto').dialog('open');
        }
      

        function AdicionarGrade() {
            $('#modalAdicionar').dialog('open');
        }

        var visualizar = false;

        function AlterarGrade() {
            $('select').toggle();

            if (visualizar) {
                $('#salvar').show();
            } else {
                $('#salvar').hide();
            }

            if ($("#alterar").text() == 'Alterar Grade') {
                $("#alterar").text('Visualizar');
                visualizar = true;
            } else {
                $("#alterar").text('Alterar Grade');
                visualizar = false;
            }

            
        }

        function Salvar() {
            alertar('Salvando Grade...');
            $('#frmGrade').ajaxSubmit({
                cache: false,
                dataType: 'html',
                success: function (data) {
                    alertar('Grade Alterada!');
                    carregaGrade();
                }
            });
        }

        function carregaGrade() {

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetGrade") %>',
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

                    $('select').change(function () {

                        if ($(this).parent().find('span').length > 0) {

                            $(this).parent().find('span').text($(this).find('option:selected').text());

                        }

                    });

                    visualizar = false;
                    $('#salvar').hide();
                }
            });

            

        }
      
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Produtos - Grade</h2>
    </div>

    <div class="conteudo-baixo vas-visualizar">
        
        <form id="frmGrade" method="post" action="<%=Url.Action("SalvarGrade") %>">

            <div style="float: right;">
                <a class="botao" href="javascript:AdicionarProduto();">Cadastrar Produto</a>
            </div>
            <div style="float: left;">
                <a class="botao" href="javascript:AdicionarGrade();">Adicionar</a>
                <a class="botao" href="javascript:AlterarGrade();" id="alterar">Alterar Grade</a>
                <a class="botao" href="javascript:Salvar();" id="salvar" style="display: none;">Salvar</a>
            </div>
            
            <br /><br />

            <div class="tabela-container">
            
                <div id="divSemArquivo" style="padding: 10px 10px 0pt; display: none;">Sem grade criada.</div>

                <div class="carregando">Carregando...</div>

                <div class="tabelas"></div>

            </div>

        </form>

    </div>

    <%//++ Modal Especialidades %>

	<div id="modalAdicionar">
			
		<form id="frmAdicionar" action="<%=Url.Action("GradeAdicionar") %>" method="post">

            Linha:<br />
            <%=Html.DropDownList("idLinha",(SelectList)ViewData["linhas"],"Selecione") %>

            <br /><br />

            Especialidade:<br />
            <%=Html.DropDownList("idEspecialidade",(SelectList)ViewData["especialidades"],"Selecione") %>

        </form>

	</div>

    <%//++ Modal Adicionar Produto %>

	<div id="modalAdicionarProduto">
			
		<form id="frmAdicionarProduto" action="<%=Url.Action("GradeAdicionarProduto") %>" method="post">
            
            Nome:<br />
            <%=Html.TextBoxFor(model => model.Nome) %>

            <br /><br />

            Linha:<br />
            <%=Html.DropDownListFor(model => model.IdLinha,(SelectList)ViewData["linhas"],"Selecione") %>


        </form>

	</div>

</asp:Content>
