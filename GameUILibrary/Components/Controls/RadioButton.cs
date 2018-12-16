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
    public class RadioButton : UIElement<bool>
    {
        [DataMember]
        public string Font { get; set; }
        [DataMember]
        public string Cursor { get; set; }
        [DataMember]
        public string BackgroundReleased { get; set; }
        [DataMember]
        public string BackgroundPressed { get; set; }
        [DataMember]
        public int DecalageEtat { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public bool ButtonDisabled { get; set; }
        [DataMember]
        public bool TextCentered { get; set; }
        [DataMember]
        public Color Color { get; set; }

        public RadioButton()
        {
            Type = Enums.EnumControl.CONTROL;
        }

        public override void ChangeValue()
        {
            Value = true;
            LastChangeState = 0;

            base.ChangeValue();
        }

        public override void Update(double time, KeyboardState keyboardState,
            MouseState mouseState, TouchCollection touchState)
        {
            if (!ButtonDisabled)
            {
                base.Update(time, keyboardState,
                    mouseState, touchState);
            }

            if (Enable)
            {

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Enable && Visible)
            {
                var bounds = GetLocalBounds();

                var font = FontManager.Instance.GetFont(Font);
                var stringSize = font.MeasureString(Text);

                if (!string.IsNullOrEmpty(BackgroundReleased) && !string.IsNullOrEmpty(BackgroundPressed))
                {
                    var backgroundReleased = TextureManager.Instance.GetTexture(BackgroundReleased);
                    var backgroundPressed = TextureManager.Instance.GetTexture(BackgroundPressed);

                    if (Value)
                    {
                        spriteBatch.Draw(backgroundPressed,
                            bounds, 
                            Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(backgroundReleased,
                            bounds,
                            Color.White);
                    }
                }

                var color = Color;

                if (ButtonDisabled)
                {
                    color = Color.Gray;
                }

                spriteBatch.DrawString(font, Text, new Vector2(bounds.X + 40, bounds.Y + (bounds.Height - stringSize.Y) * 0.5f), color);
            }
        }
    }

}
