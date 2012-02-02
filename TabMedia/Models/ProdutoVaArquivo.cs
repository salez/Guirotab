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
    public partial class ProdutoVaArquivo
    {
        public enum EnumTipo
        {
            [Description(".pdf")]
            Pdf = 'P',
        }

        public string GetCaminho()
        {
            return this.ProdutoVa.GetDiretorioArquivos() + this.Id + ProdutoVaArquivo.EnumTipo.Pdf.GetDescription();
        }

        public string GetCaminhoFisico()
        {
            return Util.Url.GetCaminhoFisico(this.ProdutoVa.GetDiretorioArquivos() + this.Id + ProdutoVaArquivo.EnumTipo.Pdf.GetDescription());
        }

        public void DeletaArquivoFisico()
        {
            File.Delete(GetCaminhoFisico());
        }
    }
}