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
    class A_Punch : GameAction
    {
        private bool firstExecute = true;

        public A_Punch()
        {
            ValidActions.Add(typeof(A_Jump));
            ValidActions.Add(typeof(A_Booster));
            ValidActions.Add(typeof(A_KnockedBack));           
        }

        public override void Execute(Character ch, GameTime gameTime)
        {
            if (firstExecute)
            {
                if (ch.onGround)
                {
                    Random rnd = new Random();
                    if (rnd.Next(0, 2) == 1)
                        ch.animator.ChangeAnimation("Punch");
                    else
                        ch.animator.ChangeAnimation("Punch_2");
                }
                else
                    ch.animator.ChangeAnimation("Jump_kick");
                if (!ch.animator.FlipX)
                {
                    ch.attackBox = new Attack(new Point(50, 30), 15, 8, 20, new Vector2(0, 0), new Point(0, 0), ch, new Point(90, 0));
                }
                else
                {
                    ch.attackBox = new Attack(new Point(50, 30), 15, 8, 20, new Vector2(0, 0), new Point(0, 0), ch, new Point(-50, 0));
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
