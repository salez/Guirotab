using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using System.Net.Mail;

namespace Controllers
{
    
    public class SistemaController : BaseController
    {
        CustomConfigRepository customConfigRepository = new CustomConfigRepository();
        //
        // GET: /Sistema/

        [AuthorizeLogin]
        [AuthorizeConfig]
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeLogin]
        [AuthorizeConfig]
        public ActionResult Editar()
        {
            CustomConfig config = customConfigRepository.GetConfig();

            return View(config);
        }

        [AuthorizeLogin]
        [HttpPost]
        [AuthorizeConfig]
        public ActionResult Editar(CustomConfig config)
        {
            customConfigRepository.UpdateConfig(config);

            return RedirectToAction("index");
        }

        [AuthorizeLogin]
        public ActionResult Login()
        {
            if (Sessao.Config.AcessoBloqueado)
            {
                return RedirectToAction("acessobloqueado");
            }
            return View();
        }

        [AuthorizeLogin]
        [HttpPost]
        public ActionResult Login(String senha)
        {
            if (Sessao.Config.AcessoBloqueado)
            {
                return RedirectToAction("acessobloqueado");
            }

            if (Sessao.Config.Logar(senha))
            {
                return Redirect(Autenticacao.GetRedirectFromLoginPage(Util.Configuracao.AppSettings("UrlDefaultAdmin")));
            }
            else
            {
                ViewData["erro"] = "senha inválida";
            }

            return View();
        }

        [AuthorizeLogin]
        public ActionResult AcessoBloqueado()
        {
            return View();
        }

        public ActionResult EnviarEmails()
        {
            RelatorioEmailRepository relatorioEmailRepository = new RelatorioEmailRepository();
            int cont = 0, erros = 0;

            var relatorioEmails = relatorioEmailRepository.GetRelatorioEmails().Where(r => r.Status == (char)RelatorioEmail.EnumStatus.Pendente);

            foreach (var relatorioEmail in relatorioEmails)
            {
                try
                {
                    if (relatorioEmail.IdProdutoVaArquivo != null) { 
                        var corpo = Util.Email.GetCorpoEmail("logEmail");

                        corpo = corpo.ReplaceChaves(new
                        {
                            nome_arquivo = relatorioEmail.ProdutoVaArquivo.Nome,
                            link = Util.Sistema.SiteUrl + Util.Url.ResolveUrl(relatorioEmail.ProdutoVaArquivo.GetCaminho())
                        });
                       
                        Util.Email.Enviar(relatorioEmail.Email, corpo, "Arquivo - " + relatorioEmail.ProdutoVaArquivo.Nome.HtmlEncode());

                        relatorioEmail.Status = (char)RelatorioEmail.EnumStatus.Enviado;
                        cont++;
                    }
                }
                catch (Exception e)
                {
                    Util.Sistema.Error.TrataErro(e, Request);

                    relatorioEmail.Status = (char)RelatorioEmail.EnumStatus.Erro;
                    erros++;
                }
            }

            relatorioEmailRepository.Save();

            return Content("Enviados:" + cont + "/Erros:" + erros);
        }

    }
}
