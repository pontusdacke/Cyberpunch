using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;

namespace Cyberpunch_game
{
    class GameHandler : DrawableGameComponent
    {
        #region Variables
        bool paused = true;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue cue;

        // Character and Enemy related
        List<Character> enemyList = new List<Character>();
        public Player Player;
        EnemySpawnerHandler enemySpawnerHandler;

        // Map/Background related 
        Background background, background2, background3, background5, background6, background7;
        DrawObject backgroundInfo;
        Map map;

        // Collision, Camera
        Collision collision;
        Camera camera;

        // Components list
        List<object> components = new List<object>(); // Keep track of all components

        KeyboardState prevState;
        #endregion
        float rotationFactor = 0f;
        #region Massive constructor
        public GameHandler(Game Game) : base(Game)
        {
            // Initiatilizing
            this.map = new Map(Game);
            this.camera = new Camera(Game);

            // Character and enemy initialization
            this.Player = new Player(Game, new Vector2(4600, 3600), "Player/player_animations", "Player", "Idle", PlayerIndex.One, 0.31f);
            this.enemySpawnerHandler = new EnemySpawnerHandler(Game, enemyList, Player, camera, map, map.enemySpawnerPositionList, components);  //skapar spawners
            
            this.collision = new Collision(map, Player, Game, enemyList, enemySpawnerHandler.getEnemySpawnList); // Needs to be done after player is initiated, but before assigning collision to player
            this.Player.collision = collision;
            this.Player.camera = camera;
            this.enemyList.Add((Character)Player);

            map.camera = camera;

            // Background initializion
            background = new Background(Game, camera, new Vector2(0.2F, 0.02F), "Background/paper", 1, true, false, false, null);
            background6 = new Background(Game, camera, new Vector2(0.1F, 0.02F), "Background/background_city_1_cloud", 1, true, false, false, null);
            background7 = new Background(Game, camera, new Vector2(0.3F, 0.02F), "Background/background_city_2_clouds", 1, true, false, false, null);
            background2 = new Background(Game, camera, new Vector2(0.4F, 0.02F), "Background/background_clouds", 1, true, true, false, null);
            background5 = new Background(Game, camera, new Vector2(0.4F, 0.02F), "Background/background_test_mist", 1, true, true, false, null);
            backgroundInfo = new DrawObject(Game, new Vector2(4500, 3450), "Background/background_info");
            backgroundInfo.camera = camera;


            // Component storing (components is a list of objects)
            components.Add(background);
            components.Add(background6);
            components.Add(background7);
            components.Add(background3);
            components.Add(background2);
            components.Add(background5);
            components.Add(backgroundInfo);

            components.Add(map);
            components.Add(enemySpawnerHandler);   //ta bort när det inte fungerar
            components.Add(Player);
        }
        #endregion
        #region GameHandler Methods
        public void Play()
        {
            if (paused)
            {
                foreach (IGameComponent i in components)
                {
                    if(!Game.Components.Contains(i)) Game.Components.Add(i);
                }
                paused = false;
                cue.Resume();
            }
        }
        public void Pause()
        {
            if(!paused)
            {
                List<IGameComponent> removables = new List<IGameComponent>();
                foreach (IGameComponent i in components)
                {
                    if (Game.Components.Contains(i))
                        Game.Components.Remove(i);
                    else
                        removables.Add(i);             
                }
                foreach (IGameComponent i in removables)
                {
                    components.Remove(i);
                }
                paused = true;
                cue.Pause();
            }
        }
        #endregion
        #region GameComponent Methods
        public override void Initialize()
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].Initialize();
            }
            camera.Zoom = 0.1f;
            base.Initialize();
        }
        protected override void LoadContent()
        {
            audioEngine = new AudioEngine("Content/CyberpunchSound.xgs");
            waveBank = new WaveBank(audioEngine, "Content/WaveB.xwb");
            soundBank = new SoundBank(audioEngine, "Content/SoundB.xsb");
            cue = soundBank.GetCue("jazzloop");
            cue.Play();
            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            // Victory condition
            if (enemySpawnerHandler.getEnemySpawnList.Count <= 0)
            {
                Game1.CurrentGameState = Game1.GameState.GameWon;
            }
            // Game over condition
            if (Player.Health <= 0)
            {
                Game1.CurrentGameState = Game1.GameState.GameOver;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F6))// && prevState.IsKeyUp(Keys.F6))
            {
                rotationFactor += 0.1f;
            }

            camera.Rotation = (float)Math.Sin(rotationFactor) / 10;
            prevState = Keyboard.GetState();
            audioEngine.Update();
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
        #endregion
    }
}
