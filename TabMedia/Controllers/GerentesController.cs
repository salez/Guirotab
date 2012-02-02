using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace GuiropaIpad.Controllers
{
    [AuthorizeLogin]
    public class GerentesController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Gerentes, Log.EnumArea.Site);
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        ProdutoRepository produtoRepository = new ProdutoRepository();

        [AuthorizePermissao]
        public ActionResult Index()
        {
            logs.Add(Log.EnumTipo.Consulta, "Consultou os Gerentes", Url.Action("Index"));

            return View();
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetGerentes()
        {
            var gerentes = usuarioRepository.GetGerentesProduto();

            string result = String.Empty;

            string modelo = Html("GetGerentes");

            foreach (var gerente in gerentes)
            {
                var produtos = string.Empty;
                var nomesProdutos = gerente.UsuarioProdutos.Select(p => p.Produto.Nome);
                var cont = 1;
                foreach (string nomeProduto in nomesProdutos)
                {
                    produtos += nomeProduto;
                    if (cont < nomesProdutos.Count())
                        produtos += "&nbsp; | &nbsp;";

                    cont++;
                }

                result += modelo.ReplaceChaves(new
                {
                    gerente_nome = gerente.Nome,
                    gerente_email = gerente.Email,
                    gerente_produtos = produtos,
                    editar = "javascript:editar(" + gerente.Id + ")"
                });
            }

            return Content(result);
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetGerentesMarketing()
        {
            var gerentes = usuarioRepository.GetGerentesMarketing();

            string result = String.Empty;

            string modelo = Html("GetGerentes");

            foreach (var gerente in gerentes)
            {
                var produtos = string.Empty;
                var nomesProdutos = gerente.UsuarioProdutos.Select(p => p.Produto.Nome);
                var cont = 1;
                foreach (string nomeProduto in nomesProdutos)
                {
                    produtos += nomeProduto;
                    if (cont < nomesProdutos.Count())
                        produtos += "&nbsp; | &nbsp;";

                    cont++;
                }

                result += modelo.ReplaceChaves(new
                {
                    gerente_nome = gerente.Nome,
                    gerente_email = gerente.Email,
                    gerente_produtos = produtos,
                    editar = "javascript:editar(" + gerente.Id + ")"
                });
            }

            return Content(result);
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetProdutos(int idGerente)
        {
            var usuario = usuarioRepository.GetUsuario(idGerente);

            if (usuario == null || !usuario.IsGerente())
                return new EmptyResult();

            string result = String.Empty;

            string modelo = Html("GetProdutos");

            var produtos = produtoRepository.GetProdutos();

            foreach (var produto in produtos.OrderBy(p => p.Nome))
            {
                result += modelo.ReplaceChaves(new
                {
                    produto_id = produto.Id,
                    produto_nome = produto.Nome,
                    produto_checked = usuario.UsuarioProdutos.Any(up => up.IdProduto == produto.Id) ? "checked=\"checked\"" : string.Empty
                });
            }

            logs.Add(Log.EnumTipo.Consulta, "Consultou os produtos do gerente '" + usuario.Nome + "'", Url.Action("index"));

            return Content(result);
        }

        [HttpPost]
        [AuthorizePermissao("index")]
        public void AtualizaProdutosGerente(int? idGerente, int?[] produtos)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            ProdutoRepository produtoRepository = new ProdutoRepository();

            if (idGerente == null)
                return;

            var usuario = usuarioRepository.GetUsuario(idGerente.Value);

            if (usuario == null || !usuario.IsGerente())
                return;

            //apaga as relações da agencia com os produtos do gerente
            usuario.ApagarRelacoesUsuarioProduto(usuarioRepository);

            usuarioRepository.Save();

            if (produtos != null)
            {
                foreach (var idProduto in produtos)
                {
                    var produto = produtoRepository.GetProduto(idProduto.Value);

                    usuario.AddProduto(usuarioRepository, produto);
                }

                usuarioRepository.Save();
            }

            logs.Add(Log.EnumTipo.Consulta, "Alterou os produtos do gerente '" + usuario.Nome + "'", Url.Action("index"));

        }

    }
}
