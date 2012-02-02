using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models;
using System.ComponentModel;

namespace Models
{
    public partial class ProdutoVaSlideArquivo
    {
        public enum EnumTipoArquivo
        {
            [Description(".pdf")]
            Pdf = 'P',
            [Description(".mp4")]
            Mp4 = 'M',
            [Description(".jpg")]
            Jpg = 'J',
            [Description(".jpg")]
            Farmacia = 'F',
            [Description(".zip")]
            Zip = 'Z'
        }

        /// <summary>
        /// Thumb é apenas para arquivos JPG
        /// </summary>
        public enum EnumTamanho
        {
            Normal,
            Thumb
        }

        public string GetCaminhoArquivo()
        {
            return GetCaminhoArquivo(EnumTamanho.Normal);
        }

        public string GetCaminhoArquivo(EnumTamanho tamanho)
        {
            return this.ProdutoVaSlide.ProdutoVa.GetDiretorio() + GetNomeArquivo(tamanho);
        }

        public string GetCaminhoArquivo(EnumTamanho tamanho, EnumTipoArquivo tipoArquivo)
        {
            return this.ProdutoVaSlide.ProdutoVa.GetDiretorio() + GetNomeArquivo(tamanho, tipoArquivo);
        }

        public string GetCaminhoArquivoFisico()
        {
            return Util.Url.GetCaminhoFisico(GetCaminhoArquivo());
        }

        public string GetCaminhoArquivoFisico(EnumTamanho tamanho)
        {
            return Util.Url.GetCaminhoFisico(GetCaminhoArquivo(tamanho));
        }

        public string GetCaminhoArquivoFisico(EnumTamanho tamanho, EnumTipoArquivo tipoArquivo)
        {
            return Util.Url.GetCaminhoFisico(GetCaminhoArquivo(tamanho, tipoArquivo));
        }

        public string GetNomeArquivo()
        {
            return GetNomeArquivo(EnumTamanho.Normal);
        }

        public string GetNomeArquivo(EnumTamanho tamanho)
        {
            return GetNomeArquivo(tamanho, ((ProdutoVaSlideArquivo.EnumTipoArquivo)this.Tipo));
        }

        public string GetNomeArquivo(EnumTamanho tamanho, EnumTipoArquivo tipoArquivo)
        {
            var sufixo = "";
            switch (tamanho)
            {
                case EnumTamanho.Normal:
                    sufixo = "";
                    break;
                case EnumTamanho.Thumb:
                    sufixo = "_pq";
                    break;
            }

            if ((ProdutoVaSlideArquivo.EnumTipoArquivo)this.Tipo == EnumTipoArquivo.Zip)
            {
                return this.Id + "/" + this.Id + sufixo + ".jpg";
            }

            return this.Id + sufixo + tipoArquivo.GetDescription();
        }

    }
}