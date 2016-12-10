using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MRB.Xaml.MVVM.Input
{

    /// <summary>
    /// the event argument class for the <see cref="RelayCommand.Executed"/> event.
    /// </summary>
    public class CommandExecutedEventArgs : EventArgs
    {

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public CommandExecutedEventArgs() { }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        public CommandExecutedEventArgs(object parameter)
        {
            this.Parameter = parameter;
        }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <param name="command">A reference to the command that executed.</param>
        public CommandExecutedEventArgs(object parameter, ICommand command) : this(parameter)
        {
            this.Command = command;
        }


        /// <summary>
        /// The command that executed.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// The command parameter.
        /// </summary>
        public object Parameter { get; private set; }

    }
}
