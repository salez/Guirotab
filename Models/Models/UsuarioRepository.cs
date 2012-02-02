using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class UsuarioRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Usuario> GetUsuarios()
        {
            var usuarios = from usuario in db.Usuarios
                           select usuario;

            return usuarios;
        }

        public IQueryable<Usuario> GetUsuariosAtivos()
        {
            var usuarios = from usuario in db.Usuarios
                           where usuario.Status == (char)Usuario.EnumStatus.Ativo
                           select usuario;

            return usuarios;
        }

        public Usuario GetAgencia(string nome)
        {
            var usuarios = from usuario in db.Usuarios
                           where
                                usuario.Grupo.Id == Usuario.EnumTipo.Agencia.GetDescription()
                                && usuario.Nome == nome
                           select usuario;

            return usuarios.SingleOrDefault();
        }

        public Usuario GetAgencia(int id)
        {
            var usuarios = from usuario in db.Usuarios
                           where
                                usuario.Grupo.Id == Usuario.EnumTipo.Agencia.GetDescription()
                                && usuario.Id == id
                           select usuario;

            return usuarios.SingleOrDefault();
        }

        public IQueryable<Usuario> GetAgencias()
        {
            var usuarios = from usuario in db.Usuarios
                           where
                                usuario.Grupo.Id == Usuario.EnumTipo.Agencia.GetDescription()
                           select usuario;

            return usuarios;
        }

        public Usuario GetGerente(string nome)
        {
            var usuarios = from usuario in db.Usuarios
                           where
                                usuario.Grupo.Id == Usuario.EnumTipo.GerenteProduto.GetDescription()
                                && usuario.Nome == nome
                           select usuario;

            return usuarios.SingleOrDefault();
        }

        public Usuario GetGerenteProduto(int id)
        {
            var usuarios = from usuario in db.Usuarios
                           where
                                usuario.Grupo.Id == Usuario.EnumTipo.GerenteProduto.GetDescription()
                                && usuario.Id == id
                           select usuario;

            return usuarios.SingleOrDefault();
        }

        public IQueryable<Usuario> GetGerentesProduto()
        {
            var usuarios = from usuario in db.Usuarios
                           where
                                usuario.Grupo.Id == Usuario.EnumTipo.GerenteProduto.GetDescription()
                           select usuario;

            return usuarios;
        }

        public Usuario GetGerenteMarketing(string nome)
        {
            var usuarios = from usuario in db.Usuarios
                           where
                                usuario.Grupo.Id == Usuario.EnumTipo.GerenteMarketing.GetDescription()
                                && usuario.Nome == nome
                           select usuario;

            return usuarios.SingleOrDefault();
        }

        public Usuario GetGerenteMarketing(int id)
        {
            var usuarios = from usuario in db.Usuarios
                           where
                                usuario.Grupo.Id == Usuario.EnumTipo.GerenteMarketing.GetDescription()
                                && usuario.Id == id
                           select usuario;

            return usuarios.SingleOrDefault();
        }

        public IQueryable<Usuario> GetGerentesMarketing()
        {
            var usuarios = from usuario in db.Usuarios
                           where
                                usuario.Grupo.Id == Usuario.EnumTipo.GerenteMarketing.GetDescription()
                           select usuario;

            return usuarios;
        }

        public Usuario GetUsuario(int id)
        {
            var usuarios = from usuario in db.Usuarios
                           select usuario;

            return usuarios.SingleOrDefault(u => u.Id == id);
        }

        public Usuario GetUsuarioAtivo(int id)
        {
            var usuarios = from usuario in db.Usuarios
                           where usuario.Status == (char)Usuario.EnumStatus.Ativo
                           select usuario;

            return usuarios.SingleOrDefault(u => u.Id == id);
        }

        public Usuario GetUsuarioByEmail(string email)
        {
            var usuarios = from usuario in db.Usuarios
                           where usuario.Status == (char)Usuario.EnumStatus.Ativo
                           select usuario;

            return usuarios.SingleOrDefault(u => u.Email == email);
        }

        public Usuario GetUsuarioByLoginOrEmail(string login, string email)
        {
            var usuarios = from usuario in db.Usuarios
                           where usuario.Status == (char)Usuario.EnumStatus.Ativo
                           select usuario;

            return usuarios.SingleOrDefault(u => u.Login == login || u.Email == email);
        }

        public Usuario GetUsuarioByLoginSenha(String login, String senha)
        {
            var usuarios = from usuario in db.Usuarios
                           where usuario.Status == (char)Usuario.EnumStatus.Ativo
                           select usuario;

            return usuarios.SingleOrDefault(u => u.Login == login && u.Senha == senha);
        }

        public Usuario GetUsuarioByEmailSenha(String email, String senha)
        {
            var usuarios = from usuario in db.Usuarios
                           where usuario.Status == (char)Usuario.EnumStatus.Ativo
                           select usuario;

            return usuarios.SingleOrDefault(u => u.Email == email && u.Senha == senha);
        }

        public int GetQtdeUsuariosCadastrados()
        {
            var usuarios = from usuario in db.Usuarios
                           where usuario.Status == (char)Usuario.EnumStatus.Ativo
                           select usuario;

            return usuarios.Count();
        }

        public bool VerificaLoginSenha(String login, String senha)
        {
            IQueryable<Usuario> usuarios = from usu in db.Usuarios
                                           where usu.Senha == senha && usu.Login == login && Convert.ToBoolean(usu.Status)
                                           select usu;

            Usuario usuario = usuarios.FirstOrDefault();

            if (usuario == null)
            {
                return false;
            }

            return true;
        }

        public UsuarioGerenteAgencia GetRelacaoGerenteAgencia(int idGerente, int idAgencia)
        {
            var relacao = from r in db.UsuarioGerenteAgencias
                              where r.IdUsuarioAgencia == idAgencia && r.IdUsuarioGerente == idGerente
                              select r;

            return relacao.SingleOrDefault();
        }

        /// <summary>
        /// adiciona relação do gerente com agencia se não existir
        /// </summary>
        /// <param name="usuarioProduto"></param>
        public void AddRelacaoGerenteAgencia(UsuarioGerenteAgencia usuarioGerenteAgencia)
        {
            if (GetRelacaoGerenteAgencia(usuarioGerenteAgencia.IdUsuarioGerente.Value, usuarioGerenteAgencia.IdUsuarioAgencia.Value) == null) {
                db.UsuarioGerenteAgencias.InsertOnSubmit(usuarioGerenteAgencia);
            }
        }

        /// <summary>
        /// adiciona relação do usuario com o produto se não existir
        /// </summary>
        /// <param name="usuarioProduto"></param>
        public void AddRelacaoUsuarioProduto(UsuarioProduto usuarioProduto)
        {
            if (GetRelacaoUsuarioProduto(usuarioProduto.IdUsuario.Value, usuarioProduto.IdProduto.Value) == null) { 
                db.UsuarioProdutos.InsertOnSubmit(usuarioProduto);
            }
        }

        public UsuarioProduto GetRelacaoUsuarioProduto(int idUsuario, int idProduto)
        {
            var relacao = from up in db.UsuarioProdutos
                          where
                              up.IdUsuario == idUsuario &&
                              up.IdProduto == idProduto
                          select up;

            return relacao.SingleOrDefault();
        }

        public void Add(Usuario usuario)
        {
            usuario.DataInclusao = DateTime.Now;

            db.Usuarios.InsertOnSubmit(usuario);
        }

        public void Delete(Usuario usuario)
        {
            db.Logs.DeleteAllOnSubmit(usuario.Logs);

            db.Usuarios.DeleteOnSubmit(usuario);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        public bool EmailExiste(String email)
        {
            return db.Usuarios.Any(model => model.Email == email);
        }

        public bool LoginExiste(String login)
        {
            return db.Usuarios.Any(model => model.Login == login);
        }

        public void DeleteAllRelacoesUsuarioProduto()
        {
            db.UsuarioProdutos.DeleteAllOnSubmit(db.UsuarioProdutos);
        }

        /// <summary>
        /// deleta todas as relações de produto com o usuario onde os produtos também estejam relacionados com o gerente
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="gerente"></param>
        public void DeleteAllRelacoesUsuarioProdutoByUsuarioGerente(Usuario usuario, Usuario gerente)
        {
            var usuarioProdutos = 
                    from up in db.UsuarioProdutos
                    where
                        up.IdUsuario == usuario.Id &&
                        gerente.UsuarioProdutos.Select(upg => upg.IdProduto).Contains(up.IdProduto)
                    select up;

            db.UsuarioProdutos.DeleteAllOnSubmit(usuarioProdutos);
        }

        /// <summary>
        /// deleta todas as relações de produto com o usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="gerente"></param>
        public void DeleteAllRelacoesUsuarioProdutoByUsuario(Usuario usuario)
        {
            var usuarioProdutos =
                    from up in db.UsuarioProdutos
                    where
                        up.IdUsuario == usuario.Id
                    select up;

            db.UsuarioProdutos.DeleteAllOnSubmit(usuarioProdutos);
        }

        public void DeleteAllRelacoesUsuarioGerenteAgencia()
        {
            db.UsuarioGerenteAgencias.DeleteAllOnSubmit(db.UsuarioGerenteAgencias);
        }

        public void InativaGerentesAgencias()
        {
            var usuarios = from u in db.Usuarios
                           where
                                u.Grupo.Id == Usuario.EnumTipo.GerenteProduto.GetDescription() ||
                                u.Grupo.Id == Usuario.EnumTipo.Agencia.GetDescription()
                           select u;

            foreach (var usuario in usuarios)
            {
                usuario.Status = (char)Usuario.EnumStatus.Inativo;
            }
        }
    }
}
