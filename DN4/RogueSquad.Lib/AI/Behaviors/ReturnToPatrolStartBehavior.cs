using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using RogueSquad.Core.Components;
using MonoGame.Extended;


namespace RogueSquad.Core.AI.Behaviors
{
    public class ReturnToPatrolStartBehavior : AIBehavior
    {

        PatrolComponent _pc;
        CollidableComponent _cc;
        PositionComponent _po;
        public ReturnToPatrolStartBehavior(PatrolComponent pc, CollidableComponent cc, PositionComponent po)
        {

            _pc = pc;
            _cc = cc;
            _po = po;
        }

        //TODO: we need to both make the reset work better and move the AI back to the absolute center hopefully without multiple collision checks
        //TODO: we should find a way to avoid collision checking when the ai has been reset or is 'inactive'
        public void Update(GameTime gameTime)
        {
            var pCenter = new RectangleF(_pc.PatrolArea.Center, new Size2(2, 2));

            if (!_cc.BoundingRectangle.Intersects(pCenter))
            {
                MoveToStart(pCenter, gameTime);
                _pc.IsReset = false;
            }
            else
            {
                //arrived
                _pc.IsReset = true;
            }
        }
        private void MoveToStart(RectangleF center, GameTime gametime)
        {
            Vector2 newPos = _po.Position;
            if (center.Position.X > _po.Position.X)
            {
                //move player 
                newPos.X += _po.Speed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (center.Position.X < _po.Position.X)
            {
                //move player 
                newPos.X -= _po.Speed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (center.Position.Y > _po.Position.Y)
            {
                //move player 
                newPos.Y += _po.Speed * gametime.ElapsedGameTime.Milliseconds;
            }
            if (center.Position.Y < _po.Position.Y)
            {
                //move player 
                newPos.Y -= _po.Speed * gametime.ElapsedGameTime.Milliseconds;
            }

            _po.Position = newPos;
        }


    }
}
