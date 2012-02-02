using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace GuiropaIpad.Controllers
{
    [AuthorizeLogin]
    public class TerritoriosController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Territorios, Log.EnumArea.Site);
        TerritorioRepository territorioRepository = new TerritorioRepository();

        [AuthorizePermissao]
        public ActionResult Index()
        {
            logs.Add(Log.EnumTipo.Consulta, "Consultou os Territórios", Url.Action("Index"));

            return View();
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetTerritorios()
        {
            var territorios = territorioRepository.GetTerritorios();

            string result = String.Empty;

            string modelo = Html("GetTerritorios");

            foreach (var territorio in territorios)
            {
                string status = string.Empty;
                
                if(territorio.Status != null){
                    status = ((Territorio.EnumStatus)territorio.Status).GetDescription();
                }
                
                result += modelo.ReplaceChaves(new
                {
                    id = territorio.Id,
                    nome = territorio.Nome,
                    email = territorio.Email,
                    cidade = territorio.Cidade,
                    linha = territorio.ProdutoLinha.Nome,
                    estado = territorio.IdEstado,
                    status = status,
                    url_produtos = Url.Action("index","produtos", new { idTerritorio = territorio.Id })
                });
            }

            return Content(result);
        }

    }
}
