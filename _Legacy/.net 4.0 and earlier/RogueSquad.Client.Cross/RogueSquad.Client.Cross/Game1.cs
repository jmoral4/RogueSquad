using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSquad.Core;
//using GeonBit.UI;
//using GeonBit.UI.Entities;
using QuakeConsole;
using System;
using MonoGame.Extended;
using RogueSquad.Core.Systems;
using RogueSquad.Core.Components;
using RogueSquad.Client.Cross.Screens;
using RogueSquad.Core.Components.General;

namespace RogueSquad.Client.Cross
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RogueSquadGame : Game
    {
        GraphicsDeviceManager graphics;        
      
        int defaultBackBufferWidth = 1024;
        int defaultBackBufferHeight = 768;        

        ScreenManager screenManager;
        ScreenFactory screenFactory;


        public RogueSquadGame() : base()
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            //DateTime buildDate = new DateTime(2000, 1, 1)
            //                        .AddDays(version.Build).AddSeconds(version.Revision * 2);
            Engine.Instance.VersionString = $"{version}";

            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            //Window.AllowUserResizing = true;
            // IsMouseVisible = true;
            
            foreach (var mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                Console.WriteLine($"Display Mode: {mode}");
            }

            Engine.Instance.DesiredFPS = 60;
            IsFixedTimeStep = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferredBackBufferWidth = defaultBackBufferWidth;
            graphics.PreferredBackBufferHeight = defaultBackBufferHeight;            

            Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (graphics.PreferredBackBufferWidth / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (graphics.PreferredBackBufferHeight / 2));

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

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // GeonBit.UI: Init the UI manager using the "hd" built-in theme
         // yjr  UserInterface.Initialize(Content, BuiltinThemes.hd);
            Engine.Instance.Init(screenManager.GraphicsDevice, this.Content);
            Engine.Instance.ScreenWidth = defaultBackBufferWidth;
            Engine.Instance.ScreenHeight = defaultBackBufferHeight;            
            //player.AddComponent(new BasicControllerComponent());
            base.Initialize();
        }


        protected override void LoadContent()
        {
        }

     
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {         
           
            //USE WITH CAUTION -- overrides all screen managers stuff -- might be useful for global key captures (printscreen?/voip) or cheat codes
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //graphics.GraphicsDevice.Clear(Color.Black);            
            base.Draw(gameTime);
        }
    }
}
