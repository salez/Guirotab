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
            [Description(".rtf")]
            Rtf = 'R',
            [Description(".jpg")]
            Jpg = 'J',
            [Description(".doc")]
            Doc = 'D',
            [Description(".docx")]
            Docx = 'E',
            [Description(".xls")]
            Xls = 'X',
            [Description(".xlsx")]
            Xlsx = 'L',
            [Description(".txt")]
            Txt = 'T',
            [Description(".csv")]
            Csv = 'V',
            [Description(".mp4")]
            Mp4 = 'M'
        }

        public string GetNome()
        {
            return this.Id + ((ProdutoVaArquivo.EnumTipo)this.Tipo).GetDescription();
        }

        public string GetCaminho()
        {
            return this.ProdutoVa.GetDiretorioArquivos() + this.GetNome();
        }

        public string GetCaminhoFisico()
        {
            return Util.Url.GetCaminhoFisico(this.GetCaminho());
        }

        public void DeletaArquivoFisico()
        {
            File.Delete(GetCaminhoFisico());
        }
    }
}