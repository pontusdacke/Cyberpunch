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
    public enum EnemyAction
    {
        MoveRight,
        MoveLeft,
        Jump, 
        Idle,
        Punch
    }
    public class Enemy : Character
    {

    #region Variables

        Player player;
        List<Character> enemyList;
        List<Character> parentsList; //Referens till listan som spawnat den
        EnemyAI ai;
        List<GameAction> guiltyActions;

    #endregion

        #region Constructors

        /// <summary>
        /// Fiende klassen
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Position">Vart fienden ska starta</param>
        /// <param name="AnimationPath"></param>
        /// <param name="EntityName"></param>
        /// <param name="startAnimation"></param>
        /// <param name="P1">Spelaren måste skickas in så att fienden vet vart han är</param>
        /// <param name="EnemyList">Skicka in listan med fiender så att de kan samarbeta</param>
        public Enemy(Game game, Vector2 Position, string AnimationPath, string EntityName, string startAnimation, Player P1, ref List<Character> EnemyList, float Scale, List<Character> parentsList)
            : base(game,Position,AnimationPath,EntityName,startAnimation, Scale, new Vector2(100,150))
        {
            health = 80;
            tocheck = new Vector2(Position.X, Position.Y);
            player = P1;
            enemyList = EnemyList;

            guiltyActions = new List<GameAction>();

            // Match with EnemyEnum
            guiltyActions.Add(new A_Move(false));
            guiltyActions.Add(new A_Move(true));
            guiltyActions.Add(new A_Jump(true));
            guiltyActions.Add(new A_Idle());
            guiltyActions.Add(new E_Punch());
             
            ai = new EnemyAI(this, guiltyActions, player, ref enemyList);
            currentAction = new A_Idle();
            this.parentsList = parentsList;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            actionQueue = ai.ExecuteLogic(actionQueue);

            if (health <= 0)
                Remove();

            base.Update(gameTime);
        }

        public void Remove()
        {
            enemyList.Remove(this);
            Game.Components.Remove(this);
            parentsList.Remove(this);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

       
    }
}
