using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class RelatorioPaginaRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<RelatorioPagina> GetRelatorioPaginas()
        {
            var relatorioPaginas = from e in db.RelatorioPaginas
                         select e;

            return relatorioPaginas;
        }

        public RelatorioPagina GetRelatorioPagina(int id)
        {
            var relatorioPaginas = from e in db.RelatorioPaginas
                         select e;

            return relatorioPaginas.SingleOrDefault(e => e.Id == id);
        }

        public void Add(RelatorioPagina relatorioPagina)
        {
            if (relatorioPagina.DataInclusao == null) {
                relatorioPagina.DataInclusao = DateTime.Now;
            }

            db.RelatorioPaginas.InsertOnSubmit(relatorioPagina);
        }

        public void Delete(RelatorioPagina relatorioPagina)
        {
            db.RelatorioPaginas.DeleteOnSubmit(relatorioPagina);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
