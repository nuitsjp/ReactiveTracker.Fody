using System;
using Reactive.Bindings;

namespace SampleConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            var viewModel = new MainPageViewModel();
            viewModel.IntProperty.Value = 2;
            Console.ReadLine();
        }
    }
}
