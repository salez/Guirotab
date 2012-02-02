using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Web.Mvc;

namespace Util.Dados
{
    /// <summary>
    /// Summary description for Dados
    /// </summary>
    public static class EnumHelper
    {
        public enum TipoValor
        {
            Char,
            Inteiro,
            String
        }

        /// <summary>
        /// Converte um enum em uma lista do respectivo enum
        /// </summary>
        /// <param name="_enum"></param>
        /// <param name="converteValorParaChar">Define se converte o enum para Char antes de colocar no Value do ListItem</param>
        /// <returns></returns>
        public static List<T> ToListEnum<T>()
        {
            Type enumType = typeof(T);
            // Can't use type constraints on value types, so have to do check like this

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");
            return new List<T>(Enum.GetValues(enumType) as IEnumerable<T>);
        }

        /// <summary>
        /// Converte um enum em um ListItemCollection, utiliza o description do enum por padrão
        /// </summary>
        /// <param name="converteValorParaChar">Define se converte o enum para Char antes de colocar no Value do ListItem</param>
        /// <returns></returns>
        public static ListItemCollection ToListCollection(Type enumtype, TipoValor tipoValor)
        {
            return ToListCollection(enumtype, tipoValor, true);
        }

        /// <summary>
        /// Converte um enum em um ListItemCollection, para o typeEnum, utilize typeof(Enum)
        /// </summary>
        /// <returns></returns>
        public static ListItemCollection ToListCollection(Type enumType, TipoValor tipoValor, Boolean utilizarDescription)
        {
            // Can't use type constraints on value types, so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            ListItemCollection listaRetorno = new ListItemCollection();

            foreach (var item in Enum.GetValues(enumType))
            {
                ListItem listItem = new ListItem();

                if (utilizarDescription)
                {
                    listItem.Text = GetDescription(item);
                }
                else
                {
                    listItem.Text = item.ToString();
                }

                switch (tipoValor)
                {
                    case TipoValor.Char:
                        listItem.Value = (Convert.ToChar(item)).ToString();
                        break;
                    case TipoValor.Inteiro:
                        listItem.Value = (Convert.ToInt32(item)).ToString();
                        break;
                    default:
                        listItem.Value = item.ToString();
                        break;
                }

                listaRetorno.Add(listItem);
            }

            return listaRetorno;
        }

        /// <summary>
        /// Converte um enum em um ListItemCollection, utiliza o description do enum por padrão
        /// </summary>
        /// <param name="converteValorParaChar">Define se converte o enum para Char antes de colocar no Value do ListItem</param>
        /// <returns></returns>
        public static SelectList ToSelectList(Type enumType, TipoValor tipoValor)
        {
            return new SelectList(ToListCollection(enumType, tipoValor, true), "Value", "Text");
        }

        public static string GetDescription(Object value)
        {
            if (value.GetType().BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            return GetDescription((Enum)value);
        }

        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}
