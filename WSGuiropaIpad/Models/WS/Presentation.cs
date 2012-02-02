using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.WS
{
    /// <summary>
    /// Presentation = VA
    /// </summary>
    public class Presentation
    {
        public String Id { get; set; }
        public String ProductId { get; set; }
        public String Title { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public String Keywords { get; set; }

        private string _Versao = string.Empty;
        public String Version {
            get {
                return _Versao; 
            } 
            set {
                if (value == null)
                {
                    _Versao = "0";
                }
                else
                {
                    _Versao = value;
                }
            }
        }
        public String LineName { get; set; }
        public String BundleURL { get; set; }
        public int Status { get; set; }
        public bool Pharmacy { get; set; }
        public String CategoryId { get; set; }
        public String Category { get; set; }
        public String CategoryOrder { get; set; }
        public String Type { get; set; }

        public void PreencherByVA(ProdutoVa va, Territorio territorio, Usuario usuario, ProdutoLinha linha)
        {
            this.Id = va.Id.ToString();
            this.ProductId = va.Produto.Id.ToString();

            if (va.Status == (char)ProdutoVa.EnumStatus.Teste)
            {
                this.Title = va.Produto.Nome;
            }
            else
            {
                this.Title = va.Produto.Nome;
            }

            this.Name = va.Nome;
            this.Description = va.Descricao;
            this.Keywords = va.PalavrasChave;

            if (va.Status == (char)ProdutoVa.EnumStatus.Teste || va.Versao == null)
            {
                this.Version = va.VersaoTeste.ToString();
            }
            else
            {
                if (va.Versao != null)
                {
                    this.Version = va.Versao.Value.ToString();
                }
            }

            this.Status = va.GetStatusWS();

            if (territorio != null)
            {
                if (territorio.ProdutoLinha != null) { 
                    this.LineName = territorio.ProdutoLinha.Nome;
                }
            }
            else if (linha != null)
            {
                this.LineName = linha.Nome;
            }
            else
            {
                //territorio simulado por usuario
                this.LineName = "TESTE";
            }

            if (territorio != null)
            {
                this.BundleURL = va.GetUrlDownload(territorio.Id);
            }
            else if (usuario != null)
            {
                //territorio simulado por usuario
                this.BundleURL = va.GetUrlDownload(usuario.TerritorioSimulado);
            }
            else if (linha != null)
            {
                this.BundleURL = va.GetUrlDownload(linha.TerritorioSimulado);
            }

            this.Pharmacy = va.ProdutoVaSlides.Any(s => s.ProdutoVaSlideArquivos.First().Tipo == (char)ProdutoVaSlideArquivo.EnumTipoArquivo.Farmacia);

            if (va.ProdutoVaCategoria != null) { 
                this.CategoryId = va.ProdutoVaCategoria.Id.ToString();
                this.Category = va.ProdutoVaCategoria.Nome;
                this.Type = va.ProdutoVaCategoria.Tipo.ToString();
            }

        }

    }
}
