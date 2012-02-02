using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models;
using System.ComponentModel;

namespace Models
{
    [MetadataType(typeof(Usuario_Validation))]
    public partial class Usuario
    {
        public enum EnumTipo
        {
            [Description("AGE")]
            Agencia,
            [Description("GER")]
            Gerente,
            [Description("DES")]
            Desenvolvedor
        }

        public enum EnumStatus
        {
            Ativo = 'A',
            Inativo = 'I'
        }
    }

    public class Usuario_Validation
    {
        [Required(ErrorMessage = "Nome obrigatório")]
        [StringLength(50, ErrorMessage = "Nome deve ter no máximo 50 caracteres")]
        public String Nome { get; set; }

        [Required(ErrorMessage = "Login obrigatório")]
        [StringLength(50, ErrorMessage = "Login deve ter no máximo 50 caracteres")]
        public String Login { get; set; }

        [StringLength(50, ErrorMessage = "Senha deve ter no máximo 50 caracteres")]
        public String Senha { get; set; }

        [Required(ErrorMessage = "Email obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email inválido")]
        [Email(ErrorMessage = "Email inválido")]
        public String Email { get; set; }

        [Required(ErrorMessage = "Status obrigatório")]
        public int Status { get; set; }

        [Required(ErrorMessage = "Grupo obrigatório")]
        public int IdGrupo { get; set; }
    }

}