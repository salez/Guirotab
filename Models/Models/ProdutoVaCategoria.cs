using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models;
using System.ComponentModel;

namespace Models
{
    public partial class ProdutoVaCategoria
    {
        public enum EnumTipo
        {
            Apresentacao = 'A',
            Anexo = 'N'
        }

    }
}