namespace Hishop.Components.Validation
{
    using System;
    using System.Reflection;

    public static class ParameterValidatorFactory
    {
        public static Validator CreateValidator(ParameterInfo paramInfo)
        {
            MetadataValidatedParameterElement validatedElement = new MetadataValidatedParameterElement();
            validatedElement.UpdateFlyweight(paramInfo);
            CompositeValidatorBuilder builder = new CompositeValidatorBuilder(validatedElement);
            foreach (IValidatorDescriptor descriptor in validatedElement.GetValidatorDescriptors())
            {
                builder.AddValueValidator(descriptor.CreateValidator(paramInfo.ParameterType, null, null));
            }
            return builder.GetValidator();
        }
    }
}

