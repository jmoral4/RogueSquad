#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSquad.Core;
using MonoGame.Extended;
using RogueSquad.Core.Systems;
using GeonBit.UI;
using GeonBit.UI.Entities;
using QuakeConsole;
using RogueSquad.Client.Dx.Screens;
using RogueSquad.Core.Components.General;
using System.Collections.Generic;
using MonoGame.Extended.ViewportAdapters;
using RogueSquad.Core.Editors;

#endregion

namespace RogueSquad.Client.Dx
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public class NpcEditorScreen : GameScreen
    {
        NpcEditor _npcEditor;
        #region Fields

        ContentManager Content;     
        
        float pauseAlpha;
        InputAction pauseAction;

        ConsoleComponent _console;
        World world;
        FramesPerSecondCounter fps;
 
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;        

        Camera2D _camera;
        ViewportAdapter _viewportAdapter;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public NpcEditorScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            pauseAction = new InputAction(
                new Buttons[] { Buttons.Start, Buttons.Back },
                new Keys[] { Keys.Escape },
                true);            
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (Content == null)
                    Content = new ContentManager(ScreenManager.Game.Services, "Content");

                _console = new ConsoleComponent(ScreenManager.Game);
              


                ScreenManager.Game.Components.Add(_console);
                fps = new FramesPerSecondCounter();
                UserInterface.Initialize(Content, BuiltinThemes.hd);
                LoadContent();
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        protected void LoadContent()
        {
            _viewportAdapter = new BoxingViewportAdapter(ScreenManager.Game.Window, ScreenManager.GraphicsDevice, 1920, 1080);
            _camera = new Camera2D(_viewportAdapter);

            // create a panel and position in center of screen
            Panel panel = new Panel(new Vector2(400, 400), PanelSkin.Default, Anchor.Center);
            UserInterface.Active.AddEntity(panel);

            // add title and text
            panel.AddChild(new Header("Rogue Squad AI Test"));
            panel.AddChild(new HorizontalLine());
            panel.AddChild(new Paragraph("Simple AI Test"));

            //var tx = Content.Load<Texture2D>("Textures/ro");

            var btn = new GeonBit.UI.Entities.Button("Close", ButtonSkin.Default, Anchor.BottomCenter);
            btn.OnClick += (e) => { Console.WriteLine("clicked"); Entity en = e; en.Parent.Visible = false; };

            // add a button at the bottom
            panel.AddChild(btn);

            world = new World(ScreenManager.GraphicsDevice, this.Content, Engine.Instance.ScreenWidth, Engine.Instance.ScreenHeight);
            _camera.Zoom = 1.0f;
            world.AttachCamera(_camera, _viewportAdapter);
            world.EnableBasicSystems();
            world.EnableDebugRendering();
            world.SetScene("Testbed");

            var interpreter = new ManualInterpreter();
            _console.Interpreter = interpreter;

            interpreter.RegisterCommand("EnableDebug", (x) => { world.EnableDebugRendering(); });
            interpreter.RegisterCommand("DisableDebug", (x) => { world.DisableDebugRendering(); });
            interpreter.RegisterCommand("Exit", (x) => { ScreenManager.Game.Exit(); });
            //_trs = new TileRenderingSystem(ScreenManager.SpriteBatch, Content.Load<Texture2D>("Assets/grass"), Content.Load<Texture2D>("Assets/stone_tile"), Content.Load<SpriteFont>("Fonts/gamefont"));
            // _trs.CreateMap(10, 10);



        }

        



        public override void Deactivate()
        {

            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            Content.Unload();

        }


        #endregion

        #region Update and Draw


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
                fps.Update(gameTime);
                // GeonBit.UIL update UI manager
                UserInterface.Active.Update(gameTime);                
                world.Update(gameTime);

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

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
            {

                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                //do game stuff

                currentKeyboardState = Keyboard.GetState();
                // Exit() is obsolete on iOS
#if !__IOS__ && !__TVOS__
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    this.ExitScreen();
#endif

                if (previousKeyboardState.IsKeyUp(Keys.OemTilde) && currentKeyboardState.IsKeyDown(Keys.OemTilde))
                {
                    _console.ToggleOpenClose();
                    if (_console.IsAcceptingInput)
                        world.Pause();
                    else
                        world.UnPause();
                }

                if (!world.IsPaused)
                {
                    if (previousKeyboardState.IsKeyUp(Keys.OemPlus) && currentKeyboardState.IsKeyDown(Keys.OemPlus))
                    {
                        _camera.ZoomIn(.1f);
                    }
                    if (previousKeyboardState.IsKeyUp(Keys.OemMinus) && currentKeyboardState.IsKeyDown(Keys.OemMinus))
                    {
                        _camera.ZoomOut(.1f);
                    }
                }

                previousKeyboardState = currentKeyboardState;
            }
        }



        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {

            //_camera = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 256, 128);
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                Color.CornflowerBlue, 0, 0);
            ScreenManager.Game.Window.Title = "Fps:" + fps.FramesPerSecond + " Entities: " + world.EntityCount;
            fps.Draw(gameTime);
            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            // spriteBatch.Begin();

            // ScreenManager.Game.Window.Title = "Fps:" + fps.FramesPerSecond + " Entities: " + world.EntityCount;
            // fps.Draw(gameTime);            
            //_trs.Draw(gameTime);
          
             world.Draw(gameTime);


            UserInterface.Active.Draw(spriteBatch);


            // spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }


        #endregion
    }
}
