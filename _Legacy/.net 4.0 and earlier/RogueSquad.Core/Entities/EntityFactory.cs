using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;
using RogueSquad.Core.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueSquad.Core.Entities
{
    public class EntityFactory
    {
        private readonly ContentManager Content;

        public EntityFactory(ContentManager content)
        {
            Content = content;
        }


        public RogueEntity Create(string entityName)
        {
            if (entityName == "PlayerChar")
            {
               // return CreatePlayerTri();
                return CreatePlayerKnight();
            }
          

            
            
            return null;
        }

        private RogueEntity CreatePlayerTri()
        {
            RogueEntity player = RogueEntity.CreateNew();
            Vector2 startLocation = new Vector2(10, 10);
            player.AddComponent(new PositionComponent { Position = startLocation, Speed = .25f });            
            var playerTexture = Content.Load<Texture2D>("Proto/LifeOfTri");
            var playerAtlas = TextureAtlas.Create("Animations/Player", playerTexture, 256, 256);
            var playerFactory = new SpriteSheetAnimationFactory(playerAtlas);
            playerFactory.Add("right", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }));
            playerFactory.Add("up", new SpriteSheetAnimationData(new[] { 4, 5, 6, 7 }));
            playerFactory.Add("up_right", new SpriteSheetAnimationData(new[] { 8, 9, 10, 11 }));
            playerFactory.Add("down_right", new SpriteSheetAnimationData(new[] { 12, 13, 14, 15 }));
            playerFactory.Add("aim", new SpriteSheetAnimationData(new[] { 16, 17, 18, 19 }, isLooping: false));
            playerFactory.Add("death", new SpriteSheetAnimationData(new[] { 20, 21, 22, 23 }, isLooping: false));
            playerFactory.Add("left", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }));
            playerFactory.Add("down", new SpriteSheetAnimationData(new[] { 4, 5, 6, 7 }));
            playerFactory.Add("up_left", new SpriteSheetAnimationData(new[] { 8, 9, 10, 11 }));
            playerFactory.Add("down_left", new SpriteSheetAnimationData(new[] { 12, 13, 14, 15 }));


            player.AddComponent(new AnimatedSpriteComponent(playerFactory)
            {
                CurrentAnimation = "right"
            });
            player.AddComponent(new CollidableComponent { BoundingRectangle = new RectangleF(startLocation.X, startLocation.Y, 64, 96) });
            player.AddComponent(new BasicControllerComponent());
            player.AddComponent(new AIComponent { IsPlayerControlled = true });
            player.AddComponent(new AnimationStateInfoComponent { CurrentAnimationName = "idle_down" });
            return player;
        }

        public RogueEntity CreateKnightAlly(Vector2 location, Vector2 followLocation, Vector2 followDistance, RogueEntity player)
        {
            RogueEntity ally = RogueEntity.CreateNew();
            var playerTex = Content.Load<Texture2D>("Assets/Walking_sm");
            ally.AddComponent(new PositionComponent { Position = location, Speed = .25f });
            //ally.AddComponent(new SpriteComponent { Texture = playerTex, Size = new Point(64, 96) });
            ally.AddComponent(new CollidableComponent { BoundingRectangle = new RectangleF(location.X, location.Y, 64, 96) });
            ally.AddComponent(
                new AIComponent
                {
                    IsPlayerControlled = false,
                    IsAlly = true,
                    Faction = "Player"
                });
            ally.AddComponent(
                new FollowComponent
                {
                    FollowTarget = player.ID,
                    FollowDistance = followDistance,
                    FollowTargetLocation = new RectangleF(followLocation.X, followLocation.Y, 62, 96)
                });
           
            var playerTexture = Content.Load<Texture2D>("Assets/KnightIsoChar");
            var playerAtlas = TextureAtlas.Create("Animations/Player", playerTexture, 84, 84);
            var playerFactory = new SpriteSheetAnimationFactory(playerAtlas);
            playerFactory.Add("idle_down", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }));
            playerFactory.Add("down", new SpriteSheetAnimationData(new[] { 4, 5, 6, 7, 8 }, isLooping: true));
            playerFactory.Add("up", new SpriteSheetAnimationData(new[] { 9, 10, 11, 12, 13 }, isLooping: true));
            playerFactory.Add("idle_up", new SpriteSheetAnimationData(new[] { 29 }));
            playerFactory.Add("idle_right", new SpriteSheetAnimationData(new[] { 14 }));
            playerFactory.Add("right", new SpriteSheetAnimationData(new[] { 15, 16, 17, 18, 19 }, isLooping: true));
            playerFactory.Add("idle_left", new SpriteSheetAnimationData(new[] { 20 }));
            playerFactory.Add("left", new SpriteSheetAnimationData(new[] { 21, 22, 23, 24, 25 }, isLooping: true));
            playerFactory.Add("atk_down", new SpriteSheetAnimationData(new[] { 27, 28 }, isLooping: false));
            playerFactory.Add("atk_up", new SpriteSheetAnimationData(new[] { 30, 31 }, isLooping: false));
            playerFactory.Add("atk_right", new SpriteSheetAnimationData(new[] { 34, 33 }, isLooping: false));
            playerFactory.Add("atk_left", new SpriteSheetAnimationData(new[] { 37, 36 }, isLooping: false));

            ally.AddComponent(new AnimatedSpriteComponent(playerFactory)
            {
                CurrentAnimation = "idle_down"
            });
            ally.AddComponent(new AnimationStateInfoComponent { CurrentAnimationName = "idle_down" });
            
            return ally;
        }

        private RogueEntity CreatePlayerKnight()
        {
            RogueEntity player = RogueEntity.CreateNew();
            Vector2 startLocation = new Vector2(1000, 500);
            player.AddComponent(new PositionComponent { Position = startLocation, Speed = .25f });
            //player.AddComponent(new SpriteComponent { Texture = playerTex, Size = new Point(64, 96) });
            var playerTexture = Content.Load<Texture2D>("Assets/KnightIsoChar");
            var playerAtlas = TextureAtlas.Create("Animations/Player", playerTexture, 84, 84);
            var playerFactory = new SpriteSheetAnimationFactory(playerAtlas);
            playerFactory.Add("idle_down", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3 }));
            playerFactory.Add("down", new SpriteSheetAnimationData(new[] { 4, 5, 6, 7, 8 }, isLooping: true));
            playerFactory.Add("up", new SpriteSheetAnimationData(new[] { 9, 10, 11, 12, 13 }, isLooping: true));
            playerFactory.Add("idle_up", new SpriteSheetAnimationData(new[] { 29 }));
            playerFactory.Add("idle_right", new SpriteSheetAnimationData(new[] { 14 }));
            playerFactory.Add("right", new SpriteSheetAnimationData(new[] { 15, 16, 17, 18, 19 }, isLooping: true));
            playerFactory.Add("idle_left", new SpriteSheetAnimationData(new[] { 20 }));
            playerFactory.Add("left", new SpriteSheetAnimationData(new[] { 21, 22, 23, 24, 25 }, isLooping: true));
            playerFactory.Add("atk_down", new SpriteSheetAnimationData(new[] { 27, 28 }, isLooping: false));
            playerFactory.Add("atk_up", new SpriteSheetAnimationData(new[] { 30, 31 }, isLooping: false));
            playerFactory.Add("atk_right", new SpriteSheetAnimationData(new[] { 34, 33 }, isLooping: false));
            playerFactory.Add("atk_left", new SpriteSheetAnimationData(new[] { 37, 36 }, isLooping: false));

            player.AddComponent(new AnimatedSpriteComponent(playerFactory)
            {
                CurrentAnimation = "idle_down"
            });
            player.AddComponent(new CollidableComponent { BoundingRectangle = new RectangleF(startLocation.X, startLocation.Y, 64, 96) });
            player.AddComponent(new BasicControllerComponent());
            player.AddComponent(new AIComponent { IsPlayerControlled = true });
            player.AddComponent(new AnimationStateInfoComponent { CurrentAnimationName = "idle_down" });

            return player;
        }

        public RogueEntity CreateAlly(Vector2 location, Vector2 followLocation, Vector2 followDistance, RogueEntity player)
        {
            RogueEntity ally = RogueEntity.CreateNew();
            var playerTex = Content.Load<Texture2D>("Assets/Walking_sm");
            ally.AddComponent(new PositionComponent { Position = location, Speed = .25f });
            ally.AddComponent(new SpriteComponent { Texture = playerTex, Size = new Point(64, 96) });
            ally.AddComponent(new CollidableComponent { BoundingRectangle = new RectangleF(location.X, location.Y, 64, 96) });
            ally.AddComponent(
                new AIComponent
                {
                    IsPlayerControlled = false,
                    IsAlly = true,
                    Faction = "Player"
                });
            ally.AddComponent(
                new FollowComponent
                {
                    FollowTarget = player.ID,
                    FollowDistance = followDistance,
                    FollowTargetLocation = new RectangleF(followLocation.X, followLocation.Y, 62, 96)
                });

            return ally;
        }

    }
}
