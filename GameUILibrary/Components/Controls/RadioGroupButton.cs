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
    public class RadioGroupButton : UIElement<string>
    {
        protected int _buttonSelected = 0;

        public RadioGroupButton()
        {
            Type = Enums.EnumControl.CONTAINER;
        }

        public override void ChangeValue()
        {
            var lastChildChanged = Children
                .Select(x => (RadioButton)x)
                .Where(x => x.Value).OrderBy(x => x.LastChangeState).FirstOrDefault();

            if (lastChildChanged != null)
            {
                _buttonSelected = Children.IndexOf(lastChildChanged);

                for (var i = 0; i < Children.Count; i++)
                {
                    var child = (RadioButton)Children[i];
                    if (i != _buttonSelected && child.Value)
                    {
                        child.Value = false;
                    }
                }

                Value = lastChildChanged.Name;

                base.ChangeValue();
            }
        }

        public override void Update(double time, KeyboardState keyboardState,
            MouseState mouseState, TouchCollection touchState)
        {
            base.Update(time, keyboardState,
                mouseState, touchState);

            if (Enable)
            {
                
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Enable && Visible)
            {
                base.Draw(spriteBatch);
            }
        }
    }

}
