using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace GuiropaIpad.Controllers
{
    [AuthorizeLogin]
    public class AgenciasController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Agencias, Log.EnumArea.Site);
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        ProdutoRepository produtoRepository = new ProdutoRepository();

        [AuthorizePermissao]
        public ActionResult Index()
        {
            logs.Add(Log.EnumTipo.Consulta, "Consultou as Agências", Url.Action("index"));

            return View();
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetAgencias()
        {
            var agencias = usuarioRepository.GetAgencias();

            string result = String.Empty;

            string modelo = Html("GetAgencias");

            foreach (var agencia in agencias)
            {
                var produtos = string.Empty;
                var nomesProdutos = agencia.UsuarioProdutos.Select(p => p.Produto.Nome);
                var cont = 1;
                foreach (string nomeProduto in nomesProdutos)
                {
                    produtos += nomeProduto;
                    if (cont < nomesProdutos.Count())
                        produtos += " &nbsp;&nbsp;|&nbsp;&nbsp; ";

                    cont++;
                }

                result += modelo.ReplaceChaves(new
                {
                    agencia_nome = agencia.Nome,
                    agencia_email = agencia.Email,
                    agencia_produtos = produtos,
                    editar = "javascript:editar(" + agencia.Id + ")"
                });
            }

            return Content(result);
        }

        [HttpGet]
        [AuthorizePermissao("index")]
        public ActionResult GetProdutos(int idAgencia)
        {
            var usuario = usuarioRepository.GetUsuario(idAgencia);

            if (usuario == null || !usuario.IsAgencia())
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

            return Content(result);
        }

        [HttpPost]
        [AuthorizePermissao("index")]
        public void AtualizaProdutosAgencia(int? idAgencia, int?[] produtos)
        {
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            ProdutoRepository produtoRepository = new ProdutoRepository();

            if (idAgencia == null)
                return;

            var usuario = usuarioRepository.GetUsuario(idAgencia.Value);

            if (usuario == null || !usuario.IsAgencia())
                return;

            //apaga as relações da agencia com os produtos
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

            logs.Add(Log.EnumTipo.Consulta, "Alterou os produtos da Agência '" + usuario.Nome + "'", Url.Action("index"));

        }

        public ActionResult Teste()
        {
            return View();
        }

    }
}
