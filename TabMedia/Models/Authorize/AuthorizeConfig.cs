using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// Necessita que o usuario esteja logado de acordo com a senha no webConfig
/// </summary>
public class AuthorizeConfig : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        return (Sessao.Config.UsuarioLogado());
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        if (filterContext.HttpContext.Request.IsAjaxRequest())
        {
            var content = new ContentResult();
            content.Content = "<script type='text/javascript'>window.location = \"" + Util.Url.ResolveUrl(Util.Sistema.Autenticacao.UrlLoginConfig) + "\"</script>";
            filterContext.Result = content;
        }
        else
        {
            filterContext.Result = new RedirectResult(Autenticacao.GetRedirectToLoginPage(Util.Sistema.Autenticacao.UrlLoginConfig));
        }
    }

}