using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class ProdutoVaSlideRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<ProdutoVaSlide> GetProdutoVaSlides()
        {
            var produtoVaSlides = from e in db.ProdutoVaSlides
                         select e;

            return produtoVaSlides;
        }

        public ProdutoVaSlide GetProdutoVaSlide(int id)
        {
            var produtoVaSlides = from e in db.ProdutoVaSlides
                         select e;

            return produtoVaSlides.SingleOrDefault(e => e.Id == id);
        }

        public void Add(ProdutoVaSlide produtoVaSlide)
        {
            produtoVaSlide.DataInclusao = DateTime.Now;

            db.ProdutoVaSlides.InsertOnSubmit(produtoVaSlide);
        }

        public void Delete(ProdutoVaSlide produtoVaSlide)
        {
            db.ProdutoVaSlideArquivos.DeleteAllOnSubmit(produtoVaSlide.ProdutoVaSlideArquivos);
            db.ProdutoVaSlides.DeleteOnSubmit(produtoVaSlide);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
