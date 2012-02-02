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
using Ionic.Zip;

namespace Controllers
{
    public class ProdutosController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Produtos, Log.EnumArea.Site);
        ProdutoRepository produtoRepository = new ProdutoRepository();
        DoutorRepository doutorRepository = new DoutorRepository();
        ProdutoLinhaRepository linhaRepository = new ProdutoLinhaRepository();
        EspecialidadeRepository especialidadeRepository = new EspecialidadeRepository();
        ProdutoVaRepository vaRepository = new ProdutoVaRepository();
        TerritorioRepository territorioRepository = new TerritorioRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();

        public DownloadResult Download(string idTerritorio, string token)
        {
            Response.ContentType = "application/zip";

            //verifica se territorio existe ou usuario com territorio Simulado existe
            var territorio = territorioRepository.GetTerritorio(idTerritorio);

            var usuario = usuarioRepository.GetUsuarios().FirstOrDefault(u => u.TerritorioSimulado == idTerritorio || u.Email == idTerritorio);

            var linha = linhaRepository.GetProdutoLinhas().FirstOrDefault(l => l.TerritorioSimulado == idTerritorio);

            if (territorio == null && usuario == null && linha == null)
                return null;

            //verifica se o territorio esta relacionado ao produto do va
            //if (!va.Produto.DoutorProdutos.Any(dp => dp.Doutor.IdTerritorio == territorio.Id))
            //    return null;

            if (token != Util.Sistema.GetTokenTerritorio(idTerritorio))
                return null;

            return new DownloadResult(Util.Sistema.AppSettings.Diretorios.CaminhoProdutosImagesZip, "_images.zip");
        }

        [AuthorizePermissao]
        public ActionResult Index()
        {
            logs.Add(Log.EnumTipo.Consulta, "Consultou os Produtos", Url.Action("Index"));

            ViewData["linhas"] = linhaRepository.GetProdutoLinhas().ToSelectList("Id", "Nome");

            return View();
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetProdutos()
        {
            IQueryable<Produto> produtos = null;

            produtos = produtoRepository.GetProdutos().OrderBy(p => p.Nome);

            string result = String.Empty;

            string modeloTabela = @"
                <table class='tabelaArquivos tabela tablesorter'>
                    <thead>
                        <tr>
                            <th style='width: 160px;'>
                            </th>
                            <th>
                                Produto
                            </th>
                            <th>
                                Linha
                            </th>
                            <th>
                                Status
                            </th>
                            <th>
                                Tem VA Ativo
                            </th>
                            <th class='acao'>&nbsp;</th>
                        </tr>
                    </thead>
                    <tbody class='arquivos'>
                                [conteudo]
                    </tbody>
                </table>".Replace("'", "\"");

            string modelo = @"
                <tr>
                    <td style='padding: 5px'>
                        <a href='javascript:trocaImagem([produto_id])'><img src='[imagem]'></a>
                    </td>
                    <td style='text-align: left'>
                        [produto]
                    </td>
                    <td style='text-align: left'>
                        [linha]
                    </td>
                    <td>
                        [status]
                    </td>
                    <td>
                        [va_ativo]
                    </td>
                    <td align='center'>
                        <a href='[url_detalhes]' class='editar'>Detalhes/Editar</a>
                    </td>
                </tr>".Replace("'", "\"");

            foreach (var produto in produtos)
            {
                result += modelo.ReplaceChaves(new {

                    produto = produto.Nome,
                    produto_id = produto.Id,
                    imagem = (produto.TemImagem)?produto.GetUrlImagemThumb():Url.Content("~/images/produto_sem_imagem.jpg"),
                    linha = produto.ProdutoLinha.Nome,
                    status = "Ativo",
                    va_ativo = (produto.ProdutoVas.Any(va => va.Status == (Char)ProdutoVa.EnumStatus.Ativo))?"Sim":"Não",
                    url_detalhes = Url.Action("index","produtosvas", new { id = produto.Id })

                });
            }

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(modeloTabela.ReplaceChaves(new { conteudo = result }));
        }

        private bool ExtensaoIsImagem(string extensao)
        {
            extensao = extensao.Replace(".", "").ToLower();
            return (extensao == "jpg" || extensao == "gif" || extensao == "png");
        }

        [HttpPost]
        [AuthorizePermissao]
        public ActionResult UploadProdutoImagem(HttpPostedFileBase fileData, int idProduto)
        {
            var produto = produtoRepository.GetProduto(idProduto);
            
            if (produto == null)
                return new EmptyResult();

            if (fileData == null)
                return new EmptyResult();

            String retorno = "";

            var tipo = fileData.ContentType;

            System.IO.FileInfo file = new FileInfo(fileData.FileName);

            if (!ExtensaoIsImagem(file.Extension))
            {
                return Content("Erro ao fazer o upload do arquivo: Formato inválido!");
            }

            var diretorioArmazenamento = Util.Sistema.AppSettings.Diretorios.DiretorioProdutosImages;
            Util.Arquivo.CreateDirectoryIfNotExists(Util.Url.GetCaminhoFisico(diretorioArmazenamento));

            var nomeArquivoOriginal = Path.GetFileNameWithoutExtension(fileData.FileName).RemoverAcentos().GerarUrlAmigavel().ToLower() + Path.GetExtension(fileData.FileName).ToLower();

            #region Imagem

            var caminhoDestino = Util.Url.GetCaminhoFisico(diretorioArmazenamento + produto.Id + "_thumb.jpg");

            //salva o upload
            fileData.SaveAs(caminhoDestino);

            Util.Imagem.Redimensionar(caminhoDestino, new Util.Imagem.Size(160, 120), manterProporcao: false);

            produto.TemImagem = true;

            #endregion

            produtoRepository.Save();

            #region gera/atualiza plist

                var produtos = produtoRepository.GetProdutos().Where(p=>p.TemImagem);

                var modelo = Util.Arquivo.LerArquivo(Util.Url.GetCaminhoFisico(Util.Sistema.AppSettings.Diretorios.DiretorioModelos + "/produtos/images/") + @"info.plist");

                //cria arquivo info.plist com todas as paginas
                FileStream fs = System.IO.File.Open(Util.Url.GetCaminhoFisico(diretorioArmazenamento + "info.plist"), FileMode.Create, FileAccess.Write);

                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                string conteudo = string.Empty;

                foreach (var p in produtos)
                {
                    conteudo += @"<dict>
									<key>ProductId</key>
									<string>" + p.Id + @"</string>
                                    <key>ImageName</key>
			                        <string>" + p.Id + "_thumb.jpg" + @"</string>
								</dict>";
                }

                conteudo = modelo.ReplaceChaves(new
                {
                    productsImages = conteudo
                });

                sw.Write(conteudo);

                sw.Close();
                sw.Dispose();

            #endregion

            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(Util.Url.GetCaminhoFisico(diretorioArmazenamento));

                zip.Save(Util.Url.GetCaminhoFisico(Util.Sistema.AppSettings.Diretorios.CaminhoProdutosImagesZip));
            }

            return Content("Imagem enviada com sucesso!");
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetProdutosTerritorio(string idTerritorio)
        {
            var territorio = new TerritorioRepository().GetTerritorio(idTerritorio);

            IQueryable<Produto> produtos = null;

            var especialidades = territorio.GetEspecialidades();

            string result = String.Empty;

            string modeloTabela = @"
                <table class='tabelaArquivos tabela tablesorter'>
                    <thead>
                        <tr>
                            <th />
                            <th>
                                Produto
                            </th>
                            <th>
                                Linha
                            </th>
                            <th>
                                Status
                            </th>
                            <th>
                                Tem VA Ativo
                            </th>
                            <th class='acao'>&nbsp;</th>
                        </tr>
                    </thead>
                    <tbody class='arquivos'>
                                [conteudo]
                    </tbody>
                </table>".Replace("'", "\"");

            string modelo = @"
                <tr>
                    <td style='text-align: left'>
                        [ordem]
                    </td>
                    <td style='text-align: left'>
                        [produto]
                    </td>
                    <td style='text-align: left'>
                        [linha]
                    </td>
                    <td>
                        [status]
                    </td>
                    <td>
                        [va_ativo]
                    </td>
                    <td align='center'>
                        <a href='[url_detalhes]' class='editar'>Detalhes/Editar</a>
                    </td>
                </tr>".Replace("'", "\"");

            foreach (var especialidade in especialidades)
            {
                produtos = territorio.GetProdutos(especialidade);

                var conteudoTabela = string.Empty;

                var cont = 1;
                foreach (var produto in produtos) { 

                    conteudoTabela += modelo.ReplaceChaves(new
                    {
                        ordem = cont++,
                        produto = produto.Nome,
                        linha = produto.ProdutoLinha.Nome,
                        status = "Ativo",
                        va_ativo = (produto.ProdutoVas.Any(va => va.Status == (Char)ProdutoVa.EnumStatus.Ativo)) ? "Sim" : "Não",
                        url_detalhes = Url.Action("index", "produtosvas", new { id = produto.Id })

                    });

                }

                result += "<h2>Especialidade: " +especialidade + "</h2><br>" + modeloTabela.ReplaceChaves(new
                {
                    conteudo = conteudoTabela
                });

            }

            logs.Add(Log.EnumTipo.Consulta, "Consultou os Produtos do Território "+territorio.Id, Url.Action("Index"));

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        [AuthorizePermissao]
        public ActionResult Grade()
        {

            logs.Add(Log.EnumTipo.Consulta, "Consultou a Grade dos Produtos", Url.Action("Grade"));

            ViewData["linhas"] = linhaRepository.GetProdutoLinhas().OrderBy(l => l.Nome).ToSelectList("Id","Nome");
            ViewData["especialidades"] = especialidadeRepository.GetEspecialidades().OrderBy(e => e.Nome).ToSelectList("Id","Nome");

            return View();
        }

        [HttpPost]
        [AuthorizePermissao("grade")]
        public ActionResult GradeAdicionar(int? idLinha, int? idEspecialidade)
        {
            if (idLinha == null || idEspecialidade == null)
                return new EmptyResult();

            var linha = linhaRepository.GetProdutoLinha(idLinha.Value);
            var especialidade = especialidadeRepository.GetEspecialidade(idEspecialidade.Value);

            if (linha == null || especialidade == null)
                return new EmptyResult();

            //verifica se ja existe doutor com a linha/especialidade
            var doutor = doutorRepository.GetDoutors().Where(d => d.DoutorEspecialidades.Any(e => e.Especialidade.Nome == especialidade.Nome) && d.ProdutoLinha.Nome == linha.Nome).FirstOrDefault();

            //se ja existe finaliza pois não é necessário adicionar
            if (doutor != null)
                return new EmptyResult();

            doutor = new Doutor();

            doutor.IdProdutoLinha = idLinha;

            doutorRepository.Add(doutor);
            doutorRepository.Save();

            doutor.AddEspecialidade(especialidade);

            return Content("[ok]");
        }

        [HttpPost]
        [AuthorizePermissao("grade")]
        public ActionResult GradeAdicionarProduto(Produto produtoEnviado)
        {
            var produto = new Produto();

            produto.Nome = produtoEnviado.Nome;
            produto.IdLinha = produtoEnviado.IdLinha;

            produtoRepository.Add(produto);
            produtoRepository.Save();

            logs.Add(Log.EnumTipo.Inclusao, "Incluiu o produto <i>" + produto.Nome + "</i>", Url.Action("Index"));

            return Content("[ok]");
        }

        [HttpPost]
        [AuthorizePermissao("grade")]
        public ActionResult SalvarGrade(string[] grade)
        {
            //formato de cada string grade: "[idDoutor]-[idProduto]"

            List<int> doutores = new List<int>();
            var cont = 1;

            foreach(var g in grade){
                if (g.Split('-').Count() > 1)
                {
                    int idDoutor, idProduto;

                    int.TryParse(g.Split('-')[0],out idDoutor);
                    int.TryParse(g.Split('-')[1], out idProduto);

                    var doutor = doutorRepository.GetDoutor(idDoutor);
                    var produto = produtoRepository.GetProduto(idProduto);

                    if (doutor == null)
                        continue;

                    if (!doutores.Contains(doutor.Id)) { 
                        //se ja nao foi limpo, limpa os produtos relacionados com aquele doutor especialista
                        doutorRepository.DeleteProdutos(doutor);
                        doutorRepository.Save();
                        doutores.Add(doutor.Id);
                    }

                    doutor.AddProduto(produto,cont++);
                }

            }

            //apaga doutores sem produtos relacionados
            doutorRepository.DeleteDoutoresSemProdutosRelacionados();
            doutorRepository.Save();

            return RedirectToAction("grade");
        }

        public string getComboProdutos(string select, Doutor doutor)
        {
            var produtos = produtoRepository.GetProdutos().OrderBy(p => p.Nome);
            var result = "<select name=\"grade\" style=\"display: none\">";
            result += "<option value=\"" + doutor.Id + "\">Nenhum</option>";

            foreach (var produto in produtos)
            {
                var selected = "";
                if (produto.Nome == select)
                {
                    selected = "selected";
                }

                result += "<option " + selected + " value=\"" + doutor.Id + "-"+ produto.Id +"\">";
                result += produto.Nome;
                result += "</option>";
            }

            result += "</select>";

            return result;
        }

        [AuthorizePermissao("grade")]
        public ActionResult GetGrade()
        {
            var doutores = doutorRepository.GetDoutors().OrderBy(d => d.ProdutoLinha.Nome);

            string result = String.Empty;

            string modeloTabela = Html("GetGradeTabela");
            string modelo = Html("GetGrade");

            foreach (var doutor in doutores)
            {
                string esp = doutor.DoutorEspecialidades.FirstOrDefault().Especialidade.Nome;
                string produtos = string.Empty;

                var cont = 1;
                foreach (var dp in doutor.DoutorProdutos.OrderBy(dp => dp.Orderm))
                {
                    produtos += "<td>" + getComboProdutos(dp.Produto.Nome, doutor) + "<span>" + dp.Produto.Nome + "</span>" + "</td>";
                    cont++;
                }

                while (cont <= 10)
                {
                    produtos += "<td>" + getComboProdutos("", doutor) + "<span /></td>";
                    cont++;
                }

                result += modelo.ReplaceChaves(new
                {

                    linha = doutor.ProdutoLinha.Nome,
                    especialidade = esp,
                    produtos = produtos

                });
            }

            result = modeloTabela.ReplaceChaves(new
            {

                conteudo = result

            });

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        
    }
}
