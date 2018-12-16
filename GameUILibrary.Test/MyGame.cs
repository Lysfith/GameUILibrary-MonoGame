using GameUILibrary.Components;
using GameUILibrary.Components.Controls;
using GameUILibrary.Components.Enums;
using GameUILibrary.Test.Model;
using GameUILibrary.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameUILibrary.Test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MyGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private UI _uiEditor;
        private UI _uiEdit;

        private ViewModel _modelEditor;
        private ViewModel _modelEdit;

        private Stopwatch _stopwatch;
        private double _updateTime;
        private double _drawTime;

        private int _frameCounter;
        private int _lastFrameCounter;
        private Stopwatch _stopwatchFps;
        private int _maxFrameCounter;
        private int _minFrameCounter;

        public MyGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1366;
            _graphics.PreferredBackBufferHeight = 768;
            //_graphics.PreferMultiSampling = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            this.IsFixedTimeStep = false;
            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += Resize;

            TouchPanel.EnableMouseTouchPoint = true;
            TouchPanel.EnableMouseGestures = true;

            TouchPanel.EnabledGestures =
                GestureType.PinchComplete |
                GestureType.Pinch;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _frameCounter = 0;
            _lastFrameCounter = 0;
            _minFrameCounter = int.MaxValue;
            _maxFrameCounter = 0;

            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            _stopwatchFps = new Stopwatch();
            _stopwatchFps.Start();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            FontManager.Instance.SetContentManager(Content);

            _uiEditor = UI.LoadJSON("UIDescription/Editor/editor.json");

            _uiEditor.SetSize(
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight
                );

            _uiEditor.Start();

            _modelEditor = new ViewModel(_uiEditor);

            //Callbacks
            _modelEditor.SetCallback("NewUI", EnumCallback.ON_VALUE_CHANGE, (sender, e) =>
            {
                var button = (UIElement<ButtonState>)sender;

                if(button.Value == ButtonState.Pressed)
                {
                    NewUI();
                }
            });

            _modelEditor.SetCallback("SaveUI", EnumCallback.ON_VALUE_CHANGE, (sender, e) =>
            {
                var button = (UIElement<ButtonState>)sender;

                if (button.Value == ButtonState.Pressed)
                {
                    SaveUI();
                }
            });

            var tree = _uiEditor.GetItem<Tree>("Tree");

            _modelEditor.SetCallback("Tree", EnumCallback.ON_VALUE_CHANGE, (sender, e) =>
            {
                var itemSelected = tree.GetItemSelected();

                if (itemSelected == "UI")
                {
                    
                }
                else
                {
                    var parent = _uiEdit.GetItem(itemSelected);

                    LoadProperties(parent);
                }
            });

            _modelEditor.SetCallback("AddButton", EnumCallback.ON_VALUE_CHANGE, (sender, e) =>
            {
                var button = (UIElement<ButtonState>)sender;
                var itemSelected = tree.GetItemSelected();

                if (button.Value == ButtonState.Pressed && _uiEdit != null && itemSelected != null)
                {
                    var newButton = new Button()
                    {
                        Name = Guid.NewGuid().ToString(),
                        Width = 100,
                        Height = 30,
                        Text = "Button",
                        Font = "Fonts/Arial-16",
                        Color = Color.White
                    };

                    if (itemSelected == "UI")
                    {
                        _uiEdit.AddItem(newButton);
                        tree.AddItem(newButton.Name, itemSelected);
                    }
                    else
                    {
                        var parent = _uiEdit.GetItem(itemSelected);

                        if (parent.Type == EnumControl.CONTAINER)
                        {
                            _uiEdit.AddItem(newButton, itemSelected);
                            tree.AddItem(newButton.Name, itemSelected);
                        }
                    }
                    
                }
            });

            _modelEditor.SetCallback("AddTextbox", EnumCallback.ON_VALUE_CHANGE, (sender, e) =>
            {
                var button = (UIElement<ButtonState>)sender;
                var itemSelected = tree.GetItemSelected();

                if (button.Value == ButtonState.Pressed && _uiEdit != null && itemSelected != null)
                {
                    var newTextbox = new Textbox()
                    {
                        Name = Guid.NewGuid().ToString(),
                        Width = 140,
                        Height = 40,
                        Text = "Textbox",
                        Background = "Textures/Controls/blue_button13",
                        Font = "Fonts/Arial-16",
                        Color = Color.Black
                    };

                    if (itemSelected == "UI")
                    {
                        _uiEdit.AddItem(newTextbox);
                        tree.AddItem(newTextbox.Name, itemSelected);
                    }
                    else
                    {
                        var parent = _uiEdit.GetItem(itemSelected);

                        if (parent.Type == EnumControl.CONTAINER)
                        {
                            _uiEdit.AddItem(newTextbox, itemSelected);
                            tree.AddItem(newTextbox.Name, itemSelected);
                        }
                    }

                }
            });

            _modelEditor.SetCallback("AddProgressBar", EnumCallback.ON_VALUE_CHANGE, (sender, e) =>
            {
                var button = (UIElement<ButtonState>)sender;
                var itemSelected = tree.GetItemSelected();

                if (button.Value == ButtonState.Pressed && _uiEdit != null && itemSelected != null)
                {
                    var newProgressBar = new ProgressBar()
                    {
                        Name = Guid.NewGuid().ToString(),
                        Width = 140,
                        Height = 50,
                        Text = "",
                        Value = 50,
                        BackgroundColor = Color.Gray,
                        Texture = "Textures/progressBar",
                        Font = "Fonts/Arial-16",
                        Color = Color.YellowGreen
                    };

                    if (itemSelected == "UI")
                    {
                        _uiEdit.AddItem(newProgressBar);
                        tree.AddItem(newProgressBar.Name, itemSelected);
                    }
                    else
                    {
                        var parent = _uiEdit.GetItem(itemSelected);

                        if (parent.Type == EnumControl.CONTAINER)
                        {
                            _uiEdit.AddItem(newProgressBar, itemSelected);
                            tree.AddItem(newProgressBar.Name, itemSelected);
                        }
                    }

                }
            });

            _modelEditor.SetCallback("AddPanel", EnumCallback.ON_VALUE_CHANGE, (sender, e) =>
            {
                var button = (UIElement<ButtonState>)sender;
                var itemSelected = tree.GetItemSelected();

                if (button.Value == ButtonState.Pressed && _uiEdit != null && itemSelected != null)
                {
                    var newPanel = new Panel()
                    {
                        Name = Guid.NewGuid().ToString(),
                        Width = 140,
                        Height = 50,
                        Texture = "Textures/Controls/grey_panel",
                        Color = Color.Gray
                    };

                    if (itemSelected == "UI")
                    {
                        _uiEdit.AddItem(newPanel);
                        tree.AddItem(newPanel.Name, itemSelected);
                    }
                    else
                    {
                        var parent = _uiEdit.GetItem(itemSelected);

                        if (parent.Type == EnumControl.CONTAINER)
                        {
                            _uiEdit.AddItem(newPanel, itemSelected);
                            tree.AddItem(newPanel.Name, itemSelected);
                        }
                    }

                }
            });


            _modelEditor.SetCallback("SaveProperties", EnumCallback.ON_VALUE_CHANGE, (sender, e) =>
            {
                var button = (UIElement<ButtonState>)sender;
                var itemSelected = tree.GetItemSelected();

                if (button.Value == ButtonState.Pressed && _uiEdit != null && itemSelected != null && itemSelected != "UI")
                {
                    var textboxX = _uiEditor.GetItem<Textbox>("TextboxX");
                    var textboxY = _uiEditor.GetItem<Textbox>("TextboxY");
                    var textboxWidth = _uiEditor.GetItem<Textbox>("TextboxWidth");
                    var textboxHeight = _uiEditor.GetItem<Textbox>("TextboxHeight");

                    var item = _uiEdit.GetItem(itemSelected);

                    int value = (int)item.X;
                    if (int.TryParse(textboxX.Text, out value))
                        item.X = int.Parse(textboxX.Text);

                    value = (int)item.Y;
                    if (int.TryParse(textboxY.Text, out value))
                        item.Y = int.Parse(textboxY.Text);

                    value = (int)item.Width;
                    if (int.TryParse(textboxWidth.Text, out value))
                        item.Width = int.Parse(textboxWidth.Text);

                    value = (int)item.Height;
                    if (int.TryParse(textboxHeight.Text, out value))
                        item.Height = int.Parse(textboxHeight.Text);

                }
            });

            TextureManager.Instance.SetContentManager(Content);
            FontManager.Instance.SetContentManager(Content);


            var textureBlank = new Texture2D(_graphics.GraphicsDevice, 1, 1);

            Color[] tcolor = new Color[1];
            textureBlank.GetData<Color>(tcolor);
            tcolor[0] = new Color(255, 255, 255, 255);
            textureBlank.SetData<Color>(tcolor);

            TextureManager.Instance.AddTexture("Blank", textureBlank);
        }

        public void Resize(object sender, EventArgs e)
        {
            if (_graphics.PreferredBackBufferWidth != Window.ClientBounds.Width
                || _graphics.PreferredBackBufferHeight != Window.ClientBounds.Height)
            {
                _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                _graphics.ApplyChanges();

                _uiEditor.SetSize(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

                if (_uiEdit != null)
                {
                    int y = (int)((_graphics.PreferredBackBufferHeight - 750) * 0.5f);
                    int x = (int)((_graphics.PreferredBackBufferWidth - 750) * 0.5f);

                    _uiEdit.OffsetX = x;
                    _uiEdit.OffsetY = y;
                }
            }
        }

        private void ReloadUI()
        {
            _uiEditor = UI.ReloadJSON(_uiEditor);
            _uiEditor.Start();
        }

        private void NewUI()
        {
            int y = (int)((_graphics.PreferredBackBufferHeight - 750) * 0.5f);
            int x = (int)((_graphics.PreferredBackBufferWidth - 750) * 0.5f);

            _uiEdit = new UI(x, y);
            _uiEdit.SetSize(750, 750);
            _uiEdit.Start();

            _modelEdit = new ViewModel(_uiEdit);

            var tree = _uiEditor.GetItem<Tree>("Tree");

            tree.Clear();
            tree.AddUi(_uiEdit);
        }

        private void SaveUI()
        {
            UI.SaveJSON("UIDescription/Editor/result.json", _uiEdit);
        }

        private void LoadProperties(UIBaseElement item)
        {
            var textboxX = _uiEditor.GetItem<Textbox>("TextboxX");
            var textboxY = _uiEditor.GetItem<Textbox>("TextboxY");
            var textboxWidth = _uiEditor.GetItem<Textbox>("TextboxWidth");
            var textboxHeight = _uiEditor.GetItem<Textbox>("TextboxHeight");

            textboxX.Text = item.X.ToString();
            textboxY.Text = item.Y.ToString();
            textboxWidth.Text = item.Width.ToString();
            textboxHeight.Text = item.Height.ToString();

            //textboxX.OnValueChange -= (x, y) => { };
            //textboxY.OnValueChange -= (x, y) => { };
            //textboxWidth.OnValueChange -= (x, y) => { };
            //textboxHeight.OnValueChange -= (x, y) => { };

            //textboxX.OnValueChange += (x, y) =>
            //{
            //    int value = (int)item.X;
            //    if(int.TryParse(((Textbox)x).Text, out value))
            //        item.X = int.Parse(((Textbox)x).Text);
            //};

            //textboxY.OnValueChange += (x, y) =>
            //{
            //    int value = (int)item.Y;
            //    if (int.TryParse(((Textbox)x).Text, out value))
            //        item.Y = int.Parse(((Textbox)x).Text);
            //};

            //textboxWidth.OnValueChange += (x, y) =>
            //{
            //    int value = (int)item.Width;
            //    if (int.TryParse(((Textbox)x).Text, out value))
            //        item.Width = int.Parse(((Textbox)x).Text);
            //};

            //textboxHeight.OnValueChange += (x, y) =>
            //{
            //    int value = (int)item.Height;
            //    if (int.TryParse(((Textbox)x).Text, out value))
            //        item.Height = int.Parse(((Textbox)x).Text);
            //};
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.F5))
                ReloadUI();

            if (Keyboard.GetState().IsKeyDown(Keys.F1))
                NewUI();

            if (Keyboard.GetState().IsKeyDown(Keys.F2))
                SaveUI();

            if (_stopwatchFps.ElapsedMilliseconds > 1000)
            {
                if (_frameCounter > _maxFrameCounter)
                {
                    _maxFrameCounter = _frameCounter;
                }

                if (_frameCounter < _minFrameCounter)
                {
                    _minFrameCounter = _frameCounter;
                }

                _lastFrameCounter = _frameCounter;
                _frameCounter = 0;
                _stopwatchFps.Restart();
            }
            else
            {
                _frameCounter++;
            }

            _stopwatch.Restart();

            _uiEditor.Update(gameTime.ElapsedGameTime.TotalSeconds);

            _updateTime = _stopwatch.ElapsedMilliseconds;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _stopwatch.Restart();

            if (_uiEdit != null)
            {
                //Top
                DrawLine(_spriteBatch, 
                    new Vector2(_uiEdit.OffsetX, _uiEdit.OffsetY), 
                    new Vector2(_uiEdit.OffsetX + _uiEdit.Width, _uiEdit.OffsetY), 
                    Color.Red);
                //Right
                DrawLine(_spriteBatch,
                    new Vector2(_uiEdit.OffsetX + _uiEdit.Width, _uiEdit.OffsetY),
                    new Vector2(_uiEdit.OffsetX + _uiEdit.Width, _uiEdit.OffsetY + _uiEdit.Height),
                    Color.Red);
                //Bottom
                DrawLine(_spriteBatch,
                    new Vector2(_uiEdit.OffsetX + _uiEdit.Width, _uiEdit.OffsetY + _uiEdit.Height),
                    new Vector2(_uiEdit.OffsetX, _uiEdit.OffsetY + _uiEdit.Height),
                    Color.Red);
                //Left
                DrawLine(_spriteBatch,
                    new Vector2(_uiEdit.OffsetX, _uiEdit.OffsetY + _uiEdit.Height),
                    new Vector2(_uiEdit.OffsetX, _uiEdit.OffsetY),
                    Color.Red);

                _uiEdit.Draw(_spriteBatch);
            }

            _uiEditor.Draw(_spriteBatch);

            var font = FontManager.Instance.GetFont("Fonts/Arial-10");
            //_spriteBatch.DrawString(font, _uiModel.Button1.Value.ToString(), new Vector2(10, 70), Color.Yellow);
            //_spriteBatch.DrawString(font, _uiModel.Button2.Value.ToString(), new Vector2(10, 90), Color.Yellow);
            //_spriteBatch.DrawString(font, _uiModel.Button3.Value.ToString(), new Vector2(10, 110), Color.Yellow);
            //_spriteBatch.DrawString(font, _uiModel.Check1.Value.ToString(), new Vector2(10, 130), Color.Yellow);

            //if (_uiModel.RadioGroup1 != null)
            //{
            //    var value = _uiModel.RadioGroup1.Value;
            //    _spriteBatch.DrawString(font, value ?? "", new Vector2(10, 150), Color.Yellow);
            //}

            //==========================
            _drawTime = _stopwatch.ElapsedMilliseconds;

            //_spriteBatch.DrawString(font, "Update : " + _updateTime.ToString("0.000") + " ms", new Vector2(10, 10), Color.Yellow);
            //_spriteBatch.DrawString(font, "Draw : " + _drawTime.ToString("0.000") + " ms", new Vector2(10, 30), Color.Yellow);

            //if (_minFrameCounter != int.MaxValue)
            //{
            //    _spriteBatch.DrawString(font,
            //        "Fps : " + _lastFrameCounter.ToString("00.00") + ", Min : " + _minFrameCounter.ToString("00.00") + ", Max : " + _maxFrameCounter.ToString("00.00"),
            //        new Vector2(10, 50), Color.Yellow);
            //}
            //else
            //{
            //    _spriteBatch.DrawString(font,
            //        "Fps : " + _lastFrameCounter.ToString("00.00") + ", Max : " + _maxFrameCounter.ToString("00.00"),
            //        new Vector2(10, 50), Color.Yellow);
            //}

            _spriteBatch.End();

            base.Draw(gameTime);
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
