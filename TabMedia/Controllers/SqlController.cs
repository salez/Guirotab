using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Controllers
{
    [AuthorizeLogin]
    public class SqlController : BaseController
    {
        //
        // GET: /Admin/Sql/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(String sql)
        {
            string connection = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            SqlConnection db = new SqlConnection(connection);

            DataTable dt = null;

            try
            {
                db.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, db);

                if (sql.Length >= 6 && sql.Substring(0, 6).ToLower() == "select")
                {
                    dt = new DataTable();
                    adapter.Fill(dt);

                    ViewData["result"] = dt.Rows.Count.ToString() + " rows selected";
                }
                else
                {
                    SqlCommand cmd = new SqlCommand(sql, db);
                    int count = cmd.ExecuteNonQuery();

                    ViewData["result"] = count.ToString() + " rows affected";
                }

                db.Close();
            }
            catch (System.Exception ex)
            {
                ViewData["result"] = ex.Message;
            }

            return View(dt);
        }

    }
}
