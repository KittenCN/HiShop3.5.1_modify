namespace Hishop.Plugins
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=false)]
    public sealed class ConfigElementAttribute : Attribute
    {
        public ConfigElementAttribute(string name)
        {
            this.InputType = Hishop.Plugins.InputType.TextBox;
            this.Name = name;
            this.Nullable = true;
        }

        public string Description { get; set; }

        public Hishop.Plugins.InputType InputType { get; set; }

        public string Name { get; private set; }

        public bool Nullable { get; set; }

        public string[] Options { get; set; }
    }
}

