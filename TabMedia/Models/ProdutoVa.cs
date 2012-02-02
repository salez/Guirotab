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
    public partial class ProdutoVa
    {
        public enum EnumStatus
        {
            Ativo = 'A',
            Inativo = 'I',
            Pendente = 'P',
            Aprovado = 'O',
            Reprovado = 'R',
            Temporario = 'T'
        }

        public string GetDiretorio()
        {
            return Util.Sistema.DiretorioProdutos + this.IdProduto + "/vas/" + this.Id + "/";
        }

        public string GetDiretorioFisico()
        {
            return Util.Url.GetCaminhoFisico(Util.Sistema.DiretorioProdutos + this.IdProduto + "/vas/" + this.Id + "/");
        }

        public string GetDiretorioArquivos()
        {
            return Util.Sistema.DiretorioProdutos + this.IdProduto + "/vas/" + this.Id + "/arquivos/";
        }

        public string GetDiretorioArquivosFisico()
        {
            return Util.Url.GetCaminhoFisico(Util.Sistema.DiretorioProdutos + this.IdProduto + "/vas/" + this.Id + "/arquivos/");
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

        public void CriaDiretoriosBase()
        {
            //cria pasta para o VA caso não exista
            if (!Directory.Exists(this.GetDiretorioFisico()))
            {
                Directory.CreateDirectory(this.GetDiretorioFisico());
            }

            //cria pasta para os arquivos do VA caso não exista
            if (!Directory.Exists(this.GetDiretorioArquivosFisico()))
            {
                Directory.CreateDirectory(this.GetDiretorioArquivosFisico());
            }
        }
    }
}