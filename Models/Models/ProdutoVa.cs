using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models;
using System.ComponentModel;
using System.IO;
using Ionic.Zip;
using System.Web.Mvc;

namespace Models
{
	public partial class ProdutoVa
	{
        public const string chaveEncriptacao = "g5u7i8555";

		public enum EnumStatus
		{
			Ativo = 'A',
			Inativo = 'I',
			Pendente = 'P',
			Aprovado = 'O',
			Reprovado = 'R',
			Temporario = 'T',
            Teste = 'S'
		}

        #region Status

        /// <summary>
        /// Retorna o status do VA
        /// </summary>
        /// <returns></returns>
        public string GetStatus()
        {
            var retorno = string.Empty;

            if (this.Status == (char)EnumStatus.Pendente) {
                retorno = "Pendente (GP)";
            } 
            else if(this.Status == (char)EnumStatus.Aprovado && this.StatusGM == (char)EnumStatus.Pendente)
            {
                retorno = "Pendente (GM)";
            } 
            else if (this.Status == (char)EnumStatus.Aprovado && this.StatusGM == (char)EnumStatus.Aprovado)
            {
                retorno = "Aprovado";
            }
            else if (this.Status == (char)EnumStatus.Reprovado || this.StatusGM == (char)EnumStatus.Reprovado)
            {
                retorno = "Reprovado";
            }
            else
            {
                retorno = ((EnumStatus)this.Status).GetDescription();
            }

            return retorno;
        }

        /// <summary>
        /// Retorna o status do VA (Web Service)
        /// </summary>
        /// <returns></returns>
        public int GetStatusWS()
        {
            int retorno = 0;

            if (this.Status == (char)EnumStatus.Teste)
            {
                retorno = 0; //teste 
            }
            else if ((this.Status == (char)EnumStatus.Pendente && this.StatusGM == (char)EnumStatus.Pendente) || (this.Status == (char)EnumStatus.Pendente && this.StatusGM == null))
            {
                retorno = 6; //"Aguardando aprovação gerente de marketing e do gerente de produto.";
            }
            else if (this.Status == (char)EnumStatus.Aprovado && this.StatusGM == (char)EnumStatus.Pendente)
            {
                retorno = 3; //"Aguardando aprovação gerente de marketing.";
            }
            else if (this.Status == (char)EnumStatus.Pendente)
            {
                retorno = 2;  //"Aguardando aprovação gerente de produto.";
            }
            else if (this.Status == (char)EnumStatus.Ativo)
            {
                retorno = 5;
            }
            else if (this.Status == (char)EnumStatus.Aprovado && this.StatusGM == (char)EnumStatus.Aprovado)
            {
                retorno = 1;
            }
            else if (this.Status == (char)EnumStatus.Reprovado || this.StatusGM == (char)EnumStatus.Reprovado)
            {
                retorno = 4;
            }
            else
            {
                retorno = 7;
            }

            return retorno;
        }

        /// <summary>
        /// Publica o VA (ativa o VA e inativa o VA atualmente ativo)
        /// </summary>
        public void Publicar(ProdutoVaRepository vaRepository)
        {
            if (this.ProdutoVaCategoria.SomenteUmAtivo)
            {
                //inativa o VA ativo do produto deste VA.
                vaRepository.GetProdutoVas()
                    .Where(v => v.IdProduto == this.IdProduto && v.Status == (char)ProdutoVa.EnumStatus.Ativo && v.IdCategoria == this.IdCategoria)
                    .Each(v => v.Status = (char)ProdutoVa.EnumStatus.Inativo);
            }

            this.Status = (char)ProdutoVa.EnumStatus.Ativo;
            this.Versao = this.Produto.GetNovaVersaoVa();

            vaRepository.Save();

            AtualizaVersaoDoutores();

            this.EnviarEmailStatus();
        }

        /// <summary>
        /// Aprova o VA ou o envia para aprovação
        /// </summary>
        public void Aprovar()
        {
            Aprovar(Sessao.Site.RetornaUsuario());
        }

        /// <summary>
        /// Aprova o VA ou o envia para aprovação
        /// </summary>
        public void Aprovar(Usuario usuario)
        {
            //var va = vaRepository.GetProdutoVa(this.Id);

            if (usuario.IsAgencia())
            {
                //envia para aprovação

                this.VersaoTeste = this.Produto.GetNovaVersaoTesteVa(); //da uma versao de teste para o va para que ele seja baixado no iPad
                this.Status = (char)ProdutoVa.EnumStatus.Pendente;
                this.StatusGM = (char)ProdutoVa.EnumStatus.Pendente;
            }

            if (Autenticacao.AutorizaPermissao("aprovar", "produtosvas", usuario))
            {

                if (usuario.IsAdministrador())
                {
                    this.Status = (char)ProdutoVa.EnumStatus.Aprovado;
                    this.StatusGM = (char)ProdutoVa.EnumStatus.Aprovado;
                }

                if (usuario.IsGerenteProduto())
                {
                    this.Status = (char)ProdutoVa.EnumStatus.Aprovado;
                    if (!this.IsAprovado())
                    {
                        this.StatusGM = (char)ProdutoVa.EnumStatus.Pendente;
                    }
                }

                if (usuario.IsGerenteMarketing())
                {
                    this.StatusGM = (char)ProdutoVa.EnumStatus.Aprovado;
                    if (!this.IsAprovado())
                    {
                        this.Status = (char)ProdutoVa.EnumStatus.Pendente;
                    }
                }
            }

            this.EnviarEmailStatus();
        }

        public void Reprovar()
        {
            Reprovar(Sessao.Site.RetornaUsuario());
        }

        public void Reprovar(Usuario usuario)
        {
            if (usuario.IsGerenteMarketing())
            {
                this.StatusGM = (char)ProdutoVa.EnumStatus.Reprovado;
            }

            if (usuario.IsGerenteProduto())
            {
                this.Status = (char)ProdutoVa.EnumStatus.Reprovado;
            }

            this.EnviarEmailStatus();
        }

        public void ColocarEmTeste(ProdutoVaRepository vaRepository)
        {
            //inativa o VA ativo do produto deste VA.
            /*vaRepository.GetProdutoVas()
                .Where(v => v.IdProduto == this.IdProduto && v.Status == (char)ProdutoVa.EnumStatus.Teste)
                .Each(v => v.Status = (char)ProdutoVa.EnumStatus.Inativo);*/

            this.Status = (char)ProdutoVa.EnumStatus.Teste;
            this.VersaoTeste = this.Produto.GetNovaVersaoTesteVa();

            vaRepository.Save();
        }

        public bool IsAprovado()
        {
            return (this.Status == (char)ProdutoVa.EnumStatus.Aprovado && this.StatusGM == (char)ProdutoVa.EnumStatus.Aprovado);
        }

        public bool IsAprovadoByGP()
        {
            return (this.Status == (char)ProdutoVa.EnumStatus.Aprovado);
        }

        public bool IsPendente()
        {
            return (this.Status == (char)ProdutoVa.EnumStatus.Pendente || this.StatusGM == (char)ProdutoVa.EnumStatus.Pendente);
        }

        public bool IsReprovado()
        {
            return (this.Status == (char)ProdutoVa.EnumStatus.Reprovado);
        }

        public bool IsInativo()
        {
            return (this.Status == (char)ProdutoVa.EnumStatus.Inativo);
        }

        public bool IsAtivo()
        {
            return (this.Status == (char)ProdutoVa.EnumStatus.Ativo);
        }

        public bool IsTemporario()
        {
            return (this.Status == (char)ProdutoVa.EnumStatus.Temporario);
        }

        public bool ValidoParaEdicao()
        {
            //se o VA já foi aprovado alguma vez e recebeu versão, ele não pode mais ser editado
            if (this.Versao != null)
                return false;

            return true;
        }

        #endregion

        #region Slides

        public ProdutoVaSlide GetSlideFarmacia()
        {
            return this.ProdutoVaSlides.Where(s => s.Tipo == ProdutoVaSlideArquivo.EnumTipoArquivo.Farmacia).FirstOrDefault();
        }

        #endregion

        #region Email

        /// <summary>
        /// Envia email sobre o status do va para os usuários que tem relação com o produto (gerentes, agências)
        /// </summary>
        public void EnviarEmailStatus()
        {
            string corpoBase = string.Empty;

            var usuarios = this.Produto.UsuarioProdutos.Select(up => up.Usuario);

            if (this.Status == (char)EnumStatus.Pendente) {
                //aguardando aprovação do gerente de produto, envia para o gp
                usuarios = usuarios.Where(u => u.IsGerenteProduto());

                corpoBase = Util.Email.GetCorpoEmail("StatusVaPendente");
            }
            else if (this.StatusGM == (char)EnumStatus.Pendente)
            {
                //aguardando aprovação do gerente de marketing, envia para o gm
                usuarios = usuarios.Where(u => u.IsGerenteMarketing());

                corpoBase = Util.Email.GetCorpoEmail("StatusVaPendente");
            }
            else if (this.Status == (char)EnumStatus.Reprovado)
            {
                //reprovado pelo gerente de produto, envia para agencia
                usuarios = usuarios.Where(u => u.IsAgencia());

                corpoBase = Util.Email.GetCorpoEmail("StatusVaReprovado");
            }
            else if (this.StatusGM == (char)EnumStatus.Reprovado)
            {
                //reprovado pelo gerente de marketing, envia para agencia e gp
                usuarios = usuarios.Where(u => u.IsAgencia() || u.IsGerenteProduto());

                corpoBase = Util.Email.GetCorpoEmail("StatusVaReprovadoGM");
            }
            else
            {
                corpoBase = Util.Email.GetCorpoEmail("StatusVa");
            }

            foreach (var usuario in usuarios) { 

                try
                {
                    var comentarios = string.Empty;

                    foreach (var c in this.ProdutoVaComentarios)
                    {
                        comentarios += "<p><b>" + c.Usuario.Nome + ":</b><br/>" + c.Descricao + "</p>";
                    }

                    var corpo = corpoBase.ReplaceChaves(new
                    {
                        usuario_nome = usuario.Nome,
                        va_data = this.DataInclusao.Formata(Util.Data.FormatoData.DiaMesAno),
                        va_status = this.GetStatus(),
                        produto_nome = this.Produto.Nome,
                        link = Util.Sistema.SiteUrl + Util.Url.UrlHelper().Action("index", "produtosvas", new { id = this.Produto.Id }),
                        comentarios = comentarios
                    });
                       
                    Util.Email.Enviar(usuario.Email, corpo, this.Produto.Nome + " - Status Alterado");

                }
                catch (Exception e)
                {
                    Util.Sistema.Error.TrataErro(e);
                }

            }

        }

        #endregion

        #region Diretórios

        public string GetDiretorio()
		{
			return Util.Sistema.AppSettings.Diretorios.DiretorioProdutos + this.IdProduto + "/vas/" + this.Id + "/";
		}

		public string GetDiretorioFisico()
		{
			return Util.Url.GetCaminhoFisico(this.GetDiretorio());
		}

		public string GetDiretorioArquivos()
		{
			return this.GetDiretorio() + "arquivos/";
		}

		public string GetDiretorioArquivosFisico()
		{
			return Util.Url.GetCaminhoFisico(GetDiretorioArquivos());
		}

        public string GetDiretorioShared()
        {
            return this.GetDiretorio() + "shared/";
        }

        public string GetDiretorioSharedFisico()
        {
            return Util.Url.GetCaminhoFisico(GetDiretorioShared());
        }

        public string GetDiretorioCss()
        {
            return this.GetDiretorio() + "css/";
        }

        public string GetDiretorioCssFisico()
        {
            return Util.Url.GetCaminhoFisico(GetDiretorioCss());
        }

        public string GetDiretorioJs()
        {
            return this.GetDiretorio() + "js/";
        }

        public string GetDiretorioJsFisico()
        {
            return Util.Url.GetCaminhoFisico(GetDiretorioJs());
        }

        public string GetDiretorioImages()
        {
            return this.GetDiretorio() + "images/";
        }

        public string GetDiretorioImagesFisico()
        {
            return Util.Url.GetCaminhoFisico(GetDiretorioImages());
        }

		public void ExcluirDiretorio()
		{
			ExcluirDiretorio(true);
		}

        public void ExcluirDiretorio(bool excluirSubdiretorios)
        {
            try
            {
                Directory.Delete(this.GetDiretorioFisico(), excluirSubdiretorios);
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                //não achou o diretorio, não faz nada.
            }
        }

		public string GetDiretorioModeloIpad()
		{
			return this.GetDiretorio() + "modelo-ipad/";
		}

		public string GetDiretorioModeloIpadFisico()
		{
			return Util.Url.GetCaminhoFisico(this.GetDiretorioModeloIpad());
		}

		public string GetDiretorioModeloIpadArquivos()
		{
			return this.GetDiretorioModeloIpad() + "arquivos/";
		}

		public string GetDiretorioModeloIpadArquivosFisico()
		{
			return Util.Url.GetCaminhoFisico(this.GetDiretorioModeloIpadArquivos());
		}

        #endregion

        #region Outros

        public string GetNomeVAZipado()
		{
			return this.Id + ".zip";
		}

		/// <summary>
		/// recupera o caminho do VA Zipado no formato que a aplicação Ipad necessita
		/// </summary>
		/// <returns></returns>
		public string GetCaminhoVAZipado()
		{
			return this.GetDiretorioModeloIpad() + this.GetNomeVAZipado();
		}

        public string GetUrlDownload()
        {
            if (!Sessao.Site.UsuarioInfo.IsDesenvolvedor())
                return string.Empty;

            return Util.Url.UrlHelper().Content(this.GetCaminhoVAZipado());
        }

        public string GetUrlDownload(string idTerritorio)
        {
            return Util.Sistema.AppSettings.UrlDownloadBase + this.Id + "/" + idTerritorio + "/" + Util.Sistema.GetTokenTerritorio(idTerritorio);
        }

		/// <summary>
		/// recupera o caminho do VA Zipado no formato que a aplicação Ipad necessita
		/// </summary>
		/// <returns></returns>
		public string GetCaminhoVAZipadoFisico()
		{
			return Util.Url.GetCaminhoFisico(this.GetCaminhoVAZipado());
		}

		public void CriaDiretoriosBase()
		{
			//cria pasta para o VA caso não exista
			Util.Arquivo.CreateDirectoryIfNotExists(this.GetDiretorioFisico());

			//cria pasta para os arquivos do VA caso não exista
			Util.Arquivo.CreateDirectoryIfNotExists(this.GetDiretorioArquivosFisico());
		}

        public class Page
        {
            public ProdutoVaSlide Slide;
            public String Conteudo;
        }

		public void CriaArquivosZip()
		{
			//nomes padrão
			var nomePastaArquivos = "attachments";
			var nomePastaPaginas = "pages";
			var nomePastaShared = "shared";
            var nomePastaCss = "css";
            var nomePastaJs = "js";
            var nomePastaImages = "images";
			var nomeArquivoThumb = "thumb.jpg";

			//diretorios padrão
			var diretorioRaiz = this.GetDiretorioModeloIpadArquivosFisico();
			var diretorioArquivos = diretorioRaiz + nomePastaArquivos + @"\";
			var diretorioPaginas = diretorioRaiz + nomePastaPaginas + @"\";
			var diretorioShared = diretorioRaiz + nomePastaPaginas + @"\" + nomePastaShared + @"\";
            var diretorioCss = diretorioRaiz + nomePastaPaginas + @"\" + nomePastaCss + @"\";
            var diretorioJs = diretorioRaiz + nomePastaPaginas + @"\" + nomePastaJs + @"\";
            var diretorioImages = diretorioRaiz + nomePastaPaginas + @"\" + nomePastaImages + @"\";

            Util.Arquivo.DeleteDirectoryIfExists(diretorioRaiz, true);

			// keys do arquivo info.plist
			var keysAttachments = string.Empty;
			List<Page> keysPages = new List<Page>();

			Util.Arquivo.CreateDirectoryIfNotExists(diretorioRaiz);

			#region gera HTMLs

                // SE FOR UM VA - APRESENTAÇÃO
                if (this.ProdutoVaCategoria.Tipo == (char)ProdutoVaCategoria.EnumTipo.Apresentacao)
                {
                    //cria diretorio para as paginas do VA
                    Util.Arquivo.CreateDirectoryIfNotExists(diretorioPaginas);

                    //cria diretorio para os arquivos compartilhados do HTML
                    Util.Arquivo.CreateDirectoryIfNotExists(diretorioShared); 
                    Util.Arquivo.CreateDirectoryIfNotExists(diretorioCss);
                    Util.Arquivo.CreateDirectoryIfNotExists(diretorioJs);
                    Util.Arquivo.CreateDirectoryIfNotExists(diretorioImages);

                    //////////////////////////////////////////
                    #region CRIA THUMB DO VA SE POSSIVEL

                    var primeiroSlide = this.ProdutoVaSlides.OrderBy(s => s.Ordem).FirstOrDefault();

                    if (primeiroSlide.Tipo == ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg)
                    {
                        //coloca o thumb do primeiro slide como thumb do VA
                        var caminhoThumb = Util.Url.GetCaminhoFisico(primeiroSlide.ProdutoVaSlideArquivos.First().GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg));
                        File.Copy(caminhoThumb, diretorioRaiz + nomeArquivoThumb, true);
                    }
                    else if (primeiroSlide.Tipo == ProdutoVaSlideArquivo.EnumTipoArquivo.Zip)
                    {
                        //coloca o thumb do primeiro slide como thumb do VA
                        var caminhoThumb = Util.Url.GetCaminhoFisico(primeiroSlide.ProdutoVaSlideArquivos.First().GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb));

                        if (File.Exists(caminhoThumb))
                        {
                            File.Copy(caminhoThumb, diretorioRaiz + nomeArquivoThumb, true);
                        }
                    }
                    else if (primeiroSlide.Tipo == ProdutoVaSlideArquivo.EnumTipoArquivo.Mp4)
                    {
                        //coloca o thumb do primeiro slide como thumb do VA
                        var caminhoThumb = Util.Url.GetCaminhoFisico(primeiroSlide.ProdutoVaSlideArquivos.First().GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg));

                        if (File.Exists(caminhoThumb))
                        {
                            File.Copy(caminhoThumb, diretorioRaiz + nomeArquivoThumb, true);
                        }
                    }

                    #endregion
                    ////////////////////////////////////////

                    //copia o que tiver na pasta shared da pasta modelo para a pasta shared do VA
                    foreach (var arquivo in Directory.GetFiles(Util.Url.GetCaminhoFisico(Util.Sistema.AppSettings.Diretorios.DiretorioModelos) + @"shared"))
                    {
                        File.Copy(arquivo, diretorioShared + Path.GetFileName(arquivo), true);
                    }

                    var modeloPagina = Util.Arquivo.LerArquivo(Util.Url.GetCaminhoFisico(Util.Sistema.AppSettings.Diretorios.DiretorioModelos) + @"modelo_pagina.html");

                    var modeloPaginaFarmacia = Util.Arquivo.LerArquivo(Util.Url.GetCaminhoFisico(Util.Sistema.AppSettings.Diretorios.DiretorioModelos + "pharmacy/index.html"));

                    var cont = 0;

                    #region cria as paginas do VA

                    var slides = this.ProdutoVaSlides;

                    foreach (var slide in slides.OrderBy(s => s.Ordem))
                    {
                        cont++;
                        var paginaAtual = "page" + cont;
                        var arquivo = slide.ProdutoVaSlideArquivos.First();

                        var diretorioPaginaAtual = diretorioPaginas + paginaAtual + "\\";

                        //cria pasta da pagina
                        Util.Arquivo.CreateDirectoryIfNotExists(diretorioPaginaAtual);

                        var isHTML = false;
                        var isVideo = false;

                        #region cria thumb

                        if (slide.Tipo == ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg)
                        {
                            //coloca o thumb do slide como thumb do VA
                            var caminhoThumb = Util.Url.GetCaminhoFisico(slide.ProdutoVaSlideArquivos.First().GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg));
                            File.Copy(caminhoThumb, diretorioPaginaAtual + "thumb.jpg", true);
                        }
                        else if (slide.Tipo == ProdutoVaSlideArquivo.EnumTipoArquivo.Zip)
                        {
                            //coloca o thumb do slide como thumb do VA
                            var caminhoThumb = Util.Url.GetCaminhoFisico(slide.ProdutoVaSlideArquivos.First().GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb));

                            if (File.Exists(caminhoThumb))
                            {
                                File.Copy(caminhoThumb, diretorioPaginaAtual + "thumb.jpg", true);
                            }
                        }
                        else if (slide.Tipo == ProdutoVaSlideArquivo.EnumTipoArquivo.Mp4)
                        {
                            //coloca o thumb do slide como thumb do VA
                            var caminhoThumb = Util.Url.GetCaminhoFisico(slide.ProdutoVaSlideArquivos.First().GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Thumb, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg));

                            if (File.Exists(caminhoThumb))
                            {
                                File.Copy(caminhoThumb, diretorioPaginaAtual + "thumb.jpg", true);
                            }
                        }

                        #endregion

                        #region cria pagina

                        if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Zip)
                        { //zip

                            isHTML = true;

                            //copia SHARED

                            if (Directory.Exists(this.GetDiretorioSharedFisico()))
                            {
                                Util.Arquivo.CopyDirectoryIfExists(this.GetDiretorioSharedFisico(), diretorioShared, true);

                                //copia o que tiver na pasta shared do VA para a pasta shared do VA
                                //foreach (var arquivoShared in Directory.GetFiles(this.GetDiretorioSharedFisico())){

                                //    File.Copy(arquivoShared, diretorioShared + Path.GetFileName(arquivoShared), true);

                                //}
                            }

                            //copia CSS

                            if (Directory.Exists(this.GetDiretorioCssFisico()))
                            {
                                Util.Arquivo.CopyDirectoryIfExists(this.GetDiretorioCssFisico(), diretorioCss, true);

                                //copia o que tiver na pasta shared do VA para a pasta shared do VA
                                //foreach (var arquivoCss in Directory.GetFiles(this.GetDiretorioCssFisico()))
                                //{
                                //    File.Copy(arquivoCss, diretorioCss + Path.GetFileName(arquivoCss), true);
                                //}
                            }

                            //copia JS

                            if (Directory.Exists(this.GetDiretorioJsFisico()))
                            {
                                Util.Arquivo.CopyDirectoryIfExists(this.GetDiretorioJsFisico(), diretorioJs, true);

                                //copia o que tiver na pasta shared do VA para a pasta shared do VA
                                //foreach (var arquivoJS in Directory.GetFiles(this.GetDiretorioJsFisico()))
                                //{

                                //    File.Copy(arquivoJS, diretorioJs + Path.GetFileName(arquivoJS), true);

                                //}
                            }

                            //copia IMAGES

                            if (Directory.Exists(this.GetDiretorioImagesFisico()))
                            {
                                Util.Arquivo.CopyDirectoryIfExists(this.GetDiretorioImagesFisico(), diretorioImages, true);

                                //copia o que tiver na pasta shared do VA para a pasta shared do VA
                                //foreach (var arquivoImages in Directory.GetFiles(this.GetDiretorioImagesFisico()))
                                //{

                                //    File.Copy(arquivoImages, diretorioImages + Path.GetFileName(arquivoImages), true);

                                //}
                            }



                            var diretorioArquivosPagina = Util.Url.GetCaminhoFisico(this.GetDiretorio() + slide.Id);

                            if (Directory.Exists(diretorioArquivosPagina))
                            {
                                //copia o que tiver na pasta do slide do VA para a pasta do slide do VA do Ipad
                                foreach (var arquivoPagina in Directory.GetFiles(diretorioArquivosPagina))
                                {

                                    File.Copy(arquivoPagina, diretorioPaginas + paginaAtual + "\\" + Path.GetFileName(arquivoPagina), true);

                                }
                            }

                        }
                        else
                        {

                            //copia imagem para a pasta
                            File.Copy(Util.Url.GetCaminhoFisico(arquivo.GetCaminhoArquivo()), diretorioPaginas + paginaAtual + "\\" + arquivo.GetNomeArquivo(), true);

                            //cria pagina
                            File.Delete(diretorioPaginas + paginaAtual + "\\" + "index.html");
                            FileStream fsPagina = File.Open(diretorioPaginas + paginaAtual + "\\" + "index.html", FileMode.Create, FileAccess.Write);

                            StreamWriter swPagina = new StreamWriter(fsPagina, System.Text.Encoding.UTF8);

                            var conteudoArquivo = String.Empty;

                            if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Mp4)
                            { //video

                                //copia imagem do video para a pasta
                                File.Copy(Util.Url.GetCaminhoFisico(arquivo.GetCaminhoArquivo(ProdutoVaSlideArquivo.EnumTamanho.Normal, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg)), diretorioPaginas + paginaAtual + "\\" + arquivo.GetNomeArquivo(ProdutoVaSlideArquivo.EnumTamanho.Normal, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg), true);

                                isVideo = true;

                                //<video id='video_gp_"+arquivo.Id+"' controls='controls' width='100%' height='100%'><source src=\"" + arquivo.GetNomeArquivo() + "\" /></video>
                                conteudoArquivo = modeloPagina.ReplaceChaves(new
                                {

                                    titulo = "Página " + cont,
                                    conteudo = "<div id='divVideo_gp_" + arquivo.Id + "' style='width: 100%; height: 100%;'></div>",
                                    scriptStart =
                                        @"<script type='text/javascript' charset='utf-8'>
                                                    function startAnimation() {
                                                                videoDiv = document.getElementById('divVideo_gp_" + arquivo.Id + @"');
                                                                video = document.createElement('video');
                                                                video.setAttribute('id', 'video_gp_" + arquivo.Id + @"');
                                                                video.setAttribute('controls', 'controls');
                                                                video.setAttribute('width', '100%');
                                                                video.setAttribute('height', '100%');
                                                                videoDiv.appendChild(video);
                
                                                                video.src='" + arquivo.GetNomeArquivo() + @"';
                                                                " + ((arquivo.VideoAutoPlay.HasValue && arquivo.VideoAutoPlay.Value) ? "video.play();" : "") + @"
                                                    }
                                                    function stopAnimation() {
                                                                video = document.getElementById('video_gp_" + arquivo.Id + @"');
                                                                video.pause();
                                                                video.src = '';
                                                                video.parentNode.removeChild(video);
                                                    }
                                            </script>"

                                });
                            }
                            else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg)
                            { //pdf
                                conteudoArquivo = modeloPagina.ReplaceChaves(new
                                {

                                    titulo = "Página " + cont,
                                    conteudo = "<img src=\"" + arquivo.GetNomeArquivo() + "\">",
                                    scriptStart = ""

                                });
                            }
                            else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Farmacia)
                            {
                                //se for farmacia utiliza o modelo de pagina de farmacia
                                conteudoArquivo = modeloPaginaFarmacia.ReplaceChaves(new
                                {

                                    titulo = "Página " + cont,
                                    conteudo = arquivo.GetNomeArquivo(),
                                    scriptStart = ""

                                });
                            }


                            swPagina.Write(conteudoArquivo);

                            swPagina.Close();
                            swPagina.Dispose();


                        }

                        #endregion

                        var page = new Page();

                        page.Slide = slide;

                        if (isVideo)
                        {
                            page.Conteudo = @"<dict>
                                                <key>id</key>
										        <string>" + slide.Id + @"</string>
                                                <key>thumbFile</key>
                                                <string>" + nomePastaPaginas + "/" + paginaAtual + @"/thumb.jpg</string>
                                                <key>items</key>
			                                    <array>
                                                    <dict>
					                                    <key>hidden</key>
					                                    <false/>
					                                    <key>name</key>
					                                    <string>" + nomePastaPaginas + "/" + paginaAtual + "/" + arquivo.GetNomeArquivo(ProdutoVaSlideArquivo.EnumTamanho.Normal, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg) + @"</string>
					                                    <key>type</key>
					                                    <string>image</string>
					                                    <key>x</key>
					                                    <integer>0</integer>
					                                    <key>y</key>
					                                    <integer>0</integer>
                                                        <key>width</key>
					                                    <integer>1024</integer>
                                                        <key>height</key>
					                                    <integer>768</integer>
				                                    </dict>
				                                    <dict>
					                                    <key>loop</key>
					                                    <false/>
                                                        <key>autoplay</key>
                                                        <" + ((arquivo.VideoAutoPlay.HasValue && arquivo.VideoAutoPlay.Value) ? "true" : "false") + @"/>
					                                    <key>name</key>
					                                    <string>" + nomePastaPaginas + "/" + paginaAtual + "/" + arquivo.GetNomeArquivo() + @"</string>
					                                    <key>type</key>
					                                    <string>movie</string>
					                                    <key>width</key>
					                                    <integer>1024</integer>
                                                        <key>height</key>
					                                    <integer>768</integer>
					                                    <key>x</key>
					                                    <integer>0</integer>
					                                    <key>y</key>
					                                    <integer>0</integer>
				                                    </dict>
			                                    </array>
			                                    <key>enableDisableButton</key>
                                                <false/>
                                                <key>enableInteraction</key>
			                                    <true/>
                                              </dict>
                                            ";
                        }
                        else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Farmacia)
                        {

                            page.Conteudo = @"<dict>
										        <key>id</key>
										        <string>" + slide.Id + @"</string>
                                                <key>thumbFile</key>
                                                <string>" + nomePastaPaginas + "/" + paginaAtual + @"/thumb.jpg</string>
                                                <key>items</key>
			                                    <array>
				                                    <dict>
                                                        <key>path</key>
					                                    <string>" + nomePastaPaginas + "/" + paginaAtual + "/" + @"index.html</string>
					                                    <key>height</key>
					                                    <integer>768</integer>
					                                    <key>type</key>
					                                    <string>pharmacy</string>
					                                    <key>width</key>
					                                    <integer>1024</integer>
					                                    <key>x</key>
					                                    <integer>0</integer>
					                                    <key>y</key>
					                                    <integer>0</integer>
                                                    </dict>
                                                </array>
									        </dict>";

                        }
                        else
                        {
                            page.Conteudo = @"<dict>
										        <key>id</key>
										        <string>" + slide.Id + @"</string>
                                                <key>thumbFile</key>
                                                <string>" + nomePastaPaginas + "/" + paginaAtual + @"/thumb.jpg</string>
                                                <key>items</key>
			                                    <array>
				                                    <dict>
                                                        <key>path</key>
					                                    <string>" + nomePastaPaginas + "/" + paginaAtual + "/" + @"index.html</string>
					                                    <key>height</key>
					                                    <integer>768</integer>
					                                    <key>type</key>
					                                    <string>webview</string>
					                                    <key>width</key>
					                                    <integer>1024</integer>
					                                    <key>x</key>
					                                    <integer>0</integer>
					                                    <key>y</key>
					                                    <integer>0</integer>
                                                        <key>enableDisableButton</key>
                                                        <" + ((isHTML) ? "true" : "false") + @"/>
                                                        <key>enableDisableInteraction</key>
			                                            <true/>
                                                    </dict>
                                                </array>
									        </dict>";
                        }

                        //preenche keysPages que será inserido no arquivo info.plist
                        keysPages.Add(page);
                    }

                    #endregion

                }

                if (this.ProdutoVaCategoria.Tipo == (char)ProdutoVaCategoria.EnumTipo.Anexo)
                {
                    //cria diretorio dos arquivos do VA
                    Util.Arquivo.CreateDirectoryIfNotExists(diretorioArquivos);

                    #region cria arquivos anexos do VA

                    foreach (var arquivo in this.ProdutoVaArquivos)
                    {

                        //copia arquivo para a pasta attachments
                        File.Copy(arquivo.GetCaminhoFisico(), diretorioArquivos + arquivo.GetNome(), true);

                        //preenche keysAttachments que será inserido no arquivo info.plist
                        keysAttachments += Util.Texto.NewLine(
                            @"<dict>
                                <key>id</key>
								<string>" + arquivo.Id + @"</string>
								<key>name</key>
								<string>" + arquivo.Nome + @"</string>
								<key>path</key>
								<string>" + nomePastaArquivos + "/" + arquivo.GetNome() + @"</string>
							 </dict>");
                    }

                    #endregion
                }

			#endregion

			#region Gera Arquivo info.plist

				var modelo = Util.Arquivo.LerArquivo(Util.Url.GetCaminhoFisico(Util.Sistema.AppSettings.Diretorios.DiretorioModelos) + @"info.plist");

			    File.Delete(diretorioRaiz + "info.plist");

                //cria arquivo info.plist com todas as paginas
                FileStream fs = File.Open(diretorioRaiz + "info.plist", FileMode.Create, FileAccess.Write);

                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                
                #region Modelo atual de 24/03/2011

                /*
                <?xml version="1.0" encoding="UTF-8"?>
                <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
                <plist version="1.0">
                <dict>
                    <key>attachments</key>
                    <array>
                        <string>attachments/image1.png</string>
                        <string>attachments/image2.png</string>
                        <string>attachments/pdf1.png</string>
                    </array>
                    <key>description</key>
                    <string>Apresentação Institucional da EMS</string>
                    <key>id</key>
                    <string>i01</string>
                    <key>name</key>
                    <string>EMS Institucional</string>
                    <key>pages</key>
                    <array>
                        <dict>
                            <key>id</key>
                            <string>100</string>
                            <key>path</key>
                            <string>pages/page1/index.html</string>
                        </dict>
                        <dict>
                            <key>id</key>
                            <string>101</string>
                            <key>path</key>
                            <string>pages/page2/index.html</string>
                        </dict>
                        <dict>
                            <key>id</key>
                            <string>102</string>
                            <key>path</key>
                            <string>pages/page3/index.html</string>
                        </dict>
                        <dict>
                            <key>id</key>
                            <string>103</string>
                            <key>path</key>
                            <string>pages/page4/index.html</string>
                        </dict>
                        <dict>
                            <key>id</key>
                            <string>104</string>
                            <key>path</key>
                            <string>pages/page5/index.html</string>
                        </dict>
                        <dict>
                            <key>id</key>
                            <string>105</string>
                            <key>path</key>
                            <string>pages/page6/index.html</string>
                        </dict>
                    </array>
                    <key>thumbFile</key>
                    <string>ems-thumb.png</string>
                    <key>version</key>
                    <string>1.0</string>
                </dict>
                </plist>

                 */

            #endregion

                string pages = string.Empty;

                foreach (var page in keysPages)
                {
                    pages += page.Conteudo;
                }

                var endDate = (this.DataLimiteVeiculacao != null) ? this.DataLimiteVeiculacao.Value.Year + "-" + this.DataLimiteVeiculacao.Value.Month.ToString().PadLeft(2, '0') + "-" + this.DataLimiteVeiculacao.Value.Day.ToString().PadLeft(2, '0') + "T09:00:00Z" : ""; 

                var conteudo = modelo.ReplaceChaves(new
                {
                    attachments = keysAttachments,
                    description = this.Nome,
                    id = this.Id,
                    productId = this.Produto.Id,
                    name = this.Produto.Nome,
                    pages = pages,
                    thumbFile = nomeArquivoThumb,
                    version = this.Versao,
                    endDate = endDate
                });
           
                sw.Write(conteudo);

                sw.Close();
                sw.Dispose();

                //cria arquivos info.plist de acordo com a especialidade
                foreach (var especialidade in this.GetEspecialidades())
                {
                    fs = File.Open(diretorioRaiz + "info-" + especialidade.Nome.ToUpper() + ".plist", FileMode.Create, FileAccess.Write);

                    sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

                                                                                                                                                                                                                                                                                #region  Modelo atual de 24/03/2011

            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
            <plist version="1.0">
            <dict>
                <key>attachments</key>
                <array>
                    <string>attachments/image1.png</string>
                    <string>attachments/image2.png</string>
                    <string>attachments/pdf1.png</string>
                </array>
                <key>description</key>
                <string>Apresentação Institucional da EMS</string>
                <key>id</key>
                <string>i01</string>
                <key>name</key>
                <string>EMS Institucional</string>
                <key>pages</key>
                <array>
                    <dict>
                        <key>id</key>
                        <string>100</string>
                        <key>path</key>
                        <string>pages/page1/index.html</string>
                    </dict>
                    <dict>
                        <key>id</key>
                        <string>101</string>
                        <key>path</key>
                        <string>pages/page2/index.html</string>
                    </dict>
                    <dict>
                        <key>id</key>
                        <string>102</string>
                        <key>path</key>
                        <string>pages/page3/index.html</string>
                    </dict>
                    <dict>
                        <key>id</key>
                        <string>103</string>
                        <key>path</key>
                        <string>pages/page4/index.html</string>
                    </dict>
                    <dict>
                        <key>id</key>
                        <string>104</string>
                        <key>path</key>
                        <string>pages/page5/index.html</string>
                    </dict>
                    <dict>
                        <key>id</key>
                        <string>105</string>
                        <key>path</key>
                        <string>pages/page6/index.html</string>
                    </dict>
                </array>
                <key>thumbFile</key>
                <string>ems-thumb.png</string>
                <key>version</key>
                <string>1.0</string>
            </dict>
            </plist>*/

            #endregion

                    pages = string.Empty;

                    foreach (var page in keysPages)
                    {
                        if (page.Slide.ProdutoVaSlideEspecialidades.Any(e => e.Especialidade.Id == especialidade.Id))
                        {
                            pages += page.Conteudo;
                        }
                    }

                    //insere os conteudos no modelo

                    conteudo = modelo.ReplaceChaves(new
                    {
                        attachments = keysAttachments,
                        description = this.Nome,
                        id = this.Id,
                        name = this.Produto.Nome,
                        pages = pages,
                        thumbFile = nomeArquivoThumb,
                        version = this.Versao,
                        endDate = endDate
                    });

                    sw.Write(conteudo);

                    sw.Close();
                    sw.Dispose();

                }

			#endregion

			using (ZipFile zip = new ZipFile())
			{
				zip.AddDirectory(this.GetDiretorioModeloIpadArquivosFisico());

				zip.Save(this.GetDiretorioModeloIpadFisico() + this.GetNomeVAZipado());
			}

		}

        public string GetHTMLVisualizacaoVA()
        {
            return GetHTMLVisualizacaoVA(840,630);
        }

		/// <summary>
		/// retorna <ul></ul> preenchindo com lista de vas
		/// </summary>
		/// <returns>
		/// <ul>
		///     <li>
		///         <img src='[url_imagem]' style='width: 840px;' />
		///     </li>
		///     <li>
		///         <video controls='controls' width='840px' height='630px'><source src='[url_video]' /></video>
		///     </li>
		///     ...
		/// </ul>
		/// </returns>
		public string GetHTMLVisualizacaoVA(int? width, int? height)
		{
			// Initialize some variables.
			String result = String.Empty;

			string modeloImagem = (@"
				<li>
					<img src='[url_imagem]' style='" + ((width != null) ? "width: " + width.Value + "px;" : "") + ((height != null) ? "height: " + height.Value + "px;": "") + @"' />
				</li>").Replace("'", "\"");

			string modeloVideo = (@"
				<li>
					<video controls='controls' " + ((width != null) ? "width='" + width.Value + "px'" : "") + " " + ((height != null) ? "height='"+height.Value+"px'" : "") + @" ><source src='[url_video]' /></video>
				</li>").Replace("'", "\"");

            string modeloIframe = (@"
				<li>
					<iframe src='[url_iframe]' " + ((width != null) ? "width='" + width.Value + "'" : "") + " " + ((height != null) ? "height='" + height.Value + "'" : "") + @" ></iframe>
				</li>").Replace("'", "\"");

			var cont = 1;
			foreach (var slide in this.ProdutoVaSlides.OrderBy(s => s.Ordem))
			{
				var arquivo = slide.ProdutoVaSlideArquivos.First();

                if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Farmacia) //IMAGEM
                {
                    result += modeloImagem.ReplaceChaves(new
                    {
                        url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo())
                    });
                }
				else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg) //IMAGEM
				{
					result += modeloImagem.ReplaceChaves(new
					{
						url_imagem = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo())
					});
				}
                else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Mp4) //IMAGEM
				{
					result += modeloVideo.ReplaceChaves(new
					{
						url_video = Util.Url.ResolveUrl(arquivo.GetCaminhoArquivo())
					});
				}
                else if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Zip) //ZIP
                {
                    result += modeloIframe.ReplaceChaves(new
                    {
                        url_iframe = Util.Url.ResolveUrl(this.GetDiretorio() + slide.Id + "/index.html")
                    });
                }

				cont++;
			}

			return "<ul>" + result + "</ul>";
		}

        public void GravaDownload(string idTerritorio)
        {
            ProdutoVaRepository vaRepository = new ProdutoVaRepository();

            vaRepository.AddDownload(this.Id, idTerritorio);
            vaRepository.Save();
        }

        public void AtualizaVersaoDoutores()
        {
            ProdutoVaRepository vaRepository = new ProdutoVaRepository();

            vaRepository.AtualizaVersaoDoutoresRelacionados(this);

            vaRepository.Save();
        }

        public ProdutoVa GerarCopia()
        {
            ProdutoVaRepository vaRepository = new ProdutoVaRepository();
            ProdutoVaSlideRepository slideRepository = new ProdutoVaSlideRepository();
            ProdutoVaSlideArquivoRepository slideArquivoRepository = new ProdutoVaSlideArquivoRepository();
            ProdutoVaArquivoRepository arquivoRepository = new ProdutoVaArquivoRepository();

            ProdutoVa va = new ProdutoVa();
            va.IdProduto = this.Produto.Id;
            va.IdUsuario = Sessao.Site.UsuarioInfo.Id;
            va.Status = (char)ProdutoVa.EnumStatus.Temporario;
            va.IdCategoria = this.IdCategoria;
            va.Nome = this.Nome;
            va.Descricao = this.Descricao;
            va.PalavrasChave = this.PalavrasChave;
            
            vaRepository.Add(va);
            vaRepository.Save();

            va.CriaDiretoriosBase();

            foreach (var slide in this.ProdutoVaSlides)
            {
                ProdutoVaSlide slideNovo = new ProdutoVaSlide();

                slideNovo.IdVa = va.Id;
                slideNovo.Ordem = slide.Ordem;

                slideRepository.Add(slideNovo);
                slideRepository.Save();

                foreach (var arquivo in slide.ProdutoVaSlideArquivos) { 
                    ProdutoVaSlideArquivo arquivoNovo = new ProdutoVaSlideArquivo();

                    arquivoNovo.IdSlide = slideNovo.Id;
                    arquivoNovo.Nome = arquivo.Nome;
                    arquivoNovo.Tipo = arquivo.Tipo;
                    arquivoNovo.VideoAutoPlay = arquivo.VideoAutoPlay;

                    slideArquivoRepository.Add(arquivoNovo);
                    slideArquivoRepository.Save();

                    if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Zip)
                    {

                        var caminhoOrigem = Util.Url.GetCaminhoFisico(this.GetDiretorio() + slide.Id);
                        var caminhoDestino = Util.Url.GetCaminhoFisico(va.GetDiretorio() + slideNovo.Id);

                        Util.Arquivo.CopyDirectory(caminhoOrigem, caminhoDestino, true);
                        Util.Arquivo.CopyDirectoryIfExists(this.GetDiretorioCssFisico(), va.GetDiretorioCssFisico(), true);
                        Util.Arquivo.CopyDirectoryIfExists(this.GetDiretorioSharedFisico(), va.GetDiretorioSharedFisico(), true);
                        Util.Arquivo.CopyDirectoryIfExists(this.GetDiretorioImagesFisico(), va.GetDiretorioImagesFisico(), true);
                        Util.Arquivo.CopyDirectoryIfExists(this.GetDiretorioJsFisico(), va.GetDiretorioJsFisico(), true);

                    }
                    else {

                        var caminhoOrigem = arquivo.GetCaminhoArquivoFisico();
                        var caminhoDestino = arquivoNovo.GetCaminhoArquivoFisico();
                    
                        File.Copy(caminhoOrigem, caminhoDestino);

                        //se for imagem, copia a thumb também
                        if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg || arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Farmacia) {

                            var caminhoOrigemThumb = arquivo.GetCaminhoArquivoFisico(ProdutoVaSlideArquivo.EnumTamanho.Thumb);
                            var caminhoDestinoThumb = arquivoNovo.GetCaminhoArquivoFisico(ProdutoVaSlideArquivo.EnumTamanho.Thumb);

                            File.Copy(caminhoOrigemThumb, caminhoDestinoThumb);
                        }

                        //se for zip(html), copia a thumb também
                        if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Zip)
                        {
                            var caminhoOrigemThumb = arquivo.GetCaminhoArquivoFisico(ProdutoVaSlideArquivo.EnumTamanho.Thumb, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg);
                            var caminhoDestinoThumb = arquivoNovo.GetCaminhoArquivoFisico(ProdutoVaSlideArquivo.EnumTamanho.Thumb, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg);

                            if (File.Exists(caminhoOrigemThumb)) { 

                                File.Copy(caminhoOrigemThumb, caminhoDestinoThumb);

                            }

                        }

                        //se for video, copia a imagem do primeiro frame do video
                        if (arquivo.Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Mp4)
                        {
                            var caminhoOrigemImagem = arquivo.GetCaminhoArquivoFisico(ProdutoVaSlideArquivo.EnumTamanho.Normal, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg);
                            var caminhoDestinoImagem = arquivoNovo.GetCaminhoArquivoFisico(ProdutoVaSlideArquivo.EnumTamanho.Normal, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg);

                            File.Copy(caminhoOrigemImagem, caminhoDestinoImagem);

                            var caminhoOrigemThumb = arquivo.GetCaminhoArquivoFisico(ProdutoVaSlideArquivo.EnumTamanho.Thumb, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg);
                            var caminhoDestinoThumb = arquivoNovo.GetCaminhoArquivoFisico(ProdutoVaSlideArquivo.EnumTamanho.Thumb, ProdutoVaSlideArquivo.EnumTipoArquivo.Jpg);

                            File.Copy(caminhoOrigemThumb, caminhoDestinoThumb);
                        }
                    }

                }

                //adiciona as especialidades
                slideNovo.AddEspecialidades(slideRepository, slide.ProdutoVaSlideEspecialidades.Select(e => e.Especialidade.Id).ToArray());
                slideRepository.Save();
            }

            foreach (var arquivo in this.ProdutoVaArquivos)
            {
                var arquivoNovo = new ProdutoVaArquivo();

                arquivoNovo.IdVa = va.Id;
                arquivoNovo.Nome = arquivo.Nome;
                arquivoNovo.Tipo = arquivo.Tipo;

                arquivoRepository.Add(arquivoNovo);
                arquivoRepository.Save();

                var caminhoOrigem = arquivo.GetCaminhoFisico();
                var caminhoDestino = arquivoNovo.GetCaminhoFisico();

                File.Copy(caminhoOrigem, caminhoDestino);
            }

            return va;

        }

        public IQueryable<Especialidade> GetEspecialidades()
        {
            DoutorProdutoRepository dpRepository = new DoutorProdutoRepository();
            EspecialidadeRepository especialidadeRepository = new EspecialidadeRepository();

            var especialidadeIds = (from dp in dpRepository.GetDoutorProdutos()
                                  where
                                       dp.Produto.Id == this.Produto.Id
                                  select dp.Doutor.DoutorEspecialidades.First().Especialidade.Id).ToList();

            var especialidades = from e in especialidadeRepository.GetEspecialidades()
                                 where 
                                    especialidadeIds.Contains(e.Id)
                                 select e;

            return especialidades;
        }

        #endregion

        #region Relatório
        
        
        public Relatorio GetRelatorio(string dataInicial, string dataFinal)
        {
            DateTime? dInicial = null;
            DateTime? dFinal = null;

            DateTime aux;

            if (DateTime.TryParse(dataInicial, out aux))
            {
                dInicial = aux;
            }
            if (DateTime.TryParse(dataFinal, out aux))
            {
                dFinal = aux;
            }

            return GetRelatorio(dInicial, dFinal);
        }

        public Relatorio GetRelatorio(DateTime? dataInicial, DateTime? dataFinal)
        {
            return GetRelatorio(dataInicial, dataFinal, null);
        }

        public Relatorio GetRelatorio(DateTime? dataInicial, DateTime? dataFinal, int? idDoutor)
        {
            var relatorio = new Relatorio();

            var relatorioPaginas = this.ProdutoVaSlides.SelectMany(s => s.RelatorioPaginas).Where(rp => rp.DoutorCadastro != null);

            if (dataInicial != null)
            {
                relatorioPaginas = relatorioPaginas.Where(rp => rp.DataInicio >= dataInicial);
            }

            if (dataFinal != null)
            {
                relatorioPaginas = relatorioPaginas.Where(rp => rp.DataInicio < dataFinal.Value.AddDays(1));
            }

            if (idDoutor != null)
            {
                relatorioPaginas = relatorioPaginas.Where(rp => rp.IdDoutorCadastro == idDoutor);
            }

            relatorio.QtdeVisualizacoes = relatorioPaginas.Count();
            relatorio.Segundos = relatorioPaginas.Sum(r => r.Segundos);

            return relatorio;
        }

        //public class Relatorio
        //{
        //    public int QtdeVisualizacoes { get; set; }
        //    public double? Segundos { get; set; }

        //    public string GetTempoTotal()
        //    {
        //        if (this.Segundos == null)
        //            return "-";

        //        var segundos = this.Segundos.Value;

        //        return segundos.FromSecondsTo("[M]:[s]");
        //    }
        //}

        #endregion

        
    }
}