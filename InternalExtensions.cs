using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MRB.Xaml.MVVM
{
    internal static class InternalExtensions
    {
        public static bool TryInvoke<TResult>(this Dispatcher dispatcher, Func<TResult> method, out TResult value)
        {
            value = default(TResult);
            try
            {
                if (null != dispatcher)
                {
                    value = (TResult)dispatcher.Invoke(method);
                }
                return true;
            }
            catch { }
            return false;
        }

    }
}
