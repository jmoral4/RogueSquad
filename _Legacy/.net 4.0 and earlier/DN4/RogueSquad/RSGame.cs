using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace RogueSquad
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RSGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D testSprite;
        SpriteFont debugFont;
        StringBuilder debugMessage;
        ControllableCharacter selectedCharacter;

        class ControllableCharacter
        { 
            public int PlayerId { get; set; } // extract to player class later
            public string FactionName { get; set; } //extract to faction class later
            public string CharacterName { get; set; }  //extract to character class 
            public Texture2D Texture { get; set; }
            public Color Color { get; set; } //used for differentiating duplicates
            public Rectangle BoundingBox { get; set; }

            public static ControllableCharacter None {
                get {
                    return new ControllableCharacter() { PlayerId = -1, Color = Color.White, BoundingBox = Rectangle.Empty, CharacterName = "UNSET", FactionName = "UNSET", Texture = null };
                }
            }
        }

        List<ControllableCharacter> _allUnits;

        public RSGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _allUnits = new List<ControllableCharacter>();
            this.IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
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
            debugMessage = new StringBuilder();
            debugFont = Content.Load<SpriteFont>("Fonts/menufont");
            testSprite = Content.Load<Texture2D>("textures/ro");
            var goodPrefix = "good_";
            var badPrefix = "bad_";
            Random rand = new Random();
            int min = 20;
            int max = 1020;

            int w = testSprite.Width;
            int h = testSprite.Height;

            for (int i = 0; i <= 10; i++)
            {
                _allUnits.Add(new ControllableCharacter { CharacterName = $"{goodPrefix}{i}", Color = Color.Blue, PlayerId = i, FactionName = "Good Guys", Texture = testSprite, BoundingBox = new Rectangle (rand.Next(min, max), rand.Next(min,max), w,h) });
            }
            for (int i = 11; i <= 20; i++)
            {
                _allUnits.Add(new ControllableCharacter { CharacterName = $"{badPrefix}{i}", Color = Color.Red, PlayerId = i, FactionName = "Bad Guys", Texture = testSprite, BoundingBox = new Rectangle(rand.Next(min, max), rand.Next(min, max), w, h) });
            }
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

            // see if our mouse intesects anyone:
            var mousePosition = Mouse.GetState().Position;
            // create a small rectangle for collisions, around the mouse cursor (2pixels)
            var mouseBox = new Rectangle(mousePosition, new Point(2, 2));
            bool matched = false;
            foreach (var item in _allUnits)
            {
                if (item.BoundingBox.Intersects(mouseBox))
                {
                    selectedCharacter = item;
                    matched = true;
                }
            }

            if (matched)
            {
                debugMessage.Clear();
                debugMessage.Append($"Id:{selectedCharacter.PlayerId} Name:{selectedCharacter.CharacterName} Faction:{selectedCharacter.FactionName} X:{mousePosition.X}Y:{mousePosition.Y}");
            }
            else
            {
                debugMessage.Clear();
            }
             

            // TODO: Add your update logic here

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
            foreach (var pc in _allUnits)
            {
                spriteBatch.Draw(testSprite, pc.BoundingBox, pc.Color);            
            }

            spriteBatch.DrawString(debugFont, debugMessage, Vector2.Zero, Color.Red);
           // spriteBatch.Draw(testSprite, Vector2.One, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
