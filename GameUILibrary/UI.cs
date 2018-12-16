using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using GameUILibrary.Components;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using GameUILibrary.Components.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameUILibrary
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class UI
    {
        [DataMember]
        public List<UIBaseElement> Children;

        private Dictionary<string, UIBaseElement> _allItems;

        public ViewModel Model { get; private set; }

        public int Height { get; set; }
        public int Width { get; set; }

        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        public string Path { get; set; }

        public Texture2D TextureBlank { get; set; }

        public UI(int offsetX = 0, int offsetY = 0) 
        {
            Children = new List<UIBaseElement>();
            _allItems = new Dictionary<string, UIBaseElement>();

            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public T GetItem<T>(string name) where T : UIBaseElement
        {
            if(_allItems.ContainsKey(name))
            {
                return (T)_allItems[name];
            }

            throw new Exception("Item not found");
        }

        public UIBaseElement GetItem(string name)
        {
            if (_allItems.ContainsKey(name))
            {
                return _allItems[name];
            }

            throw new Exception("Item not found");
        }

        public void AddItem(UIBaseElement item, string parentName = null)
        {
            if (!_allItems.ContainsKey(item.Name))
            {
                if(!string.IsNullOrEmpty(parentName))
                {
                    item.Parent = _allItems[parentName];
                    item.Parent.Children.Add(item);
                }
                else
                {
                    Children.Add(item);
                }

                item.Ui = this;

                _allItems.Add(item.Name, item);
            }
            else
            {
                throw new Exception("Item name already exist");
            }
        }

        public void AttachItem(UIBaseElement item)
        {
            if (!_allItems.ContainsKey(item.Name))
            {
                _allItems.Add(item.Name, item);
            }
            else
            {
                throw new Exception("Item name already exist");
            }
        }

        public void SetModel(ViewModel model)
        {
            Model = model;
        }

        public void SetSize(int width, int height)
        {
            Height = height;
            Width = width;
        }

        public void Start()
        {
            foreach (var control in Children)
            {
                AttachItem(control);
                control.Ui = this;
                DiscoverChild(this, control);
            }
        }

        public void Update(double time)
        {
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            var touchState = TouchPanel.GetState();

            foreach (var control in Children)
            {
                control.Update(time, keyboardState, mouseState, touchState);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var control in Children)
            {
                control.Draw(spriteBatch);
            }
        }

        public static UI LoadJSON(string path)
        {
            UI ui = JsonConvert.DeserializeObject<UI>(
                File.ReadAllText(path),
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });

            ui.Path = path;

            return ui;
        }

        public static UI ReloadJSON(UI ui)
        {
            var path = ui.Path;
            var width = ui.Width;
            var height = ui.Height;

            ui = JsonConvert.DeserializeObject<UI>(
                File.ReadAllText(path),
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });

            ui.Path = path;
            ui.Width = width;
            ui.Height = height;

            return ui;
        }



        public static void DiscoverChild(UI ui, UIBaseElement item)
        {
            foreach (var control in item.Children)
            {
                ui.AttachItem(control);
                control.Ui = ui;
                control.Parent = item;
                DiscoverChild(ui, control);
            }
        }

        public static void SaveJSON(string path, UI ui)
        {
            File.WriteAllText(
                path, 
                JsonConvert.SerializeObject(
                    ui.Children, 
                    Formatting.Indented,
                    new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Objects
                        }
                    ));
        }

        internal void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end, Color color)
        {
            if (TextureBlank != null)
            {
                Vector2 edge = end - start;
                // calculate angle to rotate line
                float angle =
                    (float)Math.Atan2(edge.Y, edge.X);

                sb.Draw(
                    TextureBlank,
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
}
