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

            foreach (var propertyInfo in target.GetType().GetRuntimeProperties())
            {
                var typeArguments = propertyInfo.GetReavtivePropertyTypeArguments();
                if (typeArguments != null)
                {
                    var property = propertyInfo.GetValue(target);
                    if (property == null) continue;

                    var subscribe =
                        SubscribeReactivePropertyMethodInfo.MakeGenericMethod(typeArguments);
                    subscribe.Invoke(target, new[] { property, tracker, target.GetType(), propertyInfo });
                    continue;
                }

                typeArguments = propertyInfo.GetReactiveCommandTypeArguments();
                if (typeArguments != null)
                {
                    var property = propertyInfo.GetValue(target);
                    if (property == null) continue;

                    var subscribe =
                        SubscribeReactiveCommandMethodInfo.MakeGenericMethod(typeArguments);
                    subscribe.Invoke(target, new[] { property, tracker, target.GetType(), propertyInfo });
                    continue;
                }

                typeArguments = propertyInfo.GetAsyncReactiveCommandTypeArguments();
                if (typeArguments != null)
                {
                    var property = propertyInfo.GetValue(target);
                    if (property == null) continue;

                    var subscribe =
                        SubscribeAsyncReactiveCommandMethodInfo.MakeGenericMethod(typeArguments);
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
