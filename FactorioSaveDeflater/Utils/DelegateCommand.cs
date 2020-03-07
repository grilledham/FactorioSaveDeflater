using System;
using System.Windows.Input;

namespace FactorioSaveDeflater.Utils
{
    public class DelegateCommand : ICommand
    {
        private Func<bool> canExecute;
        private Action execute;

        public DelegateCommand(Action execute, Func<bool>? canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            this.canExecute = canExecute ?? (() => true);
            this.execute = execute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return canExecute();
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                execute();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
