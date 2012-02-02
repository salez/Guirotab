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
            [Description(".mov")]
            Mov = 'M',
            [Description(".jpg")]
            Jpg = 'J'
        }

        public string GetCaminhoArquivo()
        {
            return GetCaminhoArquivo(String.Empty);
        }

        public string GetCaminhoArquivo(string adicionarSufixo)
        {
            return this.ProdutoVaSlide.ProdutoVa.GetDiretorio() + this.Id + adicionarSufixo + ((ProdutoVaSlideArquivo.EnumTipoArquivo)this.Tipo).GetDescription();
        }
    }
}