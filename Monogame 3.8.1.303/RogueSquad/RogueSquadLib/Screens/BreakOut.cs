using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using RogueSquadLib.Core.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using RogueSquadLib.BaseServices;
using RogueSquadLib.Entities.Breakout;

namespace RogueSquadLib.Screens;

public class BreakOut : GameScreen
{
    private List<Brick> _bricks;
    private Bar _paddle;
    private Ball _ball;
    private SpriteFont _gameFont;
    const int BrickWidth = 64;
    const int BrickHeight = 32;
    const int PaddleWidth = 128;
    const int PaddleHeight = 32;
    const int BallWidth = 32;
    const int BallHeight = 32;
    const int BrickRows = 10;

    private int currentScore = 0;
    const int MaxLives = 3;
    int currentLives = MaxLives;
    
    ContentManager content;
    private FrameCounter _frameCounter = new FrameCounter();
    InputAction escapeAction;
    InputAction pauseAction;
    float pauseAlpha;
    bool paused = false;
    private BreakoutGameState _gameState;
    private float elapsedMs = 0;
    private Texture2D _bg;
    
    Song _bgMusic;
    
    enum BreakoutGameState
    {
        Ready, Playing, Paused, GameOver
    }

    public BreakOut()
    {
        TransitionOnTime = TimeSpan.FromSeconds(1.5);
        TransitionOffTime = TimeSpan.FromSeconds(0.5);

        escapeAction = new InputAction(
            new Buttons[] { Buttons.Back },
            new Keys[] { Keys.Escape },
            true);

        pauseAction = new InputAction(
            new Buttons[] { Buttons.Start },
            new Keys[] { Keys.Space }, true);

        _gameState = BreakoutGameState.Ready;
    }

    public override void HandleInput(GameTime gameTime, InputState input)
    {
        if (input == null)
            throw new ArgumentNullException("input");
        // Look up inputs for the active player profile.
        int playerIndex = (int)ControllingPlayer.Value;
        GamePadState gamepadState = input.CurrentGamePadStates[playerIndex];
        KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];

        // The game pauses either if the user presses the pause button, or if
        // they unplug the active gamepad. This requires us to keep track of
        // whether a gamepad was ever plugged in, because we don't want to pause
        // on PC if they are playing with a keyboard and have no gamepad at all!
        bool gamePadDisconnected = !gamepadState.IsConnected &&
                                   input.GamePadWasConnected[playerIndex];
        
        PlayerIndex player;
        if (escapeAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
        {
            // escape screen
            ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
        }
        else if (pauseAction.Evaluate(input, ControllingPlayer, out player))
        {
            // simple game pause
            paused = !paused;
            if (paused)
            {
                MediaPlayer.Volume = 0.1f;    
            }
            else
            {
                MediaPlayer.Volume = 0.5f;
            }

        }
    }

    
        
    public override void Draw(GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _frameCounter.Update(deltaTime);
        var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
        ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
            Color.CornflowerBlue, 0, 0);
        SpriteBatch _spriteBatch = ScreenManager.SpriteBatch;

        _spriteBatch.Begin();
        
        _spriteBatch.Draw(_bg, ScreenManager.GraphicsDevice.Viewport.Bounds, Color.White);
        
        foreach(var brick in _bricks)
        {
            _spriteBatch.Draw(brick.Texture, brick.BoundingBox, brick.Color);
        }
        
        elapsedMs += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        if( elapsedMs > 60)
        {
            elapsedMs = 0;
            _ball.rotation += 0.25f;
            if( _ball.rotation > 360)
                _ball.rotation = 0;
        }
        
        _spriteBatch.Draw(_paddle.Texture, _paddle.BoundingBox, Color.White);
        
        _spriteBatch.Draw(_ball.Texture, _ball.BoundingBox , null, Color.White, _ball.rotation, new Vector2(_ball.Texture.Width/2,_ball.Texture.Height/2), SpriteEffects.None, 0);
        _spriteBatch.End();
        
        
        _spriteBatch.Begin();
        _spriteBatch.DrawString(_gameFont, fps, new Vector2(1, ScreenManager.GraphicsDevice.Viewport.Height-20), Color.Black);
        
        //_spriteBatch.DrawRectangle(_ball.BoundingBox, Color.Green,1);
        
        _spriteBatch.End();

    }
    
    public override void Update(GameTime gameTime, bool otherScreenHasFocus,
        bool coveredByOtherScreen)
    {
        
        // Gradually fade in or out depending on whether we are covered by the pause screen.
        base.Update(gameTime, otherScreenHasFocus, false);
        if (coveredByOtherScreen)
            pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
        else
            pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

        if (_gameState == BreakoutGameState.Playing)
        {
            
            
        }

    }
    
    public override void Activate(bool instancePreserved)
    {
        if (!instancePreserved)
        {
                InitializeGameBoard();
        }
    }

    
    private void InitializeGameBoard()
    {
        if (content == null)
            content = new ContentManager(ScreenManager.Game.Services, "Content");

        _gameFont = content.Load<SpriteFont>("GameFont");
            
        _bricks = new List<Brick>();
        //calculate the number of bricks we can fit on the screen
        int brickCount = ScreenManager.GraphicsDevice.Viewport.Width / BrickWidth;
        int totalBricks = brickCount * BrickRows; //5 rows of bricks
        Random r = new Random();
        
        //new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255)
        
        for(int i =0; i< totalBricks; i++)
        {
            for(int j =0; j< BrickRows; j++)
            {
                var brick = new Brick(content.Load<Texture2D>($"Tiles/Iso/isometric_pixel_flat_{r.Next(0,7).ToString().PadLeft(4,'0')}"), new Rectangle(i * BrickWidth, j * BrickHeight, BrickWidth, BrickHeight), Color.White);
                _bricks.Add(brick);
            }
        }
        
        _paddle = new Bar(content.Load<Texture2D>("Tiles/Iso/isometric_pixel_flat_0031"), new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width/2-(PaddleWidth/2), ScreenManager.GraphicsDevice.Viewport.Height - PaddleHeight*4, PaddleWidth, PaddleHeight), Color.White);
        
        _ball = new Ball(content.Load<Texture2D>("rock_small"), new Rectangle(ScreenManager.GraphicsDevice.Viewport.Width/2-(BallWidth/2), _paddle.BoundingBox.Location.Y - BallHeight, BallWidth, BallHeight), Color.White);
        _ball.BoundingBox = new Rectangle(_ball.BoundingBox.X + (_ball.BoundingBox.Width/2), (int)_ball.BoundingBox.Y + (_ball.BoundingBox.Height/2), BallWidth, BallHeight);
        string bg_audio = "Maarten Schellekens - Sweet Dreams - Middle-East Remix";
        _bg = content.Load<Texture2D>("cityskyline");
        
        _bgMusic = content.Load<Song>($"Audio/{bg_audio}");
        MediaPlayer.Play(_bgMusic);
        MediaPlayer.IsRepeating = true;
        MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
    }

    void MediaPlayer_MediaStateChanged(object sender, System.
        EventArgs e)
    {
        // 0.0f is silent, 1.0f is full volume
        MediaPlayer.Volume -= 0.1f;
        MediaPlayer.Play(_bgMusic);
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    public override void Unload()
    {
        base.Unload();
    }
}