using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    public class AnimationManager
  {
    private Animation _animation;

    private float _timer;

        public int duration;

        public Vector2 Position { get; set; }

    public AnimationManager(Animation animation)
    {
      _animation = animation;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(_animation.Texture,
                       Position,
                       new Rectangle(_animation.CurrFrame * _animation.FrameWidth,
                                     0,
                                    _animation.FrameWidth,
                                    _animation.FrameHeight),
                      Color.White);
           
        }

    public void Play(Animation animation)
    {
      if (_animation == animation)
        return;

      _animation = animation;

      _animation.CurrFrame = 0;

      _timer = 0;
    }

        public void Play(Animation animation, int duration)
        {
            if (_animation == animation)
                return;

            _animation = animation;

            _animation.CurrFrame = 0;

            _timer = duration;
        }

        public void PlayReverse(Animation animation, int duration)
        {
            if (_animation == animation)
                return;

            _animation = animation;

            _animation.CurrFrame = 4;

            _timer = duration;
        }




        public void Stop()
    {
      _timer = 0f;

      _animation.CurrFrame = 0;
    }

    public void Update(GameTime gameTime)
    {
      _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

      if(_timer > _animation.FrameSpeed)
      {
        _timer = 0f;

        _animation.CurrFrame++;

        if (_animation.CurrFrame >= _animation.FrameCount)
          _animation.CurrFrame = 0;
      }
    }


        public void LeftUpdate(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;

                _animation.CurrFrame--;

                if (_animation.CurrFrame >= _animation.FrameCount)
                    _animation.CurrFrame = 4;
            }
        }


        public bool AttackUpdate(GameTime gameTime,int duration)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
           


            if (_timer > _animation.FrameSpeed&&_timer<=duration)
            {
                _timer = 0f;
                _animation.CurrFrame++;
                if (_animation.CurrFrame >= _animation.FrameCount)
                    _animation.CurrFrame = 0;

            }
            if (_timer > duration)
            {
                _animation.CurrFrame = 0;
                _timer = 0;
                return true;
            }
            return false;

         
        }
    }
}