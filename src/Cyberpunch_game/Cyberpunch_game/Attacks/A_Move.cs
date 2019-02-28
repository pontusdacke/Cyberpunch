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
    class A_Move : GameAction
    {
        protected bool left;
        protected Vector2 direction2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left">Vilken riktning man ska röra sig, true är vänster, false höger</param>
        public A_Move(bool Left)
        {
            left = Left;

            if (left == true)
                direction2 = new Vector2(-1, 0);
                
            else
                direction2 = new Vector2(1, 0);

            ValidActions.Add(typeof(A_KnockedBack));
            ValidActions.Add(typeof(A_Punch)); 
            ValidActions.Add(typeof(A_Jump)); 
            ValidActions.Add(typeof(A_Idle));
            ValidActions.Add(typeof(A_Booster));
            ValidActions.Add(typeof(A_Move));
            ValidActions.Add(typeof(E_Punch));
        }

        public override void Execute(Character ch, GameTime gameTime)
        {
            //vänder karaktären
            if (direction2.X > 0)
                ch.animator.FlipX = false;
            else
                ch.animator.FlipX = true;

            if (ch.onGround == true)
            {
                ch.characterEngine.EmitterLocation = ch.animator.Location;
                ch.characterEngine.AddDustParticle(1);
                ch.animator.ChangeAnimation("Run");
            }
            ch.tocheck.X += direction2.X * ch.movespeed * (float)gameTime.ElapsedGameTime.TotalSeconds;    //detta är vart man försöker att röra sig 
            //List<Rectangle> templist = ch.collision.testCollision();   //kollar om man kolliderar när man försöker göra denna move

            List<Rectangle> collidingBlocks = ch.collision.testCollision();
            List<Rectangle> collidingEnemys = ch.collision.enemyCollision();

            if (collidingBlocks == null && collidingEnemys == null)   //om man inte kolliderar med något
            {

                ch.direction.X = direction2.X;
                ch.lastDirection.X = direction2.X;
                ch.lastDirection.Y = 0;
                ch.Position.X += direction2.X * ch.movespeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
               
                ch.direction.X = 0;
            }
            else
            {
                ch.direction.X = 0; /*direction2.X;*/   //nollställer direction om man inte kan röra sig dit man försöker att gå
                //ch.position = WallCollision.GetWallCollision(templist, gameTime, ch.GetNextHitRect, ch.tocheck, out ch.onGround);    //retunerar den nya positionen efter man kolliderat med en vägg

                bool hejhejehejehej = false; //fixa senare, dummy bool bör inte finnas borde ändra metoden
                if (collidingBlocks != null)
                    ch.Position = WallCollision.GetWallCollision(collidingBlocks, gameTime, ch.GetNextHitRect, ch.tocheck, out ch.onGround);
                if (collidingEnemys != null)
                    ch.Position = WallCollision.GetWallCollision(collidingEnemys, gameTime, ch.GetNextHitRect, ch.tocheck, out hejhejehejehej);

                ch.tocheck = new Vector2(ch.Position.X, ch.Position.Y);  //sätter tocheck till samma pos som position
            }
        }
        public override bool Valid(Type input)
        {
            return base.Valid(input);
        }

    }

}
