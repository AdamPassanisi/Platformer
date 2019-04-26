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

        protected AnimationManager _animationManager;

        protected Dictionary<string, Animation> _animations;

        public Vector2 _position;// { get; set; }

        protected Vector2 _prevPos;

        public Texture2D _texture;

        #endregion

        #region Properties
        private Boolean isAlive { get; set; }




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

        public float Speed = 3f;

        public Vector2 Velocity;


        Boolean hasJumped = false;



        // x co-ordinate movement
        // using this var for moving background along with the character
        public int Xtrans = 0;

        #endregion
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D walker;
        Rectangle walker2;


    }
}
