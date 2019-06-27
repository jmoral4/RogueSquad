using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RogueLite
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AnimatedSprite _player;
        Texture2D _playerRaw;
        Vector2 _location;
        SpriteFont _debugFont;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            // TODO: Add your initialization logic here
            _location = new Vector2(200, 200);
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
            _playerRaw = Content.Load<Texture2D>("Textures/PlayerCharacter/knight");
            // TODO: use this.Content to load your game content here
            _player = new AnimatedSprite(_playerRaw, new Rectangle(0, 0, 672, 80), new Vector2(672/7, 80), 2);

            _debugFont = Content.Load<SpriteFont>("Fonts/BasicFont");
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

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _location.X--;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _location.X++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                _location.Y--;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                _location.Y++;
            }

            // TODO: Add your update logic here
            _player.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            spriteBatch.DrawString(_debugFont, "Test debug font", Vector2.One, Color.Red);
           // spriteBatch.Draw(_playerRaw, Vector2.One, Color.White);
            _player.Draw(new Rectangle((int)_location.X,(int) _location.Y, 200,200), spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
