using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace Util
{
    public class Url
    {
        public static String GetCaminhoFisico(String caminho)
        {
            if (System.IO.Path.IsPathRooted(caminho))
                return caminho;

            return HttpContext.Current.Server.MapPath(caminho);
        }

        /// <summary>
        /// Gera uma URL que seja amigável, retirando caracteres especiais, multiplos espaços e substituindo espaço por traço
        /// </summary>
        public static string GerarUrlAmigavel(string str)
        {
            str = str.ToLower();

            // invalid chars, make into spaces
            str = Regex.Replace(str, @"[^a-z0-9 -]", "");
            // convert multiple spaces/hyphens into one space       
            str = Regex.Replace(str, @"[ -]+", " ").Trim();
            // cut and trim it
            str = str.Trim();
            // hyphens
            str = Regex.Replace(str, @" ", "-");

            return str;
        }

        /// <summary>
        /// Retorna caminho absoluta da url
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <returns></returns>
        public static string ResolveUrl(string originalUrl)
        {
            if (originalUrl == null)
                return null;

            // *** Absolute path - just return
            if (originalUrl.IndexOf("://") != -1)
                return originalUrl;

            // *** Fix up image path for ~ root app dir directory
            if (originalUrl.StartsWith("~"))
                return VirtualPathUtility.ToAbsolute(originalUrl);

            return originalUrl;
        }


    }
}
