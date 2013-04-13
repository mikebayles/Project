using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    abstract class Sprite
    {
        // Stuff needed to draw the sprite
        Texture2D textureImage;
        public Point frameSize;
        public Point currentFrame;
        Point sheetSize;
        public bool reverse;

        // Collision data
        int collisionOffset;

        // Framerate stuff
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 16;

        // Movement data
        public Vector2 speed;
        public Vector2 position;

        // Abstract definition of direction property
        public abstract Vector2 direction
        {
            get;
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, bool reverse)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame, reverse)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame, bool reverse)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.reverse = reverse;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Update frame if time to do so based on framerate
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                // Increment to next frame
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                    }
                }
            }
        }


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // Draw the sprite
            if (!reverse)
            {
                spriteBatch.Draw(textureImage,
                    position,
                    new Rectangle(currentFrame.X * frameSize.X,
                        currentFrame.Y * frameSize.Y,
                        frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    2f, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(textureImage,
                    position,
                    new Rectangle(currentFrame.X * frameSize.X,
                        currentFrame.Y * frameSize.Y,
                        frameSize.X, frameSize.Y),
                    Color.White, 0, Vector2.Zero,
                    2f, SpriteEffects.FlipHorizontally, 0);
            }
        }

        // Gets the collision rect based on position, framesize and collision offset
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }

    }
}

