using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Models;
using System.ComponentModel;
using System.IO;

namespace Models
{
    public partial class Produto
    {
        public string GetDiretorio()
        {
            return Util.Sistema.DiretorioProdutos + this.Id + "/";
        }

        public string GetDiretorioFisico()
        {
            return Util.Url.GetCaminhoFisico(Util.Sistema.DiretorioProdutos + this.Id + "/");
        }

        public string GetDiretorioVAs()
        {
            return Util.Sistema.DiretorioProdutos + this.Id + "/vas/";
        }

        public string GetDiretorioVAsFisico()
        {
            return  Util.Url.GetCaminhoFisico(Util.Sistema.DiretorioProdutos + this.Id + "/vas/");
        }

        public void CriaDiretoriosBase()
        {
            //cria pasta para o produto caso não exista
            if (!Directory.Exists(this.GetDiretorioFisico()))
            {
                Directory.CreateDirectory(this.GetDiretorioFisico());

                //cria pasta de VAS caso não exista
                if (!Directory.Exists(this.GetDiretorioVAs()))
                {
                    Directory.CreateDirectory(this.GetDiretorioVAs());
                }
            }
        }
    }
}