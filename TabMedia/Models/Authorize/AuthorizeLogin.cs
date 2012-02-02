using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// Necessita que o usuario esteja logado, mas não verifica permissoes adicionais.
/// </summary>
public class AuthorizeLogin : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        return (Sessao.Site.UsuarioLogado());
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