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

        public static TerritorioInfo TerritorioInfo
        {
            get
            {
                return (TerritorioInfo)HttpContext.Current.Session["UsuarioTerritorioSite"];
            }
        }

        public static Boolean Logar(Usuario usuario)
        {
            return Logar(usuario, false);
        }

        public static Boolean Logar(Usuario usuario, bool simulado)
        {
            UsuarioInfo usuInfo = null;

            if (usuario == null)
                return false;
            
            if (simulado)
            {
                usuInfo = (UsuarioInfo)HttpContext.Current.Session["UsuarioSite"];

                if (usuInfo.IdOriginal == null) { 
                    usuInfo.IdOriginal = usuInfo.Id;
                    usuInfo.NomeOriginal = usuInfo.Nome;
                    usuInfo.EmailOriginal = usuInfo.Email;
                    usuInfo.LoginOriginal = usuInfo.Login;
                    usuInfo.StatusOriginal = usuInfo.Status;
                    usuInfo.IdGrupoOriginal = usuInfo.IdGrupo;
                }
            }
            else
            {
                usuInfo = new UsuarioInfo();
            }

            usuInfo.Id = usuario.Id;
            usuInfo.Nome = usuario.Nome;
            usuInfo.Email = usuario.Email;
            usuInfo.Login = usuario.Login;
            usuInfo.Status = usuario.Status;
            usuInfo.IdGrupo = usuario.IdGrupo;

            HttpContext.Current.Session["UsuarioSite"] = usuInfo;
            return true;
        }

        public static Boolean Logar(Territorio territorio)
        {
            if (territorio != null)
            {
                TerritorioInfo usuInfo = new TerritorioInfo();
                usuInfo.Id = territorio.Id;
                usuInfo.Nome = territorio.Nome;

                HttpContext.Current.Session["UsuarioTerritorioSite"] = usuInfo;
                return true;
            }
            return false;
        }

        public static void LogOffUsuarioSimulado()
        {
            UsuarioInfo usuInfo = null;

            usuInfo = (UsuarioInfo)HttpContext.Current.Session["UsuarioSite"];

            if (usuInfo.IdOriginal != null)
            {
                var usuario = new UsuarioRepository().GetUsuario(usuInfo.IdOriginal.Value);
                Logar(usuario);
            }
        }

        public static void LogOff()
        {
            HttpContext.Current.Session.Clear();
        }

        public static bool UsuarioLogado()
        {
            return (HttpContext.Current.Session["UsuarioSite"] != null && !String.IsNullOrEmpty(HttpContext.Current.Session["UsuarioSite"].ToString()));
        }

        public static bool UsuarioTerritorioLogado()
        {
            return (HttpContext.Current.Session["UsuarioTerritorioSite"] != null && !String.IsNullOrEmpty(HttpContext.Current.Session["UsuarioTerritorioSite"].ToString()));
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
            if (!Sessao.Site.UsuarioLogado())
            {
                return false;
            }

            Usuario usuario = RetornaUsuario();

            if (usuario.Grupo.AcaoGrupos.Any(acaoGrupo => acaoGrupo.Acao.Nome.ToUpper() == action.ToUpper() && acaoGrupo.Acao.Controlador.Nome.ToUpper() == controller.ToUpper()))
            {
                return true;
            }
            return false;
        }
    }
}
