﻿/*
 * Authors
 * 
 * 
 * Austin Cultra
 * Adam Passanissi
 * Deepak Damera
 * 
 * Project for CS496
 * 
 */

using System;

namespace Platformer
{




#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            using (var game = new Game1())


               game.Run();
            
        }
    }
#endif
}
