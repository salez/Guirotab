using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Controllers
{
    [AuthorizePermissao]
    public class LogsController : BaseController
    {
        LogRepository logRepository = new LogRepository();
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Logs, Log.EnumArea.Admin);
        //
        // GET: /Admin/Logs/

        public ActionResult Index()
        {
            IQueryable<Log> logs = logRepository.GetLogs().OrderByDescending(l => l.DataInclusao).Take(500);

            this.logs.Add(Log.EnumTipo.Consulta, "Consultou os logs", Url.Action("index"));
            return View(logs);
        }

        [HttpPost]
        public ActionResult Index(Log log, String DataDe, String DataAte)
        {
            IQueryable<Log> logs = logRepository.GetLogs().OrderByDescending(l => l.DataInclusao).Take(500);

            if (log.Usuario.Nome != null)
                logs = logs.Where(l => l.Usuario.Nome.Contains(log.Usuario.Nome));

            if (log.Pagina != null)
                logs = logs.Where(l => l.Pagina == log.Pagina);

            if (log.TipoAcao != null)
                logs = logs.Where(l => l.TipoAcao == log.TipoAcao);

            if (DataDe != null && DataDe.IsDate())
                logs = logs.Where(l => l.DataInclusao > Convert.ToDateTime(DataDe));

            if (DataAte != null && DataAte.IsDate())
                logs = logs.Where(l => l.DataInclusao < Convert.ToDateTime(DataAte).AddDays(1));


            return View(logs);
        }

    }
}
