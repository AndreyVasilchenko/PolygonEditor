using System;
using System.Windows.Input;


namespace PolygonEditor
{
    public class UserCommand : ICommand
    {
        private Action<object> execute;
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        private Func<object, bool> canExecute;
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public UserCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
    }
}
