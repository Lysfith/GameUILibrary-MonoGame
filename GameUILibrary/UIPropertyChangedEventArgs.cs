using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary
{
    public class UIPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public virtual object Value { get; }

        public UIPropertyChangedEventArgs(string propertyName, object value)
            :base(propertyName)
        {
            Value = value;
        }
    }
}
