using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class ProdutoVaComentarioRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<ProdutoVaComentario> GetProdutoVaComentarios()
        {
            var produtoVaComentarios = from e in db.ProdutoVaComentarios
                         select e;

            return produtoVaComentarios;
        }

        public ProdutoVaComentario GetProdutoVaComentario(int id)
        {
            var produtoVaComentarios = from e in db.ProdutoVaComentarios
                         select e;

            return produtoVaComentarios.SingleOrDefault(e => e.Id == id);
        }

        public void Add(ProdutoVaComentario produtoVaComentario)
        {
            produtoVaComentario.Datainclusao = DateTime.Now;

            db.ProdutoVaComentarios.InsertOnSubmit(produtoVaComentario);
        }

        public void Delete(ProdutoVaComentario produtoVaComentario)
        {
            db.ProdutoVaComentarios.DeleteOnSubmit(produtoVaComentario);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
