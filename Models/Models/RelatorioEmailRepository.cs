using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class RelatorioEmailRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<RelatorioEmail> GetRelatorioEmails()
        {
            var relatorioEmails = from e in db.RelatorioEmails
                         select e;

            return relatorioEmails;
        }

        public RelatorioEmail GetRelatorioEmail(int id)
        {
            var relatorioEmails = from e in db.RelatorioEmails
                         select e;

            return relatorioEmails.SingleOrDefault(e => e.Id == id);
        }

        public void Add(RelatorioEmail relatorioEmail)
        {
            if(relatorioEmail.DataInclusao == null)
                relatorioEmail.DataInclusao = DateTime.Now;

            db.RelatorioEmails.InsertOnSubmit(relatorioEmail);
        }

        public void Delete(RelatorioEmail relatorioEmail)
        {
            db.RelatorioEmails.DeleteOnSubmit(relatorioEmail);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
