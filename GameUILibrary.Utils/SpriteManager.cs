using GameUILibrary.Graphic;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameUILibrary.Utils
{
    public class SpriteManager
    {
        private static SpriteManager _instance;

        public static SpriteManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SpriteManager();
                }

                return _instance;
            }
        }

        private Dictionary<string, Sprite> _sprites;

        public SpriteManager()
        {
            _sprites = new Dictionary<string, Sprite>();
        }

        public bool Exist(string name)
        {
            return _sprites.ContainsKey(name);
        }

        public Sprite GetSprite(string name)
        {
            if (Exist(name))
            {
                return _sprites[name];
            }

            throw new Exception("Le sprite " + name + " n'existe pas");
        }

        public void AddOrReplaceSprite(string name, Sprite sprite)
        {
            if (Exist(name))
            {
                _sprites[name] = sprite;
            }
            else
            {
                _sprites.Add(name, sprite);
            }
        }
    }
}
