using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

using System;


namespace Platformer
{




    public class Game1 : Game
    {


        enum GameState
        {
            MainMenu,
            Level1,
            Finish
        }
        GameState _state = GameState.MainMenu;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // if you see this, progress has been made ..
        // trying git cmds
        // Title Screen 


            bool wasTouching = false;

        SpriteFont font;


        List<string> username = new List<string>();
        List<string> password = new List<string>();

        String beingTyped = "user";

        Texture2D titlescreen;
        Texture2D titlescreen_a;
        Scrolling scrolling1;
        Scrolling scrolling2;
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


        private List<Player> _sprites;
        ConnectDB db = new ConnectDB();
        

        Menu m;

        // Initialize controller/keyboard
        GamePadState controller = GamePad.GetState(PlayerIndex.One);
        KeyboardState keyboard = Keyboard.GetState();
        KeyboardState currentState;
        KeyboardState previousState;
        int select = 0;
        Texture2D continueWithoutSaving, exit, instructions, multiplayer, newGame, returnToMainMenu, saveContinue, singePlayer, startGame, tryAgain;
        Point buttonSize;

        Texture2D logintitle, usernametitle, passwordtitle;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Sets the game to 1080p fullscreen by default
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.IsFullScreen = true;
            

        }


        // Logs the user in to the server
        public void Login(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkRed);
            spriteBatch.Begin(); //
            
            
            Keys c;

            if (beingTyped == "user")
                {
            
            if(currentState.GetPressedKeys().Length > 0)
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
                                
                                beingTyped = "password";
                            }
                    else
                    {
                        username.Add(c.ToString());
                    }
                    
                }


                }
            }


            
            if (beingTyped == "password"){
            if(currentState.GetPressedKeys().Length > 0)
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
                            db.createAccount("test", "passadfasdf");
                        else
                        {
                            password.Add(c.ToString());
                        }
                }


                }
            }




            
            int height = graphics.PreferredBackBufferHeight;
            int width = graphics.PreferredBackBufferWidth;


            spriteBatch.DrawString(font, String.Join(String.Empty, username.ToArray()), new Vector2(width/2, height/16+buttonSize.Y*3), Color.White);
            spriteBatch.DrawString(font, String.Join(String.Empty, password.ToArray()), new Vector2(width/2, height/16+buttonSize.Y*5), Color.White);

            // spriteBatch.Draw(logintitle, new Rectangle())
                       spriteBatch.Draw(logintitle, new Rectangle(new Point(width/ 2 - buttonSize.X, height/16), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White );
                       spriteBatch.Draw(usernametitle, new Rectangle(new Point(width/ 2 - buttonSize.X*2, height/16+buttonSize.Y*2), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White );
                       spriteBatch.Draw(passwordtitle, new Rectangle(new Point(width/ 2 - buttonSize.X*2, height/16+buttonSize.Y*4), new Point(buttonSize.X * 2, buttonSize.Y * 2)), Color.White );

                        spriteBatch.End(); 

        }

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
            db.updateHighScore("abg", 1 , 677);
            db.viewLeaderboards(1);*/
        }


        protected override void LoadContent()
        {
            buttonSize = new Point(graphics.PreferredBackBufferWidth * 5 / 32, graphics.PreferredBackBufferHeight * 5/72);

            font = Content.Load<SpriteFont>("font");

            // Login Page
            usernametitle = Content.Load<Texture2D>("usernametitle");
            passwordtitle = Content.Load<Texture2D>("passwordtitle");
            logintitle = Content.Load<Texture2D>("logintitle");

            spriteBatch = new SpriteBatch(GraphicsDevice);
            titlescreen = Content.Load<Texture2D>("titlescreen");
            titlescreen_a = Content.Load<Texture2D>("titlescreen(1)");

            // Buttons
            continueWithoutSaving = Content.Load<Texture2D>("continuewithoutsaving");
            exit = Content.Load<Texture2D>("exit");
            instructions = Content.Load<Texture2D>("instructions");
            multiplayer = Content.Load<Texture2D>("multiplayer");
            newGame = Content.Load<Texture2D>("newgame");
            returnToMainMenu = Content.Load<Texture2D>("returntomainmenu");
            saveContinue = Content.Load<Texture2D>("savecontinue");
            singePlayer = Content.Load<Texture2D>("singleplayer");
            startGame = Content.Load<Texture2D>("startgame");
            tryAgain = Content.Load<Texture2D>("tryagain");
            healthBar = new HealthBar(Content.Load<Texture2D>("Health"),new Vector2(400,400),100);
            healthTexture = Content.Load<Texture2D>("Health");


            // background
            // so once we scroll through one background we go onto the next
            scrolling1 = new Scrolling(Content.Load<Texture2D>("background"), new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
            scrolling2 = new Scrolling(Content.Load<Texture2D>("background"), new Rectangle(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));


            // loading tile textures here
            foreach (var _tile in tiles)
            {
                Tile.LoadContent(Content, 0);
            }
            




            m = new Menu(GraphicsDevice);
            var animations = new Dictionary<string, Animation>(){

                {"WalkRight",new Animation(Content.Load<Texture2D>("right"),8)},
                { "WalkLeft", new Animation(Content.Load<Texture2D>("left"),8)},
                { "Idle", new Animation(Content.Load<Texture2D>("idle"),5)},
                { "Death", new Animation(Content.Load<Texture2D>("death"),8)}
             };


            _sprites = new List<Player>();
            Player main_player = new Player(animations) { Position = new Vector2((int)(.0732 * GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                , (int)((0.858) * GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)), };

            _sprites.Add(main_player);
                
      
       

            //

            currentState = Keyboard.GetState();
            previousState = currentState;

        }


        protected override void UnloadContent()
        {

        }

       



        public void menu()
        {
            // Sets the background color    
            GraphicsDevice.Clear(Color.Silver);
           
            float[] selected = new float[4];

            if (previousState.IsKeyUp(Keys.Up) && currentState.IsKeyDown(Keys.Up))
            {
                select--;
            }

            if (previousState.IsKeyUp(Keys.Down) && currentState.IsKeyDown(Keys.Down))
            {
                select++;
            }




            if (select > 3)
                select = 0;
            if (select < 0)
                select = 3;

            switch (select)
            {
                case 0:
                    selected[0] = 1f;
                    selected[1] = .5f;
                    selected[2] = .5f;
                    selected[3] = .5f;
                    break;
                case 1:
                    selected[0] = .5f;
                    selected[1] = 1f;
                    selected[2] = .5f;
                    selected[3] = .5f;
                    break;
                case 2:
                    selected[0] = .5f;
                    selected[1] = .5f;
                    selected[2] = 1f;
                    selected[3] = .5f;
                    break;
                case 3:
                    selected[0] = .5f;
                    selected[1] = .5f;
                    selected[2] = .5f;
                    selected[3] = 1f;
                    break;
            }

            
            int height = graphics.PreferredBackBufferHeight;
            int width = graphics.PreferredBackBufferWidth;
            int initial = height /5;

            
            // Draws the game title
            spriteBatch.Draw(titlescreen, new Rectangle(width / 2 - width/4, height/12, width/2, height/2), Color.White);
            // Draws the menu options
            spriteBatch.Draw(singePlayer, new Rectangle(new Point(width / 2 - 150, initial + buttonSize.Y + height/20), buttonSize), Color.White * selected[0]);
            spriteBatch.Draw(multiplayer, new Rectangle(new Point(width / 2 - 150, initial + buttonSize.Y * 2+height/18), buttonSize), Color.White * selected[1]);
            spriteBatch.Draw(instructions, new Rectangle(new Point(width/ 2 - 150, initial + buttonSize.Y * 3+height/16), buttonSize), Color.White * selected[2]);
            spriteBatch.Draw(exit, new Rectangle(new Point(width / 2 - 150, initial + buttonSize.Y *4+ height/15), buttonSize), Color.White * selected[3]);

            
        }


        private void CreateTiles()
        {
            // This is the same code as I used in Initialize().
            // Duplicate code is extremely bad practice. So you should now modify 
            // Initialize() so that it calls this method instead.

            int screenWidth = GraphicsDevice.Viewport.Width;
            int screenHeight = GraphicsDevice.Viewport.Height;
           // float xPosition = Shared.random.Next(200, screenWidth/2+200);
            tiles.Add(new Tile(new Vector2(200, (float)(screenHeight*0.75))));

            tiles.Add(new Tile(new Vector2(500, (float)(screenHeight*0.75))));
            tiles.Add(new Tile(new Vector2(900, (float)(screenHeight*0.75))));

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



            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);
            foreach (var tl in tiles)
            {
                tl.Draw(spriteBatch);
            }
            spriteBatch.End();

           
            base.Draw(gameTime);
        }

        void UpdateMainMenu(GameTime gameTime)
        {
            // menu control
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
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

            

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (scrolling1.rectangle.X + scrolling1.rectangle.Width <= 0)
            {
                scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.rectangle.Width;
            }
            if (scrolling2.rectangle.X + scrolling2.rectangle.Width <= 0)
            {
                scrolling2.rectangle.X = scrolling1.rectangle.X + scrolling1.rectangle.Width;
            }
           {



                foreach (var tile in tiles)
                {
                    _sprites[0].Update(gameTime, _sprites);
                    scrolling1.Update((int)_sprites[0].Xtrans);
                    scrolling2.Update((int)_sprites[0].Xtrans);

                    tile.Update(_sprites[0].Xtrans);
                    if (_sprites[0].IsTouching(tile, _sprites[0]))
                    {

                        wasTouching = true;

                        Console.Write("Check");
                        Vector2 vec = new Vector2(1, tile.position.Y - 160f); 
                      //  _sprites[0].Velocity
                        //    = vec;

                        _sprites[0]._position.Y = tile.position.Y - 56f;
                        



                    }
                    else if (wasTouching == true)
                        {
                           //wasTouching = false;
                            //_sprites[0]._position.Y = tile.position.Y + 1f;

                        }


                    if (_sprites[0].tileTouching(tiles[0], _sprites[0]))
                        wasTouching = true;
                    if (!(_sprites[0].tileTouching(tiles[0], _sprites[0])))
                        if(wasTouching)
                        {
                        wasTouching = false;
                        _sprites[0]._position.Y = graphics.PreferredBackBufferHeight - 200f;
                        }
                    
                    /* sprite._position.Y = tile.position.Y + sprite._texture.Height;
                     if (!sprite.IsTouching(tile))
                     {


                         tile.Update(sprite.Xtrans);
                     }
                     else
                     {

                     }*/


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
                   Login(gameTime);
                    break;
                case GameState.Level1:
                    DrawLevel1(gameTime);
                    break;
                case GameState.Finish:
                    // DrawFinish(gameTime);
                    break;
                    


            }
        }

    }
}
