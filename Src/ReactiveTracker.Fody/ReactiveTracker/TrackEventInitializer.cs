using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using Reactive.Bindings;

namespace ReactiveTracker
{
    public static class TrackEventInitializer
    {
        private static readonly MethodInfo SkipMethodInfo =
            typeof(Observable).GetTypeInfo().GetDeclaredMethods("Skip")
                .Single(
                    x => x.GetParameters().Length == 2
                        && x.GetParameters()[1].ParameterType == typeof(int));

        private static readonly MethodInfo SubscribeMethodInfo =
            typeof(ObservableExtensions).GetTypeInfo().GetDeclaredMethods("Subscribe")
                .Single(
                    x => x.GetParameters().Length == 2 
                        && x.GetParameters()[1].ParameterType != typeof(CancellationToken));

        public static void Init(object target)
        {
            var trackEventAttribute = target.GetType().GetTypeInfo().GetCustomAttribute<TrackEventAttribute>();
            var tracker = (IEventTracker)Activator.CreateInstance(trackEventAttribute.EventTrackerType);

            foreach (var propertyInfo in 
                target.GetType().GetRuntimeProperties()
                    .Where(x => x.PropertyType.GetTypeInfo().ImplementedInterfaces.Any(y =>
                        y.GetTypeInfo().IsGenericType &&
                        y.GetGenericTypeDefinition() == typeof(IReactiveProperty<>))))
            {
                var property = propertyInfo.GetValue(target);
                if (property == null) continue;

                var typeArguments = propertyInfo.PropertyType.GetTypeInfo().GenericTypeArguments;

                var skipGeneric = SkipMethodInfo.MakeGenericMethod(typeArguments);
                var observable = skipGeneric.Invoke(null, new[] { property, 1 });

                var performerGeneric = typeof(Performer<>).MakeGenericType(typeArguments);
                var performer = Activator.CreateInstance(performerGeneric, tracker, target.GetType(), propertyInfo);
                var trackEvent = performerGeneric.GetTypeInfo().GetDeclaredMethod("TrackEvent");
                var actionGeneric = typeof(Action<>).MakeGenericType(typeArguments);
                var trackEventDelegate = Delegate.CreateDelegate(actionGeneric, performer, trackEvent);


                var subscribeGeneric = SubscribeMethodInfo.MakeGenericMethod(typeArguments);
                subscribeGeneric.Invoke(null, new[] { observable, trackEventDelegate });
            }
        }

        private class Performer<T>
        {
            private readonly IEventTracker _eventTracker;
            private readonly Type _type;
            private readonly PropertyInfo _propertyInfo;

            public Performer(IEventTracker eventTracker, Type type, PropertyInfo propertyInfo)
            {
                _eventTracker = eventTracker;
                _type = type;
                _propertyInfo = propertyInfo;
            }

            public void TrackEvent(T value)
            {
                _eventTracker.TrackEvent(_type, _propertyInfo, value);
            }
        }
    }

}
