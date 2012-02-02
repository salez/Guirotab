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
    public partial class Doutor
    {
        public void AddEspecialidade(Especialidade especialidade)
        {
            var doutorEspecialidadeRepository = new DoutorEspecialidadeRepository();

            DoutorEspecialidade doutorEspecialidade = new DoutorEspecialidade();

            doutorEspecialidade.IdDoutor = this.Id;
            doutorEspecialidade.IdEspecialidade = especialidade.Id;

            doutorEspecialidadeRepository.Add(doutorEspecialidade);
            doutorEspecialidadeRepository.Save();

        }

        public void AddProduto(Produto produto, int ordem)
        {
            var dpRepository = new DoutorProdutoRepository();

            DoutorProduto dp = new DoutorProduto();

            dp.IdDoutor = this.Id;
            dp.IdProduto = produto.Id;
            dp.Orderm = ordem;

            dpRepository.Add(dp);
            dpRepository.Save();

        }
    }
}