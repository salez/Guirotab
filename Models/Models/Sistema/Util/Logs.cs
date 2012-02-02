using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace Util
{
    public class Logs
    {
        public Log.EnumPagina Pagina { get; set; }
        public Log.EnumArea Area { get; set; }
        public String EntidadeNome { get; set; }
        public Log.EnumEntidadeGenero EntidadeGenero { get; set; }

        public Logs(Log.EnumPagina pagina, Log.EnumArea area)
        {
            this.Pagina = pagina;
            this.Area = area;
        }

        public Logs(Log.EnumPagina pagina, Log.EnumArea area, String entidadeNome, Log.EnumEntidadeGenero entidadeGenero)
        {
            this.Pagina = pagina;
            this.Area = area;
            this.EntidadeNome = entidadeNome;
            this.EntidadeGenero = entidadeGenero;
        }

        public void Add(Log.EnumTipo tipo, String descricao, String link)
        {
            this.Add(tipo, descricao, link, 0);
        }

        public void Add(Log.EnumTipo tipo, String descricao, String link, int idObjeto)
        {
            descricao = descricao
                .Replace("[entidade]", EntidadeNome)
                .Replace("[entidade_plural]", EntidadeNome + "s")
                .Replace("[genero]", (EntidadeGenero == Log.EnumEntidadeGenero.Masculino) ? "o" : "a")
                .Replace("[genero_plural]", (EntidadeGenero == Log.EnumEntidadeGenero.Masculino) ? "os" : "as")
                .Replace("[genero_entidade_plural]", ((EntidadeGenero == Log.EnumEntidadeGenero.Masculino) ? "os" : "as") + EntidadeNome + "s")
                .Replace("[genero_entidade]", ((EntidadeGenero == Log.EnumEntidadeGenero.Masculino) ? "o" : "a") + EntidadeNome);

            LogRepository logRepository = new LogRepository();

            Log log = new Log();

            log.Area = (Char)this.Area;
            log.DataInclusao = DateTime.Now;
            log.Descricao = descricao;

            if(Sessao.Site.UsuarioLogado()){
                log.IdUsuario = Sessao.Site.UsuarioInfo.Id;
            }

            if (Sessao.Site.UsuarioTerritorioLogado())
            {
                log.IdTerritorio = Sessao.Site.TerritorioInfo.Id;
            }

            log.Pagina = this.Pagina.ToString();
            log.TipoAcao = (Char)tipo;
            log.Link = Util.Url.ResolveUrl(link);
            log.IdObjeto = idObjeto;

            logRepository.Add(log);
            logRepository.Save();
        }
    }
}
