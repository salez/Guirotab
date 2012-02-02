using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Util
{
    public static class Header
    {
        public static String Javascript(String url)
        {
            return "<script type=\"text/javascript\" src=\"" + VirtualPathUtility.ToAbsolute(url) + "\"></script>";
        }

        public static String Css(String url)
        {
            String media = "screen";
            return Css(url, media);
        }

        public static String Css(String url, String media)
        {
            return "<link type=\"text/css\" rel=\"stylesheet\" href=\"" + VirtualPathUtility.ToAbsolute(url) + "\" media=\"screen\" />";
        }
    }
}
