using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class TerritorioRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<TerritorioProdutoVaDownload> GetTerritoriosDownload()
        {
            var downloads = from e in db.TerritorioProdutoVaDownloads
                              select e;

            return downloads;
        }

        public IQueryable<Territorio> GetTerritorios()
        {
            var territorios = from e in db.Territorios
                         select e;

            return territorios;
        }

        public Territorio GetTerritorio(string id)
        {
            var territorios = from e in db.Territorios
                              select e;

            return territorios.SingleOrDefault(e => e.Id == id);
        }

        public Territorio GetTerritorio(string id, string senha)
        {
            var territorios = from e in db.Territorios
                              select e;

            return territorios.SingleOrDefault(e => e.Id == id && e.Senha == senha);
        }

        public void Add(Territorio territorio)
        {
            territorio.DataInclusao = DateTime.Now;

            db.Territorios.InsertOnSubmit(territorio);
        }

        public void Delete(Territorio territorio)
        {
            db.RelatorioPaginas.DeleteAllOnSubmit(territorio.RelatorioPaginas);
            db.RelatorioEmails.DeleteAllOnSubmit(territorio.RelatorioEmails);
            db.TerritorioProdutoVaDownloads.DeleteAllOnSubmit(territorio.TerritorioProdutoVaDownloads);
            db.Territorios.DeleteOnSubmit(territorio);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
