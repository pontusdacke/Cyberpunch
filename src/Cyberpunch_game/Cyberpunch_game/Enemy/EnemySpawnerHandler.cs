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
    public class EnemySpawnerHandler : DrawableGameComponent
    {
        #region Values

        protected List<EnemySpawner> enemySpawnerList = new List<EnemySpawner>();
        protected Texture2D spawnerTexture;
        protected SpriteBatch spritebatch;
        protected Camera camera;
        protected List<Character> enemyList;
        protected Player player;
        protected Map map;
        protected List<Vector2> spawnerPos;
        protected List<object> components = new List<object>(); // Keep track of all components

        #endregion

        #region Properties

        public List<EnemySpawner> getEnemySpawnList
        {
            get { return enemySpawnerList; }
            set { enemySpawnerList = value; }
        }
        

        #endregion

        #region Constructors

        public EnemySpawnerHandler(Game game, List<Character> EnemyList, Player Player, Camera Camera, Map Map, List<Vector2> Spawnerpos, List<object> Components)
            : base(game)
        {
            camera = Camera;
            enemyList = EnemyList;
            player = Player;
            map = Map;
            spawnerPos = Spawnerpos;
            components = Components;
        }

        #endregion

        protected override void LoadContent()
        {
            spritebatch = new SpriteBatch(GraphicsDevice);

            spawnerTexture = Game.Content.Load<Texture2D>("Tile/spawner");

            //enemySpawnerList.Add(new EnemySpawner(Game, enemyList, player, camera, map, spawnerTexture, new Vector2(270, 70), enemySpawnerList));
            SetSpawers();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (EnemySpawner es in enemySpawnerList)
            {
                es.Update(gameTime);

                if (es.Health <= 0)
                {
                    enemySpawnerList.Remove(es);
                    player.Health = player.MaxHealth;
                    break;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (camera != null)
            {
                spritebatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.viewMatrix);

                foreach (EnemySpawner es in enemySpawnerList)
                {
                    es.Draw(spritebatch);
                }

                spritebatch.End();
            }
            base.Draw(gameTime);
        }

        public void SetSpawers()
        {
            foreach (Vector2 v2 in spawnerPos)
            {
                enemySpawnerList.Add(new EnemySpawner(Game, enemyList, player, camera, map, spawnerTexture, v2, enemySpawnerList, components));
            }
        }
        
    }
}
