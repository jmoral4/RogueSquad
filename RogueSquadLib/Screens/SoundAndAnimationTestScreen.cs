using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;
using MonoGame.Extended.ViewportAdapters;
using RogueSquadLib.BaseServices;
using RogueSquadLib.Core.Screens;
using RogueSquadLib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquadLib.Screens
{
    class SoundAndAnimationTestScreen : GameScreen
    {
        private OrthographicCamera _camera;
        SpriteFont _gameFont;
        Texture2D _backgroundtexture;
        ViewportAdapter _viewportAdapter;
        EntityManager _entityManager;
        Song song;
        Player _player;
        ContentManager content;

        InputAction pauseAction;
        float pauseAlpha;

        private FrameCounter _frameCounter = new FrameCounter();

        public SoundAndAnimationTestScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            _entityManager = new EntityManager();
            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);
        }


        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                _viewportAdapter = new ScalingViewportAdapter(ScreenManager.GraphicsDevice, 1920, 1080);
                _camera = new OrthographicCamera(_viewportAdapter);

                _gameFont = content.Load<SpriteFont>("GameFont");
                OSInfo.Instance.DebugListAllStats();

                _backgroundtexture = content.Load<Texture2D>("background");

                var playerTexture = content.Load<Texture2D>("knight");
                var playerAtlas = TextureAtlas.Create("Animations/Player", playerTexture, 84, 84);
                var playerFactory = new SpriteSheetAnimationFactory(playerAtlas);
                playerFactory.Add("idle_down", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }));
                playerFactory.Add("down", new SpriteSheetAnimationData(new[] { 4, 5, 6, 7, 8 }, isLooping: false));
                playerFactory.Add("up", new SpriteSheetAnimationData(new[] { 9, 10, 11, 12, 13 }, isLooping: false));
                playerFactory.Add("idle_up", new SpriteSheetAnimationData(new[] { 29 }));
                playerFactory.Add("idle_right", new SpriteSheetAnimationData(new[] { 14 }));
                playerFactory.Add("right", new SpriteSheetAnimationData(new[] { 15, 16, 17, 18, 19 }, isLooping: false));
                playerFactory.Add("idle_left", new SpriteSheetAnimationData(new[] { 20 }));
                playerFactory.Add("left", new SpriteSheetAnimationData(new[] { 21, 22, 23, 24, 25 }, isLooping: false));
                playerFactory.Add("atk_down", new SpriteSheetAnimationData(new[] { 27, 28 }, isLooping: false));
                playerFactory.Add("atk_up", new SpriteSheetAnimationData(new[] { 30, 31 }, isLooping: false));
                playerFactory.Add("atk_right", new SpriteSheetAnimationData(new[] { 34, 33 }, isLooping: false));
                playerFactory.Add("atk_left", new SpriteSheetAnimationData(new[] { 37, 36 }, isLooping: false));
                _player = _entityManager.AddEntity(new Player(playerFactory));
                _player.Position = new Vector2(800, 600);

                //this.song = content.Load<Song>("EF5");
                //MediaPlayer.Play(song);
                //MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            }
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(song);
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            base.Unload();
        }

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                DebugMsgQueue.Instance.Update(gameTime);
            }
        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            // TODO: Add your update logic here
            // nothing - another pointless update
            var deltaTime = gameTime.GetElapsedSeconds();
            GamePadState gamepadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamepadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];


            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {

                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {

                const float acceleration = 2f;
                if (gamepadState.IsButtonDown(Buttons.LeftThumbstickUp))
                    _player.MoveUp();

                if (gamepadState.IsButtonDown(Buttons.LeftThumbstickDown))
                    _player.MoveDown();
                if (gamepadState.IsButtonDown(Buttons.LeftThumbstickLeft))
                    _player.MoveLeft();
                if (gamepadState.IsButtonDown(Buttons.LeftThumbstickRight))
                    _player.MoveRight();



                if (gamepadState.IsButtonDown(Buttons.A))
                    _player.Attack();




                if (gamepadState.IsButtonDown(Buttons.RightTrigger))
                    _player.Fire();

                _entityManager.Update(gameTime);

                KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
                const float movementSpeed = 500;

                //var deltaTime = 0.1f;
                // ----- moving the camera vs moving the character
                if (keyboardState.IsKeyDown(Keys.W))
                    _camera.Move(new Vector2(0, -movementSpeed) * -deltaTime);
                // TODO: Add your update logic here

                if (keyboardState.IsKeyDown(Keys.S))
                    _camera.Move(new Vector2(0, -movementSpeed) * deltaTime);

                if (keyboardState.IsKeyDown(Keys.A))
                    _camera.Move(new Vector2(movementSpeed, 0) * deltaTime);


                if (keyboardState.IsKeyDown(Keys.D))
                    _camera.Move(new Vector2(-movementSpeed, 0) * deltaTime);



                //if (keyboardState.IsKeyDown(Keys.LeftAlt) && keyboardState.IsKeyDown(Keys.Enter))
                //{
                //   // ToggleFullScreenWindowed();
                //}

                //// this is an example of getting the screen world coodinates (i.e., when our viewerport is 1080p but physically 800xwhatever)
                //var mouseState = Mouse.GetState();
                //_worldPosition = _camera.ScreenToWorld(new Vector2(mouseState.X, mouseState.Y));
                //

                /*
                 * _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());
                // draw map
                _spriteBatch.End();

                _spriteBatch.Begin(transformMatrix: _viewportAdapter.GetScaleMatrix());
                // draw UI
                _spriteBatch.End();
                 */





            }
        }

        public override void Draw(GameTime gameTime)
        {
            //_camera = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 256, 128);
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                Color.CornflowerBlue, 0, 0);

            var transformMatrix = _camera.GetViewMatrix();
            SpriteBatch _spriteBatch = ScreenManager.SpriteBatch;

            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _spriteBatch.Draw(_backgroundtexture,
               new Rectangle(0, 0, _viewportAdapter.Viewport.Width, _viewportAdapter.Viewport.Height), Color.White);

            _spriteBatch.End();

            // entities
            _spriteBatch.Begin(transformMatrix: transformMatrix, samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            _entityManager.Draw(_spriteBatch);
            _spriteBatch.End();

            //_spriteBatch.Begin(transformMatrix: transformMatrix);
            //_spriteBatch.Draw(_testTexture, Vector2.One, Color.White);
            //_spriteBatch.End();
            _spriteBatch.Begin();
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);
            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
            _spriteBatch.DrawString(_gameFont, fps, new Vector2(1, 1), Color.Black);
            _spriteBatch.End();

            // TODO: Add your drawing code here
            //_spriteBatch.Begin();            
            //_spriteBatch.Draw(_transparent, _debugArea, Color.Red);
            //_spriteBatch.DrawString(_gameFont, _camera.Position.ToString(), new Vector2(0, Window.ClientBounds.Height - 32), Color.Black);
            //string msg = DebugMsgQueue.Instance.GetLast();

            //_spriteBatch.DrawString(_gameFont, msg, new Vector2(0, Window.ClientBounds.Height - 64), Color.Black);
            //_spriteBatch.End();

        }

    }
}
