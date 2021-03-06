/*
 * Public Static Void PAIN
 * Section #1
*/

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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AnimatedSprites
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteManager spriteManager;

        MouseState mbd;
        MouseState Prevmbd;

        public AudioEngine audioEngine;
        public WaveBank waveBank;
        public SoundBank soundBank;
        public Cue trackCue;

        public Video play;
        public VideoPlayer player = new VideoPlayer();

        Texture2D videoTexture;

        public enum GameState
        {
            Menu,
            Play,
            Instructions,
            Exit,
            InGame,
            GameOver,
            GamePause,
            AboutGame,
            GameFinished
        }
        

        public GameState gamestate = GameState.Menu;

        List<MainMenu> menuButtons = new List<MainMenu>();
        Texture2D mainmenuTexture;
        Texture2D logo;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferHeight = 370;
            //graphics.PreferredBackBufferWidth = 785;
            Content.RootDirectory = "Content";


            menuButtons.Add(new MainMenu(new Rectangle(400, 45, 100, 14), "Play"));
            menuButtons.Add(new MainMenu(new Rectangle(400, 75, 100, 14), "About"));
            menuButtons.Add(new MainMenu(new Rectangle(400, 105, 100, 14), "Instructions"));
            menuButtons.Add(new MainMenu(new Rectangle(400, 135, 100, 14), "Exit"));

            
            rnd = new Random();
            IsMouseVisible = true;
        }

        public Random rnd { get; set; }

        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);

            spriteManager.Enabled = false;  //disables the Update method of the SpriteManager GameComponent
            spriteManager.Visible = false;  //disables the Draw method of the SpriteManager GameComponent
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mainmenuTexture = Content.Load<Texture2D>(@"Images/space_Background");
            logo = Content.Load<Texture2D>(@"Images/logo_alien");


            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
             
            play = Content.Load<Video>(@"Audio\Ending_scene");  

            
            
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            //used to simplify the code
            KeyboardState keyboard = Keyboard.GetState();

           //only perform certain actions based on
            //the current game state
            switch (gamestate)
            {
                case GameState.Menu:
                    if(trackCue == null)
                        trackCue = soundBank.GetCue("start");
                    if(!trackCue.IsPlaying)
                        trackCue.Play();
                    break;


                case GameState.Play:
                    if (keyboard.IsKeyDown(Keys.Enter))
                    {
                        Initialize();  //resets the sprites
                        gamestate = GameState.InGame;
                        IsMouseVisible = false;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                        menuButtons.RemoveRange(0, menuButtons.Count);
                      
                    }
                    if (keyboard.IsKeyDown(Keys.Back))
                    {
                        gamestate = GameState.Menu;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    break;
                case GameState.AboutGame:
                    if (keyboard.IsKeyDown(Keys.Back))
                    {
                        gamestate = GameState.Menu;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                        
                    }
                    break;
                case GameState.InGame:
                    
                    if (keyboard.IsKeyDown(Keys.P))
                    {
                        gamestate = GameState.GamePause;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    
                    break;
                case GameState.GameOver:
                    
                    
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    
                    break;
                case GameState.GamePause:
                    if (keyboard.IsKeyDown(Keys.Space))
                    {
                        gamestate = GameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    break;
               
                case GameState.Instructions:
                    if (keyboard.IsKeyDown(Keys.Back))
                    {
                       gamestate = GameState.Menu;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    break;

                case GameState.GameFinished:
                
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                    
                    if(trackCue.IsPlaying)
                        trackCue.Stop(AudioStopOptions.Immediate);
                    break;
            }
         
            

            // TODO: Add your update logic here
            mbd = Mouse.GetState();

            foreach (MainMenu m in menuButtons)
            {
                m.mouseOver(mbd);
                if (m.text == "Play" && m.selected == true && mbd.LeftButton == ButtonState.Pressed && Prevmbd.LeftButton == ButtonState.Released)
                {
                    gamestate = GameState.Play;
                }
               
                else if (m.text == "About" && m.selected == true && mbd.LeftButton == ButtonState.Pressed && Prevmbd.LeftButton == ButtonState.Released)
                {
                    gamestate = GameState.AboutGame;
                }
                
                else if (m.text == "Instructions" && m.selected == true && mbd.LeftButton == ButtonState.Pressed && Prevmbd.LeftButton == ButtonState.Released)
                {
                    gamestate = GameState.Instructions;
                }
                
                else if(m.text == "Exit" && m.selected == true && mbd.LeftButton == ButtonState.Pressed && Prevmbd.LeftButton == ButtonState.Released)
                {
                    gamestate = GameState.Exit;
                }

            }

            Prevmbd = mbd;

            audioEngine.Update();
            base.Update(gameTime);
        }


        

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            string text = " ";

            // TODO: Add your drawing code here
            // spriteBatch.Begin();

            switch (gamestate)
            {
                case GameState.Menu:
                    spriteBatch.Begin();
                    spriteBatch.Draw(mainmenuTexture,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 1);

                    foreach (MainMenu m in menuButtons)
                    {
                        m.Draw(spriteBatch, spriteManager.font);
                    }
                    spriteBatch.End();
                    break;

                case GameState.Exit:
                    Exit();
                    break;

                case GameState.Instructions:
                    string move, movement, goal, how;
                    spriteBatch.Begin();
                    spriteBatch.Draw(mainmenuTexture,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 1);
                    text = "Instructions";
                    move = "How to Move:";
                    movement = "Up: Up Direction Key or W Key\nDown: Down Direction Key or S Key\nLeft: Left Direction Key or A Key\nRight: Right Direction Key or D Key ";
                    goal = "Goal:\nThe goal of Space Alien 2.0 is to survive till the end.\nOnce all the aliens are killed,\nYou will be fighting against main boss. \nYou have to reach and kill him to\nget his spaceship to go back to earth.";

                    how = "How to Win:\nKill all aliens through the use of the Hand Gun,\nRocket Launcher, or Lazer to progress\nthrough each level.";

                    spriteBatch.DrawString(spriteManager.font, text + "\n\n" + move + "\n" + movement + "\n\n" + goal + "\n\n" + how, new Vector2(10,0), Color.Brown);

           
             
                    spriteBatch.End();

                    break;

                case GameState.Play:
                    GraphicsDevice.Clear(Color.White);

                    // Draw text for intro splash screen
                    spriteBatch.Begin();

                    spriteBatch.Draw(logo,
                        new Rectangle(300, 80, Window.ClientBounds.Width / 4,
                            Window.ClientBounds.Height / 4), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 1);

                    string StartText = "Now entering Be prepared to fight!";
                    spriteBatch.DrawString(spriteManager.font, StartText,
                        new Vector2((Window.ClientBounds.Width / 2) - (spriteManager.font.MeasureString(StartText).X / 2),
                        (Window.ClientBounds.Height / 2) - (spriteManager.font.MeasureString(StartText).Y / 2)),
                        Color.Black);
                       

                    text = "(Press the ENTER key to begin.)";
                    spriteBatch.DrawString(spriteManager.font, text,
                        new Vector2((Window.ClientBounds.Width / 2) - (spriteManager.font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (spriteManager.font.MeasureString(text).Y / 2) + 30),
                        Color.Black);
                    spriteBatch.End();
                    break;

                case GameState.AboutGame:
                    spriteBatch.Begin();
                    spriteBatch.Draw(mainmenuTexture,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 1);
                    string AboutText = "About Game Screen";
                    string team = "Ankit Kumar: Developer\n\nAkshat Sharma: Developer and Editor\n\nMichael Bayles: Team Leader, Developer & Graphics Designer\n\n"
                    + "Weikang Yang: Developer & Graphic Designer \n\nPress 'Back' to go to the Main Menu";
                    spriteBatch.DrawString(spriteManager.font, AboutText,
                        new Vector2((300), (25)),
                            Color.Brown, 0, Vector2.Zero,
                            2f, SpriteEffects.None, 1);
                    spriteBatch.DrawString(spriteManager.font, team,
                        new Vector2((50), (85)),
                            Color.Brown, 0, Vector2.Zero,
                            1, SpriteEffects.None, 1);
                    spriteBatch.End();
                    break;

                case GameState.InGame:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();

                    //Draw background image
                    spriteBatch.Draw(spriteManager.background,
                        new Rectangle(0, 0, Window.ClientBounds.Width,
                            Window.ClientBounds.Height), null,
                            Color.White, 0, Vector2.Zero,
                            SpriteEffects.None, 1);
                    //DRAW THE FONTS
                    //1st is the SpriteFont object you wish to use to draw with
                    //2nd is the actual text you wish to draw


                    spriteBatch.End();
                    break;


                case GameState.GamePause:
                    text = "Paused \n Press SpaceBar to Continue";
                    spriteBatch.Begin();
                    spriteBatch.DrawString(spriteManager.font, text,
                        new Vector2((Window.ClientBounds.Width / 2) - (spriteManager.font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (spriteManager.font.MeasureString(text).Y / 2)),
                        Color.Black, 0, Vector2.Zero,
                        1, SpriteEffects.None, 1);
                    spriteBatch.End();
                    break;

                case GameState.GameOver:
                    text = "Game Over!";
                    string text2 = "High Score : " + spriteManager.highScore;
                    spriteBatch.Begin();
                    spriteBatch.DrawString(spriteManager.font, text,
                        new Vector2((Window.ClientBounds.Width / 2) - (spriteManager.font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (spriteManager.font.MeasureString(text).Y / 2)),
                        Color.Green, 0, Vector2.Zero,
                        1, SpriteEffects.None, 1);
                    spriteBatch.DrawString(spriteManager.font, text2,
                        new Vector2((Window.ClientBounds.Width / 2) - (spriteManager.font.MeasureString(text2).X / 2),
                        (Window.ClientBounds.Height / 2) - (spriteManager.font.MeasureString(text2).Y / 2) + 30),
                        Color.Green, 0, Vector2.Zero,
                        1, SpriteEffects.None, 1);
                    spriteBatch.End();
                    System.IO.File.WriteAllLines("scores.txt", new string[] { spriteManager.highScore.ToString() });
                    break;

                case GameState.GameFinished:
                    text = "Congratulations You Won!";
                    text2 = "High Score : " + spriteManager.highScore;
                    spriteBatch.Begin();
                    spriteBatch.DrawString(spriteManager.font, text,
                        new Vector2((Window.ClientBounds.Width / 2) - (spriteManager.font.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2) - (spriteManager.font.MeasureString(text).Y / 2)),
                        Color.Green, 0, Vector2.Zero,
                        1, SpriteEffects.None, 1);
                    spriteBatch.DrawString(spriteManager.font, text2,
                        new Vector2((Window.ClientBounds.Width / 2) - (spriteManager.font.MeasureString(text2).X / 2),
                        (Window.ClientBounds.Height / 2) - (spriteManager.font.MeasureString(text2).Y / 2) + 30),
                        Color.Green, 0, Vector2.Zero,
                        1, SpriteEffects.None, 1);
                    spriteBatch.End();
                    System.IO.File.WriteAllLines("scores.txt", new string[] { spriteManager.highScore.ToString() });
                    //soundBank.PlayCue("end");

                    player.Play(play);// Only call GetTexture if a video is playing or paused
                    if (player.State != MediaState.Stopped)
                        videoTexture = player.GetTexture();


                    // Drawing to the rectangle will stretch the 
                    // video to fill the screen
                    Rectangle screen = new Rectangle(GraphicsDevice.Viewport.X,
                        GraphicsDevice.Viewport.Y,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height);

                    // Draw the video, if we have a texture to draw.
                    if (videoTexture != null)
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(videoTexture, screen, Color.White);
                        spriteBatch.End();
                    }

                    
                    break;
            }

            base.Draw(gameTime);

        }
    }
}