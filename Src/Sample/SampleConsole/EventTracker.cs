using System;
using System.Reflection;
using ReactiveTracker;

namespace SampleConsole
{
    public class EventTracker : IEventTracker
    {
        public void TrackProperty(Type type, PropertyInfo propertyInfo, object value)
        {
            Console.WriteLine($"type:{type.Name} property:{propertyInfo.Name} value:{value}");
        }
        public void TrackCommand(Type type, PropertyInfo propertyInfo, object value)
        {
            Console.WriteLine($"type:{type.Name} property:{propertyInfo.Name} value:{value}");
        }
    }
}