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
    }
}
