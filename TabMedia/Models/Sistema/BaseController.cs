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
        if (!Sessao.Site.UsuarioLogado())
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
        }

        base.OnActionExecuting(filterContext);
    }

    protected override void OnResultExecuted(ResultExecutedContext filterContext)
    {
        if (Util.Sistema.Visitantes.ControlarVisitantesOnline)
        {
            VisitaOnlineRepository onlineRepository = new VisitaOnlineRepository();

            onlineRepository.AtualizaVisitasOnline();
        }

        base.OnResultExecuted(filterContext);
    }

    protected override void OnException(ExceptionContext filterContext)
    {
        Exception ex = filterContext.Exception;

        #region Envia Email

        String body = Util.Email.GetCorpoEmail("erro");

        String mensagem = "";

        int cont = 1;
        while (ex != null)
        {
            mensagem += "<fieldset>";
            mensagem += "<p><strong>Erro:</strong></p>";
            mensagem += "<p>" + ex.Message + "</p>";
            mensagem += "<p><strong>Stack Trace:</strong></p>";
            mensagem += "<p>" + ex.StackTrace.Nl2br() + "</p>";

            String method = "";

            if (ex.TargetSite != null)
            {
                method = ex.TargetSite.Name;
            }

            mensagem += "<p><strong>Method:</strong></p>";
            mensagem += "<p>" + method + "</p>";
            mensagem += "<p><strong>Source:</strong></p>";
            mensagem += "<p>" + ex.Source + "</p>";

            ex = ex.InnerException;
            cont++;
        }
        for (int i = 1; i <= cont; i++)
        {
            mensagem += "</fieldset>";
        }

        String urlreferrer = "";
        if (Request.UrlReferrer != null)
        {
            urlreferrer = Request.UrlReferrer.ToString();
        }
        else
        {
            urlreferrer = " - ";
        }

        String infoUsuario = "";

        #region Informações do Usuário

        try
        {
            if (Sessao.Site.UsuarioLogado())
            {
                UsuarioInfo usuario = Sessao.Site.UsuarioInfo;
                infoUsuario += "<p><strong>Id:</strong>";
                infoUsuario += usuario.Id.ToString();
                infoUsuario += "<br><strong>Login:</strong>";
                infoUsuario += usuario.Login.ToString();
                infoUsuario += "<br><strong>Nome:</strong>";
                infoUsuario += usuario.Nome.ToString();
                infoUsuario += "</p>";
            }
            else
            {
                infoUsuario = "<font color='blue'>Usuário não logado</font>";
            }
        }
        catch (Exception e)
        {
            infoUsuario = "<p><font color='red'>Ocorreu um erro ao tentar recuperar as informações do usuário.</font></p>";
            infoUsuario = "<fieldset><legend>Detalhes do Erro</legend>";

            cont = 1;
            while (e != null)
            {
                infoUsuario += "<fieldset><p><strong>Erro:</strong></p>";
                infoUsuario += "<p>" + e.Message + "</p>";
                infoUsuario += "<p><strong>Stack Trace:</strong></p>";
                infoUsuario += "<p>" + e.StackTrace + "</p>";

                String method = "";

                if (e.TargetSite != null)
                {
                    infoUsuario = e.TargetSite.Name;
                }

                infoUsuario += "<p><strong>Method:</strong></p>";
                infoUsuario += "<p>" + method + "</p>";
                infoUsuario += "<p><strong>Source:</strong></p>";
                infoUsuario += "<p>" + e.Source + "</p>";

                e = e.InnerException;
                cont++;
            }
            for (int i = 1; i <= cont; i++)
            {
                mensagem += "</fieldset>";
            }
            infoUsuario = "</fieldset>";
        }

        #endregion

        String gravouErro = "";

        #region Gravar Erro no Banco

        try
        {
            ErroRepository erroRepository = new ErroRepository();

            Erro erro = new Erro();

            erro.Pagina = Request.Url.ToString();
            erro.PaginaAnterior = urlreferrer.ToString();
            erro.RequestHost = Request.UserHostAddress.ToString();
            erro.HostName = Request.UserHostName.ToString();
            erro.UserAgent = Request.UserAgent.ToString();
            erro.InfoUsuario = infoUsuario.ToString();
            erro.ErroMsg = mensagem.ToString();

            erroRepository.Add(erro);
            erroRepository.Save();

            gravouErro = "<font color='blue'>Sim (Erro #" + erro.Id + ")</font>";
        }
        catch (Exception e)
        {
            gravouErro = "<p><font color='red'>Ocorreu um erro ao tentar gravar o erro no banco.</font></p>";
            gravouErro = "<fieldset><legend>Detalhes do Erro</legend>";

            cont = 1;
            while (e != null)
            {
                gravouErro += "<fieldset><p><strong>Erro:</strong></p>";
                gravouErro += "<p>" + e.Message + "</p>";
                gravouErro += "<p><strong>Stack Trace:</strong></p>";
                gravouErro += "<p>" + e.StackTrace + "</p>";

                String method = "";

                if (e.TargetSite != null)
                {
                    gravouErro = e.TargetSite.Name;
                }

                gravouErro += "<p><strong>Method:</strong></p>";
                gravouErro += "<p>" + method + "</p>";
                gravouErro += "<p><strong>Source:</strong></p>";
                gravouErro += "<p>" + e.Source + "</p>";

                e = e.InnerException;
                cont++;
            }
            for (int i = 1; i <= cont; i++)
            {
                mensagem += "</fieldset>";
            }
            gravouErro = "</fieldset>";
        }

        #endregion

        body = body
            .Replace("[data]", Util.Data.Formata(DateTime.Now, Util.Data.FormatoData.Completo))
            .Replace("[pagina]", Request.Url.ToString())
            .Replace("[pagina_anterior]", urlreferrer)
            .Replace("[request_host]", Request.UserHostAddress.ToString())
            .Replace("[host_name]", Request.UserHostName.ToString())
            .Replace("[user_agent]", Request.UserAgent.ToString())
            .Replace("[requisicao_ajax]", (Request.IsAjaxRequest()) ? "Sim" : "Não")
            .Replace("[info_usuario]", infoUsuario)
            .Replace("[gravou_erro]", gravouErro)
            .Replace("[erro_msg]", mensagem);

        Util.Email.Enviar(Util.Email.EmailEnvioPadrao, Util.Email.EmailDesenvolvedor, body, "Ocorreu algum erro");

        #endregion

        if (HttpContext.IsCustomErrorEnabled)
        {
            if (!Request.IsAjaxRequest())
            {
                Redirect("~/erro");
            }
            else
            {
                Content("Ocorreu um erro durante a requisição.");
            }
        }
    }
}