using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveTracker
{
    internal static class TypeExtensions
    {
        internal static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
        {
            var evaluation = generic;
            while (evaluation != typeof(object))
            {
                if (evaluation.IsGenericType
                    && evaluation.GetGenericTypeDefinition() == toCheck)
                {
                    return true;
                }

                evaluation = evaluation.BaseType;
            }

            return false;
        }
    }
}
