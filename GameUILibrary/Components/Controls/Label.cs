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
    public class Label : UIBaseElement
    {
        [DataMember]
        public string Font { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public bool TextCentered { get; set; }
        [DataMember]
        public Color Color { get; set; }
        
        public Label()
        {
            Type = Enums.EnumControl.CONTROL;
        }

        public override void Update(double time)
        {
            if (Enable)
            {

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
                var color = Color;

                if (TextCentered)
                {
                    spriteBatch.DrawString(font, Text, new Vector2(bounds.X + (bounds.Width - stringSize.X) * 0.5f, bounds.Y + (bounds.Height - stringSize.Y) * 0.5f), color);
                }
                else
                {
                    spriteBatch.DrawString(font, Text, new Vector2(bounds.X + 15, bounds.Y + (bounds.Height - stringSize.Y) * 0.5f), color);
                }
            }
        }
    }

}
