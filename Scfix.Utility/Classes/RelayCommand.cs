using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ScFix.Utility.Classes
{

	public class RelayCommand : ICommand
	{
		#region Members
		readonly Action<object> _action;
		readonly Predicate<object> _actionCanExecute;
		#endregion

		#region Constructors
		public RelayCommand(Action<object> action)
			: this(action, null)
		{
		}

		public RelayCommand(Action<object> action, Predicate<object> actionCanExecute)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			_action = action;
			_actionCanExecute = actionCanExecute;
		}
		#endregion //Constructors

		#region ICommand  Members
		public bool CanExecute(object parameter = null)
		{
			return _actionCanExecute == null ? true : _actionCanExecute(parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter = null)
		{
			_action(parameter);
		}
		#endregion //ICommand


	}
	public class RelayCommand<T> : ICommand
	{
		#region Members
		readonly Action<T> _action;
		readonly Predicate<T> _actionCanExecute;
		#endregion

		#region Constructors
		public RelayCommand(Action<T> action)
			: this(action, null)
		{
		}

		public RelayCommand(Action<T> action, Predicate<T> actionCanExecute)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			_action = action;
			_actionCanExecute = actionCanExecute;
		}
		#endregion //Constructors

		#region ICommand  Members
		public bool CanExecute(object parameter)
		{
			return _actionCanExecute == null ? true : _actionCanExecute((T)parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute(object parameter)
		{
			_action((T)parameter);
		}
		#endregion //ICommand


	}
}
