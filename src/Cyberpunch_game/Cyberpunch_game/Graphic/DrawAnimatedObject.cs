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

using BrashMonkeySpriter;

namespace Cyberpunch_game
{
    public class DrawAnimatedObject : DrawableGameComponent
    {
        public Camera camera;
        protected SpriteBatch spritebatch;

        #region Draw variabler
        public Vector2 Position;
        protected Vector2 originVector;
        protected Rectangle srcnRectangle;
        protected SpriteEffects spriteffect;
        public CharacterAnimator animator;
        protected Color drawcolor;
        protected float rotation;
        protected float layerDeapth;
        protected float drawScale;
        protected string currentAnimation;
        public Vector2 rectSize;
        
        //Load content
        protected string animatorPath;
        protected string animatorEntity;
        #endregion

        #region Properties
        public Color Color
        {
            get { return drawcolor; }
            set { drawcolor = value; }
        }
        public Vector2 Middle
        {
            get 
            {
                return new Vector2(
                    Position.X + rectSize.X/2,
                    Position.Y + rectSize.Y/2);
            }
        }
        #endregion

        protected DrawAnimatedObject(Game game)
            : base(game)
        {
            
            originVector = new Vector2(0, 0);
            spriteffect = SpriteEffects.None;
            drawcolor = Color.White;
            rotation = 0;
            layerDeapth = 1;
            if (drawScale == 0)
                drawScale = 1;
        }

        public DrawAnimatedObject(Game game, Vector2 Position, string AnimatorPath, string AnimatorObject, string StartAnimation, float Scale, Vector2 Size)
            : this(game)
        {
            rectSize = Size;
            this.Position = Position;
            animatorPath = AnimatorPath;
            animatorEntity = AnimatorObject;
            currentAnimation = StartAnimation;
            drawScale = Scale;
        }
        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spritebatch = new SpriteBatch(GraphicsDevice);

            //laddar in texturen
            animator = Game.Content.Load<CharacterModel>(animatorPath).CreateAnimator(animatorEntity);
            animator.ChangeAnimation(currentAnimation);

            animator.Location = Position;
            animator.Scale = drawScale;
            animator.Rotation = rotation;
            animator.FlipX = false;
            animator.FlipY = false;

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if (camera != null)
            {
                spritebatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, camera.viewMatrix);
                animator.Draw(spritebatch);
                spritebatch.End();
            }
            base.Draw(gameTime);
        }

    }

}

