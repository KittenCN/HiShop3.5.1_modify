namespace Hidistro.Core.Json
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.IO;

    public class DefaultResolver
    {
        public static string Convert(object value)
        {
            new JsonIgnoreAttribute();
            JsonSerializer serializer = new JsonSerializer {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
            IsoDateTimeConverter item = new IsoDateTimeConverter {
                DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
            };
            EntityConverter converter2 = new EntityConverter();
            serializer.Converters.Add(item);
            serializer.Converters.Add(converter2);
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, value);
                return writer.ToString();
            }
        }

        public static JsonConverterCollection DefaultConveter()
        {
            JsonConverterCollection converters = new JsonConverterCollection();
            IsoDateTimeConverter item = new IsoDateTimeConverter {
                DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss"
            };
            EntityConverter converter2 = new EntityConverter();
            converters.Add(item);
            converters.Add(converter2);
            return converters;
        }
    }
}

