using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace GuiropaIpad.Controllers
{
    [AuthorizeLoginTerritorio]
    public class InstalacaoController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Instalacao, Log.EnumArea.Site);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Instalar()
        {
            logs.Add(Log.EnumTipo.Consulta, "Baixou o aplicativo", Url.Action("index"));

            return Redirect("itms-services://?action=download-manifest&url=http://guiropatab.com/app/emspresentations.plist");
        }

    }
}
