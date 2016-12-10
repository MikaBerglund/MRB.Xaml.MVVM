using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MRB.Xaml.MVVM.Input
{
    /// <summary>
    /// The event argument class for the <see cref="RelayCommand.Executing"/> event.
    /// </summary>
    public class CommandExecutingEventArgs : CancelEventArgs
    {

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public CommandExecutingEventArgs() { }

        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        /// <param name="cancel">Specifies whether to cancel the command.</param>
        /// <param name="parameter">The command parameter.</param>
        /// <param name="command">A reference to the command being executed.</param>
        public CommandExecutingEventArgs([Optional]bool cancel, [Optional]object parameter, [Optional]ICommand command)
            : base(cancel)
        {
            this.Parameter = parameter;
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
