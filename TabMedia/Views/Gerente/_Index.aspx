<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Home
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">


        function relatorioComoFunciona() {

            $("#modalComoFuncionaRelatorios").dialog('open');

        }

        function excluirVa(id, data) {

            $("#dialog-confirm").html('<center>Tem certeza de que deseja excluir o VA incluído em <font color=green> \''+data+'\'</font>?</center>');

            $("#dialog-confirm").dialog({
                title: 'Atenção',
                resizable: false,
                height: 140,
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
                            data: { idVa: id },
                            cache: false,
                            success: function (data) {

                                carregaVasPendentes();

                                divDialog.dialog("close");

                            }
                        });

                    }
                }
            });

        }

        function carregaVasPendentes() {

            $.ajax({
                type: 'get',
                url: '<%=Url.Action("GetVasPendentes","Gerente") %>',
                cache: false,
                dataType: 'html',
                success: function (data) {

                    $('.carregando').hide();

                    if (data == '') {

                        $('#vasPendentesContainer').hide();
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

        function formatText(index, panel) {
            return index + "";
        };

        function FarmaciaProximo() {
            $('#como-funciona-relatorios').data('AnythingSlider').goForward();
        };

        function FarmaciaAnterior() {
            $('#como-funciona-relatorios').data('AnythingSlider').goBack();
        };

        $(function () {

            //cria o dialog de adicionar slide
            $("#modalComoFuncionaRelatorios").dialog({
                title: "Como Funciona?",
                width: 1000,
                height: 720,
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
                open: function () {

                    setTimeout(function () {
                        $('#como-funciona-relatorios').anythingSlider({
                            easing: "easeInOutExpo",
                            autoPlay: false,
                            delay: 8000,
                            startStopped: false,
                            animationTime: 500,
                            hashTags: false,
                            buildNavigation: false,
                            pauseOnHover: false,
                            startText: "Go",
                            stopText: "Stop",
                            navigationFormatter: formatText
                        });
                    }, 500);

                }
            });

            carregaVasPendentes();

        });
        
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Home</h2>
    </div>
    
    <div class="conteudo-baixo vas-visualizar">
        
        <div id="vasPendentesContainer">

        <h2 style="margin-top: 0;">VA's Pendentes</h2>

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
                            Status
                        </th>
                        <th class="acao" style='width: 100px;'>&nbsp;</th>
                    </tr>
                </thead>
                <tbody id="vasPendentes">
                    <tr style="background-color: White;">
                        <th colspan="6">&nbsp;
                            
                        </th>
                    </tr>
                </tbody>
            </table>
        </div>
        
        <br /><br />

        </div>

        	<div id="home_gerente_esquerda">
        		<h2 style="margin-top: 0;">NOVO MÓDULO: Relatórios Estatísticos</h2>
        		A sua fonte de informações para criar campanhas de forma estratégica! 
        		<br /><br /> 
				<img class="png" style="border:none;" src="<%=Url.Content("~/") %>Images/home_tres_imagens.png" />
                <br /><a href="javascript:relatorioComoFunciona();"><img class="png" style="padding-top:5px; border:none;" src="Images/home_saiba_mais.jpg" /></a>
        	</div>
            <div id="home_gerente_direita">
            	<h2>INSTALAÇÃO DO APLICATIVO</h2>
                <p>Para instalar o aplicativo no seu iPad é necessário instalar o aplicativo e depois retornar a essa página para habilitá-lo para iniciar a sua utilização.</p>
                <p>1. Clique no botão abaixo para iniciar a instalação</p>
                <a class="botao_instalar" href="itms-services://?action=download-manifest&url=http://guiropatab.com/app/emspresentations.plist" target="_blank">Instalar</a>
                <p>2. Após finalizar a instalação, clique no botão abaixo para habilitar o aplicativo</p>
                <a class="botao_habilitar" href="emspresentations://update?territory=<%=Sessao.Site.RetornaUsuario().TerritorioSimulado %>" target="_blank">Habilitar</a>
            </div>
		
        <%// Modal Como Funciona %>

        <div id="modalComoFuncionaRelatorios">

            
            <a href="javascript:FarmaciaAnterior();" class="anterior" style="float: left; width: 50px; position: relative; top: 300px;">
				<img src="<%=Url.Content("~/") %>images/anterior.gif" alt="Anterior" />
			</a>


			<div class="">

				<div class="anythingSliderFarmacia" id="como-funciona-relatorios" style="float: left;">
					<div class="wrapper">
                        <ul>
						<!--pre-visualizar-slides-->
                        <li>
                            <img src="<%=Url.Content("~/") %>images/novi01.jpg" />
                        </li>
                        <li>
                             <img src="<%=Url.Content("~/") %>images/novi02.jpg" />
                        </li>
                        <li>
                            <img src="<%=Url.Content("~/") %>images/novi03.jpg" />
                        </li>
                        <li>
                            <img src="<%=Url.Content("~/") %>images/novi04.jpg" />
                        </li>
                        </ul>
					</div>
				</div>

			</div>

			<a href="javascript:FarmaciaProximo();" class="proximo" style="float: left; width: 50px; position: relative; top: 300px; margin-left: 30px;">
				<img src="<%=Url.Content("~/") %>images/proximo.gif" alt="Próximo" />
			</a>

        </div>

        <%--<br /><br />

        <h2 style="margin-top: 0;">Avisos</h2>

        <br /><br />

        <div class="conteudo-secundario-container" style="float: left; width: 950px;">
            <div class="conteudo-secundario">
                
                <center>Nenhum Aviso</center>

            </div>
        </div>--%>

    </div>

</asp:Content>