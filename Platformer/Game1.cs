using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{



    // ??
    public class Game1 : Game
    {



        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // if you see this, progress has been made ..
        // trying git cmds
        // Title Screen 
        Texture2D titlescreen;
        Texture2D titlescreen_a;

        float opacity = 0f;
        int opacDirection = 1;
        Rectangle TitleScreen = new

            // fit user's screen bounds
            Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
        // Title Screen //


        Menu m;

        string currentScene = "mainMenu";


        // Initialize controller/keyboard
        GamePadState controller = GamePad.GetState(PlayerIndex.One);
        KeyboardState keyboard = Keyboard.GetState();
        KeyboardState currentState;
        KeyboardState previousState;
        int select = 0;
        Texture2D continueWithoutSaving, exit, instructions, mult, multiplayer, newGame, returnToMainMenu, saveContinue, singePlayer, startGame, tryAgain, leaderBoards, settings;
        ///
        Texture2D gameplay, options, multi;
        /// 
        SpriteFont textBig, textSmall;
        Point buttonSize = new Point(300, 75);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Sets the game to 1080p fullscreen by default
            graphics.PreferredBackBufferHeight = 1080;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.IsFullScreen = true;


        }


        protected override void Initialize()
        {



            base.Initialize();
        }


        protected override void LoadContent()
        {

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
            leaderBoards = Content.Load<Texture2D>("leaderboards");
            settings = Content.Load<Texture2D>("settings");
            mult = Content.Load<Texture2D>("mult");
            // m = new Menu(GraphicsDevice);
            textBig = Content.Load<SpriteFont>("textbig");
            textSmall = Content.Load<SpriteFont>("textsmall");

            //
            multi = Content.Load<Texture2D>("localm");
            gameplay = Content.Load<Texture2D>("gameplaybar1");
            //

            currentState = Keyboard.GetState();
            previousState = currentState;

        }


        protected override void UnloadContent()
        {

        }

        public bool Select()
        {
            if (keyboard.IsKeyDown(Keys.Enter) || controller.IsButtonDown(Buttons.A))
                return true;
            return false;



        }


        public void login()
        {
            GraphicsDevice.Clear(Color.AntiqueWhite);

            spriteBatch.DrawString(textBig, "Login to Your Account", new Vector2(400, 100), Color.Black);
            spriteBatch.DrawString(textSmall, "Username:  ", new Vector2(100, 200), Color.Black);

        }

        // Returns true if given key is pressed and released
        public bool clicked(Keys key)
        {
             if (previousState.IsKeyUp(key) && currentState.IsKeyDown(key))
                return true;
             return false;
        }


        public void multiplay()
        {
            
            spriteBatch.Draw(multi, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            if(clicked(Keys.Escape))
                currentScene = "mainMenu";
            if(clicked(Keys.Enter))
                currentScene = "mult";
        }

        public void multiSelect()
        {
            spriteBatch.Draw(mult, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            if(clicked(Keys.Escape))
                currentScene = "multiplayer";
        }   

        public void menu(/*Texture2D[] items*/)
        {
            // Sets the background color    
            GraphicsDevice.Clear(Color.Silver);

           

            float[] selected = new float[4];
            //


            if (clicked(Keys.Up))
            {
                select -- ;
            }

            if (clicked(Keys.Down))
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
                    if (Select())
                        currentScene = "gameplay";
                    break;
                case 1:
                    selected[0] = .5f;
                    selected[1] = 1f;
                    selected[2] = .5f;
                    selected[3] = .5f;
                    if (Select())
                        currentScene = "multiplayer";
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
                    if (Select())
                        Exit();
                    break;
            }

            int initial = 200;

            //spriteBatch.Draw(titlescreen, new Rectangle(graphics.PreferredBackBufferWidth / 2 - 400, 150, 800, 400), Color.White);

            spriteBatch.Draw(singePlayer, new Rectangle(new Point(graphics.PreferredBackBufferWidth /2-150, initial + 190), buttonSize), Color.White*selected[0]);
            spriteBatch.Draw(multiplayer, new Rectangle(new Point(graphics.PreferredBackBufferWidth /2-150, initial + 280), buttonSize), Color.White * selected[1]);
            spriteBatch.Draw(instructions, new Rectangle(new Point(graphics.PreferredBackBufferWidth / 2 - 150, initial + 370), buttonSize), Color.White * selected[2]);
            spriteBatch.Draw(exit, new Rectangle(new Point(graphics.PreferredBackBufferWidth / 2 - 150, initial + 460), buttonSize), Color.White * selected[3]);
            spriteBatch.Draw(leaderBoards, new Rectangle(graphics.PreferredBackBufferWidth - 400, 25, 100, 100), Color.White*.5f);
            spriteBatch.Draw(settings, new Rectangle(graphics.PreferredBackBufferWidth - 250, 35, 100, 100), Color.White*.5f);
        }


        public void Gameplay()
        {
            spriteBatch.Draw(gameplay, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            
            if (clicked(Keys.Escape))
                currentScene = "pause";
        }




        protected override void Update(GameTime gameTime)
        {
           
            controller = GamePad.GetState(PlayerIndex.One);
            keyboard = Keyboard.GetState();

            previousState = currentState;
            currentState = Keyboard.GetState();

            base.Update(gameTime);
        }

        public float drawTitle(float i)
        {
            
            spriteBatch.Draw(titlescreen, TitleScreen, Color.White);
                spriteBatch.Draw(titlescreen_a, TitleScreen, Color.White*i);
            if (i > 1f || i<0f)
                opacDirection *= -1;
           
            return i + .01f*opacDirection;
            
        }

        public void Pause()
        {
            GraphicsDevice.Clear(Color.BlanchedAlmond);

            
            
            int initial = 200;

            float[] selected = new float[4];
            selected[0] = .5f;
            selected[1] = .5f;
            selected[2] = .5f;
            selected[3] = .5f;

             if (clicked(Keys.Up))
            {
                select -- ;
            }

            if (clicked(Keys.Down))
            {
                select++;
            }

            if(select > 2)
                select = 0;
            if(select < 0)
                select = 2;

            selected[select] = 1f;

            switch(select)
                {
                    case 0:
                        if (Select())
                        currentScene = "gameplay";
                        break;
                    case 1:
                        break;
                    case 2:
                    if(Select())
                        currentScene = "mainMenu";
                        break;
                    
                }
        

            
            
            spriteBatch.Draw(continueWithoutSaving, new Rectangle(new Point(graphics.PreferredBackBufferWidth /2-150, initial + 190), buttonSize), Color.White*selected[0]);
            spriteBatch.Draw(instructions, new Rectangle(new Point(graphics.PreferredBackBufferWidth /2-150, initial + 280), buttonSize), Color.White * selected[1]);
            spriteBatch.Draw(exit, new Rectangle(new Point(graphics.PreferredBackBufferWidth / 2 - 150, initial + 370), buttonSize), Color.White * selected[2]);
            //spriteBatch.Draw(exit, new Rectangle(new Point(graphics.PreferredBackBufferWidth / 2 - 150, initial + 460), buttonSize), Color.White * selected[3]);
            
          

        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkRed);
            
            spriteBatch.Begin();
            
            switch (currentScene) {
                case "mainMenu":
                    menu();
                    break;
                case "gameplay":
                    Gameplay();
                    break;
                case "settings":
                    break;
                case "pause":
                    Pause();
                    break;
                case "multiplayer":
                    multiplay();
                    break;
                case "mult":
                    multiSelect();
                    break;

            }

         
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
