using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Util
{
    public static class Youtube
    {
        public static String GetIdVideo(string urlYoutube)
        {

            int pos1 = urlYoutube.IndexOf("?v=");
            int pos2 = urlYoutube.IndexOf("&");

            if (pos1 == -1)
            {
                return String.Empty;
            }

            if (pos2 != -1)
            {
                urlYoutube = urlYoutube.Substring(pos1 + 3, pos2 - (pos1 + 3));
            }
            else
            {
                urlYoutube = urlYoutube.Substring(pos1 + 3);
            }

            return urlYoutube;
        }
    }
}
