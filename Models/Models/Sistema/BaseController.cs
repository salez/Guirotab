using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Models;

public class BaseController : Controller
{
    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        /*if (!Sessao.Site.UsuarioLogado())
        {
            var cookieLogin = Request.Cookies["login"];
            var cookieCodigoLogin = Request.Cookies["loginautomatico"];

            if (cookieLogin != null && !cookieLogin.Value.IsNullOrEmpty())
            {
                UsuarioRepository usuarioRepository = new UsuarioRepository();

                Usuario usuario = usuarioRepository.GetUsuarioByLoginOrEmail(cookieLogin.Value, cookieLogin.Value);

                if (cookieCodigoLogin != null && !cookieCodigoLogin.Value.IsNullOrEmpty())
                {
                    if (usuario != null && usuario.CodigoLogin == cookieCodigoLogin.Value)
                    {
                        Sessao.Site.Logar(usuario);
                    }
                }
            }
        }*/

        base.OnActionExecuting(filterContext);
    }

    protected override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        if (Util.Sistema.AppSettings.Visitantes.ControlarVisitantesOnline)
        {
            VisitaOnlineRepository onlineRepository = new VisitaOnlineRepository();

            onlineRepository.AtualizaVisitasOnline();
        }

        base.OnResultExecuted(filterContext);
    }

    protected override void OnException(ExceptionContext filterContext)
    {
        Exception ex = filterContext.Exception;

        Util.Sistema.Error.TrataErro(ex, Request);

        if (HttpContext.IsCustomErrorEnabled)
        {
            filterContext.ExceptionHandled = true;

            if (!Request.IsAjaxRequest())
            {
                filterContext.Result = Redirect("~/erro");
            }
            else
            {
                filterContext.Result = Content("Ocorreu um erro durante a requisição.");
            }
        }

        base.OnException(filterContext);
    }


    /// <summary>
    /// retorna o html do modelo nomeArquivo que se encontra na pasta do controller, ex HtmlModelo("GetTerritorios")
    /// </summary>
    /// <param name="nomeArquivo"></param>
    protected string Html(string nomeArquivo){

        var caminhoArquivoController = Util.Url.GetCaminhoFisico("~/Views/" + this.GetControllerName() + "/html/" + nomeArquivo + ".html");

        if(System.IO.File.Exists(caminhoArquivoController))
            return Util.Arquivo.LerArquivo(caminhoArquivoController);

        caminhoArquivoController = Util.Url.GetCaminhoFisico("~/Views/" + this.GetControllerName() + "/html/" + nomeArquivo + ".htm");

        if (System.IO.File.Exists(caminhoArquivoController))
            return Util.Arquivo.LerArquivo(caminhoArquivoController);

        var caminhoArquivoShared = Util.Url.GetCaminhoFisico("~/Views/Shared/html/" + nomeArquivo + ".html");

        if (System.IO.File.Exists(caminhoArquivoShared))
            return Util.Arquivo.LerArquivo(caminhoArquivoShared);

        caminhoArquivoShared = Util.Url.GetCaminhoFisico("~/Views/Shared/html/" + nomeArquivo + ".htm");

        if (System.IO.File.Exists(caminhoArquivoShared))
            return Util.Arquivo.LerArquivo(caminhoArquivoShared);

        throw new Exception("Arquivo não encontrado em nenhum dos caminhos: \n\n " + caminhoArquivoController + "\n" + caminhoArquivoShared);
    }
}