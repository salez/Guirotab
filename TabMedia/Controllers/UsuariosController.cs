using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Controllers
{
    [AuthorizePermissao]
    public class UsuariosController : BaseController
    {
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        GrupoRepository grupoRepository = new GrupoRepository();
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Usuarios, Log.EnumArea.Admin);

        public ActionResult Index()
        {
            IQueryable<Usuario> usuarios = usuarioRepository.GetUsuarios();

            SelectList selectList = new SelectList(grupoRepository.GetGrupos(),"Id","Nome");
            ViewData["grupos"] = selectList;

            logs.Add(Log.EnumTipo.Consulta, "Consultou os usuários", Url.Action("index"));
            return View(usuarios);
        }
        
        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            IQueryable<Usuario> usuarios = usuarioRepository.GetUsuarios();

            if (usuario.Nome != null)
                usuarios = usuarios.Where(u => u.Nome.Contains(usuario.Nome));

            if (usuario.Status != null)
                usuarios = usuarios.Where(u => u.Status == usuario.Status);

            if (usuario.IdGrupo != null)
                usuarios = usuarios.Where(u => u.IdGrupo == usuario.IdGrupo);

            SelectList selectList = new SelectList(grupoRepository.GetGrupos(), "Id", "Nome");
            ViewData["grupos"] = selectList;

            logs.Add(Log.EnumTipo.Consulta, "Consultou os usuários", Url.Action("index"));
            return View(usuarios);
        }

        public ActionResult Cadastro()
        {
            ViewData["Grupos"] = grupoRepository.GetGrupos().ToSelectList("Id", "Nome", 1);
            ViewData["TipoCadastro"] = Util.Cadastro.Tipo.Inclusao;

            return View();
        }

        [HttpPost]
        [NotValidateFields(new string [] {"Login"})]
        public ActionResult Cadastro(Usuario usuario)
        {
            if (usuarioRepository.EmailExiste(usuario.Email))
            {
                ModelState.AddModelError("Email", "Email já existente.");
            }

            if (ModelState.IsValid)
            {
                usuario.Senha = usuario.Senha.ToMD5();

                usuarioRepository.Add(usuario);
                usuarioRepository.Save();

                if (usuario.IsAgencia())
                {
                    usuario.TerritorioSimulado = "a" + usuario.Id;
                }else if (usuario.IsGerente())
                {
                    usuario.TerritorioSimulado = "g" + usuario.Id;
                }

                usuarioRepository.Save();

                logs.Add(Log.EnumTipo.Consulta, "Cadastrou o usuário <i>" + usuario.Nome + "</i>", Url.Action("index"), usuario.Id);
                return RedirectToAction("index");
            }

            ViewData["Grupos"] = grupoRepository.GetGrupos().ToSelectList("Id", "Nome", 1);
            ViewData["TipoCadastro"] = Util.Cadastro.Tipo.Inclusao;
            return View(usuario);
        }

        public ActionResult Alterar(int id)
        {
            Usuario usuario = usuarioRepository.GetUsuario(id);

            ViewData["Grupos"] = grupoRepository.GetGrupos().ToSelectList("Id", "Nome", 1);
            ViewData["TipoCadastro"] = Util.Cadastro.Tipo.Alteracao;

            return View("Cadastro", usuario);
        }

        [HttpPost]
        [NotValidateFields(new string[] { "Login" })]
        public ActionResult Alterar(int id, Usuario usuarioEditado)
        {
            Usuario usuario = usuarioRepository.GetUsuario(id);

            if (usuario.Email != usuarioEditado.Email && usuarioRepository.EmailExiste(usuarioEditado.Email))
            {
                ModelState.AddModelError("Email", "Email já existente.");
            }

            if (usuario.Login != usuarioEditado.Login && usuarioRepository.LoginExiste(usuario.Login))
            {
                ModelState.AddModelError("Login", "Login já existente.");
            }

            if (ModelState.IsValid)
            {
                usuario.Login = usuarioEditado.Login;
                usuario.Nome = usuarioEditado.Nome;
                usuario.Email = usuarioEditado.Email;
                if (usuarioEditado.Senha != null && usuarioEditado.Senha != String.Empty)
                {
                    usuario.Senha = usuarioEditado.Senha.ToMD5();
                }
                usuario.Status = usuarioEditado.Status;
                usuario.IdGrupo = usuarioEditado.IdGrupo;

                if (usuario.IsAgencia())
                {
                    usuario.TerritorioSimulado = "a" + usuario.Id;
                }
                else if (usuario.IsGerente())
                {
                    usuario.TerritorioSimulado = "g" + usuario.Id;
                }
                else
                {
                    usuario.TerritorioSimulado = "";
                }

                usuarioRepository.Save();

                logs.Add(Log.EnumTipo.Alteracao, "Alterou o usuário <i>" + usuario.Nome + "</i>", Url.Action("index"), usuario.Id);
                return RedirectToAction("index");
            }

            ViewData["Grupos"] = grupoRepository.GetGrupos().ToSelectList("Id", "Nome", 1);
            ViewData["TipoCadastro"] = Util.Cadastro.Tipo.Alteracao;

            return View("cadastro", usuario);
        }

        public ActionResult Excluir(int id)
        {
            Usuario usuario = usuarioRepository.GetUsuario(id);
            usuarioRepository.Delete(usuario);
            usuarioRepository.Save();

            logs.Add(Log.EnumTipo.Exclusao, "Excluiu o usuário <i>" + usuario.Nome + "</i>", Url.Action("index"), usuario.Id);
            return RedirectToAction("index");
        }

    }
}
