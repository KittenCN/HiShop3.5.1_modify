namespace Hishop.Components.Validation
{
    using Hishop.Components.Validation.Validators;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class MetadataValidatedType : MetadataValidatedElement, IValidatedType, IValidatedElement
    {
        public MetadataValidatedType(Type targetType, string ruleset) : base(targetType, ruleset)
        {
        }

        IEnumerable<MethodInfo> IValidatedType.GetSelfValidationMethods()
        {
            Type targetType = this.TargetType;
            if (targetType.GetCustomAttributes(typeof(HasSelfValidationAttribute), false).Length != 0)
            {
                foreach (MethodInfo iteratorVariable1 in targetType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
                {
                    bool iteratorVariable2 = !(iteratorVariable1.ReturnType == typeof(void));
                    ParameterInfo[] parameters = iteratorVariable1.GetParameters();
                    if ((!iteratorVariable2 && (parameters.Length == 1)) && (parameters[0].ParameterType == typeof(ValidationResults)))
                    {
                        foreach (SelfValidationAttribute iteratorVariable4 in iteratorVariable1.GetCustomAttributes(typeof(SelfValidationAttribute), false))
                        {
                            if (this.Ruleset.Equals(iteratorVariable4.Ruleset))
                            {
                                yield return iteratorVariable1;
                            }
                        }
                    }
                }
            }
        }

        IEnumerable<IValidatedElement> IValidatedType.GetValidatedFields()
        {
            MetadataValidatedElement iteratorVariable0 = new MetadataValidatedElement(this.Ruleset);
            foreach (FieldInfo iteratorVariable1 in this.TargetType.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                iteratorVariable0.UpdateFlyweight(iteratorVariable1);
                yield return iteratorVariable0;
            }
        }

        IEnumerable<IValidatedElement> IValidatedType.GetValidatedMethods()
        {
            MetadataValidatedElement iteratorVariable0 = new MetadataValidatedElement(this.Ruleset);
            foreach (MethodInfo iteratorVariable1 in this.TargetType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                iteratorVariable1.GetParameters();
                if (ValidationReflectionHelper.IsValidMethod(iteratorVariable1))
                {
                    iteratorVariable0.UpdateFlyweight(iteratorVariable1);
                    yield return iteratorVariable0;
                }
            }
        }

        IEnumerable<IValidatedElement> IValidatedType.GetValidatedProperties()
        {
            MetadataValidatedElement iteratorVariable0 = new MetadataValidatedElement(this.Ruleset);
            foreach (PropertyInfo iteratorVariable1 in this.TargetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (ValidationReflectionHelper.IsValidProperty(iteratorVariable1))
                {
                    iteratorVariable0.UpdateFlyweight(iteratorVariable1);
                    yield return iteratorVariable0;
                }
            }
        }




    }
}

