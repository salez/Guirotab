using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.Web.Mvc;

namespace Util
{
    public static class Sistema
    {
        public static bool AmbienteDesenvolvimento()
        {
            return !AmbienteProducao();
        }

        public static bool AmbienteProducao()
        {
            return HttpContext.Current.Request.Url.Host.Contains(Util.Configuracao.AppSettings("HostOnline").ToString());
        }

        public static String SiteUrl = (!AmbienteDesenvolvimento()) ? Util.Configuracao.AppSettings("SiteUrlOnline").ToString() : Util.Configuracao.AppSettings("SiteUrlLocal").ToString();

        public static String UrlBase = VirtualPathUtility.ToAbsolute("~/");

        public static Boolean IsPhone()
        {
            String tipoVisualizacao;
            String userAgent = HttpContext.Current.Request.UserAgent;

            if (userAgent.IndexOf("iPhone") != -1)
            {
                tipoVisualizacao = "Celular";
            }
            else if (userAgent.IndexOf("Symbian") != -1)
            {
                tipoVisualizacao = "Celular";
            }
            else if (userAgent.IndexOf("Android") != -1)
            {
                tipoVisualizacao = "Celular";
            }
            else if (userAgent.IndexOf("Win") != -1)
            {
                tipoVisualizacao = "Normal";
            }
            else if (userAgent.IndexOf("Mac") != -1)
            {
                tipoVisualizacao = "Normal";
            }
            else if (userAgent.IndexOf("X11") != -1)
            {
                tipoVisualizacao = "Normal";
            }
            else if (userAgent.IndexOf("Linux") != -1)
            {
                tipoVisualizacao = "Normal";
            }
            else
            {
                tipoVisualizacao = "Celular";
            }

            if (tipoVisualizacao == "Celular")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean IsIPhone()
        {
            String userAgent = HttpContext.Current.Request.UserAgent;

            if (userAgent.IndexOf("iPhone") != -1)
            {
                return true;
            }

            return false;
        }

        public static Boolean isIPad()
        {
            String userAgent = HttpContext.Current.Request.UserAgent;

            if (userAgent.IndexOf("iPad") != -1)
            {
                return true;
            }

            return false;
        }

        public static string GetTokenTerritorio(string idTerritorio)
        {
            return Util.Criptografia.CriptografaMd5(Util.Sistema.AppSettings.AppToken + idTerritorio);
        }

        public static class Error
        {
            /// <summary>
            /// Envia email de erro para o desenvolvedor e grava no banco
            /// </summary>
            /// <param name="ex"></param>
            /// <param name="Request"></param>
            /// <returns></returns>
            public static void TrataErro(Exception ex)
            {
                TrataErro(ex, null);
            }

            /// <summary>
            /// Envia email de erro para o desenvolvedor e grava no banco
            /// </summary>
            /// <param name="ex"></param>
            /// <param name="Request"></param>
            /// <returns></returns>
            public static void TrataErro(Exception ex, bool enviarEmail)
            {
                TrataErro(ex, null, enviarEmail);
            }

            public static void TrataErro(Exception ex, HttpRequestBase Request)
            {
                TrataErro(ex, Request, enviarEmail: true);
            }

            /// <summary>
            /// Envia email de erro para o desenvolvedor e grava no banco
            /// </summary>
            /// <param name="ex"></param>
            /// <param name="Request"></param>
            /// <returns></returns>
            public static void TrataErro(Exception ex, HttpRequestBase Request, bool enviarEmail)
            {
                #region Envia Email

                String mensagem = "";

                int cont = 1;
                while (ex != null)
                {
                    mensagem += "<fieldset>";
                    mensagem += "<p><strong>Erro:</strong></p>";
                    mensagem += "<p>" + ex.Message + "</p>";
                    mensagem += "<p><strong>Stack Trace:</strong></p>";
                    mensagem += "<p>" + ex.StackTrace.Nl2br() + "</p>";

                    String method = "";

                    if (ex.TargetSite != null)
                    {
                        method = ex.TargetSite.Name;
                    }

                    mensagem += "<p><strong>Method:</strong></p>";
                    mensagem += "<p>" + method + "</p>";
                    mensagem += "<p><strong>Source:</strong></p>";
                    mensagem += "<p>" + ex.Source + "</p>";

                    ex = ex.InnerException;
                    cont++;
                }
                for (int i = 1; i <= cont; i++)
                {
                    mensagem += "</fieldset>";
                }
                
                String urlreferrer = "";
                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    urlreferrer = HttpContext.Current.Request.UrlReferrer.ToString();
                }
                else
                {
                    urlreferrer = " - ";
                }

                String infoUsuario = "";

                #region Informações do Usuário

                try
                {
                    if (Sessao.Site.UsuarioLogado())
                    {
                        UsuarioInfo usuario = Sessao.Site.UsuarioInfo;
                        infoUsuario += "<p><strong>Id:</strong>";
                        infoUsuario += usuario.Id.ToString();
                        infoUsuario += "<br><strong>Nome:</strong>";
                        infoUsuario += usuario.Nome.ToString();
                        infoUsuario += "</p>";
                    }
                    else
                    {
                        infoUsuario = "<font color='blue'>Usuário não logado</font>";
                    }
                }
                catch (Exception e)
                {
                    infoUsuario += "<p><font color='red'>Ocorreu um erro ao tentar recuperar as informações do usuário.</font></p>";
                    infoUsuario += "<fieldset><legend>Detalhes do Erro</legend>";

                    cont = 1;
                    while (e != null)
                    {
                        infoUsuario += "<fieldset><p><strong>Erro:</strong></p>";
                        infoUsuario += "<p>" + e.Message + "</p>";
                        infoUsuario += "<p><strong>Stack Trace:</strong></p>";
                        infoUsuario += "<p>" + e.StackTrace + "</p>";

                        String method = "";

                        if (e.TargetSite != null)
                        {
                            infoUsuario += e.TargetSite.Name;
                        }

                        infoUsuario += "<p><strong>Method:</strong></p>";
                        infoUsuario += "<p>" + method + "</p>";
                        infoUsuario += "<p><strong>Source:</strong></p>";
                        infoUsuario += "<p>" + e.Source + "</p>";

                        e = e.InnerException;
                        cont++;
                    }
                    for (int i = 1; i <= cont; i++)
                    {
                        mensagem += "</fieldset>";
                    }
                    infoUsuario += "</fieldset>";
                }

                #endregion

                String gravouErro = "";

                #region Gravar Erro no Banco

                ErroRepository erroRepository = null;
                Erro erro = null;

                try
                {
                    erroRepository = new ErroRepository();

                    erro = new Erro();

                    erro.Pagina = HttpContext.Current.Request.Url.ToString();
                    erro.PaginaAnterior = urlreferrer.ToString();
                    erro.RequestHost = HttpContext.Current.Request.UserHostAddress.ToString();
                    erro.HostName = HttpContext.Current.Request.UserHostName.ToString();
                    erro.UserAgent = HttpContext.Current.Request.UserAgent.ToString();
                    erro.InfoUsuario = infoUsuario.ToString();
                    erro.ErroMsg = mensagem.ToString();

                    erroRepository.Add(erro);
                    erroRepository.Save();

                    gravouErro = "<font color='blue'>Sim (Erro #" + erro.Id + ")</font>";
                }
                catch (Exception e)
                {
                    gravouErro = "<p><font color='red'>Ocorreu um erro ao tentar gravar o erro no banco.</font></p>";
                    gravouErro = "<fieldset><legend>Detalhes do Erro</legend>";

                    cont = 1;
                    while (e != null)
                    {
                        gravouErro += "<fieldset><p><strong>Erro:</strong></p>";
                        gravouErro += "<p>" + e.Message + "</p>";
                        gravouErro += "<p><strong>Stack Trace:</strong></p>";
                        gravouErro += "<p>" + e.StackTrace + "</p>";

                        String method = "";

                        if (e.TargetSite != null)
                        {
                            gravouErro = e.TargetSite.Name;
                        }

                        gravouErro += "<p><strong>Method:</strong></p>";
                        gravouErro += "<p>" + method + "</p>";
                        gravouErro += "<p><strong>Source:</strong></p>";
                        gravouErro += "<p>" + e.Source + "</p>";

                        e = e.InnerException;
                        cont++;
                    }
                    for (int i = 1; i <= cont; i++)
                    {
                        mensagem += "</fieldset>";
                    }
                    gravouErro = "</fieldset>";
                }

                #endregion

                if (enviarEmail)
                {
                    String body = Util.Email.GetCorpoEmail("erro");

                    body = body
                        .Replace("[data]", Util.Data.Formata(DateTime.Now, Util.Data.FormatoData.Completo))
                        .Replace("[pagina]", HttpContext.Current.Request.Url.ToString())
                        .Replace("[pagina_anterior]", urlreferrer)
                        .Replace("[request_host]", HttpContext.Current.Request.UserHostAddress.ToString())
                        .Replace("[host_name]", HttpContext.Current.Request.UserHostName.ToString())
                        .Replace("[user_agent]", HttpContext.Current.Request.UserAgent.ToString())
                        .Replace("[requisicao_ajax]", (Request != null) ? (Request.IsAjaxRequest()) ? "Sim" : "Não" : " - ")
                        .Replace("[info_usuario]", infoUsuario)
                        .Replace("[gravou_erro]", gravouErro)
                        .Replace("[erro_msg]", mensagem);

                    try
                    {
                        Util.Email.Enviar(Util.Email.EmailEnvioPadrao, Util.Email.EmailDesenvolvedor, body, "Ocorreu algum erro");

                        if (erro != null)
                        {
                            erro.EnviouEmail = true;

                            erroRepository.Save();
                        }
                    }
                    catch
                    {
                        if (erro != null)
                        {
                            erro.EnviouEmail = false;

                            erroRepository.Save();
                        }
                    }
                }
                else
                {
                    erro.EnviouEmail = false;

                    erroRepository.Save();
                }

                #endregion
            }
        }

        public static class AppSettings
        {

            public static String NomeSistema = Util.Configuracao.AppSettings("NomeSistema");

            public static String AppToken = (Util.Configuracao.AppSettings("AppToken") == null) ? "%8u!70p@" : Util.Configuracao.AppSettings("AppToken");

            public static String UrlDownloadBase = Util.Configuracao.AppSettings("UrlDownloadBase");

            public static String UrlDownloadBaseProdutosImagens = Util.Configuracao.AppSettings("UrlDownloadBaseProdutosImagens");

            public static String UrlImageMagickConvert = Util.Configuracao.AppSettings("UrlImageMagickConvert");

            public static String UrlFFMpeg = Util.Configuracao.AppSettings("UrlFFMpeg");

            public static String UrlIECapt = Util.Configuracao.AppSettings("UrlIECapt");

            public static int CampanhaResolucaoLargura
            {
                get
                {
                    if (Util.Configuracao.AppSettings("CampanhaResolucaoLargura") != null && Util.Configuracao.AppSettings("CampanhaResolucaoLargura") != String.Empty)
                    {

                        return Convert.ToInt32(Util.Configuracao.AppSettings("CampanhaResolucaoLargura").ToString());

                    }
                    else
                    {

                        return 1024;

                    }
                }
            }

            public static int CampanhaResolucaoAltura
            {
                get
                {
                    if (Util.Configuracao.AppSettings("CampanhaResolucaoAltura") != null && Util.Configuracao.AppSettings("CampanhaResolucaoAltura") != String.Empty)
                    {

                        return Convert.ToInt32(Util.Configuracao.AppSettings("CampanhaResolucaoAltura").ToString());

                    }
                    else
                    {

                        return 768;

                    }
                }
            }

            public static class Customizacao
            {
                public static String ClasseMenuSelecionado = Util.Configuracao.AppSettings("AdminClasseMenuSelecionado");
            }

            public static class Diretorios
            {
                public static String DiretorioImportacao = Util.Configuracao.AppSettings("DiretorioImportacao");

                public static String DiretorioModelos = Util.Configuracao.AppSettings("DiretorioModelos");

                public static String DiretorioProdutos = Util.Configuracao.AppSettings("DiretorioProdutos");

                public static String DiretorioProdutosImages = Util.Configuracao.AppSettings("DiretorioProdutos") + "_images/";

                public static String DiretorioGerenciadorArquivos = Util.Configuracao.AppSettings("DiretorioGerenciadorArquivos");

                public static String CaminhoProdutosImagesZip = Util.Configuracao.AppSettings("DiretorioProdutos") + "_images.zip";

            }

            public static class Email
            {
                public static Boolean EnviarEmailErro
                {
                    get
                    {
                        bool result;

                        Boolean.TryParse(Util.Configuracao.AppSettings("EnviarEmailErro"), out result);

                        return result;
                    }
                }

                public static Boolean EnviarEmail
                {
                    //retorna verdadeiro caso nao haja a chave no config
                    get
                    {
                        bool result;

                        try
                        {
                            result = (Util.Configuracao.AppSettings("EnviarEmail") != null) ? Boolean.Parse(Util.Configuracao.AppSettings("EnviarEmail")) : true;
                        }
                        catch
                        {
                            result = true;
                        }

                        return result;
                    }
                }

                public static bool SmtpAutenticado
                {
                    get
                    {
                        return Convert.ToBoolean(Util.Configuracao.AppSettings("SmtpAutenticado"));
                    }
                }

                public static string SmtpHost
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("SmtpHost").ToString();
                    }
                }

                public static string SmtpUsername
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("SmtpUsername").ToString();
                    }
                }

                public static string SmtpPassword
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("SmtpPassword").ToString();
                    }
                }

                public static string EmailEnvioPadrao
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("EmailEnvioPadrao").ToString();
                    }
                }

                public static string NomeEnvioPadrao
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("NomeEnvioPadrao").ToString();
                    }
                }

                public static string EmailDesenvolvedor
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("EmailDesenvolvedor").ToString();
                    }
                }

                public static string EmailContatoPadrao
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("EmailContatoPadrao").ToString();
                    }
                }

                public static string NomeContatoPadrao
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("NomeContatoPadrao").ToString();
                    }
                }

                public static bool EnableSsl
                {
                    get
                    {
                        bool result;

                        Boolean.TryParse(Util.Configuracao.AppSettings("EnableSsl"), out result);

                        return result;
                    }
                }

                public static int? SmtpPort
                {
                    get
                    {
                        if (Util.Configuracao.AppSettings("SmtpPort") != null && Util.Configuracao.AppSettings("SmtpPort") != String.Empty)
                        {

                            return Convert.ToInt32(Util.Configuracao.AppSettings("SmtpPort").ToString());

                        }
                        else
                        {

                            return null;

                        }
                    }
                }

                public static string EmailEncoding
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("EmailEncoding").ToString();
                    }
                }
            }

            public static class Autenticacao
            {
                public static string UrlLogin
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("UrlLogin").ToString();
                    }
                }

                public static string UrlLoginTerritorio
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("UrlLoginTerritorio").ToString();
                    }
                }

                public static string UrlDefault
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("UrlDefault").ToString();
                    }
                }

                public static string UrlDefaultTerritorio
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("UrlDefaultTerritorio").ToString();
                    }
                }

                public static string UrlLoginAdmin
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("UrlLoginAdmin").ToString();
                    }
                }

                public static string UrlDefaultAdmin
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("UrlDefaultAdmin").ToString();
                    }
                }

                public static string UrlLoginConfig
                {
                    get
                    {
                        return Util.Configuracao.AppSettings("UrlLoginConfig").ToString();
                    }
                }
            }

            public static class Visitantes
            {
                public static bool ControlarVisitantesOnline
                {
                    get
                    {
                        bool result;

                        Boolean.TryParse(Util.Configuracao.AppSettings("ControlarVisitantesOnline"), out result);

                        return result;
                    }
                }
            }

        }

        
    }
}
