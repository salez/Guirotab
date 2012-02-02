using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.WS
{
    /// <summary>
    /// Presentation = VA
    /// </summary>
    public class AppInfo
    {
        public String Version { get; set; }
        public String Download { get; set; }
        public String UrlDownloadProductImages { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }

    public class ProductImage
    {
        public String ProductId;
        public String Url;
    }
}
