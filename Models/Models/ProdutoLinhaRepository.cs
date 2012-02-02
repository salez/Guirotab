using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class ProdutoLinhaRepository
    {
        DBDataContext db = new DBDataContext();

        private IQueryable<ProdutoLinha> TabelaLinhas()
        {
            IQueryable<ProdutoLinha> linhas = null;

            if (!Sessao.Site.UsuarioLogado())
            {
                //se acessado pelo ipad n ha usuario logado.

                linhas = from l in db.ProdutoLinhas
                         select l;

                return linhas;
            }

            string grupo = Sessao.Site.RetornaUsuario().Grupo.Id;

            if (grupo == Usuario.EnumTipo.Desenvolvedor.GetDescription())
            {
                linhas = from l in db.ProdutoLinhas
                           select l;
            }
            else if(grupo == Usuario.EnumTipo.Administrador.GetDescription()){

                linhas = from l in db.ProdutoLinhas
                         where l.Status == (char)ProdutoLinha.EnumStatus.Ativo
                         select l;

            }else
            {
                linhas = from l in db.ProdutoLinhas
                           where l.Produtos.Any(p => p.UsuarioProdutos.Any(up => up.IdUsuario == Sessao.Site.UsuarioInfo.Id))
                           select l;
            }

            return linhas;
        }

        public IQueryable<ProdutoLinha> GetProdutoLinhas()
        {
            var produtoLinhas = from e in this.TabelaLinhas()
                         select e;

            return produtoLinhas;
        }

        public IQueryable<ProdutoLinha> GetProdutoLinhasAtivas()
        {
            var produtoLinhas = from e in this.TabelaLinhas()
                                where e.Status == (char)ProdutoLinha.EnumStatus.Ativo
                                select e;

            return produtoLinhas;
        }

        public ProdutoLinha GetProdutoLinha(int id)
        {
            var produtoLinhas = from e in this.TabelaLinhas()
                         select e;

            return produtoLinhas.SingleOrDefault(e => e.Id == id);
        }

        public ProdutoLinha GetProdutoLinha(string nome)
        {
            var produtoLinhas = from e in this.TabelaLinhas()
                                where e.Nome == nome
                                select e;

            return produtoLinhas.SingleOrDefault();
        }

        public void Add(ProdutoLinha produtoLinha)
        {
            produtoLinha.DataInclusao = DateTime.Now;

            db.ProdutoLinhas.InsertOnSubmit(produtoLinha);
        }

        public void Delete(ProdutoLinha produtoLinha)
        {
            db.ProdutoLinhas.DeleteOnSubmit(produtoLinha);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
