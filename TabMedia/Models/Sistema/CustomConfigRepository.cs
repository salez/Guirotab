using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Models
{
    public class CustomConfigRepository
    {
        public CustomConfig GetConfig()
        {
            CustomConfig config = new CustomConfig();

            config.NomeSistema = ConfigurationManager.AppSettings["NomeSistema"];
            config.SiteUrlLocal = ConfigurationManager.AppSettings["SiteUrlLocal"];
            config.SiteUrlOnline = ConfigurationManager.AppSettings["SiteUrlOnline"];
            config.HostOnline = ConfigurationManager.AppSettings["HostOnline"];

            config.NomeEnvioPadrao = ConfigurationManager.AppSettings["NomeEnvioPadrao"];
            config.EmailEnvioPadrao = ConfigurationManager.AppSettings["EmailEnvioPadrao"];
            config.SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            config.SmtpAutenticado = (ConfigurationManager.AppSettings["SmtpAutenticado"] == "true");
            config.SmtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
            config.SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];

            config.NomeContatoPadrao = ConfigurationManager.AppSettings["NomeContatoPadrao"];
            config.EmailContatoPadrao = ConfigurationManager.AppSettings["EmailContatoPadrao"];
            config.EmailDesenvolvedor = ConfigurationManager.AppSettings["EmailDesenvolvedor"];

            config.AnalyticsId = ConfigurationManager.AppSettings["AnalyticsId"];
            config.AddthisEnabled = (ConfigurationManager.AppSettings["AddthisEnabled"] == "true");
            config.AddthisUsername = ConfigurationManager.AppSettings["AddthisUsername"];

            config.VECorFundo = ConfigurationManager.AppSettings["VECorFundo"];
            config.VECorFonte = ConfigurationManager.AppSettings["VECorFonte"];
            config.VELargura = ConfigurationManager.AppSettings["VELargura"];
            config.VETamanhoSombra = ConfigurationManager.AppSettings["VETamanhoSombra"];
            config.VETamanhoCurvaBorda = ConfigurationManager.AppSettings["VETamanhoCurvaBorda"];

            //Admin
            //Login
            config.AdminCorLoginFundo = ConfigurationManager.AppSettings["AdminCorLoginFundo"];
            config.AdminCorLoginFonte = ConfigurationManager.AppSettings["AdminCorLoginFonte"];
            config.AdminImagemLogoLogin = ConfigurationManager.AppSettings["AdminImagemLogoLogin"];
            //Geral
            config.AdminCorFundo = ConfigurationManager.AppSettings["AdminCorFundo"];
            config.AdminImagemLogo = ConfigurationManager.AppSettings["AdminImagemLogo"];
            config.AdminOrientacaoMenu = ConfigurationManager.AppSettings["AdminOrientacaoMenu"];
            config.AdminCorLinksFonte = ConfigurationManager.AppSettings["AdminCorLinksFonte"];
            config.AdminCorLinksHoverFonte = ConfigurationManager.AppSettings["AdminCorLinksHoverFonte"];
            //Topo
            config.AdminCorMenuTopoFundo = ConfigurationManager.AppSettings["AdminCorMenuTopoFundo"];
            config.AdminCorMenuTopoFonte = ConfigurationManager.AppSettings["AdminCorMenuTopoFonte"];
            config.AdminCorMenuTopoBotoesFundo = ConfigurationManager.AppSettings["AdminCorMenuTopoBotoesFundo"];
            config.AdminCorMenuTopoBotoesFonte = ConfigurationManager.AppSettings["AdminCorMenuTopoBotoesFonte"];
            config.AdminCorMenuTopoBotoesHoverFundo = ConfigurationManager.AppSettings["AdminCorMenuTopoBotoesHoverFundo"];
            config.AdminCorMenuTopoBotoesHoverFonte = ConfigurationManager.AppSettings["AdminCorMenuTopoBotoesHoverFonte"];
            //Menu
            config.AdminCorMenuFundo = ConfigurationManager.AppSettings["AdminCorMenuFundo"];
            config.AdminCorMenuHoverFundo = ConfigurationManager.AppSettings["AdminCorMenuHoverFundo"];
            config.AdminCorMenuFonte = ConfigurationManager.AppSettings["AdminCorMenuFonte"];
            config.AdminCorMenuHoverFonte = ConfigurationManager.AppSettings["AdminCorMenuHoverFonte"];
            //Conteudo
            //Geral
            config.AdminCorTituloFundo = ConfigurationManager.AppSettings["AdminCorTituloFundo"];
            config.AdminCorTituloFonte = ConfigurationManager.AppSettings["AdminCorTituloFonte"];
            config.AdminCorConteudoFundo = ConfigurationManager.AppSettings["AdminCorConteudoFundo"];
            config.AdminCorConteudoFonte = ConfigurationManager.AppSettings["AdminCorConteudoFonte"];
            //Botoes
            config.AdminCorBotoesFundo = ConfigurationManager.AppSettings["AdminCorBotoesFundo"];
            config.AdminCorBotoesFonte = ConfigurationManager.AppSettings["AdminCorBotoesFonte"];
            config.AdminCorBotoesHoverFundo = ConfigurationManager.AppSettings["AdminCorBotoesHoverFundo"];
            config.AdminCorBotoesHoverFonte = ConfigurationManager.AppSettings["AdminCorBotoesHoverFonte"];
            //Tabela
            config.AdminCorTabelaFundo = ConfigurationManager.AppSettings["AdminCorTabelaFundo"];
            config.AdminCorTabelaHeaderFonte = ConfigurationManager.AppSettings["AdminCorTabelaHeaderFonte"];
            config.AdminCorTabelaHeaderSelecionadoFonte = ConfigurationManager.AppSettings["AdminCorTabelaHeaderSelecionadoFonte"];
            config.AdminCorTabelaFundo = ConfigurationManager.AppSettings["AdminCorTabelaFundo"];
            config.AdminCorTabelaHeaderFonte = ConfigurationManager.AppSettings["AdminCorTabelaHeaderFonte"];
            config.AdminCorTabelaHeaderSelecionadoFonte = ConfigurationManager.AppSettings["AdminCorTabelaHeaderSelecionadoFonte"];
            config.AdminCorTabelaLinha1Fundo = ConfigurationManager.AppSettings["AdminCorTabelaLinha1Fundo"];
            config.AdminCorTabelaLinha1Fonte = ConfigurationManager.AppSettings["AdminCorTabelaLinha1Fonte"];
            config.AdminCorTabelaLinha2Fundo = ConfigurationManager.AppSettings["AdminCorTabelaLinha2Fundo"];
            config.AdminCorTabelaLinha2Fonte = ConfigurationManager.AppSettings["AdminCorTabelaLinha2Fonte"];
            config.AdminImagemTabelaHeaderSetaCima = ConfigurationManager.AppSettings["AdminImagemTabelaHeaderSetaCima"];
            config.AdminImagemTabelaHeaderSetaBaixo = ConfigurationManager.AppSettings["AdminImagemTabelaHeaderSetaBaixo"];
            //Conteudo Secundario
            config.AdminCorConteudoSecundarioFundo = ConfigurationManager.AppSettings["AdminCorConteudoSecundarioFundo"];
            config.AdminCorConteudoSecundarioFonte = ConfigurationManager.AppSettings["AdminCorConteudoSecundarioFonte"];
            //Rodape
            config.AdminCorRodapeBotoesFundo = ConfigurationManager.AppSettings["AdminCorRodapeBotoesFundo"];
            config.AdminCorRodapeBotoesFonte = ConfigurationManager.AppSettings["AdminCorRodapeBotoesFonte"];
            config.AdminCorRodapeBotoesHoverFundo = ConfigurationManager.AppSettings["AdminCorRodapeBotoesHoverFundo"];
            config.AdminCorRodapeBotoesHoverFonte = ConfigurationManager.AppSettings["AdminCorRodapeBotoesHoverFonte"];

            return config;
        }

        public void UpdateConfig(CustomConfig config)
        {
            EditKey("NomeSistema", config.NomeSistema);
            EditKey("SiteUrlLocal", config.SiteUrlLocal);
            EditKey("SiteUrlOnline", config.SiteUrlOnline);
            EditKey("HostOnline", config.HostOnline);

            EditKey("NomeEnvioPadrao", config.NomeEnvioPadrao);
            EditKey("EmailEnvioPadrao", config.EmailEnvioPadrao);
            EditKey("SmtpHost", config.SmtpHost);
            EditKey("SmtpAutenticado", (config.SmtpAutenticado) ? "true" : "false");
            EditKey("SmtpUsername", config.SmtpUsername);
            EditKey("SmtpPassword", config.SmtpPassword);

            EditKey("NomeContatoPadrao", config.NomeContatoPadrao);
            EditKey("EmailContatoPadrao", config.EmailContatoPadrao);
            EditKey("EmailDesenvolvedor", config.EmailDesenvolvedor);

            EditKey("AnalyticsId", config.AnalyticsId);
            EditKey("AddthisEnabled", (config.AddthisEnabled) ? "true" : "false");
            EditKey("AddthisUsername", config.AddthisUsername);
            EditKey("VECorFundo", config.VECorFundo);
            EditKey("VECorFonte", config.VECorFonte);
            EditKey("VELargura", config.VELargura);
            EditKey("VETamanhoSombra", config.VETamanhoSombra);
            EditKey("VETamanhoCurvaBorda", config.VETamanhoCurvaBorda);

            //Admin
            //Login
            EditKey("AdminCorLoginFundo", config.AdminCorLoginFundo);
            EditKey("AdminCorLoginFonte", config.AdminCorLoginFonte);
            EditKey("AdminImagemLogoLogin", config.AdminImagemLogoLogin);
            //Geral
            EditKey("AdminCorFundo", config.AdminCorFundo);
            EditKey("AdminImagemLogo", config.AdminImagemLogo);
            EditKey("AdminOrientacaoMenu", config.AdminOrientacaoMenu);
            EditKey("AdminCorLinksFonte", config.AdminCorLinksFonte);
            EditKey("AdminCorLinksHoverFonte", config.AdminCorLinksHoverFonte);
            //Topo
            EditKey("AdminCorMenuTopoFundo", config.AdminCorMenuTopoFundo);
            EditKey("AdminCorMenuTopoFonte", config.AdminCorMenuTopoFonte);
            EditKey("AdminCorMenuTopoBotoesFundo", config.AdminCorMenuTopoBotoesFundo);
            EditKey("AdminCorMenuTopoBotoesFonte", config.AdminCorMenuTopoBotoesFonte);
            EditKey("AdminCorMenuTopoBotoesHoverFundo", config.AdminCorMenuTopoBotoesHoverFundo);
            EditKey("AdminCorMenuTopoBotoesHoverFonte", config.AdminCorMenuTopoBotoesHoverFonte);
            //Menu
            EditKey("AdminCorMenuFundo", config.AdminCorMenuFundo);
            EditKey("AdminCorMenuHoverFundo", config.AdminCorMenuHoverFundo);
            EditKey("AdminCorMenuFonte", config.AdminCorMenuFonte);
            EditKey("AdminCorMenuHoverFonte", config.AdminCorMenuHoverFonte);
            //Conteudo
            //Geral
            EditKey("AdminCorTituloFundo", config.AdminCorTituloFundo);
            EditKey("AdminCorTituloFonte", config.AdminCorTituloFonte);
            EditKey("AdminCorConteudoFundo", config.AdminCorConteudoFundo);
            EditKey("AdminCorConteudoFonte", config.AdminCorConteudoFonte);
            //Botoes
            EditKey("AdminCorBotoesFundo", config.AdminCorBotoesFundo);
            EditKey("AdminCorBotoesFonte", config.AdminCorBotoesFonte);
            EditKey("AdminCorBotoesHoverFundo", config.AdminCorBotoesHoverFundo);
            EditKey("AdminCorBotoesHoverFonte", config.AdminCorBotoesHoverFonte);
            //Tabela
            EditKey("AdminCorTabelaFundo", config.AdminCorTabelaFundo);
            EditKey("AdminCorTabelaHeaderFonte", config.AdminCorTabelaHeaderFonte);
            EditKey("AdminCorTabelaHeaderSelecionadoFonte", config.AdminCorTabelaHeaderSelecionadoFonte);
            EditKey("AdminCorTabelaFundo", config.AdminCorTabelaFundo);
            EditKey("AdminCorTabelaHeaderFonte", config.AdminCorTabelaHeaderFonte);
            EditKey("AdminCorTabelaHeaderSelecionadoFonte", config.AdminCorTabelaHeaderSelecionadoFonte);
            EditKey("AdminCorTabelaLinha1Fundo", config.AdminCorTabelaLinha1Fundo);
            EditKey("AdminCorTabelaLinha1Fonte", config.AdminCorTabelaLinha1Fonte);
            EditKey("AdminCorTabelaLinha2Fundo", config.AdminCorTabelaLinha2Fundo);
            EditKey("AdminCorTabelaLinha2Fonte", config.AdminCorTabelaLinha2Fonte);
            EditKey("AdminImagemTabelaHeaderSetaCima", config.AdminImagemTabelaHeaderSetaCima);
            EditKey("AdminImagemTabelaHeaderSetaBaixo", config.AdminImagemTabelaHeaderSetaBaixo);
            //Conteudo Secundario
            EditKey("AdminCorConteudoSecundarioFundo", config.AdminCorConteudoSecundarioFundo);
            EditKey("AdminCorConteudoSecundarioFonte", config.AdminCorConteudoSecundarioFonte);
            //Rodape
            EditKey("AdminCorRodapeBotoesFundo", config.AdminCorRodapeBotoesFundo);
            EditKey("AdminCorRodapeBotoesFonte", config.AdminCorRodapeBotoesFonte);
            EditKey("AdminCorRodapeBotoesHoverFundo", config.AdminCorRodapeBotoesHoverFundo);
            EditKey("AdminCorRodapeBotoesHoverFonte", config.AdminCorRodapeBotoesHoverFonte);
        }

        private void EditKey(string key, string value)
        {
            Configuration objConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/");
            AppSettingsSection objAppsettings = (AppSettingsSection)objConfig.GetSection("appSettings");
            //Edit
            if (objAppsettings != null)
            {
                objAppsettings.Settings[key].Value = value;
                objConfig.Save(ConfigurationSaveMode.Minimal);
            }
        }
    }
}