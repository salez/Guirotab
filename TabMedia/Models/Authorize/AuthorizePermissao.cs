using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

/// <summary>
/// Necessita que o usuario esteja logado e tenha acesso a todas as ações que estiverem presentes
/// </summary>
public class AuthorizePermissao : AuthorizeAttribute
{
    public string Action { get; set; }
    public string Controller { get; set; }

    public AuthorizePermissao()
    {

    }

    /// <summary>
    /// herda permissoes de action no mesmo controller, ou seja, se o usuario tiver acesso a action especificada e ao controller atual entao é validado
    /// </summary>
    public AuthorizePermissao(string action)
    {
        this.Action = action;
    }

    /// <summary>
    /// herda permissoes de action e controller especificados, ou seja, se o usuario tiver acesso a action e controller especificado entao é validado
    /// </summary>
    public AuthorizePermissao(string action, string controller)
    {
        this.Action = action;
        this.Controller = controller;
    }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        return (Sessao.Site.UsuarioLogado());
    }

    public override void OnAuthorization(AuthorizationContext filterContext)
    {
        if (Sessao.Site.UsuarioLogado())
        {
            if(this.Action == null)
                this.Action = filterContext.ActionDescriptor.ActionName;

            if(this.Controller == null)
                this.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            Grupo grupoUsuario = Sessao.Site.RetornaUsuario().Grupo;

            if (!grupoUsuario.AcaoGrupos.Any(acaoGrupo => acaoGrupo.Acao.Nome.ToLower() == this.Action.ToLower() && acaoGrupo.Acao.Controlador.Nome.ToLower() == this.Controller.ToLower()))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var content = new ContentResult();
                    content.Content = "<script type='text/javascript'>window.location = \"" + Util.Url.ResolveUrl(Util.Sistema.Autenticacao.UrlLogin) + "?erro=permissao" + "\"</script>";
                    filterContext.Result = content;
                }
                else
                {
                    //requisição sem permissão - redireciona pagina login
                    filterContext.Result = new RedirectResult(Util.Sistema.Autenticacao.UrlLogin + "?erro=permissao");
                }
            }
        }
        else
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                var content = new ContentResult();
                content.Content = "<script type='text/javascript'>window.location = \"" + Util.Url.ResolveUrl(Util.Sistema.Autenticacao.UrlLogin) + "\"</script>";
                filterContext.Result = content;
            }
            else
            {
                //requisição não logado - redireciona pagina login

                filterContext.Result = new RedirectResult(Autenticacao.GetRedirectToLoginPage(Util.Sistema.Autenticacao.UrlLogin));
            }
        }
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        if (filterContext.HttpContext.Request.IsAjaxRequest())
        {
            var content = new ContentResult();
            content.Content = "<script type='text/javascript'>window.location = \"" + Util.Url.ResolveUrl(Util.Sistema.Autenticacao.UrlLogin) + "\"</script>";
            filterContext.Result = content;
        }
        else
        {
            filterContext.Result = new RedirectResult(Autenticacao.GetRedirectToLoginPage(Util.Sistema.Autenticacao.UrlLogin));
        }
    }

}