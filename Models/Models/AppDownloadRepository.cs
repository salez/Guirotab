using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class AppDownloadRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<AppDownload> GetAppDownloads()
        {
            var appDownloads = from e in db.AppDownloads
                         select e;

            return appDownloads;
        }

        public AppDownload GetAppDownload(int id)
        {
            var appDownloads = from e in db.AppDownloads
                         select e;

            return appDownloads.SingleOrDefault(e => e.Id == id);
        }

        public void Add(AppDownload appDownload)
        {
            appDownload.DataInclusao = DateTime.Now;

            db.AppDownloads.InsertOnSubmit(appDownload);
        }

        public void Delete(AppDownload appDownload)
        {
            db.AppDownloads.DeleteOnSubmit(appDownload);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
