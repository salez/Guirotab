using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace GuiropaIpad.Controllers
{
    public class AgenciaController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Agencia, Log.EnumArea.Site);
        ProdutoVaRepository vaRepository = new ProdutoVaRepository();
        //
        // GET: /Agencia/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Historico()
        {
            logs.Add(Log.EnumTipo.Alteracao, "Consultou o Histórico", Url.Action("Historico"));

            return View();
        }

        public ActionResult GetHistorico()
        {
            string result = String.Empty;
            var vas = Sessao.Site.RetornaUsuario().ProdutoVas;

            if (vas.Count() == 0)
                return new EmptyResult();

            string modelo = @"
                <tr class='even'>
                    <td align='center'>
                        [data]
                    </td>
                    <td>
                        [produto_nome]
                    </td>
                    <td>
                        [qtde_slides]
                    </td>
                    <td>
                        [status]
                    </td>
                </tr>".Replace("'", "\"");

            foreach (var va in vas)
            {
                result += modelo.ReplaceChaves(new
                {
                    data = va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno),
                    produto_nome = va.Produto.Nome,
                    qtde_slides = va.ProdutoVaSlides.Count().ToString(),
                    status = (va.Status.HasValue) ? ((ProdutoVa.EnumStatus)va.Status).GetDescription() : "",
                });
            }

            return Content(result);
        }

    }
}
