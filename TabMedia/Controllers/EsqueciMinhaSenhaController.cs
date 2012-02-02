using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Controllers
{
    public class EsqueciMinhaSenhaController : BaseController
    {
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        //
        // GET: /Admin/EsqueciMinhaSenha/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(String email)
        {
            Usuario usuario = usuarioRepository.GetUsuarioByEmail(email);

            if (usuario != null)
            {
                String codigoRecuperacaoSenha = Util.Texto.GerarSenhaAleatoria(14);
                String body = Util.Email.GetCorpoEmail("esqueciminhasenha");

                body =
                    body.Replace("[link_recuperacao_senha]", Util.Sistema.SiteUrl + "/esqueciminhasenha/recuperar?codigo=" + codigoRecuperacaoSenha + "&usu=" + usuario.Id);

                Util.Email.Enviar(new System.Net.Mail.MailAddress(email), body, "Recuperação de Senha");

                usuario.CodigoRecuperacaoSenha = codigoRecuperacaoSenha;
                usuarioRepository.Save();

                return RedirectToAction("sucesso");
            }

            ViewData["msg"] = "<font color='red'>Email inválido</font>.";
            return View();
        }

        [HttpGet]
        public ActionResult Recuperar(string codigo, int usu)
        {
            Usuario usuario = usuarioRepository.GetUsuario(usu);

            if (usuario == null || usuario.CodigoRecuperacaoSenha != codigo || usuario.CodigoRecuperacaoSenha == String.Empty || usuario.CodigoRecuperacaoSenha == null)
            {
                return RedirectToAction("index", "login");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Recuperar(string senha)
        {
            string codigo = Request.QueryString["codigo"];
            int usu = Int32.Parse(Request.QueryString["usu"]);

            Usuario usuario = usuarioRepository.GetUsuario(usu);

            if (usuario == null || usuario.CodigoRecuperacaoSenha != codigo || usuario.CodigoRecuperacaoSenha == String.Empty || usuario.CodigoRecuperacaoSenha == null)
            {
                return RedirectToAction("index", "login");
            }

            usuario.Senha = senha.ToMD5();
            usuario.CodigoRecuperacaoSenha = String.Empty;
            usuarioRepository.Save();

            return RedirectToAction("index", "login");
        }

        public ActionResult Sucesso()
        {
            return View();
        }

    }
}
