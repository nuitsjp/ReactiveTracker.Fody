using System;
using System.Reflection;
using ReactiveTracker;

namespace SampleConsole
{
    public class EventTracker : IEventTracker
    {
        public void TrackEvent(Type type, PropertyInfo propertyInfo, object value)
        {
            Console.WriteLine($"type:{type.Name} property:{propertyInfo.Name} value:{value}");
        }
    }
}