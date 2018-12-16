using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary
{
    public class UIDescription
    {
        public string Path { get; set; }
        public Dictionary<string, string> Texts { get; set; }
        public Dictionary<string, Action<object, EventArgs>> Callbacks { get; set; }
        public Dictionary<string, SpriteFont> Fonts { get; set; }
        public Dictionary<string, Texture2D> Textures { get; set; }
    }
}
