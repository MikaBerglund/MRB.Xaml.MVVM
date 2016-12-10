using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRB.Xaml.MVVM.Input
{
    /// <summary>
    /// A command that relays the execution to delegates given in their constructors.
    /// </summary>
    public class RelayCommand : CommandBase
    {
        /// <summary>
        /// Creates a new instance of the command.
        /// </summary>
        /// <param name="execute">The delegate that represents the method to invoke when the command is executed.</param>
        public RelayCommand(Action<object> execute) : this(execute, null) { }

        /// <summary>
        /// Creates a new instance of the command.
        /// </summary>
        /// <param name="execute">The delegate that represents the method to invoke when the command is executed.</param>
        /// <param name="canExecute">A predicate that is used to determine whether the command can be executed.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (null == execute) throw new ArgumentNullException("execute");

            this.ExecuteDelegate = execute;
            this.CanExecuteDelegate = canExecute;
        }


        /// <summary>
        /// Fired before the command is executed. The command can be cancelled in an event handler to this event.
        /// </summary>
        public event EventHandler<CommandExecutingEventArgs> Executing;

        /// <summary>
        /// Fired when the command was executed.
        /// </summary>
        public event EventHandler<CommandExecutedEventArgs> Executed;


        private readonly Action<object> ExecuteDelegate;
        private readonly Predicate<object> CanExecuteDelegate;




        /// <summary>
        /// Determines whether the command can execute by relaying the call to the delegate specified in the constructor.
        /// If no such delegate was defined, the method always returns <c>true</c>.
        /// </summary>
        /// <param name="parameter">The command parameter relayed to the delegate.</param>
        public override bool CanExecute(object parameter)
        {
            if (null != this.CanExecuteDelegate)
            {
                return this.CanExecuteDelegate(parameter);
            }
            return base.CanExecute(parameter);
        }

        /// <summary>
        /// Executes the command by relaying the call to the execute delegate specified in the constructor.
        /// </summary>
        /// <param name="parameter">The command parameter relayed to the execute delegate.</param>
        public override void Execute(object parameter)
        {
            var e = new CommandExecutingEventArgs(false, parameter, this);
            this.OnExecuting(e);

            if (!e.Cancel)
            {
                this.ExecuteDelegate(parameter);
                this.OnExecuted(parameter);
            }
        }


        /// <summary>
        /// Fires the <see cref="Executed"/> event.
        /// </summary>
        /// <param name="parameter"></param>
        protected virtual void OnExecuted(object parameter)
        {
            if (null != this.Executed)
            {
                this.Executed.Invoke(this, new CommandExecutedEventArgs(parameter, this));
            }
        }

        /// <summary>
        /// Fires the <see cref="Executing"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnExecuting(CommandExecutingEventArgs e)
        {
            if (null != this.Executing)
            {
                this.Executing(this, e);
            }
        }

    }
}
