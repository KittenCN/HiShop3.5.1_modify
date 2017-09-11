namespace Hidistro.Core.Json
{
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Objects.DataClasses;
    using System.Reflection;

    public class EntityConverter : JsonConverter
    {
        public List<string> SkipNameList = new List<string>();
        public List<Type> SkipTypeList = new List<Type>();

        public EntityConverter()
        {
            this.SkipNameList.Add("EntityState");
            this.SkipNameList.Add("EntityKey");
            this.SkipTypeList.Add(typeof(RelatedEnd));
            this.SkipTypeList.Add(typeof(EntityObject));
            this.SkipTypeList.Add(typeof(EntityReference));
            this.SkipTypeList.Add(typeof(IDictionary));
            this.SkipTypeList.Add(typeof(IList));
            this.SkipTypeList.Add(typeof(ArrayList));
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(EntityObject).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return null;
        }

        private bool Skip(Type objectType, string FieldName)
        {
            bool flag = false;
            flag = this.SkipNameList.Contains(FieldName);
            if (!flag)
            {
                for (int i = 0; !flag && (i < this.SkipTypeList.Count); i++)
                {
                    flag = this.SkipTypeList[i].IsAssignableFrom(objectType);
                }
            }
            return flag;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            Type type = value.GetType();
            FieldInfo[] fields = type.GetFields();
            PropertyInfo[] properties = type.GetProperties();
            foreach (FieldInfo info in fields)
            {
                if (!this.Skip(info.FieldType, info.Name))
                {
                    writer.WritePropertyName(info.Name);
                    serializer.Serialize(writer, info.GetValue(value));
                }
            }
            foreach (PropertyInfo info2 in properties)
            {
                if (!this.Skip(info2.PropertyType, info2.Name))
                {
                    writer.WritePropertyName(info2.Name);
                    serializer.Serialize(writer, info2.GetValue(value, null));
                }
            }
            writer.WriteEndObject();
        }
    }
}

