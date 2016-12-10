using MRB.Xaml.MVVM.Input;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MRB.Xaml.MVVM.Models
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        protected ModelBase()
        {
        }



        private PropertyChangedEventHandler PropertyChangedHandler;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { this.PropertyChangedHandler += value; }
            remove { this.PropertyChangedHandler -= value; }
        }

        private EventHandler<CommandExecutedEventArgs> CommandExecutedHandler;
        public event EventHandler<CommandExecutedEventArgs> CommandExecuted
        {
            add { this.CommandExecutedHandler += value; }
            remove { this.CommandExecutedHandler -= value; }
        }


        /// <summary>
        /// Returns the dispatcher that should be used in the view model.
        /// </summary>
        protected Dispatcher CurrentDispatcher
        {
            get { return this.GetDispatcher(); }
        }



        private Dictionary<string, object> Properties = new Dictionary<string, object>();
        protected T GetProperty<T>(string propertyName)
        {
            if (this.Properties.ContainsKey(propertyName) && this.Properties[propertyName] is T)
            {
                return (T)this.Properties[propertyName];
            }

            return default(T);
        }

        protected T GetProperty<T>(string propertyName, Func<T> defaultValue)
        {
            if (this.Properties.ContainsKey(propertyName) && this.Properties[propertyName] is T)
            {
                return (T)this.Properties[propertyName];
            }

            T v = default(T);

            if (this.CurrentDispatcher.TryInvoke(defaultValue, out v))
            {
                this.SetProperty(propertyName, v);
            }
            return v;
        }

        protected T GetProperty<T>(string propertyName, T defaultValue)
        {
            return this.GetProperty<T>(propertyName, delegate () { return defaultValue; });
        }


        private Dictionary<string, int> DelayedProperties = new Dictionary<string, int>();
        private Dictionary<string, Timer> DelayedPropertyTimers = new Dictionary<string, Timer>();

        /// <summary>
        /// Delays the the change event that is fired for the property with the given name.
        /// </summary>
        /// <remarks>
        /// The change event for the given property is fired the set amount of milliseconds after the last
        /// change of the property. This enables the property to change multiple times without firing the
        /// change event. The change event is then fired when the specified amount of milliseconds has
        /// elapsed from the last change.
        /// </remarks>
        /// <param name="propertyName">The name of the property whose changed event to delay.</param>
        /// <param name="delay">The number of milliseconds to delay the change event. Set this property to zero or less to clear the delay.</param>
        protected void DelayPropertyChange(string propertyName, int delay)
        {
            if (delay > 0)
            {
                this.DelayedProperties[propertyName] = delay;
                this.DelayedPropertyTimers[propertyName] = new Timer(this.DelayedPropertyTimerCallback, propertyName, Timeout.Infinite, Timeout.Infinite);
            }
            else
            {
                if (this.DelayedProperties.ContainsKey(propertyName))
                {
                    this.DelayedProperties.Remove(propertyName);
                }
                if (this.DelayedPropertyTimers.ContainsKey(propertyName))
                {
                    this.DelayedPropertyTimers[propertyName].Dispose();
                    this.DelayedPropertyTimers.Remove(propertyName);
                }
            }
        }

        protected virtual void OnCommandExecuted(CommandExecutedEventArgs e)
        {
            if (null != this.CommandExecutedHandler)
            {
                this.CommandExecutedHandler.Invoke(this, e);
            }
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (null != this.PropertyChangedHandler)
            {
                this.PropertyChangedHandler.Invoke(this, e);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            this.OnPropertyChanged(e);
        }

        protected void SetProperty(string propertyName, object propertyValue)
        {
            bool changed = false;
            bool referenceChanged = false;
            if (this.Properties.ContainsKey(propertyName))
            {
                object propVal = this.Properties[propertyName];
                if (!object.Equals(propVal, propertyValue))
                {
                    changed = true;
                }

                referenceChanged = !object.ReferenceEquals(propVal, propertyValue);
            }
            else
            {
                changed = true;
                referenceChanged = true;
            }

            this.Properties[propertyName] = propertyValue;

            if (changed)
            {
                if (!this.DelayedProperties.ContainsKey(propertyName))
                {
                    this.OnPropertyChanged(propertyName);
                }
                else
                {
                    this.DelayedPropertyTimers[propertyName].Change(this.DelayedProperties[propertyName], Timeout.Infinite);
                }
            }

            if (referenceChanged && null != propertyValue && propertyValue is INotifyCollectionChanged)
            {
                var coll = (INotifyCollectionChanged)propertyValue;
                coll.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
                {
                    this.OnPropertyChanged(propertyName);
                };
            }
        }



        private void DelayedPropertyTimerCallback(object state)
        {
            var name = state as string;
            if (!string.IsNullOrEmpty(name))
            {
                this.OnPropertyChanged(name);
            }
        }

        private Dispatcher GetDispatcher()
        {
            if (null != Application.Current && null != Application.Current.Dispatcher)
            {
                return Application.Current.Dispatcher;
            }
            return Dispatcher.CurrentDispatcher;
        }
    }
}
