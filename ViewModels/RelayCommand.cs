using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MockBench.ViewModels
{
    public class RelayCommand : ICommand
    {
        public readonly Action _execute;

        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => true;
        public void Execute(object? parameter) => _execute();
    }
}
