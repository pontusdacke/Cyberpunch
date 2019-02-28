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
    class E_Punch : GameAction
    {
        private bool firstExecute = true;

        public E_Punch()
        {
            ValidActions.Add(typeof(A_Jump));
            ValidActions.Add(typeof(A_KnockedBack));
        }

        public override void Execute(Character ch, GameTime gameTime)
        {
            if (firstExecute)
            {
                ch.animator.ChangeAnimation("Attack");
                if (!ch.animator.FlipX)
                {
                    ch.attackBox = new Attack(new Point(100, 85), 75, 50, 20, new Vector2(2, 2), new Point(85, 100), ch, new Point(0, -85));
                }
                else
                {
                    ch.attackBox = new Attack(new Point(100, 85), 75, 50, 20, new Vector2(-2, 2), new Point(85, 100), ch, new Point(0, -85));
                }
                firstExecute = false;
            }
            else if (!ch.attackBox.Valid)
            {
                ch.animator.ChangeAnimation("Idle");
                firstExecute = true;
                ch.attackBox = null;
                ch.EndAction();
            }
        }

        public override void InteruptedAction(Cyberpunch_game.Character ch, Microsoft.Xna.Framework.GameTime gameTime)
        {
            firstExecute = true;
            ch.attackBox = null;
            ch.EndAction();
        }

        public override bool Valid(Type input)
        {
            if (base.Valid(input))
            {
                firstExecute = true;
                return true;
            }
            return false;
        }
    }
}
