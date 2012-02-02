<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Models.CustomConfig>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Editar
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">    
    <%=Util.Header.Javascript("~/scripts/jquery/jquery.colorpicker.js") %>
    <%=Util.Header.Css("~/css/colorpicker/jquery.colorpicker.css") %>
    
    <style type="text/css">
        a.customizacao{
            display: block;
            float: left;
            text-decoration: none;
            padding: 5px 10px;
        }
        
        a.customizacao.selecionado{
            background: black;
            color: White;
        }
    </style>
    
    <script type="text/javascript">
        
        function customizacao_rapida() {
            $('.customizacao_rapida').addClass('selecionado');
            $('.customizacao_avancada').removeClass('selecionado');
            
            $('#customizacao_rapida').show();
            $('#customizacao_avancada').hide();
        }

        function customizacao_avancada() {
            $('.customizacao_rapida').removeClass('selecionado');
            $('.customizacao_avancada').addClass('selecionado');
            
            $('#customizacao_rapida').hide();
            $('#customizacao_avancada').show();
        }

        function trocaCor(elemento, hex) {

            hex = hex.replace("#", "");
            
            $(elemento).css('backgroundColor', '#' + hex);
            $(elemento).attr('value', '#' + hex);
        }

        function verifica_customizacao() {

            var personalizado = '[Customização Avançada]';
            var aux;

            aux = $('#AdminCorMenuTopoFundo').val();
            if (aux == $('#AdminCorMenuFundo').val() && aux == $('#AdminCorTituloFundo').val()) {
                trocaCor('#cr_menus_fundo', aux);
            }
            else {
                $('#cr_menus_fundo').val(personalizado);
            }

            aux = $('#AdminCorMenuTopoFonte').val();
            if (aux == $('#AdminCorMenuFonte').val() && aux == $('#AdminCorTituloFonte').val()) {
                trocaCor('#cr_menus_fonte', aux);
            } else {
                $('#cr_menus_fonte').val(personalizado);
            }

            aux = $('#AdminCorMenuTopoBotoesFundo').val();
            if (aux == $('#AdminCorBotoesFundo').val() && aux == $('#AdminCorRodapeBotoesFundo').val()) {
                trocaCor('#cr_botoes_fundo', aux);
            } else {
                $('#cr_botoes_fundo').val(personalizado);
            }

            aux = $('#AdminCorMenuTopoBotoesFonte').val();
            if (aux == $('#AdminCorBotoesFonte').val() && aux == $('#AdminCorRodapeBotoesFonte').val()) {
                trocaCor('#cr_botoes_fonte', aux);
            } else {
                $('#cr_botoes_fonte').val(personalizado);
            }
            aux = $('#AdminCorMenuTopoBotoesHoverFundo').val();
            if (aux == $('#AdminCorBotoesHoverFundo').val() && aux == $('#AdminCorRodapeBotoesHoverFundo').val()) {
                trocaCor('#cr_botoes_fundo_hover', aux);
            } else {
                $('#cr_botoes_fundo_hover').val(personalizado);
            }
            aux = $('#AdminCorMenuTopoBotoesHoverFonte').val();
            if (aux == $('#AdminCorMenuHoverFonte').val() && aux == $('#AdminCorBotoesHoverFonte').val() && aux == $('#AdminCorRodapeBotoesHoverFonte').val()) {
                trocaCor('#cr_botoes_fonte_hover', aux);
            } else {
                $('#cr_botoes_fonte_hover').val(personalizado);
            }
            aux = $('#AdminCorTabelaFundo').val();
            if (aux == $('#AdminCorTabelaFundo').val()) {
                trocaCor('#cr_tabela_fundo', aux);
            } else {
                $('#cr_tabela_fundo').val(personalizado);
            }
            aux = $('#AdminCorTabelaLinha1Fonte').val();
            if (aux == $('#AdminCorTabelaLinha2Fonte').val()) {
                trocaCor('#cr_tabela_fonte', aux);
            } else {
                $('#cr_tabela_fonte').val(personalizado);
            }
            aux = $('#AdminCorTabelaHeaderSelecionadoFonte').val();
            if (aux == $('#AdminCorTabelaHeaderFonte').val() && aux == $('#AdminCorTabelaLinha1Fundo').val()) {
                trocaCor('#cr_tabela_linha1', aux);
            } else {
                $('#cr_tabela_linha1').val(personalizado);
            }
            aux = $('#AdminCorTabelaLinha2Fundo').val();
            if (aux == $('#AdminCorTabelaLinha2Fundo').val()) {
                trocaCor('#cr_tabela_linha2', aux);
            } else {
                $('#cr_tabela_linha2').val(personalizado);
            }
            aux = $('#AdminCorConteudoSecundarioFundo').val();
            if (aux == $('#AdminCorConteudoSecundarioFundo').val()) {
                trocaCor('#cr_conteudosecundario_fundo', aux);
            } else {
                $('#cr_conteudosecundario_fundo').val(personalizado);
            }
            aux = $('#AdminCorConteudoSecundarioFonte').val();
            if (aux == $('#AdminCorConteudoSecundarioFonte').val()) {
                trocaCor('#cr_conteudosecundario_fonte', aux);
            } else {
                $('#cr_conteudosecundario_fonte').val(personalizado);
            }

        }

        $(function() {

            customizacao_rapida();
            verifica_customizacao();

            $('.color').each(function() {
                var elemento = $(this);

                elemento.change(function() {
                    trocaCor(this, elemento.val());
                    var cor = elemento.val().replace("#", "");
                    elemento.ColorPickerSetColor(cor);
                });

                elemento.keyup(function() {
                    trocaCor(this, elemento.val());
                    var cor = elemento.val().replace("#", "");
                    elemento.ColorPickerSetColor(cor);
                });

                elemento.ColorPicker({
                    color: elemento.val(),
                    onShow: function(colpkr) {
                        $(colpkr).fadeIn(500);
                        return false;
                    },
                    onHide: function(colpkr) {
                        $(colpkr).fadeOut(500);
                        return false;
                    },
                    onChange: function(hsb, hex, rgb) {
                        trocaCor(elemento, hex);
                        
                        switch ($(elemento).attr('id')) {
                            case "cr_menus_fundo":
                                trocaCor('#AdminCorMenuTopoFundo', hex);
                                trocaCor('#AdminCorMenuFundo', hex);
                                trocaCor('#AdminCorTituloFundo', hex);

                                break;
                            case "cr_menus_fonte":
                                trocaCor('#AdminCorMenuTopoFonte', hex);
                                trocaCor('#AdminCorMenuFonte', hex);
                                trocaCor('#AdminCorTituloFonte', hex);

                                break;
                            case "cr_botoes_fundo":
                                trocaCor('#AdminCorMenuTopoBotoesFundo', hex);
                                trocaCor('#AdminCorBotoesFundo', hex);
                                trocaCor('#AdminCorRodapeBotoesFundo', hex);

                                break;
                            case "cr_botoes_fonte":
                                trocaCor('#AdminCorMenuTopoBotoesFonte', hex);
                                trocaCor('#AdminCorBotoesFonte', hex);
                                trocaCor('#AdminCorRodapeBotoesFonte', hex);

                                break;
                            case "cr_botoes_fundo_hover":
                                trocaCor('#AdminCorMenuTopoBotoesHoverFundo', hex);
                                trocaCor('#AdminCorBotoesHoverFundo', hex);
                                trocaCor('#AdminCorRodapeBotoesHoverFundo', hex);

                                break;
                            case "cr_botoes_fonte_hover":
                                trocaCor('#AdminCorMenuTopoBotoesHoverFonte', hex);
                                trocaCor('#AdminCorMenuHoverFonte', hex);
                                trocaCor('#AdminCorBotoesHoverFonte', hex);
                                trocaCor('#AdminCorRodapeBotoesHoverFonte', hex);

                                break;
                            case "cr_tabela_fundo":
                                trocaCor('#AdminCorTabelaFundo', hex);

                                break;
                            case "cr_tabela_fonte":
                                trocaCor('#AdminCorTabelaFonte', hex);
                                trocaCor('#AdminCorTabelaLinha1Fonte', hex);
                                trocaCor('#AdminCorTabelaLinha2Fonte', hex);

                                break;
                            case "cr_tabela_linha1":
                                trocaCor('#AdminCorTabelaHeaderSelecionadoFonte', hex);
                                trocaCor('#AdminCorTabelaHeaderFonte', hex);

                                trocaCor('#AdminCorTabelaLinha1Fundo', hex);

                                break;
                            case "cr_tabela_linha2":
                                trocaCor('#AdminCorTabelaLinha2Fundo', hex);

                                break;
                            case "cr_conteudosecundario_fundo":

                                trocaCor('#AdminCorConteudoSecundarioFundo', hex);

                                break;
                            case "cr_conteudosecundario_fonte":

                                trocaCor('#AdminCorConteudoSecundarioFonte', hex);

                                break;
                        }
                    },
                    onSubmit: function(hsb, hex, rgb, el) {
                        trocaCor(el, hex);
                        $(el).ColorPickerHide();
                    }
                });
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="titulo">
        <h2>Sistema :: Editar</h2>
    </div>
    
    <div class="conteudo-form">

    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>
            
            <fieldset>
            
                <legend>Geral</legend>
                
                <ul>
                    <li>
                        <%= Html.LabelFor(model => model.NomeSistema)%>
                        <%= Html.TextBoxFor(model => model.NomeSistema, new { maxlength = "50", @class = "validate[required]" })%>
                    </li>
                    
                    <li>
                        <%= Html.LabelFor(model => model.SiteUrlLocal) %>
                        <%= Html.TextBoxFor(model => model.SiteUrlLocal, new { maxlength = "50", @class = "validate[required]" })%>
                    </li>
                    
                    <li>
                        <%= Html.LabelFor(model => model.SiteUrlOnline) %>
                        <%= Html.TextBoxFor(model => model.SiteUrlOnline, new { maxlength = "50", @class = "validate[required]" })%>
                    </li>
                    
                    <li>
                        <%= Html.LabelFor(model => model.HostOnline) %>
                        <%= Html.TextBoxFor(model => model.HostOnline, new { maxlength = "50", @class = "validate[required]" })%>
                    </li>
                </ul>
            </fieldset>

            <fieldset>
                <legend>Email</legend>
                
                <fieldset>
                    <legend>Envio</legend>
                    
                    <ul>
                        <li>
                            <%= Html.LabelFor(model => model.NomeEnvioPadrao) %>
                            <%= Html.TextBoxFor(model => model.NomeEnvioPadrao, new { maxlength = "50", @class = "validate[required]" }) %>
                        </li>
                        
                        <li>
                            <%= Html.LabelFor(model => model.EmailEnvioPadrao) %>
                            <%= Html.TextBoxFor(model => model.EmailEnvioPadrao, new { maxlength = "50", @class = "validate[required]" })%>
                        </li>
                        
                        <li>
                            <%= Html.LabelFor(model => model.SmtpHost) %>
                            <%= Html.TextBoxFor(model => model.SmtpHost, new { maxlength = "50", @class = "" })%>
                        </li>
                        
                        <li>
                            <%= Html.LabelFor(model => model.SmtpAutenticado) %>
                            <%= Html.TextBoxFor(model => model.SmtpAutenticado, new { maxlength = "50", @class = "" })%>
                        </li>
                        
                        <li>
                            <%= Html.LabelFor(model => model.SmtpUsername) %>
                            <%= Html.TextBoxFor(model => model.SmtpUsername, new { maxlength = "50", @class = "" })%>
                        </li>
                        
                        <li>
                            <%= Html.LabelFor(model => model.SmtpPassword) %>
                            <%= Html.TextBoxFor(model => model.SmtpPassword, new { maxlength = "50", @class = "" })%>
                        </li>
                    </ul>
                </fieldset>
                
                <fieldset>
                    <legend>Contato / Fale Conosco</legend>
                    
                    <ul>
                        <li>
                            <%= Html.LabelFor(model => model.NomeContatoPadrao) %>
                            <%= Html.TextBoxFor(model => model.NomeContatoPadrao, new { maxlength = "50", @class = "validate[required]" })%>
                        </li>
                        
                        <li>
                            <%= Html.LabelFor(model => model.EmailContatoPadrao) %>
                            <%= Html.TextBoxFor(model => model.EmailContatoPadrao, new { maxlength = "50", @class = "validate[required]" })%>
                        </li>
                    </ul>
                </fieldset>
                
                <fieldset>
                    <legend>Desenvolvedor / Erros</legend>
                    
                    <ul>
                        <li>
                            <%= Html.LabelFor(model => model.EmailDesenvolvedor) %>
                            <%= Html.TextBoxFor(model => model.EmailDesenvolvedor, new { maxlength = "50", @class = "validate[required]" })%>
                        </li>
                    </ul>
                </fieldset>
                
            </fieldset>

            <fieldset>
                <legend>Analytics</legend>
                
                <ul>
                    <li>
                        <%= Html.LabelFor(model => model.AnalyticsId) %>
                        <%= Html.TextBoxFor(model => model.AnalyticsId, new { maxlength = "50", @class = "" })%> 
                        trocar pelo valor presente em ['_setAccount', 'UA-1936778-22'] do codigo fornecido pelo Google Analytics
                    </li>
                </ul>
            </fieldset>
            
            <fieldset>
                <legend>Addthis</legend>
                
                <ul>
                    <li>
                        <%= Html.LabelFor(model => model.AddthisEnabled) %>
                        <%= Html.TextBoxFor(model => model.AddthisEnabled, new { maxlength = "50", @class = "" }) %>
                    </li>
                    
                    <li>
                        <%= Html.LabelFor(model => model.AddthisUsername) %>
                        <%= Html.TextBoxFor(model => model.AddthisUsername, new { maxlength = "50", @class = "" })%>
                    </li>
                </ul>
            </fieldset>

            <fieldset>
            
                <legend>Validation Engine</legend>
                
                <ul>
                    <li>
                        <label>Cor do fundo</label>
                        <%= Html.TextBoxFor(model => model.VECorFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.VECorFundo })%>
                    </li>
                    
                    <li>
                        <label>Cor da fonte</label>
                        <%= Html.TextBoxFor(model => model.VECorFonte, new { maxlength = "50", @class = "color", style="background-color: " + Model.VECorFonte })%>
                    </li>
                    
                    <li>
                        <label>Largura (necessário unidade - ex. "px")</label>
                        <%= Html.TextBoxFor(model => model.VELargura, new { maxlength = "50" })%>
                    </li>
                    
                    <li>
                        <label>Tamanho sombra (necessário unidade - ex. "px")</label>
                        <%= Html.TextBoxFor(model => model.VETamanhoSombra) %>
                    </li>
                    
                    <li>
                        <label>Curvatura da borda (necessário unidade - ex. "px")</label>
                        <%= Html.TextBoxFor(model => model.VETamanhoCurvaBorda) %>
                    </li>
                </ul>
            </fieldset>
            
            <fieldset>
            
                <legend>Admin</legend>
                
                <h3 style="display: block; width: 100%; float: left; clear: both;">
                    <div style="float: left; text-align: center; position: relative; left: 50%; margin-left: -180px; width: 360px;">
                    <b>
                        <a href="javascript:customizacao_rapida();" class="customizacao_rapida customizacao">Customização Rápida</a>
                        <a href="javascript:customizacao_avancada();" class="customizacao_avancada customizacao">Customização Avançada</a>
                    </b>
                    </li>
                </h3>
                <br />
                
                <fieldset>
                    <legend>Login</legend>
                    
                    <ul>
                        <li>
                            <label>Imagem / Logo</label>
                            <%= Html.TextBoxFor(model => model.AdminImagemLogoLogin) %>
                        </li>
                        
                        <li>
                            <label>Cor do fundo</label>
                            <%= Html.TextBoxFor(model => model.AdminCorLoginFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorLoginFundo })%>
                        </li>
                        
                        <li>
                            <label>Cor da fonte do login</label>
                            <%= Html.TextBoxFor(model => model.AdminCorLoginFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorLoginFonte })%>
                        </li>
                     </ul>
                </fieldset>
                
                <fieldset>
                    <legend>Geral</legend>
                    
                    <ul>
                        <li>
                            <label>Cor do fundo (Background)</label>
                            <%= Html.TextBoxFor(model => model.AdminCorFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorFundo })%>
                        </li>
                        
                        <li>
                            <label>Imagem (Logo)</label>
                            <%= Html.TextBoxFor(model => model.AdminImagemLogo)%>
                        </li>
                        
                        <li>
                            <label>Orientação do Menu</label>
                            <%=Html.DropDownListFor(model => model.AdminOrientacaoMenu, new SelectList(new[] {"horizontal", "vertical"})) %>
                        </li>
                        
                        <li>
                            <label>Cor do fundo do conteúdo</label>
                            <%= Html.TextBoxFor(model => model.AdminCorConteudoFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorConteudoFundo })%>
                        </li>
                        
                        <li>
                            <label>Cor da fonte do conteúdo</label>
                            <%= Html.TextBoxFor(model => model.AdminCorConteudoFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorConteudoFonte })%>
                        </li>
                        
                        <li>
                            <label>Cor dos links</label>
                            <%= Html.TextBoxFor(model => model.AdminCorLinksFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorLinksFonte })%>
                        </li>
                        
                        <li>
                            <label>Cor dos links - hover</label>
                            <%= Html.TextBoxFor(model => model.AdminCorLinksHoverFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorLinksHoverFonte })%>
                        </li>
                    </ul>
                </fieldset>
                

                
                <div id="customizacao_rapida">
                    
                    <fieldset>
                        <legend>Menus</legend>
                        
                        <ul>
                            <li>
                                <label>Cor do fundo</label>
                                <%= Html.TextBox("cr_menus_fundo","", new { maxlength = "50", @class = "color", onchange = "customRapida('menuFundo', this.value);" })%>
                            </li>
                            
                            <li>
                                <label>Cor da fonte</label>
                                <%= Html.TextBox("cr_menus_fonte","", new { maxlength = "50", @class = "color" })%>
                            </li>
                        </ul>
                        
                    </fieldset>
                    
                    <fieldset>
                        <legend>Botões</legend>
                        
                        <ul>
                            <li>
                                <label>Cor do fundo</label>
                            
                                <%= Html.TextBox("cr_botoes_fundo","", new { maxlength = "50", @class = "color" })%>
                            </li>
                            
                            <li>
                                <label>Cor da fonte</label>
                            
                                <%= Html.TextBox("cr_botoes_fonte","", new { maxlength = "50", @class = "color" })%>
                            </li>
                            
                            <li>
                                <label>Cor do fundo - Hover </label>
                            
                                <%= Html.TextBox("cr_botoes_fundo_hover","", new { maxlength = "50", @class = "color" })%>
                            </li>
                            
                            <li>
                                <label>Cor da fonte - Hover </label>
                            
                                <%= Html.TextBox("cr_botoes_fonte_hover","", new { maxlength = "50", @class = "color" })%>
                            </li>
                        </ul>
                        
                    </fieldset>
                    
                    <fieldset>
                        <legend>Tabelas</legend>
                        
                        <ul>
                            <li>
                                <label>Cor do Fundo</label>
                                <%= Html.TextBox("cr_tabela_fundo","", new { maxlength = "50", @class = "color"})%>
                            </li>
                            
                            <li>
                                <label>Cor da Fonte</label>
                                <%= Html.TextBox("cr_tabela_fonte", "", new { maxlength = "50", @class = "color" })%>
                            </li>
                            
                            <li>
                                <label>Cor da Linha 1</label>
                                <%= Html.TextBox("cr_tabela_linha1", "", new { maxlength = "50", @class = "color" })%>
                            </li>
                            
                            <li>
                                <label>Cor da Linha 2</label>
                                <%= Html.TextBox("cr_tabela_linha2", "", new { maxlength = "50", @class = "color" })%>
                            </li>
                        </ul>
                    </fieldset>
                    <fieldset>
                        <legend>Conteúdo Secundário</legend>
                        
                        <ul>
                            <li>
                                <label>Cor do Fundo</label>
                                <%= Html.TextBox("cr_conteudosecundario_fundo", "", new { maxlength = "50", @class = "color" })%>
                            </li>
                            <li>
                                <label>Cor da Fonte</label>
                                <%= Html.TextBox("cr_conteudosecundario_fonte", "", new { maxlength = "50", @class = "color" })%>
                            </li>
                        </ul>
                    </fieldset>
                </div>
                
                <div id="customizacao_avancada">
                    
                    <fieldset>
                        <legend>Topo</legend>
                        
                        <ul>
                            <li>
                                <label>Cor do fundo do menu (topo)</label>
                                <%= Html.TextBoxFor(model => model.AdminCorMenuTopoFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorMenuTopoFundo })%>
                            </li>
                            
                            <li>
                                <label>Cor da fonte do menu (topo)</label>
                                <%= Html.TextBoxFor(model => model.AdminCorMenuTopoFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorMenuTopoFonte })%>
                            </li>
                            
                            <li>
                                <label>Cor do fundo dos botões</label>
                                <%= Html.TextBoxFor(model => model.AdminCorMenuTopoBotoesFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorMenuTopoBotoesFundo })%>
                            </li>
                            
                            <li>
                                <label>Cor da fonte dos botões</label>
                                <%= Html.TextBoxFor(model => model.AdminCorMenuTopoBotoesFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorMenuTopoBotoesFonte })%>
                            </li>
                            
                            <li>
                                <label>Cor do fundo dos botões - hover</label>
                                <%= Html.TextBoxFor(model => model.AdminCorMenuTopoBotoesHoverFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorMenuTopoBotoesHoverFundo })%>
                            </li>
                            
                            <li>
                                <label>Cor da fonte dos botões - hover</label>
                                <%= Html.TextBoxFor(model => model.AdminCorMenuTopoBotoesHoverFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorMenuTopoBotoesHoverFonte })%>
                            </li>
                        </ul>
                        
                    </fieldset>
                    
                    <fieldset>
                        <legend>Menu</legend>
                        
                        <ul>
                            <li>
                                <label>Cor do fundo</label>
                                <%= Html.TextBoxFor(model => model.AdminCorMenuFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorMenuFundo })%>
                            </li>
                            
                            <li>
                                <label>Cor do fundo - hover</label>
                                <%= Html.TextBoxFor(model => model.AdminCorMenuHoverFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorMenuHoverFundo })%>
                            </li>
                            
                            <li>
                                <label>Cor da fonte</label>
                                <%= Html.TextBoxFor(model => model.AdminCorMenuFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorMenuFonte })%>
                            </li>
                            
                            <li>
                                <label>Cor da fonte - hover</label>
                                <%= Html.TextBoxFor(model => model.AdminCorMenuHoverFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorMenuHoverFonte })%>
                            </li>
                        </ul>
                    </fieldset>
                    
                    <fieldset>
                        <legend>Conteudo</legend>
                        
                        <fieldset>
                            <legend>Geral</legend>
                            
                            <ul>
                                <li>
                                    <label>Cor do fundo dos títulos</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorTituloFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorTituloFundo })%>
                                </li>
                                <li>
                                    <label>Cor da fonte dos títulos</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorTituloFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorTituloFonte })%>
                                </li>
                            </ul>
                        </fieldset>
                        
                        <fieldset>
                            <legend>Botões</legend>
                            
                            <ul>
                                <li>
                                    <label>Cor do fundo</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorBotoesFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorBotoesFundo })%>
                                </li>
                                
                                <li>
                                    <label>Cor da fonte</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorBotoesFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorBotoesFonte })%>
                                </li>
                                
                                <li>
                                    <label>Cor do fundo - hover</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorBotoesHoverFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorBotoesHoverFundo })%>
                                </li>
                                
                                <li>
                                    <label>Cor da fonte - hover</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorBotoesHoverFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorBotoesHoverFonte })%>
                                </li>
                            </ul>
                        </fieldset>
                        <fieldset>
                            <legend>Tabelas</legend>
                            
                            <ul>
                                <li>
                                    <label>Cor do fundo</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorTabelaFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorTabelaFundo })%>
                                </li>
                                
                                <li>
                                    <label>Cor da fonte do header</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorTabelaHeaderFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorTabelaHeaderFonte })%>
                                </li>
                                
                                <li>
                                    <label>Cor da fonte do header - selecionado</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorTabelaHeaderSelecionadoFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorTabelaHeaderSelecionadoFonte })%>
                                </li>
                                
                                <li>
                                    <label>Cor do fundo da linha 1</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorTabelaLinha1Fundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorTabelaLinha1Fundo })%>
                                </li>
                                
                                <li>
                                    <label>Cor da fonte da linha 1</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorTabelaLinha1Fonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorTabelaLinha1Fonte })%>
                                </li>
                                
                                <li>
                                    <label>Cor do fundo da linha 2</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorTabelaLinha2Fundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorTabelaLinha2Fundo })%>
                                </li>
                                
                                <li>
                                    <label>Cor da fonte da linha 2</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorTabelaLinha2Fonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorTabelaLinha2Fonte })%>
                                </li>
                                
                                <li>
                                    <label>Imagem da seta para cima (ordenação das tabelas)</label>
                                    <%= Html.TextBoxFor(model => model.AdminImagemTabelaHeaderSetaCima, new { maxlength = "50"})%>
                                </li>
                                
                                <li>
                                    <label>Imagem da seta para baixo (ordenação das tabelas)</label>
                                    <%= Html.TextBoxFor(model => model.AdminImagemTabelaHeaderSetaBaixo, new { maxlength = "50" })%>
                                </li>
                            </ul>
                        </fieldset>
                        <fieldset>
                            <legend>Conteúdo Secundário</legend>
                            
                            <ul>
                                <li>
                                    <label>Cor do fundo</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorConteudoSecundarioFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorConteudoSecundarioFundo })%>
                                </li>
                                
                                <li>
                                    <label>Cor da fonte</label>
                                    <%= Html.TextBoxFor(model => model.AdminCorConteudoSecundarioFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorConteudoSecundarioFonte })%>
                                </li>
                            </ul>
                        </fieldset>
                    </fieldset>
                    <fieldset>
                        <legend>Rodapé</legend>
                        
                        <ul>
                            <li>
                                <label>Cor de fundo dos botões</label>
                                <%= Html.TextBoxFor(model => model.AdminCorRodapeBotoesFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorRodapeBotoesFundo })%>
                            </li>
                            
                            <li>
                                <label>Cor da fonte dos botões</label>
                                <%= Html.TextBoxFor(model => model.AdminCorRodapeBotoesFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorRodapeBotoesFonte })%>
                            </li>
                            
                            <li>
                                <label>Cor de fundo dos botões - hover</label>
                                <%= Html.TextBoxFor(model => model.AdminCorRodapeBotoesHoverFundo, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorRodapeBotoesHoverFundo })%>
                            </li>
                            
                            <li>
                                <label>Cor da fonte dos botões - hover</label>
                                <%= Html.TextBoxFor(model => model.AdminCorRodapeBotoesHoverFonte, new { maxlength = "50", @class = "color", style = "background-color: " + Model.AdminCorRodapeBotoesHoverFonte })%>
                            </li>
                         </ul>
                    </fieldset>
                </div>
            </fieldset>
            
            <p>
                <input type="submit" value="Save" />
            </p>

    <% } %>
    
    </div>

</asp:Content>