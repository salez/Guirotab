using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Controllers
{
    [AuthorizePermissao]
    public class PaginasController : BaseController
    {
        ControladorRepository controladorRepository = new ControladorRepository();
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Paginas, Log.EnumArea.Admin);
        //
        // GET: /Admin/Paginas/

        public ActionResult Index()
        {
            IQueryable<Controlador> controladores = controladorRepository.GetControladores();

            logs.Add(Log.EnumTipo.Consulta, "Consultou os controladores e ações", Url.Action("index"));
            return View(controladores);
        }

        public ActionResult CadastroControlador()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CadastroControlador(Controlador controlador)
        {
            if (ModelState.IsValid)
            {
                controladorRepository.Add(controlador);
                controladorRepository.Save();

                logs.Add(Log.EnumTipo.Inclusao, "Cadastrou o controlador <i>" + controlador.Nome + "</i>", Url.Action("index"), controlador.Id);
                return RedirectToAction("index");
            }
            return CadastroControlador();
        }

        public ActionResult AlterarControlador(int id)
        {
            Controlador controlador = controladorRepository.GetControlador(id);

            return View("cadastrocontrolador", controlador);
        }

        [HttpPost]
        public ActionResult AlterarControlador(int id, Controlador controladorAlterado)
        {
            if (ModelState.IsValid)
            {
                Controlador controlador = controladorRepository.GetControlador(id);

                UpdateModel(controlador);

                controladorRepository.Save();

                logs.Add(Log.EnumTipo.Alteracao, "Alterou o controlador <i>" + controlador.Nome + "</i>", Url.Action("index"), controlador.Id);
                return RedirectToAction("index");
            }

            return AlterarControlador(id);
        }

        public ActionResult ExcluirControlador(int id)
        {
            Controlador controlador = controladorRepository.GetControlador(id);
            controladorRepository.Delete(controlador);
            controladorRepository.Save();

            logs.Add(Log.EnumTipo.Exclusao, "Excluiu o controlador <i>" + controlador.Nome + "</i>", Url.Action("index"), controlador.Id);
            return RedirectToAction("index");
        }

        AcaoRepository acaoRepository = new AcaoRepository();

        public ActionResult CadastroAcao(int id)
        {
            ViewData["Controlador"] = controladorRepository.GetControlador(id).Nome;
            return View();
        }

        [HttpPost]
        public ActionResult CadastroAcao(int id, Acao acao)
        {
            if (ModelState.IsValid)
            {
                acao.IdControlador = id;
                acaoRepository.Add(acao);
                acaoRepository.Save();

                logs.Add(Log.EnumTipo.Inclusao, "Cadastrou a ação <i>" + acao.Nome + "</i> no controlador <i>" + acao.Controlador.Nome + "</i>", Url.Action("index"), acao.Id);
                return RedirectToAction("index");
            }

            return CadastroAcao(id);
        }

        public ActionResult AlterarAcao(int id)
        {
            Acao acao = acaoRepository.GetAcao(id);

            return View("cadastroacao", acao);
        }

        [HttpPost]
        public ActionResult AlterarAcao(int id, Acao acaoAlterada)
        {
            if (ModelState.IsValid)
            {
                Acao acao = acaoRepository.GetAcao(id);
                UpdateModel(acao);

                acaoRepository.Save();

                logs.Add(Log.EnumTipo.Alteracao, "Alterou a ação <i>" + acao.Nome + "</i> do controlador <i>" + acao.Controlador.Nome + "</i>", Url.Action("index"), acao.Id);
                return RedirectToAction("index");
            }

            return AlterarAcao(id);
        }

        public ActionResult ExcluirAcao(int id)
        {
            Acao acao = acaoRepository.GetAcao(id);
            acaoRepository.Delete(acao);
            acaoRepository.Save();

            logs.Add(Log.EnumTipo.Exclusao, "Excluiu a ação <i>" + acao.Nome + "</i> do controlador <i>" + acao.Controlador.Nome + "</i>", Url.Action("index"), acao.Id);
            return RedirectToAction("index");
        }
    }
}
