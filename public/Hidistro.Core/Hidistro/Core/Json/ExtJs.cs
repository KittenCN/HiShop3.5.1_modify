namespace Hidistro.Core.Json
{
    using System;
    using System.IO;
    using System.Text;

    public class ExtJs
    {
        public static string Error(string msg, params JsonAttribute[] Attrs)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"success\":false,\"msg\":\"");
            builder.Append(msg.Replace("\n\n", "").Replace("\r\n", "").Replace("\"", "'").Trim());
            builder.Append("\"");
            foreach (JsonAttribute attribute in Attrs)
            {
                builder.Append(string.Format(",\"{0}\":\"{1}\"", attribute.Name, attribute.Value.ToString()));
            }
            builder.Append("}");
            return builder.ToString();
        }

        public static string JsonData(object data, int Total, params JsonAttribute[] Attrs)
        {
            if (data == null)
            {
                return "{\"success\":true,\"total\":0,  \"data\":[]}";
            }
            StringBuilder builder = new StringBuilder();
            using (new StringWriter())
            {
                builder.Append("{\"success\":true,\"total\":");
                builder.Append(Total.ToString());
                builder.Append(",\"data\":");
                builder.Append(DefaultResolver.Convert(data));
                foreach (JsonAttribute attribute in Attrs)
                {
                    builder.Append(string.Format(",\"{0}\":{1}", attribute.Name, DefaultResolver.Convert(attribute.Value)));
                }
                builder.Append("}");
            }
            return builder.ToString();
        }

        public static string Success()
        {
            return Success("", new JsonAttribute[0]);
        }

        public static string Success(params JsonAttribute[] Attrs)
        {
            return Success("", Attrs);
        }

        public static string Success(string msg, params JsonAttribute[] Attrs)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{\"success\":true,\"msg\":\"");
            builder.Append(msg.Replace("\n\n", "").Replace("\r\n", "").Replace("\"", "'").Trim());
            builder.Append("\"");
            foreach (JsonAttribute attribute in Attrs)
            {
                builder.Append(string.Format(",\"{0}\":\"{1}\"", attribute.Name, attribute.Value.ToString()));
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}

