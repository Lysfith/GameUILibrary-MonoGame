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
using Microsoft.Xna.Framework.Input.Touch;

namespace GameUILibrary.Components
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class UIBaseElement : IDisposable
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

        public UIBaseElement Parent { get; set; }
        [DataMember]
        public List<UIBaseElement> Children { get; set; }

        public double LastChangeState { get; protected set; }

        public EnumControl Type { get; protected set; }

        [DataMember]
        public EnumVerticalAlignment VerticalAlignment;
        [DataMember]
        public EnumHorizontalAlignment HorizontalAlignment;

        [DataMember]
        public string Name { get; set; }

        public UI Ui { get; set; }

        public event EventHandler<EventArgs> OnHoverStart;
        public event EventHandler<EventArgs> OnHoverEnd;
        public event EventHandler<EventArgs> OnGainFocus;
        public event EventHandler<EventArgs> OnLostFocus;
        public event EventHandler<EventArgs> OnValueChange;

        public UIBaseElement()
        {
            VerticalAlignment = EnumVerticalAlignment.TOP;
            HorizontalAlignment = EnumHorizontalAlignment.LEFT;
            Enable = true;
            Visible = true;
            Children = new List<UIBaseElement>();
        }

        public virtual void Update(double time, KeyboardState keyboardState,
            MouseState mouseState, TouchCollection touchState)
        {
            if (Enable && Visible)
            {
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
                    child.Update(time, keyboardState, mouseState, touchState);
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
            if (OnValueChange != null)
            {
                OnValueChange(this, null);
            }
        }

        public Rectangle GetLocalBounds()
        {
            float x = X;
            float y = Y;
            float width = Width;
            float height = Height;

            if (Parent != null)
            {
                var parentRect = Parent.GetLocalBounds();

                x += parentRect.X;
                y += parentRect.Y;

                if (VerticalAlignment == EnumVerticalAlignment.BOTTOM)
                {
                    y = parentRect.Height - Y - Height;
                }
                else if (VerticalAlignment == EnumVerticalAlignment.CENTER)
                {
                    var midHeight = parentRect.Height * 0.5f;
                    y = midHeight - Height * 0.5f;
                }

                if (HorizontalAlignment == EnumHorizontalAlignment.RIGHT)
                {
                    x = parentRect.Width - X - Width;
                }
                else if (HorizontalAlignment == EnumHorizontalAlignment.CENTER)
                {
                    var midWidth = parentRect.Width * 0.5f;
                    x = midWidth - Width * 0.5f;
                }
            }
            else
            {
                if (VerticalAlignment == EnumVerticalAlignment.BOTTOM)
                {
                    y = Ui.Height - Y - Height;
                }
                else if (VerticalAlignment == EnumVerticalAlignment.CENTER)
                {
                    var midHeight = Ui.Height * 0.5f;
                    y = midHeight - Height * 0.5f;
                }

                if (HorizontalAlignment == EnumHorizontalAlignment.RIGHT)
                {
                    x = Ui.Width - X - Width;
                }
                else if (HorizontalAlignment == EnumHorizontalAlignment.CENTER)
                {
                    var midWidth = Ui.Width * 0.5f;
                    x = midWidth - Width * 0.5f;
                }

                x += Ui.OffsetX;
                y += Ui.OffsetY;
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
