﻿using System;
using Reactive.Bindings;

namespace SampleConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            var viewModel = new MainPageViewModel();
            viewModel.IntProperty.Value = 2;
            viewModel.ExecuteCommand.Execute();
            viewModel.AsyncReactiveCommand.Execute();

            var property = new ReactiveProperty<string>();

            Console.ReadLine();
        }
    }
}
