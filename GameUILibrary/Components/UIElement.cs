using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary.Components
{
    public class UIElement<T> : UIBaseElement
    {
        private T _value;
        public T Value
        {
            get { return _value; }
            set { NotifyPropertyChanged(ref _value, value, Name); }
        }

        public override void Model_PropertyChanged(object sender, UIPropertyChangedEventArgs e)
        {
            if (e.PropertyName == Name)
            {
                Value = (T)e.Value;
            }
        }
    }
}
