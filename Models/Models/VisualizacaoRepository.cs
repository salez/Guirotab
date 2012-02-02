using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class VisualizacaoRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Visualizacao> GetVisualizacoes()
        {
            IQueryable<Visualizacao> visualizacoes = from v in db.Visualizacaos
                                                     select v;

            return visualizacoes;
        }

        public void DeletaVisualizacoesOntem()
        {
            IQueryable<Visualizacao> visualizacoes = from v in db.Visualizacaos
                                                     where v.DataInclusao < Convert.ToDateTime(DateTime.Now.ToShortDateString())
                                                     select v;

            db.Visualizacaos.DeleteAllOnSubmit(visualizacoes);

            this.Save();
        }

        public Visualizacao GetVisualizacao(int id)
        {
            return db.Visualizacaos.SingleOrDefault(v => v.Id == id);
        }

        public bool TemVisualizacao(string ip, Visualizacao.EnumContexto contexto, int idObjeto)
        {
            return db.Visualizacaos.Any(v => v.IP == ip && v.Contexto == contexto.ToString() && v.IdObjeto == idObjeto);
        }

        public void Add(Visualizacao v)
        {
            v.DataInclusao = DateTime.Now;

            db.Visualizacaos.InsertOnSubmit(v);
        }

        public void Delete(Visualizacao v)
        {
            db.Visualizacaos.DeleteOnSubmit(v);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
