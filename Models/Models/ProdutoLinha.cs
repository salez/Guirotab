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
    public partial class ProdutoLinha
    {
        public enum EnumStatus
        {
            Ativo = 'A',
            Inativo = 'I',
            Teste = 'T'
        }

        public IQueryable<Produto> GetProdutos()
        {
            return new ProdutoRepository().GetProdutos().Where(p => p.ProdutoLinha.Id == this.Id);
        }

        /// <summary>
        /// retorna o territorio teste da respectiva linha
        /// </summary>
        /// <returns></returns>
        public Territorio GetTerritorioTeste()
        {
            return this.Territorios.Where(t => t.Tipo == (char)Territorio.EnumTipo.Teste).FirstOrDefault();
        }

        public Relatorio GetRelatorio(string dataInicial, string dataFinal)
        {
            DateTime? dInicial = null;
            DateTime? dFinal = null;

            DateTime aux;

            if (DateTime.TryParse(dataInicial, out aux))
            {
                dInicial = aux;
            }
            if (DateTime.TryParse(dataFinal, out aux))
            {
                dFinal = aux;
            }

            return GetRelatorio(dInicial, dFinal, null);
        }

        public Relatorio GetRelatorio(DateTime? dataInicial, DateTime? dataFinal)
        {
            return GetRelatorio(dataInicial, dataFinal, null);
        }

        public Relatorio GetRelatorio(DateTime? dataInicial, DateTime? dataFinal, int? idDoutor)
        {

            var relatorio = new Relatorio();

            var relatorioPaginas = this.Produtos.SelectMany(p => p.ProdutoVas).SelectMany(va => va.ProdutoVaSlides).SelectMany(s => s.RelatorioPaginas).Where(rp => rp.DoutorCadastro != null);

            if (dataInicial != null)
            {
                relatorioPaginas = relatorioPaginas.Where(rp => rp.DataInicio >= dataInicial);
            }

            if (dataFinal != null)
            {
                relatorioPaginas = relatorioPaginas.Where(rp => rp.DataInicio < dataFinal.Value.AddDays(1));
            }

            if (idDoutor != null)
            {
                relatorioPaginas = relatorioPaginas.Where(rp => rp.IdDoutorCadastro == idDoutor);
            }

            relatorio.QtdeVisualizacoes = relatorioPaginas.Count();
            relatorio.Segundos = relatorioPaginas.Sum(r => r.Segundos);

            //tira a mediana
            var metade1 = relatorioPaginas.OrderBy(rp => rp.Segundos).Take(relatorioPaginas.Count() / 2).Max(rp => rp.Segundos);
            var metade2 = relatorioPaginas.OrderByDescending(rp => rp.Segundos).Take(relatorioPaginas.Count() / 2).Min(rp => rp.Segundos);
            relatorio.Mediana = (metade1 + metade2) / 2;

            return relatorio;
        }

        public RelatorioUtilizacaoTerritorios GetRelatorioUtilizacao()
        {
            var relatorio = new RelatorioUtilizacaoTerritorios();

            relatorio.instalacao_ok = this.Territorios.Where(t => t.DataUltimaSincronizacao != null).Count();
            relatorio.instalacao_faltam = this.Territorios.Where(t => t.DataUltimaSincronizacao == null).Count();
            relatorio.instalacao_porcentagem = (this.Territorios.Count() > 0) ? (relatorio.instalacao_ok / this.Territorios.Count()) * 100 : 0;

            relatorio.atualizacao_ok = 0; // this.Territorios.Where(t => t.TerritorioProdutoVaDownloads.Select(d => d.ProdutoVa.Produto).Distinct().Any(p => p.ProdutoVas)); //t.GetUltimoDownload(p.Id).ProdutoVa.Id)).Count(); //TerritorioProdutoVaDownloads.Any(vd => vd.ProdutoVa.Produto.GetVAAtivo() != null && vd.ProdutoVa.Id == vd.ProdutoVa.Produto.GetVAAtivo().Id)).Count();
            relatorio.atualizacao_faltam = this.Territorios.Count() - relatorio.atualizacao_ok;
            relatorio.atualizacao_porcentagem = (this.Territorios.Count() > 0) ? (relatorio.atualizacao_ok / this.Territorios.Count()) * 100 : 0;
            relatorio.sincronizacao_hoje = this.Territorios.Where(t => t.DataUltimaSincronizacao < Util.Data.Hoje.AddDays(1) && t.DataUltimaSincronizacao >= Util.Data.Hoje).Count();
            relatorio.sincronizacao_ontem = this.Territorios.Where(t => t.DataUltimaSincronizacao < Util.Data.Hoje && t.DataUltimaSincronizacao >= Util.Data.Hoje.AddDays(-1)).Count();
            relatorio.sincronizacao_ate3dias = this.Territorios.Where(t => t.DataUltimaSincronizacao < Util.Data.Hoje.AddDays(1) && t.DataUltimaSincronizacao >= Util.Data.Hoje.AddDays(-3)).Count();
            relatorio.sincronizacao_mais3dias = this.Territorios.Where(t => t.DataUltimaSincronizacao < Util.Data.Hoje.AddDays(-3)).Count();

            return relatorio;
        }
    }

    public class RelatorioUtilizacaoTerritorios
    {
        public double instalacao_ok;
        public double instalacao_faltam;
        public double instalacao_porcentagem;
        public double atualizacao_ok;
        public double atualizacao_faltam;
        public double atualizacao_porcentagem;
        public double sincronizacao_hoje;
        public double sincronizacao_ontem;
        public double sincronizacao_ate3dias;
        public double sincronizacao_mais3dias;


    }
}