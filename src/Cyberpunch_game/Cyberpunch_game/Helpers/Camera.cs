using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace Cyberpunch_game
{
    public class Camera
    {
        #region Variabler
        public  Matrix viewMatrix;
        private Vector2 position;  
        public float Rotation = 0.0f;
        public float Zoom = 1.0f;
        int timer;
        bool runTimer = false;
        Game game;
        #endregion

        public Vector2 Position
        {
            get { return position; }
            set
            {
                SetCameraPosition(value);
                UpdateViewMatrix();
            }
        }
        public Camera(Game game)
        {
            this.game = game;
            position = Vector2.Zero;
            UpdateViewMatrix();
        }


        private void SetCameraPosition(Vector2 pos)
        {
            //Hardcoded, may need some dynamics
            int value = 100;
            if (pos.X > position.X + value) position.X = pos.X - value;
            if (pos.X < position.X - value) position.X = pos.X + value;
            if (pos.Y > position.Y + value) position.Y = pos.Y - value;
            if (pos.Y < position.Y - value) position.Y = pos.Y + value;

            if (Zoom < 0.75f && !runTimer)
            {
                timer = 0;
                runTimer = true;
            }
            if (timer < 120 && runTimer)
            {
                timer += 1;
            }
            if (timer >= 120 && runTimer)
            {
                Zoom += 0.01f;
            }
            if (Zoom >= 0.75f) runTimer = false;
        }
        private void UpdateViewMatrix()
        {
            viewMatrix =
                Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
                Matrix.CreateTranslation(new Vector3(game.GraphicsDevice.Viewport.Width * 0.5f, game.GraphicsDevice.Viewport.Height * 0.5f, 0));
        }
    }
}
