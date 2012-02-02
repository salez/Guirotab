using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class CustomConfig
    {
        //Geral
        public String NomeSistema { get; set; }

        public String SiteUrlLocal { get; set; }
        public String SiteUrlOnline { get; set; }
        public String HostOnline { get; set; }

        //Email
        public String NomeEnvioPadrao { get; set; }
        public String EmailEnvioPadrao { get; set; }
        public String SmtpHost { get; set; }
        public bool SmtpAutenticado { get; set; }
        public String SmtpUsername { get; set; }
        public String SmtpPassword { get; set; }

        public String NomeContatoPadrao { get; set; }
        public String EmailContatoPadrao { get; set; }

        public String EmailDesenvolvedor { get; set; }

        //Analytics
        public String AnalyticsId { get; set; }

        //Addthis
        public bool AddthisEnabled { get; set; }
        public String AddthisUsername { get; set; }

        //Customização

        //Validation Engine
        public String VECorFundo { get; set; }
        public String VECorFonte { get; set; }
        public String VELargura { get; set; }
        public String VETamanhoSombra { get; set; }
        public String VETamanhoCurvaBorda { get; set; }

        //Admin
        //Login
        public String AdminCorLoginFundo { get; set; }
        public String AdminCorLoginFonte { get; set; }
        public String AdminImagemLogoLogin { get; set; }
        public String AdminOrientacaoMenu { get; set; }
        //Geral
        public String AdminCorFundo { get; set; }
        public String AdminImagemLogo { get; set; }
        public String AdminCorLinksFonte { get; set; }
        public String AdminCorLinksHoverFonte { get; set; }
        //Topo
        public String AdminCorMenuTopoFundo { get; set; }
        public String AdminCorMenuTopoFonte { get; set; }
        public String AdminCorMenuTopoBotoesFundo { get; set; }
        public String AdminCorMenuTopoBotoesFonte { get; set; }
        public String AdminCorMenuTopoBotoesHoverFundo { get; set; }
        public String AdminCorMenuTopoBotoesHoverFonte { get; set; }
        //Menu
        public String AdminCorMenuFundo { get; set; }
        public String AdminCorMenuHoverFundo { get; set; }
        public String AdminCorMenuFonte { get; set; }
        public String AdminCorMenuHoverFonte { get; set; }
        //Conteudo
        //Geral
        public String AdminCorTituloFundo { get; set; }
        public String AdminCorTituloFonte { get; set; }
        public String AdminCorConteudoFundo { get; set; }
        public String AdminCorConteudoFonte { get; set; }
        //Botoes
        public String AdminCorBotoesFundo { get; set; }
        public String AdminCorBotoesFonte { get; set; }
        public String AdminCorBotoesHoverFundo { get; set; }
        public String AdminCorBotoesHoverFonte { get; set; }
        //Tabela
        public String AdminCorTabelaFundo { get; set; }
        public String AdminCorTabelaHeaderFonte { get; set; }
        public String AdminCorTabelaHeaderSelecionadoFonte { get; set; }
        public String AdminCorTabelaLinha1Fundo { get; set; }
        public String AdminCorTabelaLinha1Fonte { get; set; }
        public String AdminCorTabelaLinha2Fundo { get; set; }
        public String AdminCorTabelaLinha2Fonte { get; set; }
        public String AdminImagemTabelaHeaderSetaCima { get; set; }
        public String AdminImagemTabelaHeaderSetaBaixo { get; set; }
        //Conteudo Secundario
        public String AdminCorConteudoSecundarioFundo { get; set; }
        public String AdminCorConteudoSecundarioFonte { get; set; }
        //Rodape
        public String AdminCorRodapeBotoesFundo { get; set; }
        public String AdminCorRodapeBotoesFonte { get; set; }
        public String AdminCorRodapeBotoesHoverFundo { get; set; }
        public String AdminCorRodapeBotoesHoverFonte { get; set; }
    }
}
