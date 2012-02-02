using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Util.Dados;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Mvc.Html;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Reflection;
using System.IO;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Web.Routing;
using Ionic.Zip;

public class CustomRoute
{
    public string Controller { get; set; }
    public string Action { get; set; }

    public CustomRoute(string action, string controller)
    {
        this.Action = action;
        this.Controller = controller;
    }
}

public static class ExtensionsMethods
{

    #region object

    public static int ToInt(this object objeto){
        return int.Parse(objeto.ToString());
    }

    public static double ToDouble(this object objeto)
    {
        return double.Parse(objeto.ToString().Trim().Replace('.',','));
    }

    public static DateTime ToDateTime(this object objeto)
    {
        return DateTime.Parse(objeto.ToString());
    }

    public static decimal ToDecimal(this object objeto)
    {
        return decimal.Parse(objeto.ToString().Replace('.', ','));
    }

    /// <summary>
    /// Converte de milisegundos para formato
    /// </summary>
    /// <param name="seconds">objeto que tenha possibilidade de cast para double</param>
    /// <param name="formato">
    /// <para>[d] = Dias </para>
    /// <para>[D] = Dias Totais Fracionado </para>
    /// <para>[h] = Horas </para>
    /// <para>[H] = Horas Totais</para>
    /// <para>[m] = Minutos </para>
    /// <para>[M] = Minutos Totais</para>
    /// <para>[s] = Segundos </para>
    /// <para>[S] = Segundos Totais</para>
    /// <para>[ms] = Milisegundos </para>
    /// <para>[MS] = Milisegundos Totais</para>
    /// </param>
    /// <returns></returns>
    public static string FromMilisecondsTo(this object milisegundos, string formato)
    {
        return Util.Numeros.FromMilisecondsTo(milisegundos, formato);
    }

    /// <summary>
    /// Converte de segundos para formato
    /// </summary>
    /// <param name="seconds">objeto que tenha possibilidade de cast para double</param>
    /// <param name="formato">
    /// <para>[d] = Dias </para>
    /// <para>[D] = Dias Totais Fracionado </para>
    /// <para>[h] = Horas </para>
    /// <para>[H] = Horas Totais</para>
    /// <para>[m] = Minutos </para>
    /// <para>[M] = Minutos Totais</para>
    /// <para>[s] = Segundos </para>
    /// <para>[S] = Segundos Totais</para>
    /// <para>[ms] = Milisegundos </para>
    /// <para>[MS] = Milisegundos Totais</para>
    /// </param>
    /// <returns></returns>
    public static string FromSecondsTo(this object segundos, string formato)
    {
        return Util.Numeros.FromSecondsTo(segundos, formato);
    }

    /// <summary>
    /// Converte de minutos para formato
    /// </summary>
    /// <param name="seconds">objeto que tenha possibilidade de cast para double</param>
    /// <param name="formato">
    /// <para>[d] = Dias </para>
    /// <para>[D] = Dias Totais Fracionado </para>
    /// <para>[h] = Horas </para>
    /// <para>[H] = Horas Totais</para>
    /// <para>[m] = Minutos </para>
    /// <para>[M] = Minutos Totais</para>
    /// <para>[s] = Segundos </para>
    /// <para>[S] = Segundos Totais</para>
    /// <para>[ms] = Milisegundos </para>
    /// <para>[MS] = Milisegundos Totais</para>
    /// </param>
    /// <returns></returns>
    public static string FromMinutesTo(this object segundos, string formato)
    {
        return Util.Numeros.FromMinutesTo(segundos, formato);
    }

    /// <summary>
    /// Converte de horas para formato
    /// </summary>
    /// <param name="seconds">objeto que tenha possibilidade de cast para double</param>
    /// <param name="formato">
    /// <para>[d] = Dias </para>
    /// <para>[D] = Dias Totais Fracionado </para>
    /// <para>[h] = Horas </para>
    /// <para>[H] = Horas Totais</para>
    /// <para>[m] = Minutos </para>
    /// <para>[M] = Minutos Totais</para>
    /// <para>[s] = Segundos </para>
    /// <para>[S] = Segundos Totais</para>
    /// <para>[ms] = Milisegundos </para>
    /// <para>[MS] = Milisegundos Totais</para>
    /// </param>
    /// <returns></returns>
    public static string FromHoursTo(this object segundos, string formato)
    {
        return Util.Numeros.FromHoursTo(segundos, formato);
    }

    /// <summary>
    /// Converte de dias para formato
    /// </summary>
    /// <param name="seconds">objeto que tenha possibilidade de cast para double</param>
    /// <param name="formato">
    /// <para>[d] = Dias </para>
    /// <para>[D] = Dias Totais Fracionado </para>
    /// <para>[h] = Horas </para>
    /// <para>[H] = Horas Totais</para>
    /// <para>[m] = Minutos </para>
    /// <para>[M] = Minutos Totais</para>
    /// <para>[s] = Segundos </para>
    /// <para>[S] = Segundos Totais</para>
    /// <para>[ms] = Milisegundos </para>
    /// <para>[MS] = Milisegundos Totais</para>
    /// </param>
    /// <returns></returns>
    public static string FromDaysTo(this object segundos, string formato)
    {
        return Util.Numeros.FromDaysTo(segundos, formato);
    }

    #endregion

    #region int

    public static bool IsPar(this int n)
    {
        return (n % 2 == 0);
    }

    public static bool IsImpar(this int n)
    {
        return (n % 2 != 0);
    }

    #endregion

    #region String

    public static bool IsNullOrEmpty(this string s)
    {
        return (s == null || s == String.Empty);
    }

    public static bool IsValidEmailAddress(this string s)
    {
        return Util.Texto.IsValidEmailAddress(s);
    }

    public static bool IsNumeric(this string s)
    {
        return Util.Texto.IsNumeric(s);
    }

    public static bool IsDate(this string s)
    {
        return Util.Data.Valida(s);
    }

    public static string ToMD5(this string s)
    {
        return Util.Criptografia.CriptografaMd5(s);
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
    public static string Right(this string s, int lenght)
    {
        return Util.Texto.Right(s, lenght);
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
        return Util.Texto.Left(s, length);
    }

    public static string LimitaTexto(this string s, int maxCaracteres)
    {
        return Util.Texto.LimitaTexto(s, maxCaracteres);
    }

    public static string LimitaTexto(this string s, int maxCaracteres, string complemento)
    {
        return Util.Texto.LimitaTexto(s, maxCaracteres, complemento);
    }

    public static string LimitaTexto(this string s, int maxCaracteres, bool retirarTagsHtml)
    {
        return Util.Texto.LimitaTexto(s, maxCaracteres, retirarTagsHtml);
    }

    public static string LimitaTexto(this string s, int maxCaracteres, string complemento, bool retirarTagsHtml)
    {
        return Util.Texto.LimitaTexto(s, maxCaracteres, complemento, retirarTagsHtml);
    }

    public static string RemoverTagsHtml(this string s)
    {
        return Util.Texto.RemoverTagsHtml(s);
    }

    public static string RemoverAcentos(this string s)
    {
        return Util.Texto.RemoverAcentos(s);
    }

    public static string GerarUrlAmigavel(this string s)
    {
        return Util.Url.GerarUrlAmigavel(s);
    }

    /// <summary>
    /// gera keywords a partir do texto, limite padrao de 300 caracteres
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string GerarKeywords(this string s)
    {
        return Util.Texto.GerarKeywords(s);
    }

    /// <summary>
    /// gera keywords a partir do texto, limite padrao de 300 caracteres
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string GerarKeywords(this string s, int limitaCaracteres)
    {
        return Util.Texto.GerarKeywords(s, limitaCaracteres);
    }

    public static string FormataMoeda(this string s)
    {
        return Util.Texto.FormataMoeda(s);
    }

    public static string FormataMoeda(this string s, int qtdeDecimais)
    {
        return Util.Texto.FormataMoeda(s, qtdeDecimais);
    }

    public static string FormataCep(this string s)
    {
        return Util.Texto.FormataCep(s);
    }

    public static string FormataSomenteNumeros(this string s)
    {
        return Util.Texto.FormataSomenteNumeros(s);
    }

    public static string FormataTelefone(this string s, bool comDDD)
    {
        return Util.Texto.FormataTelefone(s, comDDD);
    }

    public static string SeparaCep(this string s, Util.Texto.enumTipoCep tipoCep)
    {
        return Util.Texto.SeparaCep(s, tipoCep);
    }

    public static string Nl2br(this string s)
    {
        return Util.Texto.Nl2br(s);
    }

    /// <summary>
    /// Encryptes a string using the supplied key. Encoding is done using RSA encryption.
    /// </summary>
    /// <param name="stringToEncrypt">String that must be encrypted.</param>
    /// <param name="key">Encryptionkey.</param>
    /// <returns>A string representing a byte array separated by a minus sign.</returns>
    /// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
    public static string Encrypt(this string stringToEncrypt, string key)
    {
        if (string.IsNullOrEmpty(stringToEncrypt))
        {
            throw new ArgumentException("An empty string value cannot be encrypted.");
        }

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Cannot encrypt using an empty key. Please supply an encryption key.");
        }
        System.Security.Cryptography.RSACryptoServiceProvider.UseMachineKeyStore = true; 

        CspParameters cspp = new CspParameters();
        cspp.KeyContainerName = key;

        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp);
        rsa.PersistKeyInCsp = true;

        byte[] bytes = rsa.Encrypt(System.Text.UTF8Encoding.UTF8.GetBytes(stringToEncrypt), true);

        return BitConverter.ToString(bytes);
    }

    /// <summary>
    /// Decryptes a string using the supplied key. Decoding is done using RSA encryption.
    /// </summary>
    /// <param name="stringToDecrypt">String that must be decrypted.</param>
    /// <param name="key">Decryptionkey.</param>
    /// <returns>The decrypted string or null if decryption failed.</returns>
    /// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
    public static string Decrypt(this string stringToDecrypt, string key)
    {
        string result = null;

        if (string.IsNullOrEmpty(stringToDecrypt))
        {
            throw new ArgumentException("An empty string value cannot be encrypted.");
        }

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Cannot decrypt using an empty key. Please supply a decryption key.");
        }

        try
        {
            CspParameters cspp = new CspParameters();
            cspp.KeyContainerName = key;

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspp);
            rsa.PersistKeyInCsp = true;

            string[] decryptArray = stringToDecrypt.Split(new string[] { "-" }, StringSplitOptions.None);
            byte[] decryptByteArray = Array.ConvertAll<string, byte>(decryptArray, (s => Convert.ToByte(byte.Parse(s, System.Globalization.NumberStyles.HexNumber))));


            byte[] bytes = rsa.Decrypt(decryptByteArray, true);

            result = System.Text.UTF8Encoding.UTF8.GetString(bytes);

        }
        finally
        {
            // no need for further processing
        }

        return result;
    }

    public static bool IsValidUrl(this string text)
    {
        return Util.Texto.IsValidUrl(text);
    }

    public static bool ContainsAny(this string s, char[] characters)
    {
        foreach (char character in characters)
        {
            if (s.Contains(character.ToString()))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Converts a string to an HTML-encoded string.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string HtmlEncode(this string data)
    {
        return HttpUtility.HtmlEncode(data);
    }

    /// <summary>
    /// Converts a string that has been HTML-encoded for HTTP transmission into a decoded string.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string HtmlDecode(this string data)
    {
        return HttpUtility.HtmlDecode(data);
    }

    public static NameValueCollection ParseQueryString(this string query)
    {
        return HttpUtility.ParseQueryString(query);
    }

    /// <summary>
    /// Encodes a URL string.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string UrlEncode(this string url)
    {
        return HttpUtility.UrlEncode(url);
    }

    /// <summary>
    /// Converts a string that has been encoded for transmission in a URL into a decoded string.
    /// </summary>
    /// <param name="url"></param>
    /// <returns>A decoded string.</returns>
    public static string UrlDecode(this string url)
    {
        return HttpUtility.UrlDecode(url);
    }

    /// <summary>
    /// Encodes the path portion of a URL string for reliable HTTP transmission from the Web server to a client.
    /// </summary>
    /// <param name="url"></param>
    /// <returns>The URL-encoded text.</returns>
    public static string UrlPathEncode(this string url)
    {
        return HttpUtility.UrlPathEncode(url);
    }

    public static T ToEnum<T>(this string value)
        where T : struct
    {
        Debug.Assert(!string.IsNullOrEmpty(value));
        return (T)Enum.Parse(typeof(T), value, true);
    }

    /// <summary>
    /// faz o replace ignorando o case
    /// </summary>
    /// <param name="original"></param>
    /// <param name="oldString"></param>
    /// <param name="newString"></param>
    /// <returns></returns>
    public static string ReplaceInsensitive(this string s, string oldString, string newString)
    {
        return Util.Texto.ReplaceInsensitive(s, oldString, newString);
    }

    /// <summary>
    /// faz o replace de texto entre chaves por string utilizando o tipo anonimo chaves, ex: new {textochave = "textoquesubstitui"} substituirá o que estiver em [textochave] por textoquesubstitui
    /// </summary>
    /// <param name="texto"></param>
    /// <param name="chavesValor">tipo anonimo ex: new {textochave = "textoquesubstitui", textochave2 = "textoquesubstitui"}</param>
    /// <returns></returns>
    public static string ReplaceChaves(this string texto, object chavesValor)
    {
        return Util.Texto.ReplaceChaves(texto, chavesValor);
    }

    /// <summary>
    /// pega o conteudo que esta entre str1 e str2
    /// </summary>
    /// <returns></returns>
    public static string GetConteudoEntre(this string texto, string str1, string str2)
    {
        return Util.Texto.GetConteudoEntre(texto, str1, str2);
    }


    /// <summary>
    /// retorna caminho absoluto da url (resolve o seguinte endereço ex: ~/images/arquivo.ext)
    /// </summary>
    public static string ResolveURL(this string url)
    {
        return Util.Url.ResolveUrl(url);
    }

    /// <summary>
    /// Check if url (http) is available.
    /// </summary>
    /// <param name="httpUri">url to check</param>
    /// <example>

    /// string url = "www.codeproject.com;
    /// if( !url.UrlAvailable())
    ///     ...codeproject is not available
    /// </example>
    /// <returns>true if available</returns>
    public static bool UrlAvailable(this string httpUrl)
    {
        return Util.Texto.UrlAvailable(httpUrl);
    }

    #endregion

    #region Enum

    public static string GetDescription(this Enum value)
    {
        return Util.Dados.EnumHelper.GetDescription(value);
    }

    public static SelectList ToSelectList(this Type enumObj, EnumHelper.TipoValor tipoValor)
    {
        return Util.Dados.EnumHelper.ToSelectList(enumObj, tipoValor);
    }

    /// <summary>
    /// Converte um enum em um ListItemCollection, utiliza o description do enum por padrão
    /// </summary>
    /// <param name="converteValorParaChar">Define se converte o enum para Char antes de colocar no Value do ListItem</param>
    /// <returns></returns>
    public static ListItemCollection ToListCollection(this Type enumtype, EnumHelper.TipoValor tipoValor)
    {
        return Util.Dados.EnumHelper.ToListCollection(enumtype, tipoValor);
    }

    /// <summary>
    /// Converte um enum em um ListItemCollection, para o typeEnum, utilize typeof(Enum)
    /// </summary>
    /// <returns></returns>
    public static ListItemCollection ToListCollection(this Type enumType, EnumHelper.TipoValor tipoValor, Boolean utilizarDescription)
    {
        return Util.Dados.EnumHelper.ToListCollection(enumType, tipoValor, utilizarDescription);
    }

    public static List<T> ToListEnum<T>(this Type enumObj)
    {
        return Util.Dados.EnumHelper.ToListEnum<T>();
    }

    #endregion

    #region IEnumerable

    public static SelectList ToSelectList(this IEnumerable e, String dataValueField, String dataTextField, object objectSelectedValue)
    {
        return new SelectList(e, dataValueField, dataTextField, objectSelectedValue);
    }

    public static SelectList ToSelectList(this IEnumerable e, String dataValueField, String dataTextField)
    {
        return new SelectList(e, dataValueField, dataTextField);
    }

    public static MultiSelectList ToMultiSelectList(this IEnumerable e, String dataValueField, String dataTextField, IEnumerable dataSelectedValues)
    {
        return new MultiSelectList(e, dataValueField, dataTextField, dataSelectedValues);
    }

    public static IEnumerable<T> Randomize<T>(this IEnumerable<T> target)
    {
        Random r = new Random();

        return target.OrderBy(x => (r.Next()));
    }

    public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T item in enumerable)
        {
            action(item);
        }
    }

    public static string ToCSV<T>(this IEnumerable<T> instance, char separator)
    {
        StringBuilder csv;
        if (instance != null)
        {
            csv = new StringBuilder();
            instance.Each(value => csv.AppendFormat("{0}{1}", value, separator));
            return csv.ToString(0, csv.Length - 1);
        }
        return null;
    }

    public static string ToCSV<T>(this IEnumerable<T> instance)
    {
        StringBuilder csv;
        if (instance != null)
        {
            csv = new StringBuilder();
            instance.Each(v => csv.AppendFormat("{0},", v));
            return csv.ToString(0, csv.Length - 1);
        }
        return null;
    }

    public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();

        // column names 
        PropertyInfo[] oProps = null;

        if (varlist == null) return dtReturn;

        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others will follow 
            if (oProps == null)
            {
                oProps = ((Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }

            dtReturn.Rows.Add(dr);
        }
        return dtReturn;
    }

    /// <summary>
    /// Concatenates a specified separator String between each element of a specified enumeration, yielding a single concatenated string.
    /// </summary>
    /// <typeparam name="T">any object</typeparam>
    /// <param name="list">The enumeration</param>
    /// <param name="separator">A String</param>
    /// <returns>A String consisting of the elements of value interspersed with the separator string.</returns>
    public static string ToString<T>(this IEnumerable<T> list, string separator)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var obj in list)
        {
            if (sb.Length > 0)
            {
                sb.Append(separator);
            }
            sb.Append(obj);
        }
        return sb.ToString();
    }

    #endregion

    #region DateTime

    public static String Formata(this DateTime? data, Util.Data.FormatoData formato)
    {
        if (data == null)
            return String.Empty;

        return Util.Data.Formata(data, formato);
    }

    public static String Formata(this DateTime data, Util.Data.FormatoData formato)
    {
        return Util.Data.Formata(data, formato);
    }

    public static String Formata(this DateTime data, Util.Data.FormatoData[] formatos)
    {
        return Util.Data.Formata(data, formatos);
    }

    /// <summary>
    /// DateDiff in SQL style. 
    /// Datepart implemented: 
    ///     "year" (abbr. "yy", "yyyy"), 
    ///     "quarter" (abbr. "qq", "q"), 
    ///     "month" (abbr. "mm", "m"), 
    ///     "day" (abbr. "dd", "d"), 
    ///     "week" (abbr. "wk", "ww"), 
    ///     "hour" (abbr. "hh"), 
    ///     "minute" (abbr. "mi", "n"), 
    ///     "second" (abbr. "ss", "s"), 
    ///     "millisecond" (abbr. "ms").
    /// </summary>
    /// <param name="DatePart"></param>
    /// <param name="EndDate"></param>
    /// <returns></returns>
    public static Int64 DateDiff(this DateTime StartDate, String DatePart, DateTime EndDate)
    {
        Int64 DateDiffVal = 0;
        System.Globalization.Calendar cal = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar;
        TimeSpan ts = new TimeSpan(EndDate.Ticks - StartDate.Ticks);
        switch (DatePart.ToLower().Trim())
        {
            #region year
            case "year":
            case "yy":
            case "yyyy":
                DateDiffVal = (Int64)(cal.GetYear(EndDate) - cal.GetYear(StartDate));
                break;
            #endregion

            #region quarter
            case "quarter":
            case "qq":
            case "q":
                DateDiffVal = (Int64)((((cal.GetYear(EndDate)
                                    - cal.GetYear(StartDate)) * 4)
                                    + ((cal.GetMonth(EndDate) - 1) / 3))
                                    - ((cal.GetMonth(StartDate) - 1) / 3));
                break;
            #endregion

            #region month
            case "month":
            case "mm":
            case "m":
                DateDiffVal = (Int64)(((cal.GetYear(EndDate)
                                    - cal.GetYear(StartDate)) * 12
                                    + cal.GetMonth(EndDate))
                                    - cal.GetMonth(StartDate));
                break;
            #endregion

            #region day
            case "day":
            case "d":
            case "dd":
                DateDiffVal = (Int64)ts.TotalDays;
                break;
            #endregion

            #region week
            case "week":
            case "wk":
            case "ww":
                DateDiffVal = (Int64)(ts.TotalDays / 7);
                break;
            #endregion

            #region hour
            case "hour":
            case "hh":
                DateDiffVal = (Int64)ts.TotalHours;
                break;
            #endregion

            #region minute
            case "minute":
            case "mi":
            case "n":
                DateDiffVal = (Int64)ts.TotalMinutes;
                break;
            #endregion

            #region second
            case "second":
            case "ss":
            case "s":
                DateDiffVal = (Int64)ts.TotalSeconds;
                break;
            #endregion

            #region millisecond
            case "millisecond":
            case "ms":
                DateDiffVal = (Int64)ts.TotalMilliseconds;
                break;
            #endregion

            default:
                throw new Exception(String.Format("DatePart \"{0}\" is unknown", DatePart));
        }
        return DateDiffVal;
    }

    public static bool IsWeekend(this DateTime value)
    {
        return (value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday);
    }

    public static bool IsWeekend(this DayOfWeek d)
    {
        return !d.IsWeekday();
    }

    public static bool IsWeekday(this DayOfWeek d)
    {
        switch (d)
        {
            case DayOfWeek.Sunday:
            case DayOfWeek.Saturday: return false;

            default: return true;
        }
    }

    public static DateTime AddWorkdays(this DateTime d, int days)
    {
        // start from a weekday
        while (d.DayOfWeek.IsWeekday()) d = d.AddDays(1.0);
        for (int i = 0; i < days; ++i)
        {
            d = d.AddDays(1.0);
            while (d.DayOfWeek.IsWeekday()) d = d.AddDays(1.0);
        }
        return d;
    }

    static public int Age(this DateTime dateOfBirth)
    {
        DateTime now = DateTime.Now; //Changes during executing
        int age = now.Year - dateOfBirth.Year;
        if (now < dateOfBirth.AddYears(age))
            age--;
        return age;
    }

    #endregion

    #region HtmlHelper

    public static MvcHtmlString MenuLinkPermissao(this HtmlHelper helper, string linkText, string actionName, string controllerName)
    {
        return MenuLinkPermissao(helper, linkText, actionName, controllerName, null);
    }

    public static MvcHtmlString MenuLinkPermissao(this HtmlHelper helper, string linkText, string actionName, string controllerName, ViewContext context)
    {
        if (Sessao.Site.UsuarioLogado() && Sessao.Site.VerificaPermissao(actionName, controllerName))
        {
            if (context != null && context.GetControllerName().Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) && context.GetActionName().Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
                return LinkExtensions.ActionLink(helper, linkText, actionName, controllerName, new { @class = Util.Sistema.AppSettings.Customizacao.ClasseMenuSelecionado });
            else
                return LinkExtensions.ActionLink(helper, linkText, actionName, controllerName);
        }
        return MvcHtmlString.Empty;
    }

    public static MvcHtmlString MenuLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, ViewContext context)
    {
        if (context.GetControllerName().Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) && context.GetActionName().Equals(actionName, StringComparison.CurrentCultureIgnoreCase))
            return LinkExtensions.ActionLink(helper, linkText, actionName, controllerName, new { @class = Util.Sistema.AppSettings.Customizacao.ClasseMenuSelecionado });
        else
            return LinkExtensions.ActionLink(helper, linkText, actionName, controllerName);
    }

    public static MvcHtmlString MenuLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, ViewContext context, List<CustomRoute> routesAdicionaisParaMenuSelecionado)
    {
        if (context != null && routesAdicionaisParaMenuSelecionado.Any(r => r.Action.Equals(context.GetControllerName(), StringComparison.InvariantCultureIgnoreCase)))
            return LinkExtensions.ActionLink(helper, linkText, actionName, controllerName, new { @class = Util.Sistema.AppSettings.Customizacao.ClasseMenuSelecionado });
        else
            return LinkExtensions.ActionLink(helper, linkText, actionName, controllerName);
    }

    #endregion

    #region ViewContext

    public static string GetControllerName(this ViewContext viewContext)
    {
        return viewContext.RouteData.Values["controller"].ToString();
    }

    public static string GetActionName(this ViewContext viewContext)
    {
        return viewContext.RouteData.Values["action"].ToString();
    }

    #endregion

    #region UrlHelper

    public static string Home(this UrlHelper helper)
    {
        return helper.Content("~/");
    }

    public static string Galeria(this UrlHelper helper)
    {
        return helper.Content("~/images/galeria/");
    }

    public static string Login(this UrlHelper helper, bool retornarParaEstaPagina)
    {
        if (retornarParaEstaPagina)
        {
            return helper.Content(Autenticacao.GetRedirectToLoginPage());
        }
        else
        {
            return helper.Content(Util.Configuracao.AppSettings("UrlLogin"));
        }
    }

    #endregion

    #region Exception

    /// <summary>
    /// Gets the most inner (deepest) exception of a given Exception object
    /// </summary>
    /// <param name="ex">Source Exception</param>
    /// <returns></returns>
    public static Exception GetMostInner(this Exception ex)
    {
        if (ex == null) throw new ArgumentNullException("ex");

        while (ex.InnerException != null)
        {
            ex = ex.InnerException;
        }
        return ex;
    }

    #endregion

    #region DirectoryInfo
    /// <summary>
    /// Recursively create directory
    /// </summary>
    /// <param name="dirInfo">Folder path to create.</param>
    public static void CreateDirectory(this DirectoryInfo dirInfo)
    {
        
        if (dirInfo.Parent != null && !dirInfo.Exists) CreateDirectory(dirInfo.Parent);
        if (!dirInfo.Exists) dirInfo.Create();
    }

    /// <summary>
    /// Delete files in a folder that are like the searchPattern, don't include subfolders.
    /// </summary>
    /// <param name="di"></param>
    /// <param name="searchPattern">DOS like pattern (example: *.xml, ??a.txt)</param>
    /// <returns>Number of files that have been deleted.</returns>
    public static int DeleteFiles(this DirectoryInfo di, string searchPattern)
    {
        return DeleteFiles(di, searchPattern, false);
    }

    /// <summary>
    /// Delete files in a folder that are like the searchPattern
    /// </summary>
    /// <param name="di"></param>
    /// <param name="searchPattern">DOS like pattern (example: *.xml, ??a.txt)</param>
    /// <param name="includeSubdirs"></param>
    /// <returns>Number of files that have been deleted.</returns>
    /// <remarks>
    /// This function relies on DirectoryInfo.GetFiles() which will first get all the FileInfo objects in memory. This is good for folders with not too many files, otherwise
    /// an implementation using Windows APIs can be more appropriate. I didn't need this functionality here but if you need it just let me know.
    /// </remarks>
    public static int DeleteFiles(this DirectoryInfo di, string searchPattern, bool includeSubdirs)
    {
        int deleted = 0;
        foreach (FileInfo fi in di.GetFiles(searchPattern, includeSubdirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
        {
            fi.Delete();
            deleted++;
        }

        return deleted;
    }
    #endregion

    #region IQueryable

    public static IQueryable<t> Paginate<t>(this IQueryable<t> content, int? pageNumber, int pageSize)
    {
        return content.Skip((pageNumber ?? 0) * pageSize).Take(pageSize);
    }

    #endregion

    #region ViewPage

    /// <summary>
    /// Nome do controller atual
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static string GetControllerName(this ViewPage page){
        return page.ViewContext.RouteData.Values["controller"].ToString().ToLower();
    }

    /// <summary>
    /// Nome do controller atual
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static string GetControllerName(this Controller controller)
    {
        return controller.ControllerContext.RouteData.Values["controller"].ToString().ToLower();
    }

    /// <summary>
    /// Nome do action atual
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static string GetActionName(this ViewPage page){
        return page.ViewContext.RouteData.Values["action"].ToString().ToLower();
    }

    /// <summary>
    /// Nome do action atual
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public static string GetActionName(this Controller controller)
    {
        return controller.ControllerContext.RouteData.Values["action"].ToString().ToLower();
    }

    #endregion

    #region Controller

    

    #endregion

    #region ZipFile

    public static void ExtractFilesInFolder(this ZipFile zip, string outputDirectory, string folder)
    {
        var selection = (from e in zip.Entries
                         where (e.FileName).StartsWith(folder + "/")
                         select e);
        
        Util.Arquivo.CreateDirectoryIfNotExists(outputDirectory);

        selection.ToList().ForEach(e =>
        {
            var caminhoDiretorio = System.IO.Path.GetDirectoryName(e.FileName).Replace(folder,"");

            //if (!caminhoDiretorio.IsNullOrEmpty())
            //{
            //    Util.Arquivo.CreateDirectoryIfNotExists(caminhoDiretorio);
            //}
            
            var nome = System.IO.Path.GetFileName(e.FileName);

            if (nome != string.Empty)
            { //nao copia pasta
                var aux = e.FileName;
                e.FileName = nome;
                e.Extract(outputDirectory + "/" + caminhoDiretorio, ExtractExistingFileAction.OverwriteSilently);
                e.FileName = aux;
            }
        });
    }

    public static IEnumerable<ZipEntry> GetFilesInFolder(this ZipFile zip, string folder)
    {
        var files = (from e in zip.Entries
                         where (e.FileName).StartsWith(folder + "/")
                         select e);

        //pula 1 pois o primeiro é o diretorio
        return files.Skip(1);
    }

    public static bool FolderExists(this ZipFile zip, string folder)
    {
        var selection = (from e in zip.Entries
                         where (e.FileName).StartsWith(folder + "/")
                         select e);

        return selection.Any();
    }

    #endregion

}