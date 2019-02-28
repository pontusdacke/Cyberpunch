using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Cyberpunch_game
{
    class A_Booster: GameAction
    {
        protected Vector2 direction2;
        protected float speed;

        public A_Booster(Game game)
        {
            ActionName = "booster";
            speed = 3;
            ValidActions.Add(typeof(A_KnockedBack));
            ValidActions.Add(typeof(A_Idle));
            ValidActions.Add(typeof(A_Move));
            ValidActions.Add(typeof(A_Booster));
        }
        public override void Execute(Character ch, GameTime gameTime)
        {
            if (ch.boostPower > 0)
            {
                Vector2 tempPos = ch.Position;  //tempvector för vart partiklarna skall ritas ut, ändras beroende på direktion

                if (ch.lastDirection.X > 0)  
                {
                    direction2.X = speed;
                    tempPos.Y += ch.rectSize.Y / 2;
                }
                if (ch.lastDirection.X < 0)
                {
                    direction2.X = -speed;
                    tempPos.Y += ch.rectSize.Y / 2;
                }
                if (ch.lastDirection.Y < 0)
                {
                    direction2.Y = -speed;
                    tempPos.Y += ch.rectSize.Y / 2;
                    tempPos.X += ch.rectSize.X / 2;
                }
                if (ch.lastDirection.Y > 0)
                {
                    direction2.Y = speed;
                    tempPos.Y += ch.rectSize.Y / 2;
                    tempPos.X += ch.rectSize.X / 2;
                }
                if (ch.direction.X != 0)
                {
                    ch.boosterEngine.EmitterLocation.X = tempPos.X;
                }
                else if (!ch.animator.FlipX)
                {
                    ch.boosterEngine.EmitterLocation.X = tempPos.X - 35;
                }
                else
                {
                    ch.boosterEngine.EmitterLocation.X = tempPos.X + 35;
                }
                ch.boosterEngine.EmitterLocation.Y = tempPos.Y - 100;
                ch.boosterEngine.AddBoosterParticle(5);
                ch.boostPower -= 5;

                ch.tocheck += direction2 * ch.movespeed * (float)gameTime.ElapsedGameTime.TotalSeconds;    //detta är vart man försöker att röra sig 
                List<Rectangle> collidingBlocks = ch.collision.testCollision();   //kollar om man kolliderar när man försöker göra denna move
                List<Rectangle> collidingEnemys = ch.collision.enemyCollision();

                if (collidingBlocks == null && collidingEnemys == null)   //om man inte kolliderar med något
                {
                    ch.Position += direction2 * ch.movespeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    if (collidingEnemys != null)
                    {
                        bool temptest = false;
                        ch.Position = WallCollision.GetWallCollision(collidingEnemys, gameTime, ch.GetNextHitRect, ch.tocheck, out temptest);    //retunerar den nya positionen efter man kolliderat med en vägg
                    }
                    if (collidingBlocks != null)
                    {
                        ch.Position = WallCollision.GetWallCollision(collidingBlocks, gameTime, ch.GetNextHitRect, ch.tocheck, out ch.onGround);    //retunerar den nya positionen efter man kolliderat med en vägg
                    }
                 
                    ch.tocheck.X = ch.Position.X;
                    ch.tocheck.Y = ch.Position.Y;
                }
                ch.direction = direction2;
                direction2 = Vector2.Zero;
               
            }
            else
            {
                ch.boostPower = -50;
            }
           
        }


    }
}
