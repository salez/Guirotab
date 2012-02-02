using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class DoutorEspecialidadeRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<DoutorEspecialidade> GetDoutorEspecialidades()
        {
            var doutorEspecialidades = from e in db.DoutorEspecialidades
                         select e;

            return doutorEspecialidades;
        }

        public DoutorEspecialidade GetDoutorEspecialidade(int id)
        {
            var doutorEspecialidades = from e in db.DoutorEspecialidades
                         select e;

            return doutorEspecialidades.SingleOrDefault(e => e.Id == id);
        }

        public void Add(DoutorEspecialidade doutorEspecialidade)
        {
            db.DoutorEspecialidades.InsertOnSubmit(doutorEspecialidade);
        }

        public void Delete(DoutorEspecialidade doutorEspecialidade)
        {
            db.DoutorEspecialidades.DeleteOnSubmit(doutorEspecialidade);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
