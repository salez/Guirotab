using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sessao
{
    public class Config
    {
        public static bool AcessoBloqueado
        {
            get { return (QtdeTentativas >= QtdeMaxTentativas); }
        }

        public static int QtdeMaxTentativas = Convert.ToInt32(Util.Configuracao.AppSettings("QtdeMaxTentativasConfig"));

        public static int QtdeTentativas
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["QtdeTentativasConfig"]);
            }
            set { HttpContext.Current.Session["QtdeTentativasConfig"] = value; }
        }

        public static Boolean Logar(String senhaConfig)
        {
            if (AcessoBloqueado)
            {
                return false;
            }

            if (Util.Configuracao.AppSettings("SenhaConfig") == senhaConfig)
            {
                HttpContext.Current.Session["UsuarioConfig"] = true;
                return true;
            }

            QtdeTentativas++;

            return false;
        }

        public static void LogOff()
        {
            HttpContext.Current.Session["UsuarioConfig"] = false;
        }

        public static bool UsuarioLogado()
        {
            return (HttpContext.Current.Session["UsuarioConfig"] != null && Convert.ToBoolean(HttpContext.Current.Session["UsuarioConfig"]));
        }

    }
}
