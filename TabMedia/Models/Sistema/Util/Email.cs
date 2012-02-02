using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Web;

namespace Util
{
    /// <summary>
    /// Summary description for Email
    /// </summary>
    public static class Email
    {
        public static MailAddress EmailContatoPadrao
        {
            get { return new MailAddress(Util.Sistema.Email.EmailContatoPadrao, Util.Sistema.Email.NomeContatoPadrao); }
        }

        public static MailAddress EmailEnvioPadrao
        {
            get { return new MailAddress(Util.Sistema.Email.EmailEnvioPadrao, Util.Sistema.Email.NomeEnvioPadrao); }
        }

        public static MailAddress EmailDesenvolvedor
        {
            get { return new MailAddress(Util.Sistema.Email.EmailDesenvolvedor, Util.Sistema.Email.EmailDesenvolvedor); }
        }

        public static String GetCorpoEmail(String nomeArquivoEmail)
        {
            return GetCorpoEmail(nomeArquivoEmail, ".htm");
        }

        /// <summary>
        /// Retorna o conteúdo no arquivo nomeArquivoEmail que deve estar localizado na pasta Email do root
        /// </summary>
        /// <param name="nomeArquivoEmail"></param>
        /// <param name="extensao">padrao: ".htm"</param>
        /// <returns></returns>
        public static String GetCorpoEmail(String nomeArquivoEmail, String extensao)
        {
            if (extensao == null || extensao == String.Empty)
                extensao = ".htm";

            Encoding iso = System.Text.Encoding.GetEncoding("ISO-8859-1");

            StreamReader reader = new StreamReader(Util.Url.GetCaminhoFisico("~/Email/" + nomeArquivoEmail + ".htm"), iso);

            String corpoEmail = reader.ReadToEnd();

            corpoEmail = corpoEmail
                .Replace("../", Util.Sistema.SiteUrl + "/")
                .Replace("[NomeSistema]", Util.Sistema.NomeSistema);

            reader.Close();
            reader.Dispose();

            return corpoEmail;
        }

        public static void Enviar(String to, String body, String subject)
        {
            MailAddress emailTo = new MailAddress(to);
            Enviar(emailTo, body, subject);
        }

        public static void Enviar(MailAddress to, String body, String subject)
        {
            MailAddressCollection emails = new MailAddressCollection();
            emails.Add(to);
            MailAddress from = Util.Email.EmailEnvioPadrao;
            Enviar(from, emails, body, subject);
        }

        public static void Enviar(String from, String to, String body, String subject)
        {
            MailAddress emailFrom = new MailAddress(from);
            MailAddress emailTo = new MailAddress(to);
            Enviar(emailFrom, emailTo, body, subject);
        }

        public static void Enviar(MailAddress from, MailAddress to, String body, String subject)
        {
            MailAddressCollection emails = new MailAddressCollection();
            emails.Add(to);
            Enviar(from, emails, body, subject);
        }

        public static void Enviar(MailAddress from, MailAddress to, MailAddress toCC, String body, String subject)
        {
            MailAddressCollection emailsTo = new MailAddressCollection();
            emailsTo.Add(to);

            MailAddressCollection emailsToCC = new MailAddressCollection();
            emailsToCC.Add(to);

            Enviar(from, emailsTo, emailsToCC, body, subject);
        }

        public static void Enviar(MailAddressCollection to, String body, String subject)
        {
            MailAddress from = Util.Email.EmailEnvioPadrao;
            Enviar(from, to, body, subject);
        }

        public static void Enviar(MailAddressCollection to, MailAddressCollection toCC, String body, String subject)
        {
            MailAddress from = Util.Email.EmailEnvioPadrao;
            Enviar(from, to, toCC, body, subject);
        }

        public static void Enviar(MailAddressCollection to, MailAddressCollection toCC, MailAddressCollection toBcc, String body, String subject)
        {
            MailAddress from = Util.Email.EmailEnvioPadrao;
            Enviar(from, to, toCC, toBcc, body, subject);
        }

        public static void Enviar(MailAddress from, MailAddressCollection to, String body, String subject)
        {
            Enviar(from, to, null, body, subject);
        }

        public static void Enviar(MailAddress from, MailAddressCollection to, MailAddressCollection toCC, String body, String subject)
        {
            Enviar(from, to, toCC, null, body, subject);
        }

        public static void Enviar(MailAddress from, MailAddressCollection to, MailAddressCollection toCC, MailAddressCollection toBcc, String body, String subject)
        {
            Enviar(from, to, toCC, toBcc, body, subject, true);
        }

        public static void Enviar(MailAddress from, MailAddressCollection to, MailAddressCollection toCC, MailAddressCollection toBCC, String body, String subject, bool incluiNomeSistemaNoAssunto)
        {
            MailMessage mail = new MailMessage();

            //destinatarios
            foreach (MailAddress email in to)
            {
                mail.To.Add(email);
            }

            if (toCC != null)
            {
                //destinatarios - cópia
                foreach (MailAddress email in toCC)
                {
                    mail.CC.Add(email);
                }
            }

            if (toBCC != null)
            {
                //destinatarios - cópia oculta
                foreach (MailAddress email in toBCC)
                {
                    mail.Bcc.Add(email);
                }
            }

            mail.From = from;
            mail.Subject = subject;

            if (incluiNomeSistemaNoAssunto)
            {
                mail.Subject = mail.Subject + " - " + Util.Sistema.NomeSistema;
            }

            mail.IsBodyHtml = true;

            mail.Body = body;

            SmtpClient smtp = new SmtpClient();

            smtp.EnableSsl = Util.Sistema.Email.EnableSsl;

            if (Util.Sistema.Email.EmailEncoding != String.Empty)
            {
                Encoding encode = System.Text.Encoding.GetEncoding(Util.Sistema.Email.EmailEncoding);
                mail.BodyEncoding = encode;
            }

            if (Util.Sistema.Email.SmtpAutenticado)
            {
                smtp.UseDefaultCredentials = false;
                NetworkCredential credencial = new NetworkCredential(Util.Sistema.Email.SmtpUsername, Util.Sistema.Email.SmtpPassword);
                smtp.Credentials = credencial;
            }

            smtp.Host = Util.Sistema.Email.SmtpHost;

            if (Util.Sistema.Email.SmtpPort != null)
            {
                smtp.Port = Util.Sistema.Email.SmtpPort.Value;
            }

            smtp.Send(mail);

            Models.EmailRepository emailRepository = new Models.EmailRepository();
            emailRepository.Add(mail, smtp);
            emailRepository.Save();
        }
    }
}