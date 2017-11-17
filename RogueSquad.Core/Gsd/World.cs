using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RogueSquad.Core.Gsd
{
    // straight-forward brute force implementation of core systems

    public class World
    {

    }

    public enum EntityTypes
    {
        Player, Tile, Enemy
    }

    public interface EntityData
    {
        int Id { get; set; }
        EntityTypes EntityType { get; set; }

    }

    public class RenderData
    {
        public Texture2D Texture { get; set; }        
    }

    public class PhysicalData
    {
        public Vector2 Position { get; set; }
        public RectangleF BoundingRectangle { get; set; }

        public void UpdateBoundingRect()
        {
            //update to current position
            BoundingRectangle = new RectangleF(Position.X, Position.Y, BoundingRectangle.Width, BoundingRectangle.Height);
        }
    }

    public class GameEntity 
    {
        public PhysicalData Physical { get; set; }
        public RenderData RenderData { get; set; }



    }


    public class EntityGenerator
    {
        ContentManager _contentRef;
        public EntityGenerator(ContentManager content)
        {
            _contentRef = content;
        }
        private GameEntity LoadEntityTemplate(string name)
        {
            //tODO: load from file
            if (name == "player_main_character")
            {
                var render = new RenderData
                {
                    Texture = _contentRef.Load<Texture2D>("Assets/robit")
                };

                var physical = new PhysicalData { Position = Vector2.One, BoundingRectangle = new RectangleF(1, 1, render.Texture.Bounds.Width, render.Texture.Bounds.Height) };



                GameEntity entity = new GameEntity
                {
                    RenderData = render,
                    Physical = physical
                };
                return entity;
            }



            return null;
        }

        public GameEntity CreatePlayerControlledCharacter(Vector2 initialPosition, string entityId)
        {

            var entity = LoadEntityTemplate(entityId);
            entity.Physical.Position = initialPosition;
            entity.Physical.UpdateBoundingRect();

            return entity;

        }
    }


}
