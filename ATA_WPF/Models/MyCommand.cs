using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ATA_WPF.Models
{
	public class MyCommand : ICommand
	{
		private readonly Action<object> _execute;
		private readonly Func<object, bool>? _canExecute;

		public MyCommand(Action<object> execute, Func<object, bool>? canExecute = null)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public event EventHandler? CanExecuteChanged;

		public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

		public bool CanExecute(object? parameter)
		{
			if (_canExecute != null)
				return _canExecute(parameter ?? new object());

			if (_execute is not null)
				return true;

			return false;
		}

		public void Execute(object? parameter) => _execute?.Invoke(obj: parameter ?? 0);
	}
}

