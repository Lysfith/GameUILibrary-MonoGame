using GameUILibrary.Test.Models;
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

        private UI _uiTest;
        private TestViewModel _uiModel;

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
            _graphics.PreferMultiSampling = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();

            this.IsFixedTimeStep = false;
            this.IsMouseVisible = true;

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

            _uiTest = UI.LoadJSON("UIDescription/Test/Test.json");

            _uiTest.SetSize(
                _graphics.PreferredBackBufferWidth,
                _graphics.PreferredBackBufferHeight
                );

            _uiModel = new TestViewModel();
            _uiTest.SetModel(_uiModel);

            _uiTest.Start();

            UI.SaveJSON("UIDescription/Test/Test2.json", _uiTest);


            TextureManager.Instance.SetContentManager(Content);
            FontManager.Instance.SetContentManager(Content);
        }

        private void ReloadUI()
        {
            _uiTest = UI.ReloadJSON(_uiTest);
            _uiTest.Start();
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

            _uiTest.Update(gameTime.ElapsedGameTime.TotalSeconds);

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

            _uiTest.Draw(_spriteBatch);

            var font = FontManager.Instance.GetFont("Fonts/Arial-10");
            _spriteBatch.DrawString(font, _uiModel.Button1.ToString(), new Vector2(10, 70), Color.Yellow);
            _spriteBatch.DrawString(font, _uiModel.Button2.ToString(), new Vector2(10, 90), Color.Yellow);
            _spriteBatch.DrawString(font, _uiModel.Button3.ToString(), new Vector2(10, 110), Color.Yellow);
            _spriteBatch.DrawString(font, _uiModel.Check1.ToString(), new Vector2(10, 130), Color.Yellow);

            if (_uiModel.RadioGroup1 != null)
            {
                _spriteBatch.DrawString(font, _uiModel.RadioGroup1, new Vector2(10, 150), Color.Yellow);
            }

            //==========================
            _drawTime = _stopwatch.ElapsedMilliseconds;

            _spriteBatch.DrawString(font, "Update : " + _updateTime.ToString("0.000") + " ms", new Vector2(10, 10), Color.Yellow);
            _spriteBatch.DrawString(font, "Draw : " + _drawTime.ToString("0.000") + " ms", new Vector2(10, 30), Color.Yellow);

            if (_minFrameCounter != int.MaxValue)
            {
                _spriteBatch.DrawString(font,
                    "Fps : " + _lastFrameCounter.ToString("00.00") + ", Min : " + _minFrameCounter.ToString("00.00") + ", Max : " + _maxFrameCounter.ToString("00.00"),
                    new Vector2(10, 50), Color.Yellow);
            }
            else
            {
                _spriteBatch.DrawString(font,
                    "Fps : " + _lastFrameCounter.ToString("00.00") + ", Max : " + _maxFrameCounter.ToString("00.00"),
                    new Vector2(10, 50), Color.Yellow);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
