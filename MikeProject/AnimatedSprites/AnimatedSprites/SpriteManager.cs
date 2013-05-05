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

        public SpriteFont font;
        int score = 500000;
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
        int midEnemyHP = 139;
        int midEnemyDamage = 50;

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

        public Texture2D background;
        Texture2D bossTexture;
        Texture2D bossMinonTexture;
        Texture2D bossMinonDeath;
        Texture2D bulletTexture;
        Texture2D projectile;
        Texture2D raptor;
        Texture2D crosshair;
        Texture2D lazer;
        Texture2D fireball;
        Texture2D bombTexture;

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

        AutomatedSprite boss;
        bool fightingBoss = false;
        int nextBossMinion = 4000;
      

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


            background = Game.Content.Load<Texture2D>(@"Images\images");
            bossTexture = Game.Content.Load<Texture2D>(@"Images/boss");
            bossMinonTexture = Game.Content.Load<Texture2D>(@"Images/golemblood");
            bossMinonDeath = Game.Content.Load<Texture2D>(@"Images/golemdeath");
            bulletTexture = Game.Content.Load<Texture2D>(@"Images\bullet");
            projectile = Game.Content.Load<Texture2D>(@"Images\projectile");
            raptor = Game.Content.Load<Texture2D>(@"Images/raptor3");
            crosshair = Game.Content.Load<Texture2D>(@"Images/cross1");
            lazer = Game.Content.Load<Texture2D>(@"Images\lazer");
            fireball = Game.Content.Load<Texture2D>(@"Images\fireball");
            bombTexture = Game.Content.Load<Texture2D>(@"Images/bomb");
            health = Game.Content.Load<Texture2D>(@"Images/health");


            //Load the font
            font = Game.Content.Load<SpriteFont>(@"ScoreFont");

            player = new Player(raptor, new Vector2(100, GraphicsDevice.Viewport.Height - 45), 50, 41);

            
            healthRect = new Rectangle(350, 10, player.HP/50, 20);
            cursor = new CursorSprite(
                crosshair, new Vector2(100, 100), new Point(50, 50), 10, new Point(0, 0),
                new Point(1, 1), new Vector2(2, 2));

            

            string text = System.IO.File.ReadAllText("scores.txt");
            highScore = int.Parse(text);
            

            boss = new AutomatedSprite(bossTexture, new Vector2(600, GraphicsDevice.Viewport.Height-150),
                new Point(200, 168), 10, Point.Zero, new Point(6, 3), Vector2.Zero, 30, "boss", 1f, bossScoreValue, bossHP, 1);
            boss.flip = SpriteEffects.FlipHorizontally;

            
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
            nextBossMinion -= gameTime.ElapsedGameTime.Milliseconds;

            if (!fightingBoss && nextSpawnTime < 0)
            {
                SpawnEnemy();
                ResetSpawnTime();
            }
            currentKey = Keyboard.GetState();
            currentMouse = Mouse.GetState();

            
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

            UpdateShip();
            UpdateBullets();
            UpdateSprites(gameTime);
            if (boss.animationComplete)
                ShootFire();
            if (boss.HP < 50000 && fightingBoss && nextBossMinion <= 0)
                SpawnBossMinion();

            if (player.HP > 0)
                healthRect.Width = player.HP/50;

            //camera.Update(gameTime, player);
             base.Update(gameTime);
        }

        public void SpawnBossMinion()
        {
            spriteList.Add(new AutomatedSprite(bossMinonTexture, new Vector2(player.position.X, 400),
                new Point(118, 92), 20, Point.Zero, new Point(16, 4), Vector2.Zero, 16, "bossminion", 1f, 1, 1, 1) { DieOnHit = false });

            spriteList.Add(new AutomatedSprite(Game.Content.Load<Texture2D>(@"Images/golemblood"), new Vector2(player.position.X - 100, 400),
                new Point(118, 92), 20, Point.Zero, new Point(16, 4), Vector2.Zero, 16, "bossminion", 1f, 1, 1, 1) { DieOnHit = false, flip = SpriteEffects.FlipHorizontally });

            nextBossMinion = 4000;
        }


        public void UpdateShip()
        {
            AutomatedSprite ship = (AutomatedSprite)spriteList.Where(a => a.collisionCueName == "ship").FirstOrDefault();
            if (ship != null && (ship.GetPosition.X == player.position.X || nextBombTime <= 0))
            {
                DropBomb(ship);

                nextBombTime = 2500;
            }

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

        public void UpdateSprites(GameTime gameTime)
        {
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
                        if (b.isVisible)
                        {
                            s.HP -= b.damageValue;
                            if (s.HP <= 0)
                            {
                                score += s.ScoreValue;
                                if (score > highScore)
                                    highScore = score;

                                if (s.collisionCueName == "bossminion")
                                {
                                    spriteList.Add(new AutomatedSprite(bossMinonDeath, s.GetPosition + new Vector2(0,5),
                                        new Point(83, 80), 10, new Point(0, 1), new Point(16, 1), new Vector2(0, 0), 50, null, 1f, 0, 1000000, 1)
                                        {
                                            DieOnHit = false,
                                            Loop = false
                                        });
                                   

                                }
                                spriteList.RemoveAt(i);
                                i--;
                                
                                break;
                            }
                        }
                        if (!b.keepGoing)
                            b.isVisible = false;

                    }
                }

                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    player.HP -= s.Damage;
                    if (s.DieOnHit)
                    {
                        spriteList.RemoveAt(i);
                        i--;
                    }

                }

                if (player.HP <= 0)
                {
                    ((Game1)Game).gamestate = Game1.GameState.GameOver;
                }

                if (s.DoNotAnimate() || s.IsOutOfBounds(Game.GraphicsDevice.Viewport.Bounds))
                {
                    spriteList.RemoveAt(i);
                    i--;
                }


                if (boss.HP <= 0 || currentKey.IsKeyDown(Keys.Y))
                    ((Game1)Game).gamestate = Game1.GameState.GameFinished;

            }



        }

        public void Shoot()
        {
            Bullet bullet = null;
            string weaponSound = "";
            if (player.SelectedWeapon == Player.Weapon.MachineGun)
            {
                bullet = new Bullet(bulletTexture, 0f, machineGunBulletDamage, 0, 0);
                bullet.keepGoing = false;
                weaponSound = "gun";
            }
            else if (player.SelectedWeapon == Player.Weapon.RocketLauncher)
            {
                bullet = new Bullet(projectile, player.rotation, rocketLauncherDamage, 5, 20);
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
            Bullet bullet = new Bullet(lazer, 0f, lazerDamage, 50,0);
            bullet.position = new Vector2(currentMouse.X, 0);
            bullet.velocity = new Vector2(0, 15);
            bullet.isVisible = true;
            bullet.keepGoing = true;
            ((Game1)Game).soundBank.PlayCue("lazer");
            bullets.Add(bullet);
            
        }

        public void ShootFire()
        {
            float x = boss.GetPosition.X - player.position.X;
            float y = boss.GetPosition.Y - player.position.Y;

            double angle = Math.Asin(y / x);
            Vector2 direction = new Vector2(-(float)Math.Cos(angle) * 5, -(float)Math.Sin(angle));



            AutomatedSprite fire = new AutomatedSprite(fireball, boss.GetPosition + new Vector2(-15, 55), new Point(45, 37),
                10, Point.Zero, new Point(1, 1), direction, 1000, "fire", 1f, 0, 100000, 500);
            fire.Bounce = false;
            spriteList.Add(fire);
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
                AutomatedSprite bomb = new AutomatedSprite(bombTexture, s.GetPosition + new Vector2(0, 2), new Point(57, 60),
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
                fightingBoss = true;
                spriteList.Add(boss);
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
