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
    public class Hud
    {

        #region Values
        protected Texture2D backgoundTexture;   //Textur för bakgrundsgrafik för en hud
        protected Texture2D barTexture;         //Grafik för själva baren, kommer bli en färg om man inte tilldelar den något
        protected Rectangle srcGraphicRect;     //Rektangel för bakgrundsgrafiken
        protected Rectangle barRectStartSize;
        protected Rectangle barRect;            //Rektangel för bargrafiken
        protected Color color;                  //färgen för baren
        protected float barValue;               //Kommer vara ett ref-värde. Är värdet som baren skall visa
        protected float maxBarValue;            //kommer att få värdet som bar har från början, om maxvärdet ändras kan denna ändras via property
        protected float toChange;
        protected bool drawBar = false;                 //om sann rita ut baren
        protected bool drawBackground = false;          //om sann rita ut background
        protected SpriteFont statusText;
        protected string text;
        protected bool drawText = false;
        protected Vector2 drawTextPos;
        protected Color textColor;
        protected float textScale;
        #endregion

        #region Properties

        public float MaxBarValue
        {
            get { return maxBarValue; }
            set { maxBarValue = value; toChange = barRect.Width / maxBarValue; }
        }      

        #endregion

        #region Constructors

        #region Default Konstruktor

        /// <summary>
        /// Default konstruktor, sätter alla stanard variabler till null
        /// </summary>
        public Hud(Game game)
        {
            srcGraphicRect = new Rectangle();
            barRect = new Rectangle();
            color = new Color();
        }

        #endregion

        #region Enbart BarKonstruktor

        #region Konstruktor 1

        /// <summary>
        /// Use to construct a hud with only a bar of the value, no other background graphics
        /// </summary>
        /// <param name="BarRectangle">The rectangle where the bar will be drawn and containd within</param>
        /// <param name="Color1">The color of the bar</param>
        /// <param name="BarValue">The value the bar will represent, must be a ref value</param>
        public Hud(Game game, Rectangle BarRectangle, Color Color1, float BarValue):this(game)
        {
            drawBar = true;
            barRect = new Rectangle(0,0, BarRectangle.Width, BarRectangle.Height);
            //barRect = BarRectangle;
            barRectStartSize = BarRectangle;
            color = Color1;
            barValue = BarValue;
            maxBarValue = barValue;
            barTexture = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            barTexture.SetData<Color>(new Color[] { color });

            toChange = barRect.Width / maxBarValue ;
        }

        #endregion

        #region Konstruktor 2

        /// <summary>
        /// Use to construct a hud with only a bar of the value and a texture graphic for the bar, no other background graphics. 
        /// </summary>
        /// <param name="BarRectangle">The rectangle where the bar will be drawn and containd within</param>
        /// <param name="Color1">The color of the bar</param>
        /// <param name="BarValue">The value the bar will represent, must be a ref value</param>
        /// /// <param name="BarTexture">The texturegraphic of the bar</param>
        public Hud(Game game, Rectangle BarRectangle, Color Color1, float BarValue, Texture2D BarTexture)
            : this(game, BarRectangle, Color1, BarValue)
        {
            barTexture = BarTexture;
        }

        #endregion

        #endregion

        #region FUll konstruktor, bar oh bakgrund

        #region Konstruktor 1

        /// <summary>
        /// Use to construct a hud with a bar that has no graphics texture only color, and other background graphics
        /// </summary>
        /// <param name="BarRectangle">The rectangle where the bar will be drawn and containd within</param>
        /// <param name="Color1">The color of the bar</param>
        /// <param name="BarValue">The value the bar will represent, must be a ref value</param>
        public Hud(Game game, Rectangle BarRectangle, Color Color1, float BarValue, Texture2D BackgoundTexture, Rectangle SrcGraphicRect)
            : this(game, BarRectangle, Color1, BarValue)
        {
            srcGraphicRect = SrcGraphicRect;
            backgoundTexture = BackgoundTexture;
            drawBackground = true;
        }

        #endregion

        #region Konstruktor 2

        /// <summary>
        /// Use to construct a hud with a bar that has a graphics texture, and other background graphics
        /// </summary>
        /// <param name="BarRectangle">The rectangle where the bar will be drawn and containd within</param>
        /// <param name="Color1">The color of the bar</param>
        /// <param name="BarValue">The value the bar will represent, must be a ref value</param>
        /// /// <param name="BarTexture">The texturegraphic of the bar</param>
        public Hud(Game game, Rectangle BarRectangle, Color Color1, float BarValue, Texture2D BarTexture, Texture2D BackgoundTexture, Rectangle SrcGraphicRect)
            : this(game, BarRectangle, Color1, BarValue, BackgoundTexture, SrcGraphicRect)
        {
            barTexture = BarTexture;
        }

        #endregion

        #endregion

        #endregion

        public void SetText(SpriteFont Font, string Text, Vector2 Position, Color color, float Scale)
        {
            statusText = Font;
            text = Text;
            drawText = true;
            drawTextPos = Position;
            textColor = color;
            textScale = Scale;
        }

        public void Update(GameTime gameTime, float BarValue)
        {
            if (BarValue != barValue) 
            {
                barValue = BarValue;
                barRect.Width = (int)(barValue * toChange);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spritebatch)
        {
            if (drawBackground == true)
            {
                spritebatch.Draw(backgoundTexture, srcGraphicRect, Color.White);
            }
            if (drawBar == true)
            {
                spritebatch.Draw(barTexture, new Vector2(barRectStartSize.X, barRectStartSize.Y), barRect, color);
            }
            if (drawText == true)
            {
                spritebatch.DrawString(statusText, text, drawTextPos, textColor, 0F, Vector2.Zero, textScale, SpriteEffects.None, 1F);
            }
        }

    }
}
