using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;



namespace Platformer
{




    public class Game1 : Game
    {


        enum GameState
        {
            MainMenu,
            Login,
            Level1,
            Finish,
            Instructions, 
            CreateAccount,
            Leaderboards
        }
        GameState _state = GameState.MainMenu;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        // Title Screen 


            bool wasTouching = false;


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

        Texture2D titlescreen;
        Texture2D titlescreen_a;
        Scrolling scrolling1;
        Scrolling scrolling2;

        // List of tiles to display on platform
        List<Tile> tiles = new List<Tile>();
        int opacDirection = 1;
        Rectangle titleScreen = new
        
           

        // fit user's screen bounds
        Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        // Title Screen //

            MouseState mouse = Mouse.GetState();


        // Heealth bar
        HealthBar healthBar;
        Texture2D healthTexture;
        Rectangle healthRectangle;

        // sprite list
        private List<Player> _sprites;

        private List<Enemy> _sprites2;
        ConnectDB db = new ConnectDB();
        Texture2D instructs;

        private Enemy enemy;

        Menu m;

        // Initialize controller/keyboard
        GamePadState controller = GamePad.GetState(PlayerIndex.One);
        KeyboardState keyboard = Keyboard.GetState();
        KeyboardState currentState;
        KeyboardState previousState;

        KeyboardState typeCurr, typePrev;

        int select = 0;

        Texture2D continueWithoutSaving, createaccountbutton, viewLeaderboards, exit, instructions, multiplayer, newGame, returnToMainMenu, saveContinue, singePlayer, startGame, tryAgain;
        Point buttonSize;

        Texture2D logintitle, usernametitle, passwordtitle, enter;
        Texture2D createaccount, confirmpassword;
        bool firstLog = true;
        bool enterable = false;
        bool incorrectLogin = false;
        bool Createenterable = false;

        Texture2D incorrect;
        Texture2D usernametaken;
        bool usertaken = false;

        SpriteFont font;

        //calculates and stores elapsed time since the game has started
        Rectangle time= new Rectangle(700,100,200,100);
       
        float  elapsed_time;
         //private SpriteFont font;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Sets the game to 1080p fullscreen by default
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            //graphics.IsFullScreen = true;


        }

        #region Leaderboards
        public void Leaderboards(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _state = GameState.MainMenu;

            previousState = currentState;
            currentState = Keyboard.GetState();
            int z = 0;

            GraphicsDevice.Clear(Color.DarkRed);
            List<string> board = new List<string>();
            if (previousState.IsKeyUp(Keys.Space) && currentState.IsKeyDown(Keys.Space))
            {
                Console.WriteLine("\n\n\n *********");
                foreach (var i in db.viewLeaderboards(1))
                {
                    int y = 0;
                    foreach (var j in i)
                    {
                        
                        Console.WriteLine(j + "\t");
                        y++;
                    }
                    z++;
                }
                //Console.WriteLine(z);
                
                GraphicsDevice.Clear(Color.Blue);
            }


           
            spriteBatch.Begin(); //



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
            spriteBatch.Begin(); //


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
            spriteBatch.Begin(); //





         
            Keys c;

            // User is entering username
            if (CreatebeingTyped == "user")
            {
               
                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];

                    if (previousState.GetPressedKeys().Length > 0)
                    {
                        if (previousState.GetPressedKeys()[0] != c)

                        {

                            if (c == Keys.Back)
                            {
                                if (Createusername.Count != 0)
                                    Createusername.RemoveAt(Createusername.Count - 1);
                            }


                            else
                            {

                                Createusername.Add(c.ToString());
                            }
                        }

                    }
                    else
                    {
                        if (c == Keys.Back)
                        {
                            if (Createusername.Count != 0)
                                Createusername.RemoveAt(Createusername.Count - 1);
                        }
                        else if (c == Keys.Tab)
                        {
                            Createcolors[0] = 0f;
                            Createcolors[1] = .5f;
                            Createcolors[2] = 0f;
                            Createcolors[3] = 0f;

                            CreatebeingTyped = "password";
                        }
                        else if (c == Keys.Right || c == Keys.Left || c == Keys.Up)
                        {

                            ;
                        }
                        else if (c == Keys.Enter)
                        {
                            CreatebeingTyped = "enter";
                            Createcolors[0] = 0f;
                            Createcolors[1] = 0f;
                            Createcolors[2] = 0f;
                            Createcolors[3] = 0.5f;
                            //  enterable = true;
                        }


                        else if (c == Keys.Down)
                        {
                            beingTyped = "enter";
                            Createcolors[0] = 0f;
                            Createcolors[1] = 0.5f;
                            Createcolors[2] = 0;
                            Createcolors[3] = 0f;

                            CreatebeingTyped = "password";
                        }
                        else
                        {
                            Createusername.Add(c.ToString());
                        }

                    }


                }
            }


            if (CreatebeingTyped == "password")
            {

                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];

                    if (previousState.GetPressedKeys().Length > 0)
                    {
                        if (previousState.GetPressedKeys()[0] != c)

                        {

                            if (c == Keys.Back)
                            {
                                if (Createpassword.Count != 0)
                                    Createpassword.RemoveAt(Createpassword.Count - 1);
                            }

                            else if (previousState.IsKeyUp(Keys.Down) && currentState.IsKeyDown(Keys.Down))
                            {
                                Createcolors[0] = 0f;
                                Createcolors[1] = 0f;
                                Createcolors[2] = .5f;
                                Createcolors[3] = 0f;

                                CreatebeingTyped = "confirm";
                            }

                            else
                            {

                                Createpassword.Add(c.ToString());
                            }
                        }

                    }
                    else
                    {
                        if (c == Keys.Back)
                        {
                            if (Createpassword.Count != 0)
                                Createpassword.RemoveAt(Createpassword.Count - 1);
                        }
                        else if (c == Keys.Up)
                        {
                            Createcolors[0] = 0.5f;
                            Createcolors[1] = 0f;
                            Createcolors[2] = 0f;
                            Createcolors[3] = 0f;

                            CreatebeingTyped = "user";
                        }
                        else if (c == Keys.Right || c == Keys.Left || c == Keys.Up || c == Keys.Tab || c == Keys.Down)
                        {

                            ;
                        }
                        else if (c == Keys.Enter)
                        {
                            CreatebeingTyped = "enter";
                            Createcolors[0] = 0f;
                            Createcolors[1] = 0f;
                            Createcolors[2] = 0f;
                            Createcolors[3] = 0.5f;
                            //  enterable = true;
                        }

                        //else if (c == Keys.Down)
                            
                        // Take to the confirm password, not working 
                        /*else if ((c == Keys.Down && previousState.GetPressedKeys().Length == 0)||(c == Keys.Down && previousState.GetPressedKeys()[0] != 0) )
                        {
                            Createcolors[0] = 0f;
                            Createcolors[1] = 0f;
                            Createcolors[2] = .5f;
                            Createcolors[3] = 0f;

                            CreatebeingTyped = "confirm";
                        }*/
                        /*
                         else if (previousState.IsKeyUp(Keys.Down) && currentState.IsKeyDown(Keys.Down))
                        {
                            Createcolors[0] = 0f;
                            Createcolors[1] = 0f;
                            Createcolors[2] = .5f;
                            Createcolors[3] = 0f;

                            CreatebeingTyped = "confirm";
                        }*/

                        else
                        {
                            Createpassword.Add(c.ToString());
                        }

                    }


                }
            }


            if (CreatebeingTyped == "confirm")
            {

                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];

                    if (previousState.GetPressedKeys().Length > 0)
                    {
                        if (previousState.GetPressedKeys()[0] != c)

                        {

                            if (c == Keys.Back)
                            {
                                if (Createpassword.Count != 0)
                                    Createpassword.RemoveAt(Createpassword.Count - 1);
                            }


                            else
                            {

                                Createpassword.Add(c.ToString());
                            }
                        }

                    }
                    else
                    {
                        if (c == Keys.Back)
                        {
                            if (Createpassword.Count != 0)
                                Createpassword.RemoveAt(Createpassword.Count - 1);
                        }
                        else if (c == Keys.Up)
                        {
                            Createcolors[0] = 0.5f;
                            Createcolors[1] = 0f;
                            Createcolors[2] = 0f;
                            Createcolors[3] = 0f;

                            CreatebeingTyped = "user";
                        }
                        else if (c == Keys.Right || c == Keys.Left || c == Keys.Up || c == Keys.Down)
                        {

                            ;
                        }
                        else if (c == Keys.Enter)
                        {
                            beingTyped = "enter";
                            Createcolors[0] = 0f;
                            Createcolors[1] = 0f;
                            Createcolors[2] = 0f;
                            Createcolors[3] = 0.5f;
                            //  enterable = true;
                        }


                       
                        else
                        {
                            Createpassword.Add(c.ToString());
                        }

                    }


                }
            }


            if (CreatebeingTyped == "enter")
            {
                c = Keys.None;
                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];
                }
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

                            // Account.GenerateHash(PASSWORD, USERNAME)

                            // Code to create account
                            // Front end hashes the password with the username as a salt and stores it in database
                            if (db.createAccount(USERNAME, Account.GenerateHash(PASSWORD, USERNAME)))
                            {

                                _state = GameState.MainMenu;
                               
                            }
                            else
                            {
                                usertaken = true;
                            }
                        }
                        else
                            Exit();

                    }
                }
            }


            


            spriteBatch.DrawString(font, String.Join(String.Empty, Createusername.ToArray()), new Vector2(width / 2, height / 16 + buttonSize.Y * 3), Color.White);
            spriteBatch.DrawString(font, String.Join(String.Empty, Createpassword.ToArray()), new Vector2(width / 2, height / 16 + buttonSize.Y * 5), Color.White);

            // spriteBatch.Draw(logintitle, new Rectangle())
            spriteBatch.Draw(createaccount, new Rectangle(new Point(width / 2 - buttonSize.X, height / 16), new Point(buttonSize.X * 2, buttonSize.Y * 3)), Color.White);
            spriteBatch.Draw(usernametitle, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 2), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + Createcolors[0]));
            spriteBatch.Draw(passwordtitle, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 4), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + Createcolors[1]));
            spriteBatch.Draw(confirmpassword, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 6), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + Createcolors[2]));

            spriteBatch.Draw(enter, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 8), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + Createcolors[3]));
            if(usertaken)
                spriteBatch.Draw(usernametaken, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 10), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White );





            spriteBatch.End();
        }
        #endregion

        // Logs the user in to the server
        #region Login
        public void Login(GameTime gameTime)
        {
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

                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];

                    if (previousState.GetPressedKeys().Length > 0)
                    {
                        if (previousState.GetPressedKeys()[0] != c)

                        {

                            if (c == Keys.Back)
                            {
                                if (username.Count != 0)
                                    username.RemoveAt(username.Count - 1);
                            }


                            else
                            {

                                username.Add(c.ToString());
                            }
                        }

                    }
                    else
                    {
                        if (c == Keys.Back)
                        {
                            if (username.Count != 0)
                                username.RemoveAt(username.Count - 1);
                        }
                        else if (c == Keys.Tab)
                        {
                            colors[0] = 0f;
                            colors[1] = .5f;
                            colors[2] = 0f;

                            beingTyped = "password";
                        }
                        else if ( c == Keys.Right || c == Keys.Left || c == Keys.Up || c == Keys.Escape || c == Keys.RightShift || c == Keys.LeftShift)
                        {

                            ;
                        }
                        else if (c == Keys.Enter)
                        {
                            beingTyped = "enter";
                            colors[0] = 0f;
                            colors[1] = 0f;
                            colors[2] = 0.5f;
                          //  enterable = true;
                        }


                        else if (c == Keys.Down)
                        {
                            colors[0] = 0f;
                            colors[1] = .5f;
                            colors[2] = 0f;

                            beingTyped = "password";
                        }
                        else
                        {
                            username.Add(c.ToString());
                        }

                    }


                }
            }


            // User is entering password
            if (beingTyped == "password") {
                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];

                    if (previousState.GetPressedKeys().Length > 0)
                    {
                        if (previousState.GetPressedKeys()[0] != c)

                        {

                            if (c == Keys.Back)
                            {
                                if (password.Count != 0)
                                    password.RemoveAt(password.Count - 1);
                            }

                           
                            else
                            {

                                password.Add(c.ToString());
                            }
                        }

                    }
                    else
                    {
                        if (c == Keys.Back)
                        {
                            if (password.Count != 0)
                                password.RemoveAt(password.Count - 1);
                        }

                        else if (c == Keys.Tab)
                        {
                           //if (db.login(String.Join(String.Empty, username.ToArray()), String.Join(String.Empty, password.ToArray())))
                            {
                                //_state = GameState.Level1;
                            }
                        }


                        else if (c == Keys.Right || c == Keys.Left || c == Keys.Down || c == Keys.Escape || c == Keys.RightShift || c == Keys.LeftShift)
                            ;

                        else if (c == Keys.Up)
                        {

                            colors[0] = 0.5f;
                            colors[1] = 0f;
                            colors[2] = 0f;

                            beingTyped = "user";
                           // enterable = true;
                        }
                        else if (c == Keys.Enter )
                        {
                            beingTyped = "enter";
                            colors[0] = 0f;
                            colors[1] = 0f;
                            colors[2] = 0.5f;
                        }
                        else
                        {
                            password.Add(c.ToString());
                        }
                    }


                }
            }

            if (beingTyped == "enter")
            {
                c = Keys.None;
                if (currentState.GetPressedKeys().Length > 0)
                {
                    c = currentState.GetPressedKeys()[0];
                }
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
                        // Code to Log in
                        if (db.login(String.Join(String.Empty, username.ToArray()), String.Join(String.Empty, password.ToArray())))
                        {
                            _state = GameState.Level1;
                            
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





            spriteBatch.DrawString(font, String.Join(String.Empty, username.ToArray()), new Vector2(width / 2, height / 16 + buttonSize.Y * 3), Color.White);
            spriteBatch.DrawString(font, String.Join(String.Empty, password.ToArray()), new Vector2(width / 2, height / 16 + buttonSize.Y * 5), Color.White);

            // spriteBatch.Draw(logintitle, new Rectangle())
            spriteBatch.Draw(logintitle, new Rectangle(new Point(width / 2 - buttonSize.X, height / 16), new Point(buttonSize.X * 2, buttonSize.Y * 3)), Color.White);
            spriteBatch.Draw(usernametitle, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 2), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + colors[0]));
           spriteBatch.Draw(passwordtitle, new Rectangle(new Point(width/ 2 - buttonSize.X*2, height/16+buttonSize.Y*4), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + colors[1]));

            spriteBatch.Draw(enter, new Rectangle(new Point(width / 2 - buttonSize.X * 2, height / 16 + buttonSize.Y * 6), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White * (.5f + colors[2]));


            spriteBatch.End();


          

            base.Update(gameTime);
        }
        #endregion

        protected override void Initialize()
        {
            


            CreateTiles();
            base.Initialize();
            db.Initialize();
            //db.createAccount("abcdefg", "12345678");
            //db.login("abc", "1234");
            //db.saveGame("abcdefg", 3);
            /*db.completeLevelForFirstTime(1, "abc", 300);
            db.completeLevelForFirstTime(1, "abd", 200);
            db.completeLevelForFirstTime(1, "abe", 500);
            db.completeLevelForFirstTime(1, "abf", 1000);
            db.completeLevelForFirstTime(1, "abg", 600);
            db.completeLevelForFirstTime(1, "abh", 700);
            db.updateHighScore("abg", 1 , 677);*/
            db.viewLeaderboards(1);
        }

        #region LoadContent
        protected override void LoadContent()
        {
            buttonSize = new Point(graphics.PreferredBackBufferWidth * 5 / 32, graphics.PreferredBackBufferHeight * 5/72);

            font = Content.Load<SpriteFont>("font");

            // Login Page
            usernametitle = Content.Load<Texture2D>("usernametitle");
            passwordtitle = Content.Load<Texture2D>("passwordtitle");
            logintitle = Content.Load<Texture2D>("logintitle");
            enter = Content.Load<Texture2D>("enter");

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
            returnToMainMenu = Content.Load<Texture2D>("returntomainmenu");
            saveContinue = Content.Load<Texture2D>("savecontinue");
            singePlayer = Content.Load<Texture2D>("singleplayer");
            viewLeaderboards = Content.Load<Texture2D>("viewleaderboards");
            startGame = Content.Load<Texture2D>("startgame");
            tryAgain = Content.Load<Texture2D>("tryagain");
            healthBar = new HealthBar(Content.Load<Texture2D>("Health"),new Vector2(400,400),100);
            healthTexture = Content.Load<Texture2D>("Health");

            incorrect = Content.Load<Texture2D>("incorrect");
            usernametaken = Content.Load<Texture2D>("usernametaken");

            font = Content.Load<SpriteFont>("demo");

            // background
            // so once we scroll through one background we go onto the next
            scrolling1 = new Scrolling(Content.Load<Texture2D>("background"), new Rectangle(0, 0, screenWidth, screenHeight));
            scrolling2 = new Scrolling(Content.Load<Texture2D>("background"), new Rectangle(screenWidth, 0, screenWidth, screenHeight));


            // loading tile textures here
            foreach (var _tile in tiles)
            {
                Tile.LoadContent(Content, 0);
            }
            



            // initiating menu
            m = new Menu(GraphicsDevice);

            // adding animation set
            // will not be using idle
            
            var animations = new Dictionary<string, Animation>(){

                {"WalkRight",new Animation(Content.Load<Texture2D>("right"),8)},
                { "WalkLeft", new Animation(Content.Load<Texture2D>("left"),8)},
                { "Idle", new Animation(Content.Load<Texture2D>("idle"),5)},
                { "Death", new Animation(Content.Load<Texture2D>("death"),8)},
                { "attack",new Animation(Content.Load<Texture2D>("attack"),6)}
            };
            var _enemy_animations = new Dictionary<string, Animation>()
            {       // enemy melee attacks
                
                 {"enemywalkR",new Animation(Content.Load<Texture2D>("enemywalkR"),5)},
                { "enemywalkL", new Animation(Content.Load<Texture2D>("enemywalkL"),5)},
                {"enemyattackR",new Animation(Content.Load<Texture2D>("enemyattackR"),5)},
                { "enemyattackL", new Animation(Content.Load<Texture2D>("enemyattackL"),5) }
            };

           

            _sprites = new List<Player>();

            Player main_player = new Player(animations,graphics) { Position = new Vector2((int)(.0732 * screenWidth)
                , (int)((0.858) * screenHeight)), };


            // places enemy, but needs to be changed a little
            
            _sprites.Add(main_player);


            enemy = new Enemy(_enemy_animations)
            {
                Position = new Vector2(700,(int)((0.838) * screenHeight))
              
          };


       

            //

            currentState = Keyboard.GetState();
            previousState = currentState;

        }
        #endregion

        protected override void UnloadContent()
        {

        }

       
        

        // Displays the main menu
        public void menu()
        {
            // Sets the background color    
            GraphicsDevice.Clear(Color.Silver);


            float[] selected = new float[6];

            if (previousState.IsKeyUp(Keys.Up) && currentState.IsKeyDown(Keys.Up))
            {
                select--;
            }

            if (previousState.IsKeyUp(Keys.Down) && currentState.IsKeyDown(Keys.Down))
            {
                select++;
            }




            if (select > 5)
                select = 0;
            if (select < 0)
                select = 5;

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



            if (previousState.IsKeyUp(Keys.Enter) && currentState.IsKeyDown(Keys.Enter))
            {
                if (select == 1)
                    _state = GameState.Login;
                if (select == 2)
                    _state = GameState.CreateAccount;
                if (select == 3)
                    _state = GameState.Leaderboards;
                if (select == 4)
                    _state = GameState.Instructions;
                if (select == 5)
                    Exit();
            }

                int height = graphics.PreferredBackBufferHeight;
            int width = graphics.PreferredBackBufferWidth;
            int initial = height /5;

            
            // Draws the game title
            spriteBatch.Draw(titlescreen, new Rectangle(width / 4, height/12, width/2, height/2), Color.White);
            // Draws the menu options
            spriteBatch.Draw(singePlayer, new Rectangle(new Point(width / 2 - buttonSize.X/2, initial + buttonSize.Y + height/20), buttonSize), Color.White * selected[0]);
            spriteBatch.Draw(multiplayer, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y * 2+height/19), buttonSize), Color.White * selected[1]);
            spriteBatch.Draw(createaccountbutton, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y * 3 + height / 18), buttonSize), Color.White * selected[2]);
            spriteBatch.Draw(viewLeaderboards, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y * 4 + height / 17), buttonSize), Color.White * selected[3]);
            spriteBatch.Draw(instructions, new Rectangle(new Point(width/ 2 - buttonSize.X / 2, initial + buttonSize.Y * 5+height/16), buttonSize), Color.White * selected[4]);
            spriteBatch.Draw(exit, new Rectangle(new Point(width / 2 - buttonSize.X / 2, initial + buttonSize.Y *6+ height/15), buttonSize), Color.White * selected[5]);


            
        }


        

        private void CreateTiles()
        {

            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;
            // float xPosition = Shared.random.Next(200, screenWidth/2+200);
            for (int i = 0; i < 10; i++)
            {
                tiles.Add(new Tile(new Vector2(screenWidth *0.2f*i, (float)(screenHeight * 0.75))));
            }
        }
        protected override void Update(GameTime gameTime)
        {


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

        public float drawTitle(float i)
        {

            spriteBatch.Draw(titlescreen, titleScreen, Color.White);
            spriteBatch.Draw(titlescreen_a, titleScreen, Color.White * i);
            if (i > 1f || i < 0f)
                opacDirection *= -1;

            return i + .01f * opacDirection;

        }

        protected void DrawLogin(GameTime gameTime)
            {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(); //

            menu();
            spriteBatch.End(); 
                
            }           

        protected void DrawMainMenu(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(); //

            menu();
            spriteBatch.End(); //
        }
        protected void DrawLevel1(GameTime gameTime)
        {

            
            spriteBatch.Begin();
            
            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);

            spriteBatch.Draw(healthTexture, healthRectangle, Color.DarkSlateBlue);
            
          //  spriteBatch.DrawString(font, elapsed_time.ToString,time,Color.White);

            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);
          //  foreach (var sprite in _sprites2)
            //    sprite.Draw(spriteBatch);
            foreach (var tl in tiles)
            {
                tl.Draw(spriteBatch);
            }
            enemy.Draw(spriteBatch);
            spriteBatch.DrawString(font, "time: " + elapsed_time + "", 
                new Vector2((float)(graphics.PreferredBackBufferWidth*0.8), (float)(graphics.PreferredBackBufferHeight * 0.05)), Color.Beige);
            spriteBatch.End();

           
            base.Draw(gameTime);
        }

        void UpdateMainMenu(GameTime gameTime)
        {
            // menu control
            
            
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && select == 0)
                _state = GameState.Level1;

            
            
            controller = GamePad.GetState(PlayerIndex.One);
            keyboard = Keyboard.GetState();

            previousState = currentState;
            currentState = Keyboard.GetState();
            
            base.Update(gameTime);
            

        }

        void UpdateLevel1(GameTime gameTime)
        {

            // level 1 action
            // enemies & objects


            int touchCount = 0;
            
            elapsed_time += gameTime.ElapsedGameTime.Milliseconds;
         //   Console.WriteLine(elapsed_time);


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                _state = GameState.MainMenu;

           
           {

                touchCount = 0;


                foreach (var tile in tiles)
                {
                    

                    _sprites[0].Update(gameTime, _sprites);
                    enemy.Update(gameTime,_sprites[0]);
                    scrolling1.Update((int)_sprites[0].Xtrans);
                    scrolling2.Update((int)_sprites[0].Xtrans);
                    healthBar.health = _sprites[0].Health;

                    tile.Update(_sprites[0].Xtrans);
                    if (scrolling1.rectangle.X + scrolling1.rectangle.Width <= 0)
                    {
                        scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.rectangle.Width;
                    }
                    if (scrolling2.rectangle.X + scrolling2.rectangle.Width <= 0)
                    {
                        scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.rectangle.Width;
                    }
                    
                    if (_sprites[0].IsTouching(tile, _sprites[0]))
                    {
                        touchCount++;

                        wasTouching = true;
                        
                        Console.Write("Check");
                        Vector2 vec = new Vector2(1, tile.position.Y - 160f); 
                      //  _sprites[0].Velocity
                        //    = vec;


                       // Removed
                       // _sprites[0]._position.Y = tile.position.Y - 56f;

                       // _sprites[0]._position.Y = tile.position.Y - 56f;

                        

                    }
                    else if (wasTouching == true)
                        {
                           //wasTouching = false;
                            //_sprites[0]._position.Y = tile.position.Y + 1f;

                        }

                    /*
                    if (_sprites[0].tileTouching(tiles[0], _sprites[0]))
                        wasTouching = true;
                    if (!(_sprites[0].tileTouching(tiles[0], _sprites[0])))
                        if(wasTouching)
                        {
                        wasTouching = false;
                        _sprites[0]._position.Y = graphics.PreferredBackBufferHeight - 200f;
                        }*/

                    /* sprite._position.Y = tile.position.Y + sprite._texture.Height;
                     if (!sprite.IsTouching(tile))
                     {


                         tile.Update(sprite.Xtrans);
                     }
                     else
                     {

                     }*/


                    //  Checks if player is hitting anything
                    // If not, he fails 
                    if (_sprites[0].jumping == false)
                    {
                        if (_sprites[0].Contact == false)
                            // fall speed
                            _sprites[0]._position.Y += 3.5f;
                    }


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
                case GameState.Instructions:
                    Instructions(gameTime);
                    break;
                case GameState.Finish:
                    // DrawFinish(gameTime);
                    break;
                    


            }
        }

    }
}
