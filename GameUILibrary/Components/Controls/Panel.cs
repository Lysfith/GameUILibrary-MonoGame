using GameUILibrary.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    public class Panel : UIBaseElement
    {
        [DataMember]
        public Color Color { get; set; }
        [DataMember]
        public string Texture { get; set; }

        public Panel()
        {
            Type = Enums.EnumControl.CONTAINER;
        }

        public override void Update(double time)
        {
            if (Enable)
            {


                base.Update(time);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Enable && Visible)
            {
                var bounds = GetLocalBounds();

                if (Texture != null)
                {
                    var texture = TextureManager.Instance.GetTexture(Texture);


                    //Corners
                    spriteBatch.Draw(texture, new Rectangle((int)bounds.X, (int)bounds.Y, 10, 10), new Rectangle(0, 0, 10, 10), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

                    spriteBatch.Draw(texture, new Rectangle((int)bounds.X, (int)bounds.Y + bounds.Height - 10, 10, 10), new Rectangle(0, texture.Height - 10, 10, 10), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

                    spriteBatch.Draw(texture, new Rectangle((int)bounds.X + bounds.Width - 10, (int)bounds.Y, 10, 10), new Rectangle(texture.Width - 10, 0, 10, 10), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

                    spriteBatch.Draw(texture, new Rectangle((int)bounds.X + bounds.Width - 10, (int)bounds.Y + bounds.Height - 10, 10, 10), new Rectangle(texture.Width - 10, texture.Height - 10, 10, 10), Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

                    //Top
                    spriteBatch.Draw(
                        texture,
                        new Rectangle((int)bounds.X + 10, (int)bounds.Y, bounds.Width - 20, 10),
                        new Rectangle((int)(texture.Width * 0.5), 0, 1, 10),
                        Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

                    ////Bottom
                    spriteBatch.Draw(
                        texture,
                        new Rectangle((int)bounds.X + 10, (int)bounds.Y + bounds.Height - 10, bounds.Width - 20, 10),
                        new Rectangle((int)(texture.Width * 0.5), texture.Height - 10, 1, 10),
                        Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    ////Left
                    spriteBatch.Draw(
                        texture,
                        new Rectangle((int)bounds.X, (int)bounds.Y + 10, 10, bounds.Height - 20),
                        new Rectangle(0, (int)(texture.Height * 0.5), 10, 1),
                        Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                    ////Right
                    spriteBatch.Draw(
                        texture,
                        new Rectangle((int)bounds.X + bounds.Width - 10, (int)bounds.Y + 10, 10, bounds.Height - 20),
                        new Rectangle(texture.Width - 10, (int)(texture.Height * 0.5), 10, 1),
                        Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);

                    //Fill
                    spriteBatch.Draw(
                        texture,
                        new Rectangle((int)bounds.X + 10, (int)bounds.Y + 10, bounds.Width - 20, bounds.Height - 20),
                        new Rectangle((int)(texture.Width * 0.5), (int)(texture.Height * 0.5), 1, 1),
                        Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 0);
                }

                base.Draw(spriteBatch);
            }
        }
    }

}
