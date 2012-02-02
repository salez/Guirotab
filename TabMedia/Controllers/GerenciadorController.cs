using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Models;
using System.Reflection;

namespace Controllers
{
    public class GerenciadorController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.GerenciadorDeArquivos, Log.EnumArea.Site);
        GerenciadorArquivoRepository arquivoRepository = new GerenciadorArquivoRepository();

        String diretorioArquivos = "~/Uploads/Gerenciador/";

        [AuthorizePermissao]
        public ActionResult Index()
        {
            logs.Add(Log.EnumTipo.Consulta, "Consultou o Gerenciador de Arquivos", Url.Action("index"));

            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        private bool ExtensaoIsPdf(string extensao)
        {
            return (extensao.Replace(".", "").ToLower() == "pdf");
        }

        private bool ExtensaoIsMov(string extensao)
        {
            return (extensao.Replace(".", "").ToLower() == "mov");
        }

        [HttpPost]
        [AuthorizePermissao]
        public ActionResult UploadArquivo(HttpPostedFileBase fileData)
        {
            var extensao = Path.GetExtension(fileData.FileName).Replace(".",String.Empty);

            GerenciadorArquivo arquivo = new GerenciadorArquivo();

            arquivo.Tipo = extensao.ToLower();
            arquivo.Nome = fileData.FileName;

            arquivoRepository.Add(arquivo);
            arquivoRepository.Save();

            var diretorioDestino = diretorioArquivos;
            var caminhoDestino = Util.Url.GetCaminhoFisico(diretorioDestino + fileData.FileName);

            fileData.SaveAs(caminhoDestino);

            logs.Add(Log.EnumTipo.Inclusao, "Enviou um arquivo: '" + arquivo.Nome + "'", Url.Action("index"), arquivo.Id);

            return Content("Arquivo enviado com sucesso!");
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetArquivos()
        {
            var arquivos = arquivoRepository.GetGerenciadorArquivos();
            string result = String.Empty;

            string modelo = @"
                <tr class='even'>
                    <td align='center'>
                        [DataInclusao]
                    </td>
                    <td>
                        [Nome]
                    </td>
                    <td>
                        [Tipo]
                    </td>
                    <td align='center'>
                        <a href='[url_download]' target='_blank' class='visualizar'>Download</a> 
                        <a href='[url_excluir]' class='excluir'>Excluir</a>
                    </td>
                </tr>".Replace("'", "\"");

            foreach (var arquivo in arquivos)
            {
                result += modelo.ReplaceChaves(new { 

                    DataInclusao = arquivo.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno),
                    Nome = arquivo.Nome,
                    Tipo = arquivo.Tipo,
                    url_download =  Url.Action("download", new { id = arquivo.Id }),
                    url_excluir = "javascript:deletar(" + arquivo.Id + ",'" + arquivo.Nome + "')"

                });
            }

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        [HttpPost]
        [AuthorizePermissao]
        public void ExcluiArquivo(int id)
        {
            var arquivo = arquivoRepository.GetGerenciadorArquivo(id);

            if (arquivo == null)
                return;

            arquivoRepository.Delete(arquivo);
            arquivoRepository.Save();

            System.IO.File.Delete(Util.Url.GetCaminhoFisico(diretorioArquivos + arquivo.Nome));

            logs.Add(Log.EnumTipo.Exclusao, "Apagou um arquivo: '" + arquivo.Nome + "'", Url.Action("index"), arquivo.Id);
        }

        [AuthorizePermissao]
        public ActionResult Download(int id)
        {
            var arquivo = arquivoRepository.GetGerenciadorArquivo(id);

            logs.Add(Log.EnumTipo.Consulta, "Fez download do arquivo: '" + arquivo.Nome + "'", Url.Action("index"), arquivo.Id);

            return new DownloadResult(diretorioArquivos + arquivo.Nome, Path.GetFileNameWithoutExtension(arquivo.Nome).GerarUrlAmigavel() + Path.GetExtension(arquivo.Nome));
        }
    }
}
