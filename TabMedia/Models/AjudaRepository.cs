using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Models
{
    public class AjudaRepository
    {
        DBDataContext db = new DBDataContext();

        public Ajuda GetAjuda()
        {
            return db.Ajudas.FirstOrDefault();
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
