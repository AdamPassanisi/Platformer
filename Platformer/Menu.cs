﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{


    public class Menu : Game
    {
         Texture2D singleplayer;


        // Single Player
        // Multiplayer
        // Options 
        // Exit
        // Leaderboard
       

        
        public SpriteBatch spritebatch;

        public Menu(GraphicsDevice g)
        {
            spritebatch = new SpriteBatch(g);
        }

    }
}
