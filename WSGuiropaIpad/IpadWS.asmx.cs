using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Web.Services.Protocols;
using System.IO;
using Models.WS;
using System.Xml;
using Models;
using System.Net.Mail;
using System.Web.Script.Services;

namespace WSGuiropaIpad
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://guiropa.tecnologia.ws/guiropaipadws/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class IpadWS : System.Web.Services.WebService
    {
        Models.DBDataContext db = new Models.DBDataContext();

        public string TipoUsuario { get; set; }

        public Territorio GetTerritorio(string territory, string senha)
        {
            return GetTerritorio(territory, senha, true);
        }

        public Territorio GetTerritorio(string territory, string senha, bool verificaSenha)
        {
            Territorio territorio = null;

            if (senha == Util.Criptografia.CriptografaMd5("%9u!70p@" + territory))
            {
                //acesso do app ipad
                territorio = db.Territorios.SingleOrDefault(t => t.Id == territory);
            }
            else if (verificaSenha)
            {
                //verifica se eh um login
                territorio = db.Territorios.SingleOrDefault(t => t.Id == territory && t.Senha == senha);
            }            

            return territorio;
        }

        public Usuario GetUsuarioByTerritorioSimulado(string territory, string senha)
        {
            Usuario usuario = null;

            if (senha == Util.Criptografia.CriptografaMd5("%9u!70p@" + territory))
            {

                usuario = db.Usuarios.FirstOrDefault(u => u.TerritorioSimulado == territory);

            }
            else
            {
                usuario = db.Usuarios.FirstOrDefault(u => u.Email == territory && u.Senha == senha);
            }

            return usuario;
        }

        public ProdutoLinha GetLinhaByTerritorioSimulado(string territory, string senha)
        {
            ProdutoLinha linha = null;

            if (senha == Util.Criptografia.CriptografaMd5("%9u!70p@" + territory))
            {

                linha = db.ProdutoLinhas.FirstOrDefault(l => l.TerritorioSimulado == territory);

            }

            return linha;
        }

        public bool ExisteTerritorio(string territory, string senha)
        {
            var territorio = GetTerritorio(territory, senha);
            var usuario = GetUsuarioByTerritorioSimulado(territory, senha);
            var linha = GetLinhaByTerritorioSimulado(territory, senha);

            if (territorio != null || linha != null)
            {
                TipoUsuario = "T";
            }
            else if (usuario != null)
            {
                if (usuario.IsAgencia())
                {
                    TipoUsuario = "A";
                }
                else if (usuario.IsGerenteProduto())
                {
                    TipoUsuario = "GP";
                }
                else if (usuario.IsGerenteMarketing())
                {
                    TipoUsuario = "GM";
                }
                else if (usuario.IsGerenteNacional())
                {
                    TipoUsuario = "GP";
                }
                else if (usuario.IsGerenteRegional())
                {
                    TipoUsuario = "GP";
                }
                else if (usuario.IsGerenteDistrital())
                {
                    TipoUsuario = "GP";
                }
                else if (usuario.IsAdministrador())
                {
                    TipoUsuario = "ADM";
                }
            }

            return (territorio != null || usuario != null || linha != null);
        }

        [WebMethod]
        public Answer Login(string territory, string password)
        {
            if (!ExisteTerritorio(territory, password))
                return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

            var territorio = GetTerritorio(territory, password);
            var usuario = GetUsuarioByTerritorioSimulado(territory, password);

            User user = new User();

            user.Type = TipoUsuario;

            if (usuario != null)
            {
                user.Name = usuario.Nome;
            }

            if (territorio != null)
            {
                user.Name = territorio.Nome;
            }

            return new Answer(Answer.EnumStatus.Ok, String.Empty, user);
        }

        [WebMethod]
        public Answer GetInfoUser(string territory, string password)
        {
            if (!ExisteTerritorio(territory, password))
                return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

            var territorio = GetTerritorio(territory, password);
            var usuario = GetUsuarioByTerritorioSimulado(territory, password);

            User user = new User();

            user.Type = TipoUsuario;

            if (usuario != null)
            {
                user.Name = usuario.Nome;
            }

            if (territorio != null)
            {
                user.Name = territorio.Nome;
            }

            if (usuario != null && usuario.IsGerente())
            {
                /*Usuario gerenteMarketing = null;

                if (usuario.IsGerenteMarketing())
                {
                    gerenteMarketing = usuario;
                }
                else
                {
                    gerenteMarketing = usuario.GetGerenteMarketing();
                }*/

                //seleciona as linhas q o gerente tem acesso
                var linhas = db.ProdutoLinhas.Where(l => l.Doutors.Any(d => usuario.UsuarioProdutos.Select(up => up.Produto.IdLinha).Contains(d.IdProdutoLinha)));

                foreach(var linha in linhas){

                    if (linha.TerritorioSimulado == null)
                        continue;

                    Territory t = new Territory();
                    t.Id = linha.TerritorioSimulado;
                    t.Name = linha.Nome;
                    user.Territorys.Add(t);
                }
            }

            return new Answer(Answer.EnumStatus.Ok, String.Empty, user);
        }

        [WebMethod]
        public Answer AccountRecovery(string territory)
        {
            var territorio = db.Territorios.SingleOrDefault(u => u.Email == territory);

            if (territorio == null)
                return new Answer(Answer.EnumStatus.ErroLogin, "Login inválido");

            return new Answer(Answer.EnumStatus.Ok);
        }

        [WebMethod]
        public Answer GetInfoDoctors(string territory, string password)
        {
            var territorio = GetTerritorio(territory, password);

            //verifica se há usuario simulando um territorio (significa que é uma agencia ou gerente acessando o sistema)
            var usuario = GetUsuarioByTerritorioSimulado(territory, password);

            //verifica se há linha simulando territorio
            var linha = GetLinhaByTerritorioSimulado(territory, password);

            if (territorio == null && usuario == null && linha == null)
                return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

            IQueryable<Doutor> doutoresQuery = null;
            var doutores = new List<Doctor>();

            if (territorio != null || linha != null)
            {
                if (territorio != null)
                {
                    // TERRITORIO

                    territorio.AtualizaUltimaSincronizacao();

                    //procura os doutores da linha do territorio
                    //doutoresQuery = from d in db.Doutors
                    //                where d.ProdutoLinha.Id == territorio.ProdutoLinha.Id
                    //                select d;


                    // GAMBI TEMPORARIA
                    var doutor = new Doctor();

                    doutor.Id = "1";
                    doutor.Name = "Ativos";
                    doutor.Address = "Ativos";
                    doutor.PhonePrefix = "";
                    doutor.PhoneNumber = "";
                    doutor.Speciality = "Ativos";

                    var produtos = db.Produtos;

                    doutor.PresentationId = produtos.ToList().SelectMany(p => p.ProdutoVas).Where(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo).Select(va => va.Id).ToArray();
                    doutor.Version = "0";

                    doutores.Add(doutor);
                }
                else
                {
                    // LINHA

                    //procura os doutores da linha do territorio
                    doutoresQuery = from d in db.Doutors
                                    where d.ProdutoLinha.Id == linha.Id
                                    select d;
                }

                if (doutoresQuery != null) { 

                    foreach (var doutorDB in doutoresQuery)
                    {

                        Doctor doutor = new Doctor();

                        doutor.Id = doutorDB.Id.ToString();
                        doutor.Name = doutorDB.Nome;
                        //doutor.CRM = doutorDB.CRM;
                        //doutor.CRM = "35555";
                        //doutor.Birthday = doutorDB.DataNascimento.Formata(Util.Data.FormatoData.DiaMesAno);
                        doutor.Address = doutorDB.Endereco;
                        doutor.PhonePrefix = doutorDB.TelefoneDDD;
                        doutor.PhoneNumber = doutorDB.Telefone;
                        //doutor.CRMUF = doutorDB.CRMUF;
                        //doutor.CRMUF = "SP";
                        var relDoutorEspecialidade = doutorDB.DoutorEspecialidades.FirstOrDefault();
                        if (relDoutorEspecialidade != null)
                        {
                            doutor.Speciality = relDoutorEspecialidade.Especialidade.Nome;
                        }

                        doutor.PresentationId = doutorDB.DoutorProdutos.Where(p => p.Produto.GetVAAtivo() != null).OrderBy(p => p.Orderm).Select(p => p.Produto.GetVAAtivo().Id).ToArray();

                        doutor.Version = doutorDB.Versao.ToString();

                        doutores.Add(doutor);
                    }

                }

            }
            else
            {
                // NAO EH UM TERRITORIO, EH UM USUARIO (AGENCIA OU GERENTE) ACESSANDO O SISTEMA PARA VISUALIZAÇÃO E/OU TESTES

                //se for gerente Nacional Distrital ou Regional, n exibe nada e vai para o final
                if (usuario.IsGerenteNacional() || usuario.IsGerenteDistrital() || usuario.IsGerenteRegional())
                {
                    goto final;
                }

                Doctor doutor = new Doctor();

                doutor.Id = "0";
                doutor.Name = "Teste";
                //doutor.CRM = doutorDB.CRM;
                //doutor.CRM = "35555";
                //doutor.Birthday = doutorDB.DataNascimento.Formata(Util.Data.FormatoData.DiaMesAno);
                doutor.Address = "Teste";
                doutor.PhonePrefix = "";
                doutor.PhoneNumber = "";
                //doutor.CRMUF = doutorDB.CRMUF;
                //doutor.CRMUF = "SP";
                //var relDoutorEspecialidade = doutorDB.DoutorEspecialidades.FirstOrDefault();
                //if (relDoutorEspecialidade != null)
                //{
                    doutor.Speciality = "Teste";
                //}

                IQueryable<Produto> produtos = db.Produtos;
                    
                if(!usuario.IsAdministrador()){
                    produtos = produtos.Where(p => p.UsuarioProdutos.Any(up => up.IdUsuario == usuario.Id));
                }
                doutor.PresentationId = produtos.ToList().SelectMany(p => p.ProdutoVas).Where(va => va.Status == (char)ProdutoVa.EnumStatus.Teste).Select(va => va.Id).ToArray();
                //soma a versao dos vas ativos e em teste para que a versão do doutor seja sempre atualizada caso haja uma novo produto em teste ou publicado
                //int? sumVersaoVasAtivos = produtos.SelectMany(p => p.ProdutoVas).Where(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo).Sum(va => va.Versao);
                //int? sumVersaoVasTestes = produtos.SelectMany(p => p.ProdutoVas).Where(va => va.Status == (char)ProdutoVa.EnumStatus.Teste).Sum(va => va.VersaoTeste);
                //sumVersaoVasAtivos = (sumVersaoVasAtivos == null) ? 0 : sumVersaoVasAtivos;
                //sumVersaoVasTestes = (sumVersaoVasTestes == null) ? 0 : sumVersaoVasTestes;
                //doutor.Version = (sumVersaoVasTestes).ToString();
                doutor.Version = "0";

                doutores.Add(doutor);

                doutor = new Doctor();

                doutor.Id = "1";
                doutor.Name = "Ativos";
                //doutor.CRM = doutorDB.CRM;
                //doutor.CRM = "35555";
                //doutor.Birthday = doutorDB.DataNascimento.Formata(Util.Data.FormatoData.DiaMesAno);
                doutor.Address = "Ativos";
                doutor.PhonePrefix = "";
                doutor.PhoneNumber = "";
                //doutor.CRMUF = doutorDB.CRMUF;
                //doutor.CRMUF = "SP";
                //var relDoutorEspecialidade = doutorDB.DoutorEspecialidades.FirstOrDefault();
                //if (relDoutorEspecialidade != null)
                //{
                doutor.Speciality = "Ativos";
                //}

                produtos = db.Produtos;

                if (!usuario.IsAdministrador())
                {
                    produtos = produtos.Where(p => p.UsuarioProdutos.Any(up => up.IdUsuario == usuario.Id));
                }
                doutor.PresentationId = produtos.ToList().SelectMany(p => p.ProdutoVas).Where(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo).Select(va => va.Id).ToArray();
                //soma a versao dos vas ativos e em teste para que a versão do doutor seja sempre atualizada caso haja uma novo produto em teste ou publicado
                //sumVersaoVasAtivos = produtos.SelectMany(p => p.ProdutoVas).Where(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo).Sum(va => va.Versao);
                //sumVersaoVasTestes = produtos.SelectMany(p => p.ProdutoVas).Where(va => va.Status == (char)ProdutoVa.EnumStatus.Teste).Sum(va => va.VersaoTeste);
                //sumVersaoVasAtivos = (sumVersaoVasAtivos == null) ? 0 : sumVersaoVasAtivos;
                //sumVersaoVasTestes = (sumVersaoVasTestes == null) ? 0 : sumVersaoVasTestes;
                //doutor.Version = (sumVersaoVasAtivos).ToString();
                doutor.Version = "0";

                doutores.Add(doutor);

                if (usuario.IsGerente()) { 

                    doutor = new Doctor();

                    doutor.Id = "2";
                    doutor.Name = "Pendentes";
                    //doutor.CRM = doutorDB.CRM;
                    //doutor.CRM = "35555";
                    //doutor.Birthday = doutorDB.DataNascimento.Formata(Util.Data.FormatoData.DiaMesAno);
                    doutor.Address = "Pendentes";
                    doutor.PhonePrefix = "";
                    doutor.PhoneNumber = "";
                    //doutor.CRMUF = doutorDB.CRMUF;
                    //doutor.CRMUF = "SP";
                    //var relDoutorEspecialidade = doutorDB.DoutorEspecialidades.FirstOrDefault();
                    //if (relDoutorEspecialidade != null)
                    //{
                    doutor.Speciality = "Pendentes";
                    //}

                    produtos = db.Produtos;

                    if (!usuario.IsAdministrador())
                    {
                        produtos = produtos.Where(p => p.UsuarioProdutos.Any(up => up.IdUsuario == usuario.Id));
                    }
                    doutor.PresentationId = produtos.ToList().SelectMany(p => p.ProdutoVas).Where(va => va.Status == (char)ProdutoVa.EnumStatus.Pendente || va.StatusGM == (char)ProdutoVa.EnumStatus.Pendente || va.Status == (char)ProdutoVa.EnumStatus.Aprovado).Select(va => va.Id).ToArray();
                    //soma a versao dos vas ativos e em teste para que a versão do doutor seja sempre atualizada caso haja uma novo produto em teste ou publicado
                    //sumVersaoVasAtivos = produtos.SelectMany(p => p.ProdutoVas).Where(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo).Sum(va => va.Versao);
                    //int? sumVersaoTeste = produtos.SelectMany(p => p.ProdutoVas).Sum(va => va.VersaoTeste);
                    //int? sumIds = produtos.SelectMany(p => p.ProdutoVas).Sum(va => va.Id);
                    //sumVersaoVasAtivos = (sumVersaoVasAtivos == null) ? 0 : sumVersaoVasAtivos;
                    //sumVersaoTeste = (sumVersaoTeste == null) ? 0 : sumVersaoTeste;
                    //doutor.Version = (sumVersaoVasAtivos + sumVersaoTeste + sumIds).ToString();
                    doutor.Version = "0";

                    doutores.Add(doutor);
                
                }
            }

            final:
            
            return new Answer(Answer.EnumStatus.Ok, String.Empty, doutores);
        }

        [WebMethod]
        public Answer GetInfoPresentations(string territory, string password)
        {
            return GetInfoPresentations(territory, password, false);
        }

        [WebMethod]
        public Answer GetInfoPresentationsAllProducts(string territory, string password)
        {
            return GetInfoPresentations(territory, password, true);
        }

        private Answer GetInfoPresentations(string territory, string password, bool returnAllProducts)
        {
            var territorio = GetTerritorio(territory, password);

            //verifica se há usuario simulando um territorio (significa que é uma agencia ou gerente acessando o sistema)
            var usuario = GetUsuarioByTerritorioSimulado(territory, password);

            var linha = GetLinhaByTerritorioSimulado(territory, password);

            if (territorio == null && usuario == null && linha == null)
                return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

            //var produtosQuery = 
            //    from p in db.Produtos
            //    where  p.DoutorProdutos.Any(dp => dp.Doutor.IdTerritorio == territorio.Id)
            //    select p;

            IQueryable<Produto> produtosQuery = null;

            var retornaProdutoTeste = false;
            var retornaProdutosPendentes = false;

            if (territorio != null)
            {
                //TERRITORIO

                if (returnAllProducts)
                {
                    if (territorio.TerritorioLinhas.Count() > 0)
                    {
                        produtosQuery = db.Produtos.Where(p => territorio.TerritorioLinhas.Select(tl => tl.IdLinha).Contains(p.IdLinha));
                    }
                    else { 
                        produtosQuery = db.Produtos.Where(p => p.IdLinha == territorio.IdLinha);
                    }
                }
                else { 
                    //procura os produtos do territorio
                    produtosQuery =
                        from p in db.Produtos
                        where p.DoutorProdutos.Any(dp => dp.Doutor.ProdutoLinha.Id == territorio.ProdutoLinha.Id)
                        select p;
                }

            }
            else if (linha != null) {

                //LINHA

                if (returnAllProducts)
                {
                    produtosQuery = db.Produtos;
                }
                else
                {
                    //procura os produtos da linha
                    produtosQuery =
                        from p in db.Produtos
                        where p.DoutorProdutos.Any(dp => dp.Doutor.ProdutoLinha.Id == linha.Id)
                        select p;
                }
            }    
            else
            {

                if (usuario.IsAdministrador())
                {
                    produtosQuery = db.Produtos;
                }
                else { 

                    //territorio na verdade é uma agencia ou gerente (sim, POG)
                    produtosQuery =
                        from p in db.Produtos
                        where p.UsuarioProdutos.Any(up => up.IdUsuario == usuario.Id)
                        select p;
                
                }

                retornaProdutoTeste = true;

                if (usuario.IsGerente())
                {
                    retornaProdutosPendentes = true;
                }

            }

            List<Presentation> presentations = new List<Presentation>();

            //se for gerente Nacional Distrital ou Regional, n exibe nada e vai para o final
            if (usuario!= null && (usuario.IsGerenteNacional() || usuario.IsGerenteDistrital() || usuario.IsGerenteRegional()))
            {
                goto final;
            }

            foreach (var produtoDB in produtosQuery)
            {

                List<ProdutoVa> vas = new List<ProdutoVa>();

                var vasAtivos = produtoDB.GetVAsAtivos();

                foreach (var vaAtivo in vasAtivos)
                {
                    vas.Add(vaAtivo);
                }
                
                if (retornaProdutoTeste)
                {
                    var vasTeste = produtoDB.GetVAsTeste();

                    foreach (var vaTeste in vasTeste) { 
                        vas.Add(vaTeste);
                    }
                }

                if (retornaProdutosPendentes)
                {
                    var vasPendentes = produtoDB.GetVAsPendentes();

                    foreach (var vaPendente in vasPendentes)
                    {
                        vas.Add(vaPendente);
                    }
                }

                foreach (var va in vas)
                {

                    if (va != null)
                    {

                        Presentation presentation = new Presentation();

                        presentation.PreencherByVA(va, territorio, usuario, linha);
                        
                        presentations.Add(presentation);

                    }

                }
                

            }

            final:

            return new Answer(Answer.EnumStatus.Ok, String.Empty, presentations);
        }

        [WebMethod]
        public Answer GetQuantityNotUpdatedByKey(string territory, string key)
        {
            if (key != "grp%85p@")
                return new Answer(Answer.EnumStatus.Erro, "Inválido");

            return GetQuantityNotUpdated(territory, Util.Criptografia.CriptografaMd5("%9u!70p@" + territory));
        }

        [WebMethod]
        public Answer GetQuantityNotUpdated(string territory, string password)
        {
            var territorio = GetTerritorio(territory, password);

            if (territorio == null)
                return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

            var downloads = from d in db.TerritorioProdutoVaDownloads
                           where d.IdTerritorio == territory
                           select d;

            //seleciona a qtde de produtos que não tem registros de download pelo territorio ainda ou que tem atualização pendente
            var qtde =
                (from produto in db.Produtos
                 where
                    produto.DoutorProdutos.Any(dp => dp.Doutor.ProdutoLinha.Id == territorio.ProdutoLinha.Id) &&
                    produto.ProdutoVas.Where(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo).OrderByDescending(va => va.Versao).FirstOrDefault().TerritorioProdutoVaDownloads.Where(t => t.IdTerritorio == territory).Count() == 0 // || downloads.Any(d => d.Data < produto.ProdutoVas.Where(va => va.Status == (char)ProdutoVa.EnumStatus.Ativo).OrderByDescending(va => va.Versao).FirstOrDefault().DataInclusao)
                select produto).Count();
            
            return new Answer(Answer.EnumStatus.Ok, String.Empty, qtde.ToString());
        }

        [WebMethod]
        public Answer GetInfoApp(string territory, string password)
        {
            var territorio = GetTerritorio(territory, password);
            var usuario = GetUsuarioByTerritorioSimulado(territory, password);
            var linha = GetLinhaByTerritorioSimulado(territory, password);

            if (territorio == null && linha == null && usuario == null)
                return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

            AppDownloadRepository appDownloadRepository = new AppDownloadRepository();

            AppDownload appDownload = null;

            if (territorio != null)
            {
                appDownload = appDownloadRepository.GetAppDownloads().Where(a => a.IdProdutoLinha == territorio.ProdutoLinha.Id && a.Tipo == (char)AppDownload.EnumTipo.Ipad).FirstOrDefault();
            }
            else if (linha != null)
            {
                appDownload = appDownloadRepository.GetAppDownloads().Where(a => a.IdProdutoLinha == linha.Id && a.Tipo == (char)AppDownload.EnumTipo.Ipad).FirstOrDefault();
            }
            else if (usuario != null)
            {
                //pega o app padrao
                appDownload = appDownloadRepository.GetAppDownloads().Where(a => a.IdProdutoLinha == null && a.Tipo == (char)AppDownload.EnumTipo.Ipad).FirstOrDefault();
            }
            
            if(appDownload == null)
                return new Answer(Answer.EnumStatus.Erro, "Não há versão para a linha do território.");

            ProdutoRepository produtoRepository = new ProdutoRepository();

            String UrlProduto = string.Empty;

            if(produtoRepository.GetProdutosSemVerificacaoUsuario().Any(p => p.TemImagem)){

                UrlProduto = Util.Sistema.AppSettings.UrlDownloadBaseProdutosImagens + territory + "/" + Util.Sistema.GetTokenTerritorio(territory);

            }

            AppInfo appInfo = new AppInfo();

            appInfo.Version = appDownload.Versao;
            appInfo.Download = "itms-services://?action=download-manifest&url=" + appDownload.Url;
            appInfo.UrlDownloadProductImages = UrlProduto;

            List<ProductImage> productImages = new List<ProductImage>();

            foreach (var produto in produtoRepository.GetProdutosSemVerificacaoUsuario().Where(p => p.TemImagem))
            {
                ProductImage productImage = new ProductImage();
                productImage.ProductId = produto.Id.ToString();
                productImage.Url = produto.GetCaminhoImagemThumb(); 
                productImages.Add(productImage);
            }

            appInfo.ProductImages = productImages;

            return new Answer(Answer.EnumStatus.Ok, string.Empty, appInfo);
        }

        [WebMethod(enableSession: true)]
        public Answer UpdatePresentationStatus(string territory, string password, int idPresentation, int status)
        {
            var usuario = GetUsuarioByTerritorioSimulado(territory, password);

            if (usuario == null)
                return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

            Sessao.Site.Logar(usuario);

            ProdutoVaRepository vaRepository = new ProdutoVaRepository();

            var va = vaRepository.GetProdutoVa(idPresentation);

            if(va == null)
                return new Answer(Answer.EnumStatus.Erro, "VA inválido");

            if (!va.ValidoParaEdicao())
                return new Answer(Answer.EnumStatus.Erro, "VA inválido para edição");

            switch (status)
            {
                case 1: //aprovar
                    va.Aprovar();
                    vaRepository.Save();
                    break;
                case 2: //reprovar
                    va.Reprovar();
                    vaRepository.Save();
                    break;
                case 3:
                    
                    if (!va.IsAprovado()) { 
                        va.Aprovar();
                        vaRepository.Save();
                    }
                    
                    if (va.IsAprovado())
                    {
                        va.Publicar(vaRepository);
                    }

                    break;

                default:
                    break;
            }
            
            Presentation presentation = new Presentation();

            presentation.PreencherByVA(va,null,usuario,null);

            return new Answer(Answer.EnumStatus.Ok, string.Empty, presentation);
        }

        [WebMethod(enableSession: true)]
        public Answer InsertPresentationComment(string territory, string password, int idPresentation, string comment)
        {
            Util.Logs logs = new Util.Logs(Models.Log.EnumPagina.ProdutosVas, Models.Log.EnumArea.WebService);
            var usuario = GetUsuarioByTerritorioSimulado(territory, password);

            if (usuario == null)
                return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

            Sessao.Site.Logar(usuario);

            ProdutoVaRepository vaRepository = new ProdutoVaRepository();

            var va = vaRepository.GetProdutoVa(idPresentation);

            if (va == null)
                return new Answer(Answer.EnumStatus.Erro, "VA inválido");

            if (comment.Length > 500)
                return new Answer(Answer.EnumStatus.Erro, "Comentário não pode ter mais do que 500 caracteres");

            ProdutoVaComentarioRepository comentarioRepository = new ProdutoVaComentarioRepository();
            ProdutoVaComentario comentario = new ProdutoVaComentario();

            comentario.IdUsuario = Sessao.Site.UsuarioInfo.Id;
            comentario.IdVa = va.Id;
            comentario.Descricao = comment;

            comentarioRepository.Add(comentario);
            comentarioRepository.Save();

            logs.Add(Models.Log.EnumTipo.Inclusao, "Comentou no VA (data: " + va.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno) + ") do Produto '" + va.Produto.Nome + "'", string.Empty);

            return new Answer(Answer.EnumStatus.Ok, string.Empty);
        }

        [WebMethod(enableSession: true)]
        public Answer GetPresentationComments(string territory, string password, int idPresentation)
        {
            Util.Logs logs = new Util.Logs(Models.Log.EnumPagina.ProdutosVas, Models.Log.EnumArea.WebService);
            var usuario = GetUsuarioByTerritorioSimulado(territory, password);

            if (usuario == null)
                return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

            Sessao.Site.Logar(usuario);

            ProdutoVaRepository vaRepository = new ProdutoVaRepository();

            var va = vaRepository.GetProdutoVa(idPresentation);

            if (va == null)
                return new Answer(Answer.EnumStatus.Erro, "VA inválido");

            List<PresentationComment> presentationComments = new List<PresentationComment>();

            foreach (var comentario in va.ProdutoVaComentarios)
            {
                PresentationComment comment = new PresentationComment();

                comment.Id = comentario.Id.ToString();
                comment.Name = comentario.Usuario.Nome;
                comment.Description = comentario.Descricao;
                comment.Date = comentario.Datainclusao.Formata(Util.Data.FormatoData.DiaMesAnoHoraMinuto);

                presentationComments.Add(comment);
            }

            return new Answer(Answer.EnumStatus.Ok, string.Empty, presentationComments);
        }

        //<log>
        //   <user>
        //      <territory>1101</territory>
        //      <password>16a5c46b4a1fd2094aab77d5315f6b64</password>
        //   </user>
        //   <logs>
        //      <log>
        //         <doctorId></doctorId>
        //         <crm>35666</crm>
        //         <crmUf>SP</crmUf>
        //         <presentationId>1</presentationId>
        //         <presentationPageId>1</presentationPageId>
        //         <duration>15.6</duration>
        //         <dateOfStart>2011-04-14 15:28:18</dateOfStart>
        //          <latitude>13,5</latitude>
        //          <longitude>14,5</longitude>
        //      </log>
        //   </logs>
        //</log>
        [WebMethod]
        public Answer Log(string xml)
        {
            
            //int? idDoutor = null;
            int idVa = 0;
            int idSlide = 0;
            double segundos = 0;
            DateTime? data = null;
            string crm = null;
            string crmUf = null;

            string userTerritorio;
            string userSenha;
            double gpsLatitude;
            double gpsLongitude;
            string type;

            #region lê informações do Xml passado

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(new StringReader(xml));

                XmlElement nodeUsuario = (XmlElement)doc.GetElementsByTagName("user")[0];
                userTerritorio = nodeUsuario.GetElementsByTagName("territory")[0].InnerText;
                userSenha = nodeUsuario.GetElementsByTagName("password")[0].InnerText;

                XmlElement nodeLogs = (XmlElement)doc.GetElementsByTagName("logs")[0];

                //faz autenticação
                var territorio = GetTerritorio(userTerritorio, userSenha);

                if (territorio == null)
                    return new Answer(Answer.EnumStatus.Erro, "Login e/ou senha inválido(s)");

                foreach (XmlNode nodeLog in nodeLogs.ChildNodes)
                {
                    /*if(nodeLog["doctorId"] != null && nodeLog["doctorId"].InnerText != null && nodeLog["doctorId"].InnerText != string.Empty){
                        idDoutor = nodeLog["doctorId"].InnerText.ToInt();
                    }*/

                    idVa = nodeLog["presentationId"].InnerText.ToInt();
                    idSlide = nodeLog["presentationPageId"].InnerText.ToInt();
                    segundos = nodeLog["duration"].InnerText.ToDouble();
                    data = nodeLog["dateOfStart"].InnerText.ToDateTime();
                    crm = nodeLog["crm"].InnerText.ToString();
                    crmUf = nodeLog["crmUf"].InnerText.ToString();
                    gpsLatitude = nodeLog["latitude"].InnerText.ToDouble();
                    gpsLongitude = nodeLog["longitude"].InnerText.ToDouble();
                    type = nodeLog["type"].InnerText.ToString();

                    #region Valida Informações passadas

                    /*
                        if(idDoutor.HasValue){
                            //verifica doutor
                            DoutorRepository doutorRepository = new DoutorRepository();
                            var doutor = doutorRepository.GetDoutor(idDoutor.Value);
                        
                            if (doutor == null)
                                return new Answer(Answer.EnumStatus.Erro, "Doutor não existente");
                        }
                        
                        //verifica VA
                        ProdutoVaRepository vaRepository = new ProdutoVaRepository();
                        var va = vaRepository.GetProdutoVa(idVa);

                        if (va == null)
                            return new Answer(Answer.EnumStatus.Erro, "Visual Aid não existente");

                        //verifica slide
                        ProdutoVaSlideRepository slideRepository = new ProdutoVaSlideRepository();
                        var slide = slideRepository.GetProdutoVaSlide(idSlide);

                        if (slide == null)
                            return new Answer(Answer.EnumStatus.Erro, "Página não existente");

                        if (slide.IdVa != va.Id)
                        return new Answer(Answer.EnumStatus.Erro, "Página não pertence ao Visual Aid informado");
                    */

                    #endregion
                }

                RelatorioPaginaRepository paginaRepository = new RelatorioPaginaRepository();
                DoutorCadastroRepository doutorCadastroRepository = new DoutorCadastroRepository();

                foreach (XmlNode nodeLog in nodeLogs.GetElementsByTagName("log"))
                {
                    type = nodeLog["type"].InnerText.ToString();

                    if (type == "presentation") { 
                        //idDoutor = nodeLog["doctorId"].InnerText.ToInt();
                        idVa = nodeLog["presentationId"].InnerText.ToInt();
                        idSlide = nodeLog["presentationPageId"].InnerText.ToInt();
                        segundos = nodeLog["duration"].InnerText.ToDouble();
                        data = nodeLog["dateOfStart"].InnerText.ToDateTime();
                        crm = nodeLog["crm"].InnerText.ToString();
                        crmUf = nodeLog["crmUf"].InnerText.ToString();
                        gpsLatitude = nodeLog["latitude"].InnerText.ToDouble();
                        gpsLongitude = nodeLog["longitude"].InnerText.ToDouble();

                        #region grava relatório

                    RelatorioPagina pagina = new RelatorioPagina();

                    pagina.IdTerritorio = territorio.Id;
                    //pagina.IdDoutor_temp = idDoutor;
                    pagina.IdVa_temp = idVa;
                    pagina.IdSlide_temp = idSlide;
                    pagina.Segundos = segundos;
                    pagina.DataInicio = data;
                    pagina.Crm = crm;
                    pagina.CrmUf = crmUf;
                    pagina.GpsLatitude = gpsLatitude;
                    pagina.GpsLongitude = gpsLongitude;

                    paginaRepository.Add(pagina);
                    paginaRepository.Save();

                    try
                    {
                        if (crm.Trim() != "" && crmUf.Trim() != "")
                        {
                            var doutorCadastro = doutorCadastroRepository.GetDoutorCadastros().Where(d => d.CRM == crm && d.CRMUF == crmUf).FirstOrDefault();

                            if (doutorCadastro == null)
                            {
                                doutorCadastro = new DoutorCadastro();
                                doutorCadastro.CRM = crm;
                                doutorCadastro.CRMUF = crmUf;

                                doutorCadastroRepository.Add(doutorCadastro);
                                doutorCadastroRepository.Save();
                            }

                            if (doutorCadastro != null)
                            {
                                pagina.IdDoutorCadastro = doutorCadastro.Id;
                                paginaRepository.Save();
                            }
                        }
                    }
                    catch { }

                    try{
                        pagina.IdVa = idVa;
                        pagina.IdSlide = idSlide;
                        //pagina.IdDoutor = idDoutor;

                        paginaRepository.Save();

                    }catch(Exception e){

                        pagina.IdVa = null;
                        pagina.IdSlide = null;
                        pagina.Xml = xml;
                        paginaRepository.Save();

                        Util.Sistema.Error.TrataErro(e, enviarEmail: false);

                    }

                    #endregion
                    }
                    else if (type == "html")
                    {
                        //faz nada
                    }
                }

                

                
            }
            catch(Exception e)
            {
                Util.Sistema.Error.TrataErro(e, enviarEmail: false);

                return new Answer(Answer.EnumStatus.Erro, "Erro ao fazer a leitura do arquivo xml");
            }

            #endregion

            return new Answer(Answer.EnumStatus.Ok, String.Empty);
        }

        //interno: VA 19 arquivo 80
        //
        //<log>
        //   <user>
        //      <territory>1101</territory>
        //      <password>16a5c46b4a1fd2094aab77d5315f6b64</password>
        //   </user>
        //   <emails>
        //      <email>
        //         <presentationId>18</presentationId>
        //         <crm>34565</crm>
        //         <crmUf>SP</crmUf>
        //         <email>rodrigo.sales@guiropa.com</email>
        //         <attachmentId>16</attachmentId>
        //         <date>2011-04-14 15:28:18</date>
        //      </email>
        //   </emails>
        //</log>
        [WebMethod]
        public Answer LogEmail(string xml)
        {
            ProdutoVaArquivoRepository arquivoRepository = new ProdutoVaArquivoRepository();
            ProdutoVaRepository vaRepository = new ProdutoVaRepository();

            string crm = string.Empty;
            string crmUf = string.Empty;
            string email = string.Empty;
            int idArquivo = 0;
            int idVa = 0;
            DateTime? data = null;

            string userTerritorio;
            string userSenha;

            #region lê informações do Xml passado

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(new StringReader(xml));

                XmlElement nodeUsuario = (XmlElement)doc.GetElementsByTagName("user")[0];
                userTerritorio = nodeUsuario.GetElementsByTagName("territory")[0].InnerText;
                userSenha = nodeUsuario.GetElementsByTagName("password")[0].InnerText;

                XmlElement nodeLogs = (XmlElement)doc.GetElementsByTagName("emails")[0];

                //faz autenticação
                var territorio = GetTerritorio(userTerritorio, userSenha);

                if (territorio == null)
                    return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

                foreach (XmlNode nodeLog in nodeLogs.ChildNodes)
                {
                    crm = nodeLog["crm"].InnerText.ToString();
                    crmUf = nodeLog["crmUf"].InnerText.ToString();
                    email = nodeLog["email"].InnerText.ToString();
                    idArquivo = nodeLog["attachmentId"].InnerText.ToInt();
                    idVa = nodeLog["presentationId"].InnerText.ToInt();
                    data = nodeLog["date"].InnerText.ToDateTime();

                    #region Valida Informações passadas

                    /*
                    //verifica VA
                    var va = vaRepository.GetProdutoVa(idVa);

                    if (va == null)
                        return new Answer(Answer.EnumStatus.Erro, "VA não existente");

                    //verifica arquivo
                    var arquivo = arquivoRepository.GetProdutoVaArquivo(idArquivo);

                    if (arquivo == null)
                        return new Answer(Answer.EnumStatus.Erro, "Arquivo não existente");

                    if (!va.ProdutoVaArquivos.Any(a => a.Id == arquivo.Id))
                        return new Answer(Answer.EnumStatus.Erro, "Arquivo não pertence ao VA informado");
                    */

                    #endregion
                }

                RelatorioEmailRepository relatorioEmailRepository = new RelatorioEmailRepository();

                foreach (XmlNode nodeLog in nodeLogs.ChildNodes)
                {
                    crm = nodeLog["crm"].InnerText.ToString();
                    crmUf = nodeLog["crmUf"].InnerText.ToString();
                    email = nodeLog["email"].InnerText.ToString();
                    idArquivo = nodeLog["attachmentId"].InnerText.ToInt();
                    idVa = nodeLog["presentationId"].InnerText.ToInt();
                    data = nodeLog["date"].InnerText.ToDateTime();

                    #region grava relatório

                    RelatorioEmail relatorioEmail = new RelatorioEmail();

                    relatorioEmail.IdTerritorio = territorio.Id;
                    relatorioEmail.IdVa_temp = idVa;
                    relatorioEmail.IdProdutoVaArquivo_temp = idArquivo;
                    relatorioEmail.Data = data;
                    relatorioEmail.Crm = crm;
                    relatorioEmail.CrmUf = crmUf;
                    relatorioEmail.Email = email;
                    relatorioEmail.IdTerritorio = territorio.Id;
                    relatorioEmail.Status = (char?)RelatorioEmail.EnumStatus.Pendente;

                    relatorioEmailRepository.Add(relatorioEmail);
                    relatorioEmailRepository.Save();

                    try
                    {
                        relatorioEmail.IdVa = idVa;
                        relatorioEmail.IdProdutoVaArquivo = idArquivo;

                        relatorioEmailRepository.Save();
                    }
                    catch (Exception e)
                    {
                        relatorioEmail.IdVa = null;
                        relatorioEmail.IdProdutoVaArquivo = null;

                        relatorioEmail.Xml = xml;
                        relatorioEmailRepository.Save();

                        Util.Sistema.Error.TrataErro(e, enviarEmail: false);
                    }

                    #endregion
                    
                }

                relatorioEmailRepository.Save();

                /*foreach (XmlNode nodeLog in nodeLogs.ChildNodes)
                {
                    idArquivo = nodeLog["attachmentId"].InnerText.ToInt();
                    email = nodeLog["email"].InnerText.ToString();

                    var arquivo = arquivoRepository.GetProdutoVaArquivo(idArquivo);

                    try
                    {
                        List<Attachment> attachments = new List<Attachment>{
                            new Attachment(arquivo.GetCaminhoFisico())
                        };

                        Util.Email.Enviar(email, Util.Email.GetCorpoEmail("logEmail"), "Anexo", attachments);

                        cont++;
                    }
                    catch(Exception e)
                    {
                        erros++;
                    }
                }*/

            }
            catch (Exception e)
            {
                Util.Sistema.Error.TrataErro(e);

                return new Answer(Answer.EnumStatus.Erro, "Erro ao fazer a leitura do arquivo xml");
            }

            #endregion

            return new Answer(Answer.EnumStatus.Ok, string.Empty);
        }

        [WebMethod]
        public Answer LogVersion(string territory, string password, string version)
        {
            var territorio = GetTerritorio(territory, password);

            if (territorio == null)
                return new Answer(Answer.EnumStatus.ErroLogin, "Login e/ou senha inválido(s)");

            territorio.AppVersion = version;

            db.SubmitChanges();

            return new Answer(Answer.EnumStatus.Ok, string.Empty);
        }

    }
}