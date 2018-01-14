using System;

namespace ReactiveTracker
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreTrackEventAttribute : Attribute
    {
        
    }
}