using System.Reactive.Linq;
using Reactive.Bindings;
using ReactiveTracker;

namespace SampleConsole
{
    [TrackEvent(typeof(EventTracker))]
    public class MainPageViewModel
    {
        public ReactiveProperty<int> IntProperty { get; } = new ReactiveProperty<int>();

        public ReactiveCommand ExecuteCommand { get; }

        public MainPageViewModel()
        {
            ExecuteCommand = IntProperty.Select(x => x != 0).ToReactiveCommand();
        }
    }
}