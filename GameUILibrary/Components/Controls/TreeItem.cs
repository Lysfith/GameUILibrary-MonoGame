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
    public class TreeItem
    {
        public event EventHandler<EventArgs> OnSelect;

        public Tree Tree { get; set; }
        public List<TreeItem> Items { get; set; }

        public string Text { get; set; }
        public bool IsOpen { get; set; }

        public float X { get; set; }
        public float Y { get; set; }

        public string Cursor { get; set; }
        public string CursorOpen { get; set; }
        public string Font { get; set; }
        public Color Color { get; set; }

        public bool IsSelected { get; set; }

        public double _lastClickOpen;

        public TreeItem(Tree tree)
        {
            Tree = tree;
            Items = new List<TreeItem>();
        }

        public void Update(double time)
        {
            _lastClickOpen += time;

            if (_lastClickOpen > 0.3)
            {
                var mouseState = Mouse.GetState();

                //Triangle
                var bounds = new Rectangle((int)X, (int)Y + 2, 10, 10);

                if (bounds.X <= mouseState.X && mouseState.X < bounds.X + bounds.Width
                    && bounds.Y <= mouseState.Y && mouseState.Y < bounds.Y + bounds.Height)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        IsOpen = !IsOpen;
                        _lastClickOpen = 0;
                    }
                }

                //Text
                var font = FontManager.Instance.GetFont(Font);
                var stringSize = font.MeasureString(Text);
                var boundsText = new Rectangle((int)X+13, (int)Y, (int)stringSize.X, (int)stringSize.Y);

                if (boundsText.X <= mouseState.X && mouseState.X < boundsText.X + boundsText.Width
                    && boundsText.Y <= mouseState.Y && mouseState.Y < bounds.Y + boundsText.Height)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        IsSelected = true;
                        if(OnSelect != null)
                        {
                            OnSelect(this, null);
                        }
                    }
                }
            }

            int cpt = 0;
            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items.ElementAt(i);

                item.X = this.X + 5;
                item.Y = this.Y + 15 + i * 15 + cpt * 15;

                item.Update(time);

                if (IsOpen)
                {
                    cpt += item.IsDraw();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //if (Enable && Visible)
            //{
            //    base.Draw(spriteBatch);

            if(HasItems())
            {
                if(IsOpen)
                {
                    var cursor = TextureManager.Instance.GetTexture(CursorOpen);
                    spriteBatch.Draw(cursor, new Rectangle((int)X, (int)Y+2, 10, 10), Color.White);
                }
                else
                {
                    var cursor = TextureManager.Instance.GetTexture(Cursor);
                    spriteBatch.Draw(cursor, new Rectangle((int)X, (int)Y+2, 10, 10), Color.White);
                }
            }

            var font = FontManager.Instance.GetFont(Font);
            var stringSize = font.MeasureString(Text);
            var color = Color;

            if(IsSelected)
            {
                var textureBlank = TextureManager.Instance.GetTexture("Blank");
                spriteBatch.Draw(textureBlank, new Rectangle((int)X+13, (int)Y, (int)stringSize.X, (int)stringSize.Y), Color.Blue);

                spriteBatch.DrawString(font, Text, new Vector2(X + 13, Y), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, Text, new Vector2(X + 13, Y), color);
            }

            if (IsOpen)
            {
                for (var i = 0; i < Items.Count; i++)
                {
                    var item = Items.ElementAt(i);
                    item.Draw(spriteBatch);
                }
            }
        }

        public int IsDraw()
        {
            int cpt = 0;
            if (HasItems() && IsOpen)
            {
                foreach (var item in Items)
                {
                    cpt += item.IsDraw();
                }

                cpt += Items.Count;
            }

            return cpt;
        }

        public bool HasItems()
        {
            return Items.Any();
        }
    }

}
