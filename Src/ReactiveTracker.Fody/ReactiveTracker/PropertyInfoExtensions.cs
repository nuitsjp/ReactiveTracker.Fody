using System;
using System.Linq;
using System.Reflection;
using Reactive.Bindings;

namespace ReactiveTracker
{
    internal static class PropertyInfoExtensions
    {
        internal static Type[] GetReavtivePropertyTypeArguments(this PropertyInfo propertyInfo)
        {
            var reactivePropertyType =
                propertyInfo.PropertyType.GetInterfaces().FirstOrDefault(
                    x => x.IsGenericType
                         && x.GetGenericTypeDefinition() == typeof(IReactiveProperty<>));
            return reactivePropertyType != null ? reactivePropertyType.GetGenericArguments() : null;
        }

        internal static Type[] GetReactiveCommandTypeArguments(this PropertyInfo propertyInfo)
        {
            for (var propertyType = propertyInfo.PropertyType; propertyType != typeof(object); propertyType = propertyType.BaseType)
            {
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(ReactiveCommand<>))
                {
                    return propertyType.GenericTypeArguments;
                }
            }
            return null;
        }

        internal static Type[] GetAsyncReactiveCommandTypeArguments(this PropertyInfo propertyInfo)
        {
            for (var propertyType = propertyInfo.PropertyType; propertyType != typeof(object); propertyType = propertyType.BaseType)
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