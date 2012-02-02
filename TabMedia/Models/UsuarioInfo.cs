using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    [Serializable]
    public class UsuarioInfo
    {
        public int Id { get; set; }
        public String Nome { get; set; }
        public String Email { get; set; }
        public String Login { get; set; }
        public char? Status { get; set; }
        public string IdGrupo { get; set; }
    }
}
