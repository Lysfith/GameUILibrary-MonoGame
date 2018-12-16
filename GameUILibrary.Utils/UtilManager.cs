using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameUILibrary.Utils
{
    public class UtilManager
    {
        protected ContentManager _content;

        public UtilManager()
        {
            
        }

        public void SetContentManager(ContentManager content)
        {
            _content = content;
        }
    }
}
