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

        //A sprite for the player and a list of automated sprites
        UserControlledSprite player;
        Animation player2;
        CursorSprite cursor;
        List<Sprite> spriteList = new List<Sprite>();

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


            //Load the player sprite
            player = new UserControlledSprite(
                Game.Content.Load<Texture2D>(@"Images/ellenripley"),
                new Vector2(0, GraphicsDevice.Viewport.Height), new Point(30,60), 10, new Point(0, 0),
                new Point(6, 2), new Vector2(8, 13),1000);


            player2 = new Animation(Game.Content.Load<Texture2D>(@"Images/raptor"), new Vector2(100, GraphicsDevice.Viewport.Height-30), 50,41);


            //Load several different automated sprites into the list
            spriteList.Add(new BouncingSprite(
                Game.Content.Load<Texture2D>(@"Images/skullball"),
                new Vector2(150, 150), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(2,2)));
            spriteList.Add(new BouncingSprite(
                Game.Content.Load<Texture2D>(@"Images/skullball"),
                new Vector2(300, 150), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(2,2)));
            spriteList.Add(new BouncingSprite(
                Game.Content.Load<Texture2D>(@"Images/skullball"),
                new Vector2(139, 357), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(2,2)));
            spriteList.Add(new BouncingSprite(
                Game.Content.Load<Texture2D>(@"Images/skullball"),
                new Vector2(600, 400), new Point(75, 75), 10, new Point(0, 0),
                new Point(6, 8), new Vector2(2,2)));

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
            // Update player
            player2.Update(gameTime);
            //player.Update(gameTime, Game.Window.ClientBounds);
            cursor.Update(gameTime, Game.Window.ClientBounds);

            // Update all sprites
            foreach (Sprite s in spriteList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);

                // Check for collisions and exit game if there is one
                if (s.collisionRect.Intersects(player.collisionRect))
                    Game.Exit();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            //Draw the background
            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);


            // Draw the player
            //player.Draw(gameTime, spriteBatch);
            player2.Draw(spriteBatch);
            cursor.Draw(gameTime, spriteBatch);

            // Draw all sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
