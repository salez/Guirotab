using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Sessao
{
    public class Site
    {

        public static UsuarioInfo UsuarioInfo
        {
            get
            {
                return (UsuarioInfo)HttpContext.Current.Session["UsuarioSite"];
            }
        }

        public static Boolean Logar(Usuario usuario)
        {
            if (usuario != null)
            {
                UsuarioInfo usuInfo = new UsuarioInfo();
                usuInfo.Id = usuario.Id;
                usuInfo.Nome = usuario.Nome;
                usuInfo.Email = usuario.Email;
                usuInfo.Login = usuario.Login;
                usuInfo.Status = usuario.Status;
                usuInfo.IdGrupo = usuario.IdGrupo;

                HttpContext.Current.Session["UsuarioSite"] = usuInfo;
                return true;
            }
            return false;
        }

        public static void LogOff()
        {
            HttpContext.Current.Session.Clear();
        }

        public static bool UsuarioLogado()
        {
            return (HttpContext.Current.Session["UsuarioSite"] != null && !String.IsNullOrEmpty(HttpContext.Current.Session["UsuarioSite"].ToString()));
        }

        public static Usuario RetornaUsuario()
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();

            UsuarioInfo usuInfo = (UsuarioInfo)HttpContext.Current.Session["UsuarioSite"];

            Usuario usuario = usuarioRepository.GetUsuario(usuInfo.Id);

            return usuario;
        }

        public static bool VerificaPermissao(String action, String controller)
        {
            Usuario usuario = RetornaUsuario();

            if (usuario.Grupo.AcaoGrupos.Any(acaoGrupo => acaoGrupo.Acao.Nome.ToUpper() == action.ToUpper() && acaoGrupo.Acao.Controlador.Nome.ToUpper() == controller.ToUpper()))
            {
                return true;
            }
            return false;
        }
    }
}
