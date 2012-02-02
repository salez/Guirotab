using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

public static class Autenticacao
{
    public static void RedirectToLoginPage()
    {
        RedirectToLoginPage(Util.Sistema.Autenticacao.UrlLogin);
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
        return GetRedirectToLoginPage(Util.Sistema.Autenticacao.UrlLogin, String.Empty);
    }

    /// <summary>
    /// Retorna url de redirecionamento para a página de login padrão
    /// </summary>
    /// <returns></returns>
    public static String GetRedirectToLoginPage(bool getUrlResolvida)
    {
        return GetRedirectToLoginPage(Util.Sistema.Autenticacao.UrlLogin, String.Empty, getUrlResolvida);
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
}