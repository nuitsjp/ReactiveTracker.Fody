using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reactive.Bindings;
using Xunit;
using ReactiveTracker;
// ReSharper disable PossibleNullReferenceException

namespace ReactiveTracker.Tests
{
    public class ObjectExtensionsFixture
    {
        public class ReactivePropertyFixtureMock
        {
            public IReactiveProperty<int> InterfaceProperty { get; } = new ReactiveProperty<int>();
            public ReactiveProperty<string> ClassProperty { get; } = new ReactiveProperty<string>();
            public ReactiveProperty<object> NullProperty { get; } = null;
        }

        [Fact]
        public void GetReactivePropertyTypeArgumentsWhenInterface()
        {
            var mock = new ReactivePropertyFixtureMock();

            var interfacePropertyInfo = typeof(ReactivePropertyFixtureMock).GetProperty("InterfaceProperty");
            var interfaceProperty = interfacePropertyInfo.GetValue(mock);
            var interfaceTypeArguments = interfaceProperty.GetReactivePropertyTypeArguments();

            Assert.NotNull(interfaceTypeArguments);
            Assert.Single(interfaceTypeArguments);
            Assert.Equal(typeof(int), interfaceTypeArguments[0]);
        }

        [Fact]
        public void GetReactivePropertyTypeArgumentsWhenClass()
        {
            var mock = new ReactivePropertyFixtureMock();

            var classPropertyInfo = typeof(ReactivePropertyFixtureMock).GetProperty("ClassProperty");
            var classeProperty = classPropertyInfo.GetValue(mock);
            var classTypeArguments = classeProperty.GetReactivePropertyTypeArguments();

            Assert.NotNull(classTypeArguments);
            Assert.Single(classTypeArguments);
            Assert.Equal(typeof(string), classTypeArguments[0]);
        }

        [Fact]
        public void GetReactivePropertyTypeArgumentsWhenNull()
        {
            var mock = new ReactivePropertyFixtureMock();

            var nullPropertyInfo = typeof(ReactivePropertyFixtureMock).GetProperty("NullProperty");
            var nullProperty = nullPropertyInfo.GetValue(mock);
            Assert.Throws<ArgumentNullException>(() => nullProperty.GetReactivePropertyTypeArguments());
        }

        public class ReactiveCommandFixtureMock
        {
            public ReactiveCommand ClassProperty { get; } = new ReactiveCommand();
            public ReactiveCommand NullProperty { get; } = null;
        }

        [Fact]
        public void GetReactiveCommandTypeArgumentsWhenClass()
        {
            var mock = new ReactiveCommandFixtureMock();

            var classPropertyInfo = typeof(ReactiveCommandFixtureMock).GetProperty("ClassProperty");
            var classeProperty = classPropertyInfo.GetValue(mock);
            var classTypeArguments = classeProperty.GetReactiveCommandTypeArguments();

            Assert.NotNull(classTypeArguments);
            Assert.Single(classTypeArguments);
            Assert.Equal(typeof(object), classTypeArguments[0]);
        }

        [Fact]
        public void GetReactiveCommandTypeArgumentsWhenNull()
        {
            var mock = new ReactiveCommandFixtureMock();

            var nullPropertyInfo = typeof(ReactiveCommandFixtureMock).GetProperty("NullProperty");
            var nullProperty = nullPropertyInfo.GetValue(mock);
            Assert.Throws<ArgumentNullException>(() => nullProperty.GetReactiveCommandTypeArguments());
        }

        public class AsyncReactiveCommandFixtureMock
        {
            public AsyncReactiveCommand ClassProperty { get; } = new AsyncReactiveCommand();
            public AsyncReactiveCommand NullProperty { get; } = null;
        }

        [Fact]
        public void GetAsyncReactiveCommandTypeArgumentsWhenClass()
        {
            var mock = new AsyncReactiveCommandFixtureMock();

            var classPropertyInfo = typeof(AsyncReactiveCommandFixtureMock).GetProperty("ClassProperty");
            var classeProperty = classPropertyInfo.GetValue(mock);
            var classTypeArguments = classeProperty.GetAsyncReactiveCommandTypeArguments();

            Assert.NotNull(classTypeArguments);
            Assert.Single(classTypeArguments);
            Assert.Equal(typeof(object), classTypeArguments[0]);
        }

        [Fact]
        public void GetAsyncReactiveCommandTypeArgumentsWhenNull()
        {
            var mock = new AsyncReactiveCommandFixtureMock();

            var nullPropertyInfo = typeof(AsyncReactiveCommandFixtureMock).GetProperty("NullProperty");
            var nullProperty = nullPropertyInfo.GetValue(mock);
            Assert.Throws<ArgumentNullException>(() => nullProperty.GetAsyncReactiveCommandTypeArguments());
        }

    }
}
