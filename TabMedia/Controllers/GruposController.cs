using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Controllers
{
    [AuthorizePermissao]
    public class GruposController : BaseController
    {
        GrupoRepository grupoRepository = new GrupoRepository();
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Grupos, Log.EnumArea.Admin);

        public ActionResult Index()
        {
            IQueryable<Grupo> grupos = grupoRepository.GetGrupos();

            logs.Add(Log.EnumTipo.Consulta, "Consultou os grupos", Url.Action("index"));
            return View(grupos);
        }

        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Cadastro(Grupo grupo)
        {
            var grupoBusca = grupoRepository.GetGrupo(grupo.Id);

            if (grupoBusca != null)
            {
                ModelState.AddModelError("TemGrupo", "Id já existente");
            }

            if (ModelState.IsValid)
            {
                grupoRepository.Add(grupo);
                grupoRepository.Save();

                logs.Add(Log.EnumTipo.Inclusao, "Cadastrou o grupo <i>" + grupo.Nome + "</i>", Url.Action("cadastro"));
                return RedirectToAction("index");
            }

            return Cadastro();
        }

        public ActionResult Alterar(string id)
        {
            Grupo grupo = grupoRepository.GetGrupo(id);

            return View("cadastro", grupo);
        }

        [HttpPost]
        public ActionResult Alterar(string id, Grupo grupoAlterado)
        {
            if (ModelState.IsValid)
            {
                Grupo grupo = grupoRepository.GetGrupo(id);

                grupo.Nome = grupoAlterado.Nome;

                grupoRepository.Save();

                logs.Add(Log.EnumTipo.Alteracao, "Alterou o grupo <i>" + grupo.Nome + "</i>", Url.Action("cadastro", new { id = id }));
                return RedirectToAction("index");
            }

            return Alterar(id);
        }

        public ActionResult Excluir(string id)
        {
            Grupo grupo = grupoRepository.GetGrupo(id);
            grupoRepository.Delete(grupo);
            grupoRepository.Save();

            logs.Add(Log.EnumTipo.Exclusao, "Excluiu o grupo <i>" + grupo.Nome + "</i>", Url.Action("index"));
            return RedirectToAction("index");
        }

        public ActionResult Permissoes(string id)
        {
            Grupo grupo = grupoRepository.GetGrupo(id);

            logs.Add(Log.EnumTipo.Consulta, "Consultou as permissões do grupo <i>" + grupo.Nome + "</i>", Url.Action("permissoes", new { id = id }));
            return View(new GruposFormView(grupo));
        }

        [HttpPost]
        public ActionResult Permissoes(string id, int[] acoes)
        {
            Grupo grupo = grupoRepository.GetGrupo(id);

            AcaoGrupoRepository acaoGrupoRepository = new AcaoGrupoRepository();

            acaoGrupoRepository.DeleteAll(id);

            if (acoes != null)
            {
                foreach (int idAcao in acoes)
                {
                    AcaoGrupo acaoGrupo = new AcaoGrupo();
                    acaoGrupo.IdGrupo = grupo.Id;
                    acaoGrupo.IdAcao = idAcao;

                    acaoGrupoRepository.Add(acaoGrupo);
                }
            }

            acaoGrupoRepository.Save();


            logs.Add(Log.EnumTipo.Alteracao, "Alterou as permissões do grupo <i>" + grupo.Nome + "</i>", Url.Action("permissoes", new { id = id }));
            return RedirectToAction("index");
        }
    }
}
