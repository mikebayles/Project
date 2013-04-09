using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprites
{
    class Animation
    {
        Texture2D texture;
        Rectangle rectangle;
        Vector2 position;
        Vector2 origin;
        Vector2 velocity;

        int currentFrame;
        int frameWidth;
        int frameHeight;

        float timer;
        float interval = 50;

        public Animation(Texture2D texture, Vector2 position, int frameHeight, int frameWidth)
        {
            this.texture = texture;
            this.position = position;
            this.frameHeight = frameHeight;
            this.frameWidth = frameWidth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, 0f, origin, 1.0f, SpriteEffects.None, 0f);
            
        }

        public void Update(GameTime gametime)
        {
            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            position = position + velocity;

            if(Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                AnimateRight(gametime);
                velocity.X = 3;
            }
            else if(Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                AnimateLeft(gametime);
                velocity.X = -3;
            }
            else
                velocity = Vector2.Zero;
        }

        public void AnimateRight(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 7)
                    currentFrame = 0;
            }
        }

        public void AnimateLeft(GameTime gameTime)
        {
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 15 || currentFrame < 7)
                    currentFrame = 7;
            }
        }
    }
}
