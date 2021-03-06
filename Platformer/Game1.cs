﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Platformer
{




    public class Game1 : Game
    {

        // different states
        // switching transfer control n displays the set state
        private enum GameState
        {
            MainMenu,
            Login,
            Level1,
            Level2,
            Finish,
            Instructions, 
            CreateAccount,
            Leaderboards,
            LevelCompleted,
            GameOver,
            Pause,
            Continue
        }
        GameState _state = GameState.MainMenu;
        GameState _previousState = GameState.Level1;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
      
       


        List<string> username = new List<string>();
        List<string> password = new List<string>();

        String beingTyped = "user";

        float[] colors = { 0.5f, 0.0f, 0.0f };

        // Create Account
        float[] Createcolors = { .5f, 0f, 0f, 0f };
        List<string> Createusername = new List<string>();
        List<string> Createpassword = new List<string>();
        String CreatebeingTyped = "user";
        bool Createfirst = false;

        Texture2D titlescreen, gameover;
        Texture2D titlescreen_a;
        Scrolling scrolling1;
        Scrolling scrolling2;
        // Level 2 
        Scrolling nightscrolling1, nightscrolling2; 

        // List of tiles to display on platform
        List<Tile> tiles = new List<Tile>();

        List<Tile> nightTiles = new List<Tile>();

        // exit door
        Door finish_line;

        Door finish_line2;

        int opacDirection = 1;
       
           

        // fit user's screen bounds
        Rectangle titleScreen = new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        // Title Screen //

        MouseState mouse = Mouse.GetState();

        bool firstboard = true;
        List<string>[] board;

        // Heealth bar
        HealthBar healthBar;
        Texture2D healthTexture;
        Rectangle healthRectangle;

        // sprite list
        private List<Player> _sprites;
        
        private Enemy enemy;
        private Enemy enemy2;
        private Enemy enemy3;



        // Database Connection
        ConnectDB db = new ConnectDB();

        Texture2D instructs;


        

        // Initialize controller/keyboard
        GamePadState controller = GamePad.GetState(PlayerIndex.One);
        KeyboardState keyboard = Keyboard.GetState();
        KeyboardState currentState;
        KeyboardState previousState;

       

        

        int select = 0;
        
        Texture2D paused, levelcompleted, continueWithoutSaving,Continue, createaccountbutton, viewLeaderboards, exit, instructions, multiplayer, newGame, returnToMainMenu, saveContinue, singePlayer, startGame, tryAgain;
        Point buttonSize;

        Texture2D logintitle, usernametitle, passwordtitle, enter;
        Texture2D createaccount, confirmpassword;
        bool firstLog = true;
        bool enterable = false;
        bool incorrectLogin = false;
        bool Createenterable = false;

        Texture2D incorrect;
        Texture2D usernametaken, badlength;
        bool BadLength = false;
        bool usertaken = false;

        Texture2D leaderboards, leaderboards2;
        List<String> ranks = new List<String>();
        List<String> users = new List<String>();
        List<String> scores = new List<String>();
        int leaderboardStart = 0;
        int leaderboardLevel = 1;

        // Logged in
        bool LOGGED_IN = false;
        String Logged_Username;
        String Logged_Password;
        bool firstBeaten = true;
        int deathCounter = 0;
        SpriteFont font;

        //calculates and stores elapsed time since the game has started
        Rectangle time= new Rectangle(700,100,200,100);
       
        float  elapsed_time;
        //private SpriteFont font;

        Texture2D finishline;

        // sound effects
        // Audio credits in README file
        Song lobby_music;
        SoundEffect jumpSound;
        SoundEffect deathSound;
        SoundEffect victorySound;
        private Dictionary<string, SoundEffect> soundEffects;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Sets the game to 1080p fullscreen by default
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            //graphics.IsFullScreen = true;


        }

        #region Level Completed
        public void LevelCompleted(GameTime gameTime)
        {
            _sprites[0].Reset();
            deathCounter = 0;
            previousState = currentState;
            currentState = Keyboard.GetState();
            
              if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _state = GameState.MainMenu;
            if (currentState.IsKeyDown(Keys.Enter) && previousState.IsKeyUp(Keys.Enter))
            {
                
                enemy._position.X = 2000;
                enemy2._position.X = 1000;
                enemy3._position.X = 1500;
                elapsed_time = 0f;
                _state = GameState.Level2;
               
            }
            GraphicsDevice.Clear(Color.Silver);


            spriteBatch.Begin();

            spriteBatch.Draw(levelcompleted, new Rectangle(0,0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.DrawString(font, ((int)(100000f - elapsed_time/10f)).ToString(), new Vector2(graphics.PreferredBackBufferWidth / 1.85f, graphics.PreferredBackBufferHeight / 2.7f), Color.White);
      
            spriteBatch.End();
        }
        #endregion

        #region Leaderboards
        public void Leaderboards(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
               {
                firstboard = true;
                _state = GameState.MainMenu;
                }

            previousState = currentState;
            currentState = Keyboard.GetState();
            int z = 0;
            String[] titles = { "Username", "Rank", "Score" };
  

            GraphicsDevice.Clear(Color.DarkRed);
          
            // Will only pull from database when the leaderboards page is opened
            if (firstboard)
            {
                
                firstboard = false;

               // Empties lists so they are no duplicates
                users.Clear();
                ranks.Clear();
                scores.Clear();
                
                
                // Gets each entry from database
                foreach (var i in db.viewLeaderboards(leaderboardLevel))
                {
                    
                    int y = 0;
                    Console.WriteLine(titles[z]);
                    foreach (var j in i)
                    {
                        switch(z)
                        {
                            case 0:
                                users.Add(i[y]);
                                break;
                            case 1:
                                ranks.Add(i[y]);
                                break;
                            case 2:
                                scores.Add(i[y]);
                                break;
                            default:
                                break;
                        }
                        
                        y++;
                    }
                    Console.WriteLine();
                    z++;
                }
             

            }



            spriteBatch.Begin(); 
            // DIsplays which level of the leaderboard is loaded
            if(leaderboardLevel == 1)
                spriteBatch.Draw(leaderboards, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            else
                spriteBatch.Draw(leaderboards2, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

            int len = ranks.Count;
            
            // Scrolls through leaderboards with arrow keys
            if (currentState.IsKeyDown(Keys.Down) && previousState.IsKeyUp(Keys.Down))
            {
                if (leaderboardStart < len - 1)
                    leaderboardStart++;
            }
            // Scrolls through leaderboards with arrow keys
            if (currentState.IsKeyDown(Keys.Up) && previousState.IsKeyUp(Keys.Up))
            {
                if (leaderboardStart > 0)
                    leaderboardStart--;
            }
            // Scrolls through leaderboards with arrow keys
            if(currentState.IsKeyDown(Keys.Right))
                {
                    leaderboardLevel = 2;
                firstboard = true;
                }
            // Scrolls through leaderboards with arrow keys
            if(currentState.IsKeyDown(Keys.Left))
                {
                    leaderboardLevel = 1;
                    firstboard = true;
                }
            // Rank
            for (int i = leaderboardStart; i < len; i++)
                spriteBatch.DrawString(font, ranks[i].ToString(), new Vector2(graphics.PreferredBackBufferWidth / 10, (graphics.PreferredBackBufferHeight / (2.5f)) + (graphics.PreferredBackBufferHeight * (i - leaderboardStart) /20f)), Color.White);
            // Username
            for (int i = leaderboardStart; i < len; i++)
                spriteBatch.DrawString(font, users[i], new Vector2(graphics.PreferredBackBufferWidth / 2.5f, (graphics.PreferredBackBufferHeight / (2.5f)) + (graphics.PreferredBackBufferHeight * (i - leaderboardStart) / 20f)), Color.White);
            // Score
            for (int i = leaderboardStart; i < len; i++)
                spriteBatch.DrawString(font, scores[i], new Vector2(graphics.PreferredBackBufferWidth / 1.2f, (graphics.PreferredBackBufferHeight / (2.5f)) + (graphics.PreferredBackBufferHeight * (i - leaderboardStart) / 20f)), Color.White);



            spriteBatch.End();




            base.Update(gameTime);

        }



        #endregion

        #region Pause
        public void Pause(GameTime gameTime)
        {
            previousState = currentState;
            currentState = Keyboard.GetState();

            // Escape allows takes user to main menu
            if (currentState.IsKeyDown(Keys.Escape) && previousState.IsKeyUp(Keys.Escape))
                _state =  GameState.MainMenu;
            // enter takes user to previous screen
            if (currentState.IsKeyDown(Keys.Enter) && previousState.IsKeyUp(Keys.Enter))
                _state = _previousState;
            spriteBatch.Begin();
            spriteBatch.Draw(paused, new Rectangle(0,0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();

             base.Update(gameTime);
        }
        #endregion


        #region Instructions
        public void Instructions(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _state = GameState.MainMenu;

            previousState = currentState;
            currentState = Keyboard.GetState();

            GraphicsDevice.Clear(Color.DarkRed);
            spriteBatch.Begin(); 
            spriteBatch.Draw(instructs, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.End();

            base.Update(gameTime);
        }
        #endregion
        // Creates a new account on the server
        #region Create Account
        public void CreateAccount(GameTime gameTime)
        {
            String USERNAME;
            String PASSWORD;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _state = GameState.MainMenu;

            // Updates keyboard states for typing recognition
            previousState = currentState;
            currentState = Keyboard.GetState();

            if (Createfirst)
            {
                if (currentState.GetPressedKeys().Length == 0)
                {
                    CreatebeingTyped = "user";
                    Createfirst = false;
                }
            }

            int height = graphics.PreferredBackBufferHeight;
            int width = graphics.PreferredBackBufferWidth;


            GraphicsDevice.Clear(Color.DarkRed);
            spriteBatch.Begin(); 
         
            Keys c;

            // User is entering username
            if (CreatebeingTyped == "user")
            {
               // Makes sure a key is being pressed
                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];

                    if (previousState.GetPressedKeys().Length > 0)
                    {
                        if (previousState.GetPressedKeys()[0] != c)

                        {
                            // If backspace is being pressed, last entry in list is removed
                            if (c == Keys.Back)
                            {
                                if (Createusername.Count != 0)
                                    Createusername.RemoveAt(Createusername.Count - 1);
                            }
                            else if (c == Keys.OemSemicolon)
                            {
                                ;
                            }
                            // No special character was pressed, character is added to list
                            else
                            {
                                c.ToString()[c.ToString().Length - 1].ToString();
                                Createusername.Add(c.ToString()[c.ToString().Length - 1].ToString());
                            }
                        }

                    }
                    else
                    {
                        // If backspace is being pressed, last entry in list is removed
                        if (c == Keys.Back)
                        {
                            if (Createusername.Count != 0)
                                Createusername.RemoveAt(Createusername.Count - 1);
                        }
                        // Tab is pressed, tpying selection goes to password field
                        else if (c == Keys.Tab)
                        {
                            Createcolors[0] = 0f;
                            Createcolors[1] = .5f;
                            Createcolors[2] = 0f;
                            Createcolors[3] = 0f;

                            CreatebeingTyped = "password";
                        }
                        // Special buttons ignored
                        else if (c == Keys.Right || c == Keys.Left || c == Keys.Up || c == Keys.LeftShift || c == Keys.RightShift)
                        {

                            ;
                        }
                        // enter is pressed, enter button is selected
                        else if (c == Keys.Enter)
                        {
                            CreatebeingTyped = "enter";
                            Createcolors[0] = 0f;
                            Createcolors[1] = 0f;
                            Createcolors[2] = 0f;
                            Createcolors[3] = 0.5f;
                            //  enterable = true;
                        }

                        // down is pressed, typing selection goes to password entry
                        else if (c == Keys.Down)
                        {
                            beingTyped = "enter";
                            Createcolors[0] = 0f;
                            Createcolors[1] = 0.5f;
                            Createcolors[2] = 0;
                            Createcolors[3] = 0f;

                            CreatebeingTyped = "password";
                        }
                        // no special character was pressed, character is added to list
                        else
                        {
                            Createusername.Add(c.ToString()[c.ToString().Length - 1].ToString());
                        }

                    }


                }
            }

            // password entry is selected
            if (CreatebeingTyped == "password")
            {
                // makes sure a key is pressed
                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];

                    if (previousState.GetPressedKeys().Length > 0)
                    {
                        if (previousState.GetPressedKeys()[0] != c)

                        {
                            // backspace is pressed, last character is removed from list
                            if (c == Keys.Back)
                            {
                                if (Createpassword.Count != 0)
                                    Createpassword.RemoveAt(Createpassword.Count - 1);
                            }

                            // character is added to list
                            else
                            {

                                Createpassword.Add(c.ToString()[c.ToString().Length - 1].ToString());
                            }
                        }

                    }
                    else
                    {
                        // backspace is pressed, last character is removed from list
                        if (c == Keys.Back)
                        {
                            if (Createpassword.Count != 0)
                                Createpassword.RemoveAt(Createpassword.Count - 1);
                        }
                        // up is pressed, selection is changed to username being typed
                        else if (c == Keys.Up)
                        {
                            Createcolors[0] = 0.5f;
                            Createcolors[1] = 0f;
                            Createcolors[2] = 0f;
                            Createcolors[3] = 0f;

                            CreatebeingTyped = "user";
                        }
                        // ignores special characters
                        else if (c == Keys.Right || c == Keys.Left || c == Keys.Up || c == Keys.Tab || c == Keys.Down || c==Keys.LeftShift || c==Keys.RightShift)
                        {

                            ;
                        }
                        // enter is pressed, enter button is selected
                        else if (c == Keys.Enter)
                        {
                            CreatebeingTyped = "enter";
                            Createcolors[0] = 0f;
                            Createcolors[1] = 0f;
                            Createcolors[2] = 0f;
                            Createcolors[3] = 0.5f;
                            
                        }
      
                   
                        // character is added to list
                        else
                        {
                            Createpassword.Add(c.ToString()[c.ToString().Length - 1].ToString());
                        }

                    }


                }
            }


            // enter button is selected
            if (CreatebeingTyped == "enter")
            {
                c = Keys.None;
                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];
                }
                // up is pressed, username field is selected for entry
                if (c == Keys.Up)
                {
                    CreatebeingTyped = "user";
                    Createcolors[0] = 0.5f;
                    Createcolors[1] = 0f;
                    Createcolors[2] = 0f;
                    Createcolors[3] = 0f;
                    Createenterable = false;
                }

                if (currentState.GetPressedKeys().Length == 0)
                    Createenterable = true;
                if (Createenterable)
                {
                    if (currentState.IsKeyDown(Keys.Enter))
                    {

                        USERNAME = String.Join(String.Empty, Createusername.ToArray());
                        PASSWORD = String.Join(String.Empty, Createpassword.ToArray());
                        // Checks that a valid username and password are given
                        if ((USERNAME.Length >= 3 && USERNAME.Length <= 40) && (PASSWORD.Length >= 8 && PASSWORD.Length <= 20))
                        {
                            
                            // Code to create account
                            // Front end hashes the password with the username as a salt and stores it in database
                            if (db.createAccount(USERNAME, Account.GenerateHash(PASSWORD, USERNAME)))
                            {

                                _state = GameState.MainMenu;

                            }
                            else
                            {
                                BadLength = false;
                                usertaken = true;
                            }
                        }
                        else
                        {
                            usertaken = false;
                            BadLength = true;
                        }

                    }
                }
            }


            

            string drawPassword = "";
            for (int i = 0; i < String.Join(String.Empty, Createpassword.ToArray()).Length; i++)
                drawPassword += "*";

            spriteBatch.DrawString(font, String.Join(String.Empty, Createusername.ToArray()), new Vector2(width / 2, height / 16 + buttonSize.Y * 3), Color.White);
            spriteBatch.DrawString(font, drawPassword, new Vector2(width / 2, height / 16 + buttonSize.Y * 5), Color.White);

            spriteBatch.Draw(createaccount, new Rectangle(new Point(width / 2 - buttonSize.X, height / 16), new Point(buttonSize.X * 2, buttonSize.Y * 3)), Color.White);
            spriteBatch.Draw(usernametitle, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 2), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + Createcolors[0]));
            spriteBatch.Draw(passwordtitle, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 4), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + Createcolors[1]));

            spriteBatch.Draw(enter, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 6), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + Createcolors[3]));
            if(usertaken)
                spriteBatch.Draw(usernametaken, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 8), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White );
            if(BadLength)
                spriteBatch.Draw(badlength, new Rectangle(new Point(width  - buttonSize.X * 2, height / 16 + buttonSize.Y * 5), new Point(buttonSize.X * 2, buttonSize.Y * 4)), Color.White);





            spriteBatch.End();
        }
        #endregion

        // Logs the user in to the server
        #region Login
        public void Login(GameTime gameTime)
        {
            String USERNAME;
            String PASSWORD;

            int height = graphics.PreferredBackBufferHeight;
            int width = graphics.PreferredBackBufferWidth;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _state = GameState.MainMenu;

            // Updates keyboard states for typing recognition
            previousState = currentState;
            currentState = Keyboard.GetState();

            GraphicsDevice.Clear(Color.DarkRed);
            spriteBatch.Begin(); //

            

            // Checks if its the first time running this function, sets the selection to username input if so
            if(firstLog)
            {
                if (currentState.GetPressedKeys().Length == 0)
                {
                    beingTyped = "user";
                    firstLog = false;
                }
            }

         
            Keys c;

            // User is entering username
            if (beingTyped == "user")
            {
               // Makes sure a key is being pressed
                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];

                    if (previousState.GetPressedKeys().Length > 0)
                    {
                        if (previousState.GetPressedKeys()[0] != c)

                        {
                            // If backspace is being pressed, last entry in list is removed
                            if (c == Keys.Back)
                            {
                                if (username.Count != 0)
                                    username.RemoveAt(username.Count - 1);
                            }

                            else if (c == Keys.OemSemicolon)
                            {
                                ;
                            }
                            // No special character was pressed, character is added to list
                            else
                            {
                               username.Add(c.ToString()[c.ToString().Length - 1].ToString());
                            }
                        }

                    }
                    else
                    {
                        // If backspace is being pressed, last entry in list is removed
                        if (c == Keys.Back)
                        {
                            if (username.Count != 0)
                                username.RemoveAt(username.Count - 1);
                        }
                        // Tab is pressed, tpying selection goes to password field
                        else if (c == Keys.Tab)
                        {
                            colors[0] = 0f;
                            colors[1] = .5f;
                            colors[2] = 0f;

                            beingTyped = "password";
                        }
                        // Special buttons ignored
                        else if (c == Keys.Right || c == Keys.Left || c == Keys.Up || c == Keys.Escape || c == Keys.RightShift || c == Keys.LeftShift || c == Keys.OemSemicolon)
                        {

                            ;
                        }
                        // enter is pressed, enter button is selected
                        else if (c == Keys.Enter)
                        {
                            beingTyped = "enter";
                            colors[0] = 0f;
                            colors[1] = 0f;
                            colors[2] = 0.5f;
                          
                        }

                        // down is pressed, typing selection goes to password entry
                        else if (c == Keys.Down)
                        {
                            colors[0] = 0f;
                            colors[1] = .5f;
                            colors[2] = 0f;

                            beingTyped = "password";
                        }
                        // no special character was pressed, character is added to list
                        else
                        {
                            username.Add(c.ToString()[c.ToString().Length - 1].ToString());
                        }

                    }


                }
            }


            // User is entering password
            if (beingTyped == "password") {
                // makes sure a key is pressed
                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];

                    if (previousState.GetPressedKeys().Length > 0)
                    {
                        if (previousState.GetPressedKeys()[0] != c)

                        {
                            // backspace is pressed, last character is removed from list
                            if (c == Keys.Back)
                            {
                                if (password.Count != 0)
                                    password.RemoveAt(password.Count - 1);
                            }

                           // character is added to list
                            else
                            {
                                password.Add(c.ToString()[c.ToString().Length - 1].ToString());
                            }
                        }

                    }
                    else
                    {
                        // backspace is pressed, last character is removed from list
                        if (c == Keys.Back)
                        {
                            if (password.Count != 0)
                                password.RemoveAt(password.Count - 1);
                        }
                        // ignores special characters
                        else if (c == Keys.Tab)
                        {
                           ;
                        }

                        // ignores special characters
                        else if (c == Keys.Right || c == Keys.Left || c == Keys.Down || c == Keys.Escape || c == Keys.RightShift || c == Keys.LeftShift || c == Keys.OemSemicolon)
                            ;
                        // up is pressed, selection is changed to username being typed
                        else if (c == Keys.Up)
                        {

                            colors[0] = 0.5f;
                            colors[1] = 0f;
                            colors[2] = 0f;

                            beingTyped = "user";
                           
                        }
                        // enter is pressed, enter button is selected
                        else if (c == Keys.Enter )
                        {
                            beingTyped = "enter";
                            colors[0] = 0f;
                            colors[1] = 0f;
                            colors[2] = 0.5f;
                        }
                        // character is added to list
                        else
                        {
                            password.Add(c.ToString()[c.ToString().Length - 1].ToString());
                        }
                    }


                }
            }
            // enter button is selected
            if (beingTyped == "enter")
            {
                c = Keys.None;
                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];
                }
                // up is pressed, username field is selected for entry
                if (c == Keys.Up)
                {
                    beingTyped = "user";
                    colors[0] = 0.5f;
                    colors[1] = 0f;
                    colors[2] = 0f;
                    enterable = false;
                }

                if (currentState.GetPressedKeys().Length == 0)
                    enterable = true;
                if(enterable)
                {
                    if (currentState.IsKeyDown(Keys.Enter))
                    {
                        USERNAME = String.Join(String.Empty, username.ToArray());
                        PASSWORD = String.Join(String.Empty, password.ToArray());
                        // Code to Log in
                        if (db.login(USERNAME, Account.GenerateHash(PASSWORD,USERNAME)))
                        {
                         
                            LOGGED_IN = true;
                            Logged_Username = USERNAME;
                            
                            _state = GameState.MainMenu;
                          
                            
                        }
                        else
                        {
                            incorrectLogin = true;
                        }

                    }
                }
            }

            if (incorrectLogin)
            {
                spriteBatch.Draw(incorrect, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 8), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White);

            }


            if(LOGGED_IN)
                _state = GameState.MainMenu;


            string drawPassword = "";
            for (int i = 0; i < String.Join(String.Empty, password.ToArray()).Length; i++)
                drawPassword += "*";
            
            spriteBatch.DrawString(font, String.Join(String.Empty, username.ToArray()), new Vector2(width / 2, height / 16 + buttonSize.Y * 3), Color.White);
            spriteBatch.DrawString(font,drawPassword, new Vector2(width / 2, height / 16 + buttonSize.Y * 5), Color.White);

            spriteBatch.Draw(logintitle, new Rectangle(new Point(width / 2 - buttonSize.X, height / 16), new Point(buttonSize.X * 2, buttonSize.Y * 3)), Color.White);
            spriteBatch.Draw(usernametitle, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 2), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + colors[0]));
           spriteBatch.Draw(passwordtitle, new Rectangle(new Point(width/ 2 - buttonSize.X*2, height/16+buttonSize.Y*4), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + colors[1]));

            spriteBatch.Draw(enter, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 6), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + colors[2]));


            spriteBatch.End();


          

            base.Update(gameTime);
        }
        #endregion
        // Called when program is first started
        protected override void Initialize()
        {
            CreateTiles();
            CreateNightTiles();
            base.Initialize();
            db.Initialize(); 
        }

        #region LoadContent
        protected override void LoadContent()
        {

            // load sound effect
            lobby_music = Content.Load<Song>("Audio/Lobby");

            
            jumpSound = Content.Load<SoundEffect>("Audio/Jump");
            deathSound = Content.Load<SoundEffect>("Audio/Heartbeat");
            victorySound = Content.Load<SoundEffect>("Audio/victory");
            soundEffects = new Dictionary<string, SoundEffect>();
            

            soundEffects.Add("Jump", jumpSound);
            soundEffects.Add("Death", deathSound);
            soundEffects.Add("Victory", victorySound);
            MediaPlayer.Volume = 0.99f;
            
            MediaPlayer.Play(lobby_music);
            
            paused = Content.Load<Texture2D>("pause");
            buttonSize = new Point(graphics.PreferredBackBufferWidth * 5 / 32, graphics.PreferredBackBufferHeight * 5/72);
            gameover = Content.Load<Texture2D>("gameover");
            font = Content.Load<SpriteFont>("font");
            levelcompleted = Content.Load<Texture2D>("levelcompleted");
            // Login Page
            usernametitle = Content.Load<Texture2D>("usernametitle");
            passwordtitle = Content.Load<Texture2D>("passwordtitle");
            logintitle = Content.Load<Texture2D>("logintitle");
            enter = Content.Load<Texture2D>("enter");

            leaderboards = Content.Load<Texture2D>("leaderboards");
            leaderboards2 = Content.Load<Texture2D>("leaderboards2");

            createaccount = Content.Load<Texture2D>("createaccount");
            confirmpassword = Content.Load<Texture2D>("confirmpassword");

            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;
            buttonSize = new Point(graphics.PreferredBackBufferWidth * 5 / 32, graphics.PreferredBackBufferHeight * 5/72);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            titlescreen = Content.Load<Texture2D>("titlescreen");
            titlescreen_a = Content.Load<Texture2D>("titlescreen(1)");

            instructs = Content.Load<Texture2D>("instructs");
            // Buttons
            createaccountbutton = Content.Load<Texture2D>("createaccountbutton");
            continueWithoutSaving = Content.Load<Texture2D>("continuewithoutsaving");
            exit = Content.Load<Texture2D>("exit");
            instructions = Content.Load<Texture2D>("instructions");
            multiplayer = Content.Load<Texture2D>("log");
            newGame = Content.Load<Texture2D>("newgame");
            Continue = Content.Load<Texture2D>("continue");
            returnToMainMenu = Content.Load<Texture2D>("returntomainmenu");
            saveContinue = Content.Load<Texture2D>("savecontinue");
            singePlayer = Content.Load<Texture2D>("singleplayer");
            viewLeaderboards = Content.Load<Texture2D>("viewleaderboards");
            startGame = Content.Load<Texture2D>("startgame");
            tryAgain = Content.Load<Texture2D>("tryagain");
            healthBar = new HealthBar(Content.Load<Texture2D>("Health"),new Vector2(400,400),100);
            healthTexture = Content.Load<Texture2D>("Health");

            incorrect = Content.Load<Texture2D>("incorrect");
            badlength = Content.Load<Texture2D>("badlength");
            usernametaken = Content.Load<Texture2D>("usernametaken");

            font = Content.Load<SpriteFont>("demo");

            // background
            // so once we scroll through one background we go onto the next
            scrolling1 = new Scrolling(Content.Load<Texture2D>("bigbackground"), new Rectangle(0, 0, screenWidth, screenHeight));
            scrolling2 = new Scrolling(Content.Load<Texture2D>("bigbackground"), new Rectangle(screenWidth, 0, screenWidth, screenHeight));

            nightscrolling1 = new Scrolling(Content.Load<Texture2D>("nightbackground"), new Rectangle(0, 0, screenWidth, screenHeight));
            nightscrolling2 = new Scrolling(Content.Load<Texture2D>("nightbackground"), new Rectangle(screenWidth, 0, screenWidth, screenHeight));
            
            finishline = Content.Load<Texture2D>("finishline");


            // loading tile textures here
            foreach (var _tile in tiles)
            {
                Tile.LoadContent(Content, 0);
            }

            foreach(var _TILE in nightTiles)
                {
                    Tile.LoadContent(Content, 0);
                }

            Door.LoadContent(Content,0);


            

            // adding animation set
            // will not be using idle
            
            var animations = new Dictionary<string, Animation>(){

                {"WalkRight",new Animation(Content.Load<Texture2D>("right"),8)},
                { "WalkLeft", new Animation(Content.Load<Texture2D>("left"),8)},
                { "Idle", new Animation(Content.Load<Texture2D>("idle"),5)},
                { "Death", new Animation(Content.Load<Texture2D>("death"),8)},
                { "attack",new Animation(Content.Load<Texture2D>("attack"),7)}
            };
            var _enemy_animations = new Dictionary<string, Animation>()
            {       // enemy melee attacks
                
                 {"enemywalkR",new Animation(Content.Load<Texture2D>("enemywalkR"),5)},
                { "enemywalkL", new Animation(Content.Load<Texture2D>("enemywalkL"),5)},
                {"enemyattackR",new Animation(Content.Load<Texture2D>("enemyattackR"),5)},
                { "enemyattackL", new Animation(Content.Load<Texture2D>("enemyattackL"),5) }
            };

           

            _sprites = new List<Player>();
           
            Player main_player = new Player(animations,graphics) { Position = new Vector2((int)(0)
                , (int)((0.858) * screenHeight)), };


            
            
            _sprites.Add(main_player);

            // initializes enemies positions
            enemy = new Enemy(_enemy_animations,graphics)
            {
                Position = new Vector2(700,(int)((0.838) * screenHeight))
              
          };

               enemy2 = new Enemy(_enemy_animations, graphics)
               {
                   Position = new Vector2(1200, (int)((0.838) * screenHeight))

               };

            enemy3 = new Enemy(_enemy_animations, graphics)
            {
                Position = new Vector2(2000, (int)((0.838) * screenHeight))

            };

          

            currentState = Keyboard.GetState();
            previousState = currentState;

        }
        #endregion

        // used to remove content from RAM, we do not need to do this for our game
        protected override void UnloadContent()
        {

        }



        #region Main Menu
        // Displays the main menu
        public void menu()
        {
            // Sets the background color    
            GraphicsDevice.Clear(Color.Silver);
            

            // List to set opacity of each menu object
            float[] selected = new float[6];

            // up is pressed, menu selection  moves up
            if (previousState.IsKeyUp(Keys.Up) && currentState.IsKeyDown(Keys.Up))
            {
                select--;
            }
            // down is pressed, menu selection moves down 
            if (previousState.IsKeyUp(Keys.Down) && currentState.IsKeyDown(Keys.Down))
            {
                select++;
            }



            // keeps menu selection within bounds, circular connection
            if (select > 5)
                select = 0;
            if (select < 0)
                select = 5;

            // sets opacities based on menu selection
            switch (select)
            {
                case 0:
                    selected[0] = 1f;
                    selected[1] = .5f;
                    selected[2] = .5f;
                    selected[3] = .5f;
                    selected[4] = .5f;
                    selected[5] = .5f;
                    break;
                case 1:
                    selected[0] = .5f;
                    selected[1] = 1f;
                    selected[2] = .5f;
                    selected[3] = .5f;
                    selected[4] = .5f;
                    selected[5] = .5f;
                    break;
                case 2:
                    selected[0] = .5f;
                    selected[1] = .5f;
                    selected[2] = 1f;
                    selected[3] = .5f;
                    selected[4] = .5f;
                    selected[5] = .5f;
                    break;
                case 3:
                    selected[0] = .5f;
                    selected[1] = .5f;
                    selected[2] = .5f;
                    selected[3] = 1f;
                    selected[4] = .5f;
                    selected[5] = .5f;
                    break;
                case 4:
                    selected[0] = .5f;
                    selected[1] = .5f;
                    selected[2] = .5f;
                    selected[3] = .5f;
                    selected[4] = 1f;
                    selected[5] = .5f;
                    break;
                case 5:
                    selected[0] = .5f;
                    selected[1] = .5f;
                    selected[2] = .5f;
                    selected[3] = .5f;
                    selected[4] = .5f;
                    selected[5] = 1f;
                    break;
            }

            int height = graphics.PreferredBackBufferHeight;
            int width = graphics.PreferredBackBufferWidth;
            int initial = height /5;

          
            // enter is pressed, does action depending on which menu item is selected
            if (previousState.IsKeyUp(Keys.Enter) && currentState.IsKeyDown(Keys.Enter))
            {
              
               
                if (select == 1)
                    _state = GameState.Login;
                if (select == 2)
                {
                    CreatebeingTyped = "user";
                    Createusername.Clear();
                    Createpassword.Clear();
                    Createenterable = false;
                    _state = GameState.CreateAccount;
                }
                if (select == 3)
                    _state = GameState.Leaderboards;
                if (select == 4)
                    _state = GameState.Instructions;
                if (select == 5)
                    Exit();
            }

              

            
            // Draws the game title
            spriteBatch.Draw(titlescreen, new Rectangle(width / 4, height/12, width/2, height/2), Color.White);
            // Draws the menu options
            spriteBatch.Draw(singePlayer, new Rectangle(new Point(width / 2 - buttonSize.X/2, initial + buttonSize.Y + height/20), buttonSize), Color.White * selected[0]);
          
            // not logged in, shows all menu options
            if(!LOGGED_IN)
            {
                spriteBatch.Draw(multiplayer, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y * 2+height/19), buttonSize), Color.White * selected[1]);
                spriteBatch.Draw(createaccountbutton, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y * 3 + height / 18), buttonSize), Color.White * selected[2]);
                spriteBatch.Draw(viewLeaderboards, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y * 4 + height / 17), buttonSize), Color.White * selected[3]);
                spriteBatch.Draw(instructions, new Rectangle(new Point(width/ 2 - buttonSize.X / 2, initial + buttonSize.Y * 5+height/16), buttonSize), Color.White * selected[4]);
                spriteBatch.Draw(exit, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y *6+ height/15), buttonSize), Color.White * selected[5]);

            }
            // logged in, does NOT show "Log in" or "create account" button
            else
            {
                // adjust menu selection for missing entries
                if(select==1)
                    select = 3;
                if(select == 2)
                    select = 0;

               
                spriteBatch.Draw(viewLeaderboards, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y * 2 + height / 19), buttonSize), Color.White * selected[3]);
                spriteBatch.Draw(instructions, new Rectangle(new Point(width/ 2 - buttonSize.X / 2, initial + buttonSize.Y * 3+height/18), buttonSize), Color.White * selected[4]);
                spriteBatch.Draw(exit, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y *4+ height/17), buttonSize), Color.White * selected[5]);
            }

            
        }
        #endregion


        
        // Creates tiles for level 2
        private void CreateNightTiles()
        {
            nightTiles.Clear();
            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;
            
            int i = 1;
            for (; i < 21; i++)
            {
                nightTiles.Add(new Tile(new Vector2(screenWidth *0.1f*i, (float)(screenHeight * 0.75))));
            }
            finish_line2 = new Door(new Vector2(screenWidth * 0.1f * (i+1), (float)(screenHeight * 0.814)));

        }
        
        // called when user dies
        public void Gameover(GameTime gameTime)
        {
            //resets tiles and finish line  
            CreateNightTiles();
            CreateTiles();
           finish_line = new Door(new Vector2(graphics.PreferredBackBufferWidth * 0.2f * (10 + 1), (float)(graphics.PreferredBackBufferHeight * 0.814)));
            finish_line2 = new Door(new Vector2(graphics.PreferredBackBufferWidth * 0.1f * (20 + 1), (float)(graphics.PreferredBackBufferHeight * 0.814)));

            // resets all enemies and player values
            deathCounter = 0;
            _sprites[0].Reset();
            enemy.Reset();
            enemy2.Reset();
            enemy3.Reset();
            enemy._position.X = 2000;
            enemy2._position.X = 1000;
            elapsed_time = 0f;
            
            previousState = currentState;
            currentState = Keyboard.GetState();

            // pressing escape takes user back to main menu
            if((currentState.IsKeyDown(Keys.Escape) && (previousState.IsKeyUp(Keys.Escape))))
                _state = GameState.MainMenu;

                GraphicsDevice.Clear(Color.White);
                spriteBatch.Begin();
                
                spriteBatch.Draw(gameover, new Rectangle(0,0,graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

                spriteBatch.End();

                base.Draw(gameTime);
        }

        // creates tiles for level 1
        private void CreateTiles()
        {
            tiles.Clear();
            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;

            int i = 0;
            for (; i < 10; i++)
            {
                tiles.Add(new Tile(new Vector2(screenWidth *0.2f*i, (float)(screenHeight * 0.75))));
            }
            finish_line = new Door(new Vector2(screenWidth * 0.2f * (i+1), (float)(screenHeight * 0.814)));


        }
        protected override void Update(GameTime gameTime)
        {
            if (_state!=GameState.MainMenu)
            {
                MediaPlayer.Pause();
            }
            if (_state == GameState.MainMenu && MediaPlayer.State==MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
            
            base.Update(gameTime);

            mouse = Mouse.GetState();

            switch (_state)
            {
                case GameState.MainMenu:
                    UpdateMainMenu(gameTime);
                    break;
                case GameState.Level1:
                    UpdateLevel1(gameTime);
                    break;
                case GameState.Login:
                    Login(gameTime);
                    break;
            }


        }


             
        // draws the main menu
        protected void DrawMainMenu(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(); //
 
            menu();
            spriteBatch.End(); //
        }

        // draws level 1
        protected void DrawLevel1(GameTime gameTime)
        {

            _previousState = GameState.Level1;
            GraphicsDevice.Clear(Color.Transparent);

            int finish = 400;

            spriteBatch.Begin();
            
            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);

            spriteBatch.Draw(healthTexture, healthRectangle, Color.DarkSlateBlue);
            
        

            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);
          
            foreach (var tl in tiles)
            {
                tl.Draw(spriteBatch);
            }
            finish_line.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
            enemy2.Draw(spriteBatch);
            spriteBatch.DrawString(font, "time: " + Math.Round((elapsed_time/1000),1) + "", 
                new Vector2((float)(graphics.PreferredBackBufferWidth*0.8), (float)(graphics.PreferredBackBufferHeight * 0.05)), Color.Beige);
            spriteBatch.End();

           
           
            base.Draw(gameTime);
        }

        void UpdateMainMenu(GameTime gameTime)
        {

            

            previousState = currentState;
            currentState = Keyboard.GetState();


            // Single player is selected, all values are set to initial values for gameplay
            if (currentState.IsKeyDown(Keys.Enter) && previousState.IsKeyUp(Keys.Enter)&& select == 0)
            {
                _sprites[0].Reset();
                enemy.Reset();
                enemy2.Reset();
                enemy._position.X = 2000;
                enemy2._position.X = 1000;
            elapsed_time = 0f;
                deathCounter = 0;

                CreateNightTiles();
                CreateTiles();
                finish_line = new Door(new Vector2(graphics.PreferredBackBufferWidth * 0.2f * (10 + 1), (float)(graphics.PreferredBackBufferHeight * 0.814)));
                finish_line2 = new Door(new Vector2(graphics.PreferredBackBufferWidth * 0.1f * (20 + 1), (float)(graphics.PreferredBackBufferHeight * 0.814)));

                // user is not logged in, taken straigt to gameplay
                if (!LOGGED_IN)
                    _state = GameState.Level1;
                // user is logged in, taken to continue/new game page
                else
                    {
                        _state = GameState.Continue;
                    }

             }
                
            
            
            controller = GamePad.GetState(PlayerIndex.One);
            keyboard = Keyboard.GetState();

            
            
            base.Update(gameTime);
            

        }

        // asks user to continue or start new game if logged in
        public void ContinueGame(GameTime gameTime)
        {
            previousState = currentState;
            currentState = Keyboard.GetState();

            if(currentState.IsKeyDown(Keys.Escape) && previousState.IsKeyUp(Keys.Escape))
                _state = GameState.MainMenu;

            // used for menu item opacities
            float[] selected = new float[2];

            // up is pressed, menu selection moves up
            if (previousState.IsKeyUp(Keys.Up) && currentState.IsKeyDown(Keys.Up))
            {
                select--;
            }
            // down is pressed, menu item moves down
            if (previousState.IsKeyUp(Keys.Down) && currentState.IsKeyDown(Keys.Down))
            {
                select++;
            }
            // keeps seletion within bounds
            if (select < 0)
                select = 1;
            if (select >1 )
                select = 0;

            // sets opacity
            if(select == 0)
            {
                selected[0] = 1f;
                selected[1] = .5f;
            }
            if(select == 1)
            {
                selected[0] = .5f;
                selected[1] = 1f;
            }

            // enter is pressed
            if (currentState.IsKeyDown(Keys.Enter) && previousState.IsKeyUp(Keys.Enter))
            {
                // Continue 
                if (select == 0)
                {
                    string level = db.continueGame(Logged_Username)[0][0];
                   switch(level) {
                        
                       case "1":
                           _state = GameState.Level1;
                           break;
                       case "2":
                            enemy3._position.X = 1500;
                           _state = GameState.Level2;
                           break;
                       default:
                            _state = GameState.Level1;
                            break;
                   }
                }

                // New game 
                else{
                    db.newGame(Logged_Username);
                    _state = GameState.Level1;
                }
            }

            int width = graphics.PreferredBackBufferWidth;
            int height = graphics.PreferredBackBufferHeight;
            int initial = height / 5;
            GraphicsDevice.Clear(Color.DarkGray);
            spriteBatch.Begin();

            spriteBatch.Draw(Continue, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y * 2+height/19), buttonSize), Color.White*selected[0]);
            spriteBatch.Draw(newGame, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y * 3 + height / 18), buttonSize), Color.White * selected[1]);
                

            spriteBatch.End();

            base.Update(gameTime);
        }               

        // updates level 1 values
        void UpdateLevel1(GameTime gameTime)
        {
            previousState = currentState;
            currentState = Keyboard.GetState();


            // level 1 action
            // enemies & objects

            if (_sprites[0].Health < 100)
            {
                deathCounter++;
                if (deathCounter >= 30)
                    _state = GameState.GameOver;
            }

            // player finishes level
            if (_sprites[0].hasEntered(finish_line,soundEffects))
            {
                // score is recored if logged in
                if(LOGGED_IN && firstBeaten) 
                    {
                    db.completeLevelForFirstTime(1, Logged_Username, (int)(100000f - elapsed_time/10f));
                    db.saveGame(Logged_Username, 1);
                    firstBeaten = false;
                    }
                // taken to level completed screen
                _state = GameState.LevelCompleted;
                
            }
            
            int touchCount = 0;
            
            elapsed_time += gameTime.ElapsedGameTime.Milliseconds;
       


            if ((currentState.IsKeyDown(Keys.Escape) && (previousState.IsKeyUp(Keys.Escape))))
                _state = GameState.Pause;

           
           {

                touchCount = 0;

               // Background scroll
               for (int i = 0; i < 10; i++)
                {
                    scrolling1.Update((int)_sprites[0].Xtrans);
                    scrolling2.Update((int)_sprites[0].Xtrans);
                }
                finish_line.Update(_sprites[0].Xtrans*3);
                foreach (var tile in tiles)
                {
                    
                    // updates player and enemy positions
                    if(_sprites[0].Health == 100)
                    _sprites[0].Update(gameTime, _sprites,soundEffects);
                    enemy.Update(gameTime,_sprites[0], soundEffects);
                    enemy2.Update(gameTime, _sprites[0], soundEffects);
                    
                    
                    healthBar.health = _sprites[0].Health;
                    if (_sprites[0].Health < 100)
                        _sprites[0].Xtrans = 0;
                    {
                        tile.Update(_sprites[0].Xtrans);
                        // user is halfway, tiles and enemies move faster to make it more smooth
                        if (_sprites[0].isHalfway && _sprites[0].Health == 100)
                        {
                            tile.Update(_sprites[0].Xtrans);
                            tile.Update(_sprites[0].Xtrans);
                            
                            // Adjusts enemies better
                            enemy._position.X -= .75f;
                            enemy2._position.X-=.75f;

                            
                           
                        }

                        // Scrolls the background with player movement
                        if (scrolling1.rectangle.X + scrolling1.rectangle.Width <= 0)
                        {
                            scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.rectangle.Width;
                        }
                        if (scrolling2.rectangle.X + scrolling2.rectangle.Width <= 0)
                        {
                            scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.rectangle.Width;
                        }
                    }

                    // Used for attacking (NOT USED IN OUR GAME)
                    if (Keyboard.GetState().IsKeyDown(Keys.E))
                    {
                        _sprites[0].Attack(enemy);
                    }

                    // user is on a tile
                    if (_sprites[0].IsTouching(tile, _sprites[0]))
                    {
                        touchCount++;

                    }
                    



                    //  Checks if player is hitting anything
                    // If not, he fails 
                    if (_sprites[0].jumping == false)
                    {
                        if (_sprites[0].Contact == false)
                            // fall speed
                            _sprites[0]._position.Y += graphics.PreferredBackBufferHeight/600f;
                    }

                    // player is on a tile, doesnt fall past tile
                    if (touchCount > 0)
                        if (_sprites[0]._position.Y > graphics.PreferredBackBufferHeight * .7f)
                        {
                            _sprites[0]._position.Y = graphics.PreferredBackBufferHeight * .7f;
                            _sprites[0].grounded = true;
                        }

                    // Player must stop falling when he reaches the ground
                    if (_sprites[0].Position.Y > graphics.PreferredBackBufferHeight * .87f)
                    {
                        _sprites[0]._position.Y = graphics.PreferredBackBufferHeight * .87f;
                        _sprites[0].grounded = true;
                    }

                    
                }

                
                healthRectangle = new Rectangle(150,50,healthBar.health,60);
               

            }

           
           


            base.Update(gameTime);
        }

        public void Level2(GameTime gameTime)
        {
            // Updates Keyboards
            previousState = currentState;
            currentState = Keyboard.GetState();

            elapsed_time += gameTime.ElapsedGameTime.Milliseconds;
            
            // Sets state that the pause menu will return to
            _previousState = GameState.Level2;

              if (currentState.IsKeyDown(Keys.Escape) && previousState.IsKeyUp(Keys.Escape))
                _state = GameState.Pause;

              GraphicsDevice.Clear(Color.Transparent);

            // Player enters the finish line
            if (_sprites[0].hasEntered(finish_line2, soundEffects))
            {
                CreateNightTiles();
                enemy._position.X = 2000;
                enemy2._position.X = 1000;
                enemy3._position.X = 1500;
               
                _state = GameState.Level2;
               
              
                // Logs score to leaderboard if player is logged in
                if (LOGGED_IN && firstBeaten)
                {
                    db.completeLevelForFirstTime(2, Logged_Username, (int)(100000f - elapsed_time / 10f));
                    db.saveGame(Logged_Username, 2);
                    firstBeaten = false;
                    
                }
                _state = GameState.LevelCompleted;
                spriteBatch.Begin();
                spriteBatch.End();
            }

            // Player takes damage, dies
            if (_sprites[0].Health < 100)
                _state = GameState.GameOver;

            int touchCount = 0;
            // Updates the player and enemies movement
            for(int i = 0; i < 10; i++)
            {
                _sprites[0].Update(gameTime, _sprites, soundEffects);
                enemy.Update(gameTime, _sprites[0], soundEffects);
                enemy2.Update(gameTime, _sprites[0], soundEffects);
               enemy3.Update(gameTime, _sprites[0], soundEffects);
            }
                // Scrolls the background
               for (int i = 0; i < 10; i++)
                {
                    nightscrolling1.Update((int)_sprites[0].Xtrans);
                    nightscrolling2.Update((int)_sprites[0].Xtrans);
                }
            finish_line2.Update(_sprites[0].Xtrans * 3);

               foreach (var tile in nightTiles)
                {

                // Player jumps onto platform
                if (_sprites[0].IsTouching(tile, _sprites[0]))
                {
                    touchCount++;
                }

                // Player is halfway in the screen
                // enemies and tiles move a bit faster to make it more smooth
                if (_sprites[0].isHalfway && _sprites[0].Health == 100)
                {
                    tile.Update(_sprites[0].Xtrans);
                    
                    enemy._position.X -= .75f/2;
                    enemy2._position.X-=.75f/2;
                    enemy3._position.X-= .75f/2;

                            
                }



                    healthBar.health = _sprites[0].Health;

                    tile.Update(_sprites[0].Xtrans);
                    if (_sprites[0].isHalfway)
                    {
                        tile.Update(_sprites[0].Xtrans);
                        tile.Update(_sprites[0].Xtrans);

                    }
                    if (nightscrolling1.rectangle.X + nightscrolling1.rectangle.Width <= 0)
                    {
                        nightscrolling1.rectangle.X = nightscrolling2.rectangle.X + nightscrolling2.rectangle.Width;
                    }
                    if (nightscrolling2.rectangle.X + nightscrolling2.rectangle.Width <= 0)
                    {
                        nightscrolling2.rectangle.X = nightscrolling1.rectangle.X + nightscrolling1.rectangle.Width;
                    }

                // gravity doesnt affect player while he is jumping
                if (_sprites[0].jumping == false)
                    {
                        if (_sprites[0].Contact == false)
                            // fall speed
                            _sprites[0]._position.Y += graphics.PreferredBackBufferHeight/1200f;
                    }

                    // player is on a tile
                    if (touchCount > 0)
                        if (_sprites[0]._position.Y > graphics.PreferredBackBufferHeight * .7f)
                        {
                            _sprites[0]._position.Y = graphics.PreferredBackBufferHeight * .7f;
                            _sprites[0].grounded = true;
                        }

                    // Player must stop falling when he reaches the ground
                    if (_sprites[0].Position.Y > graphics.PreferredBackBufferHeight * .87f)
                    {
                        _sprites[0]._position.Y = graphics.PreferredBackBufferHeight * .87f;
                        _sprites[0].grounded = true;
                    }


                }


              spriteBatch.Begin();
            
            nightscrolling1.Draw(spriteBatch);
            nightscrolling2.Draw(spriteBatch);

            spriteBatch.Draw(healthTexture, healthRectangle, Color.DarkSlateBlue);

            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);
           
            foreach (var tl in nightTiles)
            {
                tl.Draw(spriteBatch);
            }
            finish_line2.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
            enemy2.Draw(spriteBatch);
            enemy3.Draw(spriteBatch);
           spriteBatch.DrawString(font, "time: " + Math.Round((elapsed_time/1000),1) + "", 
                new Vector2((float)(graphics.PreferredBackBufferWidth*0.8), (float)(graphics.PreferredBackBufferHeight * 0.05)), Color.Beige);
           
            spriteBatch.End();

          
           
            base.Draw(gameTime);

          
        }

        // Main function that draws/run the current state
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            switch (_state)
            {
                case GameState.MainMenu:
                   DrawMainMenu(gameTime);
                    break;
                case GameState.Login:
                    Login(gameTime);
                    break;
                case GameState.CreateAccount:
                    CreateAccount(gameTime);
                    break;
                case GameState.Leaderboards:
                    Leaderboards(gameTime);
                    break;
                case GameState.Level1:
                    DrawLevel1(gameTime);
                    break;
                case GameState.Level2:
                    Level2(gameTime);
                    break;
                case GameState.Instructions:
                    Instructions(gameTime);
                    break;
                case GameState.Finish:
                    // DrawFinish(gameTime);
                    break;
                case GameState.LevelCompleted:
                    LevelCompleted(gameTime);
                    break;
                case GameState.GameOver:
                    Gameover(gameTime);
                    break;
                case GameState.Pause:
                    Pause(gameTime);
                    break;
                case GameState.Continue:
                    ContinueGame(gameTime);
                    break;
                    


            }
        }

        

    }
}
