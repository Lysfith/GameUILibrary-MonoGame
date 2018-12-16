using GameUILibrary.Components;
using GameUILibrary.Components.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary
{
    public class ViewModel
    {
        public UI Ui { get; private set; }

        public ViewModel(UI ui)
        {
            Ui = ui;
        }

        public bool SetCallback(string name, EnumCallback type, Action<object, EventArgs> callback)
        {
            var control = Ui.GetItem(name);

            if(control != null)
            {
                switch(type)
                {
                    case EnumCallback.ON_GAIN_FOCUS:
                        control.OnGainFocus += new EventHandler<EventArgs>(callback);
                        break;
                    case EnumCallback.ON_LOST_FOCUS:
                        control.OnLostFocus += new EventHandler<EventArgs>(callback);
                        break;
                    case EnumCallback.ON_HOVER_START:
                        control.OnHoverStart += new EventHandler<EventArgs>(callback);
                        break;
                    case EnumCallback.ON_HOVER_END:
                        control.OnHoverEnd += new EventHandler<EventArgs>(callback);
                        break;
                    case EnumCallback.ON_VALUE_CHANGE:
                        control.OnValueChange += new EventHandler<EventArgs>(callback);
                        break;
                }

                return true;
            }

            return false;
        }
    }
}
