using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameUILibrary;
using GameUILibrary.Components.Enums;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using GameUILibrary.Components.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GameUILibrary.Components
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class UIBaseElement : IDisposable, IUINotifyPropertyChanged
    {
        [DataMember]
        public float X { get; set; }
        [DataMember]
        public float Y { get; set; }
        [DataMember]
        public float Height { get; set; }
        [DataMember]
        public float Width { get; set; }

        public bool HasFocus { get; set; }
        public bool HasHover { get; set; }

        [DataMember]
        public bool Enable { get; set; }
        [DataMember]
        public bool Visible { get; set; }

        public UI Ui { get; set; }
        public UIBaseElement Parent { get; set; }
        [DataMember]
        public List<UIBaseElement> Children { get; set; }

        public double LastChangeState { get; protected set; }

        protected EnumControl Type;

        [DataMember]
        public string Name { get; set; }

        public event EventHandler<EventArgs> OnHoverStart;
        public event EventHandler<EventArgs> OnHoverEnd;
        public event EventHandler<EventArgs> OnGainFocus;
        public event EventHandler<EventArgs> OnLostFocus;

        public UIBaseElement()
        {
            Enable = true;
            Visible = true;
            Children = new List<UIBaseElement>();
        }

        public virtual void Update(double time)
        {
            if (Enable && Visible)
            {

                var mouseState = Mouse.GetState();

                var bounds = GetLocalBounds();

                if (bounds.X <= mouseState.X && mouseState.X < bounds.X + bounds.Width
                    && bounds.Y <= mouseState.Y && mouseState.Y < bounds.Y + bounds.Height)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        HasFocus = true;
                        ChangeValue();
                        if (OnGainFocus != null)
                        {
                            OnGainFocus(this, null);
                        }
                    }
                    HasHover = true;
                    if (OnHoverStart != null)
                    {
                        OnHoverStart(this, null);
                    }
                }
                else
                {
                    if (HasFocus && mouseState.LeftButton == ButtonState.Pressed)
                    {
                        HasFocus = false;

                        if (OnLostFocus != null)
                        {
                            OnLostFocus(this, null);
                        }
                    }

                    if (HasHover && OnHoverEnd != null)
                    {
                        OnHoverEnd(this, null);
                    }

                    HasHover = false;
                }

                foreach (var child in Children)
                {
                    child.Update(time);
                }

                LastChangeState += time;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Enable && Visible)
            {
                foreach (var child in Children)
                {
                    child.Draw(spriteBatch);
                }
            }

        }

        public virtual void ChangeValue()
        {

        }

        public Rectangle GetLocalBounds()
        {
            float x = 0;
            float y = 0;
            float width = 0;
            float height = 0;

            if (Parent != null)
            {
                var parentRect = Parent.GetLocalBounds();

                x = parentRect.Width * X / 100f + parentRect.X;
                y = parentRect.Height * Y / 100f + parentRect.Y;
                width = Type == EnumControl.CONTAINER ? parentRect.Width * Width / 100f : Width;
                height = Type == EnumControl.CONTAINER ? parentRect.Height * Height / 100f : Height;
            }
            else
            {
                x = Ui.Width * X / 100f;
                y = Ui.Height * Y / 100f;
                width = Type == EnumControl.CONTAINER ? Ui.Width * Width / 100f : Width;
                height = Type == EnumControl.CONTAINER ? Ui.Height * Height / 100f : Height;
            }

            return new Rectangle((int)x, (int)y, (int)width, (int)height);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public event UIPropertyChangedEventHandler PropertyChanged;

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

        public virtual void Model_PropertyChanged(object sender, UIPropertyChangedEventArgs e)
        {

        }
    }
}
