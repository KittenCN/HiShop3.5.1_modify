using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web
{
    public class UploadResult
    {
        public string ErrorMessage { get; set; }

        public string OriginFileName { get; set; }

        public UploadState State { get; set; }

        public string Url { get; set; }
    }


    public enum UploadState
    {
        FileAccessError = -3,
        NetworkError = -4,
        SizeLimitExceed = -1,
        Success = 0,
        TypeNotAllow = -2,
        Unknown = 1
    }

}