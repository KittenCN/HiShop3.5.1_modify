namespace Hidistro.Core
{
    using Hidistro.Core.Configuration;
    using System;

    public sealed class DataProviders
    {
        private DataProviders()
        {
        }

        public static object CreateInstance(Provider dataProvider)
        {
            if (dataProvider == null)
            {
                return null;
            }
            Type type = Type.GetType(dataProvider.Type);
            object obj2 = null;
            if (type != null)
            {
                obj2 = Activator.CreateInstance(type);
            }
            return obj2;
        }

        public static object CreateInstance(string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr))
            {
                return null;
            }
            try
            {
                return Activator.CreateInstance(Type.GetType(typeStr));
            }
            catch
            {
                return null;
            }
        }
    }
}

