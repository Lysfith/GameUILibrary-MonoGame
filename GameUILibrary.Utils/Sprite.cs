using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary.Graphic
{
    public class Sprite
    {
        public Texture2D Texture { get; private set; }
        public Rectangle RectangleSource { get; private set; }
        public Rectangle Rectangle { get; private set; }
        public Color Color { get; private set; }
        public Point Center {
            get
            {
                return Rectangle.Center;
            }
        }

        public Sprite(Texture2D texture, Rectangle rectangle, Color color)
        {
            Texture = texture;
            Rectangle = rectangle;
            Color = color;
            RectangleSource = Texture.Bounds;
        }

        public Sprite(Texture2D texture, Rectangle rectangleSource, Rectangle rectangle, Color color)
        {
            Texture = texture;
            RectangleSource = rectangleSource;
            Rectangle = rectangle;
            Color = color;
        }
    }
}
