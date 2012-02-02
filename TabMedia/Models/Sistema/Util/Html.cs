using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Util
{
    public class Html
    {
       /* public static void RenderConteudoContexto(string contexto)
        {
            ConteudoRepository conteudoRepository = new ConteudoRepository();

            HttpContext.Current.Response.Write(conteudoRepository.GetConteudoByContexto(contexto).Texto);
        }

        public static string GetConteudoContexto(string contexto)
        {
            ConteudoRepository conteudoRepository = new ConteudoRepository();

            return conteudoRepository.GetConteudoByContexto(contexto).Texto;
        }

        /// <summary>
        /// Renderiza somente o texto do contexto para ser utilizado na metatag description
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="contexto"></param>
        public static string GetDescriptionContexto(string contexto, int limitaTexto)
        {
            ConteudoRepository conteudoRepository = new ConteudoRepository();

            String texto = conteudoRepository.GetConteudoByContexto(contexto).Texto.LimitaTexto(limitaTexto, "", true);

            return texto;
        }*/
    }
}
