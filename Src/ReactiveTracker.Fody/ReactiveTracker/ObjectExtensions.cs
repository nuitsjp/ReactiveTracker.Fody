using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Reactive.Bindings;

namespace ReactiveTracker
{
    internal static class ObjectExtensions
    {
        internal static Type[] GetReavtivePropertyTypeArguments(this object property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            var reactivePropertyType =
                property.GetType().GetInterfaces().FirstOrDefault(
                    x => x.IsGenericType
                         && x.GetGenericTypeDefinition() == typeof(IReactiveProperty<>));
            return reactivePropertyType != null ? reactivePropertyType.GetGenericArguments() : null;
        }

        internal static Type[] GetReactiveCommandTypeArguments(this object property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            for (var propertyType = property.GetType(); propertyType != typeof(object); propertyType = propertyType.BaseType)
            {
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(ReactiveCommand<>))
                {
                    return propertyType.GenericTypeArguments;
                }
            }
            return null;
        }

        internal static Type[] GetAsyncReactiveCommandTypeArguments(this object property)
        {
            if (property == null) throw new ArgumentNullException(nameof(property));

            for (var propertyType = property.GetType(); propertyType != null && propertyType != typeof(object); propertyType = propertyType.BaseType)
            {
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(AsyncReactiveCommand<>))
                {
                    return propertyType.GenericTypeArguments;
                }
            }
            return null;
        }
    }
}
