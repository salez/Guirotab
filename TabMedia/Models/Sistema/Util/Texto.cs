using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;

namespace Util
{
    public static class Texto
    {
        private const string SenhaCaracteresValidos = "abcdefghijklmnopqrstuvwxyz1234567890";

        public static string GerarSenhaAleatoria(int tamanho)
        {
            return GerarSenhaAleatoria(tamanho, SenhaCaracteresValidos);
        }

        public static string GerarSenhaAleatoria(int tamanho, string caracteresValidos)
        {
            int valormaximo = caracteresValidos.Length;

            Random random = new Random(DateTime.Now.Millisecond);

            StringBuilder senha = new StringBuilder(tamanho);

            for (int indice = 0; indice < tamanho; indice++)
                senha.Append(caracteresValidos[random.Next(0, valormaximo)]);

            return senha.ToString();
        }

        /// <summary>
        /// Gerar keywords a partir de texto, limite padr„o de 300 caracteres
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string GerarKeywords(string texto)
        {
            return GerarKeywords(texto, 45);
        }

        /// <summary>
        /// Gerar keywords a partir de texto
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public static string GerarKeywords(string texto, int limitaCaracteres)
        {
            texto = " " + texto.LimitaTexto(limitaCaracteres, "");

            Regex regexAlpha = new Regex("[^a-zA-Z·¡‡¿‰ƒ‚¬„√È…Ë»ÎÀÍ ÌÕÏÃÔœÓŒı’Û”Ú“ˆ÷Ù‘˙⁄˘Ÿ¸‹˚€Á« ]");
            texto = Regex.Replace(texto, " . ", ",");
            texto = Regex.Replace(texto, " .s ", ",");
            texto = regexAlpha.Replace(texto, "")
                .ReplaceInsensitive(" das ", ",")
                .ReplaceInsensitive(" pelo ", ",")
                .ReplaceInsensitive(" pela ", ",")
                .ReplaceInsensitive(" pelos ", ",")
                .ReplaceInsensitive(" pelas ", ",")
                .ReplaceInsensitive(" agora ", ",")
                .ReplaceInsensitive(" mais ", ",")
                .ReplaceInsensitive(" mas ", ",")
                .ReplaceInsensitive(" t„o ", ",")
                .ReplaceInsensitive(" ficar ", ",")
                .ReplaceInsensitive(" ficou ", ",")
                .ReplaceInsensitive(" enquanto ", ",")
                .ReplaceInsensitive(" portanto ", ",")
                .ReplaceInsensitive(" vocÍs ", ",")
                .ReplaceInsensitive(" vocÍ ", ",")
                .ReplaceInsensitive(" ele ", ",")
                .ReplaceInsensitive(" ela ", ",")
                .ReplaceInsensitive(" eles ", ",")
                .ReplaceInsensitive(" elas ", ",")
                .ReplaceInsensitive(" com ", ",")
                .ReplaceInsensitive(" seu ", ",")
                .ReplaceInsensitive(" sua ", ",")
                .ReplaceInsensitive(" seus ", ",")
                .ReplaceInsensitive(" suas ", ",")
                .ReplaceInsensitive(" que ", ",")
                .ReplaceInsensitive(" pra ", ",")
                .ReplaceInsensitive(" por ", ",")
                .ReplaceInsensitive(" do ", ",")
                .ReplaceInsensitive(" do ", ",")
                .ReplaceInsensitive(" da ", ",")
                .ReplaceInsensitive(" das ", ",")
                .ReplaceInsensitive(" de ", ",")
                .ReplaceInsensitive(" ao ", ",")
                .ReplaceInsensitive(" em ", ",")
                .ReplaceInsensitive(" no ", ",")
                .ReplaceInsensitive(" na ", ",")
                .ReplaceInsensitive(" nos ", ",")
                .ReplaceInsensitive(" nas ", ",")
                .ReplaceInsensitive(" este ", ",")
                .ReplaceInsensitive(" esta ", ",")
                .ReplaceInsensitive(" estes ", ",")
                .ReplaceInsensitive(" estas ", ",")
                .ReplaceInsensitive(" isto ", ",")
                .ReplaceInsensitive(" aquilo ", ",")
                .ReplaceInsensitive(" aquele ", ",")
                .ReplaceInsensitive(" aquelas ", ",")
                .ReplaceInsensitive(" aqueles ", ",")
                .ReplaceInsensitive(" aquelas ", ",")
                .ReplaceInsensitive(" para ", ",")
                .ReplaceInsensitive(" outro ", ",")
                .ReplaceInsensitive(" mesmo ", ",")
                .ReplaceInsensitive(" mesmos ", ",")
                .ReplaceInsensitive(" sobre ", ",")
                .ReplaceInsensitive(" disso ", ",")
                .ReplaceInsensitive(" porque ", ",")
                .ReplaceInsensitive(" tudo ", ",")
                .ReplaceInsensitive(" melhor ", ",")
                .ReplaceInsensitive(" melhores ", ",")
                .ReplaceInsensitive(" um ", ",")
                .ReplaceInsensitive(" uma ", ",")
                .ReplaceInsensitive(" uns ", ",")
                .ReplaceInsensitive(" umas ", ",");

            return texto.Trim().Replace(" ", ",");
        }

        /// <summary>
        /// faz o replace ignorando o case.
        /// </summary>
        /// <param name="original"></param>
        /// <param name="pattern"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string ReplaceInsensitive(string original, string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) *
                      (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern,
                                              position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
        }

        /// <summary>
        /// faz o replace de texto entre chaves por string utilizando o tipo anonimo chaves, ex: new {textochave = "textoquesubstitui"} substituir· o que estiver em [textochave] por textoquesubstitui
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="chavesValor">tipo anonimo ex: new {textochave = "textoquesubstitui", textochave2 = "textoquesubstitui"}</param>
        /// <returns></returns>
        public static string ReplaceChaves(string texto, object chavesValor)
        {
            var sourceProperties = new List<PropertyInfo>(chavesValor.GetType().GetProperties());

            foreach (PropertyInfo p in sourceProperties)
            {
                var nome = p.Name;
                var valor = p.GetValue(chavesValor, null);

                if (valor == null)
                {
                    texto = texto.Replace("[" + nome + "]", String.Empty);
                    continue;
                }

                texto = texto.Replace("[" + nome + "]", valor.ToString());
            }

            return texto;
        }


        public static string QueryString(string key)
        {
            return (HttpContext.Current.Request.QueryString[key] != null) ? HttpContext.Current.Request.QueryString[key] : "";
        }

        /// <summary>
        /// Returns the last few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the 
        /// given length the complete string is returned. If length is zero or 
        /// less an empty string is returned
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Right(this string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
            {
                return s.Substring(s.Length - length, length);
            }
            else
            {
                return s;
            }
        }

        /// <summary>
        /// Returns the first few characters of the string with a length
        /// specified by the given parameter. If the string's length is less than the 
        /// given length the complete string is returned. If length is zero or 
        /// less an empty string is returned
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="length">Number of characters to return</param>
        /// <returns></returns>
        public static string Left(this string s, int length)
        {
            length = Math.Max(length, 0);

            if (s.Length > length)
            {
                return s.Substring(0, length);
            }
            else
            {
                return s;
            }
        }

        public static string LimitaTexto(string texto, int maxCaracteres)
        {
            return LimitaTexto(texto, maxCaracteres, "...", true);
        }

        public static string LimitaTexto(string texto, int maxCaracteres, string complemento)
        {
            return LimitaTexto(texto, maxCaracteres, complemento, true);
        }

        public static string LimitaTexto(string texto, int maxCaracteres, bool retirarHtmlTags)
        {
            return LimitaTexto(texto, maxCaracteres, "...", retirarHtmlTags);
        }

        public static string LimitaTexto(string texto, int maxCaracteres, string complemento, bool retirarHtmlTags)
        {
            int pos = 0;

            if (retirarHtmlTags)
            {
                texto = RemoverTagsHtml(texto);
            }

            if (maxCaracteres > texto.Length)
            {
                return texto;
            }

            texto = texto.Substring(0, maxCaracteres);

            if (texto.LastIndexOf(" ") > -1)
            {
                pos = texto.LastIndexOf(" ");
            }

            if (texto.LastIndexOf(".") > -1)
            {
                if (texto.LastIndexOf(".") > pos)
                {
                    pos = texto.LastIndexOf(".");
                }
            }

            if (pos > 0)
            {
                texto = texto.Substring(0, pos);

                if (texto[texto.Length - 1].ToString() == "." || texto[texto.Length - 1].ToString() == " ")
                {
                    texto = texto.Substring(0, texto.Length - 1);
                }
            }

            return texto + complemento;
        }

        public static string RemoverAcentos(string texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);

            StringBuilder sb = new StringBuilder();

            for (int k = 0; k < s.Length; k++)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(s[k]);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(s[k]);
                }
            }

            return sb.ToString();
        }

        public static string RemoverTagsHtml(string html)
        {
            //mÈtodo maior mas tem melhor performance que o Regex.

            char[] array = new char[html.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < html.Length; i++)
            {
                char let = html[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static bool IsNumeric(string value)
        {
            return new Regex("[0-9]").IsMatch(value);
        }

        public static bool IsValidEmailAddress(string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        public static string FormataMoeda(String s)
        {
            return FormataMoeda(s, 2);
        }

        public static string FormataMoeda(String s, int qtdeDecimais)
        {
            return String.Format("{0:F" + qtdeDecimais + "}", s);
        }

        public static String FormataCep(String cep)
        {
            cep = FormataSomenteNumeros(cep);

            return cep.Substring(0, 5) + "-" + cep.Substring(5);
        }

        public static string FormataSomenteNumeros(String s)
        {
            return Regex.Replace(s, "[^0123456789]", string.Empty);
        }

        public enum enumTipoCep
        {
            Numero,
            Digito
        }

        /// <summary>
        /// Retorna numeros ou digitos do cep sem qualquer caractere especial. Ex: 05465000, retorna 05465 ou 000
        /// </summary>
        /// <param name="cep"></param>
        /// <returns></returns>
        public static String SeparaCep(String cep, enumTipoCep tipoCep)
        {
            cep = FormataSomenteNumeros(cep);

            String retorno = "";

            switch (tipoCep)
            {
                case enumTipoCep.Numero:
                    if (cep.Length <= 5)
                    {
                        retorno = cep;
                        break;
                    }

                    retorno = cep.Substring(0, 5);
                    break;
                case enumTipoCep.Digito:
                    if (cep.Length <= 5)
                    {
                        retorno = "";
                        break;
                    }

                    int caracteresRestantes = cep.Length - 5;

                    retorno = cep.Substring(5, caracteresRestantes);
                    break;
                default:
                    break;
            }

            return retorno;
        }

        /// <summary>
        /// retorna String no formato (xx)xxxx-xxxx
        /// </summary>
        /// <param name="DDD"></param>
        /// <param name="numeros"></param>
        /// <returns></returns>
        public static String FormataTelefone(String DDD, String numeros)
        {
            DDD = FormataSomenteNumeros(DDD);
            numeros = FormataSomenteNumeros(numeros);

            return "(" + DDD + ")" + numeros.Substring(0, 4) + "-" + numeros.Substring(4);
        }

        /// <summary>
        /// retorna String no formato (xx)xxxx-xxxx
        /// </summary>
        /// <param name="DDD"></param>
        /// <param name="numeros"></param>
        /// <returns></returns>
        public static String FormataTelefone(String numeros, bool comDDD)
        {
            numeros = Regex.Replace(numeros, "[^0123456789]", string.Empty);

            if (comDDD)
            {
                String ddd = numeros.Substring(0, 2);
                numeros = numeros.Substring(2);

                return "(" + ddd + ")" + numeros.Substring(0, 4) + "-" + numeros.Substring(4);
            }

            return numeros.Substring(0, 4) + "-" + numeros.Substring(4);
        }

        public static String Nl2br(String str)
        {
            return str.Replace("\n", "<br />");
        }
    }
}
