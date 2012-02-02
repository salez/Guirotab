using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class DoutorRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Doutor> GetDoutors()
        {
            var doutors = from e in db.Doutors
                         select e;

            return doutors;
        }

        public Doutor GetDoutor(int id)
        {
            var doutors = from e in db.Doutors
                         select e;

            return doutors.SingleOrDefault(e => e.Id == id);
        }

        public void Add(Doutor doutor)
        {
            doutor.DataInclusao = DateTime.Now;

            db.Doutors.InsertOnSubmit(doutor);
        }

        public void Delete(Doutor doutor)
        {
            db.Doutors.DeleteOnSubmit(doutor);
        }

        public void DeleteDoutoresSemProdutosRelacionados()
        {
            var doutores = db.Doutors.Where(d => d.DoutorProdutos.Count() == 0);

            doutores.Each(d =>
            {
                db.DoutorEspecialidades.DeleteAllOnSubmit(d.DoutorEspecialidades);
            });

            db.Doutors.DeleteAllOnSubmit(doutores);
        }

        public void DeleteAllDoutores()
        {
            db.Doutors.Each(d =>
            {
                db.DoutorProdutos.DeleteAllOnSubmit(d.DoutorProdutos);
                db.DoutorEspecialidades.DeleteAllOnSubmit(d.DoutorEspecialidades);
            });

            db.Doutors.DeleteAllOnSubmit(db.Doutors);
        }

        public void DeleteProdutos(Doutor d)
        {
            db.DoutorProdutos.DeleteAllOnSubmit(d.DoutorProdutos);
        }

        public void DeleteProdutosEespecialidades(Doutor d)
        {
            db.DoutorProdutos.DeleteAllOnSubmit(d.DoutorProdutos);
            db.DoutorEspecialidades.DeleteAllOnSubmit(d.DoutorEspecialidades);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
