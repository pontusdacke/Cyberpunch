#region File Description
/* File: MenuPage.cs
 * Workers (By Initials): PM 
 * Description: Represents a menupage
 * Worktimes:
 * References:
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyberpunch_game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberpunch_game
{
    class MenuPage : DrawableGameComponent, IControllable
    {
        SpriteBatch spriteBatch;
        Texture2D backgroundTexture;
        List<Button> buttons;
        public Button SelectedButton;
        public int SelectedButtonIndex;
        public string Name;
        public PlayerIndex PlayerIndex { get; set; }

        public MenuPage(Game game, string name) : base(game)
        {
            this.Name = name;
            Button play = new Button("Play", "play", game);
            Button exit = new Button("Exit", "exit", game);
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public virtual void SetInput(GameAction Action)
        {
            if (Action.ActionName == "Confirm")
            {
                
            }
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 1280, 768), Color.White);
            spriteBatch.End();
        }
    }
}
