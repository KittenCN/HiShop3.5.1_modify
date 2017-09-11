namespace Hidistro.Entities.VShop
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.All, AllowMultiple=true)]
    public sealed class EnumShowTextAttribute : Attribute
    {
        public EnumShowTextAttribute(string showTest)
        {
            this.ShowText = showTest;
        }

        public string ShowText { get; private set; }
    }
}

