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
            [Description("Agência")]
            Agencia,
            [Description("Agências")]
            Agencias,
            Ajuda,
            [Description("Conteúdo")]
            Doutores,
            Emails,
            Erros,
            [Description("Gerenciador de Arquivos")]
            GerenciadorDeArquivos,
            Gerente,
            Gerentes,
            Grupos,
            [Description("Importação")]
            Importacao,
            Instalacao,
            Login,
            Logs,
            [Description("Páginas")]
            Paginas,
            [Description("VA's do Produto")]
            Produtos,
            ProdutosVas,
            [Description("Meu Cadastro")]
            MeuCadastro,
            Newsletters,
            [Description("Notícias")]
            Noticias,
            [Description("Relatórios")]
            Relatorios,
            Sistema,
            Territorios,
            [Description("Usuários")]
            Usuarios
        }

        public enum EnumArea
        {
            Admin = 'A',
            Site = 'S',
            WebService = 'W'
        }

        public enum EnumEntidadeGenero
        {
            Feminino,
            Masculino
        }
    }
}
