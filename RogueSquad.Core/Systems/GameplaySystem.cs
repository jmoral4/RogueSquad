using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Components;

namespace RogueSquad.Core.Systems
{
    public class GameplaySystem : IRogueSystem
    {
        public ComponentTypes[] DesiredComponentsTypes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AddEntity(RogueEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gametime)
        {
            throw new NotImplementedException();
        }
    }
}
