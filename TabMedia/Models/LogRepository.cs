using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class LogRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<Log> GetLogs()
        {
            IQueryable<Log> logs = from log in db.Logs
                                   select log;

            return logs;
        }

        public Log GetLog(int id)
        {
            return db.Logs.SingleOrDefault(log => log.Id == id);
        }

        public void Add(Log log)
        {
            db.Logs.InsertOnSubmit(log);
        }

        public void Delete(Log log)
        {
            db.Logs.DeleteOnSubmit(log);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
