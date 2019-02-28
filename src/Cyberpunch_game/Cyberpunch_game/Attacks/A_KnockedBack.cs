using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cyberpunch_game
{
    class A_KnockedBack : GameAction
    {
        int duration;
        Vector2 direction = new Vector2();

        public A_KnockedBack(int duration, Vector2 hit_source, Character target)
        {
            this.duration = duration;
            if (hit_source.X < target.Position.X)
                direction.X = 1;
            else
                direction.X = -1;
        }

        public override void Execute(Character ch, GameTime gameTime)
        {
            if (duration-- <= 0)
                ch.EndAction();
            else
            {
                ch.tocheck = ch.Position + (direction * 12);    //detta är vart man försöker att röra sig 
                List<Rectangle> collidingBlocks = ch.collision.testCollision();
                List<Character> collidingEnemysChar = ch.collision.enemyCollisionChar();

                if (collidingBlocks == null && collidingEnemysChar == null)   //om man inte kolliderar med något
                {
                    ch.Position = ch.Position + (direction * 12);
                }
                else
                {
                    bool hejhejehejehej = false; //fixa senare, dummy bool bör inte finnas borde ändra metoden
                    if (collidingBlocks != null)
                        ch.Position = WallCollision.GetWallCollision(collidingBlocks, gameTime, ch.GetNextHitRect, ch.tocheck, out ch.onGround);
                    if (collidingEnemysChar != null)
                    {
                        List<Rectangle> collEnemyRect = new List<Rectangle>();
                        foreach (Character chara in collidingEnemysChar)
                        {
                            collEnemyRect.Add(chara.GetNextHitRect);
                            chara.SetInput(new A_KnockedBack(15, new Vector2(ch.Position.X, ch.Position.Y), chara));
                        }
                        ch.Position = WallCollision.GetWallCollision(collEnemyRect, gameTime, ch.GetNextHitRect, ch.tocheck, out hejhejehejehej);
                    }
                    ch.tocheck = new Vector2(ch.Position.X, ch.Position.Y);  //sätter tocheck till samma pos som position
                }

            }
        }
    }
}
