using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class ControladorRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Controlador> GetControladores()
        {
            IQueryable<Controlador> controladores = from controlador in db.Controladors
                                                    select controlador;

            return controladores;
        }

        public Controlador GetControlador(int id)
        {
            return db.Controladors.SingleOrDefault(controlador => controlador.Id == id);
        }

        public void Add(Controlador controlador)
        {
            db.Controladors.InsertOnSubmit(controlador);
        }

        public void Delete(Controlador controlador)
        {
            db.Acaos.DeleteAllOnSubmit(controlador.Acaos);
            db.Controladors.DeleteOnSubmit(controlador);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
