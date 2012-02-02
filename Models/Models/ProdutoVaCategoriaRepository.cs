using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class ProdutoVaCategoriaRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<ProdutoVaCategoria> GetProdutoVaCategorias()
        {
            var produtoVaCategorias = from e in db.ProdutoVaCategorias
                         select e;

            return produtoVaCategorias;
        }

        public ProdutoVaCategoria GetProdutoVaCategoria(int id)
        {
            var produtoVaCategorias = from e in db.ProdutoVaCategorias
                         select e;

            return produtoVaCategorias.SingleOrDefault(e => e.Id == id);
        }

        public void Add(ProdutoVaCategoria produtoVaCategoria)
        {
            produtoVaCategoria.DataInclusao = DateTime.Now;

            db.ProdutoVaCategorias.InsertOnSubmit(produtoVaCategoria);
        }

        public void Delete(ProdutoVaCategoria produtoVaCategoria)
        {
            db.ProdutoVaCategorias.DeleteOnSubmit(produtoVaCategoria);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
