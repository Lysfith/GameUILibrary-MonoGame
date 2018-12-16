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
    public class ProgressBar : UIElement<double>
    {
        [DataMember]
        public string Font { get; set; }
        [DataMember]
        public Color BackgroundColor { get; set; }
        [DataMember]
        public string Texture { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public Color Color { get; set; }

        public ProgressBar()
        {
            Type = Enums.EnumControl.CONTROL;
        }

        //public override void ChangeValue()
        //{
        //    if (LastChangeState > 0.2)
        //    {
        //        if (Value == ButtonState.Pressed)
        //        {
        //            Value = ButtonState.Released;
        //        }
        //        else
        //        {
        //            Value = ButtonState.Pressed;
        //        }
        //        LastChangeState = 0;

        //        base.ChangeValue();
        //    }
        //}

        public override void Update(double time, KeyboardState keyboardState,
            MouseState mouseState, TouchCollection touchState)
        {

            base.Update(time, keyboardState,
                mouseState, touchState);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Enable && Visible)
            {
                base.Draw(spriteBatch);

                var percentSize = (int)(Width * (Value / 100));

                var bounds = GetLocalBounds();

                Texture2D textureBlank = TextureManager.Instance.GetTexture("Blank");

                spriteBatch.Draw(textureBlank,
                        new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height),
                        BackgroundColor);


                Texture2D texture = null;

                if (!string.IsNullOrEmpty(Texture))
                {
                    texture = TextureManager.Instance.GetTexture(Texture);
                }
                else
                {
                    texture = TextureManager.Instance.GetTexture("Blank");
                }

                spriteBatch.Draw(texture,
                        new Rectangle(bounds.X, bounds.Y, percentSize, bounds.Height),
                        Color);

                var color = Color;
                var font = FontManager.Instance.GetFont(Font);
                var stringSize = font.MeasureString(Text);

                spriteBatch.DrawString(font, Text, new Vector2(bounds.X + (bounds.Width - stringSize.X) * 0.5f, bounds.Y + (bounds.Height - stringSize.Y) * 0.5f), Color.White);
            }
        }
    }

}
