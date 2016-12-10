using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MRB.Xaml.MVVM.Input
{
    public class CommandExecutedEventArgs : EventArgs
    {

        public CommandExecutedEventArgs() { }

        public CommandExecutedEventArgs(object parameter)
        {
            this.Parameter = parameter;
        }

        public CommandExecutedEventArgs(object parameter, ICommand command) : this(parameter)
        {
            this.Command = command;
        }


        public ICommand Command { get; private set; }

        public object Parameter { get; private set; }

    }
}
