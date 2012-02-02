using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class ProdutoVaRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<ProdutoVa> GetProdutoVas()
        {
            var produtoVas = from e in db.ProdutoVas
                             where e.Status != (char)ProdutoVa.EnumStatus.Temporario
                            select e;

            return produtoVas;
        }

        /// <summary>
        /// retorna o VA temporario do usuario (quando está criando ou alterando)
        /// </summary>
        public ProdutoVa GetProdutoVaTemporario(int idUsuario, int idProduto)
        {
            var produtoVa = from e in db.ProdutoVas
                             where e.Status == (char)ProdutoVa.EnumStatus.Temporario
                                && e.Produto.Id == idProduto
                                && e.IdUsuario == idUsuario
                             select e;

            return produtoVa.FirstOrDefault();
        }

        public ProdutoVa GetProdutoVa(int id)
        {
            var produtoVas = from e in db.ProdutoVas
                         select e;

            return produtoVas.SingleOrDefault(e => e.Id == id);
        }

        public void Add(ProdutoVa produtoVa)
        {
            produtoVa.DataInclusao = DateTime.Now;

            db.ProdutoVas.InsertOnSubmit(produtoVa);
        }

        public void Delete(ProdutoVa produtoVa)
        {
            //deleta arquivos anexos do VA
            db.ProdutoVaArquivos.DeleteAllOnSubmit(produtoVa.ProdutoVaArquivos);

            //deleta slides do VA
            foreach (var slide in produtoVa.ProdutoVaSlides)
            {
                db.ProdutoVaSlideArquivos.DeleteAllOnSubmit(slide.ProdutoVaSlideArquivos);
            }
            db.ProdutoVaSlides.DeleteAllOnSubmit(produtoVa.ProdutoVaSlides);

            //deleta o VA
            db.ProdutoVas.DeleteOnSubmit(produtoVa);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
