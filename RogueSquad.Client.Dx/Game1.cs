﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueSquad.Core;
using GeonBit.UI;
using GeonBit.UI.Entities;
using QuakeConsole;
using System;
using MonoGame.Extended;
using RogueSquad.Core.Systems;
using RogueSquad.Core.Components;

namespace RogueSquad.Client.Dx
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ConsoleComponent _console;
        World world;
        FramesPerSecondCounter fps;
        int w = 1920;
        int h = 1080;
        int UpdatesPerSecond = 25;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
 
            Window.AllowUserResizing = true;
           // IsMouseVisible = true;
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            _console = new ConsoleComponent(this);
            Components.Add(_console);
            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = w;
            graphics.PreferredBackBufferHeight = h;
            fps = new FramesPerSecondCounter();
            
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
            // GeonBit.UI: Init the UI manager using the "hd" built-in theme
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            Engine.Instance.Init(this.graphics, this.Content);


            //player.AddComponent(new BasicControllerComponent());
            base.Initialize();
        }

        Vector2 spriteDimensions = new Vector2(80, 80);
        TileRenderingSystem _trs;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // create a panel and position in center of screen
            Panel panel = new Panel(new Vector2(400, 400), PanelSkin.Default, Anchor.Center);
            UserInterface.Active.AddEntity(panel);

            // add title and text
            panel.AddChild(new Header("Rogue Squad AI Test"));
            panel.AddChild(new HorizontalLine());
            panel.AddChild(new Paragraph("Simple AI Test"));

            var btn = new Button("Close", ButtonSkin.Default, Anchor.BottomCenter);
            btn.OnClick += (e) => { Console.WriteLine("clicked"); Entity en = e; en.Parent.Visible = false; };
            
            // add a button at the bottom
            panel.AddChild(btn);
            Vector2 startLocation = new Vector2(100, 100);


            world = new World();
            world.RegisterSystem(new BasicControllingSystem());
            world.RegisterSystem(new CollisionSystem());
            //world.RegisterSystem(new OrderedCollisionSystem(w, h));
            world.RegisterRenderSystem(new RenderingSystem(spriteBatch));
            world.RegisterRenderSystem(new DebugRenderSystem(spriteBatch));



            //uses Engine.Instance.CreateUniqueEntityId()
            RogueEntity player = RogueEntity.CreateNew();
            player.AddComponent(new PositionComponent { Position = startLocation });
            player.AddComponent(new SpriteComponent { Texture = Content.Load<Texture2D>("Assets/robit") });
            player.AddComponent(new CollidableComponent { BoundingRectangle = new RectangleF(startLocation, spriteDimensions) });
            player.AddComponent(new BasicControllerComponent());


            world.AddEntity(player);
            world.UpdateEntity(player);

            //CreateRandomNPCS();
            _trs = new TileRenderingSystem(spriteBatch, Content.Load<Texture2D>("Assets/grass"), Content.Load<Texture2D>("Assets/stone_tile"), Content.Load<SpriteFont>("Fonts/gamefont"));
            _trs.CreateMap(10,10);

        }

        private void CreateRandomNPCS()
        {
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                RogueEntity player = RogueEntity.CreateNew();
                var pos = new Vector2(r.Next(0, w - 20), r.Next(0, h - 20));
                player.AddComponent(new PositionComponent { Position = pos });
                player.AddComponent(new SpriteComponent { Texture = Content.Load<Texture2D>("Textures/ro") });
                player.AddComponent(new CollidableComponent { BoundingRectangle = new RectangleF(pos , spriteDimensions) });
                world.AddEntity(player);
                
                
            }

        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        KeyboardState previousKeyboardState;
        KeyboardState currentKeyboardState;
        double _elapsed;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        protected override void Update(GameTime gameTime)
        {
            _elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
            fps.Update(gameTime);
            // GeonBit.UIL update UI manager
            UserInterface.Active.Update(gameTime);

            if (_elapsed < 40)
            {
                return; //no update unless 40 milliseconds have passed;
            }
            else {
                _elapsed = 0;
            }
            
            currentKeyboardState = Keyboard.GetState();
            // Exit() is obsolete on iOS
            #if !__IOS__ && !__TVOS__
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            #endif            


            // TODO: Add your update logic here
            // cool.Exec();
            
            if (previousKeyboardState.IsKeyUp(Keys.OemTilde) && currentKeyboardState.IsKeyDown(Keys.OemTilde))
                _console.ToggleOpenClose();

            if (previousKeyboardState.IsKeyUp(Keys.Space) && currentKeyboardState.IsKeyDown(Keys.Space))
                CreateRandomNPCS();

                world.Update(gameTime);

            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            this.Window.Title = "Fps:" + fps.FramesPerSecond + " Entities: " + world.EntityCount;
            fps.Draw(gameTime);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // GeonBit.UI: draw UI using the spriteBatch you created above
             _trs.Draw(gameTime);
            // TODO: Add your drawing code here
            world.Draw(gameTime);
            

            UserInterface.Active.Draw(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
