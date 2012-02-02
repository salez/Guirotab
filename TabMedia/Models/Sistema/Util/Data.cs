using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Util
{
    /// <summary>
    /// Summary description for Data
    /// </summary>
    public class Data
    {
        public enum FormatoData
        {
            Completo,
            DiaMesAno,
            DiaMes,
            MesAno,
            HoraMinuto,
            HoraMinutoSegundo,
            DiaMesAnoHoraMinuto
        }

        public static bool Valida(String data)
        {
            DateTime temp;
            return DateTime.TryParse(data, out temp);
        }

        public static String Formata(Object data, FormatoData formato)
        {
            FormatoData[] formatos = {
                formato
            };

            return Formata(data, formatos);
        }

        public static String Formata(Object data, FormatoData[] formatos)
        {
            if (data == null)
            {
                return String.Empty;
            }
            return Formata(DateTime.Parse(Convert.ToString(data)), formatos);
        }

        public static String Formata(DateTime data, FormatoData formato)
        {
            FormatoData[] formatos = {
            formato
        };

            return Formata(data, formatos);
        }

        public static String Formata(DateTime data, FormatoData[] formatos)
        {
            String retorno = "";
            String juncao = "";

            foreach (FormatoData formato in formatos)
            {
                switch (formato)
                {
                    case FormatoData.Completo:
                        retorno += juncao + String.Format("{0:dd/MM/yyyy HH:mm:ss}", data);
                        juncao = " ";
                        break;
                    case FormatoData.DiaMesAno:
                        retorno += juncao + String.Format("{0:dd/MM/yyyy}", data);
                        juncao = " ";
                        break;
                    case FormatoData.DiaMes:
                        retorno += juncao + String.Format("{0:dd/MM}", data);
                        juncao = " ";
                        break;
                    case FormatoData.MesAno:
                        retorno += juncao + String.Format("{0:MM/yyyy}", data);
                        juncao = " ";
                        break;
                    case FormatoData.HoraMinuto:
                        retorno += juncao + String.Format("{0:HH:mm}", data);
                        juncao = " ";
                        break;
                    case FormatoData.HoraMinutoSegundo:
                        retorno += juncao + String.Format("{0:HH:mm:ss}", data);
                        juncao = " ";
                        break;
                    case FormatoData.DiaMesAnoHoraMinuto:
                        retorno += juncao + String.Format("{0:dd/MM/yyyy HH:mm}", data);
                        juncao = " ";
                        break;
                    default:
                        retorno += juncao + data.ToString();
                        juncao = " ";
                        break;
                }
            }

            return retorno;
        }

        public static string GetMesExtenso(int mes)
        {
            return GetMesExtenso(mes.ToString());
        }

        public static string GetMesExtenso(string mes)
        {
            mes = mes.ToLower();
            Hashtable meses = new Hashtable();
            meses["1"] = meses["jan"] = meses["janeiro"] = "Janeiro";
            meses["2"] = meses["fev"] = meses["fevereiro"] = "Fevereiro";
            meses["3"] = meses["mar"] = meses["março"] = "Março";
            meses["4"] = meses["abr"] = meses["abril"] = "Abril";
            meses["5"] = meses["mai"] = meses["maio"] = "Maio";
            meses["6"] = meses["jun"] = meses["junho"] = "Junho";
            meses["7"] = meses["jul"] = meses["julho"] = "Julho";
            meses["8"] = meses["ago"] = meses["agosto"] = "Agosto";
            meses["9"] = meses["set"] = meses["setembro"] = "Setembro";
            meses["10"] = meses["out"] = meses["outubro"] = "Outubro";
            meses["11"] = meses["nov"] = meses["novembro"] = "Novembro";
            meses["12"] = meses["dez"] = meses["dezembro"] = "Dezembro";
            return meses[mes].ToString();
        }
    }
}
