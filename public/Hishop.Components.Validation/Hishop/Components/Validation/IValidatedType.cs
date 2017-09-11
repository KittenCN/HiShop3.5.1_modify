namespace Hishop.Components.Validation
{
    using System.Collections.Generic;
    using System.Reflection;
    internal interface IValidatedType : IValidatedElement
    {
        IEnumerable<MethodInfo> GetSelfValidationMethods();
        IEnumerable<IValidatedElement> GetValidatedFields();
        IEnumerable<IValidatedElement> GetValidatedMethods();
        IEnumerable<IValidatedElement> GetValidatedProperties();
    }
}

