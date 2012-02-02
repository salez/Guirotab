using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class ProdutoVaArquivoRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<ProdutoVaArquivo> GetProdutoVaArquivos()
        {
            var produtoVaArquivos = from e in db.ProdutoVaArquivos
                         select e;

            return produtoVaArquivos;
        }

        public ProdutoVaArquivo GetProdutoVaArquivo(int id)
        {
            var produtoVaArquivos = from e in db.ProdutoVaArquivos
                         select e;

            return produtoVaArquivos.SingleOrDefault(e => e.Id == id);
        }

        public void Add(ProdutoVaArquivo produtoVaArquivo)
        {
            produtoVaArquivo.DataInclusao = DateTime.Now;

            db.ProdutoVaArquivos.InsertOnSubmit(produtoVaArquivo);
        }

        public void Delete(ProdutoVaArquivo produtoVaArquivo)
        {
            db.ProdutoVaArquivos.DeleteOnSubmit(produtoVaArquivo);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
