using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary.Utils
{
    public class FontManager : UtilManager
    {
        private static FontManager _instance;

        public static new FontManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FontManager();
                }

                return _instance;
            }
        }

        private Dictionary<string, SpriteFont> _fonts;

        public FontManager()
        {
            _fonts = new Dictionary<string, SpriteFont>();
        }

        public bool Exist(string name)
        {
            return _fonts.ContainsKey(name);
        }

        public SpriteFont GetFont(string name)
        {
            SpriteFont font = null;

            if (Exist(name))
            {
                font = _fonts[name];
            }
            else
            {
                font = _content.Load<SpriteFont>(name);
                _fonts.Add(name, font);
            }

            return font;
        }
    }
}
