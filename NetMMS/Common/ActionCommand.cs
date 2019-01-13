using System;
using System.Windows.Input;

namespace NetMMS.Common {
    public class ActionCommand<T> : ICommand {
        readonly Action<T> _execute;
        readonly Func<T, bool> _canExecute;
        
        public ActionCommand(Action<T> execute) : this(execute, x => true) {
            _execute = execute;
        }
        
        public ActionCommand(Action<T> execute, Func<T, bool> canExecute) {
            _execute = execute;
            _canExecute = canExecute;
        }
       
        public bool CanExecute(object parameter) {
            return _canExecute((T)parameter);
        }
       
        public void Execute(object parameter) {
            if (CanExecute(parameter)) {
                _execute((T)parameter);
            }
        }

        public void RaiseCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

    }
}
