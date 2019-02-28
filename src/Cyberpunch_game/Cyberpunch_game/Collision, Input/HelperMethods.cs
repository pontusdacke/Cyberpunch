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
    static class HelperMethods
    {
        public static Texture2D CreateColoredTexture(Game game, Color color, Point size)
        {
            Texture2D temp = new Texture2D(game.GraphicsDevice, size.X, size.Y, false, SurfaceFormat.Color);
            Color[] colorArray = new Color[size.X * size.Y];
            for (int i = 0; i < colorArray.Length - 1; i++)
            {
                colorArray[i] = color;
            }
            temp.SetData(colorArray);
            return temp;
        }
    }
}
