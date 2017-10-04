using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web
{
    public class UploadConfig
    {
        public string[] AllowExtensions { get; set; }

        public bool Base64 { get; set; }

        public string Base64Filename { get; set; }

        public string PathFormat { get; set; }

        public int SizeLimit { get; set; }

        public string UploadFieldName { get; set; }
    }

}