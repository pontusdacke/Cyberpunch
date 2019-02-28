using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Cyberpunch_game
{
    public class Player : Character
    {

        #region Values
        private Hud hudBoost, hudHealth;
        private Texture2D hudtexture;
        #endregion

        #region Properties
        
        #endregion

        #region Konsruktorer
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game">Game klassen</param>
        /// <param name="Position">Startpositionen för denna karaktär</param>
        /// <param name="TextureName">namnet på texturen i content</param>
        /// <param name="pindex">Plaayerindex för input</param>
        public Player(Game game, Vector2 Position, string AnimationPath, string EntityName, string startAnimation, PlayerIndex pindex, float Scale)
            : base(game, Position, AnimationPath, EntityName, startAnimation,pindex, Scale, new Vector2(90,150))
        {
            movespeed = 600;
            boostPower = 100;
            maxBoostPower = 100;
           
        }
        #endregion

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Texture2D tempHealthTexture = Game.Content.Load<Texture2D>("UI/h_bar");
            Texture2D tempBoostTexture = Game.Content.Load<Texture2D>("UI/b_bar");
            Texture2D tempBackGroundTexture = Game.Content.Load<Texture2D>("UI/back_bar");
            SpriteFont tempFont = Game.Content.Load<SpriteFont>("UI/hudText");
            hudHealth = new Hud(Game, new Rectangle(7, 3, tempHealthTexture.Width, tempHealthTexture.Height), Color.White, health, tempHealthTexture, tempBackGroundTexture, new Rectangle(7, 3, tempBackGroundTexture.Width, tempBackGroundTexture.Height));
            hudBoost = new Hud(Game, new Rectangle(7, 80, tempBoostTexture.Width, tempBoostTexture.Height), Color.White, boostPower, tempBoostTexture, tempBackGroundTexture, new Rectangle(7, 80, tempBackGroundTexture.Width, tempBackGroundTexture.Height));
            hudHealth.SetText(tempFont, "Health", new Vector2(14, 3), Color.Black, 1F);
            hudBoost.SetText(tempFont, "Booster", new Vector2(14, 80), Color.Black, 1F);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (currentAction.GetType() == typeof(A_Jump))
                animator.ChangeAnimation("Jump");
            if (direction.Y > 0 && currentAction.GetType() != typeof(A_Punch))
                animator.ChangeAnimation("Falling");
            hudBoost.Update(gameTime, boostPower);
            hudHealth.Update(gameTime, health);
            base.Update(gameTime);

            camera.Position = Position;
        }

        public override void Draw(GameTime gameTime)
        {
            spritebatch.Begin();
            hudHealth.Draw(gameTime, spritebatch);
            hudBoost.Draw(gameTime, spritebatch);
            spritebatch.End();
            base.Draw(gameTime);
           
        }

    }
}
