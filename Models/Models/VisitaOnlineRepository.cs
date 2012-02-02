using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class VisitaOnlineRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<VisitaOnline> GetVisitasOnline()
        {
            var visitas = from v in db.VisitaOnlines
                          select v;

            return visitas;
        }

        public int GetQtdeVisitasOnline()
        {
            var visitas = from v in db.VisitaOnlines
                          select v;

            return visitas.Count();
        }

        public int GetQtdeMembrosOnline()
        {
            var visitas = from v in db.VisitaOnlines
                          where v.IdUsuario != null
                          select v;

            return visitas.Count();
        }

        public void AtualizaVisitasOnline()
        {
            String ipCliente = HttpContext.Current.Request.UserHostAddress;
            VisitaOnline visita = db.VisitaOnlines.SingleOrDefault(v => v.IP == ipCliente);

            //deleta as visitas inativas
            var visitasInativas = db.VisitaOnlines.Where(v => v.DataInclusao < DateTime.Now.AddMinutes(-5));

            db.VisitaOnlines.DeleteAllOnSubmit(visitasInativas);

            //inclui ou atualiza a visita
            if (visita == null)
            {
                visita = new VisitaOnline();
                visita.IP = ipCliente;
                if (Sessao.Site.UsuarioLogado())
                {
                    visita.IdUsuario = Sessao.Site.UsuarioInfo.Id;
                }
                visita.DataInclusao = DateTime.Now;

                db.VisitaOnlines.InsertOnSubmit(visita);
            }
            else
            {
                if (Sessao.Site.UsuarioLogado() && visita.IdUsuario == null)
                    visita.IdUsuario = Sessao.Site.UsuarioInfo.Id;
                visita.DataInclusao = DateTime.Now;
            }

            this.Save();
        }

        public VisitaOnline GetVisitaOnline(int id)
        {
            return db.VisitaOnlines.SingleOrDefault(u => u.Id == id);
        }

        public void Add(VisitaOnline visita)
        {
            visita.DataInclusao = DateTime.Now;

            db.VisitaOnlines.InsertOnSubmit(visita);
        }

        public void Delete(VisitaOnline visita)
        {
            db.VisitaOnlines.DeleteOnSubmit(visita);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
