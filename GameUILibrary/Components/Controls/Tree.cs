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
    public class Tree : UIElement<string>
    {
        [DataMember]
        public string Font { get; set; }
        [DataMember]
        public string Text { get; set; }
        [DataMember]
        public Color Color { get; set; }

        public List<TreeItem> Items { get; set; }

        private Dictionary<string, TreeItem> _allItems { get; set; }

        private TreeItem _lastItemSelected;

        public Tree()
        {
            Type = Enums.EnumControl.CONTROL;

            Items = new List<TreeItem>();
            _allItems = new Dictionary<string, TreeItem>();
        }

        public void Clear()
        {
            Items.Clear();
            _allItems.Clear();
        }

        public override void ChangeValue()
        {
            Value = _lastItemSelected != null ? _lastItemSelected.Text : null;
            base.ChangeValue();
        }

        public void AddUi(UI ui)
        {
            var rootTreeItem = new TreeItem(this)
            {
                Font = "Fonts/Arial-10",
                Cursor = "Textures/tree_cursor_right",
                CursorOpen = "Textures/tree_cursor_down",
                Color = Color.Black,
                Text = "UI"
            };
            AddItem(rootTreeItem);

            foreach (var child in ui.Children)
            {
                var treeItem = new TreeItem(this)
                {
                    Font = "Fonts/Arial-10",
                    Cursor = "Textures/tree_cursor_right",
                    CursorOpen = "Textures/tree_cursor_down",
                    Color = Color.Black,
                    Text = child.Name
                };

                DiscoverChild(treeItem, child);
                AddItem(treeItem, rootTreeItem);
            }
        }

        private void DiscoverChild(TreeItem parent, UIBaseElement uiParent)
        {
            foreach (var child in uiParent.Children)
            {
                var treeItem = new TreeItem(this)
                {
                    Font = "Fonts/Arial-10",
                    Cursor = "Textures/tree_cursor_right",
                    CursorOpen = "Textures/tree_cursor_down",
                    Color = Color.Gray,
                    Text = child.Name
                };

                AddItem(treeItem, parent);
                DiscoverChild(treeItem, child);
            }
        }

        public void AddItem(TreeItem item, TreeItem parent = null)
        {
            _allItems.Add(item.Text, item);
            if (parent != null)
            {
                parent.Items.Add(item);
            }
            else
            {
                Items.Add(item);
            }
           
            item.OnSelect += ItemSelected;
        }

        public void AddItem(string item, string parent = null)
        {
            var itemNode = new TreeItem(this)
            {
                Font = "Fonts/Arial-10",
                Cursor = "Textures/tree_cursor_right",
                CursorOpen = "Textures/tree_cursor_down",
                Color = Color.Gray,
                Text = item
            };
            if (parent != null)
            {
                var parentNode = _allItems[parent];
                AddItem(itemNode, parentNode);
            }
            else
            {
                AddItem(itemNode);
            }
        }

        public void ClearSelection()
        {
            foreach (var child in Items)
            {
                if (child != _lastItemSelected)
                {
                    child.IsSelected = false;
                }

                ClearSelectionOnChild(child);
            }
        }

        public void ClearSelectionOnChild(TreeItem item)
        {
            foreach (var child in item.Items)
            {
                if (child != _lastItemSelected)
                {
                    child.IsSelected = false;
                }

                ClearSelectionOnChild(child);
            }
        }

        public void ItemSelected(object sender, EventArgs e)
        {
            _lastItemSelected = (TreeItem)sender;

            ClearSelection();

            ChangeValue();
        }

        public override void Update(double time, KeyboardState keyboardState,
            MouseState mouseState, TouchCollection touchState)
        {
            if (Enable)
            {
                var bounds = GetLocalBounds();

                int cpt = 0;
                for (var i=0;i<Items.Count;i++)
                {
                    var item = Items.ElementAt(i);

                    item.X = bounds.X + this.X;
                    item.Y = bounds.Y + this.Y + i * 15 + cpt * 15;

                    item.Update(time);

                    cpt += item.IsDraw();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Enable && Visible)
            {
                base.Draw(spriteBatch);

                foreach(var item in Items)
                {
                    item.Draw(spriteBatch);
                }
            }
        }

        public string GetItemSelected()
        {
            return _lastItemSelected != null ? _lastItemSelected.Text : null;
        }
    }

}
