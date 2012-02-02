using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Data;
using Models;

namespace GuiropaIpad.Controllers
{
    [AuthorizeLogin]
    public class ImportacaoController : BaseController
    {
        Util.Logs logs = new Util.Logs(Log.EnumPagina.Importacao, Log.EnumArea.Site);

        ProdutoRepository produtoRepository = new ProdutoRepository();
        ProdutoLinhaRepository linhaRepository = new ProdutoLinhaRepository();
        EspecialidadeRepository especialidadeRepository = new EspecialidadeRepository();
        UsuarioRepository usuarioRepository = new UsuarioRepository();
        GrupoRepository grupoRepository = new GrupoRepository();
        TerritorioRepository territorioRepository = new TerritorioRepository();
        DoutorRepository doutorRepository = new DoutorRepository();
        DoutorCadastroRepository doutorCadastroRepository = new DoutorCadastroRepository();

        static String diretorioImportacao = Util.Sistema.AppSettings.Diretorios.DiretorioImportacao;
        static String caminhoImportacao = diretorioImportacao + "importacao.xls";
        static String caminhoImportacaoTerritorios = diretorioImportacao + "importacaoTerritorios.xlsx";
        static String caminhoImportacaoFisico = Util.Url.GetCaminhoFisico(caminhoImportacao);
        static String caminhoImportacaoTerritoriosFisico = Util.Url.GetCaminhoFisico(caminhoImportacaoTerritorios);
        static String caminhoImportacaoOrdemProdutos = diretorioImportacao + "importacaoOrdemProdutos.xls";
        static String caminhoImportacaoOrdemProdutosFisico = Util.Url.GetCaminhoFisico(caminhoImportacaoOrdemProdutos);
        static String caminhoImportacaoDoutores = diretorioImportacao + "importacaoDoutores.xls";
        static String caminhoImportacaoDoutoresFisico = Util.Url.GetCaminhoFisico(caminhoImportacaoDoutores);
        static String caminhoImportacaoGerentes = diretorioImportacao + "importacaoGerentesRep.xls";
        static String caminhoImportacaoGerentesFisico = Util.Url.GetCaminhoFisico(caminhoImportacaoDoutores);

        public ActionResult Index() {

            logs.Add(Log.EnumTipo.Consulta, "Consultou a Importação", Url.Action("Index"));

            return View();

        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase fileData)
        {
            try
            {
                Util.Arquivo.CreateDirectoryIfNotExists(Util.Url.GetCaminhoFisico(diretorioImportacao));

                fileData.SaveAs(caminhoImportacaoFisico);
            }
            catch (Exception ex)
            {

                Util.Sistema.Error.TrataErro(ex, Request);

                return Content("1: Erro ao fazer o upload do arquivo: " + ex.Message);

            }

            return Content("[ok]");

        }

        [HttpPost]
        public ActionResult VerificaExcel()
        {

            List<string> erros = new List<string>();
            OleDbDataAdapter adapter = null;

            try
            {
                // ler excel
                string strConn;
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + caminhoImportacaoFisico + ";Extended Properties=Excel 8.0;";

                //You must use the $ after the object you reference in the spreadsheet
                adapter = new OleDbDataAdapter("SELECT * FROM [Todos$]", strConn);
            
            }
            catch
            {
                erros.Add("O arquivo enviado não é um arquivo de excel válido.");
            }

            DataSet myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            int count = 1;

            ////////////////////////////////
            ////// VALIDAÇÃO DO EXCEL //////
            ////////////////////////////////

            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                count++; //início na linha 2 do Excel
                try
                {
                    row["Produto"].ToString();
                    row["Linha"].ToString();
                    //row["Especialidade"].ToString();
                    row["GP"].ToString();
                    row["GM"].ToString();
                    row["Agencia"].ToString();
                }
                catch (Exception e)
                {
                    erros.Add(e.Message + "colunas do excel má formatadas, verifique o nome e a ordem no modelo de exemplo.");
                    break;
                }

                try
                {
                    string produto;
                    string linha;
                    //string[] especialidades;
                    string gerente;
                    string agencia;

                }
                catch
                {
                    erros.Add("erro não identificado - linha " + count);
                }
            }

            if (erros.Count >= 1)
            {
                //deleta o arquivo e o registro do banco caso haja algum erro no excel
                System.IO.File.Delete(caminhoImportacaoFisico);

                string msg = String.Empty;

                foreach (string erro in erros)
                {
                    msg += erro + "<br/>";
                }

                logs.Add(Log.EnumTipo.Inclusao, "Verificação Importação de Gerentes/Agências: haviam erros na planilha.", Url.Action("Index"));

                return Content(msg);
            }
            else
            {
                logs.Add(Log.EnumTipo.Inclusao, "Importação de Gerentes/Agências: planilha aprovada.", Url.Action("Index"));
                return Content("[ok]");
            }

        }

        public ActionResult ImportarExcel()
        {
            List<string> erros = new List<string>();

            // ler excel
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + caminhoImportacaoFisico + ";Extended Properties=Excel 8.0;";
            //You must use the $ after the object you reference in the spreadsheet
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Todos$]", strConn);

            DataSet myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            /////////////////////////////////
            ////// IMPORTAÇÃO DO EXCEL //////
            /////////////////////////////////

            UsuarioRepository usuarioRelacaoRepository = new UsuarioRepository();
            usuarioRelacaoRepository.DeleteAllRelacoesUsuarioProduto();
            usuarioRelacaoRepository.DeleteAllRelacoesUsuarioGerenteAgencia();
            usuarioRelacaoRepository.InativaGerentesAgencias();

            usuarioRelacaoRepository.Save();

            try
            {
                string idGrupoGerente = grupoRepository.GetGrupo(Usuario.EnumTipo.GerenteProduto.GetDescription()).Id;
                string idGrupoGerenteMarketing = grupoRepository.GetGrupo(Usuario.EnumTipo.GerenteMarketing.GetDescription()).Id;
                string idGrupoAgencia = grupoRepository.GetGrupo(Usuario.EnumTipo.Agencia.GetDescription()).Id;

                foreach (DataRow row in myDataSet.Tables[0].Rows)
                {
                    bool temEspecialidade = true;
                    try{
                        temEspecialidade = (row["Especialidade"] != null);
                    }catch{
                        temEspecialidade = false;
                    }

                    string nomeProduto = row["Produto"].ToString().Trim();
                    string nomeLinha = row["Linha"].ToString().Trim();

                    string[] nomesEspecialidades = null;

                    if (temEspecialidade) { 
                        nomesEspecialidades = row["Especialidade"].ToString().Trim().Split('/');
                        nomesEspecialidades.Each(s => s.Trim());
                    }

                    string nomeGerente = row["GP"].ToString();
                    string nomeGerenteMarketing = row["GM"].ToString();
                    string[] nomesAgencias = row["Agencia"].ToString().Trim().Split('/');

                    if (nomeProduto == null || nomeProduto == String.Empty)
                    {
                        break;
                    }

                    Produto produto = null;

                    #region Produto

                        if (nomeProduto != String.Empty)
                        {
                            var produtoExistente = produtoRepository.GetProduto(nomeProduto);
                            bool produtoExiste = (produtoExistente != null);

                            if (!produtoExiste)
                            {
                                produto = new Produto();
                            }
                            else
                            {
                                produto = produtoExistente;
                            }

                            produto.Nome = nomeProduto;

                            if (!produtoExiste)
                            {
                                //adiciona produto caso nao exista
                                produtoRepository.Add(produto);
                            }

                            produtoRepository.Save(); 
                        }

                    #endregion

                    #region Linha
                        
                        if (nomeLinha != String.Empty)
                        {
                            var linhaExistente = linhaRepository.GetProdutoLinha(nomeLinha);
                            bool linhaExiste = (linhaExistente != null);

                            ProdutoLinha linha = null;

                            if (!linhaExiste)
                            {
                                linha = new ProdutoLinha();
                            }
                            else
                            {
                                linha = linhaExistente;
                            }

                            linha.Nome = nomeLinha;

                            if (!linhaExiste)
                            {
                                //adiciona produto caso nao exista
                                linhaRepository.Add(linha);
                            }

                            linhaRepository.Save();

                            //relaciona linha com produto
                            produto.IdLinha = linha.Id;

                            produtoRepository.Save();
                        }

                    #endregion

                    #region Especialidades

                        if (nomesEspecialidades != null && nomesEspecialidades.Count() > 0)
                        {
                            nomesEspecialidades.Each(e => e = e.Trim());

                            foreach (string e in nomesEspecialidades)
                            {
                                string nomeEspecialidade = e.Trim();

                                var especialidadeExistente = especialidadeRepository.GetEspecialidade(nomeEspecialidade);
                                bool especialidadeExiste = (especialidadeExistente != null);

                                Especialidade especialidade = null;

                                if (!especialidadeExiste)
                                {
                                    especialidade = new Especialidade();
                                }
                                else
                                {
                                    especialidade = especialidadeExistente;
                                }
                                
                                especialidade.Nome = nomeEspecialidade;

                                if (!especialidadeExiste)
                                {
                                    //adiciona produto caso nao exista
                                    especialidadeRepository.Add(especialidade);
                                }

                                especialidadeRepository.Save();

                            }

                        }
                    
                    #endregion

                    Usuario gerente = null;
                    Usuario gerenteMarketing = null;

                    #region Gerente

                        if (nomeGerente != String.Empty)
                        {
                            var gerenteExistente = usuarioRepository.GetGerente(nomeGerente);
                            bool gerenteExiste = (gerenteExistente != null);

                            if (!gerenteExiste)
                            {
                                gerente = new Usuario();
                                gerente.IdGrupo = idGrupoGerente;
                            }
                            else
                            {
                                gerente = gerenteExistente;
                            }
                            
                            gerente.Nome = nomeGerente;
                            gerente.Status = (char)Usuario.EnumStatus.Ativo;

                            if (!gerenteExiste)
                            {
                                //adiciona produto caso nao exista
                                usuarioRepository.Add(gerente);
                            }

                            usuarioRepository.Save();

                            //relaciona produto com gerente
                            gerente.AddProduto(usuarioRepository, produto);

                            usuarioRepository.Save();
                        }

                    #endregion

                    #region Agencia

                        if (nomesAgencias != null && nomesAgencias.Count() > 0)
                        {
                            foreach (var a in nomesAgencias)
                            {
                                string nomeAgencia = a.Trim();

                                if (nomeAgencia != String.Empty && nomeAgencia != "NÃO TEM CAMPANHA")
                                {
                                    var agenciaExistente = usuarioRepository.GetAgencia(nomeAgencia);
                                    bool agenciaExiste = (agenciaExistente != null);

                                    Usuario agencia = null;

                                    if (!agenciaExiste)
                                    {
                                        agencia = new Usuario();
                                        agencia.IdGrupo = idGrupoAgencia;
                                    }
                                    else
                                    {
                                        agencia = agenciaExistente;
                                    }

                                    agencia.Nome = nomeAgencia;
                                    agencia.Status = (char)Usuario.EnumStatus.Ativo;

                                    if (!agenciaExiste)
                                    {
                                        //adiciona produto caso nao exista
                                        usuarioRepository.Add(agencia);
                                    }

                                    usuarioRepository.Save();

                                    //relaciona agencia com gerente
                                    gerente.AddAgencia(usuarioRepository, agencia);

                                    //relaciona produto com agencia
                                    agencia.AddProduto(usuarioRepository, produto);

                                    usuarioRepository.Save();
                                }
                            }
                        }

                    #endregion

                    #region Gerente Marketing

                    if (nomeGerenteMarketing != String.Empty)
                    {
                        var gerenteMarketingExistente = usuarioRepository.GetGerenteMarketing(nomeGerenteMarketing);
                        bool gerenteMarketingExiste = (gerenteMarketingExistente != null);

                        if (!gerenteMarketingExiste)
                        {
                            gerenteMarketing = new Usuario();
                            gerenteMarketing.IdGrupo = idGrupoGerenteMarketing;
                        }
                        else
                        {
                            gerenteMarketing = gerenteMarketingExistente;
                        }

                        gerenteMarketing.Nome = nomeGerenteMarketing;
                        gerenteMarketing.Status = (char)Usuario.EnumStatus.Ativo;

                        if (!gerenteMarketingExiste)
                        {
                            //adiciona produto caso nao exista
                            usuarioRepository.Add(gerenteMarketing);
                        }

                        usuarioRepository.Save();

                        //relaciona produto com gerente
                        gerenteMarketing.AddProduto(usuarioRepository, produto);

                        usuarioRepository.Save();
                    }

                    #endregion

                }

                //comita delete de todas as relações
                usuarioRelacaoRepository.Save();

            }
            catch (Exception e)
            {
                Util.Sistema.Error.TrataErro(e, Request);

                erros.Add("Ocorreu um erro durante a importação");
            }


            #region Gerente

            adapter = new OleDbDataAdapter("SELECT * FROM [Gerentes$]", strConn);

            myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                string gerenteNome = row["Nome"].ToString().Trim();
                string gerenteEmail = row["Email"].ToString().Trim();
                string gerenteSenha = row["Senha"].ToString().Trim();

                if (gerenteNome == null || gerenteNome == String.Empty)
                {
                    break;
                }

                Usuario gerente = null;

                if (gerenteNome != String.Empty)
                {
                    gerente = usuarioRepository.GetGerente(gerenteNome);
                    bool gerenteExiste = (gerente != null);

                    if (gerenteExiste)
                    {
                        gerente.Nome = gerenteNome;
                        gerente.Email = gerenteEmail;
                        gerente.Senha = gerenteSenha.ToMD5();
                    } 
                }

            }

            usuarioRepository.Save();

            #endregion

            #region Gerente Marketing

            adapter = new OleDbDataAdapter("SELECT * FROM [GerentesMarketing$]", strConn);

            myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                string gerenteNome = row["Nome"].ToString().Trim();
                string gerenteEmail = row["Email"].ToString().Trim();
                string gerenteSenha = row["Senha"].ToString().Trim();

                if (gerenteNome == null || gerenteNome == String.Empty)
                {
                    break;
                }

                Usuario gerente = null;

                if (gerenteNome != String.Empty)
                {
                    gerente = usuarioRepository.GetGerenteMarketing(gerenteNome);
                    bool gerenteExiste = (gerente != null);

                    if (gerenteExiste)
                    {
                        gerente.Nome = gerenteNome;
                        gerente.Email = gerenteEmail;
                        gerente.Senha = gerenteSenha.ToMD5();
                    }
                }

            }

            usuarioRepository.Save();

            #endregion

            #region Agencia

            adapter = new OleDbDataAdapter("SELECT * FROM [Agencias$]", strConn);

            myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                string agenciaNome = row["AGENCIA"].ToString().Trim();
                string agenciaEmail = row["EMAIL"].ToString().Trim();
                string agenciaSenha = row["SENHA"].ToString().Trim();

                if (agenciaNome == null || agenciaNome == String.Empty)
                {
                    break;
                }

                Usuario agencia = null;

                if (agenciaNome != String.Empty)
                {
                    agencia = usuarioRepository.GetAgencia(agenciaNome);
                    bool agenciaExiste = (agencia != null);

                    if (agenciaExiste)
                    {
                        agencia.Nome = agenciaNome;
                        agencia.Email = agenciaEmail;
                        agencia.Senha = agenciaSenha.ToMD5();
                    }
                }
            }

            usuarioRepository.Save();

            #endregion

            CriarTerritoriosAgenciaGerente();

            if (erros.Count >= 1)
            {
                //deleta o arquivo e o registro do banco caso haja algum erro no excel
                System.IO.File.Delete(caminhoImportacaoFisico);

                string msg = String.Empty;

                foreach (string erro in erros)
                {
                    msg += erro + "<br/>";
                }

                logs.Add(Log.EnumTipo.Inclusao, "Importação de Gerentes/Agências: Ocorreu algum erro durante a importação.", Url.Action("Index"));

                return Content(msg);
            }
            else
            {
                
                logs.Add(Log.EnumTipo.Inclusao, "Importação de Gerentes/Agências: importação concluída.", Url.Action("Index"));

                return Content("[ok]");
            }

        }

        [HttpPost]
        public ActionResult UploadTerritorios(HttpPostedFileBase fileData)
        {
            try
            {
                Util.Arquivo.CreateDirectoryIfNotExists(Util.Url.GetCaminhoFisico(diretorioImportacao));

                fileData.SaveAs(caminhoImportacaoTerritoriosFisico);
            }
            catch (Exception ex)
            {

                Util.Sistema.Error.TrataErro(ex, Request);

                return Content("1: Erro ao fazer o upload do arquivo: " + ex.Message);

            }

            return Content("[ok]");

        }

        [HttpPost]
        public ActionResult VerificaExcelTerritorios()
        {

            List<string> erros = new List<string>();
            OleDbDataAdapter adapter = null;
            // ler excel
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + caminhoImportacaoTerritoriosFisico + ";Extended Properties=Excel 8.0;";

            OleDbConnection oledbConn = new OleDbConnection(strConn);

            //try
            //{
                
                // Open connection
                oledbConn.Open();

                // Create OleDbCommand object and select data from worksheet Sheet1
                OleDbCommand cmd = new OleDbCommand("SELECT DISTINCT LINHA,TERRITORIO,'' AS NOME, '' AS TIPO FROM [Plan1$]", oledbConn);

                adapter = new OleDbDataAdapter();

                adapter.SelectCommand = cmd;

            //}
            //catch
            //{
            //    erros.Add("O arquivo enviado não é um arquivo de excel válido.");
            //}

            DataSet myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            int count = 1;

            ////////////////////////////////
            ////// VALIDAÇÃO DO EXCEL //////
            ////////////////////////////////

            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                count++; //início na linha 2 do Excel
                try
                {
                    row["Territorio"].ToString();
                    row["Nome"].ToString();
                    row["Linha"].ToString();
                }
                catch (Exception e)
                {
                    erros.Add(e.Message + "colunas do excel má formatadas, verifique o nome e a ordem no modelo de exemplo.");
                    break;
                }

                try
                {
                    string territorio = row["Territorio"].ToString().Trim();
                    string nome = row["Nome"].ToString().Trim();
                    string linha = row["Linha"].ToString().Trim();

                    var linhabd = linhaRepository.GetProdutoLinhas().Where(l => l.Nome == linha).FirstOrDefault();

                    if (linhabd == null)
                    {
                        erros.Add("Linha \""+linha+"\" não encontrada.");
                        break;
                    }

                }
                catch
                {
                    erros.Add("erro não identificado - linha " + count);
                }
            }

            if (erros.Count >= 1)
            {
                oledbConn.Close();

                //deleta o arquivo e o registro do banco caso haja algum erro no excel
                System.IO.File.Delete(caminhoImportacaoTerritoriosFisico);

                string msg = String.Empty;

                foreach (string erro in erros)
                {
                    msg += erro + "<br/>";
                }

                logs.Add(Log.EnumTipo.Inclusao, "Verificação Importação de Territórios: haviam erros na planilha.", Url.Action("Index"));
                

                return Content(msg);
            }
            else
            {
                logs.Add(Log.EnumTipo.Inclusao, "Verificação Importação de Territórios: planilha aprovada.", Url.Action("Index"));

                oledbConn.Close();

                return Content("[ok]");
            }

        }

        public ActionResult ImportarExcelTerritorios()
        {

            List<string> erros = new List<string>();
            OleDbDataAdapter adapter = null;
            // ler excel
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + caminhoImportacaoTerritoriosFisico + ";Extended Properties=Excel 8.0;";

            OleDbConnection oledbConn = new OleDbConnection(strConn);

            //try
            //{

            // Open connection
            oledbConn.Open();

            // Create OleDbCommand object and select data from worksheet Sheet1
            OleDbCommand cmd = new OleDbCommand("SELECT DISTINCT LINHA,TERRITORIO,NOME,SENHA FROM [Plan1$]", oledbConn);

            adapter = new OleDbDataAdapter();

            adapter.SelectCommand = cmd;

            //}
            //catch
            //{
            //    erros.Add("O arquivo enviado não é um arquivo de excel válido.");
            //}

            DataSet myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            /////////////////////////////////
            ////// IMPORTAÇÃO DO EXCEL //////
            /////////////////////////////////


            try
            {
                foreach (DataRow row in myDataSet.Tables[0].Rows)
                {
                    string idTerritorio = row["Territorio"].ToString().Trim();
                    string nomeTerritorio = row["Nome"].ToString().Trim();
                    string linhaTerritorio = row["Linha"].ToString().Trim();
                    string senhaTerritorio = row["Senha"].ToString().Trim();

                    Territorio territorio = null;

                    #region Territorio

                    if (idTerritorio != String.Empty)
                    {
                        var territorioExistente = territorioRepository.GetTerritorio(idTerritorio);
                        bool territorioExiste = (territorioExistente != null);

                        if (!territorioExiste)
                        {
                            territorio = new Territorio();
                        }
                        else
                        {
                            territorio = territorioExistente;
                        }

                        territorio.Id = idTerritorio;
                        territorio.Nome = nomeTerritorio;
                        territorio.Status = (char)Territorio.EnumStatus.Ativo;
                        territorio.Senha = senhaTerritorio.ToMD5();

                        #region Linha

                        if (linhaTerritorio != String.Empty)
                        {
                            var linhaExistente = linhaRepository.GetProdutoLinha(linhaTerritorio);
                            bool linhaExiste = (linhaExistente != null);

                            ProdutoLinha linha = null;

                            if (!linhaExiste)
                            {
                                linha = new ProdutoLinha();
                            }
                            else
                            {
                                linha = linhaExistente;
                            }

                            linha.Nome = linhaTerritorio;

                            if (!linhaExiste)
                            {
                                //adiciona produto caso nao exista
                                linhaRepository.Add(linha);
                            }

                            linhaRepository.Save();

                            territorio.IdLinha = linha.Id;
                        }

                        #endregion

                        if (!territorioExiste)
                        {
                            //adiciona territorio caso nao exista
                            territorioRepository.Add(territorio);
                        }

                        territorioRepository.Save();
                    }

                    #endregion

                }

            }
            catch (Exception e)
            {
                Util.Sistema.Error.TrataErro(e, Request);

                erros.Add("Ocorreu um erro durante a importação");
            }

            if (erros.Count >= 1)
            {
                oledbConn.Close();
                //deleta o arquivo e o registro do banco caso haja algum erro no excel
                System.IO.File.Delete(caminhoImportacaoTerritoriosFisico);

                string msg = String.Empty;

                foreach (string erro in erros)
                {
                    msg += erro + "<br/>";
                }

                logs.Add(Log.EnumTipo.Inclusao, "Verificação Importação de Territórios: ocorreu algum erro durante a importação.", Url.Action("Index"));

                return Content(msg);
            }
            else
            {
                oledbConn.Close();
                logs.Add(Log.EnumTipo.Inclusao, "Verificação Importação de Territórios: importação concluída.", Url.Action("Index"));

                return Content("[ok]");
            }

        }

        [HttpPost]
        public ActionResult UploadOrdemProdutos(HttpPostedFileBase fileData)
        {
            try
            {
                Util.Arquivo.CreateDirectoryIfNotExists(Util.Url.GetCaminhoFisico(diretorioImportacao));

                fileData.SaveAs(caminhoImportacaoOrdemProdutosFisico);
            }
            catch (Exception ex)
            {

                Util.Sistema.Error.TrataErro(ex, Request);

                return Content("1: Erro ao fazer o upload do arquivo: " + ex.Message);

            }

            return Content("[ok]");

        }

        [HttpPost]
        public ActionResult VerificaExcelOrdemProdutos()
        {

            List<string> erros = new List<string>();
            OleDbDataAdapter adapter = null;

            try
            {
                // ler excel
                string strConn;
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + caminhoImportacaoOrdemProdutosFisico + ";Extended Properties=Excel 8.0;";

                //You must use the $ after the object you reference in the spreadsheet
                adapter = new OleDbDataAdapter("SELECT * FROM [Plan1$]", strConn);

            }
            catch
            {
                erros.Add("O arquivo enviado não é um arquivo de excel válido.");
            }

            DataSet myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            int count = 1;

            ////////////////////////////////
            ////// VALIDAÇÃO DO EXCEL //////
            ////////////////////////////////

            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                count++; //início na linha 2 do Excel
                try
                {
                    row["Linha"].ToString();
                    row["Especialidade"].ToString();
                    for (int i = 1; i <= 7; i++) { 
                        row["p" + i].ToString();
                    }
                }
                catch (Exception e)
                {
                    erros.Add(e.Message + "<br><br>ERRO: Colunas do excel má formatadas, verifique o nome e a ordem no modelo de exemplo.");
                    break;
                }

                try
                {
                    string especialidade = row["Especialidade"].ToString().Trim();
                    string linha = row["Linha"].ToString().Trim();

                    var linhabd = linhaRepository.GetProdutoLinhas().Where(l => l.Nome == linha).FirstOrDefault();

                    if (linhabd == null)
                    {
                        erros.Add("Linha \"" + linha + "\" não encontrada.");
                        break;
                    }

                    var especialidadebd = especialidadeRepository.GetEspecialidade(especialidade);

                    if (especialidadebd == null)
                    {
                        erros.Add("Especialidade \"" + especialidade + "\" não encontrada.");
                        break;
                    }

                    for (int i = 1; i <= 8; i++)
                    {
                        string produto = row["p" + i.ToString()].ToString().Trim();

                        if (!String.IsNullOrEmpty(produto)) { 
                            var produtobd = produtoRepository.GetProduto(produto);

                            if (produtobd == null)
                            {
                                erros.Add("Produto \"" + produto + "\" não encontrada.");
                                break;
                            }
                        }
                    }

                }
                catch(Exception e)
                {
                    Util.Sistema.Error.TrataErro(e);

                    erros.Add("erro não identificado - linha " + count + "<br><br>ERRO:"+e.Message);
                }
            }

            if (erros.Count >= 1)
            {
                //deleta o arquivo e o registro do banco caso haja algum erro no excel
                System.IO.File.Delete(caminhoImportacaoOrdemProdutosFisico);

                string msg = String.Empty;

                foreach (string erro in erros)
                {
                    msg += erro + "<br/>";
                }

                logs.Add(Log.EnumTipo.Inclusao, "Verificação Importação de Ordem Produtos: haviam erros na planilha.", Url.Action("Index"));

                return Content(msg);
            }
            else
            {
                logs.Add(Log.EnumTipo.Inclusao, "Verificação Importação de Ordem Produtos: planilha aprovada.", Url.Action("Index"));

                return Content("[ok]");
            }

        }

        public ActionResult ImportarExcelOrdemProdutos()
        {
            List<string> erros = new List<string>();

            // ler excel
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + caminhoImportacaoOrdemProdutosFisico + ";Extended Properties=Excel 8.0;";
            //You must use the $ after the object you reference in the spreadsheet
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Plan1$]", strConn);

            DataSet myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            /////////////////////////////////
            ////// IMPORTAÇÃO DO EXCEL //////
            /////////////////////////////////

            int count = 1;

            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                string especialidade = row["Especialidade"].ToString().Trim();
                string linha = row["Linha"].ToString().Trim();

                //verifica se existe "doutor" com akela linha e especialidade, se nao houver cria
                Doutor doutor = doutorRepository.GetDoutors().Where(d => d.DoutorEspecialidades.Any(e => e.Especialidade.Nome == especialidade) && d.ProdutoLinha.Nome == linha).FirstOrDefault();

                if (doutor != null)
                {
                    doutorRepository.DeleteProdutosEespecialidades(doutor);
                }

                if (doutor == null)
                {
                    doutor = new Doutor();
                    doutorRepository.Add(doutor);
                }

                count++; //início na linha 2 do Excel

                doutor.Nome = "Doutor" + count;
                doutorRepository.Save();

                try
                {
                    row["Linha"].ToString();
                    row["Especialidade"].ToString();
                    for (int i = 1; i <= 7; i++) { 
                        row["p" + i.ToString()].ToString();
                    }
                }
                catch (Exception e)
                {
                    erros.Add(e.Message + "colunas do excel má formatadas, verifique o nome e a ordem no modelo de exemplo.");
                    break;
                }

                try
                {

                    var linhabd = linhaRepository.GetProdutoLinhas().Where(l => l.Nome == linha).FirstOrDefault();

                    if (linhabd == null)
                    {
                        erros.Add("Linha \"" + linha + "\" não encontrada.");
                        break;
                    }

                    doutor.IdProdutoLinha = linhabd.Id;

                    var especialidadebd = especialidadeRepository.GetEspecialidade(especialidade);

                    if (especialidadebd == null)
                    {
                        erros.Add("Especialidade \"" + especialidade + "\" não encontrada.");
                        break;
                    }
                    
                    doutor.AddEspecialidade(especialidadebd);

                    for (int i = 1; i <= 8; i++)
                    {
                        string produto = row["p" + i.ToString()].ToString().Trim();

                        if (!String.IsNullOrEmpty(produto)) {

                            var produtobd = produtoRepository.GetProduto(produto);

                            if (produtobd == null)
                            {
                                erros.Add("Produto \"" + produto + "\" não encontrada.");
                                break;
                            }

                            doutor.AddProduto(produtobd, i);
                        }
                    }

                    doutorRepository.Save();

                }
                catch(Exception e)
                {
                    Util.Sistema.Error.TrataErro(e);

                    erros.Add("erro não identificado");
                }
            }

            if (erros.Count >= 1)
            {
                //deleta o arquivo e o registro do banco caso haja algum erro no excel
                System.IO.File.Delete(caminhoImportacaoTerritoriosFisico);

                string msg = String.Empty;

                foreach (string erro in erros)
                {
                    msg += erro + "<br/>";
                }

                logs.Add(Log.EnumTipo.Inclusao, "Importação de Ordem Produtos: ocorreu algum erro durante a importação.", Url.Action("Index"));

                return Content(msg);
            }
            else
            {
                logs.Add(Log.EnumTipo.Inclusao, "Importação de Ordem Produtos: importação concluída.", Url.Action("Index"));

                return Content("[ok]");
            }

        }

        //cria os territorios para agencias e gerentes para que eles possam acessar o sistema Ipad e testar >> sim, POG MASTER!
        public void CriarTerritoriosAgenciaGerente()
        {
            //RelatorioPaginaRepository paginaRepository = new RelatorioPaginaRepository();

            // deleta territorios de agencia e gerentes
            var territorios = territorioRepository.GetTerritorios().Where(t => t.IdUsuario != null); 
            foreach(var territorio in territorios){
                territorioRepository.Delete(territorio);
            }
            territorioRepository.Save();

            var agencias = usuarioRepository.GetAgencias();
            var gerentes = usuarioRepository.GetGerentesProduto();
            var gerentesMarketing = usuarioRepository.GetGerentesMarketing();

            // cria territorios agencias/gerentes
            var cont = 1;
            foreach (var agencia in agencias)
            {
                agencia.TerritorioSimulado = "a" + agencia.Id.ToString();

                cont++;
            }

            foreach (var gerente in gerentes)
            {
                gerente.TerritorioSimulado = "g" + gerente.Id.ToString();

                cont++;
            }

            foreach (var gerente in gerentesMarketing)
            {
                gerente.TerritorioSimulado = "g" + gerente.Id.ToString();

                cont++;
            }

            territorioRepository.Save();
        }

        [HttpPost]
        public ActionResult UploadDoutores(HttpPostedFileBase fileData)
        {
            try
            {
                Util.Arquivo.CreateDirectoryIfNotExists(Util.Url.GetCaminhoFisico(diretorioImportacao));

                fileData.SaveAs(caminhoImportacaoDoutoresFisico);
            }
            catch (Exception ex)
            {

                Util.Sistema.Error.TrataErro(ex, Request);

                return Content("1: Erro ao fazer o upload do arquivo: " + ex.Message);

            }

            return Content("[ok]");

        }

        [HttpPost]
        public ActionResult VerificaExcelDoutores()
        {

            List<string> erros = new List<string>();
            OleDbDataAdapter adapter = null;

            try
            {
                // ler excel
                string strConn;
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + caminhoImportacaoDoutoresFisico + ";Extended Properties=Excel 8.0;";

                //You must use the $ after the object you reference in the spreadsheet
                adapter = new OleDbDataAdapter("SELECT * FROM [Plan1$]", strConn);

            }
            catch
            {
                erros.Add("O arquivo enviado não é um arquivo de excel válido.");
            }

            DataSet myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            int count = 1;

            ////////////////////////////////
            ////// VALIDAÇÃO DO EXCEL //////
            ////////////////////////////////

            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                count++; //início na linha 2 do Excel
                try
                {
                    row["CODIGO"].ToString();
                    row["SETOR"].ToString();
                    row["UFCRM"].ToString();
                    row["CRM"].ToString();
                    row["NOME"].ToString();
                    row["ESP"].ToString();
                }
                catch (Exception e)
                {
                    erros.Add(e.Message + "<br><br>ERRO: Colunas do excel má formatadas, verifique o nome e a ordem no modelo de exemplo.");
                    break;
                }
                break;
                //try
                //{
                //    string especialidade = row["ESP"].ToString().Trim();

                //    var especialidadebd = especialidadeRepository.GetEspecialidade(especialidade);

                //    if (especialidadebd == null)
                //    {
                //        erros.Add("Especialidade \"" + especialidade + "\" não encontrada.");
                //        break;
                //    }

                //    string territorio = row["SETOR"].ToString().Trim();

                //    var territoriobd = territorioRepository.GetTerritorio(territorio);

                //    if (territoriobd == null)
                //    {
                //        erros.Add("Territorio \"" + territorio + "\" não encontrado.");
                //        break;
                //    }

                //}
                //catch (Exception e)
                //{
                //    Util.Sistema.Error.TrataErro(e);

                //    erros.Add("erro não identificado - linha " + count + "<br><br>ERRO:" + e.Message);
                //}
            }

            if (erros.Count >= 1)
            {
                //deleta o arquivo e o registro do banco caso haja algum erro no excel
                System.IO.File.Delete(caminhoImportacaoOrdemProdutosFisico);

                string msg = String.Empty;

                foreach (string erro in erros)
                {
                    msg += erro + "<br/>";
                }

                logs.Add(Log.EnumTipo.Inclusao, "Verificação Importação de Doutores: haviam erros na planilha.", Url.Action("Index"));

                return Content(msg);
            }
            else
            {
                logs.Add(Log.EnumTipo.Inclusao, "Verificação Importação de Doutores: planilha aprovada.", Url.Action("Index"));

                return Content("[ok]");
            }

        }

        public ActionResult ImportarExcelDoutores()
        {
            List<string> erros = new List<string>();

            // ler excel
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + caminhoImportacaoDoutoresFisico + ";Extended Properties=Excel 8.0;";
            //You must use the $ after the object you reference in the spreadsheet
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Plan1$]", strConn);

            DataSet myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            /////////////////////////////////
            ////// IMPORTAÇÃO DO EXCEL //////
            /////////////////////////////////

            int count = 1;

            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                DoutorCadastro doutor = new DoutorCadastro();

                count++; //início na linha 2 do Excel

                //doutor.Nome = "Doutor" + count;

                //doutorRepository.Add(doutor);
                //doutorRepository.Save();

                try
                {
                    row["CODIGO"].ToString();
                    row["SETOR"].ToString();
                    row["UFCRM"].ToString();
                    row["CRM"].ToString();
                    row["NOME"].ToString();
                    row["ESP"].ToString();
                }
                catch (Exception e)
                {
                    erros.Add(e.Message + "colunas do excel má formatadas, verifique o nome e a ordem no modelo de exemplo.");
                    break;
                }

                try
                {
                    string codigo = row["CODIGO"].ToString().Trim();
                    string territorio = row["SETOR"].ToString().Trim();
                    string crmUf = row["UFCRM"].ToString().Trim();
                    string crm = row["CRM"].ToString().ToInt().ToString().Trim();
                    string nome = row["NOME"].ToString();
                    string especialidade = row["ESP"].ToString().Trim();

                    doutor.Codigo = codigo;
                    doutor.IdTerritorio = territorio;

                    var especialidadebd = especialidadeRepository.GetEspecialidade(especialidade);

                    if (especialidadebd == null)
                    {
                        erros.Add("Especialidade \"" + especialidade + "\" não encontrada.");
                        break;
                    }

                    doutor.IdEspecialidade = especialidadebd.Id;
                    doutor.CRMUF = crmUf;
                    doutor.CRM = crm;
                    doutor.Nome = nome;

                    doutorCadastroRepository.Add(doutor);
                    doutorCadastroRepository.Save();

                }
                catch (Exception e)
                {
                    Util.Sistema.Error.TrataErro(e);

                    erros.Add("erro não identificado");
                }
            }

            if (erros.Count >= 1)
            {
                //deleta o arquivo e o registro do banco caso haja algum erro no excel
                System.IO.File.Delete(caminhoImportacaoTerritoriosFisico);

                string msg = String.Empty;

                foreach (string erro in erros)
                {
                    msg += erro + "<br/>";
                }

                logs.Add(Log.EnumTipo.Inclusao, "Importação de Doutores: ocorreu algum erro durante a importação.", Url.Action("Index"));

                return Content(msg);
            }
            else
            {
                logs.Add(Log.EnumTipo.Inclusao, "Importação de Doutores: importação concluída.", Url.Action("Index"));

                return Content("[ok]");
            }

        }



        [HttpPost]
        public ActionResult UploadGerentesRep(HttpPostedFileBase fileData)
        {
            try
            {
                Util.Arquivo.CreateDirectoryIfNotExists(Util.Url.GetCaminhoFisico(diretorioImportacao));

                fileData.SaveAs(caminhoImportacaoGerentesFisico);
            }
            catch (Exception ex)
            {

                Util.Sistema.Error.TrataErro(ex, Request);

                return Content("1: Erro ao fazer o upload do arquivo: " + ex.Message);

            }

            return Content("[ok]");

        }

        [HttpPost]
        public ActionResult VerificaExcelGerentesRep()
        {

            List<string> erros = new List<string>();
            OleDbDataAdapter adapter = null;

            try
            {
                // ler excel
                string strConn;
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + caminhoImportacaoGerentesFisico + ";Extended Properties=Excel 8.0;";

                //You must use the $ after the object you reference in the spreadsheet
                adapter = new OleDbDataAdapter("SELECT * FROM [Plan1$]", strConn);

            }
            catch
            {
                erros.Add("O arquivo enviado não é um arquivo de excel válido.");
            }

            DataSet myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            int count = 1;

            ////////////////////////////////
            ////// VALIDAÇÃO DO EXCEL //////
            ////////////////////////////////

            foreach (DataRow row in myDataSet.Tables[0].Rows)
            {
                count++; //início na linha 2 do Excel
                try
                {
                    row["Territorio"].ToString();
                    row["Nome"].ToString();
                    row["Email"].ToString();
                    row["Senha"].ToString();
                    row["IdLinha"].ToString();
                }
                catch (Exception e)
                {
                    erros.Add(e.Message + "colunas do excel má formatadas, verifique o nome e a ordem no modelo de exemplo.");
                    break;
                }

                try
                {

                }
                catch
                {
                    erros.Add("erro não identificado - linha " + count);
                }
            }

            if (erros.Count >= 1)
            {
                //deleta o arquivo e o registro do banco caso haja algum erro no excel
                System.IO.File.Delete(caminhoImportacaoFisico);

                string msg = String.Empty;

                foreach (string erro in erros)
                {
                    msg += erro + "<br/>";
                }

                logs.Add(Log.EnumTipo.Inclusao, "Verificação Importação de Gerentes - Rep: haviam erros na planilha.", Url.Action("Index"));

                return Content(msg);
            }
            else
            {
                logs.Add(Log.EnumTipo.Inclusao, "Importação de Gerentes - Rep: planilha aprovada.", Url.Action("Index"));
                return Content("[ok]");
            }

        }

        public ActionResult ImportarExcelGerentesRep()
        {
            List<string> erros = new List<string>();

            // ler excel
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + caminhoImportacaoGerentesFisico + ";Extended Properties=Excel 8.0;";
            //You must use the $ after the object you reference in the spreadsheet
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM [Plan1$]", strConn);

            DataSet myDataSet = new DataSet();
            adapter.Fill(myDataSet);
            adapter.Dispose();

            /////////////////////////////////
            ////// IMPORTAÇÃO DO EXCEL //////
            /////////////////////////////////

            try
            {
                string idGrupoGerente = grupoRepository.GetGrupo(Usuario.EnumTipo.GerenteProduto.GetDescription()).Id;
                string idGrupoGerenteMarketing = grupoRepository.GetGrupo(Usuario.EnumTipo.GerenteMarketing.GetDescription()).Id;
                string idGrupoAgencia = grupoRepository.GetGrupo(Usuario.EnumTipo.Agencia.GetDescription()).Id;

                foreach (DataRow row in myDataSet.Tables[0].Rows)
                {
                    string territorio = row["Territorio"].ToString();
                    string nome = row["Nome"].ToString();
                    string email = row["Email"].ToString();
                    string senha = row["Senha"].ToString().Trim().ToLower();
                    string idLinha = row["IdLinha"].ToString();

                    Usuario usuario = null;

                    var usuarioExistente = usuarioRepository.GetUsuarioByEmail(email);

                    if (usuarioExistente == null)
                    {
                        usuario = new Usuario();
                    }
                    else
                    {
                        usuario = usuarioExistente;
                    }

                    usuario.Nome = nome;
                    usuario.Email = email;
                    usuario.Senha = Util.Criptografia.CriptografaMd5(senha);
                    usuario.Status = 'A';

                    if (territorio.Contains("0000"))
                    {
                        usuario.IdGrupo = Usuario.EnumTipo.GerenteNacional.GetDescription();
                    }
                    else if (territorio.Contains("000"))
                    {
                        usuario.IdGrupo = Usuario.EnumTipo.GerenteRegional.GetDescription();
                    }
                    else if (territorio.Contains("00"))
                    {
                        usuario.IdGrupo = Usuario.EnumTipo.GerenteDistrital.GetDescription();
                    }

                    if (usuarioExistente == null)
                    {
                        usuarioRepository.Add(usuario);
                    }
                    usuarioRepository.Save();

                    usuario.TerritorioSimulado = "g" + usuario.Id;
                    usuarioRepository.Save();

                    var linha = linhaRepository.GetProdutoLinha(idLinha.ToInt());

                    if (linha != null)
                    {
                        var produtos = doutorRepository.GetDoutors().Where(d => d.IdProdutoLinha == linha.Id).SelectMany(d=> d.DoutorProdutos).Select(dp => dp.Produto).Distinct();

                        foreach (var produto in linha.Produtos)
                        {
                            usuario.AddProduto(usuarioRepository, produto);
                        }

                        usuarioRepository.Save();
                    }

                }

            }
            catch (Exception e)
            {
                Util.Sistema.Error.TrataErro(e, Request);

                erros.Add("Ocorreu um erro durante a importação");
            }

            if (erros.Count >= 1)
            {
                //deleta o arquivo e o registro do banco caso haja algum erro no excel
                System.IO.File.Delete(caminhoImportacaoFisico);

                string msg = String.Empty;

                foreach (string erro in erros)
                {
                    msg += erro + "<br/>";
                }

                logs.Add(Log.EnumTipo.Inclusao, "Importação de Gerentes-Rep: Ocorreu algum erro durante a importação.", Url.Action("Index"));

                return Content(msg);
            }
            else
            {

                logs.Add(Log.EnumTipo.Inclusao, "Importação de Gerentes-Rep: importação concluída.", Url.Action("Index"));

                return Content("[ok]");
            }

        }
    }
}
