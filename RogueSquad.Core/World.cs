using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Systems;
using RogueSquad.Core.Components;

namespace RogueSquad.Core
{

    public sealed class World
    {      
        public List<IRogueSystem> Systems { get; set; }
        public List<IRogueRenderSystem> RenderSystems { get; set; }
        public List<RogueEntity> Entities { get; set; }

        public World()
        {
           
            Systems = new List<IRogueSystem>();
            RenderSystems = new List<IRogueRenderSystem>();
            Entities = new List<RogueEntity>();
        }

        public int EntityCount => Entities.Count;

        public void RegisterSystem(IRogueSystem system)
        {
            Systems.Add(system);
        }

        public void RegisterRenderSystem(IRogueRenderSystem system)
        {
            RenderSystems.Add(system);
        }

        public void AddEntity(RogueEntity entity)
        {

            AddEntityToSystems(entity);
            AddEntityToRenderSystems(entity);

            // add to world tracking as well - probably not needed in the future
            Entities.Add(entity);
        }

        public void RemoveEntity(RogueEntity entity)
        {
            // remove from each system

        }


        private void AddEntityToSystems(RogueEntity entity)
        {
            foreach (var sys in Systems)
            {
                //get matching components and add to system
                var hasTypes = sys.DesiredComponentsTypes.ToDictionary(x => x, y => false);

                foreach (var compType in sys.DesiredComponentsTypes)
                {
                    hasTypes[compType] = entity.HasComponent(compType);
                }

                if (hasTypes.All(x => x.Value))
                    sys.AddEntity(entity);
            }
            
        }

        private void AddEntityToRenderSystems(RogueEntity entity)
        {
            foreach (var sys in RenderSystems)
            {
                var hasTypes = sys.DesiredComponentsTypes.ToDictionary(x => x, y => false);
                foreach (var compType in sys.DesiredComponentsTypes)
                {
                    hasTypes[compType] = entity.HasComponent(compType);
                }

                if (hasTypes.All(x => x.Value))
                    sys.AddEntity(entity);
            }
        }


        public void UpdateEntity(RogueEntity entity)
        {
            //update entity's system subscriptions
        }


        public void Update(GameTime gameTime)
        {
            foreach (var system in Systems)
            {
                system.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var renderSystem in RenderSystems)
            {
                renderSystem.Draw(gameTime);
            }
        }

    }

}