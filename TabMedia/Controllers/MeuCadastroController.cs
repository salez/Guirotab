using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Controllers
{
    [AuthorizeLogin]
    public class MeuCadastroController : BaseController
    {
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        Util.Logs logs = new Util.Logs(Log.EnumPagina.MeuCadastro, Log.EnumArea.Site);
        //
        // GET: /Admin/MeuCadastro/
        public ActionResult Index()
        {
            Usuario usuario = usuarioRepository.GetUsuario(Sessao.Site.RetornaUsuario().Id);

            logs.Add(Log.EnumTipo.Consulta, "Consultou o seu cadastro", String.Empty, usuario.Id);
            return View(usuario);
        }

        [HttpPost]
        [ValidateOnlyFields(new[] { "Nome", "Senha" })]
        public ActionResult Index(Usuario usuarioEditado)
        {
            Usuario usuario = usuarioRepository.GetUsuario(Sessao.Site.RetornaUsuario().Id);

            if (ModelState.IsValid)
            {
                usuario.Nome = usuarioEditado.Nome;

                if (usuarioEditado.Senha != null && usuarioEditado.Senha != String.Empty)
                {
                    usuario.Senha = usuarioEditado.Senha.ToMD5();
                }

                usuarioRepository.Save();

                logs.Add(Log.EnumTipo.Alteracao, "Alterou o seu cadastro", String.Empty, usuario.Id);
                return RedirectToAction("sucesso");
            }

            return View(usuario);
        }

        public ActionResult Sucesso()
        {
            return View();
        }

    }
}