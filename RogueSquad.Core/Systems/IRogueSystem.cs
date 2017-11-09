using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using RogueSquad.Core.Components;

namespace RogueSquad.Core.Systems
{

    public interface IRogueSystem 
    {
        void Update(GameTime gametime);
       
        void AddEntity(RogueEntity entity);
//        void RemoveEntity(RogueEntity entity);
        ComponentTypes[] DesiredComponentsTypes { get; set; }
    }

}