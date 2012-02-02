using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Controllers
{
    [AuthorizePermissao]
    public class EmailsController : BaseController
    {
        EmailRepository emailRepository = new EmailRepository();
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Emails, Log.EnumArea.Admin);

        public ActionResult Index()
        {
            var emails = emailRepository.GetEmails().OrderByDescending(e => e.DataInclusao).Take(500);

            logs.Add(Log.EnumTipo.Consulta, "Consultou os Emails Enviados", String.Empty);

            return View(emails);
        }

        //FILTROS
        [HttpPost]
        public ActionResult Index(Email email, String Destinatario, String DataDe, String DataAte)
        {
            var emails = emailRepository.GetEmails().OrderByDescending(e => e.DataInclusao).Take(500);

            if (email.De != null)
                emails = emails.Where(e => e.De.Contains(email.De) || e.DeEmail.Contains(email.De));

            if (Destinatario != null && Destinatario != String.Empty)
                emails = emails.Where(e => e.EmailDestinatarios.Any(d => d.Email.Contains(Destinatario) || d.Nome.Contains(Destinatario)));

            if (email.Assunto != null)
            {
                emails = emails.Where(e => e.Assunto.Contains(email.Assunto));
            }

            if (DataDe != null && DataDe.IsDate())
                emails = emails.Where(e => e.DataInclusao > Convert.ToDateTime(DataDe));

            if (DataAte != null && DataAte.IsDate())
                emails = emails.Where(e => e.DataInclusao < Convert.ToDateTime(DataAte).AddDays(1));

            return View(emails);
        }

        public ActionResult ExibeMail(int id)
        {
            if (Request.IsAjaxRequest())
            {
                var email = emailRepository.GetEmail(id);

                if (email != null)
                {
                    return View(email);
                }
            }

            return new EmptyResult();
        }
    }
}
