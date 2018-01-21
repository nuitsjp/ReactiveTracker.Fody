using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reflection;
using Reactive.Bindings;
using Xunit;

namespace ReactiveTracker.Tests
{
    public class TrackEventInitializerFixture
    {
        [Fact]
        public void Init()
        {
            var mock = new Mock();
            TrackEventInitializer.Init(mock);

            mock.IntProperty.Value = 2;
            Assert.Single(EventTracker.PropertyEvents);
            Assert.Equal(typeof(Mock), EventTracker.PropertyEvents[0].Type);
            Assert.Equal("IntProperty", EventTracker.PropertyEvents[0].PropertyInfo.Name);
            Assert.Equal(2, EventTracker.PropertyEvents[0].Value);

            mock.ReactiveCommand.Execute();
            Assert.Single(EventTracker.CommandEvents);
            Assert.Equal(typeof(Mock), EventTracker.CommandEvents[0].Type);
            Assert.Equal("ReactiveCommand", EventTracker.CommandEvents[0].PropertyInfo.Name);
            Assert.Null(EventTracker.CommandEvents[0].Value);

            mock.AsyncReactiveCommand.Execute();
            Assert.Equal(2, EventTracker.CommandEvents.Count);
            Assert.Equal(typeof(Mock), EventTracker.CommandEvents[1].Type);
            Assert.Equal("AsyncReactiveCommand", EventTracker.CommandEvents[1].PropertyInfo.Name);
            Assert.Null(EventTracker.CommandEvents[1].Value);

            mock.IgnoreProperty.Value = true;
            Assert.Single(EventTracker.PropertyEvents);
        }

        public class EventTracker : IEventTracker
        {
            public static List<Event> PropertyEvents { get; } = new List<Event>();
            public static List<Event> CommandEvents { get; } = new List<Event>();
            public void TrackProperty(Type type, PropertyInfo propertyInfo, object value)
            {
                PropertyEvents.Add(new Event {Type = type, PropertyInfo = propertyInfo, Value = value});
            }

            public void TrackCommand(Type type, PropertyInfo propertyInfo, object value)
            {
                CommandEvents.Add(new Event { Type = type, PropertyInfo = propertyInfo, Value = value });
            }
        }

        public class Event
        {
            public Type Type { get; set; }
            public PropertyInfo PropertyInfo { get; set; }
            public object Value { get; set; }
        }

        [TrackEvent(typeof(EventTracker))]
        public class Mock
        {
            public bool BoolProperty { get; set; }
            public ReactiveProperty<int> IntProperty { get; } = new ReactiveProperty<int>();

            [IgnoreTrackEvent]
            public ReactiveProperty<bool> IgnoreProperty { get; } = new ReactiveProperty<bool>();

            public ReactiveCommand ReactiveCommand { get; }

            public AsyncReactiveCommand AsyncReactiveCommand { get; }

            public ReactiveCommand NullCommand { get; } = null;

            public Mock()
            {
                ReactiveCommand = IntProperty.Select(x => x != 0).ToReactiveCommand();
                AsyncReactiveCommand = IntProperty.Select(x => x != 0).ToAsyncReactiveCommand();
            }
        }

    }
}