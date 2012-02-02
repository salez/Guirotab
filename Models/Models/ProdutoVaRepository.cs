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

        private IQueryable<ProdutoVa> tabelaProdutoVas(){

            IQueryable<ProdutoVa> produtoVas = null;

            if (Sessao.Site.UsuarioInfo.IsAdministrador())
            {
                produtoVas = from vas in db.ProdutoVas
                             select vas;
            }
            else
            {
                produtoVas = from vas in db.ProdutoVas
                                 where vas.Produto.UsuarioProdutos.Any(up => up.IdUsuario == Sessao.Site.UsuarioInfo.Id)
                             select vas;
            }

            return produtoVas;

        }

        public IQueryable<ProdutoVa> GetProdutoVas()
        {
            var produtoVas = from e in tabelaProdutoVas()
                             where 
                                e.Status != (char)ProdutoVa.EnumStatus.Temporario
                            select e;

            return produtoVas;
        }

        /// <summary>
        /// retorna o VA temporario do usuario (quando está criando ou alterando)
        /// </summary>
        public ProdutoVa GetProdutoVaTemporario(int idUsuario, int idProduto, int idCategoria)
        {
            var produtoVa = from e in tabelaProdutoVas()
                             where e.Status == (char)ProdutoVa.EnumStatus.Temporario
                                && e.Produto.Id == idProduto
                                && e.IdUsuario == idUsuario
                                && e.IdCategoria == idCategoria
                             select e;

            return produtoVa.FirstOrDefault();
        }

        public ProdutoVa GetProdutoVa(int id)
        {
            var produtoVas = from e in tabelaProdutoVas()
                         select e;

            return produtoVas.SingleOrDefault(e => e.Id == id);
        }

        public ProdutoVa GetProdutoVaSemVerificacaoUsuario(int id)
        {
            //não faz a verificação de gerente e agencia pois a verificação é feita no metodo de download e é uma verificação sobre o territorio, não utilizar este método sem verificação do territorio
            var produtoVas = from vas in db.ProdutoVas
                             select vas;

            return produtoVas.SingleOrDefault(e => e.Id == id);
        }

        ///// <summary>
        ///// retorna um inteiro com uma versão acima do ultimo va publicado
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public int GetNovaVersao(int idProduto)
        //{
        //    var versao = (from e in tabelaProdutoVas()
        //                    where e.IdProduto == idProduto
        //                    select e.Versao).Max() + 1;

        //    if (versao == null)
        //        return 1;

        //    return versao.Value;
        //}

        public void AddDownload(int idVa, string idTerritorio)
        {
            TerritorioProdutoVaDownload produtoVaDownload = new TerritorioProdutoVaDownload();

            produtoVaDownload.IdVa = idVa;
            produtoVaDownload.IdTerritorio = idTerritorio;
            produtoVaDownload.Data = DateTime.Now;

            db.TerritorioProdutoVaDownloads.InsertOnSubmit(produtoVaDownload);
        }

        public void AtualizaVersaoDoutoresRelacionados(ProdutoVa va)
        {
            var doutoresRelacionados = from d in db.Doutors
                                       where d.DoutorProdutos.Any(dp => dp.Produto.Id == va.Produto.Id)
                                       select d;

            foreach (var d in doutoresRelacionados) {

                if (d.Versao == null)
                {
                    d.Versao = 1;
                }
                else
                {
                    d.Versao++;
                }

            }
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
                db.ProdutoVaSlideEspecialidades.DeleteAllOnSubmit(slide.ProdutoVaSlideEspecialidades);
            }
            db.ProdutoVaSlides.DeleteAllOnSubmit(produtoVa.ProdutoVaSlides);
            db.ProdutoVaComentarios.DeleteAllOnSubmit(produtoVa.ProdutoVaComentarios);
            db.TerritorioProdutoVaDownloads.DeleteAllOnSubmit(produtoVa.TerritorioProdutoVaDownloads);
            

            //deleta o VA
            db.ProdutoVas.DeleteOnSubmit(produtoVa);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
