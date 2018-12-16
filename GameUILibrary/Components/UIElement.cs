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
        public T Value { get; set; }
    }
}
