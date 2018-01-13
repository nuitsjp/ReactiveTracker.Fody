using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Reactive.Bindings;

namespace ReactiveTracker
{
    public static class TrackEventInitializer
    {
        public static void Init(object target)
        {
            var trackEventAttribute = target.GetType().GetTypeInfo().GetCustomAttribute<TrackEventAttribute>();
            var tracker = (IEventTracker)Activator.CreateInstance(trackEventAttribute.EventTrackerType);

            foreach (var propertyInfo in target.GetType().GetRuntimeProperties())
            {
                var property = propertyInfo.GetValue(target);
                if (property == null) continue;

                var typeArguments = property.GetReavtivePropertyTypeArguments();
                if (typeArguments != null)
                {
                    var subscribe = SubscribeReactivePropertyMethodInfo.MakeGenericMethod(typeArguments);
                    subscribe.Invoke(target, new[] { property, tracker, target.GetType(), propertyInfo });
                    continue;
                }

                typeArguments = property.GetReactiveCommandTypeArguments();
                if (typeArguments != null)
                {
                    var subscribe = SubscribeReactiveCommandMethodInfo.MakeGenericMethod(typeArguments);
                    subscribe.Invoke(target, new[] { property, tracker, target.GetType(), propertyInfo });
                    continue;
                }

                typeArguments = property.GetAsyncReactiveCommandTypeArguments();
                if (typeArguments != null)
                {
                    var subscribe = SubscribeAsyncReactiveCommandMethodInfo.MakeGenericMethod(typeArguments);
                    subscribe.Invoke(target, new[] { property, tracker, target.GetType(), propertyInfo });
                }
            }
        }

        private static readonly MethodInfo SubscribeReactivePropertyMethodInfo =
            typeof(TrackEventInitializer).GetRuntimeMethods().Single(x => x.Name == "SubscribeReactiveProperty" && x.IsGenericMethod);

        private static void SubscribeReactiveProperty<T>(object property, IEventTracker eventTracker, Type type, PropertyInfo propertyInfo)
        {
            var reactiveProperty = (ReactiveProperty<T>) property;
            reactiveProperty.Skip(1).Subscribe(x => { eventTracker.TrackProperty(type, propertyInfo, x); });
        }

        private static readonly MethodInfo SubscribeReactiveCommandMethodInfo =
            typeof(TrackEventInitializer).GetRuntimeMethods().Single(x => x.Name == "SubscribeReactiveCommand");

        private static void SubscribeReactiveCommand<T>(object property, IEventTracker eventTracker, Type type, PropertyInfo propertyInfo)
        {
            var reactiveProperty = (ReactiveCommand<T>)property;
            reactiveProperty.Subscribe(x => { eventTracker.TrackCommand(type, propertyInfo, x); });
        }

        private static readonly MethodInfo SubscribeAsyncReactiveCommandMethodInfo =
            typeof(TrackEventInitializer).GetRuntimeMethods().Single(x => x.Name == "SubscribeAsyncReactiveCommand");

        private static void SubscribeAsyncReactiveCommand<T>(object property, IEventTracker eventTracker, Type type, PropertyInfo propertyInfo)
        {
            var reactiveProperty = (AsyncReactiveCommand<T>)property;
            reactiveProperty.Subscribe(x =>
            {
                eventTracker.TrackCommand(type, propertyInfo, x); 
                return Task.CompletedTask;
            });
        }
    }

}
