namespace HiTemplate.Model
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class TemplateBase<T> : TemplateBase
    {
        protected TemplateBase()
        {
        }

        public T Model { get; set; }
    }
}

