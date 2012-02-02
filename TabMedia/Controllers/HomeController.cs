using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Ionic.Zip;
using CAVEditLib;
using GuiropaIpad.serviceEMS;
using System.Drawing;
using System.Threading;

namespace Controllers
{

    [AuthorizeLogin]
    public class HomeController : BaseController
    {
        ProdutoVaRepository vaRepository = new ProdutoVaRepository();
        ProdutoLinhaRepository linhaRepository = new ProdutoLinhaRepository();
        TerritorioRepository territorioRepository = new TerritorioRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        //
        // GET: /Admin/Home/

        void teste(object sender, Uri url, Bitmap image)
        {
            image.Save("C:/teste.bmp");
        }

        public ActionResult Index()
        {
            //AGENCIA
            if (Sessao.Site.UsuarioInfo.IsAgencia())
            {
                return View("Agencia");
            }
            //GERENTE
            else if (Sessao.Site.UsuarioInfo.IsGerente())
            {
                ViewData["vas"] = vaRepository.GetProdutoVas().Where(v => v.Status == (char)ProdutoVa.EnumStatus.Pendente);

                return RedirectToAction("index","gerente");
            }
            //OUTRO
            else
            {
                ViewData["territorios"] = territorioRepository.GetTerritorios().ToSelectList("Id", "Id");
                ViewData["gerentesProduto"] = usuarioRepository.GetGerentesProduto().ToSelectList("TerritorioSimulado", "Nome");
                ViewData["gerentesMarketing"] = usuarioRepository.GetGerentesMarketing().ToSelectList("TerritorioSimulado", "Nome");
                ViewData["agencias"] = usuarioRepository.GetAgencias().ToSelectList("TerritorioSimulado", "Nome");
                return View();
            }
        }

        [HttpPost]
        public ActionResult GetRelatorioUtilizacaoLinhas()
        {
            var linhas = linhaRepository.GetProdutoLinhasAtivas();

            string result = String.Empty;

            string modeloLinhasTab = Html("GetRelatorioUtilizacaoTabela");
            string modeloLinhasItens = Html("GetRelatorioUtilizacao");

            double total_reps = 0;
            double total_instalacao_ok = 0;
            double total_instalacao_faltam = 0;
            double total_instalacao_porcentagem = 0;
            double total_atualizacao_ok = 0;
            double total_atualizacao_faltam = 0;
            double total_atualizacao_porcentagem = 0;
            double total_sincronizacao_hoje = 0;
            double total_sincronizacao_ontem = 0;
            double total_sincronizacao_ate3dias = 0;
            double total_sincronizacao_mais3dias = 0;

            foreach (var linha in linhas)
            {
                //var territorios = linha.Territorios.Where(t => t.);
                var relatorio = linha.GetRelatorioUtilizacao();

                //relatorioLinha.Utilizacao

                var territorios = linha.Territorios;

                //get html linhas e inclui os produtos
                result += modeloLinhasItens.ReplaceChaves(new
                {
                    linha = linha.Nome,
                    reps = territorios.Count(),
                    instalacao_ok = relatorio.instalacao_ok,
                    instalacao_faltam = relatorio.instalacao_faltam,
                    instalacao_porcentagem = String.Format("{0:F2}", relatorio.instalacao_porcentagem),
                    atualizacao_ok = relatorio.atualizacao_ok,
                    atualizacao_faltam = relatorio.atualizacao_faltam,
                    atualizacao_porcentagem = String.Format("{0:F2}", relatorio.atualizacao_porcentagem),
                    sincronizacao_hoje = relatorio.sincronizacao_hoje,
                    sincronizacao_ontem = relatorio.sincronizacao_ontem,
                    sincronizacao_ate3dias = relatorio.sincronizacao_ate3dias,
                    sincronizacao_mais3dias = relatorio.sincronizacao_mais3dias
                });

                //totalVisualizacoes += relatorioDoutor.QtdeVisualizacoes;
                //totalTempo += (relatorioDoutor.Segundos != null) ? relatorioDoutor.Segundos.Value : 0;
            }

            result = modeloLinhasTab.ReplaceChaves(new
            {
                tabela_conteudo = result,
                total_reps = total_reps,
                total_instalacao_ok = total_instalacao_ok,
                total_instalacao_faltam = total_instalacao_faltam,
                total_instalacao_porcentagem = total_instalacao_porcentagem,
                total_atualizacao_ok = total_atualizacao_ok,
                total_atualizacao_faltam = total_atualizacao_faltam,
                total_atualizacao_porcentagem = total_atualizacao_porcentagem,
                total_sincronizacao_hoje = total_sincronizacao_hoje,
                total_sincronizacao_ontem = total_sincronizacao_ontem,
                total_sincronizacao_ate3dias = total_sincronizacao_ate3dias,
                total_sincronizacao_mais3dias = total_sincronizacao_mais3dias
                //total_visualizacoes = totalVisualizacoes,
                //total_tempo = totalTempo.FromSecondsTo("[M]:[s]")
            });

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        public ActionResult ConverterTerritorioAgenciaGerente()
        {
            TerritorioRepository territorioRepository = new TerritorioRepository();
            UsuarioRepository usuarioRepository = new UsuarioRepository();

            // deleta territorios de agencia e gerentes
            var territorios = territorioRepository.GetTerritorios().Where(t => t.IdUsuario != null);
            foreach (var territorio in territorios)
            {
                var usuario = usuarioRepository.GetUsuario(territorio.IdUsuario.Value);

                usuario.TerritorioSimulado = territorio.Id;

                usuarioRepository.Save();

                territorioRepository.Delete(territorio);
            }

            territorioRepository.Save();

            return Content("ok! :)");

        }

        public ActionResult GetGraficos()
        {
            var linhas = linhaRepository.GetProdutoLinhasAtivas();

            string result = String.Empty;

            string modeloGrafico = Html("GetGrafico");

            var series = "[";
            var categorias = string.Empty;

            var series_instalacao_ok = @"{
                    name: 'Instalação - OK',
                    data: [";

            var series_instalacao_faltam = @"{
                    name: 'Instalação - Faltam',
                    data: [";

            var series_atualizacao_ok = @"{
                    name: 'Atualizacao - OK',
                    data: [";

            var series_atualizacao_faltam = @"{
                    name: 'Atualizacao - Faltam',
                    data: [";

            var series_sincronizacao_hoje = @"{
                    name: 'Sincronização - Hoje',
                    data: [";

            var series_sincronizacao_ontem = @"{
                    name: 'Sincronização - Ontem',
                    data: [";

            var series_sincronizacao_ate3dias = @"{
                    name: 'Sincronização - Até 3 dias',
                    data: [";

            var series_sincronizacao_mais3dias = @"{
                    name: 'Sincronização - Mais de 3 dias',
                    data: [";

            var cont = 1;
            foreach (var linha in linhas)
            {
                //var territorios = linha.Territorios.Where(t => t.);

                //relatorioLinha.Utilizacao

                var territorios = linha.Territorios;
                var relatorio = linha.GetRelatorioUtilizacao();

                categorias += "'" + linha.Nome + "',";

                series_instalacao_ok += relatorio.instalacao_ok;
                series_instalacao_faltam += relatorio.instalacao_faltam;
                series_atualizacao_ok += relatorio.atualizacao_ok;
                series_atualizacao_faltam += relatorio.atualizacao_faltam;
                series_sincronizacao_hoje += relatorio.sincronizacao_hoje;
                series_sincronizacao_ontem += relatorio.sincronizacao_ontem;
                series_sincronizacao_ate3dias += relatorio.sincronizacao_ate3dias;
                series_sincronizacao_mais3dias += relatorio.sincronizacao_mais3dias;

                if (cont != linhas.Count())
                {
                    //se nao for o ultimo
                    series_instalacao_ok += ",";
                    series_instalacao_faltam += ",";
                    series_atualizacao_ok += ",";
                    series_atualizacao_faltam += ",";
                    series_sincronizacao_hoje += ",";
                    series_sincronizacao_ontem +=  ",";
                    series_sincronizacao_ate3dias +=  ",";
                    series_sincronizacao_mais3dias +=  ",";
                }

                cont++;
            }


            series_instalacao_ok += @"]
                        },";
            series_instalacao_faltam += @"]
                        },";
            series_atualizacao_ok += @"]
                        },";
            series_atualizacao_faltam += @"]
                        },";
            series_sincronizacao_hoje += @"]
                        },";
            series_sincronizacao_ontem += @"]
                        },";
            series_sincronizacao_ate3dias += @"]
                        },";
            series_sincronizacao_mais3dias += @"]
                        },";

            series += series_instalacao_ok + series_instalacao_faltam + series_atualizacao_ok + series_atualizacao_faltam + series_sincronizacao_hoje + series_sincronizacao_ontem + series_sincronizacao_ate3dias + series_sincronizacao_mais3dias + "]";

            result = modeloGrafico.ReplaceChaves(new
            {
                titulo = "Utilização da Plataforma",
                titulo_y = "Qtde de Representantes",
                titulo_x = "Linhas",
                subtitulo = "",
                categorias = categorias,
                series = series,
                container = "graficoUtilizacao"
            });

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        public ActionResult Convert()
        {
            CAVConverter converter = null;

            //Create the converter
            converter = new CAVConverter();

            //Set the license key
            converter.SetLicenseKey("Demo", "Demo");

            //Set the log path. If the log path is null, no log output
            converter.LogPath = @"C:\Log\Log.txt";


            //Set output video frame size
            converter.OutputOptions.FrameSize = "640x480";
            //Set output video bitrate
            converter.OutputOptions.VideoBitrate = 2000000;
            //Add convert task
            converter.AddTask(@"C:\SampleVideos\Wildlife.wmv", @"C:\SampleVideos\Wildlife-new.mp4");

            //Start convert and wait for complete
            converter.StartAndWait();

            return Content("ok");
        }

        public ActionResult Teste()
        {
            TerritorioRepository territorioRepository = new TerritorioRepository();
            serviceEMS wsCedat = new serviceEMS();

            var territorios = territorioRepository.GetTerritorios();

            foreach(var territorio in territorios.Take(10)){

                try
                {
                    var infMedico = wsCedat.getCadastroMedico("@Cedat!Service#41101", territorio.Id);

                    foreach (var medico in infMedico.listaDadosMed)
                    {

                        var a = medico.crm;
                        var b = medico.esp;
                        var c = medico.id_medico;
                        var d = medico.nome_medico;
                        var e = medico.uf_crm;

                    }

                }
                catch { }

            }

            return new EmptyResult();
        }

        public ActionResult SimularUsuario(int idUsuario)
        {
            var usuario = usuarioRepository.GetUsuario(idUsuario);

            if (Sessao.Site.Logar(usuario,true))
            {
                usuario.DataUltimoLogin = DateTime.Now;
                usuarioRepository.Save();

                //logs.Add(Log.EnumTipo.Login, "Logou no sistema", Url.Action("index"));    
            }
            
            return Redirect(Autenticacao.GetRedirectFromLoginPage(Util.Sistema.AppSettings.Autenticacao.UrlDefault));

        }

        public ActionResult LogoffUsuarioSimulado()
        {
            Sessao.Site.LogOffUsuarioSimulado();

            return RedirectToAction("index");
        }

    }

}
