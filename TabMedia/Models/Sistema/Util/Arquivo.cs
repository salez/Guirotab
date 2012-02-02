using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;

namespace Util
{
    /// <summary>
    /// Summary description for Arquivo
    /// </summary>
    public static class Arquivo
    {
        public static bool Exists(string filePath)
        {
            return Exists(filePath, false);
        }

        public static bool Exists(string filePath, bool delete)
        {
            if (File.Exists(filePath))
            {
                if (delete)
                {
                    File.Delete(filePath);
                }

                return true;
            }

            return false;
        }

    }

}