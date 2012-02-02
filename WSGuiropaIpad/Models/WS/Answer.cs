using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Models.WS
{
    [Serializable]
    public class Answer
    {
        public String Status { get; set; }
        public String Message { get; set; }
        public List<Presentation> Presentations { get; set; }
        public List<Doctor> Doctors { get; set; }
        public Presentation Presentation { get; set; }
        public String QuantityNotUpdated { get; set; }
        public AppInfo AppInfo { get; set; }
        public User User { get; set; }
        public List<PresentationComment> PresentationComments { get; set; }

        public enum EnumStatus
        {
            Ok = 0,
            Erro = 1,
            ErroLogin = 2
        }

        private Answer() { }

        public Answer(EnumStatus status)
        {
            this.Status = ((int)status).ToString();
            this.Message = String.Empty;
        }

        public Answer(EnumStatus status, String mensagem)
        {
            this.Status = ((int)status).ToString();
            this.Message = mensagem;
        }

        public Answer(EnumStatus status, String mensagem, object objeto)
        {
            this.Status = ((int)status).ToString();
            this.Message = mensagem;
            
            var tipo = objeto.GetType();

            if (tipo == typeof(Presentation))
            {
                this.Presentation = (Presentation)objeto;
            }
            else if (tipo == typeof(List<Presentation>))
            {
                this.Presentations = (List<Presentation>)objeto;
            }
            else if (tipo == typeof(List<Doctor>))
            {
                this.Doctors = (List<Doctor>)objeto;
            }
            else if (tipo == typeof(string))
            {
                this.QuantityNotUpdated = (string)objeto;
            }
            else if (tipo == typeof(AppInfo))
            {
                this.AppInfo = (AppInfo)objeto;
            }
            else if (tipo == typeof(User))
            {
                this.User = (User)objeto;
            }
            else if (tipo == typeof(List<PresentationComment>))
            {
                this.PresentationComments = (List<PresentationComment>)objeto;
            }
        }
        
    }
}
