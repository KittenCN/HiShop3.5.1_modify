namespace Hishop.Components.Validation
{
    using System;

    internal interface IValidatorDescriptor
    {
        Validator CreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder);
    }
}

