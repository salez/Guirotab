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
    public partial class DoutorCadastro
    {
        //public void AddEspecialidade(Especialidade especialidade)
        //{
        //    var doutorEspecialidadeRepository = new DoutorEspecialidadeRepository();

        //    DoutorEspecialidade doutorEspecialidade = new DoutorEspecialidade();

        //    doutorEspecialidade.IdDoutor = this.Id;
        //    doutorEspecialidade.IdEspecialidade = especialidade.Id;

        //    doutorEspecialidadeRepository.Add(doutorEspecialidade);
        //    doutorEspecialidadeRepository.Save();

        //}

        //public void AddProduto(Produto produto, int ordem)
        //{
        //    var dpRepository = new DoutorProdutoRepository();

        //    DoutorProduto dp = new DoutorProduto();

        //    dp.IdDoutor = this.Id;
        //    dp.IdProduto = produto.Id;
        //    dp.Orderm = ordem;

        //    dpRepository.Add(dp);
        //    dpRepository.Save();

        //}

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

            return GetRelatorio(dInicial, dFinal);
        }

        public Relatorio GetRelatorio(DateTime? dataInicial, DateTime? dataFinal)
        {
            var relatorio = new Relatorio();

            IEnumerable<RelatorioPagina> relatorioPaginas = this.RelatorioPaginas;

            if (dataInicial != null)
            {
                relatorioPaginas = relatorioPaginas.Where(rp => rp.DataInicio >= dataInicial);
            }

            if (dataFinal != null)
            {
                relatorioPaginas = relatorioPaginas.Where(rp => rp.DataInicio < dataFinal.Value.AddDays(1));
            }

            relatorio.QtdeVisualizacoes = relatorioPaginas.Count();
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
        //}
    }
}