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
    public partial class RelatorioEmail
    {
        public enum EnumStatus
        {
            Pendente = 'P',
            Enviado = 'E',
            Erro = 'R'
        }
    }
}