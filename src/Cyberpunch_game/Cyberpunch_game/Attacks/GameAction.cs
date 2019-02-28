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
    public class GameAction
    {
        public string   ActionName; // Describe the name of the action
        public Vector2  Direction;  // The directional force of the action
        public int      HitFrame;   // The frame in which hitcollision should be checked
        public Game1.GameState GameState;
        protected List<Type> ValidActions;

        public GameAction() 
        {
            ValidActions = new List<Type>();
        }

        public virtual void Execute(Character ch, GameTime gameTime) { }
        public virtual void InteruptedAction(Character ch, GameTime gameTime) { }
        public virtual bool Valid(Type input)
        {
            foreach (Type action in ValidActions)
                if (action == input) 
                    return true;
            return false;
        }


    }
}
