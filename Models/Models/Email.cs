using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models;
using System.ComponentModel;

namespace Models
{
    public partial class Email
    {
        public enum EnumTipo
        {
            [Description("Destinatário")]
            Destinatario = 'D',
            [Description("Cópia")]
            Copia = 'C',
            [Description("Cópia Oculta")]
            CopiaOculta = 'O'
        }
    }
}