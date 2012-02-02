using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class ProdutoVaSlideArquivoRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<ProdutoVaSlideArquivo> GetProdutoVaSlideArquivos()
        {
            var produtoVaSlideArquivos = from e in db.ProdutoVaSlideArquivos
                         select e;

            return produtoVaSlideArquivos;
        }

        public ProdutoVaSlideArquivo GetProdutoVaSlideArquivo(int id)
        {
            var produtoVaSlideArquivos = from e in db.ProdutoVaSlideArquivos
                         select e;

            return produtoVaSlideArquivos.SingleOrDefault(e => e.Id == id);
        }

        public void Add(ProdutoVaSlideArquivo produtoVaSlideArquivo)
        {
            produtoVaSlideArquivo.DataInclusao = DateTime.Now;

            db.ProdutoVaSlideArquivos.InsertOnSubmit(produtoVaSlideArquivo);
        }

        public void Delete(ProdutoVaSlideArquivo produtoVaSlideArquivo)
        {
            db.ProdutoVaSlideArquivos.DeleteOnSubmit(produtoVaSlideArquivo);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
