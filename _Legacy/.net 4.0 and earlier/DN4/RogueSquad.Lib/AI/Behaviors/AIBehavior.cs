using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace RogueSquad.Core.AI.Behaviors
{

    public interface AIBehavior
    {
        void Update(GameTime gameTime);
    }
}
