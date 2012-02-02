using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    [MetadataType(typeof(Controlador_Validation))]
    public partial class Controlador
    {
    }

    public class Controlador_Validation
    {
        [StringLength(50, ErrorMessage = "Nome não pode ter mais do que 50 caracteres.")]
        public String Nome { get; set; }
    }
}
