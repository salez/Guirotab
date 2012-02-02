using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models;
using System.ComponentModel;

namespace Models
{
    public partial class ProdutoVaSlide
    {

        public void DeletaArquivosFisicos(){

            //deleta todos arquivos físicos do slide
            foreach (var arquivoSlide in this.ProdutoVaSlideArquivos)
            {
                //pega direotorio do VA
                var diretorio = this.ProdutoVa.GetDiretorioFisico();

                //deleta os arquivos iniciados pelo id do slide (12.jpg e 12_pq.jpg, por exemplo) que representam os arquivos fisicos do slide
                var arquivos = System.IO.Directory.GetFiles(diretorio, arquivoSlide.Id + "*");
                
                foreach (var arquivo in arquivos)
                {
                    System.IO.File.Delete(arquivo);
                }
                
            }

        }

    }
}