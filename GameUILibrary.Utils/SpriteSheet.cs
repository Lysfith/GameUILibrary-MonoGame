using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary.Utils
{
    public class SpriteSheet
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public Texture2D Texture { get; private set; }
        public int SpriteSize { get; private set; }
        public int NbSpriteX { get; private set; }
        public int NbSpriteY { get; private set; }

        public SpriteSheet(int id, string name, Texture2D texture, int spriteSize)
        {
            Id = id;
            Name = name;
            Texture = texture;
            SpriteSize = spriteSize;

            NbSpriteX = Texture.Width / SpriteSize;
            NbSpriteY = Texture.Height / SpriteSize;
        }
    }
}
