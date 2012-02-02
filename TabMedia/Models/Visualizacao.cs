using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    [MetadataType(typeof(Visualizacao_Validation))]
    public partial class Visualizacao
    {
        public enum EnumContexto
        {
            Artigo,
            Galeria,
            Noticia,
            Fanfic,
            TalkShow,
            Video,
            Guia
        }

        /// <summary>
        /// Adiciona a visualização do objeto caso o mesmo nao tenha tido visualização hoje, retorna se o objeto (de acordo com id) ja teve alguma visualização do cliente hoje (dentro do contexto especificado)
        /// </summary>
        /// <param name="contextoVisualizacao"></param>
        /// <param name="idObjeto"></param>
        /// <returns></returns>
        public static bool AddVisualizacao(Visualizacao.EnumContexto contextoVisualizacao, int idObjeto)
        {
            VisualizacaoRepository visualizacaoRepository = new VisualizacaoRepository();

            //deleta as visitas de ontem
            visualizacaoRepository.DeletaVisualizacoesOntem();

            String ipCliente = HttpContext.Current.Request.UserHostAddress;
            Visualizacao.EnumContexto contexto = contextoVisualizacao;

            //verifica se já há a visualização hoje, se não grava no banco e incrementa as visualizações do objeto
            bool temVisualizacao = visualizacaoRepository.TemVisualizacao(ipCliente, contexto, idObjeto);

            if (!temVisualizacao)
            {
                Visualizacao visualizacao = new Visualizacao();
                visualizacao.Contexto = contexto.ToString();
                visualizacao.IdObjeto = idObjeto;
                visualizacao.IP = ipCliente;

                visualizacaoRepository.Add(visualizacao);
                visualizacaoRepository.Save();

                return true;
            }
            return false;
        }

    }

    public class Visualizacao_Validation
    {

    }
}
