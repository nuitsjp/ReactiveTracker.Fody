using System;
using System.Reflection;

namespace ReactiveTracker
{
    public interface IEventTracker
    {
        void TrackEvent(Type type, PropertyInfo propertyInfo, object value);
    }
}