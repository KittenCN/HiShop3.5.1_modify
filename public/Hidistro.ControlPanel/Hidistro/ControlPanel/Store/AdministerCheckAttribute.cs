namespace Hidistro.ControlPanel.Store
{
    using System;

    [AttributeUsage(AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Class, Inherited=true, AllowMultiple=false)]
    public class AdministerCheckAttribute : Attribute
    {
        private bool administratorOnly;

        public AdministerCheckAttribute()
        {
        }

        public AdministerCheckAttribute(bool administratorOnly)
        {
            this.administratorOnly = administratorOnly;
        }

        public bool AdministratorOnly
        {
            get
            {
                return this.administratorOnly;
            }
        }
    }
}

