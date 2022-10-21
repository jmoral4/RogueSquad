using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Text;
using AnimatedSprite = MonoGame.Extended.Animations.AnimatedSprite;

namespace RogueSquadLib.Entities
{
    public abstract class Entity
    {
        public bool IsDestroyed { get; private set; }

        protected Entity()
        {
            IsDestroyed = false;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void Destroy()
        {
            IsDestroyed = true;
        }
    }

    public class Player : Entity
    {
        readonly SpriteSheetAnimationFactory _spriteFactory;
        private readonly AnimatedSprite _sprite;
        public RectangleF BoundingBox;

        private float _fireCooldown;
        public Vector2 Direction => Vector2.UnitX.Rotate(Rotation);

        public Vector2 Position { get; set; }

        public float Rotation { get; set; } = 0;
        
        public Vector2 Velocity { get; set; }

        public Vector2 Scale { get; set; }

        public float HackMovementSpeed { get; set; } = 5f;

        public Player(SpriteSheetAnimationFactory spriteFactory)
        {

            _sprite = new MonoGame.Extended.Animations.AnimatedSprite(spriteFactory);
            this.Scale = new Vector2(4, 4);            
            this.Position = new Vector2(10, 10);
            _sprite.Play("idle_down").IsLooping = true;            
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity * deltaTime;
            Velocity *= 5.0f;
            if (_fireCooldown > 0)
            {
                _fireCooldown -= deltaTime;
            }
            _sprite.Update(gameTime);
        }

        string _currentDir;
        public void MoveUp()
        {
            _currentDir = "up";
            _sprite.Play("up").OnCompleted = ReturnToIdle;
            this.Position = new Vector2(this.Position.X, this.Position.Y - HackMovementSpeed);
        }

        public void MoveDown()
        {
            _currentDir = "down";
            _sprite.Play("down").OnCompleted = ReturnToIdle;
            this.Position = new Vector2(this.Position.X, this.Position.Y + HackMovementSpeed);
        }
        public void MoveLeft()
        {
            _currentDir = "left";
            _sprite.Play("left").OnCompleted = ReturnToIdle;
            this.Position = new Vector2(this.Position.X - HackMovementSpeed, this.Position.Y);
        }
        public void MoveRight()
        {
            _currentDir = "right";
            _sprite.Play("right").OnCompleted = ReturnToIdle;
            this.Position = new Vector2(this.Position.X + HackMovementSpeed, this.Position.Y);
        }

        public void Attack()
        {
            switch (_currentDir)
            {
                case "right":
                    _sprite.Play("atk_right").OnCompleted = ReturnToIdle;
                    break;
                case "left":
                    _sprite.Play("atk_left").OnCompleted = ReturnToIdle;
                    break;
                case "down":
                    _sprite.Play("atk_down").OnCompleted = ReturnToIdle;
                    break;
                case "up":
                    _sprite.Play("atk_up").OnCompleted = ReturnToIdle;
                    break;
            }

        }


        public void ReturnToIdle()
        {

            _sprite.Play("idle_" + _currentDir).IsLooping = true;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {            
            _sprite.Draw(spriteBatch, this.Position, this.Rotation, this.Scale);
        }

        public void Accelerate(float acceleration)
        {
            Velocity += Direction * acceleration;
        }

        public void LookAt(Vector2 point)
        {
            Rotation = (float)Math.Atan2(point.Y - Position.Y, point.X - Position.X);
        }

        public void Fire()
        {
            if (_fireCooldown > 0)
            {
                return;
            }

            var angle = Rotation + MathHelper.ToRadians(90);
            var muzzle1Position = Position + new Vector2(14, 0).Rotate(angle);
            var muzzle2Position = Position + new Vector2(-14, 0).Rotate(angle);
            _fireCooldown = 0.2f;
        }


    }
}
