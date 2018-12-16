using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary
{
    public class ViewModel : IUINotifyPropertyChanged
    {
        public event UIPropertyChangedEventHandler PropertyChanged;

        public ViewModel()
        {

        }

        public void NotifyPropertyChanged(string nomPropriete, object value)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new UIPropertyChangedEventArgs(nomPropriete, value));
        }

        protected bool NotifyPropertyChanged<T>(ref T variable, T valeur, [CallerMemberName] string nomPropriete = null)
        {
            if (object.Equals(variable, valeur)) return false;

            variable = valeur;
            NotifyPropertyChanged(nomPropriete, valeur);
            return true;
        }

        public virtual void View_PropertyChanged(object sender, UIPropertyChangedEventArgs e)
        {
            
        }
    }
}
