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

        public int? IdOriginal { get; set; }
        public String NomeOriginal { get; set; }
        public String EmailOriginal { get; set; }
        public String LoginOriginal { get; set; }
        public char? StatusOriginal { get; set; }
        public string IdGrupoOriginal { get; set; }

        public bool IsGerenteProduto()
        {
            return (IdGrupo == Usuario.EnumTipo.GerenteProduto.GetDescription());
        }

        public bool IsGerenteMarketing()
        {
            return (IdGrupo == Usuario.EnumTipo.GerenteMarketing.GetDescription());
        }

        public bool IsGerenteNacional()
        {
            return (IdGrupo == Usuario.EnumTipo.GerenteNacional.GetDescription());
        }

        public bool IsGerenteRegional()
        {
            return (IdGrupo == Usuario.EnumTipo.GerenteRegional.GetDescription());
        }

        public bool IsGerenteDistrital()
        {
            return (IdGrupo == Usuario.EnumTipo.GerenteDistrital.GetDescription());
        }

        public bool IsGerente()
        {
            return (IsGerenteProduto() || IsGerenteMarketing() || IsGerenteNacional() || IsGerenteRegional() || IsGerenteDistrital());
        }

        public bool IsAgencia()
        {
            return (IdGrupo == Usuario.EnumTipo.Agencia.GetDescription());
        }

        public bool IsAdministrador()
        {
            return (IdGrupo == Usuario.EnumTipo.Administrador.GetDescription() || IdGrupo == Usuario.EnumTipo.Desenvolvedor.GetDescription());
        }

        public bool IsDesenvolvedor()
        {
            return (IdGrupo == Usuario.EnumTipo.Desenvolvedor.GetDescription());
        }
    }
}
