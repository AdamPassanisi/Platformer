using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Player
    {
        #region Fields


        GraphicsDeviceManager graphics; // dimensions & scaling

        public AnimationManager _animationManager; // manage animation

        public Dictionary<string, Animation> _animations; //store dict if animations accessed by key

        public Vector2 _position ;

        private Boolean isAttacking;

        public Boolean IsAttacking { get { return isAttacking; } set { isAttacking = value; } }

        public bool Contact = false;

        protected Vector2 _prevPos;

        public Texture2D _texture;

        #endregion

        #region Properties

        private int health;

        private int totalDistance;
        
        public float TotalDistance { get { return totalDistance; }set { value = totalDistance; } }
        public int Health { get { return health; } set { health = value; } }
        public int Lives  { get; set; }

        private bool isAlive;
        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }

        




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
        public float Speed = 1f;


        public Vector2 Velocity;

        public Vector2 Acceleration = new Vector2(9.8f,0);


        Boolean hasJumped = false;

        public Boolean jumping = false;

        int jumpCount = 0;

        Boolean falling = false;

        public bool grounded = false;

        // x co-ordinate movement
        // using this var for moving background along with the character
        public float Xtrans = 0;

        public bool isHalfway = false;

        #endregion

        #region Methods

       

        // draw player on screen
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, _position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
            else throw new Exception("error animation mngr");
        }

        public virtual void Move(Dictionary<string, SoundEffect> soundEffects)
        {

            
            if (Keyboard.GetState().IsKeyDown(Keys.Left)&&Position.X>this._animations.ElementAt(0).Value.FrameWidth)
                Velocity.X = -Speed;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) )

            {
                Velocity.X = Speed;

            }

            // Jump
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && grounded)
            {
                
                if (jumpCount == 0)
                    jumpCount = 150;
                jumping = true;
                grounded = false;
                hasJumped = true;
                soundEffects["Jump"].Play();
                

            }

            if (jumpCount > 0)
            {
               
                    jumpCount--;
                // jump speed
                _position.Y -= graphics.PreferredBackBufferHeight/600f;

                if (jumpCount == 0)
                {
                    
                    hasJumped = false;
                    jumping = false;
                }
            }                   

          
          
        


        }

        protected virtual void SetAnimations()
        {
            

             if (IsAttacking)
            {
                _animationManager.Play(_animations["attack"]);
                IsAttacking = false;

            }
           else if (Velocity.X > 0)
                _animationManager.Play(_animations["WalkRight"]);
  
            else if (Velocity.X < 0)

            {
                _animationManager.Play(_animations["WalkLeft"]);
            }
            else _animationManager.Stop();
        }
        

    public Player(Dictionary<string, Animation> animations, GraphicsDeviceManager g)
    {
      graphics = g;
      _animations = animations;
      _animationManager = new AnimationManager(_animations.First().Value);
            health = 100;
            IsAlive = true;
    }

    public Player(Texture2D texture)
    {
      _texture = texture;
    }



        public virtual void Update(GameTime gameTime, List<Player> sprites, Dictionary<string, SoundEffect> soundEffects)
        {


            Move(soundEffects);

            SetAnimations();

            _animationManager.Update(gameTime);

            
            Position += Velocity;

            
            Xtrans = Velocity.X;
            TotalDistance += Xtrans;
            _prevPos = Position;



            if (_position.X > graphics.PreferredBackBufferWidth/2 )
            {
                
                Xtrans = _position.X- graphics.PreferredBackBufferWidth / 2;
                _position.X = (float)(graphics.PreferredBackBufferWidth * 0.5);
                isHalfway = true;

              

            }
            else if (_position.X < 0)
            {
                Xtrans = 0;
                _position.X = 0;

            }
            else
            {
                isHalfway = false;
            }

            Velocity = Vector2.Zero;
        }

        public bool IsTouching(Tile tile,Player sprite)
        {

            return _position.X +25f +this.Velocity.X >= tile.
                position.X && this._position.Y < tile.position.Y  && this._position.X + this.Velocity.X +10f<= tile.

          
                position.X+Tile.Texture.Width;

                
             
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

       public void CheckHealth()
        {
            if (this.Health <= 0)
            {
                IsAlive = false;
            }
        }

        public void Attack(Enemy enemy)
        {
            if ((this._position.X - enemy._position.X) <= 170
                & (this._position.X - enemy._position.X) > -70 )
            {
                // this.Velocity = Vector2.Zero;
                this.isAttacking = true;
                enemy.Health -= 34;
               
            }
          
            else
            {
                isAttacking = false;
            }

        }
        #endregion
        // checks if player is near door
        // cannot be above the door for level promotion
        public bool hasEntered(Door door, Dictionary<string, SoundEffect> soundEffects)
        {
            if (this._position.X > door.position.X && this._position.X < door.position.X + Door.Texture.Width && (this._position.Y > door.position.Y)
                ) {
                this.Health = 300;
                soundEffects["Victory"].Play();
            return true;
            }
            return false;
        }

        // setting default values for new level/spawn
        public void Reset()
        {
            this.Health = 100;
            this.TotalDistance = 0;
            this.Position = new Vector2(0, (int)((0.858) * graphics.PreferredBackBufferHeight));
        }
    }
}