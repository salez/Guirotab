using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Controllers
{
    public class LoginController : BaseController
    {
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        TerritorioRepository territorioRepository = new TerritorioRepository();
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Sistema, Log.EnumArea.Site);
        Util.Logs logsTerritorio = new Util.Logs(Log.EnumPagina.Instalacao, Log.EnumArea.Site);
        //
        // GET: /Admin/Login/

        public ActionResult Index()
        {
            if (Request.QueryString["erro"] == "permissao")
            {
                ViewData["erro"] = "<font color='red'>Você não tem permissão para executar esta ação.</font>";
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(String email, String senha)
        {
            if (email == string.Empty || senha == string.Empty)
            {
                goto erro;
            }

            Usuario usuario = usuarioRepository.GetUsuarioByEmailSenha(email, senha.ToMD5());

            Territorio territorio = territorioRepository.GetTerritorio(email, senha.ToMD5());

            if (usuario != null)
            {
                if (Sessao.Site.Logar(usuario))
                {
                    usuario.DataUltimoLogin = DateTime.Now;
                    usuarioRepository.Save();

                    logs.Add(Log.EnumTipo.Login, "Logou no sistema", Url.Action("index"));
                    return Redirect(Autenticacao.GetRedirectFromLoginPage(Util.Sistema.AppSettings.Autenticacao.UrlDefault));
                }
            }

            if (territorio != null)
            {
                if (Sessao.Site.Logar(territorio))
                {
                    usuarioRepository.Save();

                    logsTerritorio.Add(Log.EnumTipo.Login, "Logou no sistema", Url.Action("index"));
                    return Redirect(Util.Sistema.AppSettings.Autenticacao.UrlDefaultTerritorio);
                }
            }

        erro:

            ViewData["erro"] = "<font color='red'>Login e/ou senha inválido(s).</font>";

            return View();
        }

        //[HttpPost]
        //public ActionResult Index(String email, String senha)
        //{
        //    if (email == string.Empty || senha == string.Empty)
        //    {
        //        goto erro;
        //    }

        //    Usuario usuario = usuarioRepository.GetUsuarioByEmailSenha(email, senha.ToMD5());

        //    if (usuario != null)
        //    {
        //        if (Sessao.Site.Logar(usuario))
        //        {
        //            usuario.DataUltimoLogin = DateTime.Now;
        //            usuarioRepository.Save();

        //            logs.Add(Log.EnumTipo.Login, "Logou no sistema", Url.Action("index"));
        //            return Redirect(Autenticacao.GetRedirectFromLoginPage(Util.Sistema.AppSettings.Autenticacao.UrlDefault));
        //        }
        //    }

        //    erro:

        //    ViewData["erro"] = "<font color='red'>Login e/ou senha inválido(s).</font>";

        //    return View();
        //}


        public ActionResult Territorio()
        {
            if (Request.QueryString["erro"] == "permissao")
            {
                ViewData["erro"] = "<font color='red'>Você não tem permissão para executar esta ação.</font>";
            }

            return View();
        }

        [HttpPost]
        public ActionResult Territorio(String idTerritorio, String senha)
        {
            Sessao.Site.LogOff();

            if (idTerritorio == string.Empty || senha == string.Empty || senha != "ems@2011")
            {
                goto erro;
            }

            Territorio territorio = territorioRepository.GetTerritorio(idTerritorio);

            if (territorio != null)
            {
                if (Sessao.Site.Logar(territorio))
                {
                    usuarioRepository.Save();

                    logsTerritorio.Add(Log.EnumTipo.Login, "Logou no sistema", Url.Action("index"));
                    return Redirect(Autenticacao.GetRedirectFromLoginPage(Util.Sistema.AppSettings.Autenticacao.UrlDefaultTerritorio));
                }
            }

        erro:

            ViewData["erro"] = "<font color='red'>Login e/ou senha inválido(s).</font>";

            return View();
        }

        public ActionResult Logoff()
        {
            if (Sessao.Site.UsuarioLogado()) { 
                logs.Add(Log.EnumTipo.Login, "Deslogou do sistema", Url.Action("index"));
            }
            if (Sessao.Site.UsuarioTerritorioLogado()) {
                logsTerritorio.Add(Log.EnumTipo.Login, "Deslogou do sistema", Url.Action("index"));
            }

            if (Sessao.Site.UsuarioLogado())
            {
                Sessao.Site.LogOff();
                return RedirectToAction("index");
            }
            else
            {
                Sessao.Site.LogOff();
                return RedirectToAction("territorio");
            }
        }
    }
}
