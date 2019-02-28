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
    public class EnemyAI
    {

        #region Variables
        Player player;
        List<Character> enemyList;
        Enemy enemy;
        List<GameAction> guiltyActions;
        #endregion

        #region Constructors
        public EnemyAI(Enemy Enemy, List<GameAction> GuiltyActions, Player Player, ref List<Character> EnemyList)
        {
            player = Player;
            enemyList = EnemyList;
            enemy = Enemy;
            guiltyActions = GuiltyActions;
        }
        #endregion

        public Queue<GameAction> ExecuteLogic(Queue<GameAction> queue)
        {
            List<Rectangle> collidingBlocks = new List<Rectangle>();
            GameAction nextAction = FollowIfInRange((int)(player.rectSize.X * 1.5));
            if (nextAction is A_Idle)
                nextAction = Punch();

            if (nextAction == WalkLeft())
            {
                enemy.tocheck.X += -1;
                collidingBlocks = enemy.collision.testCollision();
            }
            if (nextAction == WalkRight())
            {
                enemy.tocheck.X += 1;
                collidingBlocks = enemy.collision.testCollision();
            }
            if (collidingBlocks != null && collidingBlocks.Count > 0 && enemy.onGround == true)
            {
                nextAction = Jump();
            }

            queue.Enqueue(nextAction);

            return queue;
        }
        public GameAction WalkTowards()
        {
            // 2 pixel offset to prevent stuttering
            if (enemy.Position.X > player.Position.X + 1)
                return guiltyActions[(int)EnemyAction.MoveLeft]; // left
            else if (enemy.Position.X < player.Position.X - 1)
                return guiltyActions[(int)EnemyAction.MoveRight]; // right
            else return Punch();
        }
        public GameAction WalkTowards(Character Character, Character Target)
        {
            if (Character.Position.X > Target.Position.X)
                return guiltyActions[1];
            else if (Character.Position.X < Target.Position.X)
                return guiltyActions[0];
            else return guiltyActions[3];
        }
        public GameAction FollowIfInRange(int Range)
        {
            return FollowIfInRange(Range, enemy, player);
        }
        public GameAction FollowIfInRange(int Range, Character Character, Character Target)
        {
            if (Math.Abs(Vector2.Distance(Character.Position, Target.Position)) > Math.Abs(Range))
            {
                return WalkTowards(Character, Target);
            }
            else
            {
                return guiltyActions[3];
            }
        }
        public GameAction GetOnMyLevel()
        {
            //Fuckthisshit
            return guiltyActions[2]; // 2 = jump
        }
        #region Core Actions
        public GameAction WalkRight()
        {
             return guiltyActions[(int)EnemyAction.MoveRight];
        }
        public GameAction WalkLeft()
        {
            return guiltyActions[(int)EnemyAction.MoveLeft];
        }
        public GameAction Jump()
        {
            return guiltyActions[(int)EnemyAction.Jump];
        }
        public GameAction Punch()
        {
            return guiltyActions[(int)EnemyAction.Punch];
        }
        public GameAction Idle()
        {
            return guiltyActions[(int)EnemyAction.Idle];
        }
        #endregion

    }
}
