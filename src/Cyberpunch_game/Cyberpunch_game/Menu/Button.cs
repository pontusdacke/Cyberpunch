using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Cyberpunch_game
{
    class Button : DrawableGameComponent
    {
        Texture2D background;
        SpriteFont font;
        SpriteBatch spriteBatch;
        public Game1.GameState State;
        string text;
        public bool Selected;
        float rotation;
        float scalecounter;
        float scale;

        public Button(string text, Game1.GameState state, Game Game)
            : base(Game)
        {
            this.State = state;
            this.text = text;
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //font = Game.Content.Load<SpriteFont>("Fonts/a_little_sketchy200");
            font = Game.Content.Load<SpriteFont>("Fonts/font_new200");
            this.scale = GetScaleToWindow();
            //background = Game.Content.Load<Texture2D>("Menu/ButtonBackground");

            base.LoadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (Selected)
            {
               // scalecounter += gameTime.ElapsedGameTime.Milliseconds;
                scalecounter += 0.06F;
                scale += (float)Math.Sin(scalecounter) / 1220;
            }
            else
            {
                scale = GetScaleToWindow();
                scalecounter = 0;
            }
            base.Update(gameTime);
        }
        public int FontHeight
        {
            get { return (int)font.MeasureString("AWZ").Y; }
        }
        public float GetScaleToWindow()
        {
            float newSize = (int)(GraphicsDevice.Viewport.Height * 0.15);
            float scaleValue = newSize / FontHeight;
            return scaleValue;
        }
        public void Draw(GameTime gameTime, Vector2 position)
        {
            spriteBatch.Begin();
            if (Selected)
            {
                position.X =  position.X + 50;
                spriteBatch.DrawString(font, text, position, Color.FromNonPremultiplied(85, 85,85,255), rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            if (background != null)
                spriteBatch.Draw(background, position, null, Color.White, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
            if (text != "" && !Selected)
                spriteBatch.DrawString(font, text, position, Color.Black, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
