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
    public class Relatorio
    {
        public Utilizacao Utilizacao { get; set; }
        public int QtdeVisualizacoes { get; set; }
        public double? Segundos { get; set; }
        public double? Mediana { get; set; }

        public Relatorio()
        {
            this.Utilizacao = new Utilizacao();
        }

        public string GetTempoTotal()
        {
            if (this.Segundos == null)
                return "-";

            var segundos = this.Segundos.Value;

            return segundos.FromSecondsTo("[M]:[s]");
        }

        public double GetMedia()
        {
            if (this.Segundos == null)
                return 0;

            if (this.QtdeVisualizacoes == 0)
                return 0;

            var media = (this.Segundos.Value / this.QtdeVisualizacoes);
            return media;

        }

        public string GetTempoMedio()
        {
            if (this.Segundos == null)
                return "-";

            if (this.QtdeVisualizacoes == 0)
                return "0:0";

            var segundos = this.Segundos.Value / this.QtdeVisualizacoes;

            return segundos.FromSecondsTo("[M]:[s]");
            
        }
    }

    public class Utilizacao
    {
        public int Reps { get; set; }
        public UtilizacaoInstalacao Instalacao { get; set; }
        public UtilizacaoAtualizacao Atualizacao { get; set; }
        public UtilizacaoSincronizacao Sincronizacao { get; set; }

        public class UtilizacaoInstalacao
        {
            public int Ok { get; set; }
            public int Faltam { get; set; }
            public double Porcentagem { get; set; }
        }

        public class UtilizacaoAtualizacao
        {
            public int Ok { get; set; }
            public int Faltam { get; set; }
            public double Porcentagem { get; set; }
        }

        public class UtilizacaoSincronizacao
        {
            public int Hoje { get; set; }
            public int Ontem { get; set; }
            public int AteTresDias { get; set; }
            public int MaisTresDias { get; set; }
        }

        public Utilizacao()
        {
            this.Instalacao = new UtilizacaoInstalacao();
        }
    }
}