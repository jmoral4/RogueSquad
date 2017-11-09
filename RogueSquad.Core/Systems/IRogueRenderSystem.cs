using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace RogueSquad.Core.Systems
{

    public interface IRogueRenderSystem : IRogueSystem
    {
        void Draw(GameTime gameTime);

    }

}