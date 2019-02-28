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
using Particle_Engine;

namespace Cyberpunch_game
{
    public class EnemySpawner
    {

        #region Values
        protected Game game;
        protected List<Character> MyGoons = new List<Character>();
        protected List<Character> enemyList;
        protected Player player;
        protected Camera camera;
        protected Map map;

        protected Vector2 Position;
        protected Rectangle hitRect;
        protected int health;
        protected Texture2D spawnerGfx;
        protected float counter;
        protected float spawnIntervall;
        protected List<EnemySpawner> enemySpawnerList;
        protected ParticleEngine attackHitEngine;
        protected ParticleEngine energyEffctEngine;

        protected float timer = 10;         //Initialize a 10 second timer
        protected const float TIMER = 10;
        protected float speed = 0.5F;

        protected List<object> components = new List<object>(); // Keep track of all components
        #endregion

        #region Properties

        public Rectangle HitRect
        {
            get { return hitRect; }
            set { hitRect = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        
        #endregion

        #region Constructors

        public EnemySpawner(Game game1, List<Character> EnemyList, Player Player, Camera Camera, Map Map, Texture2D texture, Vector2 spawnerPos, List<EnemySpawner> EnemySpawnerList, List<object> Components)
        {
            //behövs för att lägga till nya fiender
            game = game1;
            enemyList = EnemyList;
            player = Player;
            camera = Camera;
            map = Map;
            components = Components;

            enemySpawnerList = EnemySpawnerList;
            spawnerGfx = texture;
            hitRect = new Rectangle((int)spawnerPos.X, (int)spawnerPos.Y, texture.Width, texture.Height);
            health = 4000;
            Position = spawnerPos;
            counter = 10;
            spawnIntervall = 400;

            List<Texture2D> templist2 = new List<Texture2D>();
            templist2.Add(HelperMethods.CreateColoredTexture(game, Color.White, new Point(20, 20)));
            attackHitEngine = new ParticleEngine(templist2, new Vector2(0, 0));

            //particleengine för konsant utritning av partiklar för spawner
            List<Texture2D> templist1 = new List<Texture2D>();
            templist1.Add(HelperMethods.CreateColoredTexture(game, Color.Green, new Point(20, 20)));
            energyEffctEngine = new ParticleEngine(templist1, new Vector2(0, 0));
            energyEffctEngine.EmitterLocation = new Vector2(Position.X + hitRect.Width / 2, Position.Y + hitRect.Height / 2);
        }

        #endregion

        private void SpawnEnemy()
        {
            if (MyGoons.Count < 8)
            {
                Enemy enemy = new Enemy(game, Position, "Enemy/enemy_sprite", "Robot", "idle", player, ref enemyList, 0.5F, MyGoons);
                enemy.camera = camera;

                enemyList.Add(enemy);

                enemy.collision = new Collision(map, enemy, game, enemyList, enemySpawnerList);
                components.Add(enemy);
                game.Components.Add(enemy);
                MyGoons.Add(enemy);
            }
        }
        private void AddEnemy(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                SpawnEnemy();
            }
        }

        public void Update(GameTime gameTime)
        {
            counter -=1;
            if (counter <= 0)
            {
                SpawnEnemy();
                counter = spawnIntervall;
            }

            //lägger till nya partiklar
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= speed;
            if (timer < 0)
            {
                energyEffctEngine.AddFloatingParticle(5);
                timer = TIMER;
            }

            attackHitEngine.Update();
            energyEffctEngine.Update();
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(spawnerGfx, Position, Color.White);
            attackHitEngine.Draw(spritebatch);
            energyEffctEngine.Draw(spritebatch);
        }

        public void GotHit(Attack localAttack)
        {
            attackHitEngine.EmitterLocation = new Vector2(Position.X + hitRect.Width / 2, Position.Y + hitRect.Height / 2);
            attackHitEngine.AddExplosionParticle(5);

            health -= localAttack.Damage;
        }

    }
}
