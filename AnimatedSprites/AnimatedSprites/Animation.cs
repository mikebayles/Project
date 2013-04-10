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
        public Vector2 origin;
        Vector2 velocity;

        int currentFrame;
        int frameWidth;
        int frameHeight;

        float timer;
        float interval = 50;

        float rotation = 0;
        Vector2 distance;

        enum AnimationState
        {
            WalkingRight,
            WalkingLeft,
            Jumping
        }

        AnimationState State;

        public Animation(Texture2D texture, Vector2 position, int frameHeight, int frameWidth)
        {
            this.texture = texture;
            this.position = position;
            this.frameHeight = frameHeight;
            this.frameWidth = frameWidth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangle, Color.White, rotation, origin, 1.8f, SpriteEffects.None, 0f);
            
        }

        public void Update(GameTime gametime)
        {
            MouseState mouse = Mouse.GetState();

            if (State == AnimationState.WalkingRight)
            {
                distance.X = mouse.X - position.X;
                distance.Y = mouse.Y - position.Y;
            }
            else if (State == AnimationState.WalkingLeft)
            {
                distance.X = position.X - mouse.X;
                distance.Y = position.Y - mouse.Y;
               
            }

            rotation = (float)(Math.Atan2(distance.Y,distance.X));

            rectangle = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
            origin = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
            position = position + velocity;



            KeyboardState kbState = Keyboard.GetState();

            if(kbState.IsKeyDown(Keys.Right) || kbState.IsKeyDown(Keys.D))
            {
                AnimateRight(gametime);
                if(kbState.IsKeyDown(Keys.Space))
                    velocity.X = 6;
                else
                    velocity.X = 3;
            }
            else if(kbState.IsKeyDown(Keys.Left) || kbState.IsKeyDown(Keys.A))
            {
                AnimateLeft(gametime);
                if(kbState.IsKeyDown(Keys.Space))
                    velocity.X = -6;
                else
                    velocity.X = -3;
            }
            else
                velocity = Vector2.Zero;
        }

        public void AnimateRight(GameTime gameTime)
        {
            State = AnimationState.WalkingRight;
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
            State = AnimationState.WalkingLeft;
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;

            if (timer > interval)
            {
                currentFrame++;
                timer = 0;
                if (currentFrame > 15 || currentFrame < 8)
                    currentFrame = 8;
            }
        }
    }
}
