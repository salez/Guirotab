using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class EmailRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Email> GetEmails()
        {
            var emails = from e in db.Emails
                         select e;

            return emails;
        }

        public Email GetEmail(int id)
        {
            var emails = from e in db.Emails
                         select e;

            return emails.SingleOrDefault(e => e.Id == id);
        }

        public void Add(MailMessage mail, SmtpClient smtp)
        {
            Email email = new Email();

            email.DataInclusao = DateTime.Now;
            email.Assunto = mail.Subject;
            email.Corpo = mail.Body;
            email.CorpoHtml = mail.IsBodyHtml;
            email.De = mail.From.DisplayName;
            email.DeEmail = mail.From.Address;
            email.Encode = mail.BodyEncoding.WebName;
            email.Host = smtp.Host;
            email.Porta = smtp.Port.ToString();
            email.SmtpAutenticado = !smtp.UseDefaultCredentials;

            if (!smtp.UseDefaultCredentials && smtp.Credentials != null)
            {
                email.SmtpUsuario = ((NetworkCredential)smtp.Credentials).UserName;
                email.SmtpSenha = ((NetworkCredential)smtp.Credentials).Password;
            }

            email.SslHabilitado = smtp.EnableSsl;

            db.Emails.InsertOnSubmit(email);

            this.Save();

            if (mail.To != null)
            {
                foreach (var destinatario in mail.To)
                {
                    EmailDestinatario emailDestinatario = new EmailDestinatario();

                    emailDestinatario.Email = destinatario.Address;
                    emailDestinatario.Nome = destinatario.DisplayName;
                    emailDestinatario.Tipo = (Char)Email.EnumTipo.Destinatario;
                    emailDestinatario.IdEmail = email.Id;

                    db.EmailDestinatarios.InsertOnSubmit(emailDestinatario);
                }
            }

            if (mail.CC != null)
            {
                foreach (var destinatario in mail.CC)
                {
                    EmailDestinatario emailDestinatario = new EmailDestinatario();

                    emailDestinatario.Email = destinatario.Address;
                    emailDestinatario.Nome = destinatario.DisplayName;
                    emailDestinatario.Tipo = (Char)Email.EnumTipo.Copia;
                    emailDestinatario.IdEmail = email.Id;

                    db.EmailDestinatarios.InsertOnSubmit(emailDestinatario);
                }
            }

            if (mail.Bcc != null)
            {
                foreach (var destinatario in mail.Bcc)
                {
                    EmailDestinatario emailDestinatario = new EmailDestinatario();

                    emailDestinatario.Email = destinatario.Address;
                    emailDestinatario.Nome = destinatario.DisplayName;
                    emailDestinatario.Tipo = (Char)Email.EnumTipo.CopiaOculta;
                    emailDestinatario.IdEmail = email.Id;

                    db.EmailDestinatarios.InsertOnSubmit(emailDestinatario);
                }
            }
        }

        public void Delete(Email email)
        {
            db.EmailDestinatarios.DeleteAllOnSubmit(email.EmailDestinatarios);

            db.Emails.DeleteOnSubmit(email);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
