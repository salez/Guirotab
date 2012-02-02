using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models;
using System.ComponentModel;

namespace Models
{
    public partial class ProdutoVaSlide
    {
        public ProdutoVaSlideArquivo.EnumTipoArquivo? Tipo
        {
            get
            {
                var arquivo = this.ProdutoVaSlideArquivos.FirstOrDefault();

                if (arquivo != null)
                {
                    return (ProdutoVaSlideArquivo.EnumTipoArquivo)arquivo.Tipo;
                }

                return null;
            }
        }

        public bool IsFarmacia()
        {
            return (this.Tipo == ProdutoVaSlideArquivo.EnumTipoArquivo.Farmacia);
        }

        public void DeletaArquivosFisicos(){

            //deleta todos arquivos físicos do slide
            foreach (var arquivoSlide in this.ProdutoVaSlideArquivos)
            {
                //pega direotorio do VA
                var diretorio = this.ProdutoVa.GetDiretorioFisico();

                //deleta os arquivos iniciados pelo id do slide (12.jpg e 12_pq.jpg, por exemplo) que representam os arquivos fisicos do slide
                var arquivos = System.IO.Directory.GetFiles(diretorio, arquivoSlide.Id + "*");
                
                foreach (var arquivo in arquivos)
                {
                    System.IO.File.Delete(arquivo);
                }

                Util.Arquivo.DeleteDirectoryIfExists(diretorio + arquivoSlide.Id, true);
            }

        }

        public void DeletaEspecialidades(ProdutoVaSlideRepository slideRepository)
        {
            slideRepository.DeleteAllEspecialidades(this);
        }

        public void AddEspecialidades(ProdutoVaSlideRepository slideRepository, int[] IdsEspecialidades)
        {
            slideRepository.AddEspecialidades(this, IdsEspecialidades);
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

            return GetRelatorio(dInicial, dFinal,null);
        }

        public Relatorio GetRelatorio(DateTime? dataInicial, DateTime? dataFinal)
        {
            return GetRelatorio(dataInicial, dataFinal, null);
        }

        public Relatorio GetRelatorio(DateTime? dataInicial, DateTime? dataFinal, int? idDoutor)
        {
            var relatorio = new Relatorio();

            var relatorioPaginas = this.RelatorioPaginas.Select(rp => rp).Where(rp => rp.DoutorCadastro != null);

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