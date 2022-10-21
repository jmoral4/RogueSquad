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
using RogueSquad.Core.Utility;
using System.Collections.Generic;
using MonoGame.Extended.ViewportAdapters;

#endregion

namespace RogueSquad.Client.Dx
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    public class GameplayScreen : GameScreen
    {
        #region Fields

        ContentManager Content;     
        
        float pauseAlpha;
        InputAction pauseAction;

        ConsoleComponent _console;
        World world;
        FramesPerSecondCounter fps;
 
        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;        

        Color _fadeColor = Color.Black;

        Camera2D _camera;
        ViewportAdapter _viewportAdapter;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
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
                // GeonBit.UI: Init the UI manager using the "hd" built-in theme
                UserInterface.Initialize(Content, BuiltinThemes.hd);
                // A real game would probably have more content than this, so
                // it would take longer to load. We simulate that by delaying for a
                // while, giving you a chance to admire the beautiful loading screen.
                //Thread.Sleep(1000);

                LoadContent();

                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        private void CreateEditorWindow()
        {
 
            Panel panel = new Panel(new Vector2(1024, 768), PanelSkin.Default, Anchor.Center);
            panel.Identifier = "Editor";
            Panel imgPanel = new Panel(new Vector2(150, 150), PanelSkin.Default, Anchor.AutoInline);            
            Texture2D entityTex = Content.Load<Texture2D>("JeremyLightcap/robit");
            imgPanel.AddChild(new Image(entityTex, new Vector2(0,0), ImageDrawMode.Stretch, Anchor.TopLeft));
            panel.AddChild(imgPanel);

            Panel entityInfo = new Panel(new Vector2(800, 300), PanelSkin.Default, Anchor.AutoInline );
            Panel aiInfo = new Panel(new Vector2(600, 300), PanelSkin.Default, Anchor.AutoInline);            

            panel.AddChild(entityInfo);
            panel.AddChild(aiInfo);
            var btn = new GeonBit.UI.Entities.Button("Close", ButtonSkin.Default, Anchor.BottomCenter);
            btn.OnClick += (e) => { Console.WriteLine("clicked"); Entity en = e; en.Parent.Visible = false; };
            panel.AddChild(btn);

            UserInterface.Active.AddEntity(panel);

        }
    
 
        protected void LoadContent()
        {
            _viewportAdapter = new BoxingViewportAdapter(ScreenManager.Game.Window, ScreenManager.GraphicsDevice, 1280, 720);
            _camera = new Camera2D(_viewportAdapter);

            // create a panel and position in center of screen
            Panel panel = new Panel(new Vector2(400, 400), PanelSkin.Default, Anchor.Center)
            {
                Identifier = "Startup"
            };


            // add title and text
            panel.AddChild(new Header("Rogue Squad AI Test"));
            panel.AddChild(new HorizontalLine());
            panel.AddChild(new Paragraph("Simple AI Test"));


            var btn = new GeonBit.UI.Entities.Button("Close", ButtonSkin.Default, Anchor.BottomCenter);
            btn.OnClick += (e) => { Console.WriteLine("clicked"); Entity en = e; en.Parent.Visible = false; };

            // add a button at the bottom
            panel.AddChild(btn);

            panel.Visible = true;
            UserInterface.Active.AddEntity(panel);
            //CreateEditorWindow();
        
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

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                Color.Red, 0, 0);
            ScreenManager.Game.Window.Title = "Fps:" + fps.FramesPerSecond + " Entities: " + world.EntityCount;
            fps.Draw(gameTime);
            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

             spriteBatch.Begin();

            ScreenManager.Game.Window.Title = "Fps:" + fps.FramesPerSecond + " Entities: " + world.EntityCount;
            fps.Draw(gameTime);

            world.Draw(gameTime);            


            spriteBatch.End();

            UserInterface.Active.Draw(spriteBatch);

            // If the game is transitioning on or off, fade it out to black.
            //if (TransitionPosition > 0 || pauseAlpha > 0)
            //{
            //    float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

            //    ScreenManager.FadeBackBufferToBlack(alpha);
            //}
        }


        #endregion
    }
}
