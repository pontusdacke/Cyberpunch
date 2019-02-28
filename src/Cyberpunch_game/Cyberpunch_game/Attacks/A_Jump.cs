using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Cyberpunch_game
{
    class A_Jump : GameAction
    {
        protected bool up;
        protected Vector2 direction2;
        
        public A_Jump(bool Up)
        {
            up = Up;
            if (up == true)
                direction2 = new Vector2(0, -3);
            else
                direction2 = new Vector2(0, 6);
            ValidActions.Add(typeof(A_KnockedBack));
            ValidActions.Add(typeof(A_Booster));
            ValidActions.Add(typeof(A_Idle));
            ValidActions.Add(typeof(A_Move));
            ValidActions.Add(typeof(A_Jump));
        }

        public override void Execute(Character ch, GameTime gameTime)
        {
            ch.lastDirection.Y = direction2.Y;
            ch.lastDirection.X = 0;
            if (ch.onGround == true || ch.collision.externalInfo == true)
            {
                ch.characterEngine.EmitterLocation = ch.animator.Location;
                ch.characterEngine.AddDustParticle(40);
                ch.direction.Y = direction2.Y;
                ch.collision.externalInfo = false;
            }
        }
    }
}
