using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class ProdutoRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Produto> GetProdutos()
        {
            var produtos = from e in db.Produtos
                         select e;

            return produtos;
        }

        public Produto GetProduto(int id)
        {
            var produtos = from e in db.Produtos
                         select e;

            return produtos.SingleOrDefault(e => e.Id == id);
        }

        public void Add(Produto produto)
        {
            produto.DataInclusao = DateTime.Now;

            db.Produtos.InsertOnSubmit(produto);
        }

        public void Delete(Produto produto)
        {
            db.Produtos.DeleteOnSubmit(produto);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
