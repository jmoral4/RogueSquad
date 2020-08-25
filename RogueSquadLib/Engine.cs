using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using RogueSquadLib.BaseServices;

namespace RogueSquadLib
{
    public class Engine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _testTexture;
        private OrthographicCamera _camera;

        public Engine()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 1920, 1080);            
            _camera = new OrthographicCamera(viewportAdapter);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _testTexture = Content.Load<Texture2D>("Characters/Enemies/sorcerer attack_Animation 1_0");
            OSInfo.Instance.DebugListAllStats();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            var keyboardState = Keyboard.GetState();
            const float movementSpeed = 100;

            var deltaTime = 0.1f;

            if (keyboardState.IsKeyDown(Keys.W))
                _camera.Move(new Vector2(0, -movementSpeed) * -deltaTime);
            // TODO: Add your update logic here

            if (keyboardState.IsKeyDown(Keys.S))
                _camera.Move(new Vector2(0, -movementSpeed) * deltaTime);

            if (keyboardState.IsKeyDown(Keys.A))
                _camera.Move(new Vector2(movementSpeed, 0) * deltaTime);


            if (keyboardState.IsKeyDown(Keys.D))
                _camera.Move(new Vector2(-movementSpeed, 0) * deltaTime);

            if (keyboardState.IsKeyDown(Keys.LeftAlt) && keyboardState.IsKeyDown(Keys.Enter))
            {
               // ToggleFullScreenWindowed();
            }
            
            // this is an example of getting the screen world coodinates (i.e., when our viewerport is 1080p but physically 800xwhatever)
            var mouseState = Mouse.GetState();
            //_worldPosition = _camera.ScreenToWorld(new Vector2(mouseState.X, mouseState.Y));                       

            base.Update(gameTime);
        }



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var transformMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            _spriteBatch.Draw(_testTexture, Vector2.One, Color.White);
            _spriteBatch.End();

            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
