using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class GerenciadorArquivoRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<GerenciadorArquivo> GetGerenciadorArquivos()
        {
            var gerenciadorArquivos = from e in db.GerenciadorArquivos
                         select e;

            return gerenciadorArquivos;
        }

        public GerenciadorArquivo GetGerenciadorArquivo(int id)
        {
            var gerenciadorArquivos = from e in db.GerenciadorArquivos
                         select e;

            return gerenciadorArquivos.SingleOrDefault(e => e.Id == id);
        }

        public void Add(GerenciadorArquivo gerenciadorArquivo)
        {
            gerenciadorArquivo.DataInclusao = DateTime.Now;

            db.GerenciadorArquivos.InsertOnSubmit(gerenciadorArquivo);
        }

        public void Delete(GerenciadorArquivo gerenciadorArquivo)
        {
            db.GerenciadorArquivos.DeleteOnSubmit(gerenciadorArquivo);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
