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

namespace GameUILibrary.Components.Controls
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class Image : UIBaseElement
    {
        [DataMember]
        public string Src { get; set; }

        public Image()
        {
            Type = Enums.EnumControl.CONTROL;
        }

        public override void Update(double time, KeyboardState keyboardState,
            MouseState mouseState, TouchCollection touchState)
        {
            if (Enable)
            {


                base.Update(time, keyboardState,
                    mouseState, touchState);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Enable && Visible)
            {
                var bounds = GetLocalBounds();

                var texture = TextureManager.Instance.GetTexture(Src);

                spriteBatch.Draw(texture, new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height), Color.White);

                base.Draw(spriteBatch);
            }
        }
    }

}
