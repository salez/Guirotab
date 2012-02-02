using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class GrupoRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Grupo> GetGrupos()
        {
            IQueryable<Grupo> grupos = from grupo in db.Grupos
                                       select grupo;

            return grupos;
        }

        public Grupo GetGrupo(string id)
        {
            return db.Grupos.SingleOrDefault(grupo => grupo.Id == id);
        }

        public void Add(Grupo grupo)
        {
            db.Grupos.InsertOnSubmit(grupo);
        }

        public void Delete(Grupo grupo)
        {
            if (grupo.Usuarios.Count() > 0)
            {
                throw new Exception("Há usuários cadastrados para este grupo.");
            }

            db.AcaoGrupos.DeleteAllOnSubmit(grupo.AcaoGrupos);
            db.Grupos.DeleteOnSubmit(grupo);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}