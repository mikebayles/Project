using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;

        Texture2D mainmenuTexture;
        Texture2D backgroundTexture;
        Texture2D logo;

        enum gameState { menu , play, Instructions, Exit, InGame, GameOver, GamePause, AboutGame };
        

        gameState gamestate = gameState.menu;
        MouseState mbd;
        MouseState Prevmbd;
        SpriteFont font;

        List<MainMenu> mbs = new List<MainMenu>();


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Place where the specific options are available in menu 
            mbs.Add(new MainMenu(new Rectangle(400, 45, 100, 14), "Play"));
            mbs.Add(new MainMenu(new Rectangle(400, 75, 100, 14), "About"));
            mbs.Add(new MainMenu(new Rectangle(400, 105, 100, 14), "Instructions"));
            mbs.Add(new MainMenu(new Rectangle(400, 135, 100, 14), "Exit"));

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);

            spriteManager.Enabled = false;  //disables the Update method of the SpriteManager GameComponent
            spriteManager.Visible = false;  //disables the Draw method of the SpriteManager GameComponent

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>(@"font/SpriteFont1");
            mainmenuTexture = Content.Load<Texture2D>(@"Images/space_Background");
            backgroundTexture = Content.Load<Texture2D>(@"Images/background");
            logo = Content.Load<Texture2D>(@"Images/logo_alien");
           
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
           
            //used to simplify the code
            KeyboardState keyboard = Keyboard.GetState();

            //only perform certain actions based on
            //the current game state
            switch (gamestate)
            {
                case gameState.play:
                    if (keyboard.IsKeyDown(Keys.Enter))
                    {
                        Initialize();  //resets the sprites
                        gamestate = gameState.InGame;
                        
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    if (keyboard.IsKeyDown(Keys.Back))
                    {
                        gamestate = gameState.menu;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    break;
                case gameState.AboutGame:
                    if (keyboard.IsKeyDown(Keys.Back))
                    {
                        gamestate = gameState.menu;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    break;
                case gameState.InGame:
                    
                    if (keyboard.IsKeyDown(Keys.P))
                    {
                        gamestate = gameState.GamePause;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    break;
                case gameState.GameOver:
                    if (keyboard.IsKeyDown(Keys.Q))
                    {
                        gamestate = gameState.menu;
                    }
                    break;
                case gameState.GamePause:
                    if (keyboard.IsKeyDown(Keys.Space))
                    {
                        gamestate = gameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    break;
               
                case gameState.Instructions:
                    if (keyboard.IsKeyDown(Keys.Back))
                    {
                       gamestate = gameState.menu;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    break;
            }
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            

            // TODO: Add your update logic here
            mbd = Mouse.GetState();

            foreach (MainMenu m in mbs)
            {
                m.mouseOver(mbd);
                if (m.text == "Play" && m.selected == true && mbd.LeftButton == ButtonState.Pressed && Prevmbd.LeftButton == ButtonState.Released)
                {
                    gamestate = gameState.play;
                }
                m.mouseOver(mbd);
                if (m.text == "About" && m.selected == true && mbd.LeftButton == ButtonState.Pressed && Prevmbd.LeftButton == ButtonState.Released)
                {
                    gamestate = gameState.AboutGame;
                }
                m.mouseOver(mbd);
                if (m.text == "Instruction" && m.selected == true && mbd.LeftButton == ButtonState.Pressed && Prevmbd.LeftButton == ButtonState.Released)
                {
                    gamestate = gameState.Instructions;
                }
                m.mouseOver(mbd);
                if (m.text == "Exit" && m.selected == true && mbd.LeftButton == ButtonState.Pressed && Prevmbd.LeftButton == ButtonState.Released)
                {
                    gamestate = gameState.Exit;
                }

            }

            Prevmbd = mbd;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            string text = " ";

            // TODO: Add your drawing code here
           // spriteBatch.Begin();

            switch (gamestate)
            {
                case gameState.menu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(mainmenuTexture,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 1);

                    foreach (MainMenu m in mbs)
                    {
                        m.Draw(spriteBatch, font);
                    }
                    spriteBatch.End();
                    break;

                case gameState.Exit:
                    Exit();
                    break;

                case gameState.Instructions:
                    string move, movement, goal, how;
                    spriteBatch.Begin();
                    spriteBatch.Draw(mainmenuTexture,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 1);
                    text = "Instructions";
                    move = "How to Move:";
                    movement = "Up: ↑ Direction Key or W Key\nDown: ↓ Direction Key or S Key\nLeft: ← Direction Key or A Key\nRight: → Direction Key or D Key ";
                    goal = "Goal:\nThe goal of Space Alien 2.0 is to survive till the end.\nOnce all the aliens are killed, You will be fighting against main boss. \n You have to basically have  and kill him to get his spaceship to go back to earth.";
                   
                    how = "How to Win:\n Kill all zombies through the use of the Hand Gun, Shotgun, or Machine Gun to progress\nthrough each level.";
                   
                    spriteBatch.DrawString(font, text,
                        new Vector2((Window.ClientBounds.Width / 2) - (font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (font.MeasureString(text).Y / 2) + 30),
                        Color.Black);
                    spriteBatch.End();

                    spriteBatch.DrawString(font, move,
                        new Vector2((Window.ClientBounds.Width / 2) - (font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (font.MeasureString(text).Y / 2) + 30),
                        Color.Black);
                    spriteBatch.End();

                    spriteBatch.DrawString(font, movement,
                        new Vector2((Window.ClientBounds.Width / 2) - (font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (font.MeasureString(text).Y / 2) + 30),
                        Color.Black);
                    spriteBatch.End();

                    spriteBatch.DrawString(font, goal,
                        new Vector2((Window.ClientBounds.Width / 2) - (font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (font.MeasureString(text).Y / 2) + 30),
                        Color.Black);
                    spriteBatch.End();

                    spriteBatch.DrawString(font, how,
                        new Vector2((Window.ClientBounds.Width / 2) - (font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (font.MeasureString(text).Y / 2) + 30),
                        Color.Black);
                    spriteBatch.End();

                    break;

                case gameState.play:
                    GraphicsDevice.Clear(Color.White);

                    // Draw text for intro splash screen
                    spriteBatch.Begin();

                    spriteBatch.Draw(logo,
                        new Rectangle(300, 80, Window.ClientBounds.Width / 4,
                            Window.ClientBounds.Height / 4), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 1); 

                    string StartText = "Now entering Be prepared to fight!";
                    spriteBatch.DrawString(font, StartText,
                        new Vector2((Window.ClientBounds.Width / 2) - (font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (font.MeasureString(text).Y / 2)),
                        Color.Black);
                  
                    text = "(Press the ENTER key to begin.)";
                    spriteBatch.DrawString(font, text,
                        new Vector2((Window.ClientBounds.Width / 2) - (font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (font.MeasureString(text).Y / 2) + 30),
                        Color.Black);
                    spriteBatch.End();
                    break;

                case gameState.AboutGame:
                    spriteBatch.Begin();
                    spriteBatch.Draw(mainmenuTexture,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 1);
                    string AboutText = "About Game Screen";
                    string team = "Ankit Kumar: Developer\n\nAkshat Sharma: Developer and Editor\n\nMichael Bayles: Team Leader, Developer & Graphics Designer\n\n"
                    + "Wei Yang: Developer & Graphic Designer \n\nPress 'Back' to go to the Main Menu";
                    spriteBatch.DrawString(font, AboutText,
                        new Vector2((300), (25)),
                            Color.Red, 0, Vector2.Zero,
                            1, SpriteEffects.None, 1);
                    spriteBatch.DrawString(font, team,
                        new Vector2((50), (85)),
                            Color.Brown, 0, Vector2.Zero,
                            1, SpriteEffects.None, 1);
                    spriteBatch.End();
                    break;
                
                case gameState.InGame:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();

                    //Draw background image
                    spriteBatch.Draw(backgroundTexture,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 1);
                    //DRAW THE FONTS
                    //1st is the SpriteFont object you wish to use to draw with
                    //2nd is the actual text you wish to draw
                    

                    spriteBatch.End();
                    break;

               
                case gameState.GamePause:
                    text = "Paused \n Press SpaceBar to Continue";
                    spriteBatch.Begin();
                    spriteBatch.DrawString(font, text,
                        new Vector2((Window.ClientBounds.Width / 2) - (font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (font.MeasureString(text).Y / 2)),
                        Color.Black, 0, Vector2.Zero,
                        1, SpriteEffects.None, 1);
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
            
        }

        public System.Text.StringBuilder startText { get; set; }
    }
}
