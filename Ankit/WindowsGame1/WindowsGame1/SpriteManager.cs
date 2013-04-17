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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Player player;
        Texture2D background;
        //Projectiles
        List<Projectile> bullet = new List<Projectile>();

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            background = Game.Content.Load<Texture2D>(@"Images/background");
            player = new Player();
            //Load projectile into player
            /*player.test = new Projectile(Game.Content.Load<Texture2D>(@"Images/shi"), new Vector2(0, Game.Window.ClientBounds.Height - 90),
                new Point(50, 50), 10, new Point(0, 0), new Point(1, 1), new Vector2(4, 0), false,5);*/

            //Load sprites into player
            player.playerSprites.Add(new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"Images/hand_stand"), new Vector2(0, Game.Window.ClientBounds.Height - 90),
                new Point(75, 45), 10, new Point(0, 0), new Point(4, 1), new Vector2(2, 0), 178, false));
            player.playerSprites.Add(new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"Images/hand_run"), new Vector2(0, Game.Window.ClientBounds.Height - 90),
                new Point(75, 45), 10, new Point(0, 0), new Point(18, 1), new Vector2(2, 0), 50, false));
            player.playerSprites.Add(new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"Images/hand_stand_shoot"), new Vector2(0, Game.Window.ClientBounds.Height - 90),
                new Point(75, 45), 10, new Point(0, 0), new Point(6, 1), new Vector2(2, 0), 50, false));
            player.playerSprites.Add(new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"Images/hand_run_shoot"), new Vector2(0, Game.Window.ClientBounds.Height - 90),
                new Point(75, 45), 10, new Point(0, 0), new Point(6, 1), new Vector2(2, 0), 50, false));

            base.LoadContent();
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            if (player.IsShooting)
            {
                GenerateBullet();
                player.IsShooting = false;
            }
            // TODO: Add your update code here
            player.Update(gameTime, Game.Window.ClientBounds);
            for (int i = 0; i < bullet.Count; ++i)
            {
                Projectile s = bullet[i];

                s.Update(gameTime, Game.Window.ClientBounds);
                if(s.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    bullet.RemoveAt(i);
                    i--;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);


            player.Draw(gameTime, spriteBatch);
            foreach (Projectile t in bullet)
                t.Draw(gameTime,spriteBatch);

            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            spriteBatch.End();
            base.Draw(gameTime);
        }
        protected void GenerateBullet()
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;

            Point frameSize = new Point(50, 50);
            speed = new Vector2(4, 0);
            position = player.playerSprites[1].position;
            position.X += 90;

            bullet.Add(
                new Projectile(Game.Content.Load<Texture2D>(@"Images/shi"), position,
                new Point(50, 50), 10, new Point(0, 0), new Point(1, 1), new Vector2(9, 0), 178, false, 5));
        }


    }
}
