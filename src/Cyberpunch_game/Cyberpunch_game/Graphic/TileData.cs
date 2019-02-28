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
    public class TileData
    {
        //variabler
        private Point drawPos;
        private Rectangle boundrect;
        private Texture2D texture;
        private string identifier;  //vilken den representarar i textfilen som banan ritas från

        //properties
        public Rectangle GetBoundRect
        {
            get { return boundrect; }
        }

        public Texture2D GetTexture
        {
            get { return texture; }
        }

        public string GetIdentifier
        {
            get { return identifier; }
        }

        public Point DrawPos
        {
            get { return drawPos; }
            set { drawPos = value; }
        }

        public TileData(Rectangle BoundRect, Texture2D Texture, string Identifier)
        {
            identifier = Identifier;
            texture = Texture;
            boundrect = BoundRect;
        }

        public TileData(Rectangle BoundRect, Texture2D Texture, string Identifier, Point p)
        {
            identifier = Identifier;
            texture = Texture;
            boundrect = BoundRect;
            drawPos = p;
        }

    }
}
