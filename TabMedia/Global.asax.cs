using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace GuiropaIpad
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("closeup/{*pathInfo}");

            routes.MapRoute(
                "ProdutosImagens_Download",
                "produtos/imagens/download/{idTerritorio}/{token}",
                new { controller = "Produtos", action = "Download" },  // Parameter defaults
                new[] { "Controllers" } // Namespaces
            );

            routes.MapRoute(
                "VA_Download",
                "va/download/{idVa}/{idTerritorio}/{tokenTerritorio}",
                new { controller = "ProdutosVas", action = "Download" },  // Parameter defaults
                new[] { "Controllers" } // Namespaces
            );

            routes .MapRoute(
                "Admin_customizacao",
                "Admin/Customizacao/{action}.css",
                new { controller = "Customizacao", action = "CssAdmin" },  // Parameter defaults
                new[] { "Controllers" } // Namespaces
            );

            routes.MapRoute(
                "Customizacao",
                "Customizacao/{action}.css",
                new { controller = "Customizacao", action = "ValidationEngine" }, // Parameter defaults
                new[] { "Controllers" }
            );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new[] { "Controllers" } // Namespaces
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

            //if(HttpContext.Current.Request.Url.Host.Contains("cpro2497.publiccloud.com.br")){

            //    HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri.Replace("cpro2497.publiccloud.com.br", "guiropatab.com"), true);

            //}

            /*if (!HttpContext.Current.Request.Url.AbsolutePath.Contains("manutencao"))
            {
                HttpContext.Current.Response.Redirect("http://nycomed02.guiropatab.com/manutencao");
            }*/

            /* we guess at this point session is not already retrieved by application so we recreate cookie with the session id... */

            try
            {

                string session_param_name = "ASPSESSID";
                string session_cookie_name = "ASP.NET_SessionId";

                if (HttpContext.Current.Request.Form[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form[session_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString[session_param_name]);
                }
            }
            catch { }

            try
            {
                string auth_param_name = "AUTHID";

                string auth_cookie_name = FormsAuthentication.FormsCookieName;

                if (HttpContext.Current.Request.Form[auth_param_name] != null)
                {
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form[auth_param_name]);
                }

                else if (HttpContext.Current.Request.QueryString[auth_param_name] != null)
                {
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.QueryString[auth_param_name]);
                }
            }

            catch { }
        }

        private void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);

            if (null == cookie)
            {
                cookie = new HttpCookie(cookie_name);
            }

            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
    }
}