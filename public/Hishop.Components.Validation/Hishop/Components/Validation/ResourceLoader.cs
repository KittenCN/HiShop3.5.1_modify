namespace Hishop.Components.Validation
{
    using System;
    using System.Reflection;
    using System.Resources;

    public static class ResourceLoader
    {
        private static string LoadAssemblyString(Assembly asm, string baseName, string resourceName)
        {
            try
            {
                ResourceManager manager = new ResourceManager(baseName, asm);
                return manager.GetString(resourceName);
            }
            catch (MissingManifestResourceException)
            {
            }
            return null;
        }

        public static string LoadString(string baseName, string resourceName)
        {
            return LoadString(baseName, resourceName, Assembly.GetCallingAssembly());
        }

        public static string LoadString(string baseName, string resourceName, Assembly asm)
        {
            if (string.IsNullOrEmpty(baseName))
            {
                throw new ArgumentNullException("baseName");
            }
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }
            string str = null;
            if (asm != null)
            {
                str = SearchForResource(asm, baseName, resourceName);
            }
            if (str == null)
            {
                str = LoadAssemblyString(Assembly.GetExecutingAssembly(), baseName, resourceName);
            }
            if (str == null)
            {
                return string.Empty;
            }
            return str;
        }

        private static string SearchForResource(Assembly asm, string baseName, string resourceName)
        {
            foreach (string str in asm.GetManifestResourceNames())
            {
                string str2 = (string) str.Clone();
                if (str.EndsWith(".resources"))
                {
                    str2 = str.Replace(".resources", string.Empty);
                }
                string str3 = LoadAssemblyString(asm, str2, resourceName);
                if (!string.IsNullOrEmpty(str3))
                {
                    return str3;
                }
            }
            return null;
        }
    }
}

