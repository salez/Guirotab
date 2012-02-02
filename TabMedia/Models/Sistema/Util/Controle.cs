using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Util
{
    /// <summary>
    /// Summary description for Controle
    /// </summary>
    public static class Controle
    {
        public static void SelecionaValorCheckBoxList(CheckBoxList chk, String valor)
        {
            SelecionaValorCheckBoxList(chk, valor, false);
        }

        public static void SelecionaValorCheckBoxList(CheckBoxList chk, String valor, Boolean marcarValoresIguais)
        {
            foreach (ListItem item in chk.Items)
            {
                if (item.Value == valor)
                {
                    item.Selected = true;
                    if (!marcarValoresIguais)
                    {
                        return;
                    }
                }
            }
        }
    }
}
