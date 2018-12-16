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

namespace GameUILibrary
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class UI
    {
        [DataMember]
        public List<UIBaseElement> Children = new List<UIBaseElement>();

        public int Height { get; set; }
        public int Width { get; set; }

        public string Path { get; set; }

        public ViewModel Model { get; private set; }

        public UI() 
        {

        }

        public void SetSize(int width, int height)
        {
            Height = height;
            Width = width;
        }

        public void SetModel(ViewModel model)
        {
            Model = model;
        }

        public void Start()
        {
            foreach (var control in Children)
            {
                control.Ui = this;
                control.PropertyChanged += Model.View_PropertyChanged;
                Model.PropertyChanged += control.Model_PropertyChanged;
                DiscoverChild(this, control);
            }
        }

        public void Update(double time)
        {
            foreach (var control in Children)
            {
                control.Update(time);
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
            var model = ui.Model;

            ui = JsonConvert.DeserializeObject<UI>(
                File.ReadAllText(path),
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects
                });

            ui.Path = path;
            ui.Width = width;
            ui.Height = height;
            ui.Model = model;

            return ui;
        }



        public static void DiscoverChild(UI ui, UIBaseElement item)
        {
            foreach (var control in item.Children)
            {
                control.Ui = ui;
                control.Parent = item;
                control.PropertyChanged += ui.Model.View_PropertyChanged;
                ui.Model.PropertyChanged += control.Model_PropertyChanged;
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

    }   
}
