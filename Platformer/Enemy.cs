using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Enemy
    {


        #region Fields

        SpriteEffects se;


        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;


        public Vector2 _position;

        public int attack_counter; // how many times enemy has attacked the player

        private Boolean isAttacking;
        public Boolean IsAttacking { get { return isAttacking; }set { isAttacking = value; } }
        
        public Texture2D _texture;

        private int health;
        public int Health { get { return health; } set { health = value; } }

        public bool facingRight;

        public int screenWidth =GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        //  velocity of the enemy
        public Vector2 Velocity;


        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }


        public float Speed = 10f;
       // public float Speed = 3f;




        public Vector2 Acceleration = new Vector2(9.8f, 0);


        // x co-ordinate movement
        // using this var for moving background along with the character
        public int Xtrans = 0;

        #endregion
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D walker;
        Rectangle walker2;
        #region Methods


        public Enemy(Dictionary<string, Animation> animations)
        {
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
            health = 100;
        }

        public Enemy(Texture2D texture)
        {
            _texture = texture;
           
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, _position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
            else throw new Exception("error animation mngr");
        }


        public virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && _position.X > 50)
                Velocity.X = -Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))

            {
                Velocity.X = Speed;

            } 

        }


        protected virtual void SetAnimations()
        {

            if (isAttacking)
            {
                if (facingRight)
                {
                   
                    _animationManager.Play(_animations["enemyattackR"]);
                  // isAttacking = false;
                    attack_counter++;

                }
                else if(!facingRight)

                {
                  
                    _animationManager.Play(_animations["enemyattackL"]);
                    attack_counter++;
                 //  isAttacking = false;
                }
               
               
            }
            
           
           else  if (Velocity.X > 0)
                _animationManager.Play(_animations["enemywalkR"]);
            else if (Velocity.X < 0)

            {
                _animationManager.Play(_animations["enemywalkL"]);

            }
            //else if (Velocity.X == 0 && facingRight)

            //{
            //    _animationManager.Play(_animations["enemyidleR"]);

            //}
            //else if (Velocity.X == 0 && !facingRight)

            //{
            //    _animationManager.Play(_animations["enemyidleL"]);

            //}


        }



        public Enemy(Dictionary<string, Animation> animations, GraphicsDeviceManager g)
        {
            graphics = g;
            _animations = animations;
            _animationManager = new AnimationManager(_animations.First().Value);
        }

        



        

        public bool IsTouching(Tile tile, Player sprite
            )
        {

            return _position.X + 25f + this.Velocity.X >= tile.
                position.X && this._position.Y < tile.position.Y && this._position.X + this.Velocity.X + 10f <= tile.


                position.X + Tile.Texture.Width;



        }

        // Returns true if player is on top the tile, false otherwise
        public bool tileTouching(Tile tile, Player player)
        {
            // Checks if the player is in bounds horizontally 
            if ((player._position.X >= tile.position.X) && (player._position.X <= tile.position.X + Tile.Texture.Width))
                // Checks if the player is at the right height 
                if ((player._position.Y <= tile.position.Y))
                    return true;
                else
                    return false;
            else
                return false;
        }

        #endregion


        private void RandomMove(Player player)
        {
            Random r = new Random();
            //&& this.Position.X > screenWidth / 2 && this.Position.X > screenWidth / 3
            int cap = 32;

            /*
             * reduce cap when moving further away so that 
             * he can escape
             */
            if (r.Next(0, 50) > cap && !isAttacking)


            {

                 if (player._position.X  > this._position.X ) { 
                /*if (player._position.X  > this._position.X+this._animations.ElementAt(0).Value.FrameWidth
                    ) {*/ 
                    if (player.isHalfway)
                        this.Velocity.X =0f;
                    else
                    {
                        this.Velocity.X =1f;
                        facingRight = true;
                    }
                }
            
            else if (this.Position.X >player._position.X )
                {
                    if (player.isHalfway)
                        this.Velocity.X = -2f;
                    else
                    {
                        this.Velocity.X = -1f;
                        facingRight = false;
                    }
                }
                


            }
            else
            {
                this.Velocity = Vector2.Zero;
            }


            if (!IsAttacking&&attack_counter<1)
            {
                this.Attack(player);
            }
            if (attack_counter>=1)
            {
                this.IsAttacking = false;
            }
        }

        public void Reset()
            {
                attack_counter = 0;
            this._position = new Vector2(700, (int)((0.838) * graphics.PreferredBackBufferHeight));
            }

        public virtual void Update(GameTime gameTime, Player player)
        {
            
            
            
                RandomMove(player);
                Position += Velocity;
                _animationManager.Update(gameTime);
            
            if (collision(player, this))
                ;


            SetAnimations();





            Velocity = Vector2.Zero;
           // this.IsAttacking = false;

        }

        // Tell if enemy is touching the player
        public bool collision(Player user, Enemy enemy)
        {
            // Checks horizontal
           /* if ((enemy._position.X >= user._position.X) && (enemy._position.X <= user._position.X + user._texture.Width))
                return true;

            else*/
                return false;
        }


        private void Attack
            (Player player)
        {
            //if ((this._position.X-player._position.X)<=player._animations.ElementAt(0).Value.FrameWidth / 6
            //    & (this._position.X > player._position.X)&& player.IsAlive)
            //{
            //   // this.Velocity = Vector2.Zero;
            //    this.isAttacking=true;
            //    player.Health = player.Health-34;
            //    player.CheckHealth();
            //}
            //else if ((this._position.X - player._position.X)>= 50 && (this._position.X - player._position.X) < 0 && player.IsAlive)
            //{
            //    this.isAttacking = true;
            //    player.Health = player.Health - 34;
            //    player.CheckHealth();
            //}
            if ((this._position.X - player._position.X>-100) &&(this._position.X - player._position.X < 1) && !(this.Position.Y>player.Position.Y))
            {
                // this.Velocity = Vector2.Zero;
                this.isAttacking = true;
                player.Health = player.Health - 34;
                player.CheckHealth();
            }
           
           
            else
            {
                isAttacking = false;
            }
            
        }

    }
}
