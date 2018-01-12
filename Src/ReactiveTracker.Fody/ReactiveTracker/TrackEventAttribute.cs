using System;

namespace ReactiveTracker
{
    public sealed class TrackEventAttribute : Attribute
    {
        public Type EventTrackerType { get; }
        public TrackEventAttribute(Type eventTracker)
        {
            EventTrackerType = eventTracker;
        }
    }
}
