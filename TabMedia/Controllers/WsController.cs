using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Ionic.Zip;
using CAVEditLib;
using GuiropaIpad.serviceEMS;
using System.Xml;
using System.Text;
using System.Net;
using System.IO;

namespace Controllers
{

    public class WsController : BaseController
    {
        // used on each read operation
        byte[] buf = new byte[8192];


        public ActionResult Index()
        {
            return new EmptyResult();
        }

        public string ler(HttpWebResponse response)
        {
            // used to build entire input
            StringBuilder sb = new StringBuilder();

            // we will read data via the response stream
            Stream resStream = response.GetResponseStream();

            string tempString = null;
            int count = 0;

            do
            {
                // fill the buffer with data
                count = resStream.Read(buf, 0, buf.Length);

                // make sure we read some data
                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    tempString = Encoding.UTF8.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            } while (count > 0); // any more data to read?

            return sb.ToString();
        }

        public ActionResult Call()
        {
            Response.AddHeader("Access-Control-Allow-Origin", "*");
            Response.AddHeader("Content-Type", "text/xml");
            //ACESSA WS

            ASCIIEncoding encoding = new ASCIIEncoding();
            string postData = "territory=" + Request.QueryString["territory"];
            postData += ("&password=" + Request.QueryString["password"]);

            byte[] data = encoding.GetBytes(postData);

            string urlM = Request.QueryString["metodo"];

            if (urlM.IsNullOrEmpty())
            {
                return Content("oi");
            }

            // Prepare web request...
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(urlM);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.CookieContainer = new CookieContainer();
            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            // execute the request
            HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
            var resultado1 = ler(response);
            
            return Content(resultado1);

            //ESCREVE XML

            XmlDocument wsResponse = new XmlDocument();

            string url = "http://guiropatab.com:9998/ipadws.asmx/GetInfoPresentations"

            + Request.QueryString["FromBox"].ToString()
            + "&ToCurrency="
            + Request.QueryString["ToBox"].ToString();

            wsResponse.Load(url);

            string XMLDocument = wsResponse.InnerXml;

            Response.ContentType = "text/xml";

            return Content(XMLDocument);
        }

    }

}
