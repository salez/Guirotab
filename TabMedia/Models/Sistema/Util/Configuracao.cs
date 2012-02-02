using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.ComponentModel;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Web;

namespace Util
{
    /// <summary>
    /// Summary description for Configuracao
    /// </summary>
    [DataObject(true)]
    public static class Configuracao
    {
        public static string AppSettings(String key)
        {
            return ConfigurationManager.AppSettings[key];
        }

    }
}
