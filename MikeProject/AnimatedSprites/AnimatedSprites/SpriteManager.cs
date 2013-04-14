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


namespace AnimatedSprites
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //SpriteBatch for drawing
        SpriteBatch spriteBatch;

        Texture2D background;
        SpriteFont font;
        int score = 0;

        //A sprite for the player and a list of automated sprites
        Animation player;
        CursorSprite cursor;
        List<Sprite> spriteList = new List<Sprite>();
        List<Bullet> bullets = new List<Bullet>();

        KeyboardState pastKey;
        KeyboardState currentKey;

        MouseState pastMouse;
        MouseState currentMouse;

        int basicEnemyScoreValue = 50;
        int basicEnemyHP = 100;

        int basicBullletDamage = 50;

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            //Load the background
            background = Game.Content.Load<Texture2D>(@"Images\images");

            //Load the font
            font = Game.Content.Load<SpriteFont>(@"ScoreFont");
            
            player = new Animation(Game.Content.Load<Texture2D>(@"Images/raptor2"), new Vector2(100, GraphicsDevice.Viewport.Height-45), 50,41);


            //Load several different automated sprites into the list
            spriteList.Add(new AutomatedSprite( 
                Game.Content.Load<Texture2D>(@"Images/skullball"),
                new Vector2(150, 150), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(2,2),null,basicEnemyScoreValue,basicEnemyHP));
            spriteList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>(@"Images/skullball"),
                new Vector2(300, 150), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(2,2),null,basicEnemyScoreValue,basicEnemyHP));
            spriteList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>(@"Images/skullball"),
                new Vector2(139, 357), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(2,2),null,basicEnemyScoreValue,basicEnemyHP));
            spriteList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>(@"Images/skullball"),
                new Vector2(600, 400), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(2,2),null,basicEnemyScoreValue,basicEnemyHP));

            cursor = new CursorSprite(
                Game.Content.Load<Texture2D>(@"Images/cross1"), new Vector2(100, 100), new Point(50, 50), 10, new Point(0, 0),
                new Point(1, 1), new Vector2(2, 2));

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            currentKey = Keyboard.GetState();
            currentMouse = Mouse.GetState();

            // Update player
            player.Update(gameTime);
            cursor.Update(gameTime, Game.Window.ClientBounds);



            if (currentMouse.LeftButton == ButtonState.Pressed && pastMouse.LeftButton == ButtonState.Released)
                Shoot();

            pastKey = Keyboard.GetState();
            pastMouse = Mouse.GetState();
            UpdateBullets();
            
            // Update all sprites
            for (int i = 0; i < spriteList.Count; i++)
            {
                Sprite s = spriteList[i];
                s.Update(gameTime, Game.Window.ClientBounds);
                // Check for collisions and exit game if there is one
                foreach (Bullet b in bullets)
                {
                    if (s.collisionRect.Intersects(b.collisionRect))
                    {
                        
                        b.isVisible = false;
                        s.HP -= b.damageValue;
                        if (currentKey.IsKeyDown(Keys.H))
                            Console.WriteLine(s.HP);
                        if(s.HP == 0 && !(s is CursorSprite))
                        {
                            score += s.scoreValue;
                            spriteList.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            

            base.Update(gameTime);
        }

        public void UpdateBullets()
        {
            foreach (Bullet b in bullets)
            {
                b.position += b.velocity;
                if(Vector2.Distance(b.position,player.position) > 500)
                    b.isVisible = false;
            }
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].isVisible)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Shoot()
        {
            Bullet bullet = new Bullet(Game.Content.Load<Texture2D>(@"Images\bullet"),basicBullletDamage);
            bullet.velocity = new Vector2((float)Math.Cos(player.rotation), (float)Math.Sin(player.rotation)) * 5f + player.velocity;

            bullet.position = player.position + bullet.velocity * 5;
            
            bullet.isVisible = true;

            if (bullets.Count < 20)
                bullets.Add(bullet);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            //Draw the background
            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.DrawString(font, "Score : " + score, new Vector2(10, 10), Color.Red);
            // Draw the player
            player.Draw(spriteBatch);
            cursor.Draw(gameTime, spriteBatch);

            // Draw all sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);

            foreach (Bullet b in bullets)
            {
                b.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
