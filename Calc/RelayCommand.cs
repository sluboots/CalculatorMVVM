﻿using System;
using System.Windows.Input;

namespace Calc
{
    public abstract class BaseRelayCommand : ICommand
    {
        public abstract void Execute(object parameter);
        public abstract bool CanExecute(object parameter);
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
    public class RelayCommand : BaseRelayCommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }
        public override void Execute(object parameter)
        {
            _execute.Invoke();
        }
        public override bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }
    }
    public class RelayCommand<T> : BaseRelayCommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }
        public override void Execute(object parameter)
        {
            if (parameter is T arg)
            {
                _execute.Invoke(arg);
            }
        }
        public override bool CanExecute(object parameter)
        {
            if (parameter is T arg)
            {
                return _canExecute?.Invoke(arg) ?? true;
            }
            return false;
        }
    }
}
