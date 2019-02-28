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
using Particle_Engine;

namespace Cyberpunch_game
{
    public class Character : DrawAnimatedObject, IControllable
    {
        #region character variabler
        public Vector2 direction;    //Borde bytas ut mot enum likt CorrectionVector2 structen
        public Vector2 lastDirection = new Vector2(0, 0);
        protected Vector2 speed;
        public bool onGround = false;
        public float movespeed = 300;
        protected const float fallspeed = 300;
        protected float maxfallSpeed = 10;
        protected Queue<GameAction> actionQueue = new Queue<GameAction>();
        protected PlayerIndex playerIndex;
        protected float acceleration = 0.1F;
        public float boostPower = 100;
        protected float maxBoostPower = 100;
        protected int health = 200;
        protected int maxHealth = 200;
        protected GameAction currentAction;
        public ParticleEngine characterEngine;
        public ParticleEngine attackHitEngine;
        public ParticleEngine boosterEngine;
        public Collision collision; //test om detta inte fungarar ta bort denna
        public Vector2 tocheck;
        public Attack attackBox = null;
        List<Texture2D> tempList = new List<Texture2D>();
        List<Texture2D> gearParticleList = new List<Texture2D>();
        Texture2D testRectTexture;
        #endregion

        #region properties
        public PlayerIndex PlayerIndex { get; set; }
        public int Health
        {
            get { return health; }
            set { if (value <= maxHealth) health = value; }
        }
        public int MaxHealth
        {
            get { return maxHealth; }
        }
        public float BoostPower
        {
            get { return boostPower; }
            set { if (value <= maxBoostPower) boostPower = value; }
        }
        public Rectangle GetAttackRect
        {
            get { if (attackBox != null && attackBox.Collision != null) return attackBox.Collision; else return Rectangle.Empty; }
        }
        public Rectangle GetNextHitRect
        {
            get { return new Rectangle((int)tocheck.X, (int)tocheck.Y, (int)rectSize.X, (int)rectSize.Y); }
        }
        #endregion

        #region Constructors

        public Character(Game game, Vector2 Position, string AnimationPath, string EntityName, string startAnimation, PlayerIndex pindex, float Scale, Vector2 Size)
            : base(game, Position, AnimationPath, EntityName, startAnimation, Scale, Size)
        {
           
            currentAction = new A_Idle();
            tempList.Add(HelperMethods.CreateColoredTexture(game, Color.White, new Point(10, 10)));
            characterEngine = new ParticleEngine(tempList, new Vector2(0, 0));

            List<Texture2D> templist2 = new List<Texture2D>();
            templist2.Add(HelperMethods.CreateColoredTexture(game, Color.White, new Point(20, 20)));
            attackHitEngine = new ParticleEngine(gearParticleList, new Vector2(0, 0));

            List<Texture2D> templist3 = new List<Texture2D>();
            templist3.Add(HelperMethods.CreateColoredTexture(game, Color.White, new Point(20, 20)));
            boosterEngine = new ParticleEngine(templist3, new Vector2(0, 0));

            playerIndex = pindex;
            tocheck = new Vector2(Position.X, Position.Y);
            testRectTexture = HelperMethods.CreateColoredTexture(Game, Color.Blue, new Point(50, 50));  //ta bort när kollisionslådor fungerar
        }

        public Character(Game game, Vector2 Position, string AnimationPath, string EntityName, string startAnimation, float Scale, Vector2 Size)
            : base(game, Position, AnimationPath, EntityName, startAnimation, Scale, Size)
        {
            tempList.Add(HelperMethods.CreateColoredTexture(game, Color.White, new Point(10, 10)));
            characterEngine = new ParticleEngine(tempList, new Vector2(0, 0));

            List<Texture2D> templist2 = new List<Texture2D>();
            templist2.Add(HelperMethods.CreateColoredTexture(game, Color.White, new Point(20, 20)));
            attackHitEngine = new ParticleEngine(gearParticleList, new Vector2(0, 0));

            List<Texture2D> templist3 = new List<Texture2D>();
            templist3.Add(HelperMethods.CreateColoredTexture(game, Color.White, new Point(20, 20)));
            boosterEngine = new ParticleEngine(templist3, new Vector2(0, 0));

            tocheck = new Vector2(Position.X, Position.Y);
            testRectTexture = HelperMethods.CreateColoredTexture(Game, Color.Blue, new Point(50, 50));  //ta bort när kollisionslådor fungerar
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            if (actionQueue.Count > 0)
            {
                if (currentAction.Valid(actionQueue.Peek().GetType()))
                {
                    currentAction.InteruptedAction(this, gameTime);
                    currentAction = actionQueue.Dequeue();
                }
                currentAction.Execute(this, gameTime);
                actionQueue.Clear();
            }
            else
            {
                currentAction.Execute(this, gameTime);
                if (currentAction.GetType() != typeof(A_Idle))
                    actionQueue.Enqueue(new A_Idle());
            }
            collision.Update(gameTime);
            IncreaseBoostPower();
            gravity(gameTime);
            animator.Location = new Vector2(Position.X + rectSize.X / 2, Position.Y + rectSize.Y);
            characterEngine.Update();
            attackHitEngine.Update();
            boosterEngine.Update();
            animator.Update(gameTime);
            if (attackBox != null)
                attackBox.Update();
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            gearParticleList.Add(Game.Content.Load<Texture2D>("Particles/paper_clip"));
            gearParticleList.Add(Game.Content.Load<Texture2D>("Particles/paper_cog"));
            gearParticleList.Add(Game.Content.Load<Texture2D>("Particles/paper_screw"));
            gearParticleList.Add(Game.Content.Load<Texture2D>("Particles/paper_spring"));
        }
        private void IncreaseBoostPower()
        {
            if (boostPower < maxBoostPower)
                boostPower++;
        }
        private void gravity(GameTime gameTime)
        {
            if (direction.Y < maxfallSpeed)
                direction.Y += acceleration;

            tocheck += direction * fallspeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            List<Rectangle> collidingBlocks = collision.testCollision();
            List<Rectangle> collidingEnemys = collision.enemyCollision();
            if (collidingBlocks == null && collidingEnemys == null)
            {
                Position += direction * fallspeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (onGround == true)
                    onGround = false;
            }
            else
            {
                if (onGround == false)
                {
                    bool retardet = false;  //fixa senare, dummy bool bör inte finnas borde ändra metoden

                    if (collidingEnemys != null)
                    {
                        Position = WallCollision.GetWallCollision(collidingEnemys, gameTime, GetNextHitRect, tocheck, out retardet);
                        collidingBlocks = collision.testCollision();
                    }
                    if (collidingBlocks != null)
                    {
                        Position = WallCollision.GetWallCollision(collidingBlocks, gameTime, GetNextHitRect, tocheck, out onGround);
                        characterEngine.EmitterLocation = animator.Location;
                        characterEngine.EmitterLocation.X += 5;
                        characterEngine.AddDustParticle(40);
                        //onGround = true;
                    }
                }
                tocheck = new Vector2(Position.X, Position.Y);
                direction = Vector2.Zero;
            }

        }
        public override void Draw(GameTime gameTime)
        {
            if (camera != null)
            {
                spritebatch.Begin(SpriteSortMode.Deferred, null, SamplerState.AnisotropicClamp, null, null, null, camera.viewMatrix);
                characterEngine.Draw(spritebatch);
                attackHitEngine.Draw(spritebatch);
                boosterEngine.Draw(spritebatch);

                #region Debug
                // Uncomment to show hit rectangles
                //spritebatch.Draw(testRectTexture, GetNextHitRect, Color.Red);
                //spritebatch.Draw(testRectTexture, GetAttackRect, Color.Red);
                #endregion
                spritebatch.End();
            }
            base.Draw(gameTime);

        }
        public void GotHit(Attack localAttack)
        {
            if (currentAction.GetType() != typeof(A_KnockedBack))
            {
                attackHitEngine.EmitterLocation = Position;
                attackHitEngine.AddExplosionParticle(20);

                health -= localAttack.Damage;
                SetInput(new A_KnockedBack(15, new Vector2(localAttack.User.Position.X, localAttack.User.Position.Y), this));
            }
        }
        public void NotifyWallCollision(List<Rectangle> collidedObj, GameTime gameTime)   //kallas när man kolliderar med en vägg, tar in alla vägg tiles som den kolliderar med, fixa positionen svårt som fan
        {
            direction = Vector2.Zero;
            actionQueue.Clear();
            // position = WallCollision.GetWallCollision(collidedObj, gameTime, GetHitRectangle, position, out onGround);    //retunerar den nya positionen efter man kolliderat med en vägg
        }
        public void SetInput(GameAction Action)
        {
           


            // If case prevents bad action handling:
            // Holding booster button prevents character to move if booster bar is empty.
            //if ()  //om man försöker pausa spelet
            //{

            //}
            if (Action.GetType() == typeof(A_KnockedBack))
                actionQueue.Clear();
            if (Action.GameState != Game1.GameState.Undefined)
                Game1.CurrentGameState = Action.GameState;
            else if ((Action.ActionName == "booster" && BoostPower > 0) || Action.ActionName != "booster")
                actionQueue.Enqueue(Action);
            
        }
        public void EndAction()
        {
            currentAction = new A_Idle();
        }
    }
}