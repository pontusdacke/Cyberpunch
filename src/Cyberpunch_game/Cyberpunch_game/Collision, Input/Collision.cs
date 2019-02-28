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
    public class Collision
    {
        //variabler
        private Character character;
        private Map map;
        private Game game;  //ta bort sedan finns för attt kunna kolla kollision genom att ändra backgrund
        private List<Character> enemyList;
        private List<EnemySpawner> enemySpawnerlist;
        private List<Rectangle> collidedEnemys;
        private List<Rectangle> collidedBlocks;
        private List<Rectangle> prevCollidedEnemys;
        private List<Rectangle> prevCollidedBlocks;
        public bool externalInfo; //dummy name, used to get to easier access the enemy collisions

        public Collision(Map Map1, Character Character, Game game, List<Character> EnemyList, List<EnemySpawner> EnemySpawnerlist)
        {
            character = Character;
            map = Map1;
            this.game = game;
            enemyList = EnemyList;
            enemySpawnerlist = EnemySpawnerlist;
        }

        public void Update(GameTime gametime)
        {
            List<Character> temp = HitCollision();
            HitSpawnerCollision();
            Unstuck();
        }

        public void Unstuck()
        {
            //int blocksCounter = 0;
            //if (prevCollidedBlocks != null)
            //    foreach (Rectangle rec in prevCollidedBlocks)
            //    {
            //        if (collidedBlocks.Contains(rec))
            //            blocksCounter++;
            //    }
            //int enemysCounter = 0;
            //if (prevCollidedEnemys != null)
            //    foreach (Rectangle rec in prevCollidedEnemys)
            //    {
            //        if (collidedEnemys.Contains(rec))
            //            enemysCounter++;
            //    }
            //if (enemysCounter >= 2 || blocksCounter >= 3)
            //    character.Position.Y -= 100;
        }

        public List<Character> HitCollision()
        {
            List<Character> chars = new List<Character>();
            Rectangle temp = character.GetAttackRect;
            if (temp != Rectangle.Empty)    //om det finns en attackrektangel, kolla dom en den kolliderar med någon annan, om lägg till i listan
            {
                foreach (Character c1 in enemyList)  //kollar för alla charas
                {
                    if (temp.Intersects(c1.GetNextHitRect) && c1 != character)
                    {
                        chars.Add(c1);
                        c1.GotHit(character.attackBox);
                    }
                }
            }
            return chars;
        }

        public void HitSpawnerCollision()
        {
            if (character.GetType()== typeof(Player))
            {
                Rectangle temp = character.GetAttackRect;
                if (temp != Rectangle.Empty)    //om det finns en attackrektangel, kolla dom en den kolliderar med någon annan, om lägg till i listan
                {
                    foreach (EnemySpawner es in enemySpawnerlist)  //kollar för alla charas
                    {
                        if (temp.Intersects(es.HitRect))
                        {
                            es.GotHit(character.attackBox);
                        }
                    }
                }
            }
        }

        private bool CheckWallCollision(out List<Rectangle> collidedRect)
        {
            prevCollidedBlocks = collidedBlocks;
            collidedRect = new List<Rectangle>();   //lista där alla tiles som kollideras med läggs till
            bool isCollision = false;   //är false om det inte blir någon kollision
            foreach (TileData tD in map.GetTiles)   //loopar igenom alla tiles
                if (tD.GetBoundRect.Intersects(character.GetNextHitRect))
                {
                    collidedRect.Add(tD.GetBoundRect);  //lägger till de man kolliderar med 
                    isCollision = true; //sätter kollision till true
                }
            collidedBlocks = collidedRect;
            if (isCollision == true)
            {
                
                return true;
            }
            else
            {
                collidedRect = null;
                return false;
            }
        }

        private bool CheckEnemyCollision(out List<Rectangle> collidedRect)
        {
            prevCollidedEnemys = collidedEnemys;
            collidedRect = new List<Rectangle>();   //lista där alla tiles som kollideras med läggs till
            bool isCollision = false;   //är false om det inte blir någon kollision

            foreach (Character e in enemyList)
                if (character.GetNextHitRect.Intersects(e.GetNextHitRect) && e != character)
                {
                    collidedRect.Add(e.GetNextHitRect);
                    isCollision = true;
                }

            collidedEnemys = collidedRect;
            if (isCollision == true)
            {
                externalInfo = true;
                return true;
            }
            else
            {
                collidedRect = null;
                return false;
            }
        }

        public List<Rectangle> testCollision()
        {
            List<Rectangle> collidedRect;
            if (CheckWallCollision(out collidedRect) == true)
            {
                return collidedRect;
            }
            else
            {
                return null;
            }
        }

        public List<Rectangle> enemyCollision()
        {
            List<Rectangle> collidedRect;
            if (CheckEnemyCollision(out collidedRect) == true)
            {
                return collidedRect;
            }
            else
            {
                return null;
            }
        }

        public List<Character> enemyCollisionChar()
        {
            List<Character> collidedChar;
            if (CheckEnemyCollisionChar(out collidedChar) == true)
            {
                return collidedChar;
            }
            else
            {
                return null;
            }
        }

        private bool CheckEnemyCollisionChar(out List<Character> collidedChar)
        {
            prevCollidedEnemys = collidedEnemys;
            collidedChar = new List<Character>();   //lista där alla tiles som kollideras med läggs till
            bool isCollision = false;   //är false om det inte blir någon kollision

            foreach (Character e in enemyList)
                if (character.GetNextHitRect.Intersects(e.GetNextHitRect) && e != character)
                {
                    collidedChar.Add(e);
                    isCollision = true;
                }

            if (isCollision == true)
            {
                externalInfo = true;
                return true;
            }
            else
            {
                collidedChar = null;
                return false;
            }
        }

    }
}
