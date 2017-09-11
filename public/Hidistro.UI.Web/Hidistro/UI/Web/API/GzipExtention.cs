namespace Hidistro.UI.Web.API
{
    using System;
    using System.IO.Compression;
    using System.Web;

    public class GzipExtention
    {
        public static void Gzip(HttpContext context)
        {
            string str = context.Request.Headers["Accept-Encoding"].ToString().ToUpperInvariant();
            if (str.Length > 0)
            {
                if (str.Contains("GZIP"))
                {
                    context.Response.AppendHeader("Content-encoding", "gzip");
                    context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                }
                else if (str.Contains("DEFLATE"))
                {
                    context.Response.AppendHeader("Content-encoding", "deflate");
                    context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
                }
            }
        }
    }
}

