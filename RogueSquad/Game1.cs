using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueSquad
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont _gameFont;
        private Texture2D _heroTexture;
        private Texture2D _backdropTexture;
        private Texture2D _tiles;
        private Texture2D _bolt;
        private Dictionary<string, Texture2D> _textures;
        private BasicSprite _heroSprite;

        private List<Barrier> _barriers;
        
        private List<Rectangle> _effects;

        class Barrier
        {
            public Rectangle Rectangle { get; set; }
            public BasicSprite Sprite { get; set; }
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _textures = new Dictionary<string, Texture2D>();

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;

            _barriers = new List<Barrier>();
            _effects = new List<Rectangle>();
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

            Random r = new Random();
            var c = r.Next(4, 10);

            for (int i = 0; i < c; i++)
            {
                _barriers.Add(new Barrier
                {
                    Rectangle = new Rectangle
                    {
                        X = r.Next(0, 1000),
                        Y = r.Next(0, 1000),
                        Height = r.Next(50, 500),
                        Width = r.Next(50, 500)
                    },
                    Sprite = new BasicSprite("barrier", 256, 256, 7, 1, r.Next(0, 6))
                });

            }
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameFont = Content.Load<SpriteFont>("UI/gamefont");
            _heroTexture = Content.Load<Texture2D>("Textures/Character Tester");
            _backdropTexture = Content.Load<Texture2D>("Textures/backdrop");
            _bolt = Content.Load<Texture2D>("Textures/bolt tester");
            _tiles = Content.Load<Texture2D>("Textures/BackTiles");
            _textures.Add("hero", _heroTexture);
            _textures.Add("barrier", _tiles);
            _heroSprite = new BasicSprite("hero", 64, 32, 4, 4 );
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                
                if( _heroSprite.PlayState == SpritePlayStates.Play)
                    _heroSprite.Pause();
                else
                {
                    _heroSprite.Play();
                }
            }

            // TODO: Add your update logic here
            _heroSprite.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(_backdropTexture, Vector2.Zero, Color.White);

            foreach (var barrier in _barriers)
            {
                spriteBatch.Draw(_textures[barrier.Sprite.TextureName], barrier.Rectangle, barrier.Sprite.CurrentFrame, Color.White);
            }

            spriteBatch.Draw( _textures[_heroSprite.TextureName], new Rectangle(10,10, _heroSprite.CurrentFrame.Width, _heroSprite.CurrentFrame.Height), _heroSprite.CurrentFrame ,Color.White );


            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
