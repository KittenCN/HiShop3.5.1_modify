namespace Hidistro.Core.Json
{
    using Newtonsoft.Json;
    using System;
    using System.Data;

    public class IgnoreEntityKeyMemberConverter : JsonConverter
    {
        private const string EntityKeyMemberFullTypeName = "System.Data.EntityKeyMember";

        public override bool CanConvert(Type objectType)
        {
            return ((objectType == typeof(EntityKey)) || objectType.FullName.StartsWith("System.Data.Objects.DataClasses.EntityCollection"));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }
    }
}

