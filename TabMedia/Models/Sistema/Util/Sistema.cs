using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Util
{
    public static class Sistema
    {
        public static bool AmbienteDesenvolvimento()
        {
            return !HttpContext.Current.Request.Url.Host.Contains(Util.Configuracao.AppSettings("HostOnline").ToString());
        }

        public static String SiteUrl = (!AmbienteDesenvolvimento()) ? Util.Configuracao.AppSettings("SiteUrlOnline").ToString() : Util.Configuracao.AppSettings("SiteUrlLocal").ToString();

        public static String UrlBase = VirtualPathUtility.ToAbsolute("~/");

        public static String NomeSistema = Util.Configuracao.AppSettings("NomeSistema");

        public static String DiretorioProdutos = Util.Configuracao.AppSettings("DiretorioProdutos");

        public static String DiretorioGerenciadorArquivos = Util.Configuracao.AppSettings("DiretorioGerenciadorArquivos");

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

        public static Boolean EnviarEmailErro
        {
            get
            {
                bool result;

                Boolean.TryParse(Util.Configuracao.AppSettings("EnviarEmailErro"), out result);

                return result;
            }
        }

        public static class Email
        {
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

            public static string UrlDefault
            {
                get
                {
                    return Util.Configuracao.AppSettings("UrlDefault").ToString();
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
