<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Home
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">

        //variavel usada para retornar o conteudo das series dos graficos
        var series;

        $(function () {
            GetRelatorioUtilizacao();
            //GetGraficos();

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

        });

        function GetRelatorioUtilizacao() {

            $('.carregando').show();
            $('.tabelas').html('');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetRelatorioUtilizacaoLinhas")%>',
                data: {
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

            $('#ddlHabilitarTipo').change(function () {

                $('.ddlHabilitar').hide();

                if ($(this).val() == 'territorio') {

                    $('#ddlHabilitarTerritorio').show();
                    $('#ddlHabilitarTerritorio').change();

                } else if ($(this).val() == 'gerenteProduto') {

                    $('#ddlHabilitarGerenteProduto').show();
                    $('#ddlHabilitarGerenteProduto').change();

                } else if ($(this).val() == 'gerenteMarketing') {

                    $('#ddlHabilitarGerenteMarketing').show();
                    $('#ddlHabilitarGerenteMarketing').change();

                } else if ($(this).val() == 'agencia') {

                    $('#ddlHabilitarAgencia').show();
                    $('#ddlHabilitarAgencia').change();

                }
            });

            $('.ddlHabilitar').change(function () {

                if ($(this).val() == '') {
                    $('#aHabilitar').attr('href', '');
                } else {

                    $('#aHabilitar').attr('href', 'tabmedia://update?territory=' + $(this).val());
                }
            });

            $('.ddlHabilitar').hide();

            $('#ddlHabilitarTerritorio').show();
            $('#ddlHabilitarTerritorio').change();
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

        function relatorioComoFunciona() {

            $("#modalComoFuncionaRelatorios").dialog('open');

        }

        function GetGraficos() {

            $('.carregando').show();
            $('.tabelas').html('');

            $.ajax({
                type: 'post',
                url: '<%=Url.Action("GetGraficos")%>',
                data: {
                },
                cache: false,
                dataType: 'html',
                success: function (data) {

                    $('#graficos').html(data);

                }
            });
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<!--<div class="titulo">
        <h2>Home</h2>
    </div>
-->
    
    <%--<div style="padding: 40px;">
        <p align="center">Selecione um item do menu.</p>
    </div>
--%>
    <div class="conteudo-baixo">
         <div id="home_gerente">
        	<div id="home_gerente_esquerda">
            	<img src="<%=Url.Content("~/") %>Images/01.jpg" alt="Vitrine" />
        	</div>

            <div id="home_admin_direita">
            	<h2>Instalação do aplicativo</h2>
                <p class="home_admin_p">Para instalar o aplicativo no seu iPad é necessário instalar o aplicativo e depois retornar a essa página para habilitá-lo para iniciar a sua utilização.</p>
                <p class="home_admin_p_numerado">1.</p> <p class="home_admin_p_nao_numerado">Clique no botão abaixo para iniciar a instalação</p>
                <a class="botao_instalar" href="itms-services://?action=download-manifest&url=http://tabmedia.com.br/nycomed/app/2.0.1/manifest.plist" target="_blank">Instalar</a>  
                <p class="home_admin_p_numerado">2.</p> <p class="home_admin_p_nao_numerado">Após finalizar a instalação, clique no botão abaixo para habilitar o aplicativo.</p>

                <p>

                    <select id="ddlHabilitarTipo">
                        <option value="territorio">
                            Território
                        </option>
                        <option value="gerenteProduto">
                            Gerente de Produto
                        </option>
                        <option value="gerenteMarketing">
                            Gerente de Marketing
                        </option>
                        <option value="agencia">
                            Agência
                        </option>
                    </select>
        
                    <%=Html.DropDownList("ddlHabilitarTerritorio", (SelectList)ViewData["territorios"], "selecione", new { @class = "ddlHabilitar" })%>
                    <%=Html.DropDownList("ddlHabilitarGerenteProduto", (SelectList)ViewData["gerentesProduto"], "selecione", new { @class = "ddlHabilitar" })%> 
                    <%=Html.DropDownList("ddlHabilitarGerenteMarketing", (SelectList)ViewData["gerentesMarketing"], "selecione", new { @class = "ddlHabilitar" })%> 
                    <%=Html.DropDownList("ddlHabilitarAgencia", (SelectList)ViewData["agencias"], "selecione", new { @class = "ddlHabilitar" })%> 
                    <a href="" id="aHabilitar" class="botao_habilitar">Habilitar</a>

                </p>
		
        	</div>
        </div>

        <div class="tabela-container">
            
            <div class="carregando">Carregando...</div>

            <!--<h2>Utilização</h2>-->

            <%--<a href="[url_relatorio]" class="botao">Gerar Relatório</a>--%><br /><br />

            <div id="graficos" style="clear: both;"></div>

            <br /><br /><br />

            <div class="tabelas"></div>

        </div>

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
        
</asp:Content>
