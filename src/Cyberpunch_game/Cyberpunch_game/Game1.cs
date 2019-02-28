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
using System.IO;

namespace Cyberpunch_game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Public declaration of an enumeration that is used to control the state of the game
        public enum GameState
        {
            Undefined,
            LoadingScreen,
            MainMenu,
            Game,
            Paused,
            GameOver,
            GameWon,
            ResetToMainMenu,
            Exit,
            Settings
        };
        public static GameState CurrentGameState = GameState.MainMenu;

        // The connected players index
        public static PlayerIndex connectedIndex = PlayerIndex.One;
        
        InputManager inputManager;
        #region Input related variables
        List<KeyActionRelation> keyActionRelations;
        #endregion
        GameHandler gameHandler;
        Menu menu;

        KeyboardState lastKeyboardState;
        GamePadState lastGamePadState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
#if XBOX
            SettingsManager.InitializeFiles();
#endif
            for (PlayerIndex i = PlayerIndex.One; i < PlayerIndex.Four; i++)
            {
                GamePadState state = GamePad.GetState(i);
                if (state.IsConnected)
                {
                    connectedIndex = i;
                    break;
                }
            }

            menu = new Menu(this, connectedIndex); // Create a menu
            gameHandler = new GameHandler(this); // Create a gamehandler (an instance of the game)
            keyActionRelations = new List<KeyActionRelation>();
            inputManager = new InputManager(this, menu, keyActionRelations);
            inputManager.ActionList = GetActiveInputList();
            InitializeInput();
            Components.Add(inputManager);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        protected override void Update(GameTime gameTime)
        {
            #region GameState switch
            switch (CurrentGameState)
            {
                case GameState.LoadingScreen:
                    #region Loading Screen
                    #endregion
                    break;
                case GameState.MainMenu:
                    #region MainMenu
                    if (!Components.Contains(menu))
                    {
                        Components.Add(menu);
                        menu.EnableMainMenu();
                        inputManager.controllable = menu;
                        inputManager.ActionList = GetActiveInputList();
                    }
                    #endregion
                    break;
                case GameState.Game:
                    #region Game
                    if (Components.Contains(menu))
                    {
                        menu.DisableMenu();
                        Components.Remove(menu);
                    }
                    if (!Components.Contains(gameHandler))
                    {
                        Components.Add(gameHandler);
                        inputManager.controllable = gameHandler.Player;
                        inputManager.ActionList = GetActiveInputList();
                    }
                    gameHandler.Play();
                    break;
                    #endregion
                case GameState.Paused:
                    #region Paused
                    gameHandler.Pause();
                    if (Components.Contains(gameHandler))
                        Components.Remove(gameHandler);

                    if (!Components.Contains(menu))
                    {
                        Components.Add(menu);
                        menu.EnablePauseMenu();
                        inputManager.controllable = menu;
                        inputManager.ActionList = GetActiveInputList();
                    }

                    break;
                    #endregion
                case GameState.GameOver:
                    #region GameOver
                    if (!Components.Contains(menu))
                    {
                        Components.Add(menu);
                        menu.EnablePauseMenu();
                    }
                    if (Components.Contains(gameHandler)) Components.Remove(gameHandler);
                    gameHandler.Pause();
                    gameHandler = new GameHandler(this); // Load new game
                    CurrentGameState = GameState.MainMenu;
                    menu.EnableLoseMenu();
                    inputManager.controllable = menu;
                    inputManager.ActionList = GetActiveInputList();
                    break;
                    #endregion
                case GameState.GameWon:
                    #region GameWon
                    if (!Components.Contains(menu))
                    {
                        Components.Add(menu);
                        menu.EnablePauseMenu();
                    }
                    if (Components.Contains(gameHandler)) Components.Remove(gameHandler);
                    gameHandler.Pause();
                    gameHandler = new GameHandler(this); // Load new game
                    CurrentGameState = GameState.MainMenu;
                    menu.EnableWinMenu();
                    inputManager.controllable = menu;
                    inputManager.ActionList = GetActiveInputList();
                    break;
                    #endregion
                case GameState.ResetToMainMenu:
                    #region ResetToMainMenu
                    gameHandler = new GameHandler(this); // Load new game
                    CurrentGameState = GameState.MainMenu;
                    menu.EnableMainMenu();
                    break;
                    #endregion
                case GameState.Exit:
                    #region Exit
                    this.Exit();
                    break;
                    #endregion
                default:
                    break;
            }
            #endregion
            
            #region Global Hotkeys
            #region Keyboard
            // Escape to Exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && lastKeyboardState.IsKeyUp(Keys.Escape))
            {
                if (CurrentGameState == GameState.MainMenu) this.Exit();
            }
            // F12 toggle fullscreen
            if (Keyboard.GetState().IsKeyDown(Keys.F12) && lastKeyboardState.IsKeyUp(Keys.F12))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }
            // PrintScreen to print the screen
            if (Keyboard.GetState().IsKeyDown(Keys.PrintScreen) && lastKeyboardState.IsKeyUp(Keys.PrintScreen))
            {
                int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
                int h = GraphicsDevice.PresentationParameters.BackBufferHeight;

                // Force a frame to be drawn (otherwise back buffer is empty) 
                Draw(new GameTime());

                // Pull the picture from the buffer 
                int[] backBuffer = new int[w * h];
                GraphicsDevice.GetBackBufferData(backBuffer);

                // Save texture to .png file 
                using (Stream stream = File.OpenWrite("Screenshot/" + DateTime.UtcNow.ToString().Replace(':', ' ') + ".png"))
                {
                    using (Texture2D texture = new Texture2D(GraphicsDevice, w, h, false, GraphicsDevice.PresentationParameters.BackBufferFormat))
                    {
                        texture.SetData(backBuffer);
                        texture.SaveAsPng(stream, w, h);
                    }
                }
            }
            #endregion
            #region GamePad
            #endregion
            #region Keyboard and GamePad

            #endregion
            #endregion

            lastGamePadState = GamePad.GetState(PlayerIndex.One);
            lastKeyboardState = Keyboard.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
        private void InitializeInput()
        {
            KeyboardSettings keysettings = SettingsManager.Load("settings.xml").KeyboardSettings;
            Dictionary<Buttons, Keys> keyMapping = SettingsManager.GetKeyboardDictionary(keysettings);
            if (GamePad.GetState(Game1.connectedIndex).IsConnected)
                inputManager = new InputManager(this, menu, keyActionRelations); // Attach a inputhandler to the player
            else
                inputManager = new InputManager(this, menu, keyActionRelations, keyMapping); // Attach a inputhandler to the player
        }
        private List<KeyActionRelation> GetActiveInputList()
        {
            GamePadCapabilities gpc = GamePad.GetCapabilities(connectedIndex);
            List<KeyActionRelation> actionList = new List<KeyActionRelation>();
            if (CurrentGameState == GameState.MainMenu || CurrentGameState == GameState.Paused)
            {
                // Base Actions for all controllers
                actionList.Add(new KeyActionRelation() { Action = new GameAction() { ActionName = "Select", Direction = new Vector2(0, 0), HitFrame = -1 }, Button = Buttons.A, Down = false, Pressed = true });
                // Actions for Arcade Stick or other controllers without thumbsticks
                if (gpc.GamePadType == GamePadType.ArcadeStick
                    || (!gpc.HasRightXThumbStick && !gpc.HasRightYThumbStick))
                {
                    actionList.Add(new KeyActionRelation() { Action = new GameAction() { ActionName = "Up", Direction = new Vector2(0, -1), HitFrame = -1 }, Button = Buttons.DPadUp, Down = false, Pressed = true });
                    actionList.Add(new KeyActionRelation() { Action = new GameAction() { ActionName = "Down", Direction = new Vector2(0, 1), HitFrame = -1 }, Button = Buttons.DPadDown, Down = false, Pressed = true });
                }
                else // Actions for GamePad
                {
                    actionList.Add(new KeyActionRelation() { Action = new GameAction() { ActionName = "Up", Direction = new Vector2(0, -1), HitFrame = -1 }, Button = Buttons.LeftThumbstickUp, Down = false, Pressed = true });
                    actionList.Add(new KeyActionRelation() { Action = new GameAction() { ActionName = "Down", Direction = new Vector2(0, 1), HitFrame = -1 }, Button = Buttons.LeftThumbstickDown, Down = false, Pressed = true });
                }                
            }
            else if (CurrentGameState == GameState.Game)
            {
                // Base Actions for all controllers
                actionList.Add(new KeyActionRelation() { Action = new A_Jump(true), Button = Buttons.B, Down = false, Pressed = true });
                actionList.Add(new KeyActionRelation() { Action = new A_Punch(), Button = Buttons.A, Down = false, Pressed = true });
                actionList.Add(new KeyActionRelation() { Action = new A_Booster(this), Button = Buttons.X, Down = true, Pressed = false });
                actionList.Add(new KeyActionRelation() { Action = new GameAction() { GameState = GameState.Paused }, Button = Buttons.Start, Down = true, Pressed = false });
                // Actions for Arcade Stick or other controllers without thumbsticks
                if (gpc.GamePadType == GamePadType.ArcadeStick
                    || (!gpc.HasRightXThumbStick && !gpc.HasRightYThumbStick))
                {
                    actionList.Add(new KeyActionRelation() { Action = new A_Move(true), Button = Buttons.DPadLeft, Down = true, Pressed = false });
                    actionList.Add(new KeyActionRelation() { Action = new A_Move(false), Button = Buttons.DPadRight, Down = true, Pressed = false });
                    actionList.Add(new KeyActionRelation() { Action = new A_ChangeDirAir(true), Button = Buttons.DPadUp, Down = false, Pressed = true });
                    actionList.Add(new KeyActionRelation() { Action = new A_ChangeDirAir(false), Button = Buttons.DPadDown, Down = false, Pressed = true });
                }
                else // Actions for GamePad
                {
                    actionList.Add(new KeyActionRelation() { Action = new A_Move(true), Button = Buttons.LeftThumbstickLeft, Down = true, Pressed = false });
                    actionList.Add(new KeyActionRelation() { Action = new A_Move(false), Button = Buttons.LeftThumbstickRight, Down = true, Pressed = false });
                    actionList.Add(new KeyActionRelation() { Action = new A_ChangeDirAir(true), Button = Buttons.LeftThumbstickUp, Down = false, Pressed = true });
                    actionList.Add(new KeyActionRelation() { Action = new A_ChangeDirAir(false), Button = Buttons.LeftThumbstickDown, Down = false, Pressed = true });
                }
            }

            return actionList;
        }
    }
}
