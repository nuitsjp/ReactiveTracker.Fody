using System;
using System.Diagnostics;
using System.Reflection;
using ReactiveTracker;

namespace SampleXFormApp.ViewModels
{
    public class EventTracker : IEventTracker
    {
        public void TrackProperty(Type type, PropertyInfo propertyInfo, object value)
        {
            Debug.WriteLine($"TrackProperty type:{type.Name} property:{propertyInfo.Name} value:{value}");
        }

        public void TrackCommand(Type type, PropertyInfo propertyInfo, object value)
        {
            Debug.WriteLine($"TrackCommand type:{type.Name} property:{propertyInfo.Name} value:{value}");
        }
    }
}