using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Models;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;

namespace Controllers
{
    public class RelatoriosController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Relatorios, Log.EnumArea.Site);

        RelatorioPaginaRepository relatorioRepository = new RelatorioPaginaRepository();
        ProdutoRepository produtoRepository = new ProdutoRepository();
        ProdutoLinhaRepository linhaRepository = new ProdutoLinhaRepository();
        ProdutoVaRepository vaRepository = new ProdutoVaRepository();
        TerritorioRepository territorioRepository = new TerritorioRepository();
        RelatorioPaginaRepository relatorioPaginaRepository = new RelatorioPaginaRepository();
        DoutorCadastroRepository doutorCadastroRepository = new DoutorCadastroRepository();

        #region Produtos x Tempo
        
        [AuthorizePermissao]
        public ActionResult Index()
        {
            //return(Content(DateTime.Parse("15/06/2011").Formata(Util.Data.FormatoData.DiaMesAno)));
            logs.Add(Log.EnumTipo.Consulta, "Consultou os Relatórios (Produto x Tempo)", Url.Action("Index"));

            return View();
        }

        [HttpPost]
        [AuthorizePermissao("index")]
        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult GetRelatoriosProdutos(DateTime? dataInicial, DateTime? dataFinal, int? idDoutor)
        {
            var linhas = linhaRepository.GetProdutoLinhas();

            DoutorCadastro doutor = null;

            if (idDoutor != null)
            {
                doutor = doutorCadastroRepository.GetDoutorCadastro(idDoutor.Value);
            }
            
            string result = String.Empty;

            string modeloProdutos = Html("GetRelatoriosProdutos");
            string modeloLinhas = Html("GetRelatoriosProdutosLinhas");            

            foreach (var linha in linhas)
            {
                string strProdutos = string.Empty;

                var produtos = produtoRepository.GetProdutos().Where(p => p.ProdutoLinha.Id == linha.Id);


                var series = @"";

                var series_tempo = @"";

                var categorias = string.Empty;

                categorias += "'" + linha.Nome + "'";

                var cont = 1;
                //get html produtos
                foreach (var produto in produtos)
                {

                    var relatorioProduto = produto.GetRelatorio(dataInicial, dataFinal, idDoutor);

                    strProdutos += modeloProdutos.ReplaceChaves(new {

                        produto = produto.Nome,
                        visualizacoes = relatorioProduto.QtdeVisualizacoes,
                        tempo = relatorioProduto.GetTempoTotal(),
                        tempo_medio = relatorioProduto.GetTempoMedio(),
                        editar = "#?n=relProduto&i=" + produto.Id + ((doutor != null) ? "&d=" + doutor.Id : "")

                    });

                    series_tempo += "{name: '" + produto.Nome + "',y: " + (relatorioProduto.GetMedia()).ToString().Replace(",",".") + "}";

                    series += "{name: '" + produto.Nome + "',y: " + (relatorioProduto.QtdeVisualizacoes) + "}";

                    if (cont != produtos.Count())
                    {
                        //se nao for o ultimo
                        series += ",";
                        series_tempo += ",";

                        //categorias += ",";
                    }

                    cont++;
                }

                //series += "]";

                var relatorioLinha = linha.GetRelatorio(dataInicial, dataFinal, idDoutor);

                //get html linhas e inclui os produtos
                result += modeloLinhas.ReplaceChaves(new {
                    linha = ((doutor != null)? doutor.CRM + doutor.CRMUF + " / ": "") + linha.Nome,
                    linha_id = linha.Id,
                    produtos = strProdutos,
                    total_visualizacoes = relatorioLinha.QtdeVisualizacoes,
                    total_tempo = relatorioLinha.GetTempoTotal(),
                    total_tempo_medio = relatorioLinha.GetTempoMedio(),

                    titulo = "Relatório",
                    titulo_y = "Qtde de Representantes",
                    titulo_x = "Linhas",
                    subtitulo = "",
                    categorias = categorias,
                    grafico_series = series,
                    grafico_series_tempo = series_tempo,
                    container = "graficoUtilizacao"
                });

                
               
            }

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        [HttpPost]
        [AuthorizePermissao("index")]
        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult GetRelatorioProduto(int idProduto, DateTime? dataInicial, DateTime? dataFinal, int? idDoutor)
        {
            var produto = produtoRepository.GetProduto(idProduto);

            if (produto == null)
                return new EmptyResult();

            DoutorCadastro doutor = null;

            if (idDoutor != null)
            {
                doutor = doutorCadastroRepository.GetDoutorCadastro(idDoutor.Value);
            }

            string result = String.Empty;
            
            string modeloVaTabela = Html("GetRelatorioProdutoVasTabela");
            string modeloVas = Html("GetRelatorioProdutoVas");

            var series = @"";

            var series_tempo = @"";

            var categorias = string.Empty;

            categorias += "'" + produto.Nome + "'";

            var cont = 1;

            foreach (var va in produto.ProdutoVas.OrderByDescending(va => va.Versao))
            {
                var relatorioVa = va.GetRelatorio(dataInicial, dataFinal, idDoutor);
                string url_imagem = string.Empty;
                string url_imagem_thumb = string.Empty;
                string add_video = string.Empty;

                var slide = va.ProdutoVaSlides.OrderBy(s => s.Ordem).FirstOrDefault();

                if (slide != null) { 
                    var arquivo = slide.ProdutoVaSlideArquivos.FirstOrDefault();

                    if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg) //IMAGEM
                    {
                        url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo());
                        url_imagem_thumb = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb));
                    }
                    else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Mp4) //VIDEO
                    {
                        url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo());
                        url_imagem_thumb = Util.Url.ResolveUrl("~/images/slide_video.png");
                        add_video = "width=100";
                    }
                    else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Zip) //HTML
                    {
                        url_imagem = string.Empty;
                        url_imagem_thumb = Util.Url.ResolveUrl("~/images/slide_html.png");
                    }
                }

                result += modeloVas.ReplaceChaves(new
                {
                    url_imagem = url_imagem,
                    url_imagem_thumb = url_imagem_thumb,
                    add_video = add_video,
                    va = "Versão " + va.Versao,
                    visualizacoes = relatorioVa.QtdeVisualizacoes,
                    tempo = relatorioVa.GetTempoTotal(),
                    tempo_medio = relatorioVa.GetTempoMedio(),
                    editar = "?n=relVa&i=" + va.Id + ((doutor != null) ? "&d=" + doutor.Id : "")
                });

                series_tempo += "{name: 'Versão " + va.Versao + "',y: " + (relatorioVa.GetMedia()).ToString().Replace(",", ".") + "}";

                series += "{name: 'Versão " + va.Versao + "',y: " + (relatorioVa.QtdeVisualizacoes) + "}";

                if (cont != produto.ProdutoVas.Count())
                {
                    //se nao for o ultimo
                    series += ",";
                    series_tempo += ",";

                    //categorias += ",";
                }

                cont++;
            }

            var relatorioProduto = produto.GetRelatorio(dataInicial, dataFinal, idDoutor);

            result = modeloVaTabela.ReplaceChaves(new {
                linha = ((doutor != null)? doutor.CRM + doutor.CRMUF + " / ": "") + produto.ProdutoLinha.Nome,
                produto = produto.Nome,
                produto_id = produto.Id,
                doutor_id = ((doutor != null)? doutor.Id.ToString() : ""),
                vas = result,
                url_relatorio = Url.Action("GerarExcelProduto", new { idProduto = produto.Id, dataInicial = dataInicial.Formata(Util.Data.FormatoData.DiaMesAno), dataFinal = dataFinal.Formata(Util.Data.FormatoData.DiaMesAno) }),
                total_visualizacoes = relatorioProduto.QtdeVisualizacoes,
                total_tempo = relatorioProduto.GetTempoTotal(),
                total_tempo_medio = relatorioProduto.GetTempoMedio(),

                titulo = "Relatório",
                titulo_y = "Qtde de Representantes",
                titulo_x = "Linhas",
                subtitulo = "",
                categorias = categorias,
                grafico_series = series,
                grafico_series_tempo = series_tempo,
                container = "graficoUtilizacao"
            });

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        [HttpPost]
        [AuthorizePermissao("index")]
        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult GetRelatorioVa(int idVa, DateTime? dataInicial, DateTime? dataFinal, int? idDoutor)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return new EmptyResult();

            DoutorCadastro doutor = null;

            if (idDoutor != null)
            {
                doutor = doutorCadastroRepository.GetDoutorCadastro(idDoutor.Value);
            }

            var series = @"";
            var series_tempo = @"";
            var categorias = string.Empty;
            categorias += "'" + va.Versao + "'";

            string result = String.Empty;

            string modeloVaTabela = Html("GetRelatorioVaTabela");
            string modeloVas = Html("GetRelatorioVa");

            var cont = 1;
            foreach (var slide in va.ProdutoVaSlides.OrderBy(s => s.Ordem))
            {
                var arquivo = slide.ProdutoVaSlideArquivos.First();
                var relatorioSlide = slide.GetRelatorio(dataInicial,dataFinal);

                var especialidades = slide.ProdutoVaSlideEspecialidades.Select(se => se.Especialidade);
                var strEspecialidades = string.Empty;

                foreach (var especialidade in especialidades)
                {

                    strEspecialidades += especialidade.Nome + " ";
                }

                string url_imagem = string.Empty;
                string url_imagem_thumb = string.Empty;
                string video = string.Empty;
                string add_video = string.Empty;

                if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg) //IMAGEM
                {
                    url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo());
                    url_imagem_thumb = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb));
                }
                else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Mp4) //VIDEO
                {
                    url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo());
                    url_imagem_thumb = Util.Url.ResolveUrl("~/images/slide_video.png");
                    video = (arquivo.VideoAutoPlay.HasValue && arquivo.VideoAutoPlay.Value) ? " value=\"" + slide.Id + "\" checked=\"checked\"" : "value=\"" + slide.Id + "\" ";
                    add_video = "width=100";
                }
                else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Zip) //HTML
                {
                    url_imagem = string.Empty;
                    url_imagem_thumb = Util.Url.ResolveUrl("~/images/slide_html.png");
                }

                result += modeloVas.ReplaceChaves(new
                {
                    va = "Versão " + va.Versao,
                    visualizacoes = relatorioSlide.QtdeVisualizacoes,
                    tempo = relatorioSlide.GetTempoTotal(),
                    tempo_medio = relatorioSlide.GetTempoMedio(),
                    cont = cont,
                    especialidades = strEspecialidades,
                    url_imagem = url_imagem,
                    url_imagem_thumb = url_imagem_thumb,
                    video = video,
                    add_video = add_video
                });

                series_tempo += "{name: 'Slide " + cont + "',y: " + (relatorioSlide.GetMedia()).ToString().Replace(",", ".") + "}";

                series += "{name: 'Slide " + cont + "',y: " + (relatorioSlide.QtdeVisualizacoes) + "}";

                if (cont != va.ProdutoVaSlides.Count())
                {
                    //se nao for o ultimo
                    series += ",";
                    series_tempo += ",";

                    //categorias += ",";
                }

                cont++;
            }

            var relatorioVa = va.GetRelatorio(dataInicial, dataFinal);

            result = modeloVaTabela.ReplaceChaves(new
            {
                linha = ((doutor != null) ? doutor.CRM + doutor.CRMUF + " / " : "") + va.Produto.ProdutoLinha.Nome,
                produto = va.Produto.Nome,
                produto_id = va.Produto.Id,
                doutor_id = ((doutor != null) ? doutor.Id.ToString() : ""),
                va = "Versão" + va.Versao,
                va_id = va.Id,
                slides = result,
                url_relatorio = Url.Action("GerarExcelVa", new { idVa = va.Id, dataInicial = dataInicial.Formata(Util.Data.FormatoData.DiaMesAno), dataFinal = dataFinal.Formata(Util.Data.FormatoData.DiaMesAno) }),
                total_visualizacoes = relatorioVa.QtdeVisualizacoes,
                total_tempo = relatorioVa.GetTempoTotal(),
                total_tempo_medio = relatorioVa.GetTempoMedio(),

                titulo = "Relatório",
                titulo_y = "Qtde de Representantes",
                titulo_x = "Linhas",
                subtitulo = "",
                categorias = categorias,
                grafico_series = series,
                grafico_series_tempo = series_tempo,
                container = "graficoUtilizacao"
            });

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult GerarGrafico()
        {
            var produtos = produtoRepository.GetProdutos();

            //Criando o Chart e setando suas propriedades gerais
            Chart grafico = new Chart();
            
            grafico.Width = 2000; // largura
            grafico.Height = 500; // altura
            grafico.RenderType = RenderType.ImageTag; // Tipo da renderização do gráfico.
            grafico.ImageType = ChartImageType.Png;
            grafico.BorderSkin.SkinStyle = BorderSkinStyle.None; // Estilo da Borda
            grafico.BorderColor = System.Drawing.Color.FromArgb(26, 59, 105); // Cor da Borda
            grafico.BorderlineDashStyle = ChartDashStyle.Solid;
            grafico.BorderWidth = 2; // Largura da Borda
            grafico.Font.Bold = true;
            
            //grafico.Palette = ChartColorPalette.BrightPastel; // Paleta para as cores das series
            grafico.BackColor = ColorTranslator.FromHtml("#EBEBEB");
            grafico.AntiAliasing = AntiAliasingStyles.All;
            grafico.TextAntiAliasingQuality = TextAntiAliasingQuality.High;
            grafico.Titles.Add("Produtos x Tempo"); // Título  - Pode haver mais de um título
            
            //grafico.BackGradientStyle = GradientStyle.TopBottom; // Set Gradient Type

            //Adicionando Legenda
            var legenda = grafico.Legends.Add("legend1");
            
            legenda.Title = "Legenda"; //Titulo
            legenda.LegendStyle = LegendStyle.Row; //Estilo da legenda   
            legenda.Docking = Docking.Bottom;

            //Adicionando Area do Gráfico   
            var area = grafico.ChartAreas.Add("Area1");
            area.BackColor = Color.White;
            area.Area3DStyle.Enable3D = false; // Se quiser 3D mude pra true e pronto :D
            area.Area3DStyle.Enable3D = true;
            area.Area3DStyle.IsClustered = true;
            area.Area3DStyle.Inclination = 30;
            area.Area3DStyle.Perspective = 0;
            area.Area3DStyle.Rotation = 10;
            area.AxisX.Interval = 1;
            //area.AxisX.LabelStyle.Font.Size = 10;

            //Adicionando as series e seus respectivos valores
            var serie1 = grafico.Series.Add("serie1");
            var serie2 = grafico.Series.Add("serie2");

            serie1.LegendText = "Visualizações";
            serie2.LegendText = "Tempo";

            serie1.Color = Color.CornflowerBlue;
            serie1.BackSecondaryColor = Color.SkyBlue;
            serie1.BackGradientStyle = GradientStyle.VerticalCenter;
            serie1.BackHatchStyle = ChartHatchStyle.None;

            serie1.BorderColor = Color.Gray;
            serie1.BorderWidth = 1;
            serie1.BorderDashStyle = ChartDashStyle.Solid;
            serie1.ShadowOffset = 2;

            serie2.Color = Color.DarkCyan;
            serie2.BackSecondaryColor = Color.SkyBlue;
            serie2.BackGradientStyle = GradientStyle.VerticalCenter;
            serie2.BackHatchStyle = ChartHatchStyle.None;

            serie2.BorderColor = Color.Gray;
            serie2.BorderWidth = 1;
            serie2.BorderDashStyle = ChartDashStyle.Solid;
            serie2.ShadowOffset = 2;
            
            grafico.Series.Each(s => {
                s.ChartType = SeriesChartType.Column;
                s["DrawingStyle"] = "Cylinder";
            });

            var r = new Random();

            foreach (var produto in produtos.OrderBy(p => p.Nome))
            {
                var relatorio = produto.GetRelatorio();

                DataPoint point = new DataPoint();
                point.AxisLabel = produto.Nome.ToUpper();
                point.YValues = new double[] { r.Next(100) };
                serie1.Points.Add(point);
               

                DataPoint point2 = new DataPoint();
                point2.YValues = new double[] { r.Next(90) };
                serie2.Points.Add(point2);
            }

            //Criando um Memory Stream pra ser passado para o response
            MemoryStream ms = new MemoryStream();

            //Salvando a imagem do gráfico no Memory Stream e definindo o formato da imagem
            grafico.SaveImage(ms, ChartImageFormat.Png);

            //Retorna um FileContentReult para o Response
            return new FileContentResult(ms.ToArray(), "image/png");
        }

        #region GERAR EXCEL
        
        [AuthorizePermissao("index")]
        [OutputCache(CacheProfile = "Relatorios")]
        public FileStreamResult GerarExcelProdutos(string dataInicial, string dataFinal)
        {
            var linhas = linhaRepository.GetProdutoLinhas();

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Produtos");

                if (worksheet != null)
                {
                    worksheet.Cells["A1"].Value = "Produto";
                    worksheet.Cells["B1"].Value = "Visualizações";
                    worksheet.Cells["C1"].Value = "Tempo";

                    worksheet.Cells["A1:C1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:C1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells["A1:C1"].Style.Font.Bold = true;

                    var row = 2;

                    foreach (var linha in linhas)
                    {
                        worksheet.Cells[row,1,row,3].Merge = true;
                        worksheet.Cells[row, 1, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 1].Value = linha.Nome;

                        row++;

                        string strProdutos = string.Empty;

                        //get html produtos
                        foreach (var produto in linha.Produtos)
                        {
                            var relatorioProduto = produto.GetRelatorio(dataInicial, dataFinal);

                            worksheet.Cells[row, 1].Value = produto.Nome;
                            worksheet.Cells[row, 2].Value = relatorioProduto.QtdeVisualizacoes;
                            worksheet.Cells[row, 3].Value = relatorioProduto.GetTempoTotal();

                            row++;
                        }

                        var relatorioLinha = linha.GetRelatorio(dataInicial, dataFinal);

                        worksheet.Cells[row, 1].Value = "TOTAL";
                        worksheet.Cells[row, 2].Value = relatorioLinha.QtdeVisualizacoes;
                        worksheet.Cells[row, 3].Value = relatorioLinha.GetTempoTotal();

                        worksheet.Cells[row, 1, row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[row, 1, row, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[row, 1, row, 3].Style.Font.Bold = true;

                        row ++;

                    }

                    

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "Relatório - Produtos";
                    xlPackage.Workbook.Properties.Author = "Guiropa Ipad 1.0";
                    xlPackage.Workbook.Properties.Subject = "Relatórios";
                    xlPackage.Workbook.Properties.Keywords = "relatório produtos";
                    xlPackage.Workbook.Properties.Category = "Relatórios";
                    xlPackage.Workbook.Properties.Comments = "Relatório de Produtos";

                    // set some extended property values
                    xlPackage.Workbook.Properties.Company = "Guiropa Comunicação";
                    xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.guiropa.com");

                    // set some custom property values
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserId", Sessao.Site.UsuarioInfo.Id.ToString());
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserName", Sessao.Site.UsuarioInfo.Nome.ToString());

                    //Write it back to the client
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
                    //Response.BinaryWrite(xlPackage.GetAsByteArray());

                    worksheet.Column(1).Width = 15;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 15;

                    stream = new MemoryStream(xlPackage.GetAsByteArray());
                }
            }

            logs.Add(Log.EnumTipo.Inclusao, "Fez download de um Relatório (Produtos x Tempo)", Url.Action("Index"));

            return File(stream, "application/xls", "Produtos.xlsx");
        }

        [AuthorizePermissao("index")]
        [OutputCache(CacheProfile = "Relatorios")]
        public FileStreamResult GerarExcelProduto(int idProduto, string dataInicial, string dataFinal)
        {
            var produto = produtoRepository.GetProduto(idProduto);

            if (produto == null)
                return null;

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add(produto.Nome);

                if (worksheet != null)
                {
                    worksheet.Cells["A1"].Value = "VA";
                    worksheet.Cells["B1"].Value = "Visualizações";
                    worksheet.Cells["C1"].Value = "Tempo";

                    worksheet.Cells["A1:C1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:C1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells["A1:C1"].Style.Font.Bold = true;

                    var row = 2;

                    foreach (var va in produto.ProdutoVas.OrderByDescending(va => va.Versao))
                    {
                        var relatorioVa = va.GetRelatorio(dataInicial,dataFinal);

                        worksheet.Cells[row, 1].Value = "Versão " + va.Versao;
                        worksheet.Cells[row, 2].Value = relatorioVa.QtdeVisualizacoes;
                        worksheet.Cells[row, 3].Value = relatorioVa.GetTempoTotal();

                        row++;
                    }

                    var relatorioProduto = produto.GetRelatorio(dataInicial,dataFinal);

                    worksheet.Cells[row, 1].Value = "TOTAL";
                    worksheet.Cells[row, 2].Value = relatorioProduto.QtdeVisualizacoes;
                    worksheet.Cells[row, 3].Value = relatorioProduto.GetTempoTotal();

                    worksheet.Cells[row, 1, row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, 1, row, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells[row, 1, row, 3].Style.Font.Bold = true;

                    row++;

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "Relatório - VA's";
                    xlPackage.Workbook.Properties.Author = "Guiropa Ipad 1.0";
                    xlPackage.Workbook.Properties.Subject = "Relatórios";
                    xlPackage.Workbook.Properties.Keywords = "relatório VA's";
                    xlPackage.Workbook.Properties.Category = "Relatórios";
                    xlPackage.Workbook.Properties.Comments = "Relatório de VA's";

                    // set some extended property values
                    xlPackage.Workbook.Properties.Company = "Guiropa Comunicação";
                    xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.guiropa.com");

                    // set some custom property values
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserId", Sessao.Site.UsuarioInfo.Id.ToString());
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserName", Sessao.Site.UsuarioInfo.Nome.ToString());

                    //Write it back to the client
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
                    //Response.BinaryWrite(xlPackage.GetAsByteArray());

                    worksheet.Column(1).Width = 15;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 15;

                    stream = new MemoryStream(xlPackage.GetAsByteArray());
                }
            }

            logs.Add(Log.EnumTipo.Inclusao, "Fez download de um Relatório (Produtos x Tempo)", Url.Action("Index"));

            return File(stream, "application/xls", "Produtos.xlsx");
        }

        [AuthorizePermissao("index")]
        [OutputCache(CacheProfile = "Relatorios")]
        public FileStreamResult GerarExcelVa(int idVa, string dataInicial, string dataFinal)
        {

            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return null;

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add(va.Produto.Nome);

                if (worksheet != null)
                {
                    worksheet.Cells["A1"].Value = "Slide";
                    worksheet.Cells["B1"].Value = "Especialidades";
                    worksheet.Cells["C1"].Value = "Visualizações";
                    worksheet.Cells["D1"].Value = "Tempo";

                    worksheet.Cells["A1:D1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:D1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells["A1:D1"].Style.Font.Bold = true;

                    var row = 2;
                    var contSlide = 1;

                    foreach (var slide in va.ProdutoVaSlides)
                    {
                        var arquivo = slide.ProdutoVaSlideArquivos.First();
                        var relatorioSlide = slide.GetRelatorio(dataInicial, dataFinal);

                        var especialidades = slide.ProdutoVaSlideEspecialidades.Select(se => se.Especialidade);
                        var strEspecialidades = "Nenhum";

                        foreach (var especialidade in especialidades)
                        {

                            if (strEspecialidades == "Nenhum")
                                strEspecialidades = "";

                            strEspecialidades += especialidade.Nome + " ";
                        }

                        worksheet.Cells[row, 1].Value = "Slide " + contSlide;
                        worksheet.Cells[row, 2].Value = strEspecialidades;
                        worksheet.Cells[row, 3].Value = relatorioSlide.QtdeVisualizacoes;
                        worksheet.Cells[row, 4].Value = relatorioSlide.GetTempoTotal();

                        row++;
                        contSlide++;
                    }

                    var relatorioVa = va.GetRelatorio(dataInicial,dataFinal);

                    worksheet.Cells[row, 2].Value = "TOTAL";
                    worksheet.Cells[row, 3].Value = relatorioVa.QtdeVisualizacoes;
                    worksheet.Cells[row, 4].Value = relatorioVa.GetTempoTotal();

                    worksheet.Cells[row, 1, row, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, 1, row, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;

                    row++;

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "Relatório - " +va.Produto.Nome+ " - Versão " + va.Versao;
                    xlPackage.Workbook.Properties.Author = "Guiropa Ipad 1.0";
                    xlPackage.Workbook.Properties.Subject = "Relatórios";
                    xlPackage.Workbook.Properties.Keywords = "relatório va";
                    xlPackage.Workbook.Properties.Category = "Relatórios";
                    xlPackage.Workbook.Properties.Comments = "Relatório do VA";

                    // set some extended property values
                    xlPackage.Workbook.Properties.Company = "Guiropa Comunicação";
                    xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.guiropa.com");

                    // set some custom property values
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserId", Sessao.Site.UsuarioInfo.Id.ToString());
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserName", Sessao.Site.UsuarioInfo.Nome.ToString());

                    //Write it back to the client
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
                    //Response.BinaryWrite(xlPackage.GetAsByteArray());

                    worksheet.Column(1).Width = 15;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 15;

                    stream = new MemoryStream(xlPackage.GetAsByteArray());
                }
            }

            logs.Add(Log.EnumTipo.Inclusao, "Fez download de um Relatório (Produtos x Tempo)", Url.Action("Index"));

            return File(stream, "application/xls", "Produtos.xlsx");
        }

        #endregion

        #endregion

        #region Territórios x Download

        [AuthorizePermissao]
        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult Territorios()
        {
            logs.Add(Log.EnumTipo.Consulta, "Consultou os Relatórios (Territórios x Download)", Url.Action("Territorios"));

            return View();
        }

        [AuthorizePermissao("territorios")]
        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult GetTerritoriosLinhas()
        {
            var linhas = linhaRepository.GetProdutoLinhas();

            string result = String.Empty;

            string modeloLinhas = Html("GetTerritoriosLinhas");
            string modeloLinhasContainer = Html("GetTerritoriosLinhasContainer");

            foreach (var linha in linhas)
            {

                result += modeloLinhas.ReplaceChaves(new
                {
                    linha_id = linha.Id,
                    linha_nome = linha.Nome,
                    url = "javascript:GetTerritoriosLinha(" + linha.Id + ")"

                });

            }

            result = modeloLinhasContainer.ReplaceChaves(new
            {
                linhas = result
            });

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        [AuthorizePermissao("territorios")]
        [HttpPost]
        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult GetTerritoriosLinha(int idLinha)
        {
            var linha = linhaRepository.GetProdutoLinha(idLinha);
            var territorios = territorioRepository.GetTerritorios();

            string result = String.Empty;

            string modeloTabela = Html("GetTerritoriosLinhaTabela");
            string modeloTerritorios = Html("GetTerritoriosLinha");

            var strThProdutos = string.Empty;

            foreach (var produto in linha.Produtos)
            {
                strThProdutos += "<th>" + produto.Nome + "</th>";
            }

            foreach (var territorio in territorios)
            {
                var strTdProdutos = string.Empty;

                foreach (var produto in linha.Produtos)
                {
                    var downloadProduto = territorio.TerritorioProdutoVaDownloads.Where(d => d.ProdutoVa.Produto.Id == produto.Id).OrderByDescending(d => d.Data).FirstOrDefault();

                    strTdProdutos += "<td>";

                    if (downloadProduto != null)
                    {    

                        var versaoBaixada = downloadProduto.ProdutoVa.Versao.Value.ToString();
                        var versaoAtual = string.Empty;

                        var vaAtivo = downloadProduto.ProdutoVa.Produto.GetVAAtivo();

                        if (vaAtivo != null)
                        {
                            versaoAtual = vaAtivo.Versao.ToString();
                        }

                        strTdProdutos += versaoBaixada + "(" + versaoAtual + ")";

                    }

                    strTdProdutos += "</td>";
                }
                
                var territorioDownload = territorio.TerritorioProdutoVaDownloads.OrderByDescending(d => d.Data).FirstOrDefault();

                result += modeloTerritorios.ReplaceChaves(new
                {
                    territorio_id = territorio.Id,
                    territorio_ultimaConexao = (territorio.DataUltimaSincronizacao != null)?territorio.DataUltimaSincronizacao.Value.DateDiff("dd", DateTime.Now).ToString() : "",
                    territorio_ultimoDownload = (territorioDownload != null)?territorioDownload.Data.Value.DateDiff("dd",DateTime.Now).ToString():"",
                    produtos = strTdProdutos
                });
            }

            result = modeloTabela.ReplaceChaves(new
            {
                linha = linha.Nome,
                linha_id = linha.Id,
                produtos = strThProdutos,
                territorios = result,
                url_relatorio = Url.Action("GerarExcelTerritoriosLinha", new { idLinha = linha.Id })
            });

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        #region GERAR EXCEL

        [AuthorizePermissao("territorios")]
        [OutputCache(CacheProfile = "Relatorios")]
        public FileStreamResult GerarExcelTerritoriosLinha(int idLinha)
        {
            var territorios = territorioRepository.GetTerritorios().Where(t => t.IdLinha == idLinha);
            var produtos = produtoRepository.GetProdutos();

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Produtos");

                if (worksheet != null)
                {
                    worksheet.Cells["A1"].Value = "Território";
                    worksheet.Cells["B1"].Value = "Última Conexão";

                    var cont = 2;

                    foreach (var produto in produtos)
                    {
                        cont++;
                        
                        worksheet.Cells[1, cont].Value = produto.Nome;
                    }

                    worksheet.Cells[1, 1, 1, cont].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1, 1, cont].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, 1, 1, cont].Style.Font.Bold = true;

                    var row = 2;

                    foreach (var territorio in territorios)
                    {
                        var territorioDownload = territorio.TerritorioProdutoVaDownloads.OrderByDescending(d => d.Data).FirstOrDefault();

                        worksheet.Cells[row, 1].Value = territorio.Id;
                        worksheet.Cells[row, 2].Value = (territorio.DataUltimaSincronizacao != null) ? territorio.DataUltimaSincronizacao.Value.DateDiff("dd", DateTime.Now).ToString() : "";

                        cont = 2;

                        var strTdProdutos = string.Empty;

                        foreach (var produto in produtos)
                        {
                            cont++;

                            var downloadProduto = territorio.TerritorioProdutoVaDownloads.Where(d => d.ProdutoVa.Produto.Id == produto.Id).OrderByDescending(d => d.Data).FirstOrDefault();

                            worksheet.Cells[row, cont].Value = ((downloadProduto != null) ? downloadProduto.ProdutoVa.Versao.Value.ToString() + "(" + downloadProduto.ProdutoVa.Produto.GetVAAtivo().Versao + ")" : "");
                        }

                        row++;
                    }

                    row++;

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "Relatório - VA's";
                    xlPackage.Workbook.Properties.Author = "Guiropa Ipad 1.0";
                    xlPackage.Workbook.Properties.Subject = "Relatórios";
                    xlPackage.Workbook.Properties.Keywords = "relatório VA's";
                    xlPackage.Workbook.Properties.Category = "Relatórios";
                    xlPackage.Workbook.Properties.Comments = "Relatório de VA's";

                    // set some extended property values
                    xlPackage.Workbook.Properties.Company = "Guiropa Comunicação";
                    xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.guiropa.com");

                    // set some custom property values
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserId", Sessao.Site.UsuarioInfo.Id.ToString());
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserName", Sessao.Site.UsuarioInfo.Nome.ToString());

                    //Write it back to the client
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
                    //Response.BinaryWrite(xlPackage.GetAsByteArray());

                    worksheet.Column(1).Width = 15;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 15;

                    stream = new MemoryStream(xlPackage.GetAsByteArray());
                }
            }

            logs.Add(Log.EnumTipo.Inclusao, "Fez download de um Relatório (Territórios x Download)", Url.Action("Territorios"));

            return File(stream, "application/xls", "Produtos.xlsx");
        }

        #endregion

        #endregion

        //#region Doutor x Tempo

        [AuthorizePermissao]
        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult Doutores()
        {
            //return(Content(DateTime.Parse("15/06/2011").Formata(Util.Data.FormatoData.DiaMesAno)));
            return View();
        }

        [HttpPost]
        [AuthorizePermissao("index")]
        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult GetRelatoriosDoutores(DateTime? dataInicial, DateTime? dataFinal)
        {
            var doutores = relatorioPaginaRepository.GetRelatorioPaginas().Where(rp => rp.DoutorCadastro != null).Select(rp => rp.DoutorCadastro).Distinct();
            var linhas = linhaRepository.GetProdutoLinhas();

            string result = String.Empty;

            string modeloDoutoresTabela = Html("GetRelatorioDoutoresTabela");
            string modeloDoutores = Html("GetRelatorioDoutores");

            double totalVisualizacoes = 0;
            double totalTempo = 0;

            foreach (var doutor in doutores)
            {
                var relatorioDoutor = doutor.GetRelatorio(dataInicial,dataFinal);

                //get html linhas e inclui os produtos
                result += modeloDoutores.ReplaceChaves(new
                {
                    doutor = doutor.CRM + doutor.CRMUF,
                    visualizacoes = relatorioDoutor.QtdeVisualizacoes,
                    tempo = relatorioDoutor.GetTempoTotal(),
                    tempo_medio = relatorioDoutor.GetTempoMedio(),
                    editar = "#?n=relDoutor&i=" + doutor.Id
                });

                totalVisualizacoes += relatorioDoutor.QtdeVisualizacoes;
                totalTempo += (relatorioDoutor.Segundos != null)?relatorioDoutor.Segundos.Value:0;
            }

            result = modeloDoutoresTabela.ReplaceChaves(new
            {
                doutores = result,
                total_visualizacoes = totalVisualizacoes,
                total_tempo = totalTempo.FromSecondsTo("[M]:[s]"),
                total_tempo_medio = (totalVisualizacoes > 0)?(totalTempo / totalVisualizacoes).FromSecondsTo("[M]:[s]"):"-"
            });

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        [AuthorizePermissao("index")]
        [OutputCache(CacheProfile = "Relatorios")]
        public FileStreamResult GerarExcelDoutores()
        {
            var doutores = doutorCadastroRepository.GetDoutorCadastros().Where(d => d.RelatorioPaginas.Count() > 0);
            var produtos = produtoRepository.GetProdutos().ToList().Where(p => p.GetVAAtivo() != null);

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Produtos");

                if (worksheet != null)
                {

                    worksheet.Cells["A1:A2"].Merge = true;
                    worksheet.Cells["A1"].Value = "Crm";
                    worksheet.Cells["B1:B2"].Merge = true;
                    worksheet.Cells["B1"].Value = "CrmUf";
                    worksheet.Cells["C1:C2"].Merge = true;
                    worksheet.Cells["C1"].Value = "Territorio";

                    var cont = 3;
                    var cor1 = Color.FromArgb(220, 220, 220);
                    var cor2 = Color.FromArgb(240, 240, 240);
                    var alternado = true;

                    foreach (var produto in produtos)
                    {
                        cont++;

                        worksheet.Cells[1, cont].Value = produto.Nome;

                        //pega o va ativo
                        var va = produto.GetVAAtivo();

                        var contSlide = 0;
                        foreach (var slide in va.ProdutoVaSlides)
                        {
                            contSlide++;

                            //escreve o numero de cada slide
                            worksheet.Cells[2, (cont+contSlide-1)].Value = contSlide.ToString();
                        }
                        contSlide++;
                        worksheet.Cells[2, (cont + contSlide -1)].Value = "Total";

                        //merge nas celulas do produto
                        worksheet.Cells[1, cont, 1, cont + contSlide -1].Merge = true;
                        //pinta as celulas alternadamente pra cada produtos
                        worksheet.Cells[1, cont, 2, cont + contSlide - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, cont, 2, cont + contSlide - 1].Style.Fill.BackgroundColor.SetColor((alternado)?cor1:cor2);

                        worksheet.Cells[1, cont, 2, cont + contSlide - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[1, cont, 2, cont + contSlide - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        
                        alternado = (alternado) ? false : true;

                        cont += contSlide - 1;
                    }

                    //worksheet.Cells[1, 1, 2, cont].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //worksheet.Cells[1, 1, 2, cont].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, 1, 2, cont].Style.Font.Bold = true;

                    var row = 3;

                    foreach (var doutor in doutores)
                    {

                        worksheet.Cells[row, 1].Value = doutor.CRM;
                        worksheet.Cells[row, 2].Value = doutor.CRMUF;
                        worksheet.Cells[row, 3].Value = doutor.IdTerritorio;

                        cont = 3;

                        var strTdProdutos = string.Empty;

                        alternado = true;

                        foreach (var produto in produtos)
                        {
                            cont++;

                            var va = produto.GetVAAtivo();
                            var relatorioVa = va.GetRelatorio(null,null, doutor.Id);

                            var contSlide = 0;
                            foreach (var slide in va.ProdutoVaSlides)
                            {
                                var relatorio = slide.GetRelatorio(null,null,doutor.Id);
                                contSlide++;

                                worksheet.Cells[row, (cont + contSlide - 1)].Value = relatorio.Segundos.FromSecondsTo("[M]:[s]");
                            }
                            contSlide++;
                            worksheet.Cells[row, (cont + contSlide - 1)].Value = relatorioVa.Segundos.FromSecondsTo("[M]:[s]");

                            //pinta as celulas alternadamente pra cada produtos
                            worksheet.Cells[row, cont, row, cont + contSlide - 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, cont, row, cont + contSlide - 1].Style.Fill.BackgroundColor.SetColor((alternado) ? cor1 : cor2);

                            worksheet.Cells[row, cont, row, cont + contSlide - 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, cont, row, cont + contSlide - 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, cont, row, cont + contSlide - 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[row, cont, row, cont + contSlide - 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                            alternado = (alternado) ? false : true;

                            cont += contSlide - 1;
                        }

                        row++;
                    }

                    row++;

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "Relatório - Doutores";
                    xlPackage.Workbook.Properties.Author = "Guiropa Tab 1.0";
                    xlPackage.Workbook.Properties.Subject = "Relatórios";
                    xlPackage.Workbook.Properties.Keywords = "relatório doutores";
                    xlPackage.Workbook.Properties.Category = "Relatórios";
                    xlPackage.Workbook.Properties.Comments = "Relatório de doutores";

                    // set some extended property values
                    xlPackage.Workbook.Properties.Company = "Guiropa Comunicação";
                    xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.guiropa.com");

                    // set some custom property values
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserId", Sessao.Site.UsuarioInfo.Id.ToString());
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserName", Sessao.Site.UsuarioInfo.Nome.ToString());

                    //Write it back to the client
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
                    //Response.BinaryWrite(xlPackage.GetAsByteArray());

                    worksheet.Column(1).Width = 15;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 15;

                    stream = new MemoryStream(xlPackage.GetAsByteArray());
                }
            }

            logs.Add(Log.EnumTipo.Inclusao, "Fez download de um Relatório (Doutores)", Url.Action("Doutores"));

            return File(stream, "application/xls", "relatorio_doutores.xlsx");
        }


        [AuthorizePermissao]
        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult Versao()
        {
            //return(Content(DateTime.Parse("15/06/2011").Formata(Util.Data.FormatoData.DiaMesAno)));
            return View();
        }

        [HttpPost]
        [AuthorizePermissao("index")]
        [OutputCache(CacheProfile = "Relatorios")]
        public ActionResult GetRelatorioVersao(DateTime? dataInicial, DateTime? dataFinal)
        {
            var territorios = territorioRepository.GetTerritorios();

            string result = String.Empty;

            string modeloTabela = Html("GetRelatorioTerritorioVersaoTabela");
            string modelo = Html("GetRelatorioTerritorioVersao");

            foreach (var territorio in territorios)
            {
                //get html linhas e inclui os produtos
                result += modelo.ReplaceChaves(new
                {
                    territorio_id = territorio.Id,
                    territorio_versao = territorio.AppVersion
                });
            }

            result = modeloTabela.ReplaceChaves(new
            {
                territorios = result,
                url_relatorio = Url.Action("GerarExcelVersao")
            });

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        [AuthorizePermissao("index")]
        [OutputCache(CacheProfile = "Relatorios")]
        public FileStreamResult GerarExcelVersao()
        {
            var territorios = territorioRepository.GetTerritorios();

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Territorios");

                if (worksheet != null)
                {
                    worksheet.Cells["A1"].Value = "Territorio";
                    worksheet.Cells["B1"].Value = "Versão";

                    var cont = 2;
                    var cor1 = Color.FromArgb(220, 220, 220);
                    var cor2 = Color.FromArgb(240, 240, 240);

                    foreach (var territorio in territorios)
                    {
                        worksheet.Cells[cont, 1].Value = territorio.Id;
                        worksheet.Cells[cont, 2].Value = territorio.AppVersion;

                        cont++;
                    }

                    worksheet.Cells[1, 1, 1, 2].Style.Font.Bold = true;

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "Relatório - Territorio x Versão";
                    xlPackage.Workbook.Properties.Author = "Guiropa Tab 1.0";
                    xlPackage.Workbook.Properties.Subject = "Relatórios";
                    xlPackage.Workbook.Properties.Keywords = "relatório doutores";
                    xlPackage.Workbook.Properties.Category = "Relatórios";
                    xlPackage.Workbook.Properties.Comments = "Relatório de doutores";

                    // set some extended property values
                    xlPackage.Workbook.Properties.Company = "Guiropa Comunicação";
                    xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.guiropa.com");

                    // set some custom property values
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserId", Sessao.Site.UsuarioInfo.Id.ToString());
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserName", Sessao.Site.UsuarioInfo.Nome.ToString());

                    //Write it back to the client
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
                    //Response.BinaryWrite(xlPackage.GetAsByteArray());

                    worksheet.Column(1).Width = 15;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 15;

                    stream = new MemoryStream(xlPackage.GetAsByteArray());
                }
            }

            logs.Add(Log.EnumTipo.Inclusao, "Fez download de um Relatório (Território x Versão)", Url.Action("Versao"));

            return File(stream, "application/xls", "relatorio_territorio_versao.xlsx");
        }

        /*
        [HttpPost]
        [AuthorizePermissao("index")]
        public ActionResult GetRelatorioVa(int idVa, DateTime? dataInicial, DateTime? dataFinal)
        {
            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return new EmptyResult();

            string result = String.Empty;

            string modeloVaTabela = Html("GetRelatorioVaTabela");
            string modeloVas = Html("GetRelatorioVa");

            var cont = 1;
            foreach (var slide in va.ProdutoVaSlides)
            {
                var arquivo = slide.ProdutoVaSlideArquivos.First();
                var relatorioSlide = slide.GetRelatorio(dataInicial, dataFinal);

                var especialidades = slide.ProdutoVaSlideEspecialidades.Select(se => se.Especialidade);
                var strEspecialidades = "Nenhum";

                foreach (var especialidade in especialidades)
                {

                    if (strEspecialidades == "Nenhum")
                        strEspecialidades = "";

                    strEspecialidades += especialidade.Nome + " ";
                }

                string url_imagem = string.Empty;
                string url_imagem_thumb = string.Empty;
                string video = string.Empty;
                string add_video = string.Empty;

                if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg) //IMAGEM
                {
                    url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo());
                    url_imagem_thumb = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb));
                }
                else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Mp4) //VIDEO
                {
                    url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo());
                    url_imagem_thumb = Util.Url.ResolveUrl("~/images/slide_video.png");
                    video = (arquivo.VideoAutoPlay.HasValue && arquivo.VideoAutoPlay.Value) ? " value=\"" + slide.Id + "\" checked=\"checked\"" : "value=\"" + slide.Id + "\" ";
                    add_video = "width=100";
                }
                else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Zip) //HTML
                {
                    url_imagem = string.Empty;
                    url_imagem_thumb = Util.Url.ResolveUrl("~/images/slide_html.png");
                }

                result += modeloVas.ReplaceChaves(new
                {
                    va = "Versão " + va.Versao,
                    visualizacoes = relatorioSlide.QtdeVisualizacoes,
                    tempo = relatorioSlide.GetTempoTotal(),
                    cont = cont,
                    especialidades = strEspecialidades,
                    url_imagem = url_imagem,
                    url_imagem_thumb = url_imagem_thumb,
                    video = video,
                    add_video = add_video
                });

                cont++;
            }

            var relatorioVa = va.GetRelatorio(dataInicial, dataFinal);

            result = modeloVaTabela.ReplaceChaves(new
            {
                linha = va.Produto.ProdutoLinha.Nome,
                produto = va.Produto.Nome,
                produto_id = va.Produto.Id,
                va = "Versão" + va.Versao,
                slides = result,
                url_relatorio = Url.Action("GerarExcelVa", new { idVa = va.Id, dataInicial = dataInicial.Formata(Util.Data.FormatoData.DiaMesAno), dataFinal = dataFinal.Formata(Util.Data.FormatoData.DiaMesAno) }),
                total_visualizacoes = relatorioVa.QtdeVisualizacoes,
                total_tempo = relatorioVa.GetTempoTotal()
            });

            // Merge the list of PhotoItem types with the UserControl
            // to render HTML and return it to the jQuery call.
            return Content(result);
        }

        #region GERAR EXCEL

        [AuthorizePermissao("index")]
        public FileStreamResult GerarExcelProdutos(string dataInicial, string dataFinal)
        {
            var linhas = linhaRepository.GetProdutoLinhas();

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Produtos");

                if (worksheet != null)
                {
                    worksheet.Cells["A1"].Value = "Produto";
                    worksheet.Cells["B1"].Value = "Visualizações";
                    worksheet.Cells["C1"].Value = "Tempo";

                    worksheet.Cells["A1:C1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:C1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells["A1:C1"].Style.Font.Bold = true;

                    var row = 2;

                    foreach (var linha in linhas)
                    {
                        worksheet.Cells[row, 1, row, 3].Merge = true;
                        worksheet.Cells[row, 1, row, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[row, 1].Value = linha.Nome;

                        row++;

                        string strProdutos = string.Empty;

                        //get html produtos
                        foreach (var produto in linha.Produtos)
                        {
                            Produto.Relatorio relatorioProduto = produto.GetRelatorio(dataInicial, dataFinal);

                            worksheet.Cells[row, 1].Value = produto.Nome;
                            worksheet.Cells[row, 2].Value = relatorioProduto.QtdeVisualizacoes;
                            worksheet.Cells[row, 3].Value = relatorioProduto.GetTempoTotal();

                            row++;
                        }

                        var relatorioLinha = linha.GetRelatorio();

                        worksheet.Cells[row, 1].Value = "TOTAL";
                        worksheet.Cells[row, 2].Value = relatorioLinha.QtdeVisualizacoes;
                        worksheet.Cells[row, 3].Value = relatorioLinha.GetTempoTotal();

                        worksheet.Cells[row, 1, row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[row, 1, row, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        worksheet.Cells[row, 1, row, 3].Style.Font.Bold = true;

                        row++;

                    }



                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "Relatório - Produtos";
                    xlPackage.Workbook.Properties.Author = "Guiropa Ipad 1.0";
                    xlPackage.Workbook.Properties.Subject = "Relatórios";
                    xlPackage.Workbook.Properties.Keywords = "relatório produtos";
                    xlPackage.Workbook.Properties.Category = "Relatórios";
                    xlPackage.Workbook.Properties.Comments = "Relatório de Produtos";

                    // set some extended property values
                    xlPackage.Workbook.Properties.Company = "Guiropa Comunicação";
                    xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.guiropa.com");

                    // set some custom property values
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserId", Sessao.Site.UsuarioInfo.Id.ToString());
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserName", Sessao.Site.UsuarioInfo.Nome.ToString());

                    //Write it back to the client
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
                    //Response.BinaryWrite(xlPackage.GetAsByteArray());

                    worksheet.Column(1).Width = 15;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 15;

                    stream = new MemoryStream(xlPackage.GetAsByteArray());
                }
            }

            return File(stream, "application/xls", "Produtos.xlsx");
        }

        [AuthorizePermissao("index")]
        public FileStreamResult GerarExcelProduto(int idProduto, string dataInicial, string dataFinal)
        {
            var produto = produtoRepository.GetProduto(idProduto);

            if (produto == null)
                return null;

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add(produto.Nome);

                if (worksheet != null)
                {
                    worksheet.Cells["A1"].Value = "VA";
                    worksheet.Cells["B1"].Value = "Visualizações";
                    worksheet.Cells["C1"].Value = "Tempo";

                    worksheet.Cells["A1:C1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:C1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells["A1:C1"].Style.Font.Bold = true;

                    var row = 2;

                    foreach (var va in produto.ProdutoVas.OrderByDescending(va => va.Versao))
                    {
                        var relatorioVa = va.GetRelatorio(dataInicial, dataFinal);

                        worksheet.Cells[row, 1].Value = "Versão " + va.Versao;
                        worksheet.Cells[row, 2].Value = relatorioVa.QtdeVisualizacoes;
                        worksheet.Cells[row, 3].Value = relatorioVa.GetTempoTotal();

                        row++;
                    }

                    var relatorioProduto = produto.GetRelatorio(dataInicial, dataFinal);

                    worksheet.Cells[row, 1].Value = "TOTAL";
                    worksheet.Cells[row, 2].Value = relatorioProduto.QtdeVisualizacoes;
                    worksheet.Cells[row, 3].Value = relatorioProduto.GetTempoTotal();

                    worksheet.Cells[row, 1, row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, 1, row, 3].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells[row, 1, row, 3].Style.Font.Bold = true;

                    row++;

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "Relatório - VA's";
                    xlPackage.Workbook.Properties.Author = "Guiropa Ipad 1.0";
                    xlPackage.Workbook.Properties.Subject = "Relatórios";
                    xlPackage.Workbook.Properties.Keywords = "relatório VA's";
                    xlPackage.Workbook.Properties.Category = "Relatórios";
                    xlPackage.Workbook.Properties.Comments = "Relatório de VA's";

                    // set some extended property values
                    xlPackage.Workbook.Properties.Company = "Guiropa Comunicação";
                    xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.guiropa.com");

                    // set some custom property values
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserId", Sessao.Site.UsuarioInfo.Id.ToString());
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserName", Sessao.Site.UsuarioInfo.Nome.ToString());

                    //Write it back to the client
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
                    //Response.BinaryWrite(xlPackage.GetAsByteArray());

                    worksheet.Column(1).Width = 15;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 15;

                    stream = new MemoryStream(xlPackage.GetAsByteArray());
                }
            }

            return File(stream, "application/xls", "Produtos.xlsx");
        }

        [AuthorizePermissao("index")]
        public FileStreamResult GerarExcelVa(int idVa, string dataInicial, string dataFinal)
        {

            var va = vaRepository.GetProdutoVa(idVa);

            if (va == null)
                return null;

            MemoryStream stream = new MemoryStream();

            using (ExcelPackage xlPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add(va.Produto.Nome);

                if (worksheet != null)
                {
                    worksheet.Cells["A1"].Value = "Slide";
                    worksheet.Cells["B1"].Value = "Especialidades";
                    worksheet.Cells["C1"].Value = "Visualizações";
                    worksheet.Cells["D1"].Value = "Tempo";

                    worksheet.Cells["A1:D1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells["A1:D1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells["A1:D1"].Style.Font.Bold = true;

                    var row = 2;
                    var contSlide = 1;

                    foreach (var slide in va.ProdutoVaSlides)
                    {
                        var arquivo = slide.ProdutoVaSlideArquivos.First();
                        var relatorioSlide = slide.GetRelatorio(dataInicial, dataFinal);

                        var especialidades = slide.ProdutoVaSlideEspecialidades.Select(se => se.Especialidade);
                        var strEspecialidades = "Nenhum";

                        foreach (var especialidade in especialidades)
                        {

                            if (strEspecialidades == "Nenhum")
                                strEspecialidades = "";

                            strEspecialidades += especialidade.Nome + " ";
                        }

                        worksheet.Cells[row, 1].Value = "Slide " + contSlide;
                        worksheet.Cells[row, 2].Value = strEspecialidades;
                        worksheet.Cells[row, 3].Value = relatorioSlide.QtdeVisualizacoes;
                        worksheet.Cells[row, 4].Value = relatorioSlide.GetTempoTotal();

                        row++;
                        contSlide++;
                    }

                    var relatorioVa = va.GetRelatorio(dataInicial, dataFinal);

                    worksheet.Cells[row, 2].Value = "TOTAL";
                    worksheet.Cells[row, 3].Value = relatorioVa.QtdeVisualizacoes;
                    worksheet.Cells[row, 4].Value = relatorioVa.GetTempoTotal();

                    worksheet.Cells[row, 1, row, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, 1, row, 4].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    worksheet.Cells[row, 1, row, 4].Style.Font.Bold = true;

                    row++;

                    // set some core property values
                    xlPackage.Workbook.Properties.Title = "Relatório - " + va.Produto.Nome + " - Versão " + va.Versao;
                    xlPackage.Workbook.Properties.Author = "Guiropa Ipad 1.0";
                    xlPackage.Workbook.Properties.Subject = "Relatórios";
                    xlPackage.Workbook.Properties.Keywords = "relatório va";
                    xlPackage.Workbook.Properties.Category = "Relatórios";
                    xlPackage.Workbook.Properties.Comments = "Relatório do VA";

                    // set some extended property values
                    xlPackage.Workbook.Properties.Company = "Guiropa Comunicação";
                    xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.guiropa.com");

                    // set some custom property values
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserId", Sessao.Site.UsuarioInfo.Id.ToString());
                    xlPackage.Workbook.Properties.SetCustomPropertyValue("UserName", Sessao.Site.UsuarioInfo.Nome.ToString());

                    //Write it back to the client
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
                    //Response.BinaryWrite(xlPackage.GetAsByteArray());

                    worksheet.Column(1).Width = 15;
                    worksheet.Column(2).Width = 15;
                    worksheet.Column(3).Width = 15;

                    stream = new MemoryStream(xlPackage.GetAsByteArray());
                }
            }

            return File(stream, "application/xls", "Produtos.xlsx");
        }

        #endregion

        #endregion*/
    }
}
