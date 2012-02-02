using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Models;
using System.Runtime.InteropServices;
using System.Net.Mail;
using System.Web.UI.WebControls;
using Ionic.Zip;
using VideoEncoder;
using System.Diagnostics;
using System.Data.SqlTypes;
using HtmlAgilityPack;

namespace Controllers
{
    public class ProdutosVasController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.ProdutosVas, Log.EnumArea.Site);
        public const string chaveEncriptacao = "g5u7i8555";

        ProdutoRepository produtoRepository = new ProdutoRepository();
        ProdutoVaRepository vaRepository = new ProdutoVaRepository();
        ProdutoVaArquivoRepository arquivoRepository = new ProdutoVaArquivoRepository();
        ProdutoVaSlideRepository slideRepository = new ProdutoVaSlideRepository();
        ProdutoVaSlideArquivoRepository arquivoSlideRepository = new ProdutoVaSlideArquivoRepository();
        ProdutoVaComentarioRepository comentarioRepository = new ProdutoVaComentarioRepository();
        TerritorioRepository territorioRepository = new TerritorioRepository();
        DoutorRepository doutorRepository = new DoutorRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        ProdutoLinhaRepository linhaRepository = new ProdutoLinhaRepository();
        ProdutoVaCategoriaRepository categoriaRepository = new ProdutoVaCategoriaRepository();

        public DownloadResult Download(int idVa, string idTerritorio, string tokenTerritorio)
        {
            Response.ContentType = "application/zip";

            var va = vaRepository.GetProdutoVaSemVerificacaoUsuario(idVa);

            if (va == null)
                return null;

            //verifica se territorio existe ou usuario com territorio Simulado existe
            var territorio = territorioRepository.GetTerritorio(idTerritorio);

            var usuario = usuarioRepository.GetUsuarios().FirstOrDefault(u => u.TerritorioSimulado == idTerritorio);

            var linha = linhaRepository.GetProdutoLinhas().FirstOrDefault(l => l.TerritorioSimulado == idTerritorio);

            if (territorio == null && usuario == null && linha == null)
                return null;

            //verifica se o territorio esta relacionado ao produto do va
            //if (!va.Produto.DoutorProdutos.Any(dp => dp.Doutor.IdTerritorio == territorio.Id))
            //    return null;

            //verifica o token
            if (tokenTerritorio != Util.Sistema.GetTokenTerritorio(idTerritorio))
                return null;

            if (territorio != null) { 
                //grava que a solicitação de download foi feita para futuros relatórios (apenas para territorios, nao para agencias e gerentes)
                va.GravaDownload(territorio.Id);
            }

            return new DownloadResult(va.GetCaminhoVAZipado(), va.GetNomeVAZipado());
        }

        #region funções auxiliares

        private bool ExtensaoIsPdf(string extensao)
        {
            return (extensao.Replace(".", "").ToLower() == "pdf");
        }

        private bool ExtensaoIsMp4(string extensao)
        {
            return (extensao.Replace(".", "").ToLower() == "mp4");
        }

        private bool ExtensaoIsZip(string extensao)
        {
            return (extensao.Replace(".", "").ToLower() == "zip");
        }

        private bool ExtensaoIsImagem(string extensao)
        {
            extensao = extensao.Replace(".", "").ToLower();
            return (extensao == "jpg" || extensao == "gif" || extensao == "png");
        }

        private bool ValidaTamanhoPaginas(iTextSharp.text.pdf.PdfReader reader)
        {
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                if (reader.GetPageSize(i).Width != Util.Sistema.AppSettings.CampanhaResolucaoLargura || reader.GetPageSize(i).Height != Util.Sistema.AppSettings.CampanhaResolucaoAltura)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Index - lista de VA's

        [AuthorizePermissao]
        public ActionResult Index(int? id) //id produto
        {
            if (!id.HasValue)
                return RedirectToAction("index", "produtos");

            var produto = produtoRepository.GetProduto(id.Value);

            ViewData["categorias"] = categoriaRepository.GetProdutoVaCategorias();

            logs.Add(Log.EnumTipo.Consulta, "Consultou os VA's do Produto '" + produto.Nome + "'", Url.Action("Index", new { id = produto.Id }));

            return View(produto);
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetVas(int? id, int? idcategoria) //id produto
        {
            if (!id.HasValue)
                return RedirectToAction("index", "agenciaprodutos");

            var produto = produtoRepository.GetProduto(id.Value);

            if (produto == null)
                return new EmptyResult();

            var vas = produto.ProdutoVas.Where(va => va.IdCategoria == idcategoria).OrderByDescending(va => va.DataInclusao);

            var produtos = produtoRepository.GetProdutos();
            string result = String.Empty;

            string modelo = Html("GetVas");
            
            foreach (var va in vas)
            {
                result += modelo.ReplaceChaves(new
                {
                    data = va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno),
                    qtde_slides = va.ProdutoVaSlides.Count().ToString(),
                    versao = va.Versao.ToString(),
                    status = va.GetStatus(),
                    nome = va.Nome,
                    link_publicar_aprovar = "<a href='#'>" + "Publicar" + "</a>",
                    url_visualizar = Url.Action("visualizar", new { id = va.Id }),
                    url_duplicar = ((ProdutoVa.EnumStatus)va.Status != ProdutoVa.EnumStatus.Temporario) ? Url.Action("cadastro", new { id = va.Produto.Id, idVa = va.Id }) : string.Empty,
                    url_download = va.GetUrlDownload(),
                    url_desativar = (Sessao.Site.UsuarioInfo.IsGerenteProduto() || Sessao.Site.UsuarioInfo.IsGerenteMarketing() || Sessao.Site.UsuarioInfo.IsAdministrador())?Url.Action("DesativarVA", new { idVa = va.Id }):string.Empty,
                    editar = (va.ValidoParaEdicao()) ? "" + Url.Action("editar", new { idVa = va.Id, idProduto = id }) : string.Empty,
                    excluir = (va.ValidoParaEdicao()) ? "excluirVa('" + va.Id + "', '" + va.DataInclusao.Value.Formata(Util.Data.FormatoData.DiaMesAno) + "')" : string.Empty
                });
            }

            return Content(result);
        }

        [AuthorizePermissao]
        public ActionResult DesativarVa(int idVa)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return RedirectToAction("index","produtos");

            va.Status = (char)ProdutoVa.EnumStatus.Inativo;

            vaRepository.Save();

            logs.Add(Log.EnumTipo.Exclusao, "Desativou um VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));

            return RedirectToAction("index", new { id = va.IdProduto });
        }

        [HttpPost]
        [AuthorizePermissao]
        public void ExcluirVA(int idVa, int idProduto)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null || va.IdProduto != idProduto || !va.ValidoParaEdicao())
                return;

            vaRepository.Delete(va);
            vaRepository.Save();

            //exclui diretorio do VA
            va.ExcluirDiretorio();

            logs.Add(Log.EnumTipo.Exclusao, "Excluiu um VA (data: "+va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno)+") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));
        }

        #endregion

        #region Visualizar VA

        [AuthorizePermissao]
        public ActionResult Visualizar(int? id) //id va
        {
            if (!id.HasValue)
                return RedirectToAction("index", "produtos");

            var va = vaRepository.GetProdutoVa(id.Value);

            if (va == null)
                return RedirectToAction("index", "produtos");

            logs.Add(Log.EnumTipo.Consulta, "Visualizou o VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));

            return View(va);
        }

        [AuthorizePermissao("visualizar")]
        public ActionResult VisualizarGetHtmlVa(int? idVa) //id va
        {
            if (!idVa.HasValue)
                return new EmptyResult();

            var va = vaRepository.GetProdutoVa(idVa.Value);

            if (va == null)
                return new EmptyResult();

            return Content(va.GetHTMLVisualizacaoVA());
        }

        [HttpGet]
        [AuthorizePermissao("visualizar")]
        public ActionResult VisualizarGetArquivos(int idVa)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            var arquivos = va.ProdutoVaArquivos;

            string result = String.Empty;

            string modelo = @"
                <tr class='even'>
                    <td>
                        <a href='[url_arquivo]' target='_blank'>[nome]</a>
                    </td>
                    <td>
                        [tipo]
                    </td>
                    <td>
                        [data]
                    </td>
                    <td align='center'>
                        <a href='[url_arquivo]' target='_blank'>Visualizar</a>
                    </td>
                </tr>".Replace("'", "\"");

            foreach (var arquivo in arquivos)
            {
                result += modelo.ReplaceChaves(new
                {

                    data = arquivo.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno),
                    url_arquivo = arquivo.GetCaminho().ResolveURL(),
                    nome = arquivo.Nome,
                    tipo = (ProdutoVaArquivo.EnumTipo)arquivo.Tipo
                });
            }

            return Content(result);
        }

        [HttpGet]
        [AuthorizePermissao("visualizar")]
        public ActionResult VisualizarGetComentarios(int idVa)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            var comentarios = va.ProdutoVaComentarios;

            string result = String.Empty;

            string modelo = @"
                <li>
                    <div class='header'><b>[nome_usuario]</b> ([data_mensagem]):</div> [mensagem]
                </li>".Replace("'", "\"");

            foreach (var comentario in comentarios)
            {
                result += modelo.ReplaceChaves(new
                {
                    nome_usuario = comentario.Usuario.Nome,
                    data_mensagem = comentario.Datainclusao.Formata(Util.Data.FormatoData.DiaMesAnoHoraMinuto),
                    mensagem = comentario.Descricao
                });
            }

            if (comentarios.Count() == 0)
                return Content("<center>Não há comentários.</center>");

            return Content("<ul>" + result + "</ul>");
        }

        [HttpPost]
        [AuthorizePermissao("visualizar")]
        public void VisualizarComentar(int idVa, string mensagem)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return;

            if (mensagem.Length > 500)
                ModelState.AddModelError("mensagem", "Mensagem não pode ter mais que 500 caracteres.");

            if (ModelState.IsValid)
            {
                ProdutoVaComentario comentario = new ProdutoVaComentario();

                comentario.IdUsuario = Sessao.Site.UsuarioInfo.Id;
                comentario.IdVa = va.Id;
                comentario.Descricao = mensagem;

                comentarioRepository.Add(comentario);
                comentarioRepository.Save();
            }

            logs.Add(Log.EnumTipo.Inclusao, "Comentou no VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));
        }

        [HttpPost]
        [AuthorizePermissao("visualizar")]
        public void AprovarVA(int idVa)
        {
            if (Autenticacao.AutorizaPermissao("aprovar", "produtosvas"))
            {
                var va = vaRepository.GetProdutoVa(idVa);

                if (va == null)
                    return;

                if (ModelState.IsValid)
                {
                    va.Aprovar();

                    vaRepository.Save();

                    logs.Add(Log.EnumTipo.Alteracao, "Aprovou o VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));
                }
            }
        }

        [HttpPost]
        [AuthorizePermissao("visualizar")]
        public void PublicarVA(int idVa)
        {
            if (Autenticacao.AutorizaPermissao("publicar", "produtosvas"))
            {
                var va = vaRepository.GetProdutoVa(idVa);

                if (va == null)
                    return;

                if (ModelState.IsValid)
                {
                    if (va.Status == (char)ProdutoVa.EnumStatus.Aprovado && va.StatusGM == (char)ProdutoVa.EnumStatus.Aprovado)
                    {

                        va.Publicar(vaRepository);

                        logs.Add(Log.EnumTipo.Alteracao, "Publicou o VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));
                    }
                }
            }
        }

        [HttpPost]
        [AuthorizePermissao("visualizar")]
        public void ReprovarVA(int idVa)
        {
            if (Autenticacao.AutorizaPermissao("aprovar", "produtosvas"))
            {
                var va = vaRepository.GetProdutoVa(idVa);

                if (va == null)
                    return;

                if (ModelState.IsValid)
                {
                    va.Reprovar();
                    vaRepository.Save();

                    logs.Add(Log.EnumTipo.Alteracao, "Reprovou o VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));
                }
            }
        }

        [HttpPost]
        [AuthorizePermissao("visualizar")]
        public void AprovarEPublicarVA(int idVa)
        {
            if (Autenticacao.AutorizaPermissao("aprovar", "produtosvas") && Autenticacao.AutorizaPermissao("publicar", "produtosvas"))
            {
                this.AprovarVA(idVa);
                this.PublicarVA(idVa);
            }
        }

        #endregion

        #region cadastro / edição

        [AuthorizePermissao]
        public ActionResult Cadastro(int? id, int? idVa, int? idCategoria) //id produto
        {
            if (!id.HasValue)
                return RedirectToAction("index", "produtos");

            var produto = produtoRepository.GetProduto(id.Value);

            if (produto == null)
                return RedirectToAction("index", "produtos");

            ProdutoVaCategoria categoria = null;

            if (idCategoria != null) { 

                categoria = categoriaRepository.GetProdutoVaCategoria(idCategoria.Value);

                if (categoria == null)
                    return RedirectToAction("index", "produtos");

            }

            ProdutoVa va = null;

            if (idVa.HasValue)
            {
                //faz uma cópia do VA informado
                ProdutoVa vaCopiar = vaRepository.GetProdutoVa(idVa.Value);

                va = vaCopiar.GerarCopia();

                logs.Add(Log.EnumTipo.Inclusao, "Duplicou o VA (data: " + vaCopiar.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + vaCopiar.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));

                return RedirectToAction("editar", new { idVa = va.Id, idProduto = produto.Id });
            }
            else
            {
                if (categoria == null)
                    return RedirectToAction("index", "produtos");

                //verifica se há algum va temporario
                va = vaRepository.GetProdutoVaTemporario(Sessao.Site.UsuarioInfo.Id, produto.Id, categoria.Id);
            }

            if (va == null)
            { //nao estava criando nenhum VA, cria um novo

                va = new ProdutoVa();
                va.IdProduto = produto.Id;
                va.IdUsuario = Sessao.Site.UsuarioInfo.Id;
                va.Status = (char)ProdutoVa.EnumStatus.Temporario;
                va.IdCategoria = categoria.Id;

                vaRepository.Add(va);
                vaRepository.Save();
            }

            //cria pasta para o produto caso não exista
            produto.CriaDiretoriosBase();

            //cria pastas para o VA caso não exista
            va.CriaDiretoriosBase();

            return View(va);
        }

        [AuthorizePermissao("cadastro")]
        public ActionResult Editar(int? idVa, int? idProduto)
        {
            if (!idVa.HasValue || !idProduto.HasValue)
                return RedirectToAction("index","produtos");

            var va = vaRepository.GetProdutoVa(idVa.Value);

            //verifica se va existe e que pertence ao produto informado
            if (va == null || va.IdProduto != idProduto.Value)
                return RedirectToAction("index", "produtos");
            
            //verifica se o VA está aprovado ou ativo, se o usuario não tiver a permissão de aprovação, ele não pode editar o VA aprovado ou ativo.
            if(!va.ValidoParaEdicao())
                return RedirectToAction("index", "produtos");

            logs.Add(Log.EnumTipo.Consulta, "Entrou na edição do VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));

            return View("Cadastro", va);
        }

        public ActionResult ValidaVa(string nome, string descricao, string palavrasChave, DateTime? limiteVeiculacao)
        {
            if (nome.IsNullOrEmpty())
                ModelState.AddModelError("nome", "Nome obrigatório.");

            if (descricao.IsNullOrEmpty())
                ModelState.AddModelError("descricao", "Descrição obrigatória.");

            if (nome != null && nome.Length > 100)
                ModelState.AddModelError("nome", "Nome pode conter no máximo 100 caracteres.");

            if (descricao != null && nome.Length > 500)
                ModelState.AddModelError("descricao", "Descrição pode conter no máximo 500 caracteres.");

            if (palavrasChave != null && nome.Length > 100)
                ModelState.AddModelError("palavrasChave", "Palavras Chave pode conter no máximo 500 caracteres.");

            if (limiteVeiculacao != null && limiteVeiculacao.Value < SqlDateTime.MinValue.Value)
            {
                ModelState.AddModelError("DataLimiteVeiculacao", "Data Limite de veiculação inválida.");
            }

            if (ModelState.IsValid)
            {
                return Content("[ok]");
            }
            else
            {
                var result = string.Empty;

                foreach(var k in ModelState.Values.Select(v=> v.Errors)){
                    foreach (var e in k) { 
                    result+=e.ErrorMessage;
                    }
                }

                return Content("erro:" + result);
            }
        }

        public void PreencheVa(ProdutoVa va, string nome, string descricao, string palavrasChave, DateTime? limiteVeiculacao)
        {
            if (nome != null)
            {
                va.Nome = nome;
            }

            if (descricao != null)
            {
                va.Descricao = descricao;
            }

            if (palavrasChave != null)
            {
                va.PalavrasChave = palavrasChave;
            }

            va.DataLimiteVeiculacao = limiteVeiculacao;
        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public void CadastroSalvar(int idVa, string nome, string descricao, string palavrasChave, bool enviarAprovacao, DateTime? limiteVeiculacao)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return;

            //verifica se o VA está aprovado ou ativo, se o usuario não tiver a permissão de aprovação, ele não pode editar o VA aprovado ou ativo.
            if (!va.ValidoParaEdicao())
                return;

            ValidaVa(nome, descricao, palavrasChave, limiteVeiculacao); // faz a validação das informações passadas

            if (ModelState.IsValid) {

                PreencheVa(va, nome, descricao, palavrasChave, limiteVeiculacao);

                if (enviarAprovacao) {
                    va.Aprovar();
                }
                else if (va.Status == (char)ProdutoVa.EnumStatus.Temporario) { 
                    va.Status = (char)ProdutoVa.EnumStatus.Inativo;
                }

                //caso seja gerente, não altera o status do va, se ele estiver aprovado continuará aprovado.

                vaRepository.Save();

                logs.Add(Log.EnumTipo.Alteracao, "Alterou o VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));

            }
        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public void CadastroSalvarEPublicar(int idVa, string nome, string descricao, string palavrasChave, DateTime? limiteVeiculacao)
        {
            if (Autenticacao.AutorizaPermissao("publicar", "produtosvas"))
            {
                var va = vaRepository.GetProdutoVa(idVa);

                if (va == null)
                    return;

                //verifica se o VA está aprovado ou ativo, se o usuario não tiver a permissão de aprovação, ele não pode editar o VA aprovado ou ativo.
                if (!va.ValidoParaEdicao())
                    return;

                ValidaVa(nome, descricao, palavrasChave, limiteVeiculacao); // faz a validação das informações passadas

                if (ModelState.IsValid)
                {

                    PreencheVa(va, nome, descricao, palavrasChave, limiteVeiculacao);

                    va.Aprovar();

                    //verifica se está aprovado pelo gerente de produto e marketing
                    if(va.IsAprovado()){

                        vaRepository.Save();
                        va.Publicar(vaRepository);

                        logs.Add(Log.EnumTipo.Alteracao, "Publicou o VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));

                    }
                }
            }
        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public void CadastroSalvarETestar(int idVa, string nome, string descricao, string palavrasChave, DateTime? limiteVeiculacao)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return;

            //verifica se o VA está aprovado ou ativo, se o usuario não tiver a permissão de aprovação, ele não pode editar o VA aprovado ou ativo.
            if (!va.ValidoParaEdicao())
                return;

            ValidaVa(nome, descricao, palavrasChave, limiteVeiculacao); // faz a validação das informações passadas

            if (ModelState.IsValid)
            {
                PreencheVa(va, nome, descricao, palavrasChave, limiteVeiculacao); // preenche o va com as informações

                vaRepository.Save();
                va.ColocarEmTeste(vaRepository);

                logs.Add(Log.EnumTipo.Alteracao, "Colocou em Teste o VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));
            }
        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public ActionResult CadastroGerarZip(int idVa)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return Content("erro:VA não encontrado");

            if (!va.ValidoParaEdicao())
                return Content("erro:Sem permissão para editar o VA.");

            try
            {
                va.CriaArquivosZip();
            }
            catch(Exception e)
            {
                Util.Sistema.Error.TrataErro(e, Request);

                return Content("erro:"+e.Message.HtmlEncode());
            }

            return Content("ok");
        }

        [HttpGet]
        [AuthorizePermissao("cadastro")]
        public ActionResult CadastroPreVisualizar(int idVa) //id slide
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return new EmptyResult();

            ViewData["conteudo"] = va.GetHTMLVisualizacaoVA(Util.Sistema.AppSettings.CampanhaResolucaoLargura,Util.Sistema.AppSettings.CampanhaResolucaoAltura);

            return View();
        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public void AtualizaLimiteVeiculacao(int idVa, DateTime? limiteVeiculacao) //id slide
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return;

            if (!va.ValidoParaEdicao())
                return;

            va.DataLimiteVeiculacao = limiteVeiculacao;
            vaRepository.Save();
        }

        #region Slides
        
        [HttpGet]
        [AuthorizePermissao("cadastro")]
        public ActionResult GetSlides(int idVa, bool farmacia = false) //id va
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return new EmptyResult();

            // Initialize some variables.
            String result = String.Empty;

            string modelo = Html("GetSlides");

            if (farmacia)
            {
                modelo = Html("GetFarmacias");
            }
            //? <select><option>Transição 1</option><option>Transição 2</option><option>Transição 3</option><option>Transição 4</option></select>

            var cont = 1;

            IEnumerable<ProdutoVaSlide> slides;

            if (farmacia)
            {
                slides = va.ProdutoVaSlides.Where(s => s.IsFarmacia());
            }
            else
            {
                slides = va.ProdutoVaSlides.Where(s => !s.IsFarmacia());
            }
            
            foreach (var slide in slides.OrderBy(s => s.Ordem))
            {
                var arquivo = slide.ProdutoVaSlideArquivos.First();

                var especialidades = slide.ProdutoVaSlideEspecialidades.Select(se => se.Especialidade);

                var strEspecialidades = "Nenhum";

                foreach(var especialidade in especialidades){

                    if (strEspecialidades == "Nenhum")
                        strEspecialidades = "";

                    strEspecialidades += especialidade.Nome + " ";
                }

                string url_imagem = string.Empty;
                string url_imagem_thumb = string.Empty;
                string video = string.Empty;
                string add_video = string.Empty;

                if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Farmacia) //IMAGEM
                {
                    url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo());
                    url_imagem_thumb = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb));
                }
                else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg) //IMAGEM
                {
                    url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo());
                    url_imagem_thumb = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb));
                }
                else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Mp4) //VIDEO
                {
                    url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo());
                    url_imagem_thumb = Util.Url.ResolveUrl("~/images/slide_video.png");
                    video = (arquivo.VideoAutoPlay.HasValue && arquivo.VideoAutoPlay.Value) ? " value=\"" + slide.Id + "\" checked=\"checked\"" : "value=\"" + slide.Id + "\" ";
                    add_video = "width=100";
                }
                else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Zip) //HTML
                {
                    url_imagem = string.Empty;
                    url_imagem_thumb = Util.Url.ResolveUrl("~/images/slide_html.png");
                }

                result += modelo.ReplaceChaves(new
                {
                    id = slide.Id,
                    cont = cont,
                    especialidades = strEspecialidades,
                    url_imagem = url_imagem,
                    url_imagem_thumb = url_imagem_thumb,
                    url_excluir = "javascript:excluirSlide('" + slide.Id + "')",
                    video = video,
                    add_video = add_video
                });

                cont++;
            }

            return Content(result);
        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public ActionResult GetSlideEspecialidades(int idSlide) //id slide
        {
            var slide = slideRepository.GetProdutoVaSlide(idSlide);

            if (slide == null || !slide.ProdutoVa.ValidoParaEdicao())
                return new EmptyResult();

            // Initialize some variables.
            String result = String.Empty;

            var especialidades = slide.ProdutoVa.GetEspecialidades();           

            result = "<input type='hidden' name='IdSlide' value='"+slide.Id+"'>";

            foreach(var especialidade in especialidades){

                var strChecked = string.Empty;

                if (slide.ProdutoVaSlideEspecialidades.Select(e => e.Especialidade.Id).Contains(especialidade.Id))
                    strChecked = "checked='checked'";

                result += "<br /><label><input type='checkbox' name='IdsEspecialidade' value='" + especialidade.Id + "' " + strChecked + " /> " + especialidade.Nome + " </label>";
                
            }

            return Content(result);
        }

        public void CriaSlideImagem(ProdutoVaSlide slide, List<ProdutoVaSlide> slides, int idVa, bool farmacia, string diretorioArmazenamento, string nomeArquivoOriginal, string retorno, HttpPostedFileBase fileData)
        {
            CriaSlideImagem(slide, slides, idVa, farmacia, diretorioArmazenamento, nomeArquivoOriginal, retorno, fileData, null);
        }

        public void CriaSlideImagem(ProdutoVaSlide slide, List<ProdutoVaSlide> slides, int idVa, bool farmacia, string diretorioArmazenamento, string nomeArquivoOriginal, string retorno, ZipEntry zipEntry)
        {
            CriaSlideImagem(slide, slides, idVa, farmacia, diretorioArmazenamento, nomeArquivoOriginal, retorno, null, zipEntry);
        }

        public void CriaSlideImagem(ProdutoVaSlide slide, List<ProdutoVaSlide> slides, int idVa, bool farmacia, string diretorioArmazenamento, string nomeArquivoOriginal, string retorno, HttpPostedFileBase fileData, ZipEntry zipEntry)
        {
        
            #region Imagem

                //var CaminhoArquivoSlide = caminhoDestino.Replace("%d", i.ToString());

                //grava slide e arquivo no banco

                slide.IdVa = idVa;

                slideRepository.Add(slide);
                slideRepository.Save();

                ProdutoVaSlideArquivo arquivo = new ProdutoVaSlideArquivo();
                arquivo.IdSlide = slide.Id;

                if (farmacia)
                {
                    //se for slide das farmacias grava como tipo farmacia
                    arquivo.Tipo = (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Farmacia;
                }
                else { 
                    arquivo.Tipo = (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg;
                }

                arquivoSlideRepository.Add(arquivo);
                arquivoSlideRepository.Save();

                var caminhoDestino = Util.Url.GetCaminhoFisico(diretorioArmazenamento + arquivo.Id + ".jpg");
                
                if (fileData != null)
                {
                    fileData.SaveAs(caminhoDestino);
                }
                else
                {
                    zipEntry.Extract(Path.GetDirectoryName(caminhoDestino));
                    System.IO.File.Move(Path.GetDirectoryName(caminhoDestino) + "\\" + zipEntry.FileName, caminhoDestino);
                    System.IO.Directory.Delete(Path.GetDirectoryName(Path.GetDirectoryName(caminhoDestino) + "\\" + zipEntry.FileName), true);
                }

                //atualiza nome do arquivo no banco
                arquivo.Nome = nomeArquivoOriginal;

                arquivoSlideRepository.Save();

                //redimensiona a imagem para 1024x768
                Util.Imagem.Redimensionar(caminhoDestino, new Util.Imagem.Size(Util.Sistema.AppSettings.CampanhaResolucaoLargura, Util.Sistema.AppSettings.CampanhaResolucaoAltura), Util.Imagem.EnumFormato.JPEG, false);
                //gera miniatura
                Util.Imagem.Redimensionar(caminhoDestino, Path.GetDirectoryName(caminhoDestino) + "\\" + Path.GetFileNameWithoutExtension(caminhoDestino) + "_pq" + Path.GetExtension(caminhoDestino), new Util.Imagem.Size(133, 100), Util.Imagem.EnumFormato.JPEG, false);

                slides.Add(slide);

            #endregion

        }

        public void CriaSlideVideo(ProdutoVaSlide slide, List<ProdutoVaSlide> slides, int idVa, bool farmacia, string diretorioArmazenamento, string nomeArquivoOriginal, string retorno, HttpPostedFileBase fileData)
        {
            CriaSlideVideo(slide, slides, idVa, farmacia, diretorioArmazenamento, nomeArquivoOriginal, retorno, fileData, null);
        }

        public void CriaSlideVideo(ProdutoVaSlide slide, List<ProdutoVaSlide> slides, int idVa, bool farmacia, string diretorioArmazenamento, string nomeArquivoOriginal, string retorno, ZipEntry zipEntry)
        {
            CriaSlideVideo(slide, slides, idVa, farmacia, diretorioArmazenamento, nomeArquivoOriginal, retorno, null, zipEntry);
        }

        public void CriaSlideVideo(ProdutoVaSlide slide, List<ProdutoVaSlide> slides, int idVa, bool farmacia, string diretorioArmazenamento, string nomeArquivoOriginal, string retorno, HttpPostedFileBase fileData, ZipEntry zipEntry)
        {
            #region mp4

            //grava slide e arquivo no banco

            slide.IdVa = idVa;

            slideRepository.Add(slide);
            slideRepository.Save();

            ProdutoVaSlideArquivo arquivo = new ProdutoVaSlideArquivo();
            arquivo.IdSlide = slide.Id;
            arquivo.Tipo = (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Mp4;
            arquivo.Nome = nomeArquivoOriginal;

            arquivoSlideRepository.Add(arquivo);
            arquivoSlideRepository.Save();

            var caminhoDestino = Util.Url.GetCaminhoFisico(diretorioArmazenamento + arquivo.Id + ".mp4");

            if (fileData != null)
            {
                fileData.SaveAs(caminhoDestino);
            }
            else
            {
                zipEntry.Extract(Path.GetDirectoryName(caminhoDestino));
                System.IO.File.Move(Path.GetDirectoryName(caminhoDestino) + "\\" + zipEntry.FileName, caminhoDestino);
                System.IO.Directory.Delete(Path.GetDirectoryName(Path.GetDirectoryName(caminhoDestino) + "\\" + zipEntry.FileName), true);
            }

            retorno = "Arquivo enviado com sucesso! 1 vídeo adicionado.";

            #endregion

            #region GERA IMAGEM A PARTIR DO VIDEO

            Encoder enc = new Encoder();
            enc.FFmpegPath = Server.MapPath(Util.Sistema.AppSettings.UrlFFMpeg);
            VideoFile inputv = new VideoFile(caminhoDestino);
            //string outputVPath = Server.MapPath("~/WorkingFolder/OutputVideo.flv");
            string saveImageTo = Util.Url.GetCaminhoFisico(diretorioArmazenamento + arquivo.Id + ".jpg");

            // to get video thumbnail call
            enc.GetVideoThumbnail(inputv, saveImageTo, Util.Sistema.AppSettings.CampanhaResolucaoLargura + "x" + Util.Sistema.AppSettings.CampanhaResolucaoAltura, 0);

            //gera miniatura
            Util.Imagem.Redimensionar(saveImageTo, Path.GetDirectoryName(saveImageTo) + "\\" + Path.GetFileNameWithoutExtension(saveImageTo) + "_pq" + Path.GetExtension(saveImageTo), new Util.Imagem.Size(133, 100), Util.Imagem.EnumFormato.JPEG, false);

            #endregion

            slides.Add(slide);

        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public ActionResult UploadSlide(HttpPostedFileBase fileData, int idVa, bool farmacia = false)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return new EmptyResult();

            if (!va.ValidoParaEdicao() || va.ProdutoVaCategoria.Tipo != (char)ProdutoVaCategoria.EnumTipo.Apresentacao)
                return new EmptyResult();

            if (fileData == null)
                return new EmptyResult();

            String retorno = "";

            var tipo = fileData.ContentType;

            System.IO.FileInfo file = new FileInfo(fileData.FileName);

            if (!ExtensaoIsPdf(file.Extension) && !ExtensaoIsMp4(file.Extension) && !ExtensaoIsZip(file.Extension) && !ExtensaoIsImagem(file.Extension))
            {
                return Content("Erro ao fazer o upload do arquivo: Formato inválido!");
            }

            if (farmacia)
            {
                if (!ExtensaoIsImagem(file.Extension))
                {
                    return Content("Erro ao fazer o upload do arquivo: Formato inválido!");
                }

                //deleta slide antigo

                var slideFarmaciaAntigo = slideRepository.GetProdutoVaSlideFarmacia(va.Id);

                if (slideFarmaciaAntigo != null) { 
                    slideFarmaciaAntigo.DeletaArquivosFisicos();
                    slideRepository.Delete(slideFarmaciaAntigo);
                    slideRepository.Save();
                }
            }

            var diretorioArmazenamento = Util.Sistema.AppSettings.Diretorios.DiretorioProdutos + va.Produto.Id + "/vas/" + va.Id + "/";
            var nomeArquivoOriginal = Path.GetFileNameWithoutExtension(fileData.FileName).RemoverAcentos().GerarUrlAmigavel().ToLower() + Path.GetExtension(fileData.FileName).ToLower();

            List<ProdutoVaSlide> slides = new List<ProdutoVaSlide>();
            ProdutoVaSlide slide = new ProdutoVaSlide();

            if (ExtensaoIsImagem(file.Extension))
            {
                CriaSlideImagem(slide, slides, idVa, farmacia, diretorioArmazenamento, nomeArquivoOriginal, retorno, fileData);

                retorno = "Arquivo enviado com sucesso! 1 slide adicionado.";

            } else if (ExtensaoIsPdf(file.Extension))
            {
                #region pdf
                
                //define caminho de origem e destino
                var caminhoOrigem = Util.Url.GetCaminhoFisico(diretorioArmazenamento + "temporario_" +  Sessao.Site.UsuarioInfo.Id + "_" + va.Id + ".pdf");

                //caminho das imagens das paginas dos pdfs, %d é substituido pelo numero da página.
                var caminhoDestino = Util.Url.GetCaminhoFisico(diretorioArmazenamento + Sessao.Site.UsuarioInfo.Id + "_%d.jpg");

                //salva o upload
                fileData.SaveAs(caminhoOrigem);
                
                //le o pdf
                var reader = new iTextSharp.text.pdf.PdfReader(caminhoOrigem);

                //valida o tamanhos dos pdfs
                if (!ValidaTamanhoPaginas(reader))
                {
                    System.IO.File.Delete(caminhoOrigem);

                    return Content("Erro ao fazer o upload do arquivo: Tamanho do pdf inválido, ele deve ter exatamente "+Util.Sistema.AppSettings.CampanhaResolucaoLargura+"x"+Util.Sistema.AppSettings.CampanhaResolucaoAltura);
                }

                try
                {
                    //roda o programa IMAGEMAGICK para gerar a imagem a partir do pdf, necessário IMAGEMAGICK e GHOSTSCRIPT INSTALADOS NA MAQUINA

                    string arguments = string.Format("\"{0}\" \"{1}\"", caminhoOrigem, caminhoDestino);
                    var startInfo = new ProcessStartInfo {
                        Arguments = arguments,
                        FileName = Util.Sistema.AppSettings.UrlImageMagickConvert
                    };
                    Process.Start(startInfo).WaitForExit();

                    //gera imagem a partir do pdf
                    //GhostscriptWrapper.GeneratePageThumbs(caminhoOrigem, caminhoDestino, 1, reader.NumberOfPages, 350, 350);
                }
                catch (Exception e)
                {
                    System.IO.File.Delete(caminhoOrigem);

                    string adicional = "";

                    if (e.InnerException != null)
                    {
                        adicional = e.InnerException.Message;
                    }

                    return Content(adicional + " " + e.Message + "Erro ao tentar criar a imagem a partir do pdf, verifique se o arquivo é realmente um pdf, foi gravado corretamente e tente novamente.");
                }

                for (int i = 0; i < reader.NumberOfPages; i++)
                {
                    var CaminhoArquivoSlide = caminhoDestino.Replace("%d", i.ToString());

                    //grava slide e arquivo no banco

                    slide = new ProdutoVaSlide();
                    slide.IdVa = idVa;

                    slideRepository.Add(slide);
                    slideRepository.Save();

                    ProdutoVaSlideArquivo arquivo = new ProdutoVaSlideArquivo();
                    arquivo.IdSlide = slide.Id;
                    arquivo.Tipo = (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg;

                    arquivoSlideRepository.Add(arquivo);
                    arquivoSlideRepository.Save();

                    var nomeArquivoSlide = arquivo.Id + Path.GetExtension(CaminhoArquivoSlide);
                    var novoCaminhoArquivoSlide = Path.GetDirectoryName(CaminhoArquivoSlide) + "\\" + nomeArquivoSlide;

                    //renomeia arquivo enviado
                    System.IO.File.Move(CaminhoArquivoSlide, novoCaminhoArquivoSlide);

                    //atualiza nome do arquivo no banco
                    arquivo.Nome = nomeArquivoOriginal;

                    arquivoSlideRepository.Save();


                    //redimensiona a imagem para 1024x768
                    Util.Imagem.Redimensionar(novoCaminhoArquivoSlide, new Util.Imagem.Size(Util.Sistema.AppSettings.CampanhaResolucaoLargura, Util.Sistema.AppSettings.CampanhaResolucaoAltura), Util.Imagem.EnumFormato.JPEG, false);
                    //gera miniatura
                    Util.Imagem.Redimensionar(novoCaminhoArquivoSlide, Path.GetDirectoryName(novoCaminhoArquivoSlide) + "\\" + Path.GetFileNameWithoutExtension(novoCaminhoArquivoSlide) + "_pq" + Path.GetExtension(novoCaminhoArquivoSlide), new Util.Imagem.Size(133, 100), Util.Imagem.EnumFormato.JPEG, false);

                    slides.Add(slide);
                }

                retorno = "Arquivo enviado com sucesso! " + reader.NumberOfPages + " slides adicionados.";

                //deleta arquivo enviado
                System.IO.File.Delete(caminhoOrigem);

                #endregion
            }
            else if(ExtensaoIsMp4(file.Extension)) // MP4
            {
                CriaSlideVideo(slide, slides, idVa, farmacia, diretorioArmazenamento, nomeArquivoOriginal, retorno, fileData);

                retorno = "Arquivo enviado com sucesso! 1 slide adicionado.";
            }
            else if(ExtensaoIsZip(file.Extension)) // ZIP
            {
                retorno = "zip inválido.";

                bool temPaginas = false;

                #region zip
                
                using (ZipFile zip = ZipFile.Read(fileData.InputStream))
                {
                    var contPaginas = 1;
                   
                    ZipEntry e;

                    do
                    {
                        e = zip[contPaginas];
                        var pastaPagina = contPaginas.ToString().PadLeft(2, '0');

                        if (zip.FolderExists(pastaPagina))
                        {
                            slide = new ProdutoVaSlide();

                            //verifica se existe só um arquivo na pasta
                            var arquivosZip = zip.GetFilesInFolder(pastaPagina);

                            if (arquivosZip.Count() == 1)
                            {
                                var arquivoZip = arquivosZip.First();

                                //verifica de que tipo é o arquivo
                                if (ExtensaoIsImagem(Path.GetExtension(arquivoZip.FileName)))
                                {
                                    //como n existe arquivo html dentro da pasta, cria um slide de imagem

                                    CriaSlideImagem(slide, slides, idVa, farmacia, diretorioArmazenamento, nomeArquivoOriginal, retorno, arquivoZip);

                                    contPaginas++;
                                    continue;
                                }

                                if (ExtensaoIsMp4(Path.GetExtension(arquivoZip.FileName)))
                                {
                                    //como n existe arquivo html dentro da pasta, cria um slide de video

                                    CriaSlideVideo(slide, slides, idVa, farmacia, diretorioArmazenamento, nomeArquivoOriginal, retorno, arquivoZip);

                                    contPaginas++;
                                    continue;
                                }
                            }

                            temPaginas = true;

                            slide.IdVa = idVa;

                            slideRepository.Add(slide);
                            slideRepository.Save();

                            ProdutoVaSlideArquivo arquivo = new ProdutoVaSlideArquivo();
                            arquivo.IdSlide = slide.Id;
                            arquivo.Tipo = (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Zip;
                            arquivo.Nome = nomeArquivoOriginal;

                            arquivoSlideRepository.Add(arquivo);
                            arquivoSlideRepository.Save();

                            var diretorioPagina = diretorioArmazenamento + slide.Id;
                            var diretorioPaginaFisico = Util.Url.GetCaminhoFisico(diretorioPagina);

                            Util.Arquivo.CreateDirectoryIfNotExists(diretorioPaginaFisico);
                            zip.ExtractFilesInFolder(diretorioPaginaFisico, pastaPagina);

                            contPaginas++;

                            slides.Add(slide);

                            //tenta inserir script nas paginas html
                            try
                            {
                                var caminhoDiretorio = Util.Url.GetCaminhoFisico(diretorioPagina);
                                
                                var arquivos = System.IO.Directory.GetFiles(caminhoDiretorio).Where(f => f.ToLower().EndsWith(".html"));
                                
                                foreach (var a in arquivos)
                                {
                                    var conteudo = Util.Arquivo.LerArquivo(a);

                                    HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

                                    // There are various options, set as needed
                                    htmlDoc.OptionFixNestedTags = true;

                                    // filePath is a path to a file containing the html
                                    htmlDoc.LoadHtml(conteudo);

                                    // Use:  htmlDoc.LoadXML(xmlString);  to load from a string

                                    // ParseErrors is an ArrayList containing any errors from the Load statement
                                    /*if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
                                    {
                                        // Handle any parse errors as required

                                    }
                                    else
                                    {*/

                                        if (htmlDoc.DocumentNode != null)
                                        {
                                            HtmlNode headNode = htmlDoc.DocumentNode.SelectSingleNode("//head");

                                            var aux = headNode.InnerHtml;
                                            headNode.InnerHtml = "<script src=\"../shared/gip_functions.js\"></script>" + aux;

                                            Util.Arquivo.EscreverArquivo(a, htmlDoc.DocumentNode.OuterHtml);
                                        }
                                    //}

                                }
                            }
                            catch (Exception erro)
                            {
                                Util.Sistema.Error.TrataErro(erro);
                            }

                            //tenta gerar miniatura da página
                            try
                            {

                                var caminhoImagem = diretorioPaginaFisico + "\\" + slide.Id + ".jpg";

                                /*try
                                {

                                    ScreenshotHelper ssh = new ScreenshotHelper();
                                    ssh.Shot("file://" + Util.Url.GetCaminhoFisico(diretorioPagina + "/index.html").Replace('\\', '/'), caminhoImagem);

                                    //redimensiona imagem
                                    Util.Imagem.Redimensionar(caminhoImagem, new Util.Imagem.Size(Util.Sistema.AppSettings.CampanhaResolucaoLargura, Util.Sistema.AppSettings.CampanhaResolucaoAltura), Util.Imagem.EnumFormato.JPEG, false);
                                }
                                catch
                                {
                                    //n faz nada
                                }*/

                                //verifica se existe o thumb na pasta
                                var caminhoThumb = Util.Url.GetCaminhoFisico(diretorioPagina + "/thumb.png");
                                var caminhoThumbDestino = Path.GetDirectoryName(caminhoImagem) + "\\" + Path.GetFileNameWithoutExtension(caminhoImagem) + "_pq" + Path.GetExtension(caminhoImagem);

                                if (System.IO.File.Exists(caminhoThumb))
                                {
                                    Util.Imagem.Redimensionar(caminhoThumb, caminhoThumbDestino, new Util.Imagem.Size(160, 120), Util.Imagem.EnumFormato.JPEG, false);

                                    //se o arquivo destino for diferente apaga o original pois nao sera usado
                                    if (Path.GetFileName(caminhoThumb) != Path.GetFileName(caminhoThumbDestino))
                                    {
                                        System.IO.File.Delete(caminhoThumb);
                                    }
                                }
                                else {

                                    //gera miniatura
                                    Util.Imagem.Redimensionar(caminhoImagem, caminhoThumbDestino, new Util.Imagem.Size(160, 120), Util.Imagem.EnumFormato.JPEG, false);
                                    
                                }
                            }
                            catch (Exception erro)
                            {
                                Util.Sistema.Error.TrataErro(erro);
                            }
                        }

                    } while (zip.FolderExists(contPaginas.ToString().PadLeft(2, '0')));

                    if (zip.FolderExists("shared"))
                    {
                        zip.ExtractFilesInFolder(Util.Url.GetCaminhoFisico(diretorioArmazenamento + "shared/"), "shared");
                    }

                    if (zip.FolderExists("images"))
                    {
                        zip.ExtractFilesInFolder(Util.Url.GetCaminhoFisico(diretorioArmazenamento + "images/"), "images");
                    }

                    if (zip.FolderExists("css"))
                    {
                        zip.ExtractFilesInFolder(Util.Url.GetCaminhoFisico(diretorioArmazenamento + "css/"), "css");
                    }

                    if (zip.FolderExists("js"))
                    {
                        zip.ExtractFilesInFolder(Util.Url.GetCaminhoFisico(diretorioArmazenamento + "js/"), "js");
                    }

                    if (temPaginas)
                    {
                        retorno = "Arquivo enviado com sucesso " + (contPaginas - 1) + " slides adicionados.";
                    }

                } // fim using

                #endregion

            }

            var especialidades = slide.ProdutoVa.GetEspecialidades();

            foreach (var s in slides) { 
                s.AddEspecialidades(slideRepository, especialidades.Select(e => e.Id).ToArray());
            }

            slideRepository.Save();

            return Content(retorno);
        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public void ExcluirSlide(int idSlide, int idVa)
        {
            var slide = slideRepository.GetProdutoVaSlide(idSlide);

            if (slide == null || slide.ProdutoVa.Id != idVa)
                return;

            if (!slide.ProdutoVa.ValidoParaEdicao())
                return;

            //deleta arquivos fisicosds
            slide.DeletaArquivosFisicos();

            slideRepository.Delete(slide);
            slideRepository.Save();
        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public void AtualizarOrdem(int[] IdSlides, int idVa)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return;

            if (!va.ValidoParaEdicao())
                return;

            //verifica ids que pertencem ao VA informado
            var slides = va.ProdutoVaSlides.Where(s => IdSlides.Contains(s.Id));

            //verifica se foram enviados os ids de todos os slides
            if (IdSlides.Count() != slides.Count())
                return;

            if (IdSlides == null)
            {
                slides = va.ProdutoVaSlides.Where(s => !s.IsFarmacia()).OrderBy(s => s.Ordem);
            }

            int i;
            //atualiza ordem
            for (i = 1; i <= slides.Count(); i++)
            {
                var slide = slideRepository.GetProdutoVaSlide(IdSlides[i - 1]);

                slide.Ordem = i;
            }

            var slideFarmacia = slideRepository.GetProdutoVaSlideFarmacia(va.Id);

            //se existir slide de farmacia, o coloca como último slide
            if (slideFarmacia != null)
            {
                i++;
                slideFarmacia.Ordem = i;
            }

            slideRepository.Save();

        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public void AtualizarAutoPlay(int IdSlide, int idVa, bool autoPlay)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return;

            if (!va.ValidoParaEdicao())
                return;

            //verifica ids que pertencem ao VA informado
            var slide = va.ProdutoVaSlides.Where(s => s.Id == IdSlide).FirstOrDefault();

            //verifica se foram enviados os ids de todos os slides
            if (slide == null)
                return;

            //atualiza autoplay
            var arquivo = arquivoSlideRepository.GetProdutoVaSlideArquivos().Where(a => a.ProdutoVaSlide.Id == slide.Id).FirstOrDefault();

            if (arquivo == null)
                return;

            arquivo.VideoAutoPlay = autoPlay;

            arquivoSlideRepository.Save();

        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public void AtualizarSlideEspecialidades(int IdSlide, int?[] IdsEspecialidade)
        {
            var slide = slideRepository.GetProdutoVaSlide(IdSlide);

            if (slide == null || !slide.ProdutoVa.ValidoParaEdicao())
                return;

            slide.DeletaEspecialidades(slideRepository);

            if (IdsEspecialidade != null) { 
                int[] idsEspecialidades = IdsEspecialidade.Select(e => e.Value).ToArray();
                slide.AddEspecialidades(slideRepository, idsEspecialidades);
            }

            slideRepository.Save();

        }

        #endregion

        #region Arquivos

        [HttpGet]
        [AuthorizePermissao("cadastro")]
        public ActionResult GetArquivos(int idVa)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return new EmptyResult();

            if (!va.ValidoParaEdicao())
                return new EmptyResult();

            var arquivos = va.ProdutoVaArquivos;

            string result = String.Empty;

            string modelo = Html("GetArquivos");

            foreach (var arquivo in arquivos)
            {
                result += modelo.ReplaceChaves(new
                {

                    data = arquivo.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno),
                    url_arquivo = arquivo.GetCaminho().ResolveURL(),
                    nome = arquivo.Nome,
                    nome_aspas = "'" + arquivo.Nome + "'",
                    tipo = (ProdutoVaArquivo.EnumTipo)arquivo.Tipo,
                    url_excluir = "javascript:excluirArquivo(" + arquivo.Id + ",'"+arquivo.Nome +"')",
                    id_arquivo = arquivo.Id.ToString()

                });
            }

            return Content(result);
        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public ActionResult UploadArquivo(HttpPostedFileBase fileData, int idVa)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if(va == null)
                return new EmptyResult();

            if (!va.ValidoParaEdicao() || va.ProdutoVaCategoria.Tipo != (char)ProdutoVaCategoria.EnumTipo.Anexo)
                return new EmptyResult();

            //deleta arquivo(s) atual pq soh pode existir um por "va"
            foreach (var a in va.ProdutoVaArquivos)
            {
                this.ExcluirArquivo(a.Id, va.Id);
            }

            var extensao = Path.GetExtension(fileData.FileName).ToLower();
            
            ProdutoVaArquivo arquivo = new ProdutoVaArquivo();

            bool valido = false;

            foreach (ListItem item in typeof(ProdutoVaArquivo.EnumTipo).ToListCollection(Util.Dados.EnumHelper.TipoValor.Char))
            {
                if (item.Text == extensao)
                {
                    arquivo.Tipo = Convert.ToChar(item.Value);

                    valido = true;
                    break;
                }
            }

            if (!valido)
            {
                return Content("Erro ao fazer o upload do arquivo: Formato inválido!");
            }

            arquivo.Nome = fileData.FileName;
            arquivo.IdVa = idVa;
            

            arquivoRepository.Add(arquivo);
            arquivoRepository.Save();
            
            //pega caminho fisico do arquivo
            var caminhoDestino = arquivo.GetCaminhoFisico();

            fileData.SaveAs(caminhoDestino);

            return Content("Arquivo enviado com sucesso! Arquivo adicionado ao VA.");
        }

        [HttpPost]
        [AuthorizePermissao("cadastro")]
        public void ExcluirArquivo(int idArquivo, int idVa)
        {
            var arquivo = arquivoRepository.GetProdutoVaArquivo(idArquivo);

            if (arquivo == null || arquivo.ProdutoVa.Id != idVa)
                return;

            if (!arquivo.ProdutoVa.ValidoParaEdicao())
                return;

            arquivo.DeletaArquivoFisico();

            arquivoRepository.Delete(arquivo);
            arquivoRepository.Save();
        }

        [HttpPost]
        public void AlterarNomeArquivo(int idArquivo, int idVa, string nomeArquivo)
        {
            var va = vaRepository.GetProdutoVa(idVa);
            var arquivo = arquivoRepository.GetProdutoVaArquivo(idArquivo);

            if (va == null || arquivo == null)
                return;

            if (!va.ValidoParaEdicao() || !va.ProdutoVaArquivos.Any(a => a.Id == arquivo.Id))
                return;

            arquivo.Nome = nomeArquivo;
            arquivoRepository.Save();

            return;

        }

        #endregion

        #endregion

    }
}
