using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class AcaoGrupoRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<AcaoGrupo> GetAcaosGrupo()
        {
            IQueryable<AcaoGrupo> acaoGrupos = from acaogrupo in db.AcaoGrupos
                                               select acaogrupo;

            return acaoGrupos;
        }

        public IQueryable<AcaoGrupo> GetAcaosGrupoByAcao(int idAcao)
        {
            IQueryable<AcaoGrupo> acaoGrupos = from acaogrupo in db.AcaoGrupos
                                               where acaogrupo.IdAcao == idAcao
                                               select acaogrupo;

            return acaoGrupos;
        }

        public IQueryable<AcaoGrupo> GetAcaosGrupoByGrupo(string idGrupo)
        {
            IQueryable<AcaoGrupo> acaoGrupos = from acaogrupo in db.AcaoGrupos
                                               where acaogrupo.IdGrupo == idGrupo
                                               select acaogrupo;

            return acaoGrupos;
        }

        public void Add(AcaoGrupo acaoGrupo)
        {
            db.AcaoGrupos.InsertOnSubmit(acaoGrupo);
        }

        public void Delete(AcaoGrupo acaoGrupo)
        {
            db.AcaoGrupos.DeleteOnSubmit(acaoGrupo);
        }

        public void DeleteAll(string idGrupo)
        {
            db.AcaoGrupos.DeleteAllOnSubmit(db.AcaoGrupos.Where(acaogrupo => acaogrupo.IdGrupo == idGrupo));
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
