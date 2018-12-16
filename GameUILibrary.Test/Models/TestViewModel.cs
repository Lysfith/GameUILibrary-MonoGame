using GameUILibrary.Components;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary.Test.Models
{
    public class TestViewModel : ViewModel
    {
        private ButtonState _button1;
        public ButtonState Button1
        {
            get { return _button1; }
            set { NotifyPropertyChanged(ref _button1, value); }
        }

        private ButtonState _button2;
        public ButtonState Button2
        {
            get { return _button2; }
            set { NotifyPropertyChanged(ref _button2, value); }
        }

        private ButtonState _button3;
        public ButtonState Button3
        {
            get { return _button3; }
            set { NotifyPropertyChanged(ref _button3, value); }
        }

        private bool _check1;
        public bool Check1
        {
            get { return _check1; }
            set { NotifyPropertyChanged(ref _check1, value); }
        }

        private string _radioGroup1;
        public string RadioGroup1
        {
            get { return _radioGroup1; }
            set { NotifyPropertyChanged(ref _radioGroup1, value); }
        }

        public override void View_PropertyChanged(object sender, UIPropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Button1":
                    _button1 = (ButtonState)e.Value;
                    if (_button1 == ButtonState.Pressed)
                        Button2 = ButtonState.Released;
                    else
                        Button2 = ButtonState.Pressed;
                    break;
                case "Button2":
                    _button2 = (ButtonState)e.Value;
                    if (_button2 == ButtonState.Pressed)
                        Button1 = ButtonState.Released;
                    else
                        Button1 = ButtonState.Pressed;
                    break;
                case "Button3":
                    _button3 = (ButtonState)e.Value;
                    break;
                case "Check1":
                    _check1 = (bool)e.Value;
                    break;
                case "RadioGroup1":
                    _radioGroup1 = (string)e.Value;
                    break;
            }
        }
    }
}
