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
    class A_ChangeDirAir : GameAction
    {
        private bool up;
        private Vector2 direction;
        
        public A_ChangeDirAir(bool Up)
        {
            ValidActions.Add(typeof(A_KnockedBack));
            ValidActions.Add(typeof(A_Punch));
            ValidActions.Add(typeof(A_Booster));
            ValidActions.Add(typeof(A_Idle));
            ValidActions.Add(typeof(A_Jump));
            ValidActions.Add(typeof(A_Move));
            ValidActions.Add(typeof(E_Punch));
            ValidActions.Add(typeof(A_ChangeDirAir));
            
            up = Up;
            if (up)
                direction.Y = -1;
            else
                direction.Y = 1;
        }

        public override void Execute(Character ch, GameTime gameTime)
        {
            ch.lastDirection.Y = direction.Y;
            ch.lastDirection.X = 0;
        }
    }
}
