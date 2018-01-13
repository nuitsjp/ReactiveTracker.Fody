using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SampleXFormApp.ViewModels
{
    public class ColorViewModel
    {
        public string Name { get; }

        public Color Color { get; }

        public ColorViewModel(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        public override string ToString()
        {
            return Name;
        }

        public static IReadOnlyList<ColorViewModel> ColorViewModels { get; } =
            new List<ColorViewModel>
            {
                new ColorViewModel("White", Color.White),
                new ColorViewModel("Silver", Color.Silver),
                new ColorViewModel("Gray", Color.Gray),
                new ColorViewModel("Black", Color.Black),
                new ColorViewModel("Red", Color.Red),
                new ColorViewModel("Maroon", Color.Maroon),
                new ColorViewModel("Yellow", Color.Yellow),
                new ColorViewModel("Olive", Color.Olive),
                new ColorViewModel("Lime", Color.Lime),
                new ColorViewModel("Green", Color.Green),
                new ColorViewModel("Aqua", Color.Aqua),
                new ColorViewModel("Teal", Color.Teal),
                new ColorViewModel("Blue", Color.Blue),
                new ColorViewModel("Navy", Color.Navy),
                new ColorViewModel("Fuchsia", Color.Fuchsia),
                new ColorViewModel("Purple", Color.Purple)
            };
    }
}
