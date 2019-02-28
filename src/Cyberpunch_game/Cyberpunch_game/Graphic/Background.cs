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
    class Background : DrawObject
    {
        #region Variables
        protected bool scrollable;
        protected bool autoMove;
        protected bool constantMove;
        protected Point cameraMoved;      //Hur mycket spelaren har rört sig sen senaste updateringen  
        protected Vector2 prevCameraPos;  //Spelarens position förra framen
        protected Vector2 position2;      //Den andra bildens position
        protected Vector2 moveRatio;      //värden för hur mycket bilden ska röra sig i förhållande till spelaren i x och y led
        #endregion

        /// <summary>
        /// Visar en bakgrund som scrollar relativt till spelaren, loopar oändligt i x led men inte i y led
        /// </summary>
        /// <param name="game"></param>
        /// <param name="P1">Spelaren</param>
        /// <param name="MoveRatio">Hur mycket bilden ska röra sig i x och y led i förhållande till spelaren, ju högre desto
        /// snabbare rör sig bilden</param>
        /// <param name="TextureName">Namnet på bakgrundens textur</param>
        /// <param name="LayerDepth">Vilket lager som bakgrunden ska ritas ut på</param>
        /// <param name="SrcRectangle">Position 0,0 och ska vara lika stor som bilden</param>
        public Background(Game game, Camera Camera, Vector2 MoveRatio, string TextureName, float LayerDepth, bool Scrollable, bool AutoMove, bool ConstantMove, Rectangle? SourceRectangle) 
            : base(game, new Vector2(0,-300),TextureName,new Vector2(0,0),SpriteEffects.None,Color.White,0,LayerDepth,1)
        {
            scrollable = Scrollable;
            camera = Camera;
            prevCameraPos = camera.Position;
            moveRatio = MoveRatio;
            autoMove = AutoMove;
            constantMove = ConstantMove;
            if (SourceRectangle != null)
                srcnRectangle = (Rectangle)SourceRectangle;
        }

        public override void Update(GameTime gameTime)
        {
            if (scrollable)
            {
                cameraMoved = new Point((int)camera.Position.X - (int)prevCameraPos.X, (int)camera.Position.Y - (int)prevCameraPos.Y);
                if (constantMove == false)
                {
                    if (cameraMoved.X != 0)
                    {
                        position.X += -cameraMoved.X * moveRatio.X;
                        position2.X += -cameraMoved.X * moveRatio.X;
                    }
                }
                if (cameraMoved.Y != 0)
                {
                    position.Y += -cameraMoved.Y * moveRatio.Y;
                    position2.Y += -cameraMoved.Y * moveRatio.Y;
                }
                if (autoMove == true)
                {
                    position.X += -1.0F * moveRatio.X;
                    position2.X += -1.0F * moveRatio.X;
                }
                if (position.X < -texture.Width)
                {
                    position.X = position2.X + texture.Width;
                }

                if (position2.X < -texture.Width)
                {
                    position2.X = position.X + texture.Width;
                }

                if (position.X > texture.Width)
                {
                    position.X = position2.X - texture.Width;
                }

                if (position2.X > texture.Width)
                {
                    position2.X = position.X - texture.Width;
                }
                prevCameraPos = new Vector2(camera.Position.X, camera.Position.Y);
                cameraMoved = new Point(0, 0);
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            position2 = new Vector2(position.X + texture.Width, -300);
            srcnRectangle = new Rectangle(0,0,texture.Width,texture.Height);
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();
            spritebatch.Draw(texture, position, srcnRectangle, drawcolor, rotation, originVector, drawScale, spriteffect, layerDeapth);
            if (scrollable)
                spritebatch.Draw(texture, new Vector2 (position2.X,position2.Y), srcnRectangle, drawcolor, rotation, originVector, drawScale, spriteffect, layerDeapth);
            spritebatch.End();
        }
    }
}
