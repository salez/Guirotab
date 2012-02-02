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

        private IQueryable<Produto> TabelaProdutos()
        {
            IQueryable<Produto> produtos = null;

            string grupo = Sessao.Site.RetornaUsuario().Grupo.Id;

            if (grupo == Usuario.EnumTipo.Desenvolvedor.GetDescription() || grupo == Usuario.EnumTipo.Administrador.GetDescription())
            {
                produtos = from p in db.Produtos
                           select p;
            }
            else { 
                produtos = from p in db.Produtos
                           where p.UsuarioProdutos.Any(up => up.IdUsuario == Sessao.Site.UsuarioInfo.Id)
                           select p;
            }

            return produtos;
        }

        public IQueryable<Produto> GetProdutos()
        {
            var produtos = from e in TabelaProdutos()
                         select e;

            return produtos;
        }

        public IQueryable<Produto> GetProdutosSemVerificacaoUsuario()
        {
            var produtos = from e in db.Produtos
                           select e;

            return produtos;
        }

        public Produto GetProduto(int id)
        {
            var produtos = from e in TabelaProdutos()
                         select e;

            return produtos.SingleOrDefault(e => e.Id == id);
        }

        public Produto GetProduto(string nome)
        {
            var produtos = from e in TabelaProdutos()
                           where e.Nome == nome
                           select e;

            return produtos.FirstOrDefault();
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
