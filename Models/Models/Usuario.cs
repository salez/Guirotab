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
            GerenteProduto,
            [Description("GEM")]
            GerenteMarketing,
            [Description("DES")]
            Desenvolvedor,
            [Description("GEN")]
            GerenteNacional,
            [Description("GEG")]
            GerenteRegional,
            [Description("GED")]
            GerenteDistrital,
            [Description("AMC")]
            Administrador
        }


        public enum EnumStatus
        {
            Ativo = 'A',
            Inativo = 'I'
        }

        public IEnumerable<Usuario> GetAgencias()
        {
            if (this.IsGerente()) { 
                return this.UsuarioGerenteAgencias.Select(rel => rel.Agencia);
            }

            Exception ex = new Exception("Este usuário não é gerente.");

            Util.Sistema.Error.TrataErro(ex);

            throw ex;
        }

        /// <summary>
        /// retorna o gerente de marketing do primeiro de seus produtos que tiverem um gerente de marketing relacionado (somente para gerente de produto)
        /// </summary>
        /// <returns></returns>
        public Usuario GetGerenteMarketing()
        {
            if (this.IsGerenteProduto())
            {
                return this.UsuarioProdutos.Select(up => up.Produto.UsuarioProdutos.FirstOrDefault(up2 => up2.Usuario.IsGerenteMarketing()).Usuario).FirstOrDefault();
            }

            Exception ex = new Exception("Este usuário não é gerente de produto, operação ilegal.");

            Util.Sistema.Error.TrataErro(ex);

            throw ex;
        }

        public Usuario GetAgencia(int idAgencia)
        {
            if (this.IsGerente())
            {
                return this.UsuarioGerenteAgencias.Select(rel => rel.Agencia).Where(a => a.Id == idAgencia).FirstOrDefault();
            }

            Exception ex = new Exception("Este usuário não é gerente.");

            Util.Sistema.Error.TrataErro(ex);

            throw ex;
        }

        public IEnumerable<Usuario> GetGerentes()
        {
            if (this.IsAgencia())
            {
                return this.UsuarioGerenteAgencias.Select(rel => rel.Gerente);
            }

            Exception ex = new Exception("Este usuário não é agência.");

            Util.Sistema.Error.TrataErro(ex);

            throw ex;
        }

        /// <summary>
        /// só adiciona caso seja um gerente, caso não seja causa erro.
        /// </summary>
        public void AddAgencia(UsuarioRepository usuarioRepository, Usuario agencia)
        {
            //verifica se usuario é gerente
            if (!this.IsGerente()) { 
                Util.Sistema.Error.TrataErro(new Exception("Tentativa de adicionar uma agencia a um usuário que não é gerente."));
                return;
            }

            //verifica se agencia existe
            if(agencia != null){

                var relacaoGerenteAgencia = new UsuarioGerenteAgencia();

                relacaoGerenteAgencia.IdUsuarioGerente = this.Id;
                relacaoGerenteAgencia.IdUsuarioAgencia = agencia.Id;

                usuarioRepository.AddRelacaoGerenteAgencia(relacaoGerenteAgencia);
            }
        }

        public void AddProduto(UsuarioRepository usuarioRepository, Produto produto)
        {
            //verifica se usuario é gerente ou agencia
            if (!this.IsGerente() && !this.IsAgencia())
            {
                Util.Sistema.Error.TrataErro(new Exception("Tentativa de adicionar um produto a um usuário que não é gerente nem agência."));
                return;
            }

            //verifica se produto existe
            if (produto != null)
            {
                var relacaoUsuarioProduto = new UsuarioProduto();

                relacaoUsuarioProduto.IdUsuario = this.Id;
                relacaoUsuarioProduto.IdProduto = produto.Id;

                usuarioRepository.AddRelacaoUsuarioProduto(relacaoUsuarioProduto);
            }
        }

        /// <summary>
        /// apaga todas as relações da agencia com os produtos que também estão relacionados com o gerente.
        /// </summary>
        public void ApagarRelacoesUsuarioProdutoByGerente(UsuarioRepository usuarioRepository, Usuario gerente)
        {
            //verifica se usuario é agencia
            if (this.Grupo.Id != EnumTipo.Agencia.GetDescription())
            {
                var ex = new Exception("Tentativa de apagar relação de usuário/produto em um usuário que não é agência.");
                
                Util.Sistema.Error.TrataErro(ex);

                throw ex;
            }

            usuarioRepository.DeleteAllRelacoesUsuarioProdutoByUsuarioGerente(this, gerente);
        }

        /// <summary>
        /// apaga todas as relações da agencia com os produtos que também estão relacionados com o gerente.
        /// </summary>
        public void ApagarRelacoesUsuarioProduto(UsuarioRepository usuarioRepository)
        {
            usuarioRepository.DeleteAllRelacoesUsuarioProdutoByUsuario(this);
        }

        /// <summary>
        /// Verifica se o usuario tem algum produto relacionado
        /// </summary>
        /// <param name="idProduto"></param>
        /// <returns></returns>
        public bool temProduto(int? idProduto)
        {
            if (idProduto == null) return false;

            return this.UsuarioProdutos.Any(up => up.IdProduto == idProduto);
        }

        /// <summary>
        /// Verifica se o usuário é um gerente de produto ou de marketing
        /// </summary>
        /// <returns></returns>
        public bool IsGerente()
        {
            return (this.IdGrupo == EnumTipo.GerenteProduto.GetDescription() || this.IdGrupo == EnumTipo.GerenteMarketing.GetDescription() || this.IdGrupo == EnumTipo.GerenteNacional.GetDescription() || this.IdGrupo == EnumTipo.GerenteRegional.GetDescription() || this.IdGrupo == EnumTipo.GerenteDistrital.GetDescription());
        }

        /// <summary>
        /// verifica se o usuario é um gerente de produto
        /// </summary>
        /// <returns></returns>
        public bool IsGerenteProduto()
        {
            return (this.IdGrupo == EnumTipo.GerenteProduto.GetDescription());
        }

        /// <summary>
        /// verifica se o usuário é um gerente de marketing
        /// </summary>
        /// <returns></returns>
        public bool IsGerenteMarketing()
        {
            return (this.IdGrupo == EnumTipo.GerenteMarketing.GetDescription());
        }

        /// <summary>
        /// verifica se o usuário é um gerente de marketing
        /// </summary>
        /// <returns></returns>
        public bool IsGerenteNacional()
        {
            return (this.IdGrupo == EnumTipo.GerenteNacional.GetDescription());
        }

        /// <summary>
        /// verifica se o usuário é um gerente de marketing
        /// </summary>
        /// <returns></returns>
        public bool IsGerenteRegional()
        {
            return (this.IdGrupo == EnumTipo.GerenteRegional.GetDescription());
        }

        /// <summary>
        /// verifica se o usuário é um gerente de marketing
        /// </summary>
        /// <returns></returns>
        public bool IsGerenteDistrital()
        {
            return (this.IdGrupo == EnumTipo.GerenteDistrital.GetDescription());
        }

        /// <summary>
        /// verifica se o usuário é uma agência
        /// </summary>
        /// <returns></returns>
        public bool IsAgencia()
        {
            return (this.IdGrupo == EnumTipo.Agencia.GetDescription());
        }

        public bool IsAdministrador()
        {
            return (IdGrupo == Usuario.EnumTipo.Administrador.GetDescription() || IdGrupo == Usuario.EnumTipo.Desenvolvedor.GetDescription());
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