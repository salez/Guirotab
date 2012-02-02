using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace GuiropaIpad.Controllers
{
    [AuthorizeLogin]
    public class GerenteController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Gerente, Log.EnumArea.Site);
        ProdutoVaRepository vaRepository = new ProdutoVaRepository();

        [AuthorizePermissao]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetVasPendentes() //id produto
        {
            var vas = vaRepository.GetProdutoVas();

            if (Sessao.Site.UsuarioInfo.IsGerenteProduto())
            {
                vas = vas.Where(va => va.Status == (char)ProdutoVa.EnumStatus.Pendente);
            }
            else if (Sessao.Site.UsuarioInfo.IsGerenteMarketing())
            {
                vas = vas.Where(va => va.StatusGM == (char)ProdutoVa.EnumStatus.Pendente);
            }
            else
            {
                return new EmptyResult();
            }


            string result = String.Empty;

            string modelo = Html("GetVasPendentes");

            foreach (var va in vas)
            {
                result += modelo.ReplaceChaves(new
                {
                    data = va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno),
                    produto_nome = va.Produto.Nome,
                    qtde_slides = va.ProdutoVaSlides.Count().ToString(),
                    status = va.GetStatus(),
                    url_visualizar = Url.Action("visualizar","produtosvas", new { id = va.Id }),
                    url_editar = Url.Action("editar", "produtosvas", new { idVa = va.Id, idProduto = va.Produto.Id }),
                    url_excluir = "javascript:excluirVa('" + va.Id + "', '"+va.DataInclusao.Value.Formata(Util.Data.FormatoData.DiaMesAno)+"')"
                });
            }

            return Content(result);
        }

        [HttpPost]
        [AuthorizePermissao("ExcluirVA","produtosvas")]
        public void ExcluirVA(int idVa)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return;

            vaRepository.Delete(va);
            vaRepository.Save();

            //exclui diretorio do VA
            va.ExcluirDiretorio();

            logs.Add(Log.EnumTipo.Exclusao, "Excluiu o VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", Url.Action("Index", new { id = va.Produto.Id }));
        }

        [AuthorizePermissao]
        public ActionResult Agencias()
        {
            logs.Add(Log.EnumTipo.Consulta, "Consultou as suas Agencias", Url.Action("Agencias"));

            return View();
        }

        [HttpGet]
        [AuthorizePermissao("agencias")]
        public ActionResult GetAgencias()
        {
            var usuario = Sessao.Site.RetornaUsuario();

            var agencias = usuario.GetAgencias();

            string result = String.Empty;

            string modelo = Html("GetAgencias");

            foreach (var agencia in agencias)
            {

                var produtos = string.Empty;
                var nomesProdutos = agencia.UsuarioProdutos.Select(p => p.Produto.Nome);
                var cont= 1;

                foreach(string nomeProduto in nomesProdutos){
                    produtos += nomeProduto;
                    if(cont < nomesProdutos.Count())
                        produtos += "&nbsp;&nbsp;|&nbsp;&nbsp;";

                    cont++;
                }

                result += modelo.ReplaceChaves(new
                {
                    agencia_nome        = agencia.Nome,
                    agencia_produtos    = produtos,
                    editar              = "javascript:editar("+agencia.Id+")"
                });
            }

            return Content(result);
        }

        [HttpGet]
        [AuthorizePermissao("agencias")]
        public ActionResult GetProdutos(int idAgencia)
        {
            var usuario = Sessao.Site.RetornaUsuario();

            var agencia = usuario.GetAgencia(idAgencia);

            if (agencia == null)
                return new EmptyResult();

            string result = String.Empty;

            string modelo = Html("GetProdutos");

            foreach (var produto in usuario.UsuarioProdutos.Select(up => up.Produto).OrderBy(p => p.Nome))
            {
                result += modelo.ReplaceChaves(new
                {
                    produto_id          = produto.Id,
                    produto_nome        = produto.Nome,
                    produto_checked     = agencia.UsuarioProdutos.Any(up => up.IdProduto == produto.Id)?"checked=\"checked\"":string.Empty
                });
            }

            logs.Add(Log.EnumTipo.Consulta, "Consultou os produtos da Agencia '"+agencia.Nome+"'", Url.Action("Agencias"));

            return Content(result);
        }

        [HttpPost]
        [AuthorizePermissao("agencias")]
        public void AtualizaProdutosAgencia(int? idAgencia, int?[] produtos)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            ProdutoRepository produtoRepository = new ProdutoRepository();

            if (idAgencia == null)
                return;

            var usuarioGerente = Sessao.Site.RetornaUsuario();

            var agencia = usuarioGerente.GetAgencia(idAgencia.Value);

            if (agencia == null)
                return;

            if (produtos != null)
            {
                //verifica se algum dos ids de produto não pertence ao gerente
                foreach (var idProduto in produtos)
                {
                    if (!usuarioGerente.UsuarioProdutos.Any(up => up.IdProduto == idProduto))
                        return;
                }
            }

            agencia = usuarioRepository.GetAgencia(agencia.Id);

            //apaga as relações da agencia com os produtos do gerente
            agencia.ApagarRelacoesUsuarioProdutoByGerente(usuarioRepository, usuarioGerente);

            usuarioRepository.Save();

            if (produtos != null) {
                foreach (var idProduto in produtos)
                {
                    var produto = produtoRepository.GetProduto(idProduto.Value);

                    agencia.AddProduto(usuarioRepository, produto);
                }
            
                usuarioRepository.Save();
            }

            logs.Add(Log.EnumTipo.Consulta, "Alterou os produtos da Agencia '" + agencia.Nome + "'", Url.Action("Agencias"));

        }

    }
}
