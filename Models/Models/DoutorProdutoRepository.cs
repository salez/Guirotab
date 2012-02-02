using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Models
{
    public class DoutorProdutoRepository
    {
        DBDataContext db = new DBDataContext();

        public IQueryable<DoutorProduto> GetDoutorProdutos()
        {
            var doutorProdutos = from e in db.DoutorProdutos
                         select e;

            return doutorProdutos;
        }

        public DoutorProduto GetDoutorProduto(int id)
        {
            var doutorProdutos = from e in db.DoutorProdutos
                         select e;

            return doutorProdutos.SingleOrDefault(e => e.Id == id);
        }

        public DoutorProduto GetDoutorProduto(int idDoutor, int idProduto)
        {
            var doutorProdutos = from e in db.DoutorProdutos
                                 select e;

            return doutorProdutos.SingleOrDefault(e => e.IdDoutor == idDoutor && e.IdProduto == idProduto);
        }

        public void Add(DoutorProduto doutorProduto)
        {
            var dp = GetDoutorProduto(doutorProduto.IdDoutor.Value,doutorProduto.IdProduto.Value);
            if (dp == null)
            {
                db.DoutorProdutos.InsertOnSubmit(doutorProduto);
            }
            else
            {
                dp.Orderm = doutorProduto.Orderm;
            }
        }

        public void Delete(DoutorProduto doutorProduto)
        {
            db.DoutorProdutos.DeleteOnSubmit(doutorProduto);
        }

        public void Save()
        {
            db.SubmitChanges();
        }
    }
}
