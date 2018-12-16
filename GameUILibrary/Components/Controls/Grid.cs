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
    public class Grid : UIBaseElement
    {
        [DataMember]
        public Color BorderColor { get; set; }
        [DataMember]
        public int NbColumns { get; set; }
        [DataMember]
        public int NbRows { get; set; }

        public Grid()
        {
            Type = Enums.EnumControl.CONTAINER;
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

                if(NbColumns > 0 && NbRows > 0)
                {
                    var columnWidth = Width / NbColumns;
                    var rowHeight = Height / NbRows;

                    //-------------------- Borders ----------------------------
                    //Top
                    DrawLine(spriteBatch, new Vector2(bounds.X, bounds.Y), new Vector2(bounds.X + Width, bounds.Y), BorderColor);
                    //Right
                    DrawLine(spriteBatch, new Vector2(bounds.X + Width, bounds.Y), new Vector2(bounds.X + Width, bounds.Y + Height), BorderColor);
                    //Left
                    DrawLine(spriteBatch, new Vector2(bounds.X + Width, bounds.Y + Height), new Vector2(bounds.X, bounds.Y + Height), BorderColor);
                    //Bottom
                    DrawLine(spriteBatch, new Vector2(bounds.X, bounds.Y + Height), new Vector2(bounds.X, bounds.Y), BorderColor);

                    if (NbRows > 1)
                    {
                        for (int row = 0; row < NbRows-1; row++)
                        {
                            DrawLine(spriteBatch, 
                                new Vector2(bounds.X, bounds.Y + rowHeight * (1+row)), 
                                new Vector2(bounds.X + Width, bounds.Y + rowHeight * (1 + row)), BorderColor);
                        }
                    }

                    if (NbColumns > 1)
                    {
                        for (int col = 0; col < NbColumns - 1; col++)
                        {
                            DrawLine(spriteBatch, 
                                new Vector2(bounds.X + columnWidth * (1 + col), bounds.Y), 
                                new Vector2(bounds.X + columnWidth * (1 + col), bounds.Y + Height), BorderColor);
                        }
                    }

                    //-------------------- /Borders ----------------------------
                    int rowCounter = 0;
                    int colCounter = 0;
                    for (int i=0;i<Children.Count;i++)
                    {
                        if(i != 0 && i % NbColumns == 0)
                        {
                            rowCounter++;
                            colCounter = 0;
                        }
                        var child = Children.ElementAt(i);
                        child.X = columnWidth * colCounter;
                        child.Y = rowHeight * rowCounter;
                        child.Width = columnWidth;
                        child.Height = rowHeight;
                        child.Draw(spriteBatch);

                        colCounter++;
                    }
                }

                //base.Draw(spriteBatch);
            }
        }

        void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color)
        {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);

            sb.Draw(TextureManager.Instance.GetTexture("Blank"),
                new Rectangle(// rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    1), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle,     //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);

        }
    }

}
