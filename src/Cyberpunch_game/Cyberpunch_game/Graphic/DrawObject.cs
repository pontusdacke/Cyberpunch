using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Cyberpunch_game
{
    public class DrawObject : DrawableGameComponent
    {
        public Camera camera;

        protected SpriteBatch spritebatch;

        #region Draw variabler
        public Vector2 position;
        protected Vector2 originVector;
        protected Rectangle srcnRectangle;
        protected SpriteEffects spriteffect;
        protected Texture2D texture;
        protected Color drawcolor;
        protected float rotation;
        protected float layerDeapth;
        protected float drawScale;

        //Load content
        protected string textureName;
        #endregion

        #region Properties
        public Rectangle GetHitRectangle    //rectangle 
        { 
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }
        public Color Color
        {
            get { return drawcolor; }
            set { drawcolor = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        #endregion

        #region Constructors
        protected DrawObject(Game game)
        : base(game)
        {
            originVector = new Vector2(0, 0);
            spriteffect = SpriteEffects.None;
            drawcolor = Color.White;
            rotation = 0;
            layerDeapth = 1;
            drawScale = 1;
        }

        public DrawObject(Game game, Vector2 Position, string TextureName)
        : this(game)
        {
            position = Position;
            textureName = TextureName;
        }
        public DrawObject(Game game, Vector2 Position, string TextureName, Vector2 OriginVector, SpriteEffects SpritEffect1, Color DrawColor,
        float Rotation, float LayerDeapth, float DrawScale)
            : base(game)
        {
            position = Position;
            textureName = TextureName;
            originVector = OriginVector;
            spriteffect = SpritEffect1;
            drawcolor = DrawColor;
            rotation = Rotation;
            layerDeapth = LayerDeapth;
            drawScale = DrawScale;
        }


        public DrawObject(Game game, Vector2 Position, string TextureName, Vector2 OriginVector, SpriteEffects SpritEffect1, Color DrawColor,
        float Rotation, float LayerDeapth, float DrawScale, Rectangle SrcRectangle)
        : base(game)
        {
            position = Position;
            textureName = TextureName;
            originVector = OriginVector;
            spriteffect = SpritEffect1;
            drawcolor = DrawColor;
            rotation = Rotation;
            layerDeapth = LayerDeapth;
            drawScale = DrawScale;
            srcnRectangle = SrcRectangle;
        }

        #endregion

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spritebatch = new SpriteBatch(GraphicsDevice);

            //laddar in texturen
            texture = Game.Content.Load<Texture2D>(textureName);

            //om man ej har definerat en src rect kommer den by default att bli lika stor som bilden
            if (srcnRectangle == new Rectangle(0, 0, 0, 0))
                srcnRectangle = new Rectangle(0, 0, texture.Width, texture.Height);

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if (camera != null)
            {
                spritebatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.viewMatrix);
                spritebatch.Draw(texture, position, srcnRectangle, drawcolor, rotation, originVector, drawScale, spriteffect, layerDeapth);
                spritebatch.End();
            }
            base.Draw(gameTime);
        }

    }
}
