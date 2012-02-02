using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AcaoRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Acao> GetAcaos()
        {
            IQueryable<Acao> acoes = from acao in db.Acaos
                                     select acao;

            return acoes;
        }

        public IQueryable<Acao> GetAcaosByGrupo(string idGrupo, int idControlador)
        {
            IQueryable<Acao> acoesSelecionadas = from acao in db.Acaos
                                                 join acaogrupo in db.AcaoGrupos on acao.Id equals acaogrupo.IdAcao
                                                 where acaogrupo.IdGrupo == idGrupo && acao.IdControlador == idControlador
                                                 select acao;

            IQueryable<Acao> acoes = from acao in db.Acaos
                                     where acao.IdControlador == idControlador
                                     select acao;

            return acoes;
        }

        public Acao GetAcao(int id)
        {
            AcaoGrupo acaoGrupo = new AcaoGrupo();

            return db.Acaos.SingleOrDefault(acao => acao.Id == id);
        }

        public void Add(Acao acao)
        {
            db.Acaos.InsertOnSubmit(acao);
        }

        public void Delete(Acao acao)
        {
            db.AcaoGrupos.DeleteAllOnSubmit(acao.AcaoGrupos);
            db.Acaos.DeleteOnSubmit(acao);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
