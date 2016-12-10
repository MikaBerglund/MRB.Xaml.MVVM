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
    public class CommandExecutingEventArgs : CancelEventArgs
    {

        public CommandExecutingEventArgs() { }

        public CommandExecutingEventArgs([Optional]bool cancel, [Optional]object parameter, [Optional]ICommand command)
            : base(cancel)
        {
            this.Parameter = parameter;
            this.Command = command;
        }

        public ICommand Command { get; private set; }

        public object Parameter { get; private set; }
    }
}
