using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Particle_Engine
{
    public class ParticleEngine
    {
        List<Particle> particleList;
        List<Texture2D> textureList; //Can be changed to a single static texture if texture randomization is not required
        Random random;
        public Vector2 EmitterLocation; //Sets the location for the engine, where the particles will procedurally be generated from

        public ParticleEngine(List<Texture2D> textureList, Vector2 EmitterLocation)
        {
            this.textureList = textureList;
            this.EmitterLocation = EmitterLocation;
            particleList = new List<Particle>();
            random = new Random();
        }

        private Particle generateFallingParticle() //Generates a particle which is heavily affected by gravity, which instantly falls to the ground
        {
            Texture2D texture = textureList[random.Next(textureList.Count)];
            Vector2 position = EmitterLocation;
            Vector2 speed = new Vector2(
                1f * (float)(random.NextDouble() * 2 - 1),
                1f * (float)(random.NextDouble() * 2 - 1));
            int ttl = 100 + random.Next(20);
            float angle = 0;
            float angleSpeed = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Vector2 gravity = new Vector2(0, 5);

            Color color = new Color(
                (float)(random.NextDouble()),
                (float)(random.NextDouble()),
                (float)(random.NextDouble())
                );

            float size = (float)random.NextDouble();

            return new Particle(texture, position, speed, ttl, angle, angleSpeed, color, size, gravity);
        }

        private Particle generateFloatingParticle() //Generate particles not at all affected by gravity, which float around in a bubble-like manner
        {
            Texture2D texture = textureList[random.Next(textureList.Count)];
            Vector2 position = EmitterLocation;
            Vector2 speed = new Vector2(
                1f * (float)(random.NextDouble() * 2 - 1),
                1f * (float)(random.NextDouble() * 2 - 1));
            int ttl = 100 + random.Next(20);
            float angle = 0;
            float angleSpeed = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Vector2 gravity = new Vector2(0, 0);

            Color color = new Color(
                (float)(random.NextDouble()),
                (float)(random.NextDouble()),
                (float)(random.NextDouble())
                );

            float size = (float)random.NextDouble();

            return new Particle(texture, position, speed, ttl, angle, angleSpeed, color, size, gravity);
        }

        private Particle generateAfterburnerParticle(int direction)
        {
            Texture2D texture = textureList[random.Next(textureList.Count)];
            Vector2 position = EmitterLocation;
            int ttl = 5 * random.Next(10);
            float angle = 0;
            float angleSpeed = 0;
            Vector2 gravity;
            Vector2 speed;

            if (direction == 0)
            {
                gravity = new Vector2(0, -0.0001f * random.Next(1, 2));
                speed = new Vector2(
                   1f * (float)(random.NextDouble() * 5), 0);
            }
            else
            {
                gravity = new Vector2(5, -0.0001f * random.Next(1, 2));
                speed = new Vector2(
                   -1f * (float)(random.NextDouble() * 5), 0);
            }

            float size = (float)random.NextDouble();

            return new Particle(texture, position, speed, ttl, angle, angleSpeed, Color.LemonChiffon, size, gravity);
        }

        private Particle generateExplosionParticle() //Generate a massive amount of short-lived and speedy particles, forming an explosion pattern.
        {
            Texture2D texture = textureList[random.Next(textureList.Count)];
            Vector2 position = EmitterLocation;
            Vector2 speed = new Vector2(random.Next(-5, 5), random.Next(-5, 5));

            int ttl = 20 + random.Next(30);
            float angle = 0;
            float angleSpeed = 0;
            Vector2 gravity = new Vector2(0, 0);
            Color color = new Color((float)(random.NextDouble()), 0, 0);
            float size = (float)random.NextDouble();

            return new Particle(texture, position, speed, ttl, angle, angleSpeed, Color.White, size, gravity);
        }

        private Particle generateDustParticles()
        {
            Texture2D texture = textureList[random.Next(textureList.Count)];
            Vector2 position = EmitterLocation;
            Vector2 gravity = new Vector2(0, -0.3f);
            Vector2 speed = new Vector2(random.Next(-5, 5), 0);
            int ttl = 10 + random.Next(0,6);
            float angle = 0;
            float angleSpeed = 0;
            float size = (float)random.NextDouble();

            return new Particle(texture, EmitterLocation, speed, ttl, angle, angleSpeed, Color.Black, size, gravity);
        }

        //Main methods for generating the different types of particles
        #region Particle Creation Methods
        public void AddExplosionParticle(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                particleList.Add(generateExplosionParticle());
            }
        }

        public void AddFallingParticle(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                particleList.Add(generateFallingParticle());
            }
        }

        public void AddFloatingParticle(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                particleList.Add(generateFloatingParticle());
            }
        }

        public void AddBoosterParticle(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                particleList.Add(generateAfterburnerParticle(0));
            }
        }

        public void AddDustParticle(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                particleList.Add(generateDustParticles());   
            }
        }

        #endregion
        public void Update()
        {
            for (int i = 0; i < particleList.Count; i++)
            {
                particleList[i].Update();
                if (particleList[i].TTL <= 0)
                {
                    particleList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {   
            foreach (Particle p in particleList)
            {
                p.Draw(spriteBatch);
            }
           
        }

    }
}
