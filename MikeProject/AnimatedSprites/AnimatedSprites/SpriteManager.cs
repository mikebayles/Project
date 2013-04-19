/*
 * Public Static Void PAIN
 * Section #1
*/


//TODO: more guns, alien sprites (with animations), fix the bullet speed when player faces opposite, allow for user to go to next area, add big boss
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

        public Texture2D background;
        public SpriteFont font;
        int score = 0;

        //A sprite for the player and a list of automated sprites
        Player player;
        CursorSprite cursor;
        List<Sprite> spriteList = new List<Sprite>();
        List<Bullet> bullets = new List<Bullet>();

        KeyboardState pastKey;
        KeyboardState currentKey;

        MouseState pastMouse;
        MouseState currentMouse;

        int basicEnemyScoreValue = 50;
        int basicEnemyHP = 100;
        int basicEnemyDamage = 10;

        int machineGunBulletDamage = 50;
        int rocketLauncherDamage = 80;

        //Spawn time variables
        //int nextSpawnTimeChange = 5000;
        //int timeSinceLastSpawnTimeChange = 0;

        // Variables for spawning new enemies
        int enemySpawnMinMilliseconds = 1000;
        int enemySpawnMaxMilliseconds = 2000;
        //int enemyMinSpeed = 2;
        //int enemyMaxSpeed = 6;
        int nextSpawnTime = 0;

        Texture2D health;
        Rectangle healthRect;

        //Background bg;

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
            //bg = new Background(1570);
            //Load the font
            font = Game.Content.Load<SpriteFont>(@"ScoreFont");

            player = new Player(Game.Content.Load<Texture2D>(@"Images/raptor3"), new Vector2(100, GraphicsDevice.Viewport.Height - 45), 50, 41);

            health = Game.Content.Load<Texture2D>(@"Images/health");
            healthRect = new Rectangle(350, 10, player.HP/50, 20);
            cursor = new CursorSprite(
                Game.Content.Load<Texture2D>(@"Images/cross1"), new Vector2(100, 100), new Point(50, 50), 10, new Point(0, 0),
                new Point(1, 1), new Vector2(2, 2));

            //bg.LoadContent(Game.Content);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //bg.Update(gameTime, player.velocity.X);
            // Time to spawn enemy?
            nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0)
            {
                SpawnEnemy();

                // Reset spawn timer
                ResetSpawnTime();
            }

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
                        if(s.HP <= 0)
                        {
                            score += s.ScoreValue;
                            spriteList.RemoveAt(i);
                            i--;
                        }
                    }
                }

                if (s.collisionRect.Intersects(player.collisionRect))
                    player.HP -= s.Damage;

            }


            if (player.HP > 0)
                healthRect.Width = player.HP/50;

            
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
            Bullet bullet = null;
            if(player.SelectedWeapon == Player.Weapon.MachineGun)
             bullet= new Bullet(Game.Content.Load<Texture2D>(@"Images\bullet"),0f,machineGunBulletDamage,0);
            else if(player.SelectedWeapon == Player.Weapon.RocketLauncher)
                bullet = new Bullet(Game.Content.Load<Texture2D>(@"Images\projectile"),player.rotation, rocketLauncherDamage, 10);

            bullet.velocity = new Vector2((float)Math.Cos(player.rotation)*2, (float)Math.Sin(player.rotation)) * 5f;// new Vector2(player.velocity.X, player.velocity.Y);

            bullet.position = player.position + bullet.velocity ;
            
            bullet.isVisible = true;

            if (bullets.Count < 20)
                bullets.Add(bullet);
        }

        private void ResetSpawnTime()
        {
            if (score > 1000 && score < 2000)
            {
                enemySpawnMinMilliseconds = 500;
                enemySpawnMaxMilliseconds = 750;
            }
            else if (score > 2000)
            {
                enemySpawnMinMilliseconds = 100;
                enemySpawnMaxMilliseconds = 400;
            }

            // Set the next spawn time for an enemy
            nextSpawnTime = ((Game1)Game).rnd.Next(
                enemySpawnMinMilliseconds,
                enemySpawnMaxMilliseconds);
        }

        private void SpawnEnemy()
        {
            //spriteList.Add(new AutomatedSprite(                
            //    Game.Content.Load<Texture2D>(@"Images/classEnemy"),
            //    new Vector2(700,400), new Point(75, 75), 10, new Point(0, 0),
            //    new Point(6, 8), new Vector2(-2, 0),16, null, 1f, basicEnemyScoreValue, basicEnemyHP,basicEnemyDamage));

            spriteList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>(@"Images/classEnemy"),new Vector2(700,400),new Point(30,40),0,Point.Zero,new Point(7,1),new Vector2(-2,0),
                36, null, 2f, 10,10,10));
        }
        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
            //bg.Draw(spriteBatch);
            //Draw the background
            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            
            spriteBatch.DrawString(font, "Score : " + score, new Vector2(10, 10), Color.Red);
            // Draw the player
            player.Draw(spriteBatch);
            spriteBatch.Draw(health, healthRect, Color.White);
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
