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
    public partial class Produto
    {
        public string GetDiretorio()
        {
            return Util.Sistema.AppSettings.Diretorios.DiretorioProdutos + this.Id + "/";
        }

        public string GetDiretorioFisico()
        {
            return Util.Url.GetCaminhoFisico(Util.Sistema.AppSettings.Diretorios.DiretorioProdutos + this.Id + "/");
        }

        public string GetDiretorioVAs()
        {
            return Util.Sistema.AppSettings.Diretorios.DiretorioProdutos + this.Id + "/vas/";
        }

        public string GetDiretorioVAsFisico()
        {
            return Util.Url.GetCaminhoFisico(Util.Sistema.AppSettings.Diretorios.DiretorioProdutos + this.Id + "/vas/");
        }

        public string GetCaminhoImagemThumb()
        {
            return Util.Sistema.AppSettings.Diretorios.DiretorioProdutosImages + this.Id + "_thumb.jpg";
        }

        public string GetCaminhoImagemThumbFisico()
        {
            return Util.Url.GetCaminhoFisico(GetCaminhoImagemThumb());
        }

        public string GetUrlImagemThumb()
        {
            return Util.Url.UrlHelper().Content(GetCaminhoImagemThumb());
        }

        public void CriaDiretoriosBase()
        {
            //cria pasta para o produto caso não exista
            if (!Directory.Exists(this.GetDiretorioFisico()))
            {
                Directory.CreateDirectory(this.GetDiretorioFisico());

                //cria pasta de VAS caso não exista
                if (!Directory.Exists(this.GetDiretorioVAsFisico()))
                {
                    Directory.CreateDirectory(this.GetDiretorioVAsFisico());
                }
            }
        }

        public bool TemVAAtivo()
        {
            return this.ProdutoVas.Any(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo && va.ProdutoVaCategoria.SomenteUmAtivo == true);
        }

        public ProdutoVa GetVAAtivo()
        {
            return this.ProdutoVas.Where(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo && va.ProdutoVaCategoria.SomenteUmAtivo == true).OrderByDescending(va => va.Versao).FirstOrDefault();
        }

        /// <summary>
        /// retorna os vas independente do tipo
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProdutoVa> GetVAsAtivos()
        {
            return this.ProdutoVas.Where(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo).OrderByDescending(va => va.Versao);
        }

        public ProdutoVa GetVATeste()
        {
            return this.ProdutoVas.Where(va => va.Status == (char)ProdutoVa.EnumStatus.Teste && va.ProdutoVaCategoria.SomenteUmAtivo == true).OrderByDescending(va => va.Versao).FirstOrDefault();
        }

        public IEnumerable<ProdutoVa> GetVAsTeste()
        {
            return this.ProdutoVas.Where(va => va.Status == (char)ProdutoVa.EnumStatus.Teste).OrderByDescending(va => va.Versao);
        }

        /// <summary>
        /// Retorna os VAs pendentes (pendentes de aprovação tanto do gerente de produto quanto de marketing)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProdutoVa> GetVAsPendentes()
        {
            return this.ProdutoVas.Where(va => (va.Status == (char)ProdutoVa.EnumStatus.Pendente || va.StatusGM == (char)ProdutoVa.EnumStatus.Pendente || va.Status == (char)ProdutoVa.EnumStatus.Aprovado) && va.Status != (char)ProdutoVa.EnumStatus.Reprovado && va.StatusGM != (char)ProdutoVa.EnumStatus.Reprovado);
        }

        public int GetNovaVersaoVa()
        {
            var versao = this.ProdutoVas.Select(va => va.Versao).Max() + 1;

            if (versao == null)
                return 1;

            return versao.Value;
        }

        public int GetNovaVersaoTesteVa()
        {
            var versao = this.ProdutoVas.Select(va => va.VersaoTeste).Max() + 1;

            if (versao == null)
                return 1;

            return versao.Value;
        }

        public Relatorio GetRelatorio()
        {
            DateTime? dataInicial = null;
            DateTime? dataFinal = null;
            return GetRelatorio(dataInicial, dataFinal);
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

            return GetRelatorio(dInicial,dFinal,null);
        }

        public Relatorio GetRelatorio(DateTime? dataInicial, DateTime? dataFinal)
        {
            return GetRelatorio(dataInicial, dataFinal, null);
        }

        public Relatorio GetRelatorio(DateTime? dataInicial, DateTime? dataFinal, int? idDoutor)
        {
            RelatorioPaginaRepository relatorioPaginaRepository = new RelatorioPaginaRepository();

            var relatorio = new Relatorio();

            var relatorioPaginas = relatorioPaginaRepository.GetRelatorioPaginas().Where(rp => rp.ProdutoVaSlide.ProdutoVa.Produto.Id == this.Id); //this.ProdutoVas.SelectMany(va => va.ProdutoVaSlides).SelectMany(s => s.RelatorioPaginas).Where(rp => rp.DoutorCadastro != null);

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

            //tira a mediana
            var metade1 = relatorioPaginas.OrderBy(rp => rp.Segundos).Take(relatorioPaginas.Count() / 2).Max(rp => rp.Segundos);
            var metade2 = relatorioPaginas.OrderByDescending(rp => rp.Segundos).Take(relatorioPaginas.Count() / 2).Min(rp => rp.Segundos);
            relatorio.Mediana = (metade1 + metade2) / 2;

            relatorio.Segundos = relatorioPaginas.Sum(r => r.Segundos);

            return relatorio;
        }

        //public class Relatorio
        //{
        //    public int QtdeVisualizacoes { get; set; }
        //    public double? Segundos { get; set; }

        //    public string GetTempoTotal()
        //    {
        //        if (this.Segundos == null)
        //            return "-";

        //        var segundos = this.Segundos.Value;

        //        return segundos.FromSecondsTo("[M]:[s]");
        //    }

        //    public string GetTempoMedio()
        //    {
        //        if (this.Segundos == null)
        //            return "-";

        //        var segundos = this.Segundos.Value / this.QtdeVisualizacoes;

        //        return segundos.FromSecondsTo("[M]:[s]");
        //    }
        //}

        public void EnviarEmailAprovado()
        {
            //var gerentes = this.UsuarioProdutos.Select(up => up.)
        }
    }
}