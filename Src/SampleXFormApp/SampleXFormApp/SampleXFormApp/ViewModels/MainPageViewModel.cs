using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Reactive.Bindings;
using ReactiveTracker;
using Xamarin.Forms;

namespace SampleXFormApp.ViewModels
{
    [TrackEvent(typeof(EventTracker))]
    public class MainPageViewModel
    {
        public IReadOnlyList<ColorViewModel> Colors { get; } = ColorViewModel.ColorViewModels;
        public ReactiveProperty<ColorViewModel> SelectedColor { get; } = new ReactiveProperty<ColorViewModel>(ColorViewModel.ColorViewModels.First());
        public ReactiveProperty<ColorViewModel> DecideColor { get; } = new ReactiveProperty<ColorViewModel>(ColorViewModel.ColorViewModels.Single(x => x.Color == Color.Black));
        public ReactiveCommand DecideCommand { get; } = new ReactiveCommand();

        public MainPageViewModel()
        {
            DecideCommand.Subscribe(() => DecideColor.Value = SelectedColor.Value);
        }
    }
}
