using GameUILibrary.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    public class Button : UIElement<ButtonState>
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
        [DataMember]
        public bool ButtonPush { get; set; }

        public Button()
        {
            Type = Enums.EnumControl.CONTROL;
        }

        public override void ChangeValue()
        {
            if (LastChangeState > 0.2)
            {
                if (Value == ButtonState.Pressed)
                {
                    Value = ButtonState.Released;
                }
                else
                {
                    Value = ButtonState.Pressed;
                }
                LastChangeState = 0;
            }
        }

        public override void Update(double time)
        {
            if (!ButtonDisabled)
            {
                base.Update(time);
            }

            if (Enable)
            {
                if (ButtonPush && LastChangeState > 0.1 && Value == ButtonState.Pressed)
                {
                    Value = ButtonState.Released;
                }
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

                if (HasHover && !ButtonDisabled)
                {
                    if (!string.IsNullOrEmpty(Cursor))
                    {
                        var cursor = TextureManager.Instance.GetTexture(Cursor);

                        if (TextCentered)
                        {
                            spriteBatch.Draw(cursor, new Rectangle((int)(bounds.X + (bounds.Width - stringSize.X) * 0.5f - 40), (int)(bounds.Y + bounds.Height * 0.5 - 5), 36, 18), Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(cursor, new Rectangle((int)(bounds.X + 15 - 40), (int)(bounds.Y + bounds.Height * 0.5 - 5), 36, 18), Color.White);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(BackgroundReleased) && !string.IsNullOrEmpty(BackgroundPressed))
                {
                    var backgroundReleased = TextureManager.Instance.GetTexture(BackgroundReleased);
                    var backgroundPressed = TextureManager.Instance.GetTexture(BackgroundPressed);

                    if (Value == ButtonState.Pressed)
                    {
                        spriteBatch.Draw(backgroundPressed,
                            bounds, 
                            Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(backgroundReleased,
                            new Rectangle(bounds.X, bounds.Y- DecalageEtat, bounds.Width, bounds.Height+ DecalageEtat), 
                            Color.White);
                    }
                }

                int decalage = Value == ButtonState.Released ? DecalageEtat : 0;

                var color = Color;

                if (ButtonDisabled)
                {
                    color = Color.Gray;
                }

                if (TextCentered)
                {
                    spriteBatch.DrawString(font, Text, new Vector2(bounds.X + (bounds.Width - stringSize.X) * 0.5f, bounds.Y + (bounds.Height - stringSize.Y) * 0.5f - decalage), color);
                }
                else
                {
                    spriteBatch.DrawString(font, Text, new Vector2(bounds.X + 15, bounds.Y + (bounds.Height - stringSize.Y) * 0.5f - decalage), color);
                }
            }
        }
    }

}
