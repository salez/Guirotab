using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class EspecialidadeRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Especialidade> GetEspecialidades()
        {
            var especialidades = from e in db.Especialidades
                         select e;

            return especialidades;
        }

        public Especialidade GetEspecialidade(int id)
        {
            var especialidades = from e in db.Especialidades
                         select e;

            return especialidades.SingleOrDefault(e => e.Id == id);
        }

        public Especialidade GetEspecialidade(string nomeEspecialidade)
        {
            var especialidades = from e in db.Especialidades
                                 where e.Nome == nomeEspecialidade
                                 select e;

            return especialidades.SingleOrDefault();
        }

        public void Add(Especialidade especialidade)
        {
            especialidade.DataInclusao = DateTime.Now;

            db.Especialidades.InsertOnSubmit(especialidade);
        }

        public void Delete(Especialidade especialidade)
        {
            db.Especialidades.DeleteOnSubmit(especialidade);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
