using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Particle_Engine
{
    class Particle
    {
        public Texture2D ParticleTexture; 
        public Vector2 Position; //Position for the particle
        public int TTL; // Time to live for the particle, counts down for every update
        public float Angle; //Indicate the rotation angle
        public float AngleSpeed; // The rotation speed for the indicated angle
        public Color Color; // Used for colorization of individual Color
        public float Size; // Used for scaling the particle's size
        public Vector2 Gravity; // Should be used as a constant speed downwards
        //private float acceleration = 0.9f; //Acceleration speed for the particle, currently unused but can be used for later implemented particles
        public Vector2 Speed; //Velocity
        Rectangle sourceRectangle; 
        Vector2 origin; // The position of the middle part of the particle
        public Rectangle hitbox { get; private set; } // Unused, might be used for hitbox detection later

        public Particle(Texture2D ParticleTexture, Vector2 Position, Vector2 Speed, int TTL, float Angle, float AngleSpeed, Color Color, float Size, Vector2 Gravity)
        {
            this.ParticleTexture = ParticleTexture;
            this.Position = Position;
            this.Angle = Angle;
            this.Speed = Speed;
            this.TTL = TTL;
            this.AngleSpeed = AngleSpeed;
            this.Color = Color;
            this.Size = Size;
            this.Gravity = Gravity;

            sourceRectangle = new Rectangle(0, 0, this.ParticleTexture.Width, this.ParticleTexture.Height);
            origin = new Vector2(ParticleTexture.Width / 2, ParticleTexture.Height / 2);
        }

        public void Update()
        {
            TTL--;
            Position += Speed;

            //Gravity effect one (Nogravity to deadfall in half a second)
            Position.Y += Position.Y * (Gravity.Y / 10000); //Divided by 10k för example purposes, input value is relatively high.
            Gravity = Gravity * 1.4f;
            
            //// Gravity effect two (Unused)
            //Position += Gravity;
            //Gravity.Y += acceleration;
            //acceleration += acceleration * 0.01f;

            Angle += AngleSpeed;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(ParticleTexture, Position, sourceRectangle, Color, Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}
