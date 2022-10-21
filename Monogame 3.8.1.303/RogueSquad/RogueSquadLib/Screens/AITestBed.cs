using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using RogueSquadLib.BaseServices;
using RogueSquadLib.Core.Screens;
using RogueSquadLib.GamePlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using RogueSquadLib.Entities;

namespace RogueSquadLib.Screens
{
    internal class AITestBed : GameScreen
    {
        SpriteFont _gameFont;
        InputAction escapeAction;
        InputAction pauseAction;
        InputAction regenerateSpeedAction;
        InputAction debugToggle;
        float pauseAlpha;
        private FrameCounter _frameCounter = new FrameCounter();
        ContentManager content;
        GameWorld world;
        List<Texture2D> textures;
        List<Actor> allActors;
        List<MovementComponent> colliders;
        bool paused = false;
        bool toggleDebugRender = false;

        const int actorSize = 32;

        public AITestBed()
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

            regenerateSpeedAction = new InputAction(null,
                new Keys[] { Keys.R }, true);

            debugToggle = new InputAction(null, new Keys[] { Keys.G }, true);

        }

        public override void Activate(bool instancePreserved)
        {
            Instrument.StartTiming("Act");
            Instrument.Start("Activate", "Begin Activation");

            if (!instancePreserved)
            {
               
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                world = GameWorld.LoadFromFile("AI.ini");
                world.GenerateRandomMap();

                _gameFont = content.Load<SpriteFont>("GameFont");
                OSInfo.Instance.DebugListAllStats();

                //load the textures
                textures = new List<Texture2D>();
                foreach (var textureName in world.TextureNames)
                {
                    textures.Add(content.Load<Texture2D>(textureName));
                }


                allActors = new List<Actor>();
                colliders = new List<MovementComponent>();

                Random rand = new Random();
                var colorList = new Color[] { Color.Red, Color.Black, Color.Green, Color.Aquamarine, Color.MistyRose, Color.Yellow };
                
                for (int i = 0; i < 5; i++)
                {
                    Actor elb = new Actor();
                    elb.Faction = "elbonia";
                    elb.Name = "elbonian_soldier_" + i.ToString();
                    elb.DrawComponent.Texture = content.Load<Texture2D>("Tiles/mushroom_tan");
                    elb.DrawComponent.TextureId = 2;
                    elb.DrawComponent.Scale = new Vector2(1, 1);
                    elb.DrawComponent.SrcRect = new Rectangle(0, 0, 128, 128);

                    Color color = new Color(rand.Next(0,256),rand.Next(0,256),rand.Next(0,256));

                    elb.DrawComponent.Tint = color;// colorList[rand.Next(0,colorList.Length)];
                    elb.MovementInfo.BoundingBox = new Rectangle(rand.Next(0,1900), rand.Next(500, 900), actorSize, actorSize);
                    var r = rand.Next(10, 13);
                    float speedX = (float)r / 100;
                    r = rand.Next(10, 13);
                    float speedY = (float)r/100;

                    // need them to have a 50% probability of going negative (up on the y)
                    speedX *= rand.Next(1, 101) > 50 ? 1f : -1f;
                    speedY *= rand.Next(1, 101) > 50 ? 1f : -1f;

                    elb.MovementInfo.Speed = new Vector2(speedX, speedY);
                    colliders.Add(elb.MovementInfo);
                    allActors.Add(elb);
                }


                var mushroom = new Actor();
                mushroom.Name = "Mushroom";
                mushroom.Faction = "PlayerFaction";

                mushroom.DrawComponent.Texture = content.Load<Texture2D>("Tiles/mushroom_red");
                mushroom.DrawComponent.TextureId = 3;
                mushroom.DrawComponent.Scale = new Vector2(1, 1);
                mushroom.DrawComponent.Tint = Color.White;
                mushroom.DrawComponent.SrcRect = new Rectangle(0, 0, 128, 128);
                mushroom.MovementInfo.BoundingBox = new Rectangle(100, 100, actorSize, actorSize);
                mushroom.MovementInfo.Speed = new Vector2(0, 0);

                colliders.Add(mushroom.MovementInfo);
                allActors.Add(mushroom);


            }

            Instrument.Stop("Activate", "Ended Activation");
            Instrument.StopTiming("Act");
        }

        private void RegenerateAISpeed()
        {
            Random rand = new Random();
            foreach (var actor in allActors.Where(x => x.Faction == "elbonia"))
            {
                var r = rand.Next(1, 26);
                float speedX = (float)r / 100;
                r = rand.Next(1, 26);
                float speedY = (float)r / 100;

                speedX *= rand.Next(1, 101) > 50 ? 1f : -1f;
                speedY *= rand.Next(1, 101) > 50 ? 1f : -1f;

                actor.MovementInfo.Speed = new Vector2(speedX, speedY);
            }
            
        }

        


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            base.Unload();
        }

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            // Gradually fade in or out depending on whether we are covered by the pause screen.
            base.Update(gameTime, otherScreenHasFocus, false);
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (!paused)
            {
                CheckCollisions();

                // handle game update logic
                foreach (var actor in allActors)
                {
                    if (actor.CanMove)
                    {
                        actor.MovementInfo.UpdateBoundingBox(new Rectangle((int)(actor.MovementInfo.BoundingBox.X + (actor.MovementInfo.Speed.X * gameTime.ElapsedGameTime.Milliseconds)), (int)(actor.MovementInfo.BoundingBox.Y + (actor.MovementInfo.Speed.Y * gameTime.ElapsedGameTime.Milliseconds)), actorSize, actorSize));


                        // check collision with screen boundary and keep in-screen
                        if (actor.MovementInfo.BoundingBox.X <= 0 || actor.MovementInfo.BoundingBox.X >= (ScreenManager.Game.Window.ClientBounds.Width -actor.MovementInfo.BoundingBox.Width))
                        {
                            actor.MovementInfo.Speed = new Vector2(actor.MovementInfo.Speed.X * -1, actor.MovementInfo.Speed.Y);
                        }
                        if ( actor.MovementInfo.BoundingBox.Y <= 0)
                        {
                            // it seems that if speed is too slow, there's not enough time for the object to cross back over the threshold (0 or max/min) and in turn the object
                            // gets stuck hugging the border as it rapidly toggles due to the speed * -1, unable to bounc back...
                            actor.MovementInfo.Speed = new Vector2(actor.MovementInfo.Speed.X , Math.Abs(actor.MovementInfo.Speed.Y));
                        }
                        if (actor.MovementInfo.BoundingBox.Y >= (ScreenManager.Game.Window.ClientBounds.Height - actor.MovementInfo.BoundingBox.Height))
                        {
                            actor.MovementInfo.Speed = new Vector2(actor.MovementInfo.Speed.X,  actor.MovementInfo.Speed.Y > 0f ? actor.MovementInfo.Speed.Y * -1 : actor.MovementInfo.Speed.Y);
                        }
                       
                    }
                }

                
            }
        }

        // TODO: Important: Collision is completely broken at the moment via the N+N^2 impl, this was for testing but needs to be properly coded in.
        private void CheckCollisions()
        {
            return;
            foreach (var col in colliders)
            {
                //reset everything for a new check
                col.ResetCollision();
            }

            //O(N+N^2) check :(
            foreach (var col in colliders)
            {
                foreach (var col2 in colliders)
                {
                    // don't compare to self obviously
                    if (col.Id != col2.Id)
                    {
                        if (col.BoundingBox.Intersects(col2.BoundingBox))
                        { 
                            col.Collided = true;
                            col2.Collided = true;
                        }
                    }
                }
            }
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
            }
            else if (regenerateSpeedAction.Evaluate(input, ControllingPlayer, out player))
            {
                RegenerateAISpeed();
            }
            else if (debugToggle.Evaluate(input, ControllingPlayer, out player))
            { 
                toggleDebugRender = !toggleDebugRender;
            }
            else
            {
                // handle input in generic case
            }

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var mousePosition = new Point(mouseState.X, mouseState.Y);
                //select a tile

                var xTile = mousePosition.X / world.TileWidth;
                var yTile = mousePosition.Y / world.TileHeight;

                // TODO: Note, the mousestate triggers too quickly, attempting a click on/off results in flickering (due to 30x/s execution). 
                world.TileMap[xTile, yTile].Selected = true;
                Debug.WriteLine($"MOUSE:{mousePosition.X},{mousePosition.Y}   TILE:{xTile},{yTile}");

            }
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                //clear
                var mousePosition = new Point(mouseState.X, mouseState.Y);
                //select a tile

                var xTile = mousePosition.X / world.TileWidth;
                var yTile = mousePosition.Y / world.TileHeight;

                world.TileMap[xTile, yTile].Selected = false;
                Debug.WriteLine($"MOUSE:{mousePosition.X},{mousePosition.Y}   TILE:{xTile},{yTile}");
            }
            


        }

        public override void Draw(GameTime gameTime)
        {
            //prepare for cache coherence
           // var prepped = allActors.Select(x => new { BB = x.MovementInfo.BoundingBox, Tex = x.DrawComponent.Texture, Tint = x.DrawComponent.Tint });


            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);
            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                  Color.CornflowerBlue, 0, 0);

            SpriteBatch _spriteBatch = ScreenManager.SpriteBatch;

            _spriteBatch.Begin();


            for (int i = 0; i < world.TileMap.GetLength(0); i++)
            {
                for (int j = 0; j < world.TileMap.GetLength(1); j++)
                {

                    _spriteBatch.Draw(textures[world.TileMap[i, j].Type], new Rectangle(i * world.TileWidth, j * world.TileHeight, world.TileWidth, world.TileHeight), Color.White);
                    
                    if (toggleDebugRender)
                    {
                        _spriteBatch.DrawString(_gameFont, $"{world.TileMap[i, j].X},{world.TileMap[i, j].Y}", new Vector2(i * world.TileWidth, j * world.TileHeight), Color.Green);
                    }

                    
                }
            }            

            _spriteBatch.End();


            _spriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < world.TileMap.GetLength(0); i++)
            {
                for (int j = 0; j < world.TileMap.GetLength(1); j++)
                {
                    var current = world.TileMap[i, j];
                    if (current.Selected)
                    {
                        _spriteBatch.FillRectangle(new Rectangle(i * world.TileWidth, j * world.TileHeight, world.TileWidth, world.TileHeight), Color.Red, 0);
                        
                        // show neighbors as well
                        foreach (var n in current.Neighbors)
                        {
                            _spriteBatch.FillRectangle(new Rectangle(n.X * world.TileWidth, n.Y * world.TileHeight, world.TileWidth, world.TileHeight), Color.Yellow, 0);
                            

                        }
                    }                    
                }
            }


            _spriteBatch.End();


            _spriteBatch.Begin();



            foreach (var actor in allActors)
            {
                _spriteBatch.Draw(actor.DrawComponent.Texture, actor.MovementInfo.BoundingBox, actor.DrawComponent.SrcRect, actor.DrawComponent.Tint);
            }

            _spriteBatch.End();


            // DRAW DEBUG INFO ON TOPMOST LAYER
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_gameFont, fps, new Vector2(1, 1), Color.Black);
            _spriteBatch.DrawString(_gameFont, paused ? "paused": "", new Vector2(1, 20), Color.Red);

            if (toggleDebugRender)
            {
                foreach (var actor in allActors)
                {
                    if (actor.MovementInfo.Collided)
                    {
                        _spriteBatch.DrawRectangle(actor.MovementInfo.BoundingBox, Color.Red, 1);
                    }
                    else
                    {
                        _spriteBatch.DrawRectangle(actor.MovementInfo.BoundingBox, Color.Green, 1);
                    }
                    _spriteBatch.DrawString(_gameFont, $"SPD:{actor.MovementInfo.Speed.X},{actor.MovementInfo.Speed.Y}\nLOC:{actor.MovementInfo.BoundingBox.Location}", new Vector2(actor.MovementInfo.BoundingBox.X, actor.MovementInfo.BoundingBox.Y), Color.Black);
                }
            }
            
            _spriteBatch.End();

        }
    }
}
