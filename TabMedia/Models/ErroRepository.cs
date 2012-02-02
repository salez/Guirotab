using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class ErroRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Erro> GetErros()
        {
            IQueryable<Erro> erros = from erro in db.Erros
                                     select erro;

            return erros;
        }

        public Erro GetErro(int id)
        {
            return db.Erros.SingleOrDefault(f => f.Id == id);
        }

        public void Add(Erro erro)
        {
            erro.DataInclusao = DateTime.Now;

            db.Erros.InsertOnSubmit(erro);
        }

        public void Delete(Erro erro)
        {
            db.Erros.DeleteOnSubmit(erro);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
