using Microsoft.Xna.Framework;
using RogueSquad.Core.Components;
using System;

namespace RogueSquad.Core.Systems
{
    public class RandomMovementSystem : IRogueSystem {
        public ComponentTypes[] RequiredComponents { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddEntity(RogueEntity entity)
        {
            throw new NotImplementedException();
        }
        public bool HasEntity(RogueEntity entity)
        {
         
            return false;
        }
        public void Update(GameTime gameTime)
        {
           

        }

    }

  

}
