using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models;
using System.ComponentModel;
using System.IO;

namespace Models
{
    public partial class Territorio
    {
        public enum EnumStatus
        {
            Ativo = 'A',
            Inativo = 'I'
        }

        public enum EnumTipo
        {
            Representante = 'R',
            Teste = 'T'
        }

        public IQueryable<string> GetEspecialidades()
        {
            DoutorProdutoRepository dpRepository = new DoutorProdutoRepository();

            var especialidades = (from dp in dpRepository.GetDoutorProdutos()
                                   where
                                        dp.Doutor.ProdutoLinha.Id == this.ProdutoLinha.Id
                                   select dp.Doutor.DoutorEspecialidades.First().Especialidade.Nome).Distinct();

            return especialidades;
        }

        public TerritorioProdutoVaDownload GetUltimoDownload(int idProduto)
        {
            TerritorioRepository tRepository = new TerritorioRepository();

            var downloads = from d in tRepository.GetTerritoriosDownload()
                           where
                               d.IdTerritorio == this.Id
                               && d.ProdutoVa.Produto.Id == idProduto
                           orderby d.Data descending
                           select d;

            return downloads.FirstOrDefault();
        }

        public IQueryable<Produto> GetProdutos()
        {
            DoutorProdutoRepository dpRepository = new DoutorProdutoRepository();

            var produtos = from dp in dpRepository.GetDoutorProdutos()
                           where
                                dp.Doutor.ProdutoLinha.Id == this.ProdutoLinha.Id
                           orderby dp.Orderm
                           select dp.Produto;

            return produtos;
        }

        public IQueryable<Produto> GetProdutosComVaAtivo()
        {
            DoutorProdutoRepository dpRepository = new DoutorProdutoRepository();

            var produtos = from dp in dpRepository.GetDoutorProdutos()
                           where
                                dp.Doutor.ProdutoLinha.Id == this.ProdutoLinha.Id
                                && dp.Produto.ProdutoVas.Any(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo)
                           orderby dp.Orderm
                           select dp.Produto;

            return produtos;
        }

        public IQueryable<Produto> GetProdutos(string especialidade)
        {
            DoutorProdutoRepository dpRepository = new DoutorProdutoRepository();

            var produtos = from dp in dpRepository.GetDoutorProdutos()
                           where
                                dp.Doutor.ProdutoLinha.Id == this.ProdutoLinha.Id
                                && dp.Doutor.DoutorEspecialidades.First().Especialidade.Nome == especialidade
                           orderby dp.Orderm
                           select dp.Produto;

            return produtos;
        }

        public void AtualizaUltimaSincronizacao()
        {
            TerritorioRepository territorioRepository = new TerritorioRepository();

            var territorio = territorioRepository.GetTerritorio(this.Id);

            territorio.DataUltimaSincronizacao = DateTime.Now;
            
            territorioRepository.Save();

        }

    }
}