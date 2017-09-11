namespace Hishop.Components.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    internal interface IValidatedElement
    {
        IEnumerable<IValidatorDescriptor> GetValidatorDescriptors();

        string CompositionMessageTemplate { get; }

        string CompositionTag { get; }

        Hishop.Components.Validation.CompositionType CompositionType { get; }

        bool IgnoreNulls { get; }

        string IgnoreNullsMessageTemplate { get; }

        string IgnoreNullsTag { get; }

        System.Reflection.MemberInfo MemberInfo { get; }

        Type TargetType { get; }
    }
}

