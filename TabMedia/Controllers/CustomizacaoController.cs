using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Controllers
{
    public class CustomizacaoController : BaseController
    {
        //
        // GET: /Customizacao/

        public ActionResult CssValidationEngine()
        {
            return View();
        }

        public ActionResult CssAdmin()
        {
            return View();
        }

    }
}
