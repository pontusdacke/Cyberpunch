using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cyberpunch_game
{
    class EnemySpawner
    {
        List<Character> enemyList = new List<Character>();
        public Vector2 SpawnLocation;
        Game game;
        Player player;
        Camera camera;
        Map map;

        public EnemySpawner(Game game, Player player, List<Character> enemyList, Camera camera, Map map)
        {
            this.map = map;
            this.camera = camera;
            this.player = player;
            this.game = game;
            this.enemyList = enemyList;
            
            
        }

        private Enemy SpawnEnemy()
        {
          Enemy enemy = new Enemy(game, SpawnLocation, "Player/player_animations", "Player", "idle", player, ref enemyList, 0.20f);
          game.Components.Add(enemy);
          enemy.camera = camera;
          enemyList.Add(enemy);
          enemy.collision = new Collision(map, enemy, game, enemyList);
          return enemy;
        }

        public void AddEnemy(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                SpawnEnemy();
            }
        }

    }
}
