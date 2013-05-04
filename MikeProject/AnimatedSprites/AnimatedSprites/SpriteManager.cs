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
        public int highScore = 0;

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

        int midEnemyScoreValue = 100;
        int midEnemyHP = 80;
        int midEnemyDamage = 30;

        int advEnemyScoreValue = 150;
        int advEnemyHP = 160;
        int advEnemyDamage = 100;

        int bossHP = 50000;
        int bossScoreValue = 10000;

        int shipScoreValue = 100;
        int shipHP = 1000;
        int shipDamage = 1000;

        int bombScoreValue = 10;
        int bombHP = 50;
        int bombDamage = 1000;

        int machineGunBulletDamage = 50;
        int rocketLauncherDamage = 70;
        int lazerDamage = 1000;

    

        int nextLazerTime = 3500;
        float mAlphaValue = 1;
        

        //Spawn time variables
        //int nextSpawnTimeChange = 3500;
        //int timeSinceLastSpawnTimeChange = 0;

        // Variables for spawning new enemies
        int enemySpawnMinMilliseconds = 1000;
        int enemySpawnMaxMilliseconds = 2000;
        //int enemyMinSpeed = 2;
        //int enemyMaxSpeed = 6;
        int nextSpawnTime = 0;

        int nextBombTime = 0;
        
        Texture2D health;
        Rectangle healthRect;

        Camera camera;
      

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


            string text = System.IO.File.ReadAllText("scores.txt");
            highScore = int.Parse(text);
            //camera = new Camera(GraphicsDevice.Viewport);
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
            nextLazerTime -= gameTime.ElapsedGameTime.Milliseconds;
            nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            nextBombTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0)
            {
                SpawnEnemy();

                // Reset spawn timer
                ResetSpawnTime();
            }

            AutomatedSprite ship = (AutomatedSprite)spriteList.Where(a => a.collisionCueName == "ship").FirstOrDefault();
            if (ship != null &&(ship.GetPosition.X == player.position.X || nextBombTime <= 0))
            {
                DropBomb(ship);

                nextBombTime = 2500;
            }

            currentKey = Keyboard.GetState();
            currentMouse = Mouse.GetState();

            // Update player
            player.Update(gameTime);
            cursor.Update(gameTime, Game.Window.ClientBounds);



            if (currentMouse.LeftButton == ButtonState.Pressed && pastMouse.LeftButton == ButtonState.Released)
                Shoot();

            if (nextLazerTime <= 0)
            {
                mAlphaValue = 0;
                if (currentMouse.RightButton == ButtonState.Pressed && pastMouse.RightButton == ButtonState.Released)
                {
                    ShootLazer();
                    mAlphaValue = 255;
                    nextLazerTime = 3500;
                }
                
            }

            pastKey = Keyboard.GetState();
            pastMouse = Mouse.GetState();
            UpdateBullets();
            
            // Update all sprites
            for (int i = 0; i < spriteList.Count; i++)
            {
                if(i <0)
                    Console.WriteLine(i);
                Sprite s = spriteList[i];
                s.Update(gameTime, Game.Window.ClientBounds);
                // Check for collisions and exit game if there is one
                foreach (Bullet b in bullets)
                {
                    if (s.collisionRect.Intersects(b.collisionRect))
                    {
                        if (b.isVisible)
                        {
                            s.HP -= b.damageValue;
                            if (s.HP <= 0)
                            {
                                score += s.ScoreValue;
                                if (score > highScore)
                                    highScore = score;
                                spriteList.RemoveAt(i);
                                i--;
                                break;
                            }
                        }
                        if(!b.keepGoing)
                            b.isVisible = false;
                        
                    }
                }

                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    player.HP -= s.Damage;
                    spriteList.RemoveAt(i);
                    i--;

                }

                if (player.HP <= 0)
                {
                    ((Game1)Game).gamestate = Game1.GameState.GameOver;
                }

                if (s.IsOutOfBounds(Game.GraphicsDevice.Viewport.Bounds))
                {
                    spriteList.RemoveAt(i);
                    i--;
                }

                if(currentKey.IsKeyDown(Keys.Y))
                    ((Game1)Game).gamestate = Game1.GameState.GameFinished;

            }


            if (player.HP > 0)
                healthRect.Width = player.HP/50;

            //camera.Update(gameTime, player);
             base.Update(gameTime);
        }

        public void UpdateBullets()
        {
            foreach (Bullet b in bullets)
            {
                b.position += b.velocity;
                if(Vector2.Distance(b.position,player.position) > 1000)
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
            string weaponSound = "";
            if (player.SelectedWeapon == Player.Weapon.MachineGun)
            {
                bullet = new Bullet(Game.Content.Load<Texture2D>(@"Images\bullet"), 0f, machineGunBulletDamage, 0, 0);
                bullet.keepGoing = false;
                weaponSound = "gun";
            }
            else if (player.SelectedWeapon == Player.Weapon.RocketLauncher)
            {
                bullet = new Bullet(Game.Content.Load<Texture2D>(@"Images\projectile"), player.rotation, rocketLauncherDamage, 5, 20);
                bullet.keepGoing = false;
                weaponSound = "rocket";
            }

            bullet.velocity = new Vector2((float)Math.Cos(player.rotation)*2, (float)Math.Sin(player.rotation)) * 5f;// new Vector2(player.velocity.X, player.velocity.Y);

            bullet.position = player.position + bullet.velocity ;
            
            bullet.isVisible = true;

            if (bullets.Count < 20)
            {
                bullets.Add(bullet);
                ((Game1)Game).soundBank.PlayCue(weaponSound);
            }
        }

        public void ShootLazer()
        {
            Bullet bullet = new Bullet(Game.Content.Load<Texture2D>(@"Images\lazer"), 0f, lazerDamage, 50,0);
            bullet.position = new Vector2(currentMouse.X, 0);
            bullet.velocity = new Vector2(0, 15);
            bullet.isVisible = true;
            bullet.keepGoing = true;
            ((Game1)Game).soundBank.PlayCue("lazer");
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
                enemySpawnMinMilliseconds = 300;
                enemySpawnMaxMilliseconds = 600;
            }

            // Set the next spawn time for an enemy
            nextSpawnTime = ((Game1)Game).rnd.Next(
                enemySpawnMinMilliseconds,
                enemySpawnMaxMilliseconds);
        }

        void DropBomb(AutomatedSprite s)
        {
            
            if (s != null)
            {
                AutomatedSprite bomb = new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/bomb"), s.GetPosition + new Vector2(0, 2), new Point(57, 60),
                        10, Point.Zero, new Point(1, 1), new Vector2(0, 10), 56, null, 1f, bombScoreValue, bombHP, bombDamage);
                bomb.Bounce = false;
                spriteList.Add(bomb);
            }
        }

        private void SpawnEnemy()
        {
            if (spriteList.Where(a => a.collisionCueName == "ship").Count() == 0)
            {
                spriteList.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/ship"),new Vector2(600,50),new Point(130,111),
                    10,Point.Zero,new Point(1,1),new Vector2(-5,0),56,"ship",1f,shipScoreValue,shipHP,shipDamage));
            }


            if (score <= 1000)
            {
                spriteList.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/femaleEnemy"), new Vector2(700, 400), new Point(30, 40),
                    10, Point.Zero, new Point(7, 1), new Vector2(-1, 0), 56, null, 2f, basicEnemyScoreValue, basicEnemyHP, basicEnemyDamage));
            }
            else if (score <= 2000)
            {
                player.rocketLauncherAvailable = true;
                spriteList.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/classEnemy"), new Vector2(700, 400), new Point(30, 40),
                    10, Point.Zero, new Point(7, 1), new Vector2(-2, 0), 56, null, 2f, midEnemyScoreValue, midEnemyHP, midEnemyDamage));
            }
            else if (score <= 15000)
            {
                spriteList.Add(new AutomatedSprite(
                    Game.Content.Load<Texture2D>(@"Images/fatEnemy"), new Vector2(700, 330), new Point(47, 70),
                    10, Point.Zero, new Point(4, 1), new Vector2(-3, 0),
                    126, null, 2f, advEnemyScoreValue, advEnemyHP, advEnemyDamage));
            }
            else
            {

            }
        }
        public override void Draw(GameTime gameTime)
        {

            //spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,null,null,null,null,camera.transform);
            spriteBatch.Begin();
            //bg.Draw(spriteBatch);
            //Draw the background
            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            
            spriteBatch.DrawString(font, "Score : " + score, new Vector2(10, 30), Color.Red);
            spriteBatch.DrawString(font, "High Score : " + highScore, new Vector2(10, 10), Color.Red);
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

            if(mAlphaValue <= 0)
                spriteBatch.DrawString(font, "Lazer Ready", new Vector2(10, 50), new Color(118,244,255));
           

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
