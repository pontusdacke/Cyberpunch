using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberpunch_game
{
    class Menu : DrawableGameComponent, IControllable
    {
        Texture2D background;
        List<Button> currentButtons;
        SpriteBatch spriteBatch;
        int SCRW, SCRH;
        int currentIndex;
        KeyboardState lastState;
        public PlayerIndex PlayerIndex { get; set; }

        // Buttons
        Button play;
        Button settings;
        Button exit;
        Button resume;
        Button quit_game;

        List<object> components = new List<object>(); // Keep track of all components

        public Menu(Game Game, PlayerIndex PlayerIndex)
            : base(Game)
        {
            currentButtons = new List<Button>();
            this.PlayerIndex = PlayerIndex;
            this.SCRW = Game.GraphicsDevice.Viewport.Width;
            this.SCRH = Game.GraphicsDevice.Viewport.Height;

            //Buttons
            play = new Button("play", Game1.GameState.Game, Game);
            settings = new Button("settings", Game1.GameState.Settings, Game);
            exit = new Button("exit", Game1.GameState.Exit, Game);
            resume = new Button("resume", Game1.GameState.Game, Game);
            quit_game = new Button("main menu", Game1.GameState.ResetToMainMenu, Game);
            
            // Add all buttons for proper initialization ( Initialize() and LoadContent() )
            currentButtons.Add(play);
            components.Add(play);

            currentButtons.Add(settings);
            components.Add(settings);

            currentButtons.Add(exit);
            components.Add(exit);

            currentButtons.Add(resume);
            components.Add(resume);
#if WINDOWS
            currentButtons.Add(quit_game);
            components.Add(quit_game);
#endif
        }
        public void DisableMenu()
        {
            foreach (Button b in currentButtons)
            {
                Game.Components.Remove(b);
            }
        }
        public void EnablePauseMenu()
        {
            background = Game.Content.Load<Texture2D>("Menu/Background");
            DisableMenu(); // clears current buttons from component
            currentButtons.Clear();
            currentButtons.Add(resume);
            currentButtons.Add(settings);
            currentButtons.Add(quit_game);
            SetSelectedButton(resume);
            foreach (Button b in currentButtons)
            {
                Game.Components.Add(b);
            }
        }
        public void EnableMainMenu()
        {
            background = Game.Content.Load<Texture2D>("Menu/Background");
            DisableMenu(); // clears current buttons from component
            currentButtons.Clear(); // clears current buttons
            currentButtons.Add(play); // add buttons
            currentButtons.Add(settings);
            currentButtons.Add(exit);
            SetSelectedButton(play);
            foreach (Button b in currentButtons)
            {
                Game.Components.Add(b);
            }
        }
        public void EnableLoseMenu()
        {
            background = Game.Content.Load<Texture2D>("Menu/Background_gameover");
            DisableMenu(); // clears current buttons from component
            currentButtons.Clear(); // clears current buttons

            Button reset = new Button("reset", Game1.GameState.Game, Game);

            currentButtons.Add(reset); // add buttons
            currentButtons.Add(quit_game);
            SetSelectedButton(reset);
            foreach (Button b in currentButtons)
            {
                Game.Components.Add(b);
            }
        }
        public void EnableWinMenu()
        {
            background = Game.Content.Load<Texture2D>("Menu/Background_victory");
            DisableMenu(); // clears current buttons from component
            currentButtons.Clear(); // clears current buttons

            Button reset = new Button("reset", Game1.GameState.Game, Game);

            currentButtons.Add(reset); // add buttons
            currentButtons.Add(quit_game);
            SetSelectedButton(reset);
            foreach (Button b in currentButtons)
            {
                Game.Components.Add(b);
            }
        }
        public override void Initialize()
        {
            foreach (Button b in currentButtons)
            {
                b.Initialize();
            }
            base.Initialize();
        }
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Game.Content.Load<Texture2D>("Menu/Background");
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            foreach (Button b in currentButtons)
            {
                b.Update(gameTime);
            }
            base.Update(gameTime);
        }
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (background != null)
                spriteBatch.Draw(background, new Rectangle(0, 0, SCRW, SCRH), Color.White);
            spriteBatch.End();
            if (currentButtons.Count > 0)
            {
                for (int i = 0; i < currentButtons.Count; i++)
                {
                    currentButtons[i].Draw(gameTime, new Vector2(20, GetButtonStartHeight() + (currentButtons[i].FontHeight * currentButtons[i].GetScaleToWindow() * 0.7f * i)));
                }
            }
            base.Draw(gameTime);
        }

        private void SetSelectedButton(int index)
        {
            for (int i = 0; i < currentButtons.Count; i++)
            {
                currentButtons[i].Selected = false;
            }
            currentButtons[index].Selected = true;
            currentIndex = index;

        }
        private void SetSelectedButton(Button button)
        {
            foreach (Button b in currentButtons)
                b.Selected = false;
            button.Selected = true;
            currentIndex = currentButtons.IndexOf(button);

        }
        private float GetButtonStartHeight()
        {
            return GraphicsDevice.Viewport.Height * 0.652f; // Percentual value where the buttons should be related to the picture. Hardcode of doom
        }

        public void SetInput(GameAction Action)
        {
            currentIndex += (int)Action.Direction.Y;
            if (Action.GameState != Game1.GameState.Undefined) 
                Game1.CurrentGameState = Action.GameState;
            if (currentIndex >= currentButtons.Count) currentIndex = currentButtons.Count -1;
            if (currentIndex < 0) currentIndex = 0;
            if (Action.ActionName == "Select")
            {
                Game1.CurrentGameState = currentButtons[currentIndex].State;
            }
            SetSelectedButton(currentIndex);

        }
    }
}
