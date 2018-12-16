using GameUILibrary.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace GameUILibrary.Components.Controls
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class Textbox : UIElement<string>
    {
        [DataMember]
        public string Font { get; set; }
        [DataMember]
        public string Cursor { get; set; }
        [DataMember]
        public string Background { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public bool TextCentered { get; set; }
        [DataMember]
        public Color Color { get; set; }

        private double _lastShowCursor;
        private bool _showCursor;

        private KeyboardState _keyboardState;

        public Textbox()
        {
            Type = Enums.EnumControl.CONTROL;
        }

        public override void ChangeValue()
        {
            if (LastChangeState > 0.1)
            {
                if (_keyboardState.IsKeyDown(Keys.Back) && !string.IsNullOrEmpty(Text))
                {
                    Text = Text.Substring(0, Text.Length - 1);

                    base.ChangeValue();

                    LastChangeState = 0;
                }

                var keys = _keyboardState.GetPressedKeys();
                var keysFiltered = keys.Where(x => "abcdefghijklmnopqrstuvwxyz".Contains(x.ToString().ToLower()));
                if (keysFiltered.Any())
                {
                    foreach (var k in keysFiltered)
                    {
                        Text += k.ToString();
                    }

                    base.ChangeValue();

                    LastChangeState = 0;
                }

                if (_keyboardState.IsKeyDown(Keys.NumPad0))
                {
                    Text += "0";
                    base.ChangeValue();
                    LastChangeState = 0;
                }
                if (_keyboardState.IsKeyDown(Keys.NumPad1))
                {
                    Text += "1";
                    base.ChangeValue();
                    LastChangeState = 0;
                }
                if (_keyboardState.IsKeyDown(Keys.NumPad2))
                {
                    Text += "2";
                    base.ChangeValue();
                    LastChangeState = 0;
                }
                if (_keyboardState.IsKeyDown(Keys.NumPad3))
                {
                    Text += "3";
                    base.ChangeValue();
                    LastChangeState = 0;
                }
                if (_keyboardState.IsKeyDown(Keys.NumPad4))
                {
                    Text += "4";
                    base.ChangeValue();
                    LastChangeState = 0;
                }
                if (_keyboardState.IsKeyDown(Keys.NumPad5))
                {
                    Text += "5";
                    base.ChangeValue();
                    LastChangeState = 0;
                }
                if (_keyboardState.IsKeyDown(Keys.NumPad6))
                {
                    Text += "6";
                    base.ChangeValue();
                    LastChangeState = 0;
                }
                if (_keyboardState.IsKeyDown(Keys.NumPad7))
                {
                    Text += "7";
                    base.ChangeValue();
                    LastChangeState = 0;
                }
                if (_keyboardState.IsKeyDown(Keys.NumPad8))
                {
                    Text += "8";
                    base.ChangeValue();
                    LastChangeState = 0;
                }
                if (_keyboardState.IsKeyDown(Keys.NumPad9))
                {
                    Text += "9";
                    base.ChangeValue();
                    LastChangeState = 0;
                }

                if (_keyboardState.IsKeyDown(Keys.Space))
                {
                    Text += " ";

                    base.ChangeValue();

                    LastChangeState = 0;
                }
            }
        }

        public override void Update(double time, KeyboardState keyboardState,
            MouseState mouseState, TouchCollection touchState)
        {
            _lastShowCursor += time;

            if(_lastShowCursor > 0.5)
            {
                _showCursor = !_showCursor;
                _lastShowCursor = 0;
            }

            base.Update(time, keyboardState,
                mouseState, touchState);

            if (Enable && HasFocus)
            {
                _keyboardState = keyboardState;
                ChangeValue();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Enable && Visible)
            {
                base.Draw(spriteBatch);

                var bounds = GetLocalBounds();

                var font = FontManager.Instance.GetFont(Font);
                var stringSize = font.MeasureString(Text);

                if(string.IsNullOrEmpty(Text))
                {
                    stringSize = font.MeasureString("B");
                }

                if (!string.IsNullOrEmpty(Background))
                {
                    var background = TextureManager.Instance.GetTexture(Background);

                    spriteBatch.Draw(background,
                        bounds, 
                        Color.White);

                }

                var color = Color;
                string text = Text;

                if (HasFocus)
                {
                    if (_showCursor)
                    {
                        text += "_";
                    }
                }

                if (TextCentered)
                {
                    spriteBatch.DrawString(font, text, new Vector2(bounds.X + (bounds.Width - stringSize.X) * 0.5f, bounds.Y + (bounds.Height - stringSize.Y) * 0.5f), color);
                }
                else
                {
                    spriteBatch.DrawString(font, text, new Vector2(bounds.X, bounds.Y + (bounds.Height - stringSize.Y) * 0.5f), color);
                }
            }
        }
    }

}
