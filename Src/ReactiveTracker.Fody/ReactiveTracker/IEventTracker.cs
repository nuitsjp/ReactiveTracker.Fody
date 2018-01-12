using System;
using System.Reflection;

namespace ReactiveTracker
{
    public interface IEventTracker
    {
        void TrackProperty(Type type, PropertyInfo propertyInfo, object value);

        void TrackCommand(Type type, PropertyInfo propertyInfo, object value);
    }
}