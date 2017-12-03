using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSquad.Core;
using System;
using MonoGame.Extended;
using RogueSquad.Core.Systems;
using RogueSquad.Core.Components;
using RogueSquad.Client.Dx.Screens;
using RogueSquad.Core.Components.General;

namespace RogueSquad.Client.Uwp
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int defaultBackBufferWidth = 1920;
        int defaultBackBufferHeight = 1080;
        int UpdatesPerSecond = 25;

        ScreenManager screenManager;
        ScreenFactory screenFactory;

        public Game1()
        {
            Engine.Instance.VersionString = $"{1.0}";

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Engine.Instance.DesiredFPS = 60;
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = defaultBackBufferWidth;
            graphics.PreferredBackBufferHeight = defaultBackBufferHeight;
            
            //Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (graphics.PreferredBackBufferWidth / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (graphics.PreferredBackBufferHeight / 2));
            
            screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            AddInitialScreens();
        }
        private void AddInitialScreens()
        {
            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            //screenManager.AddScreen(new GameplayScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);

        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Engine.Instance.Init(screenManager.GraphicsDevice, this.Content);
            Engine.Instance.ScreenWidth = defaultBackBufferWidth;
            Engine.Instance.ScreenHeight = defaultBackBufferHeight;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
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
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);
        }
    }
}
