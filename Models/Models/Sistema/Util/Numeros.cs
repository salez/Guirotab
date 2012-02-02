using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Reflection;
using System.Net;

namespace Util
{
    public static class Numeros
    {
        public enum FormatoNumero{
            Milisegundos,
            Segundos,
            Minutos,
            Horas,
            Dias
        }

        public static string FromSecondsTo(object numero, string formato)
        {
            return FromTimeTo(numero, FormatoNumero.Segundos, formato);
        }

        public static string FromMilisecondsTo(object numero, string formato)
        {
            return FromTimeTo(numero, FormatoNumero.Milisegundos, formato);
        }

        public static string FromDaysTo(object numero, string formato)
        {
            return FromTimeTo(numero, FormatoNumero.Dias, formato);
        }

        public static string FromHoursTo(object numero, string formato)
        {
            return FromTimeTo(numero, FormatoNumero.Horas, formato);
        }

        public static string FromMinutesTo(object numero, string formato)
        {
            return FromTimeTo(numero, FormatoNumero.Minutos, formato);
        }

        /// <summary>
        /// Converte de segundos para <param name="formatoDestino">formato</param>
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
        public static string FromTimeTo(object objNumero, FormatoNumero formatoNumero, string formatoDestino)
        {
            TimeSpan ts;
            
            double numero = (double)objNumero;

            switch (formatoNumero)
	        {
		        case FormatoNumero.Milisegundos:
                    ts = TimeSpan.FromMilliseconds(numero);
                 break;
                case FormatoNumero.Segundos:
                    ts = TimeSpan.FromSeconds(numero);
                 break;
                case FormatoNumero.Minutos:
                    ts = TimeSpan.FromMinutes(numero);
                 break;
                case FormatoNumero.Horas:
                    ts = TimeSpan.FromHours(numero);
                 break;
                case FormatoNumero.Dias:
                    ts = TimeSpan.FromDays(numero);
                 break;
                default:
                    ts = TimeSpan.FromSeconds(numero);
                 break;
	        }

            string retorno = formatoDestino
                                .ReplaceChaves(new
                                {
                                    d = ts.Days,
                                    D = (int)ts.TotalDays,
                                    h = ts.Hours,
                                    H = (int)ts.TotalHours,
                                    m = ts.Minutes,
                                    M = (int)ts.TotalMinutes,
                                    s = ts.Seconds,
                                    S = (int)ts.TotalSeconds,
                                    ms = ts.Milliseconds,
                                    MS = (int)ts.TotalMilliseconds
                                });

            return retorno;
        }

    }
}
