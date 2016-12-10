using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;


namespace MRB.Xaml.MVVM.Input
{
    /// <summary>
    /// A base class for command classes.
    /// </summary>
    public abstract class CommandBase : ICommand
    {

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Determines whether the command can execute. The base implementation always returns <c>true</c>.
        /// </summary>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes the command with the given parameter.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        public abstract void Execute(object parameter);



        /// <summary>
        /// Fires the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var d = this.GetDispatcher();
            d.Invoke((ThreadStart)OnCanExecuteChanged, DispatcherPriority.Normal);
        }



        private Dispatcher GetDispatcher()
        {
            if (null != Application.Current)
            {
                return Application.Current.Dispatcher;
            }

            return Dispatcher.CurrentDispatcher;
        }
    }
}
