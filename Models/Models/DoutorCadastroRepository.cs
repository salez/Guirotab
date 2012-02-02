using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class DoutorCadastroRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<DoutorCadastro> GetDoutorCadastros()
        {
            var doutorCadastros = from e in db.DoutorCadastros
                         select e;

            return doutorCadastros;
        }

        public DoutorCadastro GetDoutorCadastro(int id)
        {
            var doutorCadastros = from e in db.DoutorCadastros
                         select e;

            return doutorCadastros.SingleOrDefault(e => e.Id == id);
        }

        public void Add(DoutorCadastro doutorCadastro)
        {
            doutorCadastro.DataInclusao = DateTime.Now;

            db.DoutorCadastros.InsertOnSubmit(doutorCadastro);
        }

        public void Delete(DoutorCadastro doutorCadastro)
        {
            db.DoutorCadastros.DeleteOnSubmit(doutorCadastro);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
