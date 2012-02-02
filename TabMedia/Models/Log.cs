using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Models
{
    public partial class Log
    {
        public enum EnumTipo
        {
            [Description("Inclusão")]
            Inclusao = 'I',
            Consulta = 'C',
            [Description("Alteração")]
            Alteracao = 'A',
            [Description("Exclusão")]
            Exclusao = 'E',
            [Description("Login/Logoff")]
            Login = 'L'
        }

        public enum EnumPagina
        {
            Ajuda,
            [Description("Conteúdo")]
            Conteudo,
            Emails,
            Erros,
            Eventos,
            Grupos,
            Login,
            Logs,
            [Description("Páginas")]
            Paginas,
            [Description("Meu Cadastro")]
            MeuCadastro,
            Newsletters,
            [Description("Notícias")]
            Noticias,
            Sistema,
            [Description("Usuários")]
            Usuarios
        }

        public enum EnumArea
        {
            Admin = 'A',
            Site = 'S'
        }

        public enum EnumEntidadeGenero
        {
            Feminino,
            Masculino
        }
    }
}
