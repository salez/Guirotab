using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.IO;
using System.Text;
using PdfSharp.Drawing.Layout;
using System.Drawing;
using System.Threading;
using System.Net;
using HtmlAgilityPack;

namespace Controllers
{
    public class ExemplosController : BaseController
    {
        //
        // GET: /Exemplos/

        public ActionResult Index()
        {

            return View();
        }

        //ABRE URL

        public ActionResult Html()
        {
            string result = "";

            // The HtmlWeb class is a utility class to get the HTML over HTTP
            HtmlWeb htmlWeb = new HtmlWeb();

            WebClient webClient = new WebClient();



            // used to build entire input
            StringBuilder sb = new StringBuilder();

            // used on each read operation
            byte[] buf = new byte[8192];

            // prepare the web page we will be asking for
            HttpWebRequest request = (HttpWebRequest)
                WebRequest.Create("http://games.levelupgames.uol.com.br/lunia/comunidade/ranking/nivel/todos/pagina/1");

            // execute the request
            HttpWebResponse response = (HttpWebResponse)
                request.GetResponse();

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
                    tempString = Encoding.ASCII.GetString(buf, 0, count);

                    // continue building the string
                    sb.Append(tempString);
                }
            } while (count > 0); // any more data to read?

            // Creates an HtmlDocument object from an URL
            htmlWeb.AutoDetectEncoding = true;

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(sb.ToString());

            HtmlNode tabelaRanking = document.GetElementbyId("tbRanking");

            if (tabelaRanking != null)
            {
                // Extracts all links within that node
                IEnumerable<HtmlNode> linhas = document.DocumentNode.Descendants("tr");

                // Outputs the href for external links
                foreach (HtmlNode linha in linhas)
                {
                    IEnumerable<HtmlNode> tds = linha.Descendants("td");

                    foreach (var td in tds)
                    {
                        if (td.Attributes.Contains("class"))
                        {
                            if (td.Attributes["class"].Value == "colNome")
                            {
                                result += "<br>" + td.InnerHtml;
                            }
                        }
                    }
                }

            }

            return Content(result);
        }

        // GERAR PDF

        public FileResult Pdf()
        {
            // Cria um novo documento
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Created with PDFsharp";

            //define as fontes e cores
            XFont fontTitulo = new XFont("Verdana", 13, XFontStyle.Bold);
            XFont fontTextoTopo = new XFont("Verdana", 9, XFontStyle.Regular);

            XBrush vermelho = new XSolidBrush(XColor.FromArgb(220, 31, 28));
            XBrush preto = new XSolidBrush(XColor.FromArgb(0, 0, 0));


            // cria uma pagina em branco
            PdfPage page = document.AddPage();

            // Cria o objeto XGraphics para a pagina
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //insere o background de fundo
            XImage image = XImage.FromFile(Util.Url.GetCaminhoFisico("~/Arquivos/background.jpg"));

            ////tamanho original
            //double x = (250 - image.PixelWidth * 72 / image.HorizontalResolution) / 2;

            //coloca a imagem para ocupar toda a pagina
            gfx.DrawImage(image, 0, 0, page.Width, page.Height);


            //logo
            XRect rectLogo = new XRect(70, 90, 200, 110);
            gfx.DrawRectangle(XBrushes.BlueViolet, rectLogo);



            gfx.DrawString("Insira aqui seu texto", fontTitulo, vermelho,
              new XRect(300, 90, 300, 0),
              XStringFormats.TopLeft);

            String str = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit.Donec sollicitudin, lacus in fermentum consectetur, turpis tortor facilisis felis, tempor ultricies risus justo nec tellus. Donec mollis tempus massa, id fringilla est accumsan sit amet. Phasellus varius sollicitudin nisl, at viverra urna vulputate eu";

            XTextFormatter tf = new XTextFormatter(gfx);
            XRect rect = new XRect(300, 115, 250, 150);

            tf.DrawString(str, fontTextoTopo, preto, rect, XStringFormats.TopLeft);

            MemoryStream stream = new MemoryStream();
            document.Save(stream, false);

            Response.AddHeader("content-disposition",
              "attachment; filename=teste.pdf");

            return new FileStreamResult(stream, "application/pdf");
        }
    }
}