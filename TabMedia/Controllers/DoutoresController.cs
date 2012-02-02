using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace GuiropaIpad.Controllers
{
    [AuthorizeLogin]
    public class DoutoresController : BaseController
    {
        DoutorRepository doutorRepository = new DoutorRepository();
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Doutores, Log.EnumArea.Site);

        [AuthorizePermissao]
        public ActionResult Index()
        {
            logs.Add(Log.EnumTipo.Consulta, "Consultou os Doutores", Url.Action("Index"));

            return View();
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetDoutores()
        {
            var doutores = doutorRepository.GetDoutors();

            string result = String.Empty;

            string modelo = Html("GetDoutores");

            foreach (var doutor in doutores)
            {
                var strEspecialidades = string.Empty;

                foreach (var especialidade in doutor.DoutorEspecialidades.Select(de => de.Especialidade))
                {
                    strEspecialidades += especialidade.Nome + " ";
                }

                result += modelo.ReplaceChaves(new
                {
                    doutor_linha            = doutor.ProdutoLinha.Nome,
                    doutor_territorio       = doutor.IdTerritorio,
                    doutor_crm              = doutor.CRM,
                    doutor_crmUf            = doutor.CRMUF,
                    doutor_nome             = doutor.Nome,
                    doutor_dataInclusao     = doutor.DataInclusao,
                    doutor_especialidades   = strEspecialidades,
                    doutor_endereco         = doutor.Endereco,
                    //doutor_telefone         = "(" + doutor.TelefoneDDD + ") " + doutor.Telefone.FormataTelefone(comDDD: false),
                    doutor_versao           = doutor.Versao
                });
            }

            return Content(result);
        }

    }
}
