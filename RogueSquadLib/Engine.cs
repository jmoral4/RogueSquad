using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.TextureAtlases;

using RogueSquadLib.BaseServices;
using Microsoft.Xna.Framework.Media;
using RogueSquadLib.Entities;
using RogueSquadLib.Core.Screens;
using RogueSquadLib.Util;
using System;
using RogueSquadLib.Screens;
using System.Diagnostics;

namespace RogueSquadLib
{
    public class Engine : Microsoft.Xna.Framework.Game
    {        
        ScreenManager screenManager;
        ScreenFactory screenFactory;

        private GraphicsDeviceManager _graphics;
        
        public Engine()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            AddInitialScreens();           
        }

        protected override void Initialize()
        {
            IniReader reader = new IniReader("AI.ini");
            // TODO: IMPORTANT!: Add error handling here of course
            var desiredHeight = reader.GetValue("resolution_height", "settings");
            var desiredWidth = reader.GetValue("resolution_width", "settings");

            // TODO: Add your initialization logic here
            // IMPORTANT!: The window size change _has_ to be in init or later. Otherwise it has no effect. Spent way too many minutes wondering why the window wasn't re-sizing when i had the code in the ctor
            _graphics.PreferredBackBufferHeight = Int32.Parse(desiredHeight);
            _graphics.PreferredBackBufferWidth = Int32.Parse(desiredWidth);
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        private void AddInitialScreens()
        {
            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
            //screenManager.AddScreen(new AITestBed(), null);
        }

        protected override void LoadContent()
        {           
        }
       
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && Keyboard.GetState().IsKeyDown(Keys.L))
            {
                Debug.WriteLine("Master Exit Triggered: Escape + L");
                Exit();
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {            
            base.Draw(gameTime);
        }

        
    }
}
