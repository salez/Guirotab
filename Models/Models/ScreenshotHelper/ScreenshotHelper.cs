using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

public class ScreenshotHelper
{

    private const int TIMEOUT = 30000;
    private const string TMP_NAME = "TMP_SHOT.png";

    public void Shot(string url, string caminhoDestino)
    {
        try
        {
            string arguments = url + " " + caminhoDestino;

            var startInfo = new ProcessStartInfo
            {
                Arguments = arguments,
                FileName = Util.Url.GetCaminhoFisico(Util.Sistema.AppSettings.UrlIECapt)
            };
            Process p = Process.Start(startInfo);

            //Process p = Process.Start(rootDir + "\\" + EXTRACTIMAGE_EXE, arguments);
            p.WaitForExit(TIMEOUT);
            if (!p.HasExited)
            {
                p.Kill();
                throw new Exception("Timed out while creating the thumbnail.");
            }
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex.ToString());
            throw ex;
        }
    }


}