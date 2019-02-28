using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Cyberpunch_game
{
    public class Attack
    {
        public Rectangle Collision
        {
            get
            {
                if (delay > 0) return Rectangle.Empty;
                else return collision;
            }
            private set
            {
                collision = value;
            }
        }

        Point size;
        Rectangle collision = new Rectangle();
        Vector2 currentOffset = new Vector2(0, 0);
        Point offset = new Point(0, 0);
        int duration;
        int delay;
        public int Damage { get; private set; }
        public bool Valid { get; private set; }
        public bool Done { get; private set; }
        Vector2 motionSpeed = new Vector2(0, 0);
        Point maxOffset = new Point(0, 0);
        Vector2 changed = new Vector2(0, 0);
        public Character User { get; private set; }

        public Attack(Point Size, int duration, int delay, int damage, Character user)
        {
            size = Size;
            User = user;
            collision = new Rectangle((User.GetNextHitRect.X + offset.X), (User.GetNextHitRect.Y + offset.Y), size.X, size.Y);
            this.duration = duration;
            this.delay = delay;
            Damage = damage;
            Valid = true;
        }

        public Attack(Point Size, int duration, int delay, int damage, Vector2 MotionSpeed, Point MaxOffset, Character user, Point Offset)
            : this(Size, duration, delay, damage, user)
        {
            motionSpeed = MotionSpeed;
            maxOffset = MaxOffset;
            offset = Offset;
        }

        public void Update()
        {
            if (delay <= 0)
            {
                if (duration-- <= 0)
                    Valid = false;


                if (Math.Abs(currentOffset.X) <= Math.Abs(maxOffset.X))
                {
                    currentOffset.X += motionSpeed.X;
                    collision.X = (User.GetNextHitRect.X + offset.X + (int)currentOffset.X);

                }

                if (Math.Abs(currentOffset.Y) <= Math.Abs(maxOffset.Y))
                {
                    currentOffset.Y += motionSpeed.Y;
                    collision.Y = (User.GetNextHitRect.Y + offset.Y + (int)currentOffset.Y);
                }
            }
            else
            {
                collision.X = (User.GetNextHitRect.X + offset.X);
                collision.Y = (User.GetNextHitRect.Y + offset.Y);
                delay--;
            }
        }
    }
}
