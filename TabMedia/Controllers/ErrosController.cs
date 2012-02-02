using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Controllers
{
    [AuthorizePermissao]
    public class ErrosController : BaseController
    {
        ErroRepository erroRepository = new ErroRepository();
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Erros, Log.EnumArea.Admin);
        //
        // GET: /Admin/Erros/

        public ActionResult Index()
        {
            IQueryable<Erro> erros = erroRepository.GetErros().OrderByDescending(e => e.DataInclusao);

            logs.Add(Log.EnumTipo.Consulta, "Consultou os erros", Url.Action("index"));
            return View(erros);
        }

        public ActionResult ExibeInfo(int? id)
        {
            if (id.HasValue)
            {
                var erro = erroRepository.GetErro(id.Value);

                if (erro != null)
                {
                    return View(erro);
                }
            }
            return new EmptyResult();
        }
    }
}
