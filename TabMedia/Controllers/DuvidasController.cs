using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace GuiropaIpad.Controllers
{
    [AuthorizeLogin]
    public class DuvidasController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Agencias, Log.EnumArea.Site);
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        ProdutoRepository produtoRepository = new ProdutoRepository();

        public ActionResult Index()
        {
            return View();
        }

    }
}
