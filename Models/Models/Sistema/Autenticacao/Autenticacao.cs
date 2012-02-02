using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Models;

public static class Autenticacao
{
    public enum StatusAutenticacao {
        Autorizado,
        Negado
    }

    public static void RedirectToLoginPage()
    {
        RedirectToLoginPage(Util.Sistema.AppSettings.Autenticacao.UrlLogin);
    }

    public static void RedirectToLoginPage(String paginaLogin)
    {
        RedirectToLoginPage(paginaLogin, String.Empty);
    }

    public static void RedirectToLoginPage(String paginaLogin, String parametroAdicional)
    {
        HttpContext.Current.Response.Redirect(GetRedirectToLoginPage(paginaLogin, parametroAdicional));
    }

    /// <summary>
    /// Retorna url de redirecionamento para a página de login padrão
    /// </summary>
    /// <returns></returns>
    public static String GetRedirectToLoginPage()
    {
        return GetRedirectToLoginPage(Util.Sistema.AppSettings.Autenticacao.UrlLogin, String.Empty);
    }

    /// <summary>
    /// Retorna url de redirecionamento para a página de login padrão
    /// </summary>
    /// <returns></returns>
    public static String GetRedirectToLoginPage(bool getUrlResolvida)
    {
        return GetRedirectToLoginPage(Util.Sistema.AppSettings.Autenticacao.UrlLogin, String.Empty, getUrlResolvida);
    }

    /// <summary>
    /// Retorna url de redirecionamento para a página passada no parametro paginaLogin com uma querystring de retorno a pagina atual
    /// </summary>
    /// <returns></returns>
    public static String GetRedirectToLoginPage(String paginaLogin)
    {
        return GetRedirectToLoginPage(paginaLogin, String.Empty);
    }

    /// <summary>
    /// Retorna url de redirecionamento para a página passada no parametro paginaLogin com uma querystring de retorno a pagina atual
    /// </summary>
    /// <returns></returns>
    public static String GetRedirectToLoginPage(String paginaLogin, bool getUrlResolvida)
    {
        return GetRedirectToLoginPage(paginaLogin, String.Empty, getUrlResolvida);
    }

    /// <summary>
    ///Retorna url de redirecionamento para a página passada no parametro paginaLogin com uma querystring de retorno a pagina atual
    /// </summary>
    public static String GetRedirectToLoginPage(String paginaLogin, String parametroAdicional)
    {
        return GetRedirectToLoginPage(paginaLogin, parametroAdicional, false);
    }

    /// <summary>
    ///Retorna url de redirecionamento para a página passada no parametro paginaLogin com uma querystring de retorno a pagina atual
    /// </summary>
    /// <param name="paginaLogin"></param>
    /// <param name="parametroAdicional"></param>
    /// <param name="getUrlResolvida">retorna a url resolvida: Util.Url.ResolveUrl(paginaLogin) + complemento</param>
    /// <returns></returns>
    public static String GetRedirectToLoginPage(String paginaLogin, String parametroAdicional, bool getUrlResolvida)
    {
        var complemento = "?ReturnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl) + parametroAdicional;

        if (getUrlResolvida)
            return Util.Url.ResolveUrl(paginaLogin) + complemento;

        return paginaLogin + complemento;
    }

    /// <summary>
    /// Redireciona para a url na querystring ReturnUrl ou, caso não exista, para a urlDefault.
    /// </summary>
    public static void RedirectFromLoginPage(String urlDefault)
    {
        if (HttpContext.Current.Request.QueryString["ReturnUrl"] == null || HttpContext.Current.Request.QueryString["ReturnUrl"] == String.Empty)
        {
            HttpContext.Current.Response.Redirect(urlDefault);
        }
        else
        {
            HttpContext.Current.Response.Redirect(HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["ReturnUrl"]));
        }
    }

    /// <summary>
    /// Retorna a url na querystring ReturnUrl ou, caso não exista, a urlDefault.
    /// </summary>
    public static String GetRedirectFromLoginPage(String urlDefault)
    {
        if (HttpContext.Current.Request.QueryString["ReturnUrl"] == null || HttpContext.Current.Request.QueryString["ReturnUrl"] == String.Empty)
        {
            return urlDefault;
        }
        else
        {
            return HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["ReturnUrl"]);
        }
    }

    public static bool AutorizaPermissao(string action, string controller)
    {
        return AutorizaPermissao(action,controller, Sessao.Site.RetornaUsuario());
    }

    public static bool AutorizaPermissao(string action, string controller, Usuario usuario)
    {
        return (VerificaPermissao(action, controller,usuario) == StatusAutenticacao.Autorizado);
    }

    public static StatusAutenticacao VerificaPermissao(string action, string controller)
    {
        return VerificaPermissao(action, controller, Sessao.Site.RetornaUsuario());
    }

    public static StatusAutenticacao VerificaPermissao(string action, string controller, Usuario usuario)
    {
        Grupo grupoUsuario = usuario.Grupo;

        bool autorizado =  grupoUsuario.AcaoGrupos.Any(acaoGrupo => acaoGrupo.Acao.Nome.ToLower() == action.ToLower() && acaoGrupo.Acao.Controlador.Nome.ToLower() == controller.ToLower());

        if (autorizado)
        {
            return StatusAutenticacao.Autorizado;
        }

        return StatusAutenticacao.Negado;
    }
}